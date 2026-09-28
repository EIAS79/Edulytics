namespace Edulytics.Core.AdaptivePractice;

public static class AdaptivePracticeDecisionReasonCodes
{
    public const string SessionBaseline = "SESSION_BASELINE";
    public const string RemediationLockActive = "REMEDIATION_LOCK_ACTIVE";
    public const string MisconceptionRemediation = "MISCONCEPTION_REMEDIATION";
    public const string MisconceptionConfirmationRequired =
        "MISCONCEPTION_CONFIRMATION_REQUIRED";
    public const string PrerequisiteRecovery = "PREREQUISITE_RECOVERY";
    public const string RepresentationRecovery = "REPRESENTATION_RECOVERY";
    public const string UnclassifiedRecovery = "UNCLASSIFIED_RECOVERY";
    public const string ConfirmationPassed = "CONFIRMATION_PASSED";
    public const string ProgressionEligible = "PROGRESSION_ELIGIBLE";
    public const string ProgressionBlockedRepeatedFailure =
        "PROGRESSION_BLOCKED_REPEATED_FAILURE";
    public const string ComplexityReduce = "COMPLEXITY_REDUCE";
    public const string ComplexityConsolidate = "COMPLEXITY_CONSOLIDATE";
    public const string ComplexityProgress = "COMPLEXITY_PROGRESS";
}

public sealed record AdaptivePracticeResponseObservation(
    int Sequence,
    bool IsCorrect,
    int ComplexityScore,
    string QuestionFamily,
    string? Representation = null,
    string? MisconceptionId = null,
    bool IsIndependentConfirmation = false);

public sealed record AdaptivePracticeMisconceptionEvidence(
    string MisconceptionId,
    int ObservationCount,
    bool IsBlocking,
    string? QuestionFamily = null,
    int LastObservedSequence = 0);

public sealed record AdaptivePracticeRepresentationFluency(
    string Representation,
    decimal Fluency,
    IReadOnlyList<string> QuestionFamilies);

public sealed record AdaptivePracticeRemediationState(
    bool IsLocked,
    bool ConfirmationRequired,
    int LockComplexityScore,
    string? BlockingMisconceptionId = null,
    string? PreferredQuestionFamily = null,
    string? PreferredRepresentation = null)
{
    public static AdaptivePracticeRemediationState None { get; } =
        new(false, false, 0);
}

public sealed record AdaptivePracticeLearningState(
    string SkillId,
    decimal SkillMastery,
    decimal PrerequisiteMastery,
    int CurrentComplexityScore,
    int RecentSuccessfulItems,
    IReadOnlyList<AdaptivePracticeResponseObservation> RecentResponses,
    IReadOnlyList<AdaptivePracticeMisconceptionEvidence> Misconceptions,
    IReadOnlyList<AdaptivePracticeRepresentationFluency> RepresentationFluency,
    IReadOnlyList<string> AllowedQuestionFamilies,
    AdaptivePracticeRemediationState Remediation);

public sealed record AdaptiveNextItemDecision(
    string TargetSkillId,
    int TargetComplexityScore,
    string TargetQuestionFamily,
    string? TargetRepresentation,
    string? MisconceptionFocusId,
    string ReasonCode,
    bool RequiresFreshExposure,
    bool RemediationLockActive,
    bool ConfirmationRequired,
    bool ProgressionEligible,
    string EngineVersion,
    string PolicyVersion);
