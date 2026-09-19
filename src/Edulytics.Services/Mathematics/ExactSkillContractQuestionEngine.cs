using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Ast;
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
    string ExposureFingerprint);

/// <summary>
/// Shared deterministic Mathematics Intelligence question kernel used by
/// learner Practice and teacher Assessment Builder. The caller owns curriculum
/// alignment; this engine owns exact parameter generation, solving, independent
/// verification and reconstructable fingerprints for approved question families.
/// </summary>
public sealed class ExactSkillContractQuestionEngine
{
    private const int MaxRetriesPerItem = 128;

    public IReadOnlyList<ExactSkillGeneratedQuestion> Generate(
        string fingerprintNamespace,
        string scopeKey,
        IReadOnlyList<string> allowedQuestionFamilies,
        ExactSkillQuestionDifficulty difficulty,
        int questionCount,
        int seed,
        IReadOnlyCollection<string> excludedExposureFingerprints)
    {
        if (string.IsNullOrWhiteSpace(fingerprintNamespace) ||
            string.IsNullOrWhiteSpace(scopeKey) ||
            allowedQuestionFamilies.Count == 0 ||
            questionCount is < 1 or > 50)
        {
            throw new InvalidOperationException("Exact SkillContract generation requires a valid scope, family registry and question count.");
        }

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
        var items = new List<ExactSkillGeneratedQuestion>(questionCount);

        for (var index = 0; index < questionCount; index++)
        {
            ExactSkillGeneratedQuestion? item = null;

            for (var retry = 0; retry < MaxRetriesPerItem && item is null; retry++)
            {
                var family = allowedQuestionFamilies[(index + retry) % allowedQuestionFamilies.Count];
                var problem = BuildProblem(family, random, difficulty);
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

                if (excluded.Contains(fingerprint) || generated.Contains(fingerprint))
                    continue;

                generated.Add(fingerprint);
                item = new ExactSkillGeneratedQuestion(
                    problem.Family,
                    problem.Prompt,
                    problem.Solution,
                    problem.ItemType,
                    answer,
                    problem.Parameters,
                    fingerprint);
            }

            if (item is null)
                throw new InvalidOperationException("Exact Mathematics generation exhausted uniqueness retries.");

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
        if (family == "fractions.compare.unlike.common_denominator")
            return string.Equals(
                answer,
                Compare(parameters["n1"] * parameters["d2"], parameters["n2"] * parameters["d1"]),
                StringComparison.Ordinal);

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

            "geometry.similarity.find_missing_length" =>
                parameters["scaleFactor"] > 0 &&
                value == parameters["sourceLength"] * parameters["scaleFactor"],

            "geometry.similarity.scale_factor" =>
                parameters["sourceLength"] > 0 &&
                parameters["targetLength"] % parameters["sourceLength"] == 0 &&
                value == parameters["targetLength"] / parameters["sourceLength"],

            "geometry.surface_area.rectangular_prism" =>
                value == 2 * (
                    parameters["length"] * parameters["width"] +
                    parameters["length"] * parameters["height"] +
                    parameters["width"] * parameters["height"]),

            "geometry.volume.rectangular_prism" =>
                value == parameters["length"] *
                    parameters["width"] *
                    parameters["height"],

            "geometry.rectangle.area.exact" or
            "geometry.perimeter_area.rectangle_area" =>
                value == parameters["length"] * parameters["width"],

            "geometry.rectangle.perimeter.exact" or
            "geometry.perimeter_area.rectangle_perimeter" =>
                value == 2 * (parameters["length"] + parameters["width"]),

            "geometry.right_triangle.pythagorean.exact" =>
                value > 0 &&
                value * value ==
                    parameters["legA"] * parameters["legA"] +
                    parameters["legB"] * parameters["legB"],

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

            _ => false
        };
    }

    private static ExactProblem BuildProblem(
        string family,
        Random random,
        ExactSkillQuestionDifficulty difficulty)
    {
        var scale = difficulty switch
        {
            ExactSkillQuestionDifficulty.Stretch => 2,
            ExactSkillQuestionDifficulty.Challenge => 3,
            _ => 1
        };

        return family switch
        {
            "algebra.relationships.two_unknowns.total_difference" =>
                BuildTwoUnknowns(random, scale),
            "measurement.scale.equal_intervals.read_value" =>
                BuildScaleReading(random, scale),
            "fractions.compare.unlike.common_denominator" =>
                BuildUnlikeFractionComparison(random, scale),
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
            "geometry.angles.parallel_lines" =>
                BuildParallelLineAngle(random, scale),
            "geometry.angles.supplementary" =>
                BuildSupplementaryAngle(random),
            "geometry.similarity.find_missing_length" =>
                BuildSimilarityMissingLength(random, scale),
            "geometry.similarity.scale_factor" =>
                BuildSimilarityScaleFactor(random, scale),
            "geometry.congruence.identify_criterion" =>
                BuildCongruenceCriterion(random),
            "geometry.surface_area.rectangular_prism" =>
                BuildRectangularPrismSurfaceArea(random, scale),
            "geometry.volume.rectangular_prism" =>
                BuildRectangularPrismVolume(random, scale),
            "geometry.rectangle.area.exact" =>
                BuildRectangleArea(random, scale, "geometry.rectangle.area.exact"),
            "geometry.perimeter_area.rectangle_area" =>
                BuildRectangleArea(random, scale, "geometry.perimeter_area.rectangle_area"),
            "geometry.rectangle.perimeter.exact" =>
                BuildRectanglePerimeter(random, scale, "geometry.rectangle.perimeter.exact"),
            "geometry.perimeter_area.rectangle_perimeter" =>
                BuildRectanglePerimeter(random, scale, "geometry.perimeter_area.rectangle_perimeter"),
            "geometry.right_triangle.pythagorean.exact" =>
                BuildPythagorean(random, scale),
            "trigonometry.right_triangle.ratio_exact" =>
                BuildTrigonometricRatio(random, scale),
            "trigonometry.right_triangle.find_side_exact" =>
                BuildTrigonometricFindSide(random, scale, false),
            "trigonometry.right_triangle.find_angle_exact" =>
                BuildTrigonometricFindAngle(random),
            "trigonometry.modelling.contextual" =>
                BuildTrigonometricFindSide(random, scale, true),
            ExactLinearEquationQuestionFactory.FamilyId =>
                BuildLinearEquation(random, scale),
            ExactLinearInequalityQuestionFactory.FamilyId =>
                BuildLinearInequality(random, scale),
            _ => UnsupportedFamily(family)
        };
    }

    private static ExactProblem UnsupportedFamily(string family)
    {
        MathematicsObservability.Record(MathematicsMetricKind.Unsupported);
        throw new InvalidOperationException($"Unsupported exact Mathematics question family: {family}");
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

    private static ExactProblem BuildScaleReading(Random random, int scale)
    {
        int[] intervalChoices = [2, 4, 5, 10];
        var intervals = intervalChoices[random.Next(intervalChoices.Length)];
        var step = random.Next(2, 7 + scale * 3);
        var start = random.Next(0, 10 + scale * 10);
        var end = start + intervals * step;
        var pointer = random.Next(1, intervals);

        return Problem(
            "measurement.scale.equal_intervals.read_value",
            $"A scale runs from {start} to {end} in {intervals} equal intervals. What value is at the tick {pointer} interval(s) after {start}?",
            "Find one interval by subtracting the endpoints and dividing by the number of intervals, then count from the start value.",
            AssessmentItemType.Numeric,
            ("start", start),
            ("end", end),
            ("intervals", intervals),
            ("pointer", pointer));
    }

    private static ExactProblem BuildUnlikeFractionComparison(Random random, int scale)
    {
        var denominator1 = random.Next(3, 8 + scale * 2);
        var denominator2 = random.Next(3, 9 + scale * 2);
        while (denominator2 == denominator1)
            denominator2 = random.Next(3, 9 + scale * 2);

        var numerator1 = random.Next(1, denominator1);
        var numerator2 = random.Next(1, denominator2);

        return Problem(
            "fractions.compare.unlike.common_denominator",
            $"Compare {numerator1}/{denominator1} and {numerator2}/{denominator2}. Enter <, >, or =.",
            "Compare the exact cross-products (or use a common denominator); do not compare denominators by size alone.",
            AssessmentItemType.ShortAnswer,
            ("n1", numerator1),
            ("d1", denominator1),
            ("n2", numerator2),
            ("d2", denominator2));
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

    private static ExactProblem BuildRectangularPrismSurfaceArea(Random random, int scale)
    {
        var length = random.Next(2, 7 + scale * 2);
        var width = random.Next(2, 6 + scale);
        var height = random.Next(2, 5 + scale);

        return Problem(
            "geometry.surface_area.rectangular_prism",
            $"A rectangular prism has length {length}, width {width}, and height {height}. Find its total surface area.",
            "Use 2(lw + lh + wh), then verify all six faces are counted.",
            AssessmentItemType.Numeric,
            ("length", length),
            ("width", width),
            ("height", height));
    }

    private static ExactProblem BuildRectangularPrismVolume(Random random, int scale)
    {
        var length = random.Next(2, 7 + scale * 2);
        var width = random.Next(2, 6 + scale);
        var height = random.Next(2, 5 + scale);

        return Problem(
            "geometry.volume.rectangular_prism",
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

    private static ExactProblem BuildPythagorean(Random random, int scale)
    {
        var triples = new (int A, int B, int C)[] { (3, 4, 5), (5, 12, 13), (8, 15, 17), (7, 24, 25) };
        var triple = triples[random.Next(triples.Length)];
        var multiplier = random.Next(1, Math.Max(2, scale + 1));
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
        return problem.Family switch
        {
            "algebra.relationships.two_unknowns.total_difference" =>
                ((p["total"] - p["difference"]) / 2).ToString(CultureInfo.InvariantCulture),

            "measurement.scale.equal_intervals.read_value" =>
                (p["start"] + ((p["end"] - p["start"]) / p["intervals"]) * p["pointer"])
                    .ToString(CultureInfo.InvariantCulture),

            "fractions.compare.unlike.common_denominator" =>
                Compare(p["n1"] * p["d2"], p["n2"] * p["d1"]),

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

            "geometry.angles.parallel_lines" =>
                p["expectedAngle"].ToString(CultureInfo.InvariantCulture),

            "geometry.angles.supplementary" =>
                (180 - p["knownAngle"]).ToString(CultureInfo.InvariantCulture),

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

            "geometry.surface_area.rectangular_prism" =>
                (2 * (
                    p["length"] * p["width"] +
                    p["length"] * p["height"] +
                    p["width"] * p["height"]))
                    .ToString(CultureInfo.InvariantCulture),

            "geometry.volume.rectangular_prism" =>
                (p["length"] * p["width"] * p["height"])
                    .ToString(CultureInfo.InvariantCulture),

            "geometry.rectangle.area.exact" or
            "geometry.perimeter_area.rectangle_area" =>
                (p["length"] * p["width"]).ToString(CultureInfo.InvariantCulture),

            "geometry.rectangle.perimeter.exact" or
            "geometry.perimeter_area.rectangle_perimeter" =>
                (2 * (p["length"] + p["width"])).ToString(CultureInfo.InvariantCulture),

            "geometry.right_triangle.pythagorean.exact" =>
                p["hypotenuse"].ToString(CultureInfo.InvariantCulture),

            "trigonometry.right_triangle.ratio_exact" =>
                SimplifyFraction(p["ratioNumerator"], p["ratioDenominator"]),

            "trigonometry.right_triangle.find_side_exact" or
            "trigonometry.modelling.contextual" =>
                p["expectedSide"].ToString(CultureInfo.InvariantCulture),

            "trigonometry.right_triangle.find_angle_exact" =>
                p["expectedAngle"].ToString(CultureInfo.InvariantCulture),

            ExactLinearEquationQuestionFactory.FamilyId =>
                ((p["right"] - p["offset"]) / p["coefficient"])
                    .ToString(CultureInfo.InvariantCulture),

            ExactLinearInequalityQuestionFactory.FamilyId =>
                SolveLinearInequality(p),

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
}
