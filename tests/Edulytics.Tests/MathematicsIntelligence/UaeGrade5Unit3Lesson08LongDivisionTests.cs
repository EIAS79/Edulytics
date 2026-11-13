using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// UAE Grade5 01.pdf printed pp201-206, authored A-B-C tests (NOT RUN).
public sealed class UaeGrade5Unit3Lesson08LongDivisionTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:03:08";
    [Fact]
    public void ThreeAndFourDigitExactFamiliesRouteThroughExistingDivisionEngine()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal("number.long_division.three_four_digit_one_digit",contract!.SkillId);
        Assert.Equal(2,contract.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
            for(int seed=1;seed<=8;seed++)
            foreach(var item in engine.Generate(Lesson,family,[family],
                ExactSkillQuestionDifficulty.Standard,2,seed,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,item.Parameters,item.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,"-11"));
                Assert.DoesNotContain("result",item.Parameters.Keys);
            }
        }
    }
    [Fact]
    public void LongDivisionExamplesVerifyByReverseMultiplicationAndRemainder()
    {
        var p=new Dictionary<string,int>{{"dividend",7946},{"divisor",6}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            "uae.g5.u03.l08.long_division_quotient",p,"١٣٢٤"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            "uae.g5.u03.l08.long_division_remainder",p,"٢"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            "uae.g5.u03.l08.long_division_remainder",p,"6"));
        p["dividend"]=852;p["divisor"]=4;
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            "uae.g5.u03.l08.long_division_quotient",p,"213"));
        p["dividend"]=10000;
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            "uae.g5.u03.l08.long_division_quotient",p,"2500"));
    }
}
