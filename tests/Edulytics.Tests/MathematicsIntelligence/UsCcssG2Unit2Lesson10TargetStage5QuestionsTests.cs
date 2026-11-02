using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit2Lesson10TargetStage5QuestionsTests
{
    [Fact]
    public void FourDiceFamiliesHaveExactNumericInteractions()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U02:L10",out var c));
        Assert.NotNull(c);Assert.Equal(4,c!.AllowedQuestionFamilies.Count);
        foreach(var family in c.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,interaction!.InteractionKind);
        }
    }
    [Fact]
    public void DistinctDieIndicesAndNoNegativeRemaindersAndTradeRulesHold()
    {
        var engine=new ExactSkillContractQuestionEngine();
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U02:L10",out var c));
        foreach(var family in c!.AllowedQuestionFamilies)
        foreach(var seed in new[]{20261022,20261023})
        {
            var qs=engine.Generate("im2021-g2-u02-l10",family,[family],
                ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,qs.Count);
            foreach(var q in qs)
            {
                Assert.NotEqual(q.Parameters["tensIndex"],q.Parameters["onesIndex"]);
                Assert.InRange(q.Parameters["die0"],1,6);
                Assert.InRange(q.Parameters["die1"],1,6);
                Assert.InRange(q.Parameters["die2"],1,6);
                Assert.True(q.Parameters["current"]>=q.Parameters["amount"]);
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-1"));
                if(family.EndsWith("dice_subtract_decompose",StringComparison.Ordinal))
                    Assert.True(q.Parameters["current"]%10<q.Parameters["die1"]);
                if(family.EndsWith("dice_subtract_no_decompose",StringComparison.Ordinal))
                    Assert.True(q.Parameters["current"]%10>=q.Parameters["die1"]);
                var changed=new Dictionary<string,int>(q.Parameters,StringComparer.Ordinal)
                    { ["onesIndex"]=q.Parameters["tensIndex"] };
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,changed,q.CorrectAnswer));
            }
        }
    }
}
