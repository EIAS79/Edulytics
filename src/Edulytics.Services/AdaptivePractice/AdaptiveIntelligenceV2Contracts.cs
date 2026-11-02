using Edulytics.Core.Analytics;
using Edulytics.Core.Enums;
using Edulytics.Services.Analytics;

namespace Edulytics.Services.AdaptivePractice;

public enum AdaptiveIntelligenceV2Error
{
    FeatureDisabled = 1,
    AccessDenied = 2,
    ScopeNotAvailable = 3,
    NotEnoughEvidence = 4
}

public sealed record AdaptiveIntelligenceV2Result<T>(
    T? Value,
    AdaptiveIntelligenceV2Error? Error)
    where T : class
{
    public bool Succeeded => Value is not null && Error is null;

    public static AdaptiveIntelligenceV2Result<T> Success(T value) =>
        new(value, null);

    public static AdaptiveIntelligenceV2Result<T> Failure(
        AdaptiveIntelligenceV2Error error) =>
        new(null, error);
}

public sealed record AdaptiveNextStepsView(
    Guid AcademicYearId,
    Guid ClassGroupId,
    Guid SubjectId,
    IReadOnlyList<StudentSelfNextStep> Steps,
    DateTime GeneratedAtUtc);

public sealed record AdaptiveQuestionLogRow(
    Guid SessionId,
    string LessonCode,
    string CurriculumLabel,
    string LessonTitle,
    string SkillId,
    int Sequence,
    string Prompt,
    AssessmentItemDifficulty Difficulty,
    string QuestionFamily,
    string? Representation,
    int MathematicalComplexityScore,
    string? SubmittedAnswer,
    int IncorrectAttemptCount,
    string? LastIncorrectAnswer,
    bool? IsCorrect,
    string? Feedback,
    string DecisionReasonCode,
    string? MisconceptionFocusId,
    bool IsIndependentConfirmation,
    DateTime PresentedAtUtc,
    DateTime? AnsweredAtUtc,
    long? ResponseDurationMs);

public sealed record AdaptiveQuestionLogView(
    int SessionCount,
    int QuestionCount,
    IReadOnlyList<AdaptiveQuestionLogRow> Questions,
    DateTime GeneratedAtUtc);

public sealed record AdaptiveClassroomStudentSignal(
    Guid StudentProfileId,
    string StudentNumber,
    string DisplayName,
    EvaluationPriority Priority,
    decimal? CurrentMasteryPercentage,
    bool AdaptiveActive,
    string? CurrentSkillId,
    string? CurrentQuestionFamily,
    bool? LatestAnswerCorrect,
    int ActiveMisconceptionCount,
    bool NeedsIntervention);

public sealed record AdaptiveClassroomSnapshot(
    Guid AcademicYearId,
    string AcademicYearName,
    Guid ClassGroupId,
    string ClassName,
    Guid SubjectId,
    string SubjectName,
    int StudentCount,
    int ActiveAdaptiveStudents,
    int StudentsNeedingIntervention,
    int ActiveMisconceptionSignals,
    IReadOnlyList<AdaptiveClassroomStudentSignal> Students,
    DateTime GeneratedAtUtc);

public sealed record AdaptiveDiagnosticPreview(
    Guid CurriculumAdoptionId,
    Guid LessonId,
    string LessonCode,
    Guid TargetLearningOutcomeId,
    string OutcomeCode,
    string OutcomeDescription,
    AssessmentItemDifficulty NextDifficulty,
    decimal EvidenceCreditMultiplier,
    bool RequiresFreshExposure,
    string Reason,
    string FormulaVersion,
    DateTime GeneratedAtUtc);

public interface IAdaptiveIntelligenceV2Service
{
    Task<AdaptiveIntelligenceV2Result<AdaptiveNextStepsView>> GetNextStepsAsync(
        Guid actorUserId,
        Guid academicYearId,
        Guid classGroupId,
        Guid subjectId,
        CancellationToken cancellationToken = default);

    Task<AdaptiveIntelligenceV2Result<AdaptiveQuestionLogView>> GetQuestionLogAsync(
        Guid actorUserId,
        int take = 100,
        CancellationToken cancellationToken = default);

    Task<AdaptiveIntelligenceV2Result<AdaptiveClassroomSnapshot>> GetLiveClassroomAsync(
        Guid actorUserId,
        Guid academicYearId,
        Guid classGroupId,
        Guid subjectId,
        CancellationToken cancellationToken = default);

    Task<AdaptiveIntelligenceV2Result<AdaptiveDiagnosticPreview>> GetDiagnosticPreviewAsync(
        Guid actorUserId,
        Guid curriculumAdoptionId,
        Guid lessonId,
        CancellationToken cancellationToken = default);
}
