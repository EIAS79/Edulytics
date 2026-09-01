
using Edulytics.Core.Enums;

namespace Edulytics.Services.Practice;

public enum PracticeErrorCode
{
    StudentNotFound,
    AccessDenied,
    NotEnrolled,
    Required,
    ItemNotFound,
    ItemScopeMismatch,
    ItemMissingOutcome,
    OutcomeScopeMismatch,
    AttemptNotFound,
    AttemptNotInProgress,
    AttemptIncomplete,
    ResponseItemMismatch,
    InvalidAnswer
}

public sealed record PracticeQueryResult<T>(T? Value, PracticeErrorCode? Error) where T : class
{
    public static PracticeQueryResult<T> Success(T value) => new(value, null);
    public static PracticeQueryResult<T> Failure(PracticeErrorCode error) => new(null, error);
}

public sealed record PracticeCommandResult(bool Succeeded, PracticeErrorCode? Error, Guid? EntityId = null)
{
    public static PracticeCommandResult Success(Guid? id = null) => new(true, null, id);
    public static PracticeCommandResult Failure(PracticeErrorCode error) => new(false, error);
}

public sealed record PracticeItemSummary(
    Guid Id,
    AssessmentItemType ItemType,
    AssessmentItemDifficulty Difficulty,
    string Prompt,
    Guid? LessonId,
    IReadOnlyList<Guid> OutcomeIds);

public sealed record PracticeAttemptQuestion(
    Guid AttemptItemId,
    Guid AssessmentItemId,
    int Order,
    AssessmentItemType ItemType,
    AssessmentItemDifficulty Difficulty,
    string Prompt,
    bool Answered,
    bool? IsCorrect,
    string? Feedback);

public sealed record PracticeAttemptDetails(
    Guid AttemptId,
    PracticeAttemptStatus Status,
    DateTime StartedAtUtc,
    DateTime? SubmittedAtUtc,
    decimal Score,
    decimal MaxScore,
    decimal Percentage,
    IReadOnlyList<PracticeAttemptQuestion> Questions);

public sealed record PracticeFeedback(
    Guid AttemptItemId,
    bool IsCorrect,
    decimal Score,
    string Solution);
