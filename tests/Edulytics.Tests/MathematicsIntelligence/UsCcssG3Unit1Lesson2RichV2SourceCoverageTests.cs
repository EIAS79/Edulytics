using Edulytics.Core.Curriculum;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Authored Stage A–C source contract; do not execute until Stage D.</summary>
public sealed class UsCcssG3Unit1Lesson2RichV2SourceCoverageTests
{
    private const string Code = "PED:US-CCSS-MATH:G3:U01:L02";

    [Fact]
    public void FirstEditionClassSurveyIsNotFalselyHardCoded()
    {
        var rich = RichLessonContentV2Registry.Find(Code, "en");
        Assert.NotNull(rich);
        var body = string.Join(" ", rich.ExplanationParagraphs);
        Assert.Contains("REAL data collected from the current class", body);
        Assert.Contains("teacher-led sticky-note class poll", body);
        Assert.Contains("fictional data", body);
        Assert.Contains("within 20", body);
        Assert.Contains("AFTER the Cool-down", body);
        Assert.Contains("2–3 minutes", body);
        Assert.DoesNotContain("Walk 4, Bike 3, Skateboard 1", body);
    }

    [Fact]
    public void SourceWarmupTruthsAndOneTwoStepSkillFamiliesAreExplicit()
    {
        var rich = RichLessonContentV2Registry.Find(Code, "en");
        Assert.NotNull(rich);
        Assert.Contains(rich.WorkedExamples, x =>
            x.Check.Contains("10 − 8 = 2", StringComparison.Ordinal));
        Assert.Contains(rich.WorkedExamples, x =>
            x.Check.Contains("5 − 3 = 2", StringComparison.Ordinal));
        Assert.Contains(rich.WorkedExamples, x =>
            x.Check.Contains("(5 + 3) − 2 = 6", StringComparison.Ordinal));
        Assert.Contains(rich.KeyConcepts, x =>
            x.Title == "Two matching representations");
        Assert.Contains(rich.CommonMistakes, x =>
            x.Mistake.Contains("previous lesson's six fixed", StringComparison.Ordinal));
        Assert.All(rich.WorkedExamples, x =>
            Assert.True(x.Steps.Count >= 3));
    }

    [Fact]
    public void PublisherSourceHumanAndRightsGatesRemainExplicit()
    {
        var rich = RichLessonContentV2Registry.Find(Code, "en");
        Assert.NotNull(rich);
        Assert.Contains(rich.SummaryPoints, x =>
            x.Contains("Teacher evidence is required", StringComparison.Ordinal));
        Assert.Contains(rich.SummaryPoints, x =>
            x.Contains("image pixels remain unverified", StringComparison.Ordinal));
        Assert.Empty(rich.Videos);
        Assert.Empty(rich.Visuals);
    }
}
