using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U3L06FindPatternInquiryTests
{
    const string L="PED:UAE-MOE-MATH:L6:GENERAL:03:06:STUDENT-AR-128";
    [Fact] public void ArabicInquiryHasPatternRubricAndReusedNumericFamilies()
    {
        var rich=RichLessonContentV2Registry.Find(L,"ar");
        Assert.NotNull(rich);
        Assert.Contains(rich!.Visuals,v=>v.Kind==RichLessonVisualKind.RatioTable);
        Assert.Contains(rich.Visuals,v=>v.Kind==RichLessonVisualKind.CalculationChain);
        Assert.True(rich.WorkedExamples.Count>=7);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var contract));
        Assert.Equal(2,contract!.AllowedQuestionFamilies.Count);
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out _));
        }
    }
    [Fact] public void LocalStudentBookNumericPatternsAreExact()
    {
        Assert.Equal(28.50m,4.75m*6m);
        Assert.Equal(627.50m,125.50m*5m);
        Assert.Equal(7.95m,15.90m/2m);
        Assert.Equal(383.5m,127m*3m+2.5m);
        Assert.Equal(1153m,383.5m*3m+2.5m);
        Assert.Equal(3461.5m,1153m*3m+2.5m);
        Assert.Equal(26.04m,400m-235.99m-45.99m*3m);
    }
    [Fact] public void ExistingNumberAndPricePatternsRemainVerifiable()
    {
        var e=new ExactSkillContractQuestionEngine();
        foreach(var f in new[]{"uae.g5.u05.l07.arithmetic_step_pattern","uae.g5.u05.l03.unit_price_total"})
        {
            var qs=e.Generate(L,f,[f],ExactSkillQuestionDifficulty.Standard,4,20261114,[]);
            Assert.Equal(4,qs.Count);
            foreach(var q in qs)
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
        }
    }
}
