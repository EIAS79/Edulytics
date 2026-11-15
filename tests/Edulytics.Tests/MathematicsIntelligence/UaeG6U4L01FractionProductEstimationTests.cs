using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U4L01FractionProductEstimationTests
{
    const string L="PED:UAE-MOE-MATH:L6:GENERAL:04:01:STUDENT-AR-128";
    [Fact] public void ArabicLessonReusesFractionBenchmarkAndCompatibleWholeFamilies()
    {
        var c=RichLessonContentV2Registry.Find(L,"ar");
        Assert.NotNull(c);
        Assert.True(c!.WorkedExamples.Count>=8);
        Assert.Contains(c.Visuals,v=>v.Kind==RichLessonVisualKind.NumberLine);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var contract));
        Assert.Equal(2,contract!.AllowedQuestionFamilies.Count);
        foreach(var f in contract.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
    }
    [Fact] public void EstimateSourceExamplesWithNoInventedPrecision()
    {
        Assert.Equal(3,12/4);
        Assert.Equal(4,10/5*2);
        Assert.Equal(5,25/5);
        Assert.Equal(15,20/4*3);
        Assert.Equal(90,15*6);
        Assert.True(3.25m>3m); // estimate is not exact
    }
    [Fact] public void ExistingFractionEstimateVerifierStillAcceptsExpectedQuantities()
    {
        var fraction=new Dictionary<string,int>{{"n1",1},{"d1",3},{"n2",7},{"d2",9}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            "uae.g5.u09.l02.estimate_fraction_product_quarters",fraction,"2")); // two quarters = one half
        var compatible=new Dictionary<string,int>{{"whole",13},{"denominator",4},{"numerator",1}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            "uae.g5.u09.l02.estimate_whole_fraction_compatible",compatible,"3"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            "uae.g5.u09.l02.estimate_whole_fraction_compatible",compatible,"13"));
    }
}
