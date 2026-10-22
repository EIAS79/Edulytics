using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit3Lesson7EstimateMeasureStage2QuestionsTests
{
    [Fact]
    public void FourSourceAlignedDifferenceFamiliesHaveIntegerPresenters()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U03:L07",out var contract));
        Assert.NotNull(contract);
        Assert.Equal(4,contract!.AllowedQuestionFamilies.Count);
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var presentation));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,presentation!.InteractionKind);
        }
    }
    [Fact]
    public void RandomVariantsKeepSignedOrientationAndNeverClaimRealObjectMeasurement()
    {
        var generator=new ExactSkillContractQuestionEngine();
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U03:L07",out var contract));
        foreach(var family in contract!.AllowedQuestionFamilies)
        foreach(var seed in new[]{20261022,20261023,20261024})
        {
            var qs=generator.Generate("im2021-g2-u03-l07",family,[family],
                ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,qs.Count);
            foreach(var q in qs)
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-1"));
                Assert.InRange(q.Parameters["estimate"],0,100);
                if(family.EndsWith("estimate_above_actual_gap",StringComparison.Ordinal))
                    Assert.True(q.Parameters["estimate"]>q.Parameters["actual"]);
                if(family.EndsWith("estimate_below_actual_gap",StringComparison.Ordinal))
                    Assert.True(q.Parameters["estimate"]<q.Parameters["actual"]);
                var wrong=new Dictionary<string,int>(q.Parameters,StringComparer.Ordinal){["actual"]=999};
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,wrong,q.CorrectAnswer));
            }
        }
    }
}
