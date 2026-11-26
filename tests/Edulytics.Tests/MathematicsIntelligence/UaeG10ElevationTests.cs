using Xunit;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public class UaeG10ElevationTests {
    [Theory]
    [InlineData("uae.g10.general.u14.l01.elevation_height_rounded")]
    [InlineData("uae.g10.general.u14.l01.depression_distance_rounded")]
    [InlineData("uae.g10.general.u14.l17.height_with_eye_rounded")]
    [InlineData("uae.g10.general.u14.l17.two_station_height_rounded")]
    [InlineData("uae.g10.general.u14.l17.line_of_sight_rounded")]
    [InlineData("uae.g10.general.u14.l17.angle_from_rise_run_rounded")]
    public void SeededQuestionsRoundTrip(string family){
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var engine=new ExactSkillContractQuestionEngine();
        for(int seed=1;seed<=36;seed++){
            var q=Assert.Single(engine.Generate("g10-source-test","u14-l17",new[]{family},ExactSkillQuestionDifficulty.Standard,1,seed,System.Array.Empty<string>()));
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,"-99999"));
        }
    }
}
