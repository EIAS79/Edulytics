using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Grade5 Stage A-B-C authored; NOT_RUN before Stage D.
public sealed class UaeGrade5Unit7Lesson08FractionsAsDecimalsTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:07:08";
    [Fact] public void ReusableModelsAndReducibleFractionsMatchBookGoal()
    {
        var model=new Dictionary<string,int>{{"n",11},{"d",25}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            "uae.g5.u07.l07.model_hundredths_decimal",model,"٠٫٤٤"));
        var reducible=new Dictionary<string,int>{{"n",9},{"d",12}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            "uae.g5.u07.l08.reducible_fraction_to_decimal",reducible,"0.75"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            "uae.g5.u07.l08.reducible_fraction_to_decimal",reducible,"0.25"));
        reducible["d"]=7;
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            "uae.g5.u07.l08.reducible_fraction_to_decimal",reducible,"0.75"));
    }
    [Fact] public void Grade5LessonPracticeHasReusableExactContracts()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal("decimals.place_value",contract!.SkillId);
        Assert.Equal(3,contract.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,family,[family],
                ExactSkillQuestionDifficulty.Standard,1,20261113,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-1"));
            }
        }
    }
}