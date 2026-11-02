using System.Text.Json;

namespace Edulytics.Core.Mathematics.Rollout;

public enum AdvancedMathematicsSurface
{
    Practice = 1,
    Assessment = 2,
    Exam = 3,
    Adaptive = 4
}

public enum AdvancedMathematicsReadiness
{
    EngineVerified = 1,
    PracticeReadyVerified = 2,
    AssessmentReadyVerified = 3,
    ExamReadyVerified = 4
}

public sealed record AdvancedMathematicsFamilyCertification(
    string FamilyId,
    AdvancedMathematicsReadiness HighestReadiness,
    bool AcademicReviewApproved,
    bool AdaptiveApproved,
    IReadOnlyList<string> Representations,
    IReadOnlyList<string> RequiredCapabilities);

public sealed record AdvancedMathematicsV3Policy(
    bool Enabled,
    bool PracticeEnabled,
    bool AssessmentEnabled,
    bool ExamEnabled,
    bool AdaptiveEnabled,
    IReadOnlySet<string> AllowedCurriculumLevelKeys,
    IReadOnlySet<Guid> AllowedSchoolIds)
{
    public bool AllowsSchool(Guid schoolId) =>
        AllowedSchoolIds.Count == 0 ||
        AllowedSchoolIds.Contains(schoolId);

    public bool AllowsLevel(string? levelKey) =>
        AllowedCurriculumLevelKeys.Count == 0 ||
        (!string.IsNullOrWhiteSpace(levelKey) &&
         AllowedCurriculumLevelKeys.Contains(
             levelKey.Trim().ToUpperInvariant()));
}

/// <summary>
/// V3-only certification surface. Existing verified legacy Lesson Practice
/// routing is intentionally not reclassified or modified by this registry.
/// New Advanced Mathematics V3 product routes must use this registry and the
/// rollout policy before exposing a family.
/// </summary>
public static class AdvancedMathematicsCertificationRegistry
{
    private const string ResourceName =
        "Edulytics.Core.Mathematics.Generation.question-family-registry.v1.json";

    private static readonly string[] AdvancedPrefixes =
    [
        "functions.",
        "calculus.",
        "trigonometry.",
        "geometry.coordinate.",
        "geometry.analytic.",
        "geometry.circle.",
        "geometry.conic.",
        "vectors.",
        "matrices.",
        "complex.",
        "sequences.",
        "series.",
        "limits.",
        "probability.",
        "statistics.",
        "numerical.",
        "mechanics.",
        "supporting.functions.",
        "supporting.calculus.",
        "supporting.trigonometry.",
        "supporting.geometry.analytic.",
        "supporting.vectors.",
        "supporting.matrices.",
        "supporting.complex.",
        "supporting.sequences.",
        "supporting.statistics.",
        "supporting.probability.",
        "supporting.numerical.",
        "supporting.mechanics."
    ];

    // Formal academic review remains separate from code-level mathematical
    // verification. Nothing enters this set until reviewed curriculum evidence
    // explicitly approves the family for a formal advanced programme.
    private static readonly IReadOnlySet<string>
        AcademicReviewApproved =
        new HashSet<string>(
            StringComparer.Ordinal);

    // Adaptive approval is intentionally empty in the first V3 production
    // foundation. M14 compatibility exists, but Primary 1-6 is never widened.
    private static readonly IReadOnlySet<string>
        AdaptiveApproved =
        new HashSet<string>(
            StringComparer.Ordinal);

    private static readonly IReadOnlyDictionary<
        string,
        AdvancedMathematicsFamilyCertification> ByFamily =
        Load();

    public static IReadOnlyCollection<
        AdvancedMathematicsFamilyCertification> All =>
        ByFamily.Values
            .OrderBy(
                row => row.FamilyId,
                StringComparer.Ordinal)
            .ToArray();

    public static bool IsAdvancedFamily(
        string? familyId)
    {
        if (string.IsNullOrWhiteSpace(familyId))
            return false;

        var value = familyId.Trim();
        return AdvancedPrefixes.Any(
            prefix =>
                value.StartsWith(
                    prefix,
                    StringComparison.Ordinal));
    }

    public static bool TryGet(
        string? familyId,
        out AdvancedMathematicsFamilyCertification? certification)
    {
        certification = null;

        if (string.IsNullOrWhiteSpace(familyId))
            return false;

        return ByFamily.TryGetValue(
            familyId.Trim(),
            out certification);
    }

    public static bool IsReadyFor(
        string familyId,
        AdvancedMathematicsSurface surface)
    {
        if (!TryGet(
                familyId,
                out var certification) ||
            certification is null)
        {
            return false;
        }

        return surface switch
        {
            AdvancedMathematicsSurface.Practice =>
                certification.HighestReadiness >=
                AdvancedMathematicsReadiness
                    .PracticeReadyVerified,

            AdvancedMathematicsSurface.Assessment =>
                certification.HighestReadiness >=
                AdvancedMathematicsReadiness
                    .AssessmentReadyVerified,

            AdvancedMathematicsSurface.Exam =>
                certification.HighestReadiness >=
                    AdvancedMathematicsReadiness
                        .ExamReadyVerified &&
                certification.AcademicReviewApproved,

            AdvancedMathematicsSurface.Adaptive =>
                certification.HighestReadiness >=
                    AdvancedMathematicsReadiness
                        .PracticeReadyVerified &&
                certification.AdaptiveApproved,

            _ => false
        };
    }

    public static bool CanRoute(
        string familyId,
        AdvancedMathematicsSurface surface,
        AdvancedMathematicsV3Policy policy,
        string? curriculumLevelKey,
        Guid schoolId,
        int? numericLearnerLevel = null)
    {
        ArgumentNullException.ThrowIfNull(policy);

        if (!policy.Enabled ||
            !policy.AllowsSchool(schoolId) ||
            !policy.AllowsLevel(curriculumLevelKey) ||
            !IsReadyFor(familyId, surface))
        {
            return false;
        }

        var surfaceEnabled = surface switch
        {
            AdvancedMathematicsSurface.Practice =>
                policy.PracticeEnabled,
            AdvancedMathematicsSurface.Assessment =>
                policy.AssessmentEnabled,
            AdvancedMathematicsSurface.Exam =>
                policy.ExamEnabled,
            AdvancedMathematicsSurface.Adaptive =>
                policy.AdaptiveEnabled,
            _ => false
        };

        if (!surfaceEnabled)
            return false;

        if (surface ==
                AdvancedMathematicsSurface.Adaptive &&
            (!numericLearnerLevel.HasValue ||
             numericLearnerLevel.Value <= 6))
        {
            return false;
        }

        return true;
    }

    private static IReadOnlyDictionary<
        string,
        AdvancedMathematicsFamilyCertification> Load()
    {
        var assembly =
            typeof(
                AdvancedMathematicsCertificationRegistry)
                .Assembly;

        using var stream =
            assembly.GetManifestResourceStream(
                ResourceName)
            ?? throw new InvalidOperationException(
                $"Missing embedded question-family registry: {ResourceName}.");

        using var document =
            JsonDocument.Parse(stream);

        var result =
            new Dictionary<
                string,
                AdvancedMathematicsFamilyCertification>(
                StringComparer.Ordinal);

        foreach (var family in
                 document.RootElement
                     .GetProperty("families")
                     .EnumerateArray())
        {
            var id =
                family.GetProperty("id")
                    .GetString()
                    ?.Trim()
                ?? string.Empty;

            if (!IsAdvancedFamily(id))
                continue;

            var status =
                family.GetProperty("status")
                    .GetString()
                    ?.Trim()
                ?? string.Empty;

            var engineVerified =
                status is
                    "ShadowVerified" or
                    "ProductionVerified";

            if (!engineVerified)
                continue;

            var lessonPracticeRouting =
                family.TryGetProperty(
                    "lessonPracticeRouting",
                    out var lessonRouting) &&
                lessonRouting.ValueKind ==
                    JsonValueKind.True;

            var productionRouting =
                family.TryGetProperty(
                    "productionRouting",
                    out var production) &&
                production.ValueKind ==
                    JsonValueKind.True;

            var readiness =
                productionRouting
                    ? AdvancedMathematicsReadiness
                        .AssessmentReadyVerified
                    : lessonPracticeRouting
                        ? AdvancedMathematicsReadiness
                            .PracticeReadyVerified
                        : AdvancedMathematicsReadiness
                            .EngineVerified;

            var representations =
                family.TryGetProperty(
                    "representations",
                    out var representationRows)
                    ? representationRows
                        .EnumerateArray()
                        .Select(
                            x =>
                                x.GetString()?.Trim())
                        .Where(
                            x =>
                                !string.IsNullOrWhiteSpace(x))
                        .Select(x => x!)
                        .ToArray()
                    : [];

            var capabilities =
                family.TryGetProperty(
                    "requiredCapabilities",
                    out var capabilityRows)
                    ? capabilityRows
                        .EnumerateArray()
                        .Select(
                            x =>
                                x.GetString()?.Trim())
                        .Where(
                            x =>
                                !string.IsNullOrWhiteSpace(x))
                        .Select(x => x!)
                        .ToArray()
                    : [];

            result[id] =
                new AdvancedMathematicsFamilyCertification(
                    id,
                    readiness,
                    AcademicReviewApproved.Contains(id),
                    AdaptiveApproved.Contains(id),
                    representations,
                    capabilities);
        }

        return result;
    }
}
