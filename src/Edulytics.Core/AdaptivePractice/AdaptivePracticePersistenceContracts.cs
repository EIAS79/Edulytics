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
    Task AddSessionAsync(
        AdaptivePracticeSession session,
        CancellationToken cancellationToken = default);

    Task<AdaptivePracticeSession?> GetSessionAsync(
        Guid schoolId,
        Guid sessionId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AdaptivePracticeTurn>> GetTurnsAsync(
        Guid schoolId,
        Guid sessionId,
        CancellationToken cancellationToken = default);

    Task AddDecisionAndTurnAsync(
        AdaptiveDecisionSnapshot decision,
        AdaptivePracticeTurn turn,
        CancellationToken cancellationToken = default);

    Task SaveAnsweredTurnAsync(
        AdaptivePracticeTurn turn,
        AdaptivePracticeSession session,
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
