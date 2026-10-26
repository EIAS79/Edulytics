using Edulytics.Core.Curriculum;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Authored source-alignment assertions only; intentionally NOT run before Stage D.</summary>
public sealed class UsCcssG3Unit1Lesson3RichV2SourceCoverageTests
{
    private const string Code = "PED:US-CCSS-MATH:G3:U01:L03";

    [Fact]
    public void SportsScaledAndUnscaledGraphsHaveExactlyTheSameSourceCounts()
    {
        var rich = RichLessonContentV2Registry.Find(Code, "en");
        Assert.NotNull(rich);
        var text = string.Join(" ", rich.ExplanationParagraphs);
        Assert.Contains("Football 25, Soccer 15, Basketball 20, Tennis 5", text);
        Assert.Contains("Football 5, Soccer 3, Basketball 4, Tennis 1", text);
        Assert.Contains("65", text);
        Assert.Contains("13 symbols instead of 65", text);
        Assert.Contains(rich.WorkedExamples, x =>
            x.Check.Contains("13 × 5 = 65", StringComparison.Ordinal));
    }

    [Fact]
    public void TwoStudentAndFiveFlowerKeysAndExplanationTaskArePreserved()
    {
        var rich = RichLessonContentV2Registry.Find(Code, "en");
        Assert.NotNull(rich);
        var paragraph = rich.ExplanationParagraphs.Single(x =>
            x.Contains("Andre's flowers", StringComparison.Ordinal));
        Assert.Contains("Roses 10, Tulips 5, Daisies 25, Violets 15", paragraph);
        Assert.Contains("8, 2, 8 and 6 students", paragraph);
        Assert.Contains("HOW you know", paragraph);
        Assert.Contains("TWO answerable questions", paragraph);
        Assert.Contains(rich.WorkedExamples, x =>
            x.Check.Contains("(4 + 1 + 4 + 3) × 2 = 24", StringComparison.Ordinal));
    }

    [Fact]
    public void RichV2DoesNotSwapSynthesisAndCoolDownOrClaimSourceArtwork()
    {
        var rich = RichLessonContentV2Registry.Find(Code, "en");
        Assert.NotNull(rich);
        var sequence = rich.ExplanationParagraphs.Last();
        Assert.Contains("BEFORE the Cool-down", sequence);
        Assert.Contains("AFTER the Cool-down", sequence);
        Assert.Contains("NOT the publisher", sequence, StringComparison.OrdinalIgnoreCase);
        Assert.Empty(rich.Visuals);
        Assert.Empty(rich.Videos);
        Assert.Contains(rich.SummaryPoints, point =>
            point.Contains("figure pixels", StringComparison.Ordinal));
    }
}
