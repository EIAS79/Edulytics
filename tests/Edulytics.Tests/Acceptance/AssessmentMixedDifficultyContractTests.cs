using Edulytics.Core.Assessments;
using Edulytics.Core.Enums;
using Edulytics.Services.Assessments;

namespace Edulytics.Tests.Acceptance;

public sealed class AssessmentMixedDifficultyContractTests
{
    [Fact]
    public void AtClassLevelPlansBalancedMixedDifficulty()
    {
        var plan = AssessmentBuilderGenerationPlanner.PlanItemDifficulties(
            AssessmentBuilderDifficulty.AtClassLevel,
            10,
            42);

        Assert.NotNull(plan);
        Assert.Equal(10, plan.Count);
        Assert.Equal(3, plan.Count(x => x == AssessmentBuilderDifficulty.AtClassLevel));
        Assert.Equal(5, plan.Count(x => x == AssessmentBuilderDifficulty.Stretch));
        Assert.Equal(2, plan.Count(x => x == AssessmentBuilderDifficulty.Challenge));
    }

    [Fact]
    public void AtClassLevelDoesNotLabelEveryQuestionEasyWhenMixIsPossible()
    {
        var plan = AssessmentBuilderGenerationPlanner.PlanItemDifficulties(
            AssessmentBuilderDifficulty.AtClassLevel,
            5,
            99);

        Assert.NotNull(plan);
        var itemDifficulties = plan
            .Select(AssessmentBuilderGenerationPlanner.ToItemDifficulty)
            .ToArray();

        Assert.Contains(AssessmentItemDifficulty.Easy, itemDifficulties);
        Assert.Contains(AssessmentItemDifficulty.Medium, itemDifficulties);
        Assert.Contains(AssessmentItemDifficulty.Challenging, itemDifficulties);
    }
}
