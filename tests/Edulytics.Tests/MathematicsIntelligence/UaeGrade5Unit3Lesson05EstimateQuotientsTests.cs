using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Grade5 student printed 183–188, tests written for later Stage D.
public sealed class UaeGrade5Unit3Lesson05EstimateQuotientsTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:03:05";
    [Fact]
    public void G5EstimatedQuotientsRouteToExistingG4ExactPractice()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.Equal("number.estimate_quotient_compatible_numbers",c!.SkillId);
        Assert.Equal(3,c.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in c.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,family,[family],
                ExactSkillQuestionDifficulty.Standard,2,20261113,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.DoesNotContain("answer",q.Parameters.Keys);
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-99"));
            }
        }
    }
    [Fact]
    public void CompatibleExampleIsAppropriatelyClose()
    {
        var p=new Dictionary<string,int>{{"dividend",208},{"divisor",8}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l05.estimate_compatible_quotient",p,"٢٥"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l05.select_compatible_dividend",p,"200"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l05.is_estimate_above_exact",p,"0"));
        p["divisor"]=0;
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l05.estimate_compatible_quotient",p,"25"));
    }
}
