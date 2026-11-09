using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// WRITTEN_NOT_RUN Stage A-B-C artifacts. Do not execute before Stage D.
public sealed class UaeGrade2Unit4WrittenNotRunTests
{
    [Theory]
    [InlineData("uae.g2.u04.l01.two_digit_fact_families")]
    [InlineData("uae.g2.u04.l02.subtract_decompose")]
    [InlineData("uae.g2.u04.l03.regroup_tens_for_subtraction")]
    [InlineData("uae.g2.u04.l04.subtract_one_digit_from_two")]
    [InlineData("uae.g2.u04.l05.subtract_two_digits")]
    [InlineData("uae.g2.u04.l06.vertical_subtract_two_digits")]
    [InlineData("uae.g2.u04.l07.verify_subtraction_add")]
    [InlineData("uae.g2.u04.l08.write_story_equation")]
    [InlineData("uae.g2.u04.l09.two_step_word_problems")]
    public void SourceExactQuestionsGenerateAndRejectWrongAnswers(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var engine=new ExactSkillContractQuestionEngine();
        for(var seed=1;seed<=20;seed++)
        {
            var qs=engine.Generate("uae-g2-u04",family,new[]{family},
                ExactSkillQuestionDifficulty.Standard,1,seed,Array.Empty<string>());
            Assert.NotEmpty(qs);
            foreach(var q in qs)
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"9999"));
                var ar=new string(q.CorrectAnswer.Select(c=>
                    c is >= '0' and <= '9'?(char)('٠'+c-'0'):c).ToArray());
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,ar));
            }
        }
    }
}
