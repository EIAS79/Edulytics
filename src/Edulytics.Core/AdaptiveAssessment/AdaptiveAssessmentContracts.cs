using Edulytics.Core.Analytics;
using Edulytics.Core.AssessmentIntelligence;
using Edulytics.Core.Enums;

namespace Edulytics.Core.AdaptiveAssessment;

public enum AdaptiveAssessmentMode
{
    Diagnostic = 1,
    Adaptive = 2
}

public sealed record AdaptiveResponseEvidence(
    Guid LearningOutcomeId,
    AssessmentItemDifficulty Difficulty,
    bool IsCorrect,
    decimal ScorePercentage,
    int Sequence,
    int? MathematicsComplexityScore = null,
    string? QuestionFamily = null,
    string? Representation = null,
    string? MisconceptionId = null);

public sealed record AdaptiveMisconceptionEvidence(
    string MisconceptionId,
    int Count,
    int LastObservedSequence,
    string? QuestionFamily = null);

public sealed record AdaptiveRepresentationFluency(
    string Representation,
    decimal Fluency,
    IReadOnlyList<string> QuestionFamilies);

/// <summary>
/// Audited mathematics-specific adaptive state for one exact LearningOutcome.
/// Values are ratios in [0,1]. Prerequisite mastery is supplied by the
/// curriculum/learning-state layer because Stage 20 must not invent a
/// prerequisite graph that is not yet canonically mapped.
/// </summary>
public sealed record AdaptiveMathematicsSkillState(
    Guid LearningOutcomeId,
    string OutcomeCode,
    string SkillId,
    decimal SkillMastery,
    decimal PrerequisiteMastery,
    int CurrentComplexityScore,
    int RecentSuccessfulItems,
    IReadOnlyList<AdaptiveMisconceptionEvidence> MisconceptionHistory,
    IReadOnlyList<AdaptiveRepresentationFluency> RepresentationFluency);

public sealed record AdaptiveAssessmentRequest(
    Guid SchoolId,
    Guid CurriculumAdoptionId,
    string CurriculumLevelKey,
    IReadOnlyList<Guid> LearningOutcomeIds,
    StudentLearningProfile? StudentProfile,
    AssessmentPurpose Purpose,
    IReadOnlyList<AdaptiveResponseEvidence> PreviousResponses,
    IReadOnlyList<AdaptiveMathematicsSkillState>? MathematicsSkillStates = null);

public sealed record AdaptiveAssessmentDecision(
    AdaptiveAssessmentMode Mode,
    Guid TargetLearningOutcomeId,
    AssessmentItemDifficulty NextDifficulty,
    decimal EvidenceCreditMultiplier,
    bool DifficultyReduced,
    bool RequiresFreshExposure,
    string Reason,
    string FormulaVersion,
    bool MathematicsAware = false,
    string? TargetSkillId = null,
    int? TargetComplexityScore = null,
    string? TargetQuestionFamily = null,
    string? TargetRepresentation = null,
    string? MisconceptionFocusId = null);
