using Xunit;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public class UaeG10GeometricExponentialTests {
 [Theory]
 [InlineData("uae.g10.general.u17.l05.identify_geometric")]
 [InlineData("uae.g10.general.u17.l05.common_ratio")]
 [InlineData("uae.g10.general.u17.l05.nth_term_signed")]
 [InlineData("uae.g10.general.u17.l05.nth_term_decay")]
 [InlineData("uae.g10.general.u17.l05.initial_term")]
 [InlineData("uae.g10.general.u17.l05.average_rate")]
 [InlineData("uae.g10.general.u17.l05.growth_model")]
 public void SeededQuestionsRoundTrip(string family){Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));var engine=new ExactSkillContractQuestionEngine();for(int seed=1;seed<=36;seed++){var q=Assert.Single(engine.Generate("g10-source-test","u17-l05",new[]{family},ExactSkillQuestionDifficulty.Standard,1,seed,System.Array.Empty<string>()));Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));Assert.False(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,"-99999"));}}
}
