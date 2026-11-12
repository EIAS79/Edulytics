using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit4Lesson10PrecisionTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:04:10";
    [Fact]
    public void ExactEstimateAndChoiceFamiliesAreRouted()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(3,contract!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,3,20261111,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-8"));
            }
        }
    }
    [Fact]
    public void DistinguishesExactPaymentsFromApproximateBudgets()
    {
        var p=new Dictionary<string,int>{{"factor",5},{"amount",220},{"request",1}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u04.l10.choose_answer_precision",p,"١"));
        p["request"]=0;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u04.l10.choose_answer_precision",p,"٠"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u04.l10.choose_answer_precision",p,"1"));
    }
    [Fact]
    public void ArabicContentCoversBothPrecisionCases()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Contains("التقديرية",l!.Title);
        Assert.True(l.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}
