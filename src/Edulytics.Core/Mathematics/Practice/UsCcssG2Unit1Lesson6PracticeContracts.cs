namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Grade 2 U01 L06 optional center day: digital Number Talk subskill only.
/// Card-unique puzzle gameboards and student-selected physical centers require
/// genuine teacher observation and are not auto-certified by this contract.
/// </summary>
internal static class UsCcssG2Unit1Lesson6PracticeContracts
{
    public const string Version = "usccss-g2-u01-l06-optional-center-v1";
    public static readonly IReadOnlyList<LessonPracticeContract> Reviewed =
    [
        new("PED:US-CCSS-MATH:G2:U01:L06",
            "number.whole.add_subtract",
            "TWO_DIGIT_MENTAL_NUMBER_TALK_WITHIN50",
            ["usccss.g2.u01.l06.number_talk_two_two_digit_within50"],
            "IM2021_G2_U01_L06_NUMBER_TALK_AND_PHYSICAL_NUMBER_PUZZLE",
            "READY_VERIFIED", Version)
    ];
}
