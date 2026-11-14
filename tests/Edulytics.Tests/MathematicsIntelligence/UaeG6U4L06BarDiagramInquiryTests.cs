using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U4L06BarDiagramInquiryTests
{
    const string L="PED:UAE-MOE-MATH:L6:GENERAL:04:06:STUDENT-AR-128";
    [Fact] public void OriginalArabicBarInquiryReusesExistingRatioDigitalSubskillsOnly()
    {
        var c=RichLessonContentV2Registry.Find(L,"ar");
        Assert.NotNull(c);
        Assert.Equal(10,c!.WorkedExamples.Count);
        Assert.Contains(c.Visuals,v=>v.Kind==RichLessonVisualKind.FractionBars);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var contract));
        Assert.Equal(2,contract!.AllowedQuestionFamilies.Count);
        foreach(var family in contract.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.Contains(c.ExplanationParagraphs,x=>x.Contains("المعلم"));
    }
    [Fact] public void PartWholeBarNumericsAreIndependentAndConsistent()
    {
        Assert.Equal(12,30-18);
        Assert.Equal(30,12*5/2);
        Assert.Equal(21,28*3/4);
        Assert.Equal(28,21*4/3);
        Assert.Equal(25,20*5/4);
        Assert.Equal(5,25-20);
        Assert.Equal(30,9*10/3);
        Assert.Equal(21,30-9);
        Assert.Equal(20,8*5/2);
        Assert.Equal(30,48*5/8);
        Assert.Equal(18,48-30);
    }
}
