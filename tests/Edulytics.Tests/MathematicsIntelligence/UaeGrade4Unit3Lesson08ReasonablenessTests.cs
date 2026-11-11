using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit3Lesson08ReasonablenessTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:03:08";
    [Fact]
    public void AllThreeReasonabilityFamiliesRouteGenerateAndCheck()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(3,contract!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.NotNull(ui);
            for(int seed=1;seed<=6;seed++)
            foreach(var q in engine.Generate(Lesson,f,[f],
                ExactSkillQuestionDifficulty.Standard,1,seed,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-99"));
                Assert.Equal(3,q.Parameters.Count);
                Assert.DoesNotContain("result",q.Parameters.Keys);
                var invalid=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
                invalid["a"]=0;
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,invalid,q.CorrectAnswer));
            }
        }
    }
    [Fact]
    public void ClaimTestingSeparatesValidAndInvalidEstimates()
    {
        var p=new Dictionary<string,int>{{"a",283},{"b",302},{"c",3}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u03.l08.sum_estimate",p,"٠"));
        p["c"]=2;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u03.l08.sum_estimate",p,"١"));
        p["a"]=400;p["b"]=200;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u03.l08.savings_feasible",p,"1"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u03.l08.travel_feasible",p,"١"));
        p["a"]=150;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u03.l08.savings_feasible",p,"0"));
    }
    [Fact]
    public void RichStudentLessonContainsReasonedEstimates()
    {
        var rich=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(rich);
        Assert.Contains("الإجابات المنطقية",rich!.Title);
        Assert.True(rich.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(rich.Visuals).Kind);
    }
}
