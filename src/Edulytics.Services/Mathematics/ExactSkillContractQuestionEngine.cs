using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Edulytics.Core.Enums;

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
                    throw new InvalidOperationException($"Exact Mathematics verifier rejected solver output for {family}.");

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
            _ => throw new InvalidOperationException($"Unsupported exact Mathematics question family: {family}")
        };
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

            _ => throw new InvalidOperationException($"Unsupported exact Mathematics solver family: {problem.Family}")
        };
    }

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
