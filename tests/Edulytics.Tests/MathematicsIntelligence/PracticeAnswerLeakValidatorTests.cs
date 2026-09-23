using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class PracticeAnswerLeakValidatorTests
{
    [Fact]
    public void ExplicitPromptAnswerDisclosureIsRejected()
    {
        var result = PracticeAnswerLeakValidator.ValidatePrompt(
            "supporting.geometry.shape_dimension",
            "The correct answer is 3D.",
            "3D");

        Assert.False(result.IsSafe);
        Assert.Equal(
            "PROMPT_EXPLICITLY_REVEALS_ANSWER",
            result.ReasonCode);
    }

    [Fact]
    public void LegitimateShapeChoicePromptIsAllowed()
    {
        var result = PracticeAnswerLeakValidator.ValidatePrompt(
            "supporting.geometry.shape_dimension",
            "Is a cube a 2D or 3D shape?",
            "3D");

        Assert.True(result.IsSafe);
        Assert.Equal("NO_ANSWER_LEAK", result.ReasonCode);
    }

    [Fact]
    public void ShapeDimensionVisualClassificationLabelIsRejected()
    {
        var result = PracticeAnswerLeakValidator.ValidateVisual(
            "supporting.geometry.shape_dimension",
            "3D",
            "<svg><text>3D shape</text></svg>");

        Assert.False(result.IsSafe);
        Assert.Equal(
            "SHAPE_DIMENSION_VISUAL_REVEALS_CLASSIFICATION",
            result.ReasonCode);
    }

    [Fact]
    public void NeutralShapeVisualIsAllowed()
    {
        var result = PracticeAnswerLeakValidator.ValidateVisual(
            "supporting.geometry.shape_dimension",
            "3D",
            "<svg><ellipse cx='10' cy='10'/></svg>");

        Assert.True(result.IsSafe);
    }
}
