using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U4L08WholeDivideFractionsTests
{
    const string L="PED:UAE-MOE-MATH:L6:GENERAL:04:08:STUDENT-AR-128", F="uae.g6.u04.l08.reciprocal_number";
    [Fact] public void OriginalArabicLessonReusesDivisionAndReciprocalFamilies()
    {
        var l=RichLessonContentV2Registry.Find(L,"ar");
        Assert.NotNull(l);
        Assert.Equal(10,l!.WorkedExamples.Count);
        Assert.Contains(l.Visuals,v=>v.Kind==RichLessonVisualKind.CalculationChain);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
        Assert.Equal(3,c!.AllowedQuestionFamilies.Count);
        foreach(var family in c.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
    }
    [Fact] public void ReciprocalSupportsArabicDigitsAndRejectsWrongOrZero()
    {
        var p=new Dictionary<string,int>{{"a",2},{"b",3}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(F,p,"3/2"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(F,p,"٣/٢"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,p,"2/3"));
        var whole=new Dictionary<string,int>{{"a",5},{"b",1}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(F,whole,"1/5"));
        var unit=new Dictionary<string,int>{{"a",1},{"b",8}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(F,unit,"8"));
        var invalid=new Dictionary<string,int>{{"a",0},{"b",3}};
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,invalid,"3"));
    }
    [Fact] public void WholeByFractionExactEngineRemainsAvailable()
    {
        var nonunit=new Dictionary<string,int>{{"n",4},{"d",3},{"a",2}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            "uae.g6.u04.l07.whole_divide_nonunit_fraction",nonunit,"6"));
        var unit=new Dictionary<string,int>{{"n",3},{"d",2}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u09.l09.whole_divide_unit_fraction",unit,"6"));
    }
}
