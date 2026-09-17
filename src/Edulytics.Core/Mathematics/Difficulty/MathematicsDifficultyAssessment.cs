namespace Edulytics.Core.Mathematics.Difficulty;

public enum MathematicsDifficultyBand
{
    Easy = 1,
    Medium = 2,
    Challenging = 3
}

public sealed record MathematicsDifficultyAssessment(
    MathematicsDifficultyBand Band,
    int ComplexityScore,
    MathematicsComplexityVector Complexity,
    IReadOnlyList<string> Reasons);

public sealed record MathematicsAdaptiveLearnerState(
    decimal SkillMastery,
    decimal PrerequisiteMastery,
    decimal RepresentationFluency,
    int RecentMisconceptionCount,
    int RecentSuccessfulItems);

public sealed record MathematicsAdaptiveRecommendation(
    MathematicsDifficultyBand RecommendedBand,
    int TargetComplexityScore,
    IReadOnlyList<string> Reasons);
