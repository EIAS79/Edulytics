using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U5L05CompareOrderSignedNumbersTests
{
    const string Lesson="PED:UAE-MOE-MATH:L6:GENERAL:05:05:STUDENT-AR-128",Compare="uae.g6.u05.l05.compare_signed_integers",Order="uae.g6.u05.l05.order_integer_set";
    [Fact] public void ArabicSignedCompareOrderHasExistingExactFamiliesAndThreeSteps()
    {
        var content=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(content);
        Assert.Equal(10,content!.WorkedExamples.Count);
        Assert.All(content.WorkedExamples,x=>Assert.True(x.Steps.Count>=3));
        Assert.Contains(content.Visuals,v=>v.Kind==RichLessonVisualKind.RatioTable);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.Equal(3,c!.AllowedQuestionFamilies.Count);
        foreach(var f in c.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
    }
    [Theory]
    [InlineData(-3,-8,">")]
    [InlineData(-9,0,"<")]
    [InlineData(5,-2,">")]
    [InlineData(4,4,"=")]
    public void SignedCompareUsesActualPositionNotAbsoluteValue(int a,int b,string answer)
    {
        var p=new Dictionary<string,int>{{"a",a},{"b",b}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Compare,p,answer));
    }
    [Fact] public void AscendingAndDescendingOrderWorksWithArabicDigits()
    {
        var p=new Dictionary<string,int>{{"a",5},{"b",-2},{"c",0},{"d",-7},{"mode",0}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Order,p,"-7,-2,0,5"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Order,p,"−٧،−٢،٠،٥"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Order,p,"-2,-7,0,5"));
        p["mode"]=1;
        Assert.True(ExactSkillContractQuestionEngine.Verify(Order,p,"5,0,-2,-7"));
        p["mode"]=2;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Order,p,"5,0,-2,-7"));
    }
    [Fact] public void PriorSignedSetExtremaRemainsSupported()
    {
        var p=new Dictionary<string,int>{{"a",-4},{"b",0},{"c",3},{"d",-1},{"mode",0}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u05.l02.signed_plot_set_extremum",p,"-4"));
    }
}
