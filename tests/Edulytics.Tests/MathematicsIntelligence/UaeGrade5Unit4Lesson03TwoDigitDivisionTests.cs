using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Arabic Grade5 local 01.pdf pp263–268; test written for Stage D, NOT_RUN.
public sealed class UaeGrade5Unit4Lesson03TwoDigitDivisionTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:04:03";
    [Fact]
    public void TwoDigitDivisionReusesExactEngine()
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
            for(int seed=1;seed<=6;seed++)
            foreach(var q in engine.Generate(Lesson,family,[family],ExactSkillQuestionDifficulty.Standard,1,seed,[]))
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
        }
    }
    [Fact]
    public void SourceMathAndArabicDigitsAreVerifiedWithRemainders()
    {
        var p=new Dictionary<string,int>{{"dividend",836},{"divisor",76}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u04.l03.two_digit_long_quotient",p,"١١"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u04.l03.two_digit_long_remainder",p,"0"));
        p["dividend"]=751;p["divisor"]=30;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u04.l03.two_digit_long_quotient",p,"25"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u04.l03.two_digit_long_remainder",p,"١"));
        p["divisor"]=0;
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u04.l03.two_digit_long_quotient",p,"0"));
    }
}