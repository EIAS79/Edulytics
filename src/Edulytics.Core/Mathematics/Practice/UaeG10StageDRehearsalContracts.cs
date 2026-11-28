using System.Data.Common;
using System.Text.Json;
using Edulytics.Core.Curriculum;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Stage D candidate evaluation on the disposable local PostgreSQL only.
/// Diagnostic evaluation remains disposable-only; a separate exact release token
/// is required for the owner-directed first-party release.
/// All ordinary runtime routes retain their authorization, lesson alignment,
/// verifier, session QA, renderer and grading checks.
/// </summary>
internal static class UaeG10StageDRehearsalContracts
{
    internal const string ScopePrefix = "PED:UAE-MOE-MATH:L10:";
    private const string GeneralPrefix = ScopePrefix + "GENERAL:";
    private const string Flag = "EDULYTICS_G10_CI_PRACTICE_REHEARSAL";
    private const string ReleaseFlag = "EDULYTICS_G10_DIGITAL_PRACTICE_RELEASE";
    private const string ReleaseToken = "CONFIRM_RELEASE_UAE_GRADE10_GENERAL_148_DIGITAL_PRACTICE";

    private static bool IsDisposableConnection(string variable)
    {
        var connection = Environment.GetEnvironmentVariable(variable);
        if (string.IsNullOrWhiteSpace(connection))
            return false;
        var builder = new DbConnectionStringBuilder { ConnectionString = connection };
        return builder.TryGetValue("Host", out var host) &&
            string.Equals(Convert.ToString(host), "127.0.0.1", StringComparison.Ordinal) &&
            builder.TryGetValue("Database", out var db) &&
            string.Equals(Convert.ToString(db), "edulytics_clean_ci", StringComparison.Ordinal) &&
            !builder.ContainsKey("Server") && !builder.ContainsKey("Data Source");
    }

    private static bool Enabled()
    {
        var request = Environment.GetEnvironmentVariable(Flag);
        if (string.IsNullOrEmpty(request))
            return false;
        if (request != "EXERCISE_97_DECLARED_CANDIDATES_ONLY" ||
            Environment.GetEnvironmentVariable("GITHUB_ACTIONS") != "true" ||
            Environment.GetEnvironmentVariable("EDULYTICS_CI_REHEARSAL") != "true" ||
            Environment.GetEnvironmentVariable("EDULYTICS_CI_REHEARSAL_PROGRAMME") != "UAE-G10" ||
            Environment.GetEnvironmentVariable("EDULYTICS_CLEAN_DATABASE_NAME") != "edulytics_clean_ci" ||
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Staging" ||
            !IsDisposableConnection("ConnectionStrings__DefaultConnection") ||
            !IsDisposableConnection("ConnectionStrings__MigrationConnection") ||
            !IsDisposableConnection("EDULYTICS_CLEAN_DATABASE_CONNECTION"))
            throw new InvalidOperationException("G10 diagnostic contracts may only be exercised in isolated GitHub Actions PostgreSQL.");
        return true;
    }

    private static JsonDocument Read(string resource)
    {
        using var stream = typeof(UaeG10StageDRehearsalContracts).Assembly
            .GetManifestResourceStream(resource) ??
            throw new InvalidOperationException("Missing rehearsal registry: " + resource);
        return JsonDocument.Parse(stream);
    }

    private static string[] Values(JsonElement row, string property) =>
        row.TryGetProperty(property, out var value) && value.ValueKind == JsonValueKind.Array
            ? value.EnumerateArray().Select(x => x.GetString()!).ToArray() : [];

    internal static IReadOnlyList<LessonPracticeContract> Load()
    {
        var requested = Environment.GetEnvironmentVariable(ReleaseFlag);
        var release = string.Equals(requested, ReleaseToken, StringComparison.Ordinal);
        var diagnostic = Enabled();
        if (release && diagnostic)
            throw new InvalidOperationException("G10 release and diagnostic modes must be tested separately.");
        if (!release && !diagnostic)
            return [];

        using var mappingDoc = Read("Edulytics.Core.Mathematics.Curriculum.lesson-skill-mappings.v1.json");
        using var skillDoc = Read("Edulytics.Core.Mathematics.Skills.skill-registry.v1.json");
        using var familyDoc = Read("Edulytics.Core.Mathematics.Generation.question-family-registry.v1.json");
        var skills = skillDoc.RootElement.GetProperty("skills").EnumerateArray()
            .Select(x => x.GetProperty("id").GetString()!).ToHashSet(StringComparer.Ordinal);
        var families = familyDoc.RootElement.GetProperty("families").EnumerateArray()
            .ToDictionary(x => x.GetProperty("id").GetString()!, StringComparer.Ordinal);
        var blueprint = PedagogicalLessonBlueprintRegistry.LoadEmbeddedDocuments()
            .Single(x => x.PackCode == MathematicsCurriculumPackRegistry.UaeCode &&
                x.LogicalLevel == 10 && x.Pathway == "General" &&
                x.BlueprintCode == "UAE-G10-GENERAL-T1-OGL-V1:OGL-V1");
        var approved = blueprint.Lessons.Select(x => x.LessonCode).ToHashSet(StringComparer.Ordinal);
        var mappings = mappingDoc.RootElement.GetProperty("mappings").EnumerateArray()
            .Where(x => x.GetProperty("lessonCode").GetString()!.StartsWith(GeneralPrefix, StringComparison.Ordinal))
            .ToArray();
        if (approved.Count != 148 || blueprint.Units.Count != 18 || mappings.Length != 148 ||
            !approved.SetEquals(mappings.Select(x => x.GetProperty("lessonCode").GetString()!)))
            throw new InvalidOperationException("G10 rehearsal exact catalogue mismatch.");

        var results = new List<LessonPracticeContract>();
        foreach (var row in mappings)
        {
            var code = row.GetProperty("lessonCode").GetString()!;
            var allowed = Values(row, "allowedQuestionFamilies");
            if (allowed.Length == 0)
                continue; // open work remains teacher-reviewed; no invented family
            var primary = Values(row, "primarySkills");
            var forbidden = Values(row, "forbiddenQuestionFamilies");
            if (primary.Length == 0 || primary.Any(x => !skills.Contains(x)) ||
                allowed.Length != allowed.Distinct(StringComparer.Ordinal).Count() ||
                allowed.Intersect(forbidden, StringComparer.Ordinal).Any())
                throw new InvalidOperationException("G10 rehearsal invalid skill/allowed family mapping: " + code);
            var familySkills = new List<string>();
            foreach (var id in allowed)
            {
                if (!families.TryGetValue(id, out var family) ||
                    !family.TryGetProperty("lessonPracticeRouting", out var route) ||
                    route.ValueKind != JsonValueKind.True ||
                    string.IsNullOrWhiteSpace(family.GetProperty("verificationPolicy").GetString()) ||
                    !skills.Contains(family.GetProperty("skillId").GetString()!))
                    throw new InvalidOperationException("G10 rehearsal missing registered family: " + id);
                familySkills.Add(family.GetProperty("skillId").GetString()!);
            }
            var mechanic = row.GetProperty("practiceMechanic").GetString();
            if (string.IsNullOrWhiteSpace(mechanic))
                throw new InvalidOperationException("G10 rehearsal missing mechanic: " + code);
            // Only the explicit release token or the isolated diagnostics permit
            // these exact contracts. A Published pack alone does not grant routing.
            results.Add(new LessonPracticeContract(code, primary[0], mechanic, allowed,
                release ? "G10_FIRST_PARTY_EXACT_RELEASE_TOKEN" : "G10_DISPOSABLE_CI_DIAGNOSTIC_ONLY",
                "READY_VERIFIED", release ? "g10-first-party-release-v1" : "g10-stage-d-rehearsal-v1")
            {
                SkillIds = primary.Concat(familySkills).Distinct(StringComparer.Ordinal).ToArray()
            });
        }
        if (results.Count != 148 ||
            results.Sum(x => x.AllowedQuestionFamilies.Count) != 691 ||
            results.SelectMany(x => x.AllowedQuestionFamilies).Distinct(StringComparer.Ordinal).Count() != 678)
            throw new InvalidOperationException("G10 declared digital candidate denominator drift.");
        return results;
    }
}
