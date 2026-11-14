using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U4L11MixedFractionDivisionTests
{
    const string Lesson="PED:UAE-MOE-MATH:L6:GENERAL:04:11:STUDENT-AR-128",Family="uae.g6.u04.l11.mixed_fraction_division_exact";
    [Fact] public void OriginalArabicMixedDivisionHasExistingExactPractice()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal(10,l!.WorkedExamples.Count);
        Assert.Contains(l.Visuals,v=>v.Kind==RichLessonVisualKind.CalculationChain);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.Equal(2,c!.AllowedQuestionFamilies.Count);
        foreach(var family in c.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
    }
    [Fact] public void MixedByFractionAndMixedByMixedAreReducedExactly()
    {
        var a=new Dictionary<string,int>{{"w1",1},{"n1",1},{"d1",2},{"w2",0},{"n2",3},{"d2",4}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,a,"2"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,a,"٢"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,a,"3/2"));
        var b=new Dictionary<string,int>{{"w1",2},{"n1",1},{"d1",4},{"w2",0},{"n2",3},{"d2",5}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,b,"15/4"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,b,"45/12"));
        var c=new Dictionary<string,int>{{"w1",1},{"n1",1},{"d1",4},{"w2",2},{"n2",1},{"d2",2}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,c,"1/2"));
    }
    [Fact] public void WholeByMixedAndMixedByWholeAreSupportedWithInvalidOperandsFailClosed()
    {
        var a=new Dictionary<string,int>{{"w1",5},{"n1",0},{"d1",1},{"w2",1},{"n2",1},{"d2",4}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,a,"4"));
        var b=new Dictionary<string,int>{{"w1",2},{"n1",2},{"d1",3},{"w2",2},{"n2",0},{"d2",1}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,b,"4/3"));
        var zero=new Dictionary<string,int>{{"w1",2},{"n1",2},{"d1",3},{"w2",0},{"n2",0},{"d2",1}};
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,zero,"0"));
    }
}
