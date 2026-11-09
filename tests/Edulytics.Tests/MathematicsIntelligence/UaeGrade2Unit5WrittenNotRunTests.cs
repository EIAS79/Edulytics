using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// Written but NOT executed until Stage D.
public sealed class UaeGrade2Unit5WrittenNotRunTests
{
    [Theory]
    [InlineData("uae.g2.u05.l01.count_hundreds")]
    [InlineData("uae.g2.u05.l02.compose_hto")]
    [InlineData("uae.g2.u05.l03.place_value")]
    [InlineData("uae.g2.u05.l04.logical_hto")]
    [InlineData("uae.g2.u05.l05.read_number_words")]
    [InlineData("uae.g2.u05.l06.skip_5_10_100")]
    [InlineData("uae.g2.u05.l07.compare_3digit")]
    [InlineData("uae.g2.u05.l08.ordinal")]
    public void ContractReturnsCorrectAnswerAndRejectsBadAnswer(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var engine=new ExactSkillContractQuestionEngine();
        for(var seed=1;seed<=20;seed++)
        {
            var generated=engine.Generate("uae-grade2-u05",family,new[]{family},
                ExactSkillQuestionDifficulty.Standard,1,seed,Array.Empty<string>());
            Assert.NotEmpty(generated);
            foreach(var item in generated)
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,item.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,"9999"));
            }
        }
    }
}
