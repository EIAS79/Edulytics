namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// IM 2021 G2 U01 L09 bar heights, explicit unit-one scale and source
/// category arithmetic. Student-written true graph statements are separately
/// teacher-assessed by an independent learner task.
/// </summary>
internal static class UsCcssG2Unit1Lesson9BarGraphContracts
{
    public const string Version="usccss-g2-u01-l09-bar-graphs-source-v1";
    public static readonly IReadOnlyList<LessonPracticeContract> Reviewed=
    [
        new("PED:US-CCSS-MATH:G2:U01:L09",
            "data.categorical.represent_interpret",
            "READ_NUMBER_SCALE_AND_COMPARE_BAR_CATEGORY_COUNTS",
            [
                "usccss.g2.u01.l09.bar_category_height",
                "usccss.g2.u01.l09.bar_combine_or_compare",
                "usccss.g2.u01.l09.bar_total"
            ],
            "IM2021_G2_U01_L09_BAR_GRAPH_NUMBERED_AXIS_AND_CATEGORIES",
            "READY_VERIFIED",Version)
    ];
}
