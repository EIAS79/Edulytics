using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// AUTHORING-ONLY / AUTHORED_NOT_RUN.
/// Shared LessonPracticeContractRegistry and ExactSkillContractQuestionEngine
/// remain UNTOUCHED pending a new exact G5 U03:L02 Admin lease.
/// </summary>
public sealed class UsCcssG5U03L02UnitFractionLocalPrepTests
{
    private static Dictionary<string, int> P(
        int scenario, int slot = 0, int comparison = 0,
        int whole = 0, int layout = 0) =>
        new(StringComparer.Ordinal)
        {
            ["case"] = scenario, ["layout"] = layout, ["slot"] = slot,
            ["comparison"] = comparison, ["differentWhole"] = whole
        };

    [Theory]
    [InlineData(3, 4, 12)]
    [InlineData(4, 3, 12)]
    [InlineData(2, 5, 10)]
    [InlineData(5, 2, 10)]
    public void DigitalModel_UsesTheOriginalWhole(
        int first, int second, int expectedDenominator)
    {
        Assert.True(UsCcssG5U03L02UnitFractionArea.TryCreate(
            60, 60, first, second,
            UsCcssG5U03L02UnitFractionArea.Direction.Horizontal,
            UsCcssG5U03L02UnitFractionArea.Direction.Vertical,
            out var grid));
        Assert.NotNull(grid);
        Assert.Equal(expectedDenominator, grid!.DenominatorOfOriginalWhole);
        Assert.Equal(expectedDenominator, grid.Cells.Count);
        Assert.True(UsCcssG5U03L02UnitFractionArea.IsValidDigitalGrid(grid, grid.Cells));
        Assert.True(grid.TeacherReviewRequired);
    }

    [Theory]
    [InlineData(0, 0, "1")]
    [InlineData(0, 1, "2")]
    [InlineData(0, 2, "3")]
    [InlineData(1, 2, "3")]
    [InlineData(2, 0, "1")]
    [InlineData(3, 1, "2")]
    public void ProductSelection_RejectsWrongDenominatorsAndMalformedSelections(
        int scenario, int slot, string expected)
    {
        var p = P(scenario, slot);
        Assert.True(UsCcssG5U03L02UnitFractionQuestions.Verify(
            UsCcssG5U03L02UnitFractionQuestions.ProductFamily, p, expected));
        foreach (var invalid in new[] { "0", "4", "1/3", "1/4", "1/5", "1/2", " 1", "" })
            Assert.False(UsCcssG5U03L02UnitFractionQuestions.Verify(
                UsCcssG5U03L02UnitFractionQuestions.ProductFamily, p, invalid));
    }

    [Theory]
    [InlineData(0, 1, 0, "1")]
    [InlineData(2, 3, 0, "1")]
    [InlineData(0, 2, 0, "0")]
    [InlineData(2, 1, 0, "0")]
    [InlineData(0, 1, 1, "0")]
    [InlineData(2, 3, 1, "0")]
    public void Comparison_RequiresEqualOriginalWholeAndProduct(
        int first, int second, int differentWhole, string expected)
    {
        var p = P(first, comparison: second, whole: differentWhole, layout: 3);
        Assert.True(UsCcssG5U03L02UnitFractionQuestions.Verify(
            UsCcssG5U03L02UnitFractionQuestions.CompareFamily, p, expected));
        Assert.False(UsCcssG5U03L02UnitFractionQuestions.Verify(
            UsCcssG5U03L02UnitFractionQuestions.CompareFamily, p,
            expected == "1" ? "0" : "1"));
    }

    [Fact]
    public void InvalidDomain_DoesNotEnterDigitalGrader()
    {
        foreach (var family in new[]
        {
            UsCcssG5U03L02UnitFractionQuestions.ProductFamily,
            UsCcssG5U03L02UnitFractionQuestions.CompareFamily
        })
        {
            foreach (var malformed in new[] {
                P(-1), P(4), P(0, slot: 3), P(0, comparison: -1),
                P(0, whole: 2), P(0, layout: 4) })
                Assert.False(UsCcssG5U03L02UnitFractionQuestions.Verify(
                    family, malformed, "1"));
            var injected = P(0);
            injected["foreign"] = 1;
            Assert.False(UsCcssG5U03L02UnitFractionQuestions.Verify(
                family, injected, "1"));
        }
        Assert.False(UsCcssG5U03L02UnitFractionQuestions.Supports(
            "usccss.g5.u03.l03.unapproved"));
    }

    [Fact]
    public void ShadingCannotOverlapOrMoveOutsideSelectedOriginalPartition()
    {
        Assert.True(UsCcssG5U03L02UnitFractionArea.TryCreate(
            60, 60, 3, 4,
            UsCcssG5U03L02UnitFractionArea.Direction.Horizontal,
            UsCcssG5U03L02UnitFractionArea.Direction.Vertical,
            out var grid));
        Assert.NotNull(grid);
        var overlap = grid!.Cells.ToArray();
        overlap[1] = overlap[0];
        Assert.False(UsCcssG5U03L02UnitFractionArea.IsValidDigitalGrid(grid, overlap));
        var wrongShade = grid.Cells.Select(c => c with { Shaded = false }).ToArray();
        wrongShade[^1] = wrongShade[^1] with { Shaded = true };
        Assert.False(UsCcssG5U03L02UnitFractionArea.IsValidDigitalGrid(grid, wrongShade));
        Assert.False(UsCcssG5U03L02UnitFractionArea.TryCreate(
            61, 60, 3, 4,
            UsCcssG5U03L02UnitFractionArea.Direction.Horizontal,
            UsCcssG5U03L02UnitFractionArea.Direction.Vertical, out _));
    }
}
