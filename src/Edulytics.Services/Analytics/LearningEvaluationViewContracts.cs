using Edulytics.Core.Analytics;

namespace Edulytics.Services.Analytics;

public sealed record AnalyticsStudentAssessmentEvaluationItem(
    Guid AssessmentId,
    string Title,
    DateOnly AssessmentDate,
    string? TermName,
    decimal Percentage,
    decimal? OverallChangePercentagePoints,
    decimal? ComparableSkillChangePercentagePoints,
    int ComparableSkillCount);

public sealed record AnalyticsTermEvaluationItem(
    Guid TermId,
    string TermName,
    DateOnly StartsOn,
    DateOnly EndsOn,
    decimal? AssessmentMasteryPercentage,
    decimal? PracticeMasteryPercentage,
    decimal? CombinedMasteryPercentage,
    decimal? AssessmentChangePercentagePoints,
    decimal? ComparableSkillGrowthPercentagePoints,
    int ComparableSkillCount,
    int EvidenceCount);

public sealed record AnalyticsPracticeEvaluationSummary(
    int SessionCount,
    int EvidenceCount,
    int ActiveDayCount,
    int SkillCount,
    decimal? MasteryPercentage,
    EvaluationTrendBand Trend,
    DateTime? LatestPracticeAtUtc);

public sealed record AnalyticsInterventionRecommendation(
    string SkillKey,
    string SkillName,
    EvaluationPriority Priority,
    decimal? CurrentMasteryPercentage,
    IReadOnlyList<string> RecommendedPrerequisitePath,
    string Reason);

public sealed record AnalyticsStudentEvaluationTopic(
    Guid TopicId,
    string TopicName,
    decimal? CurrentMasteryPercentage,
    decimal? AssessmentMasteryPercentage,
    decimal? PracticeMasteryPercentage,
    decimal CoveragePercentage,
    int SkillCount,
    int EvaluatedSkillCount,
    int CriticalSkillCount,
    int NeedsFocusSkillCount,
    EvaluationTrendBand Trend);

public sealed record AnalyticsStudentEvaluationPage(
    StudentSubjectEvaluation Evaluation,
    IReadOnlyList<AnalyticsStudentEvaluationTopic> Topics,
    IReadOnlyList<AnalyticsStudentAssessmentEvaluationItem> Assessments,
    IReadOnlyList<AnalyticsTermEvaluationItem> Terms,
    AnalyticsPracticeEvaluationSummary Practice,
    IReadOnlyList<AnalyticsInterventionRecommendation> Recommendations,
    IReadOnlyList<EvaluationEvidenceRecord> Evidence);

public sealed record AnalyticsStudentEvaluationRow(
    Guid StudentProfileId,
    string StudentNumber,
    string DisplayName,
    decimal? CurrentMasteryPercentage,
    decimal? AssessmentMasteryPercentage,
    decimal? PracticeMasteryPercentage,
    decimal? TransferGapPercentagePoints,
    decimal CoveragePercentage,
    decimal ConfidencePercentage,
    EvaluationConfidenceBand ConfidenceBand,
    EvaluationTrendBand ShortTermTrend,
    EvaluationTrendBand LongTermTrend,
    int SecureSkillCount,
    int NeedsFocusSkillCount,
    int CriticalSkillCount,
    int RetentionConcernCount,
    EvaluationPriority HighestPriority);

public sealed record AnalyticsEvaluationDistribution(
    int TotalStudents,
    int StrongOrSecure,
    int Developing,
    int NeedsFocus,
    int Critical,
    int InsufficientEvidence,
    int Improving,
    int Stable,
    int Declining);

public sealed record AnalyticsStudentsEvaluationPage(
    Guid AcademicYearId,
    string AcademicYearName,
    Guid ClassGroupId,
    string ClassName,
    Guid SubjectId,
    string SubjectName,
    AnalyticsEvaluationDistribution Distribution,
    IReadOnlyList<AnalyticsStudentEvaluationRow> Students);

public sealed record AnalyticsSkillCohortStudent(
    Guid StudentProfileId,
    string StudentNumber,
    string DisplayName,
    decimal? MasteryPercentage,
    decimal? AssessmentMasteryPercentage,
    decimal? PracticeMasteryPercentage,
    EvaluationSkillStatus Status,
    EvaluationTrendBand Trend,
    EvaluationConfidenceBand Confidence,
    EvaluationPriority Priority);

public sealed record AnalyticsSkillCohortEvaluation(
    Guid TopicId,
    string TopicName,
    string SkillKey,
    string SkillName,
    decimal? ClassMasteryPercentage,
    decimal? AssessmentMasteryPercentage,
    decimal? PracticeMasteryPercentage,
    decimal CoveragePercentage,
    int StudentCount,
    int EvaluatedStudentCount,
    int CriticalStudentCount,
    int NeedsFocusStudentCount,
    EvaluationTrendBand Trend,
    EvaluationPriority HighestPriority,
    IReadOnlyList<AnalyticsSkillCohortStudent> Students);

public sealed record AnalyticsTopicEvaluation(
    Guid TopicId,
    string TopicName,
    decimal? ClassMasteryPercentage,
    decimal? AssessmentMasteryPercentage,
    decimal? PracticeMasteryPercentage,
    decimal CoveragePercentage,
    int StudentCount,
    int AffectedStudentCount,
    int CriticalSkillCount,
    EvaluationTrendBand Trend,
    IReadOnlyList<AnalyticsSkillCohortEvaluation> Skills);

public sealed record AnalyticsTopicSkillEvaluationPage(
    Guid AcademicYearId,
    string AcademicYearName,
    Guid ClassGroupId,
    string ClassName,
    Guid SubjectId,
    string SubjectName,
    IReadOnlyList<AnalyticsTopicEvaluation> Topics);

public sealed record AnalyticsSupervisorClassEvaluationRow(
    Guid ClassGroupId,
    string ClassName,
    int StudentCount,
    int EvaluatedStudentCount,
    decimal? CurrentMasteryPercentage,
    decimal? AssessmentMasteryPercentage,
    decimal? PracticeMasteryPercentage,
    decimal AverageCoveragePercentage,
    int ImprovingStudentCount,
    int DecliningStudentCount,
    int CriticalStudentCount,
    int CriticalSkillCount);

public sealed record AnalyticsSupervisorSkillGap(
    string SkillKey,
    string SkillName,
    int AffectedStudentCount,
    int EvaluatedStudentCount,
    int AffectedClassCount,
    decimal? AverageMasteryPercentage,
    EvaluationPriority HighestPriority);

public sealed record AnalyticsSupervisorSubjectOverviewPage(
    Guid AcademicYearId,
    string AcademicYearName,
    Guid SubjectId,
    string SubjectName,
    int ClassCount,
    int StudentCount,
    int EvaluatedStudentCount,
    decimal? CurrentMasteryPercentage,
    decimal? AssessmentMasteryPercentage,
    decimal? PracticeMasteryPercentage,
    decimal AverageCoveragePercentage,
    int ImprovingStudentCount,
    int DecliningStudentCount,
    int CriticalStudentCount,
    IReadOnlyList<AnalyticsSupervisorClassEvaluationRow> Classes,
    IReadOnlyList<AnalyticsSupervisorSkillGap> PriorityGaps);
