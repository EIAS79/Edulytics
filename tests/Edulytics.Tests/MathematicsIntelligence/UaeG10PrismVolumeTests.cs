using Xunit;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public class UaeG10PrismVolumeTests {
    [Theory]
    [InlineData("uae.g10.general.u15.l05.triangular_prism_volume")]
    [InlineData("uae.g10.general.u15.l05.oblique_prism_volume")]
    [InlineData("uae.g10.general.u15.l05.cylinder_volume_pi_coefficient")]
    [InlineData("uae.g10.general.u15.l05.cylinder_height")]
    [InlineData("uae.g10.general.u15.l05.cylinder_radius")]
    [InlineData("uae.g10.general.u15.l05.equal_volume_prism_height")]
    [InlineData("uae.g10.general.u15.l05.removed_rectangular_volume")]
    public void SeededQuestionsRoundTrip(string family){
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var engine=new ExactSkillContractQuestionEngine();
        for(int seed=1;seed<=36;seed++){
            var q=Assert.Single(engine.Generate("g10-source-test","u15-l05",new[]{family},ExactSkillQuestionDifficulty.Standard,1,seed,System.Array.Empty<string>()));
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,"-99999"));
        }
    }
}
