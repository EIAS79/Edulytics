using Edulytics.Core.AssessmentIntelligence;
using Edulytics.Core.Assessments;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Assessments;

public static class AssessmentBuilderGenerationPlanner
{
    public static AssessmentBuilderDifficulty? ResolveDifficulty(
        AssessmentBuilderDifficulty requestedDifficulty,
        AssessmentDifficultyBand assessmentDifficulty)
    {
        if (requestedDifficulty != 0)
        {
            return Enum.IsDefined(typeof(AssessmentBuilderDifficulty), requestedDifficulty)
                ? requestedDifficulty
                : null;
        }

        return assessmentDifficulty switch
        {
            AssessmentDifficultyBand.AtClassLevel => AssessmentBuilderDifficulty.AtClassLevel,
            AssessmentDifficultyBand.Stretch => AssessmentBuilderDifficulty.Stretch,
            AssessmentDifficultyBand.Challenge => AssessmentBuilderDifficulty.Challenge,
            _ => null
        };
    }

    /// <summary>
    /// Plans item-level difficulty inside the teacher-selected assessment band.
    /// The saved band remains the overall assessment intent; it is not treated
    /// as a command to label every item with one difficulty.
    /// </summary>
    public static IReadOnlyList<AssessmentBuilderDifficulty>? PlanItemDifficulties(
        AssessmentBuilderDifficulty assessmentDifficulty,
        int questionCount,
        int seed = 0)
    {
        if (questionCount is < 1 or > 50 ||
            !Enum.IsDefined(typeof(AssessmentBuilderDifficulty), assessmentDifficulty))
        {
            return null;
        }

        var policy = assessmentDifficulty switch
        {
            AssessmentBuilderDifficulty.AtClassLevel =>
                AssessmentDifficultyPolicy.Balanced,
            AssessmentBuilderDifficulty.Stretch =>
                AssessmentDifficultyPolicy.Stretch,
            AssessmentBuilderDifficulty.Challenge =>
                new AssessmentDifficultyPolicy(5, 30, 65),
            _ => null
        };

        if (policy is null)
            return null;

        var buckets = new[]
        {
            (Difficulty: AssessmentBuilderDifficulty.AtClassLevel, Percent: policy.EasyPercent),
            (Difficulty: AssessmentBuilderDifficulty.Stretch, Percent: policy.MediumPercent),
            (Difficulty: AssessmentBuilderDifficulty.Challenge, Percent: policy.ChallengingPercent)
        };

        var allocations = buckets
            .Select(x =>
            {
                var exact = questionCount * x.Percent / 100m;
                var floor = decimal.ToInt32(decimal.Floor(exact));
                return new
                {
                    x.Difficulty,
                    Count = floor,
                    Remainder = exact - floor
                };
            })
            .ToArray();

        var counts = allocations.ToDictionary(x => x.Difficulty, x => x.Count);
        var assigned = counts.Values.Sum();
        foreach (var allocation in allocations
                     .OrderByDescending(x => x.Remainder)
                     .ThenByDescending(x => x.Difficulty)
                     .Take(questionCount - assigned))
        {
            counts[allocation.Difficulty]++;
        }

        var planned = new List<AssessmentBuilderDifficulty>(questionCount);
        foreach (var bucket in buckets)
        {
            planned.AddRange(
                Enumerable.Repeat(
                    bucket.Difficulty,
                    counts[bucket.Difficulty]));
        }

        // Deterministic Fisher-Yates shuffle prevents the assessment from
        // presenting all easy items first while keeping generation reproducible.
        var random = new Random(seed == 0 ? 1 : seed);
        for (var index = planned.Count - 1; index > 0; index--)
        {
            var swap = random.Next(index + 1);
            (planned[index], planned[swap]) = (planned[swap], planned[index]);
        }

        return planned;
    }

    public static AssessmentItemDifficulty ToItemDifficulty(
        AssessmentBuilderDifficulty difficulty) =>
        difficulty switch
        {
            AssessmentBuilderDifficulty.AtClassLevel => AssessmentItemDifficulty.Easy,
            AssessmentBuilderDifficulty.Stretch => AssessmentItemDifficulty.Medium,
            AssessmentBuilderDifficulty.Challenge => AssessmentItemDifficulty.Challenging,
            _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
        };

    public static IReadOnlyList<decimal>? DistributeMarks(
        decimal remainingMarks,
        IReadOnlyList<AssessmentItemDifficulty> difficulties,
        decimal maxScorePerQuestionOverride)
    {
        if (difficulties.Count == 0 ||
            remainingMarks <= 0m ||
            decimal.Truncate(remainingMarks) != remainingMarks ||
            maxScorePerQuestionOverride < 0m)
            return null;

        if (maxScorePerQuestionOverride > 0m)
        {
            if (!IsWholePositiveScore(maxScorePerQuestionOverride) ||
                maxScorePerQuestionOverride * difficulties.Count > remainingMarks)
                return null;

            return Enumerable.Repeat(maxScorePerQuestionOverride, difficulties.Count).ToArray();
        }

        if (remainingMarks < difficulties.Count)
            return null;

        var weights = difficulties.Select(DifficultyWeight).ToArray();
        if (weights.Any(weight => weight <= 0))
            return null;

        var marks = Enumerable.Repeat(1m, difficulties.Count).ToArray();
        var extraMarks = checked((int)(remainingMarks - difficulties.Count));
        if (extraMarks == 0)
            return marks;

        var totalWeight = weights.Sum();
        var allocatedExtra = 0;
        for (var index = 0; index < marks.Length; index++)
        {
            var additional = extraMarks * weights[index] / totalWeight;
            marks[index] += additional;
            allocatedExtra += additional;
        }

        var leftover = extraMarks - allocatedExtra;
        foreach (var index in Enumerable.Range(0, weights.Length)
                     .OrderByDescending(index => weights[index])
                     .ThenBy(index => index)
                     .Take(leftover))
        {
            marks[index] += 1m;
        }

        return marks;
    }

    private static int DifficultyWeight(AssessmentItemDifficulty difficulty) => difficulty switch
    {
        AssessmentItemDifficulty.Easy => 1,
        AssessmentItemDifficulty.Medium => 2,
        AssessmentItemDifficulty.Challenging => 3,
        _ => 0
    };

    private static bool IsWholePositiveScore(decimal value) =>
        value > 0m && value <= 10000m && decimal.Truncate(value) == value;
}
