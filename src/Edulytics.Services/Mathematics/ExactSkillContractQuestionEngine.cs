using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Generation;
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
         SupportingPracticeCompletionEngine.Supports(family.Trim()));

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
                    problem.Parameters);

                var normalizedPrompt = NormalizePrompt(problem.Prompt);
                if (excluded.Contains(fingerprint) ||
                    generated.Contains(fingerprint) ||
                    generatedPrompts.Contains(normalizedPrompt))
                {
                    continue;
                }

                generated.Add(fingerprint);
                generatedPrompts.Add(normalizedPrompt);
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
            "number.whole.divide.with_remainder.apply")
        {
            var dividend = parameters["dividend"];
            var divisor = parameters["divisor"];
            if (dividend < 0 || divisor <= 0)
                return false;

            var quotient = dividend / divisor;
            var remainder = dividend % divisor;
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
        if (SupportingPracticeCompletionEngine.Supports(problem.Family))
            return SupportingPracticeCompletionEngine.Solve(problem.Family, p);

        return problem.Family switch
        {
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
            "number.whole.divide.with_remainder.apply" =>
                $"{(p["dividend"] / p["divisor"]).ToString(CultureInfo.InvariantCulture)} r {(p["dividend"] % p["divisor"]).ToString(CultureInfo.InvariantCulture)}",

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

            "geometry.perimeter_area.rectangle_missing_side" =>
                (p["area"] / p["knownSide"]).ToString(CultureInfo.InvariantCulture),

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
        IReadOnlyDictionary<string, int> parameters)
    {
        var canonicalParameters = string.Join(
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
    private static string NormalizePrompt(string prompt) =>
        string.Join(
            " ",
            (prompt ?? string.Empty).Split(
                (char[]?)null,
                StringSplitOptions.RemoveEmptyEntries))
            .Trim();

}
