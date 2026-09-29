using Edulytics.Core.Entities;
using Edulytics.Services.AdaptivePractice;

namespace Edulytics.Tests.MathematicsIntelligence.AdaptivePractice;

public sealed class AdaptiveRemediationGuidanceEngineTests
{
    private readonly AdaptiveRemediationGuidanceEngine engine =
        new(new AdaptiveMisconceptionClassifier());

    [Fact]
    public void FirstRoundingErrorGetsAnswerAwareHintWithoutCurrentAnswerLeak()
    {
        var item = RoundingItem(
            value: 8822,
            place: 100,
            correct: "8800");

        var guidance = engine.Build(
            item,
            "8900",
            attemptNumber: 1);

        Assert.Equal(
            AdaptiveRemediationStageCodes.TargetedRetry,
            guidance.StageCode);
        Assert.Equal(
            "rounding.wrong_direction_up",
            guidance.MisconceptionId);
        Assert.Contains("2", guidance.Hint, StringComparison.Ordinal);
        Assert.Contains("less than 5", guidance.Hint, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("8800", guidance.Hint, StringComparison.Ordinal);
        Assert.Null(guidance.WorkedExample);
    }

    [Fact]
    public void SecondRoundingErrorGetsIndependentWorkedScaffold()
    {
        var item = RoundingItem(
            value: 8822,
            place: 100,
            correct: "8800");

        var guidance = engine.Build(
            item,
            "8900",
            attemptNumber: 2);

        Assert.Equal(
            AdaptiveRemediationStageCodes.ScaffoldedRecovery,
            guidance.StageCode);
        Assert.NotNull(guidance.WorkedExample);
        Assert.Contains(
            "Worked example",
            guidance.WorkedExample!,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "8800",
            guidance.WorkedExample!,
            StringComparison.Ordinal);
        Assert.Equal(2, guidance.AttemptNumber);
    }

    [Fact]
    public void FreshRoundingRecoveryExampleMatchesRecoveryPlaceAndDoesNotLeakAnswer()
    {
        var recoveryItem = RoundingItem(
            value: 773,
            place: 10,
            correct: "770");

        var workedExample =
            engine.BuildRecoveryWorkedExample(
                recoveryItem);

        Assert.Contains(
            "nearest 10",
            workedExample,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "nearest 100",
            workedExample,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "770",
            workedExample,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "773",
            workedExample,
            StringComparison.Ordinal);
    }

    [Fact]
    public void GenericFamilyStillProvidesBoundedProgressiveGuidance()
    {
        var item = new AssessmentItem
        {
            CorrectAnswer = "12",
            GenerationFamily = "supporting.number.multiply",
            Solution =
                "Use place-value partitioning, then verify with division."
        };

        var first = engine.Build(
            item,
            "13",
            attemptNumber: 1);
        var second = engine.Build(
            item,
            "13",
            attemptNumber: 2);

        Assert.Equal(
            AdaptiveRemediationStageCodes.TargetedRetry,
            first.StageCode);
        Assert.Equal(
            AdaptiveRemediationStageCodes.ScaffoldedRecovery,
            second.StageCode);
        Assert.Null(first.WorkedExample);
        Assert.NotNull(second.WorkedExample);
    }

    private static AssessmentItem RoundingItem(
        int value,
        int place,
        string correct) =>
        new()
        {
            CorrectAnswer = correct,
            GenerationFamily = "supporting.number.rounding",
            Solution =
                "Locate the two multiples and use the halfway point.",
            GenerationParametersJson =
                "{\"parameters\":{\"value\":" +
                value +
                ",\"place\":" +
                place +
                "}}"
        };
}
