namespace Edulytics.Services.Analytics;

public enum AnalyticsInterventionErrorCode
{
    AccessDenied = 1,
    InvalidRequest = 2,
    EvaluationUnavailable = 3,
    SkillNotFound = 4,
    TermNotFound = 5,
    AssessmentCreateFailed = 6,
    DeliverySetupFailed = 7,
    QuestionGenerationFailed = 8
}

public sealed record CreateInterventionCheckRequest(
    Guid StudentProfileId,
    Guid AcademicYearId,
    Guid ClassGroupId,
    Guid SubjectId,
    Guid LearningOutcomeId,
    string SkillKey,
    int QuestionCount = 5,
    int Seed = 0);

public sealed record AnalyticsInterventionCheck(
    Guid AssessmentId,
    string Title,
    Guid StudentProfileId,
    Guid LearningOutcomeId,
    string SkillKey,
    string SkillName,
    decimal? BaselineMasteryPercentage,
    decimal? BaselineAssessmentMasteryPercentage,
    DateOnly AssessmentDate,
    int QuestionCount,
    string ReviewRoute);

public sealed record AnalyticsInterventionResult(
    AnalyticsInterventionCheck? Value,
    AnalyticsInterventionErrorCode? Error)
{
    public static AnalyticsInterventionResult Success(
        AnalyticsInterventionCheck value) =>
        new(value, null);

    public static AnalyticsInterventionResult Failure(
        AnalyticsInterventionErrorCode error) =>
        new(null, error);
}

public interface IAnalyticsInterventionService
{
    Task<AnalyticsInterventionResult> CreateTargetedCheckAsync(
        Guid actorUserId,
        CreateInterventionCheckRequest request,
        CancellationToken cancellationToken = default);
}
