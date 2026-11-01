using System.Globalization;
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
         SupportingPracticeCompletionEngine.Supports(family.Trim()) ||
         UsCcssT001ExactQuestions.Supports(family.Trim()) ||
         UsCcssT001ConstructShapeGraph.Supports(family.Trim()) ||
         UsCcssT001AuthoredActivities.Supports(family.Trim()) ||
         UsCcssT001RepresentationChoices.Supports(family.Trim()) ||
         UsCcssG1Batch03DataQuestions.Supports(family.Trim()) ||
         UsCcssG1Unit2StoryQuestions.Supports(family.Trim()) ||
         UsCcssG5U03L01FractionOfHalfQuestions.Supports(family.Trim()) ||
         UsCcssG5U03L02UnitFractionQuestions.Supports(family.Trim()) ||
         UsCcssG5U03L03UnitFractionQuestions.Supports(family.Trim()) ||
         UsCcssG5U03L04ParkQuestions.Supports(family.Trim()) ||
         UsCcssG5U03L05ShadedAreaQuestions.Supports(family.Trim()) ||
         UsCcssG5U03L06NonunitAreaQuestions.Supports(family.Trim()) ||
         UsCcssG5U03L07FractionEquationQuestions.Supports(family.Trim()) ||
         UsCcssG5U03L08FlagMeasureQuestions.Supports(family.Trim()) ||
         UsCcssG5U03L09FlagDesignSupportQuestions.Supports(family.Trim()) ||
         UsCcssG7Unit1Lesson1ScaledCopyQuestions.Supports(family.Trim()) ||
         UsCcssG6Unit1Lesson1TilingQuestions.Supports(family.Trim()) ||
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
                    requestedVariant);
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
        if (UsCcssG7Unit1Lesson1ScaledCopyQuestions.Supports(family))
            return UsCcssG7Unit1Lesson1ScaledCopyQuestions.Verify(family, parameters, answer);
        if (UsCcssG6Unit1Lesson1TilingQuestions.Supports(family))
            return UsCcssG6Unit1Lesson1TilingQuestions.Verify(family, parameters, answer);
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

    private static ExactProblem BuildProblem(
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
        if (UsCcssG7Unit1Lesson1ScaledCopyQuestions.Supports(family))
        {
            var item = UsCcssG7Unit1Lesson1ScaledCopyQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
                item.ItemType, item.Parameters);
        }
        if (UsCcssG6Unit1Lesson1TilingQuestions.Supports(family))
        {
            var item = UsCcssG6Unit1Lesson1TilingQuestions.Build(family, random);
            return new ExactProblem(item.Family, item.Prompt, item.Solution,
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

        return family switch
        {
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
        var multiplier = random.Next(1, Math.Max(2, scale + 1));
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
        if (UsCcssG7Unit1Lesson1ScaledCopyQuestions.Supports(problem.Family))
            return UsCcssG7Unit1Lesson1ScaledCopyQuestions.Solve(problem.Family, p);
        if (UsCcssG6Unit1Lesson1TilingQuestions.Supports(problem.Family))
            return UsCcssG6Unit1Lesson1TilingQuestions.Solve(problem.Family, p);
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
