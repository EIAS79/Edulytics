using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// UAE Grade5 student Arabic local pp303–308; Stage D tests written NOT_RUN.
public sealed class UaeGrade5Unit5Lesson01DecimalProductEstimationTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:05:01";
    [Fact]
    public void DecimalEstimationIsRegisteredThroughExistingG4Engine()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.Equal("number.rounding_to_estimate_products",c!.SkillId);
        Assert.Equal(3,c.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in c.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.NotNull(ui);
            for(int seed=1;seed<=8;seed++)
            foreach(var q in engine.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,1,seed,[]))
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
        }
    }
    [Fact]
    public void HundredthParametersRoundAndVerifyOriginalArabicDigits()
    {
        var p=new Dictionary<string,int>{{"hundredths",5980},{"factor",5}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l01.round_decimal_to_whole",p,"٣٠٠"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l01.estimate_above_exact",p,"1"));
        p["hundredths"]=4450;p["factor"]=11;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l01.round_both_factors",p,"٤٥٠"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l01.estimate_above_exact",p,"0"));
        p["hundredths"]=462;p["factor"]=4;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l01.round_decimal_to_whole",p,"20"));
        p["hundredths"]=0;
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l01.round_decimal_to_whole",p,"0"));
    }
}