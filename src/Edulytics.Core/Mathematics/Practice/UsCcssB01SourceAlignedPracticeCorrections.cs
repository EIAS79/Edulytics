using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Eleven lesson-code-exact B01 corrections independently checked against their
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
            "READY_VERIFIED", "usccss-b01-source-scope-v1"),
        // Grade 1 U06 L07: source goal is direct-unit iteration to
        // measure the same object using different length units, not unit rates.
        new(
            "PED:US-CCSS-MATH:G1:U06:L07",
            "supporting.measurement.practical", "LENGTH_UNIT_ITERATION",
            ["supporting.measurement.unit_iteration"],
            "IM2021_G1_U06_L07_MEASURE_WITH_DIFFERENT_UNITS",
            "READY_VERIFIED", "usccss-b01-source-scope-v1"),
        // Grade 1 U07 L14: analog-clock half past (minute hand at six),
        // not circle area/circumference calculations.
        new(
            "PED:US-CCSS-MATH:G1:U07:L14",
            "supporting.measurement.practical", "HALF_HOUR_ANALOG_CLOCK",
            ["supporting.measurement.clock_half_hour"],
            "IM2021_G1_U07_L14_HALF_HOUR_CLOCK",
            "READY_VERIFIED", "usccss-b01-source-scope-v1"),
        // Grade 3 U08 L10: division within 100 linked to multiplication,
        // not grade-six unit rates.
        new(
            "PED:US-CCSS-MATH:G3:U08:L10",
            "supporting.number.directed_operations", "MULTIPLICATION_DIVISION_RELATED",
            ["supporting.number.multiply", "supporting.number.divide"],
            "IM2021_G3_U08_L10_MULTIPLICATION_DIVISION_WITHIN_100",
            "READY_VERIFIED", "usccss-b01-source-scope-v1"),
        // Grade 3 U01 L15 solves multiplication situations, not algebraic
        // factorization of polynomials.
        new(
            "PED:US-CCSS-MATH:G3:U01:L15",
            "number.whole.multiply", "MULTIPLICATION_FACTS",
            ["number.whole.multiply.fact_recall.apply"],
            "IM2021_G3_U01_L15_MULTIPLICATION_PROBLEMS",
            "READY_VERIFIED", "usccss-b01-source-scope-v1"),
        // Grade 3 U04 L07 relates multiplication to division.
        new(
            "PED:US-CCSS-MATH:G3:U04:L07",
            "supporting.number.directed_operations",
            "RELATED_MULTIPLICATION_DIVISION",
            ["supporting.number.multiply","supporting.number.divide"],
            "IM2021_G3_U04_L07_MULTIPLICATION_DIVISION",
            "READY_VERIFIED", "usccss-b01-source-scope-v1"),
        // Grade 4 U01 L04: source explicitly practices multiplication <=100,
        // not GCF or LCM.
        new(
            "PED:US-CCSS-MATH:G4:U01:L04",
            "number.whole.multiply", "MULTIPLICATION_FACT_RECALL",
            ["number.whole.multiply.fact_recall.build",
             "number.whole.multiply.fact_recall.apply"],
            "IM2021_G4_U01_L04_MULTIPLICATION_PRACTICE",
            "READY_VERIFIED", "usccss-b01-source-scope-v1"),
        // Grade 4 U01 L05: single-digit multiples in real-world contexts;
        // skip counting targets this original goal, unlike GCF/LCM.
        new(
            "PED:US-CCSS-MATH:G4:U01:L05",
            "supporting.sequences.core", "SINGLE_DIGIT_MULTIPLES",
            ["supporting.sequences.skip_count"],
            "IM2021_G4_U01_L05_MORE_MULTIPLES",
            "READY_VERIFIED", "usccss-b01-source-scope-v1")
    ];
}
