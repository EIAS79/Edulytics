using Edulytics.Core.Curriculum;
using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Stage A–C lesson ten evidence assertions; deliberately NOT executed.</summary>
public sealed class UsCcssG3Unit1Lesson10RichV2AuthoringTests
{
    private const string Code = "PED:US-CCSS-MATH:G3:U01:L10";

    [Fact]
    public void FirstEditionTaskPdfDeckAndTeacherOnlySubpartsAreMappedIndividually()
    {
        var rich = RichLessonContentV2Registry.Find(Code, "en");
        Assert.NotNull(rich);
        Assert.Equal("Drawings, Situations, and Diagrams, Oh My!", rich.Title);
        Assert.Contains(rich.ExplanationParagraphs, x =>
            x.Contains("12 cards A–L", StringComparison.Ordinal));
        Assert.Contains(rich.ExplanationParagraphs, x =>
            x.Contains("4 × 5", StringComparison.Ordinal));
        Assert.Contains(rich.ExplanationParagraphs, x =>
            x.Contains("Boxes of Shirts", StringComparison.Ordinal));
        Assert.Equal(6, rich.WorkedExamples.Count);
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code, out var lesson));
        Assert.Equal(8, lesson!.SourceSubtasks.Count);
        Assert.Contains("A–F–L", lesson.SourceSubtasks[3].Prompt);
        Assert.Contains("B–H–K", lesson.SourceSubtasks[3].Prompt);
        Assert.Contains("C–I", lesson.SourceSubtasks[3].Prompt);
        Assert.Contains("D–G", lesson.SourceSubtasks[3].Prompt);
        Assert.Contains("E–J", lesson.SourceSubtasks[3].Prompt);
        Assert.Equal(7, lesson.SourceSubtasks.Count(x=>x.AnswerType=="teacher"));
        Assert.Equal("yes-no", lesson.SourceSubtasks.Last().AnswerType);
    }

    [Fact]
    public void SourceGraphCountAndTwoDrawingContextsUseDistinctGroupSizes()
    {
        var rich = RichLessonContentV2Registry.Find(Code, "en");
        Assert.NotNull(rich);
        var body = string.Join(" ", rich.ExplanationParagraphs);
        Assert.Contains("Stop: TWO symbols = FOUR", body);
        Assert.Contains("Yield: ONE = TWO", body);
        Assert.Contains("Speed Limit: THREE = SIX", body);
        Assert.Contains("Street: FIVE = TEN", body);
        Assert.Contains("FOUR bags each holding TWO strawberries", body);
        Assert.Contains("FOUR hands each with FIVE fingers", body);
        Assert.Contains(rich.WorkedExamples, x =>
            x.Check.Contains("2+2+2=6", StringComparison.Ordinal));
    }

    [Fact]
    public void OriginalPublisherCoolDownIsRecoveredAndIndependentExtraRemainsOptional()
    {
        var rich = RichLessonContentV2Registry.Find(Code, "en");
        Assert.NotNull(rich);
        Assert.Contains(rich.CommonMistakes, x =>
            x.Mistake.Contains("Boxes of Shirts", StringComparison.Ordinal));
        Assert.Contains(rich.SummaryPoints, x =>
            x.Contains("teacher-reviewed", StringComparison.Ordinal));
        Assert.Empty(rich.Visuals);
        Assert.Empty(rich.Videos);
    }
}
