using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit3Lesson11SareeRibbonQuestionsTests
{
    [Fact]
    public void FourDifferentRibbonStoryTypeFamiliesResolveExactly()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U03:L11",out var contract));
        Assert.NotNull(contract);Assert.Equal(4,contract!.AllowedQuestionFamilies.Count);
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var x));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,x!.InteractionKind);
        }
    }
    [Fact]
    public void AllRandomNumbersAndTheStartUnknownAnswerRecomputeCorrectly()
    {
        var gen=new ExactSkillContractQuestionEngine();
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U03:L11",out var c));
        foreach(var f in c!.AllowedQuestionFamilies)
        foreach(var seed in new[]{20261022,20261023})
        {
            var questions=gen.Generate("im2021-g2-u03-l11",f,[f],
                ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,questions.Count);
            foreach(var q in questions)
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-1"));
                if(f.EndsWith("ribbon_start_unknown",StringComparison.Ordinal))
                    Assert.Equal(q.Parameters["whole"].ToString(),q.CorrectAnswer);
                else
                    Assert.Equal((q.Parameters["whole"]-q.Parameters["known"]).ToString(),q.CorrectAnswer);
                var altered=new Dictionary<string,int>(q.Parameters,StringComparer.Ordinal) { ["known"]=200 };
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,altered,q.CorrectAnswer));
            }
        }
    }
}
