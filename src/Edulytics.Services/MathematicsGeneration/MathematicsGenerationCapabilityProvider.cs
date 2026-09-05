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
/// A mapped outcome is eligible only when every required canonical skill is
/// supported. Partial coverage is deliberately rejected fail-closed.
/// </summary>
public sealed class NativeMathematicsGenerationCapabilityProvider
    : IMathematicsGenerationCapabilityProvider
{
    private static readonly IReadOnlyDictionary<CanonicalMathematicsSkill, MathematicsGeneratorFamily> Families =
        new Dictionary<CanonicalMathematicsSkill, MathematicsGeneratorFamily>
        {
            [CanonicalMathematicsSkill.WholeNumberAdditionAndSubtraction] = MathematicsGeneratorFamily.IntegerComputation,
            [CanonicalMathematicsSkill.WholeNumberAddition] = MathematicsGeneratorFamily.IntegerComputation,
            [CanonicalMathematicsSkill.WholeNumberSubtraction] = MathematicsGeneratorFamily.IntegerComputation,
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

        if (skills.Count == 0 || skills.Any(skill => !Supports(skill)))
            return [];

        return skills
            .Select(skill => Families[skill])
            .Distinct()
            .OrderBy(family => family)
            .ToArray();
    }
}
