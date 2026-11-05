using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Exact objective components for IM Algebra 1 Unit 2.
/// Open modelling, graph construction and written justification remain teacher-reviewed.
/// </summary>
public static class UsCcssAlg1Unit2AlgebraQuestions
{
    public const string ConstraintFeasibilityFamily =
        "algebra.modeling.constraints.feasible_pair";
    public const string LinearRelationshipEquationFamily =
        "algebra.modeling.linear_relationship.select_equation";

    public sealed record Problem(
        string Family,
        string Prompt,
        string Solution,
        AssessmentItemType ItemType,
        IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) =>
        family is ConstraintFeasibilityFamily or LinearRelationshipEquationFamily;

    public static Problem Build(string family, Random random)
    {
        if (!Supports(family))
            throw new InvalidOperationException("Unsupported ALG1 Unit 2 extension family.");
        ArgumentNullException.ThrowIfNull(random);

        if (family == LinearRelationshipEquationFamily)
            return BuildLinearRelationshipEquation(random);

        var fixedCost = random.Next(4, 16);
        var itemCost = random.Next(8, 21);
        var capacity = random.Next(2, 6);
        var budget = random.Next(70, 161);
        var guests = random.Next(4, 31);
        var items = random.Next(1, 11);
        var variant = random.Next(0, 16);

        return new(
            family,
            $"A group plan has fixed cost {fixedCost} dollars, each package costs {itemCost} dollars, " +
            $"each package serves at most {capacity} guests, and the budget is at most {budget} dollars. " +
            $"For {guests} guests and {items} packages, do both the capacity and budget constraints hold? " +
            "Enter yes or no. Writing and defending a complete model remains teacher-reviewed.",
            "Check capacity×packages ≥ guests and fixed cost + item cost×packages ≤ budget.",
            AssessmentItemType.ShortAnswer,
            new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["fixedCost"] = fixedCost,
                ["itemCost"] = itemCost,
                ["capacity"] = capacity,
                ["budget"] = budget,
                ["guests"] = guests,
                ["items"] = items,
                ["variant"] = variant
            });
    }

    public static string Solve(
        string family,
        IReadOnlyDictionary<string, int> parameters)
    {
        if (family == LinearRelationshipEquationFamily)
        {
            if (!ValidateLinearRelationship(parameters, out var rate, out var fixedAmount, out _))
                throw new InvalidOperationException("Invalid ALG1 U02 linear-relationship parameters.");
            return $"y={rate}x+{fixedAmount}";
        }

        if (!Validate(parameters, out var fixedCost, out var itemCost, out var capacity,
            out var budget, out var guests, out var items, out _))
            throw new InvalidOperationException("Invalid ALG1 U02 constraint parameters.");

        var feasible =
            capacity * items >= guests &&
            fixedCost + itemCost * items <= budget;
        return feasible ? "yes" : "no";
    }

    public static bool Verify(
        string family,
        IReadOnlyDictionary<string, int> parameters,
        string? answer)
    {
        if (!Supports(family) || string.IsNullOrWhiteSpace(answer))
            return false;

        if (family == LinearRelationshipEquationFamily)
            return ValidateLinearRelationship(parameters, out _, out _, out _) &&
                NormalizeEquation(answer) == NormalizeEquation(Solve(family, parameters));

        if (!Validate(parameters, out _, out _, out _, out _, out _, out _, out _))
            return false;

        return NormalizeYesNo(answer) == Solve(family, parameters);
    }

    private static Problem BuildLinearRelationshipEquation(Random random)
    {
        var rate = random.Next(2, 13);
        var fixedAmount = random.Next(3, 31);
        var variant = random.Next(0, 16);
        return new(
            LinearRelationshipEquationFamily,
            $"A service has a fixed fee of {fixedAmount} dollars and then costs {rate} dollars per unit. " +
            "Let x be the number of units and y the total cost. Write the equation relating x and y in the form y=mx+b. " +
            "Enter only the equation. Explaining which quantities vary and which are constant remains teacher-reviewed.",
            "The variable total equals fixed amount plus rate times the variable number of units.",
            AssessmentItemType.ShortAnswer,
            new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["rate"] = rate, ["fixedAmount"] = fixedAmount, ["variant"] = variant
            });
    }

    private static bool ValidateLinearRelationship(
        IReadOnlyDictionary<string, int>? parameters,
        out int rate,
        out int fixedAmount,
        out int variant)
    {
        rate = fixedAmount = variant = 0;
        return parameters is not null &&
            parameters.TryGetValue("rate", out rate) && rate is >= 2 and <= 12 &&
            parameters.TryGetValue("fixedAmount", out fixedAmount) && fixedAmount is >= 3 and <= 30 &&
            parameters.TryGetValue("variant", out variant) && variant is >= 0 and <= 15;
    }

    private static string NormalizeEquation(string value) =>
        string.Concat(value.Where(ch => !char.IsWhiteSpace(ch)))
            .Replace("×", "*", StringComparison.Ordinal)
            .Replace("·", "*", StringComparison.Ordinal)
            .ToLowerInvariant()
            .Replace("*x", "x", StringComparison.Ordinal);

    private static bool Validate(
        IReadOnlyDictionary<string, int>? parameters,
        out int fixedCost,
        out int itemCost,
        out int capacity,
        out int budget,
        out int guests,
        out int items,
        out int variant)
    {
        fixedCost = itemCost = capacity = budget = guests = items = variant = 0;
        return parameters is not null &&
            parameters.TryGetValue("fixedCost", out fixedCost) && fixedCost is >= 4 and <= 15 &&
            parameters.TryGetValue("itemCost", out itemCost) && itemCost is >= 8 and <= 20 &&
            parameters.TryGetValue("capacity", out capacity) && capacity is >= 2 and <= 5 &&
            parameters.TryGetValue("budget", out budget) && budget is >= 70 and <= 160 &&
            parameters.TryGetValue("guests", out guests) && guests is >= 4 and <= 30 &&
            parameters.TryGetValue("items", out items) && items is >= 1 and <= 10 &&
            parameters.TryGetValue("variant", out variant) && variant is >= 0 and <= 15;
    }

    private static string NormalizeYesNo(string value)
    {
        var text = value.Trim().ToLowerInvariant();
        return text switch
        {
            "y" or "yes" or "true" => "yes",
            "n" or "no" or "false" => "no",
            _ => text
        };
    }
}
