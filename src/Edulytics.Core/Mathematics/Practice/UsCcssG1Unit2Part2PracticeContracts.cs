namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Source-edition exact Grade 1 U02 L04-L05 Practice. Learner also must complete
/// non-autograded picture-story authoring and two-color center activities for UAT.
/// </summary>
internal static class UsCcssG1Unit2Part2PracticeContracts
{
    public const string Version="usccss-g1-u02-part2-practice-v1";
    public static readonly IReadOnlyList<LessonPracticeContract> Reviewed =
    [
        new("PED:US-CCSS-MATH:G1:U02:L04",
            "number.whole.add_subtract","COMPARE_RESULT_OR_CHANGE_UNKNOWN",
            ["usccss.g1.u02.story.mixed_result_or_change",
             "usccss.g1.u02.story.write_result_or_change_equation"],
            "IM2021_G1_U02_L04_MIXED_RESULT_CHANGE_TAKE_FROM","READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U02:L05",
            "number.whole.add_subtract","PICTURE_STORY_TO_EQUATION",
            ["usccss.g1.u02.center.picture_story_equation",
             "usccss.g1.u02.center.author_math_story"],
            "IM2021_G1_U02_L05_MATH_STORIES_CENTER","READY_VERIFIED",Version)
    ];
}
