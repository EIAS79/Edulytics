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

    public sealed record Problem(
        string Family,
        string Prompt,
        string Solution,
        AssessmentItemType ItemType,
        IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) =>
        family is ConstraintFeasibilityFamily;

    public static Problem Build(string family, Random random)
    {
        if (!Supports(family))
            throw new InvalidOperationException("Unsupported ALG1 Unit 2 extension family.");
        ArgumentNullException.ThrowIfNull(random);

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
        if (!Supports(family) || string.IsNullOrWhiteSpace(answer) ||
            !Validate(parameters, out _, out _, out _, out _, out _, out _, out _))
            return false;

        return NormalizeYesNo(answer) == Solve(family, parameters);
    }

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
