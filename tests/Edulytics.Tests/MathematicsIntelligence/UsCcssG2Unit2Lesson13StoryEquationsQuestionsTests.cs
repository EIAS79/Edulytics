using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit2Lesson13StoryEquationsQuestionsTests
{
    [Fact]
    public void FourExactFamiliesPreserveActualUnknownStructure()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U02:L13",out var contract));
        Assert.NotNull(contract);Assert.Equal(4,contract!.AllowedQuestionFamilies.Count);
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var interaction));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,interaction!.InteractionKind);
        }
    }
    [Fact]
    public void ExactParameterRecomputeAndInvalidStateAreEnforced()
    {
        var e=new ExactSkillContractQuestionEngine();
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U02:L13",out var contract));
        foreach(var f in contract!.AllowedQuestionFamilies)
        foreach(var seed in new[]{20261022,20261023})
        {
            var qs=e.Generate("im2021-g2-u02-l13",f,[f],
                ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,qs.Count);
            foreach(var q in qs)
            {
                Assert.Equal(q.Parameters["start"]+q.Parameters["change"],q.Parameters["total"]);
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-1"));
                var bad=new Dictionary<string,int>(q.Parameters,StringComparer.Ordinal){["total"]=101};
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,bad,q.CorrectAnswer));
            }
        }
    }
}
