namespace Edulytics.Core.Mathematics.Practice;

/// <summary>Grade 1 new U04 L06–L10 place-value numeric portions,
/// not automatic certification of physical counting, drawing, reading aloud
/// or teacher-led gallery walks.</summary>
internal static class UsCcssG1FullGradeTensAndOnesPracticeContracts
{
    public const string Version="us-ccss-g1-full-grade-u04-l06-l10-place-value-v1";
    public static readonly IReadOnlyList<LessonPracticeContract> Reviewed=
    [
        new("PED:US-CCSS-MATH:G1:U04:L06",
            "usccss.g1.ten_as_unit",
            "COUNT_MIXED_COLLECTIONS_TENS_ONES",
            ["usccss.g1.u04.l06.collection.tens_ones"],
            "IM_K5_2021_U04_L06_DIGITAL_NUMERIC_SUBSET_ONLY","READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U04:L07",
            "usccss.g1.ten_as_unit",
            "READ_TWO_DIGIT_IN_WORDS",
            ["usccss.g1.u04.l07.read.two_digit"],
            "IM_K5_2021_U04_L07_DIGITAL_NUMERIC_SUBSET_ONLY","READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U04:L08",
            "usccss.g1.ten_as_unit",
            "MATCH_TENS_ONES_AND_EXPANDED_EXPRESSION",
            ["usccss.g1.u04.l08.match.tens_ones_expression"],
            "IM_K5_2021_U04_L08_DIGITAL_NUMERIC_SUBSET_ONLY","READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U04:L09",
            "usccss.g1.ten_as_unit",
            "REPRESENT_TWO_DIGIT_WITH_EXPANDED_FORM",
            ["usccss.g1.u04.l09.show.expanded_form"],
            "IM_K5_2021_U04_L09_DIGITAL_NUMERIC_SUBSET_ONLY","READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U04:L10",
            "usccss.g1.ten_as_unit",
            "WRITE_TWO_DIGIT_FROM_TENS_ONES",
            ["usccss.g1.u04.l10.write.two_digit_numeral"],
            "IM_K5_2021_U04_L10_DIGITAL_NUMERIC_SUBSET_ONLY","READY_VERIFIED",Version)
    ];
}
