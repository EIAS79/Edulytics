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
                $"Visual Practice family has no deterministic SVG renderer: {family}");
            Assert.StartsWith("<svg", svg!, StringComparison.Ordinal);
            Assert.Contains("</svg>", svg!, StringComparison.Ordinal);
        }
    }
}
