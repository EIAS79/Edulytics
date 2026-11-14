using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U3L08EstimateDivisionTests
{
    const string L="PED:UAE-MOE-MATH:L6:GENERAL:03:08:STUDENT-AR-128";
    [Fact] public void OriginalArabicEstimationHasVisualsAndExistingFamilyCoverage()
    {
        var v=RichLessonContentV2Registry.Find(L,"ar");
        Assert.NotNull(v);
        Assert.True(v!.WorkedExamples.Count>=8);
        Assert.Contains(v.Visuals,x=>x.Kind==RichLessonVisualKind.RatioTable);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
        Assert.Equal(3,c!.AllowedQuestionFamilies.Count);
        foreach(var family in c.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out _));
        }
    }
    [Theory]
    [InlineData(11.75,3,12,3,4)]
    [InlineData(61.25,5,60,5,12)]
    [InlineData(32,3.9,32,4,8)]
    [InlineData(56,6.8,56,7,8)]
    [InlineData(704.4,49.9,700,50,14)]
    [InlineData(49.3,7,49,7,7)]
    [InlineData(98.1,25,100,25,4)]
    [InlineData(54,9.16,54,9,6)]
    [InlineData(92.4,7.8,96,8,12)]
    [InlineData(38.04,4,40,4,10)]
    [InlineData(25,4.7,25,5,5)]
    public void CompatibleNumbersGiveStatedEstimate(double dividend,double divisor,int compatibleDividend,int compatibleDivisor,int estimate)
    {
        Assert.Equal(estimate,compatibleDividend/compatibleDivisor);
        Assert.True(Math.Abs(dividend/divisor-estimate)<=Math.Max(2.5,estimate*0.25));
    }
    [Fact] public void AlreadyApprovedDecimalAndIntegerEstimatorsStillVerify()
    {
        var e=new ExactSkillContractQuestionEngine();
        foreach(var f in new[]{"uae.g5.u05.l09.estimate_decimal_quotient","uae.g5.u05.l09.compatible_decimal_dividend","uae.g5.u04.l01.estimate_two_digit_quotient"})
        {
            var q=e.Generate(L,f,[f],ExactSkillQuestionDifficulty.Standard,3,20261114,[]);
            Assert.Equal(3,q.Count);
            foreach(var item in q)
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,item.Parameters,item.CorrectAnswer));
        }
    }
}
