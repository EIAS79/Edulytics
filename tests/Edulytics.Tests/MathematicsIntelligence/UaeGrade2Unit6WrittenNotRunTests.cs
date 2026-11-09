using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;
// Stage A–B–C tests written but not run.
public sealed class UaeGrade2Unit6WrittenNotRunTests
{
    [Theory]
    [InlineData("uae.g2.u06.l01.make_hundred_add")]
    [InlineData("uae.g2.u06.l02.add_hundreds")]
    [InlineData("uae.g2.u06.l03.add_ten_hundred_mental")]
    [InlineData("uae.g2.u06.l04.regroup_ones_add")]
    [InlineData("uae.g2.u06.l05.regroup_tens_add")]
    [InlineData("uae.g2.u06.l06.add_three_digits")]
    [InlineData("uae.g2.u06.l07.vertical_three_digit_add")]
    [InlineData("uae.g2.u06.l08.guess_check_revision")]
    public void GenerateVerifyAndRejectIncorrectAnswer(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var engine=new ExactSkillContractQuestionEngine();
        for(var seed=1;seed<=24;seed++)
        {
            var qs=engine.Generate("uae-g2-u06",family,new[]{family},
                ExactSkillQuestionDifficulty.Standard,1,seed,Array.Empty<string>());
            Assert.NotEmpty(qs);
            foreach(var q in qs)
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"9999"));
            }
        }
    }
}
