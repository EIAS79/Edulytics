using Edulytics.Core.MathematicsGeneration;

namespace Edulytics.Services.Assessments;

/// <summary>
/// Converts framework-specific outcome metadata into curriculum-neutral skills.
/// Mapping is intentionally conservative: a capability is emitted only when the
/// outcome wording matches the semantics of a reviewed native generator. Broader
/// or unsupported operations fail closed rather than being treated as covered.
/// </summary>
public static class CanonicalMathematicsSkillMapper
{
    public static IReadOnlyList<CanonicalMathematicsSkill> Resolve(
        string? outcomeCode,
        string? description)
    {
        var codeText = $" {outcomeCode} ".ToUpperInvariant();
        var descriptionText = $" {description} ".ToUpperInvariant();
        var skills = new HashSet<CanonicalMathematicsSkill>();

        // The native fraction family answers a direct "fraction of a quantity"
        // computation. Generic references such as "fraction of the whole",
        // fraction multiplication, line plots, geometry and probability are not
        // equivalent and must remain closed.
        var isFractionOfQuantity = ContainsAny(
            descriptionText,
            "FRACTION OF A QUANTITY",
            "FRACTIONS OF A QUANTITY",
            "FRACTION OF QUANTITY",
            "FRACTIONS OF QUANTITY");
        if (isFractionOfQuantity)
            skills.Add(CanonicalMathematicsSkill.FractionOfQuantity);

        // The native percentage family performs a direct percentage-of-quantity
        // computation. Broad ratio/proportion standards that merely contain a
        // percentage example are not fully represented by this family.
        var isPercentageOfQuantity =
            ContainsAny(
                descriptionText,
                "PERCENT OF A QUANTITY",
                "PERCENTAGE OF A QUANTITY",
                "PERCENT OF QUANTITY",
                "PERCENTAGE OF QUANTITY") &&
            !ContainsAny(
                descriptionText,
                "RATIO AND RATE REASONING",
                "PROPORTIONAL RELATIONSHIP",
                "RATE PER 100",
                "MULTISTEP",
                "MULTI-STEP");
        if (isPercentageOfQuantity)
            skills.Add(CanonicalMathematicsSkill.PercentageOfQuantity);

        // Current UnitRateWordProblem consumes an already known unit rate and
        // applies it to a count. Ratio theory, direct proportion and computing
        // complex/fractional unit rates require different reviewed generators.
        var isUnitRate =
            ContainsAny(
                descriptionText,
                "USE A UNIT RATE",
                "USE THE UNIT RATE",
                "APPLY A UNIT RATE") &&
            !ContainsAny(
                descriptionText,
                "FRACTION",
                "FRACTIONS",
                "DECIMAL",
                "DECIMALS",
                "RATIO AND RATE REASONING",
                "PROPORTIONAL RELATIONSHIP");
        if (isUnitRate)
            skills.Add(CanonicalMathematicsSkill.UnitRateAndProportion);

        // OneStepEquation is temporarily not emitted here. The existing native
        // generator historically produced ax + b = c, which requires two inverse
        // operations and therefore does not satisfy the canonical one-step skill.
        // Re-enable only after the generator itself is corrected and regression
        // tests prove one-step semantics end to end.

        var hasWholeNumberFrameworkLocator =
            ContainsAny(codeText, ".OA.", ":OA.", ".NBT.", ":NBT.", ".NS.", ":NS.");
        var isFluencyOutcome = descriptionText.Contains("FLUENTLY", StringComparison.Ordinal);
        var hasNonWholeNumberContext =
            ContainsAny(descriptionText, "FRACTION", "FRACTIONS", "DECIMAL", "DECIMALS", "POLYNOMIAL");
        var hasDisallowedWholeNumberIntent = ContainsAny(
            descriptionText,
            "WORD PROBLEM",
            "WORD PROBLEMS",
            "DETERMINE THE UNKNOWN",
            "UNKNOWN WHOLE NUMBER",
            "REPRESENT ",
            "RECOGNIZE ",
            "IDENTIFY ",
            "FACTOR PAIR",
            "GREATEST COMMON FACTOR",
            "LEAST COMMON MULTIPLE",
            "LENGTH UNIT",
            "ARITHMETIC PATTERN",
            "MULTI-DIGIT");

        var hasAdd = descriptionText.Contains("ADD", StringComparison.Ordinal);
        var hasSubtract = descriptionText.Contains("SUBTRACT", StringComparison.Ordinal);
        var hasMultiply = descriptionText.Contains("MULTIP", StringComparison.Ordinal);
        var hasDivide = ContainsAny(descriptionText, "DIVID", "DIVISION", "QUOTIENT");

        // Generic, direct whole-number operation wording is safe for the current
        // deterministic family. For official OA/NBT/NS text we currently admit
        // only add/subtract fluency outcomes; multiplication/division standards
        // often constrain factor/dividend shape that the generator does not yet
        // carry in its profile contract.
        var hasExplicitDirectWholeNumberIntent = ContainsAny(
            descriptionText,
            "ADD WHOLE NUMBERS",
            "ADD AND SUBTRACT WHOLE NUMBERS",
            "SUBTRACT WHOLE NUMBERS",
            "MULTIPLY WHOLE NUMBERS",
            "DIVIDE WHOLE NUMBERS");
        var isReviewedLocatorFluency =
            hasWholeNumberFrameworkLocator &&
            isFluencyOutcome &&
            (hasAdd || hasSubtract) &&
            !hasMultiply &&
            !hasDivide;
        var targetsWholeNumberArithmetic =
            !hasNonWholeNumberContext &&
            !hasDisallowedWholeNumberIntent &&
            (hasExplicitDirectWholeNumberIntent || isReviewedLocatorFluency);

        if (targetsWholeNumberArithmetic)
        {
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
