using System.Text.Json;
using Edulytics.Core.Mathematics.Skills;
using Edulytics.Services.Mathematics;
using Edulytics.Web.Presentation;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class PracticeVisualFamilyCoverageTests
{
    private static readonly HashSet<string> VisualRepresentations =
        new(StringComparer.Ordinal)
        {
            "diagram_metadata",
            "graph",
            "vector",
            "right_triangle",
            "angle",
            "coordinate_pair",
            "solid_dimensions"
        };

    [Fact]
    public void ShapeDimensionVisualDoesNotRevealTheCorrectDimension()
    {
        var json = JsonSerializer.Serialize(new
        {
            parameters = new
            {
                shape = 7,
                dimension = 3,
                variant = 7
            }
        });

        var svg = PracticeMathVisualRenderer.RenderSvg(
            "supporting.geometry.shape_dimension",
            json);

        Assert.False(string.IsNullOrWhiteSpace(svg));
        Assert.DoesNotContain("3D shape", svg!, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("2D shape", svg!, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("<ellipse", svg!, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public void GradeFourLShapedVisual_RendersAllFiveSourceCasesWithoutDisclosingResult(int mode)
    {
        const string family = "usccss.g4.u06.l22.convert_l_shaped_area_perimeter";
        var json = JsonSerializer.Serialize(new { parameters = new
        {
            caseType = mode,
            sheetWidthInches = 49,
            sheetHeightInches = 29,
            bannerHeightInches = 8,
            bannerLengthFeet = 8,
            leftWidthFeet = 11,
            rightWidthYards = 2,
            lowerHeightYards = 5,
            upperRiseFeet = 6,
            suppliedInches = 1036
        }});
        var svg = PracticeMathVisualRenderer.RenderSvg(family, json);
        Assert.NotNull(svg);
        Assert.StartsWith("<svg", svg!, StringComparison.Ordinal);
        Assert.Contains("</svg>", svg!, StringComparison.Ordinal);
        Assert.Contains(mode == 0 ? "<rect" : "<polygon", svg!, StringComparison.Ordinal);
        Assert.DoesNotContain("area = ", svg!, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("perimeter = ", svg!, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GradeFourLShapedVisual_RejectsMalformedOrImpossibleMetadata()
    {
        const string family = "usccss.g4.u06.l22.convert_l_shaped_area_perimeter";
        var invalid = JsonSerializer.Serialize(new { parameters = new
        {
            caseType = 1,
            sheetWidthInches = 49,
            sheetHeightInches = 29,
            bannerHeightInches = 8,
            bannerLengthFeet = 8,
            leftWidthFeet = 11,
            rightWidthYards = 2,
            lowerHeightYards = 5,
            upperRiseFeet = 6,
            suppliedInches = 1
        }});
        Assert.Null(PracticeMathVisualRenderer.RenderSvg(family, invalid));
    }

    [Fact]
    public void EveryLessonPracticeVisualFamily_GeneratesAndRendersDeterministically()
    {
        var assembly = typeof(SkillContract).Assembly;
        using var stream = assembly.GetManifestResourceStream(
            "Edulytics.Core.Mathematics.Generation.question-family-registry.v1.json");
        Assert.NotNull(stream);

        using var document = JsonDocument.Parse(stream!);
        var families = document.RootElement
            .GetProperty("families")
            .EnumerateArray()
            .Where(row =>
                row.TryGetProperty("lessonPracticeRouting", out var routing) &&
                routing.ValueKind == JsonValueKind.True &&
                row.GetProperty("representations")
                    .EnumerateArray()
                    .Select(value => value.GetString())
                    .Any(value => value is not null && VisualRepresentations.Contains(value)))
            .Select(row => row.GetProperty("id").GetString()!)
            .OrderBy(id => id, StringComparer.Ordinal)
            .ToArray();

        Assert.NotEmpty(families);

        var engine = new ExactSkillContractQuestionEngine();
        foreach (var family in families)
        {
            Assert.True(
                ExactSkillContractQuestionEngine.SupportsFamily(family),
                $"Visual Practice family is not supported by the exact engine: {family}");

            var first = engine.Generate(
                "visual-coverage-test",
                family,
                [family],
                ExactSkillQuestionDifficulty.Standard,
                1,
                20260920,
                []).Single();

            var second = engine.Generate(
                "visual-coverage-test",
                family,
                [family],
                ExactSkillQuestionDifficulty.Standard,
                1,
                20260920,
                []).Single();

            Assert.Equal(first.Prompt, second.Prompt);
            Assert.Equal(first.CorrectAnswer, second.CorrectAnswer);
            Assert.Equal(
                JsonSerializer.Serialize(first.Parameters),
                JsonSerializer.Serialize(second.Parameters));

            var json = JsonSerializer.Serialize(new { parameters = first.Parameters });
            var svg = PracticeMathVisualRenderer.RenderSvg(family, json);

            Assert.False(
                string.IsNullOrWhiteSpace(svg),
                $"Visual Practice family has no deterministic SVG renderer: {family}; exact generated parameters: {json}");
            Assert.StartsWith("<svg", svg!, StringComparison.Ordinal);
            Assert.Contains("</svg>", svg!, StringComparison.Ordinal);
        }
    }
}
