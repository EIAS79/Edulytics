using Xunit;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public class UaeG10TrigIdentitiesTests
{
    [Theory]
    [InlineData("uae.g10.general.u14.l10.cos_from_sin_quadrant")]
    [InlineData("uae.g10.general.u14.l10.sin_from_cos_quadrant")]
    [InlineData("uae.g10.general.u14.l10.sec_from_tan_quadrant")]
    [InlineData("uae.g10.general.u14.l10.csc_from_cot_quadrant")]
    [InlineData("uae.g10.general.u14.l10.quotient_from_pair")]
    [InlineData("uae.g10.general.u14.l10.identity_numeric_constant")]
    [InlineData("uae.g10.general.u14.l10.cofunction_angle_degrees")]
    [InlineData("uae.g10.general.u14.l10.odd_even_multiplier")]
    public void SeededQuestionsRoundTrip(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var engine=new ExactSkillContractQuestionEngine();
        for(int seed=1;seed<=24;seed++){
            var q=Assert.Single(engine.Generate("g10-source-test","u14-l10",new[]{family},ExactSkillQuestionDifficulty.Standard,1,seed,System.Array.Empty<string>()));
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,"-99999"));
        }
    }
}
