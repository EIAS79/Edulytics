using Xunit;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public class UaeG10SphericalGeometryTests {
 [Theory]
 [InlineData("uae.g10.general.u15.l07.great_circle_classification")]
 [InlineData("uae.g10.general.u15.l07.great_circle_intersections")]
 [InlineData("uae.g10.general.u15.l07.great_circle_uniqueness")]
 [InlineData("uae.g10.general.u15.l07.arc_length_pi_coefficient")]
 public void SeededQuestionsRoundTrip(string family){Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));var engine=new ExactSkillContractQuestionEngine();for(int seed=1;seed<=36;seed++){var q=Assert.Single(engine.Generate("g10-source-test","u15-l07",new[]{family},ExactSkillQuestionDifficulty.Standard,1,seed,System.Array.Empty<string>()));Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));Assert.False(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,"-99999"));}}
}
