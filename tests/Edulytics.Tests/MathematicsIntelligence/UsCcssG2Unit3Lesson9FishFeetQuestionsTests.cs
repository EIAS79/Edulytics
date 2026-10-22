using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit3Lesson9FishFeetQuestionsTests
{
    [Fact]
    public void FourRepeatedInchUnitFamiliesRouteToNumericAnswer()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U03:L09",out var c));
        Assert.NotNull(c);Assert.Equal(4,c!.AllowedQuestionFamilies.Count);
        foreach(var family in c.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var kind));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,kind!.InteractionKind);
        }
    }
    [Fact]
    public void ExactFirstPartyPhysicalIterationExamplesRecomputeWithoutConversionClaims()
    {
        var engine=new ExactSkillContractQuestionEngine();
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U03:L09",out var c));
        foreach(var family in c!.AllowedQuestionFamilies)
        foreach(var seed in new[]{20261022,20261023})
        {
            var qs=engine.Generate("im2021-g2-u03-l09",family,[family],
                ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,qs.Count);
            foreach(var q in qs)
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-1"));
                Assert.InRange(q.Parameters["spans"],1,4);
                if(!family.EndsWith("repeat_and_extra_inch_tiles",StringComparison.Ordinal))
                    Assert.Equal(0,q.Parameters["extraTiles"]);
                var wrong=new Dictionary<string,int>(q.Parameters,StringComparer.Ordinal)
                    { ["spans"]=99 };
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,wrong,q.CorrectAnswer));
            }
        }
    }
}
