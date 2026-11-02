using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit3Lesson4EstimateMeasureCmQuestionsTests
{
    [Fact]
    public void FourSourceAlignedRulerAndEstimateFamiliesUseNumericPresenter()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U03:L04",out var c));
        Assert.NotNull(c);
        Assert.Equal(4,c!.AllowedQuestionFamilies.Count);
        foreach(var f in c.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var p));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,p!.InteractionKind);
        }
    }
    [Fact]
    public void GeneratedCmMathRespectsOffsetAndRealisticLengthsAndRecomputes()
    {
        var e=new ExactSkillContractQuestionEngine();
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U03:L04",out var c));
        foreach(var family in c!.AllowedQuestionFamilies)
        foreach(var seed in new[]{20261022,20261023})
        {
            var items=e.Generate("im2021-g2-u03-l04",family,[family],
                ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,items.Count);
            foreach(var q in items)
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,"-1"));
                var bad=new Dictionary<string,int>(q.Parameters,StringComparer.Ordinal);
                if(family.EndsWith("cm_ruler_iteration",StringComparison.Ordinal))
                {
                    Assert.InRange(q.Parameters["count"]*30+q.Parameters["remainder"],50,90);
                    bad["count"]=99;
                }
                else if(family.EndsWith("cm_estimate_about_right",StringComparison.Ordinal))
                {
                    Assert.True(q.Parameters["below"]<q.Parameters["benchmark"]);
                    bad["above"]=0;
                }
                else
                {
                    Assert.True(q.Parameters["finish"]>q.Parameters["start"]);
                    bad["finish"]=999;
                }
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,bad,q.CorrectAnswer));
            }
        }
    }
}
