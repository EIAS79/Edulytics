using Xunit;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public class UaeG10PyramidSurfaceTests {
    [Theory]
    [InlineData("uae.g10.general.u15.l03.square_pyramid_lateral")]
    [InlineData("uae.g10.general.u15.l03.square_pyramid_total")]
    [InlineData("uae.g10.general.u15.l03.square_pyramid_slant")]
    [InlineData("uae.g10.general.u15.l03.cone_lateral_pi_coefficient")]
    [InlineData("uae.g10.general.u15.l03.cone_total_pi_coefficient")]
    [InlineData("uae.g10.general.u15.l03.cone_sector_angle")]
    public void SeededQuestionsRoundTrip(string family){
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var engine=new ExactSkillContractQuestionEngine();
        for(int seed=1;seed<=36;seed++){
            var q=Assert.Single(engine.Generate("g10-source-test","u15-l03",new[]{family},ExactSkillQuestionDifficulty.Standard,1,seed,System.Array.Empty<string>()));
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,"-99999"));
        }
    }
}
