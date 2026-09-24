using Edulytics.Core.Entities;

namespace Edulytics.Core.Analytics;

public sealed record AnalyticsSourceSnapshot(
    IReadOnlyList<AcademicYear> AcademicYears,
    IReadOnlyList<ClassGroup> ClassGroups,
    IReadOnlyList<Subject> Subjects,
    IReadOnlyList<StudentProfile> StudentProfiles,
    IReadOnlyList<StudentEnrollment> StudentEnrollments,
    IReadOnlyList<TeacherAssignment> TeacherAssignments,
    IReadOnlyList<CurriculumTopic> CurriculumTopics,
    IReadOnlyList<LearningOutcome> LearningOutcomes,
    IReadOnlyList<Assessment> Assessments,
    IReadOnlyList<AssessmentQuestion> AssessmentQuestions,
    IReadOnlyList<QuestionLearningOutcome> OutcomeMappings,
    IReadOnlyList<AssessmentResult> AssessmentResults,
    IReadOnlyList<StudentAnswer> StudentAnswers,
    IReadOnlyList<PracticeAttempt>? PracticeAttempts = null,
    IReadOnlyList<LearningEvidence>? LearningEvidence = null);

public sealed record AnalyticsProjectionSnapshot(
    IReadOnlyList<AcademicYear> AcademicYears,
    IReadOnlyList<ClassGroup> ClassGroups,
    IReadOnlyList<Subject> Subjects,
    IReadOnlyList<StudentProfile> StudentProfiles,
    IReadOnlyList<TeacherAssignment> TeacherAssignments,
    IReadOnlyList<CurriculumTopic> CurriculumTopics,
    IReadOnlyList<LearningOutcome> LearningOutcomes,
    IReadOnlyList<StudentOutcomeMastery> StudentOutcomeMasteries,
    IReadOnlyList<ClassOutcomeSummary> ClassOutcomeSummaries,
    IReadOnlyList<ClassTopicSummary> ClassTopicSummaries,
    IReadOnlyList<ClassAssessmentTrend> ClassAssessmentTrends,
    IReadOnlyList<SchoolAnalyticsSnapshot> SchoolSnapshots)
{
    public IReadOnlyList<Term> Terms { get; init; } = [];
    public IReadOnlyList<StudentEnrollment> StudentEnrollments { get; init; } = [];
    public IReadOnlyList<Assessment> Assessments { get; init; } = [];
    public IReadOnlyList<AssessmentQuestion> AssessmentQuestions { get; init; } = [];
    public IReadOnlyList<QuestionLearningOutcome> OutcomeMappings { get; init; } = [];
    public IReadOnlyList<AssessmentItem> AssessmentItems { get; init; } = [];
    public IReadOnlyList<AssessmentItemOutcome> AssessmentItemOutcomes { get; init; } = [];
    public IReadOnlyList<AssessmentResult> AssessmentResults { get; init; } = [];
    public IReadOnlyList<StudentAnswer> StudentAnswers { get; init; } = [];
    public IReadOnlyList<PracticeAttempt> PracticeAttempts { get; init; } = [];
    public IReadOnlyList<PracticeAttemptItem> PracticeAttemptItems { get; init; } = [];
    public IReadOnlyList<PracticeResponse> PracticeResponses { get; init; } = [];
    public IReadOnlyList<LearningEvidence> LearningEvidence { get; init; } = [];
    public IReadOnlyList<CurriculumPedagogicalLesson> PedagogicalLessons { get; init; } = [];
}

public sealed record AnalyticsProjectionSet(
    IReadOnlyList<StudentOutcomeMastery> StudentOutcomeMasteries,
    IReadOnlyList<ClassOutcomeSummary> ClassOutcomeSummaries,
    IReadOnlyList<ClassTopicSummary> ClassTopicSummaries,
    IReadOnlyList<ClassAssessmentTrend> ClassAssessmentTrends,
    IReadOnlyList<SchoolAnalyticsSnapshot> SchoolSnapshots);

public enum AnalyticsPersistenceError
{
    None = 0,
    Constraint = 1,
    Unknown = 2
}

public sealed record AnalyticsPersistenceResult(
    bool Succeeded,
    AnalyticsPersistenceError Error)
{
    public static AnalyticsPersistenceResult Success() =>
        new(true, AnalyticsPersistenceError.None);

    public static AnalyticsPersistenceResult Failure(
        AnalyticsPersistenceError error) =>
        new(false, error);
}