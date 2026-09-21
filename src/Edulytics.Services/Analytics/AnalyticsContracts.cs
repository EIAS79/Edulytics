using Edulytics.Core.Enums;

namespace Edulytics.Services.Analytics;

public enum AnalyticsErrorCode
{
    AccessDenied,
    SchoolNotActive,
    RecalculationRequiresSchoolAdmin,
    InvalidSourceData,
    PersistenceError
}

public sealed record AnalyticsCommandResult(
    bool Succeeded,
    AnalyticsErrorCode? Error)
{
    public static AnalyticsCommandResult Success() =>
        new(true, null);

    public static AnalyticsCommandResult Failure(
        AnalyticsErrorCode error) =>
        new(false, error);
}

public sealed record AnalyticsQueryResult<T>(
    T? Value,
    AnalyticsErrorCode? Error)
    where T : class
{
    public static AnalyticsQueryResult<T> Success(T value) =>
        new(value, null);

    public static AnalyticsQueryResult<T> Failure(
        AnalyticsErrorCode error) =>
        new(null, error);
}

public sealed record AnalyticsFilterItem(
    Guid Id,
    string Name);

public sealed record AnalyticsOutcomeItem(
    Guid AcademicYearId,
    string AcademicYearName,
    Guid ClassGroupId,
    string ClassName,
    Guid SubjectId,
    string SubjectName,
    Guid LearningOutcomeId,
    string OutcomeCode,
    string OutcomeDescription,
    decimal MasteryPercentage,
    int StudentCount,
    int AtRiskStudentCount,
    int EvidenceCount,
    MasteryBand Band);

public sealed record AnalyticsLessonItem(
    Guid AcademicYearId,
    string AcademicYearName,
    Guid ClassGroupId,
    string ClassName,
    Guid SubjectId,
    string SubjectName,
    Guid LessonId,
    string LessonTitle,
    string UnitKey,
    string UnitTitle,
    decimal MasteryPercentage,
    int StudentCount,
    int AtRiskStudentCount,
    int EvidenceCount,
    int AssessmentCount,
    MasteryBand Band);

public sealed record AnalyticsTopicItem(
    Guid AcademicYearId,
    string AcademicYearName,
    Guid ClassGroupId,
    string ClassName,
    Guid SubjectId,
    string SubjectName,
    Guid TopicId,
    string TopicName,
    decimal MasteryPercentage,
    int OutcomeCount,
    int WeakOutcomeCount,
    int StudentCount,
    MasteryBand Band);

public sealed record AnalyticsTrendItem(
    Guid AcademicYearId,
    string AcademicYearName,
    Guid ClassGroupId,
    string ClassName,
    Guid SubjectId,
    string SubjectName,
    Guid AssessmentId,
    string AssessmentTitle,
    DateOnly AssessmentDate,
    decimal AveragePercentage,
    int StudentCount,
    int AtRiskStudentCount,
    MasteryBand Band);

public sealed record AnalyticsRiskStudentItem(
    Guid StudentProfileId,
    string StudentNumber,
    string DisplayName,
    Guid AcademicYearId,
    string AcademicYearName,
    Guid ClassGroupId,
    string ClassName,
    decimal MasteryPercentage,
    int CriticalOutcomeCount,
    MasteryBand Band);

public sealed record AnalyticsStudentLessonItem(
    Guid LessonId,
    string LessonTitle,
    string UnitTitle,
    decimal MasteryPercentage,
    int EvidenceCount,
    int AssessmentCount,
    MasteryBand Band);

public sealed record AnalyticsStudentOutcomeItem(
    Guid LearningOutcomeId,
    string OutcomeCode,
    string OutcomeDescription,
    decimal MasteryPercentage,
    int EvidenceCount,
    MasteryBand Band);

public sealed record AnalyticsStudentReport(
    Guid StudentProfileId,
    string StudentNumber,
    string DisplayName,
    Guid AcademicYearId,
    string AcademicYearName,
    Guid ClassGroupId,
    string ClassName,
    Guid SubjectId,
    string SubjectName,
    decimal? OfficialOutcomeMasteryPercentage,
    decimal? AssessmentLessonMasteryPercentage,
    IReadOnlyList<AnalyticsStudentLessonItem> Lessons,
    IReadOnlyList<AnalyticsStudentOutcomeItem> Outcomes);

public sealed record AnalyticsDashboard(
    bool HasData,
    bool IsStale,
    bool CanRecalculate,
    DateTime? CalculatedAtUtc,
    decimal OverallMasteryPercentage,
    int StudentsWithEvidence,
    int AtRiskStudents,
    int CriticalOutcomeCount,
    int WeakTopicCount,
    Guid? SelectedAcademicYearId,
    Guid? SelectedClassGroupId,
    Guid? SelectedSubjectId,
    IReadOnlyList<AnalyticsFilterItem> AcademicYears,
    IReadOnlyList<AnalyticsFilterItem> ClassGroups,
    IReadOnlyList<AnalyticsFilterItem> Subjects,
    IReadOnlyList<AnalyticsOutcomeItem> Outcomes,
    IReadOnlyList<AnalyticsTopicItem> Topics,
    IReadOnlyList<AnalyticsTrendItem> Trends,
    IReadOnlyList<AnalyticsRiskStudentItem> RiskStudents)
{
    public IReadOnlyList<AnalyticsLessonItem> Lessons { get; init; } = [];
    public int WeakLessonCount => Lessons.Count(x => x.MasteryPercentage < 60m);
}
