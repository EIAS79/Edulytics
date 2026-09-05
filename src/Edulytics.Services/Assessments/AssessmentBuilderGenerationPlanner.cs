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
