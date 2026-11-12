using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit2Lesson08BarModelTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:02:08";
    [Fact]
    public void ThreeBarModelFamiliesGenerateAndVerify()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(3,contract!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.NotNull(ui);
            for(int seed=1;seed<=5;seed++)
            foreach(var q in engine.Generate(Lesson,f,[f],
                ExactSkillQuestionDifficulty.Standard,1,seed,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-100"));
                Assert.Equal(2,q.Parameters.Count);
                Assert.DoesNotContain("result",q.Parameters.Keys);
                var wrong=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
                wrong["a"]=1;
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,wrong,q.CorrectAnswer));
            }
        }
    }
    [Fact]
    public void PartWholeAndComparisonWorkForArabicNumbers()
    {
        var p=new Dictionary<string,int>{{"a",52672},{"b",34089}};
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l08.bar_compare",p,"18583"));
        p["b"]=14089;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l08.bar_compare",p,"٣٨٬٥٨٣"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l08.bar_part_whole",p,"66761"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l08.bar_missing_part",p,"38583"));
    }
    [Fact]
    public void ArabicStudentExplanationsIncludeModelAndChecks()
    {
        var rich=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(rich);
        Assert.Contains("مخطط شريطي",rich!.Title);
        Assert.True(rich.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(rich.Visuals).Kind);
    }
}
