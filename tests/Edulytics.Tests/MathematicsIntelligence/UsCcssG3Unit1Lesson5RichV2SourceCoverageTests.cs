using Edulytics.Core.Curriculum;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Source-semantic Stage A–B–C authoring contract only: NOT run before Stage D.</summary>
public sealed class UsCcssG3Unit1Lesson5RichV2SourceCoverageTests
{
    private const string Code = "PED:US-CCSS-MATH:G3:U01:L05";

    [Fact]
    public void FirstEditionWarmupCountsAndComparisonDatasetArePresent()
    {
        var rich = RichLessonContentV2Registry.Find(Code, "en");
        Assert.NotNull(rich);
        var text = string.Join(" ", rich.ExplanationParagraphs);
        Assert.Contains("8; 2+2+2+2+2+2+2+2 = 16", text);
        Assert.Contains("5+5+5+5 = 20", text);
        Assert.Contains("5+5+5+5+5+5+5+5 = 40", text);
        Assert.Contains("Bike 12, Walk 4, Bus or Van 6, Car 3", text);
        Assert.Contains("UNCHANGED", rich.KeyConcepts[0].Definition.ToUpperInvariant());
    }

    [Fact]
    public void SourceZooCooldownAndLessonChronologyAreNotReplacedByClubNumbers()
    {
        var rich = RichLessonContentV2Registry.Find(Code, "en");
        Assert.NotNull(rich);
        var last = rich.ExplanationParagraphs.Last();
        Assert.Contains("Elephant 17, Tiger 10, Giraffe 14 and Otter 4", last);
        Assert.Contains("BEFORE the five-minute Cool-down", last);
        Assert.Contains("AFTER the Cool-down", last);
        Assert.Contains(rich.WorkedExamples, x => x.Answer.Contains("Tiger bar = 10", StringComparison.Ordinal));
        Assert.Contains(rich.WorkedExamples, x => x.Check.Contains("Tiger 10 and Otter 4", StringComparison.Ordinal));
        Assert.Empty(rich.Visuals);
        Assert.Contains(rich.CommonMistakes, x => x.Correction.Contains("0,5,10,15,20", StringComparison.Ordinal));
    }

    [Fact]
    public void ActualClassSurveyMustStayUninventedAndTeacherReviewed()
    {
        var rich = RichLessonContentV2Registry.Find(Code, "en");
        Assert.NotNull(rich);
        Assert.Contains(rich.ExplanationParagraphs, p => p.Contains("Reuse the actual class-preference survey", StringComparison.Ordinal));
        Assert.Contains(rich.CommonMistakes, x => x.Correction.Contains("independently approved class totals", StringComparison.Ordinal));
        Assert.Equal(4, rich.WorkedExamples.Count);
        Assert.All(rich.WorkedExamples, x => Assert.True(x.Steps.Count >= 3));
    }
}
