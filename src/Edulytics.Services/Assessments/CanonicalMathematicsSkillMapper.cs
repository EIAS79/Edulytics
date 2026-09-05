using Edulytics.Core.MathematicsGeneration;

namespace Edulytics.Services.Assessments;

/// <summary>
/// Converts framework-specific outcome metadata into curriculum-neutral skills.
/// The mapping layer owns curriculum vocabulary; generation providers only see
/// canonical skills.
/// </summary>
public static class CanonicalMathematicsSkillMapper
{
    public static IReadOnlyList<CanonicalMathematicsSkill> Resolve(
        string? outcomeCode,
        string? description)
    {
        var text = $"{outcomeCode} {description}".ToUpperInvariant();
        var skills = new HashSet<CanonicalMathematicsSkill>();

        if (ContainsAny(text, "FRACTION", ".NF", " NF "))
            skills.Add(CanonicalMathematicsSkill.FractionOfQuantity);

        if (ContainsAny(text, "PERCENT", "PERCENTAGE"))
            skills.Add(CanonicalMathematicsSkill.PercentageOfQuantity);

        if (ContainsAny(text, "RATIO", "RATE", "PROPORTION", ".RP", " RP "))
            skills.Add(CanonicalMathematicsSkill.UnitRateAndProportion);

        if (ContainsAny(text, "EQUATION", "EXPRESSION", "ALGEBRA", ".EE", " EE "))
            skills.Add(CanonicalMathematicsSkill.OneStepLinearEquation);

        if (ContainsAny(
                text,
                "ADD", "SUBTRACT", "MULTIP", "DIVID", "INTEGER", "WHOLE NUMBER",
                ".OA", ".NBT", ".NS", " ARITHMETIC", "CALCULAT"))
        {
            skills.Add(CanonicalMathematicsSkill.WholeNumberComputation);
        }

        return skills.OrderBy(skill => skill).ToArray();
    }

    private static bool ContainsAny(string text, params string[] values) =>
        values.Any(value => text.Contains(value, StringComparison.Ordinal));
}
