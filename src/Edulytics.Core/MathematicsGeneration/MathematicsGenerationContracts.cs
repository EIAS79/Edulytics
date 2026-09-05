using Edulytics.Core.AssessmentIntelligence;
using Edulytics.Core.Entities;

namespace Edulytics.Core.MathematicsGeneration;

public enum MathematicsGeneratorFamily
{
    IntegerComputation = 1,
    OneStepEquation = 2,
    FractionOfQuantity = 3,
    PercentageOfQuantity = 4,
    UnitRateWordProblem = 5
}

/// <summary>
/// Curriculum-neutral Mathematics skills understood by generation providers.
/// Official curriculum outcome codes map into these skills; generators never
/// need to understand a framework-specific code directly.
/// </summary>
public enum CanonicalMathematicsSkill
{
    WholeNumberComputation = 1,
    OneStepLinearEquation = 2,
    FractionOfQuantity = 3,
    PercentageOfQuantity = 4,
    UnitRateAndProportion = 5
}

public sealed record MathematicsOutcomeGenerationProfile(
    Guid LearningOutcomeId,
    string OutcomeCode,
    IReadOnlyList<MathematicsGeneratorFamily> AllowedFamilies)
{
    public IReadOnlyList<CanonicalMathematicsSkill> CanonicalSkills { get; init; } = [];
}

public sealed record MathematicsGenerationRequest(
    AssessmentBlueprint Blueprint,
    IReadOnlyList<MathematicsOutcomeGenerationProfile> OutcomeProfiles,
    int Seed = 0);

public sealed record GeneratedMathematicsItem(
    AssessmentItem Item,
    AssessmentItemOutcome OutcomeLink,
    AssessmentQuestionFamily BlueprintFamily,
    string GeneratorVersion);

public sealed record MathematicsGenerationBatch(
    Guid SchoolId,
    Guid CurriculumAdoptionId,
    string CurriculumLevelKey,
    IReadOnlyList<GeneratedMathematicsItem> Items,
    string GeneratorVersion);
