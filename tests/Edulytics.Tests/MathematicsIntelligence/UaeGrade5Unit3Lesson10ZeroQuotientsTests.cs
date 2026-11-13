using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// UAE Grade5 local student pp215-220: test authored Stage A-B-C, NOT RUN.
public sealed class UaeGrade5Unit3Lesson10ZeroQuotientsTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:03:10";
    [Fact]
    public void StageCContractsRouteToExistingDivisionEngineWithInternalZeros()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal("number.division.zero_in_quotient",contract!.SkillId);
        Assert.Equal(2,contract.AllowedQuestionFamilies.Count);
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
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,"-999"));
            }
        }
    }
    [Fact]
    public void SourceZeroQuotientsAndRemainderCannotLoseMiddlePlace()
    {
        var p=new Dictionary<string,int>{{"dividend",327},{"divisor",3}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l10.middle_zero_quotient",p,"١٠٩"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l10.middle_zero_quotient",p,"19"));
        p["dividend"]=815;p["divisor"]=2;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l10.middle_zero_remainder",p,"1"));
        p["dividend"]=4324;p["divisor"]=4;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l10.middle_zero_quotient",p,"1081"));
        p["divisor"]=0;
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l10.middle_zero_quotient",p,"1081"));
    }
}
