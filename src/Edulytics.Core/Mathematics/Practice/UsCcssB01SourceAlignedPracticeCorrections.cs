using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Sixty lesson-code-exact B01 corrections independently checked against their
/// original IM K-5 2021 teacher Lesson Purpose. Never select families from a
/// broad keyword match or alter other programmes / other B01 lesson mappings.
/// The 500-lesson L1–L6 collective gate remains authoritative.
/// </summary>
internal static class UsCcssB01SourceAlignedPracticeCorrections
{
    public static IReadOnlyList<LessonPracticeContract> Reviewed =
    [
        // IM G1 U03 L12 no exchanging ten; L22 exchanging ten is essential.
        new(
            "PED:US-CCSS-MATH:G1:U03:L12",
            "number.whole.add_subtract", "TEEN_PLUS_MINUS_ONES_NO_TEN_EXCHANGE",
            ["usccss.b01.g1.teen_no_regroup"],
            "IM2021_G1_U03_L12_NO_COMPOSE_DECOMPOSE_TEN",
            "READY_VERIFIED", "usccss-b01-teen-story-regroup-v1"),
        new(
            "PED:US-CCSS-MATH:G1:U03:L22",
            "number.whole.add_subtract", "TEEN_SUBTRACTION_DECOMPOSE_TEN",
            ["usccss.b01.g1.teen_subtract_decompose_ten"],
            "IM2021_G1_U03_L22_SUBTRACT_TEEN_DECOMPOSE_TEN",
            "READY_VERIFIED", "usccss-b01-teen-story-regroup-v1"),
        // IM G2 U02 L03 place-value stories without regrouping;
        // L11 stories within 50 with compulsory regrouping.
        new(
            "PED:US-CCSS-MATH:G2:U02:L03",
            "number.whole.add_subtract", "STORY_WITHIN100_NO_REGROUP",
            ["usccss.b01.g2.story_without_regroup100"],
            "IM2021_G2_U02_L03_STORY_NO_COMPOSE_DECOMPOSE",
            "READY_VERIFIED", "usccss-b01-teen-story-regroup-v1"),
        new(
            "PED:US-CCSS-MATH:G2:U02:L11",
            "number.whole.add_subtract", "STORY_WITHIN50_REQUIRED_REGROUP",
            ["usccss.b01.g2.story_regroup50"],
            "IM2021_G2_U02_L11_STORY_COMPOSE_DECOMPOSE_TEN",
            "READY_VERIFIED", "usccss-b01-teen-story-regroup-v1"),
        // Official IM G4 U01 L01: area as multiples of its side lengths.
        new(
            "PED:US-CCSS-MATH:G4:U01:L01",
            "supporting.number.factors_primes", "RECTANGLE_AREA_MULTIPLE_OF_SIDES",
            ["usccss.b01.g4.rectangle_area_is_multiple"],
            "IM2021_G4_U01_L01_AREA_TO_EXPLORE_MULTIPLES",
            "READY_VERIFIED", "usccss-b01-area-properties-v1"),
        // Official IM G3 U04 L10: visually partition arrays to prove
        // the distributive property of multiplication.
        new(
            "PED:US-CCSS-MATH:G3:U04:L10",
            "supporting.geometry.area_volume", "DISTRIBUTIVE_AREA_DIAGRAM",
            ["usccss.b01.g3.distributive_area_partition"],
            "IM2021_G3_U04_L10_MULTIPLICATION_AREA_DIAGRAM",
            "READY_VERIFIED", "usccss-b01-area-properties-v1"),
        // Official IM G3 U07 L10: distinguish area (square units)
        // from perimeter (linear units) in garden/playground problems.
        new(
            "PED:US-CCSS-MATH:G3:U07:L10",
            "supporting.geometry.area_volume", "PLAYGROUND_AREA_AND_PERIMETER",
            ["usccss.b01.g3.rectangle_area_perimeter_story"],
            "IM2021_G3_U07_L10_PLAYGROUND_AREA_PERIMETER",
            "READY_VERIFIED", "usccss-b01-area-properties-v1"),
        // IM teacher G3 U07 L11: same perimeter, unequal areas.
        // IM teacher G3 U07 L12: same area, unequal perimeters.
        new(
            "PED:US-CCSS-MATH:G3:U07:L11",
            "supporting.geometry.area_volume", "SAME_PERIMETER_DIFFERENT_AREA",
            ["usccss.b01.g3.same_perimeter_different_area"],
            "IM2021_G3_U07_L11_SAME_PERIMETER_DIFFERENT_AREA",
            "READY_VERIFIED", "usccss-b01-dual-rectangle-proof-v1"),
        new(
            "PED:US-CCSS-MATH:G3:U07:L12",
            "supporting.geometry.area_volume", "SAME_AREA_DIFFERENT_PERIMETER",
            ["usccss.b01.g3.same_area_different_perimeter"],
            "IM2021_G3_U07_L12_SAME_AREA_DIFFERENT_PERIMETER",
            "READY_VERIFIED", "usccss-b01-dual-rectangle-proof-v1"),
        // Original Grade 2 U01 L15-L16 ask for all three compare unknowns:
        // longer quantity, shorter quantity, or the difference.
        new(
            "PED:US-CCSS-MATH:G2:U01:L15",
            "number.whole.add_subtract", "TAPE_DIAGRAM_ALL_COMPARE_UNKNOWN_POSITIONS",
            ["usccss.b01.g2.compare_tape_unknown_positions"],
            "IM2021_G2_U01_L15_COMPARE_ALL_UNKNOWN_POSITIONS_TAPE",
            "READY_VERIFIED", "usccss-b01-tape-unknown-v1"),
        new(
            "PED:US-CCSS-MATH:G2:U01:L16",
            "number.whole.add_subtract", "COMPARE_ALL_UNKNOWN_POSITIONS",
            ["usccss.b01.g2.compare_tape_unknown_positions"],
            "IM2021_G2_U01_L16_COMPARE_UNKNOWN_ALL_POSITIONS",
            "READY_VERIFIED", "usccss-b01-tape-unknown-v1"),
        // IM 2021 G2 U01 L14: compare problems with a tape diagram,
        // where the unknown is the difference between aligned quantities.
        new(
            "PED:US-CCSS-MATH:G2:U01:L14",
            "number.whole.add_subtract", "TAPE_DIAGRAM_DIFFERENCE_UNKNOWN_WITHIN20",
            ["usccss.b01.g2.compare_tape_difference"],
            "IM2021_G2_U01_L14_BAR_GRAPH_TO_TAPE_COMPARE_SOURCE",
            "READY_VERIFIED", "usccss-b01-source-tape-diagram-v1"),
        // Teacher G3 U01 L03 interprets SCALED picture graph,
        // and L04 constructs a picture graph from frequencies and a key.
        new(
            "PED:US-CCSS-MATH:G3:U01:L03",
            "supporting.statistics.charts", "INTERPRET_SCALED_PICTURE_GRAPH",
            ["usccss.b01.g3.picture_graph.scaled_compare"],
            "IM2021_G3_U01_L03_INTERPRET_SCALED_PICTURE_GRAPH",
            "READY_VERIFIED", "usccss-b01-picture-graph-v1"),
        new(
            "PED:US-CCSS-MATH:G3:U01:L04",
            "supporting.statistics.charts", "CONSTRUCT_SCALED_PICTURE_GRAPH",
            ["usccss.b01.g3.picture_graph.construct"],
            "IM2021_G3_U01_L04_CONSTRUCT_SCALED_PICTURE_GRAPH",
            "READY_VERIFIED", "usccss-b01-picture-graph-v1"),
        // Teacher G2 U03 L15 constructs a line plot, while L14 interprets.
        new(
            "PED:US-CCSS-MATH:G2:U03:L15",
            "supporting.statistics.charts", "CREATE_NUMERIC_X_MARK_LINE_PLOT",
            ["usccss.b01.g2.measurement.read_line_plot"],
            "IM2021_G2_U03_L15_CREATE_NUMERIC_LINE_PLOT",
            "READY_VERIFIED", "usccss-b01-source-line-plot-v2"),
        // IM Grade 2 U03 L14 original: interpret measurements in a line plot.
        new(
            "PED:US-CCSS-MATH:G2:U03:L14",
            "supporting.statistics.charts", "INTERPRET_X_MARK_MEASUREMENT_LINE_PLOT",
            ["usccss.b01.g2.measurement.read_line_plot"],
            "IM2021_G2_U03_L14_INTERPRET_MEASUREMENT_LINE_PLOT",
            "READY_VERIFIED", "usccss-b01-source-line-plot-v1"),
        // Original IM G2 U03 L17 combines two-digit arithmetic,
        // measuring with a ruler, and making a numerical X-mark line plot.
        // Reuse independently verified source questions for all three goals.
        new(
            "PED:US-CCSS-MATH:G2:U03:L17",
            "supporting.measurement.practical",
            "ADD_SUBTRACT_MEASURE_AND_CONSTRUCT_LINE_PLOT",
            ["usccss.b01.g2.add_subtract_without_regroup",
             "usccss.b01.ruler.measure_without_zero",
             "usccss.b01.g2.measurement.read_line_plot"],
            "IM2021_G2_U03_L17_ADD_SUBTRACT_MEASURE_BUILD_LINE_PLOT",
            "READY_VERIFIED", "usccss-b01-mixed-measurement-plot-v1"),
        // Original teacher Grade 2 U03 L03 and L10 ruler purposes.
        new(
            "PED:US-CCSS-MATH:G2:U03:L03",
            "supporting.measurement.practical", "RULER_CM_COMPARE_LENGTHS",
            ["usccss.b01.ruler.compare_centimeters"],
            "IM2021_G2_U03_L03_CREATE_USE_RULER_CM_COMPARISON",
            "READY_VERIFIED", "usccss-b01-ruler-source-v1"),
        new(
            "PED:US-CCSS-MATH:G2:U03:L10",
            "supporting.measurement.practical", "MEASURE_FROM_NONZERO_CM_MARK",
            ["usccss.b01.ruler.measure_without_zero"],
            "IM2021_G2_U03_L10_NONZERO_ENDPOINT_RULER",
            "READY_VERIFIED", "usccss-b01-ruler-source-v1"),
        // IM 2021 G1 U07 L13: read and write only whole hours, not
        // unrelated elapsed-time computations.
        new(
            "PED:US-CCSS-MATH:G1:U07:L13",
            "supporting.measurement.practical", "READ_AND_WRITE_TIME_IN_HOURS",
            ["usccss.b01.clock.read_on_hour"],
            "IM2021_G1_U07_L13_TELL_AND_WRITE_HOURS_SOURCE",
            "READY_VERIFIED", "usccss-b01-clock-source-v2"),
        // IM 2021 G2 U06 L13: nearest five minutes with am/pm from
        // analog and digital representations.
        new(
            "PED:US-CCSS-MATH:G2:U06:L13",
            "supporting.measurement.practical", "FIVE_MINUTES_WITH_DAYPART",
            ["usccss.b01.clock.read_five_minutes_am_pm"],
            "IM2021_G2_U06_L13_FIVE_MINUTE_AM_PM_SOURCE",
            "READY_VERIFIED", "usccss-b01-clock-source-v2"),
        // Original IM teacher goals explicitly ask students to use a
        // number line for equal-size/equivalent fractions. The prior generic
        // missing-numerator contracts supplied no number-line representations.
        // This is a source-scoped L4/L5 repair, not an L1-L6 certification.
        new(
            "PED:US-CCSS-MATH:G3:U05:L12",
            "fractions.equivalent", "EQUIVALENT_FRACTIONS_ON_NUMBER_LINE",
            ["fractions.equivalent.number_line"],
            "IM2021_G3_U05_L12_DETERMINE_EQUIVALENCE_NUMBER_LINE",
            "READY_VERIFIED", "usccss-b01-fraction-number-line-v1"),
        new(
            "PED:US-CCSS-MATH:G4:U02:L04",
            "fractions.equivalent", "EQUAL_SIZE_FRACTIONS_ON_NUMBER_LINE",
            ["fractions.equivalent.number_line"],
            "IM2021_G4_U02_L04_VISUAL_SAME_SIZE_AND_LOCATION",
            "READY_VERIFIED", "usccss-b01-fraction-number-line-v1"),
        new(
            "PED:US-CCSS-MATH:G4:U02:L05",
            "fractions.equivalent", "EQUIVALENT_FRACTIONS_SAME_POINT",
            ["fractions.equivalent.number_line"],
            "IM2021_G4_U02_L05_SAME_POINT_EQUIVALENT_FRACTIONS",
            "READY_VERIFIED", "usccss-b01-fraction-number-line-v1"),
        new(
            "PED:US-CCSS-MATH:G4:U02:L08",
            "fractions.equivalent", "GENERATE_EQUIVALENTS_ON_NUMBER_LINE",
            ["fractions.equivalent.number_line"],
            "IM2021_G4_U02_L08_GENERATE_EQUIVALENTS_NUMBER_LINE",
            "READY_VERIFIED", "usccss-b01-fraction-number-line-v1"),
        // Original IM Grade 3 Unit 3 teacher preparations checked for
        // addition/subtraction within 1,000 and the specific algorithms.
        // Lesson exactness prevents generic 0–40 arithmetic from replacing
        // three-digit regrouping or subtraction across zero.
        new(
            "PED:US-CCSS-MATH:G3:U03:L03",
            "number.whole.add", "G3_PLACE_VALUE_ADDITION_STRATEGIES",
            ["usccss.b01.g3.add_within1000"],
            "IM2021_G3_U03_L03_ADD_WITHIN1000_PURPOSE_CHECKED",
            "READY_VERIFIED", "usccss-b01-g3-u03-algorithms-v1"),
        new(
            "PED:US-CCSS-MATH:G3:U03:L05",
            "number.whole.add", "G3_NEW_TEN_OR_HUNDRED_ALGORITHM",
            ["usccss.b01.g3.add_regroup1000"],
            "IM2021_G3_U03_L05_ADD_ALGORITHM_REGROUPING_PURPOSE_CHECKED",
            "READY_VERIFIED", "usccss-b01-g3-u03-algorithms-v1"),
        new(
            "PED:US-CCSS-MATH:G3:U03:L07",
            "number.whole.subtract", "G3_SUBTRACTION_STRATEGIES",
            ["usccss.b01.g3.subtract_within1000"],
            "IM2021_G3_U03_L07_SUBTRACT_WITHIN1000_PURPOSE_CHECKED",
            "READY_VERIFIED", "usccss-b01-g3-u03-algorithms-v1"),
        new(
            "PED:US-CCSS-MATH:G3:U03:L08",
            "number.whole.subtract", "G3_SUBTRACTION_EXPANDED_FORM",
            ["usccss.b01.g3.subtract_regroup1000"],
            "IM2021_G3_U03_L08_SUBTRACT_EXPANDED_FORM_PURPOSE_CHECKED",
            "READY_VERIFIED", "usccss-b01-g3-u03-algorithms-v1"),
        new(
            "PED:US-CCSS-MATH:G3:U03:L09",
            "number.whole.subtract", "G3_SUBTRACTION_DECOMPOSITION",
            ["usccss.b01.g3.subtract_regroup1000"],
            "IM2021_G3_U03_L09_SUBTRACT_REGROUP_PURPOSE_CHECKED",
            "READY_VERIFIED", "usccss-b01-g3-u03-algorithms-v1"),
        new(
            "PED:US-CCSS-MATH:G3:U03:L10",
            "number.whole.subtract", "G3_SUBTRACTION_DECOMPOSITION_STEPS",
            ["usccss.b01.g3.subtract_regroup1000"],
            "IM2021_G3_U03_L10_SUBTRACT_STEPS_PURPOSE_CHECKED",
            "READY_VERIFIED", "usccss-b01-g3-u03-algorithms-v1"),
        new(
            "PED:US-CCSS-MATH:G3:U03:L11",
            "number.whole.subtract", "G3_SUBTRACTION_ACROSS_ZEROS",
            ["usccss.b01.g3.subtract_across_zero"],
            "IM2021_G3_U03_L11_SUBTRACT_ACROSS_ZERO_PURPOSE_CHECKED",
            "READY_VERIFIED", "usccss-b01-g3-u03-algorithms-v1"),
        new(
            "PED:US-CCSS-MATH:G3:U03:L12",
            "number.whole.subtract", "G3_SUBTRACTION_STRATEGY_CHOICE",
            ["usccss.b01.g3.subtract_within1000"],
            "IM2021_G3_U03_L12_SUBTRACT_CHOICE_PURPOSE_CHECKED",
            "READY_VERIFIED", "usccss-b01-g3-u03-algorithms-v1"),
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
        // Original IM G2 U06 L06: compose and identify new shapes
        // from congruent small squares with actual square/rectangle grids.
        new(
            "PED:US-CCSS-MATH:G2:U06:L06",
            "supporting.geometry.shape_dimension", "COMPOSE_SQUARE_TILE_GEOMETRY",
            ["usccss.b01.g2.compose_equal_square_shapes"],
            "IM2021_G2_U06_L06_COMPOSE_EQUAL_SIZE_SHAPES",
            "READY_VERIFIED", "usccss-b01-g2-shape-tiles-v1"),
        // Grade 3 U08 L10: one accurate array is reused to prove
        // multiplication and inverse division within 100.
        new(
            "PED:US-CCSS-MATH:G3:U08:L10",
            "supporting.number.directed_operations", "MULTIPLICATION_DIVISION_ARRAY_INVERSES",
            ["usccss.b01.g3.multiplication_division_inverse_array"],
            "IM2021_G3_U08_L10_MULTIPLICATION_DIVISION_WITHIN_100",
            "READY_VERIFIED", "usccss-b01-array-inverse-v1"),
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
,
        new(
            "PED:US-CCSS-MATH:G1:U07:L05",
            "supporting.geometry.properties", "TRIANGLE_ATTRIBUTES",
            ["usccss.b01.triangle.attributes"],
            "IM2021_G1_U07_L05_TRIANGLE_ATTRIBUTES_PURPOSE_CHECKED",
            "READY_VERIFIED", "usccss-b01-source-task-v1"),
        new(
            "PED:US-CCSS-MATH:G1:U07:L09",
            "fractions.represent.interpret", "EQUAL_HALVES_FOURTHS",
            ["usccss.b01.partition.halves_fourths"],
            "IM2021_G1_U07_L09_EQUAL_HALVES_FOURTHS_PURPOSE_CHECKED",
            "READY_VERIFIED", "usccss-b01-source-task-v1"),
        new(
            "PED:US-CCSS-MATH:G1:U07:L10",
            "fractions.represent.interpret", "ONE_PIECE_WHOLE_PIECES",
            ["usccss.b01.partition.halves_fourths"],
            "IM2021_G1_U07_L10_ONE_PIECE_WHOLE_PIECES_PURPOSE_CHECKED",
            "READY_VERIFIED", "usccss-b01-source-task-v1"),
        // IM 2021 G2 U04 L03: represent an unlabeled whole-number
        // number-line point and use equally spaced interval reasoning.
        // Former generic "verify identity" did not ask about the line.
        // IM 2021 G2 U04 L04: positive direction, negative direction.
        new(
            "PED:US-CCSS-MATH:G2:U04:L04",
            "measurement.scale.read_equal_intervals", "NUMBER_LINE_LEFT_RIGHT_DIRECTION",
            ["usccss.b01.number_line.move_left_right_within100"],
            "IM2021_G2_U04_L04_DIRECTIONAL_WHOLE_NUMBER_LINE_PURPOSE",
            "READY_VERIFIED", "usccss-b01-g2-u04-number-line-v2"),
        // IM 2021 G2 U04 L05: estimate positions by proximity of ticks.
        new(
            "PED:US-CCSS-MATH:G2:U04:L05",
            "measurement.scale.read_equal_intervals", "NUMBER_LINE_ESTIMATE_RELATIVE_POSITION",
            ["usccss.b01.number_line.estimate_location_within100"],
            "IM2021_G2_U04_L05_ESTIMATE_WHOLE_NUMBER_POSITION_PURPOSE",
            "READY_VERIFIED", "usccss-b01-g2-u04-number-line-v2"),
        new(
            "PED:US-CCSS-MATH:G2:U04:L03",
            "measurement.scale.read_equal_intervals", "LOCATE_WHOLE_NUMBER_WITH_SKIP_COUNTING",
            ["usccss.b01.number_line.locate_whole_to100"],
            "IM2021_G2_U04_L03_NUMBER_LINE_SKIP_COUNTING_PURPOSE",
            "READY_VERIFIED", "usccss-b01-g2-u04-number-line-v2"),
        // IM 2021 G2 U04 L06: number-line representation AND arithmetic
        // within 100 are both taught; retain both verified operation families.
        new(
            "PED:US-CCSS-MATH:G2:U04:L06",
            "number.whole.add_subtract", "NUMBER_LINE_AND_ADD_SUBTRACT_WITHIN_100",
            ["usccss.b01.number_line.locate_whole_to100",
             "number.whole.add_subtract.within_100.apply",
             "number.whole.add_subtract.within_100.build"],
            "IM2021_G2_U04_L06_NUMBER_LINE_AND_ADD_SUBTRACT_PURPOSE",
            "READY_VERIFIED", "usccss-b01-g2-u04-number-line-v2"),
        new(
            "PED:US-CCSS-MATH:G2:U04:L02",
            "measurement.scale.read_equal_intervals", "NUMBER_LINE_EQUAL_INTERVALS",
            ["usccss.b01.number_line.equal_intervals"],
            "IM2021_G2_U04_L02_NUMBER_LINE_EQUAL_INTERVALS_PURPOSE_CHECKED",
            "READY_VERIFIED", "usccss-b01-source-task-v1"),
        new(
            "PED:US-CCSS-MATH:G2:U06:L11",
            "supporting.measurement.practical", "CLOCK_HALVES_QUARTERS",
            ["usccss.b01.clock.half_quarters"],
            "IM2021_G2_U06_L11_CLOCK_HALVES_QUARTERS_PURPOSE_CHECKED",
            "READY_VERIFIED", "usccss-b01-source-task-v1"),
        new(
            "PED:US-CCSS-MATH:G2:U08:L12",
            "supporting.geometry.area_volume", "PARTITION_UNIT_SQUARES",
            ["usccss.b01.rectangle.unit_squares"],
            "IM2021_G2_U08_L12_PARTITION_UNIT_SQUARES_PURPOSE_CHECKED",
            "READY_VERIFIED", "usccss-b01-source-task-v1"),
        new(
            "PED:US-CCSS-MATH:G2:U09:L03",
            "supporting.measurement.practical", "MAP_MEASURE_LENGTHS",
            ["usccss.b01.measure.map_lengths"],
            "IM2021_G2_U09_L03_MAP_MEASURE_LENGTHS_PURPOSE_CHECKED",
            "READY_VERIFIED", "usccss-b01-source-task-v1"),
        new(
            "PED:US-CCSS-MATH:G3:U01:L06",
            "supporting.statistics.charts", "BAR_GRAPH_CHOOSE_SCALE",
            ["usccss.b01.graph.choose_scale"],
            "IM2021_G3_U01_L06_BAR_GRAPH_CHOOSE_SCALE_PURPOSE_CHECKED",
            "READY_VERIFIED", "usccss-b01-source-task-v1"),
        new(
            "PED:US-CCSS-MATH:G3:U01:L16",
            "number.whole.multiply", "ARRANGE_ARRAYS",
            ["usccss.b01.array.equal_groups"],
            "IM2021_G3_U01_L16_ARRANGE_ARRAYS_PURPOSE_CHECKED",
            "READY_VERIFIED", "usccss-b01-source-task-v1"),
        new(
            "PED:US-CCSS-MATH:G3:U01:L18",
            "number.whole.multiply", "ARRAY_MULTIPLICATION_EXPRESSIONS",
            ["usccss.b01.array.equal_groups"],
            "IM2021_G3_U01_L18_ARRAY_MULTIPLICATION_EXPRESSIONS_PURPOSE_CHECKED",
            "READY_VERIFIED", "usccss-b01-source-task-v1"),
        new(
            "PED:US-CCSS-MATH:G3:U01:L21",
            "number.whole.multiply", "EQUAL_GROUPS_SEATING_PLAN",
            ["usccss.b01.array.equal_groups"],
            "IM2021_G3_U01_L21_EQUAL_GROUPS_SEATING_PLAN_PURPOSE_CHECKED",
            "READY_VERIFIED", "usccss-b01-source-task-v1"),
        new(
            "PED:US-CCSS-MATH:G3:U02:L03",
            "supporting.geometry.area_volume", "RECTANGULAR_TILING_AREA",
            ["usccss.b01.rectangle.unit_squares"],
            "IM2021_G3_U02_L03_RECTANGULAR_TILING_AREA_PURPOSE_CHECKED",
            "READY_VERIFIED", "usccss-b01-source-task-v1"),
        new(
            "PED:US-CCSS-MATH:G3:U02:L11",
            "supporting.geometry.area_volume", "AREA_MULTIPLICATION_TABLE",
            ["usccss.b01.rectangle.unit_squares"],
            "IM2021_G3_U02_L11_AREA_MULTIPLICATION_TABLE_PURPOSE_CHECKED",
            "READY_VERIFIED", "usccss-b01-source-task-v1"),
        new(
            "PED:US-CCSS-MATH:G3:U02:L15",
            "supporting.geometry.area_volume", "COMPOSITE_RECTANGLE_AREAS",
            ["usccss.b01.geometry.composite_rectangle_area"],
            "IM2021_G3_U02_L15_COMPOSITE_RECTANGLE_AREAS_PURPOSE_CHECKED",
            "READY_VERIFIED", "usccss-b01-source-task-v1"),
        new(
            "PED:US-CCSS-MATH:G3:U04:L06",
            "supporting.number.directed_operations", "DIVISION_UNKNOWN_FACTOR",
            ["usccss.b01.division.unknown_factor"],
            "IM2021_G3_U04_L06_DIVISION_UNKNOWN_FACTOR_PURPOSE_CHECKED",
            "READY_VERIFIED", "usccss-b01-source-task-v1"),
        new(
            "PED:US-CCSS-MATH:G3:U05:L01",
            "fractions.represent.interpret", "NAME_EQUAL_FRACTION_PARTS",
            ["usccss.b01.fraction.name_parts"],
            "IM2021_G3_U05_L01_NAME_EQUAL_FRACTION_PARTS_PURPOSE_CHECKED",
            "READY_VERIFIED", "usccss-b01-source-task-v1"),
        new(
            "PED:US-CCSS-MATH:G3:U06:L02",
            "supporting.measurement.practical", "RULER_FOURTHS_OF_INCH",
            ["usccss.b01.measure.quarter_inch"],
            "IM2021_G3_U06_L02_RULER_FOURTHS_OF_INCH_PURPOSE_CHECKED",
            "READY_VERIFIED", "usccss-b01-source-task-v1"),
        // Original IM G4 U01 L02: discover DIFFERENT factor pairs
        // by composing multiple rectangles with the SAME area.
        new(
            "PED:US-CCSS-MATH:G4:U01:L02",
            "supporting.number.factors_primes",
            "DIFFERENT_RECTANGLES_EQUAL_AREA_FACTOR_PAIRS",
            ["usccss.b01.g4.factor_pairs_two_rectangles"],
            "IM2021_G4_U01_L02_BUILD_RECTANGLES_TO_FIND_FACTOR_PAIRS",
            "READY_VERIFIED", "usccss-b01-g4-factor-rectangles-v1"),
        new(
            "PED:US-CCSS-MATH:G4:U01:L03",
            "supporting.number.factors_primes", "PRIME_VS_COMPOSITE",
            ["usccss.b01.factors.prime_composite"],
            "IM2021_G4_U01_L03_PRIME_VS_COMPOSITE_PURPOSE_CHECKED",
            "READY_VERIFIED", "usccss-b01-source-task-v1"),
        new(
            "PED:US-CCSS-MATH:G4:U01:L06",
            "supporting.number.factors_primes", "FACTOR_TOGGLED_LOCKERS",
            ["usccss.b01.factors.locker_square"],
            "IM2021_G4_U01_L06_FACTOR_TOGGLED_LOCKERS_PURPOSE_CHECKED",
            "READY_VERIFIED", "usccss-b01-source-task-v1"),
        new(
            "PED:US-CCSS-MATH:G4:U01:L07",
            "supporting.number.factors_primes", "FACTORS_AND_MULTIPLES",
            ["usccss.b01.factors.factors_multiples"],
            "IM2021_G4_U01_L07_FACTORS_AND_MULTIPLES_PURPOSE_CHECKED",
            "READY_VERIFIED", "usccss-b01-source-task-v1"),
        new(
            "PED:US-CCSS-MATH:G4:U01:L08",
            "supporting.number.factors_primes", "MONDRIAN_FACTOR_RECTANGLES",
            ["usccss.b01.factors.factor_pairs"],
            "IM2021_G4_U01_L08_MONDRIAN_FACTOR_RECTANGLES_PURPOSE_CHECKED",
            "READY_VERIFIED", "usccss-b01-source-task-v1"),
        new(
            "PED:US-CCSS-MATH:G4:U03:L04",
            "supporting.fractions.multiply_divide", "NON_UNIT_FRACTION_WHOLE_MULTIPLY",
            ["usccss.b01.fraction.whole_multiply"],
            "IM2021_G4_U03_L04_NON_UNIT_FRACTION_WHOLE_MULTIPLY_PURPOSE_CHECKED",
            "READY_VERIFIED", "usccss-b01-source-task-v1"),
        new(
            "PED:US-CCSS-MATH:G4:U03:L07",
            "fractions.add_subtract", "FRACTION_AS_SUM_SAME_DENOMINATOR",
            ["usccss.b01.fraction.decompose_same_denominator"],
            "IM2021_G4_U03_L07_FRACTION_AS_SUM_SAME_DENOMINATOR_PURPOSE_CHECKED",
            "READY_VERIFIED", "usccss-b01-source-task-v1"),
        new(
            "PED:US-CCSS-MATH:G4:U04:L10",
            "supporting.number.place_value_rounding", "TEN_TIMES_DIGIT_VALUE",
            ["usccss.b01.place_value.ten_times"],
            "IM2021_G4_U04_L10_TEN_TIMES_DIGIT_VALUE_PURPOSE_CHECKED",
            "READY_VERIFIED", "usccss-b01-source-task-v1"),
        new(
            "PED:US-CCSS-MATH:G2:U02:L16",
            "number.whole.add_subtract", "STORE_INVENTORY_100_ITEM_CONSTRAINTS",
            ["usccss.b01.market.inventory_100"],
            "IM2021_G2_U02_L16_OPTIONAL_STORE_100_ITEMS_PURPOSE_AND_ACTIVITY_CHECKED",
            "READY_VERIFIED", "usccss-b01-source-task-v1"),
        // Original 2021 IM G1 U07 L14-L15: recognize and write hours / half-hours.
        new(
            "PED:US-CCSS-MATH:G1:U07:L14",
            "supporting.measurement.practical", "ANALOG_HALF_HOUR_READING",
            ["usccss.b01.clock.read_half_hour"],
            "IM2021_G1_U07_L14_ANALOG_HALF_HOUR_PURPOSE",
            "READY_VERIFIED", "usccss-b01-source-clock-v1"),
        new(
            "PED:US-CCSS-MATH:G1:U07:L15",
            "supporting.measurement.practical", "WRITE_DIGITAL_HOURS_AND_HALF_HOURS",
            ["usccss.b01.clock.read_half_hour"],
            "IM2021_G1_U07_L15_WRITE_TIME_PURPOSE",
            "READY_VERIFIED", "usccss-b01-source-clock-v1"),
        // Original 2021 IM G2 U06 L12: each numbered clock tick is 5 minutes.
        new(
            "PED:US-CCSS-MATH:G2:U06:L12",
            "supporting.measurement.practical", "COUNT_BY_FIVE_ANALOG_CLOCK",
            ["usccss.b01.clock.read_five_minutes"],
            "IM2021_G2_U06_L12_FIVE_MINUTE_TIME_PURPOSE",
            "READY_VERIFIED", "usccss-b01-source-clock-v1"),
        new(
            "PED:US-CCSS-MATH:G2:U08:L07",
            "supporting.geometry.properties", "INTRO_ROWS",
            ["usccss.b01.array.equal_groups"],
            "IM2021_G2_U08_L07_INTRO_ROWS_PURPOSE",
            "READY_VERIFIED", "usccss-b01-source-array-v1"),
        new(
            "PED:US-CCSS-MATH:G2:U08:L08",
            "supporting.geometry.properties", "INTRO_COLUMNS",
            ["usccss.b01.array.columns"],
            "IM2021_G2_U08_L08_INTRO_COLUMNS_PURPOSE",
            "READY_VERIFIED", "usccss-b01-source-array-v1"),
        new(
            "PED:US-CCSS-MATH:G2:U08:L09",
            "number.whole.add_subtract", "ROW_SUM_OF_EQUAL_ADDENDS",
            ["usccss.b01.array.equal_addends"],
            "IM2021_G2_U08_L09_ROW_SUM_OF_EQUAL_ADDENDS_PURPOSE",
            "READY_VERIFIED", "usccss-b01-source-array-v1"),
        new(
            "PED:US-CCSS-MATH:G2:U08:L10",
            "number.whole.add_subtract", "ARRAY_EQUATIONS",
            ["usccss.b01.array.equal_equation"],
            "IM2021_G2_U08_L10_ARRAY_EQUATIONS_PURPOSE",
            "READY_VERIFIED", "usccss-b01-source-array-v1"),
        new(
            "PED:US-CCSS-MATH:G2:U08:L11",
            "supporting.geometry.area_volume", "RECTANGLES_WITH_TILES",
            ["usccss.b01.rectangle.unit_squares"],
            "IM2021_G2_U08_L11_RECTANGLES_WITH_TILES_PURPOSE",
            "READY_VERIFIED", "usccss-b01-source-array-v1"),
        new(
            "PED:US-CCSS-MATH:G3:U01:L17",
            "number.whole.multiply", "DRAW_ARRAYS",
            ["usccss.b01.array.equal_groups"],
            "IM2021_G3_U01_L17_DRAW_ARRAYS_PURPOSE",
            "READY_VERIFIED", "usccss-b01-source-array-v1"),
        new(
            "PED:US-CCSS-MATH:G3:U01:L19",
            "number.whole.multiply", "ARRAY_UNKNOWN_SYMBOL",
            ["usccss.b01.array.unknown_factor"],
            "IM2021_G3_U01_L19_ARRAY_UNKNOWN_SYMBOL_PURPOSE",
            "READY_VERIFIED", "usccss-b01-source-array-v1"),
        new(
            "PED:US-CCSS-MATH:G3:U01:L20",
            "number.whole.multiply", "COMMUTATIVE_ARRAY_ROTATION",
            ["usccss.b01.array.commutativity"],
            "IM2021_G3_U01_L20_COMMUTATIVE_ARRAY_ROTATION_PURPOSE",
            "READY_VERIFIED", "usccss-b01-source-array-v1"),
        new(
            "PED:US-CCSS-MATH:G3:U02:L02",
            "supporting.geometry.area_volume", "MEASURE_AREA_IN_UNIT_SQUARES",
            ["usccss.b01.rectangle.unit_squares"],
            "IM2021_G3_U02_L02_MEASURE_AREA_IN_UNIT_SQUARES_PURPOSE",
            "READY_VERIFIED", "usccss-b01-source-array-v1"),
        // IM Grade 3 Unit 5 Lesson 2: partition equal-size shape parts
        // and write their unit fraction (not unrelated identity proofs).
        new(
            "PED:US-CCSS-MATH:G3:U05:L02",
            "fractions.represent.interpret", "UNIT_FRACTION_EQUAL_PARTS",
            ["usccss.b01.fraction.unit_parts"],
            "IM2021_G3_U05_L02_EQUAL_PARTS_UNIT_FRACTION_PURPOSE",
            "READY_VERIFIED", "usccss-b01-source-fraction-v1"),
        // IM G2 U02 L01 does not regroup, L07 must decompose one ten.
        new(
            "PED:US-CCSS-MATH:G2:U02:L01",
            "number.whole.add_subtract", "TWO_DIGIT_ADD_SUBTRACT_NO_REGROUP",
            ["usccss.b01.g2.add_subtract_without_regroup"],
            "IM2021_G2_U02_L01_NO_REGROUP_SOURCE",
            "READY_VERIFIED", "usccss-b01-g2-two-digit-v1"),
        new(
            "PED:US-CCSS-MATH:G2:U02:L07",
            "number.whole.add_subtract", "SUBTRACT_TWO_DIGITS_DECOMPOSE_TEN",
            ["usccss.b01.g2.subtract_decompose_ten"],
            "IM2021_G2_U02_L07_DECOMPOSE_TEN_SOURCE",
            "READY_VERIFIED", "usccss-b01-g2-two-digit-v1"),
        // IM G1 U05 L04 only adds within 100 without composing
        // a ten; generic across-ten addition is mathematically off-purpose.
        new(
            "PED:US-CCSS-MATH:G1:U05:L04",
            "number.whole.add_subtract", "ADD_WITHIN_100_NO_COMPOSING_TEN",
            ["usccss.b01.g1.add_within100_no_compose_ten"],
            "IM2021_G1_U05_L04_ADD_WITHIN100_WITHOUT_COMPOSING_TEN",
            "READY_VERIFIED", "usccss-b01-g1-no-carry-v1"),
        // IM G2 U07 L04: no carrying or borrowing in hundreds,
        // tens, or ones; generic arithmetic within 100 is insufficient.
        new(
            "PED:US-CCSS-MATH:G2:U07:L04",
            "number.whole.add_subtract", "THREE_DIGIT_NO_COMPOSE_DECOMPOSE",
            ["usccss.b01.g2.add_subtract_1000_no_regroup"],
            "IM2021_G2_U07_L04_NO_COMPOSE_OR_DECOMPOSE_UP_TO1000",
            "READY_VERIFIED", "usccss-b01-1000-noregroup-v1"),
        // IM G3 U06 L04 interprets line plots to the nearest 1/4 inch;
        // L05 constructs that SAME evenly spaced fractional plot.
        new(
            "PED:US-CCSS-MATH:G3:U06:L04",
            "supporting.statistics.charts", "INTERPRET_FRACTIONAL_INCH_LINE_PLOT",
            ["usccss.b01.g3.quarter_inch_line_plot_frequency"],
            "IM2021_G3_U06_L04_INTERPRET_HALVES_QUARTERS_LINE_PLOT",
            "READY_VERIFIED", "usccss-b01-g3-fraction-line-plot-v1"),
        new(
            "PED:US-CCSS-MATH:G3:U06:L05",
            "supporting.statistics.charts", "CONSTRUCT_FRACTIONAL_INCH_LINE_PLOT",
            ["usccss.b01.g3.quarter_inch_line_plot_frequency"],
            "IM2021_G3_U06_L05_MAKE_HALVES_QUARTERS_LINE_PLOT",
            "READY_VERIFIED", "usccss-b01-g3-fraction-line-plot-v1"),
        // The official Grade 4 lesson explicitly includes 1/8-inch data
        // and fractional subtraction, not only the Grade 3 quarter-inch plot.
        // Both families must be verified against the source's line-plot goal.
        new(
            "PED:US-CCSS-MATH:G4:U03:L13",
            "supporting.statistics.charts", "INTERPRET_EIGHTH_INCH_LINE_PLOT_AND_SUBTRACT",
            ["usccss.b01.g3.quarter_inch_line_plot_frequency",
             "usccss.b01.g4.eighth_inch_line_plot_difference"],
            "IM2021_G4_U03_L13_EIGHTHS_EQUIVALENCE_AND_FRACTION_DIFFERENCE",
            "READY_VERIFIED", "usccss-b01-g4-eighth-line-plot-v3"),
        // IM G2 U05 L09: three-digit magnitude by relative
        // number-line positions rather than unrelated within-100 sums.
        new(
            "PED:US-CCSS-MATH:G2:U05:L09",
            "measurement.scale.read_equal_intervals",
            "THREE_DIGIT_COMPARE_BY_NUMBER_LINE",
            ["usccss.b01.g2.order_three_digits_number_line"],
            "IM2021_G2_U05_L09_THREE_DIGIT_NUMBER_LINE_PURPOSE",
            "READY_VERIFIED", "usccss-b01-book-g2-line-v1"),
        // IM 2021 G4 U04 L03: position and compare decimals on
        // an accurately spaced number line in hundredths.
        new(
            "PED:US-CCSS-MATH:G4:U04:L03",
            "supporting.decimals.place_value", "COMPARE_HUNDREDTHS_ON_NUMBER_LINE",
            ["usccss.b01.g4.decimal_number_line_greater"],
            "IM2021_G4_U04_L03_COMPARE_DECIMALS_ON_NUMBER_LINE",
            "READY_VERIFIED", "usccss-b01-decimal-line-v1"),
        // G4 U03 L10: subtract a proper fraction from whole numbers,
        // renaming the whole as equivalent denominator-sized parts.
        new(
            "PED:US-CCSS-MATH:G4:U03:L10",
            "fractions.add_subtract", "WHOLE_MINUS_PROPER_FRACTION",
            ["usccss.b01.g4.whole_minus_proper_fraction"],
            "IM2021_G4_U03_L10_WHOLE_MINUS_PROPER_FRACTION",
            "READY_VERIFIED", "usccss-b01-whole-fraction-v1"),
        // IM 2021 G1 U04 L17: compare and order tens and ones,
        // including a tens-digit tie where ones decide the order.
        new(
            "PED:US-CCSS-MATH:G1:U04:L17",
            "supporting.number.compare_order", "ORDER_TENS_THEN_ONES",
            ["usccss.b01.g1.compare_order_tens_ones"],
            "IM2021_G1_U04_L17_COMPARE_ORDER_TENS_ONES",
            "READY_VERIFIED", "usccss-b01-g1-order-tens-ones-v1"),
        // Source IM G1 U04 L03/L04: operate on complete tens only,
        // never unrelated single-digit or general across-ten questions.
        new(
            "PED:US-CCSS-MATH:G1:U04:L03",
            "number.whole.add_subtract", "ADD_SUBTRACT_TEN_FROM_TENS",
            ["usccss.b01.g1.add_subtract_10_tens"],
            "IM2021_G1_U04_L03_ADD_SUBTRACT_TEN_FROM_MULTIPLES",
            "READY_VERIFIED", "usccss-b01-g1-complete-tens-v1"),
        new(
            "PED:US-CCSS-MATH:G1:U04:L04",
            "number.whole.add_subtract", "ADD_SUBTRACT_MULTIPLES_OF_TEN",
            ["usccss.b01.g1.add_subtract_tens_multiples"],
            "IM2021_G1_U04_L04_ADD_SUBTRACT_MULTIPLES_OF_TEN",
            "READY_VERIFIED", "usccss-b01-g1-complete-tens-v1"),
        new(
            "PED:US-CCSS-MATH:G1:U04:L08",
            "supporting.number.place_value_rounding", "INTERPRET_TWO_DIGIT_BASE_TEN",
            ["usccss.b01.place_value.tens_ones"],
            "IM2021_G1_U04_L08_INTERPRET_TWO_DIGIT_BASE_TEN_PURPOSE",
            "READY_VERIFIED", "usccss-b01-source-place-value-v1"),
        new(
            "PED:US-CCSS-MATH:G1:U04:L09",
            "supporting.number.place_value_rounding", "REPRESENT_TWO_DIGIT_NUMBER",
            ["usccss.b01.place_value.tens_ones"],
            "IM2021_G1_U04_L09_REPRESENT_TWO_DIGIT_NUMBER_PURPOSE",
            "READY_VERIFIED", "usccss-b01-source-place-value-v1"),
        new(
            "PED:US-CCSS-MATH:G1:U04:L10",
            "supporting.number.place_value_rounding", "WRITE_TWO_DIGIT_BASE_TEN",
            ["usccss.b01.place_value.tens_ones"],
            "IM2021_G1_U04_L10_WRITE_TWO_DIGIT_BASE_TEN_PURPOSE",
            "READY_VERIFIED", "usccss-b01-source-place-value-v1"),
        new(
            "PED:US-CCSS-MATH:G1:U08:L07",
            "supporting.number.place_value_rounding", "COUNT_COLLECTION_UP_TO_120",
            ["usccss.b01.place_value.count_to120"],
            "IM2021_G1_U08_L07_COUNT_COLLECTION_UP_TO_120_PURPOSE",
            "READY_VERIFIED", "usccss-b01-source-place-value-v1"),
        new(
            "PED:US-CCSS-MATH:G1:U08:L08",
            "supporting.number.place_value_rounding", "TWO_DIGIT_MULTIPLE_REPRESENTATIONS",
            ["usccss.b01.place_value.tens_ones"],
            "IM2021_G1_U08_L08_TWO_DIGIT_MULTIPLE_REPRESENTATIONS_PURPOSE",
            "READY_VERIFIED", "usccss-b01-source-place-value-v1"),
        new(
            "PED:US-CCSS-MATH:G2:U05:L04",
            "supporting.number.place_value_rounding", "WRITE_THREE_DIGIT_BASE_TEN",
            ["usccss.b01.place_value.three_digits"],
            "IM2021_G2_U05_L04_WRITE_THREE_DIGIT_BASE_TEN_PURPOSE",
            "READY_VERIFIED", "usccss-b01-source-place-value-v1"),
        new(
            "PED:US-CCSS-MATH:G3:U01:L07",
            "supporting.statistics.charts", "SCALED_BAR_GRAPH_COMPARE",
            ["usccss.b01.graph.scaled_compare"],
            "IM2021_G3_U01_L07_SCALED_HOW_MANY_MORE_OR_FEWER_PURPOSE",
            "READY_VERIFIED", "usccss-b01-source-scaled-graph-v1"),
        // Original Grade 3 teacher source: L05 links area to multiplication;
        // L09 measures side lengths and multiplies to calculate area.
        new(
            "PED:US-CCSS-MATH:G3:U02:L05",
            "supporting.geometry.area_volume", "CONNECT_MULTIPLICATION_TO_RECTANGLE_ARRAY",
            ["usccss.b01.geometry.rectangle_area_grid60"],
            "IM2021_G3_U02_L05_MULTIPLICATION_RECTANGLE_AREA",
            "READY_VERIFIED", "usccss-b01-area-source-v2"),
        // Teacher G3 U02 L07: measure area in square feet/meters;
        // L10 applies rectangular area to real-world problems.
        new(
            "PED:US-CCSS-MATH:G3:U02:L07",
            "supporting.geometry.area_volume", "RECTANGLE_AREA_FEET_AND_METERS",
            ["usccss.b01.geometry.rectangle_area_real_world_units"],
            "IM2021_G3_U02_L07_SQUARE_FEET_METERS_RECTANGLE_AREA",
            "READY_VERIFIED", "usccss-b01-measured-area-v1"),
        new(
            "PED:US-CCSS-MATH:G3:U02:L10",
            "supporting.geometry.area_volume", "REAL_WORLD_AREA_MEASURED_SIDE_LENGTHS",
            ["usccss.b01.geometry.rectangle_area_real_world_units"],
            "IM2021_G3_U02_L10_REAL_WORLD_RECTANGLE_AREA",
            "READY_VERIFIED", "usccss-b01-measured-area-v1"),
        new(
            "PED:US-CCSS-MATH:G3:U02:L09",
            "supporting.geometry.area_volume", "MEASURE_RECTANGLE_SIDES_AND_MULTIPLY",
            ["usccss.b01.geometry.rectangle_area_ungridded"],
            "IM2021_G3_U02_L09_MEASURE_SIDES_MULTIPLY_AREA",
            "READY_VERIFIED", "usccss-b01-area-source-v2"),
        new(
            "PED:US-CCSS-MATH:G3:U02:L04",
            "supporting.geometry.area_volume", "COUNT_TILES_AREA_UP_TO60",
            ["usccss.b01.geometry.rectangle_area_grid60"],
            "IM2021_G3_U02_L04_COUNT_TILES_AREA_UP_TO60_PURPOSE",
            "READY_VERIFIED", "usccss-b01-source-area-v1"),
        new(
            "PED:US-CCSS-MATH:G3:U02:L08",
            "supporting.geometry.area_volume", "AREA_WITHOUT_GRID",
            ["usccss.b01.geometry.rectangle_area_ungridded"],
            "IM2021_G3_U02_L08_AREA_WITHOUT_GRID_PURPOSE",
            "READY_VERIFIED", "usccss-b01-source-area-v1"),
        new(
            "PED:US-CCSS-MATH:G3:U02:L12",
            "supporting.geometry.area_volume", "DECOMPOSE_AREA_ADDITION",
            ["usccss.b01.geometry.composite_rectangle_area"],
            "IM2021_G3_U02_L12_DECOMPOSE_AREA_ADDITION_PURPOSE",
            "READY_VERIFIED", "usccss-b01-source-area-v1"),
        new(
            "PED:US-CCSS-MATH:G3:U02:L13",
            "supporting.geometry.area_volume", "FIND_AREA_OF_COMPOSITE_RECTANGLES",
            ["usccss.b01.geometry.composite_rectangle_area"],
            "IM2021_G3_U02_L13_FIND_AREA_OF_COMPOSITE_RECTANGLES_PURPOSE",
            "READY_VERIFIED", "usccss-b01-source-area-v1"),
        // Official IM 2021 G1 U01 L02 Activity 1/2 verified from the teacher
        // lesson: match paired dot pictures to expressions; complete missing
        // dot pictures; Check It Off with two cards from 0 through 5.
        new(
            "PED:US-CCSS-MATH:G1:U01:L02",
            "number.whole.add", "MATCH_DOT_IMAGES_EXPRESSIONS_AND_CHECK_OFF",
            ["usccss.b01.g1.match_dot_cubes_expression",
             "usccss.b01.g1.complete_missing_dots",
             "usccss.b01.g1.check_off_add_within10"],
            "IM2021_G1_U01_L02_OFFICIAL_ACTIVITY_1_2_DOT_MATCHING",
            "READY_VERIFIED", "usccss-b01-source-dot-matching-v1"),
        // Original teacher preparation: G2 U06 L01 identifies 3–6 sided
        // closed polygons; G3 U07 L02 sorts triangles and quadrilaterals
        // by more specific defining attributes; G3 U02 L14 finds
        // composite area when one side length is deliberately absent.
        new(
            "PED:US-CCSS-MATH:G2:U06:L01",
            "supporting.geometry.properties", "CLASSIFY_POLYGONS_BY_SIDES",
            ["usccss.b01.shape.polygon_sides"],
            "IM2021_G2_U06_L01_POLYGON_SIDES_CORNERS_PURPOSE",
            "READY_VERIFIED", "usccss-b01-publisher-geometry-v1"),
        new(
            "PED:US-CCSS-MATH:G3:U07:L02",
            "supporting.geometry.properties", "SORT_TRIANGLE_QUADRILATERAL_SUBCATEGORIES",
            ["usccss.b01.shape.specific_subcategory"],
            "IM2021_G3_U07_L02_POLYGON_SUBCATEGORIES_PURPOSE",
            "READY_VERIFIED", "usccss-b01-publisher-geometry-v1"),
        new(
            "PED:US-CCSS-MATH:G3:U02:L14",
            "supporting.geometry.area_volume", "MISSING_SIDE_COMPOSITE_AREA",
            ["usccss.b01.area.missing_composite_side"],
            "IM2021_G3_U02_L14_UNGRIDDED_MISSING_SIDE_AREA_PURPOSE",
            "READY_VERIFIED", "usccss-b01-publisher-geometry-v1"),
        new(
            "PED:US-CCSS-MATH:G2:U04:L01",
            "measurement.scale.read_equal_intervals", "LOCATE_WHOLE_NUMBERS",
            ["usccss.b01.number_line.locate_whole_to100"],
            "IM2021_G2_U04_L01_WHOLE_NUMBERS_NUMBER_LINE_PURPOSE",
            "READY_VERIFIED", "usccss-b01-publisher-number-line-v1"),
        new(
            "PED:US-CCSS-MATH:G2:U05:L08",
            "measurement.scale.read_equal_intervals", "LOCATE_THREE_DIGITS",
            ["usccss.b01.number_line.locate_three_digits"],
            "IM2021_G2_U05_L08_THREE_DIGIT_NUMBER_LINE_PURPOSE",
            "READY_VERIFIED", "usccss-b01-publisher-number-line-v1"),
        new(
            "PED:US-CCSS-MATH:G4:U04:L11",
            "measurement.scale.read_equal_intervals", "LOCATE_LARGE_NUMBERS",
            ["usccss.b01.number_line.locate_large_numbers"],
            "IM2021_G4_U04_L11_LARGE_NUMBER_LINE_PURPOSE",
            "READY_VERIFIED", "usccss-b01-publisher-number-line-v1"),
        // IM 2021 Grade 4 Unit 3 Lesson 16: express tenths in
        // hundredths to add fractions, with a sum no greater than one.
        new(
            "PED:US-CCSS-MATH:G4:U03:L16",
            "fractions.add_subtract", "EQUIVALENT_TENTHS_HUNDREDTHS_UP_TO_ONE",
            ["usccss.b01.fraction.tenths_hundredths_within_one"],
            "IM2021_G4_U03_L16_4NF_C5_TENTHS_HUNDREDTHS_UP_TO_ONE",
            "READY_VERIFIED", "usccss-b01-source-fraction-tenths-v1"),
        // IM 2021 Grade 4 Unit 3 Lesson 17 extends identical equivalence
        // to sums strictly greater than one, including mixed whole/fraction.
        new(
            "PED:US-CCSS-MATH:G4:U03:L17",
            "fractions.add_subtract", "EQUIVALENT_TENTHS_HUNDREDTHS_OVER_ONE",
            ["usccss.b01.fraction.tenths_hundredths_over_one"],
            "IM2021_G4_U03_L17_4NF_C5_TENTHS_HUNDREDTHS_OVER_ONE",
            "READY_VERIFIED", "usccss-b01-source-fraction-tenths-v1")
    ];
}
