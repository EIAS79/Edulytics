using Xunit;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public class UaeG10AngleMeasureTests
{
    [Theory]
    [InlineData("uae.g10.general.u14.l02.principal_degree")]
    [InlineData("uae.g10.general.u14.l02.coterminal_degree")]
    [InlineData("uae.g10.general.u14.l02.degree_to_pi_coefficient")]
    [InlineData("uae.g10.general.u14.l02.pi_coefficient_to_degree")]
    [InlineData("uae.g10.general.u14.l02.principal_radian_pi_coefficient")]
    [InlineData("uae.g10.general.u14.l02.arc_pi_coefficient")]
    [InlineData("uae.g10.general.u14.l02.arc_from_radians")]
    [InlineData("uae.g10.general.u14.l02.radians_from_arc")]
    [InlineData("uae.g10.general.u14.l02.radius_from_arc")]
    [InlineData("uae.g10.general.u14.l02.parallelogram_area_exact")]
    [InlineData("uae.g10.general.u14.l02.wheel_arc_pi_coefficient")]
    public void SeededQuestionsRoundTrip(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var engine=new ExactSkillContractQuestionEngine();
        var q=Assert.Single(engine.Generate("g10-source-test","u14-l02",new[]{family},ExactSkillQuestionDifficulty.Standard,1,108,System.Array.Empty<string>()));
        Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
        Assert.False(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,"-99999"));
    }
}
