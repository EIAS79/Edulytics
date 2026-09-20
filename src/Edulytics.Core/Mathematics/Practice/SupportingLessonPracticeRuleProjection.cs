using System.Reflection;
using System.Text.Json;
using Edulytics.Core.Curriculum;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Fail-closed projection of reviewed Supporting target rules over every embedded
/// canonical lesson-content pack. Explicit hand-authored/mapping contracts remain
/// authoritative; this projection fills only still-unmapped Supporting lessons.
/// </summary>
internal static class SupportingLessonPracticeRuleProjection
{
    private const string SkillResource =
        "Edulytics.Core.Mathematics.Skills.skill-registry.v1.json";
    private const string FamilyResource =
        "Edulytics.Core.Mathematics.Generation.question-family-registry.v1.json";

    public const string ContractVersion = "supporting-target-rules-v1";

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
                foreach (var lesson in pack.Lessons)
                {
                    if (lesson.OutcomeCodes.Count != 0)
                        continue;

                    var translation = ChooseTranslation(pack, lesson);
                    if (translation is null ||
                        !SupportingPracticeTargetRuleRegistry.TryResolve(
                            lesson.LessonCode,
                            translation.Title,
                            out var rule) ||
                        rule is null)
                    {
                        continue;
                    }

                    if (!skillIds.Contains(rule.SkillId) ||
                        rule.Families.Count == 0 ||
                        rule.Families.Any(familyId =>
                            !familyById.TryGetValue(familyId, out var family) ||
                            !family.LessonPracticeRouting ||
                            !family.HasVerificationPolicy ||
                            !string.Equals(family.SkillId, rule.SkillId, StringComparison.Ordinal)))
                    {
                        continue;
                    }

                    projected.Add(new LessonPracticeContract(
                        lesson.LessonCode,
                        rule.SkillId,
                        rule.Mechanic,
                        rule.Families.Distinct(StringComparer.Ordinal).ToArray(),
                        "SupportingRule",
                        "READY_VERIFIED",
                        ContractVersion));
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
        catch (InvalidOperationException ex)
        {
            throw new InvalidOperationException(
                "Supporting Practice rule projection failed. See inner exception for the exact invalid projection condition.",
                ex);
        }
    }

    private static IEnumerable<CanonicalLessonContentPackDocument> LoadContentPacks()
    {
        var assembly = typeof(SupportingLessonPracticeRuleProjection).Assembly;
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        foreach (var resource in assembly.GetManifestResourceNames()
                     .Where(name => name.EndsWith(".lesson-content-pack.json", StringComparison.Ordinal))
                     .OrderBy(name => name, StringComparer.Ordinal))
        {
            using var stream = assembly.GetManifestResourceStream(resource);
            if (stream is null)
                continue;
            var document = JsonSerializer.Deserialize<CanonicalLessonContentPackDocument>(stream, options);
            if (document is not null)
                yield return document;
        }
    }

    private static CanonicalLessonContentPackTranslation? ChooseTranslation(
        CanonicalLessonContentPackDocument pack,
        CanonicalLessonContentPackLesson lesson) =>
        lesson.Translations.FirstOrDefault(x =>
            string.Equals(x.CultureCode, pack.AcademicLanguage, StringComparison.OrdinalIgnoreCase))
        ?? lesson.Translations.FirstOrDefault(x =>
            x.CultureCode.StartsWith("en", StringComparison.OrdinalIgnoreCase))
        ?? lesson.Translations.FirstOrDefault();

    private static JsonDocument? LoadDocument(string logicalName)
    {
        var stream = typeof(SupportingLessonPracticeRuleProjection)
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
