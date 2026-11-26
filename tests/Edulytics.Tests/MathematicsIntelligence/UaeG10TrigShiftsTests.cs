using Xunit;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public class UaeG10TrigShiftsTests
{
    [Theory]
    [InlineData("uae.g10.general.u14.l08.phase_shift_degrees")]
    [InlineData("uae.g10.general.u14.l08.vertical_shift")]
    [InlineData("uae.g10.general.u14.l08.amplitude")]
    [InlineData("uae.g10.general.u14.l08.period_degrees")]
    [InlineData("uae.g10.general.u14.l08.range_extreme")]
    [InlineData("uae.g10.general.u14.l08.quarter_cycle_value")]
    [InlineData("uae.g10.general.u14.l08.midline_from_extrema")]
    [InlineData("uae.g10.general.u14.l08.amplitude_from_extrema")]
    [InlineData("uae.g10.general.u14.l08.coefficient_from_period")]
    public void SeededQuestionsRoundTrip(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var engine=new ExactSkillContractQuestionEngine();
        for(int seed=1;seed<=24;seed++){
            var q=Assert.Single(engine.Generate("g10-source-test","u14-l08",new[]{family},ExactSkillQuestionDifficulty.Standard,1,seed,System.Array.Empty<string>()));
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,"-99999"));
        }
    }
}
