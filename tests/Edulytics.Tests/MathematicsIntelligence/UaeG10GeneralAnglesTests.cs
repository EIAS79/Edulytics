using Xunit;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public class UaeG10GeneralAnglesTests
{
    [Theory]
    [InlineData("uae.g10.general.u14.l03.coordinate_ratio")]
    [InlineData("uae.g10.general.u14.l03.reference_degree")]
    [InlineData("uae.g10.general.u14.l03.reference_radian_pi_coefficient")]
    [InlineData("uae.g10.general.u14.l03.quadrant_from_angle")]
    [InlineData("uae.g10.general.u14.l03.quadrant_from_signs")]
    [InlineData("uae.g10.general.u14.l03.axis_defined")]
    [InlineData("uae.g10.general.u14.l03.axis_value")]
    public void SeededQuestionsRoundTrip(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var engine=new ExactSkillContractQuestionEngine();
        for(int seed=1;seed<=24;seed++)
        {
            var q=Assert.Single(engine.Generate("g10-source-test","u14-l03",new[]{family},ExactSkillQuestionDifficulty.Standard,1,seed,System.Array.Empty<string>()));
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,"-99999"));
        }
    }
}
