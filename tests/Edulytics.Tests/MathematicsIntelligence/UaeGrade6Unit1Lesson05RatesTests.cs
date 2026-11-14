using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;

// UAE G6 Arabic General printed pp31-38; authored A-B-C, manual .NET NOT_RUN.
public sealed class UaeGrade6Unit1Lesson05RatesTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L6:GENERAL:01:05:STUDENT-AR-128";
    [Fact] public void RatesRichArabicAndExistingContractAreSourceSpecific()
    {
        var rich=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(rich);
        Assert.Equal("المعدلات",rich!.Title);
        Assert.True(rich.WorkedExamples.Count>=7);
        Assert.Contains(rich.Visuals,x=>x.Kind==RichLessonVisualKind.RatioTable);
        Assert.Contains(rich.Visuals,x=>x.Kind==RichLessonVisualKind.CalculationChain);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.NotNull(contract);
        Assert.Equal(2,contract!.AllowedQuestionFamilies.Count);
    }
    [Theory]
    [InlineData(36,3,12)]
    [InlineData(45,5,9)]
    [InlineData(88,4,22)]
    [InlineData(63,7,9)]
    public void RatesExactUnitArithmetic(int total,int units,int answer)
    {
        var p=new Dictionary<string,int>{{"total",total},{"quantity",units}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("ratio.unit_rate.direct",p,answer.ToString()));
        Assert.False(ExactSkillContractQuestionEngine.Verify("ratio.unit_rate.direct",p,(answer+1).ToString()));
    }
    [Fact] public void GradeSixRatePromptOnlyIsArabicAndVerifierRemainsShared()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in new[]{"ratio.unit_rate.direct","ratio.unit_rate.equivalent_ratio"})
        {
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out _));
            foreach(var q in engine.Generate(Lesson,Lesson,[family],ExactSkillQuestionDifficulty.Standard,3,20261114,[]))
            {
                Assert.Contains("وحدة",q.Prompt);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
            }
        }
    }
}
