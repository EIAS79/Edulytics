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
    int Sequence);

public sealed record AdaptiveAssessmentRequest(
    Guid SchoolId,
    Guid CurriculumAdoptionId,
    string CurriculumLevelKey,
    IReadOnlyList<Guid> LearningOutcomeIds,
    StudentLearningProfile? StudentProfile,
    AssessmentPurpose Purpose,
    IReadOnlyList<AdaptiveResponseEvidence> PreviousResponses);

public sealed record AdaptiveAssessmentDecision(
    AdaptiveAssessmentMode Mode,
    Guid TargetLearningOutcomeId,
    AssessmentItemDifficulty NextDifficulty,
    decimal EvidenceCreditMultiplier,
    bool DifficultyReduced,
    bool RequiresFreshExposure,
    string Reason,
    string FormulaVersion);
