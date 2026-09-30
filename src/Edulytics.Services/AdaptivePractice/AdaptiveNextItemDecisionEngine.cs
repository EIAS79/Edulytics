using Edulytics.Core.AdaptivePractice;
using Edulytics.Core.Mathematics.Difficulty;
using Edulytics.Services.Mathematics.Difficulty;

namespace Edulytics.Services.AdaptivePractice;

/// <summary>
/// Deterministic question-by-question decision kernel for Adaptive Practice V2.
///
/// Non-negotiable invariant:
/// an incorrect learner response can never produce a higher-complexity next
/// question. Remediation stays on the same mathematical target (or a bounded
/// lower/alternate approved form) until fresh confirmation evidence exists.
/// </summary>
public sealed class AdaptiveNextItemDecisionEngine(
    MathematicsDifficultyEngine difficultyEngine)
{
    public const int MaximumComplexityScore = 200;
    public const int ComplexityStepLimit = 12;
    private const decimal BlockingPrerequisiteThreshold = 0.50m;

    public AdaptiveNextItemDecision Decide(
        AdaptivePracticeLearningState state)
    {
        Validate(state);

        var latest = state.RecentResponses
            .OrderByDescending(x => x.Sequence)
            .FirstOrDefault();

        var activeMisconception = state.Misconceptions
            .Where(x => x.IsBlocking)
            .OrderByDescending(x => x.LastObservedSequence)
            .ThenByDescending(x => x.ObservationCount)
            .FirstOrDefault();

        var weakestRepresentation = state.RepresentationFluency
            .Where(x => x.QuestionFamilies.Any(
                family => state.AllowedQuestionFamilies.Contains(
                    family,
                    StringComparer.Ordinal)))
            .OrderBy(x => x.Fluency)
            .ThenBy(x => x.Representation, StringComparer.Ordinal)
            .FirstOrDefault();

        var family = SelectFamily(
            state,
            latest,
            activeMisconception,
            weakestRepresentation);

        var representation = SelectRepresentation(
            state,
            family,
            weakestRepresentation);

        // Misconception evidence is family-owned. A remediation preference can
        // intentionally switch the next question family, but an older blocking
        // misconception from another family must never be attached to that item.
        var familyMisconception =
            activeMisconception is not null &&
            string.Equals(
                activeMisconception.QuestionFamily,
                family,
                StringComparison.Ordinal)
                ? activeMisconception
                : null;

        var latestMisconceptionId =
            latest is not null &&
            string.Equals(
                latest.QuestionFamily,
                family,
                StringComparison.Ordinal)
                ? latest.MisconceptionId
                : null;

        var remediationMisconceptionId =
            string.Equals(
                state.Remediation.PreferredQuestionFamily,
                family,
                StringComparison.Ordinal)
                ? state.Remediation.BlockingMisconceptionId
                : null;

        if (latest is not null && !latest.IsCorrect)
        {
            var observed = Math.Clamp(
                latest.ComplexityScore,
                0,
                MaximumComplexityScore);

            var target = Math.Max(
                0,
                observed - ComplexityStepLimit);

            var recoveryReason = familyMisconception is not null
                ? AdaptivePracticeDecisionReasonCodes.MisconceptionRemediation
                : state.PrerequisiteMastery < BlockingPrerequisiteThreshold
                    ? AdaptivePracticeDecisionReasonCodes.PrerequisiteRecovery
                    : weakestRepresentation is not null &&
                      weakestRepresentation.Fluency < 0.50m
                        ? AdaptivePracticeDecisionReasonCodes.RepresentationRecovery
                        : AdaptivePracticeDecisionReasonCodes.UnclassifiedRecovery;

            return Decision(
                state,
                target,
                family,
                representation,
                familyMisconception?.MisconceptionId ??
                latestMisconceptionId,
                recoveryReason,
                remediationLockActive: true,
                confirmationRequired: true,
                isIndependentConfirmation: false,
                progressionEligible: false);
        }

        if (state.Remediation.IsLocked)
        {
            var lockCeiling = Math.Clamp(
                state.Remediation.LockComplexityScore,
                0,
                MaximumComplexityScore);

            var target = Math.Min(
                state.CurrentComplexityScore,
                lockCeiling);

            // A single correct remedial item does not unlock progression.
            // The engine asks for a fresh independent confirmation at the same
            // target/complexity envelope.
            if (state.Remediation.ConfirmationRequired)
            {
                return Decision(
                    state,
                    target,
                    family,
                    representation,
                    remediationMisconceptionId ??
                    familyMisconception?.MisconceptionId,
                    AdaptivePracticeDecisionReasonCodes
                        .MisconceptionConfirmationRequired,
                    remediationLockActive: true,
                    confirmationRequired: true,
                    isIndependentConfirmation: true,
                    progressionEligible: false);
            }

            if (familyMisconception is not null)
            {
                return Decision(
                    state,
                    target,
                    family,
                    representation,
                    familyMisconception.MisconceptionId,
                    AdaptivePracticeDecisionReasonCodes.RemediationLockActive,
                    remediationLockActive: true,
                    confirmationRequired: true,
                    isIndependentConfirmation: false,
                    progressionEligible: false);
            }
        }

        if (state.PrerequisiteMastery < BlockingPrerequisiteThreshold)
        {
            var target = Math.Max(
                0,
                state.CurrentComplexityScore - ComplexityStepLimit);

            return Decision(
                state,
                target,
                family,
                representation,
                familyMisconception?.MisconceptionId,
                AdaptivePracticeDecisionReasonCodes.PrerequisiteRecovery,
                remediationLockActive: false,
                confirmationRequired: false,
                isIndependentConfirmation: false,
                progressionEligible: false);
        }

        var representationAverage = state.RepresentationFluency.Count == 0
            ? 0.50m
            : state.RepresentationFluency.Average(x => x.Fluency);

        var misconceptionCount = state.Misconceptions
            .Where(x => x.IsBlocking)
            .Sum(x => x.ObservationCount);

        var recommendation = difficultyEngine.Recommend(
            new MathematicsAdaptiveLearnerState(
                state.SkillMastery,
                state.PrerequisiteMastery,
                representationAverage,
                misconceptionCount,
                state.RecentSuccessfulItems));

        var desired = Math.Clamp(
            recommendation.TargetComplexityScore,
            0,
            MaximumComplexityScore);

        // Session evidence is allowed to demonstrate readiness beyond an older
        // official mastery snapshot, but only after at least two consecutive
        // verified successes and while no prerequisite/misconception lock is active.
        // This is what allows: recover → confirm → then progress one bounded step.
        if (state.RecentSuccessfulItems >= 2 &&
            state.PrerequisiteMastery >= BlockingPrerequisiteThreshold &&
            familyMisconception is null)
        {
            desired = Math.Max(
                desired,
                Math.Min(
                    MaximumComplexityScore,
                    state.CurrentComplexityScore + ComplexityStepLimit));
        }

        var bounded = BoundStep(
            state.CurrentComplexityScore,
            desired);

        var reason = bounded > state.CurrentComplexityScore
            ? AdaptivePracticeDecisionReasonCodes.ComplexityProgress
            : bounded < state.CurrentComplexityScore
                ? AdaptivePracticeDecisionReasonCodes.ComplexityReduce
                : AdaptivePracticeDecisionReasonCodes.ComplexityConsolidate;

        return Decision(
            state,
            bounded,
            family,
            representation,
            null,
            reason,
            remediationLockActive: false,
            confirmationRequired: false,
            isIndependentConfirmation: false,
            progressionEligible:
                bounded > state.CurrentComplexityScore);
    }

    private static AdaptiveNextItemDecision Decision(
        AdaptivePracticeLearningState state,
        int targetComplexity,
        string family,
        string? representation,
        string? misconceptionId,
        string reason,
        bool remediationLockActive,
        bool confirmationRequired,
        bool isIndependentConfirmation,
        bool progressionEligible) =>
        new(
            state.SkillId,
            Math.Clamp(
                targetComplexity,
                0,
                MaximumComplexityScore),
            family,
            representation,
            misconceptionId,
            reason,
            RequiresFreshExposure: true,
            remediationLockActive,
            confirmationRequired,
            isIndependentConfirmation,
            progressionEligible,
            AdaptivePracticeV2Versions.EngineVersion,
            AdaptivePracticeV2Versions.PolicyVersion);

    private static int BoundStep(
        int current,
        int desired)
    {
        current = Math.Clamp(
            current,
            0,
            MaximumComplexityScore);

        desired = Math.Clamp(
            desired,
            0,
            MaximumComplexityScore);

        if (desired > current)
            return Math.Min(
                desired,
                current + ComplexityStepLimit);

        if (desired < current)
            return Math.Max(
                desired,
                current - ComplexityStepLimit);

        return current;
    }

    private static string SelectFamily(
        AdaptivePracticeLearningState state,
        AdaptivePracticeResponseObservation? latest,
        AdaptivePracticeMisconceptionEvidence? misconception,
        AdaptivePracticeRepresentationFluency? weakestRepresentation)
    {
        if (!string.IsNullOrWhiteSpace(
                state.Remediation.PreferredQuestionFamily) &&
            state.AllowedQuestionFamilies.Contains(
                state.Remediation.PreferredQuestionFamily,
                StringComparer.Ordinal))
        {
            return state.Remediation.PreferredQuestionFamily;
        }

        if (!string.IsNullOrWhiteSpace(
                misconception?.QuestionFamily) &&
            state.AllowedQuestionFamilies.Contains(
                misconception.QuestionFamily,
                StringComparer.Ordinal))
        {
            return misconception.QuestionFamily;
        }

        if (weakestRepresentation is not null)
        {
            var representationFamily =
                weakestRepresentation.QuestionFamilies
                    .FirstOrDefault(family =>
                        state.AllowedQuestionFamilies.Contains(
                            family,
                            StringComparer.Ordinal));

            if (!string.IsNullOrWhiteSpace(representationFamily))
                return representationFamily;
        }

        if (latest is not null &&
            state.AllowedQuestionFamilies.Contains(
                latest.QuestionFamily,
                StringComparer.Ordinal))
        {
            return latest.QuestionFamily;
        }

        return state.AllowedQuestionFamilies[0];
    }

    private static string? SelectRepresentation(
        AdaptivePracticeLearningState state,
        string family,
        AdaptivePracticeRepresentationFluency? weakestRepresentation)
    {
        if (!string.IsNullOrWhiteSpace(
                state.Remediation.PreferredRepresentation))
        {
            var allowed = state.RepresentationFluency.Any(
                x =>
                    string.Equals(
                        x.Representation,
                        state.Remediation.PreferredRepresentation,
                        StringComparison.Ordinal) &&
                    x.QuestionFamilies.Contains(
                        family,
                        StringComparer.Ordinal));

            if (allowed)
                return state.Remediation.PreferredRepresentation;
        }

        if (weakestRepresentation is not null &&
            weakestRepresentation.QuestionFamilies.Contains(
                family,
                StringComparer.Ordinal))
        {
            return weakestRepresentation.Representation;
        }

        return state.RepresentationFluency
            .Where(x => x.QuestionFamilies.Contains(
                family,
                StringComparer.Ordinal))
            .OrderBy(x => x.Fluency)
            .Select(x => x.Representation)
            .FirstOrDefault();
    }

    private static void Validate(
        AdaptivePracticeLearningState state)
    {
        ArgumentNullException.ThrowIfNull(state);
        ArgumentNullException.ThrowIfNull(state.RecentResponses);
        ArgumentNullException.ThrowIfNull(state.Misconceptions);
        ArgumentNullException.ThrowIfNull(state.RepresentationFluency);
        ArgumentNullException.ThrowIfNull(state.AllowedQuestionFamilies);
        ArgumentNullException.ThrowIfNull(state.Remediation);

        if (string.IsNullOrWhiteSpace(state.SkillId) ||
            state.AllowedQuestionFamilies.Count == 0 ||
            state.CurrentComplexityScore is < 0 or > MaximumComplexityScore ||
            state.RecentSuccessfulItems < 0)
        {
            throw new InvalidOperationException(
                "Adaptive Practice V2 learning state is invalid.");
        }

        ValidateRatio(
            state.SkillMastery,
            nameof(state.SkillMastery));

        ValidateRatio(
            state.PrerequisiteMastery,
            nameof(state.PrerequisiteMastery));

        if (state.RepresentationFluency.Any(x =>
                string.IsNullOrWhiteSpace(x.Representation) ||
                x.Fluency is < 0m or > 1m ||
                x.QuestionFamilies.Count == 0))
        {
            throw new InvalidOperationException(
                "Adaptive Practice V2 representation state is invalid.");
        }

        if (state.RecentResponses.Any(x =>
                x.Sequence <= 0 ||
                x.ComplexityScore is < 0 or > MaximumComplexityScore ||
                string.IsNullOrWhiteSpace(x.QuestionFamily)))
        {
            throw new InvalidOperationException(
                "Adaptive Practice V2 response history is invalid.");
        }

        if (state.RecentResponses
                .Select(x => x.Sequence)
                .Distinct()
                .Count() != state.RecentResponses.Count)
        {
            throw new InvalidOperationException(
                "Adaptive Practice V2 response sequence must be unique.");
        }

        if (state.Misconceptions.Any(x =>
                string.IsNullOrWhiteSpace(x.MisconceptionId) ||
                x.ObservationCount < 0 ||
                x.LastObservedSequence < 0))
        {
            throw new InvalidOperationException(
                "Adaptive Practice V2 misconception state is invalid.");
        }

        if (state.Remediation.IsLocked &&
            state.Remediation.LockComplexityScore
                is < 0 or > MaximumComplexityScore)
        {
            throw new InvalidOperationException(
                "Adaptive Practice V2 remediation lock is invalid.");
        }
    }

    private static void ValidateRatio(
        decimal value,
        string name)
    {
        if (value is < 0m or > 1m)
            throw new ArgumentOutOfRangeException(
                name,
                "Adaptive Practice V2 ratios must be in [0,1].");
    }
}
