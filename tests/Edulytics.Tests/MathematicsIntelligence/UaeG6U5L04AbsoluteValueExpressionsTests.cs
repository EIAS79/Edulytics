using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U5L04AbsoluteValueExpressionsTests
{
    const string L="PED:UAE-MOE-MATH:L6:GENERAL:05:04:STUDENT-AR-128",F="uae.g6.u05.l04.absolute_expression";
    [Fact] public void OriginalArabicLessonHasBoundedExistingPracticeAndThreeSteps()
    {
        var l=RichLessonContentV2Registry.Find(L,"ar");
        Assert.NotNull(l);
        Assert.Equal(10,l!.WorkedExamples.Count);
        Assert.All(l.WorkedExamples,x=>Assert.True(x.Steps.Count>=3));
        Assert.Contains(l.Visuals,v=>v.Kind==RichLessonVisualKind.CalculationChain);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
        Assert.Equal(3,c!.AllowedQuestionFamilies.Count);
        foreach(var f in c.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
    }
    [Theory]
    [InlineData(-5,7,0,"12")]
    [InlineData(-14,6,1,"8")]
    [InlineData(3,-10,1,"-7")]
    [InlineData(-3,8,0,"١١")]
    public void SumAndDifferenceOfAbsoluteValuesAreExact(int a,int b,int operation,string expected)
    {
        var p=new Dictionary<string,int>{{"a",a},{"b",b},{"operation",operation}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(F,p,expected));
    }
    [Fact] public void RejectMalformedInputAndKeepEarlierFamilies()
    {
        var bad=new Dictionary<string,int>{{"a",3},{"b",-10},{"operation",2}};
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,bad,"-7"));
        var good=new Dictionary<string,int>{{"a",3},{"b",-10},{"operation",1}};
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,good,"7"));
        var dist=new Dictionary<string,int>{{"n",-9}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u05.l03.absolute_distance",dist,"9"));
        var earlier=new Dictionary<string,int>{{"magnitude",4},{"sign",-1}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u05.l01.opposite_integer",earlier,"4"));
    }
}
