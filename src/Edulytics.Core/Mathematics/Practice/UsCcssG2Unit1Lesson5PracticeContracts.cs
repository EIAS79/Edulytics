namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// IM 2021 G2 U01 L05: 7 digit cards, choose 4 to form two
/// two-digit addends near fifty; plus real within-fifty addition.
/// Selection, explanation and alternative-optimum equations are teacher-scored.
/// </summary>
internal static class UsCcssG2Unit1Lesson5PracticeContracts
{
    public const string Version = "usccss-g2-u01-l05-add50-howclose-v1";
    public static readonly IReadOnlyList<LessonPracticeContract> Reviewed =
    [
        new("PED:US-CCSS-MATH:G2:U01:L05",
            "number.whole.add_subtract",
            "SEVEN_DIGIT_CARDS_TWO_TWO_DIGIT_NUMBERS_TARGET50",
            [
                "usccss.g2.u01.l05.seven_cards_two_numbers_min_gap",
                "usccss.g2.u01.l05.add_within50_place_value"
            ],
            "IM2021_G2_U01_L05_ADD_WITHIN50_AND_HOW_CLOSE_STAGE3",
            "READY_VERIFIED", Version)
    ];
}
