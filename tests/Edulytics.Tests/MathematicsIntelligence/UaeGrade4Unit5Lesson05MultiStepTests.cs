using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit5Lesson05MultiStepTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:05:05";
    [Fact]
    public void TwoStepFamiliesGenerateAndVerify()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(2,contract!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,3,20261111,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-1"));
                Assert.Equal(3,q.Parameters.Count);
            }
        }
    }
    [Fact]
    public void SavingsAndMissingTermsAreExact()
    {
        var p=new Dictionary<string,int>{{"first",12},{"second",3},{"count",14}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u05.l05.net_weekly_savings",p,"١٢٦"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u05.l05.missing_after_groups",p,"١٥"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u05.l05.net_weekly_savings",p,"168"));
    }
    [Fact]
    public void ArabicRichLessonHasMultistepExamples()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Contains("عدة خطوات",l!.Title);
        Assert.True(l.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}
