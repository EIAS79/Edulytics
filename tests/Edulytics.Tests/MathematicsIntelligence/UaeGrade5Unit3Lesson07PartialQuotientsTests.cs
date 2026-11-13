using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Stage A–B–C authored for UAE Grade 5, lesson 26. Stage D tests not run.
public sealed class UaeGrade5Unit3Lesson07PartialQuotientsTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:03:07";
    [Fact]
    public void PartialQuotientFamiliesUseExistingExactDivisionEngine()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal("number.division.distributive_partial_quotients",contract!.SkillId);
        Assert.Equal(3,contract.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
            foreach(var item in engine.Generate(Lesson,family,[family],
              ExactSkillQuestionDifficulty.Standard,2,20261113,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,item.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,"-1"));
                Assert.DoesNotContain("result",item.Parameters.Keys);
            }
        }
    }
    [Fact]
    public void SourceCasesCheckHundredsTensAndOnesWithArithmeticProof()
    {
        var p=new Dictionary<string,int>{{"dividend",738},{"divisor",6}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l07.distributed_quotient",p,"123"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l07.sum_partial_quotients",p,"١٢٣"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l07.tens_partial_quotient",p,"20"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l07.tens_partial_quotient",p,"120"));
        p["dividend"]=739;
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l07.distributed_quotient",p,"123"));
    }
}
