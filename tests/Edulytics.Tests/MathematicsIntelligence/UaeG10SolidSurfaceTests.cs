using Xunit;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public class UaeG10SolidSurfaceTests {
    [Theory]
    [InlineData("uae.g10.general.u15.l02.rectangular_prism_lateral")]
    [InlineData("uae.g10.general.u15.l02.rectangular_prism_total")]
    [InlineData("uae.g10.general.u15.l02.cylinder_lateral_pi_coefficient")]
    [InlineData("uae.g10.general.u15.l02.cylinder_total_pi_coefficient")]
    [InlineData("uae.g10.general.u15.l02.cylinder_height_from_lateral")]
    [InlineData("uae.g10.general.u15.l02.open_cylinder_surface_pi_coefficient")]
    public void SeededQuestionsRoundTrip(string family){
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var engine=new ExactSkillContractQuestionEngine();
        for(int seed=1;seed<=36;seed++){
            var q=Assert.Single(engine.Generate("g10-source-test","u15-l02",new[]{family},ExactSkillQuestionDifficulty.Standard,1,seed,System.Array.Empty<string>()));
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,"-99999"));
        }
    }
}
