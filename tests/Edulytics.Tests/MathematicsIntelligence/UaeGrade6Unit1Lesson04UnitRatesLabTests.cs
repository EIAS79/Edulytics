using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;

// Source PDF 68-71 printed 27-30. Tests written, not executed manually in A-B-C.
public sealed class UaeGrade6Unit1Lesson04UnitRatesLabTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L6:GENERAL:01:04:STUDENT-AR-128";
    [Fact] public void OriginalArabicUnitRateLabIsExactAndSourceScoped()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("مختبر الاستكشاف: معدلات الوحدة",l!.Title);
        Assert.True(l.WorkedExamples.Count>=5);
        Assert.Contains(l.Visuals,v=>v.Kind==RichLessonVisualKind.RatioTable);
        Assert.Contains(l.Visuals,v=>v.Kind==RichLessonVisualKind.CalculationChain);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.NotNull(contract);
        Assert.Single(contract!.AllowedQuestionFamilies);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily("ratio.unit_rate.direct"));
    }
    [Theory]
    [InlineData(14,2,7)]
    [InlineData(205,5,41)]
    [InlineData(249,3,83)]
    [InlineData(231,7,33)]
    [InlineData(72,4,18)]
    public void ExactUnitRateVerification(int total,int units,int answer)
    {
        var p=new Dictionary<string,int>{{"total",total},{"quantity",units}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("ratio.unit_rate.direct",p,answer.ToString()));
        Assert.False(ExactSkillContractQuestionEngine.Verify("ratio.unit_rate.direct",p,(answer+1).ToString()));
    }
    [Fact] public void NewGradeSixUnitRateUsesOriginalArabicPrompt()
    {
        var engine=new ExactSkillContractQuestionEngine();
        var family="ratio.unit_rate.direct";
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out _));
        foreach(var q in engine.Generate(Lesson,Lesson,[family],ExactSkillQuestionDifficulty.Standard,3,20261114,[]))
        {
            Assert.Contains("لكل وحدة",q.Prompt);
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
        }
    }
}
