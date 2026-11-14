using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade5Unit2Lesson08EstimateProductsTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:02:08";
    [Fact]
    public void G5EstimateFamiliesReuseExistingEngineWithoutLeakingAnswers()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.Equal("number.rounding_to_estimate_products",c!.SkillId);
        Assert.Equal(4,c.AllowedQuestionFamilies.Count);
        var e=new ExactSkillContractQuestionEngine();
        foreach(var family in c.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
            foreach(var q in e.Generate(Lesson,family,[family],
                ExactSkillQuestionDifficulty.Standard,2,20261113,[]))
            {
                Assert.DoesNotContain("answer",q.Parameters.Keys);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-1"));
            }
        }
    }
    [Fact]
    public void DifferentEstimationMethodsPreserveTheirDifferentNumericalOutputs()
    {
        var p=new Dictionary<string,int>{{"first",154},{"second",21}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u02.l08.round_one_factor",p,"3150"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u02.l08.round_both_factors",p,"3000"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u02.l08.compatible_factors",p,"٤٬٠٠٠"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u02.l08.estimate_high_or_low",p,"0"));
        p["second"]=0;
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u02.l08.round_both_factors",p,"0"));
    }
}
