using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit3Lesson17CenterPlotQuestionsTests
{
    [Fact]
    public void FourOptionalCenterFamiliesUseExactNumericEntry()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U03:L17",out var c));
        Assert.NotNull(c);Assert.Equal(4,c!.AllowedQuestionFamilies.Count);
        foreach(var family in c.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var kind));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,kind!.InteractionKind);
        }
    }
    [Fact]
    public void NewVariantsPreserveAtMostEightItemsAndExactlyOneUnit()
    {
        var generator=new ExactSkillContractQuestionEngine();
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U03:L17",out var c));
        foreach(var family in c!.AllowedQuestionFamilies)
        foreach(var seed in new[]{20261022,20261023})
        {
            var qs=generator.Generate("im2021-g2-u03-l17",family,[family],
                ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,qs.Count);
            foreach(var q in qs)
            {
                var p=q.Parameters;
                Assert.InRange(p["a"]+p["b"]+p["c"]+p["d"],4,8);
                Assert.InRange(p["unit"],0,1);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"-1"));
                var altered=new Dictionary<string,int>(p,StringComparer.Ordinal); altered["unit"]=2;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,altered,q.CorrectAnswer));
            }
        }
    }
}
