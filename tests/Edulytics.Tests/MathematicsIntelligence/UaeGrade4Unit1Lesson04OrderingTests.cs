using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit1Lesson04OrderingTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:01:04";

    [Fact]
    public void SelectedThreeNumberOrderingFamiliesGenerateAndVerify()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(3,contract!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
            for(int seed=1;seed<=7;seed++)
            foreach(var q in engine.Generate(Lesson,family,[family],
                ExactSkillQuestionDifficulty.Standard,1,seed,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"bad"));
                Assert.Equal(3,q.Parameters.Count);
                Assert.DoesNotContain("result",q.Parameters.Keys);
                var bad=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
                bad["c"]=10_000_000;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,bad,q.CorrectAnswer));
            }
        }
    }

    [Fact]
    public void AscendingDescendingAndMiddleAreMathematicallyExact()
    {
        var p=new Dictionary<string,int>{{"a",508700},{"b",580070},{"c",508070}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            "uae.g4.u01.l04.order_three_ascending",p,"٥٠٨٠٧٠; ٥٠٨٧٠٠; ٥٨٠٠٧٠"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            "uae.g4.u01.l04.order_three_descending",p,"580070;508700;508070"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            "uae.g4.u01.l04.middle_of_three",p,"508700"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            "uae.g4.u01.l04.order_three_ascending",p,"508700;508070;580070"));
    }

    [Fact]
    public void LearnerRichContentRendersOriginalNumberLine()
    {
        var lesson=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(lesson);
        Assert.Equal("ترتيب الأعداد",lesson!.Title);
        Assert.True(lesson.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.NumberLine,Assert.Single(lesson.Visuals).Kind);
    }
}
