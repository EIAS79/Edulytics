using Edulytics.Core.Analytics;
using Edulytics.Core.AssessmentIntelligence;
using Edulytics.Core.Enums;

namespace Edulytics.Core.Recovery;

public enum RecoveryOutcome
{
    StillWeak = 1,
    Improved = 2,
    Mastered = 3
}

public enum ReassessmentCognitiveDemand
{
    Standard = 1,
    Stretch = 2,
    Challenge = 3
}

/// <summary>
/// Reconstructable mathematical exposure signature used by Stage 21 to prove
/// that reassessment freshness is mathematical rather than wording-only.
/// </summary>
public sealed record ReassessmentMathematicalSignature(
    string ExposureFingerprint,
    string QuestionFamily,
    string CoefficientSignature,
    string Representation,
    string Strategy,
    string Context,
    string MisconceptionTrap,
    ReassessmentCognitiveDemand CognitiveDemand);

public sealed record WeaknessRecoveryRequest(
    Guid SchoolId,
    Guid CurriculumAdoptionId,
    string CurriculumLevelKey,
    Guid? CurriculumTopicId,
    Guid CurriculumPedagogicalLessonId,
    StudentLearningProfile StudentProfile,
    Guid LearningOutcomeId,
    IReadOnlyCollection<string> PreviousExposureFingerprints,
    IReadOnlyCollection<string> PreviousPrompts,
    AssessmentDifficultyPolicy ComparableDifficultyPolicy,
    int PracticeQuestionCount,
    int ReassessmentQuestionCount,
    IReadOnlyCollection<ReassessmentMathematicalSignature>? PreviousMathematicalSignatures = null);

public sealed record WeaknessRecoveryPlan(
    Guid SchoolId,
    Guid StudentProfileId,
    Guid CurriculumAdoptionId,
    string CurriculumLevelKey,
    Guid LearningOutcomeId,
    Guid CurriculumPedagogicalLessonId,
    decimal BaselineMastery,
    MasteryBand BaselineBand,
    AssessmentBlueprint TargetedPracticeBlueprint,
    AssessmentBlueprint EquivalentReassessmentBlueprint,
    IReadOnlyList<string> ExcludedExposureFingerprints,
    IReadOnlyList<string> PreviousPromptShapes,
    bool ExcludePreviouslySeenQuestions,
    string FormulaVersion,
    IReadOnlyList<ReassessmentMathematicalSignature>? PreviousMathematicalSignatures = null);

public sealed record RecoveryEvaluation(
    RecoveryOutcome Outcome,
    decimal BeforeMastery,
    decimal AfterMastery,
    decimal Delta,
    string FormulaVersion);
