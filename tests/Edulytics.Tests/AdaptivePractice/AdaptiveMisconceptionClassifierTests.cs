using Edulytics.Core.Entities;
using Edulytics.Services.AdaptivePractice;

namespace Edulytics.Tests.MathematicsIntelligence.AdaptivePractice;

public sealed class AdaptiveMisconceptionClassifierTests
{
    private readonly AdaptiveMisconceptionClassifier classifier = new();

    [Theory]
    [InlineData("3/4", "4/3", "fraction.reciprocal")]
    [InlineData("5", "-5", "sign.reversal")]
    [InlineData("7", "8", "arithmetic.off_by_one")]
    [InlineData("<", ">", "comparison.reversal")]
    public void ClassifiesOnlyReviewedDeterministicPatterns(
        string correct,
        string submitted,
        string expectedId)
    {
        var result = classifier.Classify(
            new AssessmentItem
            {
                CorrectAnswer = correct
            },
            submitted);

        Assert.NotNull(result);
        Assert.Equal(expectedId, result!.MisconceptionId);
    }

    [Theory]
    [InlineData("8822", "100", "8800", "8900", "rounding.wrong_direction_up")]
    [InlineData("8872", "100", "8900", "8800", "rounding.wrong_direction_down")]
    [InlineData("8822", "100", "8800", "8802", "rounding.lower_places_not_zeroed")]
    [InlineData("8822", "100", "8800", "8700", "rounding.adjacent_multiple")]
    public void ClassifiesReviewedWholeNumberRoundingErrors(
        string value,
        string place,
        string correct,
        string submitted,
        string expectedId)
    {
        var item = new AssessmentItem
        {
            CorrectAnswer = correct,
            GenerationFamily = "supporting.number.rounding",
            GenerationParametersJson =
                "{\"parameters\":{\"value\":" +
                value +
                ",\"place\":" +
                place +
                "}}"
        };

        var result = classifier.Classify(
            item,
            submitted);

        Assert.NotNull(result);
        Assert.Equal(
            expectedId,
            result!.MisconceptionId);
        Assert.True(result.Confidence >= 0.70m);
    }

    [Fact]
    public void ArbitraryWrongAnswerRemainsUnclassified()
    {
        var result = classifier.Classify(
            new AssessmentItem
            {
                CorrectAnswer = "12"
            },
            "99");

        Assert.Null(result);
    }

    [Fact]
    public void CorrectAnswerNeverProducesMisconception()
    {
        var result = classifier.Classify(
            new AssessmentItem
            {
                CorrectAnswer = "3/4"
            },
            "3/4");

        Assert.Null(result);
    }
}
