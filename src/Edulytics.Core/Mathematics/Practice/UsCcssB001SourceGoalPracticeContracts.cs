namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Source-targeted IM2021 teaching goal families for four repaired B001 lessons.
/// These override generic skill projections for just these canonical lesson IDs.
/// </summary>
internal static class UsCcssB001SourceGoalPracticeContracts
{
    public const string Version="usccss-b001-4-goals-exact-practice-v1";
    public static readonly IReadOnlyList<LessonPracticeContract> Reviewed =
    [
        new("PED:US-CCSS-MATH:G2:U02:L01",
            "number.whole.add_subtract",
            "NO_REGROUP_BAR_COMPARE_OR_TOTAL",
            ["usccss.g2.u02.bar_total_or_difference.no_regroup"],
            "IM2021_G2_U02_L01_GRAPH_NO_COMPOSE_DECOMPOSE_TEN",
            "READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G2:U03:L12",
            "number.whole.add_subtract",
            "TWO_STEP_LENGTH_TAPE_DIAGRAM",
            ["usccss.g2.u03.two_step_ribbon_length"],
            "IM2021_G2_U03_L12_TWO_STEP_RIBBON_LENGTH",
            "READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G3:U01:L12",
            "number.whole.multiply",
            "EQUAL_GROUPS_STORY_MULTIPLICATION",
            ["usccss.g3.u01.equal_groups.model_product"],
            "IM2021_G3_U01_L12_EQUAL_GROUPS_REPRESENT_SOLVE",
            "READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G3:U01:L15",
            "number.whole.multiply",
            "UNKNOWN_MULTIPLICATION_FACTOR",
            ["usccss.g3.u01.equal_groups.unknown_factor"],
            "IM2021_G3_U01_L15_UNKNOWN_FACTOR_EQUATIONS",
            "READY_VERIFIED",Version)
    ];
}
