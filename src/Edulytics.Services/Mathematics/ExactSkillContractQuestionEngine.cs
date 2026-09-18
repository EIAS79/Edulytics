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

        if (family is
            "fractions.add_subtract.related_denominators" or
            "fractions.add_subtract.unlike_denominators" or
            "fractions.add_subtract.mixed_same_denominator")
        {
            var d1 = parameters["d1"];
            var d2 = parameters["d2"];
            var operation = parameters["operation"];
            if (d1 <= 0 || d2 <= 0 || operation is < 0 or > 1)
                return false;

            var numerator = operation == 0
                ? parameters["n1"] * d2 + parameters["n2"] * d1
                : parameters["n1"] * d2 - parameters["n2"] * d1;
            var denominator = d1 * d2;
            if (numerator < 0)
                return false;

            return TryParseRationalAnswer(answer, out var answerNumerator, out var answerDenominator) &&
                answerDenominator > 0 &&
                (long)answerNumerator * denominator ==
                    (long)numerator * answerDenominator;
        }

        if (family is
            "fractions.representations.equal_parts" or
            "fractions.representations.number_line")
        {
            var numerator = parameters["numerator"];
            var denominator = parameters["denominator"];
            return denominator > 0 &&
                numerator >= 0 &&
                numerator <= denominator &&
                TryParseRationalAnswer(answer, out var answerNumerator, out var answerDenominator) &&
                answerDenominator > 0 &&
                (long)answerNumerator * denominator ==
                    (long)numerator * answerDenominator;
        }

        if (family == "fractions.compare.benchmark_half")
        {
            var numerator = parameters["numerator"];
            var denominator = parameters["denominator"];
            if (denominator <= 0)
                return false;

            return string.Equals(
                answer.Trim(),
                Compare(2 * numerator, denominator),
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
            "fractions.add_subtract.related_denominators" or
            "fractions.add_subtract.unlike_denominators" or
            "fractions.add_subtract.mixed_same_denominator" =>
                BuildFractionAddSubtract(family, random, scale),
            "fractions.representations.equal_parts" or
            "fractions.representations.number_line" =>
                BuildFractionRepresentation(family, random, scale),
            "fractions.compare.benchmark_half" =>
                BuildFractionBenchmarkComparison(random, scale),
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
        var operation = random.Next(0, 2);
        int n1;
        int d1;
        int n2;
        int d2;
        string leftText;
        string rightText;

        if (family == "fractions.add_subtract.related_denominators")
        {
            d1 = random.Next(2, 7 + scale);
            var factor = random.Next(2, 4 + scale);
            d2 = d1 * factor;
            n1 = random.Next(1, d1);
            n2 = random.Next(1, d2);

            if (operation == 1 && (long)n1 * d2 < (long)n2 * d1)
                (n1, d1, n2, d2) = (n2, d2, n1, d1);

            leftText = $"{n1}/{d1}";
            rightText = $"{n2}/{d2}";
        }
        else if (family == "fractions.add_subtract.unlike_denominators")
        {
            do
            {
                d1 = random.Next(3, 8 + scale * 2);
                d2 = random.Next(3, 9 + scale * 2);
            }
            while (d1 == d2 || d1 % d2 == 0 || d2 % d1 == 0);

            n1 = random.Next(1, d1);
            n2 = random.Next(1, d2);
            if (operation == 1 && (long)n1 * d2 < (long)n2 * d1)
                (n1, d1, n2, d2) = (n2, d2, n1, d1);

            leftText = $"{n1}/{d1}";
            rightText = $"{n2}/{d2}";
        }
        else
        {
            d1 = d2 = random.Next(3, 8 + scale);
            var whole1 = random.Next(1, 3 + scale);
            var whole2 = random.Next(1, 3 + scale);
            var part1 = random.Next(1, d1);
            var part2 = random.Next(1, d2);
            n1 = whole1 * d1 + part1;
            n2 = whole2 * d2 + part2;

            if (operation == 1 && n1 < n2)
                (n1, n2) = (n2, n1);

            leftText = FormatMixedFraction(n1, d1);
            rightText = FormatMixedFraction(n2, d2);
        }

        var symbol = operation == 0 ? "+" : "−";
        return Problem(
            family,
            $"Calculate {leftText} {symbol} {rightText}. Give the exact answer as an integer or reduced fraction.",
            "Represent both quantities exactly, use a common denominator when required, perform the operation, reduce the result and verify by substitution or inverse reasoning.",
            AssessmentItemType.ShortAnswer,
            ("n1", n1),
            ("d1", d1),
            ("n2", n2),
            ("d2", d2),
            ("operation", operation));
    }

    private static ExactProblem BuildFractionRepresentation(
        string family,
        Random random,
        int scale)
    {
        var denominator = random.Next(3, 9 + scale * 2);
        var numerator = random.Next(1, denominator);
        var prompt = family == "fractions.representations.equal_parts"
            ? $"A whole is divided into {denominator} equal parts and {numerator} parts are shaded. What fraction of the whole is shaded?"
            : $"The interval from 0 to 1 is divided into {denominator} equal intervals. A point is on tick {numerator} after 0. What fraction does the point represent?";

        return Problem(
            family,
            prompt,
            "The denominator gives the number of equal parts in one whole; the numerator counts the selected parts or intervals. State the exact fraction and verify its position is between 0 and 1.",
            AssessmentItemType.ShortAnswer,
            ("numerator", numerator),
            ("denominator", denominator));
    }

    private static ExactProblem BuildFractionBenchmarkComparison(
        Random random,
        int scale)
    {
        var denominator = random.Next(3, 10 + scale * 2);
        var numerator = random.Next(1, denominator);
        return Problem(
            "fractions.compare.benchmark_half",
            $"Compare {numerator}/{denominator} with 1/2. Enter <, >, or =.",
            "Compare 2 × numerator with the denominator. This is an exact integer cross-product check against the benchmark 1/2.",
            AssessmentItemType.ShortAnswer,
            ("numerator", numerator),
            ("denominator", denominator));
    }

    private static ExactProblem BuildLinearEquation(Random random, int scale)
    {
        var factory = new ExactLinearEquationQuestionFactory(
            new ExactLinearEquationSolver(),
            new ExactLinearEquationVerifier());
        var generated = factory.Generate(
            random.Next(1, int.MaxValue),
            Math.Clamp(scale, 1, 3));

        if (generated.Problem is not EquationNode equation ||
            generated.ExpectedAnswer is not EquationNode solved ||
            solved.Left is not SymbolNode symbol ||
            !string.Equals(symbol.Name, "x", StringComparison.Ordinal) ||
            solved.Right is not IntegerNode integerAnswer ||
            integerAnswer.Value < int.MinValue ||
            integerAnswer.Value > int.MaxValue)
        {
            throw new InvalidOperationException(
                "Exact linear-equation factory returned an unsupported learner-facing shape.");
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

            "fractions.add_subtract.related_denominators" or
            "fractions.add_subtract.unlike_denominators" or
            "fractions.add_subtract.mixed_same_denominator" =>
                SolveFractionAddSubtract(p),

            "fractions.representations.equal_parts" or
            "fractions.representations.number_line" =>
                FormatReducedFraction(p["numerator"], p["denominator"]),

            "fractions.compare.benchmark_half" =>
                Compare(2 * p["numerator"], p["denominator"]),

            ExactLinearEquationQuestionFactory.FamilyId =>
                ((p["right"] - p["offset"]) / p["coefficient"])
                    .ToString(CultureInfo.InvariantCulture),

            ExactLinearInequalityQuestionFactory.FamilyId =>
                SolveLinearInequality(p),

            _ => throw new InvalidOperationException($"Unsupported exact Mathematics solver family: {problem.Family}")
        };
    }

    private static string SolveFractionAddSubtract(
        IReadOnlyDictionary<string, int> parameters)
    {
        var numerator = parameters["operation"] == 0
            ? parameters["n1"] * parameters["d2"] + parameters["n2"] * parameters["d1"]
            : parameters["n1"] * parameters["d2"] - parameters["n2"] * parameters["d1"];
        var denominator = parameters["d1"] * parameters["d2"];
        return FormatReducedFraction(numerator, denominator);
    }

    private static string FormatReducedFraction(int numerator, int denominator)
    {
        if (denominator == 0)
            throw new InvalidOperationException("Fraction denominator cannot be zero.");

        if (denominator < 0)
        {
            numerator = -numerator;
            denominator = -denominator;
        }

        var divisor = GreatestCommonDivisor(Math.Abs(numerator), denominator);
        numerator /= divisor;
        denominator /= divisor;
        return denominator == 1
            ? numerator.ToString(CultureInfo.InvariantCulture)
            : $"{numerator.ToString(CultureInfo.InvariantCulture)}/{denominator.ToString(CultureInfo.InvariantCulture)}";
    }

    private static string FormatMixedFraction(int numerator, int denominator)
    {
        var whole = numerator / denominator;
        var remainder = numerator % denominator;
        if (whole == 0)
            return $"{remainder}/{denominator}";
        if (remainder == 0)
            return whole.ToString(CultureInfo.InvariantCulture);
        return $"{whole} {remainder}/{denominator}";
    }

    private static bool TryParseRationalAnswer(
        string answer,
        out int numerator,
        out int denominator)
    {
        numerator = 0;
        denominator = 1;
        var value = answer.Trim().Replace("−", "-", StringComparison.Ordinal);
        if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out numerator))
            return true;

        var slash = value.IndexOf('/');
        if (slash <= 0 || slash != value.LastIndexOf('/'))
            return false;

        return int.TryParse(
                value[..slash].Trim(),
                NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                out numerator) &&
            int.TryParse(
                value[(slash + 1)..].Trim(),
                NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                out denominator) &&
            denominator != 0;
    }

    private static int GreatestCommonDivisor(int left, int right)
    {
        left = Math.Abs(left);
        right = Math.Abs(right);
        while (right != 0)
            (left, right) = (right, left % right);
        return left == 0 ? 1 : left;
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
