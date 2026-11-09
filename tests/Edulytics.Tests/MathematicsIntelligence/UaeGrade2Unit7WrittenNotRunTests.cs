using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;
// Written during A-B-C, NOT RUN before Stage D.
public sealed class UaeGrade2Unit7WrittenNotRunTests
{
    [Theory]
    [InlineData("uae.g2.u07.l01.decompose_hundred_subtract")]
    [InlineData("uae.g2.u07.l02.subtract_hundreds")]
    [InlineData("uae.g2.u07.l03.subtract_ten_hundred_mental")]
    [InlineData("uae.g2.u07.l04.regroup_tens_for_ones")]
    [InlineData("uae.g2.u07.l05.regroup_hundreds_for_tens")]
    [InlineData("uae.g2.u07.l06.subtract_three_digit")]
    [InlineData("uae.g2.u07.l07.vertical_three_digit_sub")]
    [InlineData("uae.g2.u07.l08.story_equation_sub")]
    [InlineData("uae.g2.u07.l09.subtract_with_zeros")]
    public void VerifyValidSourceAndRejectIncorrectAnswers(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var engine=new ExactSkillContractQuestionEngine();
        for(var seed=1;seed<=24;seed++)
        {
            var items=engine.Generate("uae-grade2-u07",family,new[]{family},
                ExactSkillQuestionDifficulty.Standard,1,seed,Array.Empty<string>());
            Assert.NotEmpty(items);
            foreach(var q in items)
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"9999"));
            }
        }
    }
}
