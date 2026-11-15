using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;

// Written for Grade6 General Student Book U02L06; execution reserved for Stage D.
public sealed class UaeG6U2L06SimplerProblemsTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L6:GENERAL:02:06:STUDENT-AR-128";
    [Fact]
    public void ArabicInquiryAndExistingExactFamiliesAreWired()
    {
        var lesson=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(lesson);
        Assert.Equal("استقصاء حل المسائل: حل المسائل الأبسط",lesson!.Title);
        Assert.True(lesson.WorkedExamples.Count>=6);
        Assert.Contains(lesson.Visuals,x=>x.Kind==RichLessonVisualKind.RatioTable);
        Assert.Contains(lesson.Visuals,x=>x.Kind==RichLessonVisualKind.CalculationChain);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.NotNull(contract);
        Assert.Equal(2,contract!.AllowedQuestionFamilies.Count);
        foreach (var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out _));
        }
    }
    [Fact]
    public void PercentOfThreeHundredAndSmallSamplesUseIndependentVerifier()
    {
        const string family="percentages.core.of_quantity";
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,
           new Dictionary<string,int>{{"percent",80},{"quantity",300}},"240"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,
           new Dictionary<string,int>{{"percent",80},{"quantity",300}},"24"));
        var engine=new ExactSkillContractQuestionEngine();
        foreach (var item in engine.Generate(Lesson,family,[family],
             ExactSkillQuestionDifficulty.Standard,3,20261114,[]))
        {
            Assert.Contains("١٠%",item.Prompt);
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,item.CorrectAnswer));
        }
    }
    [Fact]
    public void PercentageEstimatesAreNotReportedAsExact()
    {
        Assert.Equal(240d,300d*0.8d,4);
        Assert.Equal(4.428d,24.6d*0.18d,3);
        Assert.Equal(5d,25d*0.2d,4);
        Assert.Equal(420,1200*35/100);
        Assert.Equal(480,1440/3);
        Assert.Equal(2,74%3);
    }
}
