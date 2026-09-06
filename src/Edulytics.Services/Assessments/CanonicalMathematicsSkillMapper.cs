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
        var codeText = $" {outcomeCode} ".ToUpperInvariant();
        var descriptionText = $" {description} ".ToUpperInvariant();
        var text = codeText + descriptionText;
        var skills = new HashSet<CanonicalMathematicsSkill>();

        var isFractionOfQuantity =
            ContainsAny(text, "FRACTION OF", "FRACTIONS OF") ||
            (ContainsAny(codeText, ".NF.", ":NF.") &&
             text.Contains("FRACTION", StringComparison.Ordinal) &&
             text.Contains("MULTIP", StringComparison.Ordinal));
        if (isFractionOfQuantity)
            skills.Add(CanonicalMathematicsSkill.FractionOfQuantity);

        if (ContainsAny(text, "PERCENT OF", "PERCENTAGE OF"))
            skills.Add(CanonicalMathematicsSkill.PercentageOfQuantity);

        var isUnitRate =
            ContainsAny(
                text,
                "UNIT RATE",
                "UNIT-RATE",
                "النسب والتناسب",
                "PROPORCJONALNOŚĆ PROSTA") ||
            (ContainsAny(codeText, ".RP.", ":RP.") &&
             ContainsAny(text, "RATIO", "RATE"));
        if (isUnitRate)
            skills.Add(CanonicalMathematicsSkill.UnitRateAndProportion);

        var isOneStepEquation =
            ContainsAny(
                text,
                "ONE-STEP EQUATION",
                "ONE STEP EQUATION",
                "حل معادلات الخطوة الواحدة",
                "حل معادلة الخطوة الواحدة") ||
            (text.Contains("SOLVE", StringComparison.Ordinal) &&
             text.Contains("EQUATION", StringComparison.Ordinal) &&
             !ContainsAny(text, "QUADRATIC", "SIMULTANEOUS", "SYSTEM OF"));
        if (isOneStepEquation)
        {
            skills.Add(CanonicalMathematicsSkill.OneStepLinearEquation);
        }

        // Whole-number generation must not be inferred from a broad framework
        // locator alone. OA/NBT/NS standards include modelling, reasoning,
        // fractions and decimals that the reviewed IntegerComputation family
        // does not yet implement. Explicit whole-number/integer wording is safe;
        // framework-code inference is limited to explicit fluency outcomes and
        // fails closed for fraction/decimal contexts.
        var hasExplicitWholeNumberScope =
            ContainsAny(
                descriptionText,
                "WHOLE NUMBER",
                "WHOLE NUMBERS",
                "INTEGER",
                "INTEGERS");
        var hasWholeNumberFrameworkLocator =
            ContainsAny(codeText, ".OA.", ":OA.", ".NBT.", ":NBT.", ".NS.", ":NS.");
        var isFluencyOutcome = descriptionText.Contains("FLUENTLY", StringComparison.Ordinal);
        var hasNonWholeNumberContext =
            ContainsAny(descriptionText, "FRACTION", "FRACTIONS", "DECIMAL", "DECIMALS");
        var targetsWholeNumberArithmetic =
            !hasNonWholeNumberContext &&
            (hasExplicitWholeNumberScope ||
             (hasWholeNumberFrameworkLocator && isFluencyOutcome));

        if (targetsWholeNumberArithmetic)
        {
            var hasAdd = descriptionText.Contains("ADD", StringComparison.Ordinal);
            var hasSubtract = descriptionText.Contains("SUBTRACT", StringComparison.Ordinal);
            var hasMultiply = descriptionText.Contains("MULTIP", StringComparison.Ordinal);
            var hasDivide = ContainsAny(descriptionText, "DIVID", "DIVISION");

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
        }

        return skills.OrderBy(skill => skill).ToArray();
    }

    private static bool ContainsAny(string text, params string[] values) =>
        values.Any(value => text.Contains(value, StringComparison.Ordinal));
}
