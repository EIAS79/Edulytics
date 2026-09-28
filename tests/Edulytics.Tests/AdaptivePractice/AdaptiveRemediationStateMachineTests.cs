using Edulytics.Core.AdaptivePractice;
using Edulytics.Services.AdaptivePractice;

namespace Edulytics.Tests.MathematicsIntelligence.AdaptivePractice;

public sealed class AdaptiveRemediationStateMachineTests
{
    private readonly AdaptiveRemediationStateMachine machine = new();

    [Fact]
    public void WrongAnswerStartsRemediationLock()
    {
        var next = machine.Apply(
            AdaptivePracticeRemediationState.None,
            Observation(
                sequence: 1,
                correct: false,
                complexity: 60,
                confirmation: false));

        Assert.True(next.IsLocked);
        Assert.True(next.ConfirmationRequired);
        Assert.Equal(60, next.LockComplexityScore);
    }

    [Fact]
    public void CorrectRemedialAnswerDoesNotUnlockProgression()
    {
        var current = new AdaptivePracticeRemediationState(
            true,
            true,
            60,
            PreferredQuestionFamily:
                "fractions.equivalent.missing_value");

        var next = machine.Apply(
            current,
            Observation(
                sequence: 2,
                correct: true,
                complexity: 48,
                confirmation: false));

        Assert.True(next.IsLocked);
        Assert.True(next.ConfirmationRequired);
    }

    [Fact]
    public void CorrectIndependentConfirmationUnlocksProgression()
    {
        var current = new AdaptivePracticeRemediationState(
            true,
            true,
            60,
            PreferredQuestionFamily:
                "fractions.equivalent.missing_value");

        var next = machine.Apply(
            current,
            Observation(
                sequence: 3,
                correct: true,
                complexity: 48,
                confirmation: true));

        Assert.False(next.IsLocked);
        Assert.False(next.ConfirmationRequired);
    }

    private static AdaptivePracticeResponseObservation Observation(
        int sequence,
        bool correct,
        int complexity,
        bool confirmation) =>
        new(
            sequence,
            correct,
            complexity,
            "fractions.equivalent.missing_value",
            Representation: "symbolic",
            IsIndependentConfirmation: confirmation);
}
