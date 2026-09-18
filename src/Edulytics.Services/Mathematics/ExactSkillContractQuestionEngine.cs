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
    IReadOnlyDictionary<string, string> Representation,
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
        IReadOnlyCollection<string> excludedExposureFingerprints,
        int? curriculumLogicalLevel = null)
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
                var problem = BuildProblem(family, random, difficulty, curriculumLogicalLevel);
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
                    problem.Representation,
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

        if (family is "fractions.simplify.lowest_terms" or
            "fractions.multiply_by_whole.exact" or
            "fractions.multiply.exact" or
            "fractions.divide_by_whole.exact" or
            "fractions.add_subtract.exact")
        {
            return string.Equals(
                NormalizeFractionAnswer(answer),
                NormalizeFractionAnswer(ExpectedFractionAnswer(family, parameters)),
                StringComparison.Ordinal);
        }

        if (family == "fractions.compare.general")
        {
            return string.Equals(
                answer.Trim(),
                Compare(parameters["n1"] * parameters["d2"], parameters["n2"] * parameters["d1"]),
                StringComparison.Ordinal);
        }

        if (family == "fractions.compare.benchmark_half")
        {
            return string.Equals(
                answer.Trim(),
                Compare(parameters["n"] * 2, parameters["d"]),
                StringComparison.Ordinal);
        }

        if (family == "geometry.congruence.identify_criterion")
        {
            var expected = parameters["criterion"] switch
            {
                0 => "SSS",
                1 => "SAS",
                2 => "ASA",
                _ => string.Empty
            };
            return string.Equals(answer.Trim().ToUpperInvariant(), expected, StringComparison.Ordinal);
        }

        if (family == "trigonometry.right_triangle.sin_cos_tan.ratio")
        {
            return string.Equals(
                NormalizeFractionAnswer(answer),
                NormalizeFractionAnswer(ExpectedTrigRatio(parameters)),
                StringComparison.Ordinal);
        }

        if (!int.TryParse(answer, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value))
            return false;

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

            "number.whole.add_subtract.within_10" or
            "number.whole.add_subtract.across_10" or
            "number.whole.add_subtract.within_100" or
            "number.whole.add_subtract.columnar" or
            "number.whole.add_subtract.contextual_within_100" or
            "number.whole.add_subtract.contextual_across_10" =>
                VerifyWholeNumberAddSubtract(family, parameters, value),

            "fractions.notation.identify_part" =>
                value == (parameters["part"] == 0 ? parameters["n"] : parameters["d"]),

            "fractions.of_quantity.exact" =>
                parameters["d"] > 0 &&
                parameters["quantity"] % parameters["d"] == 0 &&
                value == (parameters["quantity"] / parameters["d"]) * parameters["n"],

            "fractions.number_line.read" =>
                parameters["d"] > 0 &&
                parameters["position"] >= 0 &&
                parameters["position"] <= parameters["d"] &&
                value == parameters["position"],

            "fractions.mixed_numbers.number_line.improper_numerator" or
            "fractions.mixed_improper.convert_to_improper_numerator" =>
                parameters["d"] > 0 &&
                value == parameters["whole"] * parameters["d"] + parameters["n"],

            "fractions.common_denominator.missing_numerator" =>
                parameters["baseD"] > 0 &&
                parameters["targetD"] > 0 &&
                parameters["targetD"] % parameters["baseD"] == 0 &&
                value == parameters["baseN"] * (parameters["targetD"] / parameters["baseD"]),

            "algebra.linear.ax_plus_b_equals_c" =>
                parameters["a"] != 0 &&
                value == parameters["x"] &&
                parameters["a"] * value + parameters["b"] == parameters["c"],

            "geometry.coordinate.straight_line.gradient" =>
                parameters["x2"] != parameters["x1"] &&
                (parameters["y2"] - parameters["y1"]) ==
                    value * (parameters["x2"] - parameters["x1"]) &&
                value == parameters["gradient"],

            "geometry.angles.relationships.missing_angle" =>
                parameters["total"] > parameters["known"] &&
                value == parameters["total"] - parameters["known"],

            "geometry.similarity.missing_length" =>
                parameters["scale"] > 0 &&
                value == parameters["base"] * parameters["scale"],

            "geometry.perimeter_area.rectangle" =>
                parameters["length"] > 0 &&
                parameters["width"] > 0 &&
                value == (parameters["mode"] == 0
                    ? parameters["length"] * parameters["width"]
                    : 2 * (parameters["length"] + parameters["width"])),

            "geometry.surface_area_volume.cuboid" =>
                parameters["length"] > 0 &&
                parameters["width"] > 0 &&
                parameters["height"] > 0 &&
                value == (parameters["mode"] == 0
                    ? parameters["length"] * parameters["width"] * parameters["height"]
                    : 2 * (
                        parameters["length"] * parameters["width"] +
                        parameters["length"] * parameters["height"] +
                        parameters["width"] * parameters["height"])),

            "geometry.right_triangle.pythagorean.exact" =>
                parameters["a"] > 0 &&
                parameters["b"] > 0 &&
                value == parameters["c"] &&
                parameters["a"] * parameters["a"] + parameters["b"] * parameters["b"] ==
                    value * value,

            "trigonometry.right_triangle.solve_side.special" =>
                value == parameters["answer"] &&
                parameters["answer"] > 0,

            "trigonometry.right_triangle.solve_angle.special" =>
                value == parameters["angle"] &&
                value is 30 or 45 or 60,

            "trigonometry.modelling.right_triangle" =>
                value == parameters["height"] &&
                parameters["height"] > 0 &&
                parameters["distance"] > 0,

            _ => false
        };
    }

    private static ExactProblem BuildProblem(
        string family,
        Random random,
        ExactSkillQuestionDifficulty difficulty,
        int? curriculumLogicalLevel)
    {
        var scale = difficulty switch
        {
            ExactSkillQuestionDifficulty.Stretch => 2,
            ExactSkillQuestionDifficulty.Challenge => 3,
            _ => 1
        };
        var gradeScale = curriculumLogicalLevel switch
        {
            >= 12 => 4,
            >= 10 => 3,
            >= 7 => 2,
            _ => 1
        };
        var effectiveScale = Math.Max(scale, gradeScale);

        return family switch
        {
            "algebra.relationships.two_unknowns.total_difference" =>
                BuildTwoUnknowns(random, effectiveScale),
            "measurement.scale.equal_intervals.read_value" =>
                BuildScaleReading(random, effectiveScale),
            "fractions.compare.unlike.common_denominator" =>
                BuildUnlikeFractionComparison(random, effectiveScale),
            "fractions.equivalent.missing_value" =>
                BuildEquivalentMissingValue(random, effectiveScale, family, "Complete the equivalent fraction"),
            "fractions.equivalent.recognize" =>
                BuildEquivalentMissingValue(random, effectiveScale, family, "Find the missing numerator so both fractions have the same value"),
            "fractions.equivalent.generate_multiple" =>
                BuildEquivalentMissingValue(random, effectiveScale + 1, family, "Generate the equivalent fraction by completing the numerator"),
            "fractions.equivalent.number_line" =>
                BuildEquivalentMissingValue(random, effectiveScale, family, "The fractions mark the same point on a number line. Complete the numerator"),
            "fractions.equivalent.reduce_common_factor" =>
                BuildEquivalentReduction(random, effectiveScale),
            "ratio.unit_rate.direct" =>
                BuildUnitRate(random, effectiveScale),
            "ratio.unit_rate.equivalent_ratio" =>
                BuildEquivalentRatio(random, effectiveScale),
            "number.whole.add_subtract.within_10" =>
                BuildWholeNumberAddSubtract(random, family, 10, requireRegrouping: false, contextual: false),
            "number.whole.add_subtract.across_10" =>
                BuildWholeNumberAddSubtract(random, family, 40 + 20 * effectiveScale, requireRegrouping: true, contextual: false),
            "number.whole.add_subtract.within_100" =>
                BuildWholeNumberAddSubtract(random, family, 100, requireRegrouping: false, contextual: false),
            "number.whole.add_subtract.columnar" =>
                BuildWholeNumberAddSubtract(random, family, 999, requireRegrouping: true, contextual: false),
            "number.whole.add_subtract.contextual_within_100" =>
                BuildWholeNumberAddSubtract(random, family, 100, requireRegrouping: false, contextual: true),
            "number.whole.add_subtract.contextual_across_10" =>
                BuildWholeNumberAddSubtract(random, family, 40 + 20 * effectiveScale, requireRegrouping: true, contextual: true),
            "fractions.notation.identify_part" =>
                BuildFractionNotation(random, effectiveScale),
            "fractions.of_quantity.exact" =>
                BuildFractionOfQuantity(random, effectiveScale),
            "fractions.number_line.read" =>
                BuildFractionNumberLine(random, effectiveScale),
            "fractions.mixed_numbers.number_line.improper_numerator" =>
                BuildMixedNumberImproperNumerator(random, effectiveScale, family, numberLine: true),
            "fractions.mixed_improper.convert_to_improper_numerator" =>
                BuildMixedNumberImproperNumerator(random, effectiveScale, family, numberLine: false),
            "fractions.simplify.lowest_terms" =>
                BuildSimplifyFraction(random, effectiveScale),
            "fractions.common_denominator.missing_numerator" =>
                BuildCommonDenominator(random, effectiveScale),
            "fractions.multiply_by_whole.exact" =>
                BuildMultiplyFractionByWhole(random, effectiveScale),
            "fractions.multiply.exact" =>
                BuildMultiplyFractions(random, effectiveScale),
            "fractions.divide_by_whole.exact" =>
                BuildDivideFractionByWhole(random, effectiveScale),
            "fractions.add_subtract.exact" =>
                BuildAddSubtractFractions(random, effectiveScale),
            "fractions.compare.general" =>
                BuildCompareFractions(random, effectiveScale),
            "fractions.compare.benchmark_half" =>
                BuildCompareFractionToHalf(random, effectiveScale),
            "algebra.linear.ax_plus_b_equals_c" =>
                BuildLinearEquation(random, effectiveScale),
            "geometry.coordinate.straight_line.gradient" =>
                BuildCoordinateGradient(random, effectiveScale),
            "geometry.angles.relationships.missing_angle" =>
                BuildAngleRelationship(random, effectiveScale),
            "geometry.congruence.identify_criterion" =>
                BuildCongruenceCriterion(random),
            "geometry.similarity.missing_length" =>
                BuildSimilarityMissingLength(random, effectiveScale),
            "geometry.perimeter_area.rectangle" =>
                BuildRectangleMeasure(random, effectiveScale),
            "geometry.surface_area_volume.cuboid" =>
                BuildCuboidMeasure(random, effectiveScale),
            "geometry.right_triangle.pythagorean.exact" =>
                BuildPythagorean(random, effectiveScale),
            "trigonometry.right_triangle.sin_cos_tan.ratio" =>
                BuildTrigRatio(random, effectiveScale),
            "trigonometry.right_triangle.solve_side.special" =>
                BuildTrigSolveSide(random, effectiveScale),
            "trigonometry.right_triangle.solve_angle.special" =>
                BuildTrigSolveAngle(random, effectiveScale),
            "trigonometry.modelling.right_triangle" =>
                BuildTrigModelling(random, effectiveScale),
            ExactLinearInequalityQuestionFactory.FamilyId =>
                BuildLinearInequality(random, Math.Min(3, effectiveScale)),
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

    private static ExactProblem BuildFractionNotation(Random random, int scale)
    {
        var d = random.Next(3, 9 + scale);
        var n = random.Next(1, d);
        var part = random.Next(0, 2);
        return Problem(
            "fractions.notation.identify_part",
            part == 0
                ? $"In the fraction {n}/{d}, what is the numerator?"
                : $"In the fraction {n}/{d}, what is the denominator?",
            part == 0
                ? $"The numerator is the number above the fraction bar, so it is {n}."
                : $"The denominator is the number below the fraction bar, so it is {d}.",
            AssessmentItemType.Numeric,
            ("n", n), ("d", d), ("part", part));
    }

    private static ExactProblem BuildFractionOfQuantity(Random random, int scale)
    {
        var d = random.Next(2, 7 + scale);
        var n = random.Next(1, d);
        var groups = random.Next(2, 9 + scale * 2);
        var quantity = d * groups;
        return Problem(
            "fractions.of_quantity.exact",
            $"Find {n}/{d} of {quantity}.",
            $"Divide {quantity} into {d} equal parts, then take {n} part(s).",
            AssessmentItemType.Numeric,
            ("n", n), ("d", d), ("quantity", quantity));
    }

    private static ExactProblem BuildFractionNumberLine(Random random, int scale)
    {
        var d = random.Next(3, 8 + scale);
        var position = random.Next(1, d);
        return Problem(
            "fractions.number_line.read",
            $"A number line from 0 to 1 is split into {d} equal parts. A point is {position} part(s) after 0. What numerator gives its fraction with denominator {d}?",
            $"Each interval is 1/{d}. Moving {position} equal interval(s) gives {position}/{d}.",
            AssessmentItemType.Numeric,
            ("d", d), ("position", position));
    }

    private static ExactProblem BuildMixedNumberImproperNumerator(
        Random random,
        int scale,
        string family,
        bool numberLine)
    {
        var whole = random.Next(1, 4 + scale);
        var d = random.Next(2, 7 + scale);
        var n = random.Next(1, d);
        var prompt = numberLine
            ? $"The point {whole} {n}/{d} is marked on a number line. Write it as an improper fraction with denominator {d}. Enter only the numerator."
            : $"Convert {whole} {n}/{d} to an improper fraction with denominator {d}. Enter only the numerator.";
        return Problem(
            family,
            prompt,
            $"Multiply the whole number by {d}, then add {n}: {whole} × {d} + {n}.",
            AssessmentItemType.Numeric,
            ("whole", whole), ("n", n), ("d", d));
    }

    private static ExactProblem BuildSimplifyFraction(Random random, int scale)
    {
        var baseD = random.Next(3, 9 + scale);
        var baseN = random.Next(1, baseD);
        var common = GreatestCommonDivisor(baseN, baseD);
        baseN /= common;
        baseD /= common;
        var factor = random.Next(2, 5 + scale);
        var n = baseN * factor;
        var d = baseD * factor;
        return Problem(
            "fractions.simplify.lowest_terms",
            $"Simplify {n}/{d} to lowest terms.",
            $"Divide numerator and denominator by their greatest common factor. The lowest-terms fraction is {baseN}/{baseD}.",
            AssessmentItemType.ShortAnswer,
            ("n", n), ("d", d));
    }

    private static ExactProblem BuildCommonDenominator(Random random, int scale)
    {
        var baseD = random.Next(2, 7 + scale);
        var baseN = random.Next(1, baseD);
        var factor = random.Next(2, 5 + scale);
        var targetD = baseD * factor;
        return Problem(
            "fractions.common_denominator.missing_numerator",
            $"Express {baseN}/{baseD} with denominator {targetD}. Enter the numerator.",
            $"Multiply numerator and denominator by {factor}.",
            AssessmentItemType.Numeric,
            ("baseN", baseN), ("baseD", baseD), ("targetD", targetD));
    }

    private static ExactProblem BuildMultiplyFractionByWhole(Random random, int scale)
    {
        var d = random.Next(2, 8 + scale);
        var n = random.Next(1, d);
        var whole = random.Next(2, 5 + scale);
        return Problem(
            "fractions.multiply_by_whole.exact",
            $"Calculate {whole} × {n}/{d}. Give the answer as a simplified fraction.",
            $"Multiply the numerator by {whole}, keep denominator {d}, then simplify.",
            AssessmentItemType.ShortAnswer,
            ("n", n), ("d", d), ("whole", whole));
    }

    private static ExactProblem BuildMultiplyFractions(Random random, int scale)
    {
        var d1 = random.Next(2, 7 + scale);
        var d2 = random.Next(2, 7 + scale);
        var n1 = random.Next(1, d1);
        var n2 = random.Next(1, d2);
        return Problem(
            "fractions.multiply.exact",
            $"Calculate {n1}/{d1} × {n2}/{d2}. Give the answer as a simplified fraction.",
            "Multiply numerators, multiply denominators, then simplify by the greatest common divisor.",
            AssessmentItemType.ShortAnswer,
            ("n1", n1), ("d1", d1), ("n2", n2), ("d2", d2));
    }

    private static ExactProblem BuildDivideFractionByWhole(Random random, int scale)
    {
        var d = random.Next(2, 7 + scale);
        var n = random.Next(1, d);
        var whole = random.Next(2, 5 + scale);
        return Problem(
            "fractions.divide_by_whole.exact",
            $"Calculate {n}/{d} ÷ {whole}. Give the answer as a simplified fraction.",
            $"Dividing by {whole} is multiplying by 1/{whole}; then simplify.",
            AssessmentItemType.ShortAnswer,
            ("n", n), ("d", d), ("whole", whole));
    }

    private static ExactProblem BuildAddSubtractFractions(Random random, int scale)
    {
        for (var retry = 0; retry < 64; retry++)
        {
            var d1 = random.Next(2, 8 + scale);
            var d2 = random.Next(2, 8 + scale);
            var n1 = random.Next(1, d1);
            var n2 = random.Next(1, d2);
            var operation = random.Next(0, 2);
            var numerator = operation == 0
                ? n1 * d2 + n2 * d1
                : n1 * d2 - n2 * d1;
            if (numerator <= 0)
                continue;
            return Problem(
                "fractions.add_subtract.exact",
                $"Calculate {n1}/{d1} {(operation == 0 ? "+" : "−")} {n2}/{d2}. Give the answer as a simplified fraction.",
                "Use a common denominator, combine the numerators, then simplify.",
                AssessmentItemType.ShortAnswer,
                ("n1", n1), ("d1", d1), ("n2", n2), ("d2", d2), ("operation", operation));
        }
        throw new InvalidOperationException("Unable to construct a positive fraction add/subtract item.");
    }

    private static ExactProblem BuildCompareFractions(Random random, int scale)
    {
        var d1 = random.Next(2, 9 + scale);
        var d2 = random.Next(2, 9 + scale);
        var n1 = random.Next(1, d1);
        var n2 = random.Next(1, d2);
        return Problem(
            "fractions.compare.general",
            $"Compare {n1}/{d1} and {n2}/{d2}. Enter <, >, or =.",
            "Compare exact cross-products or express the fractions with a common denominator.",
            AssessmentItemType.ShortAnswer,
            ("n1", n1), ("d1", d1), ("n2", n2), ("d2", d2));
    }

    private static ExactProblem BuildCompareFractionToHalf(Random random, int scale)
    {
        var d = random.Next(3, 10 + scale);
        var n = random.Next(1, d);
        return Problem(
            "fractions.compare.benchmark_half",
            $"Compare {n}/{d} with 1/2. Enter <, >, or =.",
            $"Compare 2 × {n} with {d}; this compares {n}/{d} directly with the benchmark 1/2.",
            AssessmentItemType.ShortAnswer,
            ("n", n), ("d", d));
    }

    private static ExactProblem BuildLinearEquation(Random random, int scale)
    {
        var x = NonZeroSigned(random, 6 + scale * 4);
        var a = NonZeroSigned(random, 3 + scale * 2);
        var b = random.Next(-8 * scale, 8 * scale + 1);
        var right = checked(a * x + b);
        return Problem(
            "algebra.linear.ax_plus_b_equals_c",
            $"Solve {FormatLinearExpression(a, b)} = {right}. Enter x.",
            $"Subtract the constant term and divide by {a}; verify by substituting the result into the original equation.",
            AssessmentItemType.Numeric,
            ("a", a), ("b", b), ("c", right), ("x", x));
    }

    private static int NonZeroSigned(Random random, int limit)
    {
        var magnitude = random.Next(1, Math.Max(2, limit + 1));
        return random.Next(0, 2) == 0 ? magnitude : -magnitude;
    }

    private static ExactProblem BuildWholeNumberAddSubtract(
        Random random,
        string family,
        int maximum,
        bool requireRegrouping,
        bool contextual)
    {
        for (var attempt = 0; attempt < 128; attempt++)
        {
            var operation = random.Next(0, 2); // 0 = addition, 1 = subtraction.
            int left;
            int right;
            int answer;

            if (operation == 0)
            {
                left = random.Next(1, maximum);
                right = random.Next(1, maximum);
                answer = left + right;
                if (answer > maximum)
                    continue;

                if (requireRegrouping &&
                    (left % 10) + (right % 10) < 10)
                {
                    continue;
                }
            }
            else
            {
                answer = random.Next(0, maximum);
                right = random.Next(1, Math.Max(2, maximum - answer + 1));
                left = answer + right;
                if (left > maximum)
                    continue;

                if (requireRegrouping &&
                    left >= 10 &&
                    (left % 10) >= (right % 10))
                {
                    continue;
                }
            }

            if (family == "number.whole.add_subtract.within_10" &&
                (left > 10 || right > 10 || answer > 10))
            {
                continue;
            }

            var symbol = operation == 0 ? "+" : "−";
            var prompt = contextual
                ? operation == 0
                    ? $"A class has {left} counters and receives {right} more. How many counters are there altogether?"
                    : $"A box has {left} counters and {right} are removed. How many counters remain?"
                : $"Calculate {left} {symbol} {right}.";

            var solution = operation == 0
                ? $"Add {left} and {right}: {left} + {right} = {answer}. Check by subtracting one addend from the total."
                : $"Subtract {right} from {left}: {left} − {right} = {answer}. Check by adding {answer} and {right}.";

            return Problem(
                family,
                prompt,
                solution,
                AssessmentItemType.Numeric,
                ("operation", operation),
                ("left", left),
                ("right", right),
                ("maximum", maximum),
                ("regrouping", requireRegrouping ? 1 : 0));
        }

        throw new InvalidOperationException(
            $"Unable to construct a bounded whole-number add/subtract item for {family}.");
    }

    private static bool VerifyWholeNumberAddSubtract(
        string family,
        IReadOnlyDictionary<string, int> parameters,
        int answer)
    {
        if (!parameters.TryGetValue("operation", out var operation) ||
            !parameters.TryGetValue("left", out var left) ||
            !parameters.TryGetValue("right", out var right) ||
            !parameters.TryGetValue("maximum", out var maximum) ||
            left < 0 ||
            right < 0 ||
            maximum < 1)
        {
            return false;
        }

        var expected = operation switch
        {
            0 => left + right,
            1 when left >= right => left - right,
            _ => int.MinValue
        };
        if (answer != expected || answer < 0 || answer > maximum)
            return false;

        if (family == "number.whole.add_subtract.within_10" &&
            (left > 10 || right > 10 || answer > 10))
        {
            return false;
        }

        if (family is "number.whole.add_subtract.across_10" or
            "number.whole.add_subtract.contextual_across_10" or
            "number.whole.add_subtract.columnar")
        {
            var regrouping = operation == 0
                ? (left % 10) + (right % 10) >= 10
                : left >= 10 && (left % 10) < (right % 10);
            if (!regrouping)
                return false;
        }

        if (family is "number.whole.add_subtract.within_100" or
            "number.whole.add_subtract.contextual_within_100")
        {
            if (left > 100 || right > 100 || answer > 100)
                return false;
        }

        return true;
    }

    private static ExactProblem BuildCoordinateGradient(Random random, int scale)
    {
        var x1 = random.Next(-4 * scale, 4 * scale + 1);
        var run = random.Next(1, 3 + scale);
        var gradient = NonZeroSigned(random, 2 + scale);
        var x2 = x1 + run;
        var y1 = random.Next(-5 * scale, 5 * scale + 1);
        var y2 = y1 + gradient * run;
        return ProblemWithRepresentation(
            "geometry.coordinate.straight_line.gradient",
            $"Find the gradient of the line through ({x1}, {y1}) and ({x2}, {y2}).",
            $"Gradient = change in y ÷ change in x = ({y2} − {y1}) ÷ ({x2} − {x1}) = {gradient}.",
            AssessmentItemType.Numeric,
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["type"] = "coordinate_line",
                ["pointA"] = $"{x1},{y1}",
                ["pointB"] = $"{x2},{y2}",
                ["renderHint"] = "cartesian-grid"
            },
            ("x1", x1), ("y1", y1), ("x2", x2), ("y2", y2), ("gradient", gradient));
    }

    private static ExactProblem BuildAngleRelationship(Random random, int scale)
    {
        var mode = random.Next(0, 3);
        var total = mode switch { 0 => 90, 1 => 180, _ => 180 };
        var known = mode == 2
            ? random.Next(25, 75)
            : random.Next(15, total - 15);
        var answer = total - known;
        var prompt = mode switch
        {
            0 => $"Two angles form a right angle. One is {known}°. Find the other angle.",
            1 => $"Two adjacent angles form a straight line. One is {known}°. Find the other angle.",
            _ => $"A triangle has two equal angles of {known / 2}° each and a third angle x. Find x."
        };
        if (mode == 2)
        {
            var equal = random.Next(25, 70);
            known = 2 * equal;
            answer = 180 - known;
            prompt = $"A triangle has two equal angles of {equal}° each. Find the third angle.";
        }
        return ProblemWithRepresentation(
            "geometry.angles.relationships.missing_angle",
            prompt,
            $"Use the relevant angle total {total}° and subtract the known angle contribution {known}°.",
            AssessmentItemType.Numeric,
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["type"] = mode == 2 ? "triangle_angles" : "adjacent_angles",
                ["renderHint"] = "labelled-angle-diagram"
            },
            ("mode", mode), ("total", total), ("known", known), ("answer", answer));
    }

    private static ExactProblem BuildCongruenceCriterion(Random random)
    {
        var criterion = random.Next(0, 3);
        var prompt = criterion switch
        {
            0 => "Two triangles have all three corresponding side lengths equal. Which congruence criterion proves they are congruent? Enter SSS, SAS, or ASA.",
            1 => "Two triangles have two corresponding sides equal and the included angle equal. Which congruence criterion proves they are congruent? Enter SSS, SAS, or ASA.",
            _ => "Two triangles have two corresponding angles equal and the included side equal. Which congruence criterion proves they are congruent? Enter SSS, SAS, or ASA."
        };
        return ProblemWithRepresentation(
            "geometry.congruence.identify_criterion",
            prompt,
            criterion switch
            {
                0 => "Three corresponding side pairs are equal, so SSS applies.",
                1 => "Two sides and their included angle are equal, so SAS applies.",
                _ => "Two angles and the included side are equal, so ASA applies."
            },
            AssessmentItemType.ShortAnswer,
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["type"] = "triangle_congruence",
                ["renderHint"] = "paired-triangles"
            },
            ("criterion", criterion));
    }

    private static ExactProblem BuildSimilarityMissingLength(Random random, int scale)
    {
        var baseLength = random.Next(2, 7 + scale);
        var scaleFactor = random.Next(2, 4 + scale);
        var target = baseLength * scaleFactor;
        return ProblemWithRepresentation(
            "geometry.similarity.missing_length",
            $"Two similar shapes have scale factor {scaleFactor} from the smaller to the larger. A corresponding side on the smaller shape is {baseLength}. Find the larger side.",
            $"Multiply the corresponding length by the scale factor: {baseLength} × {scaleFactor} = {target}.",
            AssessmentItemType.Numeric,
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["type"] = "similar_shapes",
                ["scaleFactor"] = scaleFactor.ToString(CultureInfo.InvariantCulture),
                ["renderHint"] = "paired-labelled-shapes"
            },
            ("base", baseLength), ("scale", scaleFactor), ("target", target));
    }

    private static ExactProblem BuildRectangleMeasure(Random random, int scale)
    {
        var length = random.Next(4, 9 + 2 * scale);
        var width = random.Next(2, length);
        var mode = random.Next(0, 2);
        var answer = mode == 0 ? length * width : 2 * (length + width);
        return ProblemWithRepresentation(
            "geometry.perimeter_area.rectangle",
            mode == 0
                ? $"A rectangle is {length} cm by {width} cm. Find its area."
                : $"A rectangle is {length} cm by {width} cm. Find its perimeter.",
            mode == 0
                ? $"Area = length × width = {length} × {width} = {answer} cm²."
                : $"Perimeter = 2(length + width) = 2({length} + {width}) = {answer} cm.",
            AssessmentItemType.Numeric,
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["type"] = "rectangle",
                ["length"] = length.ToString(CultureInfo.InvariantCulture),
                ["width"] = width.ToString(CultureInfo.InvariantCulture),
                ["renderHint"] = "dimensioned-rectangle"
            },
            ("length", length), ("width", width), ("mode", mode));
    }

    private static ExactProblem BuildCuboidMeasure(Random random, int scale)
    {
        var length = random.Next(3, 7 + scale);
        var width = random.Next(2, 6 + scale);
        var height = random.Next(2, 5 + scale);
        var mode = random.Next(0, 2);
        var answer = mode == 0
            ? length * width * height
            : 2 * (length * width + length * height + width * height);
        return ProblemWithRepresentation(
            "geometry.surface_area_volume.cuboid",
            mode == 0
                ? $"A cuboid measures {length} cm by {width} cm by {height} cm. Find its volume."
                : $"A cuboid measures {length} cm by {width} cm by {height} cm. Find its total surface area.",
            mode == 0
                ? $"Volume = {length} × {width} × {height} = {answer} cm³."
                : $"Surface area = 2(lw + lh + wh) = {answer} cm².",
            AssessmentItemType.Numeric,
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["type"] = "cuboid",
                ["length"] = length.ToString(CultureInfo.InvariantCulture),
                ["width"] = width.ToString(CultureInfo.InvariantCulture),
                ["height"] = height.ToString(CultureInfo.InvariantCulture),
                ["renderHint"] = "dimensioned-cuboid"
            },
            ("length", length), ("width", width), ("height", height), ("mode", mode));
    }

    private static ExactProblem BuildPythagorean(Random random, int scale)
    {
        var factor = random.Next(1, 2 + scale);
        var a = 3 * factor;
        var b = 4 * factor;
        var hyp = 5 * factor;
        return ProblemWithRepresentation(
            "geometry.right_triangle.pythagorean.exact",
            $"A right triangle has perpendicular sides {a} cm and {b} cm. Find the hypotenuse.",
            $"Use a² + b² = c²: {a * a} + {b * b} = {hyp * hyp}, so c = {hyp}.",
            AssessmentItemType.Numeric,
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["type"] = "right_triangle",
                ["legA"] = a.ToString(CultureInfo.InvariantCulture),
                ["legB"] = b.ToString(CultureInfo.InvariantCulture),
                ["unknown"] = "hypotenuse",
                ["renderHint"] = "right-triangle"
            },
            ("a", a), ("b", b), ("c", hyp));
    }

    private static ExactProblem BuildTrigRatio(Random random, int scale)
    {
        var factor = random.Next(1, 2 + scale);
        var opposite = 3 * factor;
        var adjacent = 4 * factor;
        var hypotenuse = 5 * factor;
        var function = random.Next(0, 3);
        var name = function switch { 0 => "sin", 1 => "cos", _ => "tan" };
        return ProblemWithRepresentation(
            "trigonometry.right_triangle.sin_cos_tan.ratio",
            $"In a right triangle relative to angle θ, opposite = {opposite}, adjacent = {adjacent}, hypotenuse = {hypotenuse}. Find {name}(θ) as a simplified fraction.",
            function switch
            {
                0 => "sin θ = opposite/hypotenuse.",
                1 => "cos θ = adjacent/hypotenuse.",
                _ => "tan θ = opposite/adjacent."
            },
            AssessmentItemType.ShortAnswer,
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["type"] = "right_triangle",
                ["opposite"] = opposite.ToString(CultureInfo.InvariantCulture),
                ["adjacent"] = adjacent.ToString(CultureInfo.InvariantCulture),
                ["hypotenuse"] = hypotenuse.ToString(CultureInfo.InvariantCulture),
                ["angleLabel"] = "theta",
                ["renderHint"] = "right-triangle-labelled"
            },
            ("function", function), ("opposite", opposite), ("adjacent", adjacent), ("hypotenuse", hypotenuse));
    }

    private static ExactProblem BuildTrigSolveSide(Random random, int scale)
    {
        var mode = random.Next(0, 3);
        var factor = random.Next(2, 5 + scale);
        int answer;
        string prompt;
        string solution;
        if (mode == 0)
        {
            var hypotenuse = 2 * factor;
            answer = factor;
            prompt = $"A right triangle has hypotenuse {hypotenuse} cm and an acute angle of 30°. Find the side opposite 30°. Use sin 30° = 1/2.";
            solution = $"opposite = {hypotenuse} × 1/2 = {answer} cm.";
        }
        else if (mode == 1)
        {
            var hypotenuse = 2 * factor;
            answer = factor;
            prompt = $"A right triangle has hypotenuse {hypotenuse} cm and an acute angle of 60°. Find the adjacent side. Use cos 60° = 1/2.";
            solution = $"adjacent = {hypotenuse} × 1/2 = {answer} cm.";
        }
        else
        {
            var adjacent = factor;
            answer = factor;
            prompt = $"A right triangle has an acute angle of 45° and adjacent side {adjacent} cm. Find the opposite side. Use tan 45° = 1.";
            solution = $"opposite = {adjacent} × 1 = {answer} cm.";
        }
        return ProblemWithRepresentation(
            "trigonometry.right_triangle.solve_side.special",
            prompt,
            solution,
            AssessmentItemType.Numeric,
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["type"] = "right_triangle",
                ["specialAngleMode"] = mode.ToString(CultureInfo.InvariantCulture),
                ["renderHint"] = "right-triangle-labelled"
            },
            ("mode", mode), ("answer", answer));
    }

    private static ExactProblem BuildTrigSolveAngle(Random random, int scale)
    {
        var mode = random.Next(0, 3);
        var angle = mode switch { 0 => 30, 1 => 45, _ => 60 };
        var prompt = mode switch
        {
            0 => "In a right triangle, sin θ = 1/2 and θ is acute. Find θ in degrees.",
            1 => "In a right triangle, tan θ = 1 and θ is acute. Find θ in degrees.",
            _ => "In a right triangle, cos θ = 1/2 and θ is acute. Find θ in degrees."
        };
        return ProblemWithRepresentation(
            "trigonometry.right_triangle.solve_angle.special",
            prompt,
            $"Use the exact special-angle ratio; θ = {angle}°.",
            AssessmentItemType.Numeric,
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["type"] = "right_triangle",
                ["unknown"] = "angle",
                ["renderHint"] = "right-triangle-labelled"
            },
            ("mode", mode), ("angle", angle));
    }

    private static ExactProblem BuildTrigModelling(Random random, int scale)
    {
        var factor = random.Next(2, 5 + scale);
        var height = 3 * factor;
        var distance = 4 * factor;
        return ProblemWithRepresentation(
            "trigonometry.modelling.right_triangle",
            $"From a point {distance} m from the base of a vertical object, the line of sight forms a right-triangle model with tan θ = 3/4. Find the object's height.",
            $"tan θ = height/distance = 3/4, so height = {distance} × 3/4 = {height} m.",
            AssessmentItemType.Numeric,
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["type"] = "right_triangle_model",
                ["horizontalDistance"] = distance.ToString(CultureInfo.InvariantCulture),
                ["unknown"] = "verticalHeight",
                ["renderHint"] = "right-triangle-context"
            },
            ("height", height), ("distance", distance));
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

            "fractions.notation.identify_part" =>
                (p["part"] == 0 ? p["n"] : p["d"]).ToString(CultureInfo.InvariantCulture),

            "fractions.of_quantity.exact" =>
                ((p["quantity"] / p["d"]) * p["n"]).ToString(CultureInfo.InvariantCulture),

            "fractions.number_line.read" =>
                p["position"].ToString(CultureInfo.InvariantCulture),

            "fractions.mixed_numbers.number_line.improper_numerator" or
            "fractions.mixed_improper.convert_to_improper_numerator" =>
                (p["whole"] * p["d"] + p["n"]).ToString(CultureInfo.InvariantCulture),

            "fractions.simplify.lowest_terms" or
            "fractions.multiply_by_whole.exact" or
            "fractions.multiply.exact" or
            "fractions.divide_by_whole.exact" or
            "fractions.add_subtract.exact" =>
                ExpectedFractionAnswer(problem.Family, p),

            "fractions.common_denominator.missing_numerator" =>
                (p["baseN"] * (p["targetD"] / p["baseD"])).ToString(CultureInfo.InvariantCulture),

            "fractions.compare.general" =>
                Compare(p["n1"] * p["d2"], p["n2"] * p["d1"]),

            "fractions.compare.benchmark_half" =>
                Compare(p["n"] * 2, p["d"]),

            "algebra.linear.ax_plus_b_equals_c" =>
                p["x"].ToString(CultureInfo.InvariantCulture),

            "geometry.coordinate.straight_line.gradient" =>
                p["gradient"].ToString(CultureInfo.InvariantCulture),

            "geometry.angles.relationships.missing_angle" =>
                (p["total"] - p["known"]).ToString(CultureInfo.InvariantCulture),

            "geometry.congruence.identify_criterion" =>
                p["criterion"] switch { 0 => "SSS", 1 => "SAS", 2 => "ASA", _ => throw new InvalidOperationException("Invalid congruence criterion.") },

            "geometry.similarity.missing_length" =>
                (p["base"] * p["scale"]).ToString(CultureInfo.InvariantCulture),

            "geometry.perimeter_area.rectangle" =>
                (p["mode"] == 0
                    ? p["length"] * p["width"]
                    : 2 * (p["length"] + p["width"])).ToString(CultureInfo.InvariantCulture),

            "geometry.surface_area_volume.cuboid" =>
                (p["mode"] == 0
                    ? p["length"] * p["width"] * p["height"]
                    : 2 * (p["length"] * p["width"] + p["length"] * p["height"] + p["width"] * p["height"]))
                    .ToString(CultureInfo.InvariantCulture),

            "geometry.right_triangle.pythagorean.exact" =>
                p["c"].ToString(CultureInfo.InvariantCulture),

            "trigonometry.right_triangle.sin_cos_tan.ratio" =>
                ExpectedTrigRatio(p),

            "trigonometry.right_triangle.solve_side.special" =>
                p["answer"].ToString(CultureInfo.InvariantCulture),

            "trigonometry.right_triangle.solve_angle.special" =>
                p["angle"].ToString(CultureInfo.InvariantCulture),

            "trigonometry.modelling.right_triangle" =>
                p["height"].ToString(CultureInfo.InvariantCulture),

            ExactLinearInequalityQuestionFactory.FamilyId =>
                SolveLinearInequality(p),

            _ => throw new InvalidOperationException($"Unsupported exact Mathematics solver family: {problem.Family}")
        };
    }

    private static string ExpectedTrigRatio(IReadOnlyDictionary<string, int> p) =>
        p["function"] switch
        {
            0 => ReduceFraction(p["opposite"], p["hypotenuse"]),
            1 => ReduceFraction(p["adjacent"], p["hypotenuse"]),
            2 => ReduceFraction(p["opposite"], p["adjacent"]),
            _ => throw new InvalidOperationException("Unsupported trigonometric ratio selector.")
        };

    private static string ExpectedFractionAnswer(
        string family,
        IReadOnlyDictionary<string, int> p)
    {
        return family switch
        {
            "fractions.simplify.lowest_terms" =>
                ReduceFraction(p["n"], p["d"]),
            "fractions.multiply_by_whole.exact" =>
                ReduceFraction(p["whole"] * p["n"], p["d"]),
            "fractions.multiply.exact" =>
                ReduceFraction(p["n1"] * p["n2"], p["d1"] * p["d2"]),
            "fractions.divide_by_whole.exact" =>
                ReduceFraction(p["n"], p["d"] * p["whole"]),
            "fractions.add_subtract.exact" =>
                ReduceFraction(
                    p["operation"] == 0
                        ? p["n1"] * p["d2"] + p["n2"] * p["d1"]
                        : p["n1"] * p["d2"] - p["n2"] * p["d1"],
                    p["d1"] * p["d2"]),
            _ => throw new InvalidOperationException($"Unsupported fraction family: {family}")
        };
    }

    private static string ReduceFraction(int numerator, int denominator)
    {
        if (denominator == 0)
            throw new InvalidOperationException("Fraction denominator cannot be zero.");
        if (denominator < 0)
        {
            numerator = -numerator;
            denominator = -denominator;
        }
        var gcd = GreatestCommonDivisor(Math.Abs(numerator), Math.Abs(denominator));
        numerator /= gcd;
        denominator /= gcd;
        return denominator == 1
            ? numerator.ToString(CultureInfo.InvariantCulture)
            : $"{numerator.ToString(CultureInfo.InvariantCulture)}/{denominator.ToString(CultureInfo.InvariantCulture)}";
    }

    private static int GreatestCommonDivisor(int a, int b)
    {
        if (a == 0) return Math.Max(1, Math.Abs(b));
        if (b == 0) return Math.Max(1, Math.Abs(a));
        a = Math.Abs(a);
        b = Math.Abs(b);
        while (b != 0)
            (a, b) = (b, a % b);
        return Math.Max(1, a);
    }

    private static string NormalizeFractionAnswer(string answer)
    {
        var text = answer.Trim().Replace(" ", string.Empty, StringComparison.Ordinal);
        var slash = text.IndexOf('/');
        if (slash < 0)
        {
            return int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var whole)
                ? whole.ToString(CultureInfo.InvariantCulture)
                : text;
        }

        if (!int.TryParse(text[..slash], NumberStyles.Integer, CultureInfo.InvariantCulture, out var numerator) ||
            !int.TryParse(text[(slash + 1)..], NumberStyles.Integer, CultureInfo.InvariantCulture, out var denominator) ||
            denominator == 0)
        {
            return text;
        }

        return ReduceFraction(numerator, denominator);
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
            parameters.ToDictionary(x => x.Name, x => x.Value, StringComparer.Ordinal),
            new Dictionary<string, string>(StringComparer.Ordinal));

    private static ExactProblem ProblemWithRepresentation(
        string family,
        string prompt,
        string solution,
        AssessmentItemType itemType,
        IReadOnlyDictionary<string, string> representation,
        params (string Name, int Value)[] parameters) =>
        new(
            family,
            prompt,
            solution,
            itemType,
            parameters.ToDictionary(x => x.Name, x => x.Value, StringComparer.Ordinal),
            new Dictionary<string, string>(representation, StringComparer.Ordinal));

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
        IReadOnlyDictionary<string, int> Parameters,
        IReadOnlyDictionary<string, string> Representation);
}
