using Edulytics.Core.MathematicsGeneration;

namespace Edulytics.Services.Assessments;

/// <summary>
/// Converts framework-specific outcome metadata into curriculum-neutral skills.
/// Mapping is intentionally conservative: a capability is emitted only when the
/// outcome wording matches the semantics of a reviewed native generator. Broader
/// or unsupported operations remain visible as canonical skills and therefore
/// fail closed at the provider boundary.
/// </summary>
public static class CanonicalMathematicsSkillMapper
{
    public static IReadOnlyList<CanonicalMathematicsSkill> Resolve(
        string? outcomeCode,
        string? description)
    {
        var text = $" {outcomeCode} {description} ".ToUpperInvariant();
        var skills = new HashSet<CanonicalMathematicsSkill>();

        if (ContainsAny(text, "FRACTION OF", "FRACTIONS OF"))
            skills.Add(CanonicalMathematicsSkill.FractionOfQuantity);

        if (ContainsAny(text, "PERCENT OF", "PERCENTAGE OF"))
            skills.Add(CanonicalMathematicsSkill.PercentageOfQuantity);

        if (ContainsAny(text, "UNIT RATE", "UNIT-RATE"))
            skills.Add(CanonicalMathematicsSkill.UnitRateAndProportion);

        if (ContainsAny(text, "ONE-STEP EQUATION", "ONE STEP EQUATION") ||
            (text.Contains("SOLVE", StringComparison.Ordinal) &&
             text.Contains("EQUATION", StringComparison.Ordinal) &&
             !ContainsAny(text, "QUADRATIC", "SIMULTANEOUS", "SYSTEM OF")))
        {
            skills.Add(CanonicalMathematicsSkill.OneStepLinearEquation);
        }

        var hasAdd = text.Contains("ADD", StringComparison.Ordinal);
        var hasSubtract = text.Contains("SUBTRACT", StringComparison.Ordinal);
        var hasMultiply = text.Contains("MULTIP", StringComparison.Ordinal);
        var hasDivide = text.Contains("DIVID", StringComparison.Ordinal);

        if (hasAdd && hasSubtract && !hasMultiply && !hasDivide)
        {
            skills.Add(CanonicalMathematicsSkill.WholeNumberAdditionAndSubtraction);
        }
        else
        {
            if (hasAdd)
                skills.Add(CanonicalMathematicsSkill.WholeNumberAddition);
            if (hasSubtract)
                skills.Add(CanonicalMathematicsSkill.WholeNumberSubtraction);
        }

        if (hasMultiply)
            skills.Add(CanonicalMathematicsSkill.WholeNumberMultiplication);
        if (hasDivide)
            skills.Add(CanonicalMathematicsSkill.WholeNumberDivision);

        return skills.OrderBy(skill => skill).ToArray();
    }

    private static bool ContainsAny(string text, params string[] values) =>
        values.Any(value => text.Contains(value, StringComparison.Ordinal));
}
