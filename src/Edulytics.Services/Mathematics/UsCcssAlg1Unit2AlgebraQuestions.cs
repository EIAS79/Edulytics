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
    public const string PatternClassificationFamily =
        "algebra.modeling.pattern.classify_relation";

    public sealed record Problem(
        string Family,
        string Prompt,
        string Solution,
        AssessmentItemType ItemType,
        IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) =>
        family is ConstraintFeasibilityFamily or LinearRelationshipEquationFamily or
            PatternClassificationFamily;

    public static Problem Build(string family, Random random)
    {
        if (!Supports(family))
            throw new InvalidOperationException("Unsupported ALG1 Unit 2 extension family.");
        ArgumentNullException.ThrowIfNull(random);

        if (family == LinearRelationshipEquationFamily)
            return BuildLinearRelationshipEquation(random);
        if (family == PatternClassificationFamily)
            return BuildPatternClassification(random);

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
        if (family == PatternClassificationFamily)
        {
            if (!ValidatePattern(parameters, out var relation, out _, out _, out _, out _))
                throw new InvalidOperationException("Invalid ALG1 U02 pattern parameters.");
            return relation switch
            {
                0 => "linear",
                1 => "constant-total",
                2 => "inverse-variation",
                _ => throw new InvalidOperationException("Unsupported pattern relation.")
            };
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
        if (family == PatternClassificationFamily)
            return ValidatePattern(parameters, out _, out _, out _, out _, out _) &&
                NormalizePattern(answer) == Solve(family, parameters);

        if (!Validate(parameters, out _, out _, out _, out _, out _, out _, out _))
            return false;

        return NormalizeYesNo(answer) == Solve(family, parameters);
    }

    private static Problem BuildPatternClassification(Random random)
    {
        var relation = random.Next(0, 3);
        var m = random.Next(2, 8);
        var b = random.Next(1, 10);
        var k = 24 * random.Next(1, 5);
        var variant = random.Next(0, 16);

        string pairs;
        if (relation == 0)
        {
            var xs = new[] { 0, 1, 2, 4 };
            pairs = string.Join(", ", xs.Select(x => $"({x},{m * x + b})"));
        }
        else if (relation == 1)
        {
            var xs = new[] { 0, k / 4, k / 2, 3 * k / 4 };
            pairs = string.Join(", ", xs.Select(x => $"({x},{k - x})"));
        }
        else
        {
            var xs = new[] { 1, 2, 3, 6 };
            pairs = string.Join(", ", xs.Select(x => $"({x},{k / x})"));
        }

        return new(
            PatternClassificationFamily,
            $"A table contains the ordered pairs {pairs}. Classify the relationship as linear, constant-total, or inverse-variation. " +
            "Enter only the classification. Writing a general equation and explaining the pattern remain teacher-reviewed.",
            "Check for a constant rate of change, a constant sum x+y, or a constant product xy.",
            AssessmentItemType.ShortAnswer,
            new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["relation"] = relation, ["m"] = m, ["b"] = b, ["k"] = k, ["variant"] = variant
            });
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

    private static bool ValidatePattern(
        IReadOnlyDictionary<string, int>? parameters,
        out int relation,
        out int m,
        out int b,
        out int k,
        out int variant)
    {
        relation = m = b = k = variant = 0;
        return parameters is not null &&
            parameters.TryGetValue("relation", out relation) && relation is >= 0 and <= 2 &&
            parameters.TryGetValue("m", out m) && m is >= 2 and <= 7 &&
            parameters.TryGetValue("b", out b) && b is >= 1 and <= 9 &&
            parameters.TryGetValue("k", out k) && k is >= 24 and <= 96 && k % 24 == 0 &&
            parameters.TryGetValue("variant", out variant) && variant is >= 0 and <= 15;
    }

    private static string NormalizePattern(string value)
    {
        var text = value.Trim().ToLowerInvariant().Replace('_', '-').Replace(' ', '-');
        return text switch
        {
            "constant-sum" or "constant-total" => "constant-total",
            "inverse" or "inverse-variation" => "inverse-variation",
            _ => text
        };
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
