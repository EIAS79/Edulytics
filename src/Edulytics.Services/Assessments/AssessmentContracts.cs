using Edulytics.Core.Enums;

namespace Edulytics.Services.Assessments;

public enum AssessmentErrorCode
{
    AccessDenied,
    SchoolNotActive,
    Required,
    InvalidText,
    InvalidDate,
    InvalidAssessmentType,
    InvalidSchedule,
    InvalidDeliveryModeForType,
    InvalidMaxScore,
    InvalidQuestionScore,
    InvalidOrder,
    TermNotFound,
    ClassGroupNotFound,
    SubjectNotFound,
    AssessmentNotFound,
    QuestionNotFound,
    OutcomeNotFound,
    StudentNotFound,
    StudentNotEnrolled,
    TeacherNotAssigned,
    DuplicateAssessment,
    DuplicateQuestionOrder,
    DuplicateOutcomeMapping,
    OutcomeDoesNotMatchAssessment,
    AssessmentNotDraft,
    AssessmentNotOpen,
    AssessmentAlreadyClosed,
    AssessmentResultsNotReady,
    AssessmentHasNoQuestions,
    AssessmentScoreMismatch,
    QuestionMissingOutcome,
    ResultQuestionMismatch,
    ConcurrencyConflict,
    PersistenceError
}

public sealed record AssessmentCommandResult(
    bool Succeeded,
    string Field,
    AssessmentErrorCode? Error,
    Guid? EntityId = null)
{
    public static AssessmentCommandResult Success(Guid? id = null) =>
        new(true, string.Empty, null, id);

    public static AssessmentCommandResult Failure(string field, AssessmentErrorCode error) =>
        new(false, field, error);
}

public sealed record AssessmentQueryResult<T>(T? Value, AssessmentErrorCode? Error)
    where T : class
{
    public static AssessmentQueryResult<T> Success(T value) => new(value, null);
    public static AssessmentQueryResult<T> Failure(AssessmentErrorCode error) => new(null, error);
}

public sealed record AssessmentTermItem(Guid Id, Guid AcademicYearId, string Name);
public sealed record AssessmentClassItem(Guid Id, Guid AcademicYearId, Guid GradeLevelId, string Name, string Code);
public sealed record AssessmentSubjectItem(Guid Id, string Name, string Code);
public sealed record AssessmentOutcomeItem(Guid Id, string Code, string Description);

public sealed record AssessmentListItem(
    Guid Id,
    Guid SubjectId,
    Guid ClassGroupId,
    Guid AcademicYearId,
    Guid TermId,
    string Title,
    DateOnly AssessmentDate,
    decimal MaxScore,
    AssessmentStatus Status,
    byte[] RowVersion,
    AssessmentTargetType TargetType = AssessmentTargetType.Class,
    Guid? TargetStudentProfileId = null,
    AssessmentDeliveryMode DeliveryMode = AssessmentDeliveryMode.Offline,
    AssessmentDifficultyBand DifficultyBand = AssessmentDifficultyBand.AtClassLevel)
{
    public AssessmentType AssessmentType { get; init; } = AssessmentType.Exam;
    public DateTime? AvailableFromUtc { get; init; }
    public DateTime? DueAtUtc { get; init; }
    public int? AttemptTimeLimitMinutes { get; init; }
    public AssessmentResultReleaseStatus ResultReleaseStatus { get; init; } =
        AssessmentResultReleaseStatus.Withheld;
    public DateTime? ResultsPublishedAtUtc { get; init; }
    public Guid? ResultsPublishedByUserId { get; init; }
    public bool IsScored => AssessmentType == AssessmentType.Exam;
    public bool CountsTowardEvaluation => AssessmentType == AssessmentType.Exam;
}

public sealed record AssessmentQuestionItem(
    Guid Id,
    string Prompt,
    decimal MaxScore,
    int Order,
    IReadOnlyList<Guid> OutcomeIds)
{
    public string? CorrectAnswer { get; init; }
}

public sealed record AssessmentWorkspace(
    IReadOnlyList<AssessmentListItem> Assessments,
    IReadOnlyList<AssessmentTermItem> Terms,
    IReadOnlyList<AssessmentClassItem> ClassGroups,
    IReadOnlyList<AssessmentSubjectItem> Subjects)
{
    public string SchoolTimeZoneId { get; init; } = "UTC";
}

public sealed record AssessmentDetails(
    AssessmentListItem Assessment,
    IReadOnlyList<AssessmentQuestionItem> Questions,
    IReadOnlyList<AssessmentOutcomeItem> EligibleOutcomes,
    IReadOnlyList<AssessmentClassItem> ClassGroups,
    IReadOnlyList<AssessmentSubjectItem> Subjects,
    IReadOnlyList<AssessmentTermItem> Terms)
{
    public string SchoolTimeZoneId { get; init; } = "UTC";
}

public sealed record AssessmentStudentResultItem(
    Guid StudentProfileId,
    string StudentNumber,
    string DisplayName,
    Guid? ResultId,
    decimal Score,
    decimal Percentage,
    byte[]? RowVersion,
    IReadOnlyDictionary<Guid, decimal> QuestionScores)
{
    public IReadOnlyDictionary<Guid, string> QuestionResponses { get; init; } =
        new Dictionary<Guid, string>();
}

public sealed record AssessmentResultsWorkspace(
    AssessmentListItem Assessment,
    IReadOnlyList<AssessmentQuestionItem> Questions,
    IReadOnlyList<AssessmentStudentResultItem> Students);

public sealed record CreateAssessmentRequest(
    Guid ClassGroupId,
    Guid SubjectId,
    Guid TermId,
    string Title,
    DateOnly AssessmentDate,
    decimal MaxScore,
    AssessmentType AssessmentType = AssessmentType.Exam,
    AssessmentDeliveryMode DeliveryMode = AssessmentDeliveryMode.Offline,
    DateTime? AvailableFromLocal = null,
    DateTime? DueAtLocal = null,
    int? AttemptTimeLimitMinutes = null);

public sealed record ReuseAssessmentRequest(
    Guid SourceAssessmentId,
    Guid TargetClassGroupId,
    string Title);

public sealed record UpdateAssessmentRequest(
    Guid Id,
    string Title,
    DateOnly AssessmentDate,
    decimal MaxScore,
    byte[] RowVersion,
    AssessmentDeliveryMode? DeliveryMode = null,
    DateTime? AvailableFromLocal = null,
    DateTime? DueAtLocal = null,
    int? AttemptTimeLimitMinutes = null);

public sealed record CreateAssessmentQuestionRequest(
    Guid AssessmentId,
    string Prompt,
    decimal MaxScore,
    int Order,
    IReadOnlyList<Guid> OutcomeIds,
    byte[] AssessmentRowVersion);

public sealed record UpdateAssessmentQuestionRequest(
    Guid QuestionId,
    string Prompt,
    decimal MaxScore,
    int Order,
    IReadOnlyList<Guid> OutcomeIds,
    byte[] AssessmentRowVersion);

public sealed record DeleteAssessmentRequest(
    Guid AssessmentId,
    byte[] RowVersion);

public sealed record DeleteAssessmentQuestionRequest(
    Guid QuestionId,
    byte[] AssessmentRowVersion);

public sealed record MapQuestionOutcomeRequest(Guid QuestionId, Guid OutcomeId, byte[] AssessmentRowVersion);
public sealed record UnmapQuestionOutcomeRequest(Guid QuestionId, Guid OutcomeId, byte[] AssessmentRowVersion);

public sealed record SaveStudentAssessmentResultRequest(
    Guid AssessmentId,
    Guid StudentProfileId,
    IReadOnlyList<Guid> QuestionIds,
    IReadOnlyList<decimal> Scores,
    byte[]? ResultRowVersion);

public sealed record ImportAssessmentResultRow(
    Guid StudentProfileId,
    IReadOnlyList<decimal> Scores,
    byte[]? ResultRowVersion);

public sealed record ImportAssessmentResultsRequest(
    Guid AssessmentId,
    IReadOnlyList<Guid> QuestionIds,
    IReadOnlyList<ImportAssessmentResultRow> Rows);
