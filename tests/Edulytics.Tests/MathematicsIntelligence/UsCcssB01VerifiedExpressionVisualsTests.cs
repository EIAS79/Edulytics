using Edulytics.Core.Curriculum;
using Edulytics.Core.Lessons;
using Edulytics.Services.LessonContent;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssB01VerifiedExpressionVisualsTests
{
    private static CanonicalLessonTranslationRecord Body(string examples) =>
        new("en", "Math task", "Count and represent.", "Addition and subtraction.",
            examples, "Check every step.", "Do not guess.", "Count accurately.");

    [Theory]
    [InlineData("3 + 2 = 5", 3, 2, 5)]
    [InlineData("5 + 3 = 8", 5, 3, 8)]
    [InlineData("9 + 8 = 17", 9, 8, 17)]
    public void SourceEqualityProducesCorrectDotGroups(
        string equality, int first, int second, int total)
    {
        var visual = Assert.Single(UsCcssB01VerifiedExpressionVisuals.Build(
            "PED:US-CCSS-MATH:G1:U01:L02", Body("Example: " + equality)));
        Assert.Equal(RichLessonVisualKind.PlaceValueChart, visual.Kind);
        Assert.Equal(2, visual.Rows.Count);
        Assert.Equal(first, visual.Rows[0].Cells.Count(x => x == "●"));
        Assert.Equal(second, visual.Rows[1].Cells.Count(x => x == "●"));
        Assert.Contains(equality, visual.Description);
        Assert.Equal(total, visual.Rows.Sum(r => r.Cells.Count(c => c == "●")));
    }

    [Fact]
    public void SourceDerivedSubtractionShowsOriginalRemovedAndRemaining()
    {
        var visual = Assert.Single(UsCcssB01VerifiedExpressionVisuals.Build(
            "PED:US-CCSS-MATH:G2:U03:L04", Body("Example: 9 − 4 = 5")));
        Assert.Equal(RichLessonVisualKind.PlaceValueChart, visual.Kind);
        Assert.Equal(9, visual.Rows[0].Cells.Count(x => x == "●"));
        Assert.Equal(4, visual.Rows[1].Cells.Count(x => x == "○"));
        Assert.Equal(5, visual.Rows[2].Cells.Count(x => x == "●"));
    }

    [Theory]
    [InlineData("2 + 3 = 9")]
    [InlineData("3 + 2 = 2 + 3")]
    [InlineData("30 + 40 = 70")]
    [InlineData("9 − 14 = -5")]
    [InlineData("1 + 1 + 1 = 3")]
    public void UnsupportedOrIncorrectStatementsNeverCreateCounterModels(
        string sourceStatement)
    {
        var visuals = UsCcssB01VerifiedExpressionVisuals.Build(
            "PED:US-CCSS-MATH:G1:U01:L02", Body(sourceStatement));
        Assert.Empty(visuals);
    }

    [Fact]
    public void ExistingCertifiedLessonAndOtherProgrammesRemainUnchanged()
    {
        var example = Body("3 + 2 = 5");
        Assert.Empty(UsCcssB01VerifiedExpressionVisuals.Build(
            "PED:US-CCSS-MATH:G1:U01:L01", example));
        Assert.Empty(UsCcssB01VerifiedExpressionVisuals.Build(
            "PED:CAMBRIDGE-INTL-MATH:S1:U01:L02", example));
        Assert.Empty(UsCcssB01VerifiedExpressionVisuals.Build(
            "PED:US-CCSS-MATH:G8:U01:L02", example));
    }
}
