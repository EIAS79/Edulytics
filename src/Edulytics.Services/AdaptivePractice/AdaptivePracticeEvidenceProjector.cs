using Edulytics.Core.AdaptivePractice;
using Edulytics.Core.Entities;

namespace Edulytics.Services.AdaptivePractice;

public sealed record AdaptivePracticeEvidenceUpdate(
    StudentMisconceptionState? MisconceptionState,
    StudentRepresentationFluencyState? RepresentationState);

/// <summary>
/// Projects one accepted Practice response into private V2 learning state.
/// These projections never write official assessment mastery.
/// </summary>
public sealed class AdaptivePracticeEvidenceProjector(
    AdaptiveMisconceptionClassifier misconceptionClassifier)
{
    public AdaptivePracticeEvidenceUpdate Project(
        AdaptivePracticeSession session,
        AdaptivePracticeTurn turn,
        AssessmentItem item,
        IReadOnlyList<StudentMisconceptionState> existingMisconceptions,
        IReadOnlyList<StudentRepresentationFluencyState> existingRepresentations,
        bool misconceptionLoopEnabled,
        DateTime occurredAtUtc)
    {
        ArgumentNullException.ThrowIfNull(session);
        ArgumentNullException.ThrowIfNull(turn);
        ArgumentNullException.ThrowIfNull(item);
        ArgumentNullException.ThrowIfNull(existingMisconceptions);
        ArgumentNullException.ThrowIfNull(existingRepresentations);

        if (!turn.IsCorrect.HasValue ||
            !turn.AnsweredAtUtc.HasValue ||
            string.IsNullOrWhiteSpace(turn.SubmittedAnswer))
        {
            throw new InvalidOperationException(
                "Adaptive Practice evidence requires an accepted answer.");
        }

        var representation = BuildRepresentationUpdate(
            session,
            turn,
            existingRepresentations,
            occurredAtUtc);

        var misconception = misconceptionLoopEnabled
            ? BuildMisconceptionUpdate(
                session,
                turn,
                item,
                existingMisconceptions,
                occurredAtUtc)
            : null;

        return new(
            misconception,
            representation);
    }

    private StudentMisconceptionState? BuildMisconceptionUpdate(
        AdaptivePracticeSession session,
        AdaptivePracticeTurn turn,
        AssessmentItem item,
        IReadOnlyList<StudentMisconceptionState> existing,
        DateTime occurredAtUtc)
    {
        if (turn.IsCorrect == true)
        {
            if (string.IsNullOrWhiteSpace(
                    turn.MisconceptionFocusId))
            {
                return null;
            }

            var focused = existing.FirstOrDefault(
                x => string.Equals(
                    x.MisconceptionId,
                    turn.MisconceptionFocusId,
                    StringComparison.Ordinal));

            if (focused is null)
                return null;

            var cleanIndependentConfirmation =
                turn.IsIndependentConfirmation &&
                turn.IncorrectAttemptCount == 0;

            return Copy(
                focused,
                status:
                    cleanIndependentConfirmation
                        ? AdaptiveMisconceptionStatus.Resolved
                        : AdaptiveMisconceptionStatus.Remediating,
                confidence:
                    cleanIndependentConfirmation
                        ? Math.Max(0m, focused.Confidence - 0.25m)
                        : focused.Confidence,
                observationCount:
                    focused.ObservationCount,
                lastObservedAtUtc:
                    focused.LastObservedAtUtc,
                lastRemediationAtUtc:
                    occurredAtUtc,
                resolvedAtUtc:
                    cleanIndependentConfirmation
                        ? occurredAtUtc
                        : focused.ResolvedAtUtc);
        }

        var classification =
            misconceptionClassifier.Classify(
                item,
                turn.SubmittedAnswer!);

        if (classification is null)
        {
            // Unclassified incorrect answers still activate the general
            // remediation lock in the decision engine, but do not create a
            // fabricated misconception label.
            return null;
        }

        var current = existing.FirstOrDefault(
            x => string.Equals(
                x.MisconceptionId,
                classification.MisconceptionId,
                StringComparison.Ordinal));

        var count =
            (current?.ObservationCount ?? 0) + 1;
        var confidence = Math.Max(
            current?.Confidence ?? 0m,
            classification.Confidence);

        var status =
            current?.Status ==
                AdaptiveMisconceptionStatus.Resolved
                ? AdaptiveMisconceptionStatus.Reopened
                : count >=
                    classification.ObservationsRequiredToActivate
                    ? AdaptiveMisconceptionStatus.Active
                    : AdaptiveMisconceptionStatus.Suspected;

        return new StudentMisconceptionState
        {
            Id = current?.Id ?? Guid.NewGuid(),
            SchoolId = session.SchoolId,
            StudentProfileId =
                session.StudentProfileId,
            CurriculumAdoptionId =
                session.CurriculumAdoptionId,
            SkillId = session.PrimarySkillId,
            MisconceptionId =
                classification.MisconceptionId,
            QuestionFamily = turn.QuestionFamily,
            Status = status,
            Confidence = confidence,
            ObservationCount = count,
            FirstObservedAtUtc =
                current?.FirstObservedAtUtc ??
                occurredAtUtc,
            LastObservedAtUtc = occurredAtUtc,
            LastRemediationAtUtc =
                current?.LastRemediationAtUtc,
            ResolvedAtUtc = null,
            EngineVersion =
                AdaptivePracticeV2Versions.EngineVersion,
            RowVersion = current?.RowVersion ?? []
        };
    }

    private static StudentRepresentationFluencyState?
        BuildRepresentationUpdate(
            AdaptivePracticeSession session,
            AdaptivePracticeTurn turn,
            IReadOnlyList<StudentRepresentationFluencyState> existing,
            DateTime occurredAtUtc)
    {
        if (string.IsNullOrWhiteSpace(
                turn.Representation))
        {
            return null;
        }

        var current = existing.FirstOrDefault(
            x => string.Equals(
                x.Representation,
                turn.Representation,
                StringComparison.Ordinal));

        var evidence =
            (current?.EvidenceCount ?? 0) + 1;
        var successes =
            (current?.SuccessCount ?? 0) +
            (turn.IsCorrect == true ? 1 : 0);

        return new StudentRepresentationFluencyState
        {
            Id = current?.Id ?? Guid.NewGuid(),
            SchoolId = session.SchoolId,
            StudentProfileId =
                session.StudentProfileId,
            SkillId = session.PrimarySkillId,
            Representation = turn.Representation!,
            EvidenceCount = evidence,
            SuccessCount = successes,
            WeightedFluency =
                decimal.Round(
                    successes / (decimal)evidence,
                    6,
                    MidpointRounding.AwayFromZero),
            LatestEvidenceAtUtc = occurredAtUtc,
            EngineVersion =
                AdaptivePracticeV2Versions.EngineVersion,
            RowVersion = current?.RowVersion ?? []
        };
    }

    private static StudentMisconceptionState Copy(
        StudentMisconceptionState source,
        AdaptiveMisconceptionStatus status,
        decimal confidence,
        int observationCount,
        DateTime lastObservedAtUtc,
        DateTime? lastRemediationAtUtc,
        DateTime? resolvedAtUtc) =>
        new()
        {
            Id = source.Id,
            SchoolId = source.SchoolId,
            StudentProfileId =
                source.StudentProfileId,
            CurriculumAdoptionId =
                source.CurriculumAdoptionId,
            SkillId = source.SkillId,
            MisconceptionId =
                source.MisconceptionId,
            QuestionFamily =
                source.QuestionFamily,
            Status = status,
            Confidence = confidence,
            ObservationCount = observationCount,
            FirstObservedAtUtc =
                source.FirstObservedAtUtc,
            LastObservedAtUtc =
                lastObservedAtUtc,
            LastRemediationAtUtc =
                lastRemediationAtUtc,
            ResolvedAtUtc = resolvedAtUtc,
            EngineVersion =
                AdaptivePracticeV2Versions.EngineVersion,
            RowVersion = source.RowVersion
        };
}
