
namespace Edulytics.Services.Practice;

public interface IPracticeService
{
    Task<PracticeQueryResult<IReadOnlyList<PracticeItemSummary>>> ListAvailableAsync(
        Guid studentUserId,
        Guid curriculumAdoptionId,
        Guid? lessonId,
        CancellationToken cancellationToken = default);

    Task<PracticeCommandResult> StartAsync(
        Guid studentUserId,
        Guid curriculumAdoptionId,
        IReadOnlyList<Guid> itemIds,
        CancellationToken cancellationToken = default);

    Task<PracticeQueryResult<PracticeAttemptDetails>> GetAttemptAsync(
        Guid studentUserId,
        Guid attemptId,
        CancellationToken cancellationToken = default);

    Task<PracticeQueryResult<PracticeFeedback>> AnswerAsync(
        Guid studentUserId,
        Guid attemptId,
        Guid attemptItemId,
        string answer,
        CancellationToken cancellationToken = default);

    Task<PracticeQueryResult<PracticeAttemptDetails>> SubmitAsync(
        Guid studentUserId,
        Guid attemptId,
        CancellationToken cancellationToken = default);
}
