namespace Edulytics.Services.AdaptivePractice;

public sealed record AdaptiveSessionBudgetDecision(
    bool ExtendSession,
    bool CompleteSession,
    int TargetQuestionCount,
    string? StopReason);

public static class AdaptiveSessionBudgetPolicy
{
    public static AdaptiveSessionBudgetDecision Evaluate(
        int answeredSequence,
        int targetQuestionCount,
        bool remediationLockActive,
        bool confirmationRequired,
        bool answerCorrect)
    {
        if (answeredSequence <= 0 ||
            targetQuestionCount <= 0 ||
            targetQuestionCount >
                AdaptivePracticeV2Behavior.MaximumSessionItems)
        {
            throw new InvalidOperationException(
                "Adaptive Practice session budget state is invalid.");
        }

        if (answeredSequence < targetQuestionCount)
        {
            return new(
                ExtendSession: false,
                CompleteSession: false,
                TargetQuestionCount: targetQuestionCount,
                StopReason: null);
        }

        var unresolvedAdaptiveWork =
            remediationLockActive ||
            confirmationRequired ||
            !answerCorrect;

        if (unresolvedAdaptiveWork &&
            targetQuestionCount <
                AdaptivePracticeV2Behavior.MaximumSessionItems)
        {
            return new(
                ExtendSession: true,
                CompleteSession: false,
                TargetQuestionCount: targetQuestionCount + 1,
                StopReason: null);
        }

        return new(
            ExtendSession: false,
            CompleteSession: true,
            TargetQuestionCount: targetQuestionCount,
            StopReason: unresolvedAdaptiveWork
                ? "MAX_REMEDIATION_BUDGET_REACHED"
                : "QUESTION_BUDGET_REACHED");
    }
}
