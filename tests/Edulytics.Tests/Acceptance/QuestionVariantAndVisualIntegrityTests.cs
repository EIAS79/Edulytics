using System.Text.Json;
using Edulytics.Core.Mathematics.Generation;
using Edulytics.Services.Mathematics;
using Edulytics.Web.Presentation;

namespace Edulytics.Tests.Acceptance;

public sealed class QuestionVariantAndVisualIntegrityTests
{
    [Fact]
    public void VariantPolicyCapsFamiliesAtSixteenSlots()
    {
        Assert.Equal(16, QuestionVariantPolicy.MaximumVariantsPerFamily);
        Assert.Equal("v01", QuestionVariantPolicy.IdForSlot(0));
        Assert.Equal("v16", QuestionVariantPolicy.IdForSlot(15));
        Assert.Equal("v01", QuestionVariantPolicy.IdForSlot(16));
    }

    [Fact]
    public void MultiStepGenerationRotatesRealPromptStructures()
    {
        var items = new ExactSkillContractQuestionEngine().Generate(
            "variant-test",
            "reasoning",
            ["supporting.reasoning.multistep"],
            ExactSkillQuestionDifficulty.Standard,
            5,
            7341,
            []);

        Assert.Equal(5, items.Count);
        Assert.Equal(5, items.Select(x => x.VariantId).Distinct().Count());
        Assert.Equal(5, items.Select(x => x.Prompt).Distinct().Count());
        Assert.All(items, x => Assert.True(
            ExactSkillContractQuestionEngine.Verify(
                x.Family,
                x.Parameters,
                x.CorrectAnswer)));
    }

    [Fact]
    public void ShapeDimensionGenerationUsesSemanticShapesWithoutDuplicatePrompt()
    {
        var items = new ExactSkillContractQuestionEngine().Generate(
            "variant-test",
            "shape-dimension",
            ["supporting.geometry.shape_dimension"],
            ExactSkillQuestionDifficulty.Standard,
            5,
            9812,
            []);

        Assert.Equal(5, items.Count);
        Assert.Equal(5, items.Select(x => x.Prompt).Distinct().Count());
        Assert.All(items, x =>
        {
            Assert.True(x.Parameters.ContainsKey("shape"));
            Assert.True(x.Parameters.ContainsKey("dimension"));
            Assert.True(x.Parameters.ContainsKey("variant"));
        });
    }

    [Theory]
    [InlineData(0, 2)]
    [InlineData(4, 3)]
    [InlineData(7, 3)]
    public void ShapeDimensionVisualUsesPersistedSemanticShape(
        int shape,
        int dimension)
    {
        var json = JsonSerializer.Serialize(new
        {
            parameters = new
            {
                shape,
                dimension,
                variant = shape
            }
        });

        var svg = PracticeMathVisualRenderer.RenderSvg(
            "supporting.geometry.shape_dimension",
            json);

        Assert.False(string.IsNullOrWhiteSpace(svg));
        Assert.Contains(
            dimension == 2 ? "2D shape" : "3D shape",
            svg,
            StringComparison.Ordinal);
        Assert.DoesNotContain("base =", svg, StringComparison.Ordinal);
        Assert.DoesNotContain("h =", svg, StringComparison.Ordinal);
    }

    [Fact]
    public void LegacyShapeDimensionWithoutSemanticShapeSuppressesVisual()
    {
        var json = JsonSerializer.Serialize(new
        {
            parameters = new
            {
                dimension = 2,
                variant = 3913
            }
        });

        Assert.Null(PracticeMathVisualRenderer.RenderSvg(
            "supporting.geometry.shape_dimension",
            json));
    }
}
