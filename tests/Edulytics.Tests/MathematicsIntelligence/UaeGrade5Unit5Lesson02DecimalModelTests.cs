using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Source: UAE G5 local Arabic print309–314; tests NOT_RUN.
public sealed class UaeGrade5Unit5Lesson02DecimalModelTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:05:02";
    [Fact]
    public void ReusedDecimalModelEngineSupportsSourceSkill()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal("supporting.decimals.core",contract!.SkillId);
        Assert.Equal(3,contract.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,2,20261113,[]))
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
        }
    }
    [Fact]
    public void DecimalShadedModelsRegroupExactTenths()
    {
        var p=new Dictionary<string,int>{{"tenths",4},{"groups",2}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l02.model_decimal_product",p,"٠٫٨"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l02.model_shaded_hundredths",p,"80"));
        p["tenths"]=7;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l02.model_decimal_product",p,"1.4"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l02.model_complete_wholes",p,"1"));
        p["groups"]=0;
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l02.model_decimal_product",p,"0"));
    }
}