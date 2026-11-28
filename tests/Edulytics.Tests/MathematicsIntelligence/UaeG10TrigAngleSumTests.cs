using Xunit;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public class UaeG10TrigAngleSumTests {
    [Theory]
    [InlineData("uae.g10.general.u14.l12.special_angle_exact_choice")]
    [InlineData("uae.g10.general.u14.l12.sine_sum_difference")]
    [InlineData("uae.g10.general.u14.l12.cosine_sum_difference")]
    [InlineData("uae.g10.general.u14.l12.tangent_sum_difference")]
    public void SeededQuestionsRoundTrip(string family){
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var engine=new ExactSkillContractQuestionEngine();
        for(int seed=1;seed<=36;seed++){
            var q=Assert.Single(engine.Generate("g10-source-test","u14-l12",new[]{family},ExactSkillQuestionDifficulty.Standard,1,seed,System.Array.Empty<string>()));
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,"-99999"));
        }
    }
}
