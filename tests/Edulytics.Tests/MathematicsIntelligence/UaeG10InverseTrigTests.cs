using Xunit;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public class UaeG10InverseTrigTests
{
    [Theory]
    [InlineData("uae.g10.general.u14.l09.principal_angle_degrees")]
    [InlineData("uae.g10.general.u14.l09.principal_pi_coefficient")]
    [InlineData("uae.g10.general.u14.l09.inverse_defined")]
    [InlineData("uae.g10.general.u14.l09.inverse_of_trig_principal_degrees")]
    [InlineData("uae.g10.general.u14.l09.composition_rational")]
    [InlineData("uae.g10.general.u14.l09.rounded_angle_degrees")]
    [InlineData("uae.g10.general.u14.l09.right_triangle_angle_degrees")]
    [InlineData("uae.g10.general.u14.l09.reciprocal_equation_solution_count")]
    public void SeededQuestionsRoundTrip(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var engine=new ExactSkillContractQuestionEngine();
        for(int seed=1;seed<=24;seed++){
            var q=Assert.Single(engine.Generate("g10-source-test","u14-l09",new[]{family},ExactSkillQuestionDifficulty.Standard,1,seed,System.Array.Empty<string>()));
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,"-99999"));
        }
    }
}
