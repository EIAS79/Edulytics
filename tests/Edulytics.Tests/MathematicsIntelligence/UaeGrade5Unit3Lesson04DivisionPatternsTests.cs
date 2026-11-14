using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Local Grade5 Arabic source 01.pdf printed pp175–180. NOT_RUN in A-B-C.
public sealed class UaeGrade5Unit3Lesson04DivisionPatternsTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:03:04";
    [Fact]
    public void DivisionPatternsReuseExactG4EngineAndValidateAcrossSeeds()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.Equal("number.division.patterns_powers_of_ten",c!.SkillId);
        Assert.Equal(3,c.AllowedQuestionFamilies.Count);
        var e=new ExactSkillContractQuestionEngine();
        foreach(var family in c.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
            foreach(var q in e.Generate(Lesson,family,[family],
                ExactSkillQuestionDifficulty.Standard,2,20261113,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-1"));
                Assert.DoesNotContain("answer",q.Parameters.Keys);
            }
        }
    }
    [Fact]
    public void ScaledDivisionCannotLosePlaceValueFactor()
    {
        var p=new Dictionary<string,int>{{"factor",9},{"quotient",60},{"zeros",1}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l04.cancel_common_zeros",p,"٦٠"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l04.quotient_with_tens_divisor",p,"60"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l04.scale_dividend",p,"600"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l04.cancel_common_zeros",p,"6"));
        p["zeros"]=0;
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l04.scale_dividend",p,"60"));
    }
}
