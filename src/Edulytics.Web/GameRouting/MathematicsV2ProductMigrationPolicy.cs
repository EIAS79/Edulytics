namespace Edulytics.Web.GameRouting;

/// <summary>
/// Fail-closed production-routing policy for Mathematics V2 Grade 1-6 migration.
/// Stage 17 is complete only for lessons that have an approved exact SkillContract,
/// acceptable semantic evidence, a reviewed exact learner-facing mechanic and
/// READY_VERIFIED generation readiness. Every other Grade 1-6 lesson remains on
/// the existing route or is blocked for review.
/// </summary>
public static class MathematicsV2ProductMigrationPolicy
{
    public const string Grade16EnvironmentVariable = "EDULYTICS_MATH_V2_GRADE1_6_ROLLOUT";
    public const string EnvironmentVariable = "EDULYTICS_MATH_V2_PRODUCT_MIGRATION_PILOT";

    // Stable renderer/telemetry keys retained for backward compatibility.
    public const string RendererKey = "lesson-grounded-math-v2-pilot";
    public const string ClassificationSource = "math-v2-ready-contextual-pilot";

    public const string TwoUnknownsLessonCode = "PED:CAMBRIDGE-INTL-MATH:S6:6AS-MD-4:APPLY";
    public const string ScaleReadingLessonCode = "PED:CAMBRIDGE-INTL-MATH:S6:6NPV-4:APPLY";
    public const string FractionCompareLessonCode = "PED:CAMBRIDGE-INTL-MATH:S6:6F-3:BUILD";
    public const string FractionCompareApplyLessonCode = "PED:CAMBRIDGE-INTL-MATH:S6:6F-3:APPLY";
    public const string EquivalentFractionsBuildLessonCode = "PED:CAMBRIDGE-INTL-MATH:S5:5F-2:BUILD";
    public const string EquivalentFractionsApplyLessonCode = "PED:CAMBRIDGE-INTL-MATH:S5:5F-2:APPLY";
    public const string UnitRateLessonCode = "PED:US-CCSS-MATH:G6:U03:L07";

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
        new(TwoUnknownsLessonCode, "algebra.relationships.two_unknowns", "algebra-reasoning", 6, "TWO_UNKNOWNS", "READY_VERIFIED", false),
        new(ScaleReadingLessonCode, "measurement.scale.read_equal_intervals", "measurement", 6, "SCALE_READING", "READY_VERIFIED", false),
        new(FractionCompareLessonCode, "fractions.compare.unlike_denominators", "fractions", 6, "FRACTION_COMPARE_UNLIKE", "READY_VERIFIED", false),
        new(FractionCompareApplyLessonCode, "fractions.compare.unlike_denominators", "fractions", 6, "FRACTION_COMPARE_UNLIKE", "READY_VERIFIED", false),
        new(EquivalentFractionsBuildLessonCode, "fractions.equivalent", "fractions", 5, "FRACTION_EQUIVALENT", "READY_VERIFIED", false),
        new(EquivalentFractionsApplyLessonCode, "fractions.equivalent", "fractions", 5, "FRACTION_EQUIVALENT", "READY_VERIFIED", false),

        new("PED:US-CCSS-MATH:G3:U05:L10", "fractions.equivalent", "fractions", 3, "FRACTION_EQUIVALENT", "READY_VERIFIED", false),
        new("PED:US-CCSS-MATH:G3:U05:L11", "fractions.equivalent", "fractions", 3, "FRACTION_EQUIVALENT", "READY_VERIFIED", false),
        new("PED:US-CCSS-MATH:G3:U05:L12", "fractions.equivalent", "fractions", 3, "FRACTION_EQUIVALENT", "READY_VERIFIED", false),
        new("PED:US-CCSS-MATH:G4:U02:L07", "fractions.equivalent", "fractions", 4, "FRACTION_EQUIVALENT", "READY_VERIFIED", false),
        new("PED:US-CCSS-MATH:G4:U02:L08", "fractions.equivalent", "fractions", 4, "FRACTION_EQUIVALENT", "READY_VERIFIED", false),
        new("PED:US-CCSS-MATH:G4:U02:L10", "fractions.equivalent", "fractions", 4, "FRACTION_EQUIVALENT", "READY_VERIFIED", false),
        new("PED:US-CCSS-MATH:G4:U02:L11", "fractions.equivalent", "fractions", 4, "FRACTION_EQUIVALENT", "READY_VERIFIED", false),

        new(UnitRateLessonCode, "ratio.unit_rate", "ratio", 6, "UNIT_RATE", "READY_VERIFIED", false)
    ];

    private static readonly IReadOnlyDictionary<string, Grade16RolloutEntry> Grade16ByLessonCode =
        Grade16Rollout.ToDictionary(entry => entry.LessonCode, StringComparer.Ordinal);

    /// <summary>
    /// Complete Stage 17 Grade 1-6 production allow-list. The closure audit proves
    /// that every READY_VERIFIED Grade 1-6 lesson is present here and every other
    /// Grade 1-6 lesson remains fail-closed.
    /// </summary>
    public static IReadOnlyList<Grade16RolloutEntry> ApprovedGrade16Entries => Grade16Rollout;

    public static bool TryGetApprovedGrade16Entry(
        string lessonCode,
        out Grade16RolloutEntry? entry)
    {
        if (string.IsNullOrWhiteSpace(lessonCode))
        {
            entry = null;
            return false;
        }

        return Grade16ByLessonCode.TryGetValue(lessonCode.Trim(), out entry);
    }

    public static bool IsEnabledFromEnvironment() =>
        IsEnabledFromValues(
            Environment.GetEnvironmentVariable(Grade16EnvironmentVariable),
            Environment.GetEnvironmentVariable(EnvironmentVariable));

    public static bool IsEnabledFromValues(string? grade16Value, string? legacyPilotValue)
    {
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
        if (!enabled || string.IsNullOrWhiteSpace(mechanic))
            return false;

        if (!TryGetApprovedGrade16Entry(lessonCode, out var entry) || entry is null)
            return false;

        return entry.Grade is >= 1 and <= 6
            && !entry.UsesV2ShadowSolver
            && string.Equals(entry.GenerationReadiness, "READY_VERIFIED", StringComparison.Ordinal)
            && string.Equals(entry.Mechanic, mechanic.Trim(), StringComparison.Ordinal);
    }

    public static bool ShouldUsePilot(
        string lessonCode,
        string mechanic,
        bool enabled) =>
        ShouldUseGrade16Rollout(lessonCode, mechanic, enabled);
}
