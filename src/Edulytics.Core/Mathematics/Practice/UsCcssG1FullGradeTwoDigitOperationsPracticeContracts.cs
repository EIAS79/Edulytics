namespace Edulytics.Core.Mathematics.Practice;

/// <summary>Reviewed digital mathematics scope of new G1 U04 L11–L15;
/// independent mental strategy, optional center and oral comparison require
/// additional evidence beyond numerical auto-grading.</summary>
internal static class UsCcssG1FullGradeTwoDigitOperationsPracticeContracts
{
    public const string Version="us-ccss-g1-full-grade-u04-l11-l15-two-digit-operations-v1";
    public static readonly IReadOnlyList<LessonPracticeContract> Reviewed=
    [
        new("PED:US-CCSS-MATH:G1:U04:L11",
            "usccss.g1.ten_as_unit",
            "ADD_TENS_TO_TWO_DIGIT",
            ["usccss.g1.u04.l11.add_tens.two_digit"],
            "IM_K5_2021_U04_L11_DIGITAL_MATH_SUBSET_ONLY","READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U04:L12",
            "usccss.g1.ten_as_unit",
            "TEN_MORE_TEN_LESS",
            ["usccss.g1.u04.l12.mental.ten_more_less"],
            "IM_K5_2021_U04_L12_DIGITAL_MATH_SUBSET_ONLY","READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U04:L13",
            "usccss.g1.ten_as_unit",
            "CENTER_REVIEW_WITHIN20_AND_PLACE_VALUE",
            ["usccss.g1.u04.l13.center.place_value_mix"],
            "IM_K5_2021_U04_L13_DIGITAL_MATH_SUBSET_ONLY","READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U04:L14",
            "usccss.g1.ten_as_unit",
            "COMPARE_VALUES_GREATER_LESS_WORDS",
            ["usccss.g1.u04.l14.compare.two_digits_words"],
            "IM_K5_2021_U04_L14_DIGITAL_MATH_SUBSET_ONLY","READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U04:L15",
            "usccss.g1.ten_as_unit",
            "INTERPRET_GREATER_LESS_EQUALS_SYMBOLS",
            ["usccss.g1.u04.l15.compare.symbols"],
            "IM_K5_2021_U04_L15_DIGITAL_MATH_SUBSET_ONLY","READY_VERIFIED",Version)
    ];
}
