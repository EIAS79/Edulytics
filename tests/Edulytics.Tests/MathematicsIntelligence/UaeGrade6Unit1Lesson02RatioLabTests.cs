using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;

// UAE Grade 6 General Student Book printed 15-18 / PDF 56-59.
// Authored but not manually executed in A-B-C; Stage D runs the suite.
public sealed class UaeGrade6Unit1Lesson02RatioLabTests
{
    private const string Lesson = "PED:UAE-MOE-MATH:L6:GENERAL:01:02:STUDENT-AR-128";
    [Fact]
    public void ArabicLessonHasOriginalGroupModelsAndExactRatioPractice()
    {
        var l = RichLessonContentV2Registry.Find(Lesson, "ar");
        Assert.NotNull(l);
        Assert.Equal("مختبر الاستكشاف: النسب", l!.Title);
        Assert.True(l.WorkedExamples.Count >= 4);
        Assert.Contains(l.Visuals, v => v.Kind == RichLessonVisualKind.RatioTable);
        Assert.Contains(l.Visuals, v => v.Kind == RichLessonVisualKind.CalculationChain);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson, out var contract));
        Assert.NotNull(contract);
        Assert.Equal(2, contract!.AllowedQuestionFamilies.Count);
        foreach (var family in contract.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
    }
    [Theory]
    [InlineData("ratio.unit_rate.equivalent_ratio", 3, 4, 12, 9)]
    [InlineData("ratio.unit_rate.equivalent_ratio", 5, 3, 12, 20)]
    public void RatioTableIndependentVerifier(string family, int first, int second,
        int targetSecond, int answer)
    {
        var p = new Dictionary<string,int> { ["baseFirst"]=first,["baseSecond"]=second,["targetSecond"]=targetSecond };
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,answer.ToString()));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,(answer+1).ToString()));
    }
    [Fact]
    public void OriginalTwentySevenPiecesRatioSplitIsExact()
    {
        const string family = "ratio.unit_rate.divide_total";
        var p = new Dictionary<string,int>{["firstPart"]=4,["secondPart"]=5,["total"]=27,["askedPart"]=4};
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,"12"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"15"));
    }
    [Fact]
    public void ReusedEngineIssuesArabicPromptsOnlyForThisG6Lesson()
    {
        var engine = new ExactSkillContractQuestionEngine();
        foreach (string family in new[]{"ratio.unit_rate.equivalent_ratio","ratio.unit_rate.divide_total"})
        {
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family, out _));
            foreach (var q in engine.Generate(Lesson, family, [family],
                ExactSkillQuestionDifficulty.Standard, 3, 20261114, []))
            {
                Assert.Contains("نسب", q.Prompt);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family, q.Parameters, q.CorrectAnswer));
            }
        }
    }
}
