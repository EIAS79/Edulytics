namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Five individually source-scoped Grade 1 Unit 1 contracts. Reuse tested
/// T001 sorting/survey/arithmetic families where the original purposes match.
/// </summary>
internal static class UsCcssG1Batch03PracticeContracts
{
    public const string Version="usccss-g1-b03-five-practice-v1";
    public static readonly IReadOnlyList<LessonPracticeContract> Reviewed =
    [
        new("PED:US-CCSS-MATH:G1:U01:L11","supporting.statistics.charts",
            "READ_TALLY_AND_TEST_DATA_STATEMENTS",
            ["usccss.g1.u01.b03.check_data_statement",
             "usccss.g1.u01.b03.count_category_data"],
            "IM2021_G1_U01_L11_CLASS_PET_SURVEY_STATEMENTS","READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U01:L12","supporting.statistics.charts",
            "READ_COUNTS_AND_TOTALS_FROM_DATA",
            ["usccss.g1.u01.b03.count_category_data"],
            "IM2021_G1_U01_L12_COUNT_DATA_CATEGORIES","READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U01:L13","supporting.statistics.charts",
            "ASK_ANSWERABLE_DATA_QUESTIONS",
            ["usccss.g1.u01.b03.answerable_data_question",
             "usccss.g1.u01.b03.count_category_data"],
            "IM2021_G1_U01_L13_QUESTIONS_ABOUT_DATA","READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U01:L14","supporting.statistics.charts",
            "SORT_DISPLAY_AND_CENTER_GAMES",
            ["usccss.t001.g1.construct_tool_sort",
             "usccss.t001.g1.construct_shape_graph",
             "usccss.t001.g1.mixed_add_subtract"],
            "IM2021_G1_U01_L14_CENTER_DAY_THREE","READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U01:L15","supporting.statistics.charts",
            "OPTIONAL_JUNGLE_SURVEY_AND_INTERPRETATION",
            ["usccss.t001.g1.collect_survey_graph",
             "usccss.g1.u01.b03.answerable_data_question"],
            "IM2021_G1_U01_L15_OPTIONAL_JUNGLE_SURVEY","READY_VERIFIED",Version)
    ];
}
