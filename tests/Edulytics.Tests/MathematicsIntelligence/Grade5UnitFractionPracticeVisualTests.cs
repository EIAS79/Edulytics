using System.Text.Json;
using System.Text.RegularExpressions;
using Edulytics.Web.Presentation;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class Grade5UnitFractionPracticeVisualTests
{
    private const string Product =
        "usccss.g5.u03.l02.unit_fraction_of_unit_fraction";
    private const string Comparison =
        "usccss.g5.u03.l02.same_original_whole_order_comparison";

    private static string Parameters(int scenario, int layout = 0, int slot = 0,
        int comparison = 0, int differentWhole = 0) =>
        JsonSerializer.Serialize(new
        {
            parameters = new Dictionary<string, int>
            {
                ["case"] = scenario,
                ["layout"] = layout,
                ["slot"] = slot,
                ["comparison"] = comparison,
                ["differentWhole"] = differentWhole
            }
        });

    [Theory]
    [InlineData(0, 12)]
    [InlineData(1, 12)]
    [InlineData(2, 10)]
    [InlineData(3, 10)]
    public void ProductShowsExactCanonicalEqualAreaCellsWithoutAnswerText(
        int scenario, int expectedCells)
    {
        for (var layout = 0; layout < 4; layout++)
        for (var slot = 0; slot < 3; slot++)
        {
            var svg = PracticeMathVisualRenderer.RenderSvg(
                Product, Parameters(scenario, layout, slot));
            Assert.NotNull(svg);
            Assert.Contains("role='img'", svg);
            Assert.Contains("aria-label=", svg);
            Assert.Equal(expectedCells,
                Regex.Matches(svg, "class='fraction-cell ").Count);
            Assert.Single(
                Regex.Matches(svg, "class='fraction-cell fraction-shaded'")
                    .Cast<Match>());
            Assert.DoesNotContain("Select 1:", svg);
            Assert.DoesNotContain("1/12", svg);
            Assert.DoesNotContain("1/10", svg);
            Assert.DoesNotContain("http://", svg);
            Assert.DoesNotContain("https://", svg);
        }
    }

    [Theory]
    [InlineData(0, 1, 24)]
    [InlineData(2, 3, 20)]
    [InlineData(0, 2, 22)]
    public void CompareShowsTwoCanonicalWholesWithoutTextRevealingTheAnswer(
        int first, int second, int expectedCells)
    {
        for (var differentWhole = 0; differentWhole <= 1; differentWhole++)
        for (var layout = 0; layout < 4; layout++)
        {
            var svg = PracticeMathVisualRenderer.RenderSvg(
                Comparison, Parameters(first, layout, 2, second, differentWhole));
            Assert.NotNull(svg);
            Assert.Equal(expectedCells,
                Regex.Matches(svg, "class='fraction-cell ").Count);
            Assert.Equal(2,
                Regex.Matches(svg, "class='fraction-cell fraction-shaded'").Count);
            Assert.Contains("Diagram A", svg);
            Assert.Contains("Diagram B", svg);
            Assert.DoesNotContain("SAME whole", svg);
            Assert.DoesNotContain("DIFFERENT whole", svg);
            Assert.DoesNotContain("Answer 1=", svg);
            Assert.DoesNotContain("1/12", svg);
            Assert.DoesNotContain("1/10", svg);
        }
    }

    [Fact]
    public void InvalidParametersOrCrossLessonFamilyNeverRenderAnInventedFigure()
    {
        var invalid = new[]
        {
            Parameters(-1), Parameters(4), Parameters(0, 4),
            Parameters(0, 0, 3), Parameters(0, 0, 0, 4),
            Parameters(0, 0, 0, 0, 2),
            "{\"parameters\":{\"case\":0,\"layout\":0}}",
            "{\"parameters\":{\"case\":0,\"layout\":0,\"slot\":0," +
                "\"comparison\":0,\"differentWhole\":0,\"injected\":1}}"
        };
        foreach (var parameters in invalid)
        {
            Assert.Null(PracticeMathVisualRenderer.RenderSvg(Product, parameters));
            Assert.Null(PracticeMathVisualRenderer.RenderSvg(Comparison, parameters));
        }
        Assert.Null(PracticeMathVisualRenderer.RenderSvg(
            "usccss.g5.u03.l03.unit_fraction_of_unit_fraction", Parameters(0)));
        Assert.Null(PracticeMathVisualRenderer.RenderSvg(Product, "{malformed"));
    }
}
