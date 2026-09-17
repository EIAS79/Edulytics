namespace Edulytics.Web.GameRouting;

/// <summary>
/// Exact allow-list for the first Mathematics V2 product-routing pilot.
/// The pilot deliberately reuses the already accepted lesson-grounded renderer;
/// it changes only the production routing decision. Any missing/unknown value
/// fails closed to the existing route.
/// </summary>
public static class MathematicsV2ProductMigrationPolicy
{
    public const string EnvironmentVariable = "EDULYTICS_MATH_V2_PRODUCT_MIGRATION_PILOT";
    public const string RendererKey = "lesson-grounded-math-v2-pilot";
    public const string ClassificationSource = "math-v2-ready-contextual-pilot";

    public const string TwoUnknownsLessonCode = "PED:CAMBRIDGE-INTL-MATH:S6:6AS-MD-4:APPLY";
    public const string ScaleReadingLessonCode = "PED:CAMBRIDGE-INTL-MATH:S6:6NPV-4:APPLY";
    public const string FractionCompareLessonCode = "PED:CAMBRIDGE-INTL-MATH:S6:6F-3:BUILD";

    private static readonly IReadOnlyDictionary<string, string> PilotMechanics =
        new Dictionary<string, string>(StringComparer.Ordinal)
        {
            [TwoUnknownsLessonCode] = "TWO_UNKNOWNS",
            [ScaleReadingLessonCode] = "SCALE_READING",
            [FractionCompareLessonCode] = "FRACTION_COMPARE_UNLIKE"
        };

    public static bool IsEnabledFromEnvironment() =>
        IsEnabledValue(Environment.GetEnvironmentVariable(EnvironmentVariable));

    public static bool IsEnabledValue(string? value) =>
        string.Equals(value?.Trim(), "true", StringComparison.OrdinalIgnoreCase)
        || string.Equals(value?.Trim(), "1", StringComparison.Ordinal);

    public static bool ShouldUsePilot(
        string lessonCode,
        string mechanic,
        bool enabled)
    {
        if (!enabled || string.IsNullOrWhiteSpace(lessonCode) || string.IsNullOrWhiteSpace(mechanic))
            return false;

        return PilotMechanics.TryGetValue(lessonCode.Trim(), out var expectedMechanic)
            && string.Equals(expectedMechanic, mechanic.Trim(), StringComparison.Ordinal);
    }
}
