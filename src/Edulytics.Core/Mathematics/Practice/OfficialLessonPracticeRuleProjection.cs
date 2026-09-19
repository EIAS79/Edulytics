using System.Reflection;
using System.Text.Json;
using Edulytics.Core.Curriculum;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Fail-closed projection for official/outcome-mapped lessons whose canonical
/// title full-matches one reviewed anchored Practice target rule. Broad
/// keyword-only rules are deliberately excluded from this path.
/// </summary>
internal static class OfficialLessonPracticeRuleProjection
{
    private const string SkillResource =
        "Edulytics.Core.Mathematics.Skills.skill-registry.v1.json";
    private const string FamilyResource =
        "Edulytics.Core.Mathematics.Generation.question-family-registry.v1.json";

    public const string ContractVersion = "official-outcome-rules-v1";

    public static IReadOnlyList<LessonPracticeContract> Load()
    {
        try
        {
            using var skills = LoadDocument(SkillResource);
            using var families = LoadDocument(FamilyResource);
            if (skills is null || families is null)
                return [];

            var skillIds = skills.RootElement
                .GetProperty("skills")
                .EnumerateArray()
                .Select(row => row.GetProperty("id").GetString())
                .Where(value => !string.IsNullOrWhiteSpace(value))
                .Cast<string>()
                .ToHashSet(StringComparer.Ordinal);

            var familyById = families.RootElement
                .GetProperty("families")
                .EnumerateArray()
                .ToDictionary(
                    row => row.GetProperty("id").GetString()!,
                    row => new FamilyRouting(
                        row.GetProperty("skillId").GetString() ?? string.Empty,
                        row.TryGetProperty("lessonPracticeRouting", out var routing) &&
                            routing.ValueKind == JsonValueKind.True,
                        row.TryGetProperty("verificationPolicy", out var verification) &&
                            !string.IsNullOrWhiteSpace(verification.GetString())),
                    StringComparer.Ordinal);

            var projected = new List<LessonPracticeContract>();
            foreach (var pack in LoadContentPacks())
            {
                // Current Polish Phase-29 lesson nodes are documented
                // OfficialFrameworkOnly fallback identities with broad repeated
                // domain content. They require explicit eligibility evidence or
                // later exact source remediation; title rules may not promote them.
                if (string.Equals(
                        pack.PackCode,
                        "PL-NATIONAL-MATH",
                        StringComparison.Ordinal))
                {
                    continue;
                }

                foreach (var lesson in pack.Lessons)
                {
                    if (lesson.OutcomeCodes.Count == 0)
                        continue;

                    var translation = ChooseTranslation(pack, lesson);
                    if (translation is not null &&
                        SupportingPracticeTargetRuleRegistry.TryResolveReviewedOfficialLesson(
                            lesson.LessonCode,
                            translation.Title,
                            translation.Explanation,
                            translation.KeyConceptsAndRules,
                            translation.WorkedExamples,
                            out var canonicalRule) &&
                        canonicalRule is not null &&
                        IsRuntimeReadyRule(canonicalRule, skillIds, familyById))
                    {
                        projected.Add(new LessonPracticeContract(
                            lesson.LessonCode,
                            canonicalRule.SkillId,
                            canonicalRule.Mechanic,
                            canonicalRule.Families
                                .Distinct(StringComparer.Ordinal)
                                .ToArray(),
                            "OfficialReviewedCanonicalEvidence",
                            "READY_VERIFIED",
                            ContractVersion)
                        {
                            SkillIds = [canonicalRule.SkillId]
                        });
                        continue;
                    }

                    var resolvedOutcomes = lesson.OutcomeCodes
                        .Select(code =>
                            OfficialOutcomePracticeRuleRegistry.TryResolve(
                                code,
                                out var resolution)
                                ? resolution
                                : null)
                        .ToArray();

                    if (!resolvedOutcomes.All(x => x is not null))
                        continue;

                    var targetRules = resolvedOutcomes
                        .Cast<OfficialOutcomePracticeResolution>()
                        .Select(x => x.TargetRule)
                        .DistinctBy(x => x.Id, StringComparer.Ordinal)
                        .ToArray();

                    if (targetRules.Length == 0 ||
                        !targetRules.All(rule =>
                            IsRuntimeReadyRule(
                                rule,
                                skillIds,
                                familyById)))
                    {
                        continue;
                    }

                    var skillsForLesson = targetRules
                        .Select(rule => rule.SkillId)
                        .Distinct(StringComparer.Ordinal)
                        .OrderBy(value => value, StringComparer.Ordinal)
                        .ToArray();
                    var familiesForLesson = targetRules
                        .SelectMany(rule => rule.Families)
                        .Distinct(StringComparer.Ordinal)
                        .OrderBy(value => value, StringComparer.Ordinal)
                        .ToArray();
                    var mechanic = targetRules.Length == 1
                        ? targetRules[0].Mechanic
                        : "OFFICIAL_MULTI_OUTCOME";

                    projected.Add(new LessonPracticeContract(
                        lesson.LessonCode,
                        skillsForLesson[0],
                        mechanic,
                        familiesForLesson,
                        "OfficialOutcomeRule",
                        "READY_VERIFIED",
                        ContractVersion)
                    {
                        SkillIds = skillsForLesson
                    });
                }
            }

            return projected
                .GroupBy(x => x.LessonCode, StringComparer.Ordinal)
                .Select(group => group.Single())
                .OrderBy(x => x.LessonCode, StringComparer.Ordinal)
                .ToArray();
        }
        catch (JsonException)
        {
            return [];
        }
        catch (InvalidOperationException)
        {
            return [];
        }
    }

    private static bool IsRuntimeReadyRule(
        SupportingPracticeTargetRule rule,
        IReadOnlySet<string> skillIds,
        IReadOnlyDictionary<string, FamilyRouting> familyById) =>
        skillIds.Contains(rule.SkillId) &&
        rule.Families.Count > 0 &&
        rule.Families.All(familyId =>
            familyById.TryGetValue(familyId, out var family) &&
            family.LessonPracticeRouting &&
            family.HasVerificationPolicy &&
            string.Equals(
                family.SkillId,
                rule.SkillId,
                StringComparison.Ordinal));

    private static IEnumerable<CanonicalLessonContentPackDocument> LoadContentPacks()
    {
        var assembly = typeof(OfficialLessonPracticeRuleProjection).Assembly;
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        foreach (var resource in assembly.GetManifestResourceNames()
                     .Where(name => name.EndsWith(
                         ".lesson-content-pack.json",
                         StringComparison.Ordinal))
                     .OrderBy(name => name, StringComparer.Ordinal))
        {
            using var stream = assembly.GetManifestResourceStream(resource);
            if (stream is null)
                continue;
            var document = JsonSerializer.Deserialize<CanonicalLessonContentPackDocument>(
                stream,
                options);
            if (document is not null)
                yield return document;
        }
    }

    private static CanonicalLessonContentPackTranslation? ChooseTranslation(
        CanonicalLessonContentPackDocument pack,
        CanonicalLessonContentPackLesson lesson) =>
        lesson.Translations.FirstOrDefault(x =>
            string.Equals(
                x.CultureCode,
                pack.AcademicLanguage,
                StringComparison.OrdinalIgnoreCase))
        ?? lesson.Translations.FirstOrDefault(x =>
            x.CultureCode.StartsWith("en", StringComparison.OrdinalIgnoreCase))
        ?? lesson.Translations.FirstOrDefault();

    private static JsonDocument? LoadDocument(string logicalName)
    {
        var stream = typeof(OfficialLessonPracticeRuleProjection)
            .Assembly
            .GetManifestResourceStream(logicalName);
        if (stream is null)
            return null;
        using (stream)
            return JsonDocument.Parse(stream);
    }

    private sealed record FamilyRouting(
        string SkillId,
        bool LessonPracticeRouting,
        bool HasVerificationPolicy);
}
