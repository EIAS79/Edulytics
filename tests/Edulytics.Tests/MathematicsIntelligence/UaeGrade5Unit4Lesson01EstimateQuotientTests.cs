using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Grade 5 Arabic book print251-256; tests authored in ABC, NOT RUN.
public sealed class UaeGrade5Unit4Lesson01EstimateQuotientTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:04:01";
    [Fact]
    public void EstimatesRouteToExistingExtendedEngine()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal("number.estimate_quotient_compatible_numbers",contract!.SkillId);
        Assert.Equal(3,contract.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
            foreach(var item in engine.Generate(Lesson,family,[family],
                ExactSkillQuestionDifficulty.Standard,2,20261113,[]))
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,item.CorrectAnswer));
        }
    }
    [Fact]
    public void TwoDigitSourceEstimatesUseDocumentedDeterministicCompatibility()
    {
        var p=new Dictionary<string,int>{{"dividend",812},{"divisor",19}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u04.l01.estimate_two_digit_quotient",p,"40"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u04.l01.choose_compatible_dividend",p,"800"));
        p["dividend"]=533;p["divisor"]=57;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u04.l01.estimate_two_digit_quotient",p,"9"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u04.l01.choose_compatible_dividend",p,"540"));
        p["divisor"]=0;
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u04.l01.estimate_two_digit_quotient",p,"9"));
    }
}
