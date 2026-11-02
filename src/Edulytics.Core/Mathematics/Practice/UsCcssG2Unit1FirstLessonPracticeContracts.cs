namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// First-edition IM Grade 2 Unit 1 Lesson 1 arithmetic Practice.
/// Only the independently gradeable subskills are digital. Partner turn-taking,
/// a learner-authored expression record, spoken explanations, and the classroom
/// Math Community chart remain teacher-observed, not automatically certified.
/// </summary>
internal static class UsCcssG2Unit1FirstLessonPracticeContracts
{
    public const string Version = "usccss-g2-u01-l01-original-skill-v1";

    public static readonly IReadOnlyList<LessonPracticeContract> Reviewed =
    [
        new("PED:US-CCSS-MATH:G2:U01:L01",
            "number.whole.add_subtract",
            "TWO_CARDS_ADD_OR_SUBTRACT_WITHIN_10_AND_MAKE_TEN",
            [
                "usccss.g2.u01.l01.two_card_operation",
                "usccss.g2.u01.l01.match_expression_value",
                "usccss.g2.u01.l01.make_ten_missing_addend",
                "usccss.g2.u01.l01.recognize_ten_sum"
            ],
            "IM2021_G2_U01_L01_SOURCE_LESSON_REVIEWED",
            "READY_VERIFIED",
            Version)
    ];
}
