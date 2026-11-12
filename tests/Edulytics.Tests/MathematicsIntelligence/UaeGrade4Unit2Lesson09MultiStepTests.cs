using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit2Lesson09MultiStepTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:02:09";
    [Fact]
    public void FourMultistepFamiliesAreRoutedAndVerifyWithoutLeakingAnswers()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(4,contract!.AllowedQuestionFamilies.Count);
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
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-999"));
                Assert.Equal(3,q.Parameters.Count);
                Assert.DoesNotContain("result",q.Parameters.Keys);
                var corrupted=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
                corrupted["a"]=0;
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,corrupted,q.CorrectAnswer));
            }
        }
    }
    [Fact]
    public void IndependentEquationValuesAndArabicDigits()
    {
        var p=new Dictionary<string,int>{{"a",5200},{"b",725},{"c",65}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l09.add_then_subtract",p,"٥٬٨٦٠"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l09.subtract_then_add",p,"4540"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l09.add_three_parts",p,"5990"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l09.subtract_two_costs",p,"4410"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l09.add_then_subtract",p,"5861"));
    }
    [Fact]
    public void ArabicStudentLessonCoversVariableMultipleStepsAndVisual()
    {
        var rich=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(rich);
        Assert.Contains("عدة خطوات",rich!.Title);
        Assert.True(rich.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.CalculationChain,Assert.Single(rich.Visuals).Kind);
    }
}
