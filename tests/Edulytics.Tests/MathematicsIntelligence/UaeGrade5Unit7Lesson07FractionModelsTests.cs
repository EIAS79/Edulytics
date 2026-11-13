using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Written for Stage D; NOT_RUN here.
public sealed class UaeGrade5Unit7Lesson07FractionModelsTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:07:07";
    [Fact] public void SourceFractionsHaveExactDecimalModelAnswers()
    {
        var half=new Dictionary<string,int>{{"n",1},{"d",2}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u07.l07.model_tenths_decimal",half,"٠٫٥"));
        var quarters=new Dictionary<string,int>{{"n",3},{"d",4}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u07.l07.model_hundredths_decimal",quarters,"0.75"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u07.l07.count_shaded_hundredths",quarters,"75"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u07.l07.count_shaded_hundredths",quarters,"3"));
        var twentieth=new Dictionary<string,int>{{"n",7},{"d",20}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u07.l07.model_hundredths_decimal",twentieth,"0.35"));
    }
    [Fact] public void RoutesExactDecimalAndCountFamilies()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal("decimals.place_value",contract!.SkillId);
        Assert.Equal(3,contract.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            foreach(var q in engine.Generate(Lesson,family,[family],
                ExactSkillQuestionDifficulty.Standard,1,20261113,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-1"));
            }
        }
    }
}