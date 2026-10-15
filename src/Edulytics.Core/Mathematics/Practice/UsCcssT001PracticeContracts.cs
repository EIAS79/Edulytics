namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Narrow Grade 1 Unit 1 (L02-L10) IM 2021 purpose-matched Practice routing.
/// Does not imply lesson certification, nor change any other lesson contract.
/// L01 retains its previously certified publisher-goal contract.
/// </summary>
internal static class UsCcssT001PracticeContracts
{
    public const string Version="usccss-t001-ten-practice-v1";
    public static readonly IReadOnlyList<LessonPracticeContract> Reviewed =
    [
        new("PED:US-CCSS-MATH:G1:U01:L02","number.whole.add",
            "DOT_PAIR_EXPRESSION_TO_TEN",
            ["usccss.t001.g1.dot_pair_sum"],"IM2021_G1_U01_L02_DOT_CARD_SUMS","READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U01:L03","number.whole.add",
            "COUNT_ON_ONE_TWO",
            ["usccss.t001.g1.add_one_or_two"],"IM2021_G1_U01_L03_ADD_ONE_TWO","READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U01:L04","number.whole.subtract",
            "COUNT_BACK_ONE_TWO",
            ["usccss.t001.g1.subtract_one_or_two"],"IM2021_G1_U01_L04_SUBTRACT_ONE_TWO","READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U01:L05","number.whole.add_subtract",
            "MIXED_ADD_SUBTRACT_WITHIN_TEN",
            ["usccss.t001.g1.mixed_add_subtract"],"IM2021_G1_U01_L05_MIXED_WITHIN_TEN","READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U01:L06","number.whole.add_subtract",
            "FIND_PAIR_MAKE_TEN",
            ["usccss.t001.g1.make_ten_missing_part"],"IM2021_G1_U01_L06_FIND_PAIR_TEN","READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U01:L07","supporting.statistics.charts",
            "SORT_AND_COUNT_TOOLS",
            ["usccss.t001.g1.sort_math_tools"],"IM2021_G1_U01_L07_SORT_TOOLS","READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U01:L08","supporting.statistics.charts",
            "SORT_SHAPES_THREE_CATEGORIES",
            ["usccss.t001.g1.sort_shape_cards"],"IM2021_G1_U01_L08_SHAPE_SORT","READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U01:L09","supporting.statistics.charts",
            "CLASS_SURVEY_COUNTS",
            ["usccss.t001.g1.class_survey_categories"],"IM2021_G1_U01_L09_SURVEY","READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U01:L10","number.whole.add_subtract",
            "HIDDEN_PART_TEN_AND_MIXED_CENTERS",
            ["usccss.t001.g1.make_ten_missing_part",
             "usccss.t001.g1.mixed_add_subtract"],"IM2021_G1_U01_L10_MAKE_TEN_MIXED","READY_VERIFIED",Version)
    ];
}
