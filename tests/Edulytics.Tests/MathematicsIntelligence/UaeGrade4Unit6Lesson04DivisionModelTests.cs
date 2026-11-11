using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit6Lesson04DivisionModelTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:06:04";
    [Fact]
    public void DivisionModelsGenerateAndVerify()
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
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-2"));
            }
        }
    }
    [Fact]
    public void OriginalBookFlowerAndFruitModelsHold()
    {
        var p=new Dictionary<string,int>{{"total",12},{"removed",2},{"groups",2}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u06.l04.model_after_removing_items_quotient",p,"٥"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u06.l04.model_after_removing_items_remainder",p,"٠"));
        p["total"]=128;p["removed"]=4;p["groups"]=4;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u06.l04.model_after_removing_items_quotient",p,"٣١"));
        p["total"]=83;p["removed"]=0;p["groups"]=4;
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u06.l04.model_after_removing_items_remainder",p,"٣"));
    }
    [Fact]
    public void ArabicProblemSolvingRichContentExists()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Contains("تصميم نموذج",l!.Title);
        Assert.True(l.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}
