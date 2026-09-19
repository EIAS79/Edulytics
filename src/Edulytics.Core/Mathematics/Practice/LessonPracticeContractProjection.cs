using System.Reflection;
using System.Text.Json;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Fail-closed projection from approved lesson-skill mapping metadata into
/// learner-facing exact Practice contracts. Projection never authorizes a
/// lesson unless the mapping explicitly declares READY_VERIFIED and explicitly
/// lists question families that are themselves marked lessonPracticeRouting.
/// </summary>
internal static class LessonPracticeContractProjection
{
    private const string MappingResource =
        "Edulytics.Core.Mathematics.Curriculum.lesson-skill-mappings.v1.json";
    private const string SkillResource =
        "Edulytics.Core.Mathematics.Skills.skill-registry.v1.json";
    private const string FamilyResource =
        "Edulytics.Core.Mathematics.Generation.question-family-registry.v1.json";

    public const string ContractVersion = "lesson-practice-projection-v1";

    public static IReadOnlyList<LessonPracticeContract> Load()
    {
        try
        {
            using var mappings = LoadDocument(MappingResource);
            using var skills = LoadDocument(SkillResource);
            using var families = LoadDocument(FamilyResource);
            if (mappings is null || skills is null || families is null)
                return [];

            var skillIds = skills.RootElement
                .GetProperty("skills")
                .EnumerateArray()
                .Where(row => row.TryGetProperty("id", out _))
                .Select(row => row.GetProperty("id").GetString())
                .Where(value => !string.IsNullOrWhiteSpace(value))
                .ToHashSet(StringComparer.Ordinal);

            var familyById = families.RootElement
                .GetProperty("families")
                .EnumerateArray()
                .Where(row => row.TryGetProperty("id", out _))
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
            foreach (var row in mappings.RootElement.GetProperty("mappings").EnumerateArray())
            {
                if (!row.TryGetProperty("lessonCode", out var lessonCodeNode) ||
                    !row.TryGetProperty("practiceReadiness", out var readinessNode) ||
                    !string.Equals(
                        readinessNode.GetString(),
                        "READY_VERIFIED",
                        StringComparison.Ordinal) ||
                    !row.TryGetProperty("primarySkills", out var primaryNode) ||
                    primaryNode.ValueKind != JsonValueKind.Array)
                {
                    continue;
                }

                var primarySkills = primaryNode
                    .EnumerateArray()
                    .Select(value => value.GetString())
                    .Where(value => !string.IsNullOrWhiteSpace(value))
                    .Cast<string>()
                    .ToArray();
                if (primarySkills.Length != 1 || !skillIds.Contains(primarySkills[0]))
                    continue;

                if (!row.TryGetProperty("allowedQuestionFamilies", out var allowedNode) ||
                    allowedNode.ValueKind != JsonValueKind.Array)
                {
                    continue;
                }

                var allowed = allowedNode
                    .EnumerateArray()
                    .Select(value => value.GetString())
                    .Where(value => !string.IsNullOrWhiteSpace(value))
                    .Cast<string>()
                    .Distinct(StringComparer.Ordinal)
                    .ToArray();

                if (allowed.Length == 0 ||
                    allowed.Any(familyId =>
                        !familyById.TryGetValue(familyId, out var family) ||
                        !family.LessonPracticeRouting ||
                        !family.HasVerificationPolicy ||
                        !string.Equals(family.SkillId, primarySkills[0], StringComparison.Ordinal)))
                {
                    continue;
                }

                var lessonCode = lessonCodeNode.GetString();
                if (string.IsNullOrWhiteSpace(lessonCode))
                    continue;

                var mechanic = row.TryGetProperty("practiceMechanic", out var mechanicNode)
                    ? mechanicNode.GetString()
                    : null;
                var sourceType = row.TryGetProperty("sourceType", out var sourceNode)
                    ? sourceNode.GetString()
                    : null;

                projected.Add(new LessonPracticeContract(
                    lessonCode,
                    primarySkills[0],
                    string.IsNullOrWhiteSpace(mechanic) ? "SUPPORTING_EXACT" : mechanic!,
                    allowed,
                    string.IsNullOrWhiteSpace(sourceType) ? "ApprovedLessonSkillMapping" : sourceType!,
                    "READY_VERIFIED",
                    ContractVersion));
            }

            return projected;
        }
        catch (JsonException)
        {
            // A malformed manifest must fail closed without taking the site down.
            return [];
        }
        catch (InvalidOperationException)
        {
            return [];
        }
    }

    private static JsonDocument? LoadDocument(string logicalName)
    {
        var stream = typeof(LessonPracticeContractProjection)
            .Assembly
            .GetManifestResourceStream(logicalName);
        if (stream is null)
            return null;

        using (stream)
        {
            return JsonDocument.Parse(stream);
        }
    }

    private sealed record FamilyRouting(
        string SkillId,
        bool LessonPracticeRouting,
        bool HasVerificationPolicy);
}
