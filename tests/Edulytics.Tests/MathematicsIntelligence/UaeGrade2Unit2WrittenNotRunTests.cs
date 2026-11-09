using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// Tests prepared during A-B-C. They are NOT executed before Stage D.
public sealed class UaeGrade2Unit2WrittenNotRunTests
{
    [Theory]
    [InlineData("uae.g2.u02.l01.skip_count_hundred_chart")]
    [InlineData("uae.g2.u02.l02.skip_count_equal_groups")]
    [InlineData("uae.g2.u02.l03.find_number_pattern")]
    [InlineData("uae.g2.u02.l04.repeated_add_equal_groups")]
    [InlineData("uae.g2.u02.l05.repeated_add_arrays")]
    [InlineData("uae.g2.u02.l06.even_odd_classify")]
    [InlineData("uae.g2.u02.l07.equal_addends")]
    public void QuestionsGenerateAndVerifyWithArabicDigits(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var engine=new ExactSkillContractQuestionEngine();
        for(var seed=1;seed<=32;seed++)
        {
            var qs=engine.Generate("uae-grade2-unit2",family,new[]{family},
                ExactSkillQuestionDifficulty.Standard,1,seed,Array.Empty<string>());
            Assert.NotEmpty(qs);
            foreach(var q in qs)
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,"9999"));
                var arabic=new string(q.CorrectAnswer.Select(c=>
                    c is >= '0' and <= '9'?(char)('٠'+c-'0'):c).ToArray());
                Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,arabic));
            }
        }
    }
    [Fact]
    public void TamperedEvenOddParityIsRejected()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            "uae.g2.u02.l06.even_odd_classify",
            new Dictionary<string,int>{{"number",9},{"result",2}},"2"));
    }
}
