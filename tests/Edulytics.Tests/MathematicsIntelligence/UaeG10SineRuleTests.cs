using Xunit;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public class UaeG10SineRuleTests
{
    [Theory]
    [InlineData("uae.g10.general.u14.l04.sas_area_rounded")]
    [InlineData("uae.g10.general.u14.l04.aas_side_rounded")]
    [InlineData("uae.g10.general.u14.l04.third_angle")]
    [InlineData("uae.g10.general.u14.l04.ssa_triangle_count")]
    [InlineData("uae.g10.general.u14.l04.ssa_acute_solution_angle_rounded")]
    [InlineData("uae.g10.general.u14.l04.regular_polygon_area_rounded")]
    [InlineData("uae.g10.general.u14.l04.survey_distance_rounded")]
    public void SeededQuestionsRoundTrip(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var engine=new ExactSkillContractQuestionEngine();
        for(int seed=1;seed<=24;seed++)
        {
            var q=Assert.Single(engine.Generate("g10-source-test","u14-l04",new[]{family},ExactSkillQuestionDifficulty.Standard,1,seed,System.Array.Empty<string>()));
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,"-99999"));
        }
    }
}
