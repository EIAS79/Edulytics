using Xunit;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public class UaeG10RightTriangle122Tests {
    [Theory]
    [InlineData("geometry.right_triangle.pythagorean.exact")]
    [InlineData("geometry.right_triangle.pythagorean.find_leg_exact")]
    [InlineData("uae.g10.general.u14.l16.triangle_classification")]
    [InlineData("uae.g10.general.u14.l16.coordinate_distance_squared")]
    public void SeededQuestionsRoundTrip(string family){
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var engine=new ExactSkillContractQuestionEngine();
        for(int seed=1;seed<=36;seed++){
            var q=Assert.Single(engine.Generate("g10-source-test","u14-l16",new[]{family},ExactSkillQuestionDifficulty.Standard,1,seed,System.Array.Empty<string>()));
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,"-99999"));
        }
    }
}
