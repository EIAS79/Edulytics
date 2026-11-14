using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U5L09SignedRationalComparisonTests
{
    const string L="PED:UAE-MOE-MATH:L6:GENERAL:05:09:STUDENT-AR-128",Compare="uae.g6.u05.l09.compare_mixed_signed_rationals",Order="uae.g6.u05.l09.order_four_signed_rationals";
    [Fact] public void ArabicLessonHasExactComparisonAndOrderingPractice()
    {
        var lesson=RichLessonContentV2Registry.Find(L,"ar");
        Assert.NotNull(lesson);
        Assert.Equal(10,lesson!.WorkedExamples.Count);
        Assert.All(lesson.WorkedExamples,e=>Assert.True(e.Steps.Count>=3));
        Assert.Contains(lesson.Visuals,v=>v.Kind==RichLessonVisualKind.RatioTable);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
        Assert.Equal(2,c!.AllowedQuestionFamilies.Count);
        foreach(var f in c.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
    }
    [Theory]
    [InlineData(-3,8,-5,16,"<")]
    [InlineData(-1,3,-33,100,"<")]
    [InlineData(-5,6,-17,20,">")]
    [InlineData(13,20,2,3,"<")]
    [InlineData(1,2,2,4,"=")]
    public void FractionsAndSignedDecimalsCompareWithoutRounding(int n1,int d1,int n2,int d2,string symbol)
    {
        var p=new Dictionary<string,int>{{"n1",n1},{"d1",d1},{"n2",n2},{"d2",d2}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Compare,p,symbol));
    }
    [Fact] public void FourMixedRationalsOrderExactlyByOriginalPositions()
    {
        var p=new Dictionary<string,int>{
            {"n1",13},{"d1",20},{"n2",-3},{"d2",5},
            {"n3",2},{"d3",3},{"n4",4},{"d4",5},{"mode",0}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Order,p,"2,1,3,4"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Order,p,"٢،١،٣،٤"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Order,p,"2,3,1,4"));
        p["mode"]=1;
        Assert.True(ExactSkillContractQuestionEngine.Verify(Order,p,"4,3,1,2"));
        p["d4"]=0;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Order,p,"4,3,1,2"));
    }
}
