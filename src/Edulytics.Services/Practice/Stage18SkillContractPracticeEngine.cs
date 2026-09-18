using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Services.Practice;

/// <summary>
/// Server-side exact Practice generator for Stage 18 READY_VERIFIED lessons.
/// Mathematics is solved first, independently verified, and only then persisted
/// as an AssessmentItem. No contextual provider is reachable from this engine.
/// </summary>
public sealed class Stage18SkillContractPracticeEngine
{
    private const int MaxRetriesPerItem = 128;

    public IReadOnlyList<AssessmentItem> Generate(
        Guid schoolId,
        Guid curriculumAdoptionId,
        Guid lessonId,
        Stage18PracticeSkillContract contract,
        StudentPrivatePracticeDifficulty requestedDifficulty,
        int questionCount,
        int seed,
        IReadOnlyCollection<string> excludedExposureFingerprints,
        Guid createdByUserId)
    {
        ArgumentNullException.ThrowIfNull(contract);
        ArgumentNullException.ThrowIfNull(excludedExposureFingerprints);

        if (schoolId == Guid.Empty ||
            curriculumAdoptionId == Guid.Empty ||
            lessonId == Guid.Empty ||
            createdByUserId == Guid.Empty ||
            questionCount is < 1 or > 30 ||
            contract.AllowedQuestionFamilies.Count == 0)
        {
            throw new InvalidOperationException("Stage 18 Practice generation requires a valid exact lesson contract and request scope.");
        }

        var random = new Random(seed == 0 ? 1 : seed);
        var excluded = excludedExposureFingerprints
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToHashSet(StringComparer.Ordinal);
        var generated = new HashSet<string>(StringComparer.Ordinal);
        var items = new List<AssessmentItem>(questionCount);

        for (var index = 0; index < questionCount; index++)
        {
            AssessmentItem? item = null;

            for (var retry = 0; retry < MaxRetriesPerItem && item is null; retry++)
            {
                var family = contract.AllowedQuestionFamilies[(index + retry) % contract.AllowedQuestionFamilies.Count];
                var problem = BuildProblem(family, random, requestedDifficulty);
                var answer = Solve(problem);

                if (!Verify(problem, answer))
                    throw new InvalidOperationException($"Stage 18 verifier rejected solver output for {family}.");

                var fingerprint = Fingerprint(contract.LessonCode, family, problem.Parameters);
                if (excluded.Contains(fingerprint) || generated.Contains(fingerprint))
                    continue;

                generated.Add(fingerprint);
                item = BuildItem(
                    schoolId,
                    curriculumAdoptionId,
                    lessonId,
                    createdByUserId,
                    contract,
                    requestedDifficulty,
                    problem,
                    answer,
                    fingerprint);
            }

            if (item is null)
                throw new InvalidOperationException("Stage 18 Practice generation exhausted uniqueness retries.");

            items.Add(item);
        }

        return items;
    }

    public static bool VerifyPersistedItem(
        Stage18PracticeSkillContract contract,
        AssessmentItem item)
    {
        ArgumentNullException.ThrowIfNull(contract);
        ArgumentNullException.ThrowIfNull(item);

        if (!string.Equals(item.GenerationMethod, Stage18PracticeSkillContracts.GenerationMethod, StringComparison.Ordinal) ||
            string.IsNullOrWhiteSpace(item.GenerationFamily) ||
            !contract.AllowedQuestionFamilies.Contains(item.GenerationFamily, StringComparer.Ordinal) ||
            string.IsNullOrWhiteSpace(item.GenerationParametersJson))
        {
            return false;
        }

        try
        {
            using var document = JsonDocument.Parse(item.GenerationParametersJson);
            var root = document.RootElement;

            if (!string.Equals(root.GetProperty("skillId").GetString(), contract.SkillId, StringComparison.Ordinal) ||
                !string.Equals(root.GetProperty("questionFamily").GetString(), item.GenerationFamily, StringComparison.Ordinal))
            {
                return false;
            }

            var parameters = root.GetProperty("parameters");
            var values = new Dictionary<string, int>(StringComparer.Ordinal);
            foreach (var property in parameters.EnumerateObject())
                values[property.Name] = property.Value.GetInt32();

            var problem = new ExactPracticeProblem(
                item.GenerationFamily,
                item.Prompt,
                item.Solution,
                item.ItemType,
                values);

            return Verify(problem, item.CorrectAnswer);
        }
        catch (Exception exception) when (
            exception is JsonException or
            KeyNotFoundException or
            InvalidOperationException or
            FormatException)
        {
            return false;
        }
    }

    private static AssessmentItem BuildItem(
        Guid schoolId,
        Guid curriculumAdoptionId,
        Guid lessonId,
        Guid createdByUserId,
        Stage18PracticeSkillContract contract,
        StudentPrivatePracticeDifficulty requestedDifficulty,
        ExactPracticeProblem problem,
        string answer,
        string fingerprint)
    {
        return new AssessmentItem
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            CurriculumAdoptionId = curriculumAdoptionId,
            CurriculumPedagogicalLessonId = lessonId,
            Source = AssessmentItemSource.SystemGenerated,
            ItemType = problem.ItemType,
            Difficulty = ResolveDifficulty(requestedDifficulty),
            Prompt = problem.Prompt,
            CorrectAnswer = answer,
            Solution = problem.Solution,
            CreatedByUserId = createdByUserId,
            GenerationMethod = Stage18PracticeSkillContracts.GenerationMethod,
            GenerationFamily = problem.Family,
            GenerationParametersJson = JsonSerializer.Serialize(new
            {
                skillId = contract.SkillId,
                questionFamily = problem.Family,
                parameters = problem.Parameters
            }),
            ExposureFingerprint = fingerprint,
            ValidationMetadataJson = JsonSerializer.Serialize(new
            {
                stage = 18,
                alignment = "skill-contract-verified",
                readiness = "READY_VERIFIED",
                skillContract = contract.SkillId,
                allowedFamily = problem.Family,
                solver = Stage18PracticeSkillContracts.SolverIdentifier,
                verifier = Stage18PracticeSkillContracts.VerifierIdentifier,
                solverVerified = true,
                broadFallbackUsed = false,
                officialMasteryEvidence = false
            }),
            CreatedAtUtc = DateTime.UtcNow,
            RowVersion = []
        };
    }

    private static ExactPracticeProblem BuildProblem(
        string family,
        Random random,
        StudentPrivatePracticeDifficulty difficulty)
    {
        var scale = difficulty switch
        {
            StudentPrivatePracticeDifficulty.Stretch => 2,
            StudentPrivatePracticeDifficulty.Challenge => 3,
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
            _ => throw new InvalidOperationException($"Unsupported Stage 18 Practice family: {family}")
        };
    }

    private static ExactPracticeProblem BuildTwoUnknowns(Random random, int scale)
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

    private static ExactPracticeProblem BuildScaleReading(Random random, int scale)
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

    private static ExactPracticeProblem BuildUnlikeFractionComparison(Random random, int scale)
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

    private static ExactPracticeProblem BuildEquivalentMissingValue(
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

    private static ExactPracticeProblem BuildEquivalentReduction(Random random, int scale)
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

    private static ExactPracticeProblem BuildUnitRate(Random random, int scale)
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

    private static ExactPracticeProblem BuildEquivalentRatio(Random random, int scale)
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

    private static string Solve(ExactPracticeProblem problem)
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

            _ => throw new InvalidOperationException($"Unsupported Stage 18 solver family: {problem.Family}")
        };
    }

    private static bool Verify(ExactPracticeProblem problem, string answer)
    {
        var p = problem.Parameters;

        if (problem.Family == "fractions.compare.unlike.common_denominator")
            return string.Equals(answer, Compare(p["n1"] * p["d2"], p["n2"] * p["d1"]), StringComparison.Ordinal);

        if (!int.TryParse(answer, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value))
            return false;

        return problem.Family switch
        {
            "algebra.relationships.two_unknowns.total_difference" =>
                value > 0 &&
                p["total"] - value > 0 &&
                value + (p["total"] - value) == p["total"] &&
                (p["total"] - value) - value == p["difference"],

            "measurement.scale.equal_intervals.read_value" =>
                p["intervals"] > 0 &&
                (p["end"] - p["start"]) % p["intervals"] == 0 &&
                value == p["start"] + ((p["end"] - p["start"]) / p["intervals"]) * p["pointer"] &&
                p["start"] + ((p["end"] - p["start"]) / p["intervals"]) * p["intervals"] == p["end"],

            "fractions.equivalent.missing_value" or
            "fractions.equivalent.recognize" or
            "fractions.equivalent.generate_multiple" or
            "fractions.equivalent.number_line" =>
                p["baseD"] > 0 &&
                p["targetD"] > 0 &&
                p["baseN"] * p["targetD"] == value * p["baseD"],

            "fractions.equivalent.reduce_common_factor" =>
                p["largeD"] > 0 &&
                p["targetD"] > 0 &&
                p["largeN"] * p["targetD"] == value * p["largeD"],

            "ratio.unit_rate.direct" =>
                p["quantity"] > 0 &&
                value * p["quantity"] == p["total"],

            "ratio.unit_rate.equivalent_ratio" =>
                p["baseSecond"] > 0 &&
                p["targetSecond"] > 0 &&
                p["baseFirst"] * p["targetSecond"] == value * p["baseSecond"],

            _ => false
        };
    }

    private static ExactPracticeProblem Problem(
        string family,
        string prompt,
        string solution,
        AssessmentItemType itemType,
        params (string Key, int Value)[] parameters) =>
        new(
            family,
            prompt,
            solution,
            itemType,
            parameters.ToDictionary(x => x.Key, x => x.Value, StringComparer.Ordinal));

    private static string Compare(int left, int right) =>
        left < right ? "<" : left > right ? ">" : "=";

    private static AssessmentItemDifficulty ResolveDifficulty(StudentPrivatePracticeDifficulty difficulty) =>
        difficulty switch
        {
            StudentPrivatePracticeDifficulty.Stretch or StudentPrivatePracticeDifficulty.Challenge =>
                AssessmentItemDifficulty.Challenging,
            StudentPrivatePracticeDifficulty.MyLevel =>
                AssessmentItemDifficulty.Medium,
            _ => AssessmentItemDifficulty.Medium
        };

    private static string Fingerprint(
        string lessonCode,
        string family,
        IReadOnlyDictionary<string, int> parameters)
    {
        var canonicalParameters = string.Join(
            ";",
            parameters
                .OrderBy(x => x.Key, StringComparer.Ordinal)
                .Select(x => $"{x.Key}={x.Value.ToString(CultureInfo.InvariantCulture)}"));

        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(
            $"stage18|{lessonCode}|{family}|{canonicalParameters}"));

        return "stage18:" + Convert.ToHexString(bytes).ToLowerInvariant();
    }

    private sealed record ExactPracticeProblem(
        string Family,
        string Prompt,
        string Solution,
        AssessmentItemType ItemType,
        IReadOnlyDictionary<string, int> Parameters);
}
