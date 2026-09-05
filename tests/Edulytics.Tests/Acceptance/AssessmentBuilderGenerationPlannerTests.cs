using Edulytics.Core.Assessments;
using Edulytics.Core.Enums;
using Edulytics.Services.Assessments;

namespace Edulytics.Tests.Acceptance;

public sealed class AssessmentBuilderGenerationPlannerTests
{
    [Theory]
    [InlineData(AssessmentDifficultyBand.AtClassLevel, AssessmentBuilderDifficulty.AtClassLevel)]
    [InlineData(AssessmentDifficultyBand.Stretch, AssessmentBuilderDifficulty.Stretch)]
    [InlineData(AssessmentDifficultyBand.Challenge, AssessmentBuilderDifficulty.Challenge)]
    public void AutoDifficulty_UsesSavedAssessmentDifficulty(
        AssessmentDifficultyBand assessmentDifficulty,
        AssessmentBuilderDifficulty expected)
    {
        var actual = AssessmentBuilderGenerationPlanner.ResolveDifficulty(0, assessmentDifficulty);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ExplicitDifficulty_OverridesSavedAssessmentDifficulty()
    {
        var actual = AssessmentBuilderGenerationPlanner.ResolveDifficulty(
            AssessmentBuilderDifficulty.Challenge,
            AssessmentDifficultyBand.AtClassLevel);

        Assert.Equal(AssessmentBuilderDifficulty.Challenge, actual);
    }

    [Fact]
    public void AutoMarks_DistributeAllRemainingMarksByQuestionDifficulty()
    {
        var marks = AssessmentBuilderGenerationPlanner.DistributeMarks(
            10m,
            new[]
            {
                AssessmentItemDifficulty.Easy,
                AssessmentItemDifficulty.Medium,
                AssessmentItemDifficulty.Challenging
            },
            0m);

        Assert.NotNull(marks);
        Assert.Equal(new decimal[] { 2m, 3m, 5m }, marks);
        Assert.Equal(10m, marks.Sum());
    }

    [Fact]
    public void AutoMarks_FailWhenThereIsLessThanOneMarkPerQuestion()
    {
        var marks = AssessmentBuilderGenerationPlanner.DistributeMarks(
            2m,
            new[]
            {
                AssessmentItemDifficulty.Easy,
                AssessmentItemDifficulty.Medium,
                AssessmentItemDifficulty.Challenging
            },
            0m);

        Assert.Null(marks);
    }

    [Fact]
    public void ExplicitMarks_UseUniformOverrideWithoutExceedingRemainingMarks()
    {
        var marks = AssessmentBuilderGenerationPlanner.DistributeMarks(
            10m,
            new[]
            {
                AssessmentItemDifficulty.Easy,
                AssessmentItemDifficulty.Challenging,
                AssessmentItemDifficulty.Medium
            },
            2m);

        Assert.NotNull(marks);
        Assert.Equal(new decimal[] { 2m, 2m, 2m }, marks);
        Assert.Equal(6m, marks.Sum());
    }

    [Fact]
    public void ExplicitMarks_FailClosedWhenRequestedTotalExceedsRemainingMarks()
    {
        var marks = AssessmentBuilderGenerationPlanner.DistributeMarks(
            5m,
            new[]
            {
                AssessmentItemDifficulty.Easy,
                AssessmentItemDifficulty.Medium,
                AssessmentItemDifficulty.Challenging
            },
            2m);

        Assert.Null(marks);
    }
}
