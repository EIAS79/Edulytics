using Xunit;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public class UaeG10TrigDoubleHalfTests {
    [Theory]
    [InlineData("uae.g10.general.u14.l13.double_sine")]
    [InlineData("uae.g10.general.u14.l13.double_cosine")]
    [InlineData("uae.g10.general.u14.l13.double_tangent")]
    [InlineData("uae.g10.general.u14.l13.half_squared")]
    [InlineData("uae.g10.general.u14.l13.half_tangent")]
    [InlineData("uae.g10.general.u14.l13.half_exact_choice")]
    public void SeededQuestionsRoundTrip(string family){
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var engine=new ExactSkillContractQuestionEngine();
        for(int seed=1;seed<=36;seed++){
            var q=Assert.Single(engine.Generate("g10-source-test","u14-l13",new[]{family},ExactSkillQuestionDifficulty.Standard,1,seed,System.Array.Empty<string>()));
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,"-99999"));
        }
    }
}
