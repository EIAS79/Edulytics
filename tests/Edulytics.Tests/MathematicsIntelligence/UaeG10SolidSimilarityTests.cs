using Xunit;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public class UaeG10SolidSimilarityTests {
 [Theory]
 [InlineData("uae.g10.general.u15.l08.box_similarity")]
 [InlineData("uae.g10.general.u15.l08.scale_ratio")]
 [InlineData("uae.g10.general.u15.l08.surface_area_ratio")]
 [InlineData("uae.g10.general.u15.l08.volume_ratio")]
 [InlineData("uae.g10.general.u15.l08.surface_area_scaled")]
 [InlineData("uae.g10.general.u15.l08.volume_scaled")]
 [InlineData("uae.g10.general.u15.l08.length_from_volume_ratio")]
 [InlineData("uae.g10.general.u15.l08.length_from_area_ratio")]
 public void SeededQuestionsRoundTrip(string family){Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));var engine=new ExactSkillContractQuestionEngine();for(int seed=1;seed<=36;seed++){var q=Assert.Single(engine.Generate("g10-source-test","u15-l08",new[]{family},ExactSkillQuestionDifficulty.Standard,1,seed,System.Array.Empty<string>()));Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));Assert.False(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,"-99999"));}}
}
