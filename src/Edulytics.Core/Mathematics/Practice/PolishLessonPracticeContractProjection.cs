using System.Reflection;
using System.Text.Json;
using Edulytics.Core.Curriculum;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Exact fail-closed projection for the 1,569 learner-facing Polish National
/// Mathematics lessons. Authorization is OutcomeCode-based only; generated
/// Phase-29 titles are never used as mapping evidence.
/// </summary>
internal static class PolishLessonPracticeContractProjection
{
    private const string SkillResource =
        "Edulytics.Core.Mathematics.Skills.skill-registry.v1.json";
    private const string FamilyResource =
        "Edulytics.Core.Mathematics.Generation.question-family-registry.v1.json";

    public const string ContractVersion = "polish-official-outcomes-v1";

    public static IReadOnlyList<LessonPracticeContract> Load()
    {
        using var skills = LoadDocument(SkillResource);
        using var families = LoadDocument(FamilyResource);

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
        foreach (var pack in LoadPolishContentPacks())
        {
            foreach (var lesson in pack.Lessons)
            {
                if (lesson.OutcomeCodes.Count == 0)
                    continue;

                var mappings = lesson.OutcomeCodes
                    .Select(code =>
                        PolishOutcomePracticeMapRegistry.TryResolve(code, out var mapping)
                            ? mapping
                            : null)
                    .ToArray();

                if (!mappings.All(x => x is not null))
                    continue;

                var targetRules = mappings
                    .Cast<PolishOutcomePracticeMapping>()
                    .SelectMany(x => x.TargetRules)
                    .DistinctBy(x => x.Id, StringComparer.Ordinal)
                    .ToArray();

                if (targetRules.Length == 0 ||
                    !targetRules.All(rule =>
                        IsRuntimeReadyRule(rule, skillIds, familyById)))
                {
                    continue;
                }

                var skillIdsForLesson = targetRules
                    .Select(rule => rule.SkillId)
                    .Distinct(StringComparer.Ordinal)
                    .OrderBy(value => value, StringComparer.Ordinal)
                    .ToArray();

                var familyIds = targetRules
                    .SelectMany(rule => rule.Families)
                    .Distinct(StringComparer.Ordinal)
                    .OrderBy(value => value, StringComparer.Ordinal)
                    .ToArray();

                projected.Add(new LessonPracticeContract(
                    lesson.LessonCode,
                    skillIdsForLesson[0],
                    targetRules.Length == 1
                        ? targetRules[0].Mechanic
                        : "POLISH_OFFICIAL_MULTI_TARGET",
                    familyIds,
                    "PolishOfficialOutcomeMap",
                    "READY_VERIFIED",
                    ContractVersion)
                {
                    SkillIds = skillIdsForLesson
                });
            }
        }

        return projected
            .GroupBy(x => x.LessonCode, StringComparer.Ordinal)
            .Select(group =>
            {
                var entries = group.ToArray();
                if (entries.Length != 1)
                {
                    throw new InvalidOperationException(
                        $"Duplicate Polish Practice contract for {group.Key}: {entries.Length}.");
                }

                return entries[0];
            })
            .OrderBy(x => x.LessonCode, StringComparer.Ordinal)
            .ToArray();
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
            string.Equals(family.SkillId, rule.SkillId, StringComparison.Ordinal));

    private static IEnumerable<CanonicalLessonContentPackDocument> LoadPolishContentPacks()
    {
        var assembly = typeof(PolishLessonPracticeContractProjection).Assembly;
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        foreach (var resource in assembly.GetManifestResourceNames()
                     .Where(name => name.EndsWith(
                         ".lesson-content-pack.json",
                         StringComparison.OrdinalIgnoreCase))
                     .OrderBy(name => name, StringComparer.Ordinal))
        {
            using var stream = assembly.GetManifestResourceStream(resource);
            if (stream is null)
                continue;

            var document = JsonSerializer.Deserialize<CanonicalLessonContentPackDocument>(
                stream,
                options);
            if (document is not null &&
                string.Equals(
                    document.PackCode,
                    "PL-NATIONAL-MATH",
                    StringComparison.Ordinal))
            {
                yield return document;
            }
        }
    }

    private static JsonDocument LoadDocument(string logicalName)
    {
        var stream = typeof(PolishLessonPracticeContractProjection)
            .Assembly
            .GetManifestResourceStream(logicalName)
            ?? throw new InvalidOperationException(
                $"Missing embedded Mathematics registry: {logicalName}.");

        using (stream)
            return JsonDocument.Parse(stream);
    }

    private sealed record FamilyRouting(
        string SkillId,
        bool LessonPracticeRouting,
        bool HasVerificationPolicy);
}
