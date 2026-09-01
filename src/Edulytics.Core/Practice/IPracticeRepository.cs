
using Edulytics.Core.Entities;

namespace Edulytics.Core.Practice;

public interface IPracticeRepository
{
    Task<StudentProfile?> FindStudentByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<bool> IsEnrolledInAdoptionAsync(
        Guid schoolId,
        Guid studentProfileId,
        Guid curriculumAdoptionId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AssessmentItem>> ListItemsAsync(
        Guid schoolId,
        Guid curriculumAdoptionId,
        Guid? lessonId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AssessmentItem>> GetItemsAsync(
        Guid schoolId,
        IReadOnlyCollection<Guid> itemIds,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyDictionary<Guid, IReadOnlyList<Guid>>> GetOutcomeIdsAsync(
        Guid schoolId,
        IReadOnlyCollection<Guid> itemIds,
        CancellationToken cancellationToken = default);

    Task<bool> OutcomesBelongToAdoptionAsync(
        Guid schoolId,
        Guid curriculumAdoptionId,
        IReadOnlyCollection<Guid> outcomeIds,
        CancellationToken cancellationToken = default);

    Task AddAttemptAsync(
        PracticeAttempt attempt,
        IReadOnlyCollection<PracticeAttemptItem> items,
        IReadOnlyCollection<StudentItemExposure> exposures,
        CancellationToken cancellationToken = default);

    Task<PracticeAttempt?> GetAttemptAsync(
        Guid schoolId,
        Guid attemptId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PracticeAttemptItem>> GetAttemptItemsAsync(
        Guid schoolId,
        Guid attemptId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PracticeResponse>> GetResponsesAsync(
        Guid schoolId,
        Guid attemptId,
        CancellationToken cancellationToken = default);

    Task SaveResponseAsync(
        PracticeResponse response,
        CancellationToken cancellationToken = default);

    Task CompleteAttemptAsync(
        PracticeAttempt attempt,
        IReadOnlyCollection<LearningEvidence> evidence,
        CancellationToken cancellationToken = default);
}
