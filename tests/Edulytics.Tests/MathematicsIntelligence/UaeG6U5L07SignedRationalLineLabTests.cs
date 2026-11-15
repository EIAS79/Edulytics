using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U5L07SignedRationalLineLabTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L6:GENERAL:05:07:STUDENT-AR-128", Compare="uae.g6.u05.l07.compare_signed_rationals", Side="uae.g6.u05.l07.rational_relative_zero";
    [Fact] public void ArabicSignedRationalLabHasOriginalContentAndTwoExactFamilies()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal(10,l!.WorkedExamples.Count);
        Assert.All(l.WorkedExamples,x=>Assert.True(x.Steps.Count>=3));
        Assert.Contains(l.Visuals,x=>x.Kind==RichLessonVisualKind.RatioTable);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.Equal(2,c!.AllowedQuestionFamilies.Count);
        foreach(var f in c.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
    }
    [Theory]
    [InlineData(-3,4,1,10,"<")]
    [InlineData(-4,5,-1,2,"<")]
    [InlineData(7,10,-3,8,">")]
    [InlineData(1,3,1,3,"=")]
    public void SignedFractionsCompareViaExactCrossProducts(int a,int b,int c,int d,string answer)
    {
        var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",c},{"d",d}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Compare,p,answer));
    }
    [Fact] public void SignedFractionsCompareToZeroAndFailClosed()
    {
        var p=new Dictionary<string,int>{{"n",-3},{"d",4}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Side,p,"<"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Side,p,">"));
        p["n"]=0;
        Assert.True(ExactSkillContractQuestionEngine.Verify(Side,p,"="));
        p["n"]=3;
        Assert.True(ExactSkillContractQuestionEngine.Verify(Side,p,">"));
        p["d"]=0;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Side,p,">"));
        var earlier=new Dictionary<string,int>{{"a",-3},{"b",-8}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u05.l05.compare_signed_integers",earlier,">"));
    }
}
