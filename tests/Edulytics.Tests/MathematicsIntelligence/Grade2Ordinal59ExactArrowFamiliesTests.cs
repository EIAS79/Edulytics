using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class Grade2Ordinal59ExactArrowFamiliesTests
{
    [Fact]
    public void FourDistinctFamiliesUseExactKeypadNotFictionalOriginalImages()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U04:L07",out var contract));
        Assert.NotNull(contract);
        Assert.Equal(4,contract!.AllowedQuestionFamilies.Count);
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var route));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,route!.InteractionKind);
        }
    }
    [Fact]
    public void SecureGeneratorVerifiesItsOwnAnswerAndRejectsCorruptCoordinates()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U04:L07",out var contract));
        var generator=new ExactSkillContractQuestionEngine();
        foreach(var family in contract!.AllowedQuestionFamilies)
        foreach(var seed in new[]{20261022,20261023})
        {
            var questions=generator.Generate("im2021-g2-u04-l07",family,[family],
                ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,questions.Count);
            foreach(var q in questions)
            {
                Assert.Equal(7,q.Parameters["lesson"]);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-1"));
                var bad=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                bad["end"]=101;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,bad,q.CorrectAnswer));
                bad["end"]=q.Parameters["end"];
                bad["countStep"]=9;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,bad,q.CorrectAnswer));
            }
        }
    }
}
