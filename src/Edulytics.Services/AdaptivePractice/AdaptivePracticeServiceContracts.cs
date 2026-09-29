using Edulytics.Core.Enums;

namespace Edulytics.Services.AdaptivePractice;

public enum AdaptivePracticeV2Error
{
    AccessDenied = 1,
    CurriculumNotAvailable = 2,
    LessonNotAvailable = 3,
    NotEligible = 4,
    SessionNotFound = 5,
    SessionNotInProgress = 6,
    TurnNotFound = 7,
    TurnAlreadyAnswered = 8,
    InvalidAnswer = 9,
    GenerationFailed = 10,
    PersistenceFailed = 11
}

public sealed record AdaptivePracticeQuestionView(
    Guid TurnId,
    int Sequence,
    Guid AssessmentItemId,
    AssessmentItemType ItemType,
    AssessmentItemDifficulty Difficulty,
    string Prompt,
    string? GenerationFamily,
    string? GenerationParametersJson,
    string? Representation,
    int MathematicalComplexityScore,
    bool IsIndependentConfirmation,
    int IncorrectAttemptCount,
    string? LastIncorrectAnswer);

public sealed record AdaptivePracticeSessionView(
    Guid SessionId,
    Guid CurriculumAdoptionId,
    Guid LessonId,
    string LessonCode,
    string SkillId,
    int CurrentSequence,
    int TargetQuestionCount,
    bool IsCompleted,
    bool IsPaused,
    string? StopReason,
    AdaptivePracticeQuestionView? CurrentQuestion);

public sealed record AdaptivePracticeStartResult(
    AdaptivePracticeSessionView? Session,
    AdaptivePracticeV2Error? Error)
{
    public bool Succeeded => Session is not null && Error is null;

    public static AdaptivePracticeStartResult Success(
        AdaptivePracticeSessionView session) =>
        new(session, null);

    public static AdaptivePracticeStartResult Failure(
        AdaptivePracticeV2Error error) =>
        new(null, error);
}

public sealed record AdaptivePracticeAnswerResult(
    AdaptivePracticeSessionView? Session,
    bool? IsCorrect,
    string? Feedback,
    AdaptivePracticeV2Error? Error)
{
    public bool Succeeded => Session is not null && Error is null;

    public static AdaptivePracticeAnswerResult Success(
        AdaptivePracticeSessionView session,
        bool isCorrect,
        string feedback) =>
        new(session, isCorrect, feedback, null);

    public static AdaptivePracticeAnswerResult Failure(
        AdaptivePracticeV2Error error) =>
        new(null, null, null, error);
}

public interface IAdaptivePracticeV2Service
{
    Task<AdaptivePracticeStartResult> StartLessonAsync(
        Guid studentUserId,
        Guid curriculumAdoptionId,
        Guid lessonId,
        CancellationToken cancellationToken = default);

    Task<AdaptivePracticeStartResult> GetSessionAsync(
        Guid studentUserId,
        Guid sessionId,
        CancellationToken cancellationToken = default);

    Task<AdaptivePracticeAnswerResult> AnswerAsync(
        Guid studentUserId,
        Guid sessionId,
        int sequence,
        string answer,
        CancellationToken cancellationToken = default);
}
