using Xunit;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public class UaeG10TrigGraphsTests
{
    [Theory]
    [InlineData("uae.g10.general.u14.l07.amplitude")]
    [InlineData("uae.g10.general.u14.l07.period_pi_coefficient")]
    [InlineData("uae.g10.general.u14.l07.quarter_cycle_value")]
    [InlineData("uae.g10.general.u14.l07.frequency_hz")]
    [InlineData("uae.g10.general.u14.l07.range_extreme")]
    [InlineData("uae.g10.general.u14.l07.first_positive_asymptote_degree")]
    [InlineData("uae.g10.general.u14.l07.reciprocal_range_boundary")]
    public void SeededQuestionsRoundTrip(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var engine=new ExactSkillContractQuestionEngine();
        for(int seed=1;seed<=24;seed++){
            var q=Assert.Single(engine.Generate("g10-source-test","u14-l07",new[]{family},ExactSkillQuestionDifficulty.Standard,1,seed,System.Array.Empty<string>()));
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,"-99999"));
        }
    }
}
