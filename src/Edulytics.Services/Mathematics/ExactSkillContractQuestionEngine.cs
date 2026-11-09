using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Generation;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics.Generation;
using Edulytics.Services.Mathematics.Runtime;
using Edulytics.Services.Mathematics.Solving;
using Edulytics.Services.Mathematics.Verification;

namespace Edulytics.Services.Mathematics;

public enum ExactSkillQuestionDifficulty
{
    Standard = 1,
    Stretch = 2,
    Challenge = 3
}

public sealed record ExactSkillGeneratedQuestion(
    string Family,
    string Prompt,
    string Solution,
    AssessmentItemType ItemType,
    string CorrectAnswer,
    IReadOnlyDictionary<string, int> Parameters,
    string ExposureFingerprint)
{
    public string VariantId { get; init; } = QuestionVariantPolicy.IdForSlot(0);
    public ExactSkillQuestionDifficulty Difficulty { get; init; } = ExactSkillQuestionDifficulty.Standard;
}

public sealed class ExactSkillQuestionPoolExhaustedException()
    : InvalidOperationException(
        "Exact Mathematics generation exhausted uniqueness retries.");

/// <summary>
/// Shared deterministic Mathematics Intelligence question kernel used by
/// learner Practice and teacher Assessment Builder. The caller owns curriculum
/// alignment; this engine owns exact parameter generation, solving, independent
/// verification and reconstructable fingerprints for approved question families.
/// </summary>
public sealed class ExactSkillContractQuestionEngine
{
    private const int MaxRetriesPerItem = 128;

    private static readonly IReadOnlySet<string> SupportedFamilies =
        new HashSet<string>(StringComparer.Ordinal)
        {
            "uae.g1.u01.l01.read_write_1_5",
            "uae.g1.u01.l02.zero_cardinality",
            "uae.g1.u01.l03.read_write_6_8",
            "uae.g1.u01.l04.read_write_9_10",
            "uae.g1.u01.l05.read_write_11_15",
            "uae.g1.u01.l06.read_write_16_20",
            "uae.g1.u02.l01.addition_stories",
            "uae.g1.u02.l02.model_addition",
            "uae.g1.u02.l03.addition_number_sentences",
            "uae.g1.u02.l04.add_zero",
            "uae.g1.u02.l05.vertical_addition",
            "uae.g1.u02.l06.write_story_equation",
            "uae.g1.u02.l07.make_four_five",
            "uae.g1.u02.l08.make_six_seven",
            "uae.g1.u02.l09.make_eight",
            "uae.g1.u02.l10.make_nine",
            "uae.g1.u02.l11.make_ten_frame",
            "uae.g1.u02.l13.true_false_addition",
            "uae.g1.u03.l01.subtraction_stories",
            "uae.g1.u03.l02.model_subtraction",
            "uae.g1.u03.l03.subtraction_number_sentences",
            "uae.g1.u03.l04.subtract_zero_or_all",
            "uae.g1.u03.l05.vertical_subtraction",
            "uae.g1.u03.l07.compare_groups",
            "uae.g1.u03.l08.subtract_4_5",
            "uae.g1.u03.l09.subtract_6_7",
            "uae.g1.u03.l10.subtract_8",
            "uae.g1.u03.l11.subtract_9",
            "uae.g1.u03.l12.subtract_10",
            "uae.g1.u03.l13.related_add_subtract",
            "uae.g1.u03.l14.true_false_subtraction",
            "uae.g1.u04.l01.count_on_1_2_3",
            "uae.g1.u04.l02.count_on_fils",
            "uae.g1.u04.l03.add_on_number_line",
            "uae.g1.u04.l04.doubles_add",
            "uae.g1.u04.l05.near_doubles_add",
            "uae.g1.u04.l06.act_out_problem_solving",
            "uae.g1.u04.l07.make_ten_add",
            "uae.g1.u04.l08.commutative_add",
            "uae.g1.u04.l09.add_three_with_strategy",
            "uae.g1.u05.l01.count_back_1_2_3",
            "uae.g1.u05.l02.number_line_subtract",
            "uae.g1.u05.l03.doubles_subtraction",
            "uae.g1.u05.l04.story_subtraction_equation",
            "uae.g1.u05.l05.subtract_through_ten",
            "uae.g1.u05.l06.related_add_sub_facts",
            "uae.g1.u05.l07.fact_family_four_equations",
            "uae.g1.u05.l08.missing_addend_part_whole",
            "uae.g1.u06.l01.ordinal_1_10",
            "uae.g1.u06.l02.numbers_11_19",
            "uae.g1.u06.l03.groups_of_ten",
            "uae.g1.u06.l04.count_tens_fils",
            "uae.g1.u06.l05.ten_and_some_more",
            "uae.g1.u06.l06.tens_and_ones",
            "uae.g1.u06.l07.make_table",
            "uae.g1.u06.l08.numbers_to_100",
            "uae.g1.u06.l09.ten_more_ten_less",
            "uae.g1.u06.l10.count_fives_fils",
            "uae.g1.u06.l11.compare_models",
            "uae.g1.u06.l12.compare_symbols",
            "uae.g1.u06.l13.hundreds_tens_ones",
            "uae.g1.u06.l14.count_to_120",
            "uae.g1.u06.l15.read_write_to_120",
            "uae.g1.u07.l01.add_tens",
            "uae.g1.u07.l02.count_on_tens_ones",
            "uae.g1.u07.l03.add_tens_ones",
            "uae.g1.u07.l04.guess_check_revise",
            "uae.g1.u07.l05.regroup_ones_for_tens",
            "uae.g1.u07.l06.subtract_tens",
            "uae.g1.u07.l07.count_back_tens",
            "uae.g1.u07.l08.inverse_tens",
            "uae.g1.u08.l01.tally_fives",
            "uae.g1.u08.l02.record_table",
            "uae.g1.u08.l03.make_picture_graph",
            "uae.g1.u08.l04.read_picture_graph",
            "uae.g1.u08.l05.make_bar_graph",
            "uae.g1.u08.l06.read_bar_graph",
            "uae.g1.u09.l01.compare_lengths",
            "uae.g1.u09.l02.order_lengths",
            "uae.g1.u09.l03.nonstandard_measure",
            "uae.g1.u09.l04.guess_check_length",
            "uae.g1.u09.l05.analog_hour",
            "uae.g1.u09.l06.digital_hour",
            "uae.g1.u09.l07.analog_half",
            "uae.g1.u09.l08.digital_half",
            "uae.g1.u09.l09.hour_or_half",
            "uae.g1.u10.l01.square_rectangle",
            "uae.g1.u10.l02.triangle_trapezoid",
            "uae.g1.u10.l03.circle_properties",
            "uae.g1.u10.l04.compare_shapes",
            "uae.g1.u10.l05.compose_2d",
            "uae.g1.u10.l06.more_composite_2d",
            "uae.g1.u10.l07.logical_shape_clue",
            "uae.g1.u10.l08.equal_shares",
            "uae.g1.u10.l09.halves_equal",
            "uae.g1.u10.l10.fourths_equal",
            "uae.g1.u11.l01.cubes_prisms",
            "uae.g1.u11.l02.cones_cylinders",
            "uae.g1.u11.l03.three_d_pattern",
            "uae.g1.u11.l04.compose_solids",
            "usccss.g1.u01.l01.count_collection_to20",
            "usccss.g1.u01.l01.add_dot_cubes_to10",
            "number.whole.add.direct",
            "number.whole.subtract.direct",
            "number.lcm.two_numbers",
            "percentages.of_quantity.direct",
            "percentages.core.of_quantity",
            "percentages.core.increase_decrease",
            "sequences.core.arithmetic_nth_term",
            "sequences.core.geometric_nth_term",
            "sequences.core.find_position_arithmetic",
            "usccss.g11.alg2.u01.l01.affine_recurrence",
            "usccss.g11.alg2.u01.l01.hanoi_moves",
            "usccss.g11.alg2.u01.l01.fibonacci_term",
            "usccss.g11.alg2.u01.l01.halving_fraction",
            "usccss.g11.alg2.u01.l02.paper_piece_count",
            "usccss.g11.alg2.u01.l02.paper_piece_area",
            "usccss.g11.alg2.u01.l02.rational_geometric_term",
            "usccss.g11.alg2.u01.l02.growth_factor",
            "geometry.polygons.interior_angle_sum",
            "geometry.polygons.missing_interior_angle",
            "geometry.polygons.regular_interior_angle",
            "geometry.perimeter_area.triangle_area",
            "geometry.perimeter_area.rectangle_missing_side",
            "usccss.g4.u06.l15.area_partial_quotients",
            "usccss.g4.u06.l15.mural_complete_columns",
            "usccss.g4.u06.l16.base_ten_equal_groups",
            "usccss.g4.u06.l18.partial_quotients_recording",
            "usccss.g4.u06.l19.partial_quotients_remainders",
            "usccss.g4.u06.l20.contextual_remainder_decisions",
            "usccss.g4.u06.l21.multistep_contexts",
            "usccss.g4.u06.l23.four_operation_contexts",
            "usccss.g4.u06.l24.language_population_comparisons",
            "usccss.g4.u09.l05.two_digit_partial_products",
            "usccss.g4.u08.l07.polygon_perimeter_sum",
            "usccss.g4.u08.l04.symmetry_axis_counts",
            "usccss.g4.u07.l13.additive_angle_part",
            "usccss.g4.u07.l12.classify_angles_by_degree_bounds",
            "usccss.g4.u07.l10.perpendicular_right_angle_judgment",
            "usccss.g4.u07.l09.protractor_dual_scale_reading",
            "usccss.g4.u07.l08.full_turn_degree_benchmarks",
            "usccss.g4.u07.l07.clock_hour_angle_turn_compare",
            "usccss.g4.u07.l06.compare_angle_openings",
            "usccss.g4.u07.l05.angle_common_vertex_rays",
            "usccss.g4.u07.l03.dot_grid_parallel_intersect",
            "usccss.g4.u07.l02.geometry_cards_number_talk",
            "usccss.g4.u07.l01.geometric_description_precision",
            "usccss.g4.u06.l25.flower_garland_patterns",
            "usccss.g4.u06.l22.convert_l_shaped_area_perimeter",
            "fractions.represent.interpret.fraction_bar",
            "algebra.relationships.two_unknowns.total_difference",
            "measurement.scale.equal_intervals.read_value",
            "fractions.compare.unlike.common_denominator",
            "fractions.compare.unlike.select_greater",
            "fractions.compare.unlike.true_false",
            "fractions.compare.unlike.order_three",
            "fractions.equivalent.missing_value",
            "fractions.equivalent.recognize",
            "fractions.equivalent.generate_multiple",
            "fractions.equivalent.number_line",
            "fractions.equivalent.reduce_common_factor",
            "ratio.unit_rate.direct",
            "ratio.unit_rate.equivalent_ratio",
            "ratio.unit_rate.divide_total",
            "statistics.center_spread.mean",
            "statistics.center_spread.median",
            "statistics.center_spread.range",
            "probability.theoretical.simple_event",
            "probability.theoretical.complement",
            "probability.theoretical.two_coins_exactly_one",
            "geometry.coordinate.evaluate_linear_rule",
            "geometry.coordinate.y_intercept_from_rule",
            "number.whole.add_subtract.within_10.build",
            "number.whole.add_subtract.within_10.apply",
            "number.whole.add_subtract.across_ten.build",
            "number.whole.add_subtract.across_ten.apply",
            "number.whole.add_subtract.within_100.build",
            "number.whole.add_subtract.within_100.apply",
            "number.whole.add_subtract.columnar.build",
            "number.whole.add_subtract.columnar.apply",
            "number.whole.add_subtract.comparative.build",
            "number.whole.add_subtract.comparative.apply",
            "number.whole.add_subtract.complement_100.build",
            "number.whole.add_subtract.complement_100.apply",
            "number.whole.multiply.fact_recall.build",
            "number.whole.multiply.fact_recall.apply",
            "number.whole.divide.with_remainder.build",
            "number.whole.divide.with_remainder.apply",
            "fractions.of_quantity.build",
            "fractions.of_quantity.apply",
            "fractions.add_subtract.within_one.build",
            "fractions.add_subtract.within_one.apply",
            "fractions.add_subtract.same_denominator.mixed.build",
            "fractions.add_subtract.same_denominator.mixed.apply",
            "fractions.add_subtract.related.build",
            "fractions.add_subtract.related.apply",
            "fractions.add_subtract.common_denominator.build",
            "fractions.add_subtract.common_denominator.apply",
            "fractions.compare.benchmark",
            "geometry.coordinate.gradient_between_points",
            "geometry.angles.parallel_lines",
            "geometry.angles.supplementary",
            "geometry.angles.algebraic_supplementary",
            "geometry.similarity.find_missing_length",
            "geometry.similarity.scale_factor",
            "geometry.congruence.identify_criterion",
            "geometry.surface_area.rectangular_prism",
            "geometry.surface_area_volume.rectangular_prism_surface_area",
            "geometry.volume.rectangular_prism",
            "geometry.surface_area_volume.rectangular_prism_volume",
            "geometry.rectangle.area.exact",
            "geometry.perimeter_area.rectangle_area",
            "geometry.rectangle.perimeter.exact",
            "geometry.perimeter_area.rectangle_perimeter",
            "geometry.right_triangle.pythagorean.exact",
            "geometry.right_triangle.pythagorean.find_leg_exact",
            "trigonometry.right_triangle.ratio_exact",
            "trigonometry.right_triangle.find_side_exact",
            "trigonometry.right_triangle.find_angle_exact",
            "trigonometry.modelling.contextual",
            "vectors.add.exact_rational",
            "vectors.subtract.exact_rational",
            "vectors.scalar_multiply.exact_rational",
            "vectors.dot.exact_rational",
            "vectors.magnitude.exact",
            "vectors.between_points.exact",
            ExactLinearEquationQuestionFactory.FamilyId,
            "algebra.linear.variables_both_sides",
            ExactLinearInequalityQuestionFactory.FamilyId,
            "algebra.linear.inequality.variables_both_sides"
        };

    public static bool SupportsFamily(string? family) =>
        !string.IsNullOrWhiteSpace(family) &&
        (SupportedFamilies.Contains(family.Trim()) ||
          UaeGrade2Unit1Lesson01Questions.Supports(family.Trim()) ||
          UaeGrade2Unit1Lessons02And13Questions.Supports(family.Trim()) ||
          UaeGrade2Unit2Questions.Supports(family.Trim()) ||
          UaeGrade2Unit3Questions.Supports(family.Trim()) ||
          UaeGrade2Unit4Questions.Supports(family.Trim()) ||
          UaeGrade2Unit5Questions.Supports(family.Trim()) ||
          UaeGrade3Unit1Questions.Supports(family.Trim()) ||
          UaeGrade3Unit1Lesson02Questions.Supports(family.Trim()) ||
          UaeGrade3Unit1Lesson03Questions.Supports(family.Trim()) ||
          UaeGrade3Unit1Lesson04Questions.Supports(family.Trim()) ||
          UaeGrade3Unit1Lesson05Questions.Supports(family.Trim()) ||
          UaeGrade3Unit2Lesson01Questions.Supports(family.Trim()) ||
          UaeGrade3Unit2Lesson02Questions.Supports(family.Trim()) ||
          UaeGrade3Unit2Lesson03Questions.Supports(family.Trim()) ||
          UaeGrade2Unit6Questions.Supports(family.Trim()) ||
          UaeGrade2Unit7Questions.Supports(family.Trim()) ||
          UaeGrade2Unit8Questions.Supports(family.Trim()) ||
          UaeGrade2Term3Questions.Supports(family.Trim()) ||
         SupportedFamilies.Contains(family.Trim()) ||
         Grade12PrecalculusAdvancedNumericQuestions.Supports(family.Trim()) ||
         SupportingPracticeCompletionEngine.Supports(family.Trim()) ||
         UsCcssT001ExactQuestions.Supports(family.Trim()) ||
         UsCcssT001ConstructShapeGraph.Supports(family.Trim()) ||
         UsCcssT001AuthoredActivities.Supports(family.Trim()) ||
         UsCcssT001RepresentationChoices.Supports(family.Trim()) ||
         UsCcssG1Batch03DataQuestions.Supports(family.Trim()) ||
         UsCcssG1Unit2StoryQuestions.Supports(family.Trim()) ||
         UsCcssG7Unit1Lesson1ScaledCopyQuestions.Supports(family.Trim()) ||
         UsCcssG7Unit1Lesson2CorrespondenceQuestions.Supports(family.Trim()) ||
         UsCcssG7Unit1Lesson3DrawingQuestions.Supports(family.Trim()) ||
         UsCcssG7Unit1Lesson4ScaledRelationshipsQuestions.Supports(family.Trim()) ||
         UsCcssG7Unit1Lesson5FactorQuestions.Supports(family.Trim()) ||
         UsCcssG7Unit1Lesson6AreaScalingQuestions.Supports(family.Trim()) ||
         UsCcssG7Unit1Lesson7DrawingScaleQuestions.Supports(family.Trim()) ||
         UsCcssG7Unit1Lesson8MapRateQuestions.Supports(family.Trim()) ||
         UsCcssG7Unit1Lesson9CreatingDrawingsQuestions.Supports(family.Trim()) ||
         UsCcssG7Unit1Lesson10ChangingScalesQuestions.Supports(family.Trim()) ||
         UsCcssG7Unit1Lesson11UnitlessScalesQuestions.Supports(family.Trim()) ||
         UsCcssG7Unit2Lesson1EquivalentRatioQuestions.Supports(family.Trim()) ||
         UsCcssG7Unit2Lesson3UnitRatesQuestions.Supports(family.Trim()) ||
         UsCcssG7Unit2Lesson4ProportionalEquationsQuestions.Supports(family.Trim()) ||
         UsCcssG7Unit2Lesson6ConcertRecyclingQuestions.Supports(family.Trim()) ||
         UsCcssG7Unit2Lesson7TableProportionalityQuestions.Supports(family.Trim()) ||
         UsCcssG7Unit2Lesson8ClassifyEquationsQuestions.Supports(family.Trim()) ||
         UsCcssG7Unit2Lesson9RateComparisonQuestions.Supports(family.Trim()) ||
         UsCcssG7Unit2Lesson10GraphProportionsQuestions.Supports(family.Trim()) ||
         UsCcssG6Unit1Lesson1TilingQuestions.Supports(family.Trim()) ||

         UsCcssG6Unit1Lesson1TilingQuestions.Supports(family.Trim()) ||
         UsCcssG6Unit1Lesson2TangramQuestions.Supports(family.Trim()) ||
         UsCcssG6Unit1Lesson3ShadedAreaQuestions.Supports(family.Trim()) ||
         UsCcssG6Unit1Lesson4ParallelogramQuestions.Supports(family.Trim()) ||
         UsCcssG6Unit1Lesson5BaseHeightQuestions.Supports(family.Trim()) ||
         UsCcssG6Unit1Lesson6AreaQuestions.Supports(family.Trim()) ||
         UsCcssG6Unit1Lesson7TriangleQuestions.Supports(family.Trim()) ||
         UsCcssG6Unit1Lesson8TriangleAreaQuestions.Supports(family.Trim()) ||
         UsCcssG6Unit1Lesson11PolygonQuestions.Supports(family.Trim()) ||
         UsCcssG6Unit1Lesson12SurfaceAreaQuestions.Supports(family.Trim()) ||
         UsCcssG6Unit2RatioQuestions.Supports(family.Trim()) ||

         UsCcssAlg1U01L01StatisticalSelectionQuestions.Supports(family.Trim()) ||
         UsCcssAlg1U01L02RepresentationQuestions.Supports(family.Trim()) ||
         UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.Supports(family.Trim()) ||
         UsCcssAlg1Unit2AlgebraQuestions.Supports(family.Trim()) ||
         UsCcssAlg1Unit2InequalityQuestions.Supports(family.Trim()) ||
         UsCcssAlg1Unit3StatisticsQuestions.Supports(family.Trim()) ||
         UsCcssAlg1Unit4FunctionQuestions.Supports(family.Trim()) ||
         UsCcssG5U03L01FractionOfHalfQuestions.Supports(family.Trim()) ||
         UsCcssG5U03L02UnitFractionQuestions.Supports(family.Trim()) ||
         UsCcssG5U03L03UnitFractionQuestions.Supports(family.Trim()) ||
         UsCcssG5U03L04ParkQuestions.Supports(family.Trim()) ||
         UsCcssG5U03L05ShadedAreaQuestions.Supports(family.Trim()) ||
         UsCcssG5U03L06NonunitAreaQuestions.Supports(family.Trim()) ||
         UsCcssG5U03L07FractionEquationQuestions.Supports(family.Trim()) ||
         UsCcssG5U03L08FlagMeasureQuestions.Supports(family.Trim()) ||
         UsCcssG5U03L09FlagDesignSupportQuestions.Supports(family.Trim()) ||
         UsCcssG5U03L10SharingQuestions.Supports(family.Trim()) ||
         UsCcssG5U03SourceDivisionQuestions.Supports(family.Trim()) ||
         UsCcssG5U04WholeNumberSourceProducts.Supports(family.Trim()) ||
         UsCcssG5U04WholeNumberSourceDivision.Supports(family.Trim()) ||
         UsCcssG5U05SourceThousandths.Supports(family.Trim()) ||
         UsCcssG7Unit1Lesson1ScaledCopyQuestions.Supports(family.Trim()) ||

         UsCcssG4Unit2ThirdLessonQuestions.Supports(family.Trim()) ||
         UsCcssG4Unit1SeventhLessonQuestions.Supports(family.Trim()) ||
         UsCcssG4Unit1SixthLessonQuestions.Supports(family.Trim()) ||
         UsCcssG4Unit1FifthLessonQuestions.Supports(family.Trim()) ||
         UsCcssG4Unit1ThirdLessonQuestions.Supports(family.Trim()) ||
         UsCcssG4Unit1SecondLessonQuestions.Supports(family.Trim()) ||
         UsCcssG4Unit1FirstLessonQuestions.Supports(family.Trim()) ||
         UsCcssG5Unit1Lesson2Questions.Supports(family.Trim()) ||
         UsCcssG5Unit1Lesson3Questions.Supports(family.Trim()) ||
         UsCcssG5Unit1Lesson4Questions.Supports(family.Trim()) ||
         UsCcssG5Unit1Lesson5Questions.Supports(family.Trim()) ||
         UsCcssG5Unit1Lesson6Questions.Supports(family.Trim()) ||
         UsCcssG5Unit1Lesson7Questions.Supports(family.Trim()) ||
         UsCcssG5Unit1Lesson8Questions.Supports(family.Trim()) ||
         UsCcssG5Unit1Lesson9Questions.Supports(family.Trim()) ||
         UsCcssG5Unit1Lesson10Questions.Supports(family.Trim()) ||
         UsCcssG5Unit1Lesson11Questions.Supports(family.Trim()) ||
         UsCcssG5Unit1Lesson12Questions.Supports(family.Trim()) ||
         UsCcssG5Unit2SourceQuestions.Supports(family.Trim()) ||
         UsCcssG5Unit1Lesson1ComparisonQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit1FirstLessonQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit1Lesson2And3Questions.Supports(family.Trim()) ||
         UsCcssG2Unit1Lesson4Questions.Supports(family.Trim()) ||
         UsCcssG2Unit1Lesson5Questions.Supports(family.Trim()) ||
         UsCcssG2Unit1Lesson6NumberTalkQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit1Lesson8PictureGraphQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit1Lesson9BarGraphQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit1Lesson10ConstructGraphsQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit1Lesson11GraphQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit1Lesson12SortAndDisplayQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit1Lesson13BarCompareQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit1Lesson14TapeDiagramQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit1Lesson15CompareQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit1Lesson16CompareQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit1Lesson17CenterQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit1Lesson18ClassSurveyQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit2Lesson1NoRegroupQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit2Lesson2UnknownAddendQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit2Lesson3ZooStoryQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit2Lesson4OptionalCenterQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit2Lesson5DecomposeTenQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit2Lesson6TargetNumbersQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit2Lesson7SubtractTwoDigitsQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit2Lesson8DifferentWaysQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit2Lesson9MixedAddSubtractQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit2Lesson10TargetStage5Questions.Supports(family.Trim()) ||
         UsCcssG2Unit2Lesson11SeedsStoryQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit2Lesson12MancalaQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit2Lesson13StoryEquationsQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit2Lesson14TwoStepSeedsQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit2Lesson15TapeStoriesQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit2Lesson16MarketInventoryQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit3Lesson1StandardUnitsQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit3Lesson2MeasureCmQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit3Lesson3BuildRulerQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit3Lesson4EstimateMeasureCmQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit3Lesson5MetersQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit3Lesson6ReptileStoriesQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit3Lesson7EstimateMeasureStage2Questions.Supports(family.Trim()) ||
         UsCcssG2Unit3Lesson8WhatIsAnInchQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit3Lesson9FishFeetQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit3Lesson10TornTapeQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit3Lesson11SareeRibbonQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit3Lesson12FriendshipQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit3Lesson13CenterDay2Questions.Supports(family.Trim()) ||
         UsCcssG2Unit3Lesson14HandSpanLinePlotQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit3Lesson15PencilLinePlotQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit3Lesson16PlantHeightsQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit3Lesson17CenterPlotQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit3Lesson18YardstickQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit4Lesson01NumberLineQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit4Lesson02NumberLineFeaturesQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit4Lesson03UnlabeledTicksQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit4NextThreeQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit4ArrowLessonsQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit4FinalSixQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit5FirstSixQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit5ComparisonQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit6ExactQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit7FirstFourQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit7NextFourQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit7LastTenQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit8ExactQuestions.Supports(family.Trim()) ||
         UsCcssG2Unit9ExactQuestions.Supports(family.Trim()) ||
         UsCcssB001SourceGoalQuestions.Supports(family.Trim()) ||
         UsCcssG1Unit2Part2Questions.Supports(family.Trim()) ||
         UsCcssG1Unit2NextFiveQuestions.Supports(family.Trim()) ||
         UsCcssG1Unit2CompareFiveQuestions.Supports(family.Trim()) ||
         UsCcssG1Unit2EquationStoryFiveQuestions.Supports(family.Trim()) ||
         UsCcssG1NextSourceFiveQuestions.Supports(family.Trim()) ||
         UsCcssG1SourceFiveNextQuestions.Supports(family.Trim()) ||
         UsCcssG1TeenFiveQuestions.Supports(family.Trim()) ||
         UsCcssG1FullGradeStartQuestions.Supports(family.Trim()) ||
         UsCcssG1FullGradeNextQuestions.Supports(family.Trim()) ||
         UsCcssG1FullGradeStrategyAndTensQuestions.Supports(family.Trim()) ||
         UsCcssG1FullGradeTensAndOnesQuestions.Supports(family.Trim()) ||
         UsCcssG1FullGradeTwoDigitOperationsQuestions.Supports(family.Trim()) ||
         UsCcssG1FullGradeUnit5AdditionQuestions.Supports(family.Trim()) ||
         UsCcssG1FullGradeUnit4FinalQuestions.Supports(family.Trim()) ||
         UsCcssG1FullGradeUnit8ReviewQuestions.Supports(family.Trim()) ||
         UsCcssG1FullGradeUnit6MeasurementQuestions.Supports(family.Trim()) ||
         UsCcssG1HowCloseStage3Questions.Supports(family.Trim()) ||
         UsCcssG1FullGradeUnit7GeometryTimeQuestions.Supports(family.Trim()));

    public IReadOnlyList<ExactSkillGeneratedQuestion> Generate(
        string fingerprintNamespace,
        string scopeKey,
        IReadOnlyList<string> allowedQuestionFamilies,
        ExactSkillQuestionDifficulty difficulty,
        int questionCount,
        int seed,
        IReadOnlyCollection<string> excludedExposureFingerprints,
        int? preferredVariant = null)
    {
        if (string.IsNullOrWhiteSpace(fingerprintNamespace) ||
            string.IsNullOrWhiteSpace(scopeKey) ||
            allowedQuestionFamilies.Count == 0 ||
            questionCount is < 1 or > 50)
        {
            throw new InvalidOperationException("Exact SkillContract generation requires a valid scope, family registry and question count.");
        }

        if (allowedQuestionFamilies.Any(family => !SupportsFamily(family)))
            throw new InvalidOperationException(
                "Exact SkillContract generation contains a family unsupported by the Practice exact engine.");

        MathematicsResourceGuard.ValidateGenerationRequest(
            fingerprintNamespace,
            scopeKey,
            allowedQuestionFamilies,
            questionCount,
            excludedExposureFingerprints);

        var random = new Random(seed == 0 ? 1 : seed);
        var excluded = excludedExposureFingerprints
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToHashSet(StringComparer.Ordinal);
        var generated = new HashSet<string>(StringComparer.Ordinal);
        var generatedPrompts = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var generatedSemanticKeys = new HashSet<string>(StringComparer.Ordinal);
        var items = new List<ExactSkillGeneratedQuestion>(questionCount);

        for (var index = 0; index < questionCount; index++)
        {
            ExactSkillGeneratedQuestion? item = null;

            for (var retry = 0; retry < MaxRetriesPerItem && item is null; retry++)
            {
                var familyOffset = preferredVariant.HasValue
                    ? QuestionVariantPolicy.NormalizeSlot(preferredVariant.Value)
                    : 0;
                var family = allowedQuestionFamilies[
                    (familyOffset + index + retry) % allowedQuestionFamilies.Count];
                var requestedVariant = preferredVariant.HasValue
                    ? preferredVariant.Value + index + retry
                    : index + retry;
                var problem = BuildProblem(
                    family,
                    random,
                    difficulty,
                    requestedVariant,
                    allowedQuestionFamilies.Count == 1 && questionCount >= 10,
                    preferredVariant.HasValue);
                var answer = Solve(problem);

                if (!Verify(problem.Family, problem.Parameters, answer))
                {
                    MathematicsObservability.Record(MathematicsMetricKind.VerificationFailure);
                    throw new InvalidOperationException($"Exact Mathematics verifier rejected solver output for {family}.");
                }

                MathematicsObservability.Record(MathematicsMetricKind.SolverSuccess);

                var fingerprint = Fingerprint(
                    fingerprintNamespace,
                    scopeKey,
                    family,
                    problem.Parameters,
                    problem.Prompt);

                var normalizedPrompt = NormalizePrompt(problem.Prompt);
                var hasSemanticVariant =
                    RequiresPromptUniqueness(problem.Family);
                var semanticKey =
                    PracticeSemanticQuestionIdentityPolicy
                        .EnforceInsideExactGenerator(problem.Family)
                        ? PracticeSemanticQuestionIdentityPolicy
                            .Create(
                                problem.Family,
                                problem.Parameters)
                            .Key
                        : null;

                if (excluded.Contains(fingerprint) ||
                    generated.Contains(fingerprint) ||
                    (hasSemanticVariant && generatedPrompts.Contains(normalizedPrompt)) ||
                    (semanticKey is not null &&
                     generatedSemanticKeys.Contains(semanticKey)))
                {
                    continue;
                }

                generated.Add(fingerprint);
                if (hasSemanticVariant)
                    generatedPrompts.Add(normalizedPrompt);
                if (semanticKey is not null)
                    generatedSemanticKeys.Add(semanticKey);
                var variant = QuestionVariantPolicy.Describe(
                    problem.Parameters,
                    requestedVariant);
                item = new ExactSkillGeneratedQuestion(
                    problem.Family,
                    problem.Prompt,
                    problem.Solution,
                    problem.ItemType,
                    answer,
                    problem.Parameters,
                    fingerprint)
                {
                    VariantId = variant.Id,
                    Difficulty = difficulty
                };
            }

            if (item is null)
                throw new ExactSkillQuestionPoolExhaustedException();

            items.Add(item);
        }

        MathematicsObservability.Record(MathematicsMetricKind.GenerationSuccess, items.Count);
        return items;
    }

    public static bool Verify(
        string family,
        IReadOnlyDictionary<string, int> parameters,
        string answer)
    {
        if (UaeGrade2Unit1Lesson01Questions.Supports(family))
            return UaeGrade2Unit1Lesson01Questions.Verify(family, parameters, answer);
        if (UaeGrade2Unit1Lessons02And13Questions.Supports(family))
            return UaeGrade2Unit1Lessons02And13Questions.Verify(family, parameters, answer);
        if (UaeGrade2Unit2Questions.Supports(family))
            return UaeGrade2Unit2Questions.Verify(family, parameters, answer);
        if (UaeGrade2Unit3Questions.Supports(family))
            return UaeGrade2Unit3Questions.Verify(family, parameters, answer);
        if (UaeGrade2Unit4Questions.Supports(family))
            return UaeGrade2Unit4Questions.Verify(family, parameters, answer);
        if (UaeGrade2Unit5Questions.Supports(family))
            return UaeGrade2Unit5Questions.Verify(family, parameters, answer);
        if (UaeGrade3Unit1Questions.Supports(family))
            return UaeGrade3Unit1Questions.Verify(family, parameters, answer);
        if (UaeGrade3Unit1Lesson02Questions.Supports(family))
            return UaeGrade3Unit1Lesson02Questions.Verify(family, parameters, answer);
        if (UaeGrade3Unit1Lesson03Questions.Supports(family))
            return UaeGrade3Unit1Lesson03Questions.Verify(family, parameters, answer);
        if (UaeGrade3Unit1Lesson04Questions.Supports(family))
            return UaeGrade3Unit1Lesson04Questions.Verify(family, parameters, answer);
        if (UaeGrade3Unit1Lesson05Questions.Supports(family))
            return UaeGrade3Unit1Lesson05Questions.Verify(family, parameters, answer);
        if (UaeGrade3Unit2Lesson01Questions.Supports(family))
            return UaeGrade3Unit2Lesson01Questions.Verify(family, parameters, answer);
        if (UaeGrade3Unit2Lesson02Questions.Supports(family))
            return UaeGrade3Unit2Lesson02Questions.Verify(family, parameters, answer);
        if (UaeGrade3Unit2Lesson03Questions.Supports(family))
            return UaeGrade3Unit2Lesson03Questions.Verify(family, parameters, answer);
        if (UaeGrade2Unit6Questions.Supports(family))
            return UaeGrade2Unit6Questions.Verify(family, parameters, answer);
        if (UaeGrade2Unit7Questions.Supports(family))
            return UaeGrade2Unit7Questions.Verify(family, parameters, answer);
        if (UaeGrade2Unit8Questions.Supports(family))
            return UaeGrade2Unit8Questions.Verify(family, parameters, answer);
        if (UaeGrade2Term3Questions.Supports(family))
            return UaeGrade2Term3Questions.Verify(family, parameters, answer);
        if (Grade12PrecalculusAdvancedNumericQuestions.Supports(family))
            return Grade12PrecalculusAdvancedNumericQuestions.Verify(
                family, parameters, answer);
        if (UsCcssG7Unit1Lesson1ScaledCopyQuestions.Supports(family))
            return UsCcssG7Unit1Lesson1ScaledCopyQuestions.Verify(family, parameters, answer);
        if (UsCcssG7Unit1Lesson2CorrespondenceQuestions.Supports(family))
            return UsCcssG7Unit1Lesson2CorrespondenceQuestions.Verify(family, parameters, answer);
        if (UsCcssG7Unit1Lesson3DrawingQuestions.Supports(family))
            return UsCcssG7Unit1Lesson3DrawingQuestions.Verify(family, parameters, answer);
        if (UsCcssG7Unit1Lesson4ScaledRelationshipsQuestions.Supports(family))
            return UsCcssG7Unit1Lesson4ScaledRelationshipsQuestions.Verify(family, parameters, answer);
        if (UsCcssG7Unit1Lesson5FactorQuestions.Supports(family))
            return UsCcssG7Unit1Lesson5FactorQuestions.Verify(family, parameters, answer);
        if (UsCcssG7Unit1Lesson6AreaScalingQuestions.Supports(family))
            return UsCcssG7Unit1Lesson6AreaScalingQuestions.Verify(family, parameters, answer);
        if (UsCcssG7Unit1Lesson7DrawingScaleQuestions.Supports(family))
            return UsCcssG7Unit1Lesson7DrawingScaleQuestions.Verify(family, parameters, answer);
        if (UsCcssG7Unit1Lesson8MapRateQuestions.Supports(family))
            return UsCcssG7Unit1Lesson8MapRateQuestions.Verify(family, parameters, answer);
        if (UsCcssG7Unit1Lesson9CreatingDrawingsQuestions.Supports(family))
            return UsCcssG7Unit1Lesson9CreatingDrawingsQuestions.Verify(family, parameters, answer);
        if (UsCcssG7Unit1Lesson10ChangingScalesQuestions.Supports(family))
            return UsCcssG7Unit1Lesson10ChangingScalesQuestions.Verify(family, parameters, answer);
        if (UsCcssG7Unit1Lesson11UnitlessScalesQuestions.Supports(family))
            return UsCcssG7Unit1Lesson11UnitlessScalesQuestions.Verify(family, parameters, answer);
        if (UsCcssG7Unit2Lesson1EquivalentRatioQuestions.Supports(family))
            return UsCcssG7Unit2Lesson1EquivalentRatioQuestions.Verify(family, parameters, answer);
        if (UsCcssG7Unit2Lesson3UnitRatesQuestions.Supports(family))
            return UsCcssG7Unit2Lesson3UnitRatesQuestions.Verify(family, parameters, answer);
        if (UsCcssG7Unit2Lesson4ProportionalEquationsQuestions.Supports(family))
            return UsCcssG7Unit2Lesson4ProportionalEquationsQuestions.Verify(family, parameters, answer);
        if (UsCcssG7Unit2Lesson6ConcertRecyclingQuestions.Supports(family))
            return UsCcssG7Unit2Lesson6ConcertRecyclingQuestions.Verify(family, parameters, answer);
        if (UsCcssG7Unit2Lesson7TableProportionalityQuestions.Supports(family))
            return UsCcssG7Unit2Lesson7TableProportionalityQuestions.Verify(family, parameters, answer);
        if (UsCcssG7Unit2Lesson8ClassifyEquationsQuestions.Supports(family))
            return UsCcssG7Unit2Lesson8ClassifyEquationsQuestions.Verify(family, parameters, answer);
        if (UsCcssG7Unit2Lesson9RateComparisonQuestions.Supports(family))
            return UsCcssG7Unit2Lesson9RateComparisonQuestions.Verify(family, parameters, answer);
        if (UsCcssG7Unit2Lesson10GraphProportionsQuestions.Supports(family))
            return UsCcssG7Unit2Lesson10GraphProportionsQuestions.Verify(family, parameters, answer);
        if (UsCcssG6Unit1Lesson1TilingQuestions.Supports(family))
            return UsCcssG6Unit1Lesson1TilingQuestions.Verify(family, parameters, answer);
        if (parameters.ContainsKey("g6ScaledRatio"))
            return VerifyGrade6ScaledRatio(family, parameters, answer);
        if (IsGrade6FiniteSourceVariantFamily(family) && parameters.ContainsKey("g6Composite"))
            return VerifyGrade6Composite(family, parameters, answer);

        if (UsCcssG6Unit1Lesson1TilingQuestions.Supports(family))
            return UsCcssG6Unit1Lesson1TilingQuestions.Verify(family, parameters, answer);
        if (UsCcssG6Unit1Lesson2TangramQuestions.Supports(family))
            return UsCcssG6Unit1Lesson2TangramQuestions.Verify(family, parameters, answer);
        if (UsCcssG6Unit1Lesson3ShadedAreaQuestions.Supports(family))
            return UsCcssG6Unit1Lesson3ShadedAreaQuestions.Verify(family, parameters, answer);
        if (UsCcssG6Unit1Lesson4ParallelogramQuestions.Supports(family))
            return UsCcssG6Unit1Lesson4ParallelogramQuestions.Verify(family, parameters, answer);
        if (UsCcssG6Unit1Lesson5BaseHeightQuestions.Supports(family))
            return UsCcssG6Unit1Lesson5BaseHeightQuestions.Verify(family, parameters, answer);
        if (UsCcssG6Unit1Lesson6AreaQuestions.Supports(family))
            return UsCcssG6Unit1Lesson6AreaQuestions.Verify(family, parameters, answer);
        if (UsCcssG6Unit1Lesson7TriangleQuestions.Supports(family))
            return UsCcssG6Unit1Lesson7TriangleQuestions.Verify(family, parameters, answer);
        if (UsCcssG6Unit1Lesson8TriangleAreaQuestions.Supports(family))
            return UsCcssG6Unit1Lesson8TriangleAreaQuestions.Verify(family, parameters, answer);
        if (UsCcssG6Unit1Lesson11PolygonQuestions.Supports(family))
            return UsCcssG6Unit1Lesson11PolygonQuestions.Verify(family, parameters, answer);
        if (UsCcssG6Unit2RatioQuestions.Supports(family))
            return UsCcssG6Unit2RatioQuestions.Verify(family, parameters, answer);
        if (UsCcssG6Unit1Lesson12SurfaceAreaQuestions.Supports(family))
            return UsCcssG6Unit1Lesson12SurfaceAreaQuestions.Verify(family, parameters, answer);

        if (UsCcssAlg1U01L01StatisticalSelectionQuestions.Supports(family))
            return UsCcssAlg1U01L01StatisticalSelectionQuestions.Verify(family, parameters, answer);
        if (UsCcssAlg1U01L02RepresentationQuestions.Supports(family))
            return UsCcssAlg1U01L02RepresentationQuestions.Verify(family, parameters, answer);
        if (UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.Supports(family))
            return UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.Verify(family, parameters, answer);
        if (UsCcssAlg1Unit2AlgebraQuestions.Supports(family))
            return UsCcssAlg1Unit2AlgebraQuestions.Verify(family, parameters, answer);
        if (UsCcssAlg1Unit2InequalityQuestions.Supports(family))
            return UsCcssAlg1Unit2InequalityQuestions.Verify(family, parameters, answer);
        if (UsCcssAlg1Unit3StatisticsQuestions.Supports(family))
            return UsCcssAlg1Unit3StatisticsQuestions.Verify(family, parameters, answer);
        if (UsCcssAlg1Unit4FunctionQuestions.Supports(family))
            return UsCcssAlg1Unit4FunctionQuestions.Verify(family, parameters, answer);
        if (UsCcssG5U03L01FractionOfHalfQuestions.Supports(family))
            return UsCcssG5U03L01FractionOfHalfQuestions.Verify(family, parameters, answer);
        if (UsCcssG5U03L02UnitFractionQuestions.Supports(family))
            return UsCcssG5U03L02UnitFractionQuestions.Verify(family, parameters, answer);
        if (UsCcssG5U03L03UnitFractionQuestions.Supports(family))
            return UsCcssG5U03L03UnitFractionQuestions.Verify(family, parameters, answer);
        if (UsCcssG5U03L04ParkQuestions.Supports(family))
            return UsCcssG5U03L04ParkQuestions.Verify(family, parameters, answer);
        if (UsCcssG5U03L05ShadedAreaQuestions.Supports(family))
            return UsCcssG5U03L05ShadedAreaQuestions.Verify(family, parameters, answer);
        if (UsCcssG5U03L06NonunitAreaQuestions.Supports(family))
            return UsCcssG5U03L06NonunitAreaQuestions.Verify(family, parameters, answer);
        if (UsCcssG5U03L07FractionEquationQuestions.Supports(family))
            return UsCcssG5U03L07FractionEquationQuestions.Verify(family, parameters, answer);
        if (UsCcssG5U03L08FlagMeasureQuestions.Supports(family))
            return UsCcssG5U03L08FlagMeasureQuestions.Verify(family, parameters, answer);
        if (UsCcssG5U03L09FlagDesignSupportQuestions.Supports(family))
            return UsCcssG5U03L09FlagDesignSupportQuestions.Verify(family, parameters, answer);
        if (UsCcssG5U03L10SharingQuestions.Supports(family))
            return UsCcssG5U03L10SharingQuestions.Verify(family, parameters, answer);
        if (UsCcssG5U03SourceDivisionQuestions.Supports(family))
            return UsCcssG5U03SourceDivisionQuestions.Verify(family, parameters, answer);
        if (UsCcssG5U04WholeNumberSourceProducts.Supports(family))
            return UsCcssG5U04WholeNumberSourceProducts.Verify(family, parameters, answer);
        if (UsCcssG5U04WholeNumberSourceDivision.Supports(family))
            return UsCcssG5U04WholeNumberSourceDivision.Verify(family, parameters, answer);
        if (UsCcssG5U05SourceThousandths.Supports(family))
            return UsCcssG5U05SourceThousandths.Verify(family, parameters, answer);
        if (UsCcssG7Unit1Lesson1ScaledCopyQuestions.Supports(family))
            return UsCcssG7Unit1Lesson1ScaledCopyQuestions.Verify(family, parameters, answer);

        if (UsCcssG1FullGradeUnit7GeometryTimeQuestions.Supports(family))
            return UsCcssG1FullGradeUnit7GeometryTimeQuestions.Verify(family,parameters,answer);
        if (UsCcssG1HowCloseStage3Questions.Supports(family))
            return UsCcssG1HowCloseStage3Questions.Verify(parameters,answer);
        if (UsCcssG1FullGradeUnit6MeasurementQuestions.Supports(family))
            return UsCcssG1FullGradeUnit6MeasurementQuestions.Verify(family,parameters,answer);
        if (UsCcssG1FullGradeUnit8ReviewQuestions.Supports(family))
            return UsCcssG1FullGradeUnit8ReviewQuestions.Verify(family,parameters,answer);
        if (UsCcssG1FullGradeUnit4FinalQuestions.Supports(family))
            return UsCcssG1FullGradeUnit4FinalQuestions.Verify(family,parameters,answer);
        if (UsCcssG1FullGradeUnit5AdditionQuestions.Supports(family))
            return UsCcssG1FullGradeUnit5AdditionQuestions.Verify(family,parameters,answer);
        if (UsCcssG1FullGradeTwoDigitOperationsQuestions.Supports(family))
            return UsCcssG1FullGradeTwoDigitOperationsQuestions.Verify(family,parameters,answer);
        if (UsCcssG1FullGradeTensAndOnesQuestions.Supports(family))
            return UsCcssG1FullGradeTensAndOnesQuestions.Verify(family,parameters,answer);
        if (UsCcssG1FullGradeStrategyAndTensQuestions.Supports(family))
            return UsCcssG1FullGradeStrategyAndTensQuestions.Verify(family,parameters,answer);
        if (UsCcssG1FullGradeNextQuestions.Supports(family))
            return UsCcssG1FullGradeNextQuestions.Verify(family,parameters,answer);
        if (UsCcssG1FullGradeStartQuestions.Supports(family))
            return UsCcssG1FullGradeStartQuestions.Verify(family,parameters,answer);
        if (UsCcssG1TeenFiveQuestions.Supports(family))
            return UsCcssG1TeenFiveQuestions.Verify(family,parameters,answer);
        if (UsCcssG1SourceFiveNextQuestions.Supports(family))
            return UsCcssG1SourceFiveNextQuestions.Verify(family,parameters,answer);
        if (UsCcssG1NextSourceFiveQuestions.Supports(family))
            return UsCcssG1NextSourceFiveQuestions.Verify(family,parameters,answer);
        if (UsCcssG1Unit2EquationStoryFiveQuestions.Supports(family))
            return UsCcssG1Unit2EquationStoryFiveQuestions.Verify(family,parameters,answer);
        if (UsCcssG1Unit2CompareFiveQuestions.Supports(family))
            return UsCcssG1Unit2CompareFiveQuestions.Verify(family,parameters,answer);
        if (UsCcssG1Unit2NextFiveQuestions.Supports(family))
            return UsCcssG1Unit2NextFiveQuestions.Verify(family,parameters,answer);
        if (UsCcssG1Unit2Part2Questions.Supports(family))
            return UsCcssG1Unit2Part2Questions.Verify(family, parameters, answer);
        if (UsCcssG4Unit2ThirdLessonQuestions.Supports(family))
            return UsCcssG4Unit2ThirdLessonQuestions.Verify(family,parameters,answer);
        if (UsCcssG4Unit1SeventhLessonQuestions.Supports(family))
            return UsCcssG4Unit1SeventhLessonQuestions.Verify(family,parameters,answer);
        if (UsCcssG4Unit1SixthLessonQuestions.Supports(family))
            return UsCcssG4Unit1SixthLessonQuestions.Verify(family,parameters,answer);
        if (UsCcssG4Unit1FifthLessonQuestions.Supports(family))
            return UsCcssG4Unit1FifthLessonQuestions.Verify(family,parameters,answer);
        if (UsCcssG4Unit1ThirdLessonQuestions.Supports(family))
            return UsCcssG4Unit1ThirdLessonQuestions.Verify(family,parameters,answer);
        if (UsCcssG4Unit1SecondLessonQuestions.Supports(family))
            return UsCcssG4Unit1SecondLessonQuestions.Verify(family,parameters,answer);
        if (UsCcssG4Unit1FirstLessonQuestions.Supports(family))
            return UsCcssG4Unit1FirstLessonQuestions.Verify(family, parameters, answer);
        if (UsCcssG5Unit1Lesson2Questions.Supports(family))
            return UsCcssG5Unit1Lesson2Questions.Verify(family, parameters, answer);
        if (UsCcssG5Unit1Lesson3Questions.Supports(family))
            return UsCcssG5Unit1Lesson3Questions.Verify(family, parameters, answer);
        if (UsCcssG5Unit1Lesson4Questions.Supports(family))
            return UsCcssG5Unit1Lesson4Questions.Verify(family,parameters,answer);
        if (UsCcssG5Unit1Lesson5Questions.Supports(family))
            return UsCcssG5Unit1Lesson5Questions.Verify(family,parameters,answer);
        if (UsCcssG5Unit1Lesson6Questions.Supports(family))
            return UsCcssG5Unit1Lesson6Questions.Verify(family,parameters,answer);
        if (UsCcssG5Unit1Lesson7Questions.Supports(family))
            return UsCcssG5Unit1Lesson7Questions.Verify(family,parameters,answer);
        if(UsCcssG5Unit1Lesson8Questions.Supports(family))return UsCcssG5Unit1Lesson8Questions.Verify(family,parameters,answer);
        if(UsCcssG5Unit1Lesson9Questions.Supports(family))return UsCcssG5Unit1Lesson9Questions.Verify(family,parameters,answer);
        if(UsCcssG5Unit1Lesson10Questions.Supports(family))return UsCcssG5Unit1Lesson10Questions.Verify(family,parameters,answer);
        if(UsCcssG5Unit1Lesson11Questions.Supports(family))return UsCcssG5Unit1Lesson11Questions.Verify(family,parameters,answer);
        if(UsCcssG5Unit1Lesson12Questions.Supports(family))return UsCcssG5Unit1Lesson12Questions.Verify(family,parameters,answer);
        if (UsCcssG5Unit2SourceQuestions.Supports(family))
            return UsCcssG5Unit2SourceQuestions.Verify(family,parameters,answer);
        if (UsCcssG5Unit1Lesson1ComparisonQuestions.Supports(family))
            return UsCcssG5Unit1Lesson1ComparisonQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit1FirstLessonQuestions.Supports(family))
            return UsCcssG2Unit1FirstLessonQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit1Lesson2And3Questions.Supports(family))
            return UsCcssG2Unit1Lesson2And3Questions.Verify(family, parameters, answer);
        if (UsCcssG2Unit1Lesson4Questions.Supports(family))
            return UsCcssG2Unit1Lesson4Questions.Verify(family, parameters, answer);
        if (UsCcssG2Unit1Lesson5Questions.Supports(family))
            return UsCcssG2Unit1Lesson5Questions.Verify(family, parameters, answer);
        if (UsCcssG2Unit1Lesson6NumberTalkQuestions.Supports(family))
            return UsCcssG2Unit1Lesson6NumberTalkQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit1Lesson8PictureGraphQuestions.Supports(family))
            return UsCcssG2Unit1Lesson8PictureGraphQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit1Lesson9BarGraphQuestions.Supports(family))
            return UsCcssG2Unit1Lesson9BarGraphQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit1Lesson10ConstructGraphsQuestions.Supports(family))
            return UsCcssG2Unit1Lesson10ConstructGraphsQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit1Lesson11GraphQuestions.Supports(family))
            return UsCcssG2Unit1Lesson11GraphQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit1Lesson12SortAndDisplayQuestions.Supports(family))
            return UsCcssG2Unit1Lesson12SortAndDisplayQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit1Lesson13BarCompareQuestions.Supports(family))
            return UsCcssG2Unit1Lesson13BarCompareQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit1Lesson14TapeDiagramQuestions.Supports(family))
            return UsCcssG2Unit1Lesson14TapeDiagramQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit1Lesson15CompareQuestions.Supports(family))
            return UsCcssG2Unit1Lesson15CompareQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit1Lesson16CompareQuestions.Supports(family))
            return UsCcssG2Unit1Lesson16CompareQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit1Lesson17CenterQuestions.Supports(family))
            return UsCcssG2Unit1Lesson17CenterQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit1Lesson18ClassSurveyQuestions.Supports(family))
            return UsCcssG2Unit1Lesson18ClassSurveyQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit2Lesson1NoRegroupQuestions.Supports(family))
            return UsCcssG2Unit2Lesson1NoRegroupQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit2Lesson2UnknownAddendQuestions.Supports(family))
            return UsCcssG2Unit2Lesson2UnknownAddendQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit2Lesson3ZooStoryQuestions.Supports(family))
            return UsCcssG2Unit2Lesson3ZooStoryQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit2Lesson4OptionalCenterQuestions.Supports(family))
            return UsCcssG2Unit2Lesson4OptionalCenterQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit2Lesson5DecomposeTenQuestions.Supports(family))
            return UsCcssG2Unit2Lesson5DecomposeTenQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit2Lesson6TargetNumbersQuestions.Supports(family))
            return UsCcssG2Unit2Lesson6TargetNumbersQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit2Lesson7SubtractTwoDigitsQuestions.Supports(family))
            return UsCcssG2Unit2Lesson7SubtractTwoDigitsQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit2Lesson8DifferentWaysQuestions.Supports(family))
            return UsCcssG2Unit2Lesson8DifferentWaysQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit2Lesson9MixedAddSubtractQuestions.Supports(family))
            return UsCcssG2Unit2Lesson9MixedAddSubtractQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit2Lesson10TargetStage5Questions.Supports(family))
            return UsCcssG2Unit2Lesson10TargetStage5Questions.Verify(family, parameters, answer);
        if (UsCcssG2Unit2Lesson11SeedsStoryQuestions.Supports(family))
            return UsCcssG2Unit2Lesson11SeedsStoryQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit2Lesson12MancalaQuestions.Supports(family))
            return UsCcssG2Unit2Lesson12MancalaQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit2Lesson13StoryEquationsQuestions.Supports(family))
            return UsCcssG2Unit2Lesson13StoryEquationsQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit2Lesson14TwoStepSeedsQuestions.Supports(family))
            return UsCcssG2Unit2Lesson14TwoStepSeedsQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit2Lesson15TapeStoriesQuestions.Supports(family))
            return UsCcssG2Unit2Lesson15TapeStoriesQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit2Lesson16MarketInventoryQuestions.Supports(family))
            return UsCcssG2Unit2Lesson16MarketInventoryQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit3Lesson1StandardUnitsQuestions.Supports(family))
            return UsCcssG2Unit3Lesson1StandardUnitsQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit3Lesson2MeasureCmQuestions.Supports(family))
            return UsCcssG2Unit3Lesson2MeasureCmQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit3Lesson3BuildRulerQuestions.Supports(family))
            return UsCcssG2Unit3Lesson3BuildRulerQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit3Lesson4EstimateMeasureCmQuestions.Supports(family))
            return UsCcssG2Unit3Lesson4EstimateMeasureCmQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit3Lesson5MetersQuestions.Supports(family))
            return UsCcssG2Unit3Lesson5MetersQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit3Lesson6ReptileStoriesQuestions.Supports(family))
            return UsCcssG2Unit3Lesson6ReptileStoriesQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit3Lesson7EstimateMeasureStage2Questions.Supports(family))
            return UsCcssG2Unit3Lesson7EstimateMeasureStage2Questions.Verify(family, parameters, answer);
        if (UsCcssG2Unit3Lesson8WhatIsAnInchQuestions.Supports(family))
            return UsCcssG2Unit3Lesson8WhatIsAnInchQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit3Lesson9FishFeetQuestions.Supports(family))
            return UsCcssG2Unit3Lesson9FishFeetQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit3Lesson10TornTapeQuestions.Supports(family))
            return UsCcssG2Unit3Lesson10TornTapeQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit3Lesson11SareeRibbonQuestions.Supports(family))
            return UsCcssG2Unit3Lesson11SareeRibbonQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit3Lesson12FriendshipQuestions.Supports(family))
            return UsCcssG2Unit3Lesson12FriendshipQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit3Lesson13CenterDay2Questions.Supports(family))
            return UsCcssG2Unit3Lesson13CenterDay2Questions.Verify(family, parameters, answer);
        if (UsCcssG2Unit3Lesson14HandSpanLinePlotQuestions.Supports(family))
            return UsCcssG2Unit3Lesson14HandSpanLinePlotQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit3Lesson15PencilLinePlotQuestions.Supports(family))
            return UsCcssG2Unit3Lesson15PencilLinePlotQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit3Lesson16PlantHeightsQuestions.Supports(family))
            return UsCcssG2Unit3Lesson16PlantHeightsQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit3Lesson17CenterPlotQuestions.Supports(family))
            return UsCcssG2Unit3Lesson17CenterPlotQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit3Lesson18YardstickQuestions.Supports(family))
            return UsCcssG2Unit3Lesson18YardstickQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit4Lesson01NumberLineQuestions.Supports(family))
            return UsCcssG2Unit4Lesson01NumberLineQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit4Lesson02NumberLineFeaturesQuestions.Supports(family))
            return UsCcssG2Unit4Lesson02NumberLineFeaturesQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit4Lesson03UnlabeledTicksQuestions.Supports(family))
            return UsCcssG2Unit4Lesson03UnlabeledTicksQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit4NextThreeQuestions.Supports(family))
            return UsCcssG2Unit4NextThreeQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit4ArrowLessonsQuestions.Supports(family))
            return UsCcssG2Unit4ArrowLessonsQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit4FinalSixQuestions.Supports(family))
            return UsCcssG2Unit4FinalSixQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit5FirstSixQuestions.Supports(family))
            return UsCcssG2Unit5FirstSixQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit5ComparisonQuestions.Supports(family))
            return UsCcssG2Unit5ComparisonQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit9ExactQuestions.Supports(family))
            return UsCcssG2Unit9ExactQuestions.Verify(family,parameters,answer);
        if (UsCcssG2Unit8ExactQuestions.Supports(family))
            return UsCcssG2Unit8ExactQuestions.Verify(family,parameters,answer);
        if (UsCcssG2Unit7LastTenQuestions.Supports(family))
            return UsCcssG2Unit7LastTenQuestions.Verify(family,parameters,answer);
        if (UsCcssG2Unit7NextFourQuestions.Supports(family))
            return UsCcssG2Unit7NextFourQuestions.Verify(family,parameters,answer);
        if (UsCcssG2Unit7FirstFourQuestions.Supports(family))
            return UsCcssG2Unit7FirstFourQuestions.Verify(family, parameters, answer);
        if (UsCcssG2Unit6ExactQuestions.Supports(family))
            return UsCcssG2Unit6ExactQuestions.Verify(family, parameters, answer);
        if (UsCcssB001SourceGoalQuestions.Supports(family))
            return UsCcssB001SourceGoalQuestions.Verify(family, parameters, answer);
        if (UsCcssG1Unit2StoryQuestions.Supports(family))
            return UsCcssG1Unit2StoryQuestions.Verify(family, parameters, answer);
        if (UsCcssG1Batch03DataQuestions.Supports(family))
            return UsCcssG1Batch03DataQuestions.Verify(family, parameters, answer);
        if (UsCcssT001AuthoredActivities.Supports(family))
            return UsCcssT001AuthoredActivities.Verify(family, parameters, answer);
        if (UsCcssT001ConstructShapeGraph.Supports(family))
            return UsCcssT001ConstructShapeGraph.Verify(family, parameters, answer);
        if (UsCcssT001RepresentationChoices.Supports(family))
            return UsCcssT001RepresentationChoices.Verify(family, parameters, answer);
        if (UsCcssT001ExactQuestions.Supports(family))
            return UsCcssT001ExactQuestions.Verify(family, parameters, answer);
        // Grade 1 first-edition source: cardinality up to 20 and
        // addition of two dot cubes up to 10. Fail closed for tampered tasks.
        if (family.StartsWith("uae.g1.u01.l0", StringComparison.Ordinal))
        {
            var (minimum, maximum) = UaeGrade1CountingRange(family);
            var normalizedAnswer = new string(answer.Trim().Select(c =>
                c is >= '٠' and <= '٩' ? (char)('0' + c - '٠') :
                c is >= '۰' and <= '۹' ? (char)('0' + c - '۰') : c).ToArray());
            return minimum >= 0 &&
                parameters.TryGetValue("count", out var count) &&
                count >= minimum && count <= maximum &&
                parameters.TryGetValue("scene", out var scene) &&
                scene is >= 0 and <= 9 &&
                int.TryParse(normalizedAnswer, NumberStyles.Integer, CultureInfo.InvariantCulture, out var response) &&
                response == count;
        }
        if (family is "uae.g1.u02.l01.addition_stories" or "uae.g1.u02.l02.model_addition" or "uae.g1.u02.l03.addition_number_sentences" or "uae.g1.u02.l05.vertical_addition" or "uae.g1.u02.l06.write_story_equation")
        {
            var normalizedAnswer = new string(answer.Trim().Select(c =>
                c is >= '٠' and <= '٩' ? (char)('0' + c - '٠') :
                c is >= '۰' and <= '۹' ? (char)('0' + c - '۰') : c).ToArray());
            return parameters.TryGetValue("left", out var left) && left is >= 1 and <= 7 &&
                parameters.TryGetValue("right", out var right) && right is >= 1 and <= 4 &&
                left + right <= 10 &&
                parameters.TryGetValue("scene", out var scene) && scene is >= 0 and <= 5 &&
                int.TryParse(normalizedAnswer, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedUaeValue) &&
                parsedUaeValue == left + right;
        }
        if (family == "uae.g1.u02.l04.add_zero")
        {
            var normalizedAnswer = new string(answer.Trim().Select(c =>
                c is >= '٠' and <= '٩' ? (char)('0' + c - '٠') :
                c is >= '۰' and <= '۹' ? (char)('0' + c - '۰') : c).ToArray());
            return parameters.TryGetValue("left", out var left) && left is >= 0 and <= 9 &&
                parameters.TryGetValue("right", out var right) && right is >= 0 and <= 9 &&
                (left == 0 || right == 0) &&
                parameters.TryGetValue("scene", out var scene) && scene is >= 0 and <= 3 &&
                int.TryParse(normalizedAnswer, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedUaeValue) &&
                parsedUaeValue == left + right;
        }
        if (family is "uae.g1.u02.l07.make_four_five" or "uae.g1.u02.l08.make_six_seven" or "uae.g1.u02.l09.make_eight" or "uae.g1.u02.l10.make_nine" or "uae.g1.u02.l11.make_ten_frame")
        {
            var normalizedAnswer = new string(answer.Trim().Select(c =>
                c is >= '٠' and <= '٩' ? (char)('0' + c - '٠') :
                c is >= '۰' and <= '۹' ? (char)('0' + c - '۰') : c).ToArray());
            return parameters.TryGetValue("total", out var total) &&
                (family == "uae.g1.u02.l07.make_four_five" ? total is 4 or 5 :
                 family == "uae.g1.u02.l08.make_six_seven" ? total is 6 or 7 :
                 family == "uae.g1.u02.l09.make_eight" ? total == 8 :
                 family == "uae.g1.u02.l10.make_nine" ? total == 9 : total == 10) &&
                parameters.TryGetValue("left", out var left) && left >= 0 && left <= total &&
                parameters.TryGetValue("right", out var right) && right >= 0 && left + right == total &&
                parameters.TryGetValue("scene", out var scene) && scene is >= 0 and <= 3 &&
                int.TryParse(normalizedAnswer, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedUaeValue) &&
                parsedUaeValue == right;
        }
        if (family == "uae.g1.u05.l01.count_back_1_2_3")
        {
            var normalized = new string(answer.Trim().Select(c =>
                c is >= '٠' and <= '٩' ? (char)('0' + c - '٠') :
                c is >= '۰' and <= '۹' ? (char)('0' + c - '۰') : c).ToArray());
            return parameters.TryGetValue("start", out var start) && start is >= 4 and <= 20 && parameters.TryGetValue("steps", out var steps) && steps is >= 1 and <= 3 && parameters.TryGetValue("result", out var result) && result == start - steps && result >= 0 &&
                int.TryParse(normalized, NumberStyles.Integer, CultureInfo.InvariantCulture, out var v) &&
                v == (parameters["start"] - parameters["steps"]);
        }
        if (family == "uae.g1.u05.l02.number_line_subtract")
        {
            var normalized = new string(answer.Trim().Select(c =>
                c is >= '٠' and <= '٩' ? (char)('0' + c - '٠') :
                c is >= '۰' and <= '۹' ? (char)('0' + c - '۰') : c).ToArray());
            return parameters.TryGetValue("start", out var start) && start is >= 5 and <= 20 && parameters.TryGetValue("hops", out var hops) && hops is >= 1 and <= 9 && hops <= start && parameters.TryGetValue("result", out var result) && result == start - hops &&
                int.TryParse(normalized, NumberStyles.Integer, CultureInfo.InvariantCulture, out var v) &&
                v == (parameters["start"] - parameters["hops"]);
        }
        if (family == "uae.g1.u05.l03.doubles_subtraction")
        {
            var normalized = new string(answer.Trim().Select(c =>
                c is >= '٠' and <= '٩' ? (char)('0' + c - '٠') :
                c is >= '۰' and <= '۹' ? (char)('0' + c - '۰') : c).ToArray());
            return parameters.TryGetValue("part", out var part) && part is >= 1 and <= 10 && parameters.TryGetValue("whole", out var whole) && whole == 2 * part &&
                parameters.TryGetValue("scene", out var context) && context is >= 0 and <= 2 &&
                int.TryParse(normalized, NumberStyles.Integer, CultureInfo.InvariantCulture, out var v) &&
                v == (parameters["part"]);
        }
        if (family == "uae.g1.u05.l04.story_subtraction_equation")
        {
            var normalized = new string(answer.Trim().Select(c =>
                c is >= '٠' and <= '٩' ? (char)('0' + c - '٠') :
                c is >= '۰' and <= '۹' ? (char)('0' + c - '۰') : c).ToArray());
            return parameters.TryGetValue("whole", out var whole) && whole is >= 5 and <= 20 && parameters.TryGetValue("removed", out var removed) && removed >= 1 && removed <= 10 && removed < whole && parameters.TryGetValue("remaining", out var remaining) && remaining == whole - removed && parameters.TryGetValue("scene", out var scene) && scene is >= 0 and <= 2 &&
                int.TryParse(normalized, NumberStyles.Integer, CultureInfo.InvariantCulture, out var v) &&
                v == (parameters["whole"] - parameters["removed"]);
        }
        if (family == "uae.g1.u05.l05.subtract_through_ten")
        {
            var normalized = new string(answer.Trim().Select(c =>
                c is >= '٠' and <= '٩' ? (char)('0' + c - '٠') :
                c is >= '۰' and <= '۹' ? (char)('0' + c - '۰') : c).ToArray());
            return parameters.TryGetValue("whole", out var whole) && whole is >= 11 and <= 18 && parameters.TryGetValue("removed", out var removed) && removed is >= 2 and <= 9 && parameters.TryGetValue("toTen", out var toTen) && toTen == whole - 10 && toTen < removed && parameters.TryGetValue("rest", out var rest) && rest == removed - toTen && rest >= 1 && parameters.TryGetValue("result", out var result) && result == whole - removed && result == 10 - rest &&
                int.TryParse(normalized, NumberStyles.Integer, CultureInfo.InvariantCulture, out var v) &&
                v == (parameters["whole"] - parameters["removed"]);
        }
        if (family == "uae.g1.u05.l06.related_add_sub_facts")
        {
            var normalized = new string(answer.Trim().Select(c =>
                c is >= '٠' and <= '٩' ? (char)('0' + c - '٠') :
                c is >= '۰' and <= '۹' ? (char)('0' + c - '۰') : c).ToArray());
            return parameters.TryGetValue("a", out var a) && a is >= 2 and <= 9 && parameters.TryGetValue("b", out var b) && b is >= 2 and <= 10 && parameters.TryGetValue("total", out var total) && total == a + b && total <= 20 &&
                int.TryParse(normalized, NumberStyles.Integer, CultureInfo.InvariantCulture, out var v) &&
                v == (parameters["b"]);
        }
        if (family == "uae.g1.u05.l07.fact_family_four_equations")
        {
            var normalized = new string(answer.Trim().Select(c =>
                c is >= '٠' and <= '٩' ? (char)('0' + c - '٠') :
                c is >= '۰' and <= '۹' ? (char)('0' + c - '۰') : c).ToArray());
            return parameters.TryGetValue("a", out var a) && a is >= 1 and <= 9 && parameters.TryGetValue("b", out var b) && b is >= 1 and <= 9 && parameters.TryGetValue("whole", out var whole) && whole == a + b && parameters.TryGetValue("mode", out var mode) && mode is >= 0 and <= 3 &&
                int.TryParse(normalized, NumberStyles.Integer, CultureInfo.InvariantCulture, out var v) &&
                v == (parameters["mode"] < 2 ? parameters["whole"] : parameters["mode"] == 2 ? parameters["b"] : parameters["a"]);
        }
        if (family == "uae.g1.u05.l08.missing_addend_part_whole")
        {
            var normalized = new string(answer.Trim().Select(c =>
                c is >= '٠' and <= '٩' ? (char)('0' + c - '٠') :
                c is >= '۰' and <= '۹' ? (char)('0' + c - '۰') : c).ToArray());
            return parameters.TryGetValue("whole", out var whole) && whole is >= 4 and <= 20 && parameters.TryGetValue("known", out var known) && known >= 1 && known <= 10 && known < whole && parameters.TryGetValue("missing", out var missing) && missing == whole - known && parameters.TryGetValue("mode", out var mode) && mode is 0 or 1 &&
                int.TryParse(normalized, NumberStyles.Integer, CultureInfo.InvariantCulture, out var v) &&
                v == (parameters["whole"] - parameters["known"]);
        }
        if (UaeG1Final29Ordinal(family) >= 74)
            return VerifyUaeG1Final29(family, parameters, answer);
        if (UaeGrade1Unit6Ordinal(family) > 0)
            return VerifyUaeGrade1Unit6(family, parameters, answer);
        if (UaeGrade1Unit5Ordinal(family) > 0)
            return VerifyUaeGrade1Unit5(family, parameters, answer);
        if (family == "uae.g1.u04.l09.add_three_with_strategy")
        {
            var normalized = new string(answer.Trim().Select(c =>
                c is >= '٠' and <= '٩' ? (char)('0' + c - '٠') :
                c is >= '۰' and <= '۹' ? (char)('0' + c - '۰') : c).ToArray());
            return parameters.TryGetValue("a", out var a) && a is >= 1 and <= 9 &&
                parameters.TryGetValue("b", out var b) && b is >= 1 and <= 9 &&
                parameters.TryGetValue("c", out var c) && c is >= 1 and <= 9 &&
                parameters.TryGetValue("mode", out var mode) && mode is >= 0 and <= 2 &&
                (mode == 0 ? (a is >= 3 and <= 9 && a + b == 10 && c <= 8) :
                 mode == 1 ? (a <= 7 && b == c && b <= 5) :
                             (a <= 7 && b <= 7 && c <= 6)) &&
                parameters.TryGetValue("pair", out var pair) &&
                    pair == (mode == 1 ? b + c : a + b) &&
                parameters.TryGetValue("total", out var total) && total == a + b + c && total <= 20 &&
                int.TryParse(normalized, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedUaeValue) &&
                parsedUaeValue == total;
        }
        if (family == "uae.g1.u04.l08.commutative_add")
        {
            var normalized = new string(answer.Trim().Select(c =>
                c is >= '٠' and <= '٩' ? (char)('0' + c - '٠') :
                c is >= '۰' and <= '۹' ? (char)('0' + c - '۰') : c).ToArray());
            return parameters.TryGetValue("a", out var a) && a is >= 0 and <= 10 &&
                parameters.TryGetValue("b", out var b) && b is >= 0 and <= 10 &&
                parameters.TryGetValue("total", out var total) && total == a + b && total <= 20 &&
                parameters.TryGetValue("mode", out var mode) && mode is 0 or 1 &&
                parameters.TryGetValue("scene", out var scene) && scene is >= 0 and <= 3 &&
                int.TryParse(normalized, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedUaeValue) &&
                parsedUaeValue == (mode == 0 ? a : total);
        }
        if (family == "uae.g1.u04.l07.make_ten_add")
        {
            var normalized = new string(answer.Trim().Select(c =>
                c is >= '٠' and <= '٩' ? (char)('0' + c - '٠') :
                c is >= '۰' and <= '۹' ? (char)('0' + c - '۰') : c).ToArray());
            return parameters.TryGetValue("a", out var a) && a is >= 6 and <= 9 &&
                parameters.TryGetValue("b", out var b) && b is >= 2 and <= 9 &&
                parameters.TryGetValue("toTen", out var toTen) && toTen == 10 - a &&
                parameters.TryGetValue("remainder", out var remainder) && remainder == b - toTen && remainder >= 1 &&
                parameters.TryGetValue("total", out var total) && total == a + b && total <= 20 &&
                parameters.TryGetValue("scene", out var scene) && scene is >= 0 and <= 3 &&
                int.TryParse(normalized, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedUaeValue) &&
                parsedUaeValue == total;
        }
        if (family == "uae.g1.u04.l06.act_out_problem_solving")
        {
            var normalized = new string(answer.Trim().Select(c =>
                c is >= '٠' and <= '٩' ? (char)('0' + c - '٠') :
                c is >= '۰' and <= '۹' ? (char)('0' + c - '۰') : c).ToArray());
            return parameters.TryGetValue("a", out var a) &&
                parameters.TryGetValue("b", out var b) &&
                parameters.TryGetValue("c", out var c) && c is >= 0 and <= 4 &&
                parameters.TryGetValue("mode", out var mode) && mode is >= 0 and <= 4 &&
                (mode == 4
                    ? (a is >= 3 and <= 9 && b == a && c == 0)
                    : (a is >= 1 and <= 7 && b is >= 1 and <= 6 && (mode == 1 ? c >= 1 : c == 0))) &&
                parameters.TryGetValue("total", out var total) && total == a + b + c && total <= 20 &&
                int.TryParse(normalized, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedUaeValue) &&
                parsedUaeValue == (mode == 2 ? b : total);
        }
        if (family == "uae.g1.u04.l05.near_doubles_add")
        {
            var normalized = new string(answer.Trim().Select(c =>
                c is >= '٠' and <= '٩' ? (char)('0' + c - '٠') :
                c is >= '۰' and <= '۹' ? (char)('0' + c - '۰') : c).ToArray());
            return parameters.TryGetValue("base", out var baseValue) && baseValue is >= 2 and <= 9 &&
                parameters.TryGetValue("delta", out var delta) && delta is -1 or 1 &&
                parameters.TryGetValue("orientation", out var orientation) && orientation is 0 or 1 &&
                parameters.TryGetValue("scene", out var scene) && scene is >= 0 and <= 3 &&
                int.TryParse(normalized, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result) &&
                result == 2 * baseValue + delta;
        }
        if (family == "uae.g1.u04.l04.doubles_add")
        {
            var normalized = new string(answer.Trim().Select(c =>
                c is >= '٠' and <= '٩' ? (char)('0' + c - '٠') :
                c is >= '۰' and <= '۹' ? (char)('0' + c - '۰') : c).ToArray());
            return parameters.TryGetValue("equalAddend", out var equalAddend) && equalAddend is >= 1 and <= 10 &&
                parameters.TryGetValue("scene", out var scene) && scene is >= 0 and <= 3 &&
                int.TryParse(normalized, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedUaeValue) &&
                parsedUaeValue == equalAddend + equalAddend;
        }
        if (family == "uae.g1.u04.l03.add_on_number_line")
        {
            var normalized = new string(answer.Trim().Select(c =>
                c is >= '٠' and <= '٩' ? (char)('0' + c - '٠') :
                c is >= '۰' and <= '۹' ? (char)('0' + c - '۰') : c).ToArray());
            return parameters.TryGetValue("start", out var start) && start is >= 1 and <= 11 &&
                parameters.TryGetValue("hops", out var hops) && hops is >= 1 and <= 4 &&
                start + hops <= 12 &&
                parameters.TryGetValue("scene", out var scene) && scene is >= 0 and <= 3 &&
                int.TryParse(normalized, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result) &&
                result == start + hops;
        }
        if (family == "uae.g1.u04.l02.count_on_fils")
        {
            var normalized = new string(answer.Trim().Select(c =>
                c is >= '٠' and <= '٩' ? (char)('0' + c - '٠') :
                c is >= '۰' and <= '۹' ? (char)('0' + c - '۰') : c).ToArray());
            return parameters.TryGetValue("start", out var start) && start is >= 4 and <= 10 &&
                parameters.TryGetValue("added", out var added) && added is >= 1 and <= 3 &&
                start + added <= 13 &&
                parameters.TryGetValue("scene", out var scene) && scene is >= 0 and <= 3 &&
                int.TryParse(normalized, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result) &&
                result == start + added;
        }
        if (family == "uae.g1.u04.l01.count_on_1_2_3")
        {
            var normalized = new string(answer.Trim().Select(c =>
                c is >= '٠' and <= '٩' ? (char)('0' + c - '٠') :
                c is >= '۰' and <= '۹' ? (char)('0' + c - '۰') : c).ToArray());
            return parameters.TryGetValue("start", out var start) && start is >= 1 and <= 19 &&
                parameters.TryGetValue("step", out var step) && step is >= 1 and <= 3 &&
                start + step <= 20 &&
                parameters.TryGetValue("orientation", out var orientation) && orientation is 0 or 1 &&
                int.TryParse(normalized, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result) &&
                result == start + step;
        }
        if (family == "uae.g1.u03.l14.true_false_subtraction")
        {
            return parameters.TryGetValue("whole", out var whole) && whole is >= 1 and <= 10 &&
                parameters.TryGetValue("removed", out var removed) && removed >= 0 && removed <= whole &&
                parameters.TryGetValue("shown", out var shown) && shown is >= 0 and <= 10 &&
                parameters.TryGetValue("scene", out var scene) && scene is >= 0 and <= 3 &&
                string.Equals(answer.Trim(), whole - removed == shown ? "صحيحة" : "خطأ", StringComparison.Ordinal);
        }
        if (family == "uae.g1.u02.l13.true_false_addition")
        {
            return parameters.TryGetValue("left", out var left) && left is >= 0 and <= 9 &&
                parameters.TryGetValue("right", out var right) && right is >= 0 and <= 9 &&
                left + right <= 10 &&
                parameters.TryGetValue("shown", out var shown) && shown is >= 0 and <= 10 &&
                parameters.TryGetValue("scene", out var scene) && scene is >= 0 and <= 3 &&
                string.Equals(answer.Trim(), left + right == shown ? "صحيحة" : "خطأ", StringComparison.Ordinal);
        }
        if (family is "uae.g1.u03.l01.subtraction_stories" or "uae.g1.u03.l02.model_subtraction" or "uae.g1.u03.l03.subtraction_number_sentences" or "uae.g1.u03.l04.subtract_zero_or_all" or "uae.g1.u03.l05.vertical_subtraction" or "uae.g1.u03.l07.compare_groups" or "uae.g1.u03.l08.subtract_4_5" or "uae.g1.u03.l09.subtract_6_7" or "uae.g1.u03.l10.subtract_8" or "uae.g1.u03.l11.subtract_9" or "uae.g1.u03.l12.subtract_10")
        {
            var normalizedAnswer = new string(answer.Trim().Select(c =>
                c is >= '٠' and <= '٩' ? (char)('0' + c - '٠') :
                c is >= '۰' and <= '۹' ? (char)('0' + c - '۰') : c).ToArray());
            return parameters.TryGetValue("whole", out var whole) && whole is >= 1 and <= 10 &&
                parameters.TryGetValue("removed", out var removed) && removed >= 0 && removed <= whole &&
                (family == "uae.g1.u03.l04.subtract_zero_or_all" ? removed == 0 || removed == whole :
                 family == "uae.g1.u03.l08.subtract_4_5" ? whole is 4 or 5 :
                 family == "uae.g1.u03.l09.subtract_6_7" ? whole is 6 or 7 : family == "uae.g1.u03.l10.subtract_8" ? whole == 8 :
                 family == "uae.g1.u03.l11.subtract_9" ? whole == 9 :
                 family == "uae.g1.u03.l12.subtract_10" ? whole == 10 : removed >= 1) &&
                parameters.TryGetValue("scene", out var scene) && scene is >= 0 and <= 5 &&
                int.TryParse(normalizedAnswer, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedUaeValue) &&
                parsedUaeValue == whole - removed;
        }
        if (family == "uae.g1.u03.l13.related_add_subtract")
        {
            var normalizedAnswer = new string(answer.Trim().Select(c =>
                c is >= '٠' and <= '٩' ? (char)('0' + c - '٠') :
                c is >= '۰' and <= '۹' ? (char)('0' + c - '۰') : c).ToArray());
            return parameters.TryGetValue("left", out var left) && left is >= 1 and <= 9 &&
                parameters.TryGetValue("right", out var right) && right is >= 1 and <= 9 &&
                left + right <= 10 &&
                parameters.TryGetValue("removeLeft", out var removeLeft) && removeLeft is 0 or 1 &&
                int.TryParse(normalizedAnswer, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed) &&
                parsed == (removeLeft == 1 ? right : left);
        }
        if (family == "usccss.g1.u01.l01.count_collection_to20")
            return parameters.TryGetValue("count", out var count) &&
                count is >= 1 and <= 20 &&
                int.TryParse(answer, NumberStyles.Integer, CultureInfo.InvariantCulture, out var counted) &&
                counted == count;
        if (family == "usccss.g1.u01.l01.add_dot_cubes_to10")
            return parameters.TryGetValue("left", out var left) &&
                parameters.TryGetValue("right", out var right) &&
                left is >= 1 and <= 6 && right is >= 1 and <= 6 &&
                left + right <= 10 &&
                int.TryParse(answer, NumberStyles.Integer, CultureInfo.InvariantCulture, out var total) &&
                total == left + right;
        if (SupportingPracticeCompletionEngine.Supports(family))
            return SupportingPracticeCompletionEngine.Verify(family, parameters, answer);

        if (family == "fractions.compare.unlike.common_denominator")
            return string.Equals(
                answer,
                Compare(parameters["n1"] * parameters["d2"], parameters["n2"] * parameters["d1"]),
                StringComparison.Ordinal);

        if (family == "fractions.compare.unlike.select_greater")
        {
            var comparison = Compare(
                parameters["n1"] * parameters["d2"],
                parameters["n2"] * parameters["d1"]);
            var expected = comparison switch
            {
                ">" => "left",
                "<" => "right",
                _ => "equal"
            };
            return string.Equals(answer.Trim(), expected, StringComparison.OrdinalIgnoreCase);
        }

        if (family == "fractions.compare.unlike.true_false")
        {
            var actual = Compare(
                parameters["n1"] * parameters["d2"],
                parameters["n2"] * parameters["d1"]);
            var claimed = RelationFromCode(parameters["claimedRelation"]);
            var expected = string.Equals(actual, claimed, StringComparison.Ordinal)
                ? "true"
                : "false";
            return string.Equals(answer.Trim(), expected, StringComparison.OrdinalIgnoreCase);
        }

        if (family == "fractions.compare.unlike.order_three")
        {
            return string.Equals(
                NormalizeFractionOrderAnswer(answer),
                NormalizeFractionOrderAnswer(OrderThreeFractions(parameters)),
                StringComparison.Ordinal);
        }

        if (family == ExactLinearInequalityQuestionFactory.FamilyId)
        {
            var coefficient = parameters["coefficient"];
            var offset = parameters["offset"];
            var right = parameters["right"];
            if (coefficient == 0 || (right - offset) % coefficient != 0)
                return false;

            var boundary = (right - offset) / coefficient;
            var originalRelation = (InequalityRelation)parameters["relation"];
            var solvedRelation = coefficient < 0
                ? Reverse(originalRelation)
                : originalRelation;
            var expected = $"x {RelationSymbol(solvedRelation)} {boundary.ToString(CultureInfo.InvariantCulture)}";
            return string.Equals(
                NormalizeInequalityAnswer(answer),
                NormalizeInequalityAnswer(expected),
                StringComparison.Ordinal);
        }

        if (family == "algebra.linear.inequality.variables_both_sides")
        {
            var leftCoefficient = parameters["leftCoefficient"];
            var rightCoefficient = parameters["rightCoefficient"];
            var coefficient = leftCoefficient - rightCoefficient;
            var leftOffset = parameters["leftOffset"];
            var rightOffset = parameters["rightOffset"];
            if (coefficient == 0 || (rightOffset - leftOffset) % coefficient != 0)
                return false;

            var boundary = (rightOffset - leftOffset) / coefficient;
            var originalRelation = (InequalityRelation)parameters["relation"];
            var solvedRelation = coefficient < 0
                ? Reverse(originalRelation)
                : originalRelation;
            var expected = $"x {RelationSymbol(solvedRelation)} {boundary.ToString(CultureInfo.InvariantCulture)}";
            return string.Equals(
                NormalizeInequalityAnswer(answer),
                NormalizeInequalityAnswer(expected),
                StringComparison.Ordinal);
        }

        if (family == "usccss.g11.alg2.u01.l02.paper_piece_area")
        {
            if (!TryParseFraction(answer, out var n, out var d)) return false;
            return parameters["cutCount"] is >= 0 and <= 8 &&
                (long)n * (1 << parameters["cutCount"]) == (long)80 * d;
        }

        if (family == "usccss.g11.alg2.u01.l02.rational_geometric_term")
        {
            if (!TryParseFraction(answer, out var n, out var d)) return false;
            var first=parameters["first"];var rn=parameters["ratioNumerator"];
            var rd=parameters["ratioDenominator"];var term=parameters["term"];
            return rd > 0 && term is >= 1 and <= 8 &&
                (long)n * G11IntPow(rd,term-1)==
                (long)d * first * G11IntPow(rn,term-1);
        }

        if (family == "usccss.g11.alg2.u01.l02.growth_factor")
        {
            if (!TryParseFraction(answer, out var n, out var d)) return false;
            return parameters["previous"] != 0 &&
                (long)n*parameters["previous"]==(long)d*parameters["next"];
        }

        if (family == "usccss.g11.alg2.u01.l01.halving_fraction")
        {
            if (!TryParseFraction(answer, out var numerator, out var denominator)) return false;
            return parameters["n"] is >= 1 and <= 12 &&
                numerator * (1 << parameters["n"]) == denominator;
        }

        if (family.StartsWith("fractions.add_subtract.", StringComparison.Ordinal))
        {
            if (!TryParseFraction(answer, out var answerNumerator, out var answerDenominator))
                return false;

            var n1 = parameters["n1"];
            var d1 = parameters["d1"];
            var n2 = parameters["n2"];
            var d2 = parameters["d2"];
            var operation = parameters["operation"];
            if (d1 <= 0 || d2 <= 0 || operation is < 0 or > 1)
                return false;

            var expectedNumerator = operation == 0
                ? n1 * d2 + n2 * d1
                : n1 * d2 - n2 * d1;
            var expectedDenominator = d1 * d2;
            if (expectedNumerator < 0)
                return false;

            return answerNumerator * expectedDenominator ==
                expectedNumerator * answerDenominator;
        }

        if (family == "fractions.compare.benchmark")
        {
            var left = parameters["n"] * parameters["benchmarkD"];
            var right = parameters["benchmarkN"] * parameters["d"];
            return string.Equals(answer.Trim(), Compare(left, right), StringComparison.Ordinal);
        }

        if (family == "trigonometry.right_triangle.ratio_exact")
        {
            if (!TryParseFraction(answer, out var answerNumerator, out var answerDenominator))
                return false;

            var numerator = parameters["ratioNumerator"];
            var denominator = parameters["ratioDenominator"];
            return denominator > 0 &&
                answerNumerator * denominator == numerator * answerDenominator;
        }

        if (family == "geometry.congruence.identify_criterion")
        {
            var expected = parameters["criterion"] switch
            {
                0 => "SSS",
                1 => "SAS",
                2 => "ASA",
                3 => "RHS",
                _ => string.Empty
            };
            return expected.Length > 0 &&
                string.Equals(answer.Trim(), expected, StringComparison.OrdinalIgnoreCase);
        }

        if (family is "number.whole.divide.with_remainder.build" or
            "number.whole.divide.with_remainder.apply" or
            "usccss.g4.u06.l19.partial_quotients_remainders")
        {
            var dividend = parameters["dividend"];
            var divisor = parameters["divisor"];
            if (dividend < 0 || divisor <= 0)
                return false;

            var quotient = dividend / divisor;
            var remainder = dividend % divisor;
            if (family == "usccss.g4.u06.l19.partial_quotients_remainders" &&
                (!parameters.TryGetValue("partialTens", out var qTens) ||
                 !parameters.TryGetValue("partialOnes", out var qOnes) ||
                 !parameters.TryGetValue("remainder", out var rSource) ||
                 dividend < 100 || dividend > 9999 || divisor < 2 || divisor > 9 ||
                 rSource != remainder || qTens < 10 || qTens % 10 != 0 ||
                 qOnes < 0 || qOnes > 9 ||
                 (long)divisor * (qTens + qOnes) + remainder != dividend))
                return false;
            var expected = $"{quotient.ToString(CultureInfo.InvariantCulture)} r {remainder.ToString(CultureInfo.InvariantCulture)}";
            return string.Equals(
                NormalizeRemainderAnswer(answer),
                NormalizeRemainderAnswer(expected),
                StringComparison.Ordinal);
        }

        if (family == "fractions.represent.interpret.fraction_bar")
        {
            if (!TryParseFraction(answer, out var answerNumerator, out var answerDenominator))
                return false;

            return parameters["denominator"] > 0 &&
                parameters["numerator"] >= 0 &&
                parameters["numerator"] <= parameters["denominator"] &&
                answerNumerator * parameters["denominator"] ==
                    parameters["numerator"] * answerDenominator;
        }

        if (family is
            "probability.theoretical.simple_event" or
            "probability.theoretical.complement" or
            "probability.theoretical.two_coins_exactly_one")
        {
            if (!TryParseFraction(answer, out var answerNumerator, out var answerDenominator))
                return false;

            var expectedNumerator = family switch
            {
                "probability.theoretical.simple_event" => parameters["favourable"],
                "probability.theoretical.complement" => parameters["total"] - parameters["favourable"],
                "probability.theoretical.two_coins_exactly_one" => 2,
                _ => 0
            };
            var expectedDenominator = family == "probability.theoretical.two_coins_exactly_one"
                ? 4
                : parameters["total"];

            return expectedDenominator > 0 &&
                expectedNumerator >= 0 &&
                expectedNumerator <= expectedDenominator &&
                answerNumerator * expectedDenominator ==
                    expectedNumerator * answerDenominator;
        }

        if (family is
            "vectors.add.exact_rational" or
            "vectors.subtract.exact_rational" or
            "vectors.scalar_multiply.exact_rational" or
            "vectors.between_points.exact")
        {
            if (!TryParseVector2(answer, out var x, out var y))
                return false;

            return family switch
            {
                "vectors.add.exact_rational" =>
                    x == parameters["ax"] + parameters["bx"] &&
                    y == parameters["ay"] + parameters["by"],
                "vectors.subtract.exact_rational" =>
                    x == parameters["ax"] - parameters["bx"] &&
                    y == parameters["ay"] - parameters["by"],
                "vectors.scalar_multiply.exact_rational" =>
                    x == parameters["scalar"] * parameters["ax"] &&
                    y == parameters["scalar"] * parameters["ay"],
                "vectors.between_points.exact" =>
                    x == parameters["x2"] - parameters["x1"] &&
                    y == parameters["y2"] - parameters["y1"],
                _ => false
            };
        }

        if (!int.TryParse(answer, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value))
            return false;

        if (family is
            "number.whole.add_subtract.within_10.build" or
            "number.whole.add_subtract.within_10.apply" or
            "number.whole.add_subtract.across_ten.build" or
            "number.whole.add_subtract.across_ten.apply" or
            "number.whole.add_subtract.within_100.build" or
            "number.whole.add_subtract.within_100.apply" or
            "number.whole.add_subtract.columnar.build" or
            "number.whole.add_subtract.columnar.apply")
        {
            var operation = parameters["operation"];
            var left = parameters["left"];
            var right = parameters["right"];
            if (left < 0 || right < 0 || operation is < 0 or > 1)
                return false;

            var expected = operation == 0
                ? left + right
                : left - right;
            if (expected < 0 || value != expected)
                return false;

            return family switch
            {
                "number.whole.add_subtract.within_10.build" or
                "number.whole.add_subtract.within_10.apply" =>
                    left <= 10 && right <= 10 && expected <= 10,

                "number.whole.add_subtract.across_ten.build" or
                "number.whole.add_subtract.across_ten.apply" =>
                    CrossesTen(left, right, operation, expected),

                "number.whole.add_subtract.within_100.build" or
                "number.whole.add_subtract.within_100.apply" =>
                    left <= 100 && right <= 100 && expected <= 100,

                "number.whole.add_subtract.columnar.build" or
                "number.whole.add_subtract.columnar.apply" =>
                    left is >= 10 and <= 99 &&
                    right is >= 10 and <= 99 &&
                    expected <= 198 &&
                    RequiresRegrouping(left, right, operation),

                _ => false
            };
        }

        return family switch
        {
            "number.whole.add.direct" =>
                parameters["left"] >= 0 &&
                parameters["right"] >= 0 &&
                value == parameters["left"] + parameters["right"],

            "number.whole.subtract.direct" =>
                parameters["left"] >= parameters["right"] &&
                parameters["right"] >= 0 &&
                value == parameters["left"] - parameters["right"],

            "number.lcm.two_numbers" =>
                parameters["left"] > 0 &&
                parameters["right"] > 0 &&
                value == LeastCommonMultiple(parameters["left"], parameters["right"]),

            "percentages.of_quantity.direct" or
            "percentages.core.of_quantity" =>
                parameters["percent"] is >= 0 and <= 100 &&
                parameters["quantity"] >= 0 &&
                parameters["percent"] * parameters["quantity"] % 100 == 0 &&
                value * 100 == parameters["percent"] * parameters["quantity"],

            "percentages.core.increase_decrease" =>
                parameters["percent"] is > 0 and <= 100 &&
                parameters["quantity"] >= 0 &&
                parameters["direction"] is 1 or -1 &&
                parameters["quantity"] * (100 + parameters["direction"] * parameters["percent"]) % 100 == 0 &&
                value * 100 ==
                    parameters["quantity"] * (100 + parameters["direction"] * parameters["percent"]),

            "usccss.g11.alg2.u01.l01.affine_recurrence" =>
                parameters["n"] >= 1 && parameters["n"] <= 8 &&
                parameters["multiplier"] is 2 or -2 &&
                parameters["offset"] is 1 or -1 or 3 &&
                value == G11AffineRecurrenceTerm(parameters["first"], parameters["multiplier"], parameters["offset"], parameters["n"]),

            "usccss.g11.alg2.u01.l01.hanoi_moves" =>
                parameters["n"] is >= 1 and <= 12 &&
                value == G11HanoiMinimalMoves(parameters["n"]),

            "usccss.g11.alg2.u01.l01.fibonacci_term" =>
                parameters["n"] is >= 1 and <= 20 &&
                value == G11FibonacciTerm(parameters["n"]),

            "usccss.g11.alg2.u01.l02.paper_piece_count" =>
                parameters["cutCount"] is >= 0 and <= 9 &&
                value == (1 << parameters["cutCount"]),

            "sequences.core.arithmetic_nth_term" =>
                parameters["n"] > 0 &&
                value == parameters["first"] + (parameters["n"] - 1) * parameters["difference"],

            "sequences.core.geometric_nth_term" =>
                parameters["n"] > 0 &&
                parameters["ratio"] != 0 &&
                value == parameters["first"] * IntegerPower(parameters["ratio"], parameters["n"] - 1),

            "sequences.core.find_position_arithmetic" =>
                parameters["difference"] != 0 &&
                parameters["target"] == parameters["first"] +
                    (parameters["expectedN"] - 1) * parameters["difference"] &&
                value == parameters["expectedN"],

            "ratio.unit_rate.divide_total" =>
                parameters["firstPart"] > 0 &&
                parameters["secondPart"] > 0 &&
                parameters["total"] > 0 &&
                parameters["total"] % (parameters["firstPart"] + parameters["secondPart"]) == 0 &&
                value == parameters["total"] / (parameters["firstPart"] + parameters["secondPart"]) *
                    parameters["askedPart"],

            "statistics.center_spread.mean" =>
                parameters["count"] == 5 &&
                value * 5 ==
                    parameters["v1"] + parameters["v2"] + parameters["v3"] +
                    parameters["v4"] + parameters["v5"],

            "statistics.center_spread.median" =>
                parameters["v1"] <= parameters["v2"] &&
                parameters["v2"] <= parameters["v3"] &&
                parameters["v3"] <= parameters["v4"] &&
                parameters["v4"] <= parameters["v5"] &&
                value == parameters["v3"],

            "statistics.center_spread.range" =>
                parameters["max"] >= parameters["min"] &&
                value == parameters["max"] - parameters["min"],

            "geometry.coordinate.evaluate_linear_rule" =>
                value == parameters["gradient"] * parameters["x"] + parameters["intercept"],

            "geometry.coordinate.y_intercept_from_rule" =>
                value == parameters["intercept"],

            "algebra.relationships.two_unknowns.total_difference" =>
                value > 0 &&
                parameters["total"] - value > 0 &&
                value + (parameters["total"] - value) == parameters["total"] &&
                (parameters["total"] - value) - value == parameters["difference"],

            "measurement.scale.equal_intervals.read_value" =>
                parameters["intervals"] > 0 &&
                (parameters["end"] - parameters["start"]) % parameters["intervals"] == 0 &&
                value == parameters["start"] +
                    ((parameters["end"] - parameters["start"]) / parameters["intervals"]) * parameters["pointer"] &&
                parameters["start"] +
                    ((parameters["end"] - parameters["start"]) / parameters["intervals"]) * parameters["intervals"] ==
                    parameters["end"],

            "fractions.equivalent.missing_value" or
            "fractions.equivalent.recognize" or
            "fractions.equivalent.generate_multiple" or
            "fractions.equivalent.number_line" =>
                parameters["baseD"] > 0 &&
                parameters["targetD"] > 0 &&
                parameters["baseN"] * parameters["targetD"] == value * parameters["baseD"],

            "fractions.equivalent.reduce_common_factor" =>
                parameters["largeD"] > 0 &&
                parameters["targetD"] > 0 &&
                parameters["largeN"] * parameters["targetD"] == value * parameters["largeD"],

            "ratio.unit_rate.direct" =>
                parameters["quantity"] > 0 &&
                value * parameters["quantity"] == parameters["total"],

            "ratio.unit_rate.equivalent_ratio" =>
                parameters["baseSecond"] > 0 &&
                parameters["targetSecond"] > 0 &&
                parameters["baseFirst"] * parameters["targetSecond"] ==
                    value * parameters["baseSecond"],

            "number.whole.add_subtract.comparative.build" or
            "number.whole.add_subtract.comparative.apply" =>
                parameters["base"] >= 0 &&
                parameters["difference"] >= 0 &&
                value == parameters["base"] + parameters["difference"],

            "number.whole.add_subtract.complement_100.build" or
            "number.whole.add_subtract.complement_100.apply" =>
                parameters["known"] is >= 0 and <= 100 &&
                value + parameters["known"] == 100,

            "number.whole.multiply.fact_recall.build" or
            "number.whole.multiply.fact_recall.apply" =>
                parameters["left"] is >= 0 and <= 12 &&
                parameters["right"] is >= 0 and <= 12 &&
                value == parameters["left"] * parameters["right"],

            "fractions.of_quantity.build" or
            "fractions.of_quantity.apply" =>
                parameters["denominator"] > 0 &&
                parameters["numerator"] > 0 &&
                parameters["numerator"] <= parameters["denominator"] &&
                parameters["quantity"] % parameters["denominator"] == 0 &&
                value == (parameters["quantity"] / parameters["denominator"]) * parameters["numerator"],

            ExactLinearEquationQuestionFactory.FamilyId =>
                parameters["coefficient"] != 0 &&
                (parameters["right"] - parameters["offset"]) % parameters["coefficient"] == 0 &&
                value == (parameters["right"] - parameters["offset"]) / parameters["coefficient"],

            "algebra.linear.variables_both_sides" =>
                parameters["leftCoefficient"] != parameters["rightCoefficient"] &&
                (parameters["rightOffset"] - parameters["leftOffset"]) %
                    (parameters["leftCoefficient"] - parameters["rightCoefficient"]) == 0 &&
                value == (parameters["rightOffset"] - parameters["leftOffset"]) /
                    (parameters["leftCoefficient"] - parameters["rightCoefficient"]),

            "geometry.coordinate.gradient_between_points" =>
                parameters["x2"] != parameters["x1"] &&
                (parameters["y2"] - parameters["y1"]) % (parameters["x2"] - parameters["x1"]) == 0 &&
                value == (parameters["y2"] - parameters["y1"]) /
                    (parameters["x2"] - parameters["x1"]),

            "geometry.angles.parallel_lines" =>
                value == parameters["expectedAngle"] &&
                value is > 0 and < 180,

            "geometry.angles.supplementary" =>
                value == 180 - parameters["knownAngle"] &&
                parameters["knownAngle"] is > 0 and < 180,

            "geometry.angles.algebraic_supplementary" =>
                parameters["coefficientA"] > 0 &&
                parameters["coefficientB"] > 0 &&
                value == parameters["expectedX"] &&
                (parameters["coefficientA"] + parameters["coefficientB"]) * value +
                    parameters["offsetA"] + parameters["offsetB"] == 180,

            "geometry.similarity.find_missing_length" =>
                parameters["scaleFactor"] > 0 &&
                value == parameters["sourceLength"] * parameters["scaleFactor"],

            "geometry.similarity.scale_factor" =>
                parameters["sourceLength"] > 0 &&
                parameters["targetLength"] % parameters["sourceLength"] == 0 &&
                value == parameters["targetLength"] / parameters["sourceLength"],

            "geometry.surface_area.rectangular_prism" or
            "geometry.surface_area_volume.rectangular_prism_surface_area" =>
                value == 2 * (
                    parameters["length"] * parameters["width"] +
                    parameters["length"] * parameters["height"] +
                    parameters["width"] * parameters["height"]),

            "geometry.volume.rectangular_prism" or
            "geometry.surface_area_volume.rectangular_prism_volume" =>
                value == parameters["length"] *
                    parameters["width"] *
                    parameters["height"],

            "geometry.rectangle.area.exact" or
            "geometry.perimeter_area.rectangle_area" =>
                value == parameters["length"] * parameters["width"],

            "geometry.rectangle.perimeter.exact" or
            "geometry.perimeter_area.rectangle_perimeter" =>
                value == 2 * (parameters["length"] + parameters["width"]),

            "geometry.perimeter_area.triangle_area" =>
                parameters["base"] > 0 &&
                parameters["height"] > 0 &&
                parameters["base"] * parameters["height"] % 2 == 0 &&
                value * 2 == parameters["base"] * parameters["height"],

            "usccss.g4.u06.l15.area_partial_quotients" or
            "usccss.g4.u06.l15.mural_complete_columns" =>
                parameters.TryGetValue("knownSide", out var lesson101Width) &&
                parameters.TryGetValue("area", out var lesson101Area) &&
                parameters.TryGetValue("leftover", out var lesson101Leftover) &&
                lesson101Width is >= 2 and <= 9 &&
                lesson101Area is >= 100 and <= 999 &&
                lesson101Leftover >= 0 && lesson101Leftover < lesson101Width &&
                (family == "usccss.g4.u06.l15.area_partial_quotients" ? lesson101Leftover == 0 : lesson101Leftover > 0) &&
                lesson101Area % lesson101Width == lesson101Leftover &&
                value >= 0 && (long)value * lesson101Width + lesson101Leftover == lesson101Area,

            "usccss.g4.u06.l16.base_ten_equal_groups" =>
                parameters.TryGetValue("dividend",out var n102) &&
                parameters.TryGetValue("divisor",out var d102) &&
                parameters.TryGetValue("tensAfterTrade",out var t102) &&
                parameters.TryGetValue("onesAfterTrade",out var o102) &&
                d102>=2 && d102<=9 && n102>=24 && n102<=999 &&
                t102==n102/10%10+10*((n102/100)%d102) &&
                o102==n102%10+10*(t102%d102) && o102%d102==0 &&
                n102%d102==0 && value>0 && (long)value*d102==n102,

            "usccss.g4.u06.l18.partial_quotients_recording" =>
                parameters.TryGetValue("dividend", out var partialDividend) &&
                parameters.TryGetValue("divisor", out var partialDivisor) &&
                parameters.TryGetValue("tensQuotient", out var partialTens) &&
                parameters.TryGetValue("onesQuotient", out var partialOnes) &&
                partialDividend is >= 100 and <= 999 &&
                partialDivisor is >= 2 and <= 9 &&
                partialTens >= 10 && partialTens % 10 == 0 &&
                partialOnes is >= 0 and <= 9 &&
                (long)partialDivisor * (partialTens + partialOnes) == partialDividend &&
                value == partialTens + partialOnes,

            "usccss.g4.u06.l22.convert_l_shaped_area_perimeter" =>
                parameters.TryGetValue("caseType", out var l108Mode) &&
                parameters.TryGetValue("sheetWidthInches", out var l108SheetW) &&
                parameters.TryGetValue("sheetHeightInches", out var l108SheetH) &&
                parameters.TryGetValue("bannerHeightInches", out var l108BannerH) &&
                parameters.TryGetValue("bannerLengthFeet", out var l108BannerF) &&
                parameters.TryGetValue("leftWidthFeet", out var l108LeftW) &&
                parameters.TryGetValue("rightWidthYards", out var l108RightW) &&
                parameters.TryGetValue("lowerHeightYards", out var l108LowerH) &&
                parameters.TryGetValue("upperRiseFeet", out var l108Rise) &&
                parameters.TryGetValue("suppliedInches", out var l108Supply) &&
                l108Mode is >= 0 and <= 4 &&
                l108SheetW is >= 36 and <= 49 &&
                l108SheetH is >= 24 and <= 34 &&
                l108BannerH == 8 &&
                l108BannerF is >= 6 and <= 8 &&
                l108LeftW is >= 8 and <= 13 &&
                l108RightW is >= 2 and <= 4 &&
                l108LowerH is >= 3 and <= 5 &&
                l108Rise is >= 2 and <= 6 &&
                l108Supply > 24 * (l108LeftW + 3 * l108RightW + 3 * l108LowerH + l108Rise) &&
                l108Supply <= 24 * (l108LeftW + 3 * l108RightW + 3 * l108LowerH + l108Rise) + 500 &&
                value == (l108Mode switch
                {
                    0 => l108SheetW * l108SheetH - (l108BannerF * 12 * l108BannerH),
                    1 => 2 * (l108LeftW + 3 * l108RightW + 3 * l108LowerH + l108Rise),
                    2 => 24 * (l108LeftW + 3 * l108RightW + 3 * l108LowerH + l108Rise),
                    3 => (l108LeftW + 3 * l108RightW) * (3 * l108LowerH) + l108LeftW * l108Rise,
                    4 => l108Supply - 24 * (l108LeftW + 3 * l108RightW + 3 * l108LowerH + l108Rise),
                    _ => -1
                }),




            "usccss.g4.u09.l05.two_digit_partial_products" =>
                parameters.TryGetValue("caseType", out var c142) &&
                parameters.TryGetValue("variant", out var v142) &&
                parameters.TryGetValue("multiplicand", out var a142) &&
                parameters.TryGetValue("multiplier", out var b142) &&
                parameters.TryGetValue("product", out var p142) &&
                c142 is >= 0 and <= 7 &&
                v142 is >= 0 and <= 2 &&
                (a142,b142)==(c142 switch
                {
                    0 => (34,21), 1 => (23,14),
                    2 => (46,32), 3 => (72,15),
                    4 => (58,24), 5 => (16,43),
                    6 => (29,31), 7 => (87,12),
                    _ => (-1,-1)
                }) &&
                p142 == a142*b142 &&
                value == p142,

            "usccss.g4.u08.l07.polygon_perimeter_sum" =>
                parameters.TryGetValue("caseType", out var c134) &&
                parameters.TryGetValue("variant", out var v134) &&
                parameters.TryGetValue("sideA", out var a134) &&
                parameters.TryGetValue("sideB", out var b134) &&
                parameters.TryGetValue("sideC", out var d134) &&
                parameters.TryGetValue("sideD", out var e134) &&
                parameters.TryGetValue("perimeter", out var p134) &&
                c134 is >= 0 and <= 7 &&
                v134 is >= 0 and <= 2 &&
                (a134,b134,d134,e134)==(c134 switch
                {
                    0 => (18,26,13,37), 1 => (7,7,7,0),
                    2 => (4,9,4,9), 3 => (3,5,8,12),
                    4 => (15,15,15,15), 5 => (12,17,19,0),
                    6 => (8,6,8,6), 7 => (25,10,15,20),
                    _ => (-1,-1,-1,-1)
                }) &&
                p134 == a134+b134+d134+e134 &&
                value == p134,

            "usccss.g4.u08.l04.symmetry_axis_counts" =>
                parameters.TryGetValue("caseType", out var c131) &&
                parameters.TryGetValue("variant", out var v131) &&
                parameters.TryGetValue("axisCount", out var a131) &&
                c131 is >= 0 and <= 7 &&
                v131 is >= 0 and <= 2 &&
                a131 == (c131 switch
                {
                    0 => 4, 1 => 2, 2 => 3, 3 => 1,
                    4 => 0, 5 => 2, 6 => 1, 7 => 0, _ => -1
                }) &&
                value == a131,

            "usccss.g4.u07.l13.additive_angle_part" =>
                parameters.TryGetValue("caseType", out var c124) &&
                parameters.TryGetValue("variant", out var v124) &&
                parameters.TryGetValue("totalDegrees", out var t124) &&
                parameters.TryGetValue("knownDegrees", out var k124) &&
                parameters.TryGetValue("unknownDegrees", out var u124) &&
                c124 is >= 0 and <= 7 &&
                v124 is >= 0 and <= 2 &&
                (t124, k124) == (c124 switch
                {
                    0 => (90,62), 1 => (180,73),
                    2 => (360,120), 3 => (90,34),
                    4 => (180,120), 5 => (360,275),
                    6 => (90,45), 7 => (180,95),
                    _ => (-1,-1)
                }) &&
                u124 == t124-k124 &&
                u124 > 0 &&
                value == u124,

            "usccss.g4.u07.l12.classify_angles_by_degree_bounds" =>
                parameters.TryGetValue("caseType", out var l123Case) &&
                parameters.TryGetValue("variant", out var l123Variant) &&
                parameters.TryGetValue("degrees", out var l123Degrees) &&
                parameters.TryGetValue("classCode", out var l123Class) &&
                l123Case is >= 0 and <= 7 &&
                l123Variant is >= 0 and <= 2 &&
                l123Degrees == (l123Case switch
                {
                    0 => 20, 1 => 35, 2 => 45, 3 => 70,
                    4 => 90, 5 => 110, 6 => 130, 7 => 180,
                    _ => -1
                }) &&
                l123Class == (l123Degrees < 90 ? 1 :
                    l123Degrees == 90 ? 2 : l123Degrees < 180 ? 3 : 4) &&
                value == l123Class,

            "usccss.g4.u07.l10.perpendicular_right_angle_judgment" =>
                parameters.TryGetValue("caseType", out var l121Case) &&
                parameters.TryGetValue("variant", out var l121Variant) &&
                parameters.TryGetValue("angleDegrees", out var l121Degrees) &&
                parameters.TryGetValue("perpendicularAnswer", out var l121Answer) &&
                l121Case is >= 0 and <= 7 &&
                l121Variant is >= 0 and <= 2 &&
                l121Degrees == (l121Case switch
                {
                    0 or 1 or 2 or 3 => 90,
                    4 => 75, 5 => 80, 6 => 100, 7 => 120,
                    _ => -1
                }) &&
                l121Answer == (l121Degrees == 90 ? 1 : 2) &&
                value == l121Answer,

            "usccss.g4.u07.l09.protractor_dual_scale_reading" =>
                parameters.TryGetValue("caseType", out var l120Case) &&
                parameters.TryGetValue("variant", out var l120Variant) &&
                parameters.TryGetValue("zeroScale", out var l120ZeroScale) &&
                parameters.TryGetValue("outerReading", out var l120Outer) &&
                parameters.TryGetValue("innerReading", out var l120Inner) &&
                parameters.TryGetValue("targetDegrees", out var l120Target) &&
                l120Case is >= 0 and <= 7 &&
                l120Variant is >= 0 and <= 2 &&
                l120ZeroScale == (l120Variant == 1 ? 1 : 0) &&
                l120Outer is >= 1 and <= 179 &&
                l120Inner is >= 1 and <= 179 &&
                l120Outer + l120Inner == 180 &&
                l120Target == (l120Case switch
                {
                    0 => 25, 1 => 45, 2 => 65, 3 => 80,
                    4 => 100, 5 => 115, 6 => 140, 7 => 160,
                    _ => -1
                }) &&
                (l120ZeroScale == 0 ? l120Outer : l120Inner) == l120Target &&
                value == l120Target,

            "usccss.g4.u07.l08.full_turn_degree_benchmarks" =>
                parameters.TryGetValue("caseType", out var l119Mode) &&
                parameters.TryGetValue("variant", out var l119Variant) &&
                parameters.TryGetValue("questionKind", out var l119Kind) &&
                parameters.TryGetValue("degrees", out var l119Deg) &&
                parameters.TryGetValue("numerator", out var l119Num) &&
                parameters.TryGetValue("denominator", out var l119Den) &&
                l119Mode is >= 0 and <= 7 &&
                l119Variant is >= 0 and <= 2 &&
                l119Kind is >= 0 and <= 2 &&
                l119Deg is >= 1 and <= 360 &&
                l119Num is >= 1 and <= 3 &&
                l119Den is >= 1 and <= 8 &&
                l119Deg * l119Den == 360 * l119Num &&
                (l119Mode switch
                {
                    0 => l119Deg == 45 && l119Num == 1 && l119Den == 8,
                    1 => l119Deg == 60 && l119Num == 1 && l119Den == 6,
                    2 => l119Deg == 90 && l119Num == 1 && l119Den == 4,
                    3 => l119Deg == 120 && l119Num == 1 && l119Den == 3,
                    4 => l119Deg == 180 && l119Num == 1 && l119Den == 2,
                    5 => l119Deg == 240 && l119Num == 2 && l119Den == 3,
                    6 => l119Deg == 270 && l119Num == 3 && l119Den == 4,
                    7 => l119Deg == 360 && l119Num == 1 && l119Den == 1,
                    _ => false
                }) &&
                value == (l119Kind == 0 ? l119Num :
                    l119Kind == 1 ? l119Den : 360 - l119Deg),

            "usccss.g4.u07.l07.clock_hour_angle_turn_compare" =>
                parameters.TryGetValue("caseType", out var l118Case) &&
                parameters.TryGetValue("questionKind", out var l118Kind) &&
                parameters.TryGetValue("variant", out var l118Variant) &&
                parameters.TryGetValue("hourA", out var l118HourA) &&
                parameters.TryGetValue("hourB", out var l118HourB) &&
                parameters.TryGetValue("stepA", out var l118StepA) &&
                parameters.TryGetValue("stepB", out var l118StepB) &&
                l118Case is >= 0 and <= 7 &&
                l118Kind is 0 or 1 &&
                l118Variant is >= 0 and <= 2 &&
                l118HourA is >= 1 and <= 11 &&
                l118HourB is >= 1 and <= 11 &&
                l118StepA == Math.Min(l118HourA, 12 - l118HourA) &&
                l118StepB == Math.Min(l118HourB, 12 - l118HourB) &&
                (l118Case switch
                {
                    0 => l118HourA == 3 && l118HourB == 5,
                    1 => l118HourA == 8 && l118HourB == 2,
                    2 => l118HourA == 3 && l118HourB == 9,
                    3 => l118HourA == 6 && l118HourB == 5,
                    4 => l118HourA == 10 && l118HourB == 4,
                    5 => l118HourA == 7 && l118HourB == 8,
                    6 => l118HourA == 1 && l118HourB == 11,
                    7 => l118HourA == 4 && l118HourB == 6,
                    _ => false
                }) &&
                value == (l118Kind == 0
                    ? (l118StepA > l118StepB ? 1 : l118StepA < l118StepB ? 2 : 3)
                    : Math.Abs(l118StepA - l118StepB)),

            "usccss.g4.u07.l06.compare_angle_openings" =>
                parameters.TryGetValue("caseType", out var l117Mode) &&
                parameters.TryGetValue("variant", out var l117Variant) &&
                parameters.TryGetValue("start1", out var l117Start1) &&
                parameters.TryGetValue("end1", out var l117End1) &&
                parameters.TryGetValue("start2", out var l117Start2) &&
                parameters.TryGetValue("end2", out var l117End2) &&
                parameters.TryGetValue("steps1", out var l117Steps1) &&
                parameters.TryGetValue("steps2", out var l117Steps2) &&
                l117Mode is >= 0 and <= 7 &&
                l117Variant is >= 0 and <= 2 &&
                l117Start1 == 2 * l117Variant &&
                l117Start2 == (l117Start1 + 2) % 8 &&
                l117Steps1 is >= 1 and <= 4 &&
                l117Steps2 is >= 1 and <= 4 &&
                l117End1 == (l117Start1 + l117Steps1) % 8 &&
                l117End2 == (l117Start2 + l117Steps2) % 8 &&
                (l117Mode switch
                {
                    0 => l117Steps1 == 1 && l117Steps2 == 2,
                    1 => l117Steps1 == 3 && l117Steps2 == 1,
                    2 => l117Steps1 == 2 && l117Steps2 == 2,
                    3 => l117Steps1 == 4 && l117Steps2 == 3,
                    4 => l117Steps1 == 1 && l117Steps2 == 4,
                    5 => l117Steps1 == 3 && l117Steps2 == 2,
                    6 => l117Steps1 == 4 && l117Steps2 == 4,
                    7 => l117Steps1 == 2 && l117Steps2 == 3,
                    _ => false
                }) &&
                value == (l117Steps1 > l117Steps2 ? 1 : l117Steps1 < l117Steps2 ? 2 : 3),

            "usccss.g4.u07.l05.angle_common_vertex_rays" =>
                parameters.TryGetValue("caseType", out var l116Case) &&
                parameters.TryGetValue("variant", out var l116Variant) &&
                parameters.TryGetValue("start1", out var l116Start1) &&
                parameters.TryGetValue("start2", out var l116Start2) &&
                parameters.TryGetValue("direction1", out var l116Dir1) &&
                parameters.TryGetValue("direction2", out var l116Dir2) &&
                l116Case is >= 0 and <= 7 &&
                l116Variant is >= 0 and <= 2 &&
                l116Start1 is >= 0 and <= 3 &&
                l116Start2 is >= 0 and <= 3 &&
                l116Dir1 is >= 0 and <= 7 &&
                l116Dir2 is >= 0 and <= 7 &&
                l116Dir1 != l116Dir2 &&
                (l116Start1 == l116Start2) == (l116Case < 4) &&
                value == (l116Start1 == l116Start2 ? 1 : 2),

            "usccss.g4.u07.l03.dot_grid_parallel_intersect" =>
                parameters.TryGetValue("caseType", out var l114Case) &&
                parameters.TryGetValue("variant", out var l114Variant) &&
                parameters.TryGetValue("r1", out var l114R1) &&
                parameters.TryGetValue("c1", out var l114C1) &&
                parameters.TryGetValue("r2", out var l114R2) &&
                parameters.TryGetValue("c2", out var l114C2) &&
                parameters.TryGetValue("r3", out var l114R3) &&
                parameters.TryGetValue("c3", out var l114C3) &&
                parameters.TryGetValue("r4", out var l114R4) &&
                parameters.TryGetValue("c4", out var l114C4) &&
                l114Case is >= 0 and <= 7 &&
                l114Variant is >= 0 and <= 2 &&
                l114R1 is >= 1 and <= 5 && l114R2 is >= 1 and <= 5 &&
                l114R3 is >= 1 and <= 5 && l114R4 is >= 1 and <= 5 &&
                l114C1 is >= 1 and <= 5 && l114C2 is >= 1 and <= 5 &&
                l114C3 is >= 1 and <= 5 && l114C4 is >= 1 and <= 5 &&
                (l114R1 != l114R2 || l114C1 != l114C2) &&
                (l114R3 != l114R4 || l114C3 != l114C4) &&
                // Exact direction cross-product: equal zero means parallel.
                ((l114R2 - l114R1) * (l114C4 - l114C3) ==
                 (l114C2 - l114C1) * (l114R4 - l114R3)) == (l114Case < 4) &&
                // Coincident lines do not count as two distinct parallel lines.
                (l114Case >= 4 ||
                 (l114R2 - l114R1) * (l114C3 - l114C1) !=
                 (l114C2 - l114C1) * (l114R3 - l114R1)) &&
                value == (l114Case < 4 ? 1 : 2),

            "usccss.g4.u07.l02.geometry_cards_number_talk" =>
                parameters.TryGetValue("caseType", out var l113Case) &&
                parameters.TryGetValue("variant", out var l113Variant) &&
                parameters.TryGetValue("first", out var l113A) &&
                parameters.TryGetValue("second", out var l113B) &&
                l113Case is >= 0 and <= 7 &&
                l113Variant is >= 0 and <= 2 &&
                (l113Case switch
                {
                    0 or 1 or 2 or 3 => l113A == 0 && l113B == 0 &&
                        value == l113Case + 1,
                    4 => l113A == 90 && l113B == 45 && value == 45,
                    5 => l113A == 270 && l113B == 45 && value == 225,
                    6 => l113A == 270 && l113B == 135 && value == 135,
                    7 => l113A == 360 && l113B == 135 && value == 225,
                    _ => false
                }),

            "usccss.g4.u07.l01.geometric_description_precision" =>
                parameters.TryGetValue("caseType", out var l112Case) &&
                parameters.TryGetValue("preciseFirst", out var l112First) &&
                l112Case is >= 0 and <= 5 &&
                l112First is 0 or 1 &&
                value == (l112First == 1 ? 1 : 2),

            "usccss.g4.u06.l25.flower_garland_patterns" =>
                parameters.TryGetValue("caseType", out var l111Case) &&
                parameters.TryGetValue("garlandsEach", out var l111Pairs) &&
                parameters.TryGetValue("flowersMade", out var l111Flowers) &&
                parameters.TryGetValue("flowersPerGarland", out var l111GarlandSize) &&
                parameters.TryGetValue("priyaBig", out var l111Pb) &&
                parameters.TryGetValue("priyaSmall", out var l111Ps) &&
                parameters.TryGetValue("jadaBig", out var l111Jb) &&
                parameters.TryGetValue("jadaSmall", out var l111Js) &&
                parameters.TryGetValue("cutMinutes", out var l111Cut) &&
                parameters.TryGetValue("finishMinutes", out var l111Finish) &&
                parameters.TryGetValue("workers", out var l111Workers) &&
                l111Case is >= 0 and <= 5 &&
                l111Pairs is >= 10 and <= 40 &&
                l111Flowers is >= 120 and <= 250 &&
                l111GarlandSize == 16 &&
                l111Pb == 6 && l111Ps == 6 &&
                l111Jb == 4 && l111Js == 8 &&
                l111Cut == 1 && l111Finish == 2 &&
                l111Workers == 2 &&
                value == (l111Case switch
                {
                    0 => l111Pairs * (l111Pb + l111Jb),
                    1 => l111Pairs * (l111Ps + l111Js),
                    2 => l111Flowers / l111GarlandSize,
                    3 => l111Flowers % l111GarlandSize,
                    4 => l111Flowers * (l111Cut + l111Finish),
                    5 => ((l111Flowers + l111Workers - 1) / l111Workers) * (l111Cut + l111Finish),
                    _ => -1
                }),

            "usccss.g4.u06.l24.language_population_comparisons" =>
                parameters.TryGetValue("caseType", out var l110Case) &&
                parameters.TryGetValue("navajo", out var l110Navajo) &&
                parameters.TryGetValue("yupik", out var l110Yupik) &&
                parameters.TryGetValue("dakota", out var l110Dakota) &&
                parameters.TryGetValue("apache", out var l110Apache) &&
                parameters.TryGetValue("keres", out var l110Keres) &&
                parameters.TryGetValue("cherokee", out var l110Cherokee) &&
                parameters.TryGetValue("ojibwa", out var l110Ojibwa) &&
                parameters.TryGetValue("choctaw", out var l110Choctaw) &&
                parameters.TryGetValue("zuni", out var l110Zuni) &&
                parameters.TryGetValue("pima", out var l110Pima) &&
                parameters.TryGetValue("philadelphiaEnglish", out var l110Eng) &&
                parameters.TryGetValue("philadelphiaSpanish", out var l110PhSp) &&
                parameters.TryGetValue("philadelphiaIndo", out var l110PhIn) &&
                parameters.TryGetValue("philadelphiaAsian", out var l110PhAs) &&
                parameters.TryGetValue("chicagoSpanish", out var l110ChSp) &&
                parameters.TryGetValue("chicagoIndo", out var l110ChIn) &&
                l110Case is >= 0 and <= 5 &&
                l110Navajo is >= 135000 and <= 190000 &&
                l110Yupik is >= 17500 and <= 23500 &&
                l110Dakota is >= 15000 and <= 19000 &&
                l110Apache is >= 11000 and <= 16000 &&
                l110Keres is >= 11000 and <= 14000 &&
                l110Cherokee is >= 9000 and <= 13000 &&
                l110Ojibwa is >= 8000 and <= 11000 &&
                l110Choctaw is >= 8000 and <= 11000 &&
                l110Zuni is >= 8000 and <= 11000 &&
                l110Pima is >= 6000 and <= 8000 &&
                l110Eng is >= 1100000 and <= 1350000 &&
                l110PhSp is >= 110000 and <= 150000 &&
                l110PhIn is >= 5000 and <= 9000 &&
                l110PhAs is >= 200 and <= 600 &&
                l110ChSp is >= 395000 and <= 460000 &&
                l110ChIn is >= 20000 and <= 32000 &&
                l110Navajo > l110Yupik + l110Dakota + l110Apache + l110Keres +
                              l110Cherokee + l110Ojibwa + l110Choctaw + l110Zuni + l110Pima &&
                l110Eng > l110PhSp + l110PhIn + l110PhAs &&
                l110ChSp + l110ChIn > l110PhSp + l110PhIn &&
                value == (l110Case switch
                {
                    0 => l110Navajo - l110Yupik,
                    1 => l110Navajo + l110Apache + l110Cherokee,
                    2 => l110Navajo - (l110Yupik + l110Dakota + l110Apache + l110Keres +
                                        l110Cherokee + l110Ojibwa + l110Choctaw + l110Zuni + l110Pima),
                    3 => l110Eng - l110PhSp - l110PhIn - l110PhAs,
                    4 => l110ChSp + l110ChIn - l110PhSp - l110PhIn,
                    5 => (2 * l110Navajo + l110Yupik) / (2 * l110Yupik),
                    _ => -1
                }),

            "usccss.g4.u06.l23.four_operation_contexts" =>
                parameters.TryGetValue("caseType", out var l109Mode) &&
                parameters.TryGetValue("segmentA", out var l109A) &&
                parameters.TryGetValue("segmentB", out var l109B) &&
                parameters.TryGetValue("segmentC", out var l109C) &&
                parameters.TryGetValue("segmentD", out var l109D) &&
                parameters.TryGetValue("longTrips", out var l109LongTrips) &&
                parameters.TryGetValue("musicTrips", out var l109MusicTrips) &&
                parameters.TryGetValue("musicTotalFeet", out var l109Music) &&
                parameters.TryGetValue("feetPerMile", out var l109FeetPerMile) &&
                parameters.TryGetValue("days", out var l109Days) &&
                parameters.TryGetValue("week1", out var l109W1) &&
                parameters.TryGetValue("week2", out var l109W2) &&
                parameters.TryGetValue("week3", out var l109W3) &&
                parameters.TryGetValue("stepTarget", out var l109Target) &&
                l109Mode is >= 0 and <= 5 &&
                l109A is >= 100 and <= 200 &&
                l109B is >= 100 and <= 200 &&
                l109C is >= 100 and <= 200 &&
                l109D is >= 100 and <= 200 &&
                l109LongTrips == 5 && l109MusicTrips == 3 &&
                l109Music is >= 1200 and <= 2400 &&
                l109Music % (2 * l109MusicTrips) == 0 &&
                l109FeetPerMile == 5280 &&
                l109Days == 7 &&
                l109W1 >= 26000 && l109W1 <= 35000 && l109W1 % 7 == 0 &&
                l109W2 > 0 && l109W2 < l109W1 &&
                l109W1 - l109W2 is >= 2000 and <= 6000 &&
                l109W3 is >= 32000 and <= 38000 &&
                l109Target == 120000 &&
                l109LongTrips * 2 * (l109A + l109B + l109C + l109D) + l109Music < 2 * l109FeetPerMile &&
                l109W1 + l109W2 + l109W3 < l109Target &&
                value == (l109Mode switch
                {
                    0 => 2 * (l109A + l109B + l109C + l109D),
                    1 => l109Music / (2 * l109MusicTrips),
                    2 => 2 * l109FeetPerMile - (l109LongTrips * 2 * (l109A + l109B + l109C + l109D) + l109Music),
                    3 => l109W1 / l109Days,
                    4 => l109W1 - l109W2,
                    5 => l109Target - l109W1 - l109W2 - l109W3,
                    _ => -1
                }),

            "usccss.g4.u06.l21.multistep_contexts" =>
                parameters.TryGetValue("caseType", out var l107Mode) &&
                parameters.TryGetValue("count", out var l107Count) &&
                parameters.TryGetValue("ticketPrice", out var l107Price) &&
                parameters.TryGetValue("budget", out var l107Budget) &&
                parameters.TryGetValue("days", out var l107Days) &&
                parameters.TryGetValue("drinkPrice", out var l107Drink) &&
                parameters.TryGetValue("popcornPrice", out var l107Popcorn) &&
                parameters.TryGetValue("dailyRevenue", out var l107Revenue) &&
                l107Mode is >= 0 and <= 2 &&
                l107Count is >= 25 and <= 390 &&
                l107Price is >= 5 and <= 25 &&
                l107Days == 2 &&
                l107Drink is >= 5 and <= 12 &&
                l107Popcorn is >= 2 and <= 9 &&
                (long)l107Count * l107Price == l107Revenue &&
                l107Budget > l107Revenue &&
                l107Budget - l107Revenue <= 300 &&
                value == (l107Mode switch
                {
                    0 => l107Budget - l107Revenue,
                    1 => l107Count * l107Days,
                    2 => l107Count * l107Days * (l107Drink + l107Popcorn),
                    _ => -1
                }),

            "usccss.g4.u06.l20.contextual_remainder_decisions" =>
                parameters.TryGetValue("caseType", out var l106Case) &&
                parameters.TryGetValue("dividend", out var l106Total) &&
                parameters.TryGetValue("divisor", out var l106Divisor) &&
                parameters.TryGetValue("shortfall", out var l106Short) &&
                l106Case is >= 0 and <= 4 &&
                l106Total is >= 100 and <= 9999 &&
                l106Divisor is >= 2 and <= 9 &&
                l106Short >= 0 && l106Short < l106Divisor &&
                (l106Case switch
                {
                    0 or 1 or 2 => l106Short == 0 && l106Total % l106Divisor > 0,
                    3 => l106Short > 0 && l106Total % l106Divisor == l106Short,
                    4 => l106Short == 0 && l106Total % l106Divisor == 0,
                    _ => false
                }) &&
                value == (l106Case switch
                {
                    0 => l106Total / l106Divisor + 1,
                    1 => l106Total / l106Divisor,
                    2 => l106Total % l106Divisor,
                    3 => (l106Total - l106Short) / l106Divisor,
                    _ => l106Total / l106Divisor
                }),

            "geometry.perimeter_area.rectangle_missing_side" =>
                parameters["knownSide"] > 0 &&
                parameters["area"] > 0 &&
                parameters["area"] % parameters["knownSide"] == 0 &&
                value == parameters["area"] / parameters["knownSide"],

            "geometry.polygons.interior_angle_sum" =>
                parameters["sides"] >= 3 &&
                value == (parameters["sides"] - 2) * 180,

            "geometry.polygons.missing_interior_angle" =>
                parameters["sides"] is 3 or 4 &&
                parameters["knownSum"] > 0 &&
                value == (parameters["sides"] - 2) * 180 - parameters["knownSum"] &&
                value > 0,

            "geometry.polygons.regular_interior_angle" =>
                parameters["sides"] >= 3 &&
                360 % parameters["sides"] == 0 &&
                value == 180 - 360 / parameters["sides"],

            "geometry.right_triangle.pythagorean.exact" =>
                value > 0 &&
                value * value ==
                    parameters["legA"] * parameters["legA"] +
                    parameters["legB"] * parameters["legB"],

            "geometry.right_triangle.pythagorean.find_leg_exact" =>
                value > 0 &&
                value == parameters["expectedLeg"] &&
                value * value + parameters["knownLeg"] * parameters["knownLeg"] ==
                    parameters["hypotenuse"] * parameters["hypotenuse"],

            "trigonometry.right_triangle.find_side_exact" or
            "trigonometry.modelling.contextual" =>
                value == parameters["expectedSide"] &&
                value > 0,

            "trigonometry.right_triangle.find_angle_exact" =>
                value == parameters["expectedAngle"] &&
                value is 30 or 45 or 60 &&
                parameters["ratioNumerator"] > 0 &&
                parameters["ratioDenominator"] > 0 &&
                (parameters["ratioType"] switch
                {
                    0 => value == 30 &&
                        parameters["ratioNumerator"] * 2 == parameters["ratioDenominator"],
                    1 => value == 60 &&
                        parameters["ratioNumerator"] * 2 == parameters["ratioDenominator"],
                    2 => value == 45 &&
                        parameters["ratioNumerator"] == parameters["ratioDenominator"],
                    _ => false
                }),

            "vectors.dot.exact_rational" =>
                value == parameters["ax"] * parameters["bx"] +
                    parameters["ay"] * parameters["by"],

            "vectors.magnitude.exact" =>
                value > 0 &&
                value * value ==
                    parameters["ax"] * parameters["ax"] +
                    parameters["ay"] * parameters["ay"],

            _ => false
        };
    }

    // Finite original geometry examples are combined into first-party compound
    // computations. Both original source-specific questions must be solved:
    // no fake randomness, duplicated exposure fingerprints or reused answers.
    // Grade 6's selected-source variant banks are genuinely finite. Only
    // compose numeric source tasks for adaptive sessions that request a
    // preferred variant; ordinary single-lesson Practice remains unchanged.
    private static bool IsGrade6FiniteSourceVariantFamily(string family) =>
        Grade6FiniteSourceFamilyIds.Contains(family) ||
        UsCcssG6Unit1Lesson2TangramQuestions.Supports(family) ||
        UsCcssG6Unit1Lesson3ShadedAreaQuestions.Supports(family) ||
        UsCcssG6Unit1Lesson4ParallelogramQuestions.Supports(family) ||
        UsCcssG6Unit1Lesson5BaseHeightQuestions.Supports(family) ||
        UsCcssG6Unit1Lesson6AreaQuestions.Supports(family) ||
        UsCcssG6Unit1Lesson7TriangleQuestions.Supports(family) ||
        UsCcssG6Unit1Lesson8TriangleAreaQuestions.Supports(family) ||
        UsCcssG6Unit1Lesson11PolygonQuestions.Supports(family) ||
        UsCcssG6Unit1Lesson12SurfaceAreaQuestions.Supports(family) ||
        UsCcssG6Unit2RatioQuestions.Supports(family);

    private static readonly HashSet<string> Grade6FiniteSourceFamilyIds = new(StringComparer.Ordinal)
    {
        "usccss.g6.u01.l02.tangram_equal_area",
        "usccss.g6.u01.l02.tangram_piece_area",
        "usccss.g6.u01.l03.area_conservation",
        "usccss.g6.u01.l03.source_shaded_area",
        "usccss.g6.u01.l04.equal_area_rearrangement",
        "usccss.g6.u01.l04.parallelogram_exact_area",
        "usccss.g6.u01.l04.two_pairs_parallel",
        "usccss.g6.u01.l05.area_scaling",
        "usccss.g6.u01.l05.corresponding_height",
        "usccss.g6.u01.l05.small_triangle_fraction",
        "usccss.g6.u01.l06.composite_area",
        "usccss.g6.u01.l06.equal_area_reasoning",
        "usccss.g6.u01.l07.original_fraction_dimensions",
        "usccss.g6.u01.l07.parallelogram_diagonal_half",
        "usccss.g6.u01.l07.triangle_composition_logic",
        "usccss.g6.u01.l08.decomposition_invariance",
        "usccss.g6.u01.l08.original_decimal_practice",
        "usccss.g6.u01.l08.triangle_half_area",
        "usccss.g6.u01.l09.triangle_height_validity",
        "usccss.g6.u01.l10.height_construction_validity",
        "usccss.g6.u01.l12.surface_area_meaning",
        "usccss.g6.u01.l15.net_design_and_volume_reasoning",
        "usccss.g6.u01.l16.surface_volume_unit_logic"
    };

    private static ExactProblem BuildProblem(
        string family, Random random, ExactSkillQuestionDifficulty difficulty,
        int? preferredVariant = null, bool extendedFamilySession = false,
        bool adaptiveCandidate = false)
    {
        if ((!extendedFamilySession && !adaptiveCandidate) ||
            !IsGrade6FiniteSourceVariantFamily(family))
            return BuildOriginalProblem(family, random, difficulty, preferredVariant);

        var a = BuildOriginalProblem(family, random, difficulty, preferredVariant);
        if (a.ItemType == AssessmentItemType.ShortAnswer &&
            family is (UsCcssG6Unit2RatioQuestions.L02Diagrams or
                       UsCcssG6Unit2RatioQuestions.L02Cards) &&
            a.Parameters.TryGetValue("variant", out var ratioVariant))
        {
            int ratioCopies = random.Next(2, 20);
            return new ExactProblem(family, a.Prompt +
                $" Edulytics extension: imagine {ratioCopies} identical copies of this " +
                "same labeled diagram. Calculate the TOTAL depicted marks in each " +
                "named row across ALL copies. Enter exact totals in order as " +
                (family == UsCcssG6Unit2RatioQuestions.L02Cards
                    ? "A:B:C. Do not simplify the depicted totals."
                    : "A:B. Do not simplify the depicted totals."),
                "Multiply each source row count by the number of diagrams; " +
                "preserve the original named row order.",
                AssessmentItemType.ShortAnswer,
                new Dictionary<string, int>
                {
                    ["g6ScaledRatio"] = 1,
                    ["variant"] = ratioVariant,
                    ["copies"] = ratioCopies
                });
        }
        if (a.ItemType != AssessmentItemType.Numeric)
            return a;
        var b = BuildOriginalProblem(family, random, difficulty, preferredVariant);
        if (!a.Parameters.TryGetValue("variant", out int av) ||
            !b.Parameters.TryGetValue("variant", out int bv))
            throw new InvalidOperationException("Grade 6 compound requires two source variants.");

        var copies = random.Next(2, 8);
        var parameters = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["g6Composite"] = 1,
            ["a"] = av,
            ["b"] = bv,
            ["copies"] = copies
        };
        var prompt = $"Edulytics first-party Grade 6 multi-situation exercise. " +
            $"There are {copies} independent copies of Situation A and one copy of " +
            "Situation B. Solve the numerical question in each situation, and " +
            $"enter ({copies} × A's numerical answer) + (B's numerical answer). " +
            "For any yes/no question, use 1 for yes and 0 for no. " +
            "For a fraction, use an exact fraction or decimal when exact. " +
            "Situation A: " + a.Prompt + " Situation B: " + b.Prompt;
        return new ExactProblem(family, prompt,
            "Independently solve both source-skill tasks, then combine " +
            "the values with exact arithmetic; diagrams still need a teacher's review.",
            a.ItemType, parameters);
    }

    private static string Grade6ScaledRatioValue(
        string family, IReadOnlyDictionary<string, int> p)
    {
        if (family is not (UsCcssG6Unit2RatioQuestions.L02Diagrams or
                           UsCcssG6Unit2RatioQuestions.L02Cards) ||
            p.Count != 3 ||
            !p.TryGetValue("g6ScaledRatio", out var flag) || flag != 1 ||
            !p.TryGetValue("copies", out var copies) || copies is < 2 or > 19 ||
            !p.TryGetValue("variant", out var variant))
            throw new ArgumentException("Invalid Grade 6 scaled ratio.");

        var original = new Dictionary<string, int> { ["variant"] = variant };
        var baseAnswer = UsCcssG6Unit2RatioQuestions.Solve(family, original);
        return string.Join(":", baseAnswer.Split(':')
            .Select(term => (int.Parse(term, CultureInfo.InvariantCulture) * copies)
                .ToString(CultureInfo.InvariantCulture)));
    }

    private static bool VerifyGrade6ScaledRatio(
        string family, IReadOnlyDictionary<string, int> p, string? answer)
    {
        if (string.IsNullOrWhiteSpace(answer) || answer != answer.Trim())
            return false;
        try
        {
            return string.Equals(
                Grade6ScaledRatioValue(family, p), answer,
                StringComparison.Ordinal);
        }
        catch (Exception ex) when (ex is ArgumentException or FormatException or OverflowException)
        {
            return false;
        }
    }

    private static bool VerifyGrade6Composite(
        string family, IReadOnlyDictionary<string, int> parameters, string? answer)
    {
        if (!IsValidGrade6Composite(parameters) || string.IsNullOrWhiteSpace(answer))
            return false;
        try
        {
            var expected = Grade6CompositeValue(family, parameters);
            var received = ParseGrade6ExactNumber(answer.Trim());
            return expected.Numerator * received.Denominator ==
                   received.Numerator * expected.Denominator;
        }
        catch (Exception ex) when (ex is ArgumentException or FormatException or OverflowException)
        {
            return false;
        }
    }

    private static bool IsValidGrade6Composite(IReadOnlyDictionary<string, int> p) =>
        p.Count == 4 && p.TryGetValue("g6Composite", out var composite) &&
        composite == 1 && p.ContainsKey("a") && p.ContainsKey("b") &&
        p.TryGetValue("copies", out var copies) && copies is >= 2 and <= 7;

    private static (BigInteger Numerator, BigInteger Denominator) Grade6CompositeValue(
        string family, IReadOnlyDictionary<string, int> p)
    {
        if (!IsGrade6FiniteSourceVariantFamily(family) || !IsValidGrade6Composite(p))
            throw new ArgumentException("Unsupported Grade 6 composite");
        var a = new Dictionary<string, int> { ["variant"] = p["a"] };
        var b = new Dictionary<string, int> { ["variant"] = p["b"] };
        var first = Solve(new ExactProblem(family, "", "", AssessmentItemType.Numeric, a));
        var second = Solve(new ExactProblem(family, "", "", AssessmentItemType.Numeric, b));
        if (!Verify(family, a, first) || !Verify(family, b, second))
            throw new ArgumentException("Invalid source variant");
        var (an, ad) = ParseGrade6ExactNumber(first);
        var (bn, bd) = ParseGrade6ExactNumber(second);
        var numerator = p["copies"] * an * bd + bn * ad;
        var denominator = ad * bd;
        var divisor = BigInteger.GreatestCommonDivisor(numerator, denominator);
        return (numerator / divisor, denominator / divisor);
    }

    private static (BigInteger Numerator, BigInteger Denominator) ParseGrade6ExactNumber(string raw)
    {
        var value = raw.Trim();
        var slash = value.IndexOf('/');
        if (slash >= 0)
        {
            var n = BigInteger.Parse(value[..slash], CultureInfo.InvariantCulture);
            var d = BigInteger.Parse(value[(slash + 1)..], CultureInfo.InvariantCulture);
            if (d <= 0) throw new ArgumentException("Invalid denominator");
            var gcd = BigInteger.GreatestCommonDivisor(n, d);
            return (n / gcd, d / gcd);
        }
        if (!decimal.TryParse(value, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out _))
            throw new FormatException("Invalid exact numeric response");
        var dot = value.IndexOf('.');
        var places = dot < 0 ? 0 : value.Length - dot - 1;
        var ntext = value.Replace(".", "", StringComparison.Ordinal);
        var numerator = BigInteger.Parse(ntext, CultureInfo.InvariantCulture);
        var denominator = BigInteger.Pow(10, places);
        var divisor = BigInteger.GreatestCommonDivisor(numerator, denominator);
        return (numerator / divisor, denominator / divisor);
    }

    private static ExactProblem BuildOriginalProblem(
        string family,
        Random random,
        ExactSkillQuestionDifficulty difficulty,
        int? preferredVariant = null)
    {
        var scale = difficulty switch
        {
            ExactSkillQuestionDifficulty.Stretch => 2,
            ExactSkillQuestionDifficulty.Challenge => 3,
            _ => 1
        };




        if (Grade12PrecalculusAdvancedNumericQuestions.Supports(family))
        {
            var advanced = Grade12PrecalculusAdvancedNumericQuestions.Build(
                family, random, scale);
            return new ExactProblem(
                advanced.Family, advanced.Prompt, advanced.Solution,
                advanced.ItemType, advanced.Parameters);
        }

        if (UsCcssAlg1U01L01StatisticalSelectionQuestions.Supports(family))
        {
            var item = UsCcssAlg1U01L01StatisticalSelectionQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (UsCcssAlg1U01L02RepresentationQuestions.Supports(family))
        {
            var item = UsCcssAlg1U01L02RepresentationQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.Supports(family))
        {
            var item = UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (UsCcssAlg1Unit2AlgebraQuestions.Supports(family))
        {
            var item = UsCcssAlg1Unit2AlgebraQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (UsCcssAlg1Unit2InequalityQuestions.Supports(family))
        {
            var item = UsCcssAlg1Unit2InequalityQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (UsCcssAlg1Unit3StatisticsQuestions.Supports(family))
        {
            var item = UsCcssAlg1Unit3StatisticsQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (UsCcssAlg1Unit4FunctionQuestions.Supports(family))
        {
            var item = UsCcssAlg1Unit4FunctionQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG5U03L01FractionOfHalfQuestions.Supports(family))
        {
            var item = UsCcssG5U03L01FractionOfHalfQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG5U03L02UnitFractionQuestions.Supports(family))
        {
            var item = UsCcssG5U03L02UnitFractionQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG5U03L03UnitFractionQuestions.Supports(family))
        {
            var item = UsCcssG5U03L03UnitFractionQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG5U03L04ParkQuestions.Supports(family))
        {
            var item = UsCcssG5U03L04ParkQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG5U03L05ShadedAreaQuestions.Supports(family))
        {
            var item = UsCcssG5U03L05ShadedAreaQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG5U03L06NonunitAreaQuestions.Supports(family))
        {
            var item = UsCcssG5U03L06NonunitAreaQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG5U03L07FractionEquationQuestions.Supports(family))
        {
            var item = UsCcssG5U03L07FractionEquationQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG5U03L08FlagMeasureQuestions.Supports(family))
        {
            var item = UsCcssG5U03L08FlagMeasureQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG5U03L09FlagDesignSupportQuestions.Supports(family))
        {
            var item = UsCcssG5U03L09FlagDesignSupportQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG5U03L10SharingQuestions.Supports(family))
        {
            var item = UsCcssG5U03L10SharingQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG5U03SourceDivisionQuestions.Supports(family))
        {
            var item = UsCcssG5U03SourceDivisionQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG5U04WholeNumberSourceProducts.Supports(family))
        {
            var item = UsCcssG5U04WholeNumberSourceProducts.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG5U04WholeNumberSourceDivision.Supports(family))
        {
            var item = UsCcssG5U04WholeNumberSourceDivision.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG5U05SourceThousandths.Supports(family))
        {
            var item = UsCcssG5U05SourceThousandths.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG7Unit1Lesson1ScaledCopyQuestions.Supports(family))
        {
            var item = UsCcssG7Unit1Lesson1ScaledCopyQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG7Unit1Lesson2CorrespondenceQuestions.Supports(family))
        {
            var item = UsCcssG7Unit1Lesson2CorrespondenceQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG7Unit1Lesson3DrawingQuestions.Supports(family))
        {
            var item = UsCcssG7Unit1Lesson3DrawingQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG7Unit1Lesson4ScaledRelationshipsQuestions.Supports(family))
        {
            var item = UsCcssG7Unit1Lesson4ScaledRelationshipsQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG7Unit1Lesson5FactorQuestions.Supports(family))
        {
            var item = UsCcssG7Unit1Lesson5FactorQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG7Unit1Lesson6AreaScalingQuestions.Supports(family))
        {
            var item = UsCcssG7Unit1Lesson6AreaScalingQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG7Unit1Lesson7DrawingScaleQuestions.Supports(family))
        {
            var item = UsCcssG7Unit1Lesson7DrawingScaleQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG7Unit1Lesson8MapRateQuestions.Supports(family))
        {
            var item = UsCcssG7Unit1Lesson8MapRateQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG7Unit1Lesson9CreatingDrawingsQuestions.Supports(family))
        {
            var item = UsCcssG7Unit1Lesson9CreatingDrawingsQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG7Unit1Lesson10ChangingScalesQuestions.Supports(family))
        {
            var item = UsCcssG7Unit1Lesson10ChangingScalesQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG7Unit1Lesson11UnitlessScalesQuestions.Supports(family))
        {
            var item = UsCcssG7Unit1Lesson11UnitlessScalesQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG7Unit2Lesson1EquivalentRatioQuestions.Supports(family))
        {
            var item = UsCcssG7Unit2Lesson1EquivalentRatioQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG7Unit2Lesson3UnitRatesQuestions.Supports(family))
        {
            var item = UsCcssG7Unit2Lesson3UnitRatesQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG7Unit2Lesson4ProportionalEquationsQuestions.Supports(family))
        {
            var item = UsCcssG7Unit2Lesson4ProportionalEquationsQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG7Unit2Lesson6ConcertRecyclingQuestions.Supports(family))
        {
            var item = UsCcssG7Unit2Lesson6ConcertRecyclingQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG7Unit2Lesson7TableProportionalityQuestions.Supports(family))
        {
            var item = UsCcssG7Unit2Lesson7TableProportionalityQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG7Unit2Lesson8ClassifyEquationsQuestions.Supports(family))
        {
            var item = UsCcssG7Unit2Lesson8ClassifyEquationsQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG7Unit2Lesson9RateComparisonQuestions.Supports(family))
        {
            var item = UsCcssG7Unit2Lesson9RateComparisonQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG7Unit2Lesson10GraphProportionsQuestions.Supports(family))
        {
            var item = UsCcssG7Unit2Lesson10GraphProportionsQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }

        if (UsCcssG6Unit1Lesson1TilingQuestions.Supports(family))
        {
            var item = UsCcssG6Unit1Lesson1TilingQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }


        if (UsCcssG6Unit1Lesson2TangramQuestions.Supports(family))
        {
            var item = UsCcssG6Unit1Lesson2TangramQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Explanation,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG6Unit1Lesson3ShadedAreaQuestions.Supports(family))
        {
            var item = UsCcssG6Unit1Lesson3ShadedAreaQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Explanation,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG6Unit1Lesson4ParallelogramQuestions.Supports(family))
        {
            var item = UsCcssG6Unit1Lesson4ParallelogramQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Explanation,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG6Unit1Lesson5BaseHeightQuestions.Supports(family))
        {
            var item = UsCcssG6Unit1Lesson5BaseHeightQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Explanation,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG6Unit1Lesson6AreaQuestions.Supports(family))
        {
            var item = UsCcssG6Unit1Lesson6AreaQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Explanation,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG6Unit1Lesson7TriangleQuestions.Supports(family))
        {
            var item = UsCcssG6Unit1Lesson7TriangleQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Explanation,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG6Unit1Lesson8TriangleAreaQuestions.Supports(family))
        {
            var item = UsCcssG6Unit1Lesson8TriangleAreaQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Explanation,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG6Unit1Lesson11PolygonQuestions.Supports(family))
        {
            var item = UsCcssG6Unit1Lesson11PolygonQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Explanation,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG6Unit2RatioQuestions.Supports(family))
        {
            var item = UsCcssG6Unit2RatioQuestions.Build(family, random);
            return new ExactProblem(item.Family,item.Prompt,item.Explanation,item.ItemType,item.Parameters);
        }
        if (UsCcssG6Unit1Lesson12SurfaceAreaQuestions.Supports(family))
        {
            var item = UsCcssG6Unit1Lesson12SurfaceAreaQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Explanation,
                item.ItemType, item.Parameters);
        }


        if (UsCcssG1FullGradeUnit7GeometryTimeQuestions.Supports(family))
        {
            var item=UsCcssG1FullGradeUnit7GeometryTimeQuestions.Build(family,random);
            return new ExactProblem(item.Family,item.Prompt,item.Explanation,item.ItemType,item.Parameters);
        }
        if (UsCcssG1HowCloseStage3Questions.Supports(family))
        {
            var item=UsCcssG1HowCloseStage3Questions.Build(random);
            return new ExactProblem(item.Family,item.Prompt,item.Explanation,item.ItemType,item.Parameters);
        }
        if (UsCcssG1FullGradeUnit6MeasurementQuestions.Supports(family))
        {
            var item=UsCcssG1FullGradeUnit6MeasurementQuestions.Build(family,random);
            return new ExactProblem(item.Family,item.Prompt,item.Explanation,item.ItemType,item.Parameters);
        }
        if (UsCcssG1FullGradeUnit8ReviewQuestions.Supports(family))
        {
            var item=UsCcssG1FullGradeUnit8ReviewQuestions.Build(family,random);
            return new ExactProblem(item.Family,item.Prompt,item.Explanation,item.ItemType,item.Parameters);
        }
        if (UsCcssG1FullGradeUnit4FinalQuestions.Supports(family))
        {
            var item=UsCcssG1FullGradeUnit4FinalQuestions.Build(family,random);
            return new ExactProblem(item.Family,item.Prompt,item.Explanation,item.ItemType,item.Parameters);
        }
        if (UsCcssG1FullGradeUnit5AdditionQuestions.Supports(family))
        {
            var item=UsCcssG1FullGradeUnit5AdditionQuestions.Build(family,random);
            return new ExactProblem(item.Family,item.Prompt,item.Explanation,item.ItemType,item.Parameters);
        }
        if (UsCcssG1FullGradeTwoDigitOperationsQuestions.Supports(family))
        {
            var item=UsCcssG1FullGradeTwoDigitOperationsQuestions.Build(family,random);
            return new ExactProblem(item.Family,item.Prompt,item.Explanation,item.ItemType,item.Parameters);
        }
        if (UsCcssG1FullGradeTensAndOnesQuestions.Supports(family))
        {
            var item=UsCcssG1FullGradeTensAndOnesQuestions.Build(family,random);
            return new ExactProblem(item.Family,item.Prompt,item.Explanation,item.ItemType,item.Parameters);
        }
        if (UsCcssG1FullGradeStrategyAndTensQuestions.Supports(family))
        {
            var item=UsCcssG1FullGradeStrategyAndTensQuestions.Build(family,random);
            return new ExactProblem(item.Family,item.Prompt,item.Explanation,item.ItemType,item.Parameters);
        }
        if (UsCcssG1FullGradeNextQuestions.Supports(family))
        {
            var item=UsCcssG1FullGradeNextQuestions.Build(family,random);
            return new ExactProblem(item.Family,item.Prompt,item.Explanation,item.ItemType,item.Parameters);
        }
        if (UsCcssG1FullGradeStartQuestions.Supports(family))
        {
            var item=UsCcssG1FullGradeStartQuestions.Build(family,random);
            return new ExactProblem(item.Family,item.Prompt,item.Explanation,item.ItemType,item.Parameters);
        }
        if (UsCcssG1TeenFiveQuestions.Supports(family))
        {
            var item=UsCcssG1TeenFiveQuestions.Build(family,random);
            return new ExactProblem(item.Family,item.Prompt,item.Solution,item.ItemType,item.Parameters);
        }
        if (UsCcssG1SourceFiveNextQuestions.Supports(family))
        {
            var item=UsCcssG1SourceFiveNextQuestions.Build(family,random);
            return new ExactProblem(item.Family,item.Prompt,item.Solution,item.ItemType,item.Parameters);
        }
        if (UsCcssG1NextSourceFiveQuestions.Supports(family))
        {
            var item=UsCcssG1NextSourceFiveQuestions.Build(family,random);
            return new ExactProblem(item.Family,item.Prompt,item.Solution,item.ItemType,item.Parameters);
        }
        if (UsCcssG1Unit2EquationStoryFiveQuestions.Supports(family))
        {
            var item=UsCcssG1Unit2EquationStoryFiveQuestions.Build(family,random);
            return new ExactProblem(item.Family,item.Prompt,item.Solution,item.ItemType,item.Parameters);
        }
        if (UsCcssG1Unit2CompareFiveQuestions.Supports(family))
        {
            var item=UsCcssG1Unit2CompareFiveQuestions.Build(family,random);
            return new ExactProblem(item.Family,item.Prompt,item.Solution,
                item.ItemType,item.Parameters);
        }
        if (UsCcssG1Unit2NextFiveQuestions.Supports(family))
        {
            var item=UsCcssG1Unit2NextFiveQuestions.Build(family,random);
            return new ExactProblem(item.Family,item.Prompt,item.Solution,
                item.ItemType,item.Parameters);
        }
        if (UsCcssG1Unit2Part2Questions.Supports(family))
        {
            var story=UsCcssG1Unit2Part2Questions.Build(family,random);
            return new ExactProblem(story.Family,story.Prompt,story.Solution,
                story.ItemType,story.Parameters);
        }
        if (UsCcssG4Unit2ThirdLessonQuestions.Supports(family))
        {
            var item=UsCcssG4Unit2ThirdLessonQuestions.Build(family,random);
            return new ExactProblem(item.Family,item.Prompt,item.Solution,item.ItemType,item.Parameters);
        }
        if (UsCcssG4Unit1SeventhLessonQuestions.Supports(family))
        {
            var item=UsCcssG4Unit1SeventhLessonQuestions.Build(family,random);
            return new ExactProblem(item.Family,item.Prompt,item.Solution,item.ItemType,item.Parameters);
        }
        if (UsCcssG4Unit1SixthLessonQuestions.Supports(family))
        {
            var q=UsCcssG4Unit1SixthLessonQuestions.Build(family,random);
            return new ExactProblem(q.Family,q.Prompt,q.Solution,q.ItemType,q.Parameters);
        }
        if (UsCcssG4Unit1FifthLessonQuestions.Supports(family))
        {
            var item=UsCcssG4Unit1FifthLessonQuestions.Build(family,random);
            return new ExactProblem(item.Family,item.Prompt,item.Solution,item.ItemType,item.Parameters);
        }
        if (UsCcssG4Unit1ThirdLessonQuestions.Supports(family))
        {
            var item=UsCcssG4Unit1ThirdLessonQuestions.Build(family,random);
            return new ExactProblem(item.Family,item.Prompt,item.Solution,item.ItemType,item.Parameters);
        }
        if (UsCcssG4Unit1SecondLessonQuestions.Supports(family))
        {
            var p2=UsCcssG4Unit1SecondLessonQuestions.Build(family,random);
            return new ExactProblem(p2.Family,p2.Prompt,p2.Solution,p2.ItemType,p2.Parameters);
        }
        if (UsCcssG4Unit1FirstLessonQuestions.Supports(family))
        {
            var rectangle = UsCcssG4Unit1FirstLessonQuestions.Build(family, random);
            return new ExactProblem(rectangle.Family, rectangle.Prompt, rectangle.Solution,
                rectangle.ItemType, rectangle.Parameters);
        }
        if (UsCcssG5Unit1Lesson2Questions.Supports(family))
        {
            var g5 = UsCcssG5Unit1Lesson2Questions.Build(family, random);
            return new ExactProblem(g5.Family, g5.Prompt, g5.Solution,
                g5.ItemType, g5.Parameters);
        }
        if (UsCcssG5Unit1Lesson3Questions.Supports(family))
        {
            var g5 = UsCcssG5Unit1Lesson3Questions.Build(family, random);
            return new ExactProblem(g5.Family,g5.Prompt,g5.Solution,g5.ItemType,g5.Parameters);
        }
        if (UsCcssG5Unit1Lesson4Questions.Supports(family))
        {
            var g5=UsCcssG5Unit1Lesson4Questions.Build(family,random);
            return new ExactProblem(g5.Family,g5.Prompt,g5.Solution,g5.ItemType,g5.Parameters);
        }
        if (UsCcssG5Unit1Lesson5Questions.Supports(family))
        {
            var g5=UsCcssG5Unit1Lesson5Questions.Build(family,random);
            return new ExactProblem(g5.Family,g5.Prompt,g5.Solution,g5.ItemType,g5.Parameters);
        }
        if (UsCcssG5Unit1Lesson6Questions.Supports(family))
        {
            var g5=UsCcssG5Unit1Lesson6Questions.Build(family,random);
            return new ExactProblem(g5.Family,g5.Prompt,g5.Solution,g5.ItemType,g5.Parameters);
        }
        if (UsCcssG5Unit1Lesson7Questions.Supports(family))
        {
            var sourceModel=UsCcssG5Unit1Lesson7Questions.Build(family,random);
            return new ExactProblem(sourceModel.Family,sourceModel.Prompt,sourceModel.Solution,sourceModel.ItemType,sourceModel.Parameters);
        }
        if(UsCcssG5Unit1Lesson8Questions.Supports(family)){
            var item=UsCcssG5Unit1Lesson8Questions.Build(family,random);
            return new ExactProblem(item.Family,item.Prompt,item.Solution,item.ItemType,item.Parameters);
        }
        if(UsCcssG5Unit1Lesson9Questions.Supports(family)){
            var item=UsCcssG5Unit1Lesson9Questions.Build(family,random);
            return new ExactProblem(item.Family,item.Prompt,item.Solution,item.ItemType,item.Parameters);
        }
        if(UsCcssG5Unit1Lesson10Questions.Supports(family)){
            var item=UsCcssG5Unit1Lesson10Questions.Build(family,random);
            return new ExactProblem(item.Family,item.Prompt,item.Solution,item.ItemType,item.Parameters);
        }
        if(UsCcssG5Unit1Lesson11Questions.Supports(family)){
            var item=UsCcssG5Unit1Lesson11Questions.Build(family,random);
            return new ExactProblem(item.Family,item.Prompt,item.Solution,item.ItemType,item.Parameters);
        }
        if(UsCcssG5Unit1Lesson12Questions.Supports(family)){
            var item=UsCcssG5Unit1Lesson12Questions.Build(family,random);
            return new ExactProblem(item.Family,item.Prompt,item.Solution,item.ItemType,item.Parameters);
        }
        if (UsCcssG5Unit2SourceQuestions.Supports(family))
        {
            var sourceItem=UsCcssG5Unit2SourceQuestions.Build(family,random);
            return new ExactProblem(sourceItem.Family,sourceItem.Prompt,sourceItem.Solution,
                sourceItem.ItemType,sourceItem.Parameters);
        }
        if (UsCcssG5Unit1Lesson1ComparisonQuestions.Supports(family))
        {
            var comparison = UsCcssG5Unit1Lesson1ComparisonQuestions.Build(family, random);
            return new ExactProblem(comparison.Family, comparison.Prompt, comparison.Solution,
                comparison.ItemType, comparison.Parameters);
        }
        if (UsCcssG2Unit1FirstLessonQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit1FirstLessonQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit1Lesson2And3Questions.Supports(family))
        {
            var goal = UsCcssG2Unit1Lesson2And3Questions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit1Lesson4Questions.Supports(family))
        {
            var goal = UsCcssG2Unit1Lesson4Questions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit1Lesson5Questions.Supports(family))
        {
            var goal = UsCcssG2Unit1Lesson5Questions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit1Lesson6NumberTalkQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit1Lesson6NumberTalkQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit1Lesson8PictureGraphQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit1Lesson8PictureGraphQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit1Lesson9BarGraphQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit1Lesson9BarGraphQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit1Lesson10ConstructGraphsQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit1Lesson10ConstructGraphsQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit1Lesson11GraphQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit1Lesson11GraphQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit1Lesson12SortAndDisplayQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit1Lesson12SortAndDisplayQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit1Lesson13BarCompareQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit1Lesson13BarCompareQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit1Lesson14TapeDiagramQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit1Lesson14TapeDiagramQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit1Lesson15CompareQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit1Lesson15CompareQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit1Lesson16CompareQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit1Lesson16CompareQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit1Lesson17CenterQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit1Lesson17CenterQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit1Lesson18ClassSurveyQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit1Lesson18ClassSurveyQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit2Lesson1NoRegroupQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit2Lesson1NoRegroupQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit2Lesson2UnknownAddendQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit2Lesson2UnknownAddendQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit2Lesson3ZooStoryQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit2Lesson3ZooStoryQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit2Lesson4OptionalCenterQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit2Lesson4OptionalCenterQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit2Lesson5DecomposeTenQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit2Lesson5DecomposeTenQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit2Lesson6TargetNumbersQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit2Lesson6TargetNumbersQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit2Lesson7SubtractTwoDigitsQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit2Lesson7SubtractTwoDigitsQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit2Lesson8DifferentWaysQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit2Lesson8DifferentWaysQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit2Lesson9MixedAddSubtractQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit2Lesson9MixedAddSubtractQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit2Lesson10TargetStage5Questions.Supports(family))
        {
            var goal = UsCcssG2Unit2Lesson10TargetStage5Questions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit2Lesson11SeedsStoryQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit2Lesson11SeedsStoryQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit2Lesson12MancalaQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit2Lesson12MancalaQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit2Lesson13StoryEquationsQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit2Lesson13StoryEquationsQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit2Lesson14TwoStepSeedsQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit2Lesson14TwoStepSeedsQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit2Lesson15TapeStoriesQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit2Lesson15TapeStoriesQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit2Lesson16MarketInventoryQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit2Lesson16MarketInventoryQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit3Lesson1StandardUnitsQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit3Lesson1StandardUnitsQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit3Lesson2MeasureCmQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit3Lesson2MeasureCmQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit3Lesson3BuildRulerQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit3Lesson3BuildRulerQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit3Lesson4EstimateMeasureCmQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit3Lesson4EstimateMeasureCmQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit3Lesson5MetersQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit3Lesson5MetersQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit3Lesson6ReptileStoriesQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit3Lesson6ReptileStoriesQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit3Lesson7EstimateMeasureStage2Questions.Supports(family))
        {
            var goal = UsCcssG2Unit3Lesson7EstimateMeasureStage2Questions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit3Lesson8WhatIsAnInchQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit3Lesson8WhatIsAnInchQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit3Lesson9FishFeetQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit3Lesson9FishFeetQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit3Lesson10TornTapeQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit3Lesson10TornTapeQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit3Lesson11SareeRibbonQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit3Lesson11SareeRibbonQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit3Lesson12FriendshipQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit3Lesson12FriendshipQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit3Lesson13CenterDay2Questions.Supports(family))
        {
            var goal = UsCcssG2Unit3Lesson13CenterDay2Questions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit3Lesson14HandSpanLinePlotQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit3Lesson14HandSpanLinePlotQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit3Lesson15PencilLinePlotQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit3Lesson15PencilLinePlotQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit3Lesson16PlantHeightsQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit3Lesson16PlantHeightsQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit3Lesson17CenterPlotQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit3Lesson17CenterPlotQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit3Lesson18YardstickQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit3Lesson18YardstickQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit4Lesson01NumberLineQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit4Lesson01NumberLineQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit4Lesson02NumberLineFeaturesQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit4Lesson02NumberLineFeaturesQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit4Lesson03UnlabeledTicksQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit4Lesson03UnlabeledTicksQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit4NextThreeQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit4NextThreeQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit4ArrowLessonsQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit4ArrowLessonsQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit4FinalSixQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit4FinalSixQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit5FirstSixQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit5FirstSixQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit5ComparisonQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit5ComparisonQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssG2Unit9ExactQuestions.Supports(family))
        {
          var item=UsCcssG2Unit9ExactQuestions.Build(family,random);
          return new ExactProblem(item.Family,item.Prompt,item.Solution,item.ItemType,item.Parameters);
        }
        if (UsCcssG2Unit8ExactQuestions.Supports(family))
        {
          var x=UsCcssG2Unit8ExactQuestions.Build(family,random);
          return new ExactProblem(x.Family,x.Prompt,x.Solution,x.ItemType,x.Parameters);
        }
        if (UsCcssG2Unit7LastTenQuestions.Supports(family))
        {
          var item=UsCcssG2Unit7LastTenQuestions.Build(family,random);
          return new ExactProblem(item.Family,item.Prompt,item.Solution,item.ItemType,item.Parameters);
        }
        if (UsCcssG2Unit7NextFourQuestions.Supports(family))
        {
           var item=UsCcssG2Unit7NextFourQuestions.Build(family,random);
           return new ExactProblem(item.Family,item.Prompt,item.Solution,item.ItemType,item.Parameters);
        }
        if (UsCcssG2Unit7FirstFourQuestions.Supports(family))
        {
            var goal=UsCcssG2Unit7FirstFourQuestions.Build(family,random);
            return new ExactProblem(goal.Family,goal.Prompt,goal.Solution,goal.ItemType,goal.Parameters);
        }
        if (UsCcssG2Unit6ExactQuestions.Supports(family))
        {
            var goal = UsCcssG2Unit6ExactQuestions.Build(family, random);
            return new ExactProblem(goal.Family, goal.Prompt, goal.Solution,
                goal.ItemType, goal.Parameters);
        }
        if (UsCcssB001SourceGoalQuestions.Supports(family))
        {
            var goal=UsCcssB001SourceGoalQuestions.Build(family,random);
            return new ExactProblem(goal.Family,goal.Prompt,goal.Solution,
                goal.ItemType,goal.Parameters);
        }
        if (UsCcssG1Unit2StoryQuestions.Supports(family))
        {
            var story=UsCcssG1Unit2StoryQuestions.Build(family,random);
            return new ExactProblem(story.Family,story.Prompt,story.Solution,
                story.ItemType,story.Parameters);
        }
        if (UsCcssG1Batch03DataQuestions.Supports(family))
        {
            var data=UsCcssG1Batch03DataQuestions.Build(family,random);
            return new ExactProblem(data.Family,data.Prompt,data.Solution,
                data.ItemType,data.Parameters);
        }
        if (UsCcssT001AuthoredActivities.Supports(family))
        {
            var authored=UsCcssT001AuthoredActivities.Build(family,random);
            return new ExactProblem(authored.Family,authored.Prompt,authored.Solution,
                authored.ItemType,authored.Parameters);
        }
        if (UsCcssT001ConstructShapeGraph.Supports(family))
        {
            var graph=UsCcssT001ConstructShapeGraph.Build(random);
            return new ExactProblem(graph.Family,graph.Prompt,graph.Solution,
                graph.ItemType,graph.Parameters);
        }
        if (UsCcssT001RepresentationChoices.Supports(family))
        {
            var choice=UsCcssT001RepresentationChoices.Build(family,random);
            return new ExactProblem(choice.Family,choice.Prompt,choice.Solution,
                choice.ItemType,choice.Parameters);
        }
        if (UsCcssT001ExactQuestions.Supports(family))
        {
            var t001=UsCcssT001ExactQuestions.Build(family,random);
            return new ExactProblem(t001.Family,t001.Prompt,t001.Solution,
                t001.ItemType,t001.Parameters);
        }
        if (UaeGrade2Unit1Lesson01Questions.Supports(family))
        {
            var item = UaeGrade2Unit1Lesson01Questions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (SupportingPracticeCompletionEngine.Supports(family))
        {
            var supporting = SupportingPracticeCompletionEngine.Build(
                family,
                random,
                scale,
                preferredVariant);
            return new ExactProblem(
                supporting.Family,
                supporting.Prompt,
                supporting.Solution,
                supporting.ItemType,
                supporting.Parameters);
        }

        if (UaeGrade2Unit1Lessons02And13Questions.Supports(family))
        {
            var q = UaeGrade2Unit1Lessons02And13Questions.Build(family, random);
            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);
        }
        if (UaeGrade2Unit2Questions.Supports(family))
        {
            var q = UaeGrade2Unit2Questions.Build(family, random);
            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);
        }
        if (UaeGrade2Unit3Questions.Supports(family))
        {
            var q = UaeGrade2Unit3Questions.Build(family, random);
            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);
        }
        if (UaeGrade2Unit4Questions.Supports(family))
        {
            var q = UaeGrade2Unit4Questions.Build(family, random);
            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);
        }
        if (UaeGrade2Unit5Questions.Supports(family))
        {
            var q = UaeGrade2Unit5Questions.Build(family, random);
            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);
        }
        if (UaeGrade3Unit1Questions.Supports(family))
        {
            var q = UaeGrade3Unit1Questions.Build(family, random);
            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);
        }
        if (UaeGrade3Unit1Lesson02Questions.Supports(family))
        {
            var q = UaeGrade3Unit1Lesson02Questions.Build(family, random);
            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);
        }
        if (UaeGrade3Unit1Lesson03Questions.Supports(family))
        {
            var q = UaeGrade3Unit1Lesson03Questions.Build(family, random);
            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);
        }
        if (UaeGrade3Unit1Lesson04Questions.Supports(family))
        {
            var q = UaeGrade3Unit1Lesson04Questions.Build(family, random);
            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);
        }
        if (UaeGrade3Unit1Lesson05Questions.Supports(family))
        {
            var q = UaeGrade3Unit1Lesson05Questions.Build(family, random);
            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);
        }
        if (UaeGrade3Unit2Lesson01Questions.Supports(family))
        {
            var q = UaeGrade3Unit2Lesson01Questions.Build(family, random);
            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);
        }
        if (UaeGrade3Unit2Lesson02Questions.Supports(family))
        {
            var q = UaeGrade3Unit2Lesson02Questions.Build(family, random);
            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);
        }
        if (UaeGrade3Unit2Lesson03Questions.Supports(family))
        {
            var q = UaeGrade3Unit2Lesson03Questions.Build(family, random);
            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);
        }
        if (UaeGrade2Unit6Questions.Supports(family))
        {
            var q = UaeGrade2Unit6Questions.Build(family, random);
            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);
        }
        if (UaeGrade2Unit7Questions.Supports(family))
        {
            var q = UaeGrade2Unit7Questions.Build(family, random);
            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);
        }
        if (UaeGrade2Unit8Questions.Supports(family))
        {
            var q = UaeGrade2Unit8Questions.Build(family, random);
            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);
        }
        if (UaeGrade2Term3Questions.Supports(family))
        {
            var q = UaeGrade2Term3Questions.Build(family, random);
            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);
        }
        return family switch
        {
            "uae.g1.u01.l01.read_write_1_5" =>
                BuildUaeGrade1Count(family, random),
            "uae.g1.u01.l02.zero_cardinality" =>
                BuildUaeGrade1Count(family, random),
            "uae.g1.u01.l03.read_write_6_8" =>
                BuildUaeGrade1Count(family, random),
            "uae.g1.u01.l04.read_write_9_10" =>
                BuildUaeGrade1Count(family, random),
            "uae.g1.u01.l05.read_write_11_15" =>
                BuildUaeGrade1Count(family, random),
            "uae.g1.u01.l06.read_write_16_20" =>
                BuildUaeGrade1Count(family, random),
            "uae.g1.u02.l01.addition_stories" =>
                BuildUaeGrade1AdditionStory(random, family),
            "uae.g1.u02.l02.model_addition" =>
                BuildUaeGrade1AdditionStory(random, family),
            "uae.g1.u02.l03.addition_number_sentences" =>
                BuildUaeGrade1AdditionStory(random, family),
            "uae.g1.u02.l04.add_zero" =>
                BuildUaeGrade1AdditionZero(random),
            "uae.g1.u02.l05.vertical_addition" =>
                BuildUaeGrade1AdditionStory(random, family),
            "uae.g1.u02.l06.write_story_equation" =>
                BuildUaeGrade1AdditionStory(random, family),
            "uae.g1.u02.l07.make_four_five" =>
                BuildUaeGrade1ComposeNumber(random, family),
            "uae.g1.u02.l08.make_six_seven" =>
                BuildUaeGrade1ComposeNumber(random, family),
            "uae.g1.u02.l09.make_eight" =>
                BuildUaeGrade1ComposeNumber(random, family),
            "uae.g1.u02.l10.make_nine" =>
                BuildUaeGrade1ComposeNumber(random, family),
            "uae.g1.u02.l11.make_ten_frame" =>
                BuildUaeGrade1ComposeNumber(random, family),
            "uae.g1.u02.l13.true_false_addition" =>
                BuildUaeGrade1TrueFalseAddition(random),
            "uae.g1.u03.l01.subtraction_stories" =>
                BuildUaeGrade1SubtractionStory(random, family),
            "uae.g1.u03.l02.model_subtraction" =>
                BuildUaeGrade1SubtractionStory(random, family),
            "uae.g1.u03.l03.subtraction_number_sentences" =>
                BuildUaeGrade1SubtractionStory(random, family),
            "uae.g1.u03.l04.subtract_zero_or_all" =>
                BuildUaeGrade1SubtractionStory(random, family),
            "uae.g1.u03.l05.vertical_subtraction" =>
                BuildUaeGrade1SubtractionStory(random, family),
            "uae.g1.u03.l07.compare_groups" =>
                BuildUaeGrade1SubtractionStory(random, family),
            "uae.g1.u03.l08.subtract_4_5" =>
                BuildUaeGrade1SubtractionStory(random, family),
            "uae.g1.u03.l09.subtract_6_7" =>
                BuildUaeGrade1SubtractionStory(random, family),
            "uae.g1.u03.l10.subtract_8" =>
                BuildUaeGrade1SubtractionStory(random, family),
            "uae.g1.u03.l11.subtract_9" =>
                BuildUaeGrade1SubtractionStory(random, family),
            "uae.g1.u03.l12.subtract_10" =>
                BuildUaeGrade1SubtractionStory(random, family),
            "uae.g1.u03.l13.related_add_subtract" =>
                BuildUaeGrade1RelatedFacts(random),
            "uae.g1.u03.l14.true_false_subtraction" =>
                BuildUaeGrade1TrueFalseSubtraction(random),
            "uae.g1.u04.l01.count_on_1_2_3" =>
                BuildUaeGrade1CountOn(random),
            "uae.g1.u04.l02.count_on_fils" =>
                BuildUaeGrade1CountOnFils(random),
            "uae.g1.u04.l03.add_on_number_line" =>
                BuildUaeGrade1NumberLineAddition(random),
            "uae.g1.u04.l04.doubles_add" =>
                BuildUaeGrade1Doubles(random),
            "uae.g1.u04.l05.near_doubles_add" =>
                BuildUaeGrade1NearDoubles(random),
            "uae.g1.u04.l06.act_out_problem_solving" =>
                BuildUaeGrade1ActOut(random),
            "uae.g1.u04.l07.make_ten_add" =>
                BuildUaeGrade1MakeTenAdd(random),
            "uae.g1.u04.l08.commutative_add" =>
                BuildUaeGrade1CommutativeAdd(random),
            "uae.g1.u04.l09.add_three_with_strategy" =>
                BuildUaeGrade1ThreeAddends(random),
            "uae.g1.u05.l01.count_back_1_2_3" =>
                BuildUaeGrade1Unit4Lesson01(random),
            "uae.g1.u05.l02.number_line_subtract" =>
                BuildUaeGrade1Unit4Lesson02(random),
            "uae.g1.u05.l03.doubles_subtraction" =>
                BuildUaeGrade1Unit4Lesson03(random),
            "uae.g1.u05.l04.story_subtraction_equation" =>
                BuildUaeGrade1Unit4Lesson04(random),
            "uae.g1.u05.l05.subtract_through_ten" =>
                BuildUaeGrade1Unit4Lesson05(random),
            "uae.g1.u05.l06.related_add_sub_facts" =>
                BuildUaeGrade1Unit4Lesson06(random),
            "uae.g1.u05.l07.fact_family_four_equations" =>
                BuildUaeGrade1Unit4Lesson07(random),
            "uae.g1.u05.l08.missing_addend_part_whole" =>
                BuildUaeGrade1Unit4Lesson08(random),
            "uae.g1.u06.l01.ordinal_1_10" =>
                BuildUaeGrade1Unit5(1, family, random),
            "uae.g1.u06.l02.numbers_11_19" =>
                BuildUaeGrade1Unit5(2, family, random),
            "uae.g1.u06.l03.groups_of_ten" =>
                BuildUaeGrade1Unit5(3, family, random),
            "uae.g1.u06.l04.count_tens_fils" =>
                BuildUaeGrade1Unit5(4, family, random),
            "uae.g1.u06.l05.ten_and_some_more" =>
                BuildUaeGrade1Unit5(5, family, random),
            "uae.g1.u06.l06.tens_and_ones" =>
                BuildUaeGrade1Unit5(6, family, random),
            "uae.g1.u06.l07.make_table" =>
                BuildUaeGrade1Unit5(7, family, random),
            "uae.g1.u06.l08.numbers_to_100" =>
                BuildUaeGrade1Unit5(8, family, random),
            "uae.g1.u06.l09.ten_more_ten_less" =>
                BuildUaeGrade1Unit5(9, family, random),
            "uae.g1.u06.l10.count_fives_fils" =>
                BuildUaeGrade1Unit5(10, family, random),
            "uae.g1.u06.l11.compare_models" =>
                BuildUaeGrade1Unit5(11, family, random),
            "uae.g1.u06.l12.compare_symbols" =>
                BuildUaeGrade1Unit5(12, family, random),
            "uae.g1.u06.l13.hundreds_tens_ones" =>
                BuildUaeGrade1Unit5(13, family, random),
            "uae.g1.u06.l14.count_to_120" =>
                BuildUaeGrade1Unit5(14, family, random),
            "uae.g1.u06.l15.read_write_to_120" =>
                BuildUaeGrade1Unit5(15, family, random),
            "uae.g1.u07.l01.add_tens" =>
                BuildUaeGrade1Unit6(1, family, random),
            "uae.g1.u07.l02.count_on_tens_ones" =>
                BuildUaeGrade1Unit6(2, family, random),
            "uae.g1.u07.l03.add_tens_ones" =>
                BuildUaeGrade1Unit6(3, family, random),
            "uae.g1.u07.l04.guess_check_revise" =>
                BuildUaeGrade1Unit6(4, family, random),
            "uae.g1.u07.l05.regroup_ones_for_tens" =>
                BuildUaeGrade1Unit6(5, family, random),
            "uae.g1.u07.l06.subtract_tens" =>
                BuildUaeGrade1Unit6(6, family, random),
            "uae.g1.u07.l07.count_back_tens" =>
                BuildUaeGrade1Unit6(7, family, random),
            "uae.g1.u07.l08.inverse_tens" =>
                BuildUaeGrade1Unit6(8, family, random),
            "uae.g1.u08.l01.tally_fives" =>
                BuildUaeG1Final29(74, family, random),
            "uae.g1.u08.l02.record_table" =>
                BuildUaeG1Final29(75, family, random),
            "uae.g1.u08.l03.make_picture_graph" =>
                BuildUaeG1Final29(76, family, random),
            "uae.g1.u08.l04.read_picture_graph" =>
                BuildUaeG1Final29(77, family, random),
            "uae.g1.u08.l05.make_bar_graph" =>
                BuildUaeG1Final29(78, family, random),
            "uae.g1.u08.l06.read_bar_graph" =>
                BuildUaeG1Final29(79, family, random),
            "uae.g1.u09.l01.compare_lengths" =>
                BuildUaeG1Final29(80, family, random),
            "uae.g1.u09.l02.order_lengths" =>
                BuildUaeG1Final29(81, family, random),
            "uae.g1.u09.l03.nonstandard_measure" =>
                BuildUaeG1Final29(82, family, random),
            "uae.g1.u09.l04.guess_check_length" =>
                BuildUaeG1Final29(83, family, random),
            "uae.g1.u09.l05.analog_hour" =>
                BuildUaeG1Final29(84, family, random),
            "uae.g1.u09.l06.digital_hour" =>
                BuildUaeG1Final29(85, family, random),
            "uae.g1.u09.l07.analog_half" =>
                BuildUaeG1Final29(86, family, random),
            "uae.g1.u09.l08.digital_half" =>
                BuildUaeG1Final29(87, family, random),
            "uae.g1.u09.l09.hour_or_half" =>
                BuildUaeG1Final29(88, family, random),
            "uae.g1.u10.l01.square_rectangle" =>
                BuildUaeG1Final29(89, family, random),
            "uae.g1.u10.l02.triangle_trapezoid" =>
                BuildUaeG1Final29(90, family, random),
            "uae.g1.u10.l03.circle_properties" =>
                BuildUaeG1Final29(91, family, random),
            "uae.g1.u10.l04.compare_shapes" =>
                BuildUaeG1Final29(92, family, random),
            "uae.g1.u10.l05.compose_2d" =>
                BuildUaeG1Final29(93, family, random),
            "uae.g1.u10.l06.more_composite_2d" =>
                BuildUaeG1Final29(94, family, random),
            "uae.g1.u10.l07.logical_shape_clue" =>
                BuildUaeG1Final29(95, family, random),
            "uae.g1.u10.l08.equal_shares" =>
                BuildUaeG1Final29(96, family, random),
            "uae.g1.u10.l09.halves_equal" =>
                BuildUaeG1Final29(97, family, random),
            "uae.g1.u10.l10.fourths_equal" =>
                BuildUaeG1Final29(98, family, random),
            "uae.g1.u11.l01.cubes_prisms" =>
                BuildUaeG1Final29(99, family, random),
            "uae.g1.u11.l02.cones_cylinders" =>
                BuildUaeG1Final29(100, family, random),
            "uae.g1.u11.l03.three_d_pattern" =>
                BuildUaeG1Final29(101, family, random),
            "uae.g1.u11.l04.compose_solids" =>
                BuildUaeG1Final29(102, family, random),
            "usccss.g1.u01.l01.count_collection_to20" =>
                BuildG1L01Counting(random, scale),
            "usccss.g1.u01.l01.add_dot_cubes_to10" =>
                BuildG1L01CubeAddition(random, scale),
            "number.whole.add.direct" =>
                BuildWholeAdd(random, scale),
            "number.whole.subtract.direct" =>
                BuildWholeSubtract(random, scale),
            "number.lcm.two_numbers" =>
                BuildLcm(random, scale),
            "percentages.of_quantity.direct" =>
                BuildPercentageOfQuantity(random, scale, "percentages.of_quantity.direct"),
            "percentages.core.of_quantity" =>
                BuildPercentageOfQuantity(random, scale, "percentages.core.of_quantity"),
            "percentages.core.increase_decrease" =>
                BuildPercentageIncreaseDecrease(random, scale),
            "usccss.g11.alg2.u01.l01.affine_recurrence" =>
                BuildG11AffineSequence(random),
            "usccss.g11.alg2.u01.l01.hanoi_moves" =>
                BuildG11HanoiMoves(random),
            "usccss.g11.alg2.u01.l01.fibonacci_term" =>
                BuildG11FibonacciTerm(random),
            "usccss.g11.alg2.u01.l01.halving_fraction" =>
                BuildG11HalvingFraction(random),
            "usccss.g11.alg2.u01.l02.paper_piece_count" =>
                BuildG11PaperPieceCount(random),
            "usccss.g11.alg2.u01.l02.paper_piece_area" =>
                BuildG11PaperPieceArea(random),
            "usccss.g11.alg2.u01.l02.rational_geometric_term" =>
                BuildG11RationalGeometricTerm(random),
            "usccss.g11.alg2.u01.l02.growth_factor" =>
                BuildG11GeometricGrowthFactor(random),
            "sequences.core.arithmetic_nth_term" =>
                BuildArithmeticSequenceNthTerm(random, scale),
            "sequences.core.geometric_nth_term" =>
                BuildGeometricSequenceNthTerm(random, scale),
            "sequences.core.find_position_arithmetic" =>
                BuildArithmeticSequenceFindPosition(random, scale),
            "fractions.represent.interpret.fraction_bar" =>
                BuildFractionRepresentation(random, scale),
            "algebra.relationships.two_unknowns.total_difference" =>
                BuildTwoUnknowns(random, scale),
            "measurement.scale.equal_intervals.read_value" =>
                BuildScaleReading(random, scale, preferredVariant),
            "fractions.compare.unlike.common_denominator" =>
                BuildUnlikeFractionComparison(random, scale, preferredVariant),
            "fractions.compare.unlike.select_greater" =>
                BuildUnlikeFractionSelectGreater(random, scale),
            "fractions.compare.unlike.true_false" =>
                BuildUnlikeFractionTrueFalse(random, scale),
            "fractions.compare.unlike.order_three" =>
                BuildUnlikeFractionOrderThree(random, scale),
            "fractions.equivalent.missing_value" =>
                BuildEquivalentMissingValue(random, scale, family, "Complete the equivalent fraction"),
            "fractions.equivalent.recognize" =>
                BuildEquivalentMissingValue(random, scale, family, "Find the missing numerator so both fractions have the same value"),
            "fractions.equivalent.generate_multiple" =>
                BuildEquivalentMissingValue(random, scale + 1, family, "Generate the equivalent fraction by completing the numerator"),
            "fractions.equivalent.number_line" =>
                BuildEquivalentMissingValue(random, scale, family, "The fractions mark the same point on a number line. Complete the numerator"),
            "fractions.equivalent.reduce_common_factor" =>
                BuildEquivalentReduction(random, scale),
            "ratio.unit_rate.direct" =>
                BuildUnitRate(random, scale),
            "ratio.unit_rate.equivalent_ratio" =>
                BuildEquivalentRatio(random, scale),
            "ratio.unit_rate.divide_total" =>
                BuildRatioDivideTotal(random, scale),
            "statistics.center_spread.mean" =>
                BuildStatisticsMean(random, scale),
            "statistics.center_spread.median" =>
                BuildStatisticsMedian(random, scale),
            "statistics.center_spread.range" =>
                BuildStatisticsRange(random, scale),
            "probability.theoretical.simple_event" =>
                BuildTheoreticalProbabilitySimple(random, scale),
            "probability.theoretical.complement" =>
                BuildTheoreticalProbabilityComplement(random, scale),
            "probability.theoretical.two_coins_exactly_one" =>
                BuildTwoCoinProbability(random),
            "number.whole.add_subtract.within_10.build" or
            "number.whole.add_subtract.within_10.apply" or
            "number.whole.add_subtract.across_ten.build" or
            "number.whole.add_subtract.across_ten.apply" or
            "number.whole.add_subtract.within_100.build" or
            "number.whole.add_subtract.within_100.apply" or
            "number.whole.add_subtract.columnar.build" or
            "number.whole.add_subtract.columnar.apply" =>
                BuildWholeAddSubtract(family, random, scale),
            "number.whole.add_subtract.comparative.build" or
            "number.whole.add_subtract.comparative.apply" =>
                BuildComparativeDifference(family, random, scale),
            "number.whole.add_subtract.complement_100.build" or
            "number.whole.add_subtract.complement_100.apply" =>
                BuildComplementTo100(family, random),
            "number.whole.multiply.fact_recall.build" or
            "number.whole.multiply.fact_recall.apply" =>
                BuildMultiplicationFact(family, random),
            "number.whole.divide.with_remainder.build" or
            "number.whole.divide.with_remainder.apply" =>
                BuildDivisionWithRemainder(family, random, scale),
            "usccss.g4.u06.l19.partial_quotients_remainders" => BuildG4PartialQuotientsRemainder(random),
            "usccss.g4.u06.l20.contextual_remainder_decisions" => BuildG4ContextualRemainder(random),
            "usccss.g4.u06.l21.multistep_contexts" => BuildG4MultiStepContext(random),
            "usccss.g4.u06.l23.four_operation_contexts" => BuildG4FourOperations(random),
            "usccss.g4.u06.l24.language_population_comparisons" => BuildG4LanguageComparisons(random),
            "usccss.g4.u09.l05.two_digit_partial_products" => BuildG4TwoDigitPartialProducts(random),
            "usccss.g4.u08.l07.polygon_perimeter_sum" => BuildG4PolygonPerimeterSum(random),
            "usccss.g4.u08.l04.symmetry_axis_counts" => BuildG4SymmetryAxisCounts(random),
            "usccss.g4.u07.l13.additive_angle_part" => BuildG4AdditiveAnglePart(random),
            "usccss.g4.u07.l12.classify_angles_by_degree_bounds" => BuildG4AngleClassificationByBounds(random),
            "usccss.g4.u07.l10.perpendicular_right_angle_judgment" => BuildG4PerpendicularRightAngleJudgment(random),
            "usccss.g4.u07.l09.protractor_dual_scale_reading" => BuildG4ProtractorDualScaleReading(random),
            "usccss.g4.u07.l08.full_turn_degree_benchmarks" => BuildG4FullTurnDegreeBenchmarks(random),
            "usccss.g4.u07.l07.clock_hour_angle_turn_compare" => BuildG4ClockHourAngleTurnCompare(random),
            "usccss.g4.u07.l06.compare_angle_openings" => BuildG4CompareAngleOpenings(random),
            "usccss.g4.u07.l05.angle_common_vertex_rays" => BuildG4AngleCommonVertexRays(random),
            "usccss.g4.u07.l03.dot_grid_parallel_intersect" => BuildG4ParallelIntersectDotGrid(random),
            "usccss.g4.u07.l02.geometry_cards_number_talk" => BuildG4PointLineRayCardAndNumberTalk(random),
            "usccss.g4.u07.l01.geometric_description_precision" => BuildG4GeometricDescriptionPrecision(random),
            "usccss.g4.u06.l25.flower_garland_patterns" => BuildG4FlowerPatterns(random),
            "usccss.g4.u06.l22.convert_l_shaped_area_perimeter" => BuildG4MeasurementConversions(random),
            "fractions.of_quantity.build" or
            "fractions.of_quantity.apply" =>
                BuildFractionOfQuantity(family, random, scale),
            "fractions.add_subtract.within_one.build" or
            "fractions.add_subtract.within_one.apply" or
            "fractions.add_subtract.same_denominator.mixed.build" or
            "fractions.add_subtract.same_denominator.mixed.apply" or
            "fractions.add_subtract.related.build" or
            "fractions.add_subtract.related.apply" or
            "fractions.add_subtract.common_denominator.build" or
            "fractions.add_subtract.common_denominator.apply" =>
                BuildFractionAddSubtract(family, random, scale),
            "fractions.compare.benchmark" =>
                BuildFractionBenchmark(random, scale),
            "geometry.coordinate.gradient_between_points" =>
                BuildCoordinateGradient(random, scale),
            "geometry.coordinate.evaluate_linear_rule" =>
                BuildCoordinateLinearRule(random, scale),
            "geometry.coordinate.y_intercept_from_rule" =>
                BuildCoordinateIntercept(random, scale),
            "geometry.angles.parallel_lines" =>
                BuildParallelLineAngle(random, scale),
            "geometry.angles.supplementary" =>
                BuildSupplementaryAngle(random),
            "geometry.angles.algebraic_supplementary" =>
                BuildAlgebraicSupplementaryAngle(random, scale),
            "geometry.similarity.find_missing_length" =>
                BuildSimilarityMissingLength(random, scale),
            "geometry.similarity.scale_factor" =>
                BuildSimilarityScaleFactor(random, scale),
            "geometry.congruence.identify_criterion" =>
                BuildCongruenceCriterion(random),
            "geometry.surface_area.rectangular_prism" =>
                BuildRectangularPrismSurfaceArea(random, scale, "geometry.surface_area.rectangular_prism"),
            "geometry.surface_area_volume.rectangular_prism_surface_area" =>
                BuildRectangularPrismSurfaceArea(random, scale, "geometry.surface_area_volume.rectangular_prism_surface_area"),
            "geometry.volume.rectangular_prism" =>
                BuildRectangularPrismVolume(random, scale, "geometry.volume.rectangular_prism"),
            "geometry.surface_area_volume.rectangular_prism_volume" =>
                BuildRectangularPrismVolume(random, scale, "geometry.surface_area_volume.rectangular_prism_volume"),
            "geometry.rectangle.area.exact" =>
                BuildRectangleArea(random, scale, "geometry.rectangle.area.exact"),
            "geometry.perimeter_area.rectangle_area" =>
                BuildRectangleArea(random, scale, "geometry.perimeter_area.rectangle_area"),
            "geometry.rectangle.perimeter.exact" =>
                BuildRectanglePerimeter(random, scale, "geometry.rectangle.perimeter.exact"),
            "geometry.perimeter_area.rectangle_perimeter" =>
                BuildRectanglePerimeter(random, scale, "geometry.perimeter_area.rectangle_perimeter"),
            "geometry.perimeter_area.triangle_area" =>
                BuildTriangleArea(random, scale),
            "geometry.perimeter_area.rectangle_missing_side" =>
                BuildRectangleMissingSide(random, scale),
            "usccss.g4.u06.l15.area_partial_quotients" => BuildG4MuralMissingSide(random, false),
            "usccss.g4.u06.l15.mural_complete_columns" => BuildG4MuralMissingSide(random, true),
            "usccss.g4.u06.l16.base_ten_equal_groups" => BuildG4BaseTenDivision(random),
            "usccss.g4.u06.l18.partial_quotients_recording" => BuildG4PartialQuotients(random),
            "geometry.polygons.interior_angle_sum" =>
                BuildPolygonInteriorAngleSum(random, scale),
            "geometry.polygons.missing_interior_angle" =>
                BuildPolygonMissingInteriorAngle(random, scale),
            "geometry.polygons.regular_interior_angle" =>
                BuildRegularPolygonInteriorAngle(random, scale),
            "geometry.right_triangle.pythagorean.exact" =>
                BuildPythagorean(random, scale),
            "geometry.right_triangle.pythagorean.find_leg_exact" =>
                BuildPythagoreanFindLeg(random, scale),
            "trigonometry.right_triangle.ratio_exact" =>
                BuildTrigonometricRatio(random, scale),
            "trigonometry.right_triangle.find_side_exact" =>
                BuildTrigonometricFindSide(random, scale, false),
            "trigonometry.right_triangle.find_angle_exact" =>
                BuildTrigonometricFindAngle(random),
            "trigonometry.modelling.contextual" =>
                BuildTrigonometricFindSide(random, scale, true),
            "vectors.add.exact_rational" =>
                BuildVectorBinary(random, scale, "vectors.add.exact_rational", false),
            "vectors.subtract.exact_rational" =>
                BuildVectorBinary(random, scale, "vectors.subtract.exact_rational", true),
            "vectors.scalar_multiply.exact_rational" =>
                BuildVectorScalarMultiply(random, scale),
            "vectors.dot.exact_rational" =>
                BuildVectorDot(random, scale),
            "vectors.magnitude.exact" =>
                BuildVectorMagnitude(random, scale),
            "vectors.between_points.exact" =>
                BuildVectorBetweenPoints(random, scale),
            ExactLinearEquationQuestionFactory.FamilyId =>
                BuildLinearEquation(random, scale),
            "algebra.linear.variables_both_sides" =>
                BuildLinearEquationBothSides(random, scale),
            ExactLinearInequalityQuestionFactory.FamilyId =>
                BuildLinearInequality(random, scale),
            "algebra.linear.inequality.variables_both_sides" =>
                BuildLinearInequalityBothSides(random, scale),
            _ => UnsupportedFamily(family)
        };
    }

    private static ExactProblem UnsupportedFamily(string family)
    {
        MathematicsObservability.Record(MathematicsMetricKind.Unsupported);
        throw new InvalidOperationException($"Unsupported exact Mathematics question family: {family}");
    }

    private static ExactProblem BuildWholeAdd(Random random, int scale)
    {
        var max = 20 * scale + 20;
        var left = random.Next(0, max + 1);
        var right = random.Next(0, max + 1);
        return Problem(
            "number.whole.add.direct",
            $"Calculate {left} + {right}.",
            "Add the two whole numbers and check by subtracting either addend from the total.",
            AssessmentItemType.Numeric,
            ("left", left),
            ("right", right));
    }

    private static ExactProblem BuildWholeSubtract(Random random, int scale)
    {
        var max = 20 * scale + 20;
        var right = random.Next(0, max + 1);
        var difference = random.Next(0, max + 1);
        var left = right + difference;
        return Problem(
            "number.whole.subtract.direct",
            $"Calculate {left} − {right}.",
            "Subtract the second whole number and check by adding the difference back.",
            AssessmentItemType.Numeric,
            ("left", left),
            ("right", right));
    }

    private static ExactProblem BuildLcm(Random random, int scale)
    {
        var common = random.Next(2, 4 + scale);
        var leftFactor = random.Next(2, 5 + scale);
        var rightFactor = random.Next(2, 5 + scale);
        var left = common * leftFactor;
        var right = common * rightFactor;
        return Problem(
            "number.lcm.two_numbers",
            $"Find the least common multiple of {left} and {right}.",
            "List prime factors or successive multiples, then verify the result is divisible by both numbers and no smaller positive common multiple exists.",
            AssessmentItemType.Numeric,
            ("left", left),
            ("right", right));
    }

    private static ExactProblem BuildPercentageOfQuantity(Random random, int scale, string family)
    {
        int[] percentages = [10, 20, 25, 40, 50, 60, 75, 80];
        var percent = percentages[random.Next(percentages.Length)];
        var unit = 20;
        var quantity = unit * random.Next(2, 6 + scale * 2);
        return Problem(
            family,
            $"Find {percent}% of {quantity}.",
            "Convert the percentage to a fraction over 100, multiply by the quantity, and simplify. Check by reversing the percentage relationship.",
            AssessmentItemType.Numeric,
            ("percent", percent),
            ("quantity", quantity));
    }

    private static ExactProblem BuildPercentageIncreaseDecrease(Random random, int scale)
    {
        int[] percentages = [10, 20, 25, 40, 50];
        var percent = percentages[random.Next(percentages.Length)];
        var quantity = 20 * random.Next(2, 7 + scale * 2);
        var direction = random.Next(0, 2) == 0 ? 1 : -1;
        var verb = direction > 0 ? "Increase" : "Decrease";

        return Problem(
            "percentages.core.increase_decrease",
            $"{verb} {quantity} by {percent}%.",
            "Find the stated percentage of the original quantity, then add it for an increase or subtract it for a decrease. Verify with the multiplier method.",
            AssessmentItemType.Numeric,
            ("percent", percent),
            ("quantity", quantity),
            ("direction", direction));
    }

    private static int G11IntPow(int value, int exponent)
    {
        var output=1;
        for(var i=0;i<exponent;i++) output=checked(output*value);
        return output;
    }

    private static ExactProblem BuildG11PaperPieceCount(Random random)
    {
        var cuts=random.Next(1,8);
        return Problem("usccss.g11.alg2.u01.l02.paper_piece_count",
            $"An 8-by-10-inch sheet is split into equal halves at each step. How many pieces exist after {cuts} cuts?",
            "The count doubles after every cut. Start at one piece with zero cuts.",
            AssessmentItemType.Numeric,("cutCount",cuts));
    }

    private static ExactProblem BuildG11PaperPieceArea(Random random)
    {
        var cuts=random.Next(1,7);
        return Problem("usccss.g11.alg2.u01.l02.paper_piece_area",
            $"A sheet has area 80 square inches. Every cut halves every existing equal piece. What is the exact area of each piece after {cuts} cuts? Give a reduced fraction if needed.",
            "Conserve the total sheet area: 80 divided by 2^n pieces.",
            AssessmentItemType.ShortAnswer,("cutCount",cuts));
    }

    private static ExactProblem BuildG11RationalGeometricTerm(Random random)
    {
        var variants=new (int N,int D)[]{(1,2),(1,10),(3,4),(2,1),(3,1),(4,1)};
        var ratio=variants[random.Next(variants.Length)];
        var first=random.Next(2,9);
        var term=random.Next(3,6);
        return Problem("usccss.g11.alg2.u01.l02.rational_geometric_term",
            $"A geometric sequence has first term {first} and constant multiplier {ratio.N}/{ratio.D}. What is term {term}? Give an exact fraction or integer.",
            "Multiply by the common ratio once for each move to a new term, preserving exact rational values.",
            AssessmentItemType.ShortAnswer,("first",first),
            ("ratioNumerator",ratio.N),("ratioDenominator",ratio.D),("term",term));
    }

    private static ExactProblem BuildG11GeometricGrowthFactor(Random random)
    {
        var choices=new (int Previous,int Next)[]{(256,192),(40,120),(1000,500),(2,8),(200,20),(24,12)};
        var row=choices[random.Next(choices.Length)];
        return Problem("usccss.g11.alg2.u01.l02.growth_factor",
            $"Two successive terms in a sequence are {row.Previous} and {row.Next}. Assuming a geometric rule, what is the exact growth factor?",
            "Divide the later term by the earlier nonzero term and simplify.",
            AssessmentItemType.ShortAnswer,("previous",row.Previous),("next",row.Next));
    }

    // G11 Algebra 2 U01:L01 first-party recurrence question families.
    // Explicitly separate the numerical response from teacher-scored
    // explanations and legal-move demonstrations.
    private static int G11AffineRecurrenceTerm(int first, int multiplier, int offset, int n)
    {
        var term=first;
        for(var position=1;position<n;position++)
            term=checked(multiplier*term+offset);
        return term;
    }

    private static int G11HanoiMinimalMoves(int n) => checked((1 << n)-1);

    private static int G11FibonacciTerm(int n)
    {
        if(n<=2)return n-1;
        var a=0;var b=1;
        for(var i=3;i<=n;i++)
        {
            var next=checked(a+b);
            a=b;
            b=next;
        }
        return b;
    }

    private static ExactProblem BuildG11AffineSequence(Random random)
    {
        var first=random.Next(-3,5);
        var multiplier=random.Next(0,2)==0?2:-2;
        var offset=multiplier==2?-1:3;
        var n=random.Next(3,7);
        return Problem(
            "usccss.g11.alg2.u01.l01.affine_recurrence",
            $"A sequence starts with {first}. Each following term is {multiplier} times the previous term plus ({offset}). What is term {n}?",
            "Start with the first term and apply the stated operation exactly once per step. Do not reverse subtraction or change the initial value.",
            AssessmentItemType.Numeric,
            ("first",first),("multiplier",multiplier),("offset",offset),("n",n));
    }

    private static ExactProblem BuildG11HanoiMoves(Random random)
    {
        var n=random.Next(3,9);
        return Problem(
            "usccss.g11.alg2.u01.l01.hanoi_moves",
            $"Under the legal three-peg, one-disc transfer rules, what is the smallest number of moves for {n} discs?",
            "Move n-1 smaller discs aside, move the largest once, then transfer n-1 back. Explain why both subproblems are necessary.",
            AssessmentItemType.Numeric,("n",n));
    }

    private static ExactProblem BuildG11HalvingFraction(Random random)
    {
        var n=random.Next(4,8);
        return Problem(
            "usccss.g11.alg2.u01.l01.halving_fraction",
            $"A sequence starts 1/2 and each next term is half the previous term. What is term {n}? Enter an exact fraction.",
            "Halving the fraction n-1 times produces 1/2^n. Enter a fraction with the correct numerator and denominator.",
            AssessmentItemType.ShortAnswer,("n",n));
    }

    private static ExactProblem BuildG11FibonacciTerm(Random random)
    {
        var n=random.Next(5,11);
        return Problem(
            "usccss.g11.alg2.u01.l01.fibonacci_term",
            $"A sequence begins 0,1 and each later term is the sum of its previous two terms. Counting 0 as term 1, find term {n}.",
            "Keep both previous terms, add them to form the next, then slide the two-term window forward.",
            AssessmentItemType.Numeric,("n",n));
    }

    private static ExactProblem BuildArithmeticSequenceNthTerm(Random random, int scale)
    {
        var first = random.Next(-5 * scale, 10 * scale + 1);
        var difference = NonZero(random, -5 - scale * 2, 6 + scale * 2);
        var n = random.Next(5, 10 + scale * 6);
        return Problem(
            "sequences.core.arithmetic_nth_term",
            $"An arithmetic sequence starts {first}, {first + difference}, {first + 2 * difference}, ... Find term {n}.",
            "Identify the constant difference and use aₙ = a₁ + (n − 1)d. Substitute the original first term, difference and position to verify.",
            AssessmentItemType.Numeric,
            ("first", first),
            ("difference", difference),
            ("n", n));
    }

    private static ExactProblem BuildGeometricSequenceNthTerm(Random random, int scale)
    {
        var first = NonZero(random, -5 - scale, 6 + scale);
        var ratio = random.Next(2, Math.Min(5, 3 + scale) + 1);
        var n = random.Next(3, Math.Min(7, 4 + scale) + 1);
        return Problem(
            "sequences.core.geometric_nth_term",
            $"A geometric sequence starts {first}, {first * ratio}, {first * ratio * ratio}, ... Find term {n}.",
            "Identify the constant ratio and use aₙ = a₁r^(n−1). Rebuild the term by repeated multiplication to verify.",
            AssessmentItemType.Numeric,
            ("first", first),
            ("ratio", ratio),
            ("n", n));
    }

    private static ExactProblem BuildArithmeticSequenceFindPosition(Random random, int scale)
    {
        var first = random.Next(-5 * scale, 10 * scale + 1);
        var difference = NonZero(random, 1, 5 + scale * 2);
        var expectedN = random.Next(5, 10 + scale * 6);
        var target = first + (expectedN - 1) * difference;
        return Problem(
            "sequences.core.find_position_arithmetic",
            $"The arithmetic sequence starts {first}, {first + difference}, {first + 2 * difference}, ... Which term is {target}?",
            "Set aₙ = a₁ + (n − 1)d equal to the target, solve for n, then substitute the position back into the sequence rule.",
            AssessmentItemType.Numeric,
            ("first", first),
            ("difference", difference),
            ("target", target),
            ("expectedN", expectedN));
    }

    private static ExactProblem BuildFractionRepresentation(Random random, int scale)
    {
        var denominator = random.Next(3, 7 + scale);
        var numerator = random.Next(1, denominator + 1);
        return Problem(
            "fractions.represent.interpret.fraction_bar",
            $"A fraction bar is divided into {denominator} equal parts and {numerator} part(s) are shaded. Write the shaded fraction.",
            "The denominator is the number of equal parts; the numerator is the number shaded. Simplify only if the fraction has a common factor.",
            AssessmentItemType.ShortAnswer,
            ("numerator", numerator),
            ("denominator", denominator));
    }

    private static ExactProblem BuildTwoUnknowns(Random random, int scale)
    {
        var smaller = random.Next(4, 15 + 5 * scale);
        var difference = random.Next(2, 8 + 3 * scale);
        var larger = smaller + difference;
        var total = smaller + larger;

        return Problem(
            "algebra.relationships.two_unknowns.total_difference",
            $"Two boxes contain {total} counters altogether. Box B has {difference} more counters than Box A. How many counters are in Box A?",
            "Use both relationships: A + B = total and B - A = difference. Solve them together, then check both equations.",
            AssessmentItemType.Numeric,
            ("total", total),
            ("difference", difference));
    }

    private static ExactProblem BuildScaleReading(
        Random random,
        int scale,
        int? preferredVariant = null)
    {
        int[] intervalChoices = [2, 4, 5, 10];
        var intervals = intervalChoices[random.Next(intervalChoices.Length)];
        var step = random.Next(2, 7 + scale * 3);
        var start = random.Next(0, 10 + scale * 10);
        var end = start + intervals * step;
        var pointer = random.Next(1, intervals);
        var variant = preferredVariant.HasValue
            ? QuestionVariantPolicy.NormalizeSlot(preferredVariant.Value) % 4
            : random.Next(0, 4);
        var prompt = variant switch
        {
            0 => $"A scale runs from {start} to {end} in {intervals} equal intervals. What value is at the tick {pointer} interval(s) after {start}?",
            1 => $"The interval from {start} to {end} is split into {intervals} equal steps. Find the value after {pointer} step(s).",
            2 => $"Complete the scale: starting at {start}, there are {intervals} equal intervals to {end}. What number belongs at interval {pointer}?",
            _ => $"A measuring scale has endpoints {start} and {end} with {intervals} equal gaps. Determine the reading at the {pointer}th gap from {start}."
        };

        return Problem(
            "measurement.scale.equal_intervals.read_value",
            prompt,
            "Find one interval by subtracting the endpoints and dividing by the number of intervals, then count from the start value.",
            AssessmentItemType.Numeric,
            ("start", start),
            ("end", end),
            ("intervals", intervals),
            ("pointer", pointer),
            ("variant", variant));
    }

    private static ExactProblem BuildUnlikeFractionComparison(
        Random random,
        int scale,
        int? preferredVariant = null)
    {
        var denominator1 = random.Next(3, 8 + scale * 2);
        var denominator2 = random.Next(3, 9 + scale * 2);
        while (denominator2 == denominator1)
            denominator2 = random.Next(3, 9 + scale * 2);

        var numerator1 = random.Next(1, denominator1);
        var numerator2 = random.Next(1, denominator2);
        var variant = preferredVariant.HasValue
            ? QuestionVariantPolicy.NormalizeSlot(preferredVariant.Value) % 4
            : random.Next(0, 4);
        var prompt = variant switch
        {
            0 => $"Compare {numerator1}/{denominator1} and {numerator2}/{denominator2}. Enter <, >, or =.",
            1 => $"Complete the statement with <, >, or =: {numerator1}/{denominator1} __ {numerator2}/{denominator2}.",
            2 => $"Which relation makes this comparison true? {numerator1}/{denominator1} __ {numerator2}/{denominator2}. Enter <, >, or =.",
            _ => $"Decide whether {numerator1}/{denominator1} is less than, greater than, or equal to {numerator2}/{denominator2}. Answer with <, >, or =."
        };

        return Problem(
            "fractions.compare.unlike.common_denominator",
            prompt,
            "Compare the exact cross-products (or use a common denominator); do not compare denominators by size alone.",
            AssessmentItemType.ShortAnswer,
            ("n1", numerator1),
            ("d1", denominator1),
            ("n2", numerator2),
            ("d2", denominator2),
            ("variant", variant));
    }

    private static ExactProblem BuildUnlikeFractionSelectGreater(Random random, int scale)
    {
        var (n1, d1, n2, d2) = GenerateUnlikeFractionPair(random, scale, requireDifferentValues: true);
        return Problem(
            "fractions.compare.unlike.select_greater",
            $"Which fraction is greater: {n1}/{d1} or {n2}/{d2}? Enter left, right, or equal.",
            "Compare the fractions using exact cross-products or a common denominator, then identify which side has the greater value.",
            AssessmentItemType.ShortAnswer,
            ("n1", n1),
            ("d1", d1),
            ("n2", n2),
            ("d2", d2));
    }

    private static ExactProblem BuildUnlikeFractionTrueFalse(Random random, int scale)
    {
        var (n1, d1, n2, d2) = GenerateUnlikeFractionPair(random, scale, requireDifferentValues: false);
        var actual = Compare(n1 * d2, n2 * d1);
        var actualCode = RelationCode(actual);
        var useActual = random.Next(0, 2) == 0;
        var claimedCode = useActual
            ? actualCode
            : Enumerable.Range(0, 3)
                .Where(code => code != actualCode)
                .ElementAt(random.Next(0, 2));
        var claimed = RelationFromCode(claimedCode);

        return Problem(
            "fractions.compare.unlike.true_false",
            $"True or false: {n1}/{d1} {claimed} {n2}/{d2}.",
            "Check the statement by using a common denominator or exact cross-products. The truth value depends on the fraction values, not denominator size.",
            AssessmentItemType.ShortAnswer,
            ("n1", n1),
            ("d1", d1),
            ("n2", n2),
            ("d2", d2),
            ("claimedRelation", claimedCode));
    }

    private static ExactProblem BuildUnlikeFractionOrderThree(Random random, int scale)
    {
        var denominator1 = random.Next(3, 8 + scale * 2);
        var denominator2 = random.Next(3, 9 + scale * 2);
        while (denominator2 == denominator1)
            denominator2 = random.Next(3, 9 + scale * 2);
        var denominator3 = random.Next(3, 10 + scale * 2);
        while (denominator3 == denominator1 || denominator3 == denominator2)
            denominator3 = random.Next(3, 10 + scale * 2);

        var numerator1 = random.Next(1, denominator1);
        var numerator2 = random.Next(1, denominator2);
        var numerator3 = random.Next(1, denominator3);

        var guard = 0;
        while ((numerator1 * denominator2 == numerator2 * denominator1 ||
                numerator1 * denominator3 == numerator3 * denominator1 ||
                numerator2 * denominator3 == numerator3 * denominator2) &&
               guard++ < 64)
        {
            numerator2 = random.Next(1, denominator2);
            numerator3 = random.Next(1, denominator3);
        }

        if (numerator1 * denominator2 == numerator2 * denominator1 ||
            numerator1 * denominator3 == numerator3 * denominator1 ||
            numerator2 * denominator3 == numerator3 * denominator2)
        {
            throw new InvalidOperationException(
                "Unable to generate three distinct unlike fractions.");
        }

        return Problem(
            "fractions.compare.unlike.order_three",
            $"Order the fractions from least to greatest: A = {numerator1}/{denominator1}, B = {numerator2}/{denominator2}, C = {numerator3}/{denominator3}. Enter the letters, for example A < B < C.",
            "Compare the fractions pairwise using exact cross-products or a shared denominator, then place all three in ascending order.",
            AssessmentItemType.ShortAnswer,
            ("n1", numerator1),
            ("d1", denominator1),
            ("n2", numerator2),
            ("d2", denominator2),
            ("n3", numerator3),
            ("d3", denominator3));
    }

    private static (int N1, int D1, int N2, int D2) GenerateUnlikeFractionPair(
        Random random,
        int scale,
        bool requireDifferentValues)
    {
        var denominator1 = random.Next(3, 8 + scale * 2);
        var denominator2 = random.Next(3, 9 + scale * 2);
        while (denominator2 == denominator1)
            denominator2 = random.Next(3, 9 + scale * 2);

        var numerator1 = random.Next(1, denominator1);
        var numerator2 = random.Next(1, denominator2);
        var guard = 0;
        while (requireDifferentValues &&
               numerator1 * denominator2 == numerator2 * denominator1 &&
               guard++ < 64)
        {
            numerator2 = random.Next(1, denominator2);
        }

        if (requireDifferentValues &&
            numerator1 * denominator2 == numerator2 * denominator1)
        {
            throw new InvalidOperationException(
                "Unable to generate distinct unlike fractions.");
        }

        return (numerator1, denominator1, numerator2, denominator2);
    }

    private static ExactProblem BuildEquivalentMissingValue(
        Random random,
        int scale,
        string family,
        string instruction)
    {
        var denominator = random.Next(3, 8 + scale);
        var numerator = random.Next(1, denominator);
        var factor = random.Next(2, 4 + scale);
        var targetDenominator = denominator * factor;

        return Problem(
            family,
            $"{instruction}: {numerator}/{denominator} = ?/{targetDenominator}.",
            "Equivalent fractions preserve value: multiply numerator and denominator by the same factor, then verify by cross-products.",
            AssessmentItemType.Numeric,
            ("baseN", numerator),
            ("baseD", denominator),
            ("targetD", targetDenominator));
    }

    private static ExactProblem BuildEquivalentReduction(Random random, int scale)
    {
        var targetDenominator = random.Next(3, 8 + scale);
        var targetNumerator = random.Next(1, targetDenominator);
        var factor = random.Next(2, 4 + scale);
        var largeNumerator = targetNumerator * factor;
        var largeDenominator = targetDenominator * factor;

        return Problem(
            "fractions.equivalent.reduce_common_factor",
            $"Reduce {largeNumerator}/{largeDenominator} to an equivalent fraction with denominator {targetDenominator}. What is the numerator?",
            "Divide numerator and denominator by the same common factor, then verify the two fractions by cross-products.",
            AssessmentItemType.Numeric,
            ("largeN", largeNumerator),
            ("largeD", largeDenominator),
            ("targetD", targetDenominator));
    }

    private static ExactProblem BuildUnitRate(Random random, int scale)
    {
        var quantity = random.Next(2, 6 + scale);
        var unitRate = random.Next(2, 9 + scale * 3);
        var total = quantity * unitRate;

        return Problem(
            "ratio.unit_rate.direct",
            $"{total} items are produced in {quantity} minutes. How many items are produced per 1 minute?",
            "A unit rate is the amount for one unit. Divide the total amount by the number of minutes and verify by multiplication.",
            AssessmentItemType.Numeric,
            ("total", total),
            ("quantity", quantity));
    }

    private static ExactProblem BuildEquivalentRatio(Random random, int scale)
    {
        var baseSecond = random.Next(2, 6 + scale);
        var unitRate = random.Next(2, 9 + scale * 2);
        var baseFirst = baseSecond * unitRate;
        var factor = random.Next(2, 5 + scale);
        var targetSecond = baseSecond * factor;

        return Problem(
            "ratio.unit_rate.equivalent_ratio",
            $"{baseFirst} items in {baseSecond} minutes has the same unit rate as ? items in {targetSecond} minutes. Find the missing number.",
            "Equivalent ratios have the same quotient. Scale both quantities by the same factor and verify the unit rate.",
            AssessmentItemType.Numeric,
            ("baseFirst", baseFirst),
            ("baseSecond", baseSecond),
            ("targetSecond", targetSecond));
    }

    private static ExactProblem BuildRatioDivideTotal(Random random, int scale)
    {
        var firstPart = random.Next(1, 5 + scale);
        var secondPart = random.Next(1, 5 + scale);
        while (secondPart == firstPart)
            secondPart = random.Next(1, 5 + scale);

        var unit = random.Next(2, 8 + scale * 2);
        var total = (firstPart + secondPart) * unit;
        var askedPart = random.Next(0, 2) == 0 ? firstPart : secondPart;

        return Problem(
            "ratio.unit_rate.divide_total",
            $"A total of {total} is divided in the ratio {firstPart}:{secondPart}. Find the share corresponding to {askedPart} ratio part(s).",
            "Add the ratio parts to find the total number of equal parts, divide the total by that number, then multiply by the requested number of parts. Check both shares add to the original total.",
            AssessmentItemType.Numeric,
            ("firstPart", firstPart),
            ("secondPart", secondPart),
            ("total", total),
            ("askedPart", askedPart));
    }

    private static ExactProblem BuildStatisticsMean(Random random, int scale)
    {
        var mean = random.Next(3, 12 + scale * 4);
        var d1 = random.Next(0, 4 + scale);
        var d2 = random.Next(0, 4 + scale);
        int[] values = [mean - d1, mean - d2, mean, mean + d2, mean + d1];
        Array.Sort(values);

        return Problem(
            "statistics.center_spread.mean",
            $"Find the mean of {string.Join(", ", values)}.",
            "Add all five values and divide by 5. Verify that five times the mean equals the original total.",
            AssessmentItemType.Numeric,
            ("count", 5),
            ("v1", values[0]),
            ("v2", values[1]),
            ("v3", values[2]),
            ("v4", values[3]),
            ("v5", values[4]));
    }

    private static ExactProblem BuildStatisticsMedian(Random random, int scale)
    {
        var values = Enumerable.Range(0, 5)
            .Select(_ => random.Next(1, 15 + scale * 5))
            .OrderBy(x => x)
            .ToArray();

        return Problem(
            "statistics.center_spread.median",
            $"Find the median of {string.Join(", ", values)}.",
            "Order the five values from least to greatest. With five observations, the median is the third value. Verify there are two observations on each side.",
            AssessmentItemType.Numeric,
            ("v1", values[0]),
            ("v2", values[1]),
            ("v3", values[2]),
            ("v4", values[3]),
            ("v5", values[4]));
    }

    private static ExactProblem BuildStatisticsRange(Random random, int scale)
    {
        var min = random.Next(0, 10 + scale * 2);
        var range = random.Next(2, 10 + scale * 4);
        var max = min + range;

        return Problem(
            "statistics.center_spread.range",
            $"A data set has minimum {min} and maximum {max}. Find its range.",
            "Range is maximum minus minimum. Add the range back to the minimum to verify the maximum.",
            AssessmentItemType.Numeric,
            ("min", min),
            ("max", max));
    }

    private static ExactProblem BuildTheoreticalProbabilitySimple(Random random, int scale)
    {
        var total = random.Next(4, 9 + scale * 3);
        var favourable = random.Next(1, total);

        return Problem(
            "probability.theoretical.simple_event",
            $"A bag contains {total} equally likely counters, of which {favourable} are red. Find P(red). Give an exact fraction.",
            "For equally likely outcomes, theoretical probability is favourable outcomes divided by total outcomes. Verify the fraction lies between 0 and 1.",
            AssessmentItemType.ShortAnswer,
            ("favourable", favourable),
            ("total", total));
    }

    private static ExactProblem BuildTheoreticalProbabilityComplement(Random random, int scale)
    {
        var total = random.Next(4, 9 + scale * 3);
        var favourable = random.Next(1, total);

        return Problem(
            "probability.theoretical.complement",
            $"A bag contains {total} equally likely counters and {favourable} are red. Find P(not red). Give an exact fraction.",
            "The complement contains total minus favourable outcomes. Equivalently use 1 − P(red), then verify the two probabilities sum to 1.",
            AssessmentItemType.ShortAnswer,
            ("favourable", favourable),
            ("total", total));
    }

    private static ExactProblem BuildTwoCoinProbability(Random random)
    {
        var variant = random.Next(1, 20);
        return Problem(
            "probability.theoretical.two_coins_exactly_one",
            "Two independent fair coins are tossed. Find the probability of getting exactly one head. Give an exact fraction.",
            "List HH, HT, TH, TT. Exactly one head occurs in HT and TH, so 2 of 4 equally likely outcomes are favourable.",
            AssessmentItemType.ShortAnswer,
            ("variant", variant));
    }

    private static (int Minimum, int Maximum) UaeGrade1CountingRange(string family) => family switch
    {
        "uae.g1.u01.l01.read_write_1_5" => (1, 5),
        "uae.g1.u01.l02.zero_cardinality" => (0, 0),
        "uae.g1.u01.l03.read_write_6_8" => (6, 8),
        "uae.g1.u01.l04.read_write_9_10" => (9, 10),
        "uae.g1.u01.l05.read_write_11_15" => (11, 15),
        "uae.g1.u01.l06.read_write_16_20" => (16, 20),
        _ => (-1, -1)
    };

    // Extends the existing exact-counting kernel; no new Mathematics engine.
    private static ExactProblem BuildUaeGrade1Count(string family, Random random)
    {
        var (minimum, maximum) = UaeGrade1CountingRange(family);
        if (minimum < 0)
            throw new InvalidOperationException($"Unsupported UAE Grade 1 counting family: {family}");
        var count = random.Next(minimum, maximum + 1);
        var scene = random.Next(0, 10);
        string[] objects = ["خرزات", "أزرار", "نجوم", "أقلام", "مكعبات",
                            "زهور", "دوائر", "كرات", "ملصقات", "أشكال"];
        var firstFrame = new string('●', Math.Min(count, 10)).PadRight(10, '□');
        var secondFrame = count > 10
            ? " | " + new string('●', count - 10).PadRight(10, '□')
            : string.Empty;
        return Problem(
            family,
            $"عدّ {objects[scene]} النموذج واكتب الرقم: {firstFrame}{secondFrame}. كم عنصرًا؟",
            "عدّ كل نقطة مرة واحدة؛ وعند وجود إطارين اجمع عشرة الإطار الأول وآحاد الإطار الثاني. الإطارات الفارغة تمثل صفرًا.",
            AssessmentItemType.Numeric,
            ("count", count), ("scene", scene));
    }

    private static ExactProblem BuildUaeGrade1AdditionStory(Random random, string family)
    {
        var left = random.Next(1, 8);
        var right = random.Next(1, Math.Min(4, 10 - left) + 1);
        var scene = random.Next(0, 6);
        string[] objects = ["كرات", "مكعبات", "أزهار", "أقلام", "ملصقات", "حبات خرز"];
        var first = new string('●', left);
        var second = new string('●', right);
        var prompt = family switch
        {
            "uae.g1.u02.l02.model_addition" =>
                $"أكمل مخطط الجزأين والكل: الجزء الأول {left}: {first} | الجزء الثاني {right}: {second} | الكل: ؟",
            "uae.g1.u02.l03.addition_number_sentences" =>
                $"اكتب المجموع لإكمال جملة الجمع: {left} + {right} = ؟. تحقق بالنقاط: {first} | {second}",
            "uae.g1.u02.l05.vertical_addition" =>
                $"أكمل الجمع الرأسي: {left}\n+ {right}\n────\n؟. تحقق بالنقاط: {first} | {second}",
            "uae.g1.u02.l06.write_story_equation" =>
                $"اقرأ القصة: كان معي {left} {objects[scene]}، ثم حصلت على {right} أخرى. كم أصبح المجموع؟ أفهم العددين، أخطط للجمع، أكمل جملة {left} + {right} = ؟، ثم أتحقق بالنقاط: {first} | {second}",
            _ => $"لدي {left} {objects[scene]} ثم أضفت {right} أخرى. الجزء الأول: {first} | الجزء الثاني: {second}. كم العدد الكلي؟"
        };
        return Problem(family,
            prompt,
            "مثل المجموعتين في شكلين مختلفين، ضمّهما، ثم عدّ جميع العناصر مرة واحدة.",
            AssessmentItemType.Numeric,
            ("left", left), ("right", right), ("scene", scene));
    }

    private static ExactProblem BuildUaeGrade1RelatedFacts(Random random)
    {
        const string family = "uae.g1.u03.l13.related_add_subtract";
        var left = random.Next(1, 8);
        var right = random.Next(1, Math.Min(4, 10 - left) + 1);
        var total = left + right;
        var removeLeft = random.Next(0, 2);
        var removed = removeLeft == 1 ? left : right;
        return Problem(family,
            $"أربط الجمع بالطرح: {left} + {right} = {total}. إذا أزلت {removed} من {total} فكم يبقى؟ أكمل {total} − {removed} = ؟، واشرح كيف يساعدك الجمع.",
            "أبدأ من الكل، وأزيل أحد الجزأين، وأجد الجزء الآخر.",
            AssessmentItemType.Numeric,
            ("left", left), ("right", right), ("removeLeft", removeLeft));
    }

    private static ExactProblem BuildUaeGrade1SubtractionStory(Random random, string family)
    {
        var whole = family == "uae.g1.u03.l12.subtract_10" ? 10 :
            family == "uae.g1.u03.l11.subtract_9" ? 9 :
            family == "uae.g1.u03.l10.subtract_8" ? 8 :
            family == "uae.g1.u03.l09.subtract_6_7" ? random.Next(6, 8) :
            family == "uae.g1.u03.l08.subtract_4_5" ? random.Next(4, 6) : random.Next(2, 11);
        var removed = family == "uae.g1.u03.l04.subtract_zero_or_all" ?
            (random.Next(0, 2) == 0 ? 0 : whole) :
            family == "uae.g1.u03.l07.compare_groups" ? random.Next(1, whole) :
            family is "uae.g1.u03.l08.subtract_4_5" or "uae.g1.u03.l09.subtract_6_7" or "uae.g1.u03.l10.subtract_8" or "uae.g1.u03.l11.subtract_9" or "uae.g1.u03.l12.subtract_10" ? random.Next(0, whole + 1) : random.Next(1, whole + 1);
        var scene = random.Next(0, 6);
        string[] objects = ["أقلام", "زهور", "أوراق", "بطاقات", "كرات", "مكعبات"];
        var before = new string('●', whole);
        var after = new string('●', whole - removed);
        var prompt = family is "uae.g1.u03.l08.subtract_4_5" or "uae.g1.u03.l09.subtract_6_7" or "uae.g1.u03.l10.subtract_8" or "uae.g1.u03.l11.subtract_9" or "uae.g1.u03.l12.subtract_10" ?
            $"الطرح من {whole}: أرسم {whole} قطع {before} وأشطب {removed}؛ أكمل {whole} − {removed} = ؟. القطع المتبقية: {after}" :
            family == "uae.g1.u03.l07.compare_groups" ?
            $"قارن المجموعتين: المجموعة الأولى {whole} {objects[scene]} {before}، والمجموعة الثانية {removed} {objects[scene]} {new string('○', removed)}. صِل بينها واحدًا لواحد. كم عنصرًا أكثر في المجموعة الأولى؟ أكمل {whole} − {removed} = ؟." :
            family == "uae.g1.u03.l05.vertical_subtraction" ?
            $"أكمل الطرح الرأسي: {whole}\n− {removed}\n────\n؟. تحقق بالرسم: {before} ثم اشطب {removed} نقاط." :
            family == "uae.g1.u03.l04.subtract_zero_or_all" ?
            $"لدي {whole} عناصر؛ أزيل {removed}. ما عدد العناصر الباقية؟ أكمل {whole} − {removed} = ؟. ابدأ بالنقاط {before} ثم عدّ ما بقي: {after}" :
            family == "uae.g1.u03.l03.subtraction_number_sentences" ?
            $"أكمل جملة الطرح العددية: {whole} − {removed} = ؟. أشطب {removed} من {whole} نقاط: {before}، ثم أعد ما بقي." :
            family == "uae.g1.u03.l02.model_subtraction" ?
            $"مخطط جزء/جزء/كل: الكل {whole} {before}، الجزء المعروف {removed}، الجزء المجهول ؟. أكمل {whole} − {removed} = ؟. المتبقي: {after}" :
            $"كان هناك {whole} من {objects[scene]}. أُخذ منها {removed}. كم بقي؟ {whole} − {removed} = ؟. قبل: {before} | بعد: {after}";
        return Problem(family,
            prompt,
            "أمثل الكل وأحذف ما ذهب ثم أعد الباقي، وأتحقق بجمع الباقي والمزال.",
            AssessmentItemType.Numeric,
            ("whole", whole), ("removed", removed), ("scene", scene));
    }

    // Scoped addition-through-ten mechanic uses existing exact engine, not a new engine.
    // Reuses the exact family engine for addition commutativity; not a new engine.
    // Uses existing exact numeric question engine; three-addend grouping within 20.
    private static ExactProblem BuildUaeGrade1Unit4Lesson01(Random random)
    {
        const string family = "uae.g1.u05.l01.count_back_1_2_3";

        var start = random.Next(4, 21);
        var steps = random.Next(1, Math.Min(3, start) + 1);
        var result = start - steps;
        var line = string.Join(" ← ", Enumerable.Range(0, steps + 1).Select(i => (start - i).ToString(CultureInfo.InvariantCulture)));
        return Problem(family,
            $"ابدأ من {start}، وعد إلى الخلف {steps} خطوات: {line}. أكمل {start} − {steps} = ؟",
            "ابدأ بعدد معلوم، وانتقل خطوة إلى الخلف لكل واحد مطروح دون أن تعد نقطة البداية.",
            AssessmentItemType.Numeric, ("start", start), ("steps", steps), ("result", result));
    }

    private static ExactProblem BuildUaeGrade1Unit4Lesson02(Random random)
    {
        const string family = "uae.g1.u05.l02.number_line_subtract";

        var start = random.Next(5, 21);
        var hops = random.Next(1, Math.Min(9, start) + 1);
        var result = start - hops;
        var line = string.Join("  ", Enumerable.Range(0, start + 1));
        return Problem(family,
            $"خط الأعداد: {line}. ابدأ من {start} وارسم {hops} قفزات متساوية إلى الخلف. عند أي عدد تصل؟",
            "أبدأ بالعدد الكلي، وأتحرك يسارًا بعدد القفزات المطلوب؛ نقطة النهاية هي الفرق.",
            AssessmentItemType.Numeric, ("start", start), ("hops", hops), ("result", result));
    }

    private static ExactProblem BuildUaeGrade1Unit4Lesson03(Random random)
    {
        const string family = "uae.g1.u05.l03.doubles_subtraction";
        var part = random.Next(1, 11);
        var whole = 2 * part;
        var scene = random.Next(0, 3);
        var items = new[] { "مكعبات", "خرزات", "بطاقات" };
        return Problem(family,
            $"أكوّن مجموعتين متساويتين من {items[scene]}، في كل مجموعة {part}. " +
            $"المجموع {whole}؛ إذا أزلت مجموعة كاملة، فأكمل {whole} − {part} = ؟.",
            "عند حذف مجموعة من مضاعف مكوّن من مجموعتين متساويتين تبقى المجموعة الأخرى.",
            AssessmentItemType.Numeric, ("part", part), ("whole", whole), ("scene", scene));
    }

    private static ExactProblem BuildUaeGrade1Unit4Lesson04(Random random)
    {
        const string family = "uae.g1.u05.l04.story_subtraction_equation";

        var whole = random.Next(5, 21);
        var removed = random.Next(1, Math.Min(whole - 1, 10) + 1);
        var remaining = whole - removed;
        var scene = random.Next(0, 3);
        var units = new[] { "أقلام", "مكعبات", "بطاقات" };
        return Problem(family,
            $"في صندوق {whole} من {units[scene]}، أعطينا {removed} منها لصديق. مثّل المسألة واكتب جملة طرح ثم اكتب العدد المتبقي: {whole} − {removed} = ؟.",
            "أفهم وأخطط وأمثل الكل، ثم أحذف ما غادر، وأتحقق من الفرق بجمع الجزءين.",
            AssessmentItemType.Numeric, ("whole", whole), ("removed", removed), ("remaining", remaining), ("scene", scene));
    }

    private static ExactProblem BuildUaeGrade1Unit4Lesson05(Random random)
    {
        const string family = "uae.g1.u05.l05.subtract_through_ten";

        var whole = random.Next(11, 19);
        var toTen = whole - 10;
        var removed = random.Next(toTen + 1, 10);
        var rest = removed - toTen;
        var result = whole - removed;
        return Problem(family,
            $"حل {whole} − {removed} بتكوين عشرة: اطرح {toTen} أولًا للوصول إلى ١٠، ثم اطرح {rest} من ١٠. ما الناتج؟",
            "جزّئ العدد المطروح إلى جزء للوصول للعشرة وجزء متبقٍ، ثم أطرح الجزأين بالتتابع.",
            AssessmentItemType.Numeric, ("whole", whole), ("removed", removed),
            ("toTen", toTen), ("rest", rest), ("result", result));
    }

    private static ExactProblem BuildUaeGrade1Unit4Lesson06(Random random)
    {
        const string family = "uae.g1.u05.l06.related_add_sub_facts";

        var a = random.Next(2, 10);
        var b = random.Next(2, 11);
        var total = a + b;
        return Problem(family,
            $"أعرف حقيقة الجمع {a} + {b} = {total}. أستخدمها لإكمال حقيقة الطرح المترابطة: {total} − {a} = ؟.",
            "الكل ناقص جزء يساوي الجزء الآخر؛ أتحقق بالجمع.",
            AssessmentItemType.Numeric, ("a", a), ("b", b), ("total", total));
    }

    private static ExactProblem BuildUaeGrade1Unit4Lesson07(Random random)
    {
        const string family = "uae.g1.u05.l07.fact_family_four_equations";

        var a = random.Next(1, 10);
        var b = random.Next(1, 10);
        var whole = a + b;
        var mode = random.Next(0, 4);
        var prompt = mode switch
        {
            0 => $"مثلث حقائق: الجزآن {a} و{b}، الكل {whole}. أكمل: {a} + {b} = ؟",
            1 => $"مثلث حقائق: الجزآن {a} و{b}، الكل {whole}. أكمل الجمع المعكوس: {b} + {a} = ؟",
            2 => $"مثلث حقائق: الجزآن {a} و{b}، الكل {whole}. أكمل: {whole} − {a} = ؟",
            _ => $"مثلث حقائق: الجزآن {a} و{b}، الكل {whole}. أكمل: {whole} − {b} = ؟"
        };
        return Problem(family, prompt,
            "تتكون عائلة الحقائق من الجزأين والكل، ومنها حقيقتا جمع وحقيقتا طرح.",
            AssessmentItemType.Numeric, ("a", a), ("b", b), ("whole", whole), ("mode", mode));
    }

    private static ExactProblem BuildUaeGrade1Unit4Lesson08(Random random)
    {
        const string family = "uae.g1.u05.l08.missing_addend_part_whole";

        var whole = random.Next(4, 21);
        var known = random.Next(1, Math.Min(whole, 11));
        var missing = whole - known;
        var mode = random.Next(0, 2);
        var prompt = mode == 0
            ? $"أكمل جملة الجمع: {known} + ؟ = {whole}. استخدم حقيقة الطرح لإيجاد الجزء الناقص."
            : $"لدي مجموعتان مجموعهما {whole}، إحدى المجموعتين فيها {known} قطع. أكمل: ؟ + {known} = {whole}.";
        return Problem(family, prompt,
            "الجزء المجهول يساوي الكل ناقص الجزء المعروف؛ أتحقق بالجمع.",
            AssessmentItemType.Numeric, ("whole", whole), ("known", known), ("missing", missing), ("mode", mode));
    }

    // First-party UAE Grade 1 Place Value questions, fixed lesson-scoped family IDs.
    // Reuses the existing exact math engine; no separate engine and no copied publisher exercises.
    private static int UaeGrade1Unit5Ordinal(string family) => family switch
    {
        "uae.g1.u06.l01.ordinal_1_10" => 1,
        "uae.g1.u06.l02.numbers_11_19" => 2,
        "uae.g1.u06.l03.groups_of_ten" => 3,
        "uae.g1.u06.l04.count_tens_fils" => 4,
        "uae.g1.u06.l05.ten_and_some_more" => 5,
        "uae.g1.u06.l06.tens_and_ones" => 6,
        "uae.g1.u06.l07.make_table" => 7,
        "uae.g1.u06.l08.numbers_to_100" => 8,
        "uae.g1.u06.l09.ten_more_ten_less" => 9,
        "uae.g1.u06.l10.count_fives_fils" => 10,
        "uae.g1.u06.l11.compare_models" => 11,
        "uae.g1.u06.l12.compare_symbols" => 12,
        "uae.g1.u06.l13.hundreds_tens_ones" => 13,
        "uae.g1.u06.l14.count_to_120" => 14,
        "uae.g1.u06.l15.read_write_to_120" => 15,
        _ => -1
    };

    private static bool VerifyUaeGrade1Unit5(string family,
        IReadOnlyDictionary<string, int> p, string answer)
    {
        var lesson = UaeGrade1Unit5Ordinal(family);
        if (lesson < 1 || !p.TryGetValue("n", out var n) ||
            !p.TryGetValue("aux", out var aux) ||
            !p.TryGetValue("mode", out var mode) ||
            !p.TryGetValue("result", out var result)) return false;
        var expected = lesson switch
        {
            1 or 2 or 13 or 15 => n,
            3 or 4 or 7 => n * 10,
            5 or 6 or 8 => n * 10 + aux,
            9 => n + mode * 10,
            10 => n * 5,
            11 => Math.Max(n, aux),
            12 => n > aux ? 1 : n < aux ? -1 : 0,
            14 => n + 1,
            _ => -1000
        };
        var valid = lesson switch
        {
            1 => n is >= 1 and <= 10 && aux == 0 && mode is 0 or 1,
            2 => n is >= 11 and <= 19 && aux == n - 10 && mode is >= 0 and <= 2,
            3 or 4 => n is >= 1 and <= 10 && aux == 0 && mode is 0 or 1,
            5 => n is >= 1 and <= 9 && aux is >= 1 and <= 9 && mode == 0,
            6 => n is >= 1 and <= 9 && aux is >= 0 and <= 9 && mode == 0,
            7 => n is >= 1 and <= 8 && aux == 10 && mode is 0 or 1,
            8 => n is >= 1 and <= 10 && aux is >= 0 and <= 9 &&
                 (n != 10 || aux == 0) && mode == 0,
            9 => n is >= 10 and <= 90 && (mode is -1 or 1) && aux == 0,
            10 => n is >= 1 and <= 20 && aux == 0 && mode == 0,
            11 or 12 => n is >= 0 and <= 100 && aux is >= 0 and <= 100 && mode == 0,
            13 => n is >= 100 and <= 120 && aux == n % 10 && mode == n / 10 % 10,
            14 => n is >= 1 and <= 119 && aux == 0 && mode == 0,
            15 => n is >= 1 and <= 120 && aux == 0 && mode == 0,
            _ => false
        };
        if (!valid || expected != result) return false;
        if (lesson == 12)
            return answer.Trim() == (expected == 1 ? ">" : expected == -1 ? "<" : "=");
        var normalized = new string(answer.Trim().Select(c =>
            c is >= '٠' and <= '٩' ? (char)('0' + c - '٠') :
            c is >= '۰' and <= '۹' ? (char)('0' + c - '۰') : c).ToArray());
        return int.TryParse(normalized, NumberStyles.Integer,
            CultureInfo.InvariantCulture, out var value) && value == expected;
    }

    // Edulytics-original Grade 1 U6: exact lesson-bounded two-digit arithmetic.
    private static int UaeGrade1Unit6Ordinal(string family) => family switch
    {
        "uae.g1.u07.l01.add_tens" => 1,
        "uae.g1.u07.l02.count_on_tens_ones" => 2,
        "uae.g1.u07.l03.add_tens_ones" => 3,
        "uae.g1.u07.l04.guess_check_revise" => 4,
        "uae.g1.u07.l05.regroup_ones_for_tens" => 5,
        "uae.g1.u07.l06.subtract_tens" => 6,
        "uae.g1.u07.l07.count_back_tens" => 7,
        "uae.g1.u07.l08.inverse_tens" => 8,
        _ => -1
    };

    private static bool VerifyUaeGrade1Unit6(string family,
        IReadOnlyDictionary<string, int> p, string answer)
    {
        var lesson = UaeGrade1Unit6Ordinal(family);
        if (lesson < 1 || !p.TryGetValue("a", out var a) ||
            !p.TryGetValue("b", out var b) || !p.TryGetValue("mode", out var mode) ||
            !p.TryGetValue("result", out var result)) return false;
        var valid = lesson switch
        {
            1 => a is >= 10 and <= 90 && a % 10 == 0 &&
                 b is >= 10 and <= 90 && b % 10 == 0 && a + b <= 100 && mode == 0,
            2 => a is >= 10 and <= 89 && b >= 1 && a + b <= 100 &&
                 (mode == 0 && b <= 9 || mode == 1 && b % 10 == 0 && b <= 70),
            3 => a is >= 10 and <= 89 && b is >= 1 and <= 89 && a + b <= 100 &&
                 a % 10 + b % 10 < 10 && mode == 0,
            4 => a is >= 10 and <= 90 && b is >= 1 and <= 49 &&
                 b < a && mode == 0,
            5 => a is >= 11 and <= 19 && b is >= 2 and <= 9 &&
                 a % 10 + b >= 10 && mode == 0,
            // Subtracting 100 from 100 is valid: the generator can select
            // ten complete tens from ten complete tens. The old <=90 bound
            // incorrectly rejected correct solver output for that case.
            6 or 7 => a is >= 10 and <= 100 && a % 10 == 0 &&
                 b is >= 10 and <= 100 && b % 10 == 0 && b <= a && mode == 0,
            8 => a is >= 10 and <= 90 && a % 10 == 0 &&
                 b is >= 10 and <= 90 && b % 10 == 0 &&
                 (mode == 0 ? a + b <= 100 : b <= a) && mode is 0 or 1,
            _ => false
        };
        var expected = lesson switch
        {
            1 or 2 or 3 or 5 => a + b,
            4 or 6 or 7 => a - b,
            8 => mode == 0 ? a + b : a - b,
            _ => -1
        };
        if (!valid || result != expected) return false;
        var normalized = new string(answer.Trim().Select(c =>
            c is >= '٠' and <= '٩' ? (char)('0' + c - '٠') :
            c is >= '۰' and <= '۹' ? (char)('0' + c - '۰') : c).ToArray());
        return int.TryParse(normalized, NumberStyles.Integer,
            CultureInfo.InvariantCulture, out var value) && value == expected;
    }

    // First-party Grade 1 Arabic U7–U10 exact practice; open-ended modeling
    // is expressly teacher-reviewed, not asserted as automatically graded.
    private static int UaeG1Final29Ordinal(string family) => family switch
    {
        "uae.g1.u08.l01.tally_fives" => 74,
        "uae.g1.u08.l02.record_table" => 75,
        "uae.g1.u08.l03.make_picture_graph" => 76,
        "uae.g1.u08.l04.read_picture_graph" => 77,
        "uae.g1.u08.l05.make_bar_graph" => 78,
        "uae.g1.u08.l06.read_bar_graph" => 79,
        "uae.g1.u09.l01.compare_lengths" => 80,
        "uae.g1.u09.l02.order_lengths" => 81,
        "uae.g1.u09.l03.nonstandard_measure" => 82,
        "uae.g1.u09.l04.guess_check_length" => 83,
        "uae.g1.u09.l05.analog_hour" => 84,
        "uae.g1.u09.l06.digital_hour" => 85,
        "uae.g1.u09.l07.analog_half" => 86,
        "uae.g1.u09.l08.digital_half" => 87,
        "uae.g1.u09.l09.hour_or_half" => 88,
        "uae.g1.u10.l01.square_rectangle" => 89,
        "uae.g1.u10.l02.triangle_trapezoid" => 90,
        "uae.g1.u10.l03.circle_properties" => 91,
        "uae.g1.u10.l04.compare_shapes" => 92,
        "uae.g1.u10.l05.compose_2d" => 93,
        "uae.g1.u10.l06.more_composite_2d" => 94,
        "uae.g1.u10.l07.logical_shape_clue" => 95,
        "uae.g1.u10.l08.equal_shares" => 96,
        "uae.g1.u10.l09.halves_equal" => 97,
        "uae.g1.u10.l10.fourths_equal" => 98,
        "uae.g1.u11.l01.cubes_prisms" => 99,
        "uae.g1.u11.l02.cones_cylinders" => 100,
        "uae.g1.u11.l03.three_d_pattern" => 101,
        "uae.g1.u11.l04.compose_solids" => 102,
        _ => -1
    };

    private static bool VerifyUaeG1Final29(string family,
        IReadOnlyDictionary<string,int> p, string answer)
    {
        int lesson=UaeG1Final29Ordinal(family);
        if (lesson < 74 || lesson > 102 ||
            !p.TryGetValue("a",out var a) ||
            !p.TryGetValue("b",out var b) ||
            !p.TryGetValue("c",out var c) ||
            !p.TryGetValue("mode",out var mode) ||
            !p.TryGetValue("result",out var result)) return false;
        var valid=lesson switch
        {
            74 => a is >= 1 and <= 9 && b is >= 1 and <= 9 && c==0 && mode==0,
            75 or 76 or 79 => a is >= 1 and <= 9 && b is >= 1 and <= 9 && c==0 && mode==0,
            77 => a is >= 1 and <= 10 && b is >= 1 and <= 10 && c==0 && mode==0,
            78 => a is >= 1 and <= 9 && b is >= 1 and <= 9 && c==0 && mode==0,
            80 => a is >= 2 and <= 20 && b is >= 2 and <= 20 && c==0 && mode==0,
            81 => a is >= 2 and <= 20 && b is >= 2 and <= 20 && c is >= 2 and <= 20 && mode==0,
            82 => a is >= 1 and <= 12 && b==0 && c==0 && mode==0,
            83 => a is >= 10 and <= 25 && b is >= 1 and <= 9 && c==0 && mode==0,
            84 or 85 or 86 or 87 => a is >= 1 and <= 12 && b==0 && c==0 && mode==0,
            88 => a is >= 1 and <= 12 && b==0 && c==0 && mode is 0 or 1,
            89 or 90 or 92 or 95 => a is >= 1 and <= 10 && b==0 && c==0 && mode is 0 or 1,
            91 => a is >= 1 and <= 10 && b==0 && c==0 && mode is 0 or 1,
            93 or 94 => a is >= 1 and <= 5 && b is >= 1 and <= 5 && c==0 && mode==0,
            96 => a is >= 1 and <= 10 && b==0 && c==0 && mode is 0 or 1,
            97 => a is >= 2 and <= 20 && a%2==0 && b==0 && c==0 && mode is 0 or 1,
            98 => a is >= 4 and <= 24 && a%4==0 && b==0 && c==0 && mode is >= 0 and <= 2,
            99 or 100 => a is >= 1 and <= 10 && b==0 && c==0 && mode is 0 or 1,
            101 => a is >= 1 and <= 12 && b==0 && c==0 && mode is 0 or 1,
            102 => a is >= 1 and <= 4 && b is >= 1 and <= 4 && c==0 && mode==0,
            _ => false
        };
        if (!valid) return false;
        var expected=lesson switch
        {
            74 or 78 or 82 or 84 or 85 or 86 or 87 => a,
            75 or 76 or 79 or 93 or 94 or 102 => a+b,
            77 => Math.Abs(a-b),
            80 => Math.Max(a,b),
            81 => Math.Min(a,Math.Min(b,c)),
            83 => a-b,
            88 => mode * 30,
            89 => 4*a,
            90 or 92 or 95 => (mode==0 ? 3 : 4)*a,
            91 => mode==0 ? 0 : a,
            96 => (mode==0 ? 2 : 4)*a,
            97 => a/2,
            98 => a/4,
            99 => (mode==0 ? 6 : 8)*a,
            100 => (mode==0 ? 1 : 2)*a,
            101 => mode==0 ? (a%2==1 ? 1 : 2) : (a%2==1 ? 2 : 1),
            _ => -1
        };
        if (expected!=result) return false;
        var normalized=new string(answer.Trim().Select(c =>
            c is >= '٠' and <= '٩' ? (char)('0'+c-'٠') :
            c is >= '۰' and <= '۹' ? (char)('0'+c-'۰') : c).ToArray());
        return int.TryParse(normalized, NumberStyles.Integer,
           CultureInfo.InvariantCulture,out var v) && v==expected;
    }

    private static ExactProblem BuildUaeG1Final29(int lesson,string family,Random random)
    {
        int a=0,b=0,c=0,mode=0,result=0;
        string question="";
        switch (lesson)
        {
            case 74:
                a=random.Next(1,10);b=random.Next(1,10);result=a;
                question=$"نتائج تصويت: التفاح {new string('│',a)}، الموز {new string('│',b)}. أعد تنظيم كل خمس علامات ثم أكتب أصوات التفاح.";
                break;
            case 75:
                a=random.Next(1,10);b=random.Next(1,10);result=a+b;
                question=$"سجل الجدول فئتي الكتب {a} والأقلام {b}. ما العدد الكلي المسجل؟ اشرح طريقة تنظيم الصفوف.";
                break;
            case 76:
                a=random.Next(1,10);b=random.Next(1,10);result=a+b;
                question=$"تمثيل بالصور من تأليف Edulytics (كل رمز = عنصر): الطائرات {new string('●',a)} والسيارات {new string('●',b)}. كم رمزًا أحتاج لرسم الفئتين؟";
                break;
            case 77:
                a=random.Next(1,11);b=random.Next(1,11);result=Math.Abs(a-b);
                question=$"مفتاح الرسم: كل ★ يمثل طفلًا. الأخضر {new string('★',a)} والأصفر {new string('★',b)}. كم الفرق في عدد الأطفال؟";
                break;
            case 78:
                a=random.Next(1,10);b=random.Next(1,10);result=a;
                question=$"ارسم عمودًا للفئة أ بارتفاع {a} وحدات وعمودًا للفئة ب بارتفاع {b} وحدات، وكل مربع يمثل عنصرًا. كم مربعًا في العمود أ؟";
                break;
            case 79:
                a=random.Next(1,10);b=random.Next(1,10);result=a+b;
                question=$"رسم أعمدة على مقياس وحدة: القراءة {new string('■',a)} والرسم {new string('■',b)}. كم إجمالي الوحدات في العمودين؟";
                break;
            case 80:
                a=random.Next(2,21);b=random.Next(2,21);result=Math.Max(a,b);
                question=$"شريطان من مكعبات متساوية، طول الأول {a} وحدات والثاني {b} وحدات. بعد محاذاة طرفيهما ما طول الشريط الأطول؟";
                break;
            case 81:
                a=random.Next(2,21);b=random.Next(2,21);c=random.Next(2,21);
                result=Math.Min(a,Math.Min(b,c));
                question=$"أرتب أشرطة أطوالها {a} و{b} و{c} مكعبات. ما طول أقصر شريط؟";
                break;
            case 82:
                a=random.Next(1,13);result=a;
                question=$"شريط طوله {new string('▭',a)}، كل ▭ مكعب وحدة واحدة، متلاصقة بلا فجوات. كم وحدة طول الشريط؟";
                break;
            case 83:
                a=random.Next(10,26);b=random.Next(1,10);result=a-b;
                question=$"الطول الكلي للشريط {a} وحدات، وغطيت منه {b}. خمّن الباقي أولًا ثم تحقق وعدل تخمينك؛ كم وحدة لم تُغط؟";
                break;
            case 84:
                a=random.Next(1,13);result=a;
                var hours=new[]{"🕐","🕑","🕒","🕓","🕔","🕕","🕖","🕗","🕘","🕙","🕚","🕛"};
                question=$"الساعة التناظرية {hours[a-1]}. عقرب الدقائق عند ١٢؛ ما رقم الساعة؟";
                break;
            case 85:
                a=random.Next(1,13);result=a;
                question=$"شاشة الساعة الرقمية: {a:00}:00. ما رقم الساعة عندما تكون الدقائق صفرًا؟";
                break;
            case 86:
                a=random.Next(1,13);result=a;
                var halfHours=new[]{"🕜","🕝","🕞","🕟","🕠","🕡","🕢","🕣","🕤","🕥","🕦","🕧"};
                question=$"ساعة تناظرية تشير إلى {halfHours[a-1]}، والعقرب الطويل عند ٦؛ أي ساعة ونصف يمثلها؟ اكتب رقم الساعة.";
                break;
            case 87:
                a=random.Next(1,13);result=a;
                question=$"تظهر الساعة الرقمية {a:00}:30. اكتب رقم الساعة التي مضى منها نصف ساعة.";
                break;
            case 88:
                a=random.Next(1,13);mode=random.Next(0,2);result=mode*30;
                question=$"الساعة الرقمية {a:00}:{(mode==0 ? "00" : "30")}. كم دقيقة مضت بعد بداية الساعة الحالية؟";
                break;
            case 89:
                a=random.Next(1,11);mode=random.Next(0,2);result=4*a;
                question=mode==0
                    ? $"أعرض {a} مربعات منفصلة □، ولكل مربع أربعة أضلاع مستقيمة. كم ضلعًا في المربعات جميعًا؟"
                    : $"أعرض {a} مستطيلات منفصلة ▭، ولكل مستطيل أربعة أضلاع مستقيمة. كم ضلعًا في المستطيلات جميعًا؟";
                break;
            case 90:
                a=random.Next(1,11);mode=random.Next(0,2);result=a*(mode==0?3:4);
                question=mode==0
                    ? $"أرتب {a} مثلثات منفصلة △، لكل منها ثلاثة أضلاع. كم ضلعًا في المثلثات كلها؟"
                    : $"أرتب {a} أشباه منحرف منفصلة، لكل منها أربعة أضلاع. كم ضلعًا لجميع الأشكال؟";
                break;
            case 91:
                a=random.Next(1,11);mode=random.Next(0,2);result=mode==0?0:a;
                question=mode==0
                    ? $"هناك {a} دوائر منفصلة ○ في الرسم. كم ضلعًا مستقيمًا في حدود هذه الدوائر كلها؟"
                    : $"هناك {a} دوائر منفصلة ○؛ لكل دائرة حد منحني مغلق واحد. كم حدًا منحنيًا مغلقًا لكل الدوائر؟";
                break;
            case 92:
                a=random.Next(1,11);mode=random.Next(0,2);result=a*(mode==0?3:4);
                question=mode==0
                    ? $"أقارن مجموعة فيها {a} مثلثات بمجموعة مستطيلات. كم رأسًا للمثلثات المنفصلة جميعًا؟"
                    : $"أقارن مجموعة فيها {a} مستطيلات بمجموعة مثلثات. كم رأسًا للمستطيلات المنفصلة جميعًا؟";
                break;
            case 93:
                a=random.Next(1,6);b=random.Next(1,6);result=a+b;
                question=$"أنشئ رسمًا هندسيًا أصليًا من {a} مثلثات و{b} مستطيلات دون تداخل. كم قطعة هندسية استعملت؟";
                break;
            case 94:
                a=random.Next(1,6);b=random.Next(1,6);result=a+b;
                question=$"أعيد ترتيب {a} مربعات و{b} مثلثات لأكوّن شكلاً مركبًا جديدًا دون إضافة أو حذف. كم قطعة تكوّن الشكل؟";
                break;
            case 95:
                a=random.Next(1,11);mode=random.Next(0,2);result=a*(mode==0?3:4);
                question=mode==0
                    ? $"أستدل على الشكل: له ثلاثة أضلاع وثلاثة رؤوس. لدي {a} أشكال منفصلة منه. كم رأسًا لجميعها؟"
                    : $"أستدل على الشكل: له أربعة أضلاع متساوية وأربع زوايا. لدي {a} أشكال منفصلة منه. كم رأسًا لجميعها؟";
                break;
            case 96:
                a=random.Next(1,11);mode=random.Next(0,2);result=a*(mode==0?2:4);
                question=$"أقسم {a} شرائط متساوية، كل شريط على حدة إلى {(mode==0?"نصفين متساويين":"أرباع متساوية")}. كم جزءًا متساويًا في جميع الشرائط بعد التقسيم؟";
                break;
            case 97:
                a=2*random.Next(1,11);mode=random.Next(0,2);result=a/2;
                question=mode==0
                    ? $"أقسم {a} دائرة بالتساوي بين طفلين، كم دائرة في نصف المجموعة؟"
                    : $"أوزع {a} بطاقة بالتساوي على صندوقين، كم بطاقة يمثل نصف المجموعة في كل صندوق؟";
                break;
            case 98:
                a=4*random.Next(1,7);mode=random.Next(0,3);result=a/4;
                question=mode switch
                {
                    0 => $"أقسم {a} قطعة على أربع مجموعات متساوية؛ كم قطعة في الربع الواحد؟",
                    1 => $"أوزع {a} بطاقة على أربعة صناديق بالتساوي، كم بطاقة في ربع المجموعة؟",
                    _ => $"أقسم {a} مكعبًا بين أربعة أطفال بالتساوي، كم مكعبًا يأخذ كل طفل؟"
                };
                break;
            case 99:
                a=random.Next(1,11);mode=random.Next(0,2);result=a*(mode==0?6:8);
                question=mode==0
                    ? $"أفحص {a} مكعبات منفصلة، لكل مكعب ستة أوجه مربعة. كم وجهًا مسطحًا لها إجمالًا؟"
                    : $"أفحص {a} مناشير مستطيلة منفصلة، لكل منشور ثمانية رؤوس. كم رأسًا للمجسمات جميعًا؟";
                break;
            case 100:
                a=random.Next(1,11);mode=random.Next(0,2);result=a*(mode==0?1:2);
                question=mode==0
                    ? $"هناك {a} مخاريط منفصلة، لكل مخروط قاعدة دائرية واحدة. كم قاعدة دائرية لها كلها؟"
                    : $"هناك {a} أسطوانات منفصلة، لكل أسطوانة قاعدتان دائريتان. كم قاعدة دائرية لها جميعًا؟";
                break;
            case 101:
                a=random.Next(1,13);mode=random.Next(0,2);
                result=mode==0 ? (a%2==1?1:2) : (a%2==1?2:1);
                question=mode==0
                    ? $"النمط المتناوب: مخروط، أسطوانة، مخروط، أسطوانة، ... ما رمز الشكل عند الموضع {a}؟ اكتب ١ للمخروط و٢ للأسطوانة."
                    : $"النمط المتناوب: أسطوانة، مخروط، أسطوانة، مخروط، ... ما رمز الشكل عند الموضع {a}؟ اكتب ١ للمخروط و٢ للأسطوانة.";
                break;
            case 102:
                a=random.Next(1,5);b=random.Next(1,5);result=a+b;
                question=$"أكوّن مجسمًا مركبًا من {a} مكعبات و{b} أسطوانات، دون تفكيك الأجزاء. كم جسمًا ثلاثي الأبعاد استعملت؟";
                break;
            default:throw new InvalidOperationException("Unsupported UAE G1 last 29 lesson ordinal");
        }
        return Problem(family,question,
            "أستخدم رسمًا أصليًا أو نموذجًا محسوسًا مناسبًا للمهارة، وأتحقق من العدد أو الزمن أو الشكل. تقييم الرسم والتفسير عمل المعلم.",
            AssessmentItemType.Numeric,("a",a),("b",b),("c",c),
            ("mode",mode),("result",result));
    }

    private static ExactProblem BuildUaeGrade1Unit6(int lesson, string family, Random random)
    {
        var a = 0; var b = 0; var mode = 0; var result = 0; var prompt = "";
        switch (lesson)
        {
            case 1:
                a = random.Next(1, 10) * 10;
                b = random.Next(1, 11 - a / 10) * 10;
                result = a + b;
                prompt = $"أمثل {a} بقضبان عشرات، ثم أضيف {b} بقضبان أخرى. كم الكل؟";
                break;
            case 2:
                a = random.Next(10, 90); mode = random.Next(0, 2);
                b = mode == 0 ? random.Next(1, Math.Min(9, 100 - a) + 1) :
                    random.Next(1, Math.Min(7, (100 - a) / 10) + 1) * 10;
                result = a + b;
                prompt = $"أبدأ من {a} وأعد تصاعديًا {(mode == 0 ? "بالآحاد" : "بالعشرات")} حتى أضيف {b}. ما المجموع؟";
                break;
            case 3:
                a = random.Next(11, 75);
                if (a % 10 == 9) a--;
                var onesMax = 9 - a % 10;
                var tensMax = (100 - a - onesMax) / 10;
                b = 10 * random.Next(0, Math.Max(1, Math.Min(8, tensMax + 1))) +
                    random.Next(1, onesMax + 1);
                result = a + b;
                prompt = $"أجمع الآحاد مع الآحاد والعشرات مع العشرات: {a} + {b} = ؟. اكتب المجموع.";
                break;
            case 4:
                a = random.Next(20, 71); b = random.Next(1, Math.Min(30, a - 1));
                result = a - b;
                var guess = Math.Max(0, result - 2);
                prompt = $"لدي {a} كرة منها {b} حمراء والباقي خضراء. أخمّن أن الخضراء {guess} وأتحقق من المجموع. كم كرة خضراء بالفعل؟";
                break;
            case 5:
                a = random.Next(11, 20);
                b = random.Next(10 - a % 10, 10);
                result = a + b;
                prompt = $"أجمع {a} + {b}. عند وصول الآحاد إلى عشرة أو أكثر أعيد تجميع عشر آحاد كعشرة. ما الناتج؟";
                break;
            case 6:
                a = random.Next(1, 11) * 10;
                b = random.Next(1, a / 10 + 1) * 10;
                result = a - b;
                prompt = $"لدي {a} مكعبًا في حزم من عشرة، وأزيل {b} مكعبًا في حزم كاملة. ما الفرق؟";
                break;
            case 7:
                a = random.Next(1, 11) * 10;
                b = random.Next(1, a / 10 + 1) * 10;
                result = a - b;
                var line = string.Join(" ← ", Enumerable.Range(0, b / 10 + 1).Select(i => a - i * 10));
                prompt = $"استخدم خط الأعداد، وابدأ عند {a} وعد تنازليًا بخطوات عشرية لإيجاد {a} − {b}: {line}. ما الفرق؟";
                break;
            case 8:
                a = random.Next(1, 10) * 10; mode = random.Next(0, 2);
                b = mode == 0 ? random.Next(1, 11 - a / 10) * 10 :
                    random.Next(1, a / 10 + 1) * 10;
                result = mode == 0 ? a + b : a - b;
                prompt = mode == 0
                    ? $"أكمل {a} + {b} = ؟، ثم اكتب حقيقة طرح عكسية للتحقق."
                    : $"أكمل {a} − {b} = ؟، ثم اكتب حقيقة جمع مترابطة للتحقق.";
                break;
            default: throw new InvalidOperationException("Unsupported UAE G1 U6 lesson");
        }
        return Problem(family, prompt,
            "أمثل العشرات والآحاد أو أختبر تخميني، وأتحقق من الجزء والكل والعملية المناسبة.",
            AssessmentItemType.Numeric, ("a", a), ("b", b), ("mode", mode), ("result", result));
    }

    private static ExactProblem BuildUaeGrade1Unit5(int lesson, string family, Random random)
    {
        var n = 0; var aux = 0; var mode = 0; var result = 0; var prompt = "";
        switch (lesson)
        {
            case 1:
                n = random.Next(1, 11); mode = random.Next(0, 2); result = n;
                prompt = $"في طابور عشرة أطفال، يوجد {n - 1} أطفال قبل الطفل المحدد من جهة {(mode == 0 ? "اليمين" : "اليسار")}. ما موضعه الترتيبي من تلك الجهة؟";
                break;
            case 2:
                n = random.Next(11, 20); aux = n - 10; mode = random.Next(0, 3); result = n;
                prompt = mode switch
                {
                    0 => $"إطار عشر خانات ممتلئ ومعه {aux} نقاط منفصلة. ما العدد؟",
                    1 => $"حزمة من عشرة مكعبات ومعها {aux} مكعبات مفردة. اكتب العدد.",
                    _ => $"لدينا عشرة آحاد مجمعة، ثم {aux} آحاد إضافية في صينية العد. ما العدد؟"
                };
                break;
            case 3:
                n = random.Next(1, 11); mode = random.Next(0, 2); result = n * 10;
                prompt = mode == 0
                    ? $"لدي {n} قضبان عشرات، في كل قضيب عشر قطع. كم قطعة؟"
                    : $"أعد بالقفزات العشرية {n} مرات من صفر على خط الأعداد. إلى أي عدد أصل؟";
                break;
            case 4:
                n = random.Next(1, 11); mode = random.Next(0, 2); result = n * 10;
                prompt = mode == 0
                    ? $"في المحفظة {n} عملات معدنية قيمة كل واحدة ١٠ فلوس. كم فلسًا في المحفظة؟"
                    : $"على الطاولة {n} عملات من فئة ١٠ فلوس؛ أعد كل عملة بعشرة. ما القيمة الكلية بالفلس؟";
                break;
            case 5:
                n = random.Next(1, 10); aux = random.Next(1, 10); result = n * 10 + aux;
                prompt = $"لدي {n} حزم من عشر خرزات ومعها {aux} خرزات منفردة. كم خرزة إجمالًا؟";
                break;
            case 6:
                n = random.Next(1, 10); aux = random.Next(0, 10); result = n * 10 + aux;
                prompt = $"جدول القيمة المكانية: العشرات {n} والآحاد {aux}. ما العدد؟";
                break;
            case 7:
                n = random.Next(1, 9); aux = 10; mode = random.Next(0, 2); result = n * 10;
                prompt = mode == 0
                    ? $"أكتب جدول قراءة: أنجز ١٠ بطاقات كل يوم؛ بعد {n} أيام كم بطاقة أتممت؟"
                    : $"في جدول صناديق الصف عشر بطاقات في كل صندوق. إذا كان لدي {n} صناديق، ما عدد البطاقات؟";
                break;
            case 8:
                n = random.Next(1, 11); aux = n == 10 ? 0 : random.Next(0, 10);
                result = n * 10 + aux;
                prompt = $"في جدول العشرات والآحاد {n} عشرات و{aux} آحاد. اكتب العدد حتى ١٠٠.";
                break;
            case 9:
                n = random.Next(10, 91); mode = random.Next(0, 2) == 0 ? -1 : 1;
                result = n + 10 * mode;
                prompt = $"ما العدد الذي يساوي {n} {(mode == 1 ? "زائد" : "ناقص")} عشرة؟ الآحاد ثابتة.";
                break;
            case 10:
                n = random.Next(1, 21); result = n * 5;
                prompt = $"أعد بالخمسات: لدي {n} عملات من فئة ٥ فلوس. كم فلسًا؟";
                break;
            case 11:
                n = random.Next(0, 101); aux = random.Next(0, 101);
                result = Math.Max(n, aux);
                prompt = $"أمثل العددين {n} و{aux} بقضبان عشرات وآحاد. ما قيمة العدد الأكبر؟";
                break;
            case 12:
                n = random.Next(0, 101); aux = random.Next(0, 101);
                result = n > aux ? 1 : n < aux ? -1 : 0;
                prompt = $"اكتب الرمز > أو < أو = المناسب للمقارنة {n} □ {aux}.";
                break;
            case 13:
                n = random.Next(100, 121); aux = n % 10; mode = n / 10 % 10;
                result = n;
                prompt = $"لدي مئة واحدة و{mode} عشرات و{aux} آحاد. اكتب العدد بالأرقام.";
                break;
            case 14:
                n = random.Next(1, 120); result = n + 1;
                prompt = $"أكمل العد بالترتيب في مخطط الأعداد إلى ١٢٠: {n}، ؟. ما العدد اللاحق؟";
                break;
            case 15:
                n = random.Next(1, 121); result = n;
                prompt = $"اقرأ العدد الممثل في مخطط الأعداد والموضع {n} ثم اكتبه بالأرقام. يراجع المعلم القراءة الشفوية.";
                break;
            default:
                throw new InvalidOperationException($"Unsupported UAE G1 Place Value lesson: {lesson}");
        }
        return Problem(family, prompt,
            "أراجع التمثيل العربي للعدد والقيمة المكانية أو ترتيب العد، وأتحقق من الإجابة.",
            lesson == 12 ? AssessmentItemType.ShortAnswer : AssessmentItemType.Numeric,
            ("n", n), ("aux", aux), ("mode", mode), ("result", result));
    }

    private static ExactProblem BuildUaeGrade1ThreeAddends(Random random)
    {
        const string family = "uae.g1.u04.l09.add_three_with_strategy";
        var mode = random.Next(0, 3);
        var a = mode == 0 ? random.Next(3, 10) : random.Next(1, 8);
        var b = mode == 0 ? 10 - a : mode == 1 ? random.Next(1, 6) : random.Next(1, 8);
        var c = mode == 1 ? b : mode == 0 ? random.Next(1, 9) : random.Next(1, 7);
        var pair = mode == 1 ? b + c : a + b;
        var total = a + b + c;
        var prompt = mode switch
        {
            0 => $"اجمع ثلاثة أعداد بتكوين عشرة أولًا: ({a} + {b}) + {c}. ما المجموع النهائي؟",
            1 => $"اجمع ثلاث مجموعات باستعمال المضاعف: {a} + ({b} + {c}). ما المجموع الكلي؟",
            _ => $"لدي ثلاث مجموعات من {a} و{b} و{c} قطع. اختر زوجًا يسهل جمعه، ثم أوجد العدد الكلي."
        };
        return Problem(family, prompt,
            "أحدد زوجًا مناسبًا، أحسبه، وأضيف المجموعة الثالثة مرة واحدة.",
            AssessmentItemType.Numeric,
            ("a", a), ("b", b), ("c", c), ("pair", pair),
            ("total", total), ("mode", mode));
    }

    private static ExactProblem BuildUaeGrade1CommutativeAdd(Random random)
    {
        const string family = "uae.g1.u04.l08.commutative_add";
        var a = random.Next(0, 11);
        var b = random.Next(0, 11);
        var mode = random.Next(0, 2);
        var scene = random.Next(0, 4);
        var total = a + b;
        var prompt = mode == 0
            ? $"عند تبديل ترتيب الحدين في الجمع يبقى المجموع ثابتًا. أكمل {a} + {b} = {b} + ؟. اكتب العدد الناقص."
            : $"مثّل مجموعتين من {a} و{b} قطع، ثم بدّل ترتيبهما: {b} + {a} = ؟. ما المجموع بعد التبديل؟";
        return Problem(family, prompt,
            "أغير ترتيب الحدين فقط؛ لا أغير العددين ولا أضيف عناصر إلى الكل.",
            AssessmentItemType.Numeric,
            ("a", a), ("b", b), ("total", total), ("mode", mode), ("scene", scene));
    }

    private static ExactProblem BuildUaeGrade1MakeTenAdd(Random random)
    {
        const string family = "uae.g1.u04.l07.make_ten_add";
        var a = random.Next(6, 10);
        var b = random.Next(11 - a, 10);
        var toTen = 10 - a;
        var remainder = b - toTen;
        var total = a + b;
        var scene = random.Next(0, 4);
        var objects = new[] { "كرات", "خرزات", "مكعبات", "ملصقات" };
        var frame = new string('●', a).PadRight(10, '□');
        return Problem(family,
            $"لدي {a} من {objects[scene]} في إطار عشرة: {frame}، وأضفت {b} أخرى. أنقل {toTen} لإكمال العشرة ويبقى {remainder}. ما المجموع الكلي؟ اكتب الناتج.",
            "أكمل عشرة من أجزاء المجموعة الثانية ثم أضف ما تبقى: عشرة زائد الباقي.",
            AssessmentItemType.Numeric,
            ("a", a), ("b", b), ("toTen", toTen), ("remainder", remainder),
            ("total", total), ("scene", scene));
    }

    private static ExactProblem BuildUaeGrade1ActOut(Random random)
    {
        const string family = "uae.g1.u04.l06.act_out_problem_solving";
        var a = random.Next(1, 8);
        var b = random.Next(1, 7);
        var mode = random.Next(0, 5);
        if (mode == 4)
        {
            a = random.Next(3, 10);
            b = a;
        }
        var c = mode == 1 ? random.Next(1, 5) : 0;
        var total = a + b + c;
        var prompt = mode switch
        {
            1 => $"مثّل القصة بمجموعات من القطع: في ثلاث سلال {a} و{b} و{c} أشياء. كم العدد الكلي؟ اكتب جملة الجمع.",
            4 => $"مثّل مجموعتين متساويتين: في كل مجموعة {a} مكعبات. اكتب {a} + {b} = ؟.",
            2 => $"مثّل القصة: لدي {total} سيارة، وفي مجموعة {a} سيارات؛ كم سيارة في المجموعة الأخرى؟ أرسم الكل وأحدد الجزء المعروف.",
            3 => $"مثّل بأصابعك مجموعتين: {a} أزهار ثم {b} أزهار أخرى؛ كم زهرة الآن؟",
            _ => $"مثّل بأدوات العد: لدي {a} طيور ثم وصلت {b} أخرى؛ اكتب العدد الكلي."
        };
        return Problem(family, prompt,
            "أفهم، أخطط، أمثل الأجزاء والكل، ثم أتحقق من صحة الجملة في القصة.",
            AssessmentItemType.Numeric,
            ("a", a), ("b", b), ("c", c), ("total", total), ("mode", mode));
    }

    private static ExactProblem BuildUaeGrade1NearDoubles(Random random)
    {
        const string family = "uae.g1.u04.l05.near_doubles_add";
        var baseValue = random.Next(2, 10);
        var delta = random.Next(0, 2) == 0 ? -1 : 1;
        var orientation = random.Next(0, 2);
        var scene = random.Next(0, 4);
        var left = orientation == 0 ? baseValue : baseValue + delta;
        var right = orientation == 0 ? baseValue + delta : baseValue;
        var action = delta == 1 ? "أضف واحدًا" : "أنقص واحدًا";
        return Problem(family,
            $"أكمل {left} + {right} = ؟ باستخدام المضاعف {baseValue} + {baseValue}. {action} من ناتج المضاعف لتجد الإجابة.",
            "أستعمل حقيقة المضاعفات ثم أصححها بإضافة أو طرح واحد.",
            AssessmentItemType.Numeric,
            ("base", baseValue), ("delta", delta), ("orientation", orientation), ("scene", scene));
    }

    private static ExactProblem BuildUaeGrade1Doubles(Random random)
    {
        const string family = "uae.g1.u04.l04.doubles_add";
        var equalAddend = random.Next(1, 11);
        var scene = random.Next(0, 4);
        var group = new string('●', equalAddend);
        return Problem(family,
            $"كوّن مجموعتين متساويتين: {group} | {group}. اكتب المضاعف وأكمل {equalAddend} + {equalAddend} = ؟.",
            "أجمع مجموعتين لهما العدد نفسه؛ الناتج يساوي ضعفي هذا العدد.",
            AssessmentItemType.Numeric,
            ("equalAddend", equalAddend), ("scene", scene));
    }

    private static ExactProblem BuildUaeGrade1NumberLineAddition(Random random)
    {
        const string family = "uae.g1.u04.l03.add_on_number_line";
        var start = random.Next(2, 12);
        var hops = random.Next(1, Math.Min(4, 12 - start) + 1);
        var scene = random.Next(0, 4);
        var numbers = string.Join("، ", Enumerable.Range(1, hops).Select(i => (start + i).ToString(CultureInfo.InvariantCulture)));
        return Problem(family,
            $"على خط الأعداد من ٠ إلى ١٢، ابدأ من {start} ثم تحرك يمينًا {hops} قفزات بالترتيب: {numbers}. أوجد {start} + {hops} = ؟.",
            "أتقدم خطوة واحدة إلى اليمين في كل قفزة حتى أصل إلى المجموع.",
            AssessmentItemType.Numeric,
            ("start", start), ("hops", hops), ("scene", scene));
    }

    private static ExactProblem BuildUaeGrade1CountOnFils(Random random)
    {
        const string family = "uae.g1.u04.l02.count_on_fils";
        var start = random.Next(4, 11);
        var added = random.Next(1, 4);
        var scene = random.Next(0, 4);
        var hops = string.Join("، ", Enumerable.Range(1, added).Select(i => (start + i).ToString(CultureInfo.InvariantCulture)));
        return Problem(family,
            $"لدي {start} عملات من فئة فلس واحد، ثم أضيف {added} عملات أخرى من الفئة نفسها. أبدأ من {start} وأعد {hops}. كم فلسًا أصبح لدي؟",
            "كل قطعة قيمتها فلس واحد؛ أعد من العدد الأول خطوة عن كل قطعة جديدة.",
            AssessmentItemType.Numeric,
            ("start", start), ("added", added), ("scene", scene));
    }

    private static ExactProblem BuildUaeGrade1CountOn(Random random)
    {
        const string family = "uae.g1.u04.l01.count_on_1_2_3";
        var start = random.Next(4, 18);
        var step = random.Next(1, Math.Min(3, 20 - start) + 1);
        var orientation = random.Next(0, 2);
        var left = orientation == 0 ? start : step;
        var right = orientation == 0 ? step : start;
        var jumps = string.Join("، ", Enumerable.Range(1, step).Select(i => (start + i).ToString(CultureInfo.InvariantCulture)));
        return Problem(family,
            $"أكمل الجمع {left} + {right} = ؟. ابدأ العد من {start} ثم تقدم {step} خطوات؛ أعداد القفزات: {jumps}. ما العدد الأخير؟",
            "أبدأ من العدد الأكبر ثم أزيد واحدًا لكل قفزة، ولا أعد رقم البداية قفزة.",
            AssessmentItemType.Numeric,
            ("start", start), ("step", step), ("orientation", orientation));
    }

    private static ExactProblem BuildUaeGrade1TrueFalseSubtraction(Random random)
    {
        const string family = "uae.g1.u03.l14.true_false_subtraction";
        var whole = random.Next(1, 11);
        var removed = random.Next(0, whole + 1);
        var correct = random.Next(0, 2) == 0;
        var difference = whole - removed;
        var shown = correct ? difference : (difference + 1) % 11;
        var scene = random.Next(0, 4);
        var statement = scene switch
        {
            0 => $"{whole} − {removed} = {shown}",
            1 => $"{shown} = {whole} − {removed}",
            2 => $"{whole} − {removed} = {shown}",
            _ => $"{shown} = {whole} − {removed}"
        };
        return Problem(family,
            $"هل العبارة {statement} صحيحة أم خطأ؟ ابدأ بـ {whole} نقطة واشطب {removed} ثم قارن الباقي.",
            "أتأكد بعد شطب المطروح أن الباقي يساوي العدد المكتوب في الطرف الآخر.",
            AssessmentItemType.ShortAnswer,
            ("whole", whole), ("removed", removed), ("shown", shown), ("scene", scene));
    }

    private static ExactProblem BuildUaeGrade1TrueFalseAddition(Random random)
    {
        const string family = "uae.g1.u02.l13.true_false_addition";
        var left = random.Next(0, 8);
        var right = random.Next(0, Math.Min(4, 10 - left) + 1);
        var sum = left + right;
        var correct = random.Next(0, 2) == 0;
        var shown = correct ? sum : (sum + 1) % 11;
        var scene = random.Next(0, 4);
        var equation = scene switch
        {
            0 => $"{left} + {right} = {shown}",
            1 => $"{shown} = {left} + {right}",
            2 => $"{left} + {right} = {shown}",
            _ => $"{shown} = {right} + {left}"
        };
        return Problem(family,
            $"هل العبارة {equation} صحيحة أم خطأ؟ احسب وتحقق بالنقاط قبل اختيار الإجابة.",
            "أحسب المجموع الحقيقي ثم أقارنه بالطرف الآخر من علامة المساواة.",
            AssessmentItemType.ShortAnswer,
            ("left", left), ("right", right), ("shown", shown), ("scene", scene));
    }

    private static ExactProblem BuildUaeGrade1ComposeNumber(Random random, string family)
    {
        var total = family == "uae.g1.u02.l11.make_ten_frame" ? 10 :
            family == "uae.g1.u02.l10.make_nine" ? 9 :
            family == "uae.g1.u02.l09.make_eight" ? 8 :
            family == "uae.g1.u02.l08.make_six_seven" ? random.Next(6, 8) : random.Next(4, 6);
        var left = random.Next(0, total + 1);
        var right = total - left;
        var scene = random.Next(0, 4);
        var first = new string('●', left);
        var second = new string('○', right);
        var frame = family == "uae.g1.u02.l11.make_ten_frame" ?
            string.Concat(Enumerable.Range(0, 5).Select(i => i < left ? "●" : "○")) + "\n" +
            string.Concat(Enumerable.Range(5, 5).Select(i => i < left ? "●" : "○")) :
            $"{first} | {second}";
        return Problem(family,
            $"كوّن العدد {total}: الجزء الأول {left}، والجزء المفقود ؟. أكمل {left} + ؟ = {total}. النموذج: {frame}",
            "أمثل العدد الكلي بدوائر وأعد الجزء المتبقي بعد تمييز الجزء الأول.",
            AssessmentItemType.Numeric,
            ("left", left), ("right", right), ("total", total), ("scene", scene));
    }

    private static ExactProblem BuildUaeGrade1AdditionZero(Random random)
    {
        const string family = "uae.g1.u02.l04.add_zero";
        var count = random.Next(1, 10);
        var zeroFirst = random.Next(0, 2) == 0;
        var scene = random.Next(0, 4);
        var left = zeroFirst ? 0 : count;
        var right = zeroFirst ? count : 0;
        var dots = new string('●', count);
        return Problem(family,
            $"أكمل جملة الجمع: {left} + {right} = ؟. مثل العدد بالنقاط {dots} واترك مجموعة الصفر فارغة.",
            "إضافة الصفر لا تغير العدد؛ أعد عناصر المجموعة غير الفارغة.",
            AssessmentItemType.Numeric,
            ("left", left), ("right", right), ("scene", scene));
    }

    private static ExactProblem BuildG1L01Counting(Random random, int scale)
    {
        var count = random.Next(scale == 1 ? 1 : scale == 2 ? 7 : 13, 21);
        var dots = string.Join(" ", Enumerable.Repeat("●", count));
        return Problem(
            "usccss.g1.u01.l01.count_collection_to20",
            $"Count each counter once: {dots}  How many counters are there?",
            "Touch or mark each counter exactly once, then write the total.",
            AssessmentItemType.Numeric,
            ("count", count));
    }

    private static ExactProblem BuildG1L01CubeAddition(Random random, int scale)
    {
        // Real 1–6 face values; re-roll pairs whose sum is above 10.
        var left = random.Next(1, 7);
        var right = random.Next(1, Math.Min(6, 10 - left) + 1);
        return Problem(
            "usccss.g1.u01.l01.add_dot_cubes_to10",
            $"One cube shows {string.Join(" ", Enumerable.Repeat("●", left))} " +
            $"and the other shows {string.Join(" ", Enumerable.Repeat("●", right))}. " +
            "How many dots altogether?",
            "Count all the dots or start with the first cube and count on.",
            AssessmentItemType.Numeric,
            ("left", left),
            ("right", right));
    }

    private static ExactProblem BuildWholeAddSubtract(
        string family,
        Random random,
        int scale)
    {
        var isApply = family.EndsWith(".apply", StringComparison.Ordinal);
        var operation = random.Next(0, 2);
        int left;
        int right;

        if (family.Contains(".within_10.", StringComparison.Ordinal))
        {
            if (operation == 0)
            {
                left = random.Next(0, 10);
                right = random.Next(0, 11 - left);
            }
            else
            {
                left = random.Next(1, 11);
                right = random.Next(0, left + 1);
            }
        }
        else if (family.Contains(".across_ten.", StringComparison.Ordinal))
        {
            if (operation == 0)
            {
                left = random.Next(6, 10);
                right = random.Next(10 - left, 10);
            }
            else
            {
                left = random.Next(11, 19);
                var minimum = Math.Max(2, left - 9);
                var maximum = Math.Min(9, left - 1);
                right = random.Next(minimum, maximum + 1);
            }
        }
        else if (family.Contains(".within_100.", StringComparison.Ordinal))
        {
            if (operation == 0)
            {
                left = random.Next(10, 90);
                right = random.Next(1, 101 - left);
            }
            else
            {
                left = random.Next(10, 101);
                right = random.Next(1, left + 1);
            }
        }
        else if (family.Contains(".columnar.", StringComparison.Ordinal))
        {
            if (operation == 0)
            {
                do
                {
                    left = random.Next(11, 90);
                    right = random.Next(10, 100 - left);
                }
                while ((left % 10) + (right % 10) < 10);
            }
            else
            {
                do
                {
                    left = random.Next(21, 100);
                    right = random.Next(10, left);
                }
                while (left % 10 >= right % 10);
            }
        }
        else
        {
            throw new InvalidOperationException($"Unknown whole-number family: {family}");
        }

        var symbol = operation == 0 ? "+" : "−";
        var answer = operation == 0 ? left + right : left - right;
        var prompt = isApply
            ? operation == 0
                ? $"A class has {left} counters and receives {right} more. How many counters are there altogether?"
                : $"A class has {left} counters and uses {right}. How many counters remain?"
            : family.Contains(".columnar.", StringComparison.Ordinal)
                ? $"Use a columnar method to calculate {left} {symbol} {right}."
                : $"Calculate {left} {symbol} {right}.";

        var solution = family.Contains(".columnar.", StringComparison.Ordinal)
            ? "Align place values, regroup when required, perform the operation, then check with the inverse operation."
            : operation == 0
                ? "Add the two quantities and check the result by subtraction."
                : "Subtract the second quantity from the first and check the result by addition.";

        return Problem(
            family,
            prompt,
            solution,
            AssessmentItemType.Numeric,
            ("left", left),
            ("right", right),
            ("operation", operation),
            ("expected", answer),
            ("scale", scale));
    }

    private static bool CrossesTen(int left, int right, int operation, int expected)
    {
        if (operation == 0)
            return left < 10 && right < 10 && expected >= 10 && expected <= 20;

        return left > 10 && right > 0 && expected < 10 && expected >= 0;
    }

    private static bool RequiresRegrouping(int left, int right, int operation) =>
        operation == 0
            ? (left % 10) + (right % 10) >= 10
            : left % 10 < right % 10;

    private static ExactProblem BuildComparativeDifference(
        string family,
        Random random,
        int scale)
    {
        var isApply = family.EndsWith(".apply", StringComparison.Ordinal);
        var maximum = 30 + scale * 20;
        var baseValue = random.Next(5, maximum);
        var difference = random.Next(2, 10 + scale * 5);
        var expected = baseValue + difference;

        var prompt = isApply
            ? $"Aisha has {baseValue} stickers. Omar has {difference} more stickers than Aisha. How many stickers does Omar have?"
            : $"A number is {difference} greater than {baseValue}. What is the number?";

        return Problem(
            family,
            prompt,
            "Model the comparison as larger = smaller + difference, calculate the unknown quantity, then check the difference.",
            AssessmentItemType.Numeric,
            ("base", baseValue),
            ("difference", difference));
    }

    private static ExactProblem BuildComplementTo100(
        string family,
        Random random)
    {
        var isApply = family.EndsWith(".apply", StringComparison.Ordinal);
        var known = random.Next(1, 100);
        var prompt = isApply
            ? $"A target is 100 points. You already have {known} points. How many more points are needed to reach 100?"
            : $"Complete the calculation: {known} + ? = 100.";

        return Problem(
            family,
            prompt,
            "Find the complement by subtracting the known amount from 100, then verify the two parts total 100.",
            AssessmentItemType.Numeric,
            ("known", known));
    }

    private static ExactProblem BuildMultiplicationFact(
        string family,
        Random random)
    {
        var isApply = family.EndsWith(".apply", StringComparison.Ordinal);
        var left = random.Next(2, 13);
        var right = random.Next(2, 13);
        var prompt = isApply
            ? $"There are {left} equal groups with {right} objects in each group. How many objects are there altogether?"
            : $"Calculate {left} × {right}.";

        return Problem(
            family,
            prompt,
            "Use the multiplication fact and verify by repeated groups or the inverse division fact.",
            AssessmentItemType.Numeric,
            ("left", left),
            ("right", right));
    }

    private static ExactProblem BuildDivisionWithRemainder(
        string family,
        Random random,
        int scale)
    {
        var isApply = family.EndsWith(".apply", StringComparison.Ordinal);
        var divisor = random.Next(2, 7 + scale);
        var quotient = random.Next(2, 8 + scale * 3);
        var remainder = random.Next(1, divisor);
        var dividend = divisor * quotient + remainder;
        var prompt = isApply
            ? $"{dividend} counters are shared equally among {divisor} groups. How many counters are in each full group and how many are left over? Answer as 'q r r'."
            : $"Calculate {dividend} ÷ {divisor}. Give the quotient and remainder as 'q r r'.";

        return Problem(
            family,
            prompt,
            "Divide to find the whole-number quotient and remainder. Verify dividend = divisor × quotient + remainder and remainder < divisor.",
            AssessmentItemType.ShortAnswer,
            ("dividend", dividend),
            ("divisor", divisor));
    }

    private static ExactProblem BuildFractionOfQuantity(
        string family,
        Random random,
        int scale)
    {
        var isApply = family.EndsWith(".apply", StringComparison.Ordinal);
        var denominator = random.Next(2, 7 + scale);
        var numerator = random.Next(1, denominator + 1);
        var unit = random.Next(2, 8 + scale * 2);
        var quantity = denominator * unit;
        var prompt = isApply
            ? $"A collection has {quantity} items. What is {numerator}/{denominator} of the collection?"
            : $"Find {numerator}/{denominator} of {quantity}.";

        return Problem(
            family,
            prompt,
            "Divide the whole quantity by the denominator to find one equal part, multiply by the numerator, then verify the fraction relationship.",
            AssessmentItemType.Numeric,
            ("numerator", numerator),
            ("denominator", denominator),
            ("quantity", quantity));
    }

    private static ExactProblem BuildFractionAddSubtract(
        string family,
        Random random,
        int scale)
    {
        var isApply = family.EndsWith(".apply", StringComparison.Ordinal);
        var operation = random.Next(0, 2);
        int n1;
        int d1;
        int n2;
        int d2;

        if (family.Contains(".within_one.", StringComparison.Ordinal))
        {
            d1 = d2 = random.Next(4, 9 + scale);
            if (operation == 0)
            {
                n1 = random.Next(1, d1 - 1);
                n2 = random.Next(1, d1 - n1);
            }
            else
            {
                n1 = random.Next(2, d1);
                n2 = random.Next(1, n1);
            }
        }
        else if (family.Contains(".same_denominator.mixed.", StringComparison.Ordinal))
        {
            d1 = d2 = random.Next(3, 8 + scale);
            n1 = random.Next(d1 + 1, 3 * d1);
            if (operation == 0)
            {
                n2 = random.Next(1, 2 * d1);
            }
            else
            {
                n2 = random.Next(1, n1);
            }
        }
        else if (family.Contains(".related.", StringComparison.Ordinal))
        {
            d1 = random.Next(2, 6 + scale);
            var factor = random.Next(2, 4 + scale);
            d2 = d1 * factor;
            n1 = random.Next(1, d1);
            n2 = random.Next(1, d2);
            if (operation == 1 && n1 * d2 < n2 * d1)
            {
                (n1, n2) = (n2, n1);
                (d1, d2) = (d2, d1);
            }
        }
        else
        {
            d1 = random.Next(3, 8 + scale);
            do d2 = random.Next(3, 9 + scale); while (d2 == d1);
            n1 = random.Next(1, d1);
            n2 = random.Next(1, d2);
            if (operation == 1 && n1 * d2 < n2 * d1)
            {
                (n1, n2) = (n2, n1);
                (d1, d2) = (d2, d1);
            }
        }

        var symbol = operation == 0 ? "+" : "−";
        var leftText = family.Contains(".same_denominator.mixed.", StringComparison.Ordinal)
            ? FormatMixedFraction(n1, d1)
            : $"{n1}/{d1}";
        var rightText = family.Contains(".same_denominator.mixed.", StringComparison.Ordinal)
            ? FormatMixedFraction(n2, d2)
            : $"{n2}/{d2}";

        var prompt = isApply
            ? $"A quantity is {leftText} units and changes by {rightText} units using {symbol}. Find the exact result as a simplified fraction."
            : $"Calculate {leftText} {symbol} {rightText}. Give the exact answer as a simplified fraction.";

        return Problem(
            family,
            prompt,
            "Use an equivalent common denominator when necessary, combine only like fractional parts, simplify the exact result, and verify by reversing the operation.",
            AssessmentItemType.ShortAnswer,
            ("n1", n1),
            ("d1", d1),
            ("n2", n2),
            ("d2", d2),
            ("operation", operation));
    }

    private static ExactProblem BuildFractionBenchmark(Random random, int scale)
    {
        var benchmarkHalf = random.Next(0, 2) == 0;
        var benchmarkN = benchmarkHalf ? 1 : 1;
        var benchmarkD = benchmarkHalf ? 2 : 1;
        var d = random.Next(3, 9 + scale);
        var n = random.Next(1, d * (benchmarkHalf ? 1 : 2));

        return Problem(
            "fractions.compare.benchmark",
            $"Compare {n}/{d} with {(benchmarkHalf ? "1/2" : "1")}. Enter <, >, or =.",
            "Compare the fraction to the benchmark using an equivalent denominator or exact cross-products.",
            AssessmentItemType.ShortAnswer,
            ("n", n),
            ("d", d),
            ("benchmarkN", benchmarkN),
            ("benchmarkD", benchmarkD));
    }

    private static ExactProblem BuildCoordinateGradient(Random random, int scale)
    {
        var x1 = random.Next(-5 * scale, 5 * scale + 1);
        var run = random.Next(1, 4 + scale);
        var gradient = random.Next(-3 - scale, 4 + scale);
        if (gradient == 0) gradient = 1;
        var y1 = random.Next(-6 * scale, 6 * scale + 1);
        var x2 = x1 + run;
        var y2 = y1 + gradient * run;

        return Problem(
            "geometry.coordinate.gradient_between_points",
            $"Find the gradient of the line through ({x1}, {y1}) and ({x2}, {y2}).",
            "Use gradient = change in y ÷ change in x, then verify with both coordinates.",
            AssessmentItemType.Numeric,
            ("x1", x1),
            ("y1", y1),
            ("x2", x2),
            ("y2", y2));
    }

    private static ExactProblem BuildCoordinateLinearRule(Random random, int scale)
    {
        var gradient = NonZero(random, -4 - scale, 5 + scale);
        var intercept = random.Next(-8 - scale * 2, 9 + scale * 2);
        var x = random.Next(-5 - scale, 6 + scale);
        var y = gradient * x + intercept;

        return Problem(
            "geometry.coordinate.evaluate_linear_rule",
            $"For the straight line y = {FormatLinearRule(gradient, intercept)}, find y when x = {x}.",
            "Substitute the given x-value into y = mx + c. Verify the coordinate satisfies the original line equation.",
            AssessmentItemType.Numeric,
            ("gradient", gradient),
            ("intercept", intercept),
            ("x", x),
            ("y", y));
    }

    private static ExactProblem BuildCoordinateIntercept(Random random, int scale)
    {
        var gradient = NonZero(random, -4 - scale, 5 + scale);
        var intercept = random.Next(-10 - scale * 2, 11 + scale * 2);

        return Problem(
            "geometry.coordinate.y_intercept_from_rule",
            $"For the straight line y = {FormatLinearRule(gradient, intercept)}, find the y-intercept.",
            "In y = mx + c, the y-intercept is c because x = 0 on the y-axis. Substitute x = 0 to verify.",
            AssessmentItemType.Numeric,
            ("gradient", gradient),
            ("intercept", intercept));
    }

    private static ExactProblem BuildParallelLineAngle(Random random, int scale)
    {
        var known = random.Next(25, 155);
        var supplementary = random.Next(0, 2) == 1;
        var expected = supplementary ? 180 - known : known;
        var relationship = supplementary ? 1 : 0;
        var prompt = supplementary
            ? $"Two parallel lines are cut by a transversal. One interior angle is {known}°. Find the adjacent interior angle on the straight line."
            : $"Two parallel lines are cut by a transversal. One corresponding angle is {known}°. Find the corresponding angle.";

        return Problem(
            "geometry.angles.parallel_lines",
            prompt,
            supplementary
                ? "Adjacent angles on a straight line sum to 180°. Use the parallel-line relationship to confirm the position."
                : "Corresponding angles formed by a transversal across parallel lines are equal.",
            AssessmentItemType.Numeric,
            ("knownAngle", known),
            ("relationship", relationship),
            ("expectedAngle", expected),
            ("scale", scale));
    }

    private static ExactProblem BuildSupplementaryAngle(Random random)
    {
        var known = random.Next(20, 160);
        return Problem(
            "geometry.angles.supplementary",
            $"Two angles form a straight line. One angle is {known}°. Find the other angle.",
            "Angles on a straight line total 180°, so subtract the known angle from 180°.",
            AssessmentItemType.Numeric,
            ("knownAngle", known));
    }

    private static ExactProblem BuildAlgebraicSupplementaryAngle(Random random, int scale)
    {
        var coefficientA = random.Next(1, 2 + scale);
        var coefficientB = random.Next(1, 2 + scale);
        var maxX = Math.Max(3, (150 / (coefficientA + coefficientB)));
        var expectedX = random.Next(2, Math.Min(maxX, 8 + scale * 4));
        var remaining = 180 - (coefficientA + coefficientB) * expectedX;
        var offsetA = random.Next(5, Math.Max(6, remaining - 4));
        var offsetB = remaining - offsetA;
        if (offsetB <= 0)
        {
            offsetA = Math.Max(1, remaining / 2);
            offsetB = remaining - offsetA;
        }

        return Problem(
            "geometry.angles.algebraic_supplementary",
            $"Two adjacent angles on a straight line are ({coefficientA}x + {offsetA})° and ({coefficientB}x + {offsetB})°. Find x.",
            "Angles on a straight line sum to 180°. Form one linear equation, solve for x, then substitute back to verify the two angle measures total 180°.",
            AssessmentItemType.Numeric,
            ("coefficientA", coefficientA),
            ("offsetA", offsetA),
            ("coefficientB", coefficientB),
            ("offsetB", offsetB),
            ("expectedX", expectedX));
    }

    private static ExactProblem BuildSimilarityMissingLength(Random random, int scale)
    {
        var source = random.Next(2, 8 + scale);
        var factor = random.Next(2, 5 + scale);
        var target = source * factor;

        return Problem(
            "geometry.similarity.find_missing_length",
            $"Two similar shapes have scale factor {factor} from the smaller to the larger. A corresponding side on the smaller shape is {source}. Find the matching side on the larger shape.",
            "Corresponding lengths in similar shapes are multiplied by the same scale factor.",
            AssessmentItemType.Numeric,
            ("sourceLength", source),
            ("scaleFactor", factor),
            ("targetLength", target));
    }

    private static ExactProblem BuildSimilarityScaleFactor(Random random, int scale)
    {
        var source = random.Next(2, 8 + scale);
        var factor = random.Next(2, 5 + scale);
        var target = source * factor;

        return Problem(
            "geometry.similarity.scale_factor",
            $"A side of length {source} corresponds to a side of length {target} in a similar shape. Find the scale factor from the first shape to the second.",
            "Divide a corresponding target length by the source length.",
            AssessmentItemType.Numeric,
            ("sourceLength", source),
            ("targetLength", target));
    }

    private static ExactProblem BuildCongruenceCriterion(Random random)
    {
        var criterion = random.Next(0, 4);
        var variant = random.Next(1, 8);
        var sideA = 3 + variant;
        var sideB = 5 + variant;
        var sideC = 6 + variant;
        var angleA = 25 + 5 * variant;
        var angleB = 35 + 3 * variant;

        var (prompt, solution) = criterion switch
        {
            0 => (
                $"Two triangles each have corresponding side lengths {sideA}, {sideB}, and {sideC}. Which congruence criterion proves they are congruent? Answer SSS, SAS, ASA, or RHS.",
                "Three equal corresponding sides establish SSS congruence."),
            1 => (
                $"Two triangles each have corresponding sides {sideA} and {sideB} with the included angle {angleA}°. Which congruence criterion applies? Answer SSS, SAS, ASA, or RHS.",
                "Two sides and the included angle establish SAS congruence."),
            2 => (
                $"Two triangles each have corresponding angles {angleA}° and {angleB}° with the included side {sideA}. Which congruence criterion applies? Answer SSS, SAS, ASA, or RHS.",
                "Two angles and the included side establish ASA congruence."),
            _ => (
                $"Two right triangles each have hypotenuse {sideC} and one corresponding leg {sideA}. Which congruence criterion applies? Answer SSS, SAS, ASA, or RHS.",
                "Right angle, hypotenuse and one corresponding side establish RHS congruence.")
        };

        return Problem(
            "geometry.congruence.identify_criterion",
            prompt,
            solution,
            AssessmentItemType.ShortAnswer,
            ("criterion", criterion),
            ("variant", variant),
            ("sideA", sideA),
            ("sideB", sideB),
            ("sideC", sideC),
            ("angleA", angleA),
            ("angleB", angleB));
    }

    private static ExactProblem BuildRectangularPrismSurfaceArea(Random random, int scale, string family)
    {
        var length = random.Next(2, 7 + scale * 2);
        var width = random.Next(2, 6 + scale);
        var height = random.Next(2, 5 + scale);

        return Problem(
            family,
            $"A rectangular prism has length {length}, width {width}, and height {height}. Find its total surface area.",
            "Use 2(lw + lh + wh), then verify all six faces are counted.",
            AssessmentItemType.Numeric,
            ("length", length),
            ("width", width),
            ("height", height));
    }

    private static ExactProblem BuildRectangularPrismVolume(Random random, int scale, string family)
    {
        var length = random.Next(2, 7 + scale * 2);
        var width = random.Next(2, 6 + scale);
        var height = random.Next(2, 5 + scale);

        return Problem(
            family,
            $"A rectangular prism has length {length}, width {width}, and height {height}. Find its volume.",
            "Volume of a rectangular prism is length × width × height.",
            AssessmentItemType.Numeric,
            ("length", length),
            ("width", width),
            ("height", height));
    }

    private static ExactProblem BuildRectangleArea(Random random, int scale, string family)
    {
        var length = random.Next(3, 10 + scale * 3);
        var width = random.Next(2, 8 + scale * 2);
        return Problem(
            family,
            $"A rectangle has length {length} and width {width}. Find its area.",
            "Area = length × width.",
            AssessmentItemType.Numeric,
            ("length", length),
            ("width", width));
    }

    private static ExactProblem BuildRectanglePerimeter(Random random, int scale, string family)
    {
        var length = random.Next(3, 10 + scale * 3);
        var width = random.Next(2, 8 + scale * 2);
        return Problem(
            family,
            $"A rectangle has length {length} and width {width}. Find its perimeter.",
            "Perimeter = 2(length + width).",
            AssessmentItemType.Numeric,
            ("length", length),
            ("width", width));
    }

    private static ExactProblem BuildTriangleArea(Random random, int scale)
    {
        var @base = 2 * random.Next(2, 7 + scale * 2);
        var height = random.Next(2, 8 + scale * 2);
        return Problem(
            "geometry.perimeter_area.triangle_area",
            $"A triangle has base {@base} and perpendicular height {height}. Find its area.",
            "Use area = 1/2 × base × perpendicular height. Verify by doubling the area and comparing with base × height.",
            AssessmentItemType.Numeric,
            ("base", @base),
            ("height", height));
    }

    private static ExactProblem BuildG4PartialQuotientsRemainder(Random random)
    {
        var divisor = random.Next(2, 10);
        var quotient = random.Next(50, Math.Min(900, (9999 - divisor + 1) / divisor) + 1);
        var remainder = random.Next(0, divisor);
        var dividend = divisor * quotient + remainder;
        var tens = quotient / 10 * 10;
        var ones = quotient % 10;
        return Problem("usccss.g4.u06.l19.partial_quotients_remainders",
            "Use partial quotients to divide " + dividend + " by " + divisor +
            ". Show the subtractions. Give quotient and remainder as 'q r r'.",
            "Subtract " + divisor*tens + " (= " + divisor + " × " + tens +
            "), then " + divisor*ones + " (= " + divisor + " × " + ones +
            "). " + remainder + " remains. Check " + dividend + " = " +
            divisor + " × " + quotient + " + " + remainder + ".",
            AssessmentItemType.ShortAnswer,
            ("dividend",dividend),("divisor",divisor),
            ("partialTens",tens),("partialOnes",ones),("remainder",remainder));
    }

    private static ExactProblem BuildG4ContextualRemainder(Random random)
    {
        // 2021 G4 U06:L20: keep rounding, short rows and missing savings distinct.
        var caseType = random.Next(0, 5);
        var divisor = random.Next(2, 10);
        var quotient = random.Next(Math.Max(50, (100 + divisor - 1) / divisor), 650);
        var remainder = caseType == 4 ? 0 : random.Next(1, divisor);
        var dividend = divisor * quotient + remainder;
        var shortfall = caseType == 3 ? remainder : 0;
        var prompt = caseType switch
        {
            0 => $"All {dividend} muffins must go in boxes that hold {divisor} muffins each. How many boxes are needed?",
            1 => $"An auditorium has {dividend} seats arranged in full rows of {divisor}, plus one short row. How many full rows are there?",
            2 => $"An auditorium has {dividend} seats arranged in full rows of {divisor}, plus one short row. How many seats are in that short row?",
            3 => $"A school needs ${dividend} for a garden. It saves the same number of dollars each month for {divisor} months and is still short by ${shortfall}. How much did it save per month?",
            _ => $"A student invents a story for {dividend} ÷ {divisor} using equal groups. How many are in each group?"
        };
        var solution = caseType switch
        {
            0 => $"{dividend} = {divisor} × {quotient} + {remainder}; all muffins must be boxed, so use {quotient + 1} boxes.",
            1 => $"{dividend} = {divisor} × {quotient} + {remainder}; there are {quotient} full rows and one short row.",
            2 => $"{dividend} = {divisor} × {quotient} + {remainder}; the short row has {remainder} seats.",
            3 => $"Saved total is {dividend} − {shortfall} = {dividend - shortfall}; divide by {divisor} months to get {quotient} dollars.",
            _ => $"{dividend} = {divisor} × {quotient}; each of the {divisor} equal groups has {quotient}."
        };
        return Problem("usccss.g4.u06.l20.contextual_remainder_decisions", prompt, solution, AssessmentItemType.Numeric,
            ("caseType", caseType), ("dividend", dividend), ("divisor", divisor), ("shortfall", shortfall));
    }

    private static ExactProblem BuildG4MeasurementConversions(Random random)
    {
        // IM 2021 G4 U06:L22: reuse exact rectangle math and whole-unit conversions.
        var mode = random.Next(0, 5);
        var sheetW = random.Next(36, 50);
        var sheetH = random.Next(24, 35);
        const int bannerH = 8;
        var bannerF = random.Next(6, 9);
        var leftW = random.Next(8, 14);
        var rightWYd = random.Next(2, 5);
        var lowerHYd = random.Next(3, 6);
        var riseF = random.Next(2, 7);
        var bottomF = leftW + 3 * rightWYd;
        var leftH = 3 * lowerHYd + riseF;
        var perimeterF = 2 * (bottomF + leftH);
        var supply = 12 * perimeterF + random.Next(60, 501);
        var prompt = mode switch
        {
            0 => "A poster is " + sheetW + " inches by " + sheetH + " inches. A banner requires " + bannerH + " inches by " + bannerF + " feet. How many square inches of poster area are spare?",
            1 => "A six-sided L-shaped classroom has bottom " + (12 * bottomF) + " inches, right rise " + lowerHYd + " yards, inset " + rightWYd + " yards, inner rise " + riseF + " feet, top " + leftW + " feet and left side " + (12 * leftH) + " inches. How many feet is the entire outside perimeter?",
            2 => "A six-sided L-shaped classroom has bottom " + (12 * bottomF) + " inches, right rise " + lowerHYd + " yards, inset " + rightWYd + " yards, inner rise " + riseF + " feet, top " + leftW + " feet and left side " + (12 * leftH) + " inches. How many INCHES of baseboard does the perimeter require?",
            3 => "An L-shaped room comprises a " + bottomF + "-foot by " + (3 * lowerHYd) + "-foot base and an upper " + leftW + "-foot by " + riseF + "-foot rectangle. How many SQUARE FEET of carpet are required?",
            _ => "A classroom's six-sided L-shaped floor has bottom " + bottomF + " feet, right rise " + (3 * lowerHYd) + " feet, inset " + (3 * rightWYd) + " feet, inside rise " + riseF + " feet, top " + leftW + " feet and left side " + leftH + " feet. The school received " + supply + " inches of baseboard. How many INCHES are spare?"
        };
        var solution = mode switch
        {
            0 => "Available square inches minus banner area after converting feet to inches.",
            1 => "Convert all six sides to feet and sum outside perimeter.",
            2 => "Find the six-side perimeter in feet, then multiply by 12 inches per foot.",
            3 => "Add the non-overlapping rectangles' areas in square feet.",
            _ => "Convert the six-side perimeter to inches and subtract from supply."
        };
        return Problem("usccss.g4.u06.l22.convert_l_shaped_area_perimeter",
            prompt, solution, AssessmentItemType.Numeric,
            ("caseType", mode), ("sheetWidthInches", sheetW),
            ("sheetHeightInches", sheetH), ("bannerHeightInches", bannerH),
            ("bannerLengthFeet", bannerF), ("leftWidthFeet", leftW),
            ("rightWidthYards", rightWYd), ("lowerHeightYards", lowerHYd),
            ("upperRiseFeet", riseF), ("suppliedInches", supply));
    }






    private static ExactProblem BuildG4TwoDigitPartialProducts(Random random)
    {
        // 2021 G4 U09:L05 multiplication via 2-digit partial products.
        // Source visual method diagrams still teacher reviewed.
        var mode=random.Next(0,8);
        var variant=random.Next(0,3);
        var (a,b)=mode switch
        {
            0 => (34,21), 1 => (23,14), 2 => (46,32),
            3 => (72,15), 4 => (58,24), 5 => (16,43),
            6 => (29,31), _ => (87,12)
        };
        var tens=(b/10)*10;
        var ones=b%10;
        var first=a*tens;
        var second=a*ones;
        var answer=first+second;
        var cue=variant switch
        {
            0 => "Use tens and ones in a partial-products model.",
            1 => "Both partial products add to the final product.",
            _ => "Check the standard algorithm against distributive multiplication."
        };
        return Problem("usccss.g4.u09.l05.two_digit_partial_products",
            $"FIRST-PARTY multiplication: Find {a} times {b}. {cue}",
            $"Split {b} into {tens}+{ones}; {a}×{tens}={first}, {a}×{ones}={second}, total={answer}.",
            AssessmentItemType.Numeric,
            ("caseType",mode),("variant",variant),
            ("multiplicand",a),("multiplier",b),("product",answer));
    }

    private static ExactProblem BuildG4PolygonPerimeterSum(Random random)
    {
        // 2021 Grade 4 U08:L07: perimeter is sum of boundary lengths,
        // not area; original diagrams and fractional unit task teacher-led.
        var mode=random.Next(0,8);
        var variant=random.Next(0,3);
        var (a,b,c,d)=mode switch
        {
            0 => (18,26,13,37), 1 => (7,7,7,0),
            2 => (4,9,4,9), 3 => (3,5,8,12),
            4 => (15,15,15,15), 5 => (12,17,19,0),
            6 => (8,6,8,6), _ => (25,10,15,20)
        };
        var total=a+b+c+d;
        var sides=d==0?$"{a}, {b}, {c}":$"{a}, {b}, {c}, {d}";
        var cue=variant switch
        {
            0 => "Add the OUTER edge lengths only.",
            1 => "Perimeter is linear distance around the boundary, not area.",
            _ => "Use the same linear unit for every side."
        };
        return Problem("usccss.g4.u08.l07.polygon_perimeter_sum",
            $"An INDEPENDENT polygon has consecutive side lengths {sides} units. What is its perimeter in those units? {cue}",
            $"Add all boundary sides: {a}+{b}+{c}"+(d==0?"":$"+{d}")+$"={total} units.",
            AssessmentItemType.Numeric,
            ("caseType",mode),("variant",variant),("sideA",a),("sideB",b),
            ("sideC",c),("sideD",d),("perimeter",total));
    }

    private static ExactProblem BuildG4SymmetryAxisCounts(Random random)
    {
        // 2021 G4 Unit 8 lines of symmetry. Described first-party
        // canonical shapes only, NOT the original source card pictures.
        var mode = random.Next(0, 8);
        var variant = random.Next(0, 3);
        string[] shapeNames = [
            "a square", "a rectangle that is NOT a square",
            "an equilateral triangle", "an isosceles triangle that is NOT equilateral",
            "a scalene triangle", "a rhombus that is NOT a square",
            "a kite that is NOT a rhombus", "a slanted parallelogram that is NOT a rectangle"
        ];
        int[] axes = [4, 2, 3, 1, 0, 2, 1, 0];
        var name = shapeNames[mode];
        var count = axes[mode];
        var cue = variant switch
        {
            0 => "A line of symmetry folds the figure into matching halves.",
            1 => "Reflect one half to see whether the other half matches exactly.",
            _ => "Only count DISTINCT reflection axes, not turn rotations."
        };
        return Problem("usccss.g4.u08.l04.symmetry_axis_counts",
            $"An INDEPENDENTLY DESCRIBED geometric figure is {name}. How many lines of reflection symmetry does it have? {cue}",
            $"The described shape has {count} reflection line(s). This does not imply the original publisher shape card has the same geometry.",
            AssessmentItemType.Numeric,
            ("caseType", mode), ("variant", variant), ("axisCount", count));
    }

    private static ExactProblem BuildG4AdditiveAnglePart(Random random)
    {
        // 2021 IM/KH G4 U07:L13 numerical supplementary additivity.
        // Original p/q/r/s source cutouts and folded kite are teacher-led.
        var mode = random.Next(0,8);
        var variant = random.Next(0,3);
        var (total,known) = mode switch
        {
            0 => (90,62), 1 => (180,73),
            2 => (360,120), 3 => (90,34),
            4 => (180,120), 5 => (360,275),
            6 => (90,45), _ => (180,95)
        };
        var unknown = total-known;
        var cue = variant switch
        {
            0 => "Two adjacent angles make up the total turn.",
            1 => "The ray lengths do not affect angular measurement.",
            _ => "Subtract the known part from the right, straight or full turn."
        };
        return Problem("usccss.g4.u07.l13.additive_angle_part",
            $"FIRST-PARTY angle pair: two non-overlapping adjacent angles together measure {total} degrees. " +
            $"One is {known} degrees. How many degrees is the OTHER angle? {cue}",
            $"The total turn is {total} degrees and the known part is {known} degrees, so the missing part is {total}-{known}={unknown} degrees.",
            AssessmentItemType.Numeric,
            ("caseType",mode),("variant",variant),
            ("totalDegrees",total),("knownDegrees",known),
            ("unknownDegrees",unknown));
    }

    private static ExactProblem BuildG4AngleClassificationByBounds(Random random)
    {
        // Source IM 2021 Grade 4 U07:L12: acute, right,
        // obtuse and straight angles. First-party numeric classification
        // does NOT inspect source cards, letter Y drawings, or paper angles.
        var mode = random.Next(0, 8);
        var variant = random.Next(0, 3);
        int[] measures = [20, 35, 45, 70, 90, 110, 130, 180];
        var degrees = measures[mode];
        var answer = degrees < 90 ? 1 : degrees == 90 ? 2 :
            degrees < 180 ? 3 : 4;
        var cue = variant switch
        {
            0 => "One ray starts at the vertex and another defines the measured turn.",
            1 => "The line segments may have any length; only the opening is classified.",
            _ => "A straight angle can look like a line, but it is still a 180-degree turn."
        };
        return Problem("usccss.g4.u07.l12.classify_angles_by_degree_bounds",
            $"An INDEPENDENTLY AUTHORED angle measures {degrees} degrees. " +
            "Classify it using 1=acute, 2=right, 3=obtuse, 4=straight. " + cue,
            $"An angle of {degrees} degrees is " +
            (answer == 1 ? "acute (less than 90 degrees)." :
             answer == 2 ? "right (exactly 90 degrees)." :
             answer == 3 ? "obtuse (between 90 and 180 degrees)." :
             "straight (exactly 180 degrees)."),
            AssessmentItemType.Numeric,
            ("caseType", mode), ("variant", variant),
            ("degrees", degrees), ("classCode", answer));
    }

    private static ExactProblem BuildG4PerpendicularRightAngleJudgment(Random random)
    {
        // IM/KH 2021 Grade 4 Unit 7 Lesson 10: one right angle formed at
        // an intersection characterizes perpendicular LINES and EXTENDED
        // line segments. Student paper folds and original polygon diagrams
        // are handled by teachers, not represented as source-figure copies.
        var mode = random.Next(0, 8);
        var variant = random.Next(0, 3);
        int[] angleMeasures = [90, 90, 90, 90, 75, 80, 100, 120];
        var degrees = angleMeasures[mode];
        var answer = degrees == 90 ? 1 : 2;
        var context = mode switch
        {
            0 => "One line is horizontal and another goes straight upward at the intersection.",
            1 => "Two drawn lines cross diagonally at an accurately measured angle.",
            2 => "Two polygon sides meet at a shared corner; extend their supporting straight lines.",
            3 => "Two different paper creases go through point P and form four angles.",
            4 => "Two lines cross; one angle is acute.",
            5 => "Two lines cross; an acute angle is measured.",
            6 => "Two lines cross; one angle is obtuse.",
            _ => "Two lines cross; one angle is obtuse."
        };
        var cue = variant switch
        {
            0 => "Do not judge by apparent line length.",
            1 => "Use 90 degrees as the right-angle benchmark.",
            _ => "Think about the four angles around a shared intersection."
        };
        return Problem("usccss.g4.u07.l10.perpendicular_right_angle_judgment",
            $"FIRST-PARTY textual lines: {context} ONE of the angles at the intersection measures {degrees} degrees. Are the supporting two straight lines PERPENDICULAR? Answer 1 for YES, 2 for NO. {cue}",
            $"Perpendicular lines intersect at 90 degrees. The stated angle is {degrees} degrees, so the correct code is {answer} ({(answer == 1 ? "YES" : "NO")}). A physical fold/drawing still needs teacher confirmation.",
            AssessmentItemType.Numeric,
            ("caseType", mode), ("variant", variant),
            ("angleDegrees", degrees), ("perpendicularAnswer", answer));
    }

    private static ExactProblem BuildG4ProtractorDualScaleReading(Random random)
    {
        // IM/KH 2021 G4 U07:L09: 1 degree is 1/360 of a full turn,
        // and the scale beginning at zero on the aligned ray is used.
        // This first-party textual protractor model does NOT measure
        // any original 2021 figure or the student's physical instrument.
        var mode = random.Next(0, 8);
        var variant = random.Next(0, 3);
        int[] benchmarks = [25, 45, 65, 80, 100, 115, 140, 160];
        var degrees = benchmarks[mode];
        var zeroScale = variant == 1 ? 1 : 0; // 0 outer, 1 inner
        var outer = zeroScale == 0 ? degrees : 180 - degrees;
        var inner = 180 - outer;
        var zeroName = zeroScale == 0 ? "OUTER" : "INNER";
        var cue = variant switch
        {
            0 => "The first ray has been aligned with the zero at the outer edge.",
            1 => "The first ray has been aligned with the zero at the inner edge.",
            _ => "Be careful not to choose the opposite zero scale."
        };
        var prompt = $"A FIRST-PARTY half-circle protractor description has two opposite scales. Its OUTER scale reads {outer}° and INNER scale reads {inner}° where the second ray crosses the rim. The initial ray is aligned with the ZERO of the {zeroName} scale. What is the angle size in degrees? {cue}";
        return Problem("usccss.g4.u07.l09.protractor_dual_scale_reading", prompt,
            $"The zero of the {zeroName} scale is aligned with the initial ray. Read the SAME scale: {degrees}°. The other scale would give {180 - degrees}°, which is not the aligned reading.",
            AssessmentItemType.Numeric,
            ("caseType", mode), ("variant", variant),
            ("zeroScale", zeroScale), ("outerReading", outer),
            ("innerReading", inner), ("targetDegrees", degrees));
    }

    private static ExactProblem BuildG4FullTurnDegreeBenchmarks(Random random)
    {
        // 2021 IM Grade 4 Unit 7 Lesson 8: a 360-degree full turn and
        // benchmark degree/fraction angles. Paper half-circle measuring tool
        // and original figures remain teacher-reviewed; no image-copy claim.
        var mode = random.Next(0, 8);
        var kind = random.Next(0, 3);
        var variant = random.Next(0, 3);
        int[] degrees = [45, 60, 90, 120, 180, 240, 270, 360];
        int[] numerators = [1, 1, 1, 1, 1, 2, 3, 1];
        int[] denominators = [8, 6, 4, 3, 2, 3, 4, 1];
        var deg = degrees[mode];
        var num = numerators[mode];
        var den = denominators[mode];
        var promptTask = kind switch
        {
            0 => "Express this turn as a fraction of a full turn in LOWEST TERMS. What is the NUMERATOR?",
            1 => "Express this turn as a fraction of a full turn in LOWEST TERMS. What is the DENOMINATOR?",
            _ => "How many more DEGREES must the ray turn in the same direction to make exactly one 360-degree full turn?"
        };
        var cue = variant switch
        {
            0 => "Remember that a full turn around the shared endpoint is 360 degrees.",
            1 => "Use an imagined circle divided into equal angular parts.",
            _ => "You may reason with a half-turn 180 degrees or quarter-turn 90 degrees."
        };
        return Problem("usccss.g4.u07.l08.full_turn_degree_benchmarks",
            $"A ray turns {deg} degrees around its fixed vertex. {promptTask} {cue}",
            $"The turn is {deg}/360 = {num}/{den} of a full revolution. The remaining turn is {360 - deg} degrees.",
            AssessmentItemType.Numeric,
            ("caseType", mode), ("questionKind", kind), ("variant", variant),
            ("degrees", deg), ("numerator", num), ("denominator", den));
    }

    private static ExactProblem BuildG4ClockHourAngleTurnCompare(Random random)
    {
        // Source IM 2021 Grade 4 Unit 7 Lesson 7: use a CLOCK to model
        // turns. At EXACT hours (minute hand at 12) each hour-mark
        // separation represents 1/12 full turn. The smaller angle
        // has at most six hour-mark intervals. Not minute-half-hour math.
        var mode = random.Next(0, 8);
        var kind = random.Next(0, 2); // compare vs "by how many intervals?"
        var variant = random.Next(0, 3);
        var (hourA, hourB) = mode switch
        {
            0 => (3, 5),
            1 => (8, 2),
            2 => (3, 9),
            3 => (6, 5),
            4 => (10, 4),
            5 => (7, 8),
            6 => (1, 11),
            _ => (4, 6)
        };
        var aSteps = Math.Min(hourA, 12 - hourA);
        var bSteps = Math.Min(hourB, 12 - hourB);
        var task = kind == 0
            ? "Which smaller angle is larger: 1=clock A, 2=clock B, or 3=equal?"
            : "By HOW MANY 1-hour-mark intervals do the two smaller angle openings differ? Give a nonnegative whole number.";
        var context = variant switch
        {
            0 => "Treat both clock hands as rays starting from the clock centre.",
            1 => "Both clocks show exactly an hour; neither minute hand has moved past 12.",
            _ => "Follow the SHORTER turn from 12 to the hour hand, not the hand's printed length."
        };
        var prompt = $"At exactly {hourA}:00 (clock A) and exactly {hourB}:00 (clock B), each minute hand points to 12. Each hour mark is one twelfth of a full turn. {task} {context}";
        var solution = $"At exactly {hourA}:00 the smaller opening is {aSteps} hour-mark intervals; at exactly {hourB}:00 it is {bSteps}. " +
            (kind == 0 ? "Compare those openings; lengths of the drawn hands cannot change the size." :
            $"They differ by {Math.Abs(aSteps - bSteps)} intervals. One interval equals 30 degrees, though no protractor calculation is required.");
        return Problem("usccss.g4.u07.l07.clock_hour_angle_turn_compare", prompt, solution, AssessmentItemType.Numeric,
            ("caseType", mode), ("questionKind", kind), ("variant", variant),
            ("hourA", hourA), ("hourB", hourB),
            ("stepA", aSteps), ("stepB", bSteps));
    }

    private static ExactProblem BuildG4CompareAngleOpenings(Random random)
    {
        // IM 2021 G4 U07:L06 explores size as the opening/turn, not ray length.
        // Eight first-party compass directions are 1/8 of a turn apart.
        // Both compared angle openings are at most a half turn; no
        // original A-P picture cards or protected publisher tasks are graded.
        var mode = random.Next(0, 8);
        var variant = random.Next(0, 3);
        var (steps1, steps2) = mode switch
        {
            0 => (1, 2),
            1 => (3, 1),
            2 => (2, 2),
            3 => (4, 3),
            4 => (1, 4),
            5 => (3, 2),
            6 => (4, 4),
            _ => (2, 3)
        };
        var start1 = variant * 2;
        var start2 = (start1 + 2) % 8;
        var end1 = (start1 + steps1) % 8;
        var end2 = (start2 + steps2) % 8;
        string[] directions = ["north", "northeast", "east", "southeast",
            "south", "southwest", "west", "northwest"];
        var prompt = $"On a compass, adjacent named directions are one eighth of a full turn apart. Angle A has two arms from P pointing {directions[start1]} and {directions[end1]}; Angle B has two arms from Q pointing {directions[start2]} and {directions[end2]}. Compare the SMALLER openings, ignoring how long the arms are drawn: 1=A larger, 2=B larger, or 3=equal.";
        var result = steps1 > steps2 ? "A is larger" :
            steps1 < steps2 ? "B is larger" : "the openings are equal";
        return Problem("usccss.g4.u07.l06.compare_angle_openings", prompt,
            $"Count the compass intervals in each smaller opening: A has {steps1} eighth-turn steps, B has {steps2}. Therefore {result}. Arm length cannot change the opening.",
            AssessmentItemType.Numeric,
            ("caseType", mode), ("variant", variant),
            ("start1", start1), ("end1", end1),
            ("start2", start2), ("end2", end2),
            ("steps1", steps1), ("steps2", steps2));
    }

    private static ExactProblem BuildG4AngleCommonVertexRays(Random random)
    {
        // Original 2021 G4 U07:L05 introduces angles as two distinct rays
        // with a common starting vertex. Source pictures, partner drawings
        // and the inaccessible publisher Cool-down are teacher-only.
        var mode = random.Next(0, 8);
        var variant = random.Next(0, 3);
        var (start1, start2, direction1, direction2) = mode switch
        {
            0 => (0, 0, 2, 0), // P east and north
            1 => (1, 1, 1, 4), // Q northeast and south
            2 => (2, 2, 6, 0), // R west and north
            3 => (3, 3, 3, 5), // S southeast and southwest
            4 => (0, 1, 2, 0), // different starting points
            5 => (1, 2, 1, 4),
            6 => (2, 3, 6, 0),
            _ => (3, 0, 3, 5)
        };
        string[] pointNames = ["P", "Q", "R", "S"];
        string[] directions = ["north", "northeast", "east", "southeast",
            "south", "southwest", "west", "northwest"];
        var reminder = variant switch
        {
            0 => "Treat each arm as a ray continuing indefinitely.",
            1 => "The apparent length of the drawn arms does not determine the angle.",
            _ => "Look at the START of each ray, not where its drawing happens to end."
        };
        var prompt = $"Ray 1 starts at {pointNames[start1]} and extends {directions[direction1]}. Ray 2 starts at {pointNames[start2]} and extends {directions[direction2]}. Do THESE TWO named rays make an angle with one common starting vertex? Answer 1=yes or 2=no. {reminder}";
        var solution = start1 == start2
            ? $"Yes: both rays start at {pointNames[start1]} and head in different directions. That point is the angle vertex."
            : $"No: the rays start at different points ({pointNames[start1]} and {pointNames[start2]}), so as specified they do not share one starting vertex.";
        return Problem("usccss.g4.u07.l05.angle_common_vertex_rays", prompt, solution, AssessmentItemType.Numeric,
            ("caseType", mode), ("variant", variant), ("start1", start1),
            ("start2", start2), ("direction1", direction1), ("direction2", direction2));
    }

    private static ExactProblem BuildG4ParallelIntersectDotGrid(Random random)
    {
        // IM 2021 G4 U07:L03: first-party dot-grid support for relationships
        // between two distinct infinite lines. Original drawing, figure
        // counting, and student explanations remain teacher-assessed.
        var mode = random.Next(0, 8);
        var variant = random.Next(0, 3);
        var (r1, c1, r2, c2, r3, c3, r4, c4) = mode switch
        {
            0 => (1, 1, 1, 4, 3, 1, 3, 4), // horizontal parallels
            1 => (1, 1, 4, 1, 1, 3, 4, 3), // vertical parallels
            2 => (1, 1, 3, 3, 1, 3, 3, 5), // ascending parallels
            3 => (1, 5, 3, 3, 1, 3, 3, 1), // descending parallels
            4 => (2, 1, 2, 5, 1, 3, 4, 3), // right-angle crossing
            5 => (2, 1, 2, 5, 1, 1, 4, 4), // diagonal crosses horizontal
            6 => (1, 3, 4, 3, 1, 5, 4, 2), // diagonal crosses vertical
            _ => (1, 1, 4, 4, 1, 4, 4, 1)  // diagonals cross between dots
        };
        string[] firstNames = ["h", "m", "p"];
        string[] secondNames = ["g", "n", "q"];
        var prompt = $"On a 5 by 5 dot field, line {firstNames[variant]} passes through the dots at row {r1}, column {c1} and row {r2}, column {c2}. Line {secondNames[variant]} passes through row {r3}, column {c3} and row {r4}, column {c4}. Imagine BOTH straight lines extended indefinitely. Are the two different lines 1=parallel (never meet) or 2=intersecting (meet somewhere)? Answer 1 or 2.";
        var solution = mode < 4
            ? "The two lines have the same direction and are at different positions, so extending both does not make them meet. They are parallel."
            : "The two lines have different directions; when extended, they meet at one point, even when that point is between grid dots. They are intersecting.";
        return Problem("usccss.g4.u07.l03.dot_grid_parallel_intersect", prompt, solution,
            AssessmentItemType.Numeric,
            ("caseType", mode), ("variant", variant),
            ("r1", r1), ("c1", c1), ("r2", r2), ("c2", c2),
            ("r3", r3), ("c3", c3), ("r4", r4), ("c4", c4));
    }

    private static ExactProblem BuildG4PointLineRayCardAndNumberTalk(Random random)
    {
        // IM 2021 G4 U07:L02: four geometric figure categories and the four
        // original mental subtraction expressions. Original physical card sort
        // and student drawings remain teacher reviewed, not auto-graded.
        var mode = random.Next(0, 8);
        var variant = random.Next(0, 3);
        string[] letters = ["A and B", "P and Q", "R and S"];
        var names = letters[variant];
        var figure = mode switch
        {
            0 => $"A single marked dot labelled {names.Split(' ')[0]}.",
            1 => $"A straight geometric path passing through {names} and extending indefinitely in BOTH directions.",
            2 => $"A straight path that starts at the first of {names} and extends through the second in ONE direction only.",
            _ => $"A finite straight part with exactly two endpoints labelled {names}."
        };
        var first = mode switch
        {
            4 => 90,
            5 or 6 => 270,
            7 => 360,
            _ => 0
        };
        var second = mode switch
        {
            4 or 5 => 45,
            6 or 7 => 135,
            _ => 0
        };
        var prompt = mode < 4
            ? $"Read this first-party geometry card: {figure} Which type is described? Answer 1=point, 2=line, 3=ray or 4=line segment."
            : $"Mental number talk: calculate {first} − {second}. {(variant == 0 ? "Use a known benchmark." : variant == 1 ? "Explain the place-value decomposition." : "Check by adding your difference back.")}";
        var solution = mode < 4
            ? "Use dimension and endpoints: a point is a location, a line extends both ways, a ray starts at one point and continues one way, and a segment has two endpoints."
            : $"Use a subtraction strategy; check {first - second} + {second} = {first}.";
        return Problem("usccss.g4.u07.l02.geometry_cards_number_talk", prompt, solution, AssessmentItemType.Numeric,
            ("caseType", mode), ("variant", variant), ("first", first), ("second", second));
    }

    private static ExactProblem BuildG4GeometricDescriptionPrecision(Random random)
    {
        // Source IM 2021 G4 U07:L01: students compare how precisely partners
        // describe locations and straight marks, not formal line/segment definitions.
        // Independent first-party examples, never copies of protected figure cards.
        var mode = random.Next(0, 6);
        var preciseFirst = random.Next(0, 2);
        var descriptions = mode switch
        {
            0 => ("Put a line somewhere near the top.",
                  "Draw a horizontal straight stroke through the two marked top-row dots."),
            1 => ("Draw a big shape here.",
                  "Join the three dots labelled A, B and C to draw a triangle."),
            2 => ("Put that mark over there.",
                  "Mark point P where the vertical stroke crosses the horizontal stroke."),
            3 => ("Draw something diagonal.",
                  "Draw a straight stroke through dots P and R, extending it toward both paper edges."),
            4 => ("Make a quadrilateral.",
                  "Connect the four corner dots and describe which strokes are horizontal and vertical."),
            _ => ("Add a dot there.",
                  "Mark a dot halfway between labelled points A and B on the top straight stroke.")
        };
        var first = preciseFirst == 1 ? descriptions.Item2 : descriptions.Item1;
        var second = preciseFirst == 1 ? descriptions.Item1 : descriptions.Item2;
        return Problem("usccss.g4.u07.l01.geometric_description_precision",
            $"A drawing partner cannot see your card. Which instruction is clearer about position or direction? 1) {first} 2) {second}. Answer 1 or 2.",
            "Precise directions name points, relative locations, orientation, or endpoints; then compare the partner's drawing. This checks clarity, not the original open drawing.",
            AssessmentItemType.Numeric,
            ("caseType", mode), ("preciseFirst", preciseFirst));
    }

    private static ExactProblem BuildG4FlowerPatterns(Random random)
    {
        // IM K-5 2021 G4 U06:L25 optional source: 12-flower repeating
        // cycles, 25 of each garland, 155/16, and two-worker timing.
        // Original physical flower construction is teacher-led.
        var mode = random.Next(0, 6);
        var garlands = random.Next(10, 41);
        var made = random.Next(120, 251);
        const int perGarland = 16, priyaBig = 6, priyaSmall = 6;
        const int jadaBig = 4, jadaSmall = 8, cut = 1, finish = 2, workers = 2;
        var question = mode switch
        {
            0 => $"Each 12-flower Priya garland repeats 2 big and 2 small, and each 12-flower Jada garland repeats 1 big and 2 small. If they make {garlands} garlands of each type, how many big flowers altogether?",
            1 => $"Each 12-flower Priya garland repeats 2 big and 2 small, and each 12-flower Jada garland repeats 1 big and 2 small. If they make {garlands} garlands of each type, how many small flowers altogether?",
            2 => $"Two students have {made} paper flowers. Each complete garland requires {perGarland} flowers. How many complete garlands can they make?",
            3 => $"Two students have {made} paper flowers. Each complete garland requires {perGarland} flowers. How many flowers will be left after making as many complete garlands as possible?",
            4 => $"Making each paper flower needs {cut} minute to cut strips and {finish} minutes to finish. For {made} flowers, how many total worker-minutes of effort are needed?",
            _ => $"Making each of {made} paper flowers takes {cut} minute to cut and {finish} minutes to finish. Two students work simultaneously and split the flower count as equally as possible, completing their own flowers one at a time. How many minutes until all are finished?"
        };
        var solution = mode switch
        {
            0 => $"Priya uses {priyaBig} big and Jada {jadaBig} big per garland: multiply {garlands} by their combined total.",
            1 => $"Priya uses {priyaSmall} small and Jada {jadaSmall} small per garland: multiply {garlands} by their combined total.",
            2 => $"Divide {made} by {perGarland} and use only whole garlands.",
            3 => $"Divide {made} by {perGarland}; the remainder cannot form a whole garland.",
            4 => $"Each flower uses {cut}+{finish} minutes. Multiply by {made} flowers for total worker-minutes.",
            _ => $"Divide {made} flowers between two students as evenly as possible. The larger share takes {((made + 1) / 2)} flowers times {cut + finish} minutes; both work at once."
        };
        return Problem("usccss.g4.u06.l25.flower_garland_patterns", question, solution, AssessmentItemType.Numeric,
            ("caseType", mode), ("garlandsEach", garlands), ("flowersMade", made),
            ("flowersPerGarland", perGarland), ("priyaBig", priyaBig),
            ("priyaSmall", priyaSmall), ("jadaBig", jadaBig),
            ("jadaSmall", jadaSmall), ("cutMinutes", cut),
            ("finishMinutes", finish), ("workers", workers));
    }

    private static ExactProblem BuildG4LanguageComparisons(Random random)
    {
        // IM K-5 2021 G4 U06:L24: mathematical structure of original
        // Census tasks; independently generated first-party data, not publisher figures.
        var mode = random.Next(0, 6);
        var navajo = random.Next(135000, 190001);
        var yupik = random.Next(17500, 23501);
        var dakota = random.Next(15000, 19001);
        var apache = random.Next(11000, 16001);
        var keres = random.Next(11000, 14001);
        var cherokee = random.Next(9000, 13001);
        var ojibwa = random.Next(8000, 11001);
        var choctaw = random.Next(8000, 11001);
        var zuni = random.Next(8000, 11001);
        var pima = random.Next(6000, 8001);
        var english = random.Next(1100000, 1350001);
        var spanish = random.Next(110000, 150001);
        var indo = random.Next(5000, 9001);
        var asian = random.Next(200, 601);
        var chicagoSpanish = random.Next(395000, 460001);
        var chicagoIndo = random.Next(20000, 32001);
        var nine = yupik + dakota + apache + keres + cherokee + ojibwa + choctaw + zuni + pima;
        var prompt = mode switch
        {
            0 => $"In a sample language table, Navajo has {navajo} speakers and Yupik has {yupik}. How many more speak Navajo?",
            1 => $"A sample table lists Navajo {navajo}, Apache {apache} and Cherokee {cherokee} speakers. How many speak these three languages altogether?",
            2 => $"A sample language table has Navajo {navajo} speakers. The other nine counts are {yupik}, {dakota}, {apache}, {keres}, {cherokee}, {ojibwa}, {choctaw}, {zuni}, and {pima}. How many more speak Navajo than all nine combined?",
            3 => $"In a sample city table, {english} speak English only, while {spanish} speak Spanish, {indo} other Indo-European, and {asian} Asian languages. By how many does the English-only count exceed the other categories combined?",
            4 => $"In two sample cities, Chicago has {chicagoSpanish} Spanish and {chicagoIndo} other Indo-European speakers, and Philadelphia has {spanish} and {indo} respectively. How many more people speak those two language categories in Chicago?",
            _ => $"In a sample language table, {navajo} speak Navajo and {yupik} speak Yupik. About how many times as many speak Navajo? Round the ratio to the nearest whole number."
        };
        var solution = mode switch
        {
            0 => "Subtract Yupik speakers from Navajo speakers and check by addition.",
            1 => "Add Navajo, Apache and Cherokee counts, grouping to check the total.",
            2 => $"Sum all nine other categories to {nine}, then subtract from the Navajo count. Check by addition.",
            3 => "Add all three non-English categories, then subtract from the English-only count.",
            4 => "Add the two Chicago language groups, add the two Philadelphia groups, then subtract. Check with a rounded estimate.",
            _ => "Divide the larger count by the smaller and round to the nearest whole number; compare nearby multiples for reasonableness."
        };
        return Problem("usccss.g4.u06.l24.language_population_comparisons", prompt, solution, AssessmentItemType.Numeric,
            ("caseType", mode), ("navajo", navajo), ("yupik", yupik),
            ("dakota", dakota), ("apache", apache), ("keres", keres),
            ("cherokee", cherokee), ("ojibwa", ojibwa), ("choctaw", choctaw),
            ("zuni", zuni), ("pima", pima),
            ("philadelphiaEnglish", english), ("philadelphiaSpanish", spanish),
            ("philadelphiaIndo", indo), ("philadelphiaAsian", asian),
            ("chicagoSpanish", chicagoSpanish), ("chicagoIndo", chicagoIndo));
    }

    private static ExactProblem BuildG4FourOperations(Random random)
    {
        // IM K-5 2021 Grade 4 U06:L23: travel segments, round trips,
        // music-distance inversion, miles comparison, and weekly step table.
        // Reuse the existing exact whole-number kernel; no new engine.
        var mode = random.Next(0, 6);
        var a = random.Next(100, 201);
        var b = random.Next(100, 201);
        var c = random.Next(100, 201);
        var d = random.Next(100, 201);
        const int longTrips = 5, musicTrips = 3, feetPerMile = 5280, days = 7, target = 120000;
        var musicTotal = random.Next(200, 401) * musicTrips * 2;
        var week1 = random.Next(3800, 4901) * days;
        var week2 = week1 - random.Next(2000, 6001);
        var week3 = random.Next(32000, 38001);
        var oneWay = a + b + c + d;
        var roundTrip = 2 * oneWay;
        var prompt = mode switch
        {
            0 => $"A pupil travels {a}, {b}, {c}, and {d} feet between classrooms, and returns along the same route. How many feet is one whole round trip?",
            1 => $"A pupil makes {musicTrips} round trips between homeroom and music. Altogether those trips cover {musicTotal} feet. How far away is music one way?",
            2 => $"A pupil makes {longTrips} classroom round trips on a route with one-way segments {a}, {b}, {c}, and {d} feet, and also walks {musicTotal} feet to music in the week. How many feet short of two miles is the weekly total? (1 mile = {feetPerMile} feet.)",
            3 => $"A pupil takes {week1} steps in one week, the same number each of {days} days. How many steps per day?",
            4 => $"A fitness challenge has {week1} steps in week one and {week2} in week two. How many fewer steps were taken in week two?",
            _ => $"A four-week challenge goal is {target} steps. The first three weeks are {week1}, {week2}, and {week3} steps. How many more steps are needed in week four?"
        };
        var solution = mode switch
        {
            0 => $"Add segments: {oneWay} feet one way. Double to include returning: {roundTrip} feet.",
            1 => $"There are {2 * musicTrips} one-way journeys. Divide {musicTotal} by {2 * musicTrips}.",
            2 => $"Double the {oneWay}-foot route, multiply by {longTrips}, add {musicTotal} music feet, then subtract from {2 * feetPerMile}.",
            3 => $"Share {week1} steps equally among {days} days, then check by multiplication.",
            4 => $"Subtract the week-two total {week2} from week one {week1}; check by adding the difference back.",
            _ => $"Add the three known weeks ({week1} + {week2} + {week3}), subtract from {target}, then check the four-week total."
        };
        return Problem("usccss.g4.u06.l23.four_operation_contexts", prompt, solution, AssessmentItemType.Numeric,
            ("caseType", mode), ("segmentA", a), ("segmentB", b),
            ("segmentC", c), ("segmentD", d), ("longTrips", longTrips),
            ("musicTrips", musicTrips), ("musicTotalFeet", musicTotal),
            ("feetPerMile", feetPerMile), ("days", days), ("week1", week1),
            ("week2", week2), ("week3", week3), ("stepTarget", target));
    }

    private static ExactProblem BuildG4MultiStepContext(Random random)
    {
        // 2021 IM G4 U06:L21. Reuse multiplication and exact division with
        // three context-dependent questions: budget, days, and concessions.
        var caseType = random.Next(0, 3);
        var count = caseType == 0 ? random.Next(30, 66) : random.Next(160, 391);
        var ticketPrice = caseType == 0 ? random.Next(10, 24) : random.Next(7, 14);
        var revenue = count * ticketPrice;
        var budget = revenue + random.Next(20, 201);
        const int days = 2;
        var drink = random.Next(5, 13);
        var popcorn = random.Next(2, 10);
        var prompt = caseType switch
        {
            0 => "A museum trip has " + count + " students and tickets cost $" + ticketPrice + " each. Teachers have $" + budget + ". How many dollars remain after buying all tickets?",
            1 => "A movie theater earned $" + revenue + " from $" + ticketPrice + " tickets each day, for two equal-sales days. How many tickets were sold over both days?",
            _ => "A movie theater earned $" + revenue + " from $" + ticketPrice + " tickets each day, for two equal-sales days. Every ticket holder buys a $" + drink + " drink and $" + popcorn + " popcorn. What is the total in dollars from these extra purchases?"
        };
        var solution = caseType switch
        {
            0 => "Ticket cost = " + count + " × " + ticketPrice + " = " + revenue + "; subtract from $" + budget + ".",
            1 => "Divide $" + revenue + " by $" + ticketPrice + " to find tickets per day, then double.",
            _ => "Divide daily revenue by ticket price, double attendance, then multiply by (" + drink + " + " + popcorn + ")."
        };
        return Problem("usccss.g4.u06.l21.multistep_contexts",
            prompt, solution, AssessmentItemType.Numeric,
            ("caseType", caseType), ("count", count), ("ticketPrice", ticketPrice),
            ("budget", budget), ("days", days), ("drinkPrice", drink),
            ("popcornPrice", popcorn), ("dailyRevenue", revenue));
    }

    private static ExactProblem BuildG4PartialQuotients(Random random)
    {
        // IM K-5 G4 2021 U06:L18: use equations and subtraction for partial quotients.
        var divisor = random.Next(2, 10);
        var lower = Math.Max(20, (100 + divisor - 1) / divisor);
        var upper = Math.Min(125, 999 / divisor);
        var quotient = random.Next(lower, upper + 1);
        var tens = quotient / 10 * 10;
        var ones = quotient % 10;
        var dividend = divisor * quotient;
        return Problem(
            "usccss.g4.u06.l18.partial_quotients_recording",
            "Use partial quotients to divide " + dividend + " by " + divisor +
                ". Record two partial quotients in equations or a subtraction diagram.",
            "Decompose " + dividend + " into " + (divisor * tens) + " + " + (divisor * ones) +
                ". Divide each part by " + divisor + " to get " + tens + " and " + ones +
                "; add partial quotients to get " + quotient + ". Check by multiplication.",
            AssessmentItemType.Numeric,
            ("dividend", dividend),
            ("divisor", divisor),
            ("tensQuotient", tens),
            ("onesQuotient", ones));
    }

    private static ExactProblem BuildG4BaseTenDivision(Random random)
    {
        var d=random.Next(2,10);
        var q=random.Next(12,Math.Min(120,999/d)+1);
        var n=d*q;
        var tens=n/10%10+10*((n/100)%d);
        var ones=n%10+10*(tens%d);
        return Problem("usccss.g4.u06.l16.base_ten_equal_groups",
            "Share "+n+" using base-ten blocks into "+d+" equal groups.",
            "Exchange hundreds to tens and tens to ones. Each group receives "+q+". Check multiplication.",
            AssessmentItemType.Numeric,("dividend",n),("divisor",d),
            ("tensAfterTrade",tens),("onesAfterTrade",ones));
    }

    private static ExactProblem BuildG4MuralMissingSide(Random random, bool withRemainder)
    {
        // 2021 IM G4 U06:L15: first-party partial quotient and mural-remainder tasks.
        var width = random.Next(2, 10);
        var minLength = Math.Max(20, (100 + width - 1) / width);
        var maxLength = Math.Min(110, (999 - (width - 1)) / width);
        var length = random.Next(minLength, maxLength + 1);
        var leftover = withRemainder ? random.Next(1, width) : 0;
        var area = width * length + leftover;
        var tens = (length / 10) * 10;
        var ones = length - tens;
        var family = withRemainder ? "usccss.g4.u06.l15.mural_complete_columns" : "usccss.g4.u06.l15.area_partial_quotients";
        var prompt = withRemainder
            ? $"A student has {area} square tiles for a complete rectangular mural {width} tiles wide. What is the greatest whole-number length in tiles?"
            : $"A rectangular mural has area {area} square tiles and is {width} tiles wide. Find its length using partial quotients.";
        var solution = $"Use partial quotients: {width} × {tens} = {width * tens} and " +
            $"{width} × {ones} = {width * ones}. The full rectangle has " +
            $"{width} × {length} = {width * length} tiles. " +
            (withRemainder ? $"{leftover} tile(s) remain; they cannot form a full column." : "No tiles remain; check by multiplication.");
        return Problem(family, prompt, solution, AssessmentItemType.Numeric,
            ("knownSide", width), ("area", area), ("leftover", leftover));
    }

    private static ExactProblem BuildRectangleMissingSide(Random random, int scale)
    {
        var knownSide = random.Next(2, 8 + scale * 2);
        var missingSide = random.Next(3, 10 + scale * 3);
        var area = knownSide * missingSide;
        return Problem(
            "geometry.perimeter_area.rectangle_missing_side",
            $"A rectangle has area {area} and one side length {knownSide}. Find the other side length.",
            "Use area = length × width and divide the area by the known side. Multiply the two sides to verify the original area.",
            AssessmentItemType.Numeric,
            ("knownSide", knownSide),
            ("area", area),
            ("expectedSide", missingSide));
    }

    private static ExactProblem BuildPolygonInteriorAngleSum(Random random, int scale)
    {
        var sides = random.Next(3, 20 + scale * 4);
        return Problem(
            "geometry.polygons.interior_angle_sum",
            $"Find the sum of the interior angles of a {sides}-sided polygon.",
            "Split the polygon into n − 2 triangles, so the interior-angle sum is (n − 2) × 180°.",
            AssessmentItemType.Numeric,
            ("sides", sides));
    }

    private static ExactProblem BuildPolygonMissingInteriorAngle(Random random, int scale)
    {
        var sides = random.Next(0, 2) == 0 ? 3 : 4;
        var total = (sides - 2) * 180;
        var missing = sides == 3
            ? random.Next(30, 121)
            : random.Next(40, 151);
        var knownSum = total - missing;
        return Problem(
            "geometry.polygons.missing_interior_angle",
            $"A {sides}-sided polygon has known interior angles totalling {knownSum}°. Find the missing interior angle.",
            "Use the polygon interior-angle sum, subtract the sum of the known angles, and verify all interior angles total (n − 2) × 180°.",
            AssessmentItemType.Numeric,
            ("sides", sides),
            ("knownSum", knownSum),
            ("missingAngle", missing));
    }

    private static ExactProblem BuildRegularPolygonInteriorAngle(Random random, int scale)
    {
        int[] sideChoices = [3, 4, 5, 6, 8, 9, 10, 12, 15, 18, 20, 24, 30, 36, 40];
        var sides = sideChoices[random.Next(0, sideChoices.Length)];
        return Problem(
            "geometry.polygons.regular_interior_angle",
            $"Find one interior angle of a regular {sides}-sided polygon.",
            "For a regular polygon, each exterior angle is 360°/n, so each interior angle is 180° − 360°/n.",
            AssessmentItemType.Numeric,
            ("sides", sides));
    }

    private static ExactProblem BuildPythagorean(Random random, int scale)
    {
        var triples = new (int A, int B, int C)[] { (3, 4, 5), (5, 12, 13), (8, 15, 17), (7, 24, 25) };
        var triple = triples[random.Next(triples.Length)];
        var multiplier = random.Next(1, 6 + scale * 2);
        var a = triple.A * multiplier;
        var b = triple.B * multiplier;
        var hypotenuse = triple.C * multiplier;

        return Problem(
            "geometry.right_triangle.pythagorean.exact",
            $"A right triangle has perpendicular sides {a} and {b}. Find the hypotenuse.",
            "Use a² + b² = c² and take the positive square root.",
            AssessmentItemType.Numeric,
            ("legA", a),
            ("legB", b),
            ("hypotenuse", hypotenuse));
    }

    private static ExactProblem BuildPythagoreanFindLeg(Random random, int scale)
    {
        var triples = new (int A, int B, int C)[] { (3, 4, 5), (5, 12, 13), (8, 15, 17), (7, 24, 25) };
        var triple = triples[random.Next(triples.Length)];
        var multiplier = random.Next(1, 6 + scale * 2);
        var legA = triple.A * multiplier;
        var legB = triple.B * multiplier;
        var hypotenuse = triple.C * multiplier;
        var askLeg = random.Next(0, 2);
        var expectedLeg = askLeg == 0 ? legA : legB;
        var knownLeg = askLeg == 0 ? legB : legA;

        return Problem(
            "geometry.right_triangle.pythagorean.find_leg_exact",
            $"A right triangle has hypotenuse {hypotenuse} and one perpendicular side {knownLeg}. Find the other perpendicular side.",
            "Use a² + b² = c², subtract the known leg squared from the hypotenuse squared, then take the positive square root.",
            AssessmentItemType.Numeric,
            ("knownLeg", knownLeg),
            ("hypotenuse", hypotenuse),
            ("expectedLeg", expectedLeg),
            ("askLeg", askLeg));
    }

    private static ExactProblem BuildTrigonometricRatio(Random random, int scale)
    {
        var triples = new (int Opposite, int Adjacent, int Hypotenuse)[] { (3, 4, 5), (5, 12, 13), (8, 15, 17) };
        var triple = triples[random.Next(triples.Length)];
        // Preserve exact ratios while diversifying numerically distinct
        // triangles for certified 10-question practice sessions.
        var multiplier = random.Next(1, Math.Max(13, scale + 1));
        triple = (triple.Opposite * multiplier,
            triple.Adjacent * multiplier, triple.Hypotenuse * multiplier);
        var ratioType = random.Next(0, 3);
        var numerator = ratioType switch
        {
            0 => triple.Opposite,
            1 => triple.Adjacent,
            _ => triple.Opposite
        };
        var denominator = ratioType switch
        {
            0 => triple.Hypotenuse,
            1 => triple.Hypotenuse,
            _ => triple.Adjacent
        };
        var name = ratioType switch { 0 => "sin", 1 => "cos", _ => "tan" };

        return Problem(
            "trigonometry.right_triangle.ratio_exact",
            $"Relative to angle θ in a right triangle, opposite = {triple.Opposite}, adjacent = {triple.Adjacent}, hypotenuse = {triple.Hypotenuse}. Find {name}(θ) as a fraction.",
            "Use SOH-CAH-TOA and simplify the exact ratio.",
            AssessmentItemType.ShortAnswer,
            ("opposite", triple.Opposite),
            ("adjacent", triple.Adjacent),
            ("hypotenuse", triple.Hypotenuse),
            ("ratioType", ratioType),
            ("ratioNumerator", numerator),
            ("ratioDenominator", denominator),
            ("scale", scale));
    }

    private static ExactProblem BuildTrigonometricFindSide(Random random, int scale, bool modelling)
    {
        var triples = new (int Opposite, int Adjacent, int Hypotenuse)[] { (3, 4, 5), (5, 12, 13), (8, 15, 17) };
        var triple = triples[random.Next(triples.Length)];
        var multiplier = random.Next(1, Math.Max(13, scale + 1));
        var opposite = triple.Opposite * multiplier;
        var adjacent = triple.Adjacent * multiplier;
        var hypotenuse = triple.Hypotenuse * multiplier;
        var ask = random.Next(0, 2);
        var expected = ask == 0 ? opposite : adjacent;
        var prompt = modelling
            ? ask == 0
                ? $"A support cable forms a right triangle with ground distance {adjacent} and cable length {hypotenuse}. Find the vertical height."
                : $"A ramp forms a right triangle with height {opposite} and ramp length {hypotenuse}. Find the horizontal run."
            : ask == 0
                ? $"In a right triangle, the adjacent side is {adjacent} and the hypotenuse is {hypotenuse}. Using the matching exact trigonometric ratio, find the opposite side."
                : $"In a right triangle, the opposite side is {opposite} and the hypotenuse is {hypotenuse}. Using the matching exact trigonometric ratio, find the adjacent side.";

        return Problem(
            modelling ? "trigonometry.modelling.contextual" : "trigonometry.right_triangle.find_side_exact",
            prompt,
            "Identify the correct right-triangle ratio, substitute the exact known values, solve the missing side, and verify with Pythagoras.",
            AssessmentItemType.Numeric,
            ("opposite", opposite),
            ("adjacent", adjacent),
            ("hypotenuse", hypotenuse),
            ("expectedSide", expected),
            ("ask", ask));
    }

    private static ExactProblem BuildTrigonometricFindAngle(Random random)
    {
        var options = new (int Angle, int RatioType, int Numerator, int Denominator)[]
        {
            (30, 0, 1, 2), // sin 30
            (60, 1, 1, 2), // cos 60
            (45, 2, 1, 1)  // tan 45
        };
        var item = options[random.Next(options.Length)];
        var multiplier = random.Next(1, 7);
        var numerator = item.Numerator * multiplier;
        var denominator = item.Denominator * multiplier;
        var name = item.RatioType switch { 0 => "sin", 1 => "cos", _ => "tan" };
        var ratioDescription = item.RatioType switch
        {
            0 => $"opposite side {numerator} and hypotenuse {denominator}",
            1 => $"adjacent side {numerator} and hypotenuse {denominator}",
            _ => $"opposite side {numerator} and adjacent side {denominator}"
        };

        return Problem(
            "trigonometry.right_triangle.find_angle_exact",
            $"For an acute angle θ in a right triangle, the {ratioDescription}, so {name}(θ) = {numerator}/{denominator}. Find θ in degrees.",
            "Reduce the exact side ratio, match it to the standard special-angle trigonometric values, and verify the angle is acute.",
            AssessmentItemType.Numeric,
            ("ratioType", item.RatioType),
            ("ratioNumerator", numerator),
            ("ratioDenominator", denominator),
            ("expectedAngle", item.Angle),
            ("multiplier", multiplier));
    }

    private static ExactProblem BuildVectorBinary(
        Random random,
        int scale,
        string family,
        bool subtract)
    {
        var bound = 4 + scale * 3;
        var ax = NonZero(random, -bound, bound + 1);
        var ay = NonZero(random, -bound, bound + 1);
        var bx = NonZero(random, -bound, bound + 1);
        var by = NonZero(random, -bound, bound + 1);
        var symbol = subtract ? "−" : "+";
        var operation = subtract ? "subtract" : "add";

        return Problem(
            family,
            $"Let a = <{ax}, {ay}> and b = <{bx}, {by}>. Find a {symbol} b.",
            $"{operation} corresponding vector components independently, then check both coordinates.",
            AssessmentItemType.ShortAnswer,
            ("ax", ax),
            ("ay", ay),
            ("bx", bx),
            ("by", by));
    }

    private static ExactProblem BuildVectorScalarMultiply(Random random, int scale)
    {
        var bound = 4 + scale * 2;
        var ax = NonZero(random, -bound, bound + 1);
        var ay = NonZero(random, -bound, bound + 1);
        var scalar = NonZero(random, -2 - scale, 3 + scale);

        return Problem(
            "vectors.scalar_multiply.exact_rational",
            $"Let a = <{ax}, {ay}>. Find {scalar}a.",
            "Multiply every component by the same scalar and verify the direction/scale change componentwise.",
            AssessmentItemType.ShortAnswer,
            ("ax", ax),
            ("ay", ay),
            ("scalar", scalar));
    }

    private static ExactProblem BuildVectorDot(Random random, int scale)
    {
        var bound = 4 + scale * 2;
        var ax = NonZero(random, -bound, bound + 1);
        var ay = NonZero(random, -bound, bound + 1);
        var bx = NonZero(random, -bound, bound + 1);
        var by = NonZero(random, -bound, bound + 1);

        return Problem(
            "vectors.dot.exact_rational",
            $"Let a = <{ax}, {ay}> and b = <{bx}, {by}>. Find a · b.",
            "Multiply corresponding components and add the products: ax·bx + ay·by.",
            AssessmentItemType.Numeric,
            ("ax", ax),
            ("ay", ay),
            ("bx", bx),
            ("by", by));
    }

    private static ExactProblem BuildVectorMagnitude(Random random, int scale)
    {
        var triples = new (int A, int B, int C)[] { (3, 4, 5), (5, 12, 13), (8, 15, 17), (7, 24, 25) };
        var triple = triples[random.Next(triples.Length)];
        var multiplier = random.Next(1, Math.Max(2, scale + 1));
        var ax = triple.A * multiplier * (random.Next(0, 2) == 0 ? 1 : -1);
        var ay = triple.B * multiplier * (random.Next(0, 2) == 0 ? 1 : -1);
        var magnitude = triple.C * multiplier;

        return Problem(
            "vectors.magnitude.exact",
            $"Find the magnitude of v = <{ax}, {ay}>.",
            "Use |v| = √(x² + y²). Magnitude is non-negative; verify by squaring the result.",
            AssessmentItemType.Numeric,
            ("ax", ax),
            ("ay", ay),
            ("magnitude", magnitude));
    }

    private static ExactProblem BuildVectorBetweenPoints(Random random, int scale)
    {
        var bound = 4 + scale * 3;
        var x1 = random.Next(-bound, bound + 1);
        var y1 = random.Next(-bound, bound + 1);
        var dx = NonZero(random, -bound, bound + 1);
        var dy = NonZero(random, -bound, bound + 1);
        var x2 = x1 + dx;
        var y2 = y1 + dy;

        return Problem(
            "vectors.between_points.exact",
            $"A = ({x1}, {y1}) and B = ({x2}, {y2}). Find vector AB.",
            "For vector AB, subtract A from B componentwise: <x₂ − x₁, y₂ − y₁>.",
            AssessmentItemType.ShortAnswer,
            ("x1", x1),
            ("y1", y1),
            ("x2", x2),
            ("y2", y2));
    }

    private static int NonZero(Random random, int minInclusive, int maxExclusive)
    {
        var value = 0;
        while (value == 0)
            value = random.Next(minInclusive, maxExclusive);
        return value;
    }

    private static ExactProblem BuildLinearEquation(Random random, int scale)
    {
        var factory = new ExactLinearEquationQuestionFactory(
            new ExactLinearEquationSolver(),
            new ExactLinearEquationVerifier());
        var generated = factory.Generate(
            random.Next(1, int.MaxValue),
            Math.Clamp(scale, 1, 3));

        if (generated.Problem is not EquationNode)
        {
            throw new InvalidOperationException(
                "Exact linear-equation factory returned an unsupported learner-facing problem shape.");
        }

        var coefficient = int.Parse(
            generated.Parameters["coefficient"],
            NumberStyles.Integer,
            CultureInfo.InvariantCulture);
        var offset = int.Parse(
            generated.Parameters["offset"],
            NumberStyles.Integer,
            CultureInfo.InvariantCulture);
        var right = int.Parse(
            generated.Parameters["right"],
            NumberStyles.Integer,
            CultureInfo.InvariantCulture);

        return Problem(
            ExactLinearEquationQuestionFactory.FamilyId,
            $"Solve {FormatLinearExpression(coefficient, offset)} = {right.ToString(CultureInfo.InvariantCulture)}.",
            "Collect constants, divide by the non-zero coefficient, then verify the exact solution in the original equation.",
            AssessmentItemType.Numeric,
            ("coefficient", coefficient),
            ("offset", offset),
            ("right", right));
    }

    private static ExactProblem BuildLinearEquationBothSides(Random random, int scale)
    {
        var solution = NonZero(random, -6 - scale * 4, 7 + scale * 4);
        var leftCoefficient = NonZero(random, -3 - scale * 2, 4 + scale * 2);
        var rightCoefficient = NonZero(random, -3 - scale * 2, 4 + scale * 2);
        while (rightCoefficient == leftCoefficient)
            rightCoefficient = NonZero(random, -3 - scale * 2, 4 + scale * 2);

        var leftOffset = random.Next(-10 * scale, 10 * scale + 1);
        var rightOffset = checked((leftCoefficient - rightCoefficient) * solution + leftOffset);

        return Problem(
            "algebra.linear.variables_both_sides",
            $"Solve {FormatLinearExpression(leftCoefficient, leftOffset)} = {FormatLinearExpression(rightCoefficient, rightOffset)}.",
            "Collect x-terms on one side and constants on the other, divide by the non-zero resulting coefficient, then substitute into both original sides.",
            AssessmentItemType.Numeric,
            ("leftCoefficient", leftCoefficient),
            ("leftOffset", leftOffset),
            ("rightCoefficient", rightCoefficient),
            ("rightOffset", rightOffset));
    }

    private static ExactProblem BuildLinearInequalityBothSides(Random random, int scale)
    {
        var boundary = random.Next(-6 - scale * 4, 7 + scale * 4);
        var leftCoefficient = NonZero(random, -3 - scale * 2, 4 + scale * 2);
        var rightCoefficient = NonZero(random, -3 - scale * 2, 4 + scale * 2);
        while (rightCoefficient == leftCoefficient)
            rightCoefficient = NonZero(random, -3 - scale * 2, 4 + scale * 2);

        var leftOffset = random.Next(-10 * scale, 10 * scale + 1);
        var rightOffset = checked((leftCoefficient - rightCoefficient) * boundary + leftOffset);
        var relation = (InequalityRelation)random.Next(
            (int)InequalityRelation.LessThan,
            (int)InequalityRelation.GreaterThanOrEqual + 1);

        return Problem(
            "algebra.linear.inequality.variables_both_sides",
            $"Solve {FormatLinearExpression(leftCoefficient, leftOffset)} {RelationSymbol(relation)} {FormatLinearExpression(rightCoefficient, rightOffset)}. Give your answer as an inequality in x.",
            "Collect x-terms on one side. If the resulting coefficient is negative, reverse the inequality when dividing. Verify the boundary in the original inequality.",
            AssessmentItemType.ShortAnswer,
            ("leftCoefficient", leftCoefficient),
            ("leftOffset", leftOffset),
            ("rightCoefficient", rightCoefficient),
            ("rightOffset", rightOffset),
            ("relation", (int)relation));
    }

    private static ExactProblem BuildLinearInequality(Random random, int scale)
    {
        var factory = new ExactLinearInequalityQuestionFactory(
            new ExactLinearInequalitySolver(),
            new ExactLinearInequalityVerifier());
        var generated = factory.Generate(
            random.Next(1, int.MaxValue),
            Math.Clamp(scale, 1, 3));

        if (generated.Problem is not InequalityNode original ||
            generated.ExpectedAnswer is not InequalityNode solved ||
            solved.Left is not SymbolNode symbol ||
            !string.Equals(symbol.Name, "x", StringComparison.Ordinal) ||
            solved.Right is not IntegerNode integerBoundary ||
            integerBoundary.Value < int.MinValue ||
            integerBoundary.Value > int.MaxValue)
        {
            throw new InvalidOperationException(
                "Exact linear-inequality factory returned an unsupported learner-facing shape.");
        }

        var coefficient = int.Parse(
            generated.Parameters["coefficient"],
            NumberStyles.Integer,
            CultureInfo.InvariantCulture);
        var offset = int.Parse(
            generated.Parameters["offset"],
            NumberStyles.Integer,
            CultureInfo.InvariantCulture);
        var right = int.Parse(
            generated.Parameters["right"],
            NumberStyles.Integer,
            CultureInfo.InvariantCulture);
        var boundary = (int)integerBoundary.Value;

        var prompt =
            $"Solve {FormatLinearExpression(coefficient, offset)} {RelationSymbol(original.Relation)} {right.ToString(CultureInfo.InvariantCulture)}. " +
            "Give your answer as an inequality in x.";

        var solution = coefficient < 0
            ? $"Collect terms, then divide by {coefficient.ToString(CultureInfo.InvariantCulture)}. Because the coefficient is negative, reverse the inequality sign. The exact boundary is {boundary.ToString(CultureInfo.InvariantCulture)}."
            : $"Collect terms, then divide by {coefficient.ToString(CultureInfo.InvariantCulture)}. The inequality sign is preserved and the exact boundary is {boundary.ToString(CultureInfo.InvariantCulture)}.";

        return Problem(
            ExactLinearInequalityQuestionFactory.FamilyId,
            prompt,
            solution,
            AssessmentItemType.ShortAnswer,
            ("coefficient", coefficient),
            ("offset", offset),
            ("right", right),
            ("relation", (int)original.Relation));
    }

    private static string Solve(ExactProblem problem)
    {
        var p = problem.Parameters;
        if (UaeGrade2Unit1Lesson01Questions.Supports(problem.Family))
            return UaeGrade2Unit1Lesson01Questions.Solve(problem.Family, p);
        if (UaeGrade2Unit1Lessons02And13Questions.Supports(problem.Family))
            return UaeGrade2Unit1Lessons02And13Questions.Solve(problem.Family, p);
        if (UaeGrade2Unit2Questions.Supports(problem.Family))
            return UaeGrade2Unit2Questions.Solve(problem.Family, p);
        if (UaeGrade2Unit3Questions.Supports(problem.Family))
            return UaeGrade2Unit3Questions.Solve(problem.Family, p);
        if (UaeGrade2Unit4Questions.Supports(problem.Family))
            return UaeGrade2Unit4Questions.Solve(problem.Family, p);
        if (UaeGrade2Unit5Questions.Supports(problem.Family))
            return UaeGrade2Unit5Questions.Solve(problem.Family, p);
        if (UaeGrade3Unit1Questions.Supports(problem.Family))
            return UaeGrade3Unit1Questions.Solve(problem.Family, p);
        if (UaeGrade3Unit1Lesson02Questions.Supports(problem.Family))
            return UaeGrade3Unit1Lesson02Questions.Solve(problem.Family, p);
        if (UaeGrade3Unit1Lesson03Questions.Supports(problem.Family))
            return UaeGrade3Unit1Lesson03Questions.Solve(problem.Family, p);
        if (UaeGrade3Unit1Lesson04Questions.Supports(problem.Family))
            return UaeGrade3Unit1Lesson04Questions.Solve(problem.Family, p);
        if (UaeGrade3Unit1Lesson05Questions.Supports(problem.Family))
            return UaeGrade3Unit1Lesson05Questions.Solve(problem.Family, p);
        if (UaeGrade3Unit2Lesson01Questions.Supports(problem.Family))
            return UaeGrade3Unit2Lesson01Questions.Solve(problem.Family, p);
        if (UaeGrade3Unit2Lesson02Questions.Supports(problem.Family))
            return UaeGrade3Unit2Lesson02Questions.Solve(problem.Family, p);
        if (UaeGrade3Unit2Lesson03Questions.Supports(problem.Family))
            return UaeGrade3Unit2Lesson03Questions.Solve(problem.Family, p);
        if (UaeGrade2Unit6Questions.Supports(problem.Family))
            return UaeGrade2Unit6Questions.Solve(problem.Family, p);
        if (UaeGrade2Unit7Questions.Supports(problem.Family))
            return UaeGrade2Unit7Questions.Solve(problem.Family, p);
        if (UaeGrade2Unit8Questions.Supports(problem.Family))
            return UaeGrade2Unit8Questions.Solve(problem.Family, p);
        if (UaeGrade2Term3Questions.Supports(problem.Family))
            return UaeGrade2Term3Questions.Solve(problem.Family, p);
        if (Grade12PrecalculusAdvancedNumericQuestions.Supports(problem.Family))
            return Grade12PrecalculusAdvancedNumericQuestions.Solve(
                problem.Family, p);
        if (UsCcssG7Unit1Lesson1ScaledCopyQuestions.Supports(problem.Family))
            return UsCcssG7Unit1Lesson1ScaledCopyQuestions.Solve(problem.Family, p);
        if (UsCcssG7Unit1Lesson2CorrespondenceQuestions.Supports(problem.Family))
            return UsCcssG7Unit1Lesson2CorrespondenceQuestions.Solve(problem.Family, p);
        if (UsCcssG7Unit1Lesson3DrawingQuestions.Supports(problem.Family))
            return UsCcssG7Unit1Lesson3DrawingQuestions.Solve(problem.Family, p);
        if (UsCcssG7Unit1Lesson4ScaledRelationshipsQuestions.Supports(problem.Family))
            return UsCcssG7Unit1Lesson4ScaledRelationshipsQuestions.Solve(problem.Family, p);
        if (UsCcssG7Unit1Lesson5FactorQuestions.Supports(problem.Family))
            return UsCcssG7Unit1Lesson5FactorQuestions.Solve(problem.Family, p);
        if (UsCcssG7Unit1Lesson6AreaScalingQuestions.Supports(problem.Family))
            return UsCcssG7Unit1Lesson6AreaScalingQuestions.Solve(problem.Family, p);
        if (UsCcssG7Unit1Lesson7DrawingScaleQuestions.Supports(problem.Family))
            return UsCcssG7Unit1Lesson7DrawingScaleQuestions.Solve(problem.Family, p);
        if (UsCcssG7Unit1Lesson8MapRateQuestions.Supports(problem.Family))
            return UsCcssG7Unit1Lesson8MapRateQuestions.Solve(problem.Family, p);
        if (UsCcssG7Unit1Lesson9CreatingDrawingsQuestions.Supports(problem.Family))
            return UsCcssG7Unit1Lesson9CreatingDrawingsQuestions.Solve(problem.Family, p);
        if (UsCcssG7Unit1Lesson10ChangingScalesQuestions.Supports(problem.Family))
            return UsCcssG7Unit1Lesson10ChangingScalesQuestions.Solve(problem.Family, p);
        if (UsCcssG7Unit1Lesson11UnitlessScalesQuestions.Supports(problem.Family))
            return UsCcssG7Unit1Lesson11UnitlessScalesQuestions.Solve(problem.Family, p);
        if (UsCcssG7Unit2Lesson1EquivalentRatioQuestions.Supports(problem.Family))
            return UsCcssG7Unit2Lesson1EquivalentRatioQuestions.Solve(problem.Family, p);
        if (UsCcssG7Unit2Lesson3UnitRatesQuestions.Supports(problem.Family))
            return UsCcssG7Unit2Lesson3UnitRatesQuestions.Solve(problem.Family, p);
        if (UsCcssG7Unit2Lesson4ProportionalEquationsQuestions.Supports(problem.Family))
            return UsCcssG7Unit2Lesson4ProportionalEquationsQuestions.Solve(problem.Family, p);
        if (UsCcssG7Unit2Lesson6ConcertRecyclingQuestions.Supports(problem.Family))
            return UsCcssG7Unit2Lesson6ConcertRecyclingQuestions.Solve(problem.Family, p);
        if (UsCcssG7Unit2Lesson7TableProportionalityQuestions.Supports(problem.Family))
            return UsCcssG7Unit2Lesson7TableProportionalityQuestions.Solve(problem.Family, p);
        if (UsCcssG7Unit2Lesson8ClassifyEquationsQuestions.Supports(problem.Family))
            return UsCcssG7Unit2Lesson8ClassifyEquationsQuestions.Solve(problem.Family, p);
        if (UsCcssG7Unit2Lesson9RateComparisonQuestions.Supports(problem.Family))
            return UsCcssG7Unit2Lesson9RateComparisonQuestions.Solve(problem.Family, p);
        if (UsCcssG7Unit2Lesson10GraphProportionsQuestions.Supports(problem.Family))
            return UsCcssG7Unit2Lesson10GraphProportionsQuestions.Solve(problem.Family, p);
        if (UsCcssG6Unit1Lesson1TilingQuestions.Supports(problem.Family))
            return UsCcssG6Unit1Lesson1TilingQuestions.Solve(problem.Family, p);
        if (p.ContainsKey("g6ScaledRatio"))
            return Grade6ScaledRatioValue(problem.Family, p);
        if (IsGrade6FiniteSourceVariantFamily(problem.Family) &&
            p.ContainsKey("g6Composite"))
        {
            var value = Grade6CompositeValue(problem.Family, p);
            return value.Denominator == BigInteger.One
                ? value.Numerator.ToString(CultureInfo.InvariantCulture)
                : value.Numerator.ToString(CultureInfo.InvariantCulture) + "/" +
                  value.Denominator.ToString(CultureInfo.InvariantCulture);
        }

        if (UsCcssG6Unit1Lesson1TilingQuestions.Supports(problem.Family))
            return UsCcssG6Unit1Lesson1TilingQuestions.Solve(problem.Family, p);
        if (UsCcssG6Unit1Lesson2TangramQuestions.Supports(problem.Family))
            return UsCcssG6Unit1Lesson2TangramQuestions.Solve(problem.Family, p);
        if (UsCcssG6Unit1Lesson3ShadedAreaQuestions.Supports(problem.Family))
            return UsCcssG6Unit1Lesson3ShadedAreaQuestions.Solve(problem.Family, p);
        if (UsCcssG6Unit1Lesson4ParallelogramQuestions.Supports(problem.Family))
            return UsCcssG6Unit1Lesson4ParallelogramQuestions.Solve(problem.Family, p);
        if (UsCcssG6Unit1Lesson5BaseHeightQuestions.Supports(problem.Family))
            return UsCcssG6Unit1Lesson5BaseHeightQuestions.Solve(problem.Family, p);
        if (UsCcssG6Unit1Lesson6AreaQuestions.Supports(problem.Family))
            return UsCcssG6Unit1Lesson6AreaQuestions.Solve(problem.Family, p);
        if (UsCcssG6Unit1Lesson7TriangleQuestions.Supports(problem.Family))
            return UsCcssG6Unit1Lesson7TriangleQuestions.Solve(problem.Family, p);
        if (UsCcssG6Unit1Lesson8TriangleAreaQuestions.Supports(problem.Family))
            return UsCcssG6Unit1Lesson8TriangleAreaQuestions.Solve(problem.Family, p);
        if (UsCcssG6Unit1Lesson11PolygonQuestions.Supports(problem.Family))
            return UsCcssG6Unit1Lesson11PolygonQuestions.Solve(problem.Family, p);
        if (UsCcssG6Unit2RatioQuestions.Supports(problem.Family))
            return UsCcssG6Unit2RatioQuestions.Solve(problem.Family,p);
        if (UsCcssG6Unit1Lesson12SurfaceAreaQuestions.Supports(problem.Family))
            return UsCcssG6Unit1Lesson12SurfaceAreaQuestions.Solve(problem.Family, p);

        if (UsCcssAlg1U01L01StatisticalSelectionQuestions.Supports(problem.Family))
            return UsCcssAlg1U01L01StatisticalSelectionQuestions.Solve(problem.Family, p);
        if (UsCcssAlg1U01L02RepresentationQuestions.Supports(problem.Family))
            return UsCcssAlg1U01L02RepresentationQuestions.Solve(problem.Family, p);
        if (UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.Supports(problem.Family))
            return UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.Solve(problem.Family, p);
        if (UsCcssAlg1Unit2AlgebraQuestions.Supports(problem.Family))
            return UsCcssAlg1Unit2AlgebraQuestions.Solve(problem.Family, p);
        if (UsCcssAlg1Unit2InequalityQuestions.Supports(problem.Family))
            return UsCcssAlg1Unit2InequalityQuestions.Solve(problem.Family, p);
        if (UsCcssAlg1Unit3StatisticsQuestions.Supports(problem.Family))
            return UsCcssAlg1Unit3StatisticsQuestions.Solve(problem.Family, p);
        if (UsCcssAlg1Unit4FunctionQuestions.Supports(problem.Family))
            return UsCcssAlg1Unit4FunctionQuestions.Solve(problem.Family, p);
        if (UsCcssG5U03L01FractionOfHalfQuestions.Supports(problem.Family))
            return UsCcssG5U03L01FractionOfHalfQuestions.Solve(problem.Family, p);
        if (UsCcssG5U03L02UnitFractionQuestions.Supports(problem.Family))
            return UsCcssG5U03L02UnitFractionQuestions.Solve(problem.Family, p);
        if (UsCcssG5U03L03UnitFractionQuestions.Supports(problem.Family))
            return UsCcssG5U03L03UnitFractionQuestions.Solve(problem.Family, p);
        if (UsCcssG5U03L04ParkQuestions.Supports(problem.Family))
            return UsCcssG5U03L04ParkQuestions.Solve(problem.Family, p);
        if (UsCcssG5U03L05ShadedAreaQuestions.Supports(problem.Family))
            return UsCcssG5U03L05ShadedAreaQuestions.Solve(problem.Family, p);
        if (UsCcssG5U03L06NonunitAreaQuestions.Supports(problem.Family))
            return UsCcssG5U03L06NonunitAreaQuestions.Solve(problem.Family, p);
        if (UsCcssG5U03L07FractionEquationQuestions.Supports(problem.Family))
            return UsCcssG5U03L07FractionEquationQuestions.Solve(problem.Family, p);
        if (UsCcssG5U03L08FlagMeasureQuestions.Supports(problem.Family))
            return UsCcssG5U03L08FlagMeasureQuestions.Solve(problem.Family, p);
        if (UsCcssG5U03L09FlagDesignSupportQuestions.Supports(problem.Family))
            return UsCcssG5U03L09FlagDesignSupportQuestions.Solve(problem.Family, p);
        if (UsCcssG5U03L10SharingQuestions.Supports(problem.Family))
            return UsCcssG5U03L10SharingQuestions.Solve(problem.Family, p);
        if (UsCcssG5U03SourceDivisionQuestions.Supports(problem.Family))
            return UsCcssG5U03SourceDivisionQuestions.Solve(problem.Family, p);
        if (UsCcssG5U04WholeNumberSourceProducts.Supports(problem.Family))
            return UsCcssG5U04WholeNumberSourceProducts.Solve(problem.Family, p);
        if (UsCcssG5U04WholeNumberSourceDivision.Supports(problem.Family))
            return UsCcssG5U04WholeNumberSourceDivision.Solve(problem.Family, p);
        if (UsCcssG5U05SourceThousandths.Supports(problem.Family))
            return UsCcssG5U05SourceThousandths.Solve(problem.Family, p);
        if (UsCcssG7Unit1Lesson1ScaledCopyQuestions.Supports(problem.Family))
            return UsCcssG7Unit1Lesson1ScaledCopyQuestions.Solve(problem.Family, p);

        if (UsCcssG1FullGradeUnit7GeometryTimeQuestions.Supports(problem.Family))
            return UsCcssG1FullGradeUnit7GeometryTimeQuestions.Solve(problem.Family,p);
        if (UsCcssG1HowCloseStage3Questions.Supports(problem.Family))
            return UsCcssG1HowCloseStage3Questions.Solve(p);
        if (UsCcssG1FullGradeUnit6MeasurementQuestions.Supports(problem.Family))
            return UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(problem.Family,p);
        if (UsCcssG1FullGradeUnit8ReviewQuestions.Supports(problem.Family))
            return UsCcssG1FullGradeUnit8ReviewQuestions.Solve(problem.Family,p);
        if (UsCcssG1FullGradeUnit4FinalQuestions.Supports(problem.Family))
            return UsCcssG1FullGradeUnit4FinalQuestions.Solve(problem.Family,p);
        if (UsCcssG1FullGradeUnit5AdditionQuestions.Supports(problem.Family))
            return UsCcssG1FullGradeUnit5AdditionQuestions.Solve(problem.Family,p);
        if (UsCcssG1FullGradeTwoDigitOperationsQuestions.Supports(problem.Family))
            return UsCcssG1FullGradeTwoDigitOperationsQuestions.Solve(problem.Family,p);
        if (UsCcssG1FullGradeTensAndOnesQuestions.Supports(problem.Family))
            return UsCcssG1FullGradeTensAndOnesQuestions.Solve(problem.Family,p);
        if (UsCcssG1FullGradeStrategyAndTensQuestions.Supports(problem.Family))
            return UsCcssG1FullGradeStrategyAndTensQuestions.Solve(problem.Family,p);
        if (UsCcssG1FullGradeNextQuestions.Supports(problem.Family))
            return UsCcssG1FullGradeNextQuestions.Solve(problem.Family,p);
        if (UsCcssG1FullGradeStartQuestions.Supports(problem.Family))
            return UsCcssG1FullGradeStartQuestions.Solve(problem.Family,p);
        if (UsCcssG1TeenFiveQuestions.Supports(problem.Family))
            return UsCcssG1TeenFiveQuestions.Solve(problem.Family,p);
        if (UsCcssG1SourceFiveNextQuestions.Supports(problem.Family))
            return UsCcssG1SourceFiveNextQuestions.Solve(problem.Family,p);
        if (UsCcssG1NextSourceFiveQuestions.Supports(problem.Family))
            return UsCcssG1NextSourceFiveQuestions.Solve(problem.Family,p);
        if (UsCcssG1Unit2EquationStoryFiveQuestions.Supports(problem.Family))
            return UsCcssG1Unit2EquationStoryFiveQuestions.Solve(problem.Family,p);
        if (UsCcssG1Unit2CompareFiveQuestions.Supports(problem.Family))
            return UsCcssG1Unit2CompareFiveQuestions.Solve(problem.Family,p);
        if (UsCcssG1Unit2NextFiveQuestions.Supports(problem.Family))
            return UsCcssG1Unit2NextFiveQuestions.Solve(problem.Family,p);
        if (UsCcssG1Unit2Part2Questions.Supports(problem.Family))
            return UsCcssG1Unit2Part2Questions.Solve(problem.Family,p);
        if (UsCcssG4Unit2ThirdLessonQuestions.Supports(problem.Family))
            return UsCcssG4Unit2ThirdLessonQuestions.Solve(problem.Family,p);
        if (UsCcssG4Unit1SeventhLessonQuestions.Supports(problem.Family))
            return UsCcssG4Unit1SeventhLessonQuestions.Solve(problem.Family,p);
        if (UsCcssG4Unit1SixthLessonQuestions.Supports(problem.Family))
            return UsCcssG4Unit1SixthLessonQuestions.Solve(problem.Family,p);
        if (UsCcssG4Unit1FifthLessonQuestions.Supports(problem.Family))
            return UsCcssG4Unit1FifthLessonQuestions.Solve(problem.Family,p);
        if (UsCcssG4Unit1ThirdLessonQuestions.Supports(problem.Family))
            return UsCcssG4Unit1ThirdLessonQuestions.Solve(problem.Family,p);
        if (UsCcssG4Unit1SecondLessonQuestions.Supports(problem.Family))
            return UsCcssG4Unit1SecondLessonQuestions.Solve(problem.Family,p);
        if (UsCcssG4Unit1FirstLessonQuestions.Supports(problem.Family))
            return UsCcssG4Unit1FirstLessonQuestions.Solve(problem.Family, p);
        if (UsCcssG5Unit1Lesson2Questions.Supports(problem.Family))
            return UsCcssG5Unit1Lesson2Questions.Solve(problem.Family, p);
        if (UsCcssG5Unit1Lesson3Questions.Supports(problem.Family))
            return UsCcssG5Unit1Lesson3Questions.Solve(problem.Family,p);
        if (UsCcssG5Unit1Lesson4Questions.Supports(problem.Family))
            return UsCcssG5Unit1Lesson4Questions.Solve(problem.Family,p);
        if (UsCcssG5Unit1Lesson5Questions.Supports(problem.Family))
            return UsCcssG5Unit1Lesson5Questions.Solve(problem.Family,p);
        if (UsCcssG5Unit1Lesson6Questions.Supports(problem.Family))
            return UsCcssG5Unit1Lesson6Questions.Solve(problem.Family,p);
        if (UsCcssG5Unit1Lesson7Questions.Supports(problem.Family))
            return UsCcssG5Unit1Lesson7Questions.Solve(problem.Family,p);
        if(UsCcssG5Unit1Lesson8Questions.Supports(problem.Family))return UsCcssG5Unit1Lesson8Questions.Solve(problem.Family,p);
        if(UsCcssG5Unit1Lesson9Questions.Supports(problem.Family))return UsCcssG5Unit1Lesson9Questions.Solve(problem.Family,p);
        if(UsCcssG5Unit1Lesson10Questions.Supports(problem.Family))return UsCcssG5Unit1Lesson10Questions.Solve(problem.Family,p);
        if(UsCcssG5Unit1Lesson11Questions.Supports(problem.Family))return UsCcssG5Unit1Lesson11Questions.Solve(problem.Family,p);
        if(UsCcssG5Unit1Lesson12Questions.Supports(problem.Family))return UsCcssG5Unit1Lesson12Questions.Solve(problem.Family,p);
        if (UsCcssG5Unit2SourceQuestions.Supports(problem.Family))
            return UsCcssG5Unit2SourceQuestions.Solve(problem.Family,p);
        if (UsCcssG5Unit1Lesson1ComparisonQuestions.Supports(problem.Family))
            return UsCcssG5Unit1Lesson1ComparisonQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit1FirstLessonQuestions.Supports(problem.Family))
            return UsCcssG2Unit1FirstLessonQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit1Lesson2And3Questions.Supports(problem.Family))
            return UsCcssG2Unit1Lesson2And3Questions.Solve(problem.Family, p);
        if (UsCcssG2Unit1Lesson4Questions.Supports(problem.Family))
            return UsCcssG2Unit1Lesson4Questions.Solve(problem.Family, p);
        if (UsCcssG2Unit1Lesson5Questions.Supports(problem.Family))
            return UsCcssG2Unit1Lesson5Questions.Solve(problem.Family, p);
        if (UsCcssG2Unit1Lesson6NumberTalkQuestions.Supports(problem.Family))
            return UsCcssG2Unit1Lesson6NumberTalkQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit1Lesson8PictureGraphQuestions.Supports(problem.Family))
            return UsCcssG2Unit1Lesson8PictureGraphQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit1Lesson9BarGraphQuestions.Supports(problem.Family))
            return UsCcssG2Unit1Lesson9BarGraphQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit1Lesson10ConstructGraphsQuestions.Supports(problem.Family))
            return UsCcssG2Unit1Lesson10ConstructGraphsQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit1Lesson11GraphQuestions.Supports(problem.Family))
            return UsCcssG2Unit1Lesson11GraphQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit1Lesson12SortAndDisplayQuestions.Supports(problem.Family))
            return UsCcssG2Unit1Lesson12SortAndDisplayQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit1Lesson13BarCompareQuestions.Supports(problem.Family))
            return UsCcssG2Unit1Lesson13BarCompareQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit1Lesson14TapeDiagramQuestions.Supports(problem.Family))
            return UsCcssG2Unit1Lesson14TapeDiagramQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit1Lesson15CompareQuestions.Supports(problem.Family))
            return UsCcssG2Unit1Lesson15CompareQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit1Lesson16CompareQuestions.Supports(problem.Family))
            return UsCcssG2Unit1Lesson16CompareQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit1Lesson17CenterQuestions.Supports(problem.Family))
            return UsCcssG2Unit1Lesson17CenterQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit1Lesson18ClassSurveyQuestions.Supports(problem.Family))
            return UsCcssG2Unit1Lesson18ClassSurveyQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit2Lesson1NoRegroupQuestions.Supports(problem.Family))
            return UsCcssG2Unit2Lesson1NoRegroupQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit2Lesson2UnknownAddendQuestions.Supports(problem.Family))
            return UsCcssG2Unit2Lesson2UnknownAddendQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit2Lesson3ZooStoryQuestions.Supports(problem.Family))
            return UsCcssG2Unit2Lesson3ZooStoryQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit2Lesson4OptionalCenterQuestions.Supports(problem.Family))
            return UsCcssG2Unit2Lesson4OptionalCenterQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit2Lesson5DecomposeTenQuestions.Supports(problem.Family))
            return UsCcssG2Unit2Lesson5DecomposeTenQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit2Lesson6TargetNumbersQuestions.Supports(problem.Family))
            return UsCcssG2Unit2Lesson6TargetNumbersQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit2Lesson7SubtractTwoDigitsQuestions.Supports(problem.Family))
            return UsCcssG2Unit2Lesson7SubtractTwoDigitsQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit2Lesson8DifferentWaysQuestions.Supports(problem.Family))
            return UsCcssG2Unit2Lesson8DifferentWaysQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit2Lesson9MixedAddSubtractQuestions.Supports(problem.Family))
            return UsCcssG2Unit2Lesson9MixedAddSubtractQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit2Lesson10TargetStage5Questions.Supports(problem.Family))
            return UsCcssG2Unit2Lesson10TargetStage5Questions.Solve(problem.Family, p);
        if (UsCcssG2Unit2Lesson11SeedsStoryQuestions.Supports(problem.Family))
            return UsCcssG2Unit2Lesson11SeedsStoryQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit2Lesson12MancalaQuestions.Supports(problem.Family))
            return UsCcssG2Unit2Lesson12MancalaQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit2Lesson13StoryEquationsQuestions.Supports(problem.Family))
            return UsCcssG2Unit2Lesson13StoryEquationsQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit2Lesson14TwoStepSeedsQuestions.Supports(problem.Family))
            return UsCcssG2Unit2Lesson14TwoStepSeedsQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit2Lesson15TapeStoriesQuestions.Supports(problem.Family))
            return UsCcssG2Unit2Lesson15TapeStoriesQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit2Lesson16MarketInventoryQuestions.Supports(problem.Family))
            return UsCcssG2Unit2Lesson16MarketInventoryQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit3Lesson1StandardUnitsQuestions.Supports(problem.Family))
            return UsCcssG2Unit3Lesson1StandardUnitsQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit3Lesson2MeasureCmQuestions.Supports(problem.Family))
            return UsCcssG2Unit3Lesson2MeasureCmQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit3Lesson3BuildRulerQuestions.Supports(problem.Family))
            return UsCcssG2Unit3Lesson3BuildRulerQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit3Lesson4EstimateMeasureCmQuestions.Supports(problem.Family))
            return UsCcssG2Unit3Lesson4EstimateMeasureCmQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit3Lesson5MetersQuestions.Supports(problem.Family))
            return UsCcssG2Unit3Lesson5MetersQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit3Lesson6ReptileStoriesQuestions.Supports(problem.Family))
            return UsCcssG2Unit3Lesson6ReptileStoriesQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit3Lesson7EstimateMeasureStage2Questions.Supports(problem.Family))
            return UsCcssG2Unit3Lesson7EstimateMeasureStage2Questions.Solve(problem.Family, p);
        if (UsCcssG2Unit3Lesson8WhatIsAnInchQuestions.Supports(problem.Family))
            return UsCcssG2Unit3Lesson8WhatIsAnInchQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit3Lesson9FishFeetQuestions.Supports(problem.Family))
            return UsCcssG2Unit3Lesson9FishFeetQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit3Lesson10TornTapeQuestions.Supports(problem.Family))
            return UsCcssG2Unit3Lesson10TornTapeQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit3Lesson11SareeRibbonQuestions.Supports(problem.Family))
            return UsCcssG2Unit3Lesson11SareeRibbonQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit3Lesson12FriendshipQuestions.Supports(problem.Family))
            return UsCcssG2Unit3Lesson12FriendshipQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit3Lesson13CenterDay2Questions.Supports(problem.Family))
            return UsCcssG2Unit3Lesson13CenterDay2Questions.Solve(problem.Family, p);
        if (UsCcssG2Unit3Lesson14HandSpanLinePlotQuestions.Supports(problem.Family))
            return UsCcssG2Unit3Lesson14HandSpanLinePlotQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit3Lesson15PencilLinePlotQuestions.Supports(problem.Family))
            return UsCcssG2Unit3Lesson15PencilLinePlotQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit3Lesson16PlantHeightsQuestions.Supports(problem.Family))
            return UsCcssG2Unit3Lesson16PlantHeightsQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit3Lesson17CenterPlotQuestions.Supports(problem.Family))
            return UsCcssG2Unit3Lesson17CenterPlotQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit3Lesson18YardstickQuestions.Supports(problem.Family))
            return UsCcssG2Unit3Lesson18YardstickQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit4Lesson01NumberLineQuestions.Supports(problem.Family))
            return UsCcssG2Unit4Lesson01NumberLineQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit4Lesson02NumberLineFeaturesQuestions.Supports(problem.Family))
            return UsCcssG2Unit4Lesson02NumberLineFeaturesQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit4Lesson03UnlabeledTicksQuestions.Supports(problem.Family))
            return UsCcssG2Unit4Lesson03UnlabeledTicksQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit4NextThreeQuestions.Supports(problem.Family))
            return UsCcssG2Unit4NextThreeQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit4ArrowLessonsQuestions.Supports(problem.Family))
            return UsCcssG2Unit4ArrowLessonsQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit4FinalSixQuestions.Supports(problem.Family))
            return UsCcssG2Unit4FinalSixQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit5FirstSixQuestions.Supports(problem.Family))
            return UsCcssG2Unit5FirstSixQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit5ComparisonQuestions.Supports(problem.Family))
            return UsCcssG2Unit5ComparisonQuestions.Solve(problem.Family, p);
        if (UsCcssG2Unit9ExactQuestions.Supports(problem.Family))
            return UsCcssG2Unit9ExactQuestions.Solve(problem.Family,p);
        if (UsCcssG2Unit8ExactQuestions.Supports(problem.Family))
            return UsCcssG2Unit8ExactQuestions.Solve(problem.Family,p);
        if (UsCcssG2Unit7LastTenQuestions.Supports(problem.Family))
            return UsCcssG2Unit7LastTenQuestions.Solve(problem.Family,p);
        if (UsCcssG2Unit7NextFourQuestions.Supports(problem.Family))
            return UsCcssG2Unit7NextFourQuestions.Solve(problem.Family,p);
        if (UsCcssG2Unit7FirstFourQuestions.Supports(problem.Family))
            return UsCcssG2Unit7FirstFourQuestions.Solve(problem.Family,p);
        if (UsCcssG2Unit6ExactQuestions.Supports(problem.Family))
            return UsCcssG2Unit6ExactQuestions.Solve(problem.Family, p);
        if (UsCcssB001SourceGoalQuestions.Supports(problem.Family))
            return UsCcssB001SourceGoalQuestions.Solve(problem.Family,p);
        if (UsCcssG1Unit2StoryQuestions.Supports(problem.Family))
            return UsCcssG1Unit2StoryQuestions.Solve(problem.Family,p);
        if (UsCcssG1Batch03DataQuestions.Supports(problem.Family))
            return UsCcssG1Batch03DataQuestions.Solve(problem.Family,p);
        if (UsCcssT001AuthoredActivities.Supports(problem.Family))
            return UsCcssT001AuthoredActivities.Solve(problem.Family,p);
        if (UsCcssT001ConstructShapeGraph.Supports(problem.Family))
            return UsCcssT001ConstructShapeGraph.Solve(p);
        if (UsCcssT001RepresentationChoices.Supports(problem.Family))
            return UsCcssT001RepresentationChoices.Solve(problem.Family, p);
        if (UsCcssT001ExactQuestions.Supports(problem.Family))
            return UsCcssT001ExactQuestions.Solve(problem.Family, p);
        if (SupportingPracticeCompletionEngine.Supports(problem.Family))
            return SupportingPracticeCompletionEngine.Solve(problem.Family, p);

        return problem.Family switch
        {
            var uaeFamily when uaeFamily.StartsWith("uae.g1.u01.l0", StringComparison.Ordinal) =>
                p["count"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u02.l01.addition_stories" or "uae.g1.u02.l02.model_addition" or "uae.g1.u02.l03.addition_number_sentences" or "uae.g1.u02.l04.add_zero" or "uae.g1.u02.l05.vertical_addition" or "uae.g1.u02.l06.write_story_equation" =>
                (p["left"] + p["right"]).ToString(CultureInfo.InvariantCulture),
            "uae.g1.u02.l07.make_four_five" or "uae.g1.u02.l08.make_six_seven" or "uae.g1.u02.l09.make_eight" or "uae.g1.u02.l10.make_nine" or "uae.g1.u02.l11.make_ten_frame" =>
                p["right"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u02.l13.true_false_addition" =>
                p["left"] + p["right"] == p["shown"] ? "صحيحة" : "خطأ",
            "uae.g1.u03.l13.related_add_subtract" =>
                (p["removeLeft"] == 1 ? p["right"] : p["left"]).ToString(CultureInfo.InvariantCulture),
            "uae.g1.u03.l14.true_false_subtraction" =>
                p["whole"] - p["removed"] == p["shown"] ? "صحيحة" : "خطأ",
            "uae.g1.u04.l01.count_on_1_2_3" =>
                (p["start"] + p["step"]).ToString(CultureInfo.InvariantCulture),
            "uae.g1.u04.l02.count_on_fils" =>
                (p["start"] + p["added"]).ToString(CultureInfo.InvariantCulture),
            "uae.g1.u04.l03.add_on_number_line" =>
                (p["start"] + p["hops"]).ToString(CultureInfo.InvariantCulture),
            "uae.g1.u04.l04.doubles_add" =>
                (p["equalAddend"] * 2).ToString(CultureInfo.InvariantCulture),
            "uae.g1.u04.l05.near_doubles_add" =>
                (p["base"] * 2 + p["delta"]).ToString(CultureInfo.InvariantCulture),
            "uae.g1.u04.l06.act_out_problem_solving" =>
                (p["mode"] == 2 ? p["b"] : p["total"]).ToString(CultureInfo.InvariantCulture),
            "uae.g1.u04.l07.make_ten_add" =>
                (p["a"] + p["b"]).ToString(CultureInfo.InvariantCulture),
            "uae.g1.u04.l08.commutative_add" =>
                (p["mode"] == 0 ? p["a"] : p["total"]).ToString(CultureInfo.InvariantCulture),
            "uae.g1.u04.l09.add_three_with_strategy" =>
                (p["a"] + p["b"] + p["c"]).ToString(CultureInfo.InvariantCulture),
            "uae.g1.u03.l01.subtraction_stories" or "uae.g1.u03.l02.model_subtraction" or "uae.g1.u03.l03.subtraction_number_sentences" or "uae.g1.u03.l04.subtract_zero_or_all" or "uae.g1.u03.l05.vertical_subtraction" or "uae.g1.u03.l07.compare_groups" or "uae.g1.u03.l08.subtract_4_5" or "uae.g1.u03.l09.subtract_6_7" or "uae.g1.u03.l10.subtract_8" or "uae.g1.u03.l11.subtract_9" or "uae.g1.u03.l12.subtract_10" =>
                (p["whole"] - p["removed"]).ToString(CultureInfo.InvariantCulture),
            "uae.g1.u05.l01.count_back_1_2_3" =>
                (p["start"] - p["steps"]).ToString(CultureInfo.InvariantCulture),
            "uae.g1.u05.l02.number_line_subtract" =>
                (p["start"] - p["hops"]).ToString(CultureInfo.InvariantCulture),
            "uae.g1.u05.l03.doubles_subtraction" =>
                (p["part"]).ToString(CultureInfo.InvariantCulture),
            "uae.g1.u05.l04.story_subtraction_equation" =>
                (p["whole"] - p["removed"]).ToString(CultureInfo.InvariantCulture),
            "uae.g1.u05.l05.subtract_through_ten" =>
                (p["whole"] - p["removed"]).ToString(CultureInfo.InvariantCulture),
            "uae.g1.u05.l06.related_add_sub_facts" =>
                (p["b"]).ToString(CultureInfo.InvariantCulture),
            "uae.g1.u05.l07.fact_family_four_equations" =>
                (p["mode"] < 2 ? p["whole"] : p["mode"] == 2 ? p["b"] : p["a"]).ToString(CultureInfo.InvariantCulture),
            "uae.g1.u05.l08.missing_addend_part_whole" =>
                (p["whole"] - p["known"]).ToString(CultureInfo.InvariantCulture),
            "uae.g1.u06.l01.ordinal_1_10" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u06.l02.numbers_11_19" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u06.l03.groups_of_ten" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u06.l04.count_tens_fils" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u06.l05.ten_and_some_more" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u06.l06.tens_and_ones" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u06.l07.make_table" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u06.l08.numbers_to_100" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u06.l09.ten_more_ten_less" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u06.l10.count_fives_fils" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u06.l11.compare_models" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u06.l12.compare_symbols" =>
                (p["result"] == 1 ? ">" : p["result"] == -1 ? "<" : "="),
            "uae.g1.u06.l13.hundreds_tens_ones" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u06.l14.count_to_120" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u06.l15.read_write_to_120" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u07.l01.add_tens" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u07.l02.count_on_tens_ones" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u07.l03.add_tens_ones" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u07.l04.guess_check_revise" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u07.l05.regroup_ones_for_tens" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u07.l06.subtract_tens" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u07.l07.count_back_tens" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u07.l08.inverse_tens" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u08.l01.tally_fives" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u08.l02.record_table" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u08.l03.make_picture_graph" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u08.l04.read_picture_graph" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u08.l05.make_bar_graph" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u08.l06.read_bar_graph" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u09.l01.compare_lengths" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u09.l02.order_lengths" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u09.l03.nonstandard_measure" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u09.l04.guess_check_length" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u09.l05.analog_hour" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u09.l06.digital_hour" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u09.l07.analog_half" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u09.l08.digital_half" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u09.l09.hour_or_half" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u10.l01.square_rectangle" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u10.l02.triangle_trapezoid" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u10.l03.circle_properties" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u10.l04.compare_shapes" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u10.l05.compose_2d" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u10.l06.more_composite_2d" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u10.l07.logical_shape_clue" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u10.l08.equal_shares" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u10.l09.halves_equal" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u10.l10.fourths_equal" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u11.l01.cubes_prisms" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u11.l02.cones_cylinders" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u11.l03.three_d_pattern" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "uae.g1.u11.l04.compose_solids" =>
                p["result"].ToString(CultureInfo.InvariantCulture),
            "usccss.g1.u01.l01.count_collection_to20" =>
                p["count"].ToString(CultureInfo.InvariantCulture),
            "usccss.g1.u01.l01.add_dot_cubes_to10" =>
                (p["left"] + p["right"]).ToString(CultureInfo.InvariantCulture),
            "number.whole.add.direct" =>
                (p["left"] + p["right"]).ToString(CultureInfo.InvariantCulture),

            "number.whole.subtract.direct" =>
                (p["left"] - p["right"]).ToString(CultureInfo.InvariantCulture),

            "number.lcm.two_numbers" =>
                LeastCommonMultiple(p["left"], p["right"]).ToString(CultureInfo.InvariantCulture),

            "percentages.of_quantity.direct" or
            "percentages.core.of_quantity" =>
                (p["percent"] * p["quantity"] / 100).ToString(CultureInfo.InvariantCulture),

            "percentages.core.increase_decrease" =>
                (p["quantity"] * (100 + p["direction"] * p["percent"]) / 100)
                    .ToString(CultureInfo.InvariantCulture),

            "usccss.g11.alg2.u01.l01.affine_recurrence" =>
                G11AffineRecurrenceTerm(p["first"],p["multiplier"],p["offset"],p["n"])
                    .ToString(CultureInfo.InvariantCulture),
            "usccss.g11.alg2.u01.l01.hanoi_moves" =>
                G11HanoiMinimalMoves(p["n"]).ToString(CultureInfo.InvariantCulture),
            "usccss.g11.alg2.u01.l01.fibonacci_term" =>
                G11FibonacciTerm(p["n"]).ToString(CultureInfo.InvariantCulture),
            "usccss.g11.alg2.u01.l01.halving_fraction" =>
                SimplifyFraction(1, 1 << p["n"]),
            "usccss.g11.alg2.u01.l02.paper_piece_count" =>
                (1 << p["cutCount"]).ToString(CultureInfo.InvariantCulture),
            "usccss.g11.alg2.u01.l02.paper_piece_area" =>
                SimplifyFraction(80,1 << p["cutCount"]),
            "usccss.g11.alg2.u01.l02.rational_geometric_term" =>
                SimplifyFraction(checked(p["first"]*G11IntPow(p["ratioNumerator"],p["term"]-1)),
                    G11IntPow(p["ratioDenominator"],p["term"]-1)),
            "usccss.g11.alg2.u01.l02.growth_factor" =>
                SimplifyFraction(p["next"],p["previous"]),

            "sequences.core.arithmetic_nth_term" =>
                (p["first"] + (p["n"] - 1) * p["difference"]).ToString(CultureInfo.InvariantCulture),

            "sequences.core.geometric_nth_term" =>
                (p["first"] * IntegerPower(p["ratio"], p["n"] - 1)).ToString(CultureInfo.InvariantCulture),

            "sequences.core.find_position_arithmetic" =>
                p["expectedN"].ToString(CultureInfo.InvariantCulture),

            "fractions.represent.interpret.fraction_bar" =>
                SimplifyFraction(p["numerator"], p["denominator"]),

            "algebra.relationships.two_unknowns.total_difference" =>
                ((p["total"] - p["difference"]) / 2).ToString(CultureInfo.InvariantCulture),

            "measurement.scale.equal_intervals.read_value" =>
                (p["start"] + ((p["end"] - p["start"]) / p["intervals"]) * p["pointer"])
                    .ToString(CultureInfo.InvariantCulture),

            "fractions.compare.unlike.common_denominator" =>
                Compare(p["n1"] * p["d2"], p["n2"] * p["d1"]),

            "fractions.compare.unlike.select_greater" =>
                Compare(p["n1"] * p["d2"], p["n2"] * p["d1"]) switch
                {
                    ">" => "left",
                    "<" => "right",
                    _ => "equal"
                },

            "fractions.compare.unlike.true_false" =>
                string.Equals(
                    Compare(p["n1"] * p["d2"], p["n2"] * p["d1"]),
                    RelationFromCode(p["claimedRelation"]),
                    StringComparison.Ordinal)
                    ? "true"
                    : "false",

            "fractions.compare.unlike.order_three" =>
                OrderThreeFractions(p),

            "fractions.equivalent.missing_value" or
            "fractions.equivalent.recognize" or
            "fractions.equivalent.generate_multiple" or
            "fractions.equivalent.number_line" =>
                (p["baseN"] * p["targetD"] / p["baseD"]).ToString(CultureInfo.InvariantCulture),

            "fractions.equivalent.reduce_common_factor" =>
                (p["largeN"] * p["targetD"] / p["largeD"]).ToString(CultureInfo.InvariantCulture),

            "ratio.unit_rate.direct" =>
                (p["total"] / p["quantity"]).ToString(CultureInfo.InvariantCulture),

            "ratio.unit_rate.equivalent_ratio" =>
                (p["baseFirst"] * p["targetSecond"] / p["baseSecond"]).ToString(CultureInfo.InvariantCulture),

            "ratio.unit_rate.divide_total" =>
                (p["total"] / (p["firstPart"] + p["secondPart"]) * p["askedPart"])
                    .ToString(CultureInfo.InvariantCulture),

            "statistics.center_spread.mean" =>
                ((p["v1"] + p["v2"] + p["v3"] + p["v4"] + p["v5"]) / 5)
                    .ToString(CultureInfo.InvariantCulture),

            "statistics.center_spread.median" =>
                p["v3"].ToString(CultureInfo.InvariantCulture),

            "statistics.center_spread.range" =>
                (p["max"] - p["min"]).ToString(CultureInfo.InvariantCulture),

            "probability.theoretical.simple_event" =>
                SimplifyFraction(p["favourable"], p["total"]),

            "probability.theoretical.complement" =>
                SimplifyFraction(p["total"] - p["favourable"], p["total"]),

            "probability.theoretical.two_coins_exactly_one" =>
                "1/2",

            "number.whole.add_subtract.within_10.build" or
            "number.whole.add_subtract.within_10.apply" or
            "number.whole.add_subtract.across_ten.build" or
            "number.whole.add_subtract.across_ten.apply" or
            "number.whole.add_subtract.within_100.build" or
            "number.whole.add_subtract.within_100.apply" or
            "number.whole.add_subtract.columnar.build" or
            "number.whole.add_subtract.columnar.apply" =>
                (p["operation"] == 0 ? p["left"] + p["right"] : p["left"] - p["right"])
                    .ToString(CultureInfo.InvariantCulture),

            "number.whole.add_subtract.comparative.build" or
            "number.whole.add_subtract.comparative.apply" =>
                (p["base"] + p["difference"]).ToString(CultureInfo.InvariantCulture),

            "number.whole.add_subtract.complement_100.build" or
            "number.whole.add_subtract.complement_100.apply" =>
                (100 - p["known"]).ToString(CultureInfo.InvariantCulture),

            "number.whole.multiply.fact_recall.build" or
            "number.whole.multiply.fact_recall.apply" =>
                (p["left"] * p["right"]).ToString(CultureInfo.InvariantCulture),

            "number.whole.divide.with_remainder.build" or
            "number.whole.divide.with_remainder.apply" or
            "usccss.g4.u06.l19.partial_quotients_remainders" =>
                $"{(p["dividend"] / p["divisor"]).ToString(CultureInfo.InvariantCulture)} r {(p["dividend"] % p["divisor"]).ToString(CultureInfo.InvariantCulture)}",
            "usccss.g4.u06.l22.convert_l_shaped_area_perimeter" => (p["caseType"] switch { 0 => p["sheetWidthInches"] * p["sheetHeightInches"] - (p["bannerLengthFeet"] * 12 * p["bannerHeightInches"]), 1 => 2 * (p["leftWidthFeet"] + 3 * p["rightWidthYards"] + 3 * p["lowerHeightYards"] + p["upperRiseFeet"]), 2 => 24 * (p["leftWidthFeet"] + 3 * p["rightWidthYards"] + 3 * p["lowerHeightYards"] + p["upperRiseFeet"]), 3 => (p["leftWidthFeet"] + 3 * p["rightWidthYards"]) * (3 * p["lowerHeightYards"]) + p["leftWidthFeet"] * p["upperRiseFeet"], _ => p["suppliedInches"] - 24 * (p["leftWidthFeet"] + 3 * p["rightWidthYards"] + 3 * p["lowerHeightYards"] + p["upperRiseFeet"]) }).ToString(CultureInfo.InvariantCulture),
            "usccss.g4.u09.l05.two_digit_partial_products" => p["product"].ToString(CultureInfo.InvariantCulture),
            "usccss.g4.u08.l07.polygon_perimeter_sum" => p["perimeter"].ToString(CultureInfo.InvariantCulture),
            "usccss.g4.u08.l04.symmetry_axis_counts" => p["axisCount"].ToString(CultureInfo.InvariantCulture),
            "usccss.g4.u07.l13.additive_angle_part" => p["unknownDegrees"].ToString(CultureInfo.InvariantCulture),
            "usccss.g4.u07.l12.classify_angles_by_degree_bounds" => p["classCode"].ToString(CultureInfo.InvariantCulture),
            "usccss.g4.u07.l10.perpendicular_right_angle_judgment" => p["perpendicularAnswer"].ToString(CultureInfo.InvariantCulture),
            "usccss.g4.u07.l09.protractor_dual_scale_reading" => p["targetDegrees"].ToString(CultureInfo.InvariantCulture),
            "usccss.g4.u07.l08.full_turn_degree_benchmarks" => (p["questionKind"] == 0 ? p["numerator"] : p["questionKind"] == 1 ? p["denominator"] : 360 - p["degrees"]).ToString(CultureInfo.InvariantCulture),
            "usccss.g4.u07.l07.clock_hour_angle_turn_compare" => (p["questionKind"] == 0 ? (p["stepA"] > p["stepB"] ? 1 : p["stepA"] < p["stepB"] ? 2 : 3) : Math.Abs(p["stepA"] - p["stepB"])).ToString(CultureInfo.InvariantCulture),
            "usccss.g4.u07.l06.compare_angle_openings" => (p["steps1"] > p["steps2"] ? 1 : p["steps1"] < p["steps2"] ? 2 : 3).ToString(CultureInfo.InvariantCulture),
            "usccss.g4.u07.l05.angle_common_vertex_rays" => (p["start1"] == p["start2"] ? 1 : 2).ToString(CultureInfo.InvariantCulture),
            "usccss.g4.u07.l03.dot_grid_parallel_intersect" => (p["caseType"] < 4 ? 1 : 2).ToString(CultureInfo.InvariantCulture),
            "usccss.g4.u07.l02.geometry_cards_number_talk" => (p["caseType"] < 4
                ? p["caseType"] + 1
                : p["first"] - p["second"]).ToString(CultureInfo.InvariantCulture),
            "usccss.g4.u07.l01.geometric_description_precision" => (p["preciseFirst"] == 1 ? 1 : 2).ToString(CultureInfo.InvariantCulture),
            "usccss.g4.u06.l25.flower_garland_patterns" => (p["caseType"] switch { 0 => p["garlandsEach"] * (p["priyaBig"] + p["jadaBig"]), 1 => p["garlandsEach"] * (p["priyaSmall"] + p["jadaSmall"]), 2 => p["flowersMade"] / p["flowersPerGarland"], 3 => p["flowersMade"] % p["flowersPerGarland"], 4 => p["flowersMade"] * (p["cutMinutes"] + p["finishMinutes"]), _ => ((p["flowersMade"] + p["workers"] - 1) / p["workers"]) * (p["cutMinutes"] + p["finishMinutes"]) }).ToString(CultureInfo.InvariantCulture),
            "usccss.g4.u06.l24.language_population_comparisons" => (p["caseType"] switch { 0 => p["navajo"] - p["yupik"], 1 => p["navajo"] + p["apache"] + p["cherokee"], 2 => p["navajo"] - (p["yupik"] + p["dakota"] + p["apache"] + p["keres"] + p["cherokee"] + p["ojibwa"] + p["choctaw"] + p["zuni"] + p["pima"]), 3 => p["philadelphiaEnglish"] - p["philadelphiaSpanish"] - p["philadelphiaIndo"] - p["philadelphiaAsian"], 4 => p["chicagoSpanish"] + p["chicagoIndo"] - p["philadelphiaSpanish"] - p["philadelphiaIndo"], _ => (2 * p["navajo"] + p["yupik"]) / (2 * p["yupik"]) }).ToString(CultureInfo.InvariantCulture),
            "usccss.g4.u06.l23.four_operation_contexts" => (p["caseType"] switch { 0 => 2 * (p["segmentA"] + p["segmentB"] + p["segmentC"] + p["segmentD"]), 1 => p["musicTotalFeet"] / (2 * p["musicTrips"]), 2 => 2 * p["feetPerMile"] - (p["longTrips"] * 2 * (p["segmentA"] + p["segmentB"] + p["segmentC"] + p["segmentD"]) + p["musicTotalFeet"]), 3 => p["week1"] / p["days"], 4 => p["week1"] - p["week2"], _ => p["stepTarget"] - p["week1"] - p["week2"] - p["week3"] }).ToString(CultureInfo.InvariantCulture),
            "usccss.g4.u06.l21.multistep_contexts" => (p["caseType"] switch { 0 => p["budget"] - p["dailyRevenue"], 1 => p["count"] * p["days"], _ => p["count"] * p["days"] * (p["drinkPrice"] + p["popcornPrice"]) }).ToString(CultureInfo.InvariantCulture),
            "usccss.g4.u06.l20.contextual_remainder_decisions" => (p["caseType"] switch { 0 => p["dividend"] / p["divisor"] + 1, 1 => p["dividend"] / p["divisor"], 2 => p["dividend"] % p["divisor"], 3 => (p["dividend"] - p["shortfall"]) / p["divisor"], _ => p["dividend"] / p["divisor"] }).ToString(CultureInfo.InvariantCulture),

            "fractions.of_quantity.build" or
            "fractions.of_quantity.apply" =>
                ((p["quantity"] / p["denominator"]) * p["numerator"])
                    .ToString(CultureInfo.InvariantCulture),

            "fractions.add_subtract.within_one.build" or
            "fractions.add_subtract.within_one.apply" or
            "fractions.add_subtract.same_denominator.mixed.build" or
            "fractions.add_subtract.same_denominator.mixed.apply" or
            "fractions.add_subtract.related.build" or
            "fractions.add_subtract.related.apply" or
            "fractions.add_subtract.common_denominator.build" or
            "fractions.add_subtract.common_denominator.apply" =>
                SolveFractionAddSubtract(p),

            "fractions.compare.benchmark" =>
                Compare(p["n"] * p["benchmarkD"], p["benchmarkN"] * p["d"]),

            "geometry.coordinate.gradient_between_points" =>
                ((p["y2"] - p["y1"]) / (p["x2"] - p["x1"]))
                    .ToString(CultureInfo.InvariantCulture),

            "geometry.coordinate.evaluate_linear_rule" =>
                (p["gradient"] * p["x"] + p["intercept"]).ToString(CultureInfo.InvariantCulture),

            "geometry.coordinate.y_intercept_from_rule" =>
                p["intercept"].ToString(CultureInfo.InvariantCulture),

            "geometry.angles.parallel_lines" =>
                p["expectedAngle"].ToString(CultureInfo.InvariantCulture),

            "geometry.angles.supplementary" =>
                (180 - p["knownAngle"]).ToString(CultureInfo.InvariantCulture),

            "geometry.angles.algebraic_supplementary" =>
                p["expectedX"].ToString(CultureInfo.InvariantCulture),

            "geometry.similarity.find_missing_length" =>
                (p["sourceLength"] * p["scaleFactor"]).ToString(CultureInfo.InvariantCulture),

            "geometry.similarity.scale_factor" =>
                (p["targetLength"] / p["sourceLength"]).ToString(CultureInfo.InvariantCulture),

            "geometry.congruence.identify_criterion" =>
                p["criterion"] switch
                {
                    0 => "SSS",
                    1 => "SAS",
                    2 => "ASA",
                    3 => "RHS",
                    _ => throw new InvalidOperationException("Invalid congruence criterion.")
                },

            "geometry.surface_area.rectangular_prism" or
            "geometry.surface_area_volume.rectangular_prism_surface_area" =>
                (2 * (
                    p["length"] * p["width"] +
                    p["length"] * p["height"] +
                    p["width"] * p["height"]))
                    .ToString(CultureInfo.InvariantCulture),

            "geometry.volume.rectangular_prism" or
            "geometry.surface_area_volume.rectangular_prism_volume" =>
                (p["length"] * p["width"] * p["height"])
                    .ToString(CultureInfo.InvariantCulture),

            "geometry.rectangle.area.exact" or
            "geometry.perimeter_area.rectangle_area" =>
                (p["length"] * p["width"]).ToString(CultureInfo.InvariantCulture),

            "geometry.rectangle.perimeter.exact" or
            "geometry.perimeter_area.rectangle_perimeter" =>
                (2 * (p["length"] + p["width"])).ToString(CultureInfo.InvariantCulture),

            "geometry.perimeter_area.triangle_area" =>
                (p["base"] * p["height"] / 2).ToString(CultureInfo.InvariantCulture),

            "geometry.perimeter_area.rectangle_missing_side" or
            "usccss.g4.u06.l15.area_partial_quotients" or
            "usccss.g4.u06.l15.mural_complete_columns" =>
                (p["area"] / p["knownSide"]).ToString(CultureInfo.InvariantCulture),
            "usccss.g4.u06.l16.base_ten_equal_groups" => (p["dividend"]/p["divisor"]).ToString(CultureInfo.InvariantCulture),
            "usccss.g4.u06.l18.partial_quotients_recording" => (p["tensQuotient"] + p["onesQuotient"]).ToString(CultureInfo.InvariantCulture),

            "geometry.polygons.interior_angle_sum" =>
                ((p["sides"] - 2) * 180).ToString(CultureInfo.InvariantCulture),

            "geometry.polygons.missing_interior_angle" =>
                (((p["sides"] - 2) * 180) - p["knownSum"]).ToString(CultureInfo.InvariantCulture),

            "geometry.polygons.regular_interior_angle" =>
                (180 - 360 / p["sides"]).ToString(CultureInfo.InvariantCulture),

            "geometry.right_triangle.pythagorean.exact" =>
                p["hypotenuse"].ToString(CultureInfo.InvariantCulture),

            "geometry.right_triangle.pythagorean.find_leg_exact" =>
                p["expectedLeg"].ToString(CultureInfo.InvariantCulture),

            "trigonometry.right_triangle.ratio_exact" =>
                SimplifyFraction(p["ratioNumerator"], p["ratioDenominator"]),

            "trigonometry.right_triangle.find_side_exact" or
            "trigonometry.modelling.contextual" =>
                p["expectedSide"].ToString(CultureInfo.InvariantCulture),

            "trigonometry.right_triangle.find_angle_exact" =>
                p["expectedAngle"].ToString(CultureInfo.InvariantCulture),

            "vectors.add.exact_rational" =>
                FormatVector2(p["ax"] + p["bx"], p["ay"] + p["by"]),

            "vectors.subtract.exact_rational" =>
                FormatVector2(p["ax"] - p["bx"], p["ay"] - p["by"]),

            "vectors.scalar_multiply.exact_rational" =>
                FormatVector2(p["scalar"] * p["ax"], p["scalar"] * p["ay"]),

            "vectors.dot.exact_rational" =>
                (p["ax"] * p["bx"] + p["ay"] * p["by"]).ToString(CultureInfo.InvariantCulture),

            "vectors.magnitude.exact" =>
                p["magnitude"].ToString(CultureInfo.InvariantCulture),

            "vectors.between_points.exact" =>
                FormatVector2(p["x2"] - p["x1"], p["y2"] - p["y1"]),

            ExactLinearEquationQuestionFactory.FamilyId =>
                ((p["right"] - p["offset"]) / p["coefficient"])
                    .ToString(CultureInfo.InvariantCulture),

            "algebra.linear.variables_both_sides" =>
                ((p["rightOffset"] - p["leftOffset"]) /
                    (p["leftCoefficient"] - p["rightCoefficient"]))
                    .ToString(CultureInfo.InvariantCulture),

            ExactLinearInequalityQuestionFactory.FamilyId =>
                SolveLinearInequality(p),

            "algebra.linear.inequality.variables_both_sides" =>
                SolveLinearInequalityBothSides(p),

            _ => throw new InvalidOperationException($"Unsupported exact Mathematics solver family: {problem.Family}")
        };
    }

    private static string SolveFractionAddSubtract(IReadOnlyDictionary<string, int> parameters)
    {
        var n1 = parameters["n1"];
        var d1 = parameters["d1"];
        var n2 = parameters["n2"];
        var d2 = parameters["d2"];
        var operation = parameters["operation"];

        var numerator = operation == 0
            ? n1 * d2 + n2 * d1
            : n1 * d2 - n2 * d1;
        var denominator = d1 * d2;
        if (numerator < 0 || denominator <= 0)
            throw new InvalidOperationException("Invalid generated fraction operation.");

        var gcd = GreatestCommonDivisor(Math.Abs(numerator), denominator);
        numerator /= gcd;
        denominator /= gcd;
        return denominator == 1
            ? numerator.ToString(CultureInfo.InvariantCulture)
            : $"{numerator.ToString(CultureInfo.InvariantCulture)}/{denominator.ToString(CultureInfo.InvariantCulture)}";
    }

    private static string FormatVector2(int x, int y) =>
        $"<{x.ToString(CultureInfo.InvariantCulture)}, {y.ToString(CultureInfo.InvariantCulture)}>";

    private static bool TryParseVector2(string answer, out int x, out int y)
    {
        x = 0;
        y = 0;
        var text = answer.Trim();
        if (text.Length >= 2 &&
            ((text[0] == '<' && text[^1] == '>') ||
             (text[0] == '(' && text[^1] == ')') ||
             (text[0] == '[' && text[^1] == ']')))
        {
            text = text[1..^1].Trim();
        }

        var parts = text.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        return parts.Length == 2 &&
            int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out x) &&
            int.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out y);
    }

    private static bool TryParseFraction(string answer, out int numerator, out int denominator)
    {
        numerator = 0;
        denominator = 1;
        var text = answer.Trim();
        var slash = text.IndexOf('/');
        if (slash < 0)
            return int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out numerator);

        if (!int.TryParse(text[..slash].Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out numerator) ||
            !int.TryParse(text[(slash + 1)..].Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out denominator) ||
            denominator == 0)
        {
            return false;
        }

        if (denominator < 0)
        {
            numerator = -numerator;
            denominator = -denominator;
        }
        return true;
    }

    private static int IntegerPower(int value, int exponent)
    {
        if (exponent < 0)
            throw new InvalidOperationException("Exact integer sequence exponent must be non-negative.");

        var result = 1;
        for (var i = 0; i < exponent; i++)
            result = checked(result * value);
        return result;
    }

    private static int LeastCommonMultiple(int a, int b)
    {
        if (a <= 0 || b <= 0)
            throw new InvalidOperationException("LCM inputs must be positive.");
        return checked(a / GreatestCommonDivisor(a, b) * b);
    }

    private static int GreatestCommonDivisor(int a, int b)
    {
        a = Math.Abs(a);
        b = Math.Abs(b);
        while (b != 0)
            (a, b) = (b, a % b);
        return Math.Max(1, a);
    }

    private static string FormatMixedFraction(int numerator, int denominator)
    {
        if (numerator < denominator)
            return $"{numerator}/{denominator}";
        var whole = numerator / denominator;
        var remainder = numerator % denominator;
        return remainder == 0
            ? whole.ToString(CultureInfo.InvariantCulture)
            : $"{whole} {remainder}/{denominator}";
    }

    private static string SimplifyFraction(int numerator, int denominator)
    {
        if (denominator == 0)
            throw new InvalidOperationException("Fraction denominator cannot be zero.");
        if (denominator < 0)
        {
            numerator = -numerator;
            denominator = -denominator;
        }

        var gcd = GreatestCommonDivisor(Math.Abs(numerator), denominator);
        numerator /= gcd;
        denominator /= gcd;
        return denominator == 1
            ? numerator.ToString(CultureInfo.InvariantCulture)
            : $"{numerator.ToString(CultureInfo.InvariantCulture)}/{denominator.ToString(CultureInfo.InvariantCulture)}";
    }

    private static string SolveLinearInequalityBothSides(IReadOnlyDictionary<string, int> parameters)
    {
        var coefficient = parameters["leftCoefficient"] - parameters["rightCoefficient"];
        if (coefficient == 0)
            throw new InvalidOperationException("Generated inequality has zero combined coefficient.");

        var numerator = parameters["rightOffset"] - parameters["leftOffset"];
        if (numerator % coefficient != 0)
            throw new InvalidOperationException("Generated inequality has a non-integral exact boundary.");

        var boundary = numerator / coefficient;
        var relation = (InequalityRelation)parameters["relation"];
        var solvedRelation = coefficient < 0 ? Reverse(relation) : relation;
        return $"x {RelationSymbol(solvedRelation)} {boundary.ToString(CultureInfo.InvariantCulture)}";
    }

    private static string SolveLinearInequality(IReadOnlyDictionary<string, int> parameters)
    {
        var coefficient = parameters["coefficient"];
        var offset = parameters["offset"];
        var right = parameters["right"];
        if (coefficient == 0 || (right - offset) % coefficient != 0)
            throw new InvalidOperationException("Generated linear inequality has a non-integral or undefined boundary.");

        var boundary = (right - offset) / coefficient;
        var originalRelation = (InequalityRelation)parameters["relation"];
        var solvedRelation = coefficient < 0
            ? Reverse(originalRelation)
            : originalRelation;
        return $"x {RelationSymbol(solvedRelation)} {boundary.ToString(CultureInfo.InvariantCulture)}";
    }

    private static InequalityRelation Reverse(InequalityRelation relation) => relation switch
    {
        InequalityRelation.LessThan => InequalityRelation.GreaterThan,
        InequalityRelation.LessThanOrEqual => InequalityRelation.GreaterThanOrEqual,
        InequalityRelation.GreaterThan => InequalityRelation.LessThan,
        InequalityRelation.GreaterThanOrEqual => InequalityRelation.LessThanOrEqual,
        _ => throw new InvalidOperationException("Unsupported learner-facing inequality relation.")
    };

    private static string RelationSymbol(InequalityRelation relation) => relation switch
    {
        InequalityRelation.LessThan => "<",
        InequalityRelation.LessThanOrEqual => "≤",
        InequalityRelation.GreaterThan => ">",
        InequalityRelation.GreaterThanOrEqual => "≥",
        _ => throw new InvalidOperationException("Unsupported learner-facing inequality relation.")
    };

    private static string FormatLinearRule(int gradient, int intercept)
    {
        var mx = gradient switch
        {
            1 => "x",
            -1 => "-x",
            _ => $"{gradient.ToString(CultureInfo.InvariantCulture)}x"
        };

        if (intercept == 0)
            return mx;

        var sign = intercept > 0 ? "+" : "−";
        return $"{mx} {sign} {Math.Abs(intercept).ToString(CultureInfo.InvariantCulture)}";
    }

    private static string FormatLinearExpression(int coefficient, int offset)
    {
        var coefficientText = coefficient switch
        {
            1 => "x",
            -1 => "-x",
            _ => $"{coefficient.ToString(CultureInfo.InvariantCulture)}x"
        };

        if (offset == 0)
            return coefficientText;

        var sign = offset > 0 ? "+" : "−";
        return $"{coefficientText} {sign} {Math.Abs(offset).ToString(CultureInfo.InvariantCulture)}";
    }

    private static string NormalizeRemainderAnswer(string answer) =>
        answer
            .Trim()
            .ToLowerInvariant()
            .Replace("remainder", "r", StringComparison.Ordinal)
            .Replace(" ", string.Empty, StringComparison.Ordinal);

    private static string NormalizeInequalityAnswer(string answer) =>
        answer
            .Trim()
            .Replace(" ", string.Empty, StringComparison.Ordinal)
            .Replace("<=", "≤", StringComparison.Ordinal)
            .Replace(">=", "≥", StringComparison.Ordinal)
            .Replace("−", "-", StringComparison.Ordinal);

    private static ExactProblem Problem(
        string family,
        string prompt,
        string solution,
        AssessmentItemType itemType,
        params (string Name, int Value)[] parameters) =>
        new(
            family,
            prompt,
            solution,
            itemType,
            parameters.ToDictionary(x => x.Name, x => x.Value, StringComparer.Ordinal));

    private static int RelationCode(string relation) => relation switch
    {
        "<" => 0,
        ">" => 1,
        "=" => 2,
        _ => throw new InvalidOperationException("Unsupported fraction relation.")
    };

    private static string RelationFromCode(int code) => code switch
    {
        0 => "<",
        1 => ">",
        2 => "=",
        _ => throw new InvalidOperationException("Unsupported fraction relation code.")
    };

    private static string OrderThreeFractions(IReadOnlyDictionary<string, int> parameters)
    {
        var values = new List<(string Label, int Numerator, int Denominator)>
        {
            ("A", parameters["n1"], parameters["d1"]),
            ("B", parameters["n2"], parameters["d2"]),
            ("C", parameters["n3"], parameters["d3"])
        };

        values.Sort((left, right) =>
            checked(left.Numerator * right.Denominator)
                .CompareTo(checked(right.Numerator * left.Denominator)));

        return string.Join(" < ", values.Select(x => x.Label));
    }

    private static string NormalizeFractionOrderAnswer(string answer) =>
        answer.Trim()
            .ToUpperInvariant()
            .Replace(" ", string.Empty, StringComparison.Ordinal)
            .Replace("≤", "<", StringComparison.Ordinal);

    private static string Compare(int left, int right) =>
        left < right ? "<" : left > right ? ">" : "=";

    private static string Fingerprint(
        string fingerprintNamespace,
        string scopeKey,
        string family,
        IReadOnlyDictionary<string, int> parameters,
        string prompt)
    {
        // V1 G1-008: several source-based families carry unused randomized
        // helper fields (e.g. c or mode). An identical learner-visible
        // mathematical task must have ONE exposure fingerprint across seeds.
        var canonicalParameters = (UsCcssG1NextSourceFiveQuestions.Supports(family) ||
            UsCcssG1SourceFiveNextQuestions.Supports(family) ||
            UsCcssG1TeenFiveQuestions.Supports(family) ||
            UsCcssG1FullGradeStartQuestions.Supports(family) ||
            UsCcssG1FullGradeNextQuestions.Supports(family) ||
            UsCcssG1FullGradeStrategyAndTensQuestions.Supports(family) ||
            UsCcssG1FullGradeTensAndOnesQuestions.Supports(family) ||
            UsCcssG1FullGradeTwoDigitOperationsQuestions.Supports(family) ||
            UsCcssG1FullGradeUnit5AdditionQuestions.Supports(family) ||
            UsCcssG1FullGradeUnit4FinalQuestions.Supports(family) ||
            UsCcssG1FullGradeUnit8ReviewQuestions.Supports(family) ||
            UsCcssG1FullGradeUnit6MeasurementQuestions.Supports(family) ||
            UsCcssG1FullGradeUnit7GeometryTimeQuestions.Supports(family))
            ? "PROMPT=" + NormalizePrompt(prompt)
            : string.Join(
                ";",
                parameters
                    .OrderBy(x => x.Key, StringComparer.Ordinal)
                    .Select(x => $"{x.Key}={x.Value.ToString(CultureInfo.InvariantCulture)}"));

        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(
            $"{fingerprintNamespace}|{scopeKey}|{family}|{canonicalParameters}"));

        return fingerprintNamespace + ":" + Convert.ToHexString(bytes).ToLowerInvariant();
    }

    private sealed record ExactProblem(
        string Family,
        string Prompt,
        string Solution,
        AssessmentItemType ItemType,
        IReadOnlyDictionary<string, int> Parameters);

    private static bool RequiresPromptUniqueness(string family) =>
        UsCcssG1NextSourceFiveQuestions.Supports(family) ||
        UsCcssG1SourceFiveNextQuestions.Supports(family) ||
        UsCcssG1TeenFiveQuestions.Supports(family) ||
        UsCcssG1FullGradeStartQuestions.Supports(family) ||
        UsCcssG1FullGradeNextQuestions.Supports(family) ||
        UsCcssG1FullGradeStrategyAndTensQuestions.Supports(family) ||
        UsCcssG1FullGradeTensAndOnesQuestions.Supports(family) ||
        UsCcssG1FullGradeTwoDigitOperationsQuestions.Supports(family) ||
        UsCcssG1FullGradeUnit5AdditionQuestions.Supports(family) ||
        UsCcssG1FullGradeUnit4FinalQuestions.Supports(family) ||
        UsCcssG1FullGradeUnit8ReviewQuestions.Supports(family) ||
        UsCcssG1FullGradeUnit6MeasurementQuestions.Supports(family) ||
        UsCcssG1HowCloseStage3Questions.Supports(family) ||
        UsCcssG1FullGradeUnit7GeometryTimeQuestions.Supports(family) ||
        family is
            "supporting.geometry.shape_dimension" or
            "supporting.reasoning.multistep" or
            "measurement.scale.equal_intervals.read_value" or
            "fractions.compare.unlike.common_denominator" or
            "supporting.powers10.evaluate" or
            "supporting.powers10.multiply" or
            "supporting.powers10.divide" or
            "supporting.powers10.missing_exponent";

    private static string NormalizePrompt(string prompt) =>
        string.Join(
            " ",
            (prompt ?? string.Empty).Split(
                (char[]?)null,
                StringSplitOptions.RemoveEmptyEntries))
            .Trim();

}
