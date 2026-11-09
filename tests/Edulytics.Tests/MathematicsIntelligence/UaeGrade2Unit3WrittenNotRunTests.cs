using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// Tests prepared and not executed before Stage D.
public sealed class UaeGrade2Unit3WrittenNotRunTests
{
    [Theory]
    [InlineData("uae.g2.u03.l01.two_digit_decomposition")]
    [InlineData("uae.g2.u03.l02.regroup_ones_ten")]
    [InlineData("uae.g2.u03.l03.one_digit_two_digit_regroup")]
    [InlineData("uae.g2.u03.l04.add_two_digit_numbers")]
    [InlineData("uae.g2.u03.l05.vertical_add_two_digits")]
    [InlineData("uae.g2.u03.l06.add_three_four_two_digits")]
    [InlineData("uae.g2.u03.l07.model_two_digit_story")]
    public void ArithmeticSourceFamiliesRecomputeWithoutForgedAnswers(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var seed in Enumerable.Range(1,32))
        {
            var q=engine.Generate("uae-g2-unit3",family,new[]{family},
                ExactSkillQuestionDifficulty.Standard,1,seed,Array.Empty<string>());
            Assert.NotEmpty(q);
            foreach(var item in q)
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,item.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,"9999"));
                var ar=new string(item.CorrectAnswer.Select(c=>
                    c is >= '0' and <= '9'?(char)('٠'+c-'0'):c).ToArray());
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,ar));
            }
        }
    }
}
