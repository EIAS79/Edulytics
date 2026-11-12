using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit6Lesson11MultistepDivisionTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:06:11";
    [Fact]
    public void MultiStepFamiliesGenerateAndVerify()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(2,contract!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,family,[family],ExactSkillQuestionDifficulty.Standard,3,20261111,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-1"));
            }
        }
    }
    [Fact]
    public void WeeklySavingsRequiresSubtractionBeforeDivision()
    {
        var p=new Dictionary<string,int>{{"first",7},{"second",2},{"count",9}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u06.l11.weeks_to_save_target",p,"٩"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u06.l11.weeks_to_save_target",p,"٦"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u05.l05.missing_after_groups",p,"٩"));
    }
    [Fact]
    public void RichArabicCoversSequentialSolutions()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("حل المسائل الكلامية متعددة الخطوات",l!.Title);
        Assert.True(l.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}
