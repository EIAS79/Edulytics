using Xunit;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public class UaeG10RecurrencesTests {
 [Theory]
 [InlineData("uae.g10.general.u17.l06.arithmetic_recurrence_term")]
 [InlineData("uae.g10.general.u17.l06.geometric_recurrence_term")]
 [InlineData("uae.g10.general.u17.l06.affine_recurrence_term")]
 [InlineData("uae.g10.general.u17.l06.index_dependent_recurrence")]
 [InlineData("uae.g10.general.u17.l06.two_initial_terms")]
 [InlineData("uae.g10.general.u17.l06.difference_from_explicit")]
 [InlineData("uae.g10.general.u17.l06.ratio_from_explicit")]
 [InlineData("uae.g10.general.u17.l06.initial_value_from_arithmetic")]
 public void SeededQuestionsRoundTrip(string family){Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));var engine=new ExactSkillContractQuestionEngine();for(int seed=1;seed<=36;seed++){var q=Assert.Single(engine.Generate("g10-source-test","u17-l06",new[]{family},ExactSkillQuestionDifficulty.Standard,1,seed,System.Array.Empty<string>()));Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));Assert.False(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,"-99999"));}}
}
