using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Local Arabic Grade5 print pp283–288; test authored for Stage D, NOT_RUN.
public sealed class UaeGrade5Unit4Lesson06SimplerProblemTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:04:06";
    [Fact]
    public void FourSkillsUseExistingExactFourStepEngine()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal("problem_solving.four_steps_with_whole_numbers",contract!.SkillId);
        Assert.Equal(4,contract.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,2,20261113,[]))
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
        }
    }
    [Fact]
    public void UnitRateAreaAndThresholdRecomputeFromQuestionParameters()
    {
        var rate=new Dictionary<string,int>{{"groups",23},{"total",3082}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u04.l06.equal_group_unit_rate",rate,"١٣٤"));
        rate["target"]=3;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u04.l06.simpler_scaled_groups",rate,"٤٠٢"));
        var area=new Dictionary<string,int>{{"length",6},{"width",5},{"side",2}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u04.l06.remaining_area",area,"٢٦"));
        var saving=new Dictionary<string,int>{{"income",50},{"spend",10},{"target",300}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u04.l06.saving_weeks_strictly_more",saving,"٨"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u04.l06.saving_weeks_strictly_more",saving,"٧"));
    }
}