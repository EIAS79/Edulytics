namespace Edulytics.Web.GameRouting;

/// <summary>
/// Fail-closed production-routing policy for Mathematics V2 Grade 1-6 migration.
/// Stage 17 is readiness-driven: only lessons with an approved lesson-skill mapping
/// and an accepted exact learner-facing mechanic are listed here. ShadowVerified
/// solver families are not promoted by this policy and production routing remains
/// disabled for every lesson that is not explicitly listed.
/// </summary>
public static class MathematicsV2ProductMigrationPolicy
{
    /// <summary>
    /// Stage 17 Grade 1-6 rollout gate. An explicit value here takes precedence over
    /// the legacy pilot flag. Unset values remain fail-closed unless the legacy pilot
    /// flag was already explicitly enabled.
    /// </summary>
    public const string Grade16EnvironmentVariable = "EDULYTICS_MATH_V2_GRADE1_6_ROLLOUT";

    /// <summary>
    /// Backward-compatible pilot flag from PR #187. Kept during the staged migration
    /// so an existing deployment does not silently change behavior.
    /// </summary>
    public const string EnvironmentVariable = "EDULYTICS_MATH_V2_PRODUCT_MIGRATION_PILOT";

    // Keep the accepted runtime and telemetry keys stable for tranche 1. Stage 17
    // changes the readiness gate, not the renderer implementation.
    public const string RendererKey = "lesson-grounded-math-v2-pilot";
    public const string ClassificationSource = "math-v2-ready-contextual-pilot";

    public const string TwoUnknownsLessonCode = "PED:CAMBRIDGE-INTL-MATH:S6:6AS-MD-4:APPLY";
    public const string ScaleReadingLessonCode = "PED:CAMBRIDGE-INTL-MATH:S6:6NPV-4:APPLY";
    public const string FractionCompareLessonCode = "PED:CAMBRIDGE-INTL-MATH:S6:6F-3:BUILD";

    public sealed record Grade16RolloutEntry(
        string LessonCode,
        string SkillId,
        string Domain,
        int Grade,
        string Mechanic,
        string GenerationReadiness,
        bool UsesV2ShadowSolver);

    private static readonly Grade16RolloutEntry[] Grade16Rollout =
    [
        new(
            TwoUnknownsLessonCode,
            "algebra.relationships.two_unknowns",
            "algebra-reasoning",
            6,
            "TWO_UNKNOWNS",
            "READY_VERIFIED",
            false),
        new(
            ScaleReadingLessonCode,
            "measurement.scale.read_equal_intervals",
            "measurement",
            6,
            "SCALE_READING",
            "READY_VERIFIED",
            false),
        new(
            FractionCompareLessonCode,
            "fractions.compare.unlike_denominators",
            "fractions",
            6,
            "FRACTION_COMPARE_UNLIKE",
            "READY_VERIFIED",
            false)
    ];

    private static readonly IReadOnlyDictionary<string, Grade16RolloutEntry> Grade16ByLessonCode =
        Grade16Rollout.ToDictionary(entry => entry.LessonCode, StringComparer.Ordinal);

    /// <summary>
    /// The currently approved Grade 1-6 production tranche. This is intentionally
    /// small: the source lesson-skill registry currently approves only these three
    /// Grade 1-6 lessons. Future tranches must add evidence to the registry first.
    /// </summary>
    public static IReadOnlyList<Grade16RolloutEntry> ApprovedGrade16Entries => Grade16Rollout;

    public static bool IsEnabledFromEnvironment() =>
        IsEnabledFromValues(
            Environment.GetEnvironmentVariable(Grade16EnvironmentVariable),
            Environment.GetEnvironmentVariable(EnvironmentVariable));

    public static bool IsEnabledFromValues(string? grade16Value, string? legacyPilotValue)
    {
        // An explicit Stage 17 value always wins, including an explicit false/0.
        if (!string.IsNullOrWhiteSpace(grade16Value))
            return IsEnabledValue(grade16Value);

        return IsEnabledValue(legacyPilotValue);
    }

    public static bool IsEnabledValue(string? value) =>
        string.Equals(value?.Trim(), "true", StringComparison.OrdinalIgnoreCase)
        || string.Equals(value?.Trim(), "1", StringComparison.Ordinal);

    public static bool ShouldUseGrade16Rollout(
        string lessonCode,
        string mechanic,
        bool enabled)
    {
        if (!enabled || string.IsNullOrWhiteSpace(lessonCode) || string.IsNullOrWhiteSpace(mechanic))
            return false;

        if (!Grade16ByLessonCode.TryGetValue(lessonCode.Trim(), out var entry))
            return false;

        // Defense in depth: Stage 17 may never route a non-primary lesson, a shadow-only
        // solver family, or a lesson whose exact mechanic does not match the approved entry.
        return entry.Grade is >= 1 and <= 6
            && !entry.UsesV2ShadowSolver
            && string.Equals(entry.GenerationReadiness, "READY_VERIFIED", StringComparison.Ordinal)
            && string.Equals(entry.Mechanic, mechanic.Trim(), StringComparison.Ordinal);
    }

    /// <summary>
    /// Compatibility surface for the PR #187 resolver/tests. The behavior now delegates
    /// to the Stage 17 Grade 1-6 readiness gate rather than maintaining a second allow-list.
    /// </summary>
    public static bool ShouldUsePilot(
        string lessonCode,
        string mechanic,
        bool enabled) =>
        ShouldUseGrade16Rollout(lessonCode, mechanic, enabled);
}
