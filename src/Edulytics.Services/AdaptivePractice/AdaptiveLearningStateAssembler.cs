using Edulytics.Core.AdaptivePractice;
using Edulytics.Core.Entities;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Core.Mathematics.Skills;
using Edulytics.Core.Practice;
using Edulytics.Services.Analytics;

namespace Edulytics.Services.AdaptivePractice;

/// <summary>
/// Builds the deterministic V2 learner state from existing official mastery and
/// V2-private session evidence. Official mastery is read-only here.
/// </summary>
public sealed class AdaptiveLearningStateAssembler(
    AdaptiveRemediationStateMachine remediationStateMachine)
{
    private const decimal NeutralPrerequisiteMastery = 0.50m;
    private const int DefaultComplexity = 42;

    public AdaptivePracticeLearningState Build(
        StudentPrivatePracticeContext context,
        Guid lessonId,
        LessonPracticeContract contract,
        IReadOnlyList<AdaptivePracticeTurn> turns,
        IReadOnlyList<StudentMisconceptionState> misconceptionStates,
        IReadOnlyList<StudentRepresentationFluencyState> representationStates)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(contract);
        ArgumentNullException.ThrowIfNull(turns);
        ArgumentNullException.ThrowIfNull(misconceptionStates);
        ArgumentNullException.ThrowIfNull(representationStates);

        var officialOutcomeNodeIds = context.LessonOutcomes
            .Where(x => x.PedagogicalLessonId == lessonId)
            .Select(x => x.OutcomeNodeId)
            .ToHashSet();

        var officialOutcomeIds = context.LearningOutcomes
            .Where(x =>
                x.OfficialContentNodeId.HasValue &&
                officialOutcomeNodeIds.Contains(
                    x.OfficialContentNodeId.Value))
            .Select(x => x.Id)
            .ToHashSet();

        var masteryValues = context.OfficialMasteries
            .Where(x =>
                officialOutcomeIds.Contains(x.LearningOutcomeId) &&
                x.EvidenceCount > 0)
            .Select(x =>
                Math.Clamp(
                    x.MasteryPercentage / 100m,
                    0m,
                    1m))
            .ToArray();

        var skillMastery = masteryValues.Length == 0
            ? 0.50m
            : masteryValues.Average();

        var prerequisiteMastery = ResolvePrerequisiteMastery(
            context,
            contract.SkillId);

        var answered = turns
            .Where(x => x.IsCorrect.HasValue)
            .OrderBy(x => x.Sequence)
            .ToArray();

        var remediation = ReplayRemediation(answered);

        var recent = answered
            .TakeLast(8)
            .Select(ToObservation)
            .ToArray();

        var recentSuccessfulItems = 0;
        foreach (var response in answered.Reverse())
        {
            if (response.IsCorrect != true)
                break;
            recentSuccessfulItems++;
        }

        var currentComplexity = turns.Count == 0
            ? DefaultComplexity
            : Math.Clamp(
                turns.MaxBy(x => x.Sequence)!
                    .MathematicalComplexityScore,
                0,
                AdaptiveNextItemDecisionEngine.MaximumComplexityScore);

        var misconceptions = misconceptionStates
            .Where(x =>
                x.Status is
                    AdaptiveMisconceptionStatus.Active or
                    AdaptiveMisconceptionStatus.Remediating or
                    AdaptiveMisconceptionStatus.Reopened)
            .Select(x =>
                new AdaptivePracticeMisconceptionEvidence(
                    x.MisconceptionId,
                    x.ObservationCount,
                    IsBlocking: true,
                    x.QuestionFamily,
                    answered
                        .Where(turn =>
                            string.Equals(
                                turn.MisconceptionFocusId,
                                x.MisconceptionId,
                                StringComparison.Ordinal))
                        .Select(turn => turn.Sequence)
                        .DefaultIfEmpty(0)
                        .Max()))
            .ToArray();

        var representations = representationStates
            .Where(x =>
                x.EvidenceCount > 0 &&
                x.WeightedFluency is >= 0m and <= 1m)
            .Select(x =>
                new AdaptivePracticeRepresentationFluency(
                    x.Representation,
                    x.WeightedFluency,
                    contract.AllowedQuestionFamilies))
            .ToArray();

        return new AdaptivePracticeLearningState(
            contract.SkillId,
            skillMastery,
            prerequisiteMastery,
            currentComplexity,
            recentSuccessfulItems,
            recent,
            misconceptions,
            representations,
            contract.AllowedQuestionFamilies,
            remediation);
    }

    private static decimal ResolvePrerequisiteMastery(
        StudentPrivatePracticeContext context,
        string skillId)
    {
        if (!MathematicsSkillMetadataRegistry.TryResolve(
                skillId,
                out var metadata) ||
            metadata is null ||
            metadata.Prerequisites.Count == 0)
        {
            return NeutralPrerequisiteMastery;
        }

        var prerequisiteKeys =
            metadata.Prerequisites.ToHashSet(
                StringComparer.Ordinal);

        var masteryByOutcome = context.OfficialMasteries
            .Where(x => x.EvidenceCount > 0)
            .GroupBy(x => x.LearningOutcomeId)
            .ToDictionary(
                x => x.Key,
                x => x.OrderByDescending(row =>
                        row.CalculatedAtUtc)
                    .First()
                    .MasteryPercentage);

        var measuredPrerequisites =
            new List<decimal>();

        foreach (var outcome in context.LearningOutcomes)
        {
            if (!masteryByOutcome.TryGetValue(
                    outcome.Id,
                    out var mastery))
            {
                continue;
            }

            var descriptors =
                EvaluationSkillResolver.ResolveExpected(
                    outcome);

            if (descriptors.Any(descriptor =>
                    prerequisiteKeys.Contains(
                        descriptor.SkillKey)))
            {
                measuredPrerequisites.Add(
                    Math.Clamp(
                        mastery / 100m,
                        0m,
                        1m));
            }
        }

        // Missing prerequisite evidence is not interpreted as failure.
        // When evidence exists, the weakest measured canonical prerequisite
        // controls readiness, matching Student 360's weak-prerequisite intent.
        return measuredPrerequisites.Count == 0
            ? NeutralPrerequisiteMastery
            : measuredPrerequisites.Min();
    }

    private AdaptivePracticeRemediationState ReplayRemediation(
        IReadOnlyList<AdaptivePracticeTurn> answered)
    {
        var state = AdaptivePracticeRemediationState.None;

        foreach (var turn in answered)
        {
            if (turn.IncorrectAttemptCount > 0)
            {
                state = remediationStateMachine.Apply(
                    state,
                    new AdaptivePracticeResponseObservation(
                        turn.Sequence,
                        IsCorrect: false,
                        turn.MathematicalComplexityScore,
                        turn.QuestionFamily,
                        turn.Representation,
                        turn.MisconceptionFocusId,
                        IsIndependentConfirmation: false));
            }

            state = remediationStateMachine.Apply(
                state,
                ToObservation(turn));
        }

        return state;
    }

    private static AdaptivePracticeResponseObservation ToObservation(
        AdaptivePracticeTurn turn) =>
        new(
            turn.Sequence,
            turn.IsCorrect == true,
            turn.MathematicalComplexityScore,
            turn.QuestionFamily,
            turn.Representation,
            turn.MisconceptionFocusId,
            turn.IsIndependentConfirmation);
}
