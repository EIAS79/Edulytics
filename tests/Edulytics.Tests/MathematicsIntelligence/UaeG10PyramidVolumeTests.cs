using Xunit;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public class UaeG10PyramidVolumeTests {
    [Theory]
    [InlineData("uae.g10.general.u15.l06.rectangular_pyramid_volume")]
    [InlineData("uae.g10.general.u15.l06.cone_volume_pi_coefficient")]
    [InlineData("uae.g10.general.u15.l06.cone_volume_from_slant_pi_coefficient")]
    [InlineData("uae.g10.general.u15.l06.square_pyramid_volume_from_slant")]
    [InlineData("uae.g10.general.u15.l06.cone_height")]
    [InlineData("uae.g10.general.u15.l06.cone_radius")]
    [InlineData("uae.g10.general.u15.l06.composite_cylinder_cone_pi_coefficient")]
    public void SeededQuestionsRoundTrip(string family){
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var engine=new ExactSkillContractQuestionEngine();
        for(int seed=1;seed<=36;seed++){
            var q=Assert.Single(engine.Generate("g10-source-test","u15-l06",new[]{family},ExactSkillQuestionDifficulty.Standard,1,seed,System.Array.Empty<string>()));
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,"-99999"));
        }
    }
}
