using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Grade 6 U02:L01 source-aligned ratio language. Short-answer ratio comparisons
/// and independently authored square-unit color partitions reuse the existing
/// ExactSkillContractQuestionEngine. Student collections, category decisions,
/// oral justification and drawn non-rectangles are TEACHER_REVIEW_REQUIRED.
/// </summary>
internal static class UsCcssG6Unit2RatioQuestions
{
    public const string L01Collections = "usccss.g6.u02.l01.ordered_collection_ratios";
    public const string L01Shade = "usccss.g6.u02.l01.shaded_area_ratio_counts";
    public const string L02Diagrams = "usccss.g6.u02.l02.labeled_two_row_ratios";
    public const string L02Cards = "usccss.g6.u02.l02.three_ingredient_ratio_cards";
    public const string L03RecipeBatches = "usccss.g6.u02.l03.recipe_batch_scale";
    public const string L03EqualTaste = "usccss.g6.u02.l03.same_taste_equivalent_ratios";
    public const string L04ColorAmounts = "usccss.g6.u02.l04.color_mixture_quantities";
    public const string L04EqualShade = "usccss.g6.u02.l04.same_shade_proportions";
    public const string L05Equivalence = "usccss.g6.u02.l05.equivalent_ratio_definition";
    public const string L05ScaleMissing = "usccss.g6.u02.l05.scaled_ratio_missing_term";

    public const string L06DoubleLineAmounts = "usccss.g6.u02.l06.double_number_line_amounts";
    public const string L06DoubleLineLogic = "usccss.g6.u02.l06.double_number_line_structure";

    public const string L07LineValues = "usccss.g6.u02.l07.created_double_number_line_values";
    public const string L07LineLogic = "usccss.g6.u02.l07.double_number_line_units_per";

    public const string L08UnitPrices = "usccss.g6.u02.l08.unit_prices_and_counts";
    public const string L08PriceLogic = "usccss.g6.u02.l08.unit_price_interpretation";

    public const string L09SpeedDistances = "usccss.g6.u02.l09.constant_speed_quantities";
    public const string L09SpeedComparisons = "usccss.g6.u02.l09.constant_speed_equivalence";

    public const string L10CompareAmounts = "usccss.g6.u02.l10.rate_comparison_quantities";
    public const string L10EquivalentContexts = "usccss.g6.u02.l10.compare_rates_reasoning";

    public const string L11TableAmounts = "usccss.g6.u02.l11.table_equivalent_ratio_values";
    public const string L11TableLogic = "usccss.g6.u02.l11.ratio_table_reasoning";

    public const string L12TableCalculations = "usccss.g6.u02.l12.table_missing_quantity";
    public const string L12TableStrategies = "usccss.g6.u02.l12.table_equivalence_methods";

    public const string L13RepresentationValues = "usccss.g6.u02.l13.table_line_ratio_values";
    public const string L13RepresentationLogic = "usccss.g6.u02.l13.representation_selection";

    public const string L14InfoGapNumbers = "usccss.g6.u02.l14.info_gap_ratio_quantities";
    public const string L14InfoGapReasoning = "usccss.g6.u02.l14.rate_information_requests";

    public const string L15TapeParts = "usccss.g6.u02.l15.tape_diagram_part_whole";
    public const string L15TapeLogic = "usccss.g6.u02.l15.part_to_whole_reasoning";

    public const string L16MultistepRatio = "usccss.g6.u02.l16.multistep_ratio_totals";
    public const string L16RepresentationChoice = "usccss.g6.u02.l16.ratio_tool_selection";

    public const string L17FermiAssumptions = "usccss.g6.u02.l17.first_party_fermi_assumption_arithmetic";
    public const string L17FermiSensitivity = "usccss.g6.u02.l17.first_party_fermi_sensitivity";

    public const string L0301PerOne = "usccss.g6.u03.l01.per_one_source_rate";
    public const string L0301RateMeaning = "usccss.g6.u03.l01.source_rate_meaning";

    public const string L0302UnitMagnitudes = "usccss.g6.u03.l02.unit_magnitude_and_conversion";
    public const string L0302UnitCategories = "usccss.g6.u03.l02.choose_appropriate_unit_category";

    public const string L0303SourceAmounts = "usccss.g6.u03.l03.measurement_source_quantity";
    public const string L0303AttributeClassification = "usccss.g6.u03.l03.measurement_attribute_identification";

    public const string L0304Conversions = "usccss.g6.u03.l04.source_ratio_unit_conversions";
    public const string L0304DirectionChecks = "usccss.g6.u03.l04.conversion_direction_and_units";

    public const string L0305Rates = "usccss.g6.u03.l05.source_speed_price_values";
    public const string L0305Comparisons = "usccss.g6.u03.l05.same_rate_price_speed_reasoning";

    public const string L0306Rates = "usccss.g6.u03.l06.source_reciprocal_rate_values";
    public const string L0306InterpretRate = "usccss.g6.u03.l06.reciprocal_rate_meaning";

    public const string L0307UnitRateValues = "usccss.g6.u03.l07.unit_rate_table_quantities";
    public const string L0307RateInvariance = "usccss.g6.u03.l07.unit_rate_equivalence";

    public const string L0308SpeedAmounts = "usccss.g6.u03.l08.speed_distance_time_quantities";
    public const string L0308PaceMeaning = "usccss.g6.u03.l08.speed_pace_comparison";

    public const string L0309RateValues = "usccss.g6.u03.l09.source_unit_rate_calculations";
    public const string L0309ComparisonLogic = "usccss.g6.u03.l09.solve_rate_comparison";

    public const string L0310PercentNumbers = "usccss.g6.u03.l10.percent_coin_values";
    public const string L0310PercentLogic = "usccss.g6.u03.l10.percent_comparison_logic";

    public const string L0311PercentValues = "usccss.g6.u03.l11.percent_scaled_amounts";
    public const string L0311PercentReasoning = "usccss.g6.u03.l11.percent_of_quantity_reasoning";

    public const string L0312TapePercent = "usccss.g6.u03.l12.tape_diagram_percent_amounts";
    public const string L0312TapeStructure = "usccss.g6.u03.l12.percent_tape_reasoning";

    public const string L0313BenchmarkNumbers = "usccss.g6.u03.l13.benchmark_percent_values";
    public const string L0313BenchmarkLogic = "usccss.g6.u03.l13.benchmark_percent_reasoning";

    public const string L0314PercentComputations = "usccss.g6.u03.l14.percent_unknowns";
    public const string L0314PercentInterpretation = "usccss.g6.u03.l14.percent_compare_and_inverse";

    public const string L0315PercentAmounts = "usccss.g6.u03.l15.percent_of_quantity_exact";
    public const string L0315PercentProperties = "usccss.g6.u03.l15.percent_of_quantity_reasoning";

    public const string L0316FindPercent = "usccss.g6.u03.l16.find_percent_from_two_amounts";
    public const string L0316FindPercentLogic = "usccss.g6.u03.l16.percent_division_reasoning";

    public const string L0317PaintCalculations = "usccss.g6.u03.l17.first_party_bounded_room_paint_math";
    public const string L0317PaintReasoning = "usccss.g6.u03.l17.first_party_paint_model_reasoning";

    public const string L0401SourceNumerics = "usccss.g6.u04.l01.division_and_source_quantities";
    public const string L0401QuotientScale = "usccss.g6.u04.l01.predict_quotient_size";

    public const string L0402DivisionNumbers = "usccss.g6.u04.l02.division_groups_and_size";
    public const string L0402DivisionMeaningLogic = "usccss.g6.u04.l02.division_interpretations";

    public const string L0403ContextualDivision = "usccss.g6.u04.l03.division_contextual_results";
    public const string L0403DivisionInterpretation = "usccss.g6.u04.l03.division_explanation_checks";

    public const string L0404FractionGroups = "usccss.g6.u04.l04.fraction_groups_exact";
    public const string L0404FractionModels = "usccss.g6.u04.l04.fraction_division_reasoning";

    public const string L0405FractionValues = "usccss.g6.u04.l05.fraction_group_quotients";
    public const string L0405FractionMeaning = "usccss.g6.u04.l05.fractional_group_interpretation";

    public const string L0406TapeValues = "usccss.g6.u04.l06.fraction_division_values";
    public const string L0406TapeLogic = "usccss.g6.u04.l06.fraction_tape_interpretation";

    public const string L0407FractionGroups = "usccss.g6.u04.l07.fraction_of_group_numeric";
    public const string L0407FractionLogic = "usccss.g6.u04.l07.fractional_group_reasoning";

    public const string L0408GroupSizeNumbers = "usccss.g6.u04.l08.fraction_group_size";
    public const string L0408GroupSizeLogic = "usccss.g6.u04.l08.fraction_group_size_reasoning";

    public const string L0409SourceWordValues = "usccss.g6.u04.l09.contextual_fraction_division";
    public const string L0409ModelReasoning = "usccss.g6.u04.l09.division_word_model_logic";

    public const string L0410AlgorithmValues = "usccss.g6.u04.l10.fraction_division_steps";
    public const string L0410AlgorithmLogic = "usccss.g6.u04.l10.division_reciprocal_reasoning";

    public const string L0411DivisionFluency = "usccss.g6.u04.l11.divide_fractions_exact";
    public const string L0411DivisionLogic = "usccss.g6.u04.l11.fraction_algorithm_reasoning";

    public const string L0412LengthValues = "usccss.g6.u04.l12.fractional_length_comparisons";
    public const string L0412LengthReasoning = "usccss.g6.u04.l12.fractional_comparison_reasoning";

    public const string L0413AreaValues = "usccss.g6.u04.l13.rectangle_area_fractional_sides";
    public const string L0413AreaLogic = "usccss.g6.u04.l13.rectangle_area_diagram_reasoning";

    public const string L0414GeometryNumbers = "usccss.g6.u04.l14.triangle_height_and_cube_counts";
    public const string L0414GeometryLogic = "usccss.g6.u04.l14.cube_and_triangle_reasoning";

    public const string L0415Volumes = "usccss.g6.u04.l15.fractional_prism_volume";
    public const string L0415VolumeLogic = "usccss.g6.u04.l15.prism_unit_cube_reasoning";

    public const string L0416AllOperations = "usccss.g6.u04.l16.fraction_operations_context";
    public const string L0416OperationsLogic = "usccss.g6.u04.l16.fraction_ops_reasoning";

    public const string L0417BoundedPacking = "usccss.g6.u04.l17.first_party_fixed_orientation_packing_math";
    public const string L0417PackingModelLogic = "usccss.g6.u04.l17.first_party_orientation_model_reasoning";

    public const string L0501DecimalArithmetic = "usccss.g6.u05.l01.shopping_decimal_arithmetic";
    public const string L0501DecimalEstimation = "usccss.g6.u05.l01.shopping_reasonableness";

    public const string L0502PlaceValueArithmetic = "usccss.g6.u05.l02.base_ten_decimal_arithmetic";
    public const string L0502PlaceValueLogic = "usccss.g6.u05.l02.align_decimal_place_values";

    public const string L0503DecimalCalculations = "usccss.g6.u05.l03.decimal_sum_difference_results";
    public const string L0503DecimalPrinciples = "usccss.g6.u05.l03.placevalue_borrowing_correctness";

    public const string L0504PreciseArithmetic = "usccss.g6.u05.l04.precise_decimal_add_subtract";
    public const string L0504DecimalReasoning = "usccss.g6.u05.l04.placevalue_high_precision_logic";

    public const string L0505DecimalProducts = "usccss.g6.u05.l05.decimal_products_base_ten";
    public const string L0505DecimalLogic = "usccss.g6.u05.l05.decimal_product_placevalue_reasoning";

    public const string L0506Products = "usccss.g6.u05.l06.decimal_multiplication_methods";
    public const string L0506MethodsLogic = "usccss.g6.u05.l06.decimal_product_scale_reasoning";

    public const string L0507ProductValues = "usccss.g6.u05.l07.decimal_partial_product_values";
    public const string L0507PartialLogic = "usccss.g6.u05.l07.partial_product_reasoning";

    public const string L0508ProductArithmetic = "usccss.g6.u05.l08.multiplication_calculation_values";
    public const string L0508ProductLogic = "usccss.g6.u05.l08.decimal_product_reasoning";

    public const string L0509WholeDivision = "usccss.g6.u05.l09.partial_quotient_results";
    public const string L0509DivisionLogic = "usccss.g6.u05.l09.partial_quotient_principles";

    public const string L0510LongDivisionNumbers = "usccss.g6.u05.l10.long_division_quotient_results";
    public const string L0510LongDivisionLogic = "usccss.g6.u05.l10.long_division_placevalue_interpretation";

    public const string L0511DecimalQuotients = "usccss.g6.u05.l11.whole_division_decimal_results";
    public const string L0511DivisionReasoning = "usccss.g6.u05.l11.decimal_quotient_placevalue_logic";

    public const string L0512DecimalDivision = "usccss.g6.u05.l12.decimal_divided_by_whole";
    public const string L0512DecimalScaleLogic = "usccss.g6.u05.l12.division_scale_equivalence";

    public const string L0513DecimalQuotients = "usccss.g6.u05.l13.decimal_divisor_quotients";
    public const string L0513DecimalLogic = "usccss.g6.u05.l13.decimal_divisor_reasoning";

    public const string L0514DecimalContext = "usccss.g6.u05.l14.decimal_context_calculations";
    public const string L0514DecimalReasoning = "usccss.g6.u05.l14.decimal_application_reasoning";

    public const string L0515FirstPartyMeasurements = "usccss.g6.u05.l15.first_party_fixed_paper_box_scaling";
    public const string L0515MeasurementLogic = "usccss.g6.u05.l15.measurement_precision_and_area_scaling";

    public const string L0601Unknowns = "usccss.g6.u06.l01.tape_equation_unknown_values";
    public const string L0601TapeLogic = "usccss.g6.u06.l01.tape_diagram_equation_interpretation";

    public const string L0602EquationSolutions = "usccss.g6.u06.l02.variable_equation_solutions";
    public const string L0602EquationTruth = "usccss.g6.u06.l02.equation_truth_and_model_logic";

    public const string L0603HangerValues = "usccss.g6.u06.l03.hanger_equation_unknowns";
    public const string L0603HangerLogic = "usccss.g6.u06.l03.balance_equation_reasoning";

    public const string L0604SolveValues = "usccss.g6.u06.l04.equation_context_solutions";
    public const string L0604EquationReasoning = "usccss.g6.u06.l04.equation_choice_and_reasoning";

    public const string L0605FractionBarNumeric = "usccss.g6.u06.l05.rational_fraction_bar_division";
    public const string L0605FractionBarLogic = "usccss.g6.u06.l05.fraction_bar_meaning_and_solutions";

    public const string L0606Expressions = "usccss.g6.u06.l06.expression_evaluation_contexts";
    public const string L0606ExpressionLogic = "usccss.g6.u06.l06.expression_interpretation";

    public const string L0607PercentEquations = "usccss.g6.u06.l07.percent_equation_unknown_whole";
    public const string L0607PercentEquationLogic = "usccss.g6.u06.l07.percent_equation_interpretation";

    public const string L0608ExpressionValues = "usccss.g6.u06.l08.equivalent_expression_evaluation";
    public const string L0608EquivalentLogic = "usccss.g6.u06.l08.equivalent_for_all_values_reasoning";

    public const string L0609DistributeNumbers = "usccss.g6.u06.l09.distributive_numeric_products";
    public const string L0609DistributeLogic = "usccss.g6.u06.l09.distributive_area_reasoning";

    public const string L0610VariableArea = "usccss.g6.u06.l10.variable_distributive_values";
    public const string L0610AreaReasoning = "usccss.g6.u06.l10.variable_distributive_equivalence";

    public const string L0611AlgebraEvaluation = "usccss.g6.u06.l11.expansion_factor_values";
    public const string L0611DistributionLogic = "usccss.g6.u06.l11.distributive_equivalence_logic";

    public const string L0612ExponentValues = "usccss.g6.u06.l12.exponents_numeric_values";
    public const string L0612ExponentReasoning = "usccss.g6.u06.l12.exponent_and_distributive_reasoning";

    public const string L0613ExponentNumbers = "usccss.g6.u06.l13.exponents_with_rational_bases";
    public const string L0613ExponentTruth = "usccss.g6.u06.l13.exponent_equation_truth";

    public const string L0614ExponentEvaluation = "usccss.g6.u06.l14.exponent_order_numeric";
    public const string L0614ExponentOrderLogic = "usccss.g6.u06.l14.exponent_evaluation_reasoning";

    public const string L0615ExponentVariables = "usccss.g6.u06.l15.exponential_variable_values";
    public const string L0615ExponentVariablesLogic = "usccss.g6.u06.l15.exponential_expression_reasoning";

    public const string L0616RatesAndTables = "usccss.g6.u06.l16.related_quantities_numeric";
    public const string L0616RelatedLogic = "usccss.g6.u06.l16.two_variable_equation_interpretation";

    public const string L0617SpeedTables = "usccss.g6.u06.l17.constant_speed_numeric";
    public const string L0617SpeedLogic = "usccss.g6.u06.l17.constant_speed_equation_interpretation";

    public const string L0618RelationValues = "usccss.g6.u06.l18.related_area_volume_growth";
    public const string L0618RelationLogic = "usccss.g6.u06.l18.inverse_and_exponential_relationships";

    public const string L0619BoundedRelationships = "usccss.g6.u06.l19.first_party_equation_table_examples";
    public const string L0619InterpretRelationships = "usccss.g6.u06.l19.first_party_relationship_interpretation";

    public const string L0701SignedValues = "usccss.g6.u07.l01.signed_temperatures_elevation";
    public const string L0701SignLogic = "usccss.g6.u07.l01.signed_number_interpretation";

    public const string L0702OppositeNumbers = "usccss.g6.u07.l02.signed_opposites_values";
    public const string L0702OppositeLogic = "usccss.g6.u07.l02.opposite_number_line_reasoning";

    public const string L0703Comparisons = "usccss.g6.u07.l03.signed_order_comparisons";
    public const string L0703SignedLogic = "usccss.g6.u07.l03.signed_inequality_reasoning";

    public const string L0704RationalOrder = "usccss.g6.u07.l04.order_signed_numbers_source";
    public const string L0704OrderLogic = "usccss.g6.u07.l04.signed_ordering_logic";

    public const string L0704Ordering = "usccss.g6.u07.l04.rational_order_and_temperature";
    public const string L0704SourceChecks = "usccss.g6.u07.l04.rational_order_explanations_and_cumulative";

    public const string L0705SignedChanges = "usccss.g6.u07.l05.positive_negative_changes";
    public const string L0705ChangeLogic = "usccss.g6.u07.l05.signed_context_representations";

    public const string L0706AbsoluteValues = "usccss.g6.u07.l06.absolute_distance_values";
    public const string L0706AbsoluteLogic = "usccss.g6.u07.l06.absolute_value_reasoning";

    public const string L0707OrderMagnitude = "usccss.g6.u07.l07.signed_comparisons_absolute";
    public const string L0707OrderLogic = "usccss.g6.u07.l07.order_and_absolute_value_inequalities";

    public const string L0708InequalityExamples = "usccss.g6.u07.l08.inequality_bound_examples";
    public const string L0708InequalityLogic = "usccss.g6.u07.l08.inequality_constraints_logic";

    public const string L0709BoundedSolutions = "usccss.g6.u07.l09.inequality_solution_values";
    public const string L0709SolutionLogic = "usccss.g6.u07.l09.inequality_solution_set_logic";

    public const string L0710ContextSolutions = "usccss.g6.u07.l10.inequality_context_solutions";
    public const string L0710ConstraintLogic = "usccss.g6.u07.l10.inequality_context_reasoning";

    public const string L0711Coordinates = "usccss.g6.u07.l11.coordinates_and_axis_locations";
    public const string L0711CoordinateLogic = "usccss.g6.u07.l11.coordinate_reasoning";

    public const string L0712PlotAndScale = "usccss.g6.u07.l12.four_quadrant_plot_and_axis_scale";
    public const string L0712ScaleReasoning = "usccss.g6.u07.l12.axis_precision_and_plot_logic";

    public const string L0713CoordinatesData = "usccss.g6.u07.l13.coordinate_context_numeric";
    public const string L0713ContextLogic = "usccss.g6.u07.l13.interpret_axes_and_extrema";

    public const string L0714Distances = "usccss.g6.u07.l14.horizontal_vertical_coordinate_distances";
    public const string L0714DistanceLogic = "usccss.g6.u07.l14.distance_same_axis_reasoning";

    public const string L0715PolygonNumbers = "usccss.g6.u07.l15.polygon_perimeter_signed_coordinates";
    public const string L0715PolygonChecks = "usccss.g6.u07.l15.polygon_coordinate_reasoning";

    public const string L0716Factors = "usccss.g6.u07.l16.gcf_source_numbers_and_groups";
    public const string L0716GcfLogic = "usccss.g6.u07.l16.factor_context_and_sign_logic";

    public const string L0717Multiples = "usccss.g6.u07.l17.least_common_multiple_source_cycles";
    public const string L0717LcmLogic = "usccss.g6.u07.l17.multiple_cycle_reasoning";

    public const string L0718FactorMultipleChoices = "usccss.g6.u07.l18.gcf_lcm_context_problems";
    public const string L0718DecisionLogic = "usccss.g6.u07.l18.factor_multiple_selection_reasoning";

    public const string L0719FirstPartyPlotting = "usccss.g6.u07.l19.first_party_plotting_coordinates";
    public const string L0719FirstPartyPictureLogic = "usccss.g6.u07.l19.first_party_picture_precision_rules";

    public const string L0801ClassifyData = "usccss.g6.u08.l01.numerical_vs_categorical_sources";
    public const string L0801DataReasoning = "usccss.g6.u08.l01.data_type_and_dotplot_reasoning";

    public const string L0802StatisticalClassifications = "usccss.g6.u08.l02.statistical_question_classification";
    public const string L0802StatsLogic = "usccss.g6.u08.l02.expected_data_variation_reasoning";

    public const string L0803Frequency = "usccss.g6.u08.l03.dotplot_frequency_counts";
    public const string L0803PlotLogic = "usccss.g6.u08.l03.frequency_plot_interpretation";

    public const string L0804PlotValues = "usccss.g6.u08.l04.source_dotplot_comparisons";
    public const string L0804InterpretPlot = "usccss.g6.u08.l04.dotplot_center_spread_logic";

    public const string L0805DotPlotValues = "usccss.g6.u08.l05.dotplot_source_comparisons";
    public const string L0805DotPlotLogic = "usccss.g6.u08.l05.dotplot_center_spread_interpretation";

    public const string L0806HistogramNumerics = "usccss.g6.u08.l06.histogram_bin_frequencies";
    public const string L0806HistogramLogic = "usccss.g6.u08.l06.histogram_vs_dotplot_reasoning";

    public const string L0807HistogramCompare = "usccss.g6.u08.l07.histogram_center_spread_numeric";
    public const string L0807HistogramLogic = "usccss.g6.u08.l07.center_spread_and_inequality_interpretation";

    public const string L0808ShapeNumbers = "usccss.g6.u08.l08.histogram_shape_and_cumulative";
    public const string L0808ShapeLogic = "usccss.g6.u08.l08.histogram_shape_explanations";

    public const string L0809Means = "usccss.g6.u08.l09.mean_and_balanced_total";
    public const string L0809MeanLogic = "usccss.g6.u08.l09.mean_fair_share_reasoning";

    public const string L0810BalanceNumbers = "usccss.g6.u08.l10.mean_balance_source_data";
    public const string L0810BalanceLogic = "usccss.g6.u08.l10.balance_point_interpretation";

    public const string L0811MadValues = "usccss.g6.u08.l11.mean_absolute_deviation_amounts";
    public const string L0811MadReasoning = "usccss.g6.u08.l11.mad_interpretation_logic";

    public const string L0812CompareMeans = "usccss.g6.u08.l12.mean_mad_source_datasets";
    public const string L0812CompareLogic = "usccss.g6.u08.l12.mean_mad_spread_context";

    public const string L0813Medians = "usccss.g6.u08.l13.median_sorted_samples";
    public const string L0813MedianLogic = "usccss.g6.u08.l13.median_interpretations";

    public const string L0814CenterValues = "usccss.g6.u08.l14.mean_median_outlier_changes";
    public const string L0814CenterLogic = "usccss.g6.u08.l14.mean_median_choice_reasoning";

    public const string L0815QuartileValues = "usccss.g6.u08.l15.quartiles_five_number_summary";
    public const string L0815QuartileLogic = "usccss.g6.u08.l15.quartile_spread_reasoning";

    public const string L0816BoxPlotValues = "usccss.g6.u08.l16.box_plot_source_quartiles";
    public const string L0816BoxPlotLogic = "usccss.g6.u08.l16.five_number_box_plot_interpretation";

    public const string L0817BoxPlotValues = "usccss.g6.u08.l17.boxplot_athlete_pineapple_statistics";
    public const string L0817BoxPlotLogic = "usccss.g6.u08.l17.interpret_center_and_spread_boxplots";

    public const string L0818SourceStatistics = "usccss.g6.u08.l18.first_party_statistics_context";
    public const string L0818SelectionLogic = "usccss.g6.u08.l18.first_party_statistical_report_selection";

    internal sealed record Problem(string Family, string Prompt, string Explanation,
        AssessmentItemType ItemType, IReadOnlyDictionary<string,int> Parameters);

    private static readonly (string First,int A,string Second,int B)[] Pairs =
    [
        ("bananas",9,"apples",4),       // source practice P1
        ("plums",3,"apples",4),         // source practice P1
        ("apples",4,"plums",3),         // reverse of P1
        ("bananas",9,"plums",3),        // source practice P1
        ("squares",6,"triangles",3),
        ("triangles",3,"squares",6),
        ("circles",8,"stars",12),
        ("stars",12,"circles",8),
        ("red counters",6,"blue counters",9),
        ("green counters",5,"yellow counters",2),
        ("cards",10,"coins",5),
        ("shells",7,"rocks",3)
    ];

    // (total equal square units, first-color ratio term, second-color term,
    // number of units for the named first color?)
    private static readonly (int Total,int First,int Second,bool FirstColor)[] Shades =
    [
        (24,2,1,true),(24,2,1,false),
        (18,2,1,true),(18,2,1,false),
        (30,2,1,true),(30,2,1,false),
        (27,2,1,true),(27,2,1,false),
        (24,3,1,true),(24,3,1,false),
        (20,3,2,true),(20,3,2,false)
    ];

    // First-party text-accessible discrete diagrams. The source 4-green/2-white
    // practice diagram and 2-cup/6-tablespoon classroom setup are represented,
    // without using the publisher's original artwork.
    private static readonly (string First,int A,string UnitA,string Second,int B,string UnitB)[] Diagrams =
    [
        ("green paint",4,"cups","white paint",2,"cups"),
        ("white paint",2,"cups","green paint",4,"cups"),
        ("white paint",2,"cups","blue paint",6,"tablespoons"),
        ("blue paint",6,"tablespoons","white paint",2,"cups"),
        ("flour",8,"cups","water",2,"pints"),
        ("juice",6,"cups","soda water",3,"cups"),
        ("blue cubes",6,"blocks","red cubes",4,"blocks"),
        ("red blocks",3,"cubes","yellow blocks",5,"cubes"),
        ("paper tiles",5,"pieces","wood tiles",2,"pieces"),
        ("paint",7,"cups","paste",4,"tablespoons"),
        ("squares",4,"tiles","triangles",6,"tiles"),
        ("short bars",9,"pieces","long bars",3,"pieces")
    ];

    private static readonly (string A,int CountA,string UnitA,string B,int CountB,string UnitB,
        string C,int CountC,string UnitC)[] Cards =
    [
        ("raisins",2,"cups","pretzels",4,"cups","almonds",6,"cups"),
        ("tomato sauce",4,"cups","oil",2,"tablespoons","spice",1,"teaspoon"),
        ("flour",3,"cups","milk",6,"cups","eggs",2,"eggs"),
        ("water",1,"cup","fruit",3,"cups","ice",5,"cubes"),
        ("red beads",4,"beads","blue beads",6,"beads","green beads",2,"beads"),
        ("yellow markers",5,"markers","blue markers",2,"markers","red markers",3,"markers"),
        ("orange paint",2,"cups","white paint",3,"cups","black paint",4,"tablespoons"),
        ("rice",6,"cups","beans",2,"cups","herbs",4,"teaspoons"),
        ("paper squares",3,"squares","paper circles",5,"circles","paper triangles",1,"triangle"),
        ("raisins",2,"cups","nuts",4,"cups","seeds",3,"cups"),
        ("juice",7,"cups","water",3,"cups","lemon syrup",2,"tablespoons"),
        ("cardboard",4,"sheets","glue",1,"bottle","paint",6,"tubes")
    ];

    // Original source numerical skills, independently worded and keyed.
    private static readonly (string Prompt,int Answer,string Explanation)[] RecipeBatches =
    [
        ("Base recipe: 3 tsp mustard seeds, 5 tsp chili powder and 1 tsp salt. Diagram shows 9, 15 and 3 tsp respectively. How many complete batches?",3,"Every ingredient is multiplied by 3."),
        ("A batch of chocolate milk has 2 cups milk and 5 tablespoons cocoa. Find milk cups for TWO batches.",4,"2 × 2 = 4 cups milk."),
        ("A batch of chocolate milk has 2 cups milk and 5 tablespoons cocoa. Find cocoa tablespoons for TWO batches.",10,"5 × 2 = 10 tablespoons."),
        ("Sparkling-water:concentrate recipe is 3:1. How many cups sparkling water for TWO batches?",6,"3 × 2 = 6."),
        ("Sparkling-water:concentrate recipe is 3:1. How many cups concentrate for THREE batches?",3,"1 × 3 = 3."),
        ("Cookies use 5 cups flour and 2 tsp vanilla per batch. Flour for TWO batches?",10,"5 × 2 = 10 cups."),
        ("Cookies use 5 cups flour and 2 tsp vanilla per batch. Vanilla for TWO batches?",4,"2 × 2 = 4 tsp."),
        ("Cookies use 5 cups flour and 2 tsp vanilla per batch. Flour for THREE batches?",15,"5 × 3 = 15 cups."),
        ("Cookies use 5 cups flour and 2 tsp vanilla per batch. Vanilla for THREE batches?",6,"2 × 3 = 6 tsp."),
        ("Cookies use 5 cups flour and 2 tsp vanilla per batch. Flour for FIVE batches?",25,"5 × 5 = 25 cups."),
        ("Cookies use 5 cups flour and 2 tsp vanilla per batch. Vanilla for FIVE batches?",10,"2 × 5 = 10 tsp."),
        ("A drink recipe uses 1 cup water and 4 tsp powder. How many tsp powder for TWO batches?",8,"4 × 2 = 8 tsp."),
        ("A drink recipe uses 1 cup water and 4 tsp powder. How many cups water for TWO batches?",2,"1 × 2 = 2 cups."),
        ("Sparkling water:juice concentrate is 3:1. How many cups sparkling water for FIVE batches?",15,"3 × 5 = 15 cups."),
        ("Sparkling water:juice concentrate is 3:1. How many cups concentrate for FIVE batches?",5,"1 × 5 = 5 cups."),
        ("Chocolate milk is 2 cups milk and 5 tbsp cocoa per batch. Milk cups for FOUR batches?",8,"2 × 4 = 8 cups."),
        ("Chocolate milk is 2 cups milk and 5 tbsp cocoa per batch. Cocoa tablespoons for FOUR batches?",20,"5 × 4 = 20 tablespoons."),
        ("Cumulative cube review: from 80 unit cubes, what is the maximum whole-number cube EDGE length?",4,"4 cubed=64, 5 cubed=125 exceeds 80."),
        ("Cumulative cube review: from 80 unit cubes, what is the VOLUME of the largest cube you can build?",64,"The largest perfect cube no more than 80 is 4 cubed=64."),
        ("Cumulative ratio language: 6 dogs for every 5 cats. How many cats in three such groups?",15,"5 × 3 = 15 cats.")
    ];

    private static readonly (string Prompt,int Answer,string Explanation)[] EqualTaste =
    [
        ("Are 1 cup water + 4 tsp powder and 2 cups water + 8 tsp powder equal-strength batches? 1=yes,0=no.",1,"Both quantities doubled."),
        ("Are 1 cup water + 4 tsp powder and 2 cups water + 4 tsp powder equal-strength batches? 1=yes,0=no.",0,"Only water doubled."),
        ("Do sparkling water:grape concentrate 3:1 and 6:2 preserve the recipe? 1=yes,0=no.",1,"Both values doubled."),
        ("Do sparkling water:grape concentrate 3:1 and 6:3 preserve the recipe? 1=yes,0=no.",0,"Different scaling of the two quantities."),
        ("Do cookie flour:vanilla 5:2 and 10:4 preserve the recipe? 1=yes,0=no.",1,"Both doubled."),
        ("Do cookie flour:vanilla 5:2 and 10:3 preserve the recipe? 1=yes,0=no.",0,"The vanilla quantity was not doubled."),
        ("Do chocolate-milk milk:cocoa 2:5 and 4:10 preserve the recipe? 1=yes,0=no.",1,"Both doubled."),
        ("Do chocolate-milk milk:cocoa 2:5 and 4:9 preserve the recipe? 1=yes,0=no.",0,"Ratio changed."),
        ("Do spice-mix mustard:chili:salt 3:5:1 and 9:15:3 preserve the batch ratio? 1=yes,0=no.",1,"All three ingredients tripled."),
        ("Do spice-mix mustard:chili:salt 3:5:1 and 9:15:2 preserve the batch ratio? 1=yes,0=no.",0,"Salt was doubled rather than tripled.")
    ];

    // Color-mixing ratios from original teacher/student/practice tasks, recast
    // as authored single-number selected Practice. Physical color matching and
    // accessibility/tactile dough experiments are teacher-assessed.
    private static readonly (string Prompt,int Answer,string Explanation)[] ColorAmounts =
    [
        ("For a mixture 5 mL blue and 15 mL yellow, how many mL BLUE for TWO identical batches?",10,"Both ingredient quantities double."),
        ("For a mixture 5 mL blue and 15 mL yellow, how many mL YELLOW for TWO identical batches?",30,"15 × 2 = 30 mL."),
        ("Diego uses 10 tbsp yellow and 2 tbsp blue. How many tbsp yellow would go with 3 tbsp blue to preserve shade?",15,"Yellow-to-blue is 5:1; 3 blue needs 15 yellow."),
        ("Diego uses 10 tbsp yellow and 2 tbsp blue. How many tbsp blue with 15 tbsp yellow to preserve shade?",3,"15:3 = 10:2."),
        ("A 1:3 blue:yellow formula has 25 mL blue. How many total mL yellow does it need?",75,"25 × 3 = 75 mL yellow."),
        ("An altered 5:15 mL blue:yellow mixture received +20 mL of EACH and now is 25:35. How many MORE mL yellow to restore 1:3?",40,"Target yellow75; 75-35=40 mL."),
        ("A 17 mL blue and 13 mL yellow mixture needs 1:3 blue:yellow. How much EXTRA yellow must be added?",38,"Yellow target51; 51-13=38 mL."),
        ("Clare's sky blue uses 2 cups blue for 1 gallon white. For TWO batches, how many cups BLUE?",4,"2 × 2 = 4 cups blue."),
        ("Clare's sky blue uses 2 cups blue for 1 gallon white. For TWO batches, how many gallons WHITE?",2,"1 × 2 = 2 gallons white."),
        ("Diego has 10 tbsp yellow for 2 tbsp blue. If BOTH double, how many tbsp yellow?",20,"10 × 2 = 20 tbsp."),
        ("Diego has 10 tbsp yellow for 2 tbsp blue. If BOTH double, how many tbsp blue?",4,"2 × 2 = 4 tbsp."),
        ("Purple formula blue:red=8:3. Jada has 24 mL blue. How many mL red preserve the ratio?",9,"Scale both by3: 24:9."),
        ("Diego's original yellow:blue=10:2. How many tbsp yellow for ONE tbsp blue?",5,"10:2 equivalent to 5:1."),
        ("Diego's original yellow:blue=10:2. How many tbsp blue for 30 tbsp yellow?",6,"Triple every component:30:6."),
        ("An independent orange mix uses3 tbsp red and6 tbsp yellow. Four batches: how many tbsp RED?",12,"3 ×4=12 tbsp red."),
        ("Diego's original green mix has10 tbsp yellow and2 tbsp blue. How many tbsp BLUE in one batch?",2,"Given count is 2 tablespoons blue."),
        ("A purple mix uses 8 mL blue and 3 mL red. How many mL BLUE in the source-size batch?",8,"The first named ingredient quantity is 8 mL."),
        ("Cumulative square area: a square of side5ft occupies how many square feet?",25,"5×5=25 square feet.")
    ];

    private static readonly (string Prompt,int Answer,string Explanation)[] EqualShades =
    [
        ("Will 5:15 blue:yellow and10:30 blue:yellow predict the same shade under consistent ingredients? 1=yes,0=no.",1,"Both double."),
        ("Will 5:15 blue:yellow and25:35 blue:yellow predict the same shade? 1=yes,0=no.",0,"Adding 20 to each does not preserve the ratio."),
        ("Will 10:2 yellow:blue and15:3 yellow:blue preserve Diego's shade? 1=yes,0=no.",1,"Both yellow:blue ratios are 5:1."),
        ("Will 10:2 yellow:blue and11:3 yellow:blue preserve Diego's shade? 1=yes,0=no.",0,"11:3 is not 5:1."),
        ("Will 8:3 blue:red and24:9 blue:red preserve the purple recipe? 1=yes,0=no.",1,"Both scaled by3."),
        ("Will 8:3 blue:red and16:9 blue:red preserve the purple recipe? 1=yes,0=no.",0,"Scale factors are inconsistent."),
        ("Will 2 cups blue:1 gallon white and4 cups blue:2 gallons white preserve the two-batch recipe? 1=yes,0=no.",1,"Both ingredient amounts double in their own units."),
        ("Will 2 cups blue:1 gallon white and4 cups blue:1 gallon white preserve the recipe? 1=yes,0=no.",0,"Only blue increases."),
        ("Will 5:15 blue:yellow and17:51 blue:yellow preserve the numeric ratio? 1=yes,0=no.",1,"Both simplify to1:3."),
        ("Will 5:15 blue:yellow and17:13 blue:yellow preserve the ratio? 1=yes,0=no.",0,"17:13 differs from1:3."),
        ("Does 1 cup flour:5 tbsp water and2 cups flour:10 tbsp water preserve proportions? 1=yes,0=no.",1,"Equal scaling can preserve dough texture under consistent mixing."),
        ("Does 1 cup flour:5 tbsp water and2 cups flour:6 tbsp water preserve proportions? 1=yes,0=no.",0,"Scaling is not common for both quantities.")
    ];

    // Extensible per-lesson numerical selected Practice bank: avoid creating a
    // new engine or modifying the shared exact dispatcher for each Unit 2 lesson.
    // Every bank question is source-scoped; diagrams/explanations are teacher reviewed.
    private sealed record LaterRatioProblem(string Prompt,int Answer,string Explanation);
    private static readonly Dictionary<string,LaterRatioProblem[]> AdditionalLessonFamilies = new(StringComparer.Ordinal)
    {
        [L05Equivalence] =
        [
            new("Are 4:5 and 8:10 equivalent? Enter 1=yes,0=no.",1,"Scale both terms by2."),
            new("Are 18:3 and 6:1 equivalent? 1=yes,0=no.",1,"Divide both by3."),
            new("Are 2:7 and10000:35000 equivalent? 1=yes,0=no.",1,"Both multiply by5000."),
            new("Are 6:4 and18:8 equivalent? 1=yes,0=no.",0,"6-to18 scales ×3,4-to8 ×2."),
            new("Are 3:6 and6:3 equivalent? 1=yes,0=no.",0,"Reversing terms changes the ratio."),
            new("Are 8:12 and12:18 equivalent? 1=yes,0=no.",1,"Both scale ×1.5."),
            new("Are 2:3 and4:5 equivalent? 1=yes,0=no.",0,"Adding2 to each does not preserve a ratio."),
            new("Are 3:4 and15:20 equivalent? 1=yes,0=no.",1,"Scale both by5."),
            new("Are 3:4 and9:10 equivalent? 1=yes,0=no.",0,"Scaling factors3 and2.5 differ."),
            new("Are 3:6:10 and15:30:50 equivalent ordered ingredient ratios? 1=yes,0=no.",1,"All ingredients scale ×5."),
            new("Are 3:6:10 and15:30:40 equivalent? 1=yes,0=no.",0,"Soup changes by different factor."),
            new("Are 4:6 and2:3 equivalent? 1=yes,0=no.",1,"Divide both by2."),
            new("Are 6:4 and18:12 equivalent? 1=yes,0=no.",1,"Both scale ×3."),
            new("Are 6:4 and3:2 equivalent? 1=yes,0=no.",1,"Both divide by2."),
            new("Are 6:4 and18:8 equivalent? 1=yes,0=no.",0,"18/6 differs from8/4.")
        ],
        [L05ScaleMissing] =
        [
            new("One original ratio4:5 becomes ?:10 when both terms double. Find missing first term.",8,"4×2=8."),
            new("One original ratio4:5 becomes 8:? when both terms double. Find missing second term.",10,"5×2=10."),
            new("18:3 simplifies to ?:1. Find first term.",6,"Divide both by3."),
            new("3 cups pasta,6 ounces tuna,10 ounces soup per batch. Soup ounces for HALF a batch?",5,"10×1/2=5 ounces."),
            new("3 cups pasta,6 ounces tuna,10 ounces soup. Pasta cups for FIVE batches?",15,"3×5=15 cups."),
            new("3 cups pasta,6 ounces tuna,10 ounces soup. Tuna ounces for FIVE batches?",30,"6×5=30 ounces."),
            new("3 cups pasta,6 ounces tuna,10 ounces soup. Soup ounces for FIVE batches?",50,"10×5=50 ounces."),
            new("A fixed-height 9 by18 inch casserole tray has base area in square inches?",162,"9×18=162 square inches."),
            new("Same-height casserole double batch needs double the 162 sq-in original base area. New base area?",324,"162×2=324 square inches."),
            new("Same-height casserole half batch needs half the 162 sq-in original base area. New base area?",81,"162/2=81 square inches."),
            new("Source fruit bowl:6 bananas,4 apples,3 oranges. If bananas scale by2, new banana count?",12,"6×2=12."),
            new("Source fruit bowl:6 bananas,4 apples,3 oranges. If oranges scale by6, new orange count?",18,"3×6=18."),
            new("In a4:6 ratio, multiply both terms by3. What is the NEW second term?",18,"6×3=18."),
            new("In a4:5 ratio, multiply both terms by4. What is the NEW second term?",20,"5×4=20."),
            new("Source fruit bowl:6 bananas and3 oranges. How many bananas for EACH orange?",2,"6/3=2."),
            new("In a 6:4 pair, simplifying both by2 gives3:?. Find missing second term.",2,"4/2=2.")
        ],
        [L06DoubleLineAmounts] =
        [
            new("Paint has 2 cups yellow for every 3 cups red. YELLOW cups in 3 batches?",6,"2×3=6 yellow."),
            new("Paint has 2 cups yellow for every 3 cups red. RED cups in 3 batches?",9,"3×3=9 red."),
            new("Cookies use 5 cups flour per 3 eggs. FLOUR cups in 2 batches?",10,"5×2=10 flour."),
            new("Cookies use 5 cups flour per 3 eggs. EGGS in 2 batches?",6,"3×2=6 eggs."),
            new("Cookies use 5 cups flour per 3 eggs. FLOUR cups in 3 batches?",15,"5×3=15 cups."),
            new("Cookies use 5 cups flour per 3 eggs. EGGS in 3 batches?",9,"3×3=9."),
            new("Cookies use 5 cups flour per 3 eggs. FLOUR cups in 4 batches?",20,"5×4=20."),
            new("Cookies use 5 cups flour per 3 eggs. EGGS in 4 batches?",12,"3×4=12."),
            new("Cookies use 5 cups flour per 3 eggs. FLOUR cups with 6 eggs?",10,"Two batches need10 cups flour."),
            new("Cookies use 5 cups flour per 3 eggs. EGGS with 15 cups flour?",9,"Three batches need9 eggs."),
            new("Purple paint uses 3 cups red to 2 cups blue per batch. Number of BATCHES for 12 cups red?",4,"12÷3=4 batches."),
            new("Purple paint uses 3 cups red to 2 cups blue per batch. Number of BATCHES for 6 cups blue?",3,"6÷2=3 batches."),
            new("A pizza plan uses 3 pizzas for every 7 children. How many pizzas for 21 children?",9,"Three sets of 7 children need9 pizzas."),
            new("A parallelogram has base 6 units and perpendicular height4 units. Find its area.",24,"6×4=24 square units."),
            new("A triangle has base8 units and perpendicular height6 units. Find its area.",24,"8×6÷2=24 square units."),
            new("Purple paint uses 3 red cups for every 2 blue cups. How many red cups in 2 batches?",6,"3×2=6 red cups.")
        ],
        [L06DoubleLineLogic] =
        [
            new("Both axes of a double number line share aligned zero marks for their quantities. 1=yes,0=no.",1,"The axes show the same number of batches at aligned ticks."),
            new("For a 2-yellow:3-red mix, 6 yellow and 9 red represent3 batches. 1=yes,0=no.",1,"Both quantities ×3."),
            new("For a 2-yellow:3-red mix, 6 yellow and 6 red preserve the shade. 1=yes,0=no.",0,"6:6 differs from2:3."),
            new("For cookies 5 flour:3 eggs, four batches are 20 cups flour and12 eggs. 1=yes,0=no.",1,"Both quantities ×4."),
            new("For cookies 5 flour:3 eggs, four batches are15 cups flour and12 eggs. 1=yes,0=no.",0,"Flour scales×3, eggs×4."),
            new("Purple ratio3red:2blue,12red:8blue is the same shade. 1=yes,0=no.",1,"Both ×4."),
            new("Purple ratio3red:2blue,12red:6blue is the same shade. 1=yes,0=no.",0,"Factors4 and3 differ."),
            new("Pizza ratio7 kids:3 pizzas,21 kids:9 pizzas is equivalent. 1=yes,0=no.",1,"Both triple."),
            new("Pizza ratio7 kids:3 pizzas,21 kids:3 pizzas is equivalent. 1=yes,0=no.",0,"Only children triple."),
            new("A double number line can use different numerical scales on the two parallel axes while ticks align as batches. 1=yes,0=no.",1,"Same batch positions but different units/scales."),
            new("The ratio 5 cups flour:3 eggs equals 6 cups flour:4 eggs. 1=yes,0=no.",0,"5×4 differs from6×3."),
            new("The ratio2 yellow:3 red equals4 yellow:6 red. 1=yes,0=no.",1,"Both double.")
        ],
        [L07LineValues] =
        [
            new("One cinnamon-roll batch uses2 tablespoons sugar per1 teaspoon cinnamon. Sugar tbsp for3 batches?",6,"2×3=6 tablespoons."),
            new("One cinnamon-roll batch uses2 tbsp sugar per1 tsp cinnamon. Cinnamon tsp for3 batches?",3,"1×3=3 teaspoons."),
            new("One cinnamon-roll batch uses2 tbsp sugar per1 tsp cinnamon. Sugar tbsp for4 batches?",8,"2×4=8 tablespoons."),
            new("One cinnamon-roll batch uses2 tbsp sugar per1 tsp cinnamon. Cinnamon tsp for4 batches?",4,"4 teaspoons."),
            new("One cinnamon-roll batch uses2 tbsp sugar per1 tsp cinnamon. Sugar tbsp for5 batches?",10,"2×5=10 tablespoons."),
            new("One cinnamon-roll batch uses2 tbsp sugar per1 tsp cinnamon. Cinnamon tsp for5 batches?",5,"5 teaspoons."),
            new("Meatloaf uses2 pounds beef per half cup of bread crumbs. Beef pounds for4 batches?",8,"2×4=8 pounds."),
            new("Meatloaf uses2 pounds beef per half cup bread crumbs. Breadcrumb HALF-CUP UNITS for4 batches?",4,"Four halves make2 actual cups."),
            new("Fruit punch has4 cups pineapple juice and5 cups orange juice. Pineapple cups for3 batches?",12,"4×3=12 cups."),
            new("Fruit punch has4 cups pineapple juice and5 cups orange juice. Orange cups for3 batches?",15,"5×3=15 cups."),
            new("Fruit punch has4 cups pineapple juice and5 cups orange juice. Orange cups for5 batches?",25,"5×5=25 cups."),
            new("Pink paint has2 cups red per7 cups white. Red14 cups means how many batches?",7,"14÷2=7."),
            new("Pink paint has2 cups red per7 cups white. White cups for7 batches?",49,"7×7=49."),
            new("A triple batch of a fruit punch with one-third cup lime per batch uses how many THIRD-CUP portions of lime?",3,"3×1/3 cup=1 cup, or3 third-cup portions."),
            new("Find a ratio equivalent to3:11 scaled by4. What is the first term?",12,"3×4=12."),
            new("Find a ratio equivalent to3:11 scaled by4. What is the second term?",44,"11×4=44.")
        ],
        [L07LineLogic] =
        [
            new("For one cinnamon roll filling, 2 tablespoons sugar PER teaspoon cinnamon means for each teaspoon. 1=yes,0=no.",1,"Per means for each."),
            new("A double number line may use uneven tick spacing while claiming equal consecutive batches. 1=yes,0=no.",0,"Equal batches need equally spaced ticks along each line."),
            new("For cinnamon2 sugar:1 cinnamon, three batches6:3 preserve the amounts per batch. 1=yes,0=no.",1,"Both triple."),
            new("For cinnamon2 sugar:1 cinnamon, three batches6:4 preserve amounts per batch. 1=yes,0=no.",0,"Cinnamon quadrupled."),
            new("For meatloaf2lb beef:1/2cup crumbs, 4lb beef:1cup crumbs is equivalent. 1=yes,0=no.",1,"Both double."),
            new("For meatloaf2lb beef:1/2cup crumbs,4lb beef:2cups crumbs is equivalent. 1=yes,0=no.",0,"Different multipliers."),
            new("Fruit punch4 pineapple:5 orange,12 pineapple:15 orange has the same ratio. 1=yes,0=no.",1,"Both triple."),
            new("Fruit punch4 pineapple:5 orange,12 pineapple:20 orange has the same ratio. 1=yes,0=no.",0,"Scaling ×3 vs×4."),
            new("A label of cups cannot be silently replaced with pounds in a double number line. 1=yes,0=no.",1,"Unit semantics matter."),
            new("14 cups red and49 cups white represent seven 2:7 pink-paint batches. 1=yes,0=no.",1,"Both ×7."),
            new("For a double number line, aligned positions show the same batch count across two labeled scales. 1=yes,0=no.",1,"Parallel lines connect paired quantities."),
            new("In 6pints red:2pints yellow, there are3pints red per1pint yellow. 1=yes,0=no.",1,"6:2=3:1.")
        ],
        [L08UnitPrices] =
        [
            new("Source model: 2oz gold cost2640 dollars. Price for1oz in DOLLARS?",1320,"2640÷2=1320 dollars per ounce."),
            new("Source model: 2oz gold cost2640 dollars. Price for3oz in DOLLARS?",3960,"3×1320=3960 dollars."),
            new("Source model: 2oz gold cost2640 dollars. Price for4oz in DOLLARS?",5280,"4×1320=5280 dollars."),
            new("Tomatoes:4lb cost14 dollars. Price for1lb in CENTS?",350,"1400 cents÷4=350 cents per pound."),
            new("Tomatoes:4lb cost14 dollars. Price for2lb in CENTS?",700,"350×2=700 cents."),
            new("Tomatoes:4lb cost14 dollars. Price for3lb in CENTS?",1050,"350×3=1050 cents."),
            new("Four movie tickets cost48 dollars. Price for1 ticket in DOLLARS?",12,"48÷4=12 dollars per ticket."),
            new("Four movie tickets cost48 dollars. Price for5 tickets in DOLLARS?",60,"12×5=60 dollars."),
            new("Four movie tickets cost48 dollars. Price for11 tickets in DOLLARS?",132,"12×11=132 dollars."),
            new("Twelve eggs cost3 dollars. Price for1 egg in CENTS?",25,"300÷12=25 cents per egg."),
            new("Three pounds of peanuts cost7 dollars50 cents. Price per pound in CENTS?",250,"750÷3=250 cents."),
            new("Four rolls of toilet paper cost2 dollars. Price per roll in CENTS?",50,"200÷4=50 cents."),
            new("Ten apples cost3 dollars50 cents. Price per apple in CENTS?",35,"350÷10=35 cents."),
            new("Clare smoothie uses2 tsp chocolate syrup; doubled recipe requires how many tsp syrup?",4,"Double2 to4 teaspoons, not5."),
            new("Clare smoothie doubled should use4 tsp syrup but Kiran added5 tsp. How many tsp EXTRA used?",1,"5-4=1 tsp extra."),
            new("Trapezoidal stage base20ft/10ft with height12ft. What is the area of one trapezoid in ft squared?",180,"(20+10)×12/2=180."),
            new("Trapezoidal stage horizontal top area180ft², perimeter56ft, vertical depth2ft. Area including top and all side panels, excluding bottom?",292,"180+56×2=292 square feet.")
        ],
        [L08PriceLogic] =
        [
            new("If4 tickets cost48 dollars, a ticket costs12 dollars. 1=yes,0=no.",1,"48÷4=12."),
            new("If4 tickets cost48 dollars, 11 tickets cost120 dollars at the same rate. 1=yes,0=no.",0,"11×12=132 dollars."),
            new("If4lb tomatoes cost14 dollars, 1lb costs3 dollars50 cents. 1=yes,0=no.",1,"1400cents÷4=350."),
            new("If4lb tomatoes cost14 dollars, 3lb cost12 dollars. 1=yes,0=no.",0,"3lb=10.50 dollars."),
            new("If12 eggs cost3 dollars, each egg costs25 cents. 1=yes,0=no.",1,"300÷12=25 cents."),
            new("If12 eggs cost3 dollars, 6 eggs cost2 dollars at the same rate. 1=yes,0=no.",0,"6×25cents=150cents."),
            new("If10 apples cost3 dollars50 cents, one apple costs35 cents. 1=yes,0=no.",1,"350÷10=35."),
            new("A price per item compares price and number of items, with units attached. 1=yes,0=no.",1,"Unit price expresses currency for one item."),
            new("At a constant price, doubling the number of items without increasing cost preserves price per item. 1=yes,0=no.",0,"Cost must scale too."),
            new("Two ounces gold2640 dollars makes 3 ounces3960 dollars at this rate. 1=yes,0=no.",1,"1320 dollars per ounce."),
            new("Doubling 1cup yogurt3tbsp peanut butter2tsp syrup requires2cups6tbsp4tsp. 1=yes,0=no.",1,"All quantities double."),
            new("Source smoother with2cups yogurt6tbsp peanut butter5tsp syrup matches twice the original recipe exactly. 1=yes,0=no.",0,"Syrup should4tsp.")
        ],
        [L09SpeedDistances] =
        [
            new("A scooter travels30 feet in2 seconds. Speed in feet per SECOND?",15,"30÷2=15 feet per second."),
            new("A scooter travels30 feet in2 seconds. Feet traveled after1 second at constant speed?",15,"15×1=15 feet."),
            new("A scooter travels30 feet in2 seconds. Feet after3 seconds?",45,"15×3=45 feet."),
            new("A scooter travels30 feet in2 seconds. Feet after4 seconds?",60,"15×4=60 feet."),
            new("A scooter travels30 feet in2 seconds. Feet after5 seconds?",75,"15×5=75 feet."),
            new("Cargo ship travels150 nautical miles in6 hours. Nautical miles in ONE hour?",25,"150÷6=25 nautical miles per hour."),
            new("Cargo ship travels150 nautical miles in6 hours. Nautical miles in3 hours?",75,"25×3=75 nautical miles."),
            new("Han runs10m in2.7sec. At the same speed, time to run50m in TENTHS OF A SECOND?",135,"5×2.7=13.5 seconds or135 tenths."),
            new("Priya runs10m in2.4sec. At the same speed, time to run50m in TENTHS OF A SECOND?",120,"5×2.4=12 seconds or120 tenths."),
            new("Pasta dough uses150 grams flour per egg. Grams for6 eggs?",900,"150×6=900g."),
            new("Pasta dough uses150 grams flour per egg. Eggs for450g flour?",3,"450÷150=3 eggs."),
            new("Four frozen vegetable bags cost11 dollars96 cents. Price for one bag in CENTS?",299,"1196÷4=299 cents."),
            new("Four frozen vegetable bags cost11 dollars96 cents. Nine bags cost how many CENTS?",2691,"299×9=2691 cents."),
            new("Five cats have4 paws per cat. Total number of PAWS?",20,"5×4=20 paws."),
            new("Five cats have2 ears per cat. Total number of EARS?",10,"5×2=10 ears."),
            new("Five cats have4 paws and2 ears each. How many MORE paws than ears in total?",10,"20-10=10."),
            new("At constant speed2 meters every4 seconds, how many meters in10 seconds?",5,"2/4=1/2m/s;10×1/2=5m."),
            new("At constant speed5m every10s, how many seconds to travel20m?",40,"4×10=40 seconds.")
        ],
        [L09SpeedComparisons] =
        [
            new("Han runs10m in2.7 seconds while Priya runs10m in2.4 seconds. Is Priya faster? 1=yes,0=no.",1,"Same distance in less time means faster."),
            new("Han runs10m in2.7sec while Priya runs10m in2.4sec. Is Han faster? 1=yes,0=no.",0,"Priya takes less time."),
            new("Scooter30ft in2sec versus skateboard55ft in4sec: is scooter faster? 1=yes,0=no.",1,"15ft/s exceeds13.75ft/s."),
            new("Scooter30ft in2sec versus skateboard55ft in4sec: is skateboard faster? 1=yes,0=no.",0,"13.75ft/s below15ft/s."),
            new("A scooter travels30ft in2sec. Can it travel75ft in5sec at constant speed? 1=yes,0=no.",1,"15ft/s×5=75ft."),
            new("A scooter travels30ft in2sec. Can it travel55ft in5sec at constant speed? 1=yes,0=no.",0,"At15ft/s,5sec makes75ft."),
            new("A ship at150 nautical miles per6hours travels25 nautical miles per hour. 1=yes,0=no.",1,"150÷6=25."),
            new("A ship at150 nautical miles per6hours travels30 nautical miles per hour. 1=yes,0=no.",0,"Rate is25."),
            new("On a constant-speed double number line, equal time steps yield equal distance steps. 1=yes,0=no.",1,"Each step repeats the same distance/time ratio."),
            new("Five cats have paws-to-ears ratio20:10=2:1. 1=yes,0=no.",1,"20/10=2."),
            new("The ratios198:1287 and2:13 are equivalent. 1=yes,0=no.",1,"198×13=1287×2."),
            new("At the same distance, the runner taking MORE seconds has greater average speed. 1=yes,0=no.",0,"More time for same distance means lower speed.")
        ],
        [L10CompareAmounts] =
        [
            new("Slug travels3 cm in3 sec. Speed in centimeters per second?",1,"3/3=1cm per second."),
            new("Snail travels6 cm in6 sec. Speed in centimeters per second?",1,"6/6=1cm per second."),
            new("Plane travels3360 miles in6 hours. Speed in miles per hour?",560,"3360/6=560mph."),
            new("Plane travels3360 miles in6 hours. Miles in10 hours at same speed?",5600,"560×10=5600 miles."),
            new("Ground beef costs5 dollars per pound. Price for3 pounds in CENTS?",1500,"3×500=1500cents."),
            new("Ground beef costs5 dollars per pound. Price for one-half pound in CENTS?",250,"500/2=250cents."),
            new("Ground beef costs5 dollars per pound. Price for one-quarter pound in CENTS?",125,"500/4=125cents."),
            new("Ground beef costs5 dollars per pound. Price for three-quarters of a pound in CENTS?",375,"500×3/4=375cents."),
            new("Ground beef costs5 dollars per pound. Price for3¾ pounds in CENTS?",1875,"500×15/4=1875cents."),
            new("Tulips cost11 dollars for5 bulbs at store A. Price per bulb in CENTS?",220,"1100/5=220 cents."),
            new("Tulips cost13 dollars for6 bulbs at store B. Price for12 bulbs at that rate in CENTS?",2600,"Two six-bulb bundles cost26 dollars."),
            new("Purple paint model has5 cups purple and2cups white. For 15cups purple, white cups at same ratio?",6,"Scale both×3:15purple6white."),
            new("Purple paint model has5 cups purple and2cups white. For 3 batches, purple cups?",15,"5×3=15."),
            new("Spice mix6tsp garlic to15tsp salt. Garlic tsp for5tsp salt?",2,"6:15=2:5."),
            new("Spice mix6tsp garlic to15tsp salt. Salt tsp for8tsp garlic?",20,"8×5/2=20."),
            new("Spice mix6tsp garlic to15tsp salt. If total14tsp at same ratio, salt tsp?",10,"2:5 ratio, 14total gives4garlic10salt."),
            new("Spice mix6tsp garlic to15tsp salt. How much MORE salt than garlic in original recipe?",9,"15-6=9tsp."),
            new("Slugs3cm in3sec and snails6cm in6sec: for a10 second period at1cm/sec, distance in cm?",10,"Constant rate1cm/sec×10=10cm.")
        ],
        [L10EquivalentContexts] =
        [
            new("Slug3cm in3sec and snail6cm in6sec travel at the same speed. 1=yes,0=no.",1,"Both1cm per second."),
            new("Snail6cm in6sec is faster than slug3cm in3sec because its distance is larger. 1=yes,0=no.",0,"Rate is the same."),
            new("Milkshake2 chocolate scoops:1cup milk is more chocolate-heavy than3 scoops:2cups milk. 1=yes,0=no.",1,"2 per cup exceeds1.5 per cup."),
            new("Milkshake3 chocolate scoops:2cups milk is more chocolate-heavy than2 scoops:1cup milk. 1=yes,0=no.",0,"3/2<2."),
            new("Purple mix B15 purple:8 white has MORE white per purple than A5 purple:2 white. 1=yes,0=no.",1,"8/15>2/5."),
            new("Purple mix B15 purple:8white preserves A5 purple:2white ratio exactly. 1=yes,0=no.",0,"3×5=15 but3×2=6, not8."),
            new("Store A5 tulips for11dollars and store B6 for13dollars have exactly equal unit prices. 1=yes,0=no.",0,"A=2.20 dollars each, B≈2.17."),
            new("Store B6 tulips for13dollars is cheaper per bulb than A5 for11dollars. 1=yes,0=no.",1,"13/6<11/5."),
            new("Plane3360 miles in6hours moves at560 miles per hour. 1=yes,0=no.",1,"3360/6=560."),
            new("One-quarter pound beef at5 dollars per pound costs one-quarter of5 dollars. 1=yes,0=no.",1,"125cents."),
            new("Spice garlic6:salt15 is equivalent to garlic2:salt5. 1=yes,0=no.",1,"Both÷3."),
            new("With equal travel TIME, the runner covering more DISTANCE has greater average speed. 1=yes,0=no.",1,"Distance per same time comparison.")
        ],
        [L11TableAmounts] =
        [
            new("A car travels70km every hour. Distance in km after14 hours?",980,"70Ã—14=980km."),
            new("A car travels70km every hour. Distance in km after3 hours?",210,"70Ã—3=210km."),
            new("Olives20 pounds make3 liters olive oil. How many liters from3000 pounds?",450,"3000/20Ã—3=450 liters."),
            new("Olives20 pounds make3 liters. How many liters from100 pounds?",15,"100/20Ã—3=15 liters."),
            new("A recess needs2 adults for24 children. Adults needed for72 children?",6,"72/24Ã—2=6."),
            new("A recess needs2 adults for24 children. Children for8 adults?",96,"8/2Ã—24=96."),
            new("A recess needs2 adults for24 children. Children for4 adults?",48,"24Ã—2=48."),
            new("A recess needs2 adults for24 children. Children for6 adults?",72,"24Ã—3=72."),
            new("Jogging at25 beats every10s, beats in60s?",150,"25Ã—6=150beats per minute."),
            new("Six ears corn cost180cents. Price per ear in CENTS?",30,"180/6=30."),
            new("Twelve apples cost288cents. Price per apple in CENTS?",24,"288/12=24."),
            new("Five tomatoes cost310cents. Price per tomato in CENTS?",62,"310/5=62."),
            new("Orange paint source first row5 quarts yellow for6 quarts red. Yellow quarts for3 batches?",15,"5Ã—3=15."),
            new("Orange paint source first row5 quarts yellow for6 quarts red. Red quarts for3 batches?",18,"6Ã—3=18."),
            new("Olive oil20lb for3L. Pounds olives for15L?",100,"15/3Ã—20=100."),
            new("A car goes70km per hour. How many hours to travel210km?",3,"210/70=3hours."),
            new("For2 adults and24 children per group, total people in3 equal groups?",78,"3Ã—(2+24)=78.")
        ],
        [L11TableLogic] =
        [
            new("Car1hour70km and14hours980km are a constant-rate ratio. 1=yes,0=no.",1,"Scale both quantitiesÃ—14."),
            new("Car1hour70km and14hours900km keep the same speed. 1=yes,0=no.",0,"14Ã—70=980km."),
            new("20lb olives3L and100lb olives15L are equivalent. 1=yes,0=no.",1,"BothÃ—5."),
            new("20lb olives3L and100lb olives12L are equivalent. 1=yes,0=no.",0,"Liters should15."),
            new("2 adults24children and6 adults72children are equivalent ratios. 1=yes,0=no.",1,"BothÃ—3."),
            new("2 adults24children and6 adults48children are equivalent. 1=yes,0=no.",0,"AdultsÃ—3 but childrenÃ—2."),
            new("A ratio table can list equivalent pairs in any row order, unlike an ordered number line. 1=yes,0=no.",1,"Rows need not follow geometric ordering."),
            new("Table entries must preserve correct labeled units for both columns. 1=yes,0=no.",1,"Column units give ratios meaning."),
            new("Jogging25 beats in10seconds is150bpm, which is lower than160bpm. 1=yes,0=no.",1,"25Ã—6=150."),
            new("Jogging25 beats in10seconds exceeds160bpm. 1=yes,0=no.",0,"It is150bpm."),
            new("Six corn ears for180cents means30cents per ear. 1=yes,0=no.",1,"Divide180 by6."),
            new("Two ratio table rows can be equivalent if only one column is multiplied by2. 1=yes,0=no.",0,"Both corresponding values scale equally.")
        ],
        [L12TableCalculations] =
        [
            new("Each penny has mass2.5g. Priya collected2400g. Number of pennies?",960,"2400 divided by2.5 equals960 pennies."),
            new("Each penny weighs2.5g. Priya collected2400g. Total money in CENTS?",960,"960 pennies are960 cents."),
            new("Kiran reads5 pages in20 minutes. Minutes for11 pages at same pace?",44,"20 divided by5 is4minutes per page; 11 takes44minutes."),
            new("Kiran reads5 pages in20 minutes. Minutes per page?",4,"20 divided by5 gives4minutes."),
            new("Mai makes4 pizzas with10 ounces of cheese. Ounces per pizza in HALF-OUNCE units?",5,"10/4=2.5oz=5 half-ounce units."),
            new("Mai makes4 pizzas with10 ounces cheese. Ounces for15 pizzas in HALF-OUNCE units?",75,"15×2.5=37.5oz=75 half-ounce units."),
            new("Clare earns90 dollars for5 hours. Dollars per hour?",18,"90 divided by5 gives18dollars/hour."),
            new("Clare earns90 dollars for5 hours. Seconds needed to earn25 cents at same rate?",50,"18dollars/hour is1800 cents/3600 sec=0.5 cents/sec, so25 cents needs50sec."),
            new("Car travels20miles in half hour. Miles per hour?",40,"20 divided by0.5=40mph."),
            new("Car travels30miles in three-quarter hour. Miles per hour?",40,"30 divided by0.75=40mph."),
            new("A fruit mixture has9fl oz cranberry and21fl oz apple. Cranberry fl oz for2 groups of3:7?",6,"One scaled3:7 pair repeats twice to6:14."),
            new("A fruit mixture has9fl oz cranberry and21fl oz apple. Apple fl oz for2 groups of3:7?",14,"2×7=14."),
            new("600:450 simplifies to4:3 by dividing first term by what positive whole number?",150,"600/150=4."),
            new("From a ratio4:3, multiplying both terms by15 gives what FIRST term?",60,"4×15=60."),
            new("Mai uses10oz cheese for4 pizzas. Cheese ounces for8 pizzas?",20,"Double both quantities."),
            new("A table has5pages per20min. Pages read in60minutes?",15,"60/20×5=15pages.")
        ],
        [L12TableStrategies] =
        [
            new("600:450 and60:45 are equivalent ratios. 1=yes,0=no.",1,"Both divide by10."),
            new("60:45 and4:3 are equivalent ratios. 1=yes,0=no.",1,"Both divide by15."),
            new("600:450 and4:3 are equivalent ratios. 1=yes,0=no.",1,"Both divide by150."),
            new("600:450 and60:40 are equivalent ratios. 1=yes,0=no.",0,"Second must45."),
            new("5 pages in20min means reading each page at an average4 minutes. 1=yes,0=no.",1,"20/5=4."),
            new("5 pages in20min means11 pages take40 minutes at same pace. 1=yes,0=no.",0,"11×4=44."),
            new("4 pizzas need10oz cheese, so one pizza uses2.5oz. 1=yes,0=no.",1,"10/4=2.5."),
            new("4 pizzas need10oz cheese, so15 pizzas need30oz. 1=yes,0=no.",0,"15×2.5=37.5."),
            new("20miles in half hour and30miles in three-quarter hour represent same speed. 1=yes,0=no.",1,"Both40mph."),
            new("20miles in half hour and30miles in one full hour represent same speed. 1=yes,0=no.",0,"40 vs30mph."),
            new("A ratio table allows multiplying both quantities by a common nonzero positive factor to reach a new row. 1=yes,0=no.",1,"Same multiplicative relationship."),
            new("Clare earns90 dollars in5hours; she earns25 cents in50sec at that constant rate. 1=yes,0=no.",1,"18 dollars/hour=0.5 cents/sec.")
        ],
        [L13RepresentationValues] =
        [
            new("Bread recipe3 tablespoons olive oil to2 cloves garlic. Oil tablespoons for4 batches?",12,"3 times4=12."),
            new("Bread recipe3 tablespoons oil to2 cloves garlic. Garlic cloves for4 batches?",8,"2 times4=8."),
            new("Bread recipe3 tablespoons oil to2 cloves garlic. Oil tablespoons for2 batches?",6,"3 times2=6."),
            new("Bread recipe3 tablespoons oil to2 cloves garlic. Garlic cloves for5 batches?",10,"2 times5=10."),
            new("Clare travels72 miles in2hours. Miles per hour?",36,"72 divided by2=36mph."),
            new("Clare travels72 miles in2hours. Miles in3hours?",108,"36 times3=108."),
            new("Clare travels72 miles in2hours. Miles in6hours?",216,"36 times6=216."),
            new("Clare travels72 miles in2hours. Miles in13 HALF-HOURS?",234,"13 times18=234 miles in6.5h."),
            new("Clare travels72 miles in2hours. Miles in1hour?",36,"One half of72 is36."),
            new("Lin travels23mi in half hour. Miles in1hour at same rate?",46,"23 times2=46mph."),
            new("Diego travels25mi in half hour. Miles in1hour at same rate?",50,"25 times2=50mph."),
            new("Lin takes65 minutes to travel60miles, Diego70minutes. Who is faster? Enter1 for Lin,2 for Diego.",1,"For same60miles Lin takes fewer minutes."),
            new("Sports drink uses5/3 tbsp powder per12oz water. How many batches with5 tbsp powder?",3,"5 divided by5/3 is3."),
            new("Sports drink uses5/3 tbsp powder per12oz water. How many batches with36oz water?",3,"36/12=3."),
            new("Three tbsp olive oil per two cloves garlic. How many tbsp oil for10 cloves?",15,"10/2 times3=15."),
            new("A car at36mph travels how many miles in HALF an hour?",18,"36/2=18.")
        ],
        [L13RepresentationLogic] =
        [
            new("A ratio table can list equivalent rows in non-increasing order. 1=yes,0=no.",1,"Table row ordering is independent of geometric distance."),
            new("The physical spacing on a double number line encodes differences in the two quantities. 1=yes,0=no.",1,"Both axes map numerical distances."),
            new("A table is always invalid for very small fractions. 1=yes,0=no.",0,"Tables can show fractional values."),
            new("A ratio table is often easier than a physically drawn long number line for very large values. 1=yes,0=no.",1,"Geometry can become impractical."),
            new("3tbsp oil:2 garlic cloves and6tbsp oil:4 cloves preserve recipe ratio. 1=yes,0=no.",1,"Both double."),
            new("3tbsp oil:2 garlic cloves and6tbsp oil:5 cloves preserve ratio. 1=yes,0=no.",0,"Factors differ."),
            new("Clare72miles in2h travels at36 miles per hour. 1=yes,0=no.",1,"72/2=36."),
            new("Lin23mi in30min is faster than Diego25mi in30min. 1=yes,0=no.",0,"Same time, Diego covers more distance."),
            new("For same60miles, Lin65min is faster than Diego70min. 1=yes,0=no.",1,"Less time for same distance."),
            new("A drink with5/3 tablespoons powder and12oz water is exactly3 batches when using5 tbsp and36oz. 1=yes,0=no.",1,"Scale both by3."),
            new("For bread 3 tbsp oil:2 cloves garlic,5 tablespoons oil corresponds to10 cloves at the same rate. 1=yes,0=no.",0,"10 cloves needs15 tbsp."),
            new("A table and a double number line may describe the same equivalent ratio family. 1=yes,0=no.",1,"Both preserve corresponding pair values.")
        ],
        [L14InfoGapNumbers] =
        [
            new("Vinegar costs3 dollars for2 gallons. Cost of1 gallon in CENTS?",150,"300cents divided by2=150cents."),
            new("Vinegar costs3 dollars for2 gallons. Cost of15 gallons in CENTS?",2250,"15 times150cents=2250cents."),
            new("Vinegar costs3 dollars for2 gallons. Cost of30 gallons in CENTS?",4500,"30 times150=4500cents."),
            new("Eight pounds pasta cost12 dollars. Price for1 pound in CENTS?",150,"1200 divided by8=150cents."),
            new("Eight pounds pasta cost12 dollars. Price for21 pounds in CENTS?",3150,"21 times150=3150cents."),
            new("Lin reads20pages in35minutes. At same rate, time for47 pages in QUARTER-MINUTES?",329,"47 times35 divided by20=82.25minutes=329quarter-minute units."),
            new("Lin reads20pages in35minutes. Additional time beyond1hour for47pages, in SECONDS?",1335,"82.25minutes minus60=22.25minutes=1335seconds."),
            new("Diego types140words in4minutes. Words per minute?",35,"140 divided by4=35words/min."),
            new("Diego types140words in4minutes. Minutes for385words?",11,"385/35=11minutes."),
            new("Diego types140words in4minutes. Words in15minutes?",525,"35 times15=525words."),
            new("Train travels30miles in one-third hour. Miles per hour?",90,"30 divided by1/3=90mph."),
            new("Train travels20miles in half hour. Miles per hour?",40,"20 divided by1/2=40mph."),
            new("Triangular-prism net has three rectangle faces length12,10,10 and height4 inches. Total area of rectangles in square inches?",128,"4 times(12+10+10)=128."),
            new("Two congruent triangles each base12in and height8in. Combined triangle area in square inches?",96,"Two triangles each12 times8 divided by2=48, combined96."),
            new("Prism source net combines rectangular area128 and triangular ends96 square inches. Total surface area?",224,"128+96=224square inches."),
            new("A rate of140words per4minutes, words in2minutes?",70,"35 times2=70words.")
        ],
        [L14InfoGapReasoning] =
        [
            new("At a constant cost of3 dollars per2gallons,1gallon costs1.50 dollars. 1=yes,0=no.",1,"Divide both amounts by2."),
            new("At3 dollars per2 gallons,4 gallons cost3 dollars. 1=yes,0=no.",0,"Four gallons cost6 dollars."),
            new("At3 dollars per2 gallons,30 gallons cost45 dollars. 1=yes,0=no.",1,"15 times the source pair."),
            new("At3 dollars per2 gallons,2 dollars buys30 gallons. 1=yes,0=no.",0,"2 dollars buys4/3 gallons."),
            new("At3 dollars per2 gallons,1 dollar buys2/3 gallon. 1=yes,0=no.",1,"Divide both by3."),
            new("For21lb pasta at8lb per12dollars, knowing the unit price helps. 1=yes,0=no.",1,"1.50 dollars per pound."),
            new("Lin47pages at20pages/35minutes will finish within60minutes. 1=yes,0=no.",0,"It takes82.25minutes."),
            new("Diego140words in4minutes has a constant rate35words/minute. 1=yes,0=no.",1,"140/4=35."),
            new("A train30miles in1/3hour is faster than another20miles in1/2hour. 1=yes,0=no.",1,"90mph exceeds40mph."),
            new("An info-gap solver should ask specifically for the missing relevant quantity and explain why. 1=yes,0=no.",1,"Ask precise what/why questions."),
            new("A learner must guess unknown source information rather than ask their partner in an info-gap. 1=yes,0=no.",0,"The activity expects explicit requests."),
            new("For a price-to-weight problem, the unit price can be used to scale the required weight. 1=yes,0=no.",1,"Multiply dollars/pound by pounds.")
        ],
        [L15TapeParts] =
        [
            new("Kennel has cats:dogs=4:5, 27 animals total. Animals per ratio PART?",3,"27 divided by9 parts=3."),
            new("Kennel has cats:dogs=4:5, 27 total. How many cats?",12,"4 parts times3=12."),
            new("Kennel has cats:dogs=4:5, 27 total. How many dogs?",15,"5 parts times3=15."),
            new("Kennel has cats:dogs=4:5, 27 total. Number of ratio parts?",9,"4+5=9."),
            new("Weather has sunny:rainy=4:1, 30 total days. How many rainy days?",6,"30 divided by5parts=6."),
            new("Weather has sunny:rainy=4:1,30 total days. How many sunny days?",24,"4 times6=24."),
            new("Bike ride covers32miles in160minutes. Minutes per mile?",5,"160/32=5min per mile."),
            new("Bike ride covers32miles in160minutes. Minutes for100miles?",500,"100 times5=500minutes."),
            new("Cashier earns58 dollars in8hours. Hourly pay in CENTS?",725,"5800cents/8=725 cents."),
            new("Cashier earns58 dollars in8hours. Earnings for3hours in CENTS?",2175,"725 times3=2175cents."),
            new("Three-pound orange bags cost4 dollars. Price per pound in CENTS rounded to nearest whole cent?",133,"400/3=133.333 cents, rounded 133."),
            new("Eight-pound orange bags cost9 dollars. Price per pound in THOUSANDTHS of a DOLLAR?",1125,"9/8=1.125dollars per pound."),
            new("First-party mixture3 cups yellow and2 cups blue. Total green mixture volume in CUPS under additive liquids?",5,"3+2=5cups."),
            new("First-party mixture3 cups yellow and2 cups blue. Total cups for4 identical batches?",20,"4 times(3+2)=20."),
            new("A ratio3 red to2 yellow has5 total parts. If there are25 total cups, red cups?",15,"25/5 times3=15."),
            new("A ratio3 red to2 yellow has5 total parts. If there are25 total cups, yellow cups?",10,"25/5 times2=10.")
        ],
        [L15TapeLogic] =
        [
            new("For cats:dogs4:5, part-to-whole cats:total is4:9. 1=yes,0=no.",1,"Total parts4+5=9."),
            new("For cats:dogs4:5, part-to-whole dogs:total is5:4. 1=yes,0=no.",0,"Dogs to whole=5:9."),
            new("At cats:dogs4:5 with27 animals, 12 cats and15 dogs fits. 1=yes,0=no.",1,"Both are3 times ratio terms."),
            new("At cats:dogs4:5 with27 animals, 15 cats and12 dogs fits. 1=yes,0=no.",0,"Order is reversed."),
            new("At sunny:rainy4:1 over30 days, there are6 rainy days. 1=yes,0=no.",1,"One part6."),
            new("At sunny:rainy4:1 over30 days, there are10 rainy days. 1=yes,0=no.",0,"Total5parts, each6."),
            new("When two paint liquids have compatible volume units, 3cups yellow+2cups blue models5cups of mixture, assuming additive volumes. 1=yes,0=no.",1,"Sums compatible liquid volumes under model assumptions."),
            new("You may always add measured cups of dry oatmeal to water and claim the same total fluid volume. 1=yes,0=no.",0,"Volume can be nonadditive."),
            new("For a ratio a:b, the whole consists of a+b equal ratio parts in the part-part-whole model. 1=yes,0=no.",1,"Sum ratio units for two categories."),
            new("Four sunny days for every rainy day means rainy:total=1:5. 1=yes,0=no.",1,"Four sunny and one rainy part."),
            new("At32miles per160minutes,100miles takes500minutes at same pace. 1=yes,0=no.",1,"5 minutes/mile."),
            new("A 3-pound orange bag4 dollars has lower cost per pound than an8-pound bag9 dollars. 1=yes,0=no.",0,"4/3=1.333 vs9/8=1.125.")
        ],
        [L16MultistepRatio] =
        [
            new("Piggy bank ratios nickels:dimes=2:3 and dimes:quarters=2:5. Combined smallest integer ratio for NICKELS?",4,"Bring dime ratio to6: nickels4,dimes6,quarters15."),
            new("Piggy bank ratios nickels:dimes=2:3 and dimes:quarters=2:5. Combined smallest ratio for DIMES?",6,"Nickels4:dimes6:quarters15."),
            new("Piggy bank ratios nickels:dimes=2:3 and dimes:quarters=2:5. Combined smallest ratio for QUARTERS?",15,"Nickels4:dimes6:quarters15."),
            new("Piggy bank has500 coins with nickels:dimes:quarters=4:6:15. How many NICKELS?",80,"500 divided by25=20;4×20=80."),
            new("Piggy bank has500 coins with nickels:dimes:quarters=4:6:15. How many DIMES?",120,"6×20=120."),
            new("Piggy bank has500 coins with nickels:dimes:quarters=4:6:15. How many QUARTERS?",300,"15×20=300."),
            new("Piggy bank has80nickels120dimes300quarters. Total VALUE in CENTS?",9100,"80×5+120×10+300×25=9100."),
            new("Horse A gallops32ft/sec and horse B28ft/sec. How many feet farther after5sec?",20,"(32-28)×5=20 feet."),
            new("Horse A gallops32ft/sec. Distance in feet after5sec?",160,"32×5=160feet."),
            new("Horse B gallops28ft/sec. Distance in feet after5sec?",140,"28×5=140feet."),
            new("Andre paid13 dollars for3 books. At the same rate Diego pays how many DOLLARS for12 books?",52,"12/3×13=52 dollars."),
            new("Andre paid13 dollars for3 books. At same rate, how many dollars for6 books?",26,"6/3×13=26."),
            new("Nickels:dimes:quarters=4:6:15 has how many total RATIO PARTS?",25,"4+6+15=25."),
            new("A ratio2:3 scales to 6:9. If total objects are15, how many FIRST category?",6,"2/5 of15=6."),
            new("A rectangular area model needs 3 equal parts width and5 height. Area in square units?",15,"3×5=15."),
            new("Horse A and B rates differ by how many FEET PER SECOND?",4,"32-28=4.")
        ],
        [L16RepresentationChoice] =
        [
            new("Nickels:dimes2:3 and dimes:quarters2:5 together give4:6:15. 1=yes,0=no.",1,"Common dime amount6."),
            new("Nickels:dimes2:3 and dimes:quarters2:5 together give2:3:5. 1=yes,0=no.",0,"The dime count must agree across ratios."),
            new("A bank with nickels80,dimes120,quarters300 has500 total coins. 1=yes,0=no.",1,"80+120+300=500."),
            new("The value of80 nickels,120 dimes and300 quarters is91 dollars. 1=yes,0=no.",1,"400+1200+7500=9100cents."),
            new("32ft/s and28ft/s differ by20ft after5seconds. 1=yes,0=no.",1,"Difference4ft/s times5=20ft."),
            new("If two horses have speeds32 and28 feet/sec, their distance difference shrinks over time. 1=yes,0=no.",0,"It grows4feet each second."),
            new("Books priced3 for13 dollars cost52 dollars for12 at same rate. 1=yes,0=no.",1,"Four groups of3 books cost4×13."),
            new("For a ratio-total question, a tape diagram can help count the number of equal parts. 1=yes,0=no.",1,"Equal blocks depict the whole."),
            new("For constant speeds over time, a double number line can show aligned elapsed time and distance. 1=yes,0=no.",1,"Same time steps align rates."),
            new("One model is required for every ratio problem even when another representation is valid. 1=yes,0=no.",0,"Students may choose a fitting tool."),
            new("The nickels:dimes ratio4:6 simplifies to2:3. 1=yes,0=no.",1,"Divide both by2."),
            new("The dimes:quarters ratio6:15 simplifies to2:5. 1=yes,0=no.",1,"Divide both by3.")
        ],
        [L17FermiAssumptions] =
        [
            new("FIRST-PARTY Fermi assumption: a school has24 classrooms with25 students each. Estimated TOTAL students?",600,"24×25=600 under these explicit assumptions."),
            new("FIRST-PARTY Fermi assumption:600 students use3 pages each daily. Daily pages?",1800,"600×3=1800 pages daily."),
            new("FIRST-PARTY Fermi assumption: a class has25 students, each uses4 pencils in a term. Class pencils?",100,"25×4=100 pencils."),
            new("FIRST-PARTY Fermi assumption:40 classes each use100 pencils per term. School pencils?",4000,"40×100=4000 pencils."),
            new("FIRST-PARTY Fermi assumption: 5 schooldays per week,40 weeks. Total schooldays?",200,"5×40=200 days."),
            new("FIRST-PARTY Fermi assumption:600 students make2 journeys per schoolday over200 schooldays. Total one-way journeys?",240000,"600×2×200=240000."),
            new("FIRST-PARTY Fermi assumption: a bus holds40 riders. Buses needed to carry600 riders in one trip?",15,"600/40=15 buses."),
            new("FIRST-PARTY Fermi assumption: bookshelves each hold30 books, library has45 shelves. Estimated books?",1350,"45×30=1350 books."),
            new("FIRST-PARTY Fermi assumption: 1200 bottles discarded over5 days. Bottles per day under a uniform model?",240,"1200/5=240 bottles/day."),
            new("FIRST-PARTY Fermi assumption: a hallway has80 floor tiles per row and6 rows. Total tiles?",480,"80×6=480 tiles."),
            new("FIRST-PARTY Fermi assumption: school cafeteria serves250 lunches a day for20 days. Monthly lunches?",5000,"250×20=5000 lunches."),
            new("FIRST-PARTY Fermi assumption: 300 pupils each walk2000 meters per day. Total meters walked?",600000,"300×2000=600000 meters.")
        ],
        [L17FermiSensitivity] =
        [
            new("Assuming24 classrooms each contain25 students gives600 students, not an exact census. 1=yes,0=no.",1,"600 is an estimate under assumptions."),
            new("Assuming24 classrooms have25 students each proves every actual class has25 students. 1=yes,0=no.",0,"Model assumptions are not observations."),
            new("If assumed students per class double and number of classes stays fixed, predicted total doubles. 1=yes,0=no.",1,"Multiplicative scaling."),
            new("If number of classes doubles but student count per class stays fixed, predicted total stays unchanged. 1=yes,0=no.",0,"Predicted total doubles."),
            new("Two different plausible assumptions can produce different legitimate Fermi estimates. 1=yes,0=no.",1,"Open-ended estimates depend on premises."),
            new("The exact numeric answer from a fixed-assumptions exercise automatically proves a student's open Fermi research project correct. 1=yes,0=no.",0,"Teacher must assess assumptions, evidence and process."),
            new("Choosing upper and lower bounds is useful for evaluating sensitivity of an estimate. 1=yes,0=no.",1,"Bounds communicate uncertainty."),
            new("A Fermi estimate should always omit units to make arithmetic easier. 1=yes,0=no.",0,"Units are essential."),
            new("Breaking a broad question into smaller measurable questions is appropriate Fermi modeling. 1=yes,0=no.",1,"Subquestions make estimates manageable."),
            new("One can multiply estimated riders per bus by buses to estimate passenger capacity. 1=yes,0=no.",1,"Capacity scales multiplicatively."),
            new("A rough estimate should be presented as an exact observed count solely because arithmetic is correct. 1=yes,0=no.",0,"Assumptions remain uncertain."),
            new("Recording an assumption's source and how changing it affects the result strengthens the estimate. 1=yes,0=no.",1,"Transparent model assumptions support review.")
        ],
        [L0301PerOne] =
        [
            new("Source elevator moves310 feet in10 seconds. Feet per second?",31,"310/10=31 feet per second."),
            new("Source elevator moves310 feet in10 seconds. Feet traveled in12 seconds at constant speed?",372,"31×12=372 feet."),
            new("Source elevator moves310 feet in10 seconds. Feet traveled in5 seconds?",155,"31×5=155 feet."),
            new("Han earns33 dollars for4 hours babysitting. Hourly pay in CENTS?",825,"3300/4=825 cents."),
            new("Han earns33 dollars for4 hours babysitting. Pay for7 hours in CENTS?",5775,"7×825=5775 cents."),
            new("Han earns33 dollars for4 hours babysitting. Pay for8 hours in DOLLARS?",66,"Two batches of4 hours."),
            new("Five dog-food cans cost435 cents. Price for one can in CENTS?",87,"435/5=87 cents."),
            new("Five dog-food cans cost435 cents. Cost of11 cans in CENTS?",957,"87×11=957 cents."),
            new("A restaurant clears4 tables in10minutes. Minutes to clear26 tables at same rate?",65,"26×10/4=65 minutes."),
            new("A restaurant clears4 tables in10minutes. How many tables in30minutes at this rate?",12,"4×3=12 tables."),
            new("Sandwich uses4 ounces meat and3 ounces cheese. Combined ounces per sandwich?",7,"4+3=7 ounces."),
            new("Sandwich shop used91 total ounces meat and cheese at7oz per sandwich. How many sandwiches?",13,"91/7=13."),
            new("Sandwich shop made13 sandwiches using4 ounces meat each. Meat used in ounces?",52,"13×4=52."),
            new("Sandwich shop made13 sandwiches using3 ounces cheese each. Cheese used in ounces?",39,"13×3=39."),
            new("The modeled building climb is828meters in6hours. Meters per hour?",138,"828/6=138 m/h."),
            new("At modeled138meters/hour, meters climbed in2 hours?",276,"138×2=276m."),
            new("At modeled138meters/hour, meters climbed in5 hours?",690,"138×5=690m."),
            new("Window washing:15 windows in18minutes. Minutes for10 windows at same active pace?",12,"18/15×10=12min."),
            new("Window washing:15 windows per18minutes. Total active washing time for24348 windows in TENTHS OF MINUTES?",292176,"24348×18/15=29217.6 min or292176 tenths.")
        ],
        [L0301RateMeaning] =
        [
            new("An elevator going310feet in10seconds has a modeled constant speed31feet per second. 1=yes,0=no.",1,"310÷10=31."),
            new("An elevator going310feet in10seconds covers372feet in12seconds at the same rate. 1=yes,0=no.",1,"31×12=372."),
            new("Han's pay33dollars in4hours predicts7hours pays57.75dollars at constant rate. 1=yes,0=no.",1,"8.25dollars/hour×7."),
            new("Han's pay33dollars in4hours means11dollars per hour. 1=yes,0=no.",0,"Correct rate8.25."),
            new("Five cans for435cents means 11 cans cost957cents. 1=yes,0=no.",1,"87cents per can."),
            new("Five cans for435cents means 11 cans cost870cents. 1=yes,0=no.",0,"11×87=957cents."),
            new("Four tables in10minutes means26 tables require65minutes at same pace. 1=yes,0=no.",1,"2.5 minutes per table."),
            new("One sandwich with4oz meat and3oz cheese uses7oz total. 1=yes,0=no.",1,"4+3=7oz."),
            new("A modeling assumption that window washing remains constant guarantees real workers never take breaks. 1=yes,0=no.",0,"A simplified prediction cannot guarantee conditions."),
            new("Reciprocal units minutes/window and windows/minute describe related but different rate questions. 1=yes,0=no.",1,"Swap numerator and denominator."),
            new("A mathematical prediction of a building's window-washing time proves the current real window count. 1=yes,0=no.",0,"The window count and rate are historical classroom assumptions."),
            new("A per-one rate is expressed using named units, such as feet per second or dollars per hour. 1=yes,0=no.",1,"Units determine the interpretation.")
        ],
        [L0302UnitMagnitudes] =
        [
            new("A 2-yard piece of string equals how many FEET?",6,"2 yards×3feet/yard=6 feet."),
            new("A 2-yard piece of string equals how many INCHES?",72,"2×36inches=72 inches."),
            new("One yard of string equals how many INCHES?",36,"3 feet×12inches/foot=36."),
            new("One meter equals how many CENTIMETERS?",100,"One meter=100cm."),
            new("One liter equals how many MILLILITERS?",1000,"One liter=1000mL."),
            new("A 200mL volume is what whole-number FIFTH of one liter? Enter numerator when denominator5.",1,"200/1000=1/5."),
            new("A recipe uses5 cups apple juice and2 cups sparkling water per7 total cups. For35 cups total, APPLE cups?",25,"35/7×5=25."),
            new("A recipe uses5 cups apple juice and2 cups sparkling water per7 total cups. For35 cups total, SPARKLING cups?",10,"35/7×2=10."),
            new("Three hats cost2250 cents. Cost per hat in CENTS?",750,"2250/3=750cents."),
            new("Three hats cost2250 cents. Number of hats purchasable with6000 cents at same price?",8,"6000/750=8."),
            new("Light travels180 million km in10minutes in model. Millions of km per MINUTE?",18,"180/10=18 million."),
            new("Light travels180 million km in10minutes in model. Kilometers per SECOND?",300000,"180000000/600=300000 km/s."),
            new("A yard equals3feet. A string of4yards is how many feet?",12,"4×3=12."),
            new("A string is72inches long. How many yards?",2,"72/36=2 yards."),
            new("A milliliter is one THOUSANDTH of a liter. How many milliliters in3liters?",3000,"3×1000=3000mL."),
            new("Source benchmark: two meters of string correspond to how many CENTIMETERS?",200,"2×100=200cm.")
        ],
        [L0302UnitCategories] =
        [
            new("A centimeter is a unit of length, not of volume. 1=yes,0=no.",1,"cm measures length."),
            new("A square centimeter is an appropriate unit of linear length. 1=yes,0=no.",0,"Square cm measures area."),
            new("A cubic centimeter measures volume. 1=yes,0=no.",1,"Three-dimensional extent."),
            new("A kilogram is an appropriate unit of mass for a hippopotamus. 1=yes,0=no.",1,"Large animal mass."),
            new("A millimeter is larger than a meter. 1=yes,0=no.",0,"One meter=1000mm."),
            new("A kilometer is larger than a meter. 1=yes,0=no.",1,"1km=1000m."),
            new("A yard is longer than a foot. 1=yes,0=no.",1,"1yard=3feet."),
            new("A quart is smaller than a cup in customary volume measurement. 1=yes,0=no.",0,"1quart=4cups."),
            new("A pound is larger than an ounce in customary weight measurement. 1=yes,0=no.",1,"1pound=16ounces."),
            new("A liter is larger than a milliliter. 1=yes,0=no.",1,"1liter=1000mL."),
            new("A gram is larger than a kilogram. 1=yes,0=no.",0,"1kg=1000g."),
            new("It is sensible to measure a large lake volume in cubic kilometers. 1=yes,0=no.",1,"Large-scale volume."),
            new("It is sensible to measure a spaghetti noodle length in centimeters. 1=yes,0=no.",1,"Small length."),
            new("A tissue box surface area can be measured in square inches. 1=yes,0=no.",1,"Surface area uses square units.")
        ],
        [L0303SourceAmounts] =
        [
            new("A height of57 inches is approximately how many CENTIMETERS rounded to nearest whole centimeter?",145,"57×2.54=144.78cm≈145cm."),
            new("The original multiple-choice height57in in cm gives options A22.4,B57,C144.8,D3551. Enter the correct OPTION NUMBER 1–4.",3,"C is144.8cm≈57×2.54."),
            new("A pot holding20 US quarts is about how many LITERS rounded to nearest whole liter?",19,"20×0.94635≈18.93L≈19L."),
            new("The original pot20quarts choices A7.57,B19,C21,D75.7 liters. Enter correct OPTION NUMBER 1–4.",2,"B≈19L."),
            new("An object weighing4.5 pounds is about how many kilograms rounded to nearest whole kg?",2,"4.5×0.4536≈2.04kg, about2kg."),
            new("Original choice4.5lb in kg offers A2.04,B4.5,C9.92,D4500. Enter correct OPTION NUMBER 1–4.",1,"A≈2.04kg."),
            new("Noah buys15 baseball cards for900cents. Cost per card in CENTS?",60,"900÷15=60 cents."),
            new("Noah buys15 baseball cards for900cents. Cost of30 cards in CENTS?",1800,"30×60=1800cents."),
            new("Noah buys15 baseball cards for900cents. Cost of12 cards in CENTS?",720,"12×60=720cents."),
            new("Jada travels135miles in3hours. Speed in miles per hour?",45,"135÷3=45mph."),
            new("Andre travels228miles in6hours. Speed in miles per hour?",38,"228÷6=38mph."),
            new("Jada45mph and Andre38mph. Enter1 for Jada faster,2 for Andre faster.",1,"45>38mph."),
            new("Classroom length11 meters: centimeters in that length?",1100,"11×100=1100 centimeters."),
            new("One yard is3 feet. A6-foot rope is how many yards?",2,"6÷3=2 yards."),
            new("If a room is11meters long, will its yard measure be MORE than11? Enter1=yes,0=no.",1,"A yard is shorter than a meter, so it takes more yards."),
            new("A box with a geometric volume of3 liters is how many MILLILITERS?",3000,"3×1000=3000mL.")
        ],
        [L0303AttributeClassification] =
        [
            new("Identify the attribute for centimeters across a handprint. Codes1=length,2=area,3=volume,4=mass. Enter code.",1,"Centimeters measure length."),
            new("Identify attribute for square inches needed to wrap a box. Codes1=length,2=area,3=volume,4=mass.",2,"Square inches measure surface area."),
            new("Identify attribute for gallons in a fish tank. Codes1=length,2=area,3=volume,4=mass.",3,"Gallons measure volume/capacity."),
            new("Identify attribute for pounds in a bag of potatoes. Codes1=length,2=area,3=volume,4=mass.",4,"Pounds measure weight."),
            new("Identify attribute for feet across a swimming pool. Codes1=length,2=area,3=volume,4=mass.",1,"Feet measure length."),
            new("Identify attribute for ounces of grapes as weight. Codes1=length,2=area,3=volume,4=mass.",4,"Ounces as labeled weight measure mass/weight."),
            new("Identify attribute for liters in a punch bowl. Codes1=length,2=area,3=volume,4=mass.",3,"Liters measure volume."),
            new("Identify attribute for square feet of grass in a lawn. Codes1=length,2=area,3=volume,4=mass.",2,"Square feet measure area."),
            new("Identify attribute for cubic centimeters inside a box. Codes1=length,2=area,3=volume,4=mass.",3,"Cubic centimeters measure volume."),
            new("Identify attribute for kilograms of animal mass. Codes1=length,2=area,3=volume,4=mass.",4,"Kilograms measure mass."),
            new("Identify attribute for meters of string. Codes1=length,2=area,3=volume,4=mass.",1,"Meters measure length."),
            new("Identify attribute for square kilometers of land. Codes1=length,2=area,3=volume,4=mass.",2,"Square kilometers measure area.")
        ],
        [L0304Conversions] =
        [
            new("Currency example:4250pesos for250dollars. Pesos per dollar?",17,"4250/250=17pesos per dollar."),
            new("Currency example:4250pesos for250dollars. Sweater510pesos costs how many DOLLARS?",30,"510/17=30dollars."),
            new("Currency example:4250pesos for250dollars. Pesos for3 dollars?",51,"3×17=51pesos."),
            new("One gallon is approximated as3785milliliters. How many mL in3gallons?",11355,"3785×3=11355mL."),
            new("One gallon contains4quarts. How many quarts in3gallons?",12,"4×3=12quarts."),
            new("One gallon approx3785mL. About how many mL in1quart rounded to WHOLE mL?",946,"3785/4=946.25mL, rounded946."),
            new("Six quarts makes how many HALF-GALLONS?",3,"6 quarts=1.5 gallons=3 half-gallons."),
            new("One kilogram weighs approximately35ounces. A28ounce bat weighs how many TENTHS of kilogram?",8,"28/35=0.8kg=8 tenths."),
            new("One kilogram is approximately35ounces. A28ounce bat weighs about how many GRAMS?",800,"0.8kg=800grams under stated approximation."),
            new("Trail mix7oz almonds5oz raisins. Ounces total in one batch?",12,"7+5=12."),
            new("Trail mix7 almonds5 raisins for12 total. In16oz mix, almond quantity in THIRD-OUNCE UNITS?",28,"16×7/12=28/3oz, or28 third-ounce units."),
            new("An ant moves980inches in5minutes. Distance in inches in1minute?",196,"980/5=196inches."),
            new("An ant moves980inches in5minutes. Distance in inches in7minutes?",1372,"196×7=1372inches."),
            new("A quarter cup contains4tablespoons. How many TABLESPOONS in1cup?",16,"4quarters×4tablespoons=16."),
            new("Two cups make1pint. How many TABLESPOONS per PINT?",32,"16×2=32."),
            new("One gallon contains4quarts. How many quarts in6 gallons?",24,"4×6=24quarts.")
        ],
        [L0304DirectionChecks] =
        [
            new("To convert6 quarts to gallons, divide by4. 1=yes,0=no.",1,"6/4=1.5gal."),
            new("To convert6 quarts to gallons, multiply by4 giving24gallons. 1=yes,0=no.",0,"Quarts are smaller than gallons."),
            new("One gallon contains4quarts;3gallons contains12quarts. 1=yes,0=no.",1,"Multiply3×4."),
            new("One quart is approximately946.25mL when one gallon is approximated as3785mL. 1=yes,0=no.",1,"3785/4."),
            new("One kg≈35ounces;28ounces≈0.8kg. 1=yes,0=no.",1,"28/35=0.8."),
            new("One kg≈35ounces;28ounces≈35kg. 1=yes,0=no.",0,"Wrong direction/size."),
            new("A mile is a length unit and a cup is a volume unit. 1=yes,0=no.",1,"Different measurement attributes."),
            new("A gram is a volume unit equivalent to a milliliter for every material. 1=yes,0=no.",0,"Mass/volume depend on density."),
            new("Converting a distance from feet to centimeters changes the physical distance. 1=yes,0=no.",0,"Only the numerical representation changes."),
            new("An ant's980 inches in5 minutes implies1372 inches in7 minutes at constant speed. 1=yes,0=no.",1,"Both quantities scale by7/5."),
            new("A currency rate17 pesos per dollar means510pesos is30dollars. 1=yes,0=no.",1,"510/17=30."),
            new("For trail mix7oz almonds5oz raisins, a16oz bag needs16oz almonds to retain composition. 1=yes,0=no.",0,"Almonds are7/12 of total."),
            new("There are16tablespoons per cup and2cups per pint, so32tablespoons per pint. 1=yes,0=no.",1,"16×2=32."),
            new("A ratio table can help carry measurement units through cross-system conversions. 1=yes,0=no.",1,"Matching labels prevent reciprocal errors.")
        ],
        [L0305Rates] =
        [
            new("Mai scoots15 meters in6 seconds. Speed in TENTHS of a meter per second?",25,"15/6=2.5m/s=25tenths."),
            new("Priya scoots22 meters in10 seconds. Speed in TENTHS of meter per second?",22,"22/10=2.2m/s=22tenths."),
            new("Mai travels15m in6s and Priya22m in10s. Enter1 for Mai faster,2 for Priya faster.",1,"Mai2.5m/s exceeds Priya2.2m/s."),
            new("Store X sells4 same-size juice cans for248cents. Unit price in CENTS?",62,"248/4=62."),
            new("Store Y sells5 same-size juice cans for300cents. Unit price in CENTS?",60,"300/5=60."),
            new("Store Z sells one juice can for59cents. Unit price in CENTS?",59,"Given price59."),
            new("Stores X62c/can,Y60c/can,Z59c/can. Enter1 for X,2 for Y,3 for Z as cheapest.",3,"Z at59cents."),
            new("A 450-square-foot apartment costs540000 dollars. Dollars per SQUARE FOOT?",1200,"540000/450=1200 dollars/sqft."),
            new("A 2100-square-foot home costs110 dollars per square foot. Total cost in DOLLARS?",231000,"2100×110=231000 dollars."),
            new("A liter contains33.8 US fluid ounces and a gallon128 US fluid ounces. Approximately how many WHOLE liters per gallon?",4,"128/33.8≈3.787≈4 liters."),
            new("A unit rate comparison has128 fl oz per gallon and33.8 fl oz per liter. Which listed number (2,3,4,5) estimates liters/gallon best?",4,"128/33.8≈3.79, nearest4."),
            new("Diego is165 centimeters tall. Andre is1.7meters tall. Enter1 for Diego taller,2 for Andre taller.",2,"1.7m=170cm, Andre taller."),
            new("A 1.7-meter height is how many CENTIMETERS?",170,"1.7×100=170cm."),
            new("Two scooter rates15m/6s and22m/10s: difference in TENTHS m/s?",3,"2.5-2.2=0.3m/s=3tenths."),
            new("Store X unit price62c/can and Store Z59c/can. Savings in CENTS per can from choosing Z?",3,"62-59=3cents."),
            new("A home with1200 dollars/sqft for450sqft has total price in DOLLARS?",540000,"1200×450=540000 dollars."),
            new("A unit rate of 110dollars/sqft applies to100sqft. Cost in DOLLARS?",11000,"110×100=11000 dollars.")
        ],
        [L0305Comparisons] =
        [
            new("Mai15m in6s has faster speed than Priya22m in10s. 1=yes,0=no.",1,"2.5m/s >2.2m/s."),
            new("Mai15m in6s is slower than Priya22m in10s because Mai covered less distance. 1=yes,0=no.",0,"Time durations differ; compare per second."),
            new("Store X4cans for248cents means62cents per can. 1=yes,0=no.",1,"248/4=62."),
            new("Store Y5cans for300cents means59cents per can. 1=yes,0=no.",0,"300/5=60."),
            new("Store Z at59cents per can is cheaper than Y at60cents. 1=yes,0=no.",1,"59<60."),
            new("An apartment costing540000 dollars for450sqft has unit price1200 dollars/sqft. 1=yes,0=no.",1,"540000/450=1200."),
            new("A 2100sqft home at110 dollars/sqft costs231000 dollars. 1=yes,0=no.",1,"2100×110=231000."),
            new("Given gallon128fl oz and liter33.8fl oz, rounding to4liters slightly OVERestimates actual liters per gallon. 1=yes,0=no.",1,"128/33.8≈3.787."),
            new("One meter equals100 centimeters, so1.7m is170cm. 1=yes,0=no.",1,"1.7×100=170."),
            new("165cm is taller than1.7m. 1=yes,0=no.",0,"1.7m=170cm."),
            new("Comparing prices of packages of different sizes is best done after finding per-item price. 1=yes,0=no.",1,"Common unit price."),
            new("When comparing two moving rates, distance alone proves who is faster even when times differ. 1=yes,0=no.",0,"Use distance/time for each."),
            new("A price per square foot must include dollars and square-foot units. 1=yes,0=no.",1,"Units explain the quotient.")
        ],
        [L0306Rates] =
        [
            new("Raspberries cost12 dollars for3 pints. Cost per pint in DOLLARS?",4,"12/3=4dollars/pint."),
            new("Raspberries cost12 dollars for3 pints. Number of QUARTER-PINT portions per dollar?",1,"3/12=1/4pint per dollar, one quarter-pint portion."),
            new("Raspberries cost12 dollars for3 pints. Pints purchasable with20 dollars?",5,"20/4=5pints."),
            new("Raspberries cost12 dollars for3 pints. Cost for8 pints in DOLLARS?",32,"8×4=32."),
            new("Pink paint uses4 cups WHITE for3 cups RED. White cups for6 cups red?",8,"Double both."),
            new("Pink paint4 white:3 red. Red cups for8 white?",6,"Double both."),
            new("Pink paint4 white:3 red. For1 cup WHITE, red amount in QUARTER-CUP portions?",3,"3/4cup is3 quarter-cup units."),
            new("Pink paint4 white:3 red. For1 cup RED, white amount in THIRD-CUP portions?",4,"4/3cups white is4 third-cup units."),
            new("Polenta uses5 cups water for2 cups cornmeal. For3 cups water, cornmeal in FIFTH-CUP portions?",6,"3×2/5=6/5cups, six fifth-cup portions."),
            new("Polenta uses5 cups water for2 cups cornmeal. For3 cups cornmeal, water in HALF-CUP portions?",15,"3×5/2=7.5cups, fifteen half-cup portions."),
            new("Paint covers350square feet per gallon. Gallons for1750square feet?",5,"1750/350=5gallons."),
            new("Andre types208 words in4 minutes. Words per minute?",52,"208/4=52."),
            new("Noah types342 words in6 minutes. Words per minute?",57,"342/6=57."),
            new("Andre52words/min and Noah57words/min. Enter1 for Andre faster,2 for Noah faster.",2,"Noah types5words/minute more."),
            new("Eight ears corn cost256 cents. Cost per ear in CENTS?",32,"256/8=32cents."),
            new("Twelve ears corn cost432cents. Cost per ear in CENTS?",36,"432/12=36cents."),
            new("Which corn package is lower cost per ear? Enter1=8ears256cents,2=12ears432cents.",1,"32cents is less than36cents."),
            new("100meters is approximately109yards. Which original choice A33.3 B91 C100 D109 is nearest? Enter OPTION NUMBER.",4,"D≈109yards.")
        ],
        [L0306InterpretRate] =
        [
            new("At3pints for12 dollars, the price is4 dollars per pint. 1=yes,0=no.",1,"12/3=4."),
            new("At3pints for12 dollars, each dollar buys4 pints. 1=yes,0=no.",0,"One dollar buys1/4pint."),
            new("The reciprocal unit rate of4 dollars/pint is1/4 pint per dollar. 1=yes,0=no.",1,"Reciprocal when both quantities positive."),
            new("At3pints for12 dollars,20 dollars buys5 pints. 1=yes,0=no.",1,"20/4=5."),
            new("For polenta5 water:2 cornmeal,3cups water needs1.2cups cornmeal. 1=yes,0=no.",1,"3×2/5=1.2."),
            new("For polenta5 water:2 cornmeal,3cups cornmeal needs7.5cups water. 1=yes,0=no.",1,"3×5/2=7.5."),
            new("For paint350sqft per gallon,1750sqft requires5gallons. 1=yes,0=no.",1,"1750/350=5."),
            new("For paint350sqft per gallon, each square foot uses350 gallons. 1=yes,0=no.",0,"Each square foot uses1/350 gallon."),
            new("Typing Andre208/4 gives52wpm and Noah342/6 gives57wpm. Noah is faster. 1=yes,0=no.",1,"57>52."),
            new("Corn8 for256cents is a better unit price than12 for432cents. 1=yes,0=no.",1,"32cents per ear <36."),
            new("A rate a/b answers amount of first quantity per one second quantity. 1=yes,0=no.",1,"Units determine interpretation."),
            new("The reciprocal b/a gives a different unit interpretation of the same nonzero quantity relationship. 1=yes,0=no.",1,"E.g. dollars/pint and pints/dollar."),
            new("A rate per one should be reported without units because only the numeral matters. 1=yes,0=no.",0,"Units are essential.")
        ],
        [L0307UnitRateValues] =
        [
            new("A car travels55 miles per hour. Distance in miles after2 hours?",110,"55×2=110 miles."),
            new("A car travels55mph. Distance in HALF-MILE UNITS after half an hour?",55,"0.5h×55=27.5miles=55 half-miles."),
            new("A car travels55mph. Distance in HALF-MILE UNITS after1.5 hours?",165,"1.5×55=82.5miles=165 half-miles."),
            new("Salsa recipe:2oz onions for16oz tomatoes. Tomatoes ounces per1 onion ounce?",8,"16/2=8."),
            new("Salsa recipe:6oz onions for48oz tomatoes. Tomatoes per onion ounce?",8,"48/6=8."),
            new("Ten burritos for12 dollars. Price per burrito in CENTS?",120,"1200/10=120cents."),
            new("Six burritos for7.50 dollars. Price per burrito in CENTS?",125,"750/6=125cents."),
            new("Party needs60 burritos. Cost of6 ten-burrito specials in DOLLARS?",72,"6×12=72 dollars."),
            new("Party needs60 burritos. Cost of10 six-burrito specials in DOLLARS?",75,"10×7.50=75 dollars."),
            new("A banana costs50 cents. Price for4 bananas in CENTS?",200,"50×4=200 cents."),
            new("A banana costs50 cents. Price for6 bananas in CENTS?",300,"50×6=300 cents."),
            new("A banana costs50 cents. Price for7 bananas in CENTS?",350,"50×7=350 cents."),
            new("A banana costs50 cents. Price for10 bananas in CENTS?",500,"50×10=500 cents."),
            new("Bananas cost50 cents each. Number purchasable for10 dollars?",20,"1000cents/50=20 bananas."),
            new("Bananas cost50 cents each. Number purchasable for16.50 dollars?",33,"1650cents/50=33."),
            new("Plane A travels2800 miles in5 hours. Miles per hour?",560,"2800/5=560."),
            new("Plane B travels3885 miles in7 hours. Miles per hour?",555,"3885/7=555."),
            new("A vehicle has15 gallons gas at35 miles/gallon. Total range in miles?",525,"15×35=525 miles."),
            new("Gas used for100 miles at35miles/gallon, expressed in SEVENTHS OF A GALLON: how many sevenths?",20,"100/35=20/7 gallons."),
            new("Cereal weighs600grams,1kg≈2.2lb. Weight in HUNDREDTHS OF A POUND?",132,"600g=0.6kg≈1.32lb=132hundredths.")
        ],
        [L0307RateInvariance] =
        [
            new("Onions2oz tomatoes16oz and onions6oz tomatoes48oz have the same tomatoes per onion ounce. 1=yes,0=no.",1,"Both8 tomato ounces per1 onion ounce."),
            new("Onions2oz tomatoes16oz and onions6oz tomatoes40oz have the same rate. 1=yes,0=no.",0,"40/6≠8."),
            new("Car55miles per hour travels110miles in2hours. 1=yes,0=no.",1,"Both same unit rate55mph."),
            new("Car55mph travels100miles in2hours at that rate. 1=yes,0=no.",0,"It travels110."),
            new("10 burritos/$12 has a lower price per burrito than6 burritos/$7.50. 1=yes,0=no.",1,"1.20<1.25 per burrito."),
            new("Six orders of10 burritos for12dollars each cost less than ten orders of6 for7.50dollars each. 1=yes,0=no.",1,"72<75dollars."),
            new("A banana at50cents each,4 for200cents and7 for350cents are equivalent ratios. 1=yes,0=no.",1,"Same50cents per banana."),
            new("A banana at50cents each,4 for200cents and7 for300cents are equivalent ratios. 1=yes,0=no.",0,"300/7 differs from50."),
            new("Plane A2800mi/5h=560mph and plane B3885mi/7h=555mph, so A is faster. 1=yes,0=no.",1,"560>555."),
            new("Plane B3885mi/7h is faster than A2800mi/5h because total distance is longer. 1=yes,0=no.",0,"Compare speed not total distance."),
            new("35miles per gallon and1/35 gallon per mile are reciprocal rates of a modeled constant fuel efficiency. 1=yes,0=no.",1,"Units reverse."),
            new("At35miles/gallon,15gallons is525 miles of modeled range. 1=yes,0=no.",1,"15×35=525."),
            new("If two pairs represent equivalent ratios, their unit rates must match for the same quantity order and units. 1=yes,0=no.",1,"Equal quotient."),
            new("A table's unit-rate column should change if quantities scale by the same positive factor. 1=yes,0=no.",0,"Unit rate is invariant.")
        ],
        [L0308SpeedAmounts] =
        [
            new("Kangaroo hops2km in3minutes. Time for5km at constant speed in SECONDS?",450,"5×3/2=7.5min=450seconds."),
            new("Kangaroo hops2km in3minutes. Distance in2minutes in THIRD-KILOMETER units?",4,"2×(2/3)=4/3km, four thirds."),
            new("Kangaroo hops2km in3minutes. Time in MINUTES to travel4km?",6,"Twice the distance, twice the time."),
            new("Mai runs4 laps of400m track. Total distance in METERS?",1600,"4×400=1600m."),
            new("Mai runs250meters/minute. Time for4 laps totaling1600meters, in TENTHS OF MINUTE?",64,"1600/250=6.4minutes=64tenths."),
            new("Mai runs250m/min. Time for4 laps of400m each in SECONDS?",384,"1600/250×60=384seconds."),
            new("Han runs at12min/mile. Distance in HALF-MILE units after30minutes?",5,"30/12=2.5miles=five half-miles."),
            new("Han runs at12min/mile. Distance in MILES after60minutes?",5,"60/12=5miles."),
            new("Tyler runs at15min/mile. Distance in MILES after60minutes?",4,"60/15=4miles."),
            new("Han and Tyler start opposite ends of10miles: Han5mi and Tyler4mi after one hour. Gap in MILES after1hour?",1,"10-(5+4)=1mile."),
            new("Han12min/mile and Tyler15min/mile start10mi apart opposite. Time to meet in SECONDS assuming constant speeds?",4000,"Their closing rate=1/12+1/15=3/20mi/min,10/(3/20)=200/3min=4000sec."),
            new("Skateboarder A moves15ft/s, B22ft/s. Speed DIFFERENCE feet per second?",7,"22-15=7ft/s."),
            new("Skateboarder B22ft/s vs A15ft/s over4minutes. Distance difference in FEET?",1680,"4min=240s;7×240=1680ft."),
            new("Four tablespoons equal1/4cup. Tablespoons in1 cup?",16,"4×4=16tbsp."),
            new("Four tbsp equal1/4cup,2cups per pint. Tablespoons in1PINT?",32,"16×2=32tbsp."),
            new("Cube A side2 units, cube B side4 units. Surface area of A in SQUARE UNITS?",24,"6×2²=24."),
            new("Cube B side4 units. Surface area in SQUARE UNITS?",96,"6×4²=96."),
            new("Cube A side2 units, cube B side4 units. B-to-A volume multiplication FACTOR?",8,"4³/2³=64/8=8.")
        ],
        [L0308PaceMeaning] =
        [
            new("A kangaroo hopping2km in3min needs7.5min for5km at constant speed. 1=yes,0=no.",1,"Multiply3min by5/2."),
            new("A kangaroo hopping2km in3min covers5km in5min at same speed. 1=yes,0=no.",0,"It takes7.5min."),
            new("Mai at250m/min completes4×400m laps in6.4min. 1=yes,0=no.",1,"1600/250=6.4min."),
            new("Han12min/mile runs faster than Tyler15min/mile. 1=yes,0=no.",1,"Fewer minutes per mile means faster."),
            new("Han12min/mile and Tyler15min/mile run toward each other10mi apart, they meet within1hour. 1=yes,0=no.",0,"They cover5+4=9miles in an hour."),
            new("Han12min/mile and Tyler15min/mile cover a combined9miles in1hour. 1=yes,0=no.",1,"5+4=9."),
            new("Two skateboarders22ft/s and15ft/s differ by1680feet after4minutes. 1=yes,0=no.",1,"240sec×7ft/s."),
            new("A 4minute period contains4 seconds. 1=yes,0=no.",0,"Four minutes=240 seconds."),
            new("A cube of side4 has double the surface area of cube side2. 1=yes,0=no.",0,"Surface area scales by4."),
            new("A cube of side4 has eight times the volume of a cube side2. 1=yes,0=no.",1,"Volume scales by2 cubed=8."),
            new("A pace measured in minutes per mile is the reciprocal of speed in miles per minute for a constant positive rate. 1=yes,0=no.",1,"Reciprocal quantities/units."),
            new("Time to travel a fixed distance at constant speed halves if speed doubles. 1=yes,0=no.",1,"time=distance/speed."),
            new("On a constant-speed table, doubling time doubles distance traveled from the same origin. 1=yes,0=no.",1,"Proportional movement.")
        ],
        [L0309RateValues] =
        [
            new("Copier prints480 pages in4minutes. Copies per MINUTE?",120,"480/4=120 copies/minute."),
            new("Copier prints480 pages in4minutes. Copies in10minutes?",1200,"120×10=1200copies."),
            new("Copier prints480 pages in4minutes. Minutes to print720copies?",6,"720/120=6minutes."),
            new("School bus mass9tons. Pounds assuming2000lb per ton?",18000,"9×2000=18000lbs."),
            new("Elephant weighs5500kilograms. Approx pounds using2.2lbs/kg?",12100,"5500×2.2=12100lbs."),
            new("Grand piano weighs15840ounces. Pounds given16ounces/lb?",990,"15840/16=990lbs."),
            new("Horse weighs1100lbs. Equivalent ounces at16oz/lb?",17600,"1100×16=17600ounces."),
            new("Rank from heaviest to lightest bus9tons, elephant5500kg, horse1100lb, piano15840oz. Enter1 for bus,2 elephant,3 horse,4 piano as HEAVIEST.",1,"School bus18000lb is heaviest."),
            new("A worker earns10dollars for half hour. Dollars per hour?",20,"10/0.5=20dollars/hour."),
            new("A worker earns30dollars for1.5hours. Dollars per hour?",20,"30/1.5=20."),
            new("A worker earns30dollars for2hours. Dollars per hour?",15,"30/2=15."),
            new("A worker earns20dollars per hour. Pay in DOLLARS for3hours?",60,"20×3=60."),
            new("Compute one-half times17, expressed in TENTHS of a unit.",85,"17/2=8.5, or85tenths."),
            new("Compute three-quarters of200.",150,"200×3/4=150."),
            new("Compute0.2 times40.",8,"0.2×40=8."),
            new("Compute0.25 times60.",15,"0.25×60=15."),
            new("A fast printer makes120 copies per minute. Time for360 copies in MINUTES?",3,"360/120=3."),
            new("At20 dollars/hour, pay for half an hour in DOLLARS?",10,"20×1/2=10.")
        ],
        [L0309ComparisonLogic] =
        [
            new("A machine prints480copies in4minutes or120copies/minute. 1=yes,0=no.",1,"480÷4=120."),
            new("The copier at120copies/min prints720copies in6min. 1=yes,0=no.",1,"720/120=6."),
            new("The bus9tons weighs18000pounds under2000lb/ton. 1=yes,0=no.",1,"9×2000=18000."),
            new("The elephant5500kg weighs about12100lb using2.2lb/kg. 1=yes,0=no.",1,"Approximate conversion."),
            new("The piano15840oz weighs more than a horse1100lb. 1=yes,0=no.",0,"Piano990lb, horse1100lb."),
            new("Job A half hour10dollars and job B1.5hours30dollars have equal hourly pay. 1=yes,0=no.",1,"Both20dollars/hour."),
            new("A2hour job for30dollars pays more per hour than half hour for10dollars. 1=yes,0=no.",0,"15dollars/hour vs20."),
            new("The three jobs 10dollars/half hour,30/1.5h,30/2h all pay the same rate. 1=yes,0=no.",0,"Third pays less."),
            new("Unit-rate reasoning may require converting measurements to the same unit before comparing. 1=yes,0=no.",1,"Incompatible unit comparisons mislead."),
            new("A whole-package price is enough to determine per-slice cheese price without knowing slice count. 1=yes,0=no.",0,"The source image count must be known."),
            new("The heart-shaped polygon's exact area can be confirmed from an unlabeled text summary alone without inspecting the drawing. 1=yes,0=no.",0,"Teacher/source image dimensions matter."),
            new("An exact arithmetic result for a source problem does not automatically grade a student's open-ended visual justification. 1=yes,0=no.",1,"Human review remains needed."),
            new("A percentage can be interpreted as a rate per100 in next lessons. 1=yes,0=no.",1,"100-based ratio.")
        ],
        [L0310PercentNumbers] =
        [
            new("Four US dimes are40 cents. Percent of one dollar?",40,"40/100=40%."),
            new("One nickel and three pennies total8 cents. Percent of one dollar?",8,"5+3=8cents=8%."),
            new("Five quarters and one dime total135cents. Percent of one dollar?",135,"5×25+10=135cents=135%."),
            new("Three dimes total30cents. Percent of one dollar?",30,"30%."),
            new("One quarter plus one nickel total30cents. Percent of one dollar?",30,"25+5=30%."),
            new("Five quarters, one dime and one nickel total140cents. Percent of one dollar?",140,"125+10+5=140%."),
            new("A historical half-cent coin is what TENTHS of a PERCENT of a dollar?",5,"0.5cent/100cents=0.5%=5tenths of a percent."),
            new("A 3-cent coin is what percent of one dollar?",3,"3cents=3%."),
            new("A 20-cent coin is what percent of one dollar?",20,"20%."),
            new("A 2.5-dollar historical coin is what percent of a dollar?",250,"2.5×100%=250%."),
            new("A 5-dollar coin is what percent of a dollar?",500,"5×100%=500%."),
            new("Elena buys8 tokens for440cents. Cents per token?",55,"440/8=55cents."),
            new("Elena buys8 tokens for440cents. Tokens purchasable with605cents?",11,"605/55=11."),
            new("Elena buys8 tokens for440cents. Cost of19 tokens in CENTS?",1045,"19×55=1045cents."),
            new("Snail travels10cm in4minutes. Seconds needed for24cm at constant speed?",576,"24×4/10=9.6min=576sec."),
            new("Snail travels10cm in4minutes. Cm in6minutes?",15,"10/4×6=15cm."),
            new("Three tacos cost18 dollars. Dollars per taco?",6,"18/3=6."),
            new("Three tacos cost18 dollars. Total DOLLARS for4 tacos?",24,"6×4=24."),
            new("Three tacos cost18 dollars. Total DOLLARS for5 tacos?",30,"6×5=30."),
            new("Three tacos cost18 dollars. Total DOLLARS for6 tacos?",36,"6×6=36.")
        ],
        [L0310PercentLogic] =
        [
            new("40cents is40% of one dollar. 1=yes,0=no.",1,"One dollar=100cents."),
            new("135 cents is135% of one dollar. 1=yes,0=no.",1,"Percentage can exceed100 when comparing quantities."),
            new("A percentage can never be more than100% for any type of comparison. 1=yes,0=no.",0,"Comparison quantities can exceed the reference."),
            new("If 25% of a water bottle is consumed, the consumed portion cannot exceed the whole bottle in that part-whole context. 1=yes,0=no.",1,"Part of the reference whole."),
            new("Two and a half dollars equals250% of a dollar. 1=yes,0=no.",1,"2.5 times the reference."),
            new("A 5dollar amount equals5% of a dollar. 1=yes,0=no.",0,"It equals500%."),
            new("Four dimes and one quarter total65% of a dollar. 1=yes,0=no.",1,"40+25=65cents."),
            new("Two quarters equal50% of one dollar. 1=yes,0=no.",1,"50/100=50%."),
            new("A 30-cent coin collection is30% of a dollar. 1=yes,0=no.",1,"30 cents per100cents."),
            new("Percent means per hundred, so100% represents the full reference amount. 1=yes,0=no.",1,"Rate per100."),
            new("At55 cents per token, 11tokens cost605cents. 1=yes,0=no.",1,"11×55=605."),
            new("A snail10cm in4min travels24cm in6min at the same rate. 1=yes,0=no.",0,"6min yields15cm."),
            new("Three tacos for18 dollars gives each taco6dollars. 1=yes,0=no.",1,"18/3=6.")
        ],
        [L0311PercentValues] =
        [
            new("Mai attempted40 free throws and made25%. Successful throws?",10,"40×25/100=10."),
            new("Priya previously made12 free throws and today made150% as many. Successful throws today?",18,"12×1.5=18."),
            new("200mg vitamin C is250% of daily recommended amount. Daily100% in MILLIGRAMS?",80,"200/2.5=80mg."),
            new("At school100 students represent40% who prefer hip hop. Total sixth graders?",250,"100/0.4=250."),
            new("20% of150 apples is how many apples?",30,"150×0.2=30."),
            new("75% of80 students is how many?",60,"80×3/4=60."),
            new("The scooter traveled1020inches in5seconds. Distance in TENTHS OF A CENTIMETER?",25908,"1020×2.54=2590.8cm=25908tenths."),
            new("Skateboard traveled90feet in5seconds. Distance in TENTHS OF A CENTIMETER?",27432,"90×30.48=2743.2cm=27432tenths."),
            new("Bike traveled4800centimeters in5seconds. Distance in CENTIMETERS?",4800,"Given4800cm."),
            new("Go-cart traveled0.03kilometers in5seconds. Distance in CENTIMETERS?",3000,"0.03km=30meters=3000cm."),
            new("Fastest of skateboard90ft, scooter1020in, bike4800cm, go-cart0.03km in same5s. Enter1=skateboard,2=scooter,3=bike,4=go-cart.",3,"Bike4800cm in5s is farthest."),
            new("A batch uses10lb potatoes for15lb mashed potatoes. Mash in HALF-POUND units from15lb raw potatoes?",45,"15×1.5=22.5lb=45half-pound units."),
            new("A recipe uses10lb potatoes to make15lb mash. Raw pounds needed for50lb mash in THIRD-POUND units?",100,"50×10/15=100/3lb=100 third-pound units."),
            new("A medium bottle has320mL reference,150% of reference is how many mL?",480,"320×1.5=480mL."),
            new("Forty free throws correspond to100%. Number corresponding to50%?",20,"Half of40=20."),
            new("Twelve successful throws represent100%. Number at125%?",15,"12×1.25=15."),
            new("200mg is250% of reference. Quantity for50% of reference, in mg?",40,"Reference80mg, half=40.")
        ],
        [L0311PercentReasoning] =
        [
            new("25% of40 attempts is10 successful throws. 1=yes,0=no.",1,"40/4=10."),
            new("150% of12 is18, which is larger than the reference12. 1=yes,0=no.",1,"150% is one and a half times."),
            new("150% of12 must be less than12 because percentages cannot exceed100. 1=yes,0=no.",0,"Comparison percentages may exceed100."),
            new("200mg as250% of allowance means allowance80mg. 1=yes,0=no.",1,"200/2.5=80."),
            new("If100 pupils are40% of a group, the whole group consists of250 pupils. 1=yes,0=no.",1,"100/0.4=250."),
            new("If100 pupils are40% of a group, total pupils=140. 1=yes,0=no.",0,"40% of140=56."),
            new("On a percent double number line the100% reference aligns with the entire reference quantity. 1=yes,0=no.",1,"Corresponding marks."),
            new("Skateboard90ft equals2743.2cm using30.48cm/foot. 1=yes,0=no.",1,"90×30.48."),
            new("Go-cart0.03km equals3000cm. 1=yes,0=no.",1,"0.03km=30m."),
            new("Bike4800cm in5seconds is faster than go-cart3000cm in5seconds. 1=yes,0=no.",1,"Same time, greater distance."),
            new("Potatoes10lbs produce15lbs mash, so15lbs potatoes make22.5lbs mash at constant rate. 1=yes,0=no.",1,"Both×1.5."),
            new("Percentage comparisons should identify which amount is100%. 1=yes,0=no.",1,"Without reference the percentage is ambiguous."),
            new("A 200mg quantity can be250% of a smaller80mg reference. 1=yes,0=no.",1,"200/80=2.5.")
        ],
        [L0312TapePercent] =
        [
            new("Bakery has40 muffin flavors;25% contain chocolate. Number with chocolate?",10,"40×1/4=10."),
            new("Bakery has40 flavors;25% chocolate. Number without chocolate?",30,"40-10=30."),
            new("School band70 students:40% sixth graders. Count sixth graders?",28,"70×0.4=28."),
            new("School band70 students:20% seventh graders. Count seventh graders?",14,"70×0.2=14."),
            new("Band70 with40% sixth and20% seventh. Percentage eighth graders?",40,"100-40-20=40%."),
            new("Band70 with40% sixth and20% seventh. Count eighth graders?",28,"70×40%=28."),
            new("Jada spent60dollars, which is120% of budget. Monthly budget in DOLLARS?",50,"60/1.2=50."),
            new("Jada budget50dollars. Spending120% of budget in DOLLARS?",60,"50×1.2=60."),
            new("Five tickets cost12dollars50cents. Price per ticket in CENTS?",250,"1250/5=250cents."),
            new("Eight tickets cost20dollars16cents. Price per ticket in CENTS?",252,"2016/8=252cents."),
            new("Buying5 tickets for12.50 dollars or8 for20.16 dollars, which is cheaper per ticket? Enter1 for five-pack,2 for eight-pack.",1,"250cents vs252cents."),
            new("An athlete runs8miles in50minutes. Time for9miles in SECONDS at same rate?",3375,"9×50/8=56.25min=3375sec."),
            new("An athlete runs8miles in50minutes. Distance traveled in60minutes in TENTHS OF A MILE?",96,"8×60/50=9.6miles=96tenths."),
            new("Under a tape model, one quarter of80 students is how many?",20,"80/4=20."),
            new("Under a tape model, three quarters of80 students is how many?",60,"3×20=60."),
            new("A bill of60dollars is120% of a monthly50dollar budget. Amount OVER budget in DOLLARS?",10,"60-50=10."),
            new("Forty muffin flavors with ten chocolate flavors: percent chocolate?",25,"10/40×100%=25.")
        ],
        [L0312TapeStructure] =
        [
            new("25% of40 muffin flavors is10 flavors. 1=yes,0=no.",1,"One of four equal quarters."),
            new("25% of40 muffin flavors is25 flavors. 1=yes,0=no.",0,"It is10."),
            new("Band40% sixth and20% seventh leaves40% eighth. 1=yes,0=no.",1,"100-40-20=40%."),
            new("Band70 members with40% sixth has28 sixth graders. 1=yes,0=no.",1,"0.4×70=28."),
            new("Band70 members with20% seventh has20 seventh graders. 1=yes,0=no.",0,"20% of70=14."),
            new("A tape with four equal blocks can model25% as one block. 1=yes,0=no.",1,"One-quarter equals25% of amount."),
            new("Jada60dollars spent is120% of a50dollar budget. 1=yes,0=no.",1,"60/50=120%."),
            new("If60dollars is120% of budget, budget is72dollars. 1=yes,0=no.",0,"Budget is50."),
            new("Five tickets cost1250cents, so250cents per ticket. 1=yes,0=no.",1,"1250/5=250."),
            new("Eight tickets cost2016cents, so252cents per ticket. 1=yes,0=no.",1,"2016/8=252."),
            new("One can determine exact walking percentages from an original image without knowing the picture's measured quantities. 1=yes,0=no.",0,"Diagram data needed."),
            new("Tapes can represent both percentages below100 and comparisons larger than100 by extending beyond the reference whole. 1=yes,0=no.",1,"Extend tape beyond100%."),
            new("At8miles per50minutes,9miles takes56.25minutes. 1=yes,0=no.",1,"50×9/8=56.25.")
        ],
        [L0313BenchmarkNumbers] =
        [
            new("Andre lives1.6km from school. Fifty percent of that distance in METERS?",800,"Half of1.6km=0.8km=800meters."),
            new("Diego lives half a mile from school. Fifty percent of that in QUARTER-MILE UNITS?",1,"Half of1/2mile is1/4mile."),
            new("Diego lives half a mile from school. Fifty percent in YARDS?",440,"One quarter mile=440yards."),
            new("Laptop sale saves35 dollars, which is10% of original price. Original price in DOLLARS?",350,"35/0.10=350dollars."),
            new("If15 is out of30, what PERCENT is15 of30?",50,"15/30×100=50%."),
            new("If3 is out of12, what PERCENT is3 of12?",25,"3/12×100=25%."),
            new("If6 is out of10, what PERCENT is6 of10?",60,"6/10×100=60%."),
            new("20% of60 is how many?",12,"One fifth of60=12."),
            new("20% of125 is how many?",25,"125/5=25."),
            new("Diego has75% of10dollars. Amount in CENTS?",750,"0.75×1000=750cents."),
            new("Noah has25% of30dollars. Amount in CENTS?",750,"0.25×3000=750cents."),
            new("Nine is what percent of36?",25,"9/36=1/4=25%."),
            new("Nine is what percent of18?",50,"9/18=1/2=50%."),
            new("Nine is what percent of90?",10,"9/90=1/10=10%."),
            new("Nine is what percent of12?",75,"9/12=3/4=75%."),
            new("Lin walks2.5miles/hour, Andre3miles/hour from opposite ends22mile trail. Meeting time in HOURS?",4,"22/(2.5+3)=4 hours."),
            new("Lin walks2.5mph, Andre3mph toward each other22mi apart. Combined miles covered after2hours?",11,"(2.5+3)×2=11miles."),
            new("One hour has60minutes. Twenty-five percent of hour in MINUTES?",15,"60/4=15minutes."),
            new("One hour has60minutes. Seventy-five percent of hour in MINUTES?",45,"3×15=45minutes.")
        ],
        [L0313BenchmarkLogic] =
        [
            new("50% of any positive quantity is one-half of that quantity. 1=yes,0=no.",1,"50/100=1/2."),
            new("25% of a quantity is one-fourth of it. 1=yes,0=no.",1,"25/100=1/4."),
            new("10% of a quantity is one-tenth of it. 1=yes,0=no.",1,"10/100=1/10."),
            new("75% of a quantity is three-fourths of it. 1=yes,0=no.",1,"75/100=3/4."),
            new("For any real number,20% of it equals that number divided by5. 1=yes,0=no.",1,"20/100=1/5."),
            new("Twenty percent of60 is20, because percent means subtract20. 1=yes,0=no.",0,"20%=12."),
            new("Saving35 dollars at10% discount means original price350 dollars. 1=yes,0=no.",1,"35×10=350."),
            new("Fifteen is50% of30. 1=yes,0=no.",1,"15 is half30."),
            new("Three is25% of12. 1=yes,0=no.",1,"3 is quarter12."),
            new("Six is60% of10. 1=yes,0=no.",1,"6/10=60%."),
            new("75% of10dollars and25% of30dollars are equal. 1=yes,0=no.",1,"Both7.50dollars."),
            new("Lin2.5mph and Andre3mph start22miles apart and move toward each other, meeting after4hours. 1=yes,0=no.",1,"Combined speed5.5mph."),
            new("A 10% reduction of350dollars saves35 dollars. 1=yes,0=no.",1,"350×0.1=35.")
        ],
        [L0314PercentComputations] =
        [
            new("160 is what percentage of40?",400,"160/40×100=400%."),
            new("40 is160% of what number?",25,"40/1.6=25."),
            new("What number is40% of160?",64,"0.4×160=64."),
            new("An item is20% off and Mai saves13 dollars. Original price in DOLLARS?",65,"13/0.2=65."),
            new("A scarf originally costs16 dollars and a shopper saves12 dollars. Percent discount?",75,"12/16×100=75%."),
            new("A scarf originally costs16 dollars and shopper saves12. Sale price in DOLLARS?",4,"16-12=4."),
            new("Evaluate120% of100.",120,"1.2×100=120."),
            new("Evaluate50% of150.",75,"0.5×150=75."),
            new("Evaluate150% of50.",75,"1.5×50=75."),
            new("Evaluate20% of800.",160,"0.2×800=160."),
            new("Evaluate200% of30.",60,"2×30=60."),
            new("Evaluate500% of400.",2000,"5×400=2000."),
            new("Evaluate1% of1000.",10,"0.01×1000=10."),
            new("An ant travels30cm in2minutes. Speed in centimeters per minute?",15,"30/2=15cm/min."),
            new("An ant travels30cm in2minutes. SECONDS per centimeter?",4,"120sec/30cm=4sec/cm."),
            new("Three and a half US cups at236.6mL per cup: total in TENTHS OF MILLILITER?",8281,"3.5×236.6=828.1mL."),
            new("One meter is approximately100cm. 75% of a meter in CENTIMETERS?",75,"0.75×100=75cm."),
            new("A coupon saves15 dollars on an original75-dollar price. What discount percent?",20,"15/75×100=20%.")
        ],
        [L0314PercentInterpretation] =
        [
            new("160 is400% of40. Enter1=yes,0=no.",1,"Four times the reference."),
            new("40 is160% of25. Enter1=yes,0=no.",1,"1.6×25=40."),
            new("40% of160 is64. Enter1=yes,0=no.",1,"0.4×160=64."),
            new("A20% sale saving13 dollars implies original65 dollars. Enter1=yes,0=no.",1,"13/0.2=65."),
            new("A12-dollar saving from16 dollars is a75% discount. Enter1=yes,0=no.",1,"12/16=0.75."),
            new("120% of100 exceeds100. Enter1=yes,0=no.",1,"120>100."),
            new("50% of150 exceeds100. Enter1=yes,0=no.",0,"75<100."),
            new("150% of50 exceeds100. Enter1=yes,0=no.",0,"75<100."),
            new("20% of800 exceeds100. Enter1=yes,0=no.",1,"160>100."),
            new("200% of30 exceeds100. Enter1=yes,0=no.",0,"60<100."),
            new("500% of400 exceeds100. Enter1=yes,0=no.",1,"2000>100."),
            new("1% of1000 exceeds100. Enter1=yes,0=no.",0,"10<100."),
            new("3.5 cups at236.6mL each contains less than1liter. Enter1=yes,0=no.",1,"828.1mL<1000mL."),
            new("An ant that travels30cm in2minutes travels15cm per minute. Enter1=yes,0=no.",1,"30/2=15."),
            new("Solving an information-gap percentage problem requires identifying the100% reference. Enter1=yes,0=no.",1,"Percent comparisons require a known or inferred whole."),
            new("A discount of20% is always20 dollars regardless of original price. Enter1=yes,0=no.",0,"Discount depends on the reference price.")
        ],
        [L0315PercentAmounts] =
        [
            new("Forty percent of75?",30,"0.4×75=30."),
            new("Priya practices152% as long as Han's75 minutes. How many minutes?",114,"1.52×75=114."),
            new("Tyler practices64% as long as Han's75 minutes. How many minutes?",48,"0.64×75=48."),
            new("Amusement park has1575 visitors.56% adults. How many adults?",882,"1575×0.56=882."),
            new("Amusement park has1575 visitors.16% teens. How many teenagers?",252,"1575×0.16=252."),
            new("Amusement park has1575 visitors.28% children. How many children?",441,"1575×0.28=441."),
            new("Find55% of180.",99,"180×0.55=99."),
            new("Find300% of26.",78,"26×3=78."),
            new("Find12% of700.",84,"700×0.12=84."),
            new("Find20% of60.",12,"60÷5=12."),
            new("25% of which number equals6?",24,"6÷0.25=24."),
            new("What percent of100 is14?",14,"14/100=14%."),
            new("Find50% of90.",45,"90÷2=45."),
            new("10% of which number is7?",70,"7÷0.1=70."),
            new("Find30% of70.",21,"70×0.3=21."),
            new("Six sandwich rolls cost528 cents. Per roll price in CENTS?",88,"528/6=88."),
            new("Four sandwich rolls cost340 cents. Per roll price in CENTS?",85,"340/4=85."),
            new("For24 rolls four six-packs at528cents each cost how many CENTS?",2112,"4×528=2112."),
            new("For24 rolls six four-packs at340cents each cost how many CENTS?",2040,"6×340=2040."),
            new("Trip chaperone:student ratio3:20,92 total people. How many chaperones?",12,"92÷23=4 parts groups;3×4=12."),
            new("Trip chaperone:student ratio3:20,92 total. How many students?",80,"20×4=80.")
        ],
        [L0315PercentProperties] =
        [
            new("Two fifths of75 equals40% of75. 1=yes,0=no.",1,"2/5=0.4."),
            new("Two fifths of any number equals40% of that number. 1=yes,0=no.",1,"Equivalent factors."),
            new("152% of75 minutes is114minutes. 1=yes,0=no.",1,"75×1.52=114."),
            new("64% of75 minutes is64minutes. 1=yes,0=no.",0,"It is48min."),
            new("Amusement park categories56%,16%,28% sum to100%. 1=yes,0=no.",1,"56+16+28=100."),
            new("Of1575 people56% adults equals882. 1=yes,0=no.",1,"1575×0.56."),
            new("55% of180 is greater than300% of26. 1=yes,0=no.",1,"99>78."),
            new("12% of700 is greater than55% of180. 1=yes,0=no.",0,"84<99."),
            new("300% of26 equals78. 1=yes,0=no.",1,"Triple26."),
            new("A six-pack of rolls for528cents is a better unit price than four-pack for340cents. 1=yes,0=no.",0,"88c versus85c each."),
            new("Four six-packs cost2112cents and six four-packs cost2040cents for24 rolls. 1=yes,0=no.",1,"Four-packs save72 cents."),
            new("A 3:20 chaperone:student ratio with92 total people gives12 chaperones. 1=yes,0=no.",1,"23 parts, each4."),
            new("A percent multiplication factor over1 can produce a value greater than the reference. 1=yes,0=no.",1,"Such as152%."),
            new("When finding A% of B, multiplying B by A/100 gives the amount. 1=yes,0=no.",1,"Definition of percent per100.")
        ],
        [L0316FindPercent] =
        [
            new("A ride minimum height is40 inches.34in is what percent of40in?",85,"34/40×100=85%."),
            new("A ride minimum height is40 inches.54in is what percent of40in?",135,"54/40×100=135%."),
            new("Tool normally costs80dollars, savings12dollars. Savings percent?",15,"12/80×100=15%."),
            new("A tub capacity80gallons and fills4gal/minute for6minutes. Gallons filled?",24,"4×6=24."),
            new("A tub capacity80gallons and fills4gal/minute for6minutes. Percent filled?",30,"24/80×100=30%."),
            new("Backpack30dollars, sale price85% of usual. Sale price in CENTS?",2550,"30×0.85=25.50dollars."),
            new("Sweatshirt18dollars, sale85% of usual. Sale price in CENTS?",1530,"18×0.85=15.30dollars."),
            new("Soccer ball24dollars80cents, sale85% of usual. Sale price in CENTS?",2108,"2480×0.85=2108cents."),
            new("Eight six-packs of hot dogs at210cents each cost how many CENTS?",1680,"8×210=1680."),
            new("Six eight-packs of hot dogs at312cents each cost how many CENTS?",1872,"6×312=1872."),
            new("Both hotdog package offers supply48 hotdogs. Cheaper choice? Enter1 for six-packs,2 for eight-packs.",1,"1680cents <1872cents."),
            new("Elena56in tall; centimeters using2.54cm per inch, answer in HUNDREDTHS OF CM?",14224,"56×2.54=142.24cm."),
            new("Elena56in tall; meters in TEN-THOUSANDTHS of meter?",14224,"142.24cm=1.4224meters."),
            new("Usual price80 dollars reduced12. Sale price in DOLLARS?",68,"80-12=68."),
            new("85% of80 is how many DOLLARS?",68,"0.85×80=68."),
            new("40in minimum; 34in is how many INCHES below minimum?",6,"40-34=6."),
            new("40in minimum;54in is how many INCHES above minimum?",14,"54-40=14."),
            new("Tub80gal fills24gal. Gallons still unfilled?",56,"80-24=56.")
        ],
        [L0316FindPercentLogic] =
        [
            new("To find the percent C is of B (B nonzero), compute C/B×100. 1=yes,0=no.",1,"Definition of percent relative to reference."),
            new("34inches is85% of40inches. 1=yes,0=no.",1,"34/40=.85."),
            new("54inches is135% of40inches. 1=yes,0=no.",1,"54/40=1.35."),
            new("54inches cannot be135% of40inches because percentages never exceed100. 1=yes,0=no.",0,"Comparison may exceed100."),
            new("Saving12dollars off80 represents15% of usual price. 1=yes,0=no.",1,"12/80=.15."),
            new("Saving12dollars off80 represents85% of usual price. 1=yes,0=no.",0,"85% is the price remaining."),
            new("A tub80gallons with24gallons is30% full. 1=yes,0=no.",1,"24/80=.3."),
            new("Sale price85% of30dollars is25.50 dollars. 1=yes,0=no.",1,"0.85×30=25.5."),
            new("Sale price85% of24.80dollars is21.08 dollars. 1=yes,0=no.",1,"0.85×24.8=21.08."),
            new("For48 hotdogs, eight six-packs at2.10 each are cheaper than six eight-packs at3.12. 1=yes,0=no.",1,"16.80<18.72."),
            new("56 inches equals142.24cm given2.54cm per inch. 1=yes,0=no.",1,"56×2.54."),
            new("The reference quantity B is the100% amount when asking what percent C is of B. 1=yes,0=no.",1,"C/B×100%."),
            new("40inches is100% of the ride minimum40inches. 1=yes,0=no.",1,"Equal amounts."),
            new("A percentage based on one reference can be transferred to another unrelated reference without recalculating the amount. 1=yes,0=no.",0,"Amounts depend on base.")
        ],
        [L0317PaintCalculations] =
        [
            new("Given east wall9ft long and8ft high, its area in SQUARE FEET?",72,"9×8=72sqft."),
            new("Given total paintable wall area300sqft and two coats, total paint coverage in SQUARE FEET?",600,"300×2=600 square feet of coverage."),
            new("Source paint covers175sqft with2quarts. Coverage per QUART in HALF-SQUARE-FOOT UNITS?",175,"175/2=87.5sqft per quart, or175 half-sqft units."),
            new("Two coats require600sqft coverage and each2quarts covers175sqft. Minimum WHOLE QUARTS to buy?",7,"600/(175/2)=6.857... quarts, so7 whole quarts."),
            new("Paint costs1090cents per quart. Cost of7quarts WITHOUT discount in CENTS?",7630,"7×1090=7630cents."),
            new("Paint costs1090cents per quart with20% off. Discount per QUART in CENTS?",218,"0.2×1090=218."),
            new("Paint costs1090cents per quart with20% off. Sale price per QUART in CENTS?",872,"1090-218=872cents."),
            new("Seven quart paint cans with20% discount cost how many CENTS?",6104,"7×872=6104cents."),
            new("One gallon costs3490cents and3quarts each1090cents before discount. Total in CENTS?",6760,"3490+3×1090=6760cents."),
            new("One gallon3490cents plus3discounted quarts872cents each. Total in CENTS?",6106,"3490+3×872=6106cents."),
            new("Two gallons cost3490cents each. Total in CENTS?",6980,"2×3490=6980."),
            new("Compare seven discounted quart cans6104cents with one gallon plus3 discounted quarts6106cents. Savings in CENTS by choosing seven quarts?",2,"6106-6104=2cents."),
            new("Source east wall72sqft of total300sqft. Percent of wall area represented by east wall?",24,"72/300×100=24%."),
            new("Painting east72sqft with two coats takes96minutes. Minutes per SQUARE FOOT of wall area, in THIRD-MINUTE UNITS?",4,"96/72=4/3min/sqft."),
            new("At same solo pace as east wall96min/72sqft, solo minutes for entire300sqft?",400,"300×96/72=400min."),
            new("At same solo pace, remaining minutes after east wall96 of solo total400?",304,"400-96=304."),
            new("With friend, remaining painting takes150minutes instead of304solo. Minutes saved?",154,"304-150=154minutes."),
            new("Room's south window5ft by3ft not painted. Area excluded in SQFT?",15,"5×3=15 square feet."),
            new("A narrow paintable wall face measures0.5ft wide by8ft tall. Area in SQUARE FEET?",4,"0.5×8=4sqft."),
            new("Paint purchase: seven quarts supply how many HALF-SQUARE-FOOT COVERAGE UNITS given each quart87.5 sqft?",1225,"7×87.5=612.5sqft, or1225 half-square-foot units.")
        ],
        [L0317PaintReasoning] =
        [
            new("Two coats on300sqft of walls require covering600sqft in total. 1=yes,0=no.",1,"Two complete coats double coverage needed."),
            new("If each quart covers87.5sqft and600sqft must be coated,6 whole quarts are sufficient. 1=yes,0=no.",0,"6×87.5=525, need7."),
            new("Seven quarts can cover612.5sqft at87.5sqft per quart, enough for600sqft. 1=yes,0=no.",1,"Capacity exceeds need."),
            new("The east wall72sqft is25% of300sqft total. 1=yes,0=no.",0,"72/300=24%."),
            new("The east wall72sqft is24% of300sqft total. 1=yes,0=no.",1,"72/300×100=24%."),
            new("A painter's quoted rate96min for72sqft can estimate solo time for300sqft as400min under unchanged conditions. 1=yes,0=no.",1,"96/72×300=400."),
            new("If the rest takes304min alone but150min with a friend, saved painting time is154min. 1=yes,0=no.",1,"304-150=154."),
            new("Quarts sold with20% discount have an effective price872cents when original is1090cents. 1=yes,0=no.",1,"1090×.8=872."),
            new("One gallon and3 discounted quarts cost6106cents given gallon3490 and quart872. 1=yes,0=no.",1,"3490+2616=6106."),
            new("Seven discounted quarts cost6104cents, two cents less than one gallon plus3 discounted quarts. 1=yes,0=no.",1,"6106-6104=2."),
            new("Drying breaks must be added to painting minutes to answer a question restricted to active painting time. 1=yes,0=no.",0,"Prompt excludes drying/lunch pauses."),
            new("Open-ended project supplies costs have one guaranteed identical total for every valid purchase plan. 1=yes,0=no.",0,"Container and extra supplies decisions vary."),
            new("Areas of doors and windows should be excluded if those surfaces are not painted. 1=yes,0=no.",1,"Subtract nonpainted surfaces."),
            new("A numerically correct practice answer automatically verifies a student's floor-plan diagram and supply rationale. 1=yes,0=no.",0,"Teacher review is required."),
            new("The original lesson has numbered work on preparing, budgeting and painting time, even though it has no separate practice.html. 1=yes,0=no.",1,"Activities17.1–17.3 form the original capstone.")
        ],
        [L0401SourceNumerics] =
        [
            new("To obtain a quotient CLOSEST to1 with divisor40.01, choose dividend4,40,4000. Enter chosen number.",40,"40/40.01≈0.99975."),
            new("To obtain a quotient MUCH LESS than1 with divisor40.01, choose dividend4,40,4000. Enter chosen number.",4,"4/40.01≈0.1."),
            new("To obtain a quotient MUCH GREATER than1 with divisor40.01, choose dividend4,40,4000. Enter chosen number.",4000,"4000/40.01≈100."),
            new("100 divided by1/1000 equals?",100000,"100×1000=100000."),
            new("Rocking horse limit60 pounds;33 pounds is what PERCENT of limit?",55,"33/60×100=55%."),
            new("Rocking horse limit60 pounds;114 pounds is what PERCENT of limit?",190,"114/60×100=190%."),
            new("Rocking horse limit60 pounds;95% of weight limit in POUNDS?",57,"0.95×60=57."),
            new("Playlist has90 songs.40% rock. Number of rock songs?",36,"0.4×90=36."),
            new("Playlist has90 songs.10% country. Number of country songs?",9,"0.1×90=9."),
            new("Playlist has90 songs.30% hip hop. Number of hip-hop songs?",27,"0.3×90=27."),
            new("Playlist has90 songs. Rest electronica after40% rock10% country30% hip hop. Count?",18,"20%×90=18."),
            new("Garden hose emits9quarts in6seconds. Seconds to emit12quarts at same rate?",8,"9/6=1.5qt/s;12/1.5=8sec."),
            new("Garden hose emits9quarts in6seconds. Quarts in10seconds?",15,"1.5×10=15quarts."),
            new("Compare decimals0.7 and0.70. Enter1 if first is larger,0 if equal,2 if smaller.",0,"Trailing zero does not change value."),
            new("Compare0.03+6/10 with0.30+6/100. Enter1 if first larger,0 equal,2 smaller.",1,"0.63>0.36."),
            new("Compare0.9 vs0.12. Enter1 if first larger,0 equal,2 smaller.",1,"0.9>0.12.")
        ],
        [L0401QuotientScale] =
        [
            new("100 divided by1/1000 is much greater than1. 1=yes,0=no.",1,"Dividing by a tiny positive number produces large quotient."),
            new("50 and1/3 divided by50 and1/4 is close to1. 1=yes,0=no.",1,"Dividend and divisor almost equal."),
            new("4.7 divided by5.2 is close to1. 1=yes,0=no.",1,"Quotient≈0.904."),
            new("2 divided by7335 is much smaller than1. 1=yes,0=no.",1,"Divisor greatly exceeds dividend."),
            new("2000001 divided by9 is much greater than1. 1=yes,0=no.",1,"Dividend greatly exceeds divisor."),
            new("0.002 divided by2000 is much smaller than1. 1=yes,0=no.",1,"One millionth."),
            new("40 divided by40.01 is close to1. 1=yes,0=no.",1,"Quotient≈0.99975."),
            new("4 divided by40.01 is much greater than1. 1=yes,0=no.",0,"Quotient≈0.10."),
            new("4000 divided by40.01 is much greater than1. 1=yes,0=no.",1,"Quotient≈99.975."),
            new("For positive a, dividing a by a positive number smaller than1 may produce a quotient larger than a. 1=yes,0=no.",1,"E.g.2/0.5=4."),
            new("Dividing by a positive number much larger than a positive dividend gives quotient less than1. 1=yes,0=no.",1,"Ratio dividend/divisor<1."),
            new("A thicker object generally needs MORE copies than thin object to fill same stack height. 1=yes,0=no.",0,"Thicker means fewer copies."),
            new("For a positive dividend, a larger positive divisor produces a larger quotient. 1=yes,0=no.",0,"Monotonic inverse relationship."),
            new("Two numerically close positive numbers yield a quotient close to1. 1=yes,0=no.",1,"Provided one is not zero and difference small relative to magnitude.")
        ],
        [L0402DivisionNumbers] =
        [
            new("20 pounds strawberries shared by5 people. Pounds PER PERSON?",4,"20÷5=4lb per person."),
            new("20 pounds strawberries shared5lb per person. Number of PEOPLE?",4,"20÷5=4people."),
            new("A museum needs180 dollars. Tickets cost15 dollars EACH. Number of tickets?",12,"180÷15=12 tickets."),
            new("A museum needs180 dollars for15 tickets. Price per ticket in DOLLARS?",12,"180÷15=12 dollars."),
            new("Ten divided by5 equals?",2,"5×2=10."),
            new("Four and a half divided by3, answer in HALVES?",3,"4.5÷3=1.5=3halves."),
            new("One half divided by4, answer in EIGHTHS?",1,"1/2÷4=1/8."),
            new("Two and a half gallons divided equally among5 bottles. Fill per bottle in HALF-GALLONS?",1,"2.5÷5=0.5gallon."),
            new("200 golf balls divided into4 buckets. Balls per bucket?",50,"200÷4=50."),
            new("Sixteen socks in pairs. Number of pairs?",8,"16÷2=8."),
            new("a divided by6 equals1. What is a?",6,"6/6=1."),
            new("a divided by6 is greater than1, and a is a positive integer. Smallest a?",7,"Any a>6 works; smallest integer7."),
            new("a divided by6 is less than1 and positive a is a whole number. Largest positive a?",5,"Largest positive whole a<6 is5."),
            new("0.25 is what PERCENT of1?",25,"0.25×100=25."),
            new("0.1 is what PERCENT of1?",10,"0.1×100=10."),
            new("0.75 is what PERCENT of1?",75,"0.75×100=75."),
            new("1.5 is what PERCENT of1?",150,"1.5×100=150."),
            new("1.4 is what PERCENT of1?",140,"1.4×100=140."),
            new("Jada walks3mph, Elena2.8mph, both for3hours. Difference in TENTHS OF MILE?",6,"(3-2.8)×3=0.6mile=6tenths."),
            new("Two and a half gallons split five equal bottles. Number of QUARTS each, assuming4quarts per gallon?",2,"0.5gallon=2quarts.")
        ],
        [L0402DivisionMeaningLogic] =
        [
            new("20÷5=4 can mean20lb shared among5 people gives4lb per person. 1=yes,0=no.",1,"Number of groups known."),
            new("20÷5=4 can mean20lb split into5lb portions yields4 people. 1=yes,0=no.",1,"Size of each group known."),
            new("20÷5=4 has one and only one possible contextual interpretation. 1=yes,0=no.",0,"Unknown groups or unknown group size."),
            new("180÷15=12 can represent12 tickets at15 dollars each. 1=yes,0=no.",1,"12×15=180."),
            new("180÷15=12 can represent15 tickets at12 dollars each. 1=yes,0=no.",1,"15×12=180."),
            new("4.5÷3=1.5 can be checked by multiplying1.5×3=4.5. 1=yes,0=no.",1,"Inverse operation."),
            new("1/2 divided by4 equals1/8. 1=yes,0=no.",1,"Four equal parts of a half."),
            new("Two and a half gallons among5 bottles means each gets2 gallons. 1=yes,0=no.",0,"Each gets half a gallon."),
            new("200 balls among4 equal buckets means50 per bucket. 1=yes,0=no.",1,"200/4=50."),
            new("Sixteen socks in pairs make8 pairs. 1=yes,0=no.",1,"16/2=8."),
            new("If a/6 is greater than1 for positive a, then a>6. 1=yes,0=no.",1,"Divisor6."),
            new("If a/6=1 then a=6. 1=yes,0=no.",1,"Equal factors."),
            new("If a/6 is less than1 for positive a, a must be greater than6. 1=yes,0=no.",0,"a<6."),
            new("The quotient can represent either number of groups or amount per group depending on units. 1=yes,0=no.",1,"Context determines unit of unknown."),
            new("At3mph versus2.8mph for3h, distance difference is0.6mile. 1=yes,0=no.",1,"9 vs8.4 miles.")
        ],
        [L0403ContextualDivision] =
        [
            new("Mai has36 dollars and movie tickets cost4.50dollars each. Number of tickets?",8,"36÷4.50=8."),
            new("Sixteen items arranged in groups of8. Number of groups?",2,"16÷8=2."),
            new("Sixteen items arranged in2 groups. Items per group?",8,"16÷2=8."),
            new("Four divided by one and one-third equals how many groups?",3,"4÷(4/3)=3."),
            new("Mini muffins cost300cents per12. Price per muffin in CENTS?",25,"300÷12=25."),
            new("At25cents per muffin, how many muffins can be bought for200cents?",8,"200÷25=8."),
            new("At25cents per muffin, price of16 muffins in CENTS?",400,"16×25=400."),
            new("Family monthly budget2400 dollars,44% housing. Dollars on housing?",1056,"2400×0.44=1056."),
            new("Family monthly budget2400 dollars,23% food. Dollars on food?",552,"2400×0.23=552."),
            new("Family monthly budget2400 dollars,6% clothing. Dollars on clothing?",144,"2400×0.06=144."),
            new("Family monthly budget2400 dollars,17% transportation. Dollars on transport?",408,"2400×0.17=408."),
            new("Family budget2400 with44% housing23% food6% clothing17% transport. Percent savings?",10,"100-44-23-6-17=10%."),
            new("Family budget2400 dollars with10% remaining. Savings in DOLLARS?",240,"2400×0.1=240."),
            new("A whole4 is divided into groups of4/3. How many groups?",3,"Three groups of4/3 make4."),
            new("Noah says division always reduces positive values. Counterexample2 divided by0.5 equals?",4,"2÷0.5=4."),
            new("Movie tickets cost450cents each. Cost for8 tickets in CENTS?",3600,"450×8=3600."),
            new("Twelve muffins cost300cents, cost of24 muffins in CENTS?",600,"2×300=600.")
        ],
        [L0403DivisionInterpretation] =
        [
            new("36 dollars at4.50 dollars per ticket purchases8 tickets. 1=yes,0=no.",1,"8×4.50=36."),
            new("For16÷8=2, 2 can represent number of equal groups with8 in each. 1=yes,0=no.",1,"16=2×8."),
            new("For16÷2=8, 8 can represent group size if there are2 equal groups. 1=yes,0=no.",1,"16=2×8."),
            new("Both16÷8=2 and16÷2=8 can be represented by the same group arrangement with differing unknowns. 1=yes,0=no.",1,"Groups and items per group swap."),
            new("4÷(1⅓)=3 because3 groups of1⅓ make4. 1=yes,0=no.",1,"3×4/3=4."),
            new("Dividing by a positive fraction less than1 always gives a smaller positive result than the dividend. 1=yes,0=no.",0,"E.g.2÷0.5=4."),
            new("Mini muffins are25cents each if a dozen costs300cents. 1=yes,0=no.",1,"300÷12=25."),
            new("Two dollars buys8 muffins at25cents each. 1=yes,0=no.",1,"200÷25=8."),
            new("Sixteen mini muffins at25cents each cost4 dollars. 1=yes,0=no.",1,"16×25=400cents."),
            new("In a budget44%+23%+6%+17%=90%, leaving10% savings. 1=yes,0=no.",1,"Total allocated90%."),
            new("Housing44% of2400dollars equals1056dollars. 1=yes,0=no.",1,"2400×.44=1056."),
            new("A correct multiplication equation can check a division quotient. 1=yes,0=no.",1,"Inverse relationship."),
            new("The division quotient always has the same real-world units as the dividend. 1=yes,0=no.",0,"Depends on grouping interpretation."),
            new("Teacher review is needed for a student-created visual diagram even when arithmetic is automatically checked. 1=yes,0=no.",1,"Diagram correctness needs a separate rubric.")
        ],
        [L0404FractionGroups] =
        [
            new("A bag holds3lb cat food and cat eats3/4lb each week. Weeks per bag?",4,"3÷(3/4)=4weeks."),
            new("How many one-third units are in one and two-thirds?",5,"5/3 divided by1/3=5."),
            new("Unknown ? multiplied by1/8 equals3. What is??",24,"24×1/8=3."),
            new("Fifteen multiplied by2/5 yields what whole number?",6,"15×2/5=6."),
            new("Six divided by2/5 yields what whole number?",15,"6×5/2=15."),
            new("Six divided by15, expressed in FIFTHS?",2,"6/15=2/5, two fifths."),
            new("Six multiplied by4/3 yields what whole number?",8,"6×4/3=8."),
            new("Eight divided by4/3 yields what whole number?",6,"8×3/4=6."),
            new("Eight divided by6, expressed in THIRDS?",4,"8/6=4/3, four thirds."),
            new("Sixteen multiplied by7/8 yields what whole number?",14,"16×7/8=14."),
            new("Fourteen divided by7/8 yields what whole number?",16,"14×8/7=16."),
            new("Fourteen divided by16, expressed in EIGHTHS?",7,"14/16=7/8."),
            new("Eight amusement park tickets total100 dollars. Price per ticket in CENTS?",1250,"10000/8=1250cents."),
            new("Noah brings13 dollars, ticket costs12.50. How many CENTS remain if he buys one?",50,"1300-1250=50cents."),
            new("Eight divided by0.001 equals?",8000,"8×1000=8000."),
            new("Twelve is150% of what number?",8,"12/1.5=8."),
            new("Five is50% of what number?",10,"5/0.5=10."),
            new("10% of what number is300?",3000,"300/0.10=3000."),
            new("5% of what number is72?",1440,"72/0.05=1440."),
            new("Twenty is80% of what number?",25,"20/0.8=25.")
        ],
        [L0404FractionModels] =
        [
            new("Three pounds divided by3/4 pound per week lasts4weeks. 1=yes,0=no.",1,"4×3/4=3."),
            new("Three pounds divided by3/4 pound per week lasts3weeks. 1=yes,0=no.",0,"3×3/4=2.25."),
            new("One and two-thirds contains5 one-third groups. 1=yes,0=no.",1,"5/3÷1/3=5."),
            new("The equation?×1/8=3 asks how many eighths fit in3. 1=yes,0=no.",1,"Answer24."),
            new("The equation?×1/8=3 asks for one eighth of3. 1=yes,0=no.",0,"It asks count of1/8 groups."),
            new("Fifteen times2/5 equals6. 1=yes,0=no.",1,"30/5=6."),
            new("Six times4/3 equals8. 1=yes,0=no.",1,"6×4/3=8."),
            new("Sixteen times7/8 equals14. 1=yes,0=no.",1,"16×7/8=14."),
            new("One whole divided into thirds contains3 one-third parts. 1=yes,0=no.",1,"1÷1/3=3."),
            new("Two wholes contain6 one-third parts. 1=yes,0=no.",1,"2×3=6."),
            new("The number of groups can increase when dividing by a positive fraction less than1. 1=yes,0=no.",1,"3÷3/4=4."),
            new("An8-person ticket charge totaling100dollars means12.50 dollars per student, so13dollars is sufficient. 1=yes,0=no.",1,"13>12.50."),
            new("When a yellow pattern-block hexagon represents1 whole, a one-third block covers one of3 equal area parts. 1=yes,0=no.",1,"Unit whole fixed by hexagon."),
            new("Fraction models must clearly label which shape represents the unit whole. 1=yes,0=no.",1,"Reference matters.")
        ],
        [L0405FractionValues] =
        [
            new("How many1/3 groups are in1/2? Answer in HALF-GROUP UNITS.",3,"1/2÷1/3=3/2 groups, three half-groups."),
            new("Seven divided by1/2 gives how many groups?",14,"7×2=14."),
            new("Six divided by3/8 gives how many groups?",16,"6×8/3=16."),
            new("One and7/8 divided by5/16 equals how many groups?",6,"15/8×16/5=6."),
            new("One and1/2 divided by2/5 equals how many QUARTER-GROUPS?",15,"3/2×5/2=15/4, fifteen quarter-groups."),
            new("Three wholes contain how many ONE-FOURTH pieces?",12,"3÷1/4=12."),
            new("Six wholes contain how many TWO-THIRDS pieces?",9,"6÷2/3=9."),
            new("Milk vendor A sells2L for380cents. Unit price in CENTS per liter?",190,"380/2=190cents/L."),
            new("Milk vendor B sells1.5L for270cents. Unit price in CENTS per liter?",180,"270/1.5=180cents/L."),
            new("For milk, vendor A190c/L, vendor B180c/L. Cheaper vendor? Enter1 A or2 B.",2,"B is10cents/L cheaper."),
            new("Flour:sugar recipe5cups:2cups. Sugar per cup flour in FIFTH-CUP portions?",2,"2/5cup sugar per cup flour."),
            new("Flour:sugar recipe5:2. Flour per cup sugar in HALF-CUP portions?",5,"5/2cup flour, five half-cup portions."),
            new("Flour:sugar recipe5:2. With7 cups sugar, flour cups in HALF-CUP units?",35,"7×5/2=17.5cups,35 half-cups."),
            new("Flour:sugar recipe5:2. With6 cups flour, sugar in FIFTH-CUP units?",12,"6×2/5=12/5cups."),
            new("A third of one whole fits in one half how many HALF-COUNTS?",3,"1.5 groups=3 halves."),
            new("One whole is divided into thirds. How many thirds?",3,"1÷1/3=3."),
            new("Three dollars80 cents buys2L. Price for4L in CENTS?",760,"Double."),
            new("The first milk vendor sells2L for380cents. Cost per HALF-LITER in CENTS?",95,"190c/L×0.5=95.")
        ],
        [L0405FractionMeaning] =
        [
            new("One half contains one and a half one-third groups. 1=yes,0=no.",1,"1/2÷1/3=1.5."),
            new("Half divided by third equals two-thirds. 1=yes,0=no.",0,"Correct3/2."),
            new("Seven has fourteen one-half groups. 1=yes,0=no.",1,"7×2=14."),
            new("Six has sixteen groups of3/8. 1=yes,0=no.",1,"6÷3/8=16."),
            new("1⅞ contains6 groups of5/16. 1=yes,0=no.",1,"30/16÷5/16=6."),
            new("1½ contains3¾ groups of2/5. 1=yes,0=no.",1,"1.5÷0.4=3.75."),
            new("The number of equal groups may be a non-integer fraction or mixed number. 1=yes,0=no.",1,"Partial group fits."),
            new("The yellow hexagon, rather than the red trapezoid, must be1 whole in every task. 1=yes,0=no.",0,"Source lesson compares different whole references."),
            new("One-half divided by one-third can be shown using equivalent sixths:3 sixths versus2 sixths. 1=yes,0=no.",1,"Three sixths contains1.5 pairs of sixths."),
            new("Milk vendor B at180cents per liter is cheaper than vendor A at190cents per liter. 1=yes,0=no.",1,"Compare same units."),
            new("Six whole cups of flour requires12/5 cups sugar at ratio5:2. 1=yes,0=no.",1,"6×2/5."),
            new("Seven cups sugar needs17.5 cups flour at ratio5:2. 1=yes,0=no.",1,"7×5/2."),
            new("A division equation can be verified by multiplying quotient times divisor. 1=yes,0=no.",1,"Unknown factor."),
            new("A drawing of a half group requires identifying the entire reference group first. 1=yes,0=no.",1,"Whole reference matters.")
        ],
        [L0406TapeValues] =
        [
            new("Three divided by1/4 equals how many groups?",12,"3×4=12."),
            new("Three divided by3/5 equals how many groups?",5,"3×5/3=5."),
            new("Seven days split into1/2-day periods. Number of periods?",14,"7×2=14."),
            new("One divided by5/6 gives how many FIFTH-GROUP UNITS?",6,"1÷5/6=6/5groups."),
            new("One divided by4/5 gives how many QUARTER-GROUP UNITS?",5,"1÷4/5=5/4 groups."),
            new("Ten percent of70?",7,"70/10=7."),
            new("Ten percent of110?",11,"110/10=11."),
            new("Twenty-five percent of160?",40,"160/4=40."),
            new("Twenty-five percent of48?",12,"48/4=12."),
            new("Fifty percent of90?",45,"90/2=45."),
            new("Fifty percent of350?",175,"350/2=175."),
            new("Seventy-five percent of300?",225,"300×3/4=225."),
            new("Seventy-five percent of48?",36,"48×3/4=36."),
            new("How many1/2 groups fit into2 wholes?",4,"2×2=4."),
            new("How many3/4 groups fit into3 wholes?",4,"3÷.75=4."),
            new("How many2/5 groups fit into4 wholes?",10,"4÷.4=10."),
            new("An unknown number of4/5 groups makes1 whole. Answer in QUARTER-GROUP UNITS?",5,"Unknown=5/4 groups."),
            new("How many1/3 groups fit into2 wholes?",6,"2×3=6.")
        ],
        [L0406TapeLogic] =
        [
            new("3÷1/4=12. Enter1=yes,0=no.",1,"Twelve quarter groups fit3."),
            new("3÷3/5=5. Enter1=yes,0=no.",1,"Five groups of3/5 make3."),
            new("A week has14 half-day periods. Enter1=yes,0=no.",1,"7÷1/2=14."),
            new("There are6/5 groups of5/6 in1 whole. Enter1=yes,0=no.",1,"(6/5)×(5/6)=1."),
            new("1÷4/5=5/4. Enter1=yes,0=no.",1,"Reciprocal factor."),
            new("The equation ?×4/5=1 represents how many4/5-sized groups fit in1. Enter1=yes,0=no.",1,"Original choice D."),
            new("The equation 1÷4/5=? represents how many4/5-sized groups fit in1. Enter1=yes,0=no.",1,"Original choice E."),
            new("The equation 4/5÷1=? represents how many4/5-sized groups fit in1. Enter1=yes,0=no.",0,"It asks about division by1."),
            new("Tape intervals may be unequal even when representing equal-size fractional groups. Enter1=yes,0=no.",0,"Group-size intervals should match."),
            new("75% of48 is36. Enter1=yes,0=no.",1,"Three quarters."),
            new("10% of70 is7. Enter1=yes,0=no.",1,"One tenth."),
            new("25% of160 is40. Enter1=yes,0=no.",1,"One quarter."),
            new("50% of350 is175. Enter1=yes,0=no.",1,"One half."),
            new("A diagram must label the reference whole before counting fractional groups. Enter1=yes,0=no.",1,"Fraction definition depends on whole.")
        ],
        [L0407FractionGroups] =
        [
            new("One batch needs1/2lb flour. How many batches with1lb?",2,"1÷1/2=2."),
            new("One batch needs1/2lb flour. With3/4lb, batches in HALF-BATCH UNITS?",3,"3/4÷1/2=3/2 batches."),
            new("One batch needs1/2lb flour. With1/4lb, batches in HALF-BATCH UNITS?",1,"1/4÷1/2=1/2 batch."),
            new("Whiskers weighs2⅔kg and Piglio4kg. Whiskers as fraction of Piglio, in THIRDS?",2,"(8/3)/4=2/3."),
            new("Piglio4kg to Whiskers2⅔kg. Piglio as fraction in HALF-UNITS?",3,"4/(8/3)=3/2."),
            new("Andre's trip is1⅝km, walked1/3km. Fraction completed in THIRTY-NINTHS?",8,"(1/3)/(13/8)=8/39."),
            new("What fraction of2½ is4/5? Answer in TWENTY-FIFTHS?",8,"(4/5)/(5/2)=8/25."),
            new("How many groups of3/4 are in11/4? Answer in THIRDS?",11,"(11/4)/(3/4)=11/3 groups."),
            new("How many groups of3/4 are in6½? Answer in THIRDS?",26,"(13/2)×(4/3)=26/3."),
            new("Four divided by2/7 gives how many groups?",14,"4×7/2=14."),
            new("One-fourth divided by one-half, answer in HALF-GROUPS?",1,"1/4÷1/2=1/2 group."),
            new("One-third divided by one-half, answer in THIRD-GROUP UNITS?",2,"1/3÷1/2=2/3 group."),
            new("Two-thirds divided by four-thirds, answer in HALF-GROUPS?",1,"(2/3)/(4/3)=1/2."),
            new("One-sixth divided by one-third, answer in HALF-GROUPS?",1,"1/6÷1/3=1/2."),
            new("A full group is1/2kg, available1/8kg. Fraction of full group in QUARTERS?",1,"1/8÷1/2=1/4."),
            new("Two whole kilograms split into half-kilogram groups. Number of groups?",4,"2÷1/2=4.")
        ],
        [L0407FractionLogic] =
        [
            new("One-quarter pound flour makes half of a batch needing one-half pound. 1=yes,0=no.",1,"1/4÷1/2=1/2."),
            new("Three-quarters pound makes one and one-half batches needing half-pound each. 1=yes,0=no.",1,"3/4÷1/2=1.5."),
            new("One pound flour makes only half a batch needing half-pound each. 1=yes,0=no.",0,"Makes2 batches."),
            new("Whiskers2⅔kg is2/3 as heavy as Piglio4kg. 1=yes,0=no.",1,"8/3÷4=2/3."),
            new("Piglio4kg is1½ as heavy as Whiskers2⅔kg. 1=yes,0=no.",1,"4÷8/3=1.5."),
            new("The equation ?×1⅝=1/3 asks what fraction of Andre's trip has been walked. 1=yes,0=no.",1,"Part divided by total."),
            new("The equation ?×1⅝=1/3 asks what trip distance remains in kilometers. 1=yes,0=no.",0,"It asks fractional portion."),
            new("Four divided by2/7 asks how many two-seventh-sized groups fit into four. 1=yes,0=no.",1,"Original choice B."),
            new("Four divided by2/7 is equal to4×2/7. 1=yes,0=no.",0,"Division uses reciprocal."),
            new("If an amount is smaller than one whole group, the number of groups can be between0 and1. 1=yes,0=no.",1,"Fractional group."),
            new("A group-count model uses group count×group size=total amount. 1=yes,0=no.",1,"Multiplicative structure."),
            new("11/4 divided by3/4 gives11/3 groups. 1=yes,0=no.",1,"Cancel quarters."),
            new("The original unit whole matters when describing a fraction of a group. 1=yes,0=no.",1,"Reference and scale must be defined."),
            new("A small positive dividend divided by a larger positive divisor yields a quotient greater than1. 1=yes,0=no.",0,"Quotient below1.")
        ],
        [L0408GroupSizeNumbers] =
        [
            new("One cup strawberries makes3/4 cake. Cups for full cake in THIRD-CUP units?",4,"1÷3/4=4/3cups."),
            new("1½cups raspberries make3/4 cake. Cups for full cake?",2,"1.5÷0.75=2."),
            new("A painter covers9/2 square yards with3 gallons. Gallons per square yard in THIRD-GALLON units?",2,"3/(9/2)=2/3gallons per sqyd."),
            new("A painter covers9/2 square yards with3 gallons. Square yards per gallon in HALF-SQUARE-YARD units?",3,"(9/2)/3=3/2sqyd per gallon."),
            new("Han walks1/4mile and is1/3 of way to school. Full distance in QUARTER-MILE units?",3,"1/4÷1/3=3/4mile,3quarters."),
            new("Four fifths divided by two thirds: answer in FIFTH-GROUP UNITS?",6,"(4/5)/(2/3)=6/5groups."),
            new("Books1.5in wide fill a36in shelf. How many books?",24,"36÷1.5=24."),
            new("Fifty-six divided by8 equals?",7,"56/8=7."),
            new("56 divided by8000000 in MILLIONTHS?",7,"56/8000000=7/1000000."),
            new("56 divided by0.000008 equals?",7000000,"56×125000=7000000."),
            new("Choose a divisor16 for56÷16. Answer in HALF-UNITS?",7,"56/16=3.5, or7 halves."),
            new("One cup enough for3/4 cake. How many cups for half cake in THIRD-CUP units?",2,"Full4/3 cups; half2/3 cups."),
            new("A wall covers4.5 square yards with3 gallons. Gallons per1.5square yards?",1,"Divide into3 sections1.5sqyd each."),
            new("A cake requires4/3 cups strawberries. 3 whole cakes need how many cups?",4,"3×4/3=4."),
            new("Raspberry cake needs2 cups per cake. 5 cakes need how many cups?",10,"2×5=10."),
            new("If Han's full journey3/4mile and he walked1/4mile, remaining distance in QUARTER-MILE units?",2,"3/4-1/4=2/4 mile.")
        ],
        [L0408GroupSizeLogic] =
        [
            new("One cup for3/4 cake means4/3 cups for full cake. 1=yes,0=no.",1,"1÷3/4=4/3."),
            new("One and one-half cups for3/4 cake means2 cups per full cake. 1=yes,0=no.",1,"1.5÷.75=2."),
            new("Three gallons for4.5square yards means2/3 gallon per square yard. 1=yes,0=no.",1,"3/4.5=2/3."),
            new("One-quarter mile is1/3 of3/4mile. 1=yes,0=no.",1,"(1/4)/(3/4)=1/3."),
            new("Four-fifths divided by two-thirds equals6/5. 1=yes,0=no.",1,"Multiply by reciprocal."),
            new("Twenty-four books of1.5in each fit a36in shelf. 1=yes,0=no.",1,"24×1.5=36."),
            new("56÷8million is much less than1. 1=yes,0=no.",1,"Seven millionths."),
            new("56÷0.000008 is far greater than1. 1=yes,0=no.",1,"7million."),
            new("56÷8 equals7, so it is between1 and7. 1=yes,0=no.",0,"Equals7, not strictly less."),
            new("56÷16 equals3.5, strictly between1 and7. 1=yes,0=no.",1,"1<3.5<7."),
            new("If size of each group is unknown, total÷number of groups finds the group size. 1=yes,0=no.",1,"Partition interpretation."),
            new("For fractions, an amount can be less than the size of a whole group, requiring quotient below1. 1=yes,0=no.",1,"Fraction of group."),
            new("A picture-based tape diagram should identify which section is one whole. 1=yes,0=no.",1,"Reference needed."),
            new("A numeric quotient alone proves a student's original drawn source diagram accurate. 1=yes,0=no.",0,"Teacher verifies diagram.")
        ],
        [L0409SourceWordValues] =
        [
            new("Friends share2½lb berries, each gets5/4lb. Number of friends?",2,"(5/2)/(5/4)=2."),
            new("Friends share2½lb berries equally among5. Pounds per person in HALF-POUND UNITS?",1,"2.5/5=0.5lb."),
            new("Two-fifths kg soil is one-third of container. Full container kg in FIFTHS?",6,"2/5÷1/3=6/5kg."),
            new("A container holds6/5kg. Can1kg fit? Enter1 yes,0 no.",1,"1<6/5."),
            new("Three quarters hour rain fills2/5 gauge. Minutes elapsed?",45,"3/4hour=45min."),
            new("Rain fills2/5 gauge in45min. Filled fraction after60min in FIFTEENTHS?",8,"2/5×60/45=8/15."),
            new("Three museum tickets cost1275cents. Price per ticket in CENTS?",425,"1275/3=425."),
            new("Three museum tickets cost1275cents. Five tickets cost in CENTS?",2125,"425×5=2125."),
            new("Elena walks60m in15sec. Meters per second?",4,"60/15=4."),
            new("Noah walks50m in10sec. Meters per second?",5,"50/10=5."),
            new("Who faster at4m/s Elena or5m/s Noah? Enter1 Elena,2 Noah.",2,"Noah5>4."),
            new("Trail mix1 batch uses2cups cereal. Four batches use cereal CUPS?",8,"2×4=8."),
            new("Trail mix1 batch uses1/3cup almonds. Three batches use almonds CUPS?",1,"3×1/3=1."),
            new("Trail mix1 batch uses1/4cup raisins. Four batches use raisins CUPS?",1,"4×1/4=1."),
            new("Trail mix1 batch uses1/3cup almonds. Two batches in THIRD-CUP portions?",2,"2/3cups."),
            new("Trail mix1 batch uses1/4cup raisins. Three batches in QUARTER-CUP portions?",3,"3/4cup."),
            new("Trail mix1 batch uses2cups cereal. Three batches use cereal CUPS?",6,"3×2=6."),
            new("Rain fills2/5 gauge in3/4hour. Fraction filled in15minutes in FIFTEENTHS?",2,"2/5×15/45=2/15.")
        ],
        [L0409ModelReasoning] =
        [
            new("2½lb split into5/4lb groups gives2groups. 1=yes,0=no.",1,"2.5/1.25=2."),
            new("2½lb split among5 people gives½lb each. 1=yes,0=no.",1,"2.5/5=.5."),
            new("2/5kg fills1/3 of a container, so full holds6/5kg. 1=yes,0=no.",1,"2/5÷1/3=6/5."),
            new("One kg fits in a6/5kg capacity container. 1=yes,0=no.",1,"1<1.2."),
            new("Rain gauge2/5 full at45min will be8/15 full after60min at steady rainfall. 1=yes,0=no.",1,"2/5×4/3=8/15."),
            new("The equation3/4÷2/5 directly gives fraction filled at60minutes. 1=yes,0=no.",0,"It gives time per full-gauge scale, not filled fraction at60min."),
            new("Three tickets cost12.75dollars, so five cost21.25 at constant rate. 1=yes,0=no.",1,"4.25 each."),
            new("Noah5m/sec is faster than Elena4m/sec. 1=yes,0=no.",1,"Speed per equal second."),
            new("Trail mix cereal amount for4 batches is8cups at2cups/batch. 1=yes,0=no.",1,"2×4=8."),
            new("Three batches of trail mix at1/3cup almonds per batch require1cup almonds. 1=yes,0=no.",1,"3×1/3=1."),
            new("Four batches each1/4cup raisins require1cup. 1=yes,0=no.",1,"4×1/4=1."),
            new("An unscripted fraction word problem may require deciding which quantity represents groups. 1=yes,0=no.",1,"Contextual interpretation."),
            new("A tape diagram can help check whether a model treats rate as constant. 1=yes,0=no.",1,"Corresponding segments."),
            new("Rainfall must remain steady to infer future gauge fill from a short observation. 1=yes,0=no.",1,"Constant-rate assumption.")
        ],
        [L0410AlgorithmValues] =
        [
            new("24 apples divided into half-apple shares. Number of shares?",48,"24÷1/2=48."),
            new("One divided by1/10 equals?",10,"1×10=10."),
            new("Four divided by1/10 equals?",40,"4×10=40."),
            new("Eighteen divided by1/10 equals?",180,"18×10=180."),
            new("Four divided by2/10 equals?",20,"4×10/2=20."),
            new("Four divided by8/10 equals?",5,"4×10/8=5."),
            new("Five divided by1/10 equals?",50,"5×10=50."),
            new("Five divided by3/10, answer in THIRDS?",50,"5×10/3=50/3."),
            new("Five divided by9/10, answer in NINTHS?",50,"5×10/9=50/9."),
            new("Two and a half divided by1/8 equals?",20,"5/2×8=20."),
            new("Two and a half divided by5/8 equals?",4,"5/2×8/5=4."),
            new("Road paving3/5km per week. Time to pave1km in THIRD-WEEK UNITS?",5,"1÷3/5=5/3weeks."),
            new("Pancake mix1¾lb; using7/8lb. Fraction used in HALF-UNIT FRACTIONS?",1,"7/8÷7/4=1/2."),
            new("Twenty-five percent of400?",100,"One-quarter400."),
            new("Fifty percent of90?",45,"One-half90."),
            new("Seventy-five percent of200?",150,"Three quarters200."),
            new("Ten percent of8000?",800,"One tenth."),
            new("Five percent of20?",1,"One twentieth."),
            new("4÷0.2 gives?",20,"4÷1/5=20."),
            new("4÷0.8 gives?",5,"4÷4/5=5.")
        ],
        [L0410AlgorithmLogic] =
        [
            new("Dividing a positive number by1/10 is the same as multiplying by10. 1=yes,0=no.",1,"Reciprocal of1/10."),
            new("24÷1/2=12. 1=yes,0=no.",0,"24 half-apple shares double to48."),
            new("4÷2/10 equals20. 1=yes,0=no.",1,"Multiply by10 and divide by2."),
            new("4÷8/10 equals5. 1=yes,0=no.",1,"4×10/8=5."),
            new("To divide by2/5, multiply by5 and then divide by2. 1=yes,0=no.",1,"Reciprocal5/2."),
            new("5÷3/10 equals50/3. 1=yes,0=no.",1,"5×10/3."),
            new("5÷9/10 equals50/9. 1=yes,0=no.",1,"5×10/9."),
            new("2½÷5/8 equals4. 1=yes,0=no.",1,"20÷5=4."),
            new("Paving3/5km per week takes5/3 weeks per kilometer at a constant rate. 1=yes,0=no.",1,"1/(3/5)=5/3."),
            new("A box1¾lb of pancake mix with7/8lb used has half used. 1=yes,0=no.",1,"7/8÷14/8=1/2."),
            new("The numerator and denominator of a nonzero divisor fraction can be swapped to multiply by the reciprocal. 1=yes,0=no.",1,"a÷(b/c)=a×(c/b)."),
            new("Dividing by a fraction less than1 always makes the quotient smaller for positive dividend. 1=yes,0=no.",0,"It gets larger."),
            new("A tape diagram and reciprocal algorithm should produce the same correct quotient. 1=yes,0=no.",1,"Equivalent reasoning."),
            new("One must avoid using0 as the divisor of a fraction-division expression. 1=yes,0=no.",1,"Division by zero undefined.")
        ],
        [L0411DivisionFluency] =
        [
            new("14/15 divided by7/5, answer in THIRDS?",2,"14/15×5/7=2/3."),
            new("4/3 divided by5/2, answer in FIFTEENTHS?",8,"4/3×2/5=8/15."),
            new("15/4 divided by5/8 equals?",6,"15/4×8/5=6."),
            new("Kiran has2¾lb flour and fills4⅛ equal bags. Pounds per bag in THIRDS?",2,"(11/4)/(33/8)=2/3lb."),
            new("Four and a half divided by1/8 equals?",36,"9/2×8=36."),
            new("Four and a half divided by1/4 equals?",18,"9/2×4=18."),
            new("Four and a half divided by1/6 equals?",27,"9/2×6=27."),
            new("A phone reaches2/5 charged in1/3hour at constant rate. Time to full in MINUTES?",50,"(1/3)/(2/5)=5/6h=50min."),
            new("A bucket2/5 full holds2½lb water. Weight when full in QUARTER-POUND units?",25,"5/2÷2/5=25/4lb."),
            new("14/15 divided by7/5: multiplying first by5 and then dividing by7 gives numerator in THIRDS?",2,"14/15×5÷7=2/3."),
            new("4/3 divided by5/2: reciprocal multiplier denominator?",5,"Reciprocal of5/2 is2/5."),
            new("4/3 divided by5/2: reciprocal multiplier numerator?",2,"Reciprocal is2/5."),
            new("A phone needs50min per charge. Time for3 charges at same constant model in MINUTES?",150,"3×50=150."),
            new("A bucket full holds25/4lb. Half-full water in EIGHTH-POUND units?",25,"Half of25/4=25/8lb."),
            new("Two and three-quarters pounds is how many QUARTER-POUND portions?",11,"11/4 divided by1/4=11."),
            new("Four and one-eighth bags expressed in EIGHTH-BAG portions?",33,"4⅛=33/8."),
            new("A fraction6/5 is what number of FIFTHS?",6,"Six fifths.")
        ],
        [L0411DivisionLogic] =
        [
            new("14/15÷7/5=2/3. 1=yes,0=no.",1,"14/15×5/7=2/3."),
            new("For14/15÷7/5, multiply14/15 by5 and then by1/7; valid. 1=yes,0=no.",1,"Original option A."),
            new("For14/15÷7/5, multiply14/15 by5 then divide7; valid. 1=yes,0=no.",1,"Original option D."),
            new("For14/15÷7/5, multiply14/15 by7 then divide5; valid. 1=yes,0=no.",0,"Incorrect reciprocal."),
            new("Clare4/3÷5/2=10/3 is correct. 1=yes,0=no.",0,"Correct8/15."),
            new("Dividing4/3 by5/2 means multiply4/3 by2/5. 1=yes,0=no.",1,"Reciprocal algorithm."),
            new("15/4 divided by5/8 equals6. 1=yes,0=no.",1,"15/4×8/5=6."),
            new("2¾lb divided into4⅛ equal bags yields2/3lb per bag. 1=yes,0=no.",1,"11/4÷33/8=2/3."),
            new("4½ divided by1/8 equals36. 1=yes,0=no.",1,"9/2×8=36."),
            new("At2/5 charge in1/3hour, constant rate full charge time is5/6hour. 1=yes,0=no.",1,"1/3÷2/5=5/6."),
            new("For phone fraction charged2/5 in1/3hour, 2/5×?=1/3 is a valid equation for full time. 1=yes,0=no.",1,"Unknown time5/6."),
            new("For phone fraction charged2/5 in1/3hour, 2/5÷1/3 directly gives full time. 1=yes,0=no.",0,"It gives fraction per hour6/5."),
            new("2/5 of a full bucket weighs2.5lb, so full bucket water weighs6.25lb. 1=yes,0=no.",1,"2.5÷.4=6.25."),
            new("Multiplying by a fraction's reciprocal works only when the divisor is nonzero. 1=yes,0=no.",1,"Zero denominator not allowed.")
        ],
        [L0412LengthValues] =
        [
            new("One inch is about2 11/20cm. Three inches in TWENTIETHS of a centimeter?",153,"3×51/20=153/20cm."),
            new("One inch is about51/20cm. One centimeter as fraction of inch in FIFTY-FIRSTS?",20,"1÷51/20=20/51in."),
            new("Ten cm divided by2 11/20cm per inch: inches in FIFTY-FIRSTS?",200,"10×20/51=200/51in."),
            new("Zookeeper6¼ft, giraffe9⅜ft. Giraffe height compared with zookeeper in HALF-UNITS?",3,"(75/8)/(25/4)=3/2."),
            new("Zookeeper6¼ft, giraffe9⅜ft. Zookeeper height as fraction of giraffe in THIRDS?",2,"(25/4)/(75/8)=2/3."),
            new("A rectangular tile is1½ft long, bathroom length10½ft. Number of tiles along length?",7,"10.5/1.5=7."),
            new("Tile span1½ft, floor width6½ft. Number of TILE SPANS in THIRDS?",13,"6.5/1.5=13/3 tile lengths."),
            new("Floor width6½ft and tile side1½ft. Minimum WHOLE tiles across row if pieces may not combine?",5,"Need cover4⅓ tile spans, round up5 tiles."),
            new("In3/4cup oatmeal there is1/10 daily iron. Daily iron fraction per1cup in FIFTEENTHS?",2,"(1/10)/(3/4)=2/15."),
            new("What fraction of1/2 is1/3? Answer in THIRDS?",2,"(1/3)/(1/2)=2/3."),
            new("How many groups of4/5 are in2? Answer in HALF-GROUP UNITS?",5,"2÷4/5=5/2 groups."),
            new("The zookeeper6¼ft tall expressed in QUARTER-FOOT portions?",25,"25/4feet."),
            new("Giraffe9⅜ft expressed in EIGHTH-FOOT portions?",75,"75/8feet."),
            new("3 inches at2.55cm per inch, in HUNDREDTHS of centimeter?",765,"3×2.55=7.65cm."),
            new("A floor is10½ft long with tiles1½ft each. Total feet of7 tile lengths in HALVES?",21,"7×1.5=10.5ft=21 halves."),
            new("One cup iron fraction2/15. Three cups iron fraction in FIFTHS?",2,"3×2/15=2/5."),
            new("A box6 feet high compared with a1½-foot item. Multiplicative ratio?",4,"6/1.5=4.")
        ],
        [L0412LengthReasoning] =
        [
            new("A giraffe9⅜ft is one and a half times6¼ft. 1=yes,0=no.",1,"9.375/6.25=1.5."),
            new("The zookeeper6¼ft is two-thirds as tall as the giraffe9⅜ft. 1=yes,0=no.",1,"6.25/9.375=2/3."),
            new("Three inches at2.55cm per inch are7.65cm. 1=yes,0=no.",1,"3×2.55."),
            new("One centimeter is20/51 of an inch given51/20cm per inch. 1=yes,0=no.",1,"Reciprocal."),
            new("Seven tiles of1.5ft span exactly10.5ft. 1=yes,0=no.",1,"7×1.5=10.5."),
            new("Six and a half feet contains exactly4 whole1.5ft spans with no remainder. 1=yes,0=no.",0,"There are4⅓ spans."),
            new("Across6.5ft using1.5ft squares, at least5 tile-length positions are required if no reused cuts. 1=yes,0=no.",1,"Round up from4⅓."),
            new("Three quarters cup containing1/10 daily iron implies1cup contains2/15. 1=yes,0=no.",1,"1/10÷3/4=2/15."),
            new("One third is two thirds of one half. 1=yes,0=no.",1,"1/3÷1/2=2/3."),
            new("Two contains two and a half groups of4/5. 1=yes,0=no.",1,"2÷4/5=2.5."),
            new("Comparison questions 'how many times as tall' can be modeled by dividing heights in same units. 1=yes,0=no.",1,"Multiplicative comparison."),
            new("A number of tile-span lengths always equals the integer number of physical uncut tiles purchased. 1=yes,0=no.",0,"Fractional spans and cuts differ."),
            new("A source ruler with approximate inch-centimeter factor should be marked approximate, not an exact standard. 1=yes,0=no.",1,"2.55 is approximate."),
            new("Original source diagrams can be evaluated using a numeric quotient only without checking lengths. 1=yes,0=no.",0,"Teacher checks representations.")
        ],
        [L0413AreaValues] =
        [
            new("Rectangle area11square meters, known side3⅔meters. Other side in METERS?",3,"11÷11/3=3."),
            new("TV area462square inches and length16½in. Width in INCHES?",28,"462÷16.5=28."),
            new("Rectangle area17½square inches, shorter side3½in. Longer side INCHES?",5,"17.5÷3.5=5."),
            new("A floor is12ft by15ft. Total area in SQUARE FEET?",180,"12×15=180."),
            new("Square tile side1⅓ft. Its area in NINTHS OF SQUARE FOOT?",16,"(4/3)²=16/9."),
            new("Floor length12ft with tile edge1⅓ft. Number of complete tile lengths?",9,"12÷4/3=9."),
            new("Floor width15ft with tile edge1⅓ft. Number of tile-edge spans in QUARTERS?",45,"15÷4/3=45/4."),
            new("42inch bookshelf with1½inch books fits how many BOOKS?",28,"42÷1.5=28."),
            new("Five shelves each42in long. Total shelf space in INCHES?",210,"5×42=210."),
            new("Five shelves each42in long. Total shelf space in HALF-FEET?",35,"210in=17.5feet=35 half-feet."),
            new("5/32 divided by25/4, quotient in FORTIETHS?",1,"(5/32)×(4/25)=1/40."),
            new("How many1⅔ groups in1⅚? Answer in TENTHS?",11,"11/6÷5/3=11/10."),
            new("How many1⅔ groups in4⅓? Answer in FIFTHS?",13,"13/3÷5/3=13/5."),
            new("How many1⅔ groups in5/6? Answer in HALVES?",1,"5/6÷5/3=1/2."),
            new("A hose fills3gallons in1¼minutes. Seconds to fill50gallons at same rate?",1250,"50×(5/4)/3×60=1250 seconds."),
            new("A rectangle3m by3⅔m has area in SQUARE METERS?",11,"3×11/3=11."),
            new("TV width28in and length16½in. Area in SQUARE INCHES?",462,"28×16.5=462."),
            new("Original area11m² with one side11/3m. Unknown side in THIRD-METER units?",9,"3meters=9 thirds.")
        ],
        [L0413AreaLogic] =
        [
            new("A rectangle of area11m² and side3⅔m has other side3m. 1=yes,0=no.",1,"11÷11/3=3."),
            new("TV equation16½×w=462 can represent its area. 1=yes,0=no.",1,"Original option B."),
            new("TV equation462÷16½=w can represent its missing width. 1=yes,0=no.",1,"Original option C."),
            new("TV equation462÷w=16½ can represent its length. 1=yes,0=no.",1,"Original option D."),
            new("TV equation w×462=16½ correctly relates original dimensions. 1=yes,0=no.",0,"Original option A invalid."),
            new("A rectangular area17½in² divided by side3½in gives other side5in. 1=yes,0=no.",1,"17.5/3.5=5."),
            new("A square with side1⅓ft has area16/9 square feet. 1=yes,0=no.",1,"4/3 squared."),
            new("A12ft by15ft floor has180sqft area. 1=yes,0=no.",1,"12×15=180."),
            new("The fraction101¼ tile-areas can itself be purchased as101¼ whole uncut physical tiles without cutting. 1=yes,0=no.",0,"Geometric area units differ from purchasing constraints."),
            new("A floor width15ft requires11¼ tile-width spans of1⅓ft each. 1=yes,0=no.",1,"15÷4/3=45/4."),
            new("Five shelves each42in contain17½feet of total length. 1=yes,0=no.",1,"210in/12=17.5ft."),
            new("The dimensions of a rectangle should share units before calculating the product area. 1=yes,0=no.",1,"Convert lengths consistently."),
            new("A diagram with a fractional side can be verified by multiply side lengths to get area. 1=yes,0=no.",1,"Inverse operations."),
            new("Teacher inspection of the actual tile layout is needed before claiming an exact purchase count. 1=yes,0=no.",1,"Cuts and reuse assumptions matter.")
        ],
        [L0414GeometryNumbers] =
        [
            new("A larger cube has4inch sides and is built of1/2inch cubes. Number of cubes along one edge?",8,"4/(1/2)=8."),
            new("A larger cube has4inch sides and is built of1/2inch cubes. Total small cubes?",512,"8³=512."),
            new("Triangle area7⅞cm² and base5¼cm. Height in CENTIMETERS?",3,"2×(63/8)/(21/4)=3."),
            new("Prism1½in by2in by2½in. Volume in HALF-CUBIC-INCH UNITS?",15,"1.5×2×2.5=7.5in³=15 halves."),
            new("Prism1½in by2in by2½in, packed with 1/2inch cubes. Number of small cubes?",60,"3×4×5=60 cubes."),
            new("One cube of side1/2inch. Volume in EIGHTH-CUBIC-INCH UNITS?",1,"(1/2)³=1/8in³."),
            new("A prism5×5×8 units. Its volume in CUBIC UNITS?",200,"5×5×8=200."),
            new("Prism5×5×8 packed with1/3-unit cubes. Number of little cubes?",5400,"15×15×24=5400."),
            new("Fence boards6¼in wide for150in fence with no gaps. Number of boards?",24,"150÷25/4=24."),
            new("Two and1/7 divided by2/7, answer in HALF-GROUPS?",15,"15/7÷2/7=15/2."),
            new("17/20 divided by1/4, answer in FIFTHS?",17,"17/20×4=17/5."),
            new("Bucket holds11⅔ gallons and is5/6 full. Full capacity in GALLONS?",14,"35/3÷5/6=14."),
            new("Gym has80 children,75% wearing socks. How many wearing socks?",60,"80×.75=60."),
            new("Gym has80 children,75% wearing socks. How many NOT wearing socks?",20,"80×.25=20."),
            new("Lin wants75 dollars and saved37.50. Percent saved?",50,"37.5/75=50%."),
            new("Noah wants60dollars and saved45. Percent saved?",75,"45/60=75%."),
            new("Lin saved50% of trip goal. Percent remaining?",50,"100-50=50%."),
            new("Noah saved75% of concert ticket goal. Percent remaining?",25,"100-75=25%."),
            new("Triangle with base5¼cm and height3cm has area in EIGHTH-SQUARE-CM units?",63,"5.25×3÷2=7.875=63/8."),
            new("A4-inch cube volume in CUBIC INCHES?",64,"4³=64.")
        ],
        [L0414GeometryLogic] =
        [
            new("A cube of side4in contains8 half-inch edges along each side. 1=yes,0=no.",1,"4÷0.5=8."),
            new("A cube of side4in contains512 smaller half-inch cubes. 1=yes,0=no.",1,"8³=512."),
            new("Triangle area7⅞cm² base5¼cm has perpendicular height3cm. 1=yes,0=no.",1,"A=bh/2."),
            new("A slanted triangle side can automatically be used as height even when not perpendicular to base. 1=yes,0=no.",0,"Height is perpendicular."),
            new("A prism1.5×2×2.5in has volume7.5 cubic inches. 1=yes,0=no.",1,"Multiply three edges."),
            new("A half-inch cube volume equals1/2 cubic inch. 1=yes,0=no.",0,"Cube volume1/8."),
            new("A1.5×2×2.5in prism contains60 half-inch cubes. 1=yes,0=no.",1,"3×4×5."),
            new("Five by five by eight prism can contain5400 cubes with edge1/3unit. 1=yes,0=no.",1,"15×15×24."),
            new("Multiplying whole-prism volume200 by27 gives count of1/3-unit cubes. 1=yes,0=no.",1,"Each tiny cube volume1/27."),
            new("A150inch fence with6.25inch boards and no gaps needs24 boards. 1=yes,0=no.",1,"150/6.25=24."),
            new("A bucket11⅔gallons at5/6 full has capacity14 gallons. 1=yes,0=no.",1,"11⅔÷5/6=14."),
            new("75% of80 kids are wearing socks so20 are not. 1=yes,0=no.",1,"25%×80=20."),
            new("Volume uses cubic units, not squared units. 1=yes,0=no.",1,"Three dimensions."),
            new("The small cube count equals the ratio of large volume to small cube volume. 1=yes,0=no.",1,"Volume quotient.")
        ],
        [L0415Volumes] =
        [
            new("Pool24ft by15ft, water height1⅓ft. Volume in CUBIC FEET?",480,"24×15×4/3=480."),
            new("Prism2⅖in×3⅕in×2in. Volume in TWENTY-FIFTHS of a cubic inch?",384,"12/5×16/5×2=384/25."),
            new("Prism2⅖in×3⅕in×2in. Count of1/5inch cubes along first edge?",12,"(12/5)/(1/5)=12."),
            new("Prism2⅖in×3⅕in×2in. Count1/5in cubes along second edge?",16,"(16/5)/(1/5)=16."),
            new("Prism2⅖in×3⅕in×2in. Count1/5in cubes along third edge?",10,"2/(1/5)=10."),
            new("Prism2⅖in×3⅕in×2in. Total count1/5inch unit cubes?",1920,"12×16×10=1920."),
            new("Prism2⅖in×3⅕in×2in. Total count2/5inch cubes?",240,"6×8×5=240."),
            new("A1/5inch cube has volume in ONE-HUNDRED-TWENTY-FIFTH cubic-inch units?",1,"1/5³=1/125."),
            new("A right triangle legs3/4 and1 unit. Area in EIGHTHS of sq unit?",3,"3/4×1÷2=3/8."),
            new("Right triangle area3/8 and hypotenuse base5/4, perpendicular height in FIFTHS?",3,"2×3/8÷5/4=3/5."),
            new("Salt lick cube edge5/12ft. Volume in 1728THS cubic feet?",125,"5³/12³=125/1728."),
            new("Salt lick packing box1¼ft×1⅔ft×5/6ft. Cubes along first edge of5/12ft?",3,"5/4÷5/12=3."),
            new("Salt lick packing box1¼ft×1⅔ft×5/6ft. Cubes along second edge?",4,"5/3÷5/12=4."),
            new("Salt lick packing box1¼ft×1⅔ft×5/6ft. Cubes along third edge?",2,"5/6÷5/12=2."),
            new("Salt lick packing box1¼ft×1⅔ft×5/6ft. Total count of5/12ft cubes?",24,"3×4×2=24."),
            new("How many groups of1/3inch fit in3/4inch? Answer in QUARTER-GROUP UNITS?",9,"3/4÷1/3=9/4."),
            new("One and2/5 groups of1⅔in. Length in THIRDS of inch?",7,"7/5×5/3=7/3in."),
            new("Art paste uses1cup flour per1/2cup water. Water cups for4flour?",2,"4×1/2=2."),
            new("Art paste1cup flour per1/2cup water. Flour cups with3cups water?",6,"3×2=6.")
        ],
        [L0415VolumeLogic] =
        [
            new("Pool24×15×1⅓feet contains480 cubic feet at given water height. 1=yes,0=no.",1,"24×15×4/3=480."),
            new("A prism2⅖×3⅕×2in has volume384/25in³. 1=yes,0=no.",1,"Multiply edge lengths."),
            new("A prism packed with1/5in cubes has1920 of them under stated dimensions. 1=yes,0=no.",1,"12×16×10."),
            new("A prism packed with2/5in cubes has240 of them under stated dimensions. 1=yes,0=no.",1,"6×8×5."),
            new("Larger2/5in cubes require MORE cubes than smaller1/5in cubes to fill same prism. 1=yes,0=no.",0,"Larger cubes take more volume each."),
            new("A1/5in unit cube has volume1/5 cubic inch. 1=yes,0=no.",0,"Volume1/125."),
            new("A right triangle with perpendicular legs3/4 and1 has area3/8 sq unit. 1=yes,0=no.",1,"Multiply legs÷2."),
            new("If triangle area3/8 and new base5/4, its perpendicular altitude is3/5. 1=yes,0=no.",1,"2A/b=3/5."),
            new("Salt lick cube side5/12ft has volume smaller than1 cubic foot. 1=yes,0=no.",1,"125/1728<1."),
            new("Salt lick shipping box with3×4×2 cube counts can hold24 equal cubes. 1=yes,0=no.",1,"Multiply fit counts."),
            new("A fractional prism volume requires expressing lengths in a common measurement unit. 1=yes,0=no.",1,"Unit consistency."),
            new("Cubic inches and square inches are interchangeable units when finding a prism's volume. 1=yes,0=no.",0,"Different dimensions."),
            new("One and two-fifths groups of1⅔in is7/3in. 1=yes,0=no.",1,"7/5×5/3=7/3."),
            new("1cup flour to1/2cup water means4cups flour require2cups water. 1=yes,0=no.",1,"Scale both×4.")
        ],
        [L0416AllOperations] =
        [
            new("Each orange yields1/4cup juice. Number of oranges needed for2½cups?",10,"2.5÷.25=10."),
            new("Hiking sign shows3/4mile back to parking and1½miles ahead to summit. Total hike in QUARTER-MILE UNITS?",9,"3/4+6/4=9/4=2.25mi."),
            new("Hiking sign3/4mi back1½mi ahead. Fraction of full hike already traveled in THIRDS?",1,"(3/4)/(9/4)=1/3."),
            new("Hiking sign3/4mi back1½mi ahead. Remaining as a MULTIPLE of distance walked?",2,"1.5÷0.75=2."),
            new("Hiking total2¼mi compared with3/4 already walked. Whole as multiple of walked?",3,"2.25÷.75=3."),
            new("Priya cat5½lb, dog8¼lb. Cat as fraction of dog in THIRDS?",2,"(11/2)/(33/4)=2/3."),
            new("Priya cat5½lb, dog8¼lb. Combined weight in QUARTER-POUND UNITS?",55,"11/2+33/4=55/4."),
            new("Priya cat5½lb, dog8¼lb. Dog heavier in QUARTER-POUND UNITS?",11,"33/4-22/4=11/4."),
            new("Ice box7½ft×6ft×6ft with cubes1½ft per edge. Cubes along first edge?",5,"7.5÷1.5=5."),
            new("Ice box7½ft×6ft×6ft with cubes1½ft per edge. Cubes along second edge?",4,"6÷1.5=4."),
            new("Ice box7½ft×6ft×6ft with cubes1½ft edge. Total cubes?",80,"5×4×4=80."),
            new("School club sold300shirts,31% to fifth graders. Number?",93,"300×.31=93."),
            new("School club sold300shirts,52% to sixth graders. Number?",156,"300×.52=156."),
            new("School club sold300shirts, remaining17% to teachers. Number?",51,"300×.17=51."),
            new("Jada pennies:dimes2:3, total55coins. Pennies?",22,"55÷5=11 per part;2×11=22."),
            new("Jada pennies:dimes2:3, total55coins. Dimes?",33,"3×11=33."),
            new("Jada22pennies33dimes. Total VALUE in CENTS?",352,"22×1+33×10=352."),
            new("Ratio of pennies:dimes2:3 with no total. Can exact coin counts be determined? Enter1 yes,0 no.",0,"Scale factor unknown."),
            new("Ten oranges supply how many QUARTER-CUP portions of juice?",10,"10×1/4cup=2.5cups."),
            new("Cubic ice blocks of edge1½ft have volume in EIGHTH-CUBIC-FT UNITS?",27,"(3/2)³=27/8ft³.")
        ],
        [L0416OperationsLogic] =
        [
            new("2½cups orange juice at1/4cup per orange requires10 oranges. 1=yes,0=no.",1,"Divide target by per-orange amount."),
            new("The equation?×1/4=2½ models required oranges. 1=yes,0=no.",1,"Original option A."),
            new("The equation2½÷1/4=? models required oranges. 1=yes,0=no.",1,"Original option D."),
            new("The equation1/4÷2½=? models required oranges. 1=yes,0=no.",0,"Wrong direction."),
            new("At hiking sign3/4mi back to lot and1½mi ahead to summit, distance already walked is1/3 of whole. 1=yes,0=no.",1,"3/4 of9/4 total."),
            new("At hiking sign, distance remaining is twice the already walked distance. 1=yes,0=no.",1,"1.5/.75=2."),
            new("At hiking sign, total hike is triple distance already walked. 1=yes,0=no.",1,"2.25/.75=3."),
            new("Cat5½lb is2/3 as heavy as dog8¼lb. 1=yes,0=no.",1,"5.5/8.25=2/3."),
            new("Dog8¼lb is2¾lb heavier than cat5½lb. 1=yes,0=no.",1,"8.25-5.5=2.75."),
            new("An ice box7½×6×6ft can hold80 cubes of1½ft edges with no gaps. 1=yes,0=no.",1,"5×4×4."),
            new("300shirts 31% fifth+52% sixth leaves17% teachers. 1=yes,0=no.",1,"100-83=17%."),
            new("From pennies:dimes2:3 alone, exact coin counts can be determined without a total. 1=yes,0=no.",0,"Only proportion, not scale."),
            new("With55coins at ratio2:3, Jada has22pennies and33dimes. 1=yes,0=no.",1,"11 per ratio unit."),
            new("The value of22pennies and33dimes is3 dollars52 cents. 1=yes,0=no.",1,"22+330=352cents."),
            new("Choice of fraction operation depends on whether total, difference, rate or number of groups is unknown. 1=yes,0=no.",1,"Meaning drives operation.")
        ],
        [L0417BoundedPacking] =
        [
            new("An artist must ship270 jewelry boxes. SMALL shipping box can fit3×3×2 boxes under specified orientation. Capacity per carton?",18,"3×3×2=18."),
            new("A SMALL carton fits18 jewelry boxes. Cartons needed for270 jewelry boxes?",15,"270/18=15."),
            new("SMALL cartons each cost680cents. Shipping270 items using15 cartons costs how many CENTS?",10200,"15×680=10200cents."),
            new("MEDIUM 1 orientation fits6×11×2 jewelry boxes. Capacity?",132,"6×11×2=132."),
            new("MEDIUM 1 cartons fit132 jewelry boxes each. How many cartons for270?",3,"Ceiling270/132=3."),
            new("Three MEDIUM 1 shipping cartons at1345cents each cost CENTS?",4035,"3×1345=4035."),
            new("MEDIUM 2 orientation fits6×4×6 jewelry boxes. Capacity?",144,"6×4×6=144."),
            new("MEDIUM 2 cartons fit144 per carton. How many needed for270?",2,"Ceiling270/144=2."),
            new("Two MEDIUM 2 cartons at1345cents each cost CENTS?",2690,"2×1345=2690."),
            new("LARGE carton chosen orientation fits16×5×3 jewelry boxes. Capacity?",240,"16×5×3=240."),
            new("LARGE cartons fit240 each. How many needed for270?",2,"Ceiling270/240=2."),
            new("Two LARGE shipping cartons at1875cents each cost CENTS?",3750,"2×1875=3750."),
            new("Two MEDIUM 2 cartons can hold how many jewelry boxes with144-per-carton packing?",288,"2×144=288."),
            new("If two MEDIUM 2 cartons hold288 in total but only270 ordered, unused item SLOTS?",18,"288-270=18 slots."),
            new("One MEDIUM 2 carton fits144 and one MEDIUM 1 fits132. Combined capacity?",276,"144+132=276."),
            new("One MEDIUM 2 and one MEDIUM 1 at1345cents each cost CENTS?",2690,"2×1345=2690."),
            new("One LARGE carton fits240, one MEDIUM 1 fits132. Combined capacity?",372,"240+132=372."),
            new("One LARGE and one MEDIUM 1 cost CENTS at1875 and1345cents?",3220,"1875+1345=3220."),
            new("A jewelry box has edge lengths1¾in,2¼in,¾in. Volume in SIXTY-FOURTHS OF CUBIC INCH?",189,"7/4×9/4×3/4=189/64in³."),
            new("Small box5⅜in along a1¾in jewelry-box edge. Maximum full boxes along this axis?",3,"5.375/1.75=3.071, floor3."),
            new("Small box8⅝in along a2¼in jewelry-box edge. Maximum full boxes along this axis?",3,"8.625/2.25=3.833, floor3."),
            new("Small box1⅝in along a¾in jewelry-box edge. Maximum full boxes along this axis?",2,"1.625/.75=2.166, floor2."),
            new("Large box12in edge along¾in jewelry-box edge. Count along that axis?",16,"12/.75=16."),
            new("Large box12in edge along2¼in jewelry-box edge. Count along that axis?",5,"12/2.25=5.333, floor5."),
            new("Large box5½in edge along1¾in jewelry-box edge. Count along that axis?",3,"5.5/1.75=3.14, floor3.")
        ],
        [L0417PackingModelLogic] =
        [
            new("Under one fixed orientation, per-axis full-box counts can be multiplied to get a rectangular grid capacity. 1=yes,0=no.",1,"Product of three integer packing counts."),
            new("Jewelry boxes may have different packing capacities when rotated. 1=yes,0=no.",1,"Different edge alignment."),
            new("A capacity of144 per carton can ship270 items using just1 carton. 1=yes,0=no.",0,"Need2."),
            new("Two cartons each144 capacity can hold288 jewelry boxes. 1=yes,0=no.",1,"2×144=288."),
            new("At the historical source classroom prices, two medium2 cartons cost26.90 dollars. 1=yes,0=no.",1,"2×13.45=26.90."),
            new("Two medium2 cartons at historical prices are cheaper than two large cartons at18.75 each. 1=yes,0=no.",1,"26.90<37.50."),
            new("A chosen fixed orientation costing26.90 dollars proves global optimality among all mixed packing schemes and live USPS rates. 1=yes,0=no.",0,"This is a bounded model, not complete optimization or current pricing."),
            new("Small box packing counts3×3×2 imply18 jewelry boxes per carton. 1=yes,0=no.",1,"Product18."),
            new("Two medium1 cartons each132 would hold270 jewelry boxes. 1=yes,0=no.",0,"264<270; need3."),
            new("One medium2 capacity144 and one medium1 capacity132 total276, enough for270. 1=yes,0=no.",1,"276≥270."),
            new("Packaging material thickness, damaged capacity and real postal restrictions may change a physical shipping decision. 1=yes,0=no.",1,"Assumptions matter."),
            new("The source's historical flat-rate prices should be treated as verified current USPS prices in2026. 1=yes,0=no.",0,"Historical classroom dataset, not live prices."),
            new("A student-created orientation drawing needs human assessment even when a per-axis multiplication is correct. 1=yes,0=no.",1,"Actual packing feasibility must be reviewed."),
            new("For270 boxes with carton capacity144, the count is rounded UP to2 cartons. 1=yes,0=no.",1,"Ceiling, not rounding to nearest."),
            new("The cheapest plan among only four uniform-orientation illustrated methods may not be the cheapest unconstrained plan. 1=yes,0=no.",1,"Mixed orientations and cartons may change optimum.")
        ],
        [L0501DecimalArithmetic] =
        [
            new("Mai has1450 cents, spends435 cents at snack bar and525 at arcade. Amount LEFT in CENTS?",490,"1450-435-525=490 cents."),
            new("A large pizza costs750 cents. With4000 cents, maximum number of WHOLE pizzas?",5,"4000/750=5 and remainder250 cents."),
            new("Five pizzas at750cents each cost how many CENTS?",3750,"5×750=3750."),
            new("Adults tickets550cents, students425cents. Two adult tickets in CENTS?",1100,"2×550=1100."),
            new("Three student tickets425cents each cost in CENTS?",1275,"3×425=1275."),
            new("Two adults at550cents plus3 students at425cents cost in CENTS?",2375,"1100+1275=2375."),
            new("Family paid2500cents for2375cents tickets. Change in CENTS?",125,"2500-2375=125."),
            new("Chicken costs320cents/lb. Three pounds cost in CENTS?",960,"320×3=960."),
            new("Beef costs459cents/lb. Three pounds cost in CENTS?",1377,"459×3=1377."),
            new("Three pounds of beef versus chicken; difference in CENTS?",417,"1377-960=417."),
            new("A bottle holds1½L, glass holds1/5L. How many glasses in HALF-GLASS UNITS?",15,"3/2÷1/5=15/2 glasses."),
            new("A jug holds16L, bottle1½L. Number of bottles in THIRD-BOTTLE UNITS?",32,"16÷3/2=32/3 bottles."),
            new("Triangle base5½ units area19¼ units². Height in UNITS?",7,"2×19.25/5.5=7."),
            new("Five sixths divided by1/6 equals?",5,"5/6×6=5."),
            new("One and1/6 divided by1/12 equals?",14,"7/6×12=14."),
            new("10/6 divided by1/24 equals?",40,"10/6×24=40."),
            new("A chicken price320cents/lb. Price per pound in DOLLAR-CENTS?",320,"Money amount in cents is320."),
            new("Mai has1450cents and spends960cents total. Remainder CENTS?",490,"1450-960=490.")
        ],
        [L0501DecimalEstimation] =
        [
            new("Mai spent4.35+5.25=9.60 dollars. 1=yes,0=no.",1,"435+525=960cents."),
            new("Mai has4.90 dollars left from14.50 after spending4.35 and5.25. 1=yes,0=no.",1,"14.50-9.60=4.90."),
            new("40 dollars buys6 whole pizzas priced7.50 dollars each. 1=yes,0=no.",0,"6×7.50=45>40."),
            new("40 dollars buys5 pizzas at7.50 dollars each. 1=yes,0=no.",1,"5×7.50=37.50."),
            new("Two adult tickets and3 student tickets cost23.75 dollars. 1=yes,0=no.",1,"11+12.75=23.75."),
            new("When ticket cost23.75 is paid with25 dollars, change is1.25. 1=yes,0=no.",1,"25-23.75=1.25."),
            new("Three pounds of beef priced4.59/lb cost13.77 dollars. 1=yes,0=no.",1,"3×4.59=13.77."),
            new("Three pounds of chicken priced3.20/lb cost9.60 dollars. 1=yes,0=no.",1,"3×3.20=9.60."),
            new("Three pounds of beef cost4.17 dollars more than chicken at source prices. 1=yes,0=no.",1,"13.77-9.60=4.17."),
            new("An estimate can help reject an impossible total before exact decimal calculation. 1=yes,0=no.",1,"Benchmark estimate validates reasonableness."),
            new("Money units must be consistent when combining dollars and cents. 1=yes,0=no.",1,"Convert to same unit."),
            new("1½L bottle fills7½ one-fifth-liter glasses. 1=yes,0=no.",1,"1.5/.2=7.5."),
            new("Two triangles with same base5½ and perpendicular height7 have area19¼. 1=yes,0=no.",1,"5.5×7÷2=19.25."),
            new("Adding decimal prices with misaligned decimal places may produce a different numerical result. 1=yes,0=no.",1,"Place value matters.")
        ],
        [L0502PlaceValueArithmetic] =
        [
            new("0.137 +0.284 expressed in THOUSANDTHS?",421,"137+284=421 thousandths."),
            new("3.25 +1 expressed in HUNDREDTHS?",425,"325+100=425 hundredths."),
            new("0.5+1.15 expressed in HUNDREDTHS?",165,"50+115=165."),
            new("10.6+1.7 expressed in TENTHS?",123,"106+17=123 tenths."),
            new("123+0.2 expressed in TENTHS?",1232,"1230+2=1232."),
            new("0.216 expressed as number of THOUSANDTHS?",216,"2tenths+1hundredth+6thousandths=216."),
            new("0.304 expressed as number of THOUSANDTHS?",304,"3tenths+4thousandths=304."),
            new("In0.137 there are how many THOUSANDTHS altogether?",137,"0.137=137 thousandths."),
            new("In0.284 there are how many THOUSANDTHS altogether?",284,"0.284=284 thousandths."),
            new("0.137+0.284 includes how many HUNDREDTHS beyond regrouping? Enter whole hundredths in0.421.",42,"0.421=42hundredths plus1thousandth."),
            new("Andre completes135 facts in90sec. Facts per second in HALF-FACT units?",3,"135/90=1.5facts/s=3half-facts."),
            new("Noah completes75 facts in60sec. Facts per second in QUARTER-FACT units?",5,"75/60=1.25facts/s=5quarter-facts."),
            new("Andre1.5facts/s and Noah1.25facts/s. Enter1 Andre faster,2 Noah faster.",1,"1.5>1.25."),
            new("Andre and Noah rate gap in QUARTER-FACTS per second?",1,"1.5-1.25=.25."),
            new("The value0.137+0.284 expressed in TEN-THOUSANDTHS?",4210,"0.421=4210 ten-thousandths."),
            new("The sum0.5+1.15 expressed in THOUSANDTHS?",1650,"1.65=1650thousandths."),
            new("The difference1.15 minus0.5 in HUNDREDTHS?",65,"115-50=65."),
            new("If ten thousandths combine, how many HUNDREDTHS?",1,"10 thousandths=1 hundredth.")
        ],
        [L0502PlaceValueLogic] =
        [
            new("0.137+0.284=0.421. 1=yes,0=no.",1,"137+284=421thousandths."),
            new("0.137+0.284=0.311. 1=yes,0=no.",0,"Requires regrouping hundredths."),
            new("3.25+1=4.25. 1=yes,0=no.",1,"Align ones."),
            new("0.5+1.15=1.65. 1=yes,0=no.",1,"0.50+1.15."),
            new("10.6+1.7=12.3. 1=yes,0=no.",1,"106tenths+17tenths."),
            new("123+0.2=123.2. 1=yes,0=no.",1,"Place-value alignment."),
            new("Ten thousandths equal one hundredth. 1=yes,0=no.",1,"10×.001=.01."),
            new("Twelve hundredths can be regrouped as one tenth and2 hundredths. 1=yes,0=no.",1,"0.12=0.1+0.02."),
            new("One can add a ones-place digit directly to a tenths-place digit without converting units. 1=yes,0=no.",0,"Convert or align same units."),
            new("0.5 is numerically equal to0.50. 1=yes,0=no.",1,"Trailing zeros don't change value."),
            new("Andre135facts/90sec is faster than Noah75facts/60sec. 1=yes,0=no.",1,"1.5 vs1.25facts/s."),
            new("A physical base-ten block representation can show same decimal using different regroupings. 1=yes,0=no.",1,"E.g.10 thousandths=hundredth."),
            new("A decimal point should be aligned in vertical addition to match place value. 1=yes,0=no.",1,"Column unit alignment."),
            new("Teacher should verify blocks and visual digit placement separately from a numeric sum. 1=yes,0=no.",1,"Drawing rubric.")
        ],
        [L0503DecimalCalculations] =
        [
            new("1.13 minus0.46, answer in HUNDREDTHS?",67,"113-46=67 hundredths."),
            new("0.027+0.004 in THOUSANDTHS?",31,"27+4=31 thousandths."),
            new("0.203+0.01 in THOUSANDTHS?",213,"203+10=213."),
            new("1.2+0.145 in THOUSANDTHS?",1345,"1200+145=1345."),
            new("20-1.97 in HUNDREDTHS?",1803,"2000-197=1803."),
            new("0.3-0.006 in THOUSANDTHS?",294,"300-6=294."),
            new("14.6-1.4 in TENTHS?",132,"146-14=132."),
            new("38.60-6.75 in HUNDREDTHS?",3185,"3860-675=3185."),
            new("241.76-2.18 in HUNDREDTHS?",23958,"24176-218=23958."),
            new("Three pencils each30cents cost in CENTS?",90,"3×30=90."),
            new("A hat1450cents and2binders320cents each. Cost in CENTS?",2090,"1450+640=2090."),
            new("Three pencils30c each,hat1450c,2binders320c each. Total in CENTS?",2180,"90+1450+640=2180."),
            new("Pencils cost30c each; maximum whole pencils purchasable with500cents?",16,"500/30=16 remainder20."),
            new("Prism7½cm×12cm×15½cm. Volume in CUBIC CENTIMETERS?",1395,"7.5×12×15.5=1395."),
            new("Prism7½×12×15½cm packed with1/2cm cubes. Number of small cubes?",11160,"15×24×31=11160."),
            new("Car travels75miles in60minutes. Distance in18minutes in HALF-MILE units?",45,"75×18/60=22.5miles."),
            new("0.012-0.007 in THOUSANDTHS?",5,"12-7=5."),
            new("0.300-0.006 in THOUSANDTHS?",294,"300-6=294.")
        ],
        [L0503DecimalPrinciples] =
        [
            new("1.13-0.46=0.67. 1=yes,0=no.",1,"113-46=67 hundredths."),
            new("0.027+0.004=0.031. 1=yes,0=no.",1,"31 thousandths."),
            new("0.203+0.01=0.213. 1=yes,0=no.",1,"Align hundredths."),
            new("1.2+0.145=1.345. 1=yes,0=no.",1,"1.200+0.145."),
            new("20-1.97 cannot be calculated because20 has no decimal digits. 1=yes,0=no.",0,"20.00-1.97=18.03."),
            new("0.3-0.006=0.294. 1=yes,0=no.",1,"0.300-.006=.294."),
            new("0.3-0.006=0.024. 1=yes,0=no.",0,"Wrong place-value borrowing."),
            new("14.6-1.4=13.2. 1=yes,0=no.",1,"14.6-1.4=13.2."),
            new("38.60-6.75=31.85. 1=yes,0=no.",1,"Regroup hundredths/tenths."),
            new("241.76-2.18=239.58. 1=yes,0=no.",1,"Subtract using aligned decimal."),
            new("Three pencils+hat+two binders cost21.80 dollars and exceed20 dollars. 1=yes,0=no.",1,".90+14.50+6.40=21.80."),
            new("A 0.5cm cube volume is0.125cm³, so11160 of them pack1395cm³. 1=yes,0=no.",1,"11160×1/8=1395."),
            new("75miles in60minutes means22.5miles in18minutes at constant rate. 1=yes,0=no.",1,"75×18/60."),
            new("The same decimal number can be written with trailing zeros without changing its value. 1=yes,0=no.",1,"0.3=0.300.")
        ],
        [L0504PreciseArithmetic] =
        [
            new("7.2-3.67 expressed in HUNDREDTHS?",353,"7.20-3.67=3.53."),
            new("16-1.4 expressed in TENTHS?",146,"16.0-1.4=14.6."),
            new("1-0.1978 in TEN-THOUSANDTHS?",8022,"1.0000-.1978=.8022."),
            new("Chocolates label0.384lb, actual0.3798lb. Difference in TEN-THOUSANDTHS of pound?",42,"0.3840-.3798=.0042."),
            new("1.036 plus what decimal equals4? Unknown in THOUSANDTHS?",2964,"4-1.036=2.964."),
            new("0.738 plus what equals1? Unknown in THOUSANDTHS?",262,"1-.738=.262."),
            new("0.5137 plus what equals1? Unknown in TEN-THOUSANDTHS?",4863,"1-.5137=.4863."),
            new("Large cube10ft edge packs small cubes2½ft edge. Number along one edge?",4,"10/2.5=4."),
            new("Large cube10ft edge packs2½ft edge cubes. Number of crates?",64,"4³=64."),
            new("Chef bakes2 loaves for9 customers. Loaves for63 customers?",14,"63/9×2=14."),
            new("Chef bakes2 loaves for9 customers. Customers when making20 loaves?",90,"20/2×9=90."),
            new("Chef bakes2 loaves for9 customers. Fraction of loaf per customer in NINTHS?",2,"2/9 loaf per customer."),
            new("Six loaves at2 per9 customers feed how many customers?",27,"6/2×9=27."),
            new("0.3840-0.3798 equals how many MILLIONTHS of a pound?",4200,".0042=4200 millionths."),
            new("14.6-1.4 in HUNDREDTHS?",1320,"13.2=1320hundredths.")
        ],
        [L0504DecimalReasoning] =
        [
            new("7.2-3.67=3.53. 1=yes,0=no.",1,"7.20-3.67."),
            new("7.2-3.67=3.05. 1=yes,0=no.",0,"Misaligned place value."),
            new("16-1.4=14.6. 1=yes,0=no.",1,"16.0-1.4."),
            new("1-0.1978=0.8022. 1=yes,0=no.",1,"1.0000-.1978=.8022."),
            new("0.3840 is equal to0.384. 1=yes,0=no.",1,"Trailing zero."),
            new("0.3798lb chocolates are lighter than0.384lb label by0.0042lb. 1=yes,0=no.",1,"Subtract label minus actual."),
            new("0.3798lb are heavier than0.384lb. 1=yes,0=no.",0,"Actual less."),
            new("1.036+2.964=4. 1=yes,0=no.",1,"Regroup thousandths."),
            new("0.738+0.262=1. 1=yes,0=no.",1,"Sum1000thousandths."),
            new("0.5137+0.4863=1. 1=yes,0=no.",1,"Ten-thousandths complement."),
            new("2.5ft cubes along10ft edge fit4 each side. 1=yes,0=no.",1,"10/2.5=4."),
            new("Cube10ft edge packs64 cubes of2.5ft edge when aligned. 1=yes,0=no.",1,"4³=64."),
            new("63 customers at2 loaves per9 need14 loaves. 1=yes,0=no.",1,"63/9×2."),
            new("20 loaves at2 for9 customers correspond to90 customers. 1=yes,0=no.",1,"20/2×9."),
            new("In decimal subtraction, digits should be aligned by place value even when numbers have different displayed precision. 1=yes,0=no.",1,"Decimal points aligned.")
        ],
        [L0505DecimalProducts] =
        [
            new("122.1×1/100 in THOUSANDTHS?",1221,"122.1/100=1.221."),
            new("11.8×1/100 in TEN-THOUSANDTHS?",1180,"11.8/100=.118=1180 ten-thousandths."),
            new("1350.1×1/100 in THOUSANDTHS?",13501,"13.501."),
            new("1.704×1/100 in HUNDRED-THOUSANDTHS?",1704,"1.704/100=.01704, 1704 hundred-thousandths."),
            new("0.06×0.154 in HUNDRED-THOUSANDTHS?",924,".00924=924 hundred-thousandths."),
            new("0.01×0.02 in TEN-THOUSANDTHS?",2,".0002."),
            new("0.3×0.2 in HUNDREDTHS?",6,".06."),
            new("1.2×5 in TENTHS?",60,"6.0=60tenths."),
            new("0.9×1.1 in HUNDREDTHS?",99,".99."),
            new("1.5×2 in TENTHS?",30,"3.0."),
            new("0.0004×0.005 in MILLIONTHS?",2,".000002."),
            new("33.1+1.95 in HUNDREDTHS?",3505,"35.05."),
            new("1.075+27.105 in THOUSANDTHS?",28180,"28.180."),
            new("0.401+9.28 in THOUSANDTHS?",9681,"9.681."),
            new("13.2-1.78 in HUNDREDTHS?",1142,"11.42."),
            new("23.11-0.376 in THOUSANDTHS?",22734,"22.734."),
            new("0.9-0.245 in THOUSANDTHS?",655,"0.655."),
            new("An area diagram represents0.3×0.2 in HUNDREDTH-SQUARE-UNIT parts?",6,"6 of100 equal unit squares.")
        ],
        [L0505DecimalLogic] =
        [
            new("0.06×0.154=0.00924. 1=yes,0=no.",1,"6×154=924 scaled by1/100000."),
            new("0.06×0.154 can be written6×1/100×154×1/1000. 1=yes,0=no.",1,"Source option A."),
            new("0.06×0.154 can be written6×154×1/100000. 1=yes,0=no.",1,"Source option B."),
            new("0.06×0.154 can be written6×0.1×154×0.01. 1=yes,0=no.",0,"This equals.924, not.00924."),
            new("0.06×0.154 can be written6×154×0.00001. 1=yes,0=no.",1,"Source option D."),
            new("0.01×0.02=0.0002. 1=yes,0=no.",1,"1/100×2/100."),
            new("0.3×0.2=0.06. 1=yes,0=no.",1,"3/10×2/10."),
            new("0.9×1.1=0.99. 1=yes,0=no.",1,"9×11/100."),
            new("0.0004×0.005=0.000002. 1=yes,0=no.",1,"4×5×10^-7=2×10^-6."),
            new("Multiplying positive number by1/100 reduces its value to one hundredth. 1=yes,0=no.",1,"Shift place values by factor100."),
            new("Moving decimal points is a shortcut for scaling base-ten units, not a new arbitrary arithmetic rule. 1=yes,0=no.",1,"Place-value interpretation."),
            new("1.2×5=6. 1=yes,0=no.",1,"Six whole units."),
            new("A quadrilateral's drawn area must be graded using the actual polygon, not just a numeric result. 1=yes,0=no.",1,"Teacher geometry rubric."),
            new("The equivalent representations of decimal factors rely on associative and commutative multiplication. 1=yes,0=no.",1,"Regrouping factors.")
        ],
        [L0506Products] =
        [
            new("1.2×0.11 in THOUSANDTHS?",132,"0.132."),
            new("0.34×0.02 in TEN-THOUSANDTHS?",68,"0.0068."),
            new("120×0.002 in THOUSANDTHS?",240,"0.24."),
            new("0.3×0.5 in HUNDREDTHS?",15,"0.15."),
            new("Buffalo gasoline price per gallon in CENTS?",229,"Given $2.29."),
            new("Toronto gasoline91cents per liter,3.8 liters/gallon. Cost per gallon rounded to CENTS?",346,"91×3.8=345.8cents≈346."),
            new("Gas Toronto≈346cents/gal vs Buffalo229cents/gal. Difference in CENTS?",117,"346−229=117."),
            new("10.3+3.7 in TENTHS?",140,"14.0."),
            new("20.99−4.97 in HUNDREDTHS?",1602,"16.02."),
            new("15.99+23.51 in HUNDREDTHS?",3950,"39.50."),
            new("1.893−0.353 in THOUSANDTHS?",1540,"1.540."),
            new("49/50 divided by7/6, answer in TWENTY-FIFTHS?",21,"(49/50)×(6/7)=21/25."),
            new("1.05×2.8 in HUNDREDTHS?",294,"2.94."),
            new("0.0015×0.024 in MILLIONTHS?",36,"0.000036."),
            new("0.25×1.6 in TENTHS?",4,"0.4."),
            new("One gallon3.8L, at91cents/L. Price in TENTHS OF CENT?",3458,"91×3.8=345.8cents."),
            new("0.3×0.5 in THOUSANDTHS?",150,"0.150."),
            new("A rectangular area model of0.3by0.5 has how many HUNDREDTH-SQUARE-UNIT parts?",15,"3tenths×5tenths=15hundredths.")
        ],
        [L0506MethodsLogic] =
        [
            new("1.2×0.11=0.132. 1=yes,0=no.",1,"12×11=132 thousandths."),
            new("0.34×0.02=0.0068. 1=yes,0=no.",1,"34×2=68 ten-thousandths."),
            new("120×0.002=0.24. 1=yes,0=no.",1,"120×2 thousandths."),
            new("0.3×0.5=0.15. 1=yes,0=no.",1,"15 hundredths."),
            new("An area model for0.3×0.5 can have cells each0.01 square unit. 1=yes,0=no.",1,"3×5 of hundredth squares."),
            new("In Toronto,3.8 liters at0.91 dollars/L cost about3.46 dollars. 1=yes,0=no.",1,"3.458 rounds3.46."),
            new("Buffalo2.29dollars/gal is more expensive than Toronto3.46/gal under source conversion. 1=yes,0=no.",0,"Toronto costs more."),
            new("Difference between Toronto3.46 and Buffalo2.29 is1.17 dollars. 1=yes,0=no.",1,"3.46−2.29=1.17."),
            new("1.05×2.8=2.94 because105×28=2940 and product scale is thousandths. 1=yes,0=no.",1,"2940/1000=2.94."),
            new("0.0015×0.024=0.000036. 1=yes,0=no.",1,"15×24=360, scale1/10000000."),
            new("49/50÷7/6=21/25. 1=yes,0=no.",1,"49×6/(50×7)=21/25."),
            new("Products may be computed using fractions, powers of ten or partial area products. 1=yes,0=no.",1,"Equivalent strategies."),
            new("A numeric product alone guarantees a student's drawn area partitions are correct. 1=yes,0=no.",0,"Teacher visual review."),
            new("Multiplication by a positive number below1 produces a smaller positive product than the original factor. 1=yes,0=no.",1,"Scale factor below1.")
        ],
        [L0507ProductValues] =
        [
            new("3×0.6 in TENTHS?",18,"1.8."),
            new("0.4×2 in TENTHS?",8,"0.8."),
            new("0.4×0.6 in HUNDREDTHS?",24,"0.24."),
            new("3×2 in UNITS?",6,"6."),
            new("3.1×1.4 in HUNDREDTHS?",434,"4.34."),
            new("For3.1×1.4, area3×1 in HUNDREDTHS?",300,"3."),
            new("For3.1×1.4, area3×0.4 in HUNDREDTHS?",120,"1.2."),
            new("For3.1×1.4, area0.1×1 in HUNDREDTHS?",10,"0.1."),
            new("For3.1×1.4, area0.1×0.4 in HUNDREDTHS?",4,"0.04."),
            new("0.36×0.53 in TEN-THOUSANDTHS?",1908,".1908."),
            new("2.5×1.4 in TENTHS?",35,"3.5."),
            new("0.64×0.81 in TEN-THOUSANDTHS?",5184,"0.5184."),
            new("Diego buys12 muffins for420cents. Price per muffin in CENTS?",35,"420/12=35."),
            new("Diego buys12 muffins for420cents. Price for4 in CENTS?",140,"4×35=140."),
            new("Diego buys muffins35cents each. Maximum with300cents?",8,"300/35=8 whole,20c leftover."),
            new("0.4×0.6 plus0.4×2 in HUNDREDTHS?",104,".24+.8=1.04."),
            new("Rectangle18×23 whole units. Area?",414,"18×23=414."),
            new("An area3×0.6 plus0.4×2 plus0.4×0.6 plus3×2 in HUNDREDTHS?",884,"1.8+.8+.24+6=8.84.")
        ],
        [L0507PartialLogic] =
        [
            new("3.1×1.4=4.34. 1=yes,0=no.",1,"3×1+3×.4+.1×1+.1×.4=4.34."),
            new("0.36×0.53=0.1908. 1=yes,0=no.",1,"36×53/10000."),
            new("2.5×1.4=3.5. 1=yes,0=no.",1,"25×14/100."),
            new("0.64×0.81=0.5184. 1=yes,0=no.",1,"64×81/10000."),
            new("The area3×0.6 equals1.8. 1=yes,0=no.",1,"Source subrectangle."),
            new("The area0.4×0.6 equals0.24. 1=yes,0=no.",1,"Two decimal factors."),
            new("3.1×1.4 can be partitioned into four subrectangles using3+.1 and1+.4. 1=yes,0=no.",1,"Distributive law."),
            new("The sum of partial products equals the full rectangle area. 1=yes,0=no.",1,"No overlaps/gaps."),
            new("12 muffins for4.20 means four cost1.40. 1=yes,0=no.",1,"35c each."),
            new("A3-dollar budget buys9 whole35-cent muffins. 1=yes,0=no.",0,"9 cost315c; maximum8."),
            new("Adding digits from different decimal place columns without respecting units preserves all values. 1=yes,0=no.",0,"Regroup properly."),
            new("The method of whole-number partial products also extends to decimals after accounting for scale. 1=yes,0=no.",1,"Place-value decomposition."),
            new("A numeric correct answer alone validates a drawn source rectangle's placement labels. 1=yes,0=no.",0,"Requires visual checking."),
            new("An area model can represent products smaller than1 square unit. 1=yes,0=no.",1,"Fractional side lengths.")
        ],
        [L0508ProductArithmetic] =
        [
            new("0.54×3.8 in THOUSANDTHS?",2052,"2.052."),
            new("0.54×0.8 in THOUSANDTHS?",432,".432 source partial area."),
            new("0.54×3 in HUNDREDTHS?",162,"1.62."),
            new("0.03×0.65 in TEN-THOUSANDTHS?",195,".0195."),
            new("5.4×2.4 in HUNDREDTHS?",1296,"12.96."),
            new("1.67×3.5 in THOUSANDTHS?",5845,"5.845."),
            new("Blueberries0.6lb at398cents/lb. Cost in THOUSANDTHS OF DOLLAR?",2388,".6×3.98=2.388dollars."),
            new("Clementines1.8lb at249cents/lb. Cost in THOUSANDTHS OF DOLLAR?",4482,"1.8×2.49=4.482dollars."),
            new("Combined fruit cost2.388+4.482 in CENTS?",687,"6.870dollars=687cents."),
            new("7.4−0.0022 in TEN-THOUSANDTHS?",73978,"7.3978."),
            new("7.39−0.0012 in TEN-THOUSANDTHS?",73888,"7.3888."),
            new("Which difference is greater:7.4-.0022 or7.39-.0012? Enter1 first,2 second.",1,"7.3978>7.3888."),
            new("Andre plants3 saplings in30minutes. Minutes per sapling?",10,"30÷3=10."),
            new("Andre plants14 saplings at10minutes each. Total MINUTES?",140,"14×10=140."),
            new("0.54×3.8 represented in TEN-THOUSANDTHS?",20520,"2.052=20520 ten-thousandths."),
            new("Three times65 equals?",195,"3×65=195."),
            new("5.4×2.4 as partial 5×2+5×.4+.4×2+.4×.4, HUNDREDTHS?",1296,"10+2+.8+.16=12.96."),
            new("1.67×3.5 in TEN-THOUSANDTHS?",58450,"5.8450.")
        ],
        [L0508ProductLogic] =
        [
            new("0.54×3.8=2.052. 1=yes,0=no.",1,".54×(3+.8)=1.62+.432."),
            new("0.54×0.8=0.432. 1=yes,0=no.",1,"54×8 thousandths."),
            new("0.54×3=1.62. 1=yes,0=no.",1,"54hundredths×3."),
            new("0.03×0.65=0.0195. 1=yes,0=no.",1,"3×65/10000."),
            new("5.4×2.4=12.96. 1=yes,0=no.",1,"54×24/100."),
            new("1.67×3.5=5.845. 1=yes,0=no.",1,"167×35/1000."),
            new("Blueberries.6lb at3.98 and clementines1.8lb at2.49 have combined cost6.87 dollars rounded to cents. 1=yes,0=no.",1,"2.388+4.482=6.87."),
            new("7.4-.0022 is greater than7.39-.0012. 1=yes,0=no.",1,"7.3978>7.3888."),
            new("Andre at10min per sapling needs140min for14. 1=yes,0=no.",1,"14×10."),
            new("Decimal multiplication can be checked against an estimate of product magnitude. 1=yes,0=no.",1,"Reasonableness."),
            new("A decimal algorithm can be explained by using whole-number multiplication then a compensating power of ten. 1=yes,0=no.",1,"Place-value scaling."),
            new("A partial area diagram's total is the sum of non-overlapping subrectangle areas. 1=yes,0=no.",1,"Partition geometry."),
            new("Decimal-product answers should be rounded to cents only when the money context requires it. 1=yes,0=no.",1,"Intermediate precision matters."),
            new("The original pictured subrectangle mapping is automatically verified by a total numeric product. 1=yes,0=no.",0,"Image requires teacher review.")
        ],
        [L0509WholeDivision] =
        [
            new("2105÷5 equals?",421,"400+20+1=421."),
            new("657÷3 equals?",219,"200+10+9=219."),
            new("1150÷46 equals?",25,"46×25=1150."),
            new("534÷6 equals?",89,"6×(80+9)=534."),
            new("1032÷43 equals?",24,"43×24=1032."),
            new("In 2105÷5 partial quotients, subtract2000 and first partial quotient?",400,"5×400=2000."),
            new("After subtracting2000 from2105, remainder?",105,"2105-2000=105."),
            new("From105 divide by5 using partial quotient20, subtract how much?",100,"5×20=100."),
            new("After subtracting100 from105, remainder?",5,"105−100=5."),
            new("From657÷3, Andre uses partial quotient200. Subtraction amount?",600,"3×200=600."),
            new("For657÷3, Andre uses200,10,9. Their SUM?",219,"219."),
            new("For657÷3, Jada uses50,100,60,9. Sum?",219,"50+100+60+9=219."),
            new("534÷6 quotient partial80 and9. Subtraction for80?",480,"80×6=480."),
            new("534−480 remainder?",54,"534-480=54."),
            new("Rectangle3.25in×6.1in area in THOUSANDTHS of in²?",19825,"3.25×6.1=19.825."),
            new("Square side4.6in area in HUNDREDTHS of in²?",2116,"4.6²=21.16."),
            new("Parallelogram base5.875 height3.5in area in TEN-THOUSANDTHS?",205625,"5.875×3.5=20.5625."),
            new("Triangle base7.18 height5.4in area in THOUSANDTHS?",19386,"7.18×5.4/2=19.386."),
            new("Which polygon has largest area? Enter1 rectangle,2 square,3 parallelogram,4 triangle.",2,"Square21.16 > parallelogram20.5625."),
            new("Spider web4micrometers, fiber1/100000m=10 micrometers. Difference in MICROMETERS?",6,"10-4=6 micrometers."),
            new("Which is thicker:1 spider web4micrometers or2 fabric fiber10micrometers? Enter1 or2.",2,"Fiber10>4.")
        ],
        [L0509DivisionLogic] =
        [
            new("2105÷5=421. 1=yes,0=no.",1,"5×421=2105."),
            new("657÷3=219. 1=yes,0=no.",1,"3×219=657."),
            new("Andre and Jada may choose different partial quotients for657÷3 yet reach219. 1=yes,0=no.",1,"Same total quotient."),
            new("Andre's200+10+9 equals219. 1=yes,0=no.",1,"Sum partial quotients."),
            new("Jada's50+100+60+9 equals219. 1=yes,0=no.",1,"Valid alternative."),
            new("1150÷46=25. 1=yes,0=no.",1,"46×25=1150."),
            new("534÷6=89. 1=yes,0=no.",1,"6×89=534."),
            new("1032÷43=24. 1=yes,0=no.",1,"43×24=1032."),
            new("Partial quotients are pieces of the quotient whose divisor products are subtracted from dividend. 1=yes,0=no.",1,"Repeated subtract multiples."),
            new("One must subtract only multiples of the divisor to preserve the quotient decomposition. 1=yes,0=no.",1,"Place-value structure."),
            new("A correct final quotient automatically confirms every step of a student's partial-quotient notation is valid. 1=yes,0=no.",0,"Teacher checks reasoning."),
            new("Square side4.6 has area21.16sqin, largest among source polygons. 1=yes,0=no.",1,"Compare measured areas."),
            new("A shirt fiber10micrometers is thicker than4micrometer web. 1=yes,0=no.",1,"10>4."),
            new("One micrometer is one millionth of a meter. 1=yes,0=no.",1,"Metric definition.")
        ],
        [L0510LongDivisionNumbers] =
        [
            new("696 divided by12 equals?",58,"12×58=696."),
            new("For696÷12, first quotient5 represents which place? Enter1 hundreds,2 tens,3 ones,4 tenths.",2,"5tens means50."),
            new("917÷7 equals?",131,"7×131=917."),
            new("972÷9 equals?",108,"9×108=972."),
            new("180×9 equals?",1620,"Han's erroneous result doesn't equal972."),
            new("For917÷7,7×100 removed first from917 gives remainder?",217,"917-700=217."),
            new("For917÷7,7×30 removes how much?",210,"7×30=210."),
            new("For917÷7, after removing700 and210, remainder?",7,"917-700-210=7."),
            new("For972÷9, first quotient100 removes how much?",900,"9×100=900."),
            new("For972÷9, after removing900 remains?",72,"972−900=72."),
            new("For972÷9, next quotient8 removes how much?",72,"9×8=72."),
            new("One ounce yogurt contains1.2grams sugar,14.25oz contains grams in TENTHS?",171,"14.25×1.2=17.1g."),
            new("Coin2mass27.22g minus coin1mass16.718g in THOUSANDTHS OF GRAM?",10502,"27.220−16.718=10.502."),
            new("12 multiplied by50 equals?",600,"First tens quotient of696/12."),
            new("696−600 equals?",96,"Remainder after first step."),
            new("96 divided by12 equals?",8,"Second quotient digit8."),
            new("For917÷7, how many HUNDREDS are in first partial quotient?",1,"100 groups of7."),
            new("If quotient58 and divisor12, dividend?",696,"58×12=696.")
        ],
        [L0510LongDivisionLogic] =
        [
            new("696÷12=58. 1=yes,0=no.",1,"12×58=696."),
            new("The leading5 in quotient58 for696÷12 represents5 tens. 1=yes,0=no.",1,"50, not5 ones."),
            new("The leading5 in quotient58 for696÷12 represents5 hundreds. 1=yes,0=no.",0,"Not enough hundreds."),
            new("917÷7=131. 1=yes,0=no.",1,"7×131=917."),
            new("In917÷7, subtracting the7 written under initial9 refers to7 hundreds in the place-value algorithm. 1=yes,0=no.",1,"Place-value interpretation."),
            new("Han's result180 for972÷9 is correct. 1=yes,0=no.",0,"180×9=1620."),
            new("972÷9=108. 1=yes,0=no.",1,"9×108=972."),
            new("The0 in quotient108 represents the fact that there are no tens of groups in the middle step. 1=yes,0=no.",1,"Place holder matters."),
            new("14.25 ounces at1.2grams per ounce yields17.1grams sugar. 1=yes,0=no.",1,"Multiply14.25×1.2."),
            new("27.22grams minus16.718grams is10.502grams. 1=yes,0=no.",1,"Decimal place alignment."),
            new("A long-division digit's place value matters for each subtraction. 1=yes,0=no.",1,"Compressed notation."),
            new("Long division and partial quotients can yield the same quotient with different recording formats. 1=yes,0=no.",1,"Equivalent algorithms."),
            new("Checking a division by multiplication can reveal an impossible quotient. 1=yes,0=no.",1,"Divisor×quotient should equal dividend for zero remainder."),
            new("A correct total quotient alone automatically confirms the source diagram and student regrouping work. 1=yes,0=no.",0,"Teacher checks steps.")
        ],
        [L0511DecimalQuotients] =
        [
            new("Three quarters as a decimal in HUNDREDTHS?",75,"3÷4=0.75."),
            new("Three fiftieths as a decimal in HUNDREDTHS?",6,"3÷50=0.06."),
            new("Seven twenty-fifths as a decimal in HUNDREDTHS?",28,"7÷25=0.28."),
            new("Mai walked1/8 of30-mile trail. Distance in EIGHTH-MILE UNITS?",30,"30÷8=3.75miles=30 eighths."),
            new("Mai walked1/8 of30-mile trail. Distance in HUNDREDTHS of mile?",375,"3.75miles."),
            new("99 divided by12, quotient in HUNDREDTHS?",825,"99/12=8.25."),
            new("216 divided by5, quotient in TENTHS?",432,"216/5=43.2."),
            new("1988 divided by8, quotient in HUNDREDTHS?",24850,"1988/8=248.5."),
            new("Nine twenty-fifths as decimal in HUNDREDTHS?",36,"9/25=.36."),
            new("Eighteen fiftieths as decimal in HUNDREDTHS?",36,"18/50=.36."),
            new("Thirty-six hundredths as decimal in HUNDREDTHS?",36,"36/100=.36."),
            new("Five minus which decimal equals4.329? Answer in THOUSANDTHS?",671,"5-4.329=.671."),
            new("One minus which decimal equals0.015? Answer in THOUSANDTHS?",985,"1-.015=.985."),
            new("One minus which decimal equals0.863? Answer in THOUSANDTHS?",137,"1-.863=.137."),
            new("1.24×0.15, answer in TEN-THOUSANDTHS?",1860,"1.24×0.15=.186."),
            new("124×15 equals?",1860,"Exact source fact."),
            new("7/25 as hundredths count?",28,"Multiply numerator and denominator by4."),
            new("1/8 of80 miles gives how many MILES?",10,"80÷8=10.")
        ],
        [L0511DivisionReasoning] =
        [
            new("3/4=0.75. 1=yes,0=no.",1,"75/100."),
            new("3/50=0.6. 1=yes,0=no.",0,"Correct.06."),
            new("7/25=0.28. 1=yes,0=no.",1,"28/100."),
            new("99÷12=8.25. 1=yes,0=no.",1,"12×8.25=99."),
            new("216÷5=43.2. 1=yes,0=no.",1,"43.2×5=216."),
            new("1988÷8=248.5. 1=yes,0=no.",1,"8×248.5=1988."),
            new("9/25=18/50=36/100. 1=yes,0=no.",1,"Equivalent fractions."),
            new("18/50=0.36. 1=yes,0=no.",1,"36/100."),
            new("A decimal point may be appended with zero digits when extending a whole number dividend to find decimal quotient. 1=yes,0=no.",1,"Equivalent numerical value."),
            new("A terminating decimal quotient can be checked by multiplying it by divisor. 1=yes,0=no.",1,"Inverse relationship."),
            new("5-0.671=4.329. 1=yes,0=no.",1,"Subtraction."),
            new("1-.985=.015. 1=yes,0=no.",1,"Subtraction."),
            new("1-.137=.863. 1=yes,0=no.",1,"Subtraction."),
            new("1.24×.15=.186. 1=yes,0=no.",1,"Use place-value scaling from124×15."),
            new("A base-ten diagram can justify a quotient even when long division supplies the decimal digits. 1=yes,0=no.",1,"Equivalent representations."),
            new("Every quotient of two whole numbers has a terminating decimal. 1=yes,0=no.",0,"Some repeat, e.g.1/3.")
        ],
        [L0512DecimalDivision] =
        [
            new("4.5÷0.09 equals?",50,"Scale both×100:450÷9=50."),
            new("45÷0.9 equals?",50,"Scale both×10."),
            new("450÷9 equals?",50,"450/9=50."),
            new("4500÷90 equals?",50,"4500/90=50."),
            new("7.89÷2, result in THOUSANDTHS?",3945,"3.945×1000."),
            new("39.54÷3, result in HUNDREDTHS?",1318,"13.18×100."),
            new("0.176÷5, result in TEN-THOUSANDTHS?",352,"0.0352×10000."),
            new("Four students share17.52 dollars profit. Cents per person?",438,"1752÷4=438 cents."),
            new("11 inches in centimeters, using2.54cm/in, answer in HUNDREDTHS cm?",2794,"11×2.54=27.94cm."),
            new("8.5 inches in centimeters, using2.54cm/in, answer in THOUSANDTHS cm?",21590,"8.5×2.54=21.59cm or21590thousandths."),
            new("US letter paper11×8.5 inches. Area in TENTHS of square inch?",935,"11×8.5=93.5sqin."),
            new("European A4 paper21.0×29.7cm. Area in TENTHS of square centimeter?",6237,"21×29.7=623.7."),
            new("US letter27.94cm×21.59cm. Area in TEN-THOUSANDTHS sqcm?",6032246,"27.94×21.59=603.2246."),
            new("Compare A4 area623.7sqcm and US letter603.2246sqcm. Enter1 for A4 larger,2 for letter larger.",1,"623.7>603.2246."),
            new("Three equal groups divide9.6. Size of each group in TENTHS?",32,"9.6÷3=3.2."),
            new("One and a half divided by5. Answer in TENTHS?",3,"1.5÷5=0.3."),
            new("Eight and four tenths divided by4, result in TENTHS?",21,"8.4÷4=2.1."),
            new("Four point five divided by0.09 is same as450 divided by9. Quotient?",50,"Scale100.")
        ],
        [L0512DecimalScaleLogic] =
        [
            new("4.5÷0.09=45÷0.9=450÷9=50. 1=yes,0=no.",1,"Scale both amounts."),
            new("Multiplying both nonzero divisor and dividend by same positive factor preserves quotient. 1=yes,0=no.",1,"Ratio invariant."),
            new("Multiplying dividend alone by10 preserves quotient. 1=yes,0=no.",0,"Changes result tenfold."),
            new("7.89÷2=3.945. 1=yes,0=no.",1,"Long division."),
            new("39.54÷3=13.18. 1=yes,0=no.",1,"13.18×3=39.54."),
            new("0.176÷5=0.0352. 1=yes,0=no.",1,"0.0352×5=0.176."),
            new("17.52 dollars evenly split among4 gives4.38 dollars each. 1=yes,0=no.",1,"1752cents/4."),
            new("US letter dimensions converted by2.54cm/in are27.94cm and21.59cm. 1=yes,0=no.",1,"Multiply each inch length."),
            new("A4 area21×29.7cm exceeds US letter area27.94×21.59cm. 1=yes,0=no.",1,"623.7>603.2246."),
            new("A base-ten diagram can represent decimal quantities split into equal groups. 1=yes,0=no.",1,"Fractional place values."),
            new("4.5÷0.09=0.5. 1=yes,0=no.",0,"Quotient50."),
            new("Equal split of dollars can be checked by converting the result to cents before division. 1=yes,0=no.",1,"Avoids decimal point confusion."),
            new("When scaling division, zero cannot be used as a multiplier for both numbers, because divisor would be0. 1=yes,0=no.",1,"Undefined."),
            new("Paper area is found by multiplying both side lengths in same unit. 1=yes,0=no.",1,"Square units.")
        ],
        [L0513DecimalQuotients] =
        [
            new("109.2÷6, quotient in TENTHS?",182,"109.2/6=18.2."),
            new("31.59÷13, quotient in HUNDREDTHS?",243,"31.59/13=2.43."),
            new("61.12÷3.2, quotient in TENTHS?",191,"61.12/3.2=19.1."),
            new("611.2÷32, quotient in TENTHS?",191,"Equivalent scaling."),
            new("6112÷320, quotient in TENTHS?",191,"Equivalent scaling."),
            new("Bag weighs5.1kilograms and each penny2.5grams. Approximate count of pennies?",2040,"5100g/2.5g=2040."),
            new("2.5-1.6, answer in TENTHS?",9,"0.9."),
            new("0.72-0.4, answer in HUNDREDTHS?",32,"0.32."),
            new("11.3-1.75, answer in HUNDREDTHS?",955,"9.55."),
            new("73-1.3, answer in TENTHS?",717,"71.7."),
            new("Plant B6⅔in, Plant C4 4/15in. Plant C as fraction of B in TWENTY-FIFTHS?",16,"(64/15)/(20/3)=16/25."),
            new("Plant B6⅔in, C4 4/15in. Difference in FIFTHS of inch?",12,"20/3-64/15=12/5."),
            new("460 students walk.20% public transit count?",92,"460×.20=92."),
            new("460 students walk.5% bike count?",23,"460×.05=23."),
            new("460 students walk.110% school bus count?",506,"460×1.1=506."),
            new("2.43×13 in HUNDREDTHS?",3159,"31.59."),
            new("A 5.1kg bag is how many GRAMS?",5100,"1kg=1000grams."),
            new("1092÷60, result in TENTHS?",182,"18.2.")
        ],
        [L0513DecimalLogic] =
        [
            new("109.2÷6=1092÷60. 1=yes,0=no.",1,"Multiply both by10."),
            new("109.2÷6=18.2. 1=yes,0=no.",1,"Check18.2×6."),
            new("31.59÷13=2.43. 1=yes,0=no.",1,"13×2.43=31.59."),
            new("In 31.59÷13, the subtraction52 from55 in long division represents TENTHS. 1=yes,0=no.",1,"55 tenths step."),
            new("In31.59÷13, the subtraction39 from39 in long division represents HUNDREDTHS. 1=yes,0=no.",1,"39 hundredths step."),
            new("61.12÷3.2=611.2÷32. 1=yes,0=no.",1,"Scale×10."),
            new("61.12÷3.2=19.1. 1=yes,0=no.",1,"3.2×19.1=61.12."),
            new("Bag5.1kg containing2.5gram pennies has about2040 pennies neglecting bag weight. 1=yes,0=no.",1,"5100/2.5."),
            new("0.72-0.4=0.32. 1=yes,0=no.",1,"Subtract with place value."),
            new("11.3-1.75=9.55. 1=yes,0=no.",1,"Difference."),
            new("Plant C4 4/15 is16/25 as tall as plant B6⅔. 1=yes,0=no.",1,"Fraction ratio."),
            new("School bus110% of460 walkers equals506. 1=yes,0=no.",1,"Scale factor1.10."),
            new("Scaling only divisor changes quotient. 1=yes,0=no.",1,"Denominator factor."),
            new("The original penny count assumes penny-only mass, ignoring bag mass. 1=yes,0=no.",1,"Model assumption.")
        ],
        [L0514DecimalContext] =
        [
            new("A12m ribbon has9 pieces cut at0.4m each. Remaining ribbon in TENTHS OF METER?",84,"12-9×.4=8.4."),
            new("Diego has8.4m remaining ribbon. Each wreath needs0.6m. Number of wreaths?",14,"8.4/.6=14."),
            new("Amazon rainforest6.42million km² in1994, 2014 at50/59. Closest WHOLE TENTH million km²?",54,"6.42×50/59≈5.44million, about5.4."),
            new("Jada homework141 test scores87,81,91. Total points earned?",400,"141+87+81+91=400."),
            new("Maximum before final homework150 plus3 tests100 each. Total possible?",450,"150+300=450."),
            new("Jada with400/450 before final. Percentage in NINTHS OF PERCENT?",800,"400/450×100=800/9 percent."),
            new("Jada receives92 of100 final. Total earned?",492,"400+92=492."),
            new("After92 final, total possible points?",550,"450+100=550."),
            new("Jada needs90% of550 total points. Minimum points required?",495,"550×.9=495."),
            new("Jada has400 before final and needs495 total. Minimum final score?",95,"495-400=95."),
            new("0.151-0.028 in THOUSANDTHS?",123,"0.123."),
            new("0.106-0.0315 in TEN-THOUSANDTHS?",745,"0.0745."),
            new("3.572-2.6014 in TEN-THOUSANDTHS?",9706,"0.9706."),
            new("24.2÷1.1 equals?",22,"242÷11=22."),
            new("13.25÷0.4 in EIGHTHS?",265,"13.25/.4=33.125=265 eighths."),
            new("For170.28÷0.08, quotient in HALVES?",4257,"2128.5=4257 halves."),
            new("Remainder after9 ribbon cuts of0.4m from12m in CENTIMETERS?",840,"8.4m=840cm."),
            new("12m ribbon divided into0.6m sections before cuts. Number?",20,"12/.6=20."),
            new("If class total550 points,90% is how many?",495,"550×.9=495.")
        ],
        [L0514DecimalReasoning] =
        [
            new("Ribbon12m minus9 pieces0.4m leaves8.4m. 1=yes,0=no.",1,"12-3.6=8.4."),
            new("Eight point four meters can be cut into14 pieces of0.6m. 1=yes,0=no.",1,"8.4/.6=14."),
            new("Amazon6.42million×50/59 is approximately5.4million. 1=yes,0=no.",1,"50/59≈.847."),
            new("Jada earns400 points of450 possible before final, less than90%. 1=yes,0=no.",1,"400/450≈88.89%."),
            new("If Jada scores92 on100-point final she reaches90% overall. 1=yes,0=no.",0,"492/550≈89.45%."),
            new("Jada needs95 on final to reach495 of550 total points. 1=yes,0=no.",1,"495/550=90%."),
            new("Jada can compute final course percentage by averaging homework/test percentages without accounting for point totals. 1=yes,0=no.",0,"Use points earned/total possible."),
            new("0.151-0.028=0.123. 1=yes,0=no.",1,"Align decimals."),
            new("0.106-0.0315=0.0745. 1=yes,0=no.",1,"Decimal difference."),
            new("3.572-2.6014=.9706. 1=yes,0=no.",1,"Decimal difference."),
            new("24.2÷1.1=22. 1=yes,0=no.",1,"Both×10."),
            new("13.25÷0.4=33.125. 1=yes,0=no.",1,"Scale10."),
            new("170.28÷0.08=2128.5. 1=yes,0=no.",1,"Scale100."),
            new("On hurdle diagrams, intervals between hurdles are counted between markers, not automatically equal to total hurdle count. 1=yes,0=no.",1,"Geometry of marked gaps."),
            new("An estimate may be preferable when original source asks closest value rather than exact value. 1=yes,0=no.",1,"Precision appropriate to task.")
        ],
        [L0515FirstPartyMeasurements] =
        [
            new("A6-inch square sheet is how many CENTIMETERS at2.54cm/in, in HUNDREDTHS?",1524,"6×2.54=15.24cm."),
            new("An8-inch square sheet is how many CENTIMETERS in HUNDREDTHS?",2032,"8×2.54=20.32cm."),
            new("A12-inch square sheet in HUNDREDTHS of cm?",3048,"12×2.54=30.48cm."),
            new("Paper12in side compared with6in side. Length ratio?",2,"12/6=2."),
            new("Paper8in vs6in side. Ratio in THIRDS?",4,"8/6=4/3."),
            new("Paper9in vs6in side. Ratio in HALVES?",3,"9/6=3/2."),
            new("Area of6in square sheet in SQUARE INCHES?",36,"6×6=36."),
            new("Area of12in square sheet in SQUARE INCHES?",144,"12²=144."),
            new("Area of12in square as multiple of6in square?",4,"144/36=4."),
            new("An ideal open-top box has square base4cm side and height1cm. Surface area in SQCM?",32,"4²+4×4×1=32."),
            new("Ideal open-top box base8cm side height2cm. Surface area in SQCM?",128,"8²+4×8×2=128."),
            new("Ideal open-top box base8cm height2cm: height relative to1cm box?",2,"2/1=2."),
            new("Ideal open-top box base8cm height2cm: area relative to base4cm height1cm box?",4,"128/32=4."),
            new("A sheet12in is how many TIMES area of6in sheet?",4,"Double sides gives quadruple area."),
            new("A24in sheet compared with6in sheet has side multiplier?",4,"24/6=4."),
            new("Area multiplier of24in vs6in square sheets?",16,"4²=16."),
            new("A12in sheet compared with6in, difference in INCHES of side?",6,"12-6=6."),
            new("An ideal base4×4cm and height1cm open box, base area in SQCM?",16,"4×4."),
            new("An ideal base8×8cm and height2cm open box, four side areas total SQCM?",64,"4×8×2."),
            new("Two independently measured box heights1.0 and2.0cm. Height ratio?",2,"2/1=2.")
        ],
        [L0515MeasurementLogic] =
        [
            new("A12-inch square has twice the side length of a6-inch square. 1=yes,0=no.",1,"12/6=2."),
            new("Doubling a square's side length doubles its area. 1=yes,0=no.",0,"Area scales by factor4."),
            new("A12inch square has4times area of6inch square. 1=yes,0=no.",1,"144/36=4."),
            new("The quotient of the lengths of an8in and6in sheet is4/3. 1=yes,0=no.",1,"8/6=4/3."),
            new("A 12-inch side is30.48cm at2.54cm per inch. 1=yes,0=no.",1,"12×2.54."),
            new("Open-top box surface area includes the top cover as a full sixth face. 1=yes,0=no.",0,"No top."),
            new("An ideal open box with base4cm square and height1cm has surface area32sqcm. 1=yes,0=no.",1,"16+16=32."),
            new("Doubling all physical box dimensions increases surface area by factor4. 1=yes,0=no.",1,"Squared scale."),
            new("Doubling all box dimensions increases volume by factor8. 1=yes,0=no.",1,"Cubed scale."),
            new("If paper measurements are only precise to about1mm, quoting ratios to six decimal places may imply unsupported precision. 1=yes,0=no.",1,"Precision limitation."),
            new("Different students can measure slightly different folded box edges owing to folds and paper thickness. 1=yes,0=no.",1,"Real-world uncertainty."),
            new("An original physical box's exact height is guaranteed by the nominal paper side length even without measuring or knowing fold method. 1=yes,0=no.",0,"Depends on folds."),
            new("The source lesson is an optional hands-on capstone with two project activities. 1=yes,0=no.",1,"Folding and measurement comparison."),
            new("Without original measured dimensions one cannot validly auto-grade a specific student's origami box surface area against a fixed answer. 1=yes,0=no.",1,"Teacher evaluates measured inputs and tolerance."),
            new("A box surface area formula needs square units, while side lengths need linear units. 1=yes,0=no.",1,"Dimension analysis.")
        ],
        [L0601Unknowns] =
        [
            new("x+4=17. Find x.",13,"17-4=13."),
            new("5×x=35. Find x.",7,"35÷5=7."),
            new("x+3=9. Find x.",6,"9-3=6."),
            new("3×x=9. Find x.",3,"9÷3=3."),
            new("9=3×x. Find x.",3,"9÷3=3."),
            new("3+x=9. Find x.",6,"9-3=6."),
            new("x=9-3. Find x.",6,"x=6."),
            new("x=9÷3. Find x.",3,"x=3."),
            new("x+x+x=9. Find x.",3,"3x=9."),
            new("x+9=16. Find x.",7,"16-9=7."),
            new("4×x=28. Find x.",7,"28÷4=7."),
            new("Potatoes cost252cents for4.5lb. Cents per POUND?",56,"252/4.5=56."),
            new("Broccoli cost775cents for2.5lb. Cents per POUND?",310,"775/2.5=310."),
            new("Pears cost245cents for2.5lb. Cents per POUND?",98,"245/2.5=98."),
            new("Sports drink16.9fl oz; Andre drinks80%. Amount in HUNDREDTHS fl oz?",1352,"16.9×.8=13.52."),
            new("Calcium reference1200mg, one cup milk provides25%. Milligrams?",300,"1200/4=300."),
            new("A tape has17 total, known part4. Unknown part?",13,"17-4=13."),
            new("A tape has5 equal segments totaling35. Each segment?",7,"35/5=7.")
        ],
        [L0601TapeLogic] =
        [
            new("Equation x+4=17 can be shown with two adjoining parts x and4 totaling17. 1=yes,0=no.",1,"Additive diagram."),
            new("Equation5x=35 can be shown by5 equal x parts totaling35. 1=yes,0=no.",1,"Multiplicative diagram."),
            new("x+3=9 and3+x=9 describe same additive diagram. 1=yes,0=no.",1,"Commutative addition."),
            new("3x=9 and9=3x describe same equal-group relation. 1=yes,0=no.",1,"Symmetry of equality."),
            new("3x=9 andx+3=9 represent same unknown value. 1=yes,0=no.",0,"x=3 vsx=6."),
            new("x+x+x=9 can represent3x=9. 1=yes,0=no.",1,"Three equal parts."),
            new("x+9=16 has x=7. 1=yes,0=no.",1,"7+9=16."),
            new("4x=28 has x=7. 1=yes,0=no.",1,"4×7=28."),
            new("A tape representing5x=35 should contain exactly5 equal-sized unknown pieces. 1=yes,0=no.",1,"Repeated equal groups."),
            new("An equation and tape model should use same total and part sizes. 1=yes,0=no.",1,"Represent same relationship."),
            new("Potato unit price252cents/4.5lb equals56cents/lb. 1=yes,0=no.",1,"Rate division."),
            new("Broccoli775cents/2.5lb equals310cents/lb. 1=yes,0=no.",1,"Rate division."),
            new("Drinking80% of16.9fl oz yields13.52fl oz. 1=yes,0=no.",1,"Multiply by.8."),
            new("25% of1200mg is300mg. 1=yes,0=no.",1,"Quarter1200."),
            new("A correct x numerical value alone certifies that a student's drawn tape diagram correctly represents the equation. 1=yes,0=no.",0,"Teacher must check arrangement."),
            new("Division and multiplication equations with the same numeric x always require same visual number of pieces. 1=yes,0=no.",0,"Additive and multiplicative structure differ.")
        ],
        [L0602EquationSolutions] =
        [
            new("Bottle24oz and10oz remain. Amount x consumed in OUNCES?",14,"24-10=14."),
            new("Five equal pencils total34.5in. One length x in TENTHS OF INCH?",69,"34.5/5=6.9in."),
            new("2a=4.6. a in TENTHS?",23,"a=2.3."),
            new("b+2=4.6. b in TENTHS?",26,"b=2.6."),
            new("c÷2=4.6. c in TENTHS?",92,"c=9.2."),
            new("d-2=4.6. d in TENTHS?",66,"d=6.6."),
            new("e+3/8=2. e in EIGHTHS?",13,"e=13/8."),
            new("(1/8)f=3. Find f.",24,"f=24."),
            new("g÷(8/5)=1. g in FIFTHS?",8,"g=8/5."),
            new("One orange supplies75% of45mg recommended vitamin C. Amount in HUNDREDTHS MG?",3375,"45×.75=33.75."),
            new("Band90 children,20% own instruments. Owner count?",18,"90×.2=18."),
            new("Band90 children, remaining80% rent. Renter count?",72,"90×.8=72."),
            new("Band90 children, percentage renters?",80,"100-20=80%."),
            new("0.25×1.4 in HUNDREDTHS?",35,"0.35."),
            new("0.061×0.43 in HUNDRED-THOUSANDTHS?",2623,"0.02623."),
            new("1.017×0.072 in MILLIONTHS?",73224,"0.073224."),
            new("5.226×0.037 in MILLIONTHS?",193362,"0.193362."),
            new("If x+10=24. Find x.",14,"Subtract10."),
            new("If5x=34.5, find x in TENTHS?",69,"Divide by5.")
        ],
        [L0602EquationTruth] =
        [
            new("5+0=0 is true. 1=yes,0=no.",0,"5≠0."),
            new("15×0=0 is true. 1=yes,0=no.",1,"Zero product."),
            new("1.4+2.7=4.1 is true. 1=yes,0=no.",1,"Decimal sum."),
            new("(2/3)×(5/9)=7/12 is true. 1=yes,0=no.",0,"Product10/27."),
            new("4⅔=5-⅓ is true. 1=yes,0=no.",1,"Both14/3."),
            new("24-10=x models consumed water from24 to10. 1=yes,0=no.",1,"Original C."),
            new("x+10=24 models water consumed. 1=yes,0=no.",1,"Original D."),
            new("10x=24 models consumed water. 1=yes,0=no.",0,"Different multiplicative relation."),
            new("5x=34.5 models five pencils length x each. 1=yes,0=no.",1,"Original B."),
            new("34.5÷5=x models each pencil. 1=yes,0=no.",1,"Original C."),
            new("5+x=34.5 models five identical pencil lengths totaling34.5. 1=yes,0=no.",0,"Wrong operation."),
            new("For2a=4.6, a=2.3 makes equality true. 1=yes,0=no.",1,"Check2×2.3=4.6."),
            new("A variable stands for a quantity, and a solution makes the equation true. 1=yes,0=no.",1,"Definition."),
            new("Writing10m can mean10×m. 1=yes,0=no.",1,"Juxtaposition notation."),
            new("A number may be a solution without satisfying an arbitrary different equation. 1=yes,0=no.",1,"Equation-dependent truth."),
            new("20% of90band students own instruments, so80% rent. 1=yes,0=no.",1,"Complement100%."),
            new("1.4+2.7=4.1, so1.4 and2.7 must be equal. 1=yes,0=no.",0,"Equality relates sums, not addends.")
        ],
        [L0603HangerValues] =
        [
            new("Balanced hanger has3 equal x shapes against6 unit weights. x?",2,"3x=6."),
            new("Two identical circles x balance3 unit squares. x in HALVES?",3,"2x=3 gives1.5."),
            new("Hanger w+1.3=2.7. Value w in TENTHS?",14,"w=1.4."),
            new("Hanger3y=5.1. Value y in TENTHS?",17,"y=1.7."),
            new("Hanger z+1/3=1/2. z in SIXTHS?",1,"z=1/6."),
            new("12-m=4. Find m.",8,"12-4=8."),
            new("12=4m. Find m.",3,"12/4=3."),
            new("m-4=12. Find m.",16,"12+4=16."),
            new("m/4=12. Find m.",48,"4×12=48."),
            new("Rectangle area14sq units, x=2⅓. Other side y?",6,"14/(7/3)=6."),
            new("Rectangle area14sq units, x=4⅕. Other side y in THIRDS?",10,"14/(21/5)=10/3."),
            new("Rectangle area14sq units, x=7/6. Other side y?",12,"14/(7/6)=12."),
            new("Lin game goal20dollars,25% saved. Dollars?",5,"20×1/4=5."),
            new("Lin game goal20dollars,75% saved. Dollars?",15,"20×3/4=15."),
            new("Lin game goal20dollars,125% saved. Dollars?",25,"20×1.25=25."),
            new("A balanced scale has 3x=6, if one x removed from each side where possible, x remains?",2,"x=2."),
            new("Three identical weights y balance5.1units. Weight y in HUNDREDTHS?",170,"5.1/3=1.7."),
            new("A hanger has z+1/3=1/2. z in TWELFTHS?",2,"1/6=2/12.")
        ],
        [L0603HangerLogic] =
        [
            new("A hanger with3 identical x and6 unit weights is represented by3x=6. 1=yes,0=no.",1,"Original options A,C."),
            new("For3x=6, the equation x×x×x=6 is equivalent. 1=yes,0=no.",0,"Product x³ rather than3x."),
            new("Removing equal weights from each side of a balanced hanger keeps balance. 1=yes,0=no.",1,"Equality-preserving step."),
            new("Adding an amount to one side only always keeps a balanced hanger in balance. 1=yes,0=no.",0,"Breaks equality."),
            new("Two circles x balance three unit squares represents2x=3. 1=yes,0=no.",1,"Source hanger A."),
            new("w+1.3=2.7 gives w=1.4. 1=yes,0=no.",1,"Subtract1.3."),
            new("3y=5.1 gives y=1.7. 1=yes,0=no.",1,"Divide both sides3."),
            new("z+1/3=1/2 gives z=1/6. 1=yes,0=no.",1,"Subtract one third."),
            new("If3x=6, dividing both sides by3 preserves the solution. 1=yes,0=no.",1,"x=2."),
            new("12-m=4 means m=8. 1=yes,0=no.",1,"Subtract8."),
            new("12=4m means m=3. 1=yes,0=no.",1,"Divide4."),
            new("m-4=12 means m=16. 1=yes,0=no.",1,"Add4."),
            new("m/4=12 means m=48. 1=yes,0=no.",1,"Multiply4."),
            new("A matching tape diagram and hanger must represent same unknown, parts and total. 1=yes,0=no.",1,"Structural equivalence."),
            new("A correct numerical x automatically proves student drew the original hanger geometry correctly. 1=yes,0=no.",0,"Teacher checks drawing."),
            new("The equation3x=6 is additive3 copies of x, not multiplication x cubed. 1=yes,0=no.",1,"x+x+x=3x.")
        ],
        [L0604SolveValues] =
        [
            new("Kiran backpack3lb lighter than Clare14lb. Weight Kiran in POUNDS?",11,"14-3=11."),
            new("Each notebook has60 sheets. Five notebooks total sheets?",300,"5×60=300."),
            new("2x=5. x in HALVES?",5,"x=2.5."),
            new("y+1.8=14.7. y in TENTHS?",129,"y=12.9."),
            new("6=(1/2)z. Find z.",12,"z=12."),
            new("3¼=½+w. w in QUARTERS?",11,"13/4-2/4=11/4."),
            new("2.5t=10. Find t.",4,"10/2.5=4."),
            new("3x=18. Find x.",6,"x=6."),
            new("3+x=18. Find x.",15,"x=15."),
            new("17-6=x. Find x.",11,"x=11."),
            new("21.2×0.02 in THOUSANDTHS?",424,"0.424."),
            new("2.05×0.004 in TEN-THOUSANDTHS?",82,"0.0082."),
            new("Twenty-five percent of60grams in GRAMS?",15,"60/4=15."),
            new("Kiran weight11lb, Clare14lb. Difference in POUNDS?",3,"14-11=3."),
            new("Four notebooks60 sheets each. Total sheets?",240,"4×60=240."),
            new("A backpack weighs11lb and is3lb lighter than Clare's. Clare's weight?",14,"11+3=14."),
            new("If2.5t=10, t repeated twice and half a time has total10. t?",4,"2.5×4=10."),
            new("Equation3¼=½+w, verify w=11/4. Sum in QUARTERS?",13,"2/4+11/4=13/4.")
        ],
        [L0604EquationReasoning] =
        [
            new("x+3=14 models Kiran's lighter backpack compared to14lb. 1=yes,0=no.",1,"Original appropriate equation."),
            new("x=14-3 models Kiran's weight. 1=yes,0=no.",1,"Difference."),
            new("3x=14 models Kiran's lighter backpack. 1=yes,0=no.",0,"Wrong relation."),
            new("For notebooks5×60 sheets, y=5×60 models total sheets. 1=yes,0=no.",1,"Source valid."),
            new("For five notebooks60 sheets each, y/5=60 models total y. 1=yes,0=no.",1,"Equivalent."),
            new("For five notebooks60 sheets each,5y=60 is correct for total y. 1=yes,0=no.",0,"Would give12."),
            new("2x=5 has x=2.5. 1=yes,0=no.",1,"Divide2."),
            new("y+1.8=14.7 has y=12.9. 1=yes,0=no.",1,"Subtract1.8."),
            new("6=½z has z=12. 1=yes,0=no.",1,"Double."),
            new("3¼=½+w has w=2¾. 1=yes,0=no.",1,"Subtract½."),
            new("2.5t=10 has t=4. 1=yes,0=no.",1,"Divide2.5."),
            new("3x=18 and3+x=18 both have same solution6. 1=yes,0=no.",0,"Second x15."),
            new("25% of60 equals quarter of60. 1=yes,0=no.",1,"15."),
            new("21.2×0.02=.424. 1=yes,0=no.",1,"Place-value multiplication."),
            new("2.05×0.004=.0082. 1=yes,0=no.",1,"Place-value product."),
            new("One can solve a linear equation using a diagram, inverse operation, or equality-preserving steps. 1=yes,0=no.",1,"Strategies flexible.")
        ],
        [L0605FractionBarNumeric] =
        [
            new("3.15÷0.45 equals?",7,"315/45=7."),
            new("3/4×x=15. Find x.",20,"15÷3/4=20."),
            new("4a=32. a?",8,"32/4=8."),
            new("4=32b. b in EIGHTHS?",1,"b=1/8."),
            new("10c=26. c in TENTHS?",26,"c=2.6."),
            new("26=100d. d in HUNDREDTHS?",26,"d=0.26."),
            new("3/4+x=2. x in QUARTERS?",5,"x=5/4."),
            new("1.5x=6. x?",4,"6/1.5=4."),
            new("Lilac mixture20% blue uses4cups. Total batch CUPS?",20,"4/0.2=20."),
            new("Lilac mix40% white and total20cups. White cups?",8,"20×.4=8."),
            new("Lilac mix20% blue40% white. Red percentage?",40,"Remaining40%."),
            new("Lilac mix20cups with40% red. Red cups?",8,"20×.4=8."),
            new("Triangle P base12in height8in. Area SQ IN?",48,"12×8/2=48."),
            new("Triangle Q base15in height6.5in. Area in EIGHTHS sqin?",390,"15×6.5/2=48.75=390eighths."),
            new("Triangle Q area48.75, P area48. Difference in HUNDREDTHS sqin?",75,"0.75."),
            new("Compare triangles P48 and Q48.75. Enter1 P larger,2 Q larger.",2,"Q bigger."),
            new("Fraction2.5/0.5 equals?",5,"2.5÷0.5=5."),
            new("Fraction half over three-fifths, answer in SIXTHS?",5,"(1/2)/(3/5)=5/6.")
        ],
        [L0605FractionBarLogic] =
        [
            new("3.15/0.45=3.15÷0.45. 1=yes,0=no.",1,"Fraction bar is division."),
            new("3.15/0.45=7. 1=yes,0=no.",1,"Divide315/45."),
            new("3.15×0.45 represents same value as3.15/0.45. 1=yes,0=no.",0,"Multiplication versus division."),
            new("3.15×(1/0.45) equals3.15/0.45. 1=yes,0=no.",1,"Reciprocal product."),
            new("3.15÷45/100 equals3.15/0.45. 1=yes,0=no.",1,"0.45=45/100."),
            new("3.15×100/45 equals3.15÷0.45. 1=yes,0=no.",1,"Multiply reciprocal."),
            new("For3/4 x=15, x=15÷3/4 is valid. 1=yes,0=no.",1,"Original choice A/E."),
            new("For3/4 x=15, x=(4/3)×15 is valid. 1=yes,0=no.",1,"Original choice C."),
            new("For3/4 x=15, x=(3/4)×15 is valid. 1=yes,0=no.",0,"Wrong operation."),
            new("4=32b gives b=1/8. 1=yes,0=no.",1,"Four thirty-seconds."),
            new("26=100d givesd=0.26. 1=yes,0=no.",1,"Division100."),
            new("Lilac paint with20% blue4cups has20cups total. 1=yes,0=no.",1,"4÷.2=20."),
            new("White40% and red40% each8cups in20cup lilac batch. 1=yes,0=no.",1,"40%×20=8."),
            new("Triangle Q with15in base6.5in height has larger area than triangle P12in base8in height. 1=yes,0=no.",1,"48.75>48."),
            new("A denominator0 is allowed for arbitrary fraction bar expressions. 1=yes,0=no.",0,"Division by zero invalid."),
            new("Diagram of a fundraiser thermometer requires reading the tick spacing rather than guessing from unlabeled art. 1=yes,0=no.",1,"Visual source must be checked.")
        ],
        [L0606Expressions] =
        [
            new("Blue ribbon10in, red7in shorter. Red inches?",3,"10-7=3."),
            new("Blue ribbon27in, red7in shorter. Red inches?",20,"27-7=20."),
            new("Red ribbon12in, blue7in longer. Blue inches?",19,"12+7=19."),
            new("Tyler has3times as many books as Mai. Tyler15. Mai books?",5,"15/3=5."),
            new("Tyler21books, Mai one third as many. Mai?",7,"21/3=7."),
            new("Tyler18books, Mai one third. Mai?",6,"18/3=6."),
            new("Bottle holds24oz, has x=9oz currently. Empty capacity?",15,"24-9=15."),
            new("Bottle holds24oz and x=17oz water. Empty capacity?",7,"24-17=7."),
            new("Han house450m from school. Lin135m closer. Lin distance?",315,"450-135=315."),
            new("Tyler36 songs, Noah one-quarter as many. Noah songs?",9,"36/4=9."),
            new("50dollars spent10% clothes,20% games. Clothes dollars?",5,".1×50=5."),
            new("50dollars spent10% clothes,20% games. Games dollars?",10,".2×50=10."),
            new("50dollars after clothes and games, rest books. Books dollars?",35,"50-5-10=35."),
            new("Bin capacity50gal,5gal content percent?",10,"5/50×100=10."),
            new("Bin capacity50gal,30gal content percent?",60,"30/50=60%."),
            new("Bin capacity50gal,45gal content percent?",90,"45/50=90%."),
            new("Bin capacity50gal,100gal hypothetical amount percent?",200,"100/50=200% comparison, exceeds physical capacity."),
            new("Three times x at x=6?",18,"3×6."),
            new("x-7 for x=27?",20,"27-7."),
            new("x/3 for x=21?",7,"21/3.")
        ],
        [L0606ExpressionLogic] =
        [
            new("Blue ribbon length x and red7in shorter: red length expression x-7. 1=yes,0=no.",1,"Difference."),
            new("Blue x and red7 shorter: red expression7-x. 1=yes,0=no.",0,"Reversed order."),
            new("Blue x and red7 shorter: blue length if red12 is19. 1=yes,0=no.",1,"12+7=19."),
            new("Tyler has3times Mai's books, Tyler x books: Mai expression x/3. 1=yes,0=no.",1,"Divide3."),
            new("Tyler3times Mai, Tyler x: Mai expression3x. 1=yes,0=no.",0,"3x is Tyler in terms of Mai."),
            new("Bottle holds24oz, has x oz now:24-x is available space. 1=yes,0=no.",1,"Capacity minus contents."),
            new("A variable expression can be meaningful without being an equation with equals sign. 1=yes,0=no.",1,"Expression denotes value."),
            new("Han450m from school, Lin135 closer means Lin315m away. 1=yes,0=no.",1,"450-135=315."),
            new("Noah one quarter of Tyler36songs has9. 1=yes,0=no.",1,"36/4."),
            new("Of50dollars, spending10% plus20% leaves70% for books. 1=yes,0=no.",1,"100-30=70."),
            new("5gal is10% of50gal bin capacity. 1=yes,0=no.",1,"5/50=.1."),
            new("100gal is200% of50gal reference, though exceeds the actual bin capacity. 1=yes,0=no.",1,"Comparison vs physical limit."),
            new("x-7 evaluated at x=10 equals3. 1=yes,0=no.",1,"Substitution."),
            new("x/3 evaluated at x=21 equals7. 1=yes,0=no.",1,"Substitution."),
            new("The expression24-x represents empty ounces only when x is contents between0 and24. 1=yes,0=no.",1,"Contextual domain."),
            new("Expression meaning remains unchanged for all valid x even as numerical value varies. 1=yes,0=no.",1,"Generalization.")
        ],
        [L0607PercentEquations] =
        [
            new("Crew paves3/4mile,50% of job. Total road in HALF-MILE UNITS?",3,"3/4÷.5=1.5miles=3halves."),
            new("40% of x is35. x in HALVES?",175,"35/.4=87.5, or175halves."),
            new("Priya finishes9 questions,60% of exam. Total questions?",15,"9/.6=15."),
            new("20% of a is11. a?",55,"11/.2=55."),
            new("75% of b is12. b?",16,"12/.75=16."),
            new("80% of c is20. c?",25,"20/.8=25."),
            new("200% of d is18. d?",9,"18/2=9."),
            new("2n-3=7. Find n.",5,"2n=10."),
            new("2n-3=7. Coefficient of n?",2,"2."),
            new("(2/5)x=1/8. x in SIXTEENTHS?",5,"1/8÷2/5=5/16."),
            new("0.009÷0.001=?",9,"Scale thousand."),
            new("0.009÷0.002 in HALVES?",9,"4.5=9halves."),
            new("0.0045÷0.001 in HALVES?",9,"4.5."),
            new("0.0045÷0.002 in QUARTERS?",9,"2.25=9quarters."),
            new("A goal is87.50dollars and40% achieved. Dollars achieved in CENTS?",3500,"87.5×.4=35."),
            new("A goal is15 questions,60% completed. Completed?",9,".6×15=9."),
            new("A road1.5miles total,50% complete. Completed in QUARTER-MILE UNITS?",3,"0.75mile=3quarters."),
            new("A quantity9 is200% of a reference. Reference in HALF-UNITS?",9,"9/2=4.5,9 halves."),
            new("20% of55=?",11,"0.2×55."),
            new("75% of16=?",12,"0.75×16.")
        ],
        [L0607PercentEquationLogic] =
        [
            new("40% of x is35 can be written0.4x=35. 1=yes,0=no.",1,"Percent as decimal multiplier."),
            new("40% of x=35 has x=87.5. 1=yes,0=no.",1,"Divide by.4."),
            new("60% of exam is9 questions means exam has15 questions. 1=yes,0=no.",1,"9/.6=15."),
            new("A crew's3/4mile paved is50%, so whole road1.5miles. 1=yes,0=no.",1,"Double."),
            new("20% of a=11 implies a=2.2. 1=yes,0=no.",0,"Actual55."),
            new("75% of b=12 implies b=16. 1=yes,0=no.",1,"12/.75."),
            new("80% of c=20 implies c=25. 1=yes,0=no.",1,"20/.8."),
            new("200% of d=18 implies d=9. 1=yes,0=no.",1,"2d=18."),
            new("Percent comparisons can exceed100% when the compared amount exceeds reference. 1=yes,0=no.",1,"200%=double."),
            new("2n-3=7 has n=5. 1=yes,0=no.",1,"2×5-3=7."),
            new("In2n-3=7, coefficient of n is2. 1=yes,0=no.",1,"Multiplies variable."),
            new("x=5/16 solves (2/5)x=1/8. 1=yes,0=no.",1,"2/5×5/16=1/8."),
            new("0.009÷0.002=4.5. 1=yes,0=no.",1,"9/2."),
            new("0.0045÷0.002=2.25. 1=yes,0=no.",1,"4.5/2."),
            new("Identifying the100% reference is necessary to interpret a percent equation. 1=yes,0=no.",1,"Defines whole."),
            new("A double number line should align0%,100% with0 and the full reference amount. 1=yes,0=no.",1,"Proportional marks.")
        ],
        [L0608ExpressionValues] =
        [
            new("x+3 at x=1?",4,"1+3=4."),
            new("2x at x=1?",2,"2×1=2."),
            new("x+3 at x=2?",5,"2+3=5."),
            new("2x at x=2?",4,"2×2=4."),
            new("x+3 at x=3?",6,"3+3=6."),
            new("2x at x=3?",6,"2×3=6."),
            new("x+3 at x=4?",7,"4+3=7."),
            new("2x at x=4?",8,"2×4=8."),
            new("4x at x=5?",20,"4×5=20."),
            new("15+x at x=5?",20,"15+5=20."),
            new("4x at x=6?",24,"4×6=24."),
            new("15+x at x=6?",21,"15+6=21."),
            new("2b+b at b=3?",9,"3×3=9."),
            new("3b at b=3?",9,"3×3=9."),
            new("80% of x=100. Find x.",125,"100/.8=125."),
            new("Jada dog5½in puppy and grows14½in. Current height in INCHES?",20,"5.5+14.5=20."),
            new("Lin picked9¾lb apples,3 times Andre. Andre in QUARTER-POUND UNITS?",13,"9.75/3=3.25=13quarters."),
            new("2.3×1.4 in HUNDREDTHS?",322,"3.22."),
            new("1.72×2.6 in THOUSANDTHS?",4472,"4.472."),
            new("18.2×0.2 in HUNDREDTHS?",364,"3.64."),
            new("15×1.2=?",18,"18."),
            new("141.75÷2.5 in TENTHS?",567,"56.7.")
        ],
        [L0608EquivalentLogic] =
        [
            new("x+3 and2x are equal for x=3. 1=yes,0=no.",1,"Both6."),
            new("x+3 and2x are equivalent for EVERY x. 1=yes,0=no.",0,"Atx=1,4vs2."),
            new("x+3 and2x are equal at x=1. 1=yes,0=no.",0,"4vs2."),
            new("4x and15+x are equal whenx=5. 1=yes,0=no.",1,"20 each."),
            new("4x and15+x are equivalent for everyx. 1=yes,0=no.",0,"Atx=6,24vs21."),
            new("2b+b and3b are equivalent for everyb. 1=yes,0=no.",1,"Combine like terms."),
            new("2b+b and3b have equal values for b=1,2,3. 1=yes,0=no.",1,"Identity."),
            new("Two expressions equal at one selected variable value must be equivalent. 1=yes,0=no.",0,"Counterexample sufficient."),
            new("To disprove equivalence, provide one input where evaluated values differ. 1=yes,0=no.",1,"Counterexample."),
            new("To prove equivalence, checking only x=3 suffices for all possible x. 1=yes,0=no.",0,"Not proof."),
            new("80%x=100 gives x=125. 1=yes,0=no.",1,"0.8×125=100."),
            new("Dog puppy height5.5 plus14.5 growth gives20in. 1=yes,0=no.",1,"Add."),
            new("Andre picked3.25lb if Lin picked9.75lb triple Andre. 1=yes,0=no.",1,"9.75/3=3.25."),
            new("2.3×1.4=3.22. 1=yes,0=no.",1,"Decimal multiplication."),
            new("1.72×2.6=4.472. 1=yes,0=no.",1,"Decimal multiplication."),
            new("An equivalent-expression length model should preserve the same variable part structure for every value of the variable. 1=yes,0=no.",1,"Structural identity.")
        ],
        [L0609DistributeNumbers] =
        [
            new("Rectangle height5, width2+4. Area?",30,"5×6=30."),
            new("Rectangle height5, left width2. Left area?",10,"5×2=10."),
            new("Rectangle height5, right width4. Right area?",20,"5×4=20."),
            new("19×50=?",950,"(10+9)×50=500+450."),
            new("20×50 minus50=?",950,"1000-50."),
            new("98×24=?",2352,"(100-2)×24=2400-48."),
            new("21×15=?",315,"(20+1)×15=300+15."),
            new("0.51×40 in TENTHS?",204,"20.4=204tenths."),
            new("Eight friends each buy popcorn for299cents. Total CENTS?",2392,"8×299=2392."),
            new("120% of x is78. x?",65,"78/1.2=65."),
            new("Kiran15years; aunt17 older. Aunt age?",32,"15+17=32."),
            new("Kiran30years; aunt17 older. Aunt age?",47,"30+17=47."),
            new("Aunt age60,17 older than Kiran. Kiran age?",43,"60-17=43."),
            new("Four copies of a=3 have total value?",12,"4×3=12."),
            new("Four copies of a=2 have total value?",8,"4×2=8."),
            new("4a with a=1=?",4,"4×1."),
            new("8 popcorn bags at3dollars each cost in CENTS?",2400,"8×300=2400."),
            new("Popcorn 299c each for8 bags saves how many CENTS vs300c each?",8,"8×1=8cents."),
            new("(100-2)×24: amount subtracted from2400?",48,"2×24=48."),
            new("5×(2+4) decomposed to5×2+5×4 total?",30,"10+20=30.")
        ],
        [L0609DistributeLogic] =
        [
            new("5(2+4) is the area of rectangle height5 width6. 1=yes,0=no.",1,"Original A."),
            new("5×2+5×4 gives same outer rectangle area. 1=yes,0=no.",1,"Original C."),
            new("5×6 gives same area. 1=yes,0=no.",1,"Original F."),
            new("5×2+4 gives same area as5(2+4). 1=yes,0=no.",0,"14 vs30."),
            new("5×2×4 gives same area as5×6. 1=yes,0=no.",0,"40 vs30."),
            new("19×50 can be found by10×50 plus9×50. 1=yes,0=no.",1,"Additive distribution."),
            new("19×50 can be found from20×50 minus50. 1=yes,0=no.",1,"Subtraction distribution."),
            new("98×24=(100-2)×24. 1=yes,0=no.",1,"Decompose98."),
            new("21×15=(20+1)×15. 1=yes,0=no.",1,"Decompose21."),
            new("0.51×40=0.5×40+0.01×40. 1=yes,0=no.",1,"Distribution decimals."),
            new("8 popcorn bags at2.99 each cost23.92 dollars. 1=yes,0=no.",1,"8×3 minus8cents."),
            new("a+a+a+a and4a are equivalent for every real a. 1=yes,0=no.",1,"Repeated addition."),
            new("120% of65 is78. 1=yes,0=no.",1,"1.2×65=78."),
            new("Kiran aunt17years older: aunt age is x+17 when Kiran x. 1=yes,0=no.",1,"Add17."),
            new("Kiran aunt60years, Kiran43. 1=yes,0=no.",1,"60-17=43."),
            new("For a rectangle partition, total area equals sum of non-overlapping subareas. 1=yes,0=no.",1,"Area additivity.")
        ],
        [L0610VariableArea] =
        [
            new("Rectangle height a=3, widths2+3+4. Total area?",27,"3×9=27."),
            new("Height a=5, widths2,3,4. Area left segment?",10,"5×2=10."),
            new("Height a=5, widths2,3,4. Area middle segment?",15,"5×3=15."),
            new("Height a=5, widths2,3,4. Area right segment?",20,"5×4=20."),
            new("Height a=5, total width9. Total area?",45,"9×5=45."),
            new("A rectangle height6, total width m=8, unshaded width2. Shaded area?",36,"6×(8-2)=36."),
            new("Rectangle height t=3, partition widths5,4. Total area?",27,"(5+4)×3=27."),
            new("35×91-35×89=?",70,"35×(91-89)=70."),
            new("22×87+22×13=?",2200,"22×100=2200."),
            new("(9/11)×(7/10)-(9/11)×(3/10), answer in FIFTY-FIFTHS?",18,"9/11×4/10=18/55."),
            new("111=14a. a in FOURTEENTHS?",111,"a=111/14."),
            new("13.65=b+4.88. b in HUNDREDTHS?",877,"8.77."),
            new("c+1/3=5⅛. c in TWENTY-FOURTHS?",115,"41/8-1/3=115/24."),
            new("(2/5)d=17/4. d in EIGHTHS?",85,"17/4×5/2=85/8."),
            new("5.16=4e. e in HUNDREDTHS?",129,"1.29."),
            new("Andre5½laps in8minutes. Minutes per lap in ELEVENTHS?",16,"8/(11/2)=16/11."),
            new("4b where b=6?",24,"4×6."),
            new("b+b+b+b where b=6?",24,"4×6."),
            new("Rectangle area2a+3a+4a at a=7?",63,"9a=63."),
            new("For6(m-2) when m=5, shaded area?",18,"6×3=18.")
        ],
        [L0610AreaReasoning] =
        [
            new("2a+3a+4a=(2+3+4)a. 1=yes,0=no.",1,"Both9a."),
            new("2a+3a+4a=9a for every a. 1=yes,0=no.",1,"Combine like terms."),
            new("A rectangle of height6 total widthm with unshaded width2 has shaded area6(m-2). 1=yes,0=no.",1,"Subtract narrow area."),
            new("That shaded rectangle area is6(2-m) for positive m>2. 1=yes,0=no.",0,"Wrong sign."),
            new("5t+4t and9t represent same rectangle area. 1=yes,0=no.",1,"Original A,C."),
            new("t(5+4) represents same area as9t. 1=yes,0=no.",1,"Original E."),
            new("t+5+4 represents same rectangle area as9t for every t. 1=yes,0=no.",0,"Original invalid B."),
            new("4×5×t represents same area as9t for every t. 1=yes,0=no.",0,"Original invalid D."),
            new("35×91-35×89=35×2. 1=yes,0=no.",1,"Distribute common factor."),
            new("22×87+22×13=22×100. 1=yes,0=no.",1,"Group factors."),
            new("4b is equivalent to b+b+b+b. 1=yes,0=no.",1,"Original A."),
            new("4b is equivalent to2b+2b. 1=yes,0=no.",1,"Original C."),
            new("4b is equivalent to b÷1/4. 1=yes,0=no.",1,"Original E."),
            new("5½ laps in8min at steady pace gives16/11 minutes per lap. 1=yes,0=no.",1,"8÷11/2."),
            new("A variable rectangle area diagram can justify equivalent expressions for ALL values of the variable. 1=yes,0=no.",1,"Symbolic partition."),
            new("Area measured in square units must equal length times height with same linear unit. 1=yes,0=no.",1,"Dimensional relationship.")
        ],
        [L0611AlgebraEvaluation] =
        [
            new("4(x+2) at x=3?",20,"4×5=20."),
            new("Expanded4x+8 at x=3?",20,"12+8=20."),
            new("(6+8)x at x=3?",42,"14×3=42."),
            new("4(2x+3) at x=5?",52,"4×13=52."),
            new("8x+12 at x=5?",52,"40+12=52."),
            new("6(x+y+z) atx=1,y=2,z=3?",36,"6×6=36."),
            new("8y-24 at y=5?",16,"40-24=16."),
            new("8(y-3) at y=5?",16,"8×2=16."),
            new("2(4y-12) at y=5?",16,"2(20-12)=16."),
            new("16x+36 at x=2?",68,"32+36=68."),
            new("4(4x+9) at x=2?",68,"4×17=68."),
            new("2(8x+18) at x=2?",68,"2×34=68."),
            new("Square side3cm. Perimeter in CM?",12,"4×3=12."),
            new("Square side7cm. Perimeter in CM?",28,"4×7=28."),
            new("Square perimeter360cm. Side length CM?",90,"360÷4=90."),
            new("Square side3cm. Area SQCM?",9,"3²=9."),
            new("Square side7cm. Area SQCM?",49,"7²=49."),
            new("Square area121cm². Side length CM?",11,"11²=121."),
            new("10=4a. a in HALVES?",5,"a=2.5."),
            new("5b=17.5. b in TENTHS?",35,"b=3.5."),
            new("1.036=10c. c in TEN-THOUSANDTHS?",1036,"c=.1036."),
            new("0.6d=1.8. d?",3,"d=3."),
            new("15=0.1e. e?",150,"e=150.")
        ],
        [L0611DistributionLogic] =
        [
            new("4(x+2) and4x+8 are equivalent. 1=yes,0=no.",1,"Distribution."),
            new("(6+8)x and14x equivalent. 1=yes,0=no.",1,"Combine coefficients."),
            new("4(2x+3) equals8x+12 for all x. 1=yes,0=no.",1,"Expand."),
            new("6(x+y+z)=6x+6y+6z for all x,y,z. 1=yes,0=no.",1,"Distribution."),
            new("8y-24=8(y-3) for all y. 1=yes,0=no.",1,"Priya correct."),
            new("8y-24=2(4y-12) for all y. 1=yes,0=no.",1,"Han correct."),
            new("16x+36=4(4x+9). 1=yes,0=no.",1,"Original C."),
            new("16x+36=2(8x+18). 1=yes,0=no.",1,"Original D."),
            new("16x+36=16(x+20) for all x. 1=yes,0=no.",0,"Adds320."),
            new("16x+36=2(8x+36) for all x. 1=yes,0=no.",0,"Constant72."),
            new("(1/2)z equals z÷2. 1=yes,0=no.",1,"Original B."),
            new("(1/2)z equals(1/4)z+(1/4)z. 1=yes,0=no.",1,"Original D."),
            new("(1/2)z equals z+z for every z. 1=yes,0=no.",0,"2z vs half."),
            new("Square perimeter4s and area s² for side s. 1=yes,0=no.",1,"Geometry formulas."),
            new("A121sqcm square has11cm side. 1=yes,0=no.",1,"Square root."),
            new("An optional practice lesson can be useful without introducing a brand-new mathematical topic. 1=yes,0=no.",1,"Spiraled distribution practice.")
        ],
        [L0612ExponentValues] =
        [
            new("2 to power6 equals?",64,"2×2×2×2×2×2=64."),
            new("4 cubed equals?",64,"4³=64."),
            new("8 squared equals?",64,"8²=64."),
            new("2 to power8 equals?",256,"2^8=256."),
            new("3 to power4 equals?",81,"3^4=81."),
            new("9 squared equals?",81,"9²=81."),
            new("4 to power5 equals?",1024,"4^5=1024."),
            new("4 to power6 equals?",4096,"4×1024=4096."),
            new("4 to power4 equals?",256,"1024÷4=256."),
            new("4 cubed times4 squared equals?",1024,"4^(3+2)=4^5."),
            new("6 cubed equals?",216,"6×6×6=216."),
            new("2 cubed times3 cubed equals?",216,"8×27=216."),
            new("3 cubed times8 equals?",216,"27×8=216."),
            new("a-2.01=5.5. a in HUNDREDTHS?",751,"a=7.51."),
            new("b+2.01=5.5. b in HUNDREDTHS?",349,"b=3.49."),
            new("10c=13.71. c in THOUSANDTHS?",1371,"c=1.371."),
            new("100d=13.71. d in TEN-THOUSANDTHS?",1371,"d=.1371."),
            new("16% of250=?",40,".16×250=40."),
            new("250% of16=?",40,"2.5×16=40."),
            new("45% of72 in TENTHS?",324,"0.45×72=32.4."),
            new("45/72×100, answer in HALVES?",125,"62.5=125halves."),
            new("6(m+n) when m=3 n=4?",42,"6×7=42.")
        ],
        [L0612ExponentReasoning] =
        [
            new("2^6=64. 1=yes,0=no.",1,"Original option A."),
            new("4^3=64. 1=yes,0=no.",1,"Original option C."),
            new("8^2=64. 1=yes,0=no.",1,"Original option D."),
            new("2^8=64. 1=yes,0=no.",0,"256."),
            new("3^4=81. 1=yes,0=no.",1,"Original option D."),
            new("9^2=3^4. 1=yes,0=no.",1,"Original option F."),
            new("4^6=4096. 1=yes,0=no.",1,"4×1024."),
            new("4^3×4^2=4^5. 1=yes,0=no.",1,"Same base combine exponents."),
            new("6^3=216. 1=yes,0=no.",1,"6×6×6."),
            new("2^3×3^3=216. 1=yes,0=no.",1,"(2×3)^3."),
            new("3xy+6yz can be factored as3y(x+2z). 1=yes,0=no.",1,"Common factor3y."),
            new("3xy+6yz can be factored as y(3x+6z). 1=yes,0=no.",1,"Common factor y."),
            new("6(m+n)=6m+6n. 1=yes,0=no.",1,"Source A,C,E."),
            new("6(m+n)=6n+m for all m,n. 1=yes,0=no.",0,"Missing factor6 for m."),
            new("45/100×72 equals45/72×100. 1=yes,0=no.",0,"32.4 vs62.5."),
            new("16% of250 equals250% of16. 1=yes,0=no.",1,"Both40."),
            new("An exponent of3 counts three factors of same base, not base multiplied by3. 1=yes,0=no.",1,"Repeated multiplication."),
            new("A graphical repeated-times3 dot pattern can be represented using powers of3. 1=yes,0=no.",1,"Exponential growth.")
        ],
        [L0613ExponentNumbers] =
        [
            new("3×3×3×3×3 equals?",243,"3^5=243."),
            new("Noah adds5 four times. Result?",20,"4×5=20."),
            new("Diego starts1 multiplies5 four times. Result?",625,"5^4=625."),
            new("9×9×3 equals?",243,"81×3=243."),
            new("3 to power5 equals?",243,"243."),
            new("7+7+7 equals?",21,"21."),
            new("3 repeated addition7 times equals?",21,"7×3."),
            new("A square side3/5units has area in TWENTY-FIFTHS?",9,"(3/5)²=9/25."),
            new("Square area1/16sq units. Side length in QUARTERS?",1,"Positive side1/4."),
            new("Cube edge2/3unit. Volume in TWENTY-SEVENTHS?",8,"(2/3)^3=8/27."),
            new("Cube volume27/64 cubic units. Edge length in QUARTERS?",3,"Cube edge3/4."),
            new("Shaded rectangle total width10, left part c=4, height3. Shaded area?",18,"3(10-4)=18."),
            new("Movie ticket850cents, sales29886dollars. Number of tickets?",3516,"29886/8.50=3516."),
            new("Garden8½ft by6⅓ft. Perimeter in THIRDS of foot?",89,"2×(17/2+19/3)=89/3ft."),
            new("Fence perimeter89/3ft, panels2/3ft wide. Panel span count in HALVES?",89,"89/3÷2/3=89/2 spans."),
            new("Minimum whole panels needed for fence89/2 panel spans (no joint reuse)?",45,"Round44.5 up to45."),
            new("(1/7)^3 denominator?",343,"7³."),
            new("4 to first power equals?",4,"4^1=4."),
            new("6 cubed equals?",216,"6×6×6=216."),
            new("3 cubed times3 squared equals?",243,"3^5=243.")
        ],
        [L0613ExponentTruth] =
        [
            new("3×3×3×3×3=3^5. 1=yes,0=no.",1,"Original B,C equivalence."),
            new("3×5=3^5. 1=yes,0=no.",0,"15 vs243."),
            new("9×9×3=3^5. 1=yes,0=no.",1,"3²×3²×3=3^5."),
            new("7+7+7=3+3+3+3+3+3+3. 1=yes,0=no.",1,"21 both."),
            new("(1/7)^3=3/7. 1=yes,0=no.",0,"1/343 not3/7."),
            new("4^1=4×1. 1=yes,0=no.",1,"Both4."),
            new("6+6+6=6³. 1=yes,0=no.",0,"18 vs216."),
            new("Noah four additions of5 yields20. 1=yes,0=no.",1,"Repeated addition."),
            new("Diego starts1 and multiplies5 four times, yields625. 1=yes,0=no.",1,"5^4."),
            new("A square of side3/5 has area9/25sq units. 1=yes,0=no.",1,"Square side."),
            new("Cube side2/3 has volume8/27cu units. 1=yes,0=no.",1,"Cubic exponent."),
            new("Cube volume27/64 has positive side3/4. 1=yes,0=no.",1,"(3/4)^3."),
            new("Shaded rectangle area with height3, total width10, left widthc is3(10-c). 1=yes,0=no.",1,"Original A."),
            new("The same shaded area equals30-3c. 1=yes,0=no.",1,"Original E."),
            new("The shaded area equals3(c-10) when 0<c<10. 1=yes,0=no.",0,"Negative."),
            new("At850cents/ticket,29886dollars sales represents3516 tickets. 1=yes,0=no.",1,"Exact division."),
            new("A fence 44.5 panel-widths long requires rounding up to45 whole panels when panels are whole units and no reuse. 1=yes,0=no.",1,"Purchase constraint.")
        ],
        [L0614ExponentEvaluation] =
        [
            new("Eight multiplied by3 squared equals?",72,"8×9=72."),
            new("(8×3) squared equals?",576,"24²=576."),
            new("8×2 cubed equals?",64,"8×8=64."),
            new("7+2 cubed=?",15,"7+8=15."),
            new("9×3 to first power=?",27,"9×3=27."),
            new("20 minus2 to fourth power=?",4,"20-16=4."),
            new("2×6 squared equals?",72,"2×36=72."),
            new("8×(1/2) squared equals?",2,"8×1/4=2."),
            new("1/3×3 cubed equals?",9,"27/3=9."),
            new("(1/5×5) to fifth power=?",1,"1^5=1."),
            new("(4×5) cubed equals?",8000,"20³=8000."),
            new("4×5 cubed equals?",500,"4×125=500."),
            new("Han has10 separate cubes side5in each. Total volume CUBIC INCHES?",1250,"10×5³=1250."),
            new("Han has10 separate cubes side5in each. Total exterior surface area SQ INCHES ignoring shared faces?",1500,"10×6×5²=1500."),
            new("125% of e=30. e?",24,"30/1.25=24."),
            new("35% of f=14. f?",40,"14/.35=40."),
            new("2.4y=13.75. y in FORTY-EIGHTHS?",275,"13.75/2.4=275/48."),
            new("15×23=?",345,"10×23+5×23=230+115."),
            new("5 squared equals?",25,"5²."),
            new("3 cubed equals?",27,"3³."),
            new("1/3 to fourth power denominator?",81,"3^4=81."),
            new("2 to fourth power equals?",16,"16.")
        ],
        [L0614ExponentOrderLogic] =
        [
            new("8×3²=72. 1=yes,0=no.",1,"Original A."),
            new("3²×8=72. 1=yes,0=no.",1,"Original D."),
            new("72 equals8×3². 1=yes,0=no.",1,"Original F."),
            new("(8×3)²=72. 1=yes,0=no.",0,"576."),
            new("8×2³=72. 1=yes,0=no.",0,"64."),
            new("7+2³=15. 1=yes,0=no.",1,"Evaluate8 first."),
            new("20-2⁴=4. 1=yes,0=no.",1,"Subtract16."),
            new("8×(½)²=2. 1=yes,0=no.",1,"8×¼=2."),
            new("1/3×3³=9. 1=yes,0=no.",1,"27÷3."),
            new("(4×5)³=8000. 1=yes,0=no.",1,"Original B,E,G."),
            new("4×5³=8000. 1=yes,0=no.",0,"500."),
            new("Ten separate5-inch cubes total1250cuin. 1=yes,0=no.",1,"10×125."),
            new("Ten separate5-inch cubes each surface150sqin total1500sqin if faces separate. 1=yes,0=no.",1,"10×6×25."),
            new("(1/3)^4=4/3. 1=yes,0=no.",0,"1/81."),
            new("125% of24 is30. 1=yes,0=no.",1,"1.25×24."),
            new("35% of40 is14. 1=yes,0=no.",1,".35×40."),
            new("2.4y=13.75 can be solved by13.75÷2.4. 1=yes,0=no.",1,"Original C,D."),
            new("15×23 can be partitioned to10×23+5×23=345. 1=yes,0=no.",1,"Distribution."),
            new("Parentheses determine whether product is exponent's base. 1=yes,0=no.",1,"Order of operations.")
        ],
        [L0615ExponentVariables] =
        [
            new("2 to power x whenx=3?",8,"2³=8."),
            new("x squared whenx=3?",9,"3²=9."),
            new("1 to power x whenx=3?",1,"1³=1."),
            new("x to first power whenx=3?",3,"3¹=3."),
            new("(1/2)^x whenx=3, answer in EIGHTHS?",1,"1/8."),
            new("2+x cubed whenx=3?",29,"2+27=29."),
            new("x squared whenx=1/2, answer in QUARTERS?",1,"(1/2)²=1/4."),
            new("3x²+y forx=5,y=3?",78,"3×25+3=78."),
            new("10y+x² forx=6,y=4?",76,"40+36=76."),
            new("2 cubed equals?",8,"2³=8."),
            new("3 squared equals?",9,"3²=9."),
            new("1 to power31 equals?",1,"1^31=1."),
            new("31 to first power equals?",31,"31¹=31."),
            new("4 squared equals?",16,"4²=16."),
            new("2 to fourth power equals?",16,"2⁴=16."),
            new("(1/2)^3, answer in EIGHTHS?",1,"1/8."),
            new("(1/3)^2, answer in NINTHS?",1,"1/9."),
            new("7+x²=16, positive x?",3,"x²=9."),
            new("5-x²=1, positive x?",2,"x²=4."),
            new("2×2³=2^x. Find x?",4,"2^4=16."),
            new("3^4/3^x=27. Find x?",1,"81/3=27."),
            new("Adult pass costs1.6×child5dollars. Adult DOLLARS?",8,"1.6×5=8."),
            new("Adult pass if child10dollars?",16,"1.6×10=16."),
            new("Adult pass if child15dollars?",24,"1.6×15=24."),
            new("Jada reads5pages/20min. Pages in60minutes?",15,"Three groups of20min.")
        ],
        [L0615ExponentVariablesLogic] =
        [
            new("Forx=3,2^x=8. 1=yes,0=no.",1,"2³."),
            new("Forx=3,x²=9. 1=yes,0=no.",1,"3²."),
            new("Forx=3,1^x=3. 1=yes,0=no.",0,"One to any positive whole exponent1."),
            new("Forx=3,(1/2)^x=1/8. 1=yes,0=no.",1,"1/2 cubed."),
            new("Forx=3,2+x³=29. 1=yes,0=no.",1,"2+27."),
            new("Forx=1/2,x²=1/4. 1=yes,0=no.",1,"Square."),
            new("Forx=5,y=3,3x²+y=78. 1=yes,0=no.",1,"3×25+3."),
            new("Forx=6,y=4,10y+x²=76. 1=yes,0=no.",1,"40+36."),
            new("2³ is greater than3². 1=yes,0=no.",0,"8<9."),
            new("4² equals2⁴. 1=yes,0=no.",1,"16."),
            new("(1/2)^3 is greater than(1/3)^2. 1=yes,0=no.",1,"1/8>1/9."),
            new("7+x²=16 has positive solution x3. 1=yes,0=no.",1,"3²=9."),
            new("5-x²=1 has positive solution x2. 1=yes,0=no.",1,"x²=4."),
            new("2×2³=2^4. 1=yes,0=no.",1,"16."),
            new("3^4/3¹=27. 1=yes,0=no.",1,"81÷3."),
            new("Adult pass1.6times child price has equation a=1.6w. 1=yes,0=no.",1,"Proportional."),
            new("Reading5pages per20min means15 pages in1hour. 1=yes,0=no.",1,"Three times."),
            new("3x² is equivalent to(3x)² for all x. 1=yes,0=no.",0,"3x² vs9x²."),
            new("The coefficient in3x² is3, multiplying the square ofx. 1=yes,0=no.",1,"Exponent first.")
        ],
        [L0616RatesAndTables] =
        [
            new("Brownie recipe uses1/2cup sugar per batch. Cups for8batches in HALF-CUP units?",8,"8×1/2=4cups."),
            new("Brownies1/2cup sugar per batch. Three batches sugar in HALF-CUP units?",3,"3half-cups."),
            new("Brownies1/2cup sugar per batch. Six batches sugar CUPS?",3,"6/2=3."),
            new("Brownies1/2cup sugar per batch. Two batches sugar CUPS?",1,"2×.5=1."),
            new("Fruit snack90calories per serving. Five servings calories?",450,"90×5=450."),
            new("Fruit snack90calories per serving.720 calories how many servings?",8,"720/90=8."),
            new("Fruit snack90calories per serving. Three servings calories?",270,"270."),
            new("Book sale20% off. Percentage original price paid?",80,"100-20=80%."),
            new("Original book5dollars. Sale price in CENTS?",400,"5×.8=4 dollars."),
            new("Original book9dollars. Sale price in CENTS?",720,"9×.8=7.20."),
            new("Original book10dollars. Sale price in CENTS?",800,"10×.8=8."),
            new("Evaluate(6-x)^3+y withx=4,y=6.",14,"2³+6=14."),
            new("Evaluate2+x³ forx=4?",66,"2+64=66."),
            new("Evaluate2^x-2y forx=4,y=6?",4,"16-12=4."),
            new("(1/2)^x forx=4 in SIXTEENTHS?",1,"1/16."),
            new("1^x+2^x forx=4?",17,"1+16=17."),
            new("2^x/x² forx=4?",1,"16/16=1."),
            new("12.34×0.7 in THOUSANDTHS?",8638,"8.638."),
            new("302.1÷0.5 in TENTHS?",6042,"604.2."),
            new("12.15÷0.02 in HALVES?",1215,"607.5=1215halves."),
            new("1.375÷0.11 in HALVES?",25,"12.5=25 halves."),
            new("Brownie graph point(8,4) shows sugar cups?",4,"Eight batches require4cups.")
        ],
        [L0616RelatedLogic] =
        [
            new("Brownie sugar cups s and batches x follow s=x/2. 1=yes,0=no.",1,"0.5cup per batch."),
            new("Point(8,4) means8 brownie batches require4cups sugar. 1=yes,0=no.",1,"Graph coordinates."),
            new("Point(8,4) means8cups sugar produce4batches. 1=yes,0=no.",0,"Axes reversed."),
            new("At brownie rate,4batches use2cups sugar. 1=yes,0=no.",1,"4×.5."),
            new("Fruit snack calories c and servings n follow c=90n. 1=yes,0=no.",1,"90 each."),
            new("Fruit snack servings n can be written n=c/90. 1=yes,0=no.",1,"Inverse equation."),
            new("Book20% off means sale price80% of original. 1=yes,0=no.",1,"Complement."),
            new("Book sale price s and original p follow s=0.8p. 1=yes,0=no.",1,"20% discount."),
            new("At p=9dollars, book sale price is7.20. 1=yes,0=no.",1,"9×.8."),
            new("The brownie graph for s=x/2 includes origin(0,0). 1=yes,0=no.",1,"No batches, no sugar."),
            new("The equation s=x/2 produces a straight-line proportional relationship. 1=yes,0=no.",1,"Constant ratio."),
            new("The equation s=0.8p describes a proportional discount relation. 1=yes,0=no.",1,"Constant multiplier."),
            new("Forx=4,y=6, (6-x)^3+y=14. 1=yes,0=no.",1,"8+6."),
            new("Forx=4,y=6,2^x-2y=4. 1=yes,0=no.",1,"16-12."),
            new("12.34×0.7=8.638. 1=yes,0=no.",1,"Decimal product."),
            new("1.375÷0.11=12.5. 1=yes,0=no.",1,"Division."),
            new("In a coordinate graph, x and y axis units are necessary for interpreting points meaningfully. 1=yes,0=no.",1,"Quantity meaning.")
        ],
        [L0617SpeedTables] =
        [
            new("Car speed50mph. Distance in2hours MILES?",100,"50×2=100."),
            new("Car speed50mph. Distance in1.5hours MILES?",75,"50×1.5=75."),
            new("Car speed50mph. Hours for50miles?",1,"50/50=1."),
            new("Car speed50mph. Hours for300miles?",6,"300/50=6."),
            new("Ship plot says25miles in1hour. Speed mph?",25,"25/1=25."),
            new("Ship25mph. Hours for100miles?",4,"100/25=4."),
            new("Ship25mph. Hours for250miles?",10,"250/25=10."),
            new("Ship25mph. Distance in8hours MILES?",200,"25×8=200."),
            new("2^x=8, find x?",3,"2³=8."),
            new("2^x=2, x?",1,"2¹=2."),
            new("x²=100, positive x?",10,"10²=100."),
            new("x²=1/100, positive x in TENTHS?",1,"x=1/10."),
            new("x¹=7, x?",7,"x=7."),
            new("2^x×2³=2⁷. x?",4,"x+3=7."),
            new("2^x÷2³=2⁵. x?",8,"x-3=5."),
            new("5x+30x-15x at x=3?",60,"20x=60."),
            new("Forx=1,y=2,z=3, evaluate7x²-z?",4,"7-3=4."),
            new("Forx=1,y=2,z=3, (x+4)³-y?",123,"125-2=123."),
            new("Forx=1,y=2,z=3, y(x+3³)?",56,"2×28=56."),
            new("Forx=1,y=2,z=3, (7-y+z)²?",64,"8²=64."),
            new("Forx=1,y=2,z=3, .241x+x³ in THOUSANDTHS?",1241,"1.241.")
        ],
        [L0617SpeedLogic] =
        [
            new("Car50mph distance d and time t satisfy d=50t. 1=yes,0=no.",1,"Constant speed."),
            new("In d=50t, when time input t is chosen, distance d is dependent. 1=yes,0=no.",1,"Depends on time."),
            new("Car50mph distance d=300 gives time6hours. 1=yes,0=no.",1,"300/50=6."),
            new("Ship graph point(25,1) on axes distance,time means25miles in1hour. 1=yes,0=no.",1,"Original axes."),
            new("Ship speed25mph gives t=d/25. 1=yes,0=no.",1,"Time as function of distance."),
            new("Ship point(250,10) means250hours in10miles. 1=yes,0=no.",0,"Reversed axes."),
            new("A car50mph and ship25mph cover same distance in same positive time. 1=yes,0=no.",0,"Car twice speed."),
            new("For a fixed positive speed, distance and time form a proportional linear relationship. 1=yes,0=no.",1,"d=vt."),
            new("If one switches which variable is independent, equation and axes should be interpreted accordingly. 1=yes,0=no.",1,"Inverse relation t=d/v."),
            new("2^x=8 hasx=3. 1=yes,0=no.",1,"Exponent."),
            new("2^x×2³=2⁷ givesx=4. 1=yes,0=no.",1,"Same base."),
            new("2^x÷2³=2⁵ givesx=8. 1=yes,0=no.",1,"Same base."),
            new("5x+30x-15x=20x for all x. 1=yes,0=no.",1,"Equivalent."),
            new("5(x+6x-3x) equals5x+30x-15x. 1=yes,0=no.",1,"Original A."),
            new("(5+30-15)x equals20x. 1=yes,0=no.",1,"Original B."),
            new("5x(1+6-3) equals20x. 1=yes,0=no.",1,"Original D."),
            new("x(5+30x-15x) is equivalent to20x for all x. 1=yes,0=no.",0,"Extra x multiplication."),
            new("Source constant-speed graph must carry both time and distance units. 1=yes,0=no.",1,"Axis meaning.")
        ],
        [L0618RelationValues] =
        [
            new("Parallelogram area12sqin and base1.5in. Height INCHES?",8,"12/1.5=8."),
            new("Parallelogram area12sqin base3in. Height?",4,"12/3=4."),
            new("Parallelogram area12sqin base4in. Height?",3,"12/4=3."),
            new("Bike trip24miles at3mph. Hours?",8,"24/3=8."),
            new("Bike trip24miles at4mph. Hours?",6,"24/4=6."),
            new("Bike trip24miles at6mph. Hours?",4,"24/6=4."),
            new("V=10s³ when s=2. Volume?",80,"10×8=80."),
            new("V=10s³ when s=4. Volume?",640,"10×64=640."),
            new("V=10s³ when s=3. Volume?",270,"10×27=270."),
            new("Coin doubles daily: day1 number?",2,"2¹=2."),
            new("Coin day2 number?",4,"2²=4."),
            new("Coin day3 number?",8,"2³=8."),
            new("Coin day7 number?",128,"2⁷=128."),
            new("Coin day10 number?",1024,"2^10=1024."),
            new("Coin day12 number?",4096,"2^12=4096."),
            new("Coin day15 number?",32768,"2^15=32768."),
            new("Coin day16 number?",65536,"2^16=65536."),
            new("Coin day28 number?",268435456,"2^28."),
            new("Peaches3.1lb cost772cents. Approx unit cents PER POUND rounded nearest cent?",249,"772/3.1≈249.03cents/lb."),
            new("Lemonade sales980cents, each cup35cents. Cups sold?",28,"980/35=28."),
            new("Cost15cents per cup for28cups. Total cost CENTS?",420,"28×15=420."),
            new("Profit on sales980cents with420cents cost. Profit CENTS?",560,"980-420=560.")
        ],
        [L0618RelationLogic] =
        [
            new("Parallelogram area12sqin gives height h=12/b for base b>0. 1=yes,0=no.",1,"Area base×height."),
            new("With constant area12, doubling base halves height. 1=yes,0=no.",1,"Inverse relationship."),
            new("A24mile ride at constant rate rmph takes t=24/r hours. 1=yes,0=no.",1,"Distance/speed."),
            new("At3mph,24mile ride takes8hours. 1=yes,0=no.",1,"24/3=8."),
            new("At6mph,24mile ride takes4hours. 1=yes,0=no.",1,"24/6=4."),
            new("V=10s³ gives V80 for s2. 1=yes,0=no.",1,"10×8."),
            new("V=10s³ gives V640 for s4. 1=yes,0=no.",1,"10×64."),
            new("Doubling s in V=10s³ multiplies V by8. 1=yes,0=no.",1,"Cubic scaling."),
            new("Starting one magical coin doubles to2 coins day1, number at day d is2^d. 1=yes,0=no.",1,"Source doubling."),
            new("Day15 magical coin value32768dollars is lower than50000dollars. 1=yes,0=no.",1,"32768<50000."),
            new("Day16 coin value65536 exceeds50000. 1=yes,0=no.",1,"Crossing threshold."),
            new("Day28 magical doubling produces268435456 coins. 1=yes,0=no.",1,"2^28."),
            new("The doubling coins grow linearly by adding2 each day. 1=yes,0=no.",0,"Exponential doubling."),
            new("At7.72 dollars for3.1lb peaches, nearest-cent price/lb2.49 dollars. 1=yes,0=no.",1,"772/3.1≈249.03."),
            new("Selling28 lemonades at35c each gives9.80 dollars. 1=yes,0=no.",1,"28×.35=9.8."),
            new("Making28 lemonades at15c each costs4.20 dollars. 1=yes,0=no.",1,"28×.15=4.2."),
            new("At those sales/costs, profit5.60 dollars. 1=yes,0=no.",1,"9.8-4.2."),
            new("An inverse constant-area graph follows the same straight line through origin as a fixed unit price. 1=yes,0=no.",0,"Hyperbolic inverse versus proportional."),
            new("Source graph points require units and explanation when students invent a story. 1=yes,0=no.",1,"Context meaningful.")
        ],
        [L0619BoundedRelationships] =
        [
            new("A walking model has d=4t miles after t hours. Distance at t=3?",12,"4×3=12miles."),
            new("At4mph, time for20miles in HOURS?",5,"20/4=5."),
            new("An ideal snack relation has c=90n calories for n servings. Calories in3 servings?",270,"90×3=270."),
            new("At90calories/serving,630calories gives how many servings?",7,"630/90=7."),
            new("A brownie relation s=x/2 cups for x batches. Sugar at x=8 in CUPS?",4,"8/2=4."),
            new("At half cup per batch,5 batches sugar in HALF-CUP UNITS?",5,"Five halves."),
            new("Book sale price s=.8p dollars, for p=20 dollars sale DOLLARS?",16,".8×20=16."),
            new("At20% discount, original50dollars sale DOLLARS?",40,".8×50=40."),
            new("A rectangular banner has area12sqin and base b=3in. Height h?",4,"12/3=4."),
            new("Banner h=12/b, at b=6in height?",2,"12/6=2."),
            new("A24mile ride at speed r=6mph. Time t hours?",4,"24/6=4."),
            new("A24mile ride at r=4mph. Time hours?",6,"24/4=6."),
            new("Prism relation V=10s³ at s=2. Volume?",80,"10×2³=80."),
            new("Prism relation V=10s³ at s=4. Volume?",640,"10×4³=640."),
            new("A fictional doubling sequence has n=2^d at day d. Count day5?",32,"2^5=32."),
            new("A fictional doubling sequence day d=10. Count?",1024,"2^10=1024."),
            new("A taxi-like abstract fixed price example has cost c=5x for x rides. Cost at x=7 in DOLLARS?",35,"5×7."),
            new("A unit price example has 3 tickets each8dollars, total cost?",24,"3×8."),
            new("An equation y=3x+2 gives y at x=4?",14,"3×4+2."),
            new("A matching table for y=3x+2 has x=0. What y?",2,"Intercept2."),
            new("An equation y=2x² gives y at x=3?",18,"2×9=18."),
            new("A source-aligned arbitrary equation d=25t has distance at t=4 in MILES?",100,"25×4=100.")
        ],
        [L0619InterpretRelationships] =
        [
            new("At constant4mph, graph point(3,12) with hours first and miles second fits d=4t. 1=yes,0=no.",1,"t3,d12."),
            new("At4mph, graph point(3,12) means3miles in12hours when time is the horizontal axis. 1=yes,0=no.",0,"Axes reversed."),
            new("Relation c=90n has a straight line through origin when n is servings and c calories. 1=yes,0=no.",1,"Constant proportional rate."),
            new("Brownie relation s=x/2 at x=8 gives4 cups sugar. 1=yes,0=no.",1,"Half per batch."),
            new("Book sale s=0.8p represents paying80% of original price. 1=yes,0=no.",1,"20% off."),
            new("For banner area12, b×h=12 and h=12/b for positive base. 1=yes,0=no.",1,"Inverse relation."),
            new("At constant area, doubling base means doubling height. 1=yes,0=no.",0,"Height halves."),
            new("Travel time t=24/r for fixed24mile trip decreases as positive speed increases. 1=yes,0=no.",1,"Inverse relation."),
            new("V=10s³ yields eight times the volume when positive s doubles. 1=yes,0=no.",1,"Cubic scaling."),
            new("Doubling n=2^d grows by adding2 coins every day. 1=yes,0=no.",0,"Geometric growth."),
            new("The relation y=3x+2 passes through(0,0). 1=yes,0=no.",0,"Intercept2."),
            new("The relation y=3x+2 at x=4 yields14. 1=yes,0=no.",1,"Substitute."),
            new("All real-world graphs follow straight lines through the origin. 1=yes,0=no.",0,"Inverse, nonlinear and intercept models."),
            new("If an equation and table match, each table row substituted into the equation gives true equality. 1=yes,0=no.",1,"Test all rows."),
            new("Units and axis labels are required to interpret a graphed point as a real-world statement. 1=yes,0=no.",1,"Context."),
            new("The original capstone contains nine equation-table relationships requiring teacher-verified matching. 1=yes,0=no.",1,"Original project statement."),
            new("One first-party fixed numeric example by itself proves the original nine source diagrams have been checked. 1=yes,0=no.",0,"Teacher original task check needed."),
            new("A graph produced from a table must use consistent scale on each axis. 1=yes,0=no.",1,"Correct plotting."),
            new("Equations, graphs, tables and verbal descriptions may represent one underlying relationship. 1=yes,0=no.",1,"Multiple representations."),
            new("A student can change their graph without revising their equation if the represented quantities no longer agree. 1=yes,0=no.",0,"All representations must remain consistent.")
        ],
        [L0701SignedValues] =
        [
            new("Temperature8degrees drops10degrees. New temperature?",-2,"8-10=-2."),
            new("Diver at-25ft below sea rises15ft. New elevation feet?",-10,"-25+15=-10."),
            new("Whale at sea surface elevation feet?",0,"Surface reference zero."),
            new("Whale swims300ft down. Elevation?",-300,"0-300=-300."),
            new("Whale swims150ft farther down from-300. Elevation?",-450,"-300-150=-450."),
            new("Difference between temperatures-11 and-15 in DEGREES?",4,"-11-(-15)=4."),
            new("A submarine at-8ft descends2ft. Elevation?",-10,"-8-2=-10."),
            new("A location10ft below sea level. Signed elevation?",-10,"Negative means below reference."),
            new("A location8ft above sea level. Signed elevation?",8,"Positive above zero."),
            new("Temperature3degrees below zero. Signed degrees?",-3,"-3."),
            new("Temperature5degrees above zero. Signed degrees?",5,"+5."),
            new("Temperature6degrees above zero. Signed degrees?",6,"+6."),
            new("Temperature2¾degrees below zero, answer in QUARTER-DEGREE units?",-11,"-11/4 degrees."),
            new("2.1÷1.5 in TENTHS?",14,"=1.4."),
            new("Andre drank15oz, which is3/5 bottle. Full bottle OUNCES?",25,"15÷3/5=25."),
            new("Bottle15oz, Jada drinks8.5oz. Left in HALF-OUNCE UNITS?",13,"6.5oz=13halves."),
            new("Second bottle16oz is8/5 first bottle. First bottle capacity?",10,"16÷8/5=10."),
            new("Rectangle area24 sq units, one side2¾. Other side in ELEVENTHS?",96,"24÷11/4=96/11."),
            new("Whale elevation after first300ft descent and150ft more, difference from surface feet?",450,"450 below, difference magnitude450."),
            new("From diver-25ft to-10ft ascent distance?",15,"15ft upward.")
        ],
        [L0701SignLogic] =
        [
            new("Temperature-11degrees is warmer than-15degrees. 1=yes,0=no.",1,"-11>-15."),
            new("Elevation-10ft is closer to sea surface than-8ft. 1=yes,0=no.",0,"Distance10>8."),
            new("Zero elevation marks sea level in the example. 1=yes,0=no.",1,"Reference."),
            new("Whale at surface has elevation0ft. 1=yes,0=no.",1,"Sea level."),
            new("Whale300ft below sea level has elevation-300ft. 1=yes,0=no.",1,"Negative below."),
            new("Further descent150ft from-300 gives-150ft. 1=yes,0=no.",0,"It is-450."),
            new("A temperature drop10 from8 gives-2. 1=yes,0=no.",1,"8-10=-2."),
            new("A diver rising15ft from-25ft reaches-10ft. 1=yes,0=no.",1,"-25+15=-10."),
            new("On a vertical elevation line, numbers above0 are positive. 1=yes,0=no.",1,"Standard orientation."),
            new("-15 is greater than-11. 1=yes,0=no.",0,"-15 lies lower/left."),
            new("Below-zero temperatures can be written with a minus sign. 1=yes,0=no.",1,"Signed notation."),
            new("Negative elevation always means impossibility. 1=yes,0=no.",0,"Under sea level exists."),
            new("The convention zero degrees C can represent freezing point of water. 1=yes,0=no.",1,"Temperature reference."),
            new("Zero is neither positive nor negative. 1=yes,0=no.",1,"Signed-number definitions."),
            new("The number line can extend to values below0. 1=yes,0=no.",1,"Negative numbers."),
            new("Elevation-10ft is farther from sea level than-8ft. 1=yes,0=no.",1,"Distances10 versus8.")
        ],
        [L0702OppositeNumbers] =
        [
            new("Opposite of-5?",5,"+5."),
            new("Opposite of28?",-28,"-28."),
            new("Opposite of-10.4 in TENTHS?",104,"10.4 positive."),
            new("Opposite of0.875 in THOUSANDTHS?",-875,"-0.875."),
            new("Opposite of0?",0,"Zero."),
            new("Opposite of-8003?",8003,"+8003."),
            new("Opposite of-2?",2,"+2."),
            new("Opposite of0.5 in HALVES?",-1,"-1/2."),
            new("Opposite of-1.5 in HALF-UNITS?",3,"+1.5."),
            new("Negative four thirds in THIRDS?",-4,"-4/3."),
            new("5degrees above0. Temperature?",5,"+5."),
            new("3degrees below0. Temperature?",-3,"-3."),
            new("6degrees above0. Temperature?",6,"+6."),
            new("2¾degrees below0 in QUARTER-DEGREE units?",-11,"-11/4."),
            new("8x=2/3. x in TWELFTHS?",1,"x=1/12."),
            new("1½=2x. x in QUARTERS?",3,"x=3/4."),
            new("5x=2/7. x in THIRTY-FIFTHS?",2,"2/35."),
            new("(1/4)x=5. x?",20,"5×4."),
            new("(2/3)x=1/5. x in TENTHS?",3,"3/10."),
            new("2x=3. x in HALVES?",3,"1.5."),
            new("5y=3. y in FIFTHS?",3,"3/5."),
            new("0.3z=.009. z in THOUSANDTHS?",30,".03."),
            new("6in=15.24cm. One foot in HUNDREDTHS CM?",3048,"30.48cm."),
            new("6in=15.24cm. One yard in HUNDREDTHS CM?",9144,"91.44cm.")
        ],
        [L0702OppositeLogic] =
        [
            new("-5 and+5 are opposites. 1=yes,0=no.",1,"Equal distance5 opposite sides."),
            new("-10.4 and+10.4 are opposites. 1=yes,0=no.",1,"Same magnitude."),
            new("0 is its own opposite. 1=yes,0=no.",1,"-0=0."),
            new("28 and-28 are the same number. 1=yes,0=no.",0,"Opposites distinct nonzero."),
            new("The opposite of-8003 is8003. 1=yes,0=no.",1,"Sign reversal."),
            new("Negative1.5 and positive3/2 are opposite numbers. 1=yes,0=no.",1,"Equal magnitude."),
            new("Negative3/2 and negative1.5 are two different locations on number line. 1=yes,0=no.",0,"Same value."),
            new("Opposite of0.5 is-0.5. 1=yes,0=no.",1,"Reflection around zero."),
            new("The numbers -1.5 and-4/3 are equal. 1=yes,0=no.",0,"-1.5 vs-1.333."),
            new("The opposite of a positive number is negative. 1=yes,0=no.",1,"Except zero not positive."),
            new("A folded number line at0 pairs numbers equidistant on each side. 1=yes,0=no.",1,"Reflection."),
            new("Temperatures-2¾ and-3: -2¾ is warmer. 1=yes,0=no.",1,"-2.75>-3."),
            new("If8x=2/3 then x=1/12. 1=yes,0=no.",1,"Division8."),
            new("If(1/4)x=5 thenx=20. 1=yes,0=no.",1,"Multiply4."),
            new("A yard is36in and 91.44cm when one inch is2.54cm. 1=yes,0=no.",1,"36×2.54."),
            new("Zero occupies the midpoint between any two numbers that are opposites. 1=yes,0=no.",1,"Symmetry."),
            new("A signed number's opposite always has a different absolute value. 1=yes,0=no.",0,"Same distance from0.")
        ],
        [L0703Comparisons] =
        [
            new("Compare -5 and2. Enter1 if first larger,0 equal,2 if smaller.",2,"-5<2."),
            new("Compare3 and-8. Enter1 if first larger,0 equal,2 if smaller.",1,"3>-8."),
            new("Compare-12 and-15. Enter1 if first larger,0 equal,2 if smaller.",1,"-12>-15."),
            new("Compare-12.5 and-12. Enter1 if first larger,0 equal,2 if smaller.",2,"-12.5<-12."),
            new("Compare-8.7 and-8.4. Enter1 if first larger,0 equal,2 if smaller.",2,"-8.7<-8.4."),
            new("Compare0.4 and-1.5. Enter1 if first larger,0 equal,2 if smaller.",1,"Positive>negative."),
            new("Compare-1.7 and-1.1. Enter1 if first larger,0 equal,2 if smaller.",2,"-1.7<-1.1."),
            new("Compare-1.5 and-1.7. Enter1 if first larger,0 equal,2 if smaller.",1,"-1.5>-1.7."),
            new("Lowest elevation California in feet?",-282,"Below sea."),
            new("Lowest elevation Colorado feet?",3350,"Positive."),
            new("Lowest elevation Louisiana feet?",-8,"Negative."),
            new("Lowest elevation New Mexico feet?",2842,"Positive."),
            new("Lowest elevation Wyoming feet?",3099,"Positive."),
            new("Track400meters per lap. 2laps meters?",800,"2×400."),
            new("Track400meters per lap. 5laps meters?",2000,"5×400."),
            new("Track400meters per lap. 14laps meters?",5600,"14×400."),
            new("7600meters at400m per lap. Laps?",19,"7600÷400."),
            new("Stadium16000 capacity,13920 attendees. Percent filled?",87,"13920/16000=.87."),
            new("Stadium16000 capacity,13920 attendees. Percent NOT filled?",13,"100-87."),
            new("Number of empty seats at capacity16000 with13920 attendees?",2080,"16000-13920."),
            new("Difference from elevation-282ft to-8ft in FEET?",274,"-8-(-282)=274.")
        ],
        [L0703SignedLogic] =
        [
            new("-5>2 is true. 1=yes,0=no.",0,"Negative less than positive."),
            new("3>-8 is true. 1=yes,0=no.",1,"Positive greater."),
            new("-12>-15 is true. 1=yes,0=no.",1,"-12 to right."),
            new("-12.5>-12 is true. 1=yes,0=no.",0,"-12.5 left."),
            new("-8.7<-8.4 is true. 1=yes,0=no.",1,"Original statement."),
            new("-8.7 is left of-8.4. 1=yes,0=no.",1,"Original choice B."),
            new("-8.4 is greater than-8.7. 1=yes,0=no.",1,"Original F."),
            new("-8.7 is farther right than-8.4. 1=yes,0=no.",0,"Actually left."),
            new("Zero is greater than every negative number. 1=yes,0=no.",1,"Number-line order."),
            new("A number farther from zero is always greater, including negatives. 1=yes,0=no.",0,"Negative reversal."),
            new("California elevation-282ft is lower than Louisiana-8ft. 1=yes,0=no.",1,"-282<-8."),
            new("Colorado3350ft is higher than Wyoming3099ft. 1=yes,0=no.",1,"3350>3099."),
            new("Wyoming3099ft is higher than New Mexico2842ft. 1=yes,0=no.",1,"3099>2842."),
            new("-1.5 is greater than-1.7. 1=yes,0=no.",1,"-1.5 right."),
            new("At400m/lap,7600m equals19laps. 1=yes,0=no.",1,"Division."),
            new("Stadium13920 of16000 seats filled is87%. 1=yes,0=no.",1,"13920/16000."),
            new("On a number line, if a is right of b then a>b. 1=yes,0=no.",1,"Definition order.")
        ],
        [L0704RationalOrder] =
        [
            new("Compare1.3 to-5. Enter1 if1.3 greater,0 otherwise.",1,"Positive>negative."),
            new("Compare-6 to-5. Enter1 if-6 greater,0 otherwise.",0,"-6<-5."),
            new("Compare-12 to-5. Enter1 if-12 greater,0 otherwise.",0,"-12<-5."),
            new("Compare1/7 to-5. Enter1 if fraction greater,0 otherwise.",1,"Positive>negative."),
            new("Compare-1 to-5. Enter1 if-1 greater,0 otherwise.",1,"-1 right."),
            new("Compare-4 to-5. Enter1 if-4 greater,0 otherwise.",1,"-4>-5."),
            new("Of ordered list -1.5,-1,-.5,0,.5,1, least in HALF-UNITS?",-3,"-1.5= -3 halves."),
            new("Of ordered list -1.5,-1,-.5,0,.5,1, greatest?",1,"+1."),
            new("Argon boiling point-185.8C in TENTHS?",-1858,"-185.8."),
            new("Chlorine boiling point-34C?",-34,"-34."),
            new("Fluorine boiling point-188.1C in TENTHS?",-1881,"-188.1."),
            new("Hydrogen boiling point-252.87C in HUNDREDTHS?",-25287,"-252.87."),
            new("Krypton boiling point-153.2C in TENTHS?",-1532,"-153.2."),
            new("99+54=?",153,"100+54-1."),
            new("244-99=?",145,"244-100+1."),
            new("99×6=?",594,"600-6."),
            new("99×15=?",1485,"1500-15."),
            new("1/2 divided by2 in QUARTERS?",1,"1/4."),
            new("2÷2=?",1,"1."),
            new("1/2 divided by1/2=?",1,"1."),
            new("38/79 divided by38/79=?",1,"1."),
            new("Opposite of0?",0,"0."),
            new("Greatest boiling point of selected elements, in DEGREES C?",-34,"Chlorine-34 C."),
            new("Compare-188.1 to-185.8. Enter1 if first greater,0 otherwise.",0,"Fluorine colder.")
        ],
        [L0704OrderLogic] =
        [
            new("1.3>-5 is true. 1=yes,0=no.",1,"A."),
            new("-6>-5 is true. 1=yes,0=no.",0,"B false."),
            new("-12>-5 is true. 1=yes,0=no.",0,"C false."),
            new("1/7>-5 is true. 1=yes,0=no.",1,"D."),
            new("-1>-5 is true. 1=yes,0=no.",1,"E."),
            new("-4>-5 is true. 1=yes,0=no.",1,"F."),
            new("-1.5<-1<-0.5<0<0.5<1 is correctly ordered. 1=yes,0=no.",1,"From left to right."),
            new("Hydrogen boiling-252.87C is lower than fluorine-188.1C. 1=yes,0=no.",1,"More negative."),
            new("Argon-185.8C is higher than krypton-153.2C. 1=yes,0=no.",0,"-185.8<-153.2."),
            new("Chlorine-34C is highest of five listed elements. 1=yes,0=no.",1,"Least negative."),
            new("Zero is its own opposite. 1=yes,0=no.",1,"-0=0."),
            new("Every negative number is smaller than every positive number. 1=yes,0=no.",1,"Number-line order."),
            new("A negative number with larger absolute value is always greater than a negative with smaller absolute value. 1=yes,0=no.",0,"Reverse."),
            new("99×6=594. 1=yes,0=no.",1,"(100-1)6."),
            new("99×15=1485. 1=yes,0=no.",1,"(100-1)15."),
            new("1/2÷2=1/4. 1=yes,0=no.",1,"Half split2."),
            new("A positive quantity doubles in pounds, so its equivalent kilograms double, assuming fixed unit conversion. 1=yes,0=no.",1,"Constant scaling.")
        ],
        [L0704Ordering] =
        [
            new("Compare -6 and -5. Enter1 for greater,0 for equal,2 for less.",2,"-6<-5."),
            new("Compare -12 and -5. Enter1 for greater,0 for equal,2 for less.",2,"-12<-5."),
            new("Compare 1.3 and -5. Enter1 for greater,0 for equal,2 for less.",1,"1.3>-5."),
            new("Compare 1/7 and -5. Enter1 for greater,0 for equal,2 for less.",1,"A positive fraction is greater than a negative number."),
            new("Compare -1 and -5. Enter1 for greater,0 for equal,2 for less.",1,"-1 lies to the right of -5."),
            new("Compare -4 and -5. Enter1 for greater,0 for equal,2 for less.",1,"-4>-5."),
            new("Order -1.5,-1,-0.5,0,0.5,1. Which is LEAST? Enter numerical answer in HALVES.",-3,"-1.5=-3/2 is least."),
            new("Order -1.5,-1,-0.5,0,0.5,1. Which negative is SECOND least? Enter whole number.",-1,"-1 comes after -1.5."),
            new("Order -1.5,-1,-0.5,0,0.5,1. Which is THIRD least? Enter HALF UNITS.",-1,"-0.5=-1 half unit."),
            new("Order -1.5,-1,-0.5,0,0.5,1. Which is FOURTH least?",0,"0 is after all three negatives."),
            new("Which element has the lowest listed boiling point in °C? Enter1=Argon -185.8,2=Chlorine -34,3=Fluorine -188.1,4=Hydrogen -252.87,5=Krypton -153.2.",4,"Hydrogen has the smallest temperature -252.87."),
            new("Which element has the HIGHEST listed boiling point? Enter1=Argon,2=Chlorine,3=Fluorine,4=Hydrogen,5=Krypton.",2,"-34°C for chlorine is largest."),
            new("Of fluorine -188.1 and argon -185.8, which is LOWER? Enter1 fluorine,2 argon.",1,"-188.1<-185.8."),
            new("Of krypton -153.2 and argon -185.8, which is HIGHER? Enter1 krypton,2 argon.",1,"-153.2>-185.8."),
            new("Among -34,-153.2,-185.8,-188.1,-252.87, which is SECOND smallest, in TENTHS OF °C?",-1881,"Fluorine -188.1 is second smallest."),
            new("What is the opposite of zero?",0,"Zero is its own opposite."),
            new("Compare -1/2 and 0. Enter1 for greater,0 equal,2 less.",2,"Negative half less than zero."),
            new("Compare 1/2 and 0. Enter1 for greater,0 equal,2 less.",1,"Positive half greater than zero."),
            new("Compare -1.5 and -0.5. Enter1 for greater,0 equal,2 less.",2,"-1.5<-0.5."),
            new("Compare 0 and -1. Enter1 for greater,0 equal,2 less.",1,"0>-1.")
        ],
        [L0704SourceChecks] =
        [
            new("Of options1.3,-6,-12,1/7,-1,-4, 1.3 is greater than -5. Enter1 yes,0 no.",1,"1.3>-5."),
            new("Of the options, -6 is greater than -5. Enter1 yes,0 no.",0,"-6<-5."),
            new("Of the options, -12 is greater than -5. Enter1 yes,0 no.",0,"-12<-5."),
            new("Of the options, 1/7 is greater than -5. Enter1 yes,0 no.",1,"Positive fractions exceed negatives."),
            new("Of the options, -1 is greater than -5. Enter1 yes,0 no.",1,"-1>-5."),
            new("Of the options, -4 is greater than -5. Enter1 yes,0 no.",1,"-4>-5."),
            new("Zero is its own additive opposite because0+0=0. Enter1 yes,0 no.",1,"Opposite of a is -a; -0=0."),
            new("All negative rational numbers are less than all positive rational numbers. Enter1 yes,0 no.",1,"Negative values lie left of0, positive right."),
            new("On a horizontal number line -12 lies LEFT of -5. Enter1 yes,0 no.",1,"-12<-5."),
            new("Hydrogen -252.87°C has lower listed boiling point than fluorine -188.1°C. Enter1 yes,0 no.",1,"-252.87<-188.1."),
            new("99 plus54 equals?",153,"100+54-1=153."),
            new("244 minus99 equals?",145,"244-100+1=145."),
            new("99 times6 equals?",594,"(100-1)×6=594."),
            new("99 times15 equals?",1485,"1500-15=1485."),
            new("One half divided by2: numerator in FOURTHS?",1,"1/2÷2=1/4."),
            new("Two divided by2 equals?",1,"2/2=1."),
            new("One half divided by one half equals?",1,"Equal positive operands have quotient1."),
            new("38/79 divided by38/79 equals?",1,"A nonzero number divided by itself equals1."),
            new("A baby's weight doubling in pounds implies its kilograms measurement also doubles using fixed conversion. Enter1 yes,0 no.",1,"Unit conversion multiplies by a fixed constant."),
            new("If5°C is greater than -5°C, its opposite -5°C is greater than5°C. Enter1 yes,0 no.",0,"Order reverses under negation.")
        ],
        [L0705SignedChanges] =
        [
            new("Tuesday high is4degrees less than Monday. Signed change?",-4,"A decrease."),
            new("Wednesday high is3.5degrees less than Tuesday. Change in HALF-DEGREE UNITS?",-7,"-3.5."),
            new("Thursday high is6.5degrees more than Wednesday. Change in HALF-DEGREE UNITS?",13,"+6.5."),
            new("Friday high is2degrees less than Thursday. Signed change?",-2,"Decreased2."),
            new("Puppy gains5lb. Signed weight change?",5,"Positive gain."),
            new("Aquarium leaks2gallons. Signed volume change?",-2,"Loss."),
            new("Andre receives10dollars. Signed account change?",10,"Income."),
            new("Kiran gives gift10dollars. Signed money change?",-10,"Expense."),
            new("Climber descends550ft. Signed elevation change?",-550,"Downward."),
            new("Bank has50dollars and a20dollar charge. Balance?",30,"50-20."),
            new("Bank has50dollars and80dollar charge, allowing overdraft. Balance?",-30,"50-80=-30."),
            new("Two points4 units from0. Positive point?",4,"+4."),
            new("Two points4 units from0. Negative point?",-4,"-4."),
            new("Distance between-5/2 and+5/2 units?",5,"Separation5."),
            new("2 cubed times3?",24,"8×3."),
            new("4 squared divided by2?",8,"16/2."),
            new("3 to power1?",3,"3."),
            new("6 squared divided by4?",9,"36/4."),
            new("2 cubed minus2?",6,"8-2."),
            new("10 squared plus5 squared?",125,"100+25."),
            new("From Monday7°, Tuesday4° lower, Tuesday temperature?",3,"7-4."),
            new("Tuesday3°, Wednesday3.5° lower, Wednesday in HALF-DEGREES?",-1,"-0.5, one negative half.")
        ],
        [L0705ChangeLogic] =
        [
            new("Tuesday4 degrees lower than Monday is signed change-4. 1=yes,0=no.",1,"Decrease."),
            new("Wednesday3.5 degrees lower is change-3.5. 1=yes,0=no.",1,"Decrease."),
            new("Thursday6.5 degrees higher is change+6.5. 1=yes,0=no.",1,"Increase."),
            new("A puppy gains5lb, represented by+5. 1=yes,0=no.",1,"Gain."),
            new("Aquarium leaks2 gallons, represented by-2. 1=yes,0=no.",1,"Loss."),
            new("Receiving10dollars can be represented by+10. 1=yes,0=no.",1,"Receipt."),
            new("Giving away10dollars can be represented by-10. 1=yes,0=no.",1,"Expense."),
            new("Climber descends550ft, represented by-550. 1=yes,0=no.",1,"Down."),
            new("A balance50 dollars minus80 can be-30 if overdraft is allowed. 1=yes,0=no.",1,"Debt."),
            new("Negative change always means a negative final quantity. 1=yes,0=no.",0,"May remain positive."),
            new("Positive change always yields positive final quantity. 1=yes,0=no.",0,"Starting negative may remain negative."),
            new("-4 and4 are each distance4 from0. 1=yes,0=no.",1,"Opposites."),
            new("Numbers-5/2 and5/2 are distance5 apart. 1=yes,0=no.",1,"5/2-(-5/2)=5."),
            new("2³×3=24. 1=yes,0=no.",1,"8×3."),
            new("10²+5²=125. 1=yes,0=no.",1,"100+25."),
            new("A signed number for expense or stock loss depends on a declared sign convention. 1=yes,0=no.",1,"Define perspective."),
            new("A negative inventory change need not mean negative current stock. 1=yes,0=no.",1,"Quantity difference vs total.")
        ],
        [L0706AbsoluteValues] =
        [
            new("Absolute value of-7?",7,"Distance7."),
            new("Absolute value of7?",7,"Distance7."),
            new("Absolute value of0?",0,"0."),
            new("Absolute value of-1.5 in HALF-UNITS?",3,"1.5=3 halves."),
            new("Absolute value of3/2 in HALF-UNITS?",3,"3/2."),
            new("Absolute value of-2.7 in TENTHS?",27,"2.7."),
            new("Absolute value of-1?",1,"1."),
            new("Absolute value of-6?",6,"6."),
            new("Absolute value of6?",6,"6."),
            new("Travel target325miles,26 completed. Percent?",8,"26/325=.08."),
            new("Trip target325miles,72% completed. Miles?",234,".72×325=234."),
            new("Trip target325miles,377 traveled. Percent?",116,"377/325=1.16."),
            new("Elena donates4.44 dollars from37 earned. Percent?",12,"4.44/37=.12."),
            new("Elena donates1.80 from15 earned. Percent?",12,"1.8/15=.12."),
            new("Elena donates3.12 from26 earned. Percent?",12,"3.12/26=.12."),
            new("Elena donates3.60 from30 earned. Percent?",12,"3.6/30=.12."),
            new("Three to fourth power relative to three cubed: ratio?",3,"3^(4-3)=3."),
            new("Five cubed relative to five squared: ratio?",5,"5."),
            new("Seven to tenth power relative to seventh to eighth: ratio?",49,"7²=49."),
            new("Seventeen to sixth power relative to seventeenth to fourth: ratio?",289,"17²."),
            new("Five to tenth power relative five to fourth: ratio?",15625,"5⁶=15625."),
            new("A donation m=50 dollars at12% in DOLLARS?",6,".12×50=6."),
            new("Absolute value of-100?",100,"Distance100.")
        ],
        [L0706AbsoluteLogic] =
        [
            new("Numbers with absolute value3/2 include -3/2 and+3/2. 1=yes,0=no.",1,"Equal distances."),
            new("Only temperature-6C is6 degrees from0, not+6C. 1=yes,0=no.",0,"Both±6."),
            new("Temperature-6C is6 degrees from0. 1=yes,0=no.",1,"Source B."),
            new("Temperature+6C is6degrees from0. 1=yes,0=no.",1,"Source D."),
            new("|-2.7|=2.7. 1=yes,0=no.",1,"Magnitude."),
            new("|-1|=1. 1=yes,0=no.",1,"Magnitude."),
            new("Order0,1,1.3,2,2.7 for source transformed list is ascending. 1=yes,0=no.",1,"Absolute first."),
            new("Absolute value of any real number is nonnegative. 1=yes,0=no.",1,"Distance."),
            new("|-7|<|3|. 1=yes,0=no.",0,"7>3."),
            new("|-7|=|7|. 1=yes,0=no.",1,"Opposites."),
            new("26 miles of325 equals8% of total. 1=yes,0=no.",1,"26/325=.08."),
            new("377 miles of325 is116%, above100%. 1=yes,0=no.",1,"1.16."),
            new("Elena's donations4.44/37 and1.80/15 represent same12%. 1=yes,0=no.",1,"Both .12."),
            new("Donation d from earned m at fixed12% can be written d=.12m. 1=yes,0=no.",1,"Dependent donation."),
            new("3^4 is three times3^3. 1=yes,0=no.",1,"Power ratio3."),
            new("7^10/7^8=49. 1=yes,0=no.",1,"7²."),
            new("A larger absolute value necessarily means a greater signed number. 1=yes,0=no.",0,"-100 has magnitude100 but <0."),
            new("Absolute value can represent depth below sea surface without a sign indicating direction. 1=yes,0=no.",1,"Magnitude only.")
        ],
        [L0707OrderMagnitude] =
        [
            new("Absolute value-7feet?",7,"7 feet from0."),
            new("Absolute value-63?",63,"63."),
            new("Compare-32 to15. Enter1 if first greater,0 equal,2 smaller.",2,"-32<15."),
            new("Compare|-32| to|15|. Enter1 if first greater,0 equal,2 smaller.",1,"32>15."),
            new("Compare5 to-5. Enter1 if first greater,0 equal,2 smaller.",1,"5>-5."),
            new("Compare|5| to|-5|. Enter1 if first greater,0 equal,2 smaller.",0,"Equal5."),
            new("Compare2 to-17. Enter1 if first greater,0 equal,2 smaller.",1,"2>-17."),
            new("Compare2 to|-17|. Enter1 if first greater,0 equal,2 smaller.",2,"2<17."),
            new("Compare|-27| to|-45|. Enter1 if first greater,0 equal,2 smaller.",2,"27<45."),
            new("Compare|-27| to-45. Enter1 if first greater,0 equal,2 smaller.",1,"27>-45."),
            new("Mai receives25 dollars. Signed change?",25,"Credit."),
            new("Mai earns14 dollars babysitting. Signed change?",14,"Credit."),
            new("Mai spends10 dollars concert. Signed change?",-10,"Expense."),
            new("Mai donates3 dollars charity. Signed change?",-3,"Expense."),
            new("Mai earns2 dollars interest. Signed change?",2,"Credit."),
            new("Death Valley record temperature°F?",-45,"Source historical record."),
            new("Danbury record temperature°F?",-37,"Source historical record."),
            new("Monticello FL record temperature°F?",-2,"Source historical record."),
            new("East Saint Louis record temperature°F?",-36,"Source historical record."),
            new("Greenville GA record temperature°F?",-17,"Source historical record."),
            new("How much colder GA-17 than FL-2 in DEGREES?",15,"-2-(-17)=15."),
            new("0.024÷0.015 in FIFTHS?",8,"1.6=8/5."),
            new("0.24÷0.015=?",16,"16."),
            new("0.024÷0.15 in HUNDREDTHS?",16,"0.16."),
            new("24÷15 in FIFTHS?",8,"1.6.")
        ],
        [L0707OrderLogic] =
        [
            new("|-7|=7. 1=yes,0=no.",1,"A corresponds4."),
            new("|-63|>4. 1=yes,0=no.",1,"B corresponds1."),
            new("4>-4. 1=yes,0=no.",1,"C corresponds5."),
            new("|4|=|-4|. 1=yes,0=no.",1,"D corresponds6."),
            new("-63<4. 1=yes,0=no.",1,"E corresponds2."),
            new("|-63|>|4|. 1=yes,0=no.",1,"F corresponds3."),
            new("-32>15. 1=yes,0=no.",0,"Negative less."),
            new("|-32|>|15|. 1=yes,0=no.",1,"32>15."),
            new("5>-5. 1=yes,0=no.",1,"Positive greater."),
            new("|5|=|-5|. 1=yes,0=no.",1,"Both5."),
            new("2>|-17|. 1=yes,0=no.",0,"2<17."),
            new("|-27|<|-45|. 1=yes,0=no.",1,"27<45."),
            new("|-27|>-45. 1=yes,0=no.",1,"27>-45."),
            new("Death Valley-45F is lowest of five cited records. 1=yes,0=no.",1,"-45 minimum."),
            new("Florida-2F is colder than Georgia-17F. 1=yes,0=no.",0,"-17 colder."),
            new("Connecticut-37F is colder than Illinois-36F. 1=yes,0=no.",1,"-37 colder."),
            new("0.024÷0.015=1.6. 1=yes,0=no.",1,"24/15."),
            new("The minus symbol on -a may produce positive number if a itself is negative. 1=yes,0=no.",1,"Opposite."),
            new("An absolute value inequality compares magnitude, not necessarily signed order. 1=yes,0=no.",1,"Distinguish.")
        ],
        [L0708InequalityExamples] =
        [
            new("Book sale price must be less than5 dollars. Is499cents permitted? Enter1 yes,0 no.",1,"4.99<5."),
            new("Book sale price must be less than5 dollars. Is500cents permitted? Enter1 yes,0 no.",0,"Strict inequality."),
            new("Book sale price must be less than5 dollars. Is450cents permitted? Enter1 yes,0 no.",1,"4.50<5."),
            new("If book prices are multiples of one cent below5 dollars, maximum positive price in CENTS?",499,"4.99."),
            new("Kiran homework starts before7pm ends after8pm. Minimum elapsed exceeds how many HOURS?",1,"Strictly >1."),
            new("Rectangular prism dimensions4,d,d. Volume for d=1?",4,"4×1²=4."),
            new("Prism dimensions4,d,d. Volume for d=2?",16,"4×4=16."),
            new("Prism dimensions4,d,d. Volume for d=1/2?",1,"4×1/4=1."),
            new("A number -15 lies left of-12. Enter1 yes,0 no.",1,"-15<-12."),
            new("Absolute value-15?",15,"Magnitude15."),
            new("Absolute value-12?",12,"Magnitude12."),
            new("Compare -15 and-12. Enter1 if first greater,0 equal,2 smaller.",2,"-15<-12."),
            new("2a+7a when a=3?",27,"9a=27."),
            new("5z-10 when z=4?",10,"20-10=10."),
            new("c-2cd with c=3,d=2?",-9,"3-12=-9."),
            new("r+r+r+r with r=7?",28,"4r=28."),
            new("2x-1/2 with x=3 in HALF-UNITS?",11,"6-.5=5.5=11 halves."),
            new("A book costs4.98 dollars, maximum budget5. Difference in CENTS?",2,"500-498=2."),
            new("If strict x<5, is x=5 a solution? Enter1 yes,0 no.",0,"Boundary excluded."),
            new("If x>9, is x=10 a solution? Enter1 yes,0 no.",1,"10>9."),
            new("If x<9, is x=8 a solution? Enter1 yes,0 no.",1,"8<9.")
        ],
        [L0708InequalityLogic] =
        [
            new("A book sale where every price is less than5 dollars uses p<5. 1=yes,0=no.",1,"Strict upper bound."),
            new("At a p<5 sale, price5dollars is permitted. 1=yes,0=no.",0,"Excluded."),
            new("If priced to nearest cent,4.99 is most expensive strictly under5. 1=yes,0=no.",1,"Discrete cent step."),
            new("If prices are arbitrary real dollars strictly below5, there is a single largest real price. 1=yes,0=no.",0,"No maximum."),
            new("Kiran begins homework before7pm and ends after8pm, so h>1 hour is definitely true. 1=yes,0=no.",1,"Elapsed strictly greater than1."),
            new("That homework duration must be greater than2hours. 1=yes,0=no.",0,"Could be1.5."),
            new("That homework duration can be less than1hour. 1=yes,0=no.",0,"Impossible."),
            new("That homework duration may be less than2hours. 1=yes,0=no.",1,"Possible, not guaranteed."),
            new("A prism4×d×d has V=4d². 1=yes,0=no.",1,"Multiply edges."),
            new("Atd=1/2, V=1. 1=yes,0=no.",1,"4×quarter."),
            new("|-15|>|-12|. 1=yes,0=no.",1,"15>12."),
            new("|-12|=12. 1=yes,0=no.",1,"Magnitude."),
            new("-15<-12. 1=yes,0=no.",1,"Signed order."),
            new("|-12|>-15. 1=yes,0=no.",1,"12>-15."),
            new("12>-12. 1=yes,0=no.",1,"Positive greater."),
            new("An inequality with variable measurement may have infinitely many possible solutions. 1=yes,0=no.",1,"Continuous domain."),
            new("A count of books must generally be a whole nonnegative number, even if inequality includes real fractions. 1=yes,0=no.",1,"Contextual domain."),
            new("Strict bounds are marked with open circles on a number line. 1=yes,0=no.",1,"Endpoint excluded.")
        ],
        [L0709BoundedSolutions] =
        [
            new("Inequality k>5. Is4 a solution? Enter1 yes,0 no.",0,"4≤5."),
            new("Inequality k>5. Is5 a solution?",0,"Strictly greater."),
            new("Inequality k>5. Is6 a solution?",1,"6>5."),
            new("Inequality k>5. Is5.2 a solution?",1,"5.2>5."),
            new("Inequality k>5. Is5.01 a solution?",1,"5.01>5."),
            new("Inequality k>5. Is0.5 a solution?",0,".5<5."),
            new("Speed limit60mph. Is60mph permitted?",1,"s≤60."),
            new("Speed limit60mph. Is61mph permitted?",0,"Exceeds limit."),
            new("Boston low52F and high60F. Is52F included in range?",1,"Inclusive extremes."),
            new("Boston low52 high60F. Is60F included?",1,"Inclusive."),
            new("Boston low52 high60F. Is61F permitted?",0,"Above high."),
            new("Source true claim -5<|-5|? Enter1 yes,0 no.",1,"-5<5."),
            new("Source true claim |-6|<-5? Enter1 yes,0 no.",0,"6<-5 false."),
            new("Source true claim |-6|<3? Enter1 yes,0 no.",0,"6<3 false."),
            new("Source true claim4<|-7|? Enter1 yes,0 no.",1,"4<7."),
            new("Source true claim|-7|<|-8|? Enter1 yes,0 no.",1,"7<8."),
            new("Positive solution x to x^4=81?",3,"3^4=81."),
            new("Positive x to x²=100?",10,"10²."),
            new("x³=64. x?",4,"4³."),
            new("x⁵=32. x?",2,"2^5."),
            new("Phone usual250dollars paid150dollars. Paid percentage?",60,"150/250=60%."),
            new("Phone usual250dollars 75% paid. Dollars in QUARTERS?",750,"187.5=750 quarters."),
            new("At t=55F, is temperature between52 and60 inclusive?",1,"52≤55≤60.")
        ],
        [L0709SolutionLogic] =
        [
            new("6 is a solution to k>5. 1=yes,0=no.",1,"6>5."),
            new("5 is a solution to k>5. 1=yes,0=no.",0,"Excluded endpoint."),
            new("5.01 is a solution to k>5. 1=yes,0=no.",1,"Positive difference."),
            new("The solution set k>5 includes infinitely many real numbers. 1=yes,0=no.",1,"Unbounded continuum."),
            new("A posted maximum speed60mph can be represented by s≤60. 1=yes,0=no.",1,"At most60."),
            new("Exactly60mph violates a60mph speed limit in the mathematical example. 1=yes,0=no.",0,"At limit allowed."),
            new("A car speed must be nonnegative in usual speed magnitude context. 1=yes,0=no.",1,"Contextual domain."),
            new("Temperature from low52F through high60F can be represented52≤T≤60. 1=yes,0=no.",1,"Inclusive high/low."),
            new("The temperatures52 and60 are excluded from a daily recorded low/high interval. 1=yes,0=no.",0,"Both recorded."),
            new("-5<|-5|. 1=yes,0=no.",1,"-5<5."),
            new("|-6|<-5. 1=yes,0=no.",0,"6<-5 false."),
            new("4<|-7|. 1=yes,0=no.",1,"4<7."),
            new("|-7|<|-8|. 1=yes,0=no.",1,"7<8."),
            new("x^4=81 has positive integer solution3. 1=yes,0=no.",1,"3^4=81."),
            new("A phone bought150 from250 normal costs60% of usual. 1=yes,0=no.",1,".6."),
            new("75% of250dollars is187.50dollars. 1=yes,0=no.",1,"187.5."),
            new("An inequality's solution is a value making its statement true. 1=yes,0=no.",1,"Substitution."),
            new("For k>5, the endpoint5 is drawn with an open circle. 1=yes,0=no.",1,"Strict."),
            new("For s≤60, the endpoint60 is drawn with a filled circle. 1=yes,0=no.",1,"Inclusive.")
        ],
        [L0710ContextSolutions] =
        [
            new("Egg carton holds12, e eggs satisfies0<e<12. Is e=0 allowed?1 yes0 no.",0,"Excluded."),
            new("Egg carton0<e<12. Is e=1 allowed?1 yes0 no.",1,"1 in interval."),
            new("Egg carton0<e<12. Is e=11 allowed?1 yes0 no.",1,"11<12."),
            new("Egg carton0<e<12. Is e=12 allowed?1 yes0 no.",0,"Full cart excluded."),
            new("Egg carton0<e<12 whole eggs. Largest possible e?",11,"Eleven."),
            new("Jada taller54in and shorter60in. Is j=55 allowed?1 yes0 no.",1,"54<55<60."),
            new("Jada taller54in and shorter60in. Is j=54 allowed?1 yes0 no.",0,"Strict lower."),
            new("Jada taller54in and shorter60in. Is j=60 allowed?1 yes0 no.",0,"Strict upper."),
            new("Jada54<j<60. If integer inches, smallest j?",55,"55."),
            new("Jada54<j<60. If integer inches, largest j?",59,"59."),
            new("Tyler money t>10. Elena e>t. Mai m>e. Is m>10 necessarily true?1 yes0 no.",1,"Transitivity."),
            new("Tyler t>10, Elena e>t, Mai m>e. Is e>10 necessarily true?1 yes0 no.",1,"Transitivity."),
            new("Tyler t>10. Is t>10 true?1 yes0 no.",1,"Given."),
            new("Tyler t<e. Is that necessarily true?1 yes0 no.",1,"Elena more."),
            new("Elena e>m. Is that necessarily true?1 yes0 no.",0,"Mai more than Elena."),
            new("Compare -9/20 vs-0.5. Enter1 if first greater,0 equal,2 smaller.",1,"-0.45>-0.5."),
            new("(1/2)^3 in EIGHTHS?",1,"One eighth."),
            new("(1/2)^3 denominator?",8,"2³=8."),
            new("Elena is5ft tall. Inches?",60,"5×12."),
            new("Diego4ft6in. Inches?",54,"4×12+6."),
            new("Difference in height threshold60-54 inches?",6,"6in."),
            new("Of eggs1 through11, number of integer possibilities?",11,"11."),
            new("A square weighs s grams and circle12grams. Which is heavier when s=15? Enter1 square,2 circle.",1,"15>12.")
        ],
        [L0710ConstraintLogic] =
        [
            new("Egg carton0<e<12 allows11 whole eggs. 1=yes,0=no.",1,"11<12."),
            new("Egg count e can be3.7 eggs. 1=yes,0=no.",0,"Discrete whole count."),
            new("Egg carton0<e<12 excludes0 and12. 1=yes,0=no.",1,"Strict."),
            new("Height j>54 and j<60 means54<j<60. 1=yes,0=no.",1,"Combined bounds."),
            new("Jada can be54in if she is taller than Diego54in. 1=yes,0=no.",0,"Strictly taller."),
            new("Jada can be59.5in if shorter than Elena60in. 1=yes,0=no.",1,"Continuous height."),
            new("If Tyler has over10 dollars and Elena more than Tyler, Elena must have over10. 1=yes,0=no.",1,"Transitivity."),
            new("If Mai more than Elena more than Tyler, Mai more than Tyler. 1=yes,0=no.",1,"Transitivity."),
            new("Tyler has over10, so Tyler must have exactly11 dollars. 1=yes,0=no.",0,"Could be10.01 etc."),
            new("-9/20 is greater than-0.5. 1=yes,0=no.",1,"-0.45>-0.5."),
            new("(1/2)^3=1/8. 1=yes,0=no.",1,"Source A,B,E."),
            new("1/6 is equivalent to(1/2)^3. 1=yes,0=no.",0,"1/6≠1/8."),
            new("A real-world hanger that tips must be modeled with a strict or non-strict inequality according to physical evidence. 1=yes,0=no.",1,"Weight comparison."),
            new("A positive physical square weight s cannot be0 even if a symbolic inequality alone seems to allow0. 1=yes,0=no.",1,"Domain."),
            new("A single numeric solution is enough to describe all values satisfying an inequality. 1=yes,0=no.",0,"Solution set matters."),
            new("The question's context can rule out negative numbers even when symbolic inequality allows them. 1=yes,0=no.",1,"Physical quantities."),
            new("A strict inequality's endpoint is not included. 1=yes,0=no.",1,"Open interval."),
            new("An inequality can compare two unknown quantities, such as m>e>t. 1=yes,0=no.",1,"Relational constraints.")
        ],
        [L0711Coordinates] =
        [
            new("Coordinate point(-2,3). Which quadrant? Enter1-4.",2,"Negative x positive y is quadrant II."),
            new("Coordinate point(2,3). Which quadrant? Enter1-4.",1,"Both coordinates positive, quadrant I."),
            new("Coordinate point(-2,-3). Which quadrant? Enter1-4.",3,"Both negative, quadrant III."),
            new("Coordinate point(2,-3). Which quadrant? Enter1-4.",4,"Positive x negative y, quadrant IV."),
            new("Original four vertices x=-2/2 and y=3/-3 form rectangle. Width in units?",4,"2-(-2)=4."),
            new("Original rectangle vertices x=-2/2,y=3/-3. Height in units?",6,"3-(-3)=6."),
            new("Original rectangle area in SQUARE UNITS?",24,"4×6=24."),
            new("Original rectangle perimeter in UNITS?",20,"2(4+6)=20."),
            new("Source figure point A is1 unit right from origin on x-axis. Enter x coordinate.",1,"A=(1,0)."),
            new("Source figure point A lies on x-axis. Enter y coordinate.",0,"A=(1,0)."),
            new("Source figure point B on y-axis halfway between-2 and-4. Enter y coordinate.",-3,"B=(0,-3)."),
            new("Source figure point C x=-6, halfway between y=4 and6. Enter y coordinate.",5,"C=(-6,5)."),
            new("Source figure point D x halfway-4 and-2, y halfway-4 and-6. Enter x coordinate.",-3,"D=(-3,-5)."),
            new("Source figure point D x=-3,y halfway between-4 and-6. Enter y coordinate.",-5,"D=(-3,-5)."),
            new("Source figure point E x between-2 and0, y=4. Enter x coordinate.",-1,"E=(-1,4)."),
            new("A horizontal line passes(-3.5,4),(0,4),(6.2,4). Enter constant y.",4,"Any point on same horizontal line has y=4."),
            new("Minneapolis and Tucson temperatures are opposites, Tucson24°C warmer. Tucson temperature in °C?",12,"Let Tucson t and Minneapolis -t: t-(-t)=24 so t=12."),
            new("Lin runs29meters in10seconds. Distance in TENTHS of a meter per second?",29,"29/10=2.9m/s,29 tenths."),
            new("Lin runs29meters in10seconds at constant speed. Distance in METERS per minute?",174,"2.9×60=174m."),
            new("Each chocolate box has20 bars sold at150cents each. Revenue per box in CENTS?",3000,"20×150=3000cents."),
            new("At30dollars per chocolate box, sales of7boxes collect in DOLLARS?",210,"7×30=210."),
            new("At30dollars per chocolate box, collected240dollars indicates boxes sold?",8,"240/30=8.")
        ],
        [L0711CoordinateLogic] =
        [
            new("Coordinate(-2,3) is in quadrantII. 1=yes,0=no.",1,"x negative, y positive."),
            new("Coordinate(2,-3) is in quadrantIII. 1=yes,0=no.",0,"Positive x, negative y gives quadrantIV."),
            new("Coordinate(-2,-3) is in quadrantIII. 1=yes,0=no.",1,"Both negative."),
            new("Coordinate(2,3) is in quadrantI. 1=yes,0=no.",1,"Both positive."),
            new("Points(-2,3),(2,3),(-2,-3),(2,-3) form a rectangle when joined along edges. 1=yes,0=no.",1,"Opposite corners and edges."),
            new("The line through(-3.5,4),(0,4),(6.2,4) is horizontal. 1=yes,0=no.",1,"All y values4."),
            new("A point(8,4) lies on the horizontal line y=4. 1=yes,0=no.",1,"Same y coordinate."),
            new("A point(4,8) lies on the horizontal line y=4. 1=yes,0=no.",0,"Its y is8."),
            new("The point(0,-3) lies in quadrantIII. 1=yes,0=no.",0,"Points on axes lie in no quadrant."),
            new("The point(1,0) lies on x-axis. 1=yes,0=no.",1,"y=0."),
            new("The origin is(0,0) and belongs to no quadrant. 1=yes,0=no.",1,"Axes intersection."),
            new("Ordered coordinates(x,y) list horizontal displacement first. 1=yes,0=no.",1,"x before y."),
            new("Temperature12°C vs-12°C differs by24°C. 1=yes,0=no.",1,"12-(-12)=24."),
            new("29m in10s at constant speed implies174m in60s. 1=yes,0=no.",1,"Multiply both by6."),
            new("20 bars at1.50dollars means30dollars per box. 1=yes,0=no.",1,"20×1.5=30."),
            new("In m=30b the number of boxes b is independent if revenue depends on box count. 1=yes,0=no.",1,"m scales from b."),
            new("If the point's graph source image is missing, automatically guessing coordinate readings is safe. 1=yes,0=no.",0,"Teacher verifies visual descriptions and scale.")
        ],
        [L0712PlotAndScale] =
        [
            new("Point(1/5,4/5). Quadrant 1–4?",1,"Both positive, quadrant I."),
            new("Point(-3/5,2/5). Quadrant 1–4?",2,"Negative x,positive y."),
            new("Point(-1 1/5,-4/5). Quadrant 1–4?",3,"Both negative."),
            new("Point(1/5,-3/5). Quadrant 1–4?",4,"Positive x,negative y."),
            new("Point(-1 1/5,-4/5). x in FIFTHS?",-6,"-1 1/5=-6/5."),
            new("Point(1/5,4/5). y in FIFTHS?",4,"4/5."),
            new("Point(-3/5,2/5). x in FIFTHS?",-3,"-3/5."),
            new("Point(1/5,-3/5). y in FIFTHS?",-3,"-3/5."),
            new("Data points (-50,0),(150,100),(200,-100),(350,50),(-250,0). Minimum x?",-250,"-250 is smallest x."),
            new("Data points (-50,0),(150,100),(200,-100),(350,50),(-250,0). Maximum x?",350,"350 is largest x."),
            new("For those data points, minimum y?",-100,"-100."),
            new("For those data points, maximum y?",100,"100."),
            new("A uniform coordinate axis with tick interval50 from-250 through350 spans how many intervals?",12,"(350-(-250))/50=12."),
            new("Square centered at origin with corners(-3,-3),(-3,3),(3,3),(3,-3). Side length?",6,"3-(-3)=6."),
            new("Square centered origin with corners ±3,±3. Area in square units?",36,"6²=36."),
            new("Jada buys notebooks at175cents each. Cost of2 notebooks in CENTS?",350,"2×175."),
            new("Jada buys notebooks at175cents each. Cost of5 in CENTS?",875,"5×175."),
            new("Jada buys notebooks at175cents each. Cost of10 in CENTS?",1750,"10×175."),
            new("At175cents per notebook, is point(2,3.50) consistent with x=count,y=dollars? Enter1 yes,0 no.",1,"2×1.75=3.50."),
            new("At175cents per notebook, is point(5,8.75) consistent? Enter1 yes,0 no.",1,"5×1.75=8.75."),
            new("At175cents per notebook, is point(9,15.35) consistent? Enter1 yes,0 no.",0,"9×1.75=15.75, not15.35."),
            new("At175cents per notebook, is point(17.50,10) consistent? Enter1 yes,0 no.",0,"Count not integer and equation fails."),
            new("Water use15,000,000 gallons for28.6 acres. Nearest of 5000,50000,500000,5000000 gallons per acre? Enter option1–4.",3,"15million/28.6≈524,476, nearest500,000."),
            new("Loss of4 points represented as signed change?",-4,"Decrease4."),
            new("Loss of10 dollars represented as signed change?",-10,"Decrease10.")
        ],
        [L0712ScaleReasoning] =
        [
            new("To plot fifth-coordinate values, intervals of1/5 on the axes can represent them precisely. 1=yes,0=no.",1,"Ticks in fifths."),
            new("The x and y axes must always share the same numeric scale. 1=yes,0=no.",0,"Axes can use appropriate different scales with labels."),
            new("The point(-250,0) lies on negative x-axis, not in a quadrant. 1=yes,0=no.",1,"y=0."),
            new("The point(200,-100) is in quadrantIV. 1=yes,0=no.",1,"x>0,y<0."),
            new("The point(150,100) is in quadrantI. 1=yes,0=no.",1,"Both coordinates positive."),
            new("A tick interval of50 can display every integer-coordinate value of sample (-50,0),(150,100),(200,-100),(350,50),(-250,0) at ticks. 1=yes,0=no.",1,"All sample coordinates multiples of50."),
            new("Choosing an x-axis only from0 to200 displays both-250 and350 accurately. 1=yes,0=no.",0,"Range excludes values."),
            new("Four corners at(-3,-3),(-3,3),(3,3),(3,-3) make a square centered at origin. 1=yes,0=no.",1,"Symmetric corners ±3."),
            new("A loss of4 points is represented by-4. 1=yes,0=no.",1,"Signed decrease."),
            new("A gain of50 yards is represented by+50. 1=yes,0=no.",1,"Signed increase."),
            new("An elevation above sea level is ordinarily modeled with positive sign. 1=yes,0=no.",1,"Above reference zero."),
            new("The equation c=1.75n models notebook cost c in dollars for n notebooks. 1=yes,0=no.",1,"Unit rate."),
            new("If c=1.75n, plotting notebooks on x-axis and cost on y-axis is consistent. 1=yes,0=no.",1,"n independent,c dependent."),
            new("A reported point(1.75,1) on graph cost vs notebooks represents one notebook costing1.75. 1=yes,0=no.",0,"Coordinates reversed; (1,1.75) works."),
            new("A grid axis scale may use an interval50 rather than 1 for data ranging-250 to350. 1=yes,0=no.",1,"Clarity depends on range."),
            new("Appropriate labels, equal axis tick spacing, and origin placement matter to interpretation. 1=yes,0=no.",1,"Correct geometry and communication.")
        ],
        [L0713CoordinatesData] =
        [
            new("Submarine time0hours after noon, elevation in meters?",-567,"Given table at t=0."),
            new("Submarine after1hour, elevation in meters?",-892,"Source table t=1."),
            new("Submarine after2hours, elevation in meters?",-1606,"Source table t=2."),
            new("Submarine after3hours, elevation in meters?",-1289,"Source table t=3."),
            new("Submarine after4hours, elevation in meters?",-990,"Source table t=4."),
            new("Submarine after5hours, elevation in meters?",-702,"Source table t=5."),
            new("Submarine after6hours, elevation in meters?",-365,"Source table t=6."),
            new("Which time after noon has LOWEST submarine elevation? Enter hour0–6.",2,"-1606meters is lowest at2."),
            new("Which time after noon has HIGHEST submarine elevation? Enter hour0–6.",6,"-365 is largest elevation."),
            new("From t0 elevation-567m to t1 -892m, signed elevation change?",-325,"-892-(-567)=-325meters."),
            new("From t2 elevation-1606m to t3 -1289m, signed change?",317,"-1289-(-1606)=+317m."),
            new("From t5 elevation-702m to t6 -365m, signed change?",337,"-365-(-702)=337."),
            new("On graph x=hours relative noon, y=temp°C, at9a.m., time coordinate?",-3,"9 is3hours before noon."),
            new("On graph x=hours relative noon, y=temp°C, at11a.m., time coordinate?",-1,"One hour before noon."),
            new("At9a.m. below freezing on x=time relative noon,y=temp, quadrant number?",3,"Negative time and temperature."),
            new("At11a.m.10°C on that plane, quadrant number?",2,"Negative x positive y."),
            new("At noon and 0°C, quadrant code0 for on origin?",0,"(0,0) is origin."),
            new("Solve3a=12. What is a?",4,"a=12/3=4."),
            new("Solve b+3.3=8.9. b in TENTHS?",56,"b=5.6=56tenths."),
            new("Solve1=1/4 c. What is c?",4,"Multiply both sides4."),
            new("Solve5.5=d+1/4. d in QUARTERS?",21,"d=5.25=21/4."),
            new("Solve2e=6.4. e in TENTHS?",32,"e=3.2=32tenths."),
            new("Ride accepts height greater than42 and less than60inches. Is55 accepted? Enter1 yes,0 no.",1,"42<55<60."),
            new("Ride accepts h>42 and h<60. Is42 accepted? Enter1 yes,0 no.",0,"Strict inequality excludes42."),
            new("Ride accepts h>42 and h<60. Is60 accepted? Enter1 yes,0 no.",0,"Strict inequality excludes60.")
        ],
        [L0713ContextLogic] =
        [
            new("The ordered pair(2,-1606) means2hours after noon and elevation-1606meters. 1=yes,0=no.",1,"x=time,y=elevation."),
            new("The ordered pair(-1606,2) means the same when x=time and y=elevation. 1=yes,0=no.",0,"Coordinate order matters."),
            new("Among listed submarine elevations, -1606m is deepest. 1=yes,0=no.",1,"Lowest numeric elevation."),
            new("The submarine elevation rises by317m from hour2 to hour3. 1=yes,0=no.",1,"-1289-(-1606)=317."),
            new("An underwater elevation of-365m is greater than-1606m. 1=yes,0=no.",1,"Closer to sea level."),
            new("For time before/after noon,9a.m. maps to x=-3. 1=yes,0=no.",1,"Three hours before."),
            new("At9a.m. below freezing point(x,y) lies in quadrantIII. 1=yes,0=no.",1,"Both negative."),
            new("At11a.m.10°C point(-1,10) lies in quadrantII. 1=yes,0=no.",1,"Negative x positive y."),
            new("The point(0,0) represents noon at0°C for this graph. 1=yes,0=no.",1,"Zero both quantities."),
            new("At noon5°C, the point lies on positive y-axis and not within a quadrant. 1=yes,0=no.",1,"x=0."),
            new("Ride rule h>42 and h<60 includes height42 inches. 1=yes,0=no.",0,"Strict greater than42."),
            new("Ride rule h>42 and h<60 includes height59 inches. 1=yes,0=no.",1,"59 within interval."),
            new("Ride rule42<h<60 includes height60 inches. 1=yes,0=no.",0,"Strict smaller than60."),
            new("Submarine time axis may use1hour ticks while elevation uses200m ticks if labeled clearly. 1=yes,0=no.",1,"Axis scales can differ."),
            new("A coordinate plane without units or reference can be interpreted uniquely as submarine elevation and time. 1=yes,0=no.",0,"Context labels needed."),
            new("3a=12 has a=4. 1=yes,0=no.",1,"3×4=12."),
            new("b+3.3=8.9 gives b=5.6. 1=yes,0=no.",1,"Subtract3.3."),
            new("5½=d+¼ gives d=5¼. 1=yes,0=no.",1,"Subtract quarter."),
            new("When graphing before/after noon, times before noon may have negative x values. 1=yes,0=no.",1,"Signed time reference."),
            new("A point representing hour0,-567m is on negative y-axis, not in quadrantIV. 1=yes,0=no.",1,"x=0.")
        ],
        [L0714Distances] =
        [
            new("Distance in units between A(3,5) and B(3,6)?",1,"Same x, vertical distance|6-5|=1."),
            new("Distance in units between C(-2,-3) and D(-2,-6)?",3,"Same x,|-6-(-3)|=3."),
            new("Distance in units between E(-3,1) and F(-3,-1)?",2,"Same x,|-1-1|=2."),
            new("P=(-2,-1). Move3 units RIGHT. New x?",1,"-2+3=1."),
            new("P=(-2,-1). Move3 units RIGHT. New y?",-1,"Horizontal move leaves y."),
            new("P=(-2,-1). Move3 units LEFT. New x?",-5,"-2-3=-5."),
            new("P=(-2,-1). Move3 units LEFT. New y?",-1,"y unchanged."),
            new("P=(-2,-1). Move3 units UP. New x?",-2,"Vertical move leaves x."),
            new("P=(-2,-1). Move3 units UP. New y?",2,"-1+3=2."),
            new("P=(-2,-1). Move3 units DOWN. New x?",-2,"x unchanged."),
            new("P=(-2,-1). Move3 units DOWN. New y?",-4,"-1-3=-4."),
            new("Horizontal distance from(-4,3) to(5,3) in units?",9,"|5-(-4)|=9."),
            new("Vertical distance from(7,-8) to(7,2) in units?",10,"|2-(-8)|=10."),
            new("Vertical distance from(-3,-2.5) to(-3,1.5) in units?",4,"|1.5-(-2.5)|=4."),
            new("Sparkling orange juice uses4L orange and5L soda. Total liters for one batch?",9,"4+5=9L."),
            new("Sparkling orange juice soda is what fraction of total in NINTHS?",5,"5/9."),
            new("Sparkling orange juice orange is what fraction of total in NINTHS?",4,"4/9."),
            new("For 9L mixture, soda amount L?",5,"Five parts soda."),
            new("For 18L mixture, soda amount L?",10,"18×5/9=10."),
            new("For 8L orange juice, total mixed beverage L?",18,"8×9/4=18."),
            new("For 4L orange juice, soda L?",5,"Scale ratio4:5."),
            new("Suitcase23kg, approximate10kg=22lb. Weight in TENTHS OF POUND?",506,"23×22/10=50.6lb."),
            new("Suitcase can weigh AT MOST50lb. Under approximate23kg≈50.6lb, permitted? Enter1 yes,0 no.",0,"50.6>50."),
            new("Points(-5,-1) and(1,-1) are horizontally how many units apart?",6,"1-(-5)=6.")
        ],
        [L0714DistanceLogic] =
        [
            new("A(3,5) and B(3,6) form a vertical segment. 1=yes,0=no.",1,"x coordinate unchanged."),
            new("C(-2,-3) to D(-2,-6) has length3. 1=yes,0=no.",1,"Absolute y difference3."),
            new("E(-3,1) to F(-3,-1) has length2. 1=yes,0=no.",1,"|1-(-1)|=2."),
            new("Distance between(3,5) and(3,6) equals y difference6-5 in absolute value. 1=yes,0=no.",1,"Same x."),
            new("Horizontal distance between(-4,3) and(5,3) is1 because signs differ. 1=yes,0=no.",0,"Distance9."),
            new("Points(-5,-1),(1,-1),(-2,2),(-2,-4) each lie exactly3 units from(-2,-1) along an axis. 1=yes,0=no.",1,"Each shift has magnitude3."),
            new("Any point exactly3 units from(-2,-1) must share x or y coordinate. 1=yes,0=no.",0,"There are also diagonal points on the radius3 circle."),
            new("Reflecting(x,y) across vertical y-axis produces(-x,y). 1=yes,0=no.",1,"Horizontal reflection."),
            new("Reflecting(x,y) across horizontal x-axis produces(x,-y). 1=yes,0=no.",1,"Vertical reflection."),
            new("A horizontal segment's endpoints share the same y coordinate. 1=yes,0=no.",1,"Horizontal displacement only."),
            new("A vertical segment's endpoints share the same x coordinate. 1=yes,0=no.",1,"Vertical displacement only."),
            new("A point(-2,-1) is in quadrantIII. 1=yes,0=no.",1,"Both negative."),
            new("Sparkling juice4 orange:5 soda means5/9 of mixed beverage is soda. 1=yes,0=no.",1,"Total9 parts."),
            new("If orange amount j is known, total juice mix t=(9/4)j. 1=yes,0=no.",1,"Total/orange=9/4."),
            new("If mixed total t is known, soda amount s=(5/9)t. 1=yes,0=no.",1,"Soda/total=5/9."),
            new("A checked bag limited to50lb is allowed at50.6lb. 1=yes,0=no.",0,"50.6 exceeds maximum."),
            new("The factor22lb per10kg is approximate, so a careful solution marks50.6lb approximate. 1=yes,0=no.",1,"Rounded conversion."),
            new("For axis-aligned distance, subtract coordinates and take absolute value to ensure nonnegative length. 1=yes,0=no.",1,"Length must be nonnegative.")
        ],
        [L0715PolygonNumbers] =
        [
            new("Rectangle corners(3,0),(3,-5),(-4,0),(-4,-5). Horizontal side length?",7,"|3-(-4)|=7."),
            new("Same rectangle vertical side length?",5,"|0-(-5)|=5."),
            new("Same rectangle perimeter?",24,"2(7+5)=24."),
            new("Same rectangle area in square units?",35,"7×5=35."),
            new("A square has perimeter20. Length of each side?",5,"20/4=5."),
            new("An axis-aligned square uses vertex(-3,5) with side5 to the right and down. Second vertex x?",2,"-3+5=2."),
            new("Same square from(-3,5), second vertex y?",5,"Stay horizontal at5."),
            new("Same square corners(-3,5),(2,5),(2,0),(-3,0). Area in SQUARE UNITS?",25,"5×5=25."),
            new("Source polygon(-3,2)->(2,2). First horizontal edge length?",5,"2-(-3)=5."),
            new("Source polygon(2,2)->(2,-4). Second vertical edge length?",6,"2-(-4)=6."),
            new("Source polygon(2,-4)->(-1,-4). Third edge length?",3,"2-(-1)=3."),
            new("Source polygon(-1,-4)->(-1,-2). Fourth edge length?",2,"-2-(-4)=2."),
            new("Source polygon(-1,-2)->(-3,-2). Fifth edge length?",2,"-1-(-3)=2."),
            new("Source polygon(-3,-2)->(-3,2). Sixth edge length?",4,"2-(-2)=4."),
            new("Total perimeter of source six-edge polygon?",22,"5+6+3+2+2+4=22."),
            new("Jada's cat mass3.45kg and Andre's cat1.2kg more. Andre mass in HUNDREDTHS OF KG?",465,"3.45+1.2=4.65kg."),
            new("Andre cat4.65kg, Jada3.45kg. Difference in HUNDREDTHS KG?",120,"4.65-3.45=1.2kg."),
            new("Apples cost1.60 dollars/lb in market, grocery charges1.5times. Grocery cost in CENTS per lb?",240,"160×1.5=240cents."),
            new("A square side5 with bottom-left(-3,0). Top-right x coordinate?",2,"x=-3+5=2."),
            new("A square side5 with bottom-left(-3,0). Top-right y coordinate?",5,"y=0+5."),
            new("Source six-edge polygon perimeter22. Half its perimeter in UNITS?",11,"22/2=11.")
        ],
        [L0715PolygonChecks] =
        [
            new("Rectangle x values3 and-4 has width7. 1=yes,0=no.",1,"|3-(-4)|=7."),
            new("Rectangle y values0 and-5 has height5. 1=yes,0=no.",1,"|0-(-5)|=5."),
            new("A7-by5 rectangle has perimeter24 and area35. 1=yes,0=no.",1,"2(7+5) and7×5."),
            new("Square perimeter20 means side length4. 1=yes,0=no.",0,"Side5."),
            new("Points(-3,5),(2,5),(2,0),(-3,0) are corners of a square of side5. 1=yes,0=no.",1,"Five units both axes."),
            new("Source six-edge polygon has total perimeter22. 1=yes,0=no.",1,"5+6+3+2+2+4."),
            new("Source polygon edge(-1,-4) to(-1,-2) is horizontal. 1=yes,0=no.",0,"x constant, vertical."),
            new("Source polygon edge(2,-4) to(-1,-4) is horizontal length3. 1=yes,0=no.",1,"y constant."),
            new("Perimeter adds lengths of all outer edges, rather than area squares inside. 1=yes,0=no.",1,"Boundary length."),
            new("Teacher should check graph orientation/scale and vertex connections, not just a numeric perimeter. 1=yes,0=no.",1,"Diagram evidence distinct from math."),
            new("Andre cat mass x=3.45+1.2 is a valid original equation. 1=yes,0=no.",1,"Heavier by1.2."),
            new("Andre cat mass x-1.2=3.45 is also valid. 1=yes,0=no.",1,"Subtract difference."),
            new("Andre cat mass x=3.45-1.2 is valid when he weighs more. 1=yes,0=no.",0,"Would be lighter."),
            new("Grocery apples price y=1.5×1.60 is valid. 1=yes,0=no.",1,"Multiplier of market price."),
            new("Grocery apples price y/1.5=1.60 is valid. 1=yes,0=no.",1,"Equivalent equation."),
            new("Grocery apples price y=1.60/1.5 is valid for more expensive grocery. 1=yes,0=no.",0,"Incorrect inverse."),
            new("Horizontal/vertical segments may cross zero; using absolute coordinate differences yields correct length. 1=yes,0=no.",1,"Nonnegative length."),
            new("Polygon vertex order is irrelevant to shape if connected in a different arbitrary order. 1=yes,0=no.",0,"Different order can alter edges."),
            new("A coordinate polygon may contain both positive and negative vertex coordinates. 1=yes,0=no.",1,"Four-quadrant figures.")
        ],
        [L0716Factors] =
        [
            new("Gift bags24 pencils,36 stickers split equally without leftovers. Maximum bag count?",12,"GCF(24,36)=12."),
            new("12 gift bags distribute24pencils. Pencils per bag?",2,"24/12=2."),
            new("12 gift bags distribute36stickers. Stickers per bag?",3,"36/12=3."),
            new("6 equal gift bags distribute24pencils. Pencils per bag?",4,"24/6=4."),
            new("6 equal gift bags distribute36stickers. Stickers per bag?",6,"36/6=6."),
            new("4 gift bags distribute24pencils. Pencils per bag?",6,"24/4=6."),
            new("4 gift bags distribute36stickers. Stickers per bag?",9,"36/4=9."),
            new("List factors of42. Largest factor?",42,"42 divides itself."),
            new("Is7 a factor of42? Enter1 yes,0 no.",1,"42/7=6."),
            new("Is8 a factor of42? Enter1 yes,0 no.",0,"42 not divisible by8."),
            new("Greatest common factor of42 and15?",3,"42 factors1,2,3,6,7,14,21,42;15 factors1,3,5,15."),
            new("Greatest common factor of42 and50?",2,"Both divisible by2; larger common factors none."),
            new("Greatest common factor of90 and75?",15,"90=6×15,75=5×15."),
            new("Chorus90 sixth graders and75 seventh graders. Maximum identical mixed groups?",15,"GCF90,75=15."),
            new("With15 identical groups, sixth graders per group?",6,"90/15=6."),
            new("With15 identical groups, seventh graders per group?",5,"75/15=5."),
            new("Fraction1/7÷1/8, answer in SEVENTHS?",8,"1/7×8=8/7."),
            new("Fraction12/5÷6/5 equals?",2,"12/5×5/6=2."),
            new("Fraction1/10÷10, answer in HUNDREDTHS?",1,"1/100."),
            new("Fraction9/10÷10/9, answer in HUNDREDTHS?",81,"9/10×9/10=81/100."),
            new("Elephant runs25miles/hour. Distance in3hours?",75,"25×3=75."),
            new("Giraffe runs16miles perhalf-hour. Miles per HOUR?",32,"16/.5=32."),
            new("Giraffe at32mph. Distance in3hours?",96,"32×3=96."),
            new("Giraffe32mph vs elephant25mph. Speed difference mph?",7,"32-25=7.")
        ],
        [L0716GcfLogic] =
        [
            new("12 is a common factor of24 and36. 1=yes,0=no.",1,"Both multiples of12."),
            new("18 is a common factor of24 and36. 1=yes,0=no.",0,"24 not multiple18."),
            new("The greatest common factor of24 and36 is12. 1=yes,0=no.",1,"Largest dividing both."),
            new("Making 5 equal bags with24 pencils and36 stickers leaves no remainder. 1=yes,0=no.",0,"Neither divisible by5."),
            new("Making 3 equal bags with24 pencils and36 stickers leaves no remainder. 1=yes,0=no.",1,"8 pencils and12 stickers per bag."),
            new("42 and15 have GCF3. 1=yes,0=no.",1,"Both divisible by3."),
            new("42 and50 have GCF2. 1=yes,0=no.",1,"Common factors1,2."),
            new("For90 sixth and75 seventh, groups of15 have6 sixth and5 seventh in each. 1=yes,0=no.",1,"90/15=6,75/15=5."),
            new("For mixed identical chorus groups, count should be a common divisor of both class totals. 1=yes,0=no.",1,"Each grade splits equally."),
            new("Monday paycheck deposit650dollars is a positive balance change. 1=yes,0=no.",1,"Deposit adds funds."),
            new("Tuesday40dollar ATM withdrawal is negative signed balance change. 1=yes,0=no.",1,"Money withdrawn."),
            new("Wednesday20dollar returned-merchandise credit is negative balance change. 1=yes,0=no.",0,"Credit increases balance."),
            new("Thursday125dollar phone charge is negative balance change. 1=yes,0=no.",1,"Deduction."),
            new("Friday45dollar written check is negative balance change. 1=yes,0=no.",1,"Outgoing check."),
            new("Saturday80dollar withdrawal is negative balance change. 1=yes,0=no.",1,"Money outgoing."),
            new("Sunday10dollar cash-back deposit is negative balance change. 1=yes,0=no.",0,"Credit increases balance."),
            new("Giraffe32mph is faster than elephant25mph under given constant-rate assumptions. 1=yes,0=no.",1,"32>25."),
            new("Giraffe96miles/3hours exceeds elephant75miles/3hours by21miles. 1=yes,0=no.",1,"96-75=21."),
            new("GCF finds the biggest equal-group count that divides two totals with no leftovers. 1=yes,0=no.",1,"Common divisor."),
            new("A correct count of bags proves a student's chosen arrangement is understandable without inspecting their drawing. 1=yes,0=no.",0,"Teacher evaluates grouping representation.")
        ],
        [L0717Multiples] =
        [
            new("Green light every4 seconds, yellow every5 seconds. First simultaneous blink after start in SECONDS?",20,"LCM(4,5)=20."),
            new("Red light every12 seconds, blue every9 seconds. First simultaneous blink in SECONDS?",36,"LCM(12,9)=36."),
            new("Least common multiple of10 and15?",30,"30 divisible by10 and15."),
            new("Least common multiple of8 and12?",24,"24 divisible by8 and12."),
            new("Cups sold8/pack, napkins12/pack. Fewest number of CUP packs for equal items?",3,"24 cups needs3 packs."),
            new("Cups sold8/pack, napkins12/pack. Fewest NAPKIN packs for equal items?",2,"24 napkins needs2 packs."),
            new("With3 cup packs of8 and2 napkin packs of12, number of matched sets?",24,"24 cups and24 napkins."),
            new("Which of20,25,30,35 is a multiple of both10 and15? Enter number.",30,"30 shared multiple."),
            new("Next simultaneous blink of lights4sec and5sec AFTER20s, enter time?",40,"Multiples of20."),
            new("Next simultaneous blink of12sec and9sec AFTER36s, enter time?",72,"Multiples of36."),
            new("Least common multiple of6 and8?",24,"24 divisible by6 and8."),
            new("Least common multiple of4 and6?",12,"12 divisible by4 and6."),
            new("Least common multiple of9 and12?",36,"36 divisible by9 and12."),
            new("Least common multiple of7 and5?",35,"Relatively prime."),
            new("Top horizontal edge from(-5,3) to(3,3). Length?",8,"3-(-5)=8."),
            new("Bottom horizontal edge from(1,-2) to(-3,-2). Length?",4,"1-(-3)=4."),
            new("Rectangle A(-6,9),B(5,9), C(5,2),D(-6,2). Width?",11,"5-(-6)=11."),
            new("Same sample rectangle height?",7,"9-2=7."),
            new("School fundraising goal2500dollars, raised20%. Dollars raised?",500,"2500×.2=500."),
            new("School fundraising goal2500dollars, raised175%. Dollars raised?",4375,"2500×1.75=4375."),
            new("Fundraising175% of2500. Dollars ABOVE goal?",1875,"4375-2500=1875."),
            new("For lights4sec and5sec, how many green blinks by time20sec excluding time0?",5,"20/4=5."),
            new("For lights4sec and5sec, how many yellow blinks by time20sec excluding time0?",4,"20/5=4.")
        ],
        [L0717LcmLogic] =
        [
            new("20 is the least positive common multiple of4 and5. 1=yes,0=no.",1,"LCM=20."),
            new("36 is least positive common multiple of12 and9. 1=yes,0=no.",1,"36=3×12=4×9."),
            new("Multiples of10 up to100 include10,20,30,40,50,60,70,80,90,100. 1=yes,0=no.",1,"Successive tens."),
            new("Multiples of15 up to100 include15,30,45,60,75,90. 1=yes,0=no.",1,"Six multiples under100."),
            new("LCM(10,15)=15. 1=yes,0=no.",0,"15 not divisible by10."),
            new("LCM(10,15)=30. 1=yes,0=no.",1,"Smallest common multiple."),
            new("Three packages of8 cups and two packages of12 napkins give24 matched place settings. 1=yes,0=no.",1,"24 each."),
            new("Two packs of8 cups and two of12 napkins make the same count. 1=yes,0=no.",0,"16 vs24."),
            new("LCM is the smallest positive number divisible by both inputs. 1=yes,0=no.",1,"Definition."),
            new("The greatest common factor and least common multiple are always equal. 1=yes,0=no.",0,"Different operations."),
            new("Lights blinking at4-second and5-second intervals coincide at20,40,60sec. 1=yes,0=no.",1,"Multiples20."),
            new("The source top edge(-5,3) to(3,3) is8units long. 1=yes,0=no.",1,"|3+5|=8."),
            new("The source bottom edge(1,-2) to(-3,-2) is4units long. 1=yes,0=no.",1,"|1+3|=4."),
            new("Given rectangle A(-6,9) B(5,9), other vertices have to be uniquely(5,2),(-6,2). 1=yes,0=no.",0,"Many heights/placements possible."),
            new("20% of2500dollars is500dollars. 1=yes,0=no.",1,"One fifth."),
            new("175% of2500dollars is4375dollars. 1=yes,0=no.",1,"1.75×2500."),
            new("Equivalent package groups should have no leftover cups or napkins in the minimum matched-set example. 1=yes,0=no.",1,"Exactly24 each."),
            new("An LCM story usually concerns a shared repeat time or minimum equal multiple, not maximum equal group size. 1=yes,0=no.",1,"GCF handles equal largest groups.")
        ],
        [L0718FactorMultipleChoices] =
        [
            new("Mai6minutes per sign, Clare8minutes. First simultaneous completion in MINUTES?",24,"LCM(6,8)=24."),
            new("Mai6minutes per sign, Noah5minutes. First simultaneous completion in MINUTES?",30,"LCM(6,5)=30."),
            new("Clare8minutes per sign, Noah5minutes. First simultaneous completion in MINUTES?",40,"LCM(8,5)=40."),
            new("Mai6,Clare8,Noah5 minute sign intervals. First simultaneous completion of ALL THREE in minutes?",120,"LCM(6,8,5)=120."),
            new("Within30 minutes, do Mai and Clare finish a sign simultaneously? Enter1 yes,0 no.",1,"They coincide at24min."),
            new("Within30 minutes, do Mai and Noah finish simultaneously? Enter1 yes,0 no.",1,"They coincide at30min."),
            new("Within30 minutes, do Clare and Noah finish simultaneously? Enter1 yes,0 no.",0,"First at40min."),
            new("Within30 minutes, do all three finish simultaneously? Enter1 yes,0 no.",0,"First at120min."),
            new("Diego has48 chocolate chip,64 vanilla,100 raisin cookies. Greatest possible identical bags?",4,"GCF(48,64,100)=4."),
            new("With4 bags, chocolate chip cookies per bag?",12,"48/4=12."),
            new("With4 bags, vanilla cookies per bag?",16,"64/4=16."),
            new("With4 bags, raisin cookies per bag?",25,"100/4=25."),
            new("With2 equal bags, chocolate chip cookies per bag?",24,"48/2=24."),
            new("With2 equal bags, vanilla cookies per bag?",32,"64/2=32."),
            new("With2 equal bags, raisin cookies per bag?",50,"100/2=50."),
            new("Product12×8?",96,"96."),
            new("Greatest common factor of12 and8?",4,"Factors common1,2,4."),
            new("Least common multiple of12 and8?",24,"24 divisible by12 and8."),
            new("Product GCF(12,8)×LCM(12,8)?",96,"4×24=96."),
            new("Given point(5.5,-7), choose second(5.5,1). Vertical length in UNITS?",8,"|1-(-7)|=8."),
            new("Given point(3,3.5), choose second(8,3.5). Horizontal length in UNITS?",5,"|8-3|=5."),
            new("Half times37 minus half times7 equals?",15,"(37-7)/2=15."),
            new("3.5×40+3.5×60 equals?",350,"3.5×100=350."),
            new("999×5 equals?",4995,"(1000-1)×5=4995.")
        ],
        [L0718DecisionLogic] =
        [
            new("Mai6-minute and Clare8-minute signs can coincide at24min. 1=yes,0=no.",1,"LCM24."),
            new("Mai6-minute and Noah5-minute signs can coincide at30min. 1=yes,0=no.",1,"LCM30."),
            new("Clare8-minute and Noah5-minute signs coincide by30min. 1=yes,0=no.",0,"First40min."),
            new("All three 6,8,5 minute sign patterns have a shared completion by30min. 1=yes,0=no.",0,"LCM120."),
            new("Using GCF is appropriate for finding maximum number of identical full cookie bags with no leftovers. 1=yes,0=no.",1,"Largest shared divisor."),
            new("Using LCM is appropriate for first matching future completion time of periodic events. 1=yes,0=no.",1,"Smallest shared positive multiple."),
            new("Cookie totals48,64,100 have greatest common factor4. 1=yes,0=no.",1,"Four is largest dividing all."),
            new("Eight identical cookie bags divide48,64,100 without leftovers. 1=yes,0=no.",0,"100 not divisible by8."),
            new("Four equal cookie bags contain12 chocolate,16 vanilla,25 raisin each. 1=yes,0=no.",1,"All exact."),
            new("Two equal cookie bags contain24 chocolate,32 vanilla,50 raisin each. 1=yes,0=no.",1,"Alternative."),
            new("For positive whole numbers12 and8, GCF×LCM equals12×8. 1=yes,0=no.",1,"4×24=96."),
            new("GCF(12,8)=24. 1=yes,0=no.",0,"GCF is4."),
            new("LCM(12,8)=24. 1=yes,0=no.",1,"Smallest shared multiple."),
            new("A vertical segment must share the same x coordinate at both endpoints. 1=yes,0=no.",1,"x constant."),
            new("A horizontal segment must share the same y coordinate at both endpoints. 1=yes,0=no.",1,"y constant."),
            new("The only possible vertical point paired with(5.5,-7) is(5.5,1). 1=yes,0=no.",0,"Many shared-x points."),
            new("Half times37 minus half times7 is15. 1=yes,0=no.",1,"Distributive property."),
            new("3.5 times40 plus3.5 times60 is350. 1=yes,0=no.",1,"3.5×100."),
            new("999×5=4995. 1=yes,0=no.",1,"5000-5."),
            new("A correct numeric result alone validates the optional student-generated Factors and Multiples Bingo activity. 1=yes,0=no.",0,"Teacher reviews constructed activity.")
        ],
        [L0719FirstPartyPlotting] =
        [
            new("Independent coordinate design includes point(-4,-2). Quadrant?",3,"Both negative: quadrantIII."),
            new("Design point(-4,2). Quadrant?",2,"Negative x and positive y."),
            new("Design point(-2,4). Quadrant?",2,"Negative x and positive y."),
            new("Design point(0,2). Code0 for axis?",0,"x=0 lies on y-axis."),
            new("Design point(2,4). Quadrant?",1,"Both positive."),
            new("Design point(4,2). Quadrant?",1,"Both positive."),
            new("Design point(4,-2). Quadrant?",4,"Positive x negative y."),
            new("Horizontal distance between(-4,-2) and(4,-2)?",8,"x difference8."),
            new("Vertical distance between(-4,-2) and(-4,2)?",4,"y difference4."),
            new("Vertical distance between(4,-2) and(4,2)?",4,"y difference4."),
            new("Horizontal distance between(-2,4) and(2,4)?",4,"x difference4."),
            new("Mirror of(-4,2) across y-axis. New x?",4,"Negate x."),
            new("Mirror of(-4,2) across y-axis. New y?",2,"y unchanged."),
            new("Mirror of(-2,4) across y-axis. New x?",2,"Negate x."),
            new("Mirror of(2,4) across x-axis. New y?",-4,"Negate y."),
            new("A design with corners(-2,-1),(-2,3),(2,3),(2,-1) has width?",4,"2-(-2)=4."),
            new("Same rectangle height?",4,"3-(-1)=4."),
            new("Same rectangle perimeter?",16,"2(4+4)=16."),
            new("Same rectangle area in square units?",16,"4×4=16."),
            new("To connect vertices in source-described order of a closed drawing, final point should repeat first? Enter1 yes,0 no.",1,"Repeating start closes the polyline."),
            new("If a plot uses x values-4 to4, total x span?",8,"4-(-4)=8."),
            new("Point(-3,0) lies on which code? Enter0 for axis,1-4 quadrants.",0,"y=0 on x-axis."),
            new("Point(0,-2) lies on which code? Enter0 for axis,1-4 quadrants.",0,"x=0 on y-axis.")
        ],
        [L0719FirstPartyPictureLogic] =
        [
            new("Coordinates(3,-2) place a point in quadrantIV. 1=yes,0=no.",1,"Positive x negative y."),
            new("Coordinates(-3,2) place a point in quadrantI. 1=yes,0=no.",0,"Negative x positive y isII."),
            new("Points(-4,2),(4,2) reflect across y-axis. 1=yes,0=no.",1,"Opposite x same y."),
            new("Points(-2,4),(2,4) reflect across y-axis. 1=yes,0=no.",1,"Symmetric."),
            new("To mirror a point across y-axis, negate its y coordinate and keep x. 1=yes,0=no.",0,"Negate x."),
            new("To mirror across x-axis, negate y while preserving x. 1=yes,0=no.",1,"Reflection across horizontal axis."),
            new("A point with x=0 belongs to one of four quadrants. 1=yes,0=no.",0,"On y-axis."),
            new("A point with y=0 lies on x-axis. 1=yes,0=no.",1,"Horizontal axis."),
            new("If a drawing app connects points in entered order, changing order may change the picture. 1=yes,0=no.",1,"Polyline path depends on sequence."),
            new("A coordinate-picture application should accurately honor each entered ordered pair rather than guessing intent. 1=yes,0=no.",1,"Precision goal."),
            new("The original 7.19 is an optional creative coordinate plotting capstone. 1=yes,0=no.",1,"Teacher lesson narrative."),
            new("The original 7.19 archive includes a standalone practice.html with a source-keyed numeric answer. 1=yes,0=no.",0,"No standalone practice."),
            new("The original 7.19 has a numbered original Cool-down question. 1=yes,0=no.",0,"Only activities19.1 and19.2."),
            new("A student-made cat picture can be automatically certified identical to source art solely by checking rectangle perimeter. 1=yes,0=no.",0,"Teacher/artifact comparison required."),
            new("An independently authored rectangle calculation is different from the original Cat Pictures artwork activity. 1=yes,0=no.",1,"Not a publisher image key."),
            new("A graph with x/y axes not labeled can still be mathematically evaluated without considering scale. 1=yes,0=no.",0,"Scale matters."),
            new("The drawn order of points can create line segments crossing multiple quadrants. 1=yes,0=no.",1,"Lines connect across axes."),
            new("A valid first-party Cool-down can be independent from a protected or absent publisher question and use teacher scoring. 1=yes,0=no.",1,"Owner authorized."),
            new("Mirror symmetry can be checked by opposite x coordinates and matching y coordinates. 1=yes,0=no.",1,"Across y-axis.")
        ],
        [L0801ClassifyData] =
        [
            new("Favorite breakfast food produces1=categorical,2=numerical. Enter code.",1,"Foods are category labels."),
            new("How students got to school produces1=categorical,2=numerical. Enter code.",1,"Walking, bus, etc are categories."),
            new("Number of different teachers for a student produces1=categorical,2=numerical. Enter code.",2,"A count is numerical."),
            new("Last item eaten or drunk produces1=categorical,2=numerical. Enter code.",1,"Item names are categories."),
            new("Minutes from waking to leaving produces1=categorical,2=numerical. Enter code.",2,"Measured duration in minutes."),
            new("Students' heights in centimeters produce1=categorical,2=numerical. Enter code.",2,"Quantitative length."),
            new("Favorite sport produces1=categorical,2=numerical. Enter code.",1,"Sport labels."),
            new("Number of siblings produces1=categorical,2=numerical. Enter code.",2,"Quantitative count."),
            new("Lunch preference (rice or pasta) produces1=categorical,2=numerical. Enter code.",1,"Label not amount."),
            new("Distance from home to school in kilometers produces1=categorical,2=numerical. Enter code.",2,"Measured numerical distance."),
            new("Student ID number produces1=categorical identifier,2=quantitative measurement. Enter code.",1,"Digits are identifiers, not magnitudes."),
            new("A class of10 surveyed students should show how many total dots on a one-dot-per-student plot?",10,"One dot per observation."),
            new("Independent fictional travel data5,7,8,10,10,12,12,12,15,18min. How many observations?",10,"Ten values."),
            new("Same independent data: frequency for12minutes?",3,"12 occurs3 times."),
            new("Same independent data: frequency for10minutes?",2,"10 occurs twice."),
            new("Same independent data: smallest commute duration in MINUTES?",5,"Minimum5."),
            new("Same independent data: largest commute duration in MINUTES?",18,"Maximum18."),
            new("Same independent data: range in MINUTES?",13,"18-5=13."),
            new("Triangle DEF with D(-4,-4),E(-2,-4),F(-3,-1). Horizontal base length?",2,"|-2-(-4)|=2."),
            new("Same triangle vertical altitude to base y=-4 from F y=-1?",3,"-1-(-4)=3."),
            new("Same triangle area in SQUARE UNITS?",3,"(2×3)/2=3.")
        ],
        [L0801DataReasoning] =
        [
            new("Favorite breakfast produces categorical responses such as eggs or oatmeal. 1=yes,0=no.",1,"Categories."),
            new("Number of teachers produces numerical responses such as4 or5. 1=yes,0=no.",1,"Counts."),
            new("A dot plot of ten individual commute times should have ten dots if every response is included once. 1=yes,0=no.",1,"One dot per student."),
            new("Numerical commute time observations should be labeled with minutes. 1=yes,0=no.",1,"Measurement units."),
            new("If a plotted dot represents a student and the title says ten students, a count of eight dots is complete. 1=yes,0=no.",0,"Two responses missing."),
            new("Favorite sport is more appropriately described using categories than arithmetic averages. 1=yes,0=no.",1,"Names not quantitative."),
            new("Categorical data can always be meaningfully averaged numerically. 1=yes,0=no.",0,"No numeric magnitude."),
            new("A dot plot of numerical values stacks dots at equal values. 1=yes,0=no.",1,"Frequency stacking."),
            new("A numerical survey variable should identify units when measuring durations or lengths. 1=yes,0=no.",1,"E.g. minutes, centimeters."),
            new("The source graph with a school-commute survey needs teacher checking of the original image and dot count. 1=yes,0=no.",1,"Image-specific assessment."),
            new("A child answering 'walked' for school transport gives categorical data. 1=yes,0=no.",1,"Method category."),
            new("A child answering '12 minutes' to readiness time gives numeric data. 1=yes,0=no.",1,"Time measurement."),
            new("A triangle with horizontal base2 and perpendicular height3 has area3 square units. 1=yes,0=no.",1,"2×3÷2."),
            new("The area of triangle DEF is6 because base2×height3 without halving. 1=yes,0=no.",0,"Area3."),
            new("A dot plot should preserve how many responses each distinct numeric value received. 1=yes,0=no.",1,"Frequency information."),
            new("Numerical data may be either integer counts or noninteger measurements. 1=yes,0=no.",1,"Both numerical."),
            new("The teacher can assess open-ended student-created survey questions beyond an exact-integer grader. 1=yes,0=no.",1,"Rubric needed.")
        ],
        [L0802StatisticalClassifications] =
        [
            new("Sixth-grade students asked what grade they are in: statistical? Enter1 yes,0 no.",0,"All responses sixth grade, no expected variability."),
            new("Ice-cream question A: How far ARE WE from the ice cream shop now? Statistical? Enter1 yes,0 no.",0,"Single fixed distance."),
            new("Ice-cream question B: Most popular flavor this week? Statistical? Enter1 yes,0 no.",1,"Different orders supply variable responses."),
            new("Ice-cream question C: What does a group of four people typically spend? Statistical? Enter1 yes,0 no.",1,"Different groups spend differently."),
            new("Ice-cream question D: Do kids usually prefer a cup or cone? Statistical? Enter1 yes,0 no.",1,"Preferences vary."),
            new("Ice-cream question E: Number of toppings available right now? Statistical? Enter1 yes,0 no.",0,"A fixed shop count."),
            new("School question A: most popular lunch choice? Statistical? Enter1 yes,0 no.",1,"Lunch choices vary."),
            new("School question B: What school do these students attend, given one named school? Statistical? Enter1 yes,0 no.",0,"All from same school."),
            new("School question C: How many math teachers work in the school? Statistical? Enter1 yes,0 no.",0,"Single school count."),
            new("School question D: Common age of teachers? Statistical? Enter1 yes,0 no.",1,"Teacher ages differ."),
            new("School question E: Typical hours of sleep students get? Statistical? Enter1 yes,0 no.",1,"Sleep times vary."),
            new("School question F: Usually travel to school by which method? Statistical? Enter1 yes,0 no.",1,"Modes vary."),
            new("Height of many female athletes numerical(2) or categorical(1)? Enter code.",2,"Height measured quantitatively."),
            new("School adults football fan yes/no data numerical(2) or categorical(1)? Enter code.",1,"Response category."),
            new("Driver red-light waiting time in seconds numerical(2) or categorical(1)? Enter code.",2,"Measured seconds."),
            new("What is the numerical y-scale interval in halves if each tick is1/6 unit? Enter number of TWELFTHS?",2,"1/6=2/12."),
            new("Water bottle between1 and1.5 quarts. Is4/3 a possible amount? Enter1 yes,0 no.",1,"4/3≈1.333 in interval."),
            new("Water bottle between1 and1.5 quarts. Is5/4 possible? Enter1 yes,0 no.",1,"1.25 in interval."),
            new("Water bottle between1 and1.5 quarts. Is5/3 possible? Enter1 yes,0 no.",0,"1.667 exceeds1.5."),
            new("Water bottle between1 and1.5 quarts. Is3/4 possible? Enter1 yes,0 no.",0,"0.75 below1."),
            new("Absolute value of-17?",17,"Distance17 from zero."),
            new("Absolute value of-18?",18,"Distance18 from zero."),
            new("Among-18,17,18,19,20, which least?",-18,"Only negative."),
            new("For point(-4/3,2/3), quadrant1-4?",2,"Negative x positive y."),
            new("With data x from-40 to20, choose tick5 or tick100? Enter5 or100.",5,"Five unit scale fits increments and range.")
        ],
        [L0802StatsLogic] =
        [
            new("A statistical question anticipates varying responses from the group being studied. 1=yes,0=no.",1,"Variability central."),
            new("What grade are students in if all are known sixth graders? Statistical. 1=yes,0=no.",0,"No variability expected."),
            new("What grade do students in a school attend across grades6-8? Statistical. 1=yes,0=no.",1,"Grade varies."),
            new("Favorite ice cream flavor across customers is statistical. 1=yes,0=no.",1,"Different flavors."),
            new("How many toppings are on a fixed menu right now is statistical because counts are numbers. 1=yes,0=no.",0,"Number alone not variability."),
            new("The question of a typical wait at a red light requires data from multiple observed waits. 1=yes,0=no.",1,"Durations vary."),
            new("Does each adult like football? As survey of adults it can vary categorically. 1=yes,0=no.",1,"Yes/no variation."),
            new("To study athletes' heights, use a numeric length unit such as centimeters. 1=yes,0=no.",1,"Units specified."),
            new("To study preferences, category labels are valid data even though they are not numbers. 1=yes,0=no.",1,"Categorical variation."),
            new("A statistical question always must have numerical responses. 1=yes,0=no.",0,"May be categorical."),
            new("Water amount w between1 and1.5 means1<w<1.5. 1=yes,0=no.",1,"Strict interval."),
            new("w=1 or w=1.5 fits 1<w<1.5. 1=yes,0=no.",0,"Endpoints excluded."),
            new("w=4/3 fits 1<w<1.5. 1=yes,0=no.",1,"≈1.333."),
            new("w=5/4 fits1<w<1.5. 1=yes,0=no.",1,"1.25."),
            new("Order |-17|,|-18|,-18,|19|,20 from least to greatest as -18,17,18,19,20. 1=yes,0=no.",1,"Absolute values positive."),
            new("When graphing fractional third units, a1/6 unit tick can locate1/6 and thirds precisely. 1=yes,0=no.",1,"2 sixths per third."),
            new("A coordinate axis is properly scaled only if x and y use identical units and increments. 1=yes,0=no.",0,"Can differ appropriately."),
            new("A response of12 minutes to wait is numeric, while a yes/no fan response is categorical. 1=yes,0=no.",1,"Different variable types.")
        ],
        [L0803Frequency] =
        [
            new("Basket attempts data1,2,1,3,1,4,4,3,1,2,5,2. Number of students?",12,"Twelve observations."),
            new("Same basket attempts data. Frequency of1 attempt?",4,"Four1s."),
            new("Same basket attempts data. Frequency of2 attempts?",3,"Three2s."),
            new("Same basket attempts data. Frequency of3 attempts?",2,"Two3s."),
            new("Same basket attempts data. Frequency of4 attempts?",2,"Two4s."),
            new("Same basket attempts data. Frequency of5 attempts?",1,"One5."),
            new("Total dots from frequencies4,3,2,2,1?",12,"4+3+2+2+1=12."),
            new("Most frequent basket-attempt count?",1,"Mode1."),
            new("Largest basket-attempt count?",5,"Max5."),
            new("Smallest basket-attempt count?",1,"Min1."),
            new("Basket attempts range (max minus min)?",4,"5-1=4."),
            new("Number of basket-attempt observations requiring at most2 attempts?",7,"4+3=7."),
            new("Number of basket-attempt observations requiring at least3 attempts?",5,"2+2+1=5."),
            new("Is class size measured numerically? Enter1 yes,0 no.",1,"Count per class."),
            new("Are car colors numerical dot-plot data? Enter1 yes,0 no.",0,"Categories."),
            new("Are favorite sports numerical dot-plot data? Enter1 yes,0 no.",0,"Categories."),
            new("Are newborn baby birth weights numerical measurements? Enter1 yes,0 no.",1,"Quantitative mass."),
            new("Are goals scored per soccer match numerical data? Enter1 yes,0 no.",1,"Count per game."),
            new("Solve9v=1. v in NINTHS?",1,"v=1/9."),
            new("Solve1.37w=0. w?",0,"w=0."),
            new("Solve1=7/10 x. x in SEVENTHS?",10,"x=10/7."),
            new("Solve12.1=12.1+y. y?",0,"Subtract12.1."),
            new("Solve3/5+z=1. z in FIFTHS?",2,"z=2/5."),
            new("Fraction2/5 divided by2, answer in FIFTHS?",1,"1/5."),
            new("Fraction2/5 divided by5, answer in TWENTY-FIFTHS?",2,"2/25."),
            new("Two divided by2/5 equals?",5,"2×5/2=5."),
            new("Five divided by2/5, answer in HALVES?",25,"5×5/2=25/2."),
            new("Triangle A right base5 height5, area in HALVES of square unit?",25,"25/2."),
            new("Triangle B base4 height3, area in square units?",6,"4×3÷2=6."),
            new("Triangle C base4 height4, area in square units?",8,"4×4÷2=8.")
        ],
        [L0803PlotLogic] =
        [
            new("One dot in a dot plot conventionally represents one observation. 1=yes,0=no.",1,"Count frequency."),
            new("The basket attempts example has four students with1 attempt. 1=yes,0=no.",1,"Four1 entries."),
            new("The basket attempts example has five students with2 attempts. 1=yes,0=no.",0,"Three2 entries."),
            new("The basket attempt value3 has frequency2. 1=yes,0=no.",1,"Two3 entries."),
            new("Dot plots of numerical data can stack repeated values as multiple dots. 1=yes,0=no.",1,"Displays frequency."),
            new("A soccer-goal count across games is suitable for a numerical dot plot. 1=yes,0=no.",1,"Goals integer counts."),
            new("Favorite sport labels are better shown by categorical bar chart, not numerical dot plot. 1=yes,0=no.",1,"Categories distinct."),
            new("Car colors are quantitative numbers because paint colors have names. 1=yes,0=no.",0,"Categorical labels."),
            new("Weights of newborn babies measured in kilograms can be numerical dot-plot data. 1=yes,0=no.",1,"Measured values."),
            new("The original 20inch estimation dot plot can be given an exact class count without inspecting its picture. 1=yes,0=no.",0,"Source image count must be observed."),
            new("A frequency table can be transformed to a dot plot using one dot per observation. 1=yes,0=no.",1,"Equivalent summary."),
            new("The total of all frequencies should equal sample size. 1=yes,0=no.",1,"Every observation assigned."),
            new("Basket attempts frequency totals4+3+2+2+1=12. 1=yes,0=no.",1,"Twelve students."),
            new("The basket attempt data has range5-1=4. 1=yes,0=no.",1,"Max-min."),
            new("Triangle area with base5 height5 equals25 square units. 1=yes,0=no.",0,"Half product,12.5."),
            new("Triangle B base4 height3 area6. 1=yes,0=no.",1,"4×3/2."),
            new("Two divided by2/5 equals5. 1=yes,0=no.",1,"Multiply reciprocal."),
            new("The student must label dot plots with the numeric variable and units where applicable. 1=yes,0=no.",1,"Meaningful presentation.")
        ],
        [L0804PlotValues] =
        [
            new("Homework dot plots A14..21,B11..20,C16..20. Lowest typical homework hours grade plot? Enter1 A,2 B,3 C.",2,"Plot B has lower center."),
            new("Source homework plots: narrowest spread and tenth-grade plot? Enter1 A,2 B,3 C.",3,"C ranges16..20, narrower."),
            new("Which source homework plot represents eighth grade? Enter1 A,2 B,3 C.",1,"B sixth, C tenth, A eighth."),
            new("Homework plot A runs hours14..21. Range in HOURS?",7,"21-14=7."),
            new("Homework plot B runs hours11..20. Range in HOURS?",9,"20-11=9."),
            new("Homework plot C runs hours16..20. Range in HOURS?",4,"20-16=4."),
            new("Homework plot A counts1,1,2,3,3,4,3,3. Sample size?",20,"Sum20."),
            new("Homework plot B counts1,0,3,2,2,4,3,3,1,1. Sample size?",20,"Sum20."),
            new("Homework plot C counts2,6,9,2,1. Sample size?",20,"Sum20."),
            new("Homework plot C most frequent time in HOURS?",18,"9 dots above18."),
            new("Homework plot C frequency of17hours?",6,"6 dots above17."),
            new("Homework plot A most frequent time in HOURS?",19,"4 dots above19."),
            new("Homework plot B most frequent time in HOURS?",16,"4 dots above16."),
            new("Animated movie G is most plausibly age plot A,B,C. Enter1 A,2 B,3 C.",2,"B has many young children."),
            new("Which movie age plot is centered near30years? Enter1 A,2 B,3 C.",3,"C mostly21–39, around30."),
            new("Movie A is mostly adults near40 years. Closest typical age among 8,30,41? Enter8,30 or41.",41,"A mode cluster late thirties–forties."),
            new("Independent Mai games points8,9,9,10,11,12,13,14,14,20. Number of games?",10,"Ten observations."),
            new("Same Mai example: number of games in8–14 points?",9,"Nine within range."),
            new("Same Mai example: number of games >18 points?",1,"Only20."),
            new("3.727+1.384 in THOUSANDTHS?",5111,"5.111."),
            new("3.727-1.384 in THOUSANDTHS?",2343,"2.343."),
            new("5.01×4.8 in THOUSANDTHS?",24048,"24.048."),
            new("5.01÷4.8 in HUNDRED-THOUSANDTHS?",104375,"1.04375.")
        ],
        [L0804InterpretPlot] =
        [
            new("Plot B homework data have lower typical hours than plots A and C. 1=yes,0=no.",1,"More hours around15–17."),
            new("Plot C homework data have less spread than plot A. 1=yes,0=no.",1,"C range4, A range7."),
            new("The tenth-grade plot is C because its values are more alike. 1=yes,0=no.",1,"Narrowest."),
            new("The eighth-grade plot is A and sixth-grade plot B under source summary. 1=yes,0=no.",1,"Relative centers and spreads."),
            new("A dot plot with more dots at18hours has a common value18hours. 1=yes,0=no.",1,"Frequency."),
            new("One extreme value may be an outlier relative to a cluster. 1=yes,0=no.",1,"Unusual observation."),
            new("Mai reports most games8–14 points and one game more than twice typical9, so one game can be20points. 1=yes,0=no.",1,"20>18."),
            new("A game with17points is more than twice typical9points. 1=yes,0=no.",0,"17<18."),
            new("For a G-rated animated movie, distribution with many very young attendees is plausible. 1=yes,0=no.",1,"Movie B."),
            new("A plot centered around30years is more likely movie C than movie A in the source descriptions. 1=yes,0=no.",1,"C mostly 20s–30s."),
            new("Movie A has ages primarily around forty rather than three-to-ten years. 1=yes,0=no.",1,"Source image text."),
            new("Center describes typical location of data and spread describes variation. 1=yes,0=no.",1,"Distinct statistical descriptions."),
            new("Two plots can have similar centers but different spreads. 1=yes,0=no.",1,"Different variability."),
            new("A claim of an exact image count must be based on the actual original dot plot, not a guessed figure. 1=yes,0=no.",1,"Teacher-verifiable source."),
            new("The class with larger range necessarily has larger typical value. 1=yes,0=no.",0,"Range vs center different."),
            new("3.727+1.384=5.111. 1=yes,0=no.",1,"Decimal addition."),
            new("5.01×4.8=24.048. 1=yes,0=no.",1,"Multiplication."),
            new("5.01÷4.8=1.04375. 1=yes,0=no.",1,"Division."),
            new("A frequency graph should show both scale and a description of measurement units. 1=yes,0=no.",1,"Context."),
            new("A source student-created plot with one outlier needs teacher review, not just one keyed numeral. 1=yes,0=no.",1,"Open-ended drawing.")
        ],
        [L0805DotPlotValues] =
        [
            new("Source dotplot A has most data 7–9 and likely what? Enter1=ages,2=school-night sleep,3=non-school sleep.",2,"A typical7–9hours school sleep."),
            new("Source dotplot B has wider sleep values5–12. Enter1=ages,2=school-night sleep,3=non-school sleep.",3,"B non-school sleep."),
            new("Source dotplot C has ages10–12. Enter1=ages,2=school-night sleep,3=non-school sleep.",1,"C ages."),
            new("Source plot A frequencies5:1,6:1,7:3,8:3,9:2. Total observations?",10,"1+1+3+3+2=10."),
            new("Source plot B frequencies5:1,7:1,8:1,9:3,10:2,11:1,12:1. Total observations?",10,"Sum10."),
            new("Source plot C frequencies10:1,11:3,12:6. Total observations?",10,"1+3+6=10."),
            new("Source plot C mode?",12,"Six dots at12."),
            new("Source plot A minimum value?",5,"Smallest5."),
            new("Source plot B maximum value?",12,"Largest12."),
            new("Travel times US approximate min2max20. Range in MINUTES?",18,"20-2=18."),
            new("Travel times Canada approximate min1max30. Range MINUTES?",29,"30-1=29."),
            new("Travel times Australia approx min5max45. Range MINUTES?",40,"45-5=40."),
            new("Travel times New Zealand approx min3max24. Range MINUTES?",21,"24-3=21."),
            new("Travel times South Africa approx min5max60. Range MINUTES?",55,"60-5=55."),
            new("Which country had largest approximate range? Enter1 US,2 Canada,3 Australia,4 New Zealand,5 South Africa.",5,"South Africa range55."),
            new("Which country had smallest approximate range? Enter1 US,2 Canada,3 Australia,4 New Zealand,5 South Africa.",1,"US range18."),
            new("Which country has lower approximate median time, Canada or Australia? Enter1 Canada,2 Australia.",1,"Approx Canada7min, Australia17.5min."),
            new("Cherries cost4dollars per pound, Tyler has10dollars. Maximum amount in HALF-POUND UNITS?",5,"10/4=2.5 pounds=5 halves."),
            new("Can Tyler buy2lb cherries for10dollars? Enter1 yes,0 no.",1,"2×4=8≤10."),
            new("Can Tyler buy3lb cherries for10dollars? Enter1 yes,0 no.",0,"12>10."),
            new("Can Tyler buy negative1lb cherries? Enter1 yes,0 no.",0,"Quantity nonnegative."),
            new("Could Tyler spend10.50dollars with budget10? Enter1 yes,0 no.",0,"Cannot exceed10."),
            new("Could Tyler spend8 dollars with budget10? Enter1 yes,0 no.",1,"8 within budget."),
            new("Could Tyler spend2 dollars with budget10 if fraction of pound sold? Enter1 yes,0 no.",1,"Half pound at4/lb costs2."),
            new("Cherries1.5lb at4dollars per pound cost in CENTS?",600,"1.5×4=6dollars.")
        ],
        [L0805DotPlotLogic] =
        [
            new("Ages around10–12 fit a set of sixth-graders plausibly better than sleep hours10–12 every night. 1=yes,0=no.",1,"Original plot C ages."),
            new("A class age distribution can be tightly centered around11 or12. 1=yes,0=no.",1,"Similar age peers."),
            new("A data set with large min-to-max range has greater range than one with smaller range. 1=yes,0=no.",1,"Definition."),
            new("South Africa approximate listed commute-time range55minutes is greater than US18minutes. 1=yes,0=no.",1,"55>18."),
            new("The source's approximate United States travel times have no variability because all students are sixth graders. 1=yes,0=no.",0,"Different travel durations."),
            new("The source Canada times have approximate min1 and max30, range29minutes. 1=yes,0=no.",1,"30-1=29."),
            new("The source New Zealand range24-3=21minutes. 1=yes,0=no.",1,"21."),
            new("A country with greater range must also have greater average travel time. 1=yes,0=no.",0,"Spread and center distinct."),
            new("The source dot plots about student ages and hours of sleep share numeric labels but need different measured units. 1=yes,0=no.",1,"Years versus hours."),
            new("A 0–10 importance-of-pollution scale supports numerical rating analysis. 1=yes,0=no.",1,"Ordinal rating scale."),
            new("An exact claim that pollution rating6 is or is not center needs the original dot plot's frequency, not guessed distribution. 1=yes,0=no.",1,"Inspect figure."),
            new("Cherry cost4dollars per pound with budget10 implies0≤c≤2.5lb. 1=yes,0=no.",1,"Divide cost constraint4c≤10."),
            new("Cherry quantity c=3lb is permitted with10dollars. 1=yes,0=no.",0,"Costs12."),
            new("Cherry quantity c=2lb is permitted with10dollars. 1=yes,0=no.",1,"Costs8."),
            new("Cherry quantity c=-1lb is valid because it gives a negative cost. 1=yes,0=no.",0,"Purchases nonnegative."),
            new("For Tyler's budget, amount spent m can be between0 and10dollars inclusive. 1=yes,0=no.",1,"0≤m≤10."),
            new("The amount spent m=10.5dollars fits budget10. 1=yes,0=no.",0,"Exceeds10."),
            new("Dot plots with equal centers can have different spreads, so both should be described. 1=yes,0=no.",1,"Distribution characteristics.")
        ],
        [L0806HistogramNumerics] =
        [
            new("Source histogram A corresponds to which dot plot1–5?",3,"A shares bins with plot3."),
            new("Source histogram B corresponds to which dot plot1–5?",1,"B shows31–49 distribution plot1."),
            new("Source histogram C corresponds to which dot plot1–5?",5,"C corresponds plot5 broad0–50 bins."),
            new("Source histogram D corresponds to which dot plot1–5?",2,"D begins20–25 matching plot2."),
            new("Source histogram E corresponds to which dot plot1–5?",4,"E matches plot4 values9–36."),
            new("40 runners: 4 to less than6 minutes frequency?",1,"Given1."),
            new("40 runners: 6 to less than8 minutes frequency?",5,"Given5."),
            new("40 runners: 8 to less than10 minutes frequency?",13,"Given13."),
            new("40 runners:10 to less than12 minutes frequency?",12,"Given12."),
            new("40 runners:12 to less than14 minutes frequency?",7,"Given7."),
            new("40 runners:14 to less than16 minutes frequency?",2,"Given2."),
            new("Total frequency of run-time bins1,5,13,12,7,2?",40,"Sum40."),
            new("How many runners took less than8minutes?",6,"1+5=6."),
            new("How many runners took8 to under12minutes?",25,"13+12=25."),
            new("How many runners took at least12minutes?",9,"7+2=9."),
            new("Which run-time interval has highest frequency? Enter1 for4-6,2 for6-8,3 for8-10,4 for10-12,5 for12-14,6 for14-16.",3,"8–10 has13."),
            new("Square vertex(-2,3) with other corners(2,3),(2,-1),(-2,-1). Side length?",4,"2-(-2)=4."),
            new("Same square area in SQUARE UNITS?",16,"4×4=16."),
            new("Same square perimeter in UNITS?",16,"4×4=16."),
            new("A histogram bin from8 up to but not including10 includes time9? Enter1 yes,0 no.",1,"8≤9<10."),
            new("A histogram bin from8 up to but not including10 includes time10? Enter1 yes,0 no.",0,"Right endpoint excluded."),
            new("A 4-minute to6-minute histogram bin has width in MINUTES?",2,"6-4=2.")
        ],
        [L0806HistogramLogic] =
        [
            new("Histograms group numerical observations into value intervals. 1=yes,0=no.",1,"Bins."),
            new("Dot plots can display each individual measured value more directly than grouped histograms. 1=yes,0=no.",1,"Values vs frequency bins."),
            new("A bar for interval8 to less than10 includes10 as right endpoint. 1=yes,0=no.",0,"Upper bound excluded."),
            new("The six run-time bins have frequencies summing40. 1=yes,0=no.",1,"1+5+13+12+7+2=40."),
            new("The most frequent source run-time bin is8–10minutes. 1=yes,0=no.",1,"13."),
            new("More runners were10–12 than8–10 minutes. 1=yes,0=no.",0,"12 versus13."),
            new("The source run-time histogram counts25 students in8–12minute intervals. 1=yes,0=no.",1,"13+12=25."),
            new("Exactly13 runners finished in9minutes based only on8–10 histogram bin. 1=yes,0=no.",0,"Bin includes different times."),
            new("Using adjacent bars for a numerical histogram helps show contiguous intervals. 1=yes,0=no.",1,"Bin structure."),
            new("A histogram can be constructed for 40 runner times from the provided frequency table without knowing each exact original time. 1=yes,0=no.",1,"Bin counts known."),
            new("The source shark histogram's exact total and maximum can be asserted from the text-only prompt without its figure. 1=yes,0=no.",0,"Teacher must view chart."),
            new("A square from(-2,3),(2,3),(2,-1),(-2,-1) has side4. 1=yes,0=no.",1,"Four-unit spans."),
            new("Two histograms with same bins but different frequencies represent different distributions. 1=yes,0=no.",1,"Counts differ."),
            new("Numerical histograms and categorical bar charts are interchangeable without changing the meaning of x positions. 1=yes,0=no.",0,"Categories versus ordered intervals."),
            new("Bin choice can affect apparent shape but not the observations being summarized. 1=yes,0=no.",1,"Grouping."),
            new("A frequency table and its corresponding histogram must agree in every bin count. 1=yes,0=no.",1,"Consistency."),
            new("The first source histogram A aligns with plot3 by bin aggregates. 1=yes,0=no.",1,"A↔3."),
            new("The source histogram B aligns with plot1. 1=yes,0=no.",1,"B↔1."),
            new("The source histogram D aligns with plot2. 1=yes,0=no.",1,"D↔2.")
        ],
        [L0807HistogramCompare] =
        [
            new("Two source text-message histograms show sixth and seventh grade, which has wider approximate range? Enter6 or7.",6,"Sixth graph spans many more bins."),
            new("Original sixth-grade message graph spans approx45 to155. Approx span in MESSAGES?",110,"155-45≈110."),
            new("Original seventh-grade graph spans approx75 to125. Approx span in MESSAGES?",50,"125-75≈50."),
            new("Original first-grade? Here use sixth message range110 versus seventh50. Difference in approximate spans?",60,"110-50≈60."),
            new("Jada has d dimes worth strictly over30cents and strictly under100cents. Smallest possible d?",4,"Four dimes=40cents."),
            new("Jada d dimes with30<10d<100. Largest possible d?",9,"Nine dimes=90cents."),
            new("How many integer dime counts satisfy30<10d<100?",6,"d=4,5,6,7,8,9."),
            new("Could Jada own10dimes under strict total less than1dollar? Enter1 yes,0 no.",0,"10dimes=100cents excluded."),
            new("Could Jada own3dimes under strict total over30cents? Enter1 yes,0 no.",0,"3=30cents excluded."),
            new("Could Jada own6dimes? Enter1 yes,0 no.",1,"60cents allowed."),
            new("Rank4,7/4,1/4,0,-5/4,-3.5,-4. Greatest value?",4,"4 highest."),
            new("Same ordered list: second greatest in QUARTERS?",7,"7/4=1.75."),
            new("Same ordered list: third greatest in QUARTERS?",1,"1/4."),
            new("Same ordered list: fourth greatest?",0,"Zero."),
            new("Same list: fifth greatest in QUARTERS?",-5,"-5/4."),
            new("Same list: sixth greatest in HALVES?",-7,"-3.5=-7/2."),
            new("Same list: least value?",-4,"-4 smallest."),
            new("A histogram group of40 runners should have how many total observations across bins?",40,"Source sample size40."),
            new("A40-runner histogram with all40 in a single 9–11minute interval has how many observations outside that interval?",0,"All inside."),
            new("A comparison histogram from40 students with bins [8,9):5,[9,10):15,[10,11):15,[11,12):5. Total?",40,"5+15+15+5."),
            new("The independent example bin [9,10) has frequency?",15,"Given15."),
            new("What is the distance between3.5 and-4 on number line in HALF-UNITS?",15,"3.5-(-4)=7.5=15 halves.")
        ],
        [L0807HistogramLogic] =
        [
            new("Two histograms can have similar typical values but very different spread. 1=yes,0=no.",1,"Center and spread are distinct."),
            new("The source sixth-grade message histogram is more spread out than seventh-grade histogram. 1=yes,0=no.",1,"More extreme bins."),
            new("The source text describing 100-student histograms has inconsistent OCR-style frequency totals; exact dot counts must be confirmed from actual figure. 1=yes,0=no.",1,"Description not fully reliable."),
            new("Bin counts must sum to sample size for a verified histogram. 1=yes,0=no.",1,"Frequency consistency."),
            new("A histogram centered near10 minutes can have less variability by clustering more observations near10. 1=yes,0=no.",1,"Narrower distribution."),
            new("A histogram with all40 times in9–11min has no students outside9–11. 1=yes,0=no.",1,"No outside observations."),
            new("Jada's dimes condition is30<10d<100 cents. 1=yes,0=no.",1,"Strict bounds."),
            new("Jada can have d=3 under this strict condition. 1=yes,0=no.",0,"Exactly30 not over."),
            new("Jada can have d=9 under this strict condition. 1=yes,0=no.",1,"90cents."),
            new("Jada can have d=10 under this strict condition. 1=yes,0=no.",0,"Exactly1dollar."),
            new("Jada has six integer solutions d=4 through9. 1=yes,0=no.",1,"Six possibilities."),
            new("Seven quarters (7/4) is greater than4. 1=yes,0=no.",0,"1.75<4."),
            new("One-quarter is positive, so it is greater than0. 1=yes,0=no.",1,"0.25>0."),
            new("-3½ is greater than-5/4. 1=yes,0=no.",0,"-3.5<-1.25."),
            new("-4 is the least among -4,-3½,-5/4,0,¼,7/4,4. 1=yes,0=no.",1,"Farthest left."),
            new("A graph's apparent shape may depend on bin width choices. 1=yes,0=no.",1,"Grouping effect."),
            new("A source comparison about number of messages must label each horizontal axis in messages per week. 1=yes,0=no.",1,"Meaningful units."),
            new("Teacher should review a student's second 40-runner histogram for center, spread and bin frequency consistency. 1=yes,0=no.",1,"Open-ended graph.")
        ],
        [L0808ShapeNumbers] =
        [
            new("Original histogram summarizes how many wild bears?",143,"143 stated in prompt; individual bins image-dependent."),
            new("Independent histogram bin frequencies1,4,6,4,1. Sample size?",16,"1+4+6+4+1=16."),
            new("Independent histogram frequencies1,4,6,4,1. Highest frequency?",6,"Center bin frequency6."),
            new("Independent histogram frequencies1,4,6,4,1. Number in two outermost bins combined?",2,"1+1=2."),
            new("Independent histogram frequencies1,4,6,4,1. Total observations in three central bins?",14,"4+6+4=14."),
            new("Independent histogram bins[0,5),[5,10),[10,15),[15,20),[20,25). Middle bin index1–5?",3,"Third bin[10,15)."),
            new("Histogram frequencies4,0,0,3 over four bins. How many empty bins?",2,"Second and third."),
            new("Histogram frequencies4,0,0,3. Total observations?",7,"4+3=7."),
            new("Histogram frequencies2,3,2 over three bins. Total observations?",7,"2+3+2=7."),
            new("Compute4×1³?",4,"4."),
            new("Compute4×2³?",32,"4×8=32."),
            new("Compute4×(1/2)³ in HALVES?",1,"4×1/8=1/2."),
            new("Compute4×3³?",108,"4×27=108."),
            new("Water bottle has12ounces, which is60% full. Capacity in OUNCES?",20,"12/.6=20oz."),
            new("Same bottle full20oz, current12oz. Ounces to fill?",8,"20-12=8."),
            new("Same bottle full20oz, 40% remaining. Ounces remaining?",8,"20×.4=8."),
            new("A histogram bin[10,15) includes value14? Enter1 yes,0 no.",1,"14<15."),
            new("A histogram bin[10,15) includes15? Enter1 yes,0 no.",0,"Upper endpoint excluded."),
            new("A distribution of frequencies3,0,5 has how many nonempty bins?",2,"First and third."),
            new("Independent histogram frequencies1,4,6,4,1. Is it symmetric? Enter1 yes,0 no.",1,"Mirrored counts."),
            new("Independent frequencies1,4,6,4,1 have number of peaked bins with maximum6?",1,"One maximum bin."),
            new("In a 143-bear source sample, an arbitrary random bin frequency cannot be inferred from only143. Enter1 possible exact inference,0 no.",0,"Need source image.")
        ],
        [L0808ShapeLogic] =
        [
            new("Histogram bars summarize numerical intervals, while bar graphs often display category counts. 1=yes,0=no.",1,"Different variable types."),
            new("Histogram shape may be symmetric, skewed, clustered, gapped or have an unusual bar. 1=yes,0=no.",1,"Qualitative features."),
            new("Symmetry means approximately similar frequencies on either side of a central region. 1=yes,0=no.",1,"Mirror structure."),
            new("The independent histogram1,4,6,4,1 has symmetric bar counts. 1=yes,0=no.",1,"Mirror."),
            new("Frequencies4,0,0,3 have two empty bins. 1=yes,0=no.",1,"Gaps."),
            new("A gap is an interval containing no observed values. 1=yes,0=no.",1,"Frequency zero."),
            new("A peak is an interval with relatively many observations. 1=yes,0=no.",1,"High bar."),
            new("Data of pop album song lengths may be more nearly symmetric than schoolwide phone talk durations, though actual data are needed to be certain. 1=yes,0=no.",1,"Plausible model, not guarantee."),
            new("A histogram of students' favorite colors can be interpreted as a numerical length distribution without converting variable type. 1=yes,0=no.",0,"Categories not lengths."),
            new("A bar graph's category order is intrinsic numeric order like histogram bins. 1=yes,0=no.",0,"Categories often arbitrary."),
            new("An exact range for143 bear lengths can be calculated from sample count143 alone. 1=yes,0=no.",0,"Need observed values."),
            new("Student age distribution in one sixth-grade class is likely tightly grouped in ages. 1=yes,0=no.",1,"Students similar age."),
            new("A family restaurant with adults and children could show multiple age clusters or gaps. 1=yes,0=no.",1,"Context-dependent."),
            new("A histogram's appearance may change when bin widths change. 1=yes,0=no.",1,"Binning influences shape."),
            new("Four times(1/2)cubed equals one half. 1=yes,0=no.",1,"4/8=1/2."),
            new("Four times2 cubed equals16. 1=yes,0=no.",0,"32."),
            new("12ounces as60% full means20-ounce bottle. 1=yes,0=no.",1,"12/0.6=20."),
            new("A data set with two separated clusters necessarily has exactly one central peak. 1=yes,0=no.",0,"Can be bimodal."),
            new("Appropriate narrative can distinguish descriptive appearance from numerical certainty. 1=yes,0=no.",1,"Avoid fabricated chart readings.")
        ],
        [L0809Means] =
        [
            new("Four boxes blocks32,18,41,9. Total blocks?",100,"32+18+41+9=100."),
            new("Four boxes blocks32,18,41,9. Equal share per box?",25,"100/4=25."),
            new("Box1 has32 blocks, should end25. Number to remove?",7,"32-25=7."),
            new("Box2 has18, should end25. Number to add?",7,"25-18=7."),
            new("Box3 has41, should end25. Number to remove?",16,"41-25=16."),
            new("Box4 has9, should end25. Number to add?",16,"25-9=16."),
            new("Correct source choices for block equalization A or E: enter1 if A valid,0 otherwise.",1,"A four equal piles25."),
            new("Correct source choices for block equalization: enter1 if E valid,0 otherwise.",1,"Transfer7 from1 to2 and16 from3 to4."),
            new("Source choice D transfer7 from1 to2 and21 from3 to4 makes all boxes equal? Enter1 yes,0 no.",0,"Third20 fourth30."),
            new("Mini-golf strokes2,3,1,4,5,2,3,4,3. Number of holes?",9,"Nine greens."),
            new("Same mini-golf strokes. Total strokes?",27,"Sum27."),
            new("Same mini-golf strokes. Mean strokes per hole?",3,"27/9=3."),
            new("Library fundraising three classes2550,4975,3725 CENTS. Combined in CENTS?",11250,"2550+4975+3725=11250."),
            new("Same classes collected11250 cents. Equal share per class in CENTS?",3750,"11250/3=3750cents."),
            new("Quiz scores first3 are4,3,4; target average4 over4 quizzes. Total target points?",16,"4×4=16."),
            new("Quiz scores4,3,4,x target average4. Required last score?",5,"16-11=5."),
            new("Earthworm sample source container A has how many measured worms?",25,"Original states25 per container."),
            new("Earthworm sample source container B has how many measured worms?",25,"Original states25 per container."),
            new("Which earthworm container tends longer? Enter1 A,2 B.",1,"A has more measurements around40–60mm."),
            new("Which earthworm container has many around15mm? Enter1 A,2 B.",2,"B has many5–20mm."),
            new("If x=3, compute x²?",9,"3×3=9."),
            new("A solution to x²=16 among3,4,5 is?",4,"4²=16; negative4 also solves."),
            new("Mean of five numbers1,2,3,4,5?",3,"Sum15/5."),
            new("Mean of three numbers2,4,9?",5,"15/3=5."),
            new("Mean of four numbers5,5,5,5?",5,"20/4=5.")
        ],
        [L0809MeanLogic] =
        [
            new("Mean can be interpreted as each group's value if total were fairly redistributed across same number of groups. 1=yes,0=no.",1,"Fair share."),
            new("Redistributing100 blocks among4 boxes equally gives25 each. 1=yes,0=no.",1,"100/4."),
            new("Removing7 from32 and adding7 to18 makes both25. 1=yes,0=no.",1,"Balance pair."),
            new("Removing21 from41 and adding21 to9 makes them equal25. 1=yes,0=no.",0,"20 and30."),
            new("Removing16 from41 and adding16 to9 makes both25. 1=yes,0=no.",1,"25 and25."),
            new("The total must remain constant during pure redistribution. 1=yes,0=no.",1,"No extra blocks."),
            new("Mini-golf scores across9 holes sum27, so mean3. 1=yes,0=no.",1,"27/9=3."),
            new("A mean always has to equal one of original observed values. 1=yes,0=no.",0,"Can be fractional/non-observed."),
            new("Three classes total112.50 dollars divided equally means37.50 each. 1=yes,0=no.",1,"112.5/3=37.5."),
            new("Quiz grades4,3,4,5 average4 across four quizzes. 1=yes,0=no.",1,"16/4=4."),
            new("Quiz grades4,3,4,4 average4 across four quizzes. 1=yes,0=no.",0,"15/4=3.75."),
            new("Container A worm lengths tend longer than B in source histograms. 1=yes,0=no.",1,"A higher interval clusters."),
            new("A worm length15mm is plausible as a typical value in container B. 1=yes,0=no.",1,"Most B short."),
            new("The displayed exact times for each worm can be reconstructed from grouped source histograms. 1=yes,0=no.",0,"Only interval counts."),
            new("x=3 satisfies x²=16. 1=yes,0=no.",0,"3²=9."),
            new("x=-4 satisfies x²=16. 1=yes,0=no.",1,"(-4)²=16."),
            new("Mean equals total sum divided by number of observations. 1=yes,0=no.",1,"Formula."),
            new("If all values in dataset increase by2, mean also increases by2. 1=yes,0=no.",1,"Shift all values."),
            new("Teacher can assess a student's balancing drawing and explanations beyond numeric total. 1=yes,0=no.",1,"Rubric.")
        ],
        [L0810BalanceNumbers] =
        [
            new("Kiran walks16,11,18,12,13 minutes across5days. Sum minutes?",70,"16+11+18+12+13=70."),
            new("Kiran walks16,11,18,12,13. Mean in minutes?",14,"70/5=14."),
            new("Deviation for16min from mean14. Distance?",2,"16-14=2."),
            new("Deviation for11min from mean14. Absolute distance?",3,"14-11=3."),
            new("Deviation for18min from mean14. Absolute distance?",4,"18-14=4."),
            new("Deviation for12min from mean14. Absolute distance?",2,"14-12=2."),
            new("Deviation for13min from mean14. Absolute distance?",1,"14-13=1."),
            new("Sum distances to LEFT of mean14 for11,12,13?",6,"3+2+1=6."),
            new("Sum distances to RIGHT mean14 for16,18?",6,"2+4=6."),
            new("Noah20 points, Mai30, Clare unknown. Mean40 across3. Total score target?",120,"3×40=120."),
            new("Noah20, Mai30, mean40 across3. Clare points?",70,"120-20-30=70."),
            new("Compare -2 vs3. Enter1 first larger,0 equal,2 smaller.",2,"-2<3."),
            new("Compare |-12| vs |15|. Enter1 first larger,0 equal,2 smaller.",2,"12<15."),
            new("Compare3 vs-4. Enter1 first larger,0 equal,2 smaller.",1,"3>-4."),
            new("Compare |15| vs|-12|. Enter1 first larger,0 equal,2 smaller.",1,"15>12."),
            new("Compare7 vs-11. Enter1 first larger,0 equal,2 smaller.",1,"7>-11."),
            new("Compare -4 vs |5|. Enter1 first larger,0 equal,2 smaller.",2,"-4<5."),
            new("Compare2/3 and3/4. Which is larger? Enter1 for2/3,2 for3/4.",2,"2/3=8/12,3/4=9/12."),
            new("Rectangle has height5, width x+y. Area if x=3 and y=2?",25,"5(3+2)=25."),
            new("Rectangle height5 left width x=3 right width y=2. Left area?",15,"5×3=15."),
            new("Rectangle height5 left width3 right width2. Right area?",10,"5×2=10."),
            new("Independent set2,4,6,8,10. Mean?",6,"Sum30/5=6."),
            new("Independent set2,4,6,8,10. Sum signed deviations from mean?",0,"-4-2+0+2+4=0.")
        ],
        [L0810BalanceLogic] =
        [
            new("Mean of Kiran's16,11,18,12,13-minute times is14minutes. 1=yes,0=no.",1,"Sum70/5."),
            new("15minutes is exact mean of Kiran's walks. 1=yes,0=no.",0,"Mean14."),
            new("For Kiran's data, total distances left of mean equal distances right of mean. 1=yes,0=no.",1,"6 on each side."),
            new("Kiran's16min time is2minutes to right of mean14. 1=yes,0=no.",1,"+2."),
            new("Kiran's11min time is3minutes to left of mean14. 1=yes,0=no.",1,"-3."),
            new("Mean is a balance point because signed deviations sum zero. 1=yes,0=no.",1,"Positive and negative offsets cancel."),
            new("All values must be identical for their mean to be a balance point. 1=yes,0=no.",0,"Varied data can balance."),
            new("Noah20, Mai30 and Clare70 have mean40. 1=yes,0=no.",1,"120/3=40."),
            new("If Noah20 and Mai30, Clare40 produces mean40. 1=yes,0=no.",0,"Sum90 mean30."),
            new("2/3 is less than3/4. 1=yes,0=no.",1,"8/12<9/12."),
            new("Rectangle area height5 width x+y equals5(x+y). 1=yes,0=no.",1,"Distributive."),
            new("Rectangle area5(x+y) equals5x+5y. 1=yes,0=no.",1,"Distributive property."),
            new("Rectangle total area equals5+xy under source geometry. 1=yes,0=no.",0,"Incorrect area."),
            new("Absolute distance from mean ignores which side but signed difference records side. 1=yes,0=no.",1,"Distance versus direction."),
            new("Mean uses total of all data divided by count. 1=yes,0=no.",1,"Fair share."),
            new("A mean need not equal any original observation. 1=yes,0=no.",1,"Possible."),
            new("The values2,4,6,8,10 have mean6. 1=yes,0=no.",1,"Sum30/5."),
            new("The positive deviations from mean may exactly offset negative deviations. 1=yes,0=no.",1,"Zero sum.")
        ],
        [L0811MadValues] =
        [
            new("Han reads25,28,32,36,42pages in5days. Sum pages?",163,"25+28+32+36+42=163."),
            new("Han's mean pages in TENTHS?",326,"163/5=32.6."),
            new("Distance25pages from mean32.6 in TENTHS?",76,"|25-32.6|=7.6."),
            new("Distance28pages from mean32.6 in TENTHS?",46,"4.6."),
            new("Distance32pages from mean32.6 in TENTHS?",6,"0.6."),
            new("Distance36pages from mean32.6 in TENTHS?",34,"3.4."),
            new("Distance42pages from mean32.6 in TENTHS?",94,"9.4."),
            new("Sum absolute deviations in TENTHS?",256,"7.6+4.6+.6+3.4+9.4=25.6."),
            new("Mean absolute deviation for5 days in HUNDREDTHS?",512,"25.6/5=5.12."),
            new("Travel time mean9minutes and MAD4.2minutes. Which is typical travel time? Enter9 or42 tenths-coded as9 or42.",9,"Mean9, MAD4.2 is spread."),
            new("GroupA mean9 MAD4.2, GroupB mean9 MAD7. Which has more variability? Enter1 A,2 B.",2,"B MAD7>4.2."),
            new("An archer first mean distance1.6in, MAD1.3in, second round farther but more consistent: example mean2.4in MAD0.5in. Mean in TENTHS?",24,"Chosen valid example2.4."),
            new("Same archer example second MAD0.5in in TENTHS?",5,"Smaller than1.3."),
            new("Independent data3,4,5,6,7. Mean?",5,"25/5=5."),
            new("Independent data3,4,5,6,7. Sum absolute distances from mean?",6,"2+1+0+1+2=6."),
            new("Independent data3,4,5,6,7. MAD in TENTHS?",12,"6/5=1.2."),
            new("Independent data5,5,5,5,5. Mean?",5,"All five equal5."),
            new("Independent data5,5,5,5,5. MAD?",0,"No deviation."),
            new("Independent data0,0,9,9. Mean in HALVES?",9,"9/2=4.5."),
            new("Independent data0,0,9,9. MAD in HALVES?",9,"4.5."),
            new("Source Han data max pages?",42,"42 maximum."),
            new("Source Han data min pages?",25,"25 minimum."),
            new("Source Han data range pages?",17,"42-25=17.")
        ],
        [L0811MadReasoning] =
        [
            new("Mean absolute deviation is the mean of distances of each value from their mean. 1=yes,0=no.",1,"Definition."),
            new("MAD may be negative for mostly negative numbers. 1=yes,0=no.",0,"Absolute distances nonnegative."),
            new("Han's page counts25,28,32,36,42 have mean32.6. 1=yes,0=no.",1,"163/5=32.6."),
            new("Han's five days have MAD5.12 pages. 1=yes,0=no.",1,"25.6/5=5.12."),
            new("Han's page mean is exactly30. 1=yes,0=no.",0,"32.6."),
            new("Travel-time mean9min describes typical commute more directly than MAD4.2min. 1=yes,0=no.",1,"Mean center."),
            new("MAD4.2min describes typical size of variation from the mean. 1=yes,0=no.",1,"Spread."),
            new("Two groups with equal mean9 but MAD7 and4.2 have equally spread times. 1=yes,0=no.",0,"MAD7 more spread."),
            new("If mean grows but MAD shrinks, values may be farther from zero but more consistent around their own mean. 1=yes,0=no.",1,"Mean location and variation differ."),
            new("Archer farther on second round means second mean distance can be above1.6 inches. 1=yes,0=no.",1,"Greater average distance."),
            new("Archer more consistent second round means second MAD may be below1.3 inches. 1=yes,0=no.",1,"Smaller spread."),
            new("An archer round with mean2.4in and MAD0.5in is a possible valid example. 1=yes,0=no.",1,"Meets two inequalities."),
            new("If every value in a dataset is equal, MAD equals zero. 1=yes,0=no.",1,"No deviations."),
            new("Mean and MAD always represent the same number. 1=yes,0=no.",0,"Center vs spread."),
            new("Adding10 to every observation increases mean by10 but leaves MAD unchanged. 1=yes,0=no.",1,"All deviations same."),
            new("Multiplying every observed quantity by2 doubles MAD. 1=yes,0=no.",1,"Scaled distances."),
            new("A labeled dot plot can show center and spread simultaneously. 1=yes,0=no.",1,"Visual context."),
            new("The student's open-ended second-dot-plot design needs teacher review. 1=yes,0=no.",1,"Many valid solutions.")
        ],
        [L0812CompareMeans] =
        [
            new("US travel times2,2,6,6,7,7,7,11,17,20. Total in minutes?",85,"Sum85."),
            new("US data mean in TENTHS MINUTES?",85,"85/10=8.5min."),
            new("US data MAD in TENTHS MINUTES?",45,"4.5min."),
            new("Australia times5,7,9,15,15,20,20,20,25,45. Total minutes?",181,"Sum181."),
            new("Australia mean in TENTHS minutes?",181,"181/10=18.1."),
            new("Australia MAD in TENTHS minutes?",79,"7.9min."),
            new("South Africa times5,5,10,10,15,15,30,40,45,60. Total minutes?",235,"Sum235."),
            new("South Africa mean in TENTHS minutes?",235,"23.5min."),
            new("South Africa MAD in TENTHS minutes?",162,"16.2min."),
            new("Which has greater MAD US or Australia? Enter1 US,2 Australia.",2,"7.9>4.5."),
            new("Which has lower mean South Africa or Australia? Enter1 SA,2 Australia.",2,"18.1<23.5."),
            new("Which has lower MAD SA or Australia? Enter1 SA,2 Australia.",2,"7.9<16.2."),
            new("US mean8.5, Australia18.1. Difference in TENTHS MINUTES?",96,"18.1-8.5=9.6."),
            new("SA mean23.5, Australia18.1. Difference in TENTHS MINUTES?",54,"23.5-18.1=5.4."),
            new("Sunnyside mean score50, MAD4; Shadyside mean60, MAD15. Which is MORE consistent? Enter1 Sunnyside,2 Shadyside.",1,"Smaller MAD4."),
            new("Which basketball team has higher mean points? Enter1 Sunnyside,2 Shadyside.",2,"60>50."),
            new("Sunnyside MAD4, Shadyside15. Difference in MAD points?",11,"15-4=11."),
            new("Rectangle side lengths a=4,b=6. Perimeter?",20,"2a+2b=20."),
            new("Compare -2 and-8. Which is GREATER? Enter -2 or -8.",-2,"-2 closer zero."),
            new("Elephant seal expected weight5500lbs,5 seals exactly same? Enter1 guaranteed,0 no.",0,"Population varies."),
            new("Independent data2,2,2,2 mean?",2,"All values2."),
            new("Independent data2,2,2,2 MAD?",0,"No variation."),
            new("Independent data0,0,10,10 mean?",5,"20/4=5."),
            new("Independent data0,0,10,10 MAD?",5,"Average absolute deviation5.")
        ],
        [L0812CompareLogic] =
        [
            new("US dataset mean8.5 and MAD4.5 based on given ten values. 1=yes,0=no.",1,"Recomputed."),
            new("Australia dataset mean18.1 MAD7.9. 1=yes,0=no.",1,"Recomputed."),
            new("Australia's MAD is significantly greater than US MAD in source option D. 1=yes,0=no.",1,"7.9>4.5."),
            new("South Africa data mean23.5 MAD16.2. 1=yes,0=no.",1,"Source given times."),
            new("Australia's mean is smaller than South Africa mean. 1=yes,0=no.",1,"18.1<23.5."),
            new("Australia's MAD is smaller than South Africa MAD. 1=yes,0=no.",1,"7.9<16.2."),
            new("A smaller MAD means commuters are more consistent relative to their group's mean. 1=yes,0=no.",1,"Less average deviation."),
            new("A smaller mean means commuters typically have shorter travel times. 1=yes,0=no.",1,"Lower average."),
            new("Sunnyside50 mean4 MAD is more consistent than Shadyside60 mean15 MAD. 1=yes,0=no.",1,"Smaller spread."),
            new("Shadyside has higher mean points but greater variability. 1=yes,0=no.",1,"60>50,15>4."),
            new("Mean and MAD provide the same type of information, so MAD alone gives typical score. 1=yes,0=no.",0,"MAD measures spread."),
            new("Rectangle perimeter a+a+b+b and2a+2b are equivalent expressions. 1=yes,0=no.",1,"Combine like terms."),
            new("Numbers -3,-5,-7 lie strictly between-8 and-2. 1=yes,0=no.",1,"All within open interval."),
            new("-9 lies between-8 and-2. 1=yes,0=no.",0,"Outside."),
            new("Five elephant seals all weigh exactly5500lb simply because adults generally average about5500lb. 1=yes,0=no.",0,"Variation."),
            new("Statistical variability means individual values can differ around a reported typical value. 1=yes,0=no.",1,"Data distribution."),
            new("Shifting all data by10 increases mean10 and leaves MAD same. 1=yes,0=no.",1,"Offsets unchanged."),
            new("Teacher assesses source graphs and explanations of comparability. 1=yes,0=no.",1,"Multiple valid responses.")
        ],
        [L0813Medians] =
        [
            new("Game scores130,150,120,170,130,120,160,160,190,140. Count observations?",10,"Ten rounds."),
            new("Same scores sorted. Fifth smallest score?",140,"120,120,130,130,140,150,160,160,170,190."),
            new("Same scores sorted. Sixth smallest score?",150,"Sixth150."),
            new("Same scores median?",145,"(140+150)/2=145, source choiceB."),
            new("Same scores smallest?",120,"Lowest120."),
            new("Same scores largest?",190,"Highest190."),
            new("Same scores range?",70,"190-120=70."),
            new("Same scores total?",1470,"130+150+120+170+130+120+160+160+190+140=1470."),
            new("Same scores mean?",147,"1470/10=147."),
            new("Clare has exactly12 students with higher scores and12 lower; number of students including Clare?",25,"12+1+12."),
            new("Clare with12 above and12 below occupies which rank from LOWEST?",13,"Middle13th."),
            new("Independent five-value data5,7,12,12,14. Median?",12,"Middle third12."),
            new("Independent five-value data5,7,12,12,14. Mean?",10,"Sum50/5."),
            new("Independent five-value data5,7,12,12,14. Range?",9,"14-5=9."),
            new("Independent data1,3,5,9. Median?",4,"Average middle3 and5."),
            new("Independent data1,3,5,9. Mean in HALVES?",9,"18/4=4.5."),
            new("Source dot plot median candidates6,12,13,15. Which is largest listed median?",15,"Largest15."),
            new("Source dot plot candidates6,12,13,15. Which smallest?",6,"Smallest6."),
            new("Five-value set2,8,12,13,15. Median?",12,"Third in ordered list."),
            new("Five-value set2,8,12,13,15. Mean?",10,"Sum50/5."),
            new("Ten pupils sleep example independent6,7,7,7,8,8,8,9,9,10. Median in HALVES?",16,"(8+8)/2=8=16halves."),
            new("Median of odd sample1,5,7?",5,"Middle5."),
            new("Median of even sample1,5,7,9?",6,"(5+7)/2=6.")
        ],
        [L0813MedianLogic] =
        [
            new("To calculate median, first order numerical data from least to greatest. 1=yes,0=no.",1,"Ordered center."),
            new("Ten game scores have median145 after averaging middle140 and150. 1=yes,0=no.",1,"Source choice B."),
            new("The median of10 scores is always the fifth smallest observation without averaging. 1=yes,0=no.",0,"Average fifth and sixth."),
            new("If exactly12 students scored higher and12 lower than Clare, Clare is median in25 students. 1=yes,0=no.",1,"Middle13th."),
            new("An odd sample of five has median third ordered observation. 1=yes,0=no.",1,"Position3."),
            new("An even sample of four has median average of second and third ordered values. 1=yes,0=no.",1,"Two middle."),
            new("Median must equal the mean for any numerical data. 1=yes,0=no.",0,"Different measures."),
            new("Game scores source mean147 and median145 differ. 1=yes,0=no.",1,"Sum1470/10 vs median145."),
            new("Extreme unusually large values can change mean more strongly than median. 1=yes,0=no.",1,"Median resistant."),
            new("The median may be a value not actually observed in even datasets. 1=yes,0=no.",1,"Average middles."),
            new("Five-value independent set5,7,12,12,14 has median12 but mean10. 1=yes,0=no.",1,"Middle12 sum50."),
            new("A median of an odd data set is calculated without sorting if points are given in random order. 1=yes,0=no.",0,"Must identify middle by rank."),
            new("Dot plot heights represent frequencies needed to determine ordered middle observations. 1=yes,0=no.",1,"Cumulative counts."),
            new("A statistical question about number of siblings can use median. 1=yes,0=no.",1,"Numerical counts."),
            new("A favorite lunch category cannot meaningfully be median as a numerical quantity. 1=yes,0=no.",1,"No numerical order."),
            new("The original wild-bear histogram exact largest bear weight cannot be recovered from given sample size143 alone. 1=yes,0=no.",1,"Bin limits needed."),
            new("If a histogram bins measurements, exact individual median may not be determined without within-bin values. 1=yes,0=no.",1,"Approximate interval only."),
            new("A dataset with equal numbers above and below middle observation may use that observation as median. 1=yes,0=no.",1,"Middle split.")
        ],
        [L0814CenterValues] =
        [
            new("Priya's first data1,8,6,2,4. Sum?",21,"1+8+6+2+4=21."),
            new("Priya's first data1,8,6,2,4. Mean in FIFTHS?",21,"21/5=4.2."),
            new("Priya's first data1,8,6,2,4. Median?",4,"Ordered1,2,4,6,8."),
            new("Replace8 with18; revised sum?",31,"1+18+6+2+4=31."),
            new("Replace8 with18; revised mean in FIFTHS?",31,"31/5=6.2."),
            new("Replace8 with18; revised median?",4,"Ordered1,2,4,6,18."),
            new("Mean increase from4.2 to6.2 in WHOLE units?",2,"Increase10/5=2."),
            new("Original choice for mean increase and median unchanged: A=1 B=2 C=3 D=4. Enter number.",2,"Option B."),
            new("Han homework scores100,100,100,100,95,100,90,100,0. Number of scores?",9,"Nine assignments."),
            new("Han's nine homework scores. Sum?",785,"6×100+95+90+0=785."),
            new("Han's homework mean in NINTHS?",785,"785/9."),
            new("Han's homework median?",100,"Fifth score in sorted0,90,95,100,100,100,100,100,100."),
            new("Han scored100 how many times?",6,"Six100s."),
            new("Han's lowest homework score?",0,"Zero."),
            new("Han's highest homework score?",100,"One hundred."),
            new("Independent data4,5,5,6,100. Median?",5,"Third5."),
            new("Independent data4,5,5,6,100. Sum?",120,"120."),
            new("Independent data4,5,5,6,100. Mean?",24,"120/5."),
            new("Independent comparison4,5,5,6,100: mean exceeds median by?",19,"24-5=19."),
            new("Coordinate A(1,0). Quadrant code0 for axis?",0,"y=0 on x axis."),
            new("Coordinate B(0,0.5). Quadrant code0 for axis?",0,"x=0 on y axis."),
            new("Coordinate C(4,3.5). Quadrant 1–4?",1,"Both positive."),
            new("Coordinate D(1.5,0.5). Quadrant 1–4?",1,"Both positive.")
        ],
        [L0814CenterLogic] =
        [
            new("Replacing8 with18 in five values changes mean from4.2 to6.2. 1=yes,0=no.",1,"Add10 across5 -> +2."),
            new("Replacing8 with18 changes median from4 to18. 1=yes,0=no.",0,"Median remains4."),
            new("Original mean increases while median remains unchanged, choice B. 1=yes,0=no.",1,"Sorted center unaffected."),
            new("Six100s,95,90,0 scores have median100. 1=yes,0=no.",1,"Fifth of9 is100."),
            new("Those Han scores have mean approximately87.2. 1=yes,0=no.",1,"785/9≈87.22."),
            new("The zero homework outlier reduces mean more than median. 1=yes,0=no.",1,"Extreme value."),
            new("A median can be preferable for describing typical scores when an exceptional zero distorts mean. 1=yes,0=no.",1,"Resistance to outliers."),
            new("Mean and median must always coincide for symmetric distributions. 1=yes,0=no.",0,"Often near, but not mathematical universal absent conditions."),
            new("For nearly symmetric distributions mean and median often are close. 1=yes,0=no.",1,"Distribution shape."),
            new("Teacher ages may have few unusually old values that pull mean upward relative to median. 1=yes,0=no.",1,"Right tail."),
            new("The original teacher-age exact mean can be certified from an ambiguous plot description alone. 1=yes,0=no.",0,"Requires source plot verification."),
            new("Data4,5,5,6,100 have mean24 and median5. 1=yes,0=no.",1,"Big outlier."),
            new("When data contains extreme outlier, median always becomes incorrect statistic for every question. 1=yes,0=no.",0,"Choice depends on purpose."),
            new("Mean can still be useful for totals and shared resources even if outlier exists. 1=yes,0=no.",1,"Fair share."),
            new("Median depends on sorted relative position rather than total sum. 1=yes,0=no.",1,"Middle statistic."),
            new("Changing an extreme observation without changing ranks can leave median unchanged. 1=yes,0=no.",1,"Order-middle stable."),
            new("A dot plot with symmetric cluster may suggest mean close to median. 1=yes,0=no.",1,"Often close."),
            new("A student choice of center requires justification with data context. 1=yes,0=no.",1,"Teacher rubric.")
        ],
        [L0815QuartileValues] =
        [
            new("A dataset has20 all-different ordered values. How many lie strictly between first and third quartile using median-of-halves convention?",10,"Ranks6..15."),
            new("20 distinct ordered values. Number strictly between Q1 and median using split-halves convention?",5,"Ranks6..10."),
            new("Bowling Mai median120 IQR5, Priya median118 IQR15. Which has less variability by IQR? Enter1 Mai,2 Priya.",1,"5<15."),
            new("Bowling Mai IQR5 vs Priya15. Difference in IQR?",10,"15-5=10."),
            new("US travel times2,2,7,7,8,8,8,11,17,20. Median?",8,"Middle eighth values8,8."),
            new("US sorted travel times first quartile Q1?",7,"Lower five middle7."),
            new("US sorted travel times third quartile Q3?",11,"Upper five middle11."),
            new("US travel-time IQR?",4,"11-7=4."),
            new("Canada sorted1,2,5,5,7,7,10,15,28,30. Median?",7,"Two7s."),
            new("Canada first quartile?",5,"Middle of lower five5."),
            new("Canada third quartile?",15,"Middle upper five15."),
            new("Canada IQR?",10,"15-5=10."),
            new("Australia sorted5,7,9,15,15,20,20,20,25,45. Median in HALVES?",35,"(15+20)/2=17.5."),
            new("Australia first quartile?",9,"Lower five median9."),
            new("Australia third quartile?",20,"Upper five median20."),
            new("Australia IQR?",11,"20-9=11."),
            new("New Zealand sorted3,6,7,10,10,15,15,15,20,24. Median in HALVES?",25,"(10+15)/2=12.5."),
            new("New Zealand first quartile?",7,"Lower five median7."),
            new("New Zealand third quartile?",15,"Upper five median15."),
            new("New Zealand IQR?",8,"15-7=8."),
            new("South Africa sorted5,5,10,10,15,15,30,40,45,60. Median?",15,"Middle two15."),
            new("South Africa first quartile?",10,"Lower five middle10."),
            new("South Africa third quartile?",40,"Upper five middle40."),
            new("South Africa IQR?",30,"40-10=30."),
            new("Jar has20 pennies equal16% of coins. Total number coins?",125,"20/.16=125."),
            new("Dataset2,4,6,8,10. Range?",8,"10-2=8.")
        ],
        [L0815QuartileLogic] =
        [
            new("IQR equals third quartile minus first quartile. 1=yes,0=no.",1,"Q3-Q1."),
            new("Five-number summary comprises min,Q1,median,Q3,max. 1=yes,0=no.",1,"Definition."),
            new("Median is the second quartile Q2. 1=yes,0=no.",1,"50th percentile."),
            new("Q1 marks an approximate lower-quarter cut in ordered data. 1=yes,0=no.",1,"25th percentile."),
            new("Q3 marks the upper-three-quarter cut. 1=yes,0=no.",1,"75th percentile."),
            new("For20 distinct ordered values under split-half convention, ten values fall betweenQ1 andQ3. 1=yes,0=no.",1,"Central ten."),
            new("Mai IQR5 shows less middle-half variability than Priya IQR15. 1=yes,0=no.",1,"Smaller spread."),
            new("US commute IQR4, Canada10, Australia11, NZ8, SA30 match source five plots. 1=yes,0=no.",1,"Computed from sorted times."),
            new("South African commute IQR30 is largest among the five. 1=yes,0=no.",1,"30."),
            new("US commute IQR4 is smallest among the five. 1=yes,0=no.",1,"4."),
            new("US median8 and Australia median17.5 differ. 1=yes,0=no.",1,"17.5>8."),
            new("A smaller IQR always guarantees a smaller full range. 1=yes,0=no.",0,"Outliers may extend range."),
            new("The interquartile range measures spread of middle50% of ordered data. 1=yes,0=no.",1,"Central half."),
            new("The range max-min can be sensitive to unusual extremes. 1=yes,0=no.",1,"Outliers."),
            new("Source 20pennies make16% of jar of125 coins. 1=yes,0=no.",1,"20/.16=125."),
            new("The exact values of the original word-game scores can be recovered without observing their dot-plot image. 1=yes,0=no.",0,"Image required."),
            new("An IQR is an average of absolute deviations from mean. 1=yes,0=no.",0,"That's MAD."),
            new("A student must label quartiles with the underlying measurement unit when presenting data. 1=yes,0=no.",1,"Context.")
        ],
        [L0816BoxPlotValues] =
        [
            new("Given five-number seconds min40 Q1=45 median48 Q3=50 max60. Range in seconds?",20,"60-40=20."),
            new("Same box plot. IQR seconds?",5,"50-45=5."),
            new("Same box plot. Left whisker length from min to Q1?",5,"45-40=5."),
            new("Same box plot. Right whisker length from Q3 to max?",10,"60-50=10."),
            new("Same box plot. Left half of box Q1 to median length?",3,"48-45=3."),
            new("Same box plot. Right half of box median to Q3 length?",2,"50-48=2."),
            new("Homework data3,10,12,4,7,9,5,5,11,11,5,12,11. Count?",13,"Thirteen values."),
            new("Same homework data sorted. Minimum?",3,"Min3."),
            new("Same homework data sorted. Maximum?",12,"Max12."),
            new("Same homework data sorted. Median?",9,"Seventh ordered value9."),
            new("Same homework data. First quartile Q1?",5,"Lower6 median5."),
            new("Same homework data. Third quartile Q3?",11,"Upper6 median11."),
            new("Same homework data. IQR?",6,"11-5=6."),
            new("Same homework data. Range?",9,"12-3=9."),
            new("Given100 mice and box interval Q1 to Q3, percentage represented?",50,"Middle half."),
            new("Given100 mice and interval Q1 to median, approximate count?",25,"Quarter of100."),
            new("Jada donates26 dollars, one of three equal spending/saving/donating portions. Total earnings in DOLLARS?",78,"26×3=78."),
            new("Earnings78dollars across babysitting30, walking dogs28, errands20. Total?",78,"30+28+20."),
            new("Same earnings78, donation26. Remaining for spending and saving combined?",52,"26+26=52."),
            new("A box summary40,45,48,50,60. Lower quartile in SECONDS?",45,"Q1=45."),
            new("A box summary40,45,48,50,60. Upper quartile in SECONDS?",50,"Q3=50."),
            new("Dataset3,4,5,5,5,7,9,10,11,11,11,12,12. How many values below9?",6,"Six values."),
            new("A box spanning45 to50 seconds has width in SECONDS?",5,"IQR."),
            new("For a box plot of100 subjects, middle50% corresponds to how many subjects?",50,"About50 observations.")
        ],
        [L0816BoxPlotLogic] =
        [
            new("A box plot displays minimum,Q1,median,Q3,maximum. 1=yes,0=no.",1,"Five-number summary."),
            new("The original seconds example has Q1=45, median48 and Q3=50. 1=yes,0=no.",1,"Given."),
            new("For summary40,45,48,50,60 the IQR equals5 seconds. 1=yes,0=no.",1,"50-45."),
            new("For summary40,45,48,50,60 range equals20 seconds. 1=yes,0=no.",1,"60-40."),
            new("A box plot normally reveals all exact observed values like a dot plot. 1=yes,0=no.",0,"It compresses data."),
            new("A box includes the middle50% of data betweenQ1 andQ3. 1=yes,0=no.",1,"Quartile interval."),
            new("13 homework values have median9 hours. 1=yes,0=no.",1,"Seventh after sorting."),
            new("13 homework values have Q1=5 and Q3=11. 1=yes,0=no.",1,"Medians of lower and upper6."),
            new("13 homework values IQR6 hours. 1=yes,0=no.",1,"11-5."),
            new("In a100-mouse study, roughly50 lie within theQ1–Q3 box. 1=yes,0=no.",1,"Middle half."),
            new("The original 100-mice box interval0.5–1sec can be counted exactly without source graph/percentile positions. 1=yes,0=no.",0,"Needs actual figure."),
            new("Jada donates one-third of earnings and gave26 dollars, so total78. 1=yes,0=no.",1,"3×26."),
            new("The breakdown of78 dollars by three jobs is uniquely determined from donation alone. 1=yes,0=no.",0,"Many valid."),
            new("A dot plot may show individual frequency lost in box plot. 1=yes,0=no.",1,"Summary only."),
            new("The original books-read maximum and median can be assigned exact source values using only the text prompt without box plot. 1=yes,0=no.",0,"Image needed."),
            new("The numeric type of a box plot scale must match measured variable units. 1=yes,0=no.",1,"e.g. seconds/hours."),
            new("A five-number summary can be computed from ordered raw data. 1=yes,0=no.",1,"Sort."),
            new("A lower IQR indicates tighter middle half but not necessarily smaller total range. 1=yes,0=no.",1,"Extremes may differ.")
        ],
        [L0817BoxPlotValues] =
        [
            new("Athlete basketball median height in TENTHS of inch?",785,"78.5in."),
            new("Football median in TENTHS of inch?",738,"73.8in."),
            new("Hockey median in TENTHS of inch?",731,"73.1in."),
            new("Baseball median in TENTHS of inch?",728,"72.8in."),
            new("Basketball IQR in TENTHS of inch using Q3 80.3 Q1 76.1?",42,"4.2in."),
            new("Football IQR in TENTHS of inch Q3 75.1 Q1 70.1?",50,"5.0in."),
            new("Hockey IQR in TENTHS of inch Q3 75 Q1 72?",30,"3in."),
            new("Baseball IQR in TENTHS of inch Q3 74.5 Q1 72?",25,"2.5in."),
            new("Which sport has largest full range? Enter1 basket,2 football,3 hockey,4 baseball.",1,"Basketball 91.1-67.3=23.8in."),
            new("Which sport has smallest full range? Enter1 basket,2 football,3 hockey,4 baseball.",4,"Baseball 76.2-70=6.2in."),
            new("Which two sports show most similar median heights? Enter1 hockey+baseball,2 hockey+basketball,3 football+basketball.",1,"73.1 vs72.8in."),
            new("Pineapple Crate1 median in TENTHS of pound?",40,"4lb."),
            new("Pineapple Crate2 median in TENTHS of pound?",20,"2lb."),
            new("Pineapple Crate3 median in TENTHS of pound?",41,"4.1lb."),
            new("Crate1 IQR in HUNDREDTHS of pound Q1=3.25 Q3=4.6?",135,"1.35lb."),
            new("Crate2 IQR in HUNDREDTHS Q1=1.25 Q3=2.25?",100,"1lb."),
            new("Crate3 IQR in HUNDREDTHS Q1=3.6 Q3=4.4?",80,"0.8lb."),
            new("Which crate has greatest full range? Enter1,2,3.",1,"1:1.5 to6=4.5."),
            new("Which crate has heaviest maximum pineapple? Enter1,2,3.",1,"Crate1 max6lb."),
            new("Which crate has lightest minimum? Enter1,2,3.",2,"Crate2 min0.75lb."),
            new("A TV show mean age35 years MAD20, another mean30 MAD5. Which show more variable? Enter1 former,2 latter.",1,"MAD20>5."),
            new("A TV show mean35 years MAD20, another mean30 MAD5. Which has lower mean age? Enter1 former,2 latter.",2,"30<35."),
            new("Approximately what fraction of100 calls are at or below Q3 in box plot? Whole percent?",75,"Q3 around75th percentile."),
            new("Fire response times of100 calls, about how many lie above Q3?",25,"Upper quarter."),
            new("Crate1 max6lb vs Crate2 max3.4lb, difference in TENTHS lb?",26,"2.6lb.")
        ],
        [L0817BoxPlotLogic] =
        [
            new("Hockey and baseball box plots have close median heights73.1 and72.8. 1=yes,0=no.",1,"Near centers."),
            new("Basketball athlete heights have largest full range in source. 1=yes,0=no.",1,"Range23.8."),
            new("Baseball has smallest full range of four sports. 1=yes,0=no.",1,"6.2."),
            new("Basketball has smallest IQR of four sports. 1=yes,0=no.",0,"Baseball IQR2.5; basketball4.2."),
            new("Football IQR5in exceeds hockey3in. 1=yes,0=no.",1,"5>3."),
            new("Pineapple crate1 most variable by full range. 1=yes,0=no.",1,"Original option A."),
            new("Pineapple heaviest in crate1 (max6lb). 1=yes,0=no.",1,"Original B."),
            new("Pineapple lightest in crate1. 1=yes,0=no.",0,"Crate2 min0.75."),
            new("Crate3 has greatest median AND greatest IQR. 1=yes,0=no.",0,"IQR crate1 greater."),
            new("More than half pineapple samples in crates1 and3 exceed Crate2 maximum3.4lb according to their medians. 1=yes,0=no.",1,"Medians4 and4.1 above3.4, source E."),
            new("Crate1 and Crate3 medians4 and4.1 are close. 1=yes,0=no.",1,"Difference0.1."),
            new("A box plot middle interval Q1–Q3 covers about half of data. 1=yes,0=no.",1,"IQR span."),
            new("A fire response-time box plot is sufficient to determine exact 100 individual times. 1=yes,0=no.",0,"Only summary."),
            new("If response-time Q3 is13min, about75% of values are at or below13min. 1=yes,0=no.",1,"75th percentile under given premise."),
            new("Whether source fire response Q3 equals13min must be inspected from original chart. 1=yes,0=no.",1,"Missing graphical measurements."),
            new("A TV show with MAD20 has greater age variability than a show with MAD5. 1=yes,0=no.",1,"MAD spread."),
            new("One cannot infer every viewer's age from mean and MAD only. 1=yes,0=no.",1,"Insufficient detail."),
            new("Same IQR and different median means similar middle-half spread but different center. 1=yes,0=no.",1,"Center vs spread."),
            new("Teacher should review open-ended box plot reasoning and original images. 1=yes,0=no.",1,"Human-reviewed graphical task.")
        ],
        [L0818SourceStatistics] =
        [
            new("Source student summary cookie-weight sample. How many cookies?",30,"30 cookies measured."),
            new("Source cookie weights mean in GRAMS?",21,"Given mean21g."),
            new("Source cookie weights MAD in TENTHS of gram?",56,"5.6g."),
            new("Source five-number summary cookie minimum grams?",9,"Given min9g."),
            new("Source cookie Q1 grams?",16,"Given Q1 16."),
            new("Source cookie median in HALF-GRAM UNITS?",41,"20.5g."),
            new("Source cookie Q3 grams?",26,"Given26."),
            new("Source cookie maximum grams?",34,"Given max34."),
            new("Source cookie IQR grams?",10,"26-16=10."),
            new("Source cookie full range grams?",25,"34-9=25."),
            new("Source cookie mean21g over30 cookies. Total combined weight in GRAMS?",630,"21×30=630g."),
            new("Source cookie mean21g and MAD5.6g. Lower mean-minus-MAD in TENTHS GRAMS?",154,"21-5.6=15.4."),
            new("Source cookie mean21g and MAD5.6g. Upper mean-plus-MAD in TENTHS GRAMS?",266,"21+5.6=26.6."),
            new("Source cookie mean21g median20.5g. Difference in HALF-GRAM UNITS?",1,"0.5g."),
            new("Source cookie five-number: lower whisker Q1-min in GRAMS?",7,"16-9=7."),
            new("Source cookie five-number: upper whisker max-Q3 in GRAMS?",8,"34-26=8."),
            new("Independent hypothetical perch ages1,2,2,3,4. Sample size?",5,"Five fictional observations."),
            new("Same illustrative ages1,2,2,3,4. Mean in FIFTHS OF YEAR?",12,"12/5=2.4."),
            new("Same fictional perch ages1,2,2,3,4. Median years?",2,"Middle third2."),
            new("Same fictional perch ages1,2,2,3,4. Range years?",3,"4-1=3."),
            new("Illustrative homework count0,0,2,4,4,10. Sum?",20,"20 problems."),
            new("Illustrative homework count0,0,2,4,4,10. Mean in THIRDS?",10,"20/6=10/3."),
            new("Illustrative homework count0,0,2,4,4,10. Median?",3,"Middle2 and4 average3."),
            new("Illustrative homework0,0,2,4,4,10. Range?",10,"10-0=10.")
        ],
        [L0818SelectionLogic] =
        [
            new("The original U08:L18 is a statistical application lesson with Wild Bears, Math Homework and Yellow Perch tasks. 1=yes,0=no.",1,"Original task headings."),
            new("The original U08:L18 has a separate original practice.html worksheet. 1=yes,0=no.",0,"Archive has no practice.html."),
            new("The original U08:L18 includes a teacher-restricted Cool-down heading18.5 about Time Spent on Chores. 1=yes,0=no.",1,"Source teacher index."),
            new("The source student summary includes30 cookie weights with mean21grams. 1=yes,0=no.",1,"Student summary."),
            new("Source cookie MAD equals5.6grams. 1=yes,0=no.",1,"Student summary."),
            new("Source cookie median equals20.5grams. 1=yes,0=no.",1,"Five-number."),
            new("Source cookie Q1=16 and Q3=26 give IQR10grams. 1=yes,0=no.",1,"26-16=10."),
            new("Source cookie minimum9 and maximum34 give range25grams. 1=yes,0=no.",1,"34-9=25."),
            new("The mean21g multiplied by30 cookies gives total630g in mathematical model. 1=yes,0=no.",1,"Mean×count."),
            new("An outlier can make median/IQR more informative than mean/MAD for a skewed distribution. 1=yes,0=no.",1,"Context matters."),
            new("The choice of mean versus median must be justified using the statistical question and data shape. 1=yes,0=no.",1,"Not fixed rule."),
            new("A box plot is the best tool when exact frequencies of every original individual value are essential. 1=yes,0=no.",0,"Dot plot retains individuals."),
            new("A histogram can show patterns of larger samples by intervals. 1=yes,0=no.",1,"Bins summarize."),
            new("Different histogram bin widths may change visual appearance. 1=yes,0=no.",1,"Grouping choice."),
            new("A project claim that yellow perch populations will definitely survive can be proven solely by a sample histogram. 1=yes,0=no.",0,"Observational data and uncertainty."),
            new("Original Lake Michigan fish statistics should be represented with labeled actual data rather than fabricated measurements. 1=yes,0=no.",1,"Source evidence."),
            new("The illustrative ages1,2,2,3,4 are NOT claimed original yellow-perch research data. 1=yes,0=no.",1,"Independent example."),
            new("A full teacher assessment should evaluate chosen graph, center, spread and limitations. 1=yes,0=no.",1,"Project rubric."),
            new("An independent first-party numerical check does not certify original lesson artwork or scientific claims. 1=yes,0=no.",1,"Selected practice boundary."),
            new("A sample statistic is an estimate/description, not necessarily a causal population conclusion. 1=yes,0=no.",1,"Statistical inference limitations.")
        ]
    };

    public static bool Supports(string? family) =>
        family is L01Collections or L01Shade or L02Diagrams or L02Cards or L03RecipeBatches or L03EqualTaste
        or L04ColorAmounts or L04EqualShade ||
        (family is not null && AdditionalLessonFamilies.ContainsKey(family));

    public static Problem Build(string family, Random random)
    {
        if (!Supports(family)) throw new ArgumentException("Unknown G6 U02 ratio family",nameof(family));
        ArgumentNullException.ThrowIfNull(random);
        var variant = random.Next(AdditionalLessonFamilies.TryGetValue(family,out var extras)
            ? extras.Length : family switch
        {
            L01Collections => Pairs.Length,
            L01Shade => Shades.Length,
            L02Diagrams => Diagrams.Length,
            L02Cards => Cards.Length,
            L03RecipeBatches => RecipeBatches.Length,
            L03EqualTaste => EqualTaste.Length,
            L04ColorAmounts => ColorAmounts.Length,
            L04EqualShade => EqualShades.Length,
            _ => throw new ArgumentOutOfRangeException(nameof(family))
        });
        if (AdditionalLessonFamilies.TryGetValue(family,out var authored))
        {
            var item=authored[variant];
            return new(family,item.Prompt,item.Explanation+
                " Teacher checks source diagrams, ratio reasoning and physical groupings.",
                AssessmentItemType.Numeric,new Dictionary<string,int>{{"variant",variant}});
        }
        if (family is L04ColorAmounts or L04EqualShade)
        {
            var r=family==L04ColorAmounts?ColorAmounts[variant]:EqualShades[variant];
            return new(family,r.Prompt,
                r.Explanation+" Teacher checks actual mixture observations, same-shade model, correct units and labeled discrete diagrams.",
                AssessmentItemType.Numeric,new Dictionary<string,int>{{"variant",variant}});
        }
        if (family is L03RecipeBatches or L03EqualTaste)
        {
            return new(family,row.Prompt,
                row.Explanation+" Teacher checks the student's physical recipe grouping, visual ratios, ingredient units and explanation separately.",
                AssessmentItemType.Numeric,new Dictionary<string,int>{{"variant",variant}});
        }
        if (family == L02Diagrams)
        {
            var (first,a,unitA,second,b,unitB)=Diagrams[variant];
            return new(family,
                $"A labeled two-row diagram has {a} marks for {first} ({unitA}) and {b} marks for {second} ({unitB}). What is the ratio of the FIRST row to the SECOND? Enter the depicted A:B counts.",
                $"The labeled diagram represents {a} {unitA} of {first} for {b} {unitB} of {second}; order is {a}:{b}. Units must remain attached to their labels. Teacher checks physical cube arrangement and student sketches.",
                AssessmentItemType.ShortAnswer,new Dictionary<string,int>{{"variant",variant}});
        }
        if (family == L02Cards)
        {
            var (a,na,ua,b,nb,ub,c,nc,uc)=Cards[variant];
            return new(family,
                $"A three-row recipe diagram names {na} {ua} {a}, {nb} {ub} {b} and {nc} {uc} {c}. In THAT named order, what is the three-part ratio? Enter A:B:C.",
                $"The three separate labeled rows give {na}:{nb}:{nc}; the full names/units are {a} ({ua}), {b} ({ub}), {c} ({uc}). Teacher assesses actual diagram, card matching and partner critique.",
                AssessmentItemType.ShortAnswer,new Dictionary<string,int>{{"variant",variant}});
        }
        if (family == L01Collections)
        {
            var (first,a,second,b) = Pairs[variant];
            return new(family,
                $"A labeled collection has {a} {first} and {b} {second}. Give the ratio of {first} to {second}, in that order, using A:B notation.",
                $"The order is {first} first ({a}) and {second} second ({b}), so {a}:{b}. Equivalent positive ratios are accepted. Student-created category choices and diagrams require teacher review.",
                AssessmentItemType.ShortAnswer,new Dictionary<string,int>{{"variant",variant}});
        }

        var (total,x,y,firstColor) = Shades[variant];
        var first = total * x / (x+y);
        var second = total * y / (x+y);
        return new(family,
            $"A shape is made of {total} equal square units. Color A to color B is {x}:{y}. How many units should be color {(firstColor?"A":"B")}?",
            $"The ratio has {x+y} equal parts; each part is {total/(x+y)} square units. Color A takes {first} and B takes {second}. Teacher reviews actual coloring, equal units, and the student's other differently shaped diagram.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"variant",variant}});
    }

    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p) =>
        Supports(family) && p is {Count:1} && p.TryGetValue("variant",out var v) &&
        v >= 0 && v < (AdditionalLessonFamilies.TryGetValue(family!,out var additional)
            ? additional.Length : family switch
        {
            L01Collections => Pairs.Length,
            L01Shade => Shades.Length,
            L02Diagrams => Diagrams.Length,
            L02Cards => Cards.Length,
            L03RecipeBatches => RecipeBatches.Length,
            L03EqualTaste => EqualTaste.Length,
            L04ColorAmounts => ColorAmounts.Length,
            L04EqualShade => EqualShades.Length,
            _ => 0
        });

    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if (!Valid(family,p)) throw new ArgumentException("Invalid ratio-family variant.");
        var v=p["variant"];
        if (AdditionalLessonFamilies.TryGetValue(family,out var authored))
            return authored[v].Answer.ToString(CultureInfo.InvariantCulture);
        if (family is L04ColorAmounts or L04EqualShade)
        {
            var r=family==L04ColorAmounts?ColorAmounts[v]:EqualShades[v];
            return r.Answer.ToString(CultureInfo.InvariantCulture);
        }
        if (family is L03RecipeBatches or L03EqualTaste)
        {
            var row=family==L03RecipeBatches?RecipeBatches[v]:EqualTaste[v];
            return row.Answer.ToString(CultureInfo.InvariantCulture);
        }
        if (family==L02Diagrams)
        {
            var d=Diagrams[v];
            return $"{d.A}:{d.B}";
        }
        if (family==L02Cards)
        {
            var c=Cards[v];
            return $"{c.CountA}:{c.CountB}:{c.CountC}";
        }
        if (family==L01Collections)
        {
            var pair=Pairs[v];
            return $"{pair.A}:{pair.B}";
        }

        var s=Shades[v];
        return (s.Total*(s.FirstColor?s.First:s.Second)/(s.First+s.Second))
            .ToString(CultureInfo.InvariantCulture);
    }

    public static bool Verify(string family,IReadOnlyDictionary<string,int>? p,string? answer)
    {
        if (!Valid(family,p) || string.IsNullOrWhiteSpace(answer)) return false;
        var expected=Solve(family,p!);
        if (AdditionalLessonFamilies.ContainsKey(family))
            return int.TryParse(answer,NumberStyles.None,CultureInfo.InvariantCulture,out var bankNumber)
                && bankNumber.ToString(CultureInfo.InvariantCulture)==expected;
        if (family is L04ColorAmounts or L04EqualShade)
            return int.TryParse(answer,NumberStyles.None,CultureInfo.InvariantCulture,out var colorNumber)
                && colorNumber.ToString(CultureInfo.InvariantCulture)==expected;
        if (family is L03RecipeBatches or L03EqualTaste)
            return int.TryParse(answer,NumberStyles.None,CultureInfo.InvariantCulture,out var n)
                && n.ToString(CultureInfo.InvariantCulture)==expected;
        if (family==L01Shade)
            return int.TryParse(answer,NumberStyles.None,CultureInfo.InvariantCulture,out var value)
                && value.ToString(CultureInfo.InvariantCulture)==expected;

        if (family is L02Diagrams or L02Cards)
            return string.Equals(answer.Trim().Replace(" ","",StringComparison.Ordinal),
                expected,StringComparison.Ordinal);
        var pieces=answer.Split(':');
        if (pieces.Length!=2 ||
            !int.TryParse(pieces[0].Trim(),NumberStyles.None,CultureInfo.InvariantCulture,out var a) ||
            !int.TryParse(pieces[1].Trim(),NumberStyles.None,CultureInfo.InvariantCulture,out var b) ||
            a<=0 || b<=0 || a>100000 || b>100000) return false;
        var pair=Pairs[p!["variant"]];
        return (long)a*pair.B==(long)b*pair.A;
    }
}
