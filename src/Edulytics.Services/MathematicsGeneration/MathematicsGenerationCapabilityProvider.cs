using Edulytics.Core.MathematicsGeneration;

namespace Edulytics.Services.MathematicsGeneration;

/// <summary>
/// Describes which curriculum-neutral skills a generation provider can serve.
/// This is deliberately independent of official curriculum codes.
/// </summary>
public interface IMathematicsGenerationCapabilityProvider
{
    string ProviderKey { get; }

    bool Supports(CanonicalMathematicsSkill skill);

    IReadOnlyList<MathematicsGeneratorFamily> ResolveFamilies(
        IReadOnlyCollection<CanonicalMathematicsSkill> skills);
}

/// <summary>
/// Capability contract for Edulytics' deterministic native Mathematics engine.
/// Additional providers can implement the same contract without changing
/// curriculum adapters or UI capability rules.
/// </summary>
public sealed class NativeMathematicsGenerationCapabilityProvider
    : IMathematicsGenerationCapabilityProvider
{
    private static readonly IReadOnlyDictionary<CanonicalMathematicsSkill, MathematicsGeneratorFamily> Families =
        new Dictionary<CanonicalMathematicsSkill, MathematicsGeneratorFamily>
        {
            [CanonicalMathematicsSkill.WholeNumberComputation] = MathematicsGeneratorFamily.IntegerComputation,
            [CanonicalMathematicsSkill.OneStepLinearEquation] = MathematicsGeneratorFamily.OneStepEquation,
            [CanonicalMathematicsSkill.FractionOfQuantity] = MathematicsGeneratorFamily.FractionOfQuantity,
            [CanonicalMathematicsSkill.PercentageOfQuantity] = MathematicsGeneratorFamily.PercentageOfQuantity,
            [CanonicalMathematicsSkill.UnitRateAndProportion] = MathematicsGeneratorFamily.UnitRateWordProblem
        };

    public string ProviderKey => "edulytics-native-mathematics";

    public bool Supports(CanonicalMathematicsSkill skill) => Families.ContainsKey(skill);

    public IReadOnlyList<MathematicsGeneratorFamily> ResolveFamilies(
        IReadOnlyCollection<CanonicalMathematicsSkill> skills)
    {
        ArgumentNullException.ThrowIfNull(skills);

        return skills
            .Where(Supports)
            .Select(skill => Families[skill])
            .Distinct()
            .OrderBy(family => family)
            .ToArray();
    }
}
