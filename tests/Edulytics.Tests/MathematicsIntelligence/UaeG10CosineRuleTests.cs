using Xunit;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public class UaeG10CosineRuleTests
{
    [Theory]
    [InlineData("uae.g10.general.u14.l05.sas_side_rounded")]
    [InlineData("uae.g10.general.u14.l05.sas_square_exact")]
    [InlineData("uae.g10.general.u14.l05.sss_cosine_exact")]
    [InlineData("uae.g10.general.u14.l05.sss_angle_rounded")]
    [InlineData("uae.g10.general.u14.l05.triangle_type")]
    [InlineData("uae.g10.general.u14.l05.choose_law")]
    [InlineData("uae.g10.general.u14.l05.indirect_distance_rounded")]
    public void SeededQuestionsRoundTrip(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var engine=new ExactSkillContractQuestionEngine();
        for(int seed=1;seed<=24;seed++)
        {
            var q=Assert.Single(engine.Generate("g10-source-test","u14-l05",new[]{family},ExactSkillQuestionDifficulty.Standard,1,seed,System.Array.Empty<string>()));
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,"-99999"));
        }
    }
}
