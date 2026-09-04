using Edulytics.Core.Enums;

namespace Edulytics.Services.Assessments;

public enum StudentAssessmentDeliveryErrorCode
{
    AccessDenied = 1,
    SchoolNotActive = 2,
    ProfileNotLinked = 3,
    AssessmentNotFound = 4,
    AssessmentNotOpen = 5,
    AssessmentOffline = 6,
    NotTargeted = 7,
    AlreadySubmitted = 8,
    InvalidSubmission = 9,
    PersistenceError = 10
}

public sealed record StudentAssessmentQuestion(Guid Id, int Order, string Prompt, decimal MaxScore);

public sealed record StudentAssessmentAttempt(
    Guid AssessmentId,
    string Title,
    DateOnly AssessmentDate,
    decimal MaxScore,
    AssessmentDifficultyBand DifficultyBand,
    IReadOnlyList<StudentAssessmentQuestion> Questions);

public sealed record StudentAssessmentResponse(Guid QuestionId, string ResponseText);

public sealed record StudentAssessmentSubmission(
    Guid AssessmentId,
    string Title,
    decimal Score,
    decimal MaxScore,
    decimal Percentage,
    DateTime SubmittedAtUtc);

public sealed record StudentAssessmentDeliveryResult<T>(T? Value, StudentAssessmentDeliveryErrorCode? Error)
    where T : class
{
    public static StudentAssessmentDeliveryResult<T> Success(T value) => new(value, null);
    public static StudentAssessmentDeliveryResult<T> Failure(StudentAssessmentDeliveryErrorCode error) => new(null, error);
}

public interface IStudentAssessmentDeliveryService
{
    Task<StudentAssessmentDeliveryResult<StudentAssessmentAttempt>> GetAttemptAsync(
        Guid actorUserId,
        Guid assessmentId,
        CancellationToken cancellationToken = default);

    Task<StudentAssessmentDeliveryResult<StudentAssessmentSubmission>> SubmitAsync(
        Guid actorUserId,
        Guid assessmentId,
        IReadOnlyList<StudentAssessmentResponse> responses,
        CancellationToken cancellationToken = default);
}
