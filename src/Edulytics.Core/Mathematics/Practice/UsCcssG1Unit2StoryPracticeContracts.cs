namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Exact source-goal scoped Practice for IM 2021 G1 Unit 2 lessons 1–3.
/// These numerical answer families ask different questions; do not substitute
/// a general arithmetic worksheet for original expression/equation reasoning.
/// </summary>
internal static class UsCcssG1Unit2StoryPracticeContracts
{
    public const string Version="usccss-g1-u02-story-practice-authored-v2";

    public static readonly IReadOnlyList<LessonPracticeContract> Reviewed =
    [
        new("PED:US-CCSS-MATH:G1:U02:L01",
            "number.whole.add_subtract",
            "STORY_TO_EXPRESSION",
            ["usccss.g1.u02.story.write_expression","usccss.g1.u02.story.choose_expression"],
            "IM2021_G1_U02_L01_ADD_TO_TAKE_FROM_RESULT_UNKNOWN",
            "READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U02:L02",
            "number.whole.add_subtract",
            "STORY_TO_EQUATION",
            ["usccss.g1.u02.story.write_equation","usccss.g1.u02.story.choose_equation"],
            "IM2021_G1_U02_L02_MATCH_STORY_EQUATION",
            "READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U02:L03",
            "number.whole.add_subtract",
            "ADD_TO_CHANGE_UNKNOWN",
            ["usccss.g1.u02.story.add_change_unknown"],
            "IM2021_G1_U02_L03_FIND_CHANGE_NOT_TOTAL",
            "READY_VERIFIED",Version)
    ];
}
