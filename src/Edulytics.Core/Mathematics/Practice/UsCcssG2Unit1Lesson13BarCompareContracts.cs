namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Source-specific G2 U01 L13 compare two unit-one bar values within 20,
/// find difference, related addition and subtraction, not category totals.
/// Student explanatory methods have a separate teacher-reviewed companion.
/// </summary>
internal static class UsCcssG2Unit1Lesson13BarCompareContracts
{
    public const string Version="usccss-g2-u01-l13-bar-compare-difference-v1";
    public static readonly IReadOnlyList<LessonPracticeContract> Reviewed=
    [
        new("PED:US-CCSS-MATH:G2:U01:L13",
            "data.categorical.represent_interpret",
            "COMPARE_BAR_UNKNOWN_DIFFERENCE_WITHIN20_COUNT_ON_BACK",
            [
                "usccss.g2.u01.l13.bar_unknown_difference",
                "usccss.g2.u01.l13.related_addition_missing_difference",
                "usccss.g2.u01.l13.related_subtraction_difference"
            ],
            "IM2021_G2_U01_L13_BAR_COMPARE_TWO_EQUATIONS",
            "READY_VERIFIED",Version)
    ];
}
