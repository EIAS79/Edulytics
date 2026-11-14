using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// UAE G5 Student local printed pp169–174. Written for Stage D, NOT RUN.
public sealed class UaeGrade5Unit3Lesson03TwoDigitDivisionTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:03:03";
    [Fact]
    public void ExactQuotientAndRemainderFamiliesAreRoutedWithoutAnswerLeak()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.Equal("number.base_ten_division_with_remainders",c!.SkillId);
        Assert.Equal(2,c.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in c.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,family,[family],
                     ExactSkillQuestionDifficulty.Standard,2,20261113,[]))
            {
                Assert.DoesNotContain("answer",q.Parameters.Keys);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-1"));
            }
        }
    }
    [Fact]
    public void TwoDigitDivisionHonoursRemainderAndRange()
    {
        var p=new Dictionary<string,int>{{"dividend",87},{"divisor",6}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l03.two_digit_quotient",p,"١٤"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l03.two_digit_remainder",p,"٣"));
        p["divisor"]=0;
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l03.two_digit_quotient",p,"14"));
        p["divisor"]=6;p["dividend"]=124;
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l03.two_digit_quotient",p,"20"));
    }
}
