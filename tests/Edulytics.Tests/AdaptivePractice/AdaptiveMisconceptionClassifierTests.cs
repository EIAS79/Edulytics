using Edulytics.Core.Entities;
using Edulytics.Services.AdaptivePractice;

namespace Edulytics.Tests.AdaptivePractice;

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
