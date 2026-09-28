using Edulytics.Core.AdaptivePractice;

namespace Edulytics.Services.AdaptivePractice;

/// <summary>
/// Versioned remediation progression gate.
///
/// Wrong answer => lock.
/// Correct remedial answer => lock remains.
/// Correct fresh independent confirmation => lock clears.
/// </summary>
public sealed class AdaptiveRemediationStateMachine
{
    public AdaptivePracticeRemediationState Apply(
        AdaptivePracticeRemediationState current,
        AdaptivePracticeResponseObservation response)
    {
        ArgumentNullException.ThrowIfNull(current);
        ArgumentNullException.ThrowIfNull(response);

        if (response.Sequence <= 0 ||
            response.ComplexityScore is < 0 or >
                AdaptiveNextItemDecisionEngine.MaximumComplexityScore ||
            string.IsNullOrWhiteSpace(response.QuestionFamily))
        {
            throw new InvalidOperationException(
                "Adaptive Practice V2 remediation observation is invalid.");
        }

        if (!response.IsCorrect)
        {
            return new AdaptivePracticeRemediationState(
                IsLocked: true,
                ConfirmationRequired: true,
                LockComplexityScore: response.ComplexityScore,
                BlockingMisconceptionId: response.MisconceptionId,
                PreferredQuestionFamily: response.QuestionFamily,
                PreferredRepresentation: response.Representation);
        }

        if (!current.IsLocked)
            return AdaptivePracticeRemediationState.None;

        if (!response.IsIndependentConfirmation)
        {
            return current with
            {
                ConfirmationRequired = true
            };
        }

        return AdaptivePracticeRemediationState.None;
    }
}
