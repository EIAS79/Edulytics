using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit1Lesson06FourStepTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:01:06";
    [Fact]
    public void ThreeFourStepNumericalGoalsAreSelectedWithoutAnswerLeak()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(3,contract!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,f,[f],
                    ExactSkillQuestionDifficulty.Standard,2,20261111,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-100"));
                Assert.Equal(2,q.Parameters.Count);
                Assert.DoesNotContain("result",q.Parameters.Keys);
            }
        }
    }
    [Fact]
    public void OriginalWordProblemNumericsAreVerifiedIndependently()
    {
        var p=new Dictionary<string,int>{{"a",248920},{"b",261305-248920}};
        // Boundary: the second value is below the generator's range and must be rejected.
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            "uae.g4.u01.l06.find_increase",p,"12385"));
        p["a"]=325480;p["b"]=172310;
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            "uae.g4.u01.l06.find_increase",p,"١٧٢٬٣١٠"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            "uae.g4.u01.l06.find_total",p,"497790"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            "uae.g4.u01.l06.find_remaining",p,"172310"));
        p["a"]=450000;
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            "uae.g4.u01.l06.find_total",p,"622310"));
    }
    [Fact]
    public void RichStudentPageShowsFourStepVisualAndTeacherExplanation()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("استقصاء حل المسائل: استخدام خطة الخطوات الأربع",l!.Title);
        Assert.True(l.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.CalculationChain,Assert.Single(l.Visuals).Kind);
        Assert.Contains(l.ExplanationParagraphs,x=>x.Contains("المعلم"));
    }
}
