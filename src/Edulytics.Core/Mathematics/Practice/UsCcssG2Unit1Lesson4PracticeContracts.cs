namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// IM 2021 Grade 2 U01 L04: exact digital subskills only.
/// Five-card choice and learner-authored closest-to-20 equation with possibly
/// several equally correct card sets remain teacher assessed.
/// </summary>
internal static class UsCcssG2Unit1Lesson4PracticeContracts
{
    public const string Version = "usccss-g2-u01-l04-how-close-v1";
    public static readonly IReadOnlyList<LessonPracticeContract> Reviewed =
    [
        new("PED:US-CCSS-MATH:G2:U01:L04",
            "number.whole.add_subtract",
            "HOW_CLOSE_FIVE_DIGIT_CARDS_AND_WITHIN20_STRATEGIES",
            [
                "usccss.g2.u01.l04.five_card_min_distance",
                "usccss.g2.u01.l04.within20_add_or_sub"
            ],
            "IM2021_G2_U01_L04_HOW_CLOSE_THREE_OF_FIVE_AND_MAKE_TEN",
            "READY_VERIFIED", Version)
    ];
}
