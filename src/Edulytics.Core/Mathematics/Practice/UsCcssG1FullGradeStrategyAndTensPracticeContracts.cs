namespace Edulytics.Core.Mathematics.Practice;

/// <summary>G1 U03 L24–27 and U04 L01–05 source-activity numeric
/// contracts. U03 L28 is deliberately omitted: optional student-authored
/// story creation needs a teacher rubric, not a numeric PASS shortcut.</summary>
internal static class UsCcssG1FullGradeStrategyAndTensPracticeContracts
{
    public const string Version="us-ccss-g1-full-grade-u03-l24-through-u04-l05-numeric-practice-v1";
    public static readonly IReadOnlyList<LessonPracticeContract> Reviewed=
    [
        new("PED:US-CCSS-MATH:G1:U03:L24",
            "number.whole.add_subtract",
            "COUNT_ON_VS_TAKE_AWAY",
            ["usccss.g1.u03.l24.strategy.count_on_or_away"],
            "IM_K5_2021_U03_L24_NUMERIC_ONLY_SOURCE_GOAL",
            "READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U03:L25",
            "number.whole.add_subtract",
            "FLEXIBLE_SUBTRACTION_METHOD",
            ["usccss.g1.u03.l25.strategy.flexible_subtract"],
            "IM_K5_2021_U03_L25_NUMERIC_ONLY_SOURCE_GOAL",
            "READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U03:L26",
            "number.whole.add_subtract",
            "STORY_START_CHANGE_RESULT_UNKNOWNS",
            ["usccss.g1.u03.l26.story.unknown_positions"],
            "IM_K5_2021_U03_L26_NUMERIC_ONLY_SOURCE_GOAL",
            "READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U03:L27",
            "number.whole.add_subtract",
            "TEACHER_LED_CENTER_DIGITAL_NUMERIC_COMPONENT",
            ["usccss.g1.u03.l27.center.add_subtract"],
            "IM_K5_2021_U03_L27_NUMERIC_ONLY_SOURCE_GOAL",
            "READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U04:L01",
            "usccss.g1.ten_as_unit",
            "COUNT_GROUPS_OF_TEN_UP_TO_60",
            ["usccss.g1.u04.l01.count.groups_of_ten"],
            "IM_K5_2021_U04_L01_NUMERIC_ONLY_SOURCE_GOAL",
            "READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U04:L02",
            "usccss.g1.ten_as_unit",
            "MATCH_NUMERAL_GROUPS_OF_TEN",
            ["usccss.g1.u04.l02.match.tens_equivalence"],
            "IM_K5_2021_U04_L02_NUMERIC_ONLY_SOURCE_GOAL",
            "READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U04:L03",
            "usccss.g1.ten_as_unit",
            "ADD_SUBTRACT_ONE_TEN",
            ["usccss.g1.u04.l03.tens.add_or_remove_one"],
            "IM_K5_2021_U04_L03_NUMERIC_ONLY_SOURCE_GOAL",
            "READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U04:L04",
            "usccss.g1.ten_as_unit",
            "ADD_SUBTRACT_MULTIPLES_OF_TEN",
            ["usccss.g1.u04.l04.tens.add_remove_multiples"],
            "IM_K5_2021_U04_L04_NUMERIC_ONLY_SOURCE_GOAL",
            "READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U04:L05",
            "usccss.g1.ten_as_unit",
            "TEACHER_LED_TENS_CENTER_DIGITAL_NUMERIC_COMPONENT",
            ["usccss.g1.u04.l05.center.tens_and_within20"],
            "IM_K5_2021_U04_L05_NUMERIC_ONLY_SOURCE_GOAL",
            "READY_VERIFIED",Version)
    ];
}
