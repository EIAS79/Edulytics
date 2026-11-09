using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;
// Prepared during A-B-C. Runtime tests MUST be run at Stage D, not here.
public sealed class UaeGrade2Unit8WrittenNotRunTests
{
    [Theory]
    [InlineData("uae.g2.u08.l01.survey_collect_count")]
    [InlineData("uae.g2.u08.l02.make_picture_graph")]
    [InlineData("uae.g2.u08.l03.analyze_picture_graph")]
    [InlineData("uae.g2.u08.l04.draw_bar_graph")]
    [InlineData("uae.g2.u08.l05.analyze_bar_graph")]
    [InlineData("uae.g2.u08.l06.make_data_table")]
    [InlineData("uae.g2.u08.l07.make_dot_plot")]
    [InlineData("uae.g2.u08.l08.analyze_dot_plot")]
    public void DataQuestionsGeneratedAndCheckedIndependently(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var engine=new ExactSkillContractQuestionEngine();
        for(var seed=1;seed<=32;seed++)
        {
            var items=engine.Generate("uae-g2-unit8",family,new[]{family},
                ExactSkillQuestionDifficulty.Standard,1,seed,Array.Empty<string>());
            Assert.NotEmpty(items);
            foreach(var q in items)
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
