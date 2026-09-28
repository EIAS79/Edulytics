using Edulytics.Core.AdaptivePractice;
using Edulytics.Core.Enums;
using Edulytics.Core.Interfaces;

namespace Edulytics.Core.Entities;

public sealed class AdaptivePracticeSession : ISchoolScoped
{
    public Guid Id { get; set; }
    public Guid SchoolId { get; set; }
    public Guid StudentProfileId { get; set; }
    public Guid CurriculumAdoptionId { get; set; }
    public string CurriculumLevelKey { get; set; } = string.Empty;
    public Guid CurriculumPedagogicalLessonId { get; set; }
    public string PrimarySkillId { get; set; } = string.Empty;
    public AdaptivePracticePurpose Purpose { get; set; }
    public AdaptivePracticeSessionStatus Status { get; set; }
    public string EngineVersion { get; set; } = string.Empty;
    public string PolicyVersion { get; set; } = string.Empty;
    public string CapabilityVersion { get; set; } = string.Empty;
    public string FeatureFlagSnapshotJson { get; set; } = "{}";
    public int TargetQuestionCount { get; set; }
    public int CurrentSequence { get; set; }
    public DateTime StartedAtUtc { get; set; }
    public DateTime? CompletedAtUtc { get; set; }
    public string? StopReason { get; set; }
    public byte[] RowVersion { get; set; } = [];
}

public sealed class AdaptiveDecisionSnapshot : ISchoolScoped
{
    public Guid Id { get; set; }
    public Guid SchoolId { get; set; }
    public Guid SessionId { get; set; }
    public int Sequence { get; set; }
    public string EngineVersion { get; set; } = string.Empty;
    public string PolicyVersion { get; set; } = string.Empty;
    public decimal SkillMasteryBefore { get; set; }
    public decimal PrerequisiteMasteryBefore { get; set; }
    public string RepresentationFluencyJson { get; set; } = "[]";
    public string ActiveMisconceptionsJson { get; set; } = "[]";
    public int CurrentComplexity { get; set; }
    public int TargetComplexity { get; set; }
    public string SelectedFamily { get; set; } = string.Empty;
    public string? SelectedRepresentation { get; set; }
    public string? MisconceptionFocusId { get; set; }
    public string FreshnessConstraintsJson { get; set; } = "{}";
    public string DecisionReasonCode { get; set; } = string.Empty;
    public string DecisionTraceJson { get; set; } = "{}";
    public DateTime CreatedAtUtc { get; set; }
}

public sealed class AdaptivePracticeTurn : ISchoolScoped
{
    public Guid Id { get; set; }
    public Guid SchoolId { get; set; }
    public Guid SessionId { get; set; }
    public int Sequence { get; set; }
    public Guid AssessmentItemId { get; set; }
    public Guid DecisionSnapshotId { get; set; }
    public string SkillId { get; set; } = string.Empty;
    public string QuestionFamily { get; set; } = string.Empty;
    public string? Representation { get; set; }
    public int MathematicalComplexityScore { get; set; }
    public AssessmentItemDifficulty UiDifficultyBand { get; set; }
    public string? MisconceptionFocusId { get; set; }
    public DateTime PresentedAtUtc { get; set; }
    public DateTime? AnsweredAtUtc { get; set; }
    public string? SubmittedAnswer { get; set; }
    public bool? IsCorrect { get; set; }
    public decimal? Score { get; set; }
    public string? Feedback { get; set; }
    public long? ResponseDurationMs { get; set; }
    public string ExposureFingerprint { get; set; } = string.Empty;
    public string SemanticIdentityKey { get; set; } = string.Empty;
    public byte[] RowVersion { get; set; } = [];
}

public sealed class StudentMisconceptionState : ISchoolScoped
{
    public Guid Id { get; set; }
    public Guid SchoolId { get; set; }
    public Guid StudentProfileId { get; set; }
    public Guid CurriculumAdoptionId { get; set; }
    public string SkillId { get; set; } = string.Empty;
    public string MisconceptionId { get; set; } = string.Empty;
    public string? QuestionFamily { get; set; }
    public AdaptiveMisconceptionStatus Status { get; set; }
    public decimal Confidence { get; set; }
    public int ObservationCount { get; set; }
    public DateTime FirstObservedAtUtc { get; set; }
    public DateTime LastObservedAtUtc { get; set; }
    public DateTime? LastRemediationAtUtc { get; set; }
    public DateTime? ResolvedAtUtc { get; set; }
    public string EngineVersion { get; set; } = string.Empty;
    public byte[] RowVersion { get; set; } = [];
}

public sealed class StudentRepresentationFluencyState : ISchoolScoped
{
    public Guid Id { get; set; }
    public Guid SchoolId { get; set; }
    public Guid StudentProfileId { get; set; }
    public string SkillId { get; set; } = string.Empty;
    public string Representation { get; set; } = string.Empty;
    public int EvidenceCount { get; set; }
    public int SuccessCount { get; set; }
    public decimal WeightedFluency { get; set; }
    public DateTime LatestEvidenceAtUtc { get; set; }
    public string EngineVersion { get; set; } = string.Empty;
    public byte[] RowVersion { get; set; } = [];
}
