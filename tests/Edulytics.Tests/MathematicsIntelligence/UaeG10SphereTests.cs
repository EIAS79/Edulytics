using Xunit;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public class UaeG10SphereTests {
    [Theory]
    [InlineData("uae.g10.general.u15.l04.surface_pi_coefficient")]
    [InlineData("uae.g10.general.u15.l04.volume_pi_coefficient")]
    [InlineData("uae.g10.general.u15.l04.hemisphere_total_surface_pi_coefficient")]
    [InlineData("uae.g10.general.u15.l04.hemisphere_volume_pi_coefficient")]
    [InlineData("uae.g10.general.u15.l04.radius_from_surface")]
    [InlineData("uae.g10.general.u15.l04.radius_from_volume")]
    [InlineData("uae.g10.general.u15.l04.radius_from_great_circle")]
    public void SeededQuestionsRoundTrip(string family){
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var engine=new ExactSkillContractQuestionEngine();
        for(int seed=1;seed<=36;seed++){
            var q=Assert.Single(engine.Generate("g10-source-test","u15-l04",new[]{family},ExactSkillQuestionDifficulty.Standard,1,seed,System.Array.Empty<string>()));
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,"-99999"));
        }
    }
}
