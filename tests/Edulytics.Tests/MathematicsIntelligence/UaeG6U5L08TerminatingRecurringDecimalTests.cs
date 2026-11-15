using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U5L08TerminatingRecurringDecimalTests
{
    const string Lesson="PED:UAE-MOE-MATH:L6:GENERAL:05:08:STUDENT-AR-128", Kind="uae.g6.u05.l08.classify_fraction_decimal", Exact="uae.g6.u05.l08.fraction_exact_decimal";
    [Fact] public void ArabicLessonHasThreeFamiliesAndWorkedSolutions()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal(10,l!.WorkedExamples.Count);
        Assert.All(l.WorkedExamples,e=>Assert.True(e.Steps.Count>=3));
        Assert.Contains(l.Visuals,v=>v.Kind==RichLessonVisualKind.RatioTable);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.Equal(3,c!.AllowedQuestionFamilies.Count);
        foreach(var family in c.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
    }
    [Theory]
    [InlineData(3,8,"منتهٍ","0.375")]
    [InlineData(1,3,"دوري","0.(3)")]
    [InlineData(-2,9,"دوري","-0.(2)")]
    [InlineData(5,6,"دوري","0.8(3)")]
    [InlineData(5,12,"دوري","0.41(6)")]
    [InlineData(3,11,"دوري","0.(27)")]
    [InlineData(1,7,"دوري","0.(142857)")]
    [InlineData(-7,4,"منتهٍ","-1.75")]
    public void ClassificationAndExactCycleAreSeparateFromApproximation(int n,int d,string kind,string value)
    {
        var p=new Dictionary<string,int>{{"n",n},{"d",d}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Kind,p,kind));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Exact,p,value));
        if(value.Contains("("))
            Assert.False(ExactSkillContractQuestionEngine.Verify(Exact,p,value.Replace("(","").Replace(")","")));
    }
    [Fact] public void ArabicNumbersAndInvalidDenominatorsFailClosed()
    {
        var p=new Dictionary<string,int>{{"n",1},{"d",3}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Exact,p,"٠.(٣)"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Exact,p,"0.333"));
        p["d"]=0;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Exact,p,"0"));
        var prior=new Dictionary<string,int>{{"n",9},{"d",12}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u07.l08.reducible_fraction_to_decimal",prior,"0.75"));
    }
}
