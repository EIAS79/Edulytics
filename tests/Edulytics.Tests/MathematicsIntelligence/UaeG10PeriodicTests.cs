using Xunit;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public class UaeG10PeriodicTests
{
    [Theory]
    [InlineData("uae.g10.general.u14.l06.unit_circle_coordinate")]
    [InlineData("uae.g10.general.u14.l06.period_from_peaks")]
    [InlineData("uae.g10.general.u14.l06.periodic_value")]
    [InlineData("uae.g10.general.u14.l06.cycle_count")]
    [InlineData("uae.g10.general.u14.l06.axis_trig_value")]
    [InlineData("uae.g10.general.u14.l06.special_sin_cos_rational")]
    [InlineData("uae.g10.general.u14.l06.period_pi_coefficient")]
    public void SeededQuestionsRoundTrip(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var engine=new ExactSkillContractQuestionEngine();
        for(int seed=1;seed<=24;seed++)
        {
            var q=Assert.Single(engine.Generate("g10-source-test","u14-l06",new[]{family},ExactSkillQuestionDifficulty.Standard,1,seed,System.Array.Empty<string>()));
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,"-99999"));
        }
    }
}
