using Edulytics.Core.Analytics;
using Edulytics.Core.Enums;

namespace Edulytics.Core.AssessmentIntelligence;

public enum AssessmentPurpose
{
    Practice = 1,
    TeacherAssessment = 2,
    StudentPersonalTest = 3,
    Diagnostic = 4,
    EquivalentReassessment = 5
}

public enum AssessmentQuestionFamily
{
    DirectComputation = 1,
    StructuredMethod = 2,
    AppliedProblem = 3,
    MathematicalReasoning = 4
}

public sealed record AssessmentDifficultyPolicy(
    int EasyPercent,
    int MediumPercent,
    int ChallengingPercent)
{
    public static AssessmentDifficultyPolicy Balanced { get; } = new(30, 50, 20);
    public static AssessmentDifficultyPolicy Supportive { get; } = new(50, 40, 10);
    public static AssessmentDifficultyPolicy Stretch { get; } = new(15, 45, 40);
}

public sealed record AssessmentBlueprintRequest(
    Guid SchoolId,
    Guid CurriculumAdoptionId,
    string CurriculumLevelKey,
    Guid? CurriculumTopicId,
    Guid? CurriculumPedagogicalLessonId,
    IReadOnlyList<Guid> LearningOutcomeIds,
    StudentLearningProfile? StudentProfile,
    AssessmentPurpose Purpose,
    int QuestionCount,
    AssessmentDifficultyPolicy DifficultyPolicy,
    IReadOnlyCollection<string> ExcludedExposureFingerprints);

public sealed record OutcomeBlueprintAllocation(
    Guid LearningOutcomeId,
    int ItemCount,
    decimal PriorityScore,
    string PriorityReason);

public sealed record DifficultyBlueprintAllocation(
    AssessmentItemDifficulty Difficulty,
    int ItemCount);

public sealed record QuestionFamilyBlueprintAllocation(
    AssessmentQuestionFamily Family,
    int ItemCount);

public sealed record ItemTypeBlueprintAllocation(
    AssessmentItemType ItemType,
    int ItemCount);

public sealed record OutcomeEvidenceRequirement(
    Guid LearningOutcomeId,
    int RequiredItemCount,
    bool RequiresScoredResponse,
    bool RequiresValidSolution);

public sealed record AssessmentBlueprint(
    Guid SchoolId,
    Guid CurriculumAdoptionId,
    string CurriculumLevelKey,
    Guid? CurriculumTopicId,
    Guid? CurriculumPedagogicalLessonId,
    AssessmentPurpose Purpose,
    int QuestionCount,
    IReadOnlyList<OutcomeBlueprintAllocation> OutcomeAllocations,
    IReadOnlyList<DifficultyBlueprintAllocation> DifficultyAllocations,
    IReadOnlyList<QuestionFamilyBlueprintAllocation> QuestionFamilyAllocations,
    IReadOnlyList<ItemTypeBlueprintAllocation> ItemTypeAllocations,
    IReadOnlyList<OutcomeEvidenceRequirement> RequiredEvidence,
    IReadOnlyList<string> ExcludedExposureFingerprints,
    string FormulaVersion);
