using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Three source-aligned G2 U02 L02 exact families, CI intentionally unrun.</summary>
public sealed class UsCcssG2Unit2Lesson2UnknownAddendQuestionsTests
{
    [Fact]
    public void ExactFamiliesResolveOnlyForG2U02L02AsIntegerAnswers()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U02:L02",out var contract));
        Assert.NotNull(contract);
        Assert.Equal(3,contract!.AllowedQuestionFamilies.Count);
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,interaction!.InteractionKind);
        }
    }
    [Fact]
    public void RandomizedNoRegroupAndCountdownAreRecomputedIndependentOfAnswer()
    {
        var engine=new ExactSkillContractQuestionEngine();
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U02:L02",out var contract));
        foreach(var family in contract!.AllowedQuestionFamilies)
        foreach(var seed in new[]{20261022,20261023})
        {
            var batch=engine.Generate("im2021-g2-u02-l02",family,[family],
                ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,batch.Count);
            foreach(var q in batch)
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,"-1"));
                var changed=new Dictionary<string,int>(q.Parameters,StringComparer.Ordinal);
                if(family.EndsWith("count_back_tens",StringComparison.Ordinal))
                {
                    Assert.Equal(q.Parameters["start"]-10*q.Parameters["steps"],q.Parameters["end"]);
                    changed["steps"]=99;
                }
                else
                {
                    Assert.Equal(q.Parameters["known"]+q.Parameters["missing"],q.Parameters["total"]);
                    Assert.True(q.Parameters["known"]%10+q.Parameters["missing"]%10<10);
                    changed["total"]=101;
                }
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,changed,q.CorrectAnswer));
            }
        }
    }
}
