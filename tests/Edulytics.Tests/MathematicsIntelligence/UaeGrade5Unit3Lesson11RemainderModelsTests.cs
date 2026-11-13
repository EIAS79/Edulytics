using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// UAE Grade 5 source pp221-226, tests authored but NOT RUN.
public sealed class UaeGrade5Unit3Lesson11RemainderModelsTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:03:11";
    [Fact]
    public void ModelSharingContractsReuseExactDivisionEngine()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal("number.division.equal_groups_remainder_models",contract!.SkillId);
        Assert.Equal(2,contract.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
            foreach(var item in engine.Generate(Lesson,family,[family],
                     ExactSkillQuestionDifficulty.Standard,2,20261113,[]))
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,item.CorrectAnswer));
        }
    }
    [Fact]
    public void RemainderInRealLifeEqualsUnallocatedObjectsNotAnotherFullGroup()
    {
        var p=new Dictionary<string,int>{{"dividend",46},{"divisor",3}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l11.equal_group_count",p,"15"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l11.model_leftover_count",p,"1"));
        p["dividend"]=47;p["divisor"]=4;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l11.equal_group_count",p,"11"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l11.model_leftover_count",p,"3"));
        p["dividend"]=48;
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l11.model_leftover_count",p,"0"));
    }
}
