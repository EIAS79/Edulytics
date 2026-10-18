namespace Edulytics.Core.Mathematics.Practice;

/// <summary>Source-reviewed original IM/Kendall Hunt 2021 G1 U02 L16–L20.
/// Reuse verified centers for L16; retain exact teacher-led activity limitations.
/// </summary>
internal static class UsCcssG1Unit2EquationsAndStoriesPracticeContracts
{
    public const string Version="usccss-g1-u02-l16-l20-source-practice-v1";
    public static readonly IReadOnlyList<LessonPracticeContract> Reviewed=
    [
        new("PED:US-CCSS-MATH:G1:U02:L16",
            "number.whole.add_subtract","CENTER_CAPTURE_SQUARES_SUBTRACTION",
            ["usccss.g1.u02.center.capture_squares.select_legal_edge",
             "usccss.g1.u02.center.mental_subtraction_within10"],
            "IM2021_G1_U02_L16_CAPTURE_SQUARES_SUBTRACT_CENTER", "READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U02:L17",
            "number.whole.add_subtract","COMPARE_STORY_UNKNOWN_STRUCTURES",
            ["usccss.g1.u02.story.compare_structure_and_equation"],
            "IM2021_G1_U02_L17_COMPARE_STORY_STRUCTURE", "READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U02:L18",
            "number.whole.add_subtract","MATCH_STORIES_AND_UNKNOWN_BOX_EQUATIONS",
            ["usccss.g1.u02.story.match_equation_with_unknown_box"],
            "IM2021_G1_U02_L18_UNKNOWN_BOX_STORY_EQUATIONS", "READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U02:L19",
            "number.whole.add_subtract","TWO_TRUE_EQUATIONS_PER_STORY",
            ["usccss.g1.u02.story.two_matching_equations"],
            "IM2021_G1_U02_L19_LOTERIA_RELATED_FACTS", "READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U02:L20",
            "number.whole.add_subtract","COMPOSE_STORY_FROM_EQUATION",
            ["usccss.g1.u02.story.compose_scaffold_from_equation"],
            "IM2021_G1_U02_L20_STORY_AUTHORING_SCAFFOLD", "READY_VERIFIED",Version)
    ];
}
