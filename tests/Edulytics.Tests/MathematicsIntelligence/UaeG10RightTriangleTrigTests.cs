using Xunit;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public class UaeG10RightTriangleTrigTests
{
    [Theory]
    [InlineData("uae.g10.general.u14.l01.ratio_six_exact")]
    [InlineData("uae.g10.general.u14.l01.side_opposite_exact")]
    [InlineData("uae.g10.general.u14.l01.side_adjacent_exact")]
    [InlineData("uae.g10.general.u14.l01.side_hypotenuse_exact")]
    [InlineData("uae.g10.general.u14.l01.angle_inverse_sine_rounded")]
    [InlineData("uae.g10.general.u14.l01.angle_inverse_cosine_rounded")]
    [InlineData("uae.g10.general.u14.l01.angle_inverse_tangent_rounded")]
    [InlineData("uae.g10.general.u14.l01.special_angle_exact")]
    [InlineData("uae.g10.general.u14.l01.elevation_height_rounded")]
    [InlineData("uae.g10.general.u14.l01.depression_distance_rounded")]
    public void SeededQuestionsRoundTrip(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var engine=new ExactSkillContractQuestionEngine();
        var questions=engine.Generate("g10-source-test","u14-l01",new[]{family},ExactSkillQuestionDifficulty.Standard,1,107,System.Array.Empty<string>());
        var q=Assert.Single(questions);
        Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
        Assert.False(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,"-99999"));
    }
}
