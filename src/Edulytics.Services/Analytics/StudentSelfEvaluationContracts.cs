using Edulytics.Core.Analytics;

namespace Edulytics.Services.Analytics;

public enum StudentSelfEvaluationErrorCode
{
    AccessDenied = 1,
    SchoolNotActive = 2,
    ProfileNotLinked = 3,
    ScopeNotAvailable = 4,
    InvalidSourceData = 5
}

public sealed record StudentSelfEvaluationResult<T>(
    T? Value,
    StudentSelfEvaluationErrorCode? Error)
    where T : class
{
    public static StudentSelfEvaluationResult<T> Success(T value) =>
        new(value, null);

    public static StudentSelfEvaluationResult<T> Failure(
        StudentSelfEvaluationErrorCode error) =>
        new(null, error);
}

public sealed record StudentPrivatePracticeSkillEvaluation(
    string SkillKey,
    string SkillName,
    decimal? MasteryPercentage,
    int EvidenceCount,
    int SessionCount,
    EvaluationTrendBand Trend,
    DateTime? LatestEvidenceAtUtc);

public sealed record StudentSelfSkillEvaluation(
    string SkillKey,
    string SkillName,
    decimal? OfficialMasteryPercentage,
    decimal? AssessmentMasteryPercentage,
    decimal? PrivatePracticeMasteryPercentage,
    decimal? PracticeToAssessmentGapPercentagePoints,
    EvaluationSkillStatus OfficialStatus,
    EvaluationTrendBand OfficialTrend,
    EvaluationTrendBand PrivatePracticeTrend,
    EvaluationConfidenceBand EvidenceConfidence,
    int OfficialEvidenceCount,
    int PrivatePracticeEvidenceCount,
    EvaluationPriority Priority);

public sealed record StudentSelfPracticeSummary(
    int SessionCount,
    int EvidenceCount,
    int ActiveDayCount,
    int SkillCount,
    decimal? MasteryPercentage,
    EvaluationTrendBand Trend,
    DateTime? LatestEvidenceAtUtc);

public sealed record StudentSelfNextStep(
    string SkillKey,
    string SkillName,
    EvaluationPriority Priority,
    string Message);

public sealed record StudentSelfEvaluationPage(
    StudentSubjectEvaluation OfficialEvaluation,
    StudentSelfPracticeSummary PrivatePractice,
    decimal? PrivatePracticeToAssessmentGapPercentagePoints,
    IReadOnlyList<StudentSelfSkillEvaluation> Skills,
    IReadOnlyList<StudentSelfNextStep> NextSteps,
    IReadOnlyList<AnalyticsStudentAssessmentEvaluationItem> Assessments,
    IReadOnlyList<AnalyticsTermEvaluationItem> Terms);


public sealed record StudentSelfEvaluationReportPage(
    StudentSelfEvaluationPage Source,
    Guid? SelectedTermId,
    string? SelectedTermName,
    AnalyticsTermEvaluationItem? SelectedTerm,
    IReadOnlyList<AnalyticsStudentAssessmentEvaluationItem> Assessments,
    DateTime GeneratedAtUtc);
