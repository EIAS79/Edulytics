using Edulytics.Core.AssessmentIntelligence;
using Edulytics.Core.Entities;

namespace Edulytics.Core.MathematicsGeneration;

public enum MathematicsGeneratorFamily
{
    IntegerComputation = 1,
    OneStepEquation = 2,
    FractionOfQuantity = 3,
    PercentageOfQuantity = 4,
    UnitRateWordProblem = 5,
    CurriculumContextCheck = 6
}

/// <summary>
/// Curriculum-neutral Mathematics skills understood by generation providers.
/// Official curriculum outcome codes map into these skills; generators never
/// need to understand a framework-specific code directly.
/// </summary>
public enum CanonicalMathematicsSkill
{
    WholeNumberAdditionAndSubtraction = 1,
    WholeNumberAddition = 2,
    WholeNumberSubtraction = 3,
    WholeNumberMultiplication = 4,
    WholeNumberDivision = 5,
    OneStepLinearEquation = 6,
    FractionOfQuantity = 7,
    PercentageOfQuantity = 8,
    UnitRateAndProportion = 9
}

public sealed record MathematicsOutcomeGenerationProfile(
    Guid LearningOutcomeId,
    string OutcomeCode,
    IReadOnlyList<MathematicsGeneratorFamily> AllowedFamilies)
{
    public IReadOnlyList<CanonicalMathematicsSkill> CanonicalSkills { get; init; } = [];

    /// <summary>
    /// Optional curriculum-neutral ceiling for direct whole-number computation.
    /// For wording such as "within 20" the generator must keep operands/results
    /// inside this bound. Null means the Outcome itself did not state a numeric
    /// ceiling; the registered curriculum-level policy may still apply.
    /// </summary>
    public int? IntegerComputationMaximum { get; init; }

    /// <summary>
    /// Sanitized curriculum/outcome context used only by the local contextual
    /// Mathematics provider. It never upgrades contextual generation to native
    /// verified status and it is never treated as an official curriculum code.
    /// </summary>
    public string? GenerationContext { get; init; }

    /// <summary>
    /// True only when the outcome is served by the deterministic curriculum-
    /// contextual fallback rather than a reviewed native mathematical family.
    /// </summary>
    public bool IsContextualAssisted { get; init; }
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
