using Edulytics.Core.Entities;
using Edulytics.Core.MathematicsGeneration;

namespace Edulytics.Services.Assessments;

public static class NativeMathematicsOutcomeProfileResolver
{
    public static MathematicsOutcomeGenerationProfile? Resolve(LearningOutcome outcome)
    {
        ArgumentNullException.ThrowIfNull(outcome);
        var families = ResolveFamilies(outcome.Code, outcome.Description);

        return families.Length == 0
            ? null
            : new MathematicsOutcomeGenerationProfile(
                outcome.Id,
                outcome.Code,
                families);
    }

    public static bool Supports(string? code, string? description) =>
        ResolveFamilies(code, description).Length > 0;

    private static MathematicsGeneratorFamily[] ResolveFamilies(string? code, string? description)
    {
        var text = $"{code} {description}".ToUpperInvariant();
        var families = new HashSet<MathematicsGeneratorFamily>();

        if (ContainsAny(text, "FRACTION", ".NF", " NF "))
            families.Add(MathematicsGeneratorFamily.FractionOfQuantity);

        if (ContainsAny(text, "PERCENT", "PERCENTAGE"))
            families.Add(MathematicsGeneratorFamily.PercentageOfQuantity);

        if (ContainsAny(text, "RATIO", "RATE", "PROPORTION", ".RP", " RP "))
            families.Add(MathematicsGeneratorFamily.UnitRateWordProblem);

        if (ContainsAny(text, "EQUATION", "EXPRESSION", "ALGEBRA", ".EE", " EE "))
            families.Add(MathematicsGeneratorFamily.OneStepEquation);

        if (ContainsAny(
                text,
                "ADD", "SUBTRACT", "MULTIP", "DIVID", "INTEGER", "WHOLE NUMBER",
                ".OA", ".NBT", ".NS", " ARITHMETIC", "CALCULAT"))
        {
            families.Add(MathematicsGeneratorFamily.IntegerComputation);
        }

        return families.OrderBy(x => x).ToArray();
    }

    private static bool ContainsAny(string text, params string[] values) =>
        values.Any(value => text.Contains(value, StringComparison.Ordinal));
}
