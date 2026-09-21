using Edulytics.Core.Enums;
using Edulytics.Services.Mathematics;
using Edulytics.Services.MathematicsGeneration;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class GradeAwareExactDifficultyPolicyTests
{
    [Theory]
    [InlineData("G1", AssessmentItemDifficulty.Challenging, MathematicsCurriculumDifficultyStage.EarlyPrimary, ExactSkillQuestionDifficulty.Stretch)]
    [InlineData("GRADE 2", AssessmentItemDifficulty.Medium, MathematicsCurriculumDifficultyStage.EarlyPrimary, ExactSkillQuestionDifficulty.Standard)]
    [InlineData("S6", AssessmentItemDifficulty.Challenging, MathematicsCurriculumDifficultyStage.Primary, ExactSkillQuestionDifficulty.Challenge)]
    [InlineData("L7", AssessmentItemDifficulty.Challenging, MathematicsCurriculumDifficultyStage.LowerSecondary, ExactSkillQuestionDifficulty.Challenge)]
    [InlineData("G8", AssessmentItemDifficulty.Medium, MathematicsCurriculumDifficultyStage.LowerSecondary, ExactSkillQuestionDifficulty.Stretch)]
    [InlineData("HS", AssessmentItemDifficulty.Challenging, MathematicsCurriculumDifficultyStage.UpperSecondary, ExactSkillQuestionDifficulty.Challenge)]
    [InlineData("L10", AssessmentItemDifficulty.Challenging, MathematicsCurriculumDifficultyStage.UpperSecondary, ExactSkillQuestionDifficulty.Challenge)]
    public void Resolve_CalibratesRequestedDifficultyToCurriculumLevel(
        string levelKey,
        AssessmentItemDifficulty requested,
        MathematicsCurriculumDifficultyStage expectedStage,
        ExactSkillQuestionDifficulty expectedEffective)
    {
        var decision = GradeAwareExactDifficultyPolicy.Resolve(levelKey, requested);

        Assert.Equal(expectedStage, decision.Stage);
        Assert.Equal(expectedEffective, decision.EffectiveDifficulty);
    }

    [Fact]
    public void Resolve_PreservesEasyAsStandardAtEverySupportedStage()
    {
        foreach (var key in new[] { "G1", "S4", "L8", "HS", "L12" })
        {
            Assert.Equal(
                ExactSkillQuestionDifficulty.Standard,
                GradeAwareExactDifficultyPolicy.Resolve(
                    key,
                    AssessmentItemDifficulty.Easy).EffectiveDifficulty);
        }
    }

    [Fact]
    public void Resolve_FailsClosedForUnknownCurriculumLevel()
    {
        Assert.Throws<InvalidOperationException>(() =>
            GradeAwareExactDifficultyPolicy.Resolve(
                "UNKNOWN",
                AssessmentItemDifficulty.Medium));
    }
}
