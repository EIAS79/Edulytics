using Edulytics.Core.Entities;

namespace Edulytics.Core.AdaptivePractice;

public enum AdaptivePracticePurpose
{
    PrivatePractice = 1,
    Diagnostic = 2,
    InterventionPractice = 3,
    ReassessmentPreparation = 4,
    LiveClassroomPractice = 5
}

public enum AdaptivePracticeSessionStatus
{
    InProgress = 1,
    Completed = 2,
    Paused = 3,
    FailedSafe = 4,
    Abandoned = 5
}

public enum AdaptiveMisconceptionStatus
{
    Suspected = 1,
    Active = 2,
    Remediating = 3,
    Resolved = 4,
    Reopened = 5
}

public interface IAdaptivePracticeRepository
{
    Task CreateSessionWithFirstTurnAsync(
        AdaptivePracticeSession session,
        AssessmentItem item,
        IReadOnlyList<AssessmentItemOutcome> itemOutcomes,
        StudentItemExposure exposure,
        AdaptiveDecisionSnapshot decision,
        AdaptivePracticeTurn turn,
        CancellationToken cancellationToken = default);

    Task<AdaptivePracticeSession?> GetSessionAsync(
        Guid schoolId,
        Guid sessionId,
        CancellationToken cancellationToken = default);

    Task<AdaptivePracticeTurn?> GetTurnAsync(
        Guid schoolId,
        Guid sessionId,
        int sequence,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AdaptivePracticeTurn>> GetTurnsAsync(
        Guid schoolId,
        Guid sessionId,
        CancellationToken cancellationToken = default);

    Task<AssessmentItem?> GetItemAsync(
        Guid schoolId,
        Guid assessmentItemId,
        CancellationToken cancellationToken = default);

    Task CommitAnsweredTurnAsync(
        AdaptivePracticeSession session,
        AdaptivePracticeTurn answeredTurn,
        StudentMisconceptionState? misconceptionState,
        StudentRepresentationFluencyState? representationState,
        AssessmentItem? nextItem,
        IReadOnlyList<AssessmentItemOutcome> nextItemOutcomes,
        StudentItemExposure? nextExposure,
        AdaptiveDecisionSnapshot? nextDecision,
        AdaptivePracticeTurn? nextTurn,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<StudentMisconceptionState>>
        GetMisconceptionStatesAsync(
            Guid schoolId,
            Guid studentProfileId,
            Guid curriculumAdoptionId,
            string skillId,
            CancellationToken cancellationToken = default);

    Task UpsertMisconceptionStateAsync(
        StudentMisconceptionState state,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<StudentRepresentationFluencyState>>
        GetRepresentationStatesAsync(
            Guid schoolId,
            Guid studentProfileId,
            string skillId,
            CancellationToken cancellationToken = default);

    Task UpsertRepresentationStateAsync(
        StudentRepresentationFluencyState state,
        CancellationToken cancellationToken = default);
}
