using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Four lesson-code-exact B01 corrections independently checked against their
/// original IM K-5 2021 teacher Lesson Purpose. Never select families from a
/// broad keyword match or alter other programmes / other B01 lesson mappings.
/// The 500-lesson L1–L6 collective gate remains authoritative.
/// </summary>
internal static class UsCcssB01SourceAlignedPracticeCorrections
{
    public static IReadOnlyList<LessonPracticeContract> Reviewed =
    [
        new(
            "PED:US-CCSS-MATH:G1:U01:L12",
            "supporting.statistics.charts", "COUNT_CATEGORIES_AND_TOTAL",
            ["supporting.statistics.category_total"],
            "IM2021_G1_U01_L12_CATEGORICAL_COUNTS_SOURCE_ALIGNED",
            "READY_VERIFIED", "usccss-b01-source-scope-v1"),
        // IM 2021 Grade 2 Unit 1 Lesson 11: question and answer about
        // categorical data in picture and bar graphs, NOT unit-rate ratios.
        new(
            "PED:US-CCSS-MATH:G2:U01:L11",
            "supporting.statistics.charts", "INTERPRET_CATEGORICAL_DATA",
            ["supporting.statistics.category_total",
             "supporting.statistics.compare_category_counts"],
            "IM2021_G2_U01_L11_CATEGORICAL_DATA_SOURCE_ALIGNED",
            "READY_VERIFIED", "usccss-b01-source-scope-v1"),
        // IM 2021 Grade 2 Unit 5 Lessons 1 & 5: compose hundreds from
        // tens/ones and write 3-digit numbers in expanded/base-ten form.
        // Neither lesson is about mean, median or range.
        new(
            "PED:US-CCSS-MATH:G2:U05:L01",
            "supporting.number.place_value_rounding", "BASE_TEN_COMPOSITION",
            ["supporting.number.place_value"],
            "IM2021_G2_U05_L01_COMPOSE_HUNDRED_SOURCE_ALIGNED",
            "READY_VERIFIED", "usccss-b01-source-scope-v1"),
        new(
            "PED:US-CCSS-MATH:G2:U05:L05",
            "supporting.number.place_value_rounding", "EXPANDED_FORM",
            ["supporting.number.place_value"],
            "IM2021_G2_U05_L05_EXPANDED_FORM_SOURCE_ALIGNED",
            "READY_VERIFIED", "usccss-b01-source-scope-v1")
    ];
}
