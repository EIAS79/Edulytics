using Edulytics.Core.Enums;

namespace Edulytics.Core.Analytics;

public enum EvaluationEvidenceSource
{
    Assessment = 1,
    Practice = 2
}

public enum EvaluationSkillResolutionKind
{
    ExactSkillContract = 1,
    ReviewedOutcomeMapping = 2,
    OutcomeProxy = 3
}

public enum EvaluationConfidenceBand
{
    Insufficient = 0,
    Limited = 1,
    Moderate = 2,
    Strong = 3,
    VeryStrong = 4
}

public enum EvaluationTrendBand
{
    InsufficientEvidence = 0,
    RapidlyDeclining = 1,
    Declining = 2,
    Stable = 3,
    Improving = 4,
    RapidlyImproving = 5
}

public enum EvaluationRetentionBand
{
    InsufficientEvidence = 0,
    Stable = 1,
    Concern = 2,
    SignificantConcern = 3
}

public enum EvaluationSkillStatus
{
    NotYetAssessed = 0,
    InsufficientEvidence = 1,
    Critical = 2,
    NeedsFocus = 3,
    Developing = 4,
    Secure = 5,
    Strong = 6
}

public enum EvaluationPriority
{
    None = 0,
    Low = 1,
    Medium = 2,
    High = 3,
    Critical = 4
}

public sealed record EvaluationSkillDescriptor(
    string SkillKey,
    string SkillName,
    EvaluationSkillResolutionKind ResolutionKind,
    IReadOnlyList<string> PrerequisiteSkillKeys);

public sealed record EvaluationEvidenceRecord(
    string EvidenceKey,
    Guid SchoolId,
    Guid StudentProfileId,
    Guid AcademicYearId,
    Guid ClassGroupId,
    Guid SubjectId,
    Guid? TermId,
    Guid TopicId,
    Guid LearningOutcomeId,
    string OutcomeCode,
    string OutcomeDescription,
    string SkillKey,
    string SkillName,
    EvaluationSkillResolutionKind SkillResolutionKind,
    Guid? LessonId,
    EvaluationEvidenceSource Source,
    Guid SourceId,
    Guid? SourceInstanceId,
    Guid ItemId,
    string? SourceTitle,
    string? QuestionFamily,
    AssessmentItemDifficulty? Difficulty,
    decimal Score,
    decimal MaxScore,
    decimal MappingWeight,
    bool? IsCorrect,
    DateTime OccurredAtUtc)
{
    public decimal Percentage =>
        MaxScore <= 0m
            ? 0m
            : decimal.Round(
                Score / MaxScore * 100m,
                2,
                MidpointRounding.AwayFromZero);
}

public sealed record EvaluationEvidenceSummary(
    int TotalEvidence,
    int AssessmentEvidence,
    int PracticeEvidence,
    int IndependentAssessmentCount,
    int PracticeSessionCount,
    int DifficultyBandCount,
    DateTime? LatestEvidenceAtUtc);

public sealed record StudentSkillEvaluation(
    Guid AcademicYearId,
    Guid ClassGroupId,
    Guid SubjectId,
    Guid TopicId,
    Guid LearningOutcomeId,
    string OutcomeCode,
    string OutcomeDescription,
    string SkillKey,
    string SkillName,
    EvaluationSkillResolutionKind SkillResolutionKind,
    decimal? CurrentMasteryPercentage,
    decimal? AssessmentMasteryPercentage,
    decimal? PracticeMasteryPercentage,
    decimal? PracticeToAssessmentGapPercentagePoints,
    decimal ConfidencePercentage,
    EvaluationConfidenceBand ConfidenceBand,
    EvaluationTrendBand ShortTermTrend,
    EvaluationTrendBand LongTermTrend,
    EvaluationRetentionBand Retention,
    EvaluationSkillStatus Status,
    decimal GapCriticalityScore,
    EvaluationPriority InterventionPriority,
    IReadOnlyList<string> PrerequisiteSkillKeys,
    IReadOnlyList<string> WeakPrerequisiteSkillKeys,
    EvaluationEvidenceSummary Evidence,
    string FormulaVersion,
    bool TargetedCheckAvailable = false);

public sealed record StudentSubjectEvaluation(
    Guid SchoolId,
    Guid StudentProfileId,
    string StudentNumber,
    string DisplayName,
    Guid AcademicYearId,
    string AcademicYearName,
    Guid ClassGroupId,
    string ClassName,
    Guid SubjectId,
    string SubjectName,
    decimal? CurrentMasteryPercentage,
    decimal? AssessmentMasteryPercentage,
    decimal? PracticeMasteryPercentage,
    decimal? PracticeToAssessmentGapPercentagePoints,
    decimal CurriculumCoveragePercentage,
    int ExpectedSkillCount,
    int EvaluatedSkillCount,
    decimal ConfidencePercentage,
    EvaluationConfidenceBand ConfidenceBand,
    EvaluationTrendBand ShortTermTrend,
    EvaluationTrendBand LongTermTrend,
    int SecureSkillCount,
    int DevelopingSkillCount,
    int NeedsFocusSkillCount,
    int CriticalSkillCount,
    int RetentionConcernCount,
    IReadOnlyList<StudentSkillEvaluation> Skills,
    string FormulaVersion);
