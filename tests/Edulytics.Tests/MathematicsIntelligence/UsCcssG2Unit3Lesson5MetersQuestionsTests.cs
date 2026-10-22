using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit3Lesson5MetersQuestionsTests
{
    [Fact]
    public void FourOriginalMetricPracticeFamiliesResolveForLesson39()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U03:L05",out var c));
        Assert.NotNull(c);
        Assert.Equal(4,c!.AllowedQuestionFamilies.Count);
        foreach(var family in c.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,interaction!.InteractionKind);
        }
    }
    [Fact]
    public void AllVariantsUseOneHundredCmPerMeterAndRejectTamperedParameters()
    {
        var gen=new ExactSkillContractQuestionEngine();
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U03:L05",out var c));
        foreach(var f in c!.AllowedQuestionFamilies)
        foreach(var seed in new[]{20261022,20261023})
        {
            var qs=gen.Generate("im2021-g2-u03-l05",f,[f],
                ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,qs.Count);
            foreach(var q in qs)
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-1"));
                var bad=new Dictionary<string,int>(q.Parameters,StringComparer.Ordinal);
                if(f.EndsWith("compare_reptile_tape_lengths",StringComparison.Ordinal))
                {
                    Assert.True(q.Parameters["longCm"]>q.Parameters["shortCm"]);
                    bad["longCm"]=0;
                }
                else
                {
                    Assert.Equal(q.Parameters["meters"]*100+q.Parameters["remainingCm"],q.Parameters["totalCm"]);
                    bad["totalCm"]=1;
                }
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,bad,q.CorrectAnswer));
            }
        }
    }
}
