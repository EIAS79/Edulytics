using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit3Lesson1StandardUnitsQuestionsTests
{
    [Fact]
    public void FourSourceUnitLengthFamiliesRouteToIntegerQuestions()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U03:L01",out var c));
        Assert.NotNull(c);
        Assert.Equal(4,c!.AllowedQuestionFamilies.Count);
        foreach(var f in c.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var presentation));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,presentation!.InteractionKind);
        }
    }
    [Fact]
    public void RandomUnitCountsAreExactAndBadLengthsRejected()
    {
        var engine=new ExactSkillContractQuestionEngine();
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U03:L01",out var c));
        foreach(var f in c!.AllowedQuestionFamilies)
        foreach(var seed in new[]{20261022,20261023})
        {
            var qs=engine.Generate("im2021-g2-u03-l01",f,[f],
                ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,qs.Count);
            foreach(var q in qs)
            {
                Assert.Equal(0,q.Parameters["lengthCm"]%10);
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-1"));
                var changed=new Dictionary<string,int>(q.Parameters,StringComparer.Ordinal)
                    { ["lengthCm"]=41 };
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,changed,q.CorrectAnswer));
            }
        }
    }
}
