using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit2Lesson7SubtractTwoDigitsQuestionsTests
{
    [Fact]
    public void FourReviewedNumericFamiliesHaveExactPresenters()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U02:L07",out var c));
        Assert.NotNull(c);
        Assert.Equal(4,c!.AllowedQuestionFamilies.Count);
        foreach(var family in c.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var choice));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,choice!.InteractionKind);
        }
    }
    [Fact]
    public void RandomizedBorrowedTensAreValidatedRecomputedAndTamperResistant()
    {
        var generator=new ExactSkillContractQuestionEngine();
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U02:L07",out var c));
        foreach(var family in c!.AllowedQuestionFamilies)
        foreach(var seed in new[]{20261022,20261023})
        {
            var qs=generator.Generate("im2021-g2-u02-l07",family,[family],
                ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,qs.Count);
            foreach(var q in qs)
            {
                Assert.True(q.Parameters["upperOnes"]<q.Parameters["lowerOnes"]);
                Assert.Equal(q.Parameters["upper"],10*q.Parameters["upperTens"]+q.Parameters["upperOnes"]);
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-1"));
                var tampered=new Dictionary<string,int>(q.Parameters,StringComparer.Ordinal)
                    {["upper"]=999};
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,tampered,q.CorrectAnswer));
            }
        }
    }
}
