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
    int ReassessmentQuestionCount);

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
    string FormulaVersion);

public sealed record RecoveryEvaluation(
    RecoveryOutcome Outcome,
    decimal BeforeMastery,
    decimal AfterMastery,
    decimal Delta,
    string FormulaVersion);
