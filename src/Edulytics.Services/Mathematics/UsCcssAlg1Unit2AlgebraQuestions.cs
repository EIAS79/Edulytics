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
    public const string OneVariableSolutionCheckFamily =
        "algebra.equation.one_variable.solution_check";
    public const string TwoVariableSolutionCheckFamily =
        "algebra.equation.two_variable.solution_check";
    public const string EquationEquivalenceFamily =
        "algebra.equations.equivalence.classify";
    public const string LiteralEquationIsolateFamily =
        "algebra.literal_equation.isolate.select_form";
    public const string StandardFormSlopeInterceptFamily =
        "algebra.linear.standard_form.slope_intercept";

    public sealed record Problem(
        string Family,
        string Prompt,
        string Solution,
        AssessmentItemType ItemType,
        IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) =>
        family is ConstraintFeasibilityFamily or LinearRelationshipEquationFamily or
            PatternClassificationFamily or OneVariableSolutionCheckFamily or
            TwoVariableSolutionCheckFamily or EquationEquivalenceFamily or
            LiteralEquationIsolateFamily or StandardFormSlopeInterceptFamily;

    public static Problem Build(string family, Random random)
    {
        if (!Supports(family))
            throw new InvalidOperationException("Unsupported ALG1 Unit 2 extension family.");
        ArgumentNullException.ThrowIfNull(random);

        if (family == LinearRelationshipEquationFamily)
            return BuildLinearRelationshipEquation(random);
        if (family == PatternClassificationFamily)
            return BuildPatternClassification(random);
        if (family is OneVariableSolutionCheckFamily or TwoVariableSolutionCheckFamily)
            return BuildSolutionCheck(family, random);
        if (family == EquationEquivalenceFamily)
            return BuildEquationEquivalence(random);
        if (family == LiteralEquationIsolateFamily)
            return BuildLiteralEquationIsolate(random);
        if (family == StandardFormSlopeInterceptFamily)
            return BuildStandardFormSlopeIntercept(random);

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
        if (family is OneVariableSolutionCheckFamily or TwoVariableSolutionCheckFamily)
        {
            if (!ValidateSolutionCheck(parameters, out var a, out var b, out var c, out var x, out var y, out _))
                throw new InvalidOperationException("Invalid ALG1 U02 solution-check parameters.");
            var ok = family == OneVariableSolutionCheckFamily ? a * x + b == c : a * x + b * y == c;
            return ok ? "yes" : "no";
        }
        if (family == EquationEquivalenceFamily)
        {
            if (!ValidateEquivalence(parameters, out _, out _, out _, out var operation, out _))
                throw new InvalidOperationException("Invalid ALG1 U02 equivalence parameters.");
            return operation <= 2 ? "equivalent" : "not-equivalent";
        }
        if (family == LiteralEquationIsolateFamily)
        {
            if (!ValidateLiteralIsolate(parameters, out _, out var correct, out _))
                throw new InvalidOperationException("Invalid ALG1 U02 literal-equation parameters.");
            return correct switch { 0 => "a", 1 => "b", 2 => "c", _ => throw new InvalidOperationException() };
        }
        if (family == StandardFormSlopeInterceptFamily)
        {
            if (!ValidateSlopeIntercept(parameters, out var slope, out var intercept, out _, out _))
                throw new InvalidOperationException("Invalid ALG1 U02 slope-intercept parameters.");
            return $"{slope},{intercept}";
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
        if (family is OneVariableSolutionCheckFamily or TwoVariableSolutionCheckFamily)
            return ValidateSolutionCheck(parameters, out _, out _, out _, out _, out _, out _) &&
                NormalizeYesNo(answer) == Solve(family, parameters);
        if (family == EquationEquivalenceFamily)
            return ValidateEquivalence(parameters, out _, out _, out _, out _, out _) &&
                NormalizeEquivalence(answer) == Solve(family, parameters);
        if (family == LiteralEquationIsolateFamily)
            return ValidateLiteralIsolate(parameters, out _, out _, out _) &&
                NormalizeChoice(answer) == Solve(family, parameters);
        if (family == StandardFormSlopeInterceptFamily)
            return ValidateSlopeIntercept(parameters, out _, out _, out _, out _) &&
                NormalizePair(answer) == Solve(family, parameters);

        if (!Validate(parameters, out _, out _, out _, out _, out _, out _, out _))
            return false;

        return NormalizeYesNo(answer) == Solve(family, parameters);
    }

    private static Problem BuildStandardFormSlopeIntercept(Random random)
    {
        var slope = random.Next(-5, 6);
        if (slope == 0) slope = 2;
        var intercept = random.Next(-9, 10);
        var scale = random.Next(1, 5);
        var variant = random.Next(0, 16);
        var a = -slope * scale;
        var b = scale;
        var c = intercept * scale;

        return new(
            StandardFormSlopeInterceptFamily,
            $"Rewrite {a}x{(b >= 0 ? "+" : "")}{b}y={c} as y=mx+b. " +
            "Enter the slope and y-intercept as m,b. Interpreting the quantities in a context remains teacher-reviewed.",
            "Solve the equation for y; the coefficient of x is the slope and the constant term is the y-intercept.",
            AssessmentItemType.ShortAnswer,
            new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["slope"] = slope, ["intercept"] = intercept, ["scale"] = scale, ["variant"] = variant
            });
    }

    private static Problem BuildLiteralEquationIsolate(Random random)
    {
        var template = random.Next(0, 4);
        var correct = random.Next(0, 3);
        var variant = random.Next(0, 16);
        var (equation, target, right, wrong1, wrong2) = template switch
        {
            0 => ("A=bh", "h", "h=A/b", "h=Ab", "h=b/A"),
            1 => ("P=2l+2w", "w", "w=(P-2l)/2", "w=P-2l/2", "w=(P+2l)/2"),
            2 => ("v=u+at", "t", "t=(v-u)/a", "t=(v+u)/a", "t=a(v-u)"),
            3 => ("C=mx+b", "x", "x=(C-b)/m", "x=(C+b)/m", "x=m(C-b)"),
            _ => throw new InvalidOperationException()
        };
        var options = new[] { wrong1, wrong2, wrong1 };
        options[correct] = right;
        if (correct == 0) { options[1] = wrong1; options[2] = wrong2; }
        else if (correct == 1) { options[0] = wrong1; options[2] = wrong2; }
        else { options[0] = wrong1; options[1] = wrong2; }

        return new(
            LiteralEquationIsolateFamily,
            $"For {equation}, which option correctly isolates {target}? " +
            $"A) {options[0]}  B) {options[1]}  C) {options[2]}. Enter A, B, or C. " +
            "Writing a complete chain of equivalent equations remains teacher-reviewed.",
            "Undo operations on both sides in reverse order while preserving equivalence.",
            AssessmentItemType.ShortAnswer,
            new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["template"] = template, ["correct"] = correct, ["variant"] = variant
            });
    }

    private static Problem BuildEquationEquivalence(Random random)
    {
        var a = random.Next(2, 8);
        var b = random.Next(-6, 7);
        var x = random.Next(-4, 9);
        var c = a * x + b;
        var operation = random.Next(0, 5);
        var k = random.Next(2, 6);
        var variant = random.Next(0, 16);

        var original = $"{a}x{(b >= 0 ? "+" : "")}{b}={c}";
        var transformed = operation switch
        {
            0 => $"{a}x{(b + k >= 0 ? "+" : "")}{b + k}={c + k}",
            1 => $"{a * k}x{(b * k >= 0 ? "+" : "")}{b * k}={c * k}",
            2 => $"{a}x{(b - k >= 0 ? "+" : "")}{b - k}={c - k}",
            3 => $"{a}x{(b + k >= 0 ? "+" : "")}{b + k}={c}",
            4 => $"{a * k}x{(b >= 0 ? "+" : "")}{b}={c}",
            _ => throw new InvalidOperationException()
        };

        return new(
            EquationEquivalenceFamily,
            $"Are the equations {original} and {transformed} equivalent, meaning they have exactly the same solution set? " +
            "Enter equivalent or not-equivalent. Explaining the operation remains teacher-reviewed.",
            "Operations applied equally to both sides preserve the solution set; one-sided changes generally do not.",
            AssessmentItemType.ShortAnswer,
            new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["a"] = a, ["b"] = b, ["c"] = c, ["operation"] = operation, ["variant"] = variant
            });
    }

    private static Problem BuildSolutionCheck(string family, Random random)
    {
        var a = random.Next(2, 9);
        var b = random.Next(-7, 8);
        if (b == 0) b = 3;
        var x = random.Next(-5, 11);
        var y = random.Next(-5, 11);
        var shouldMatch = random.Next(0, 2) == 1;
        var c = family == OneVariableSolutionCheckFamily ? a * x + b : a * x + b * y;
        if (!shouldMatch) c += random.Next(1, 5);
        var variant = random.Next(0, 16);
        var prompt = family == OneVariableSolutionCheckFamily
            ? $"Does x={x} satisfy {a}x{(b >= 0 ? "+" : "")}{b}={c}? Enter yes or no."
            : $"Does the ordered pair ({x},{y}) satisfy {a}x{(b >= 0 ? "+" : "")}{b}y={c}? Enter yes or no.";
        return new(
            family,
            prompt + " Explain the contextual meaning of a solution separately with the teacher.",
            "Substitute the proposed value or ordered pair into the original equation and compare both sides.",
            AssessmentItemType.ShortAnswer,
            new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["a"] = a, ["b"] = b, ["c"] = c, ["x"] = x, ["y"] = y, ["variant"] = variant
            });
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

    private static bool ValidateSlopeIntercept(
        IReadOnlyDictionary<string, int>? parameters,
        out int slope,
        out int intercept,
        out int scale,
        out int variant)
    {
        slope = intercept = scale = variant = 0;
        return parameters is not null &&
            parameters.TryGetValue("slope", out slope) && slope is >= -5 and <= 5 && slope != 0 &&
            parameters.TryGetValue("intercept", out intercept) && intercept is >= -9 and <= 9 &&
            parameters.TryGetValue("scale", out scale) && scale is >= 1 and <= 4 &&
            parameters.TryGetValue("variant", out variant) && variant is >= 0 and <= 15;
    }

    private static string NormalizePair(string value) =>
        string.Concat(value.Where(ch => !char.IsWhiteSpace(ch)))
            .Replace("(", "", StringComparison.Ordinal)
            .Replace(")", "", StringComparison.Ordinal);

    private static bool ValidateLiteralIsolate(
        IReadOnlyDictionary<string, int>? parameters,
        out int template,
        out int correct,
        out int variant)
    {
        template = correct = variant = 0;
        return parameters is not null &&
            parameters.TryGetValue("template", out template) && template is >= 0 and <= 3 &&
            parameters.TryGetValue("correct", out correct) && correct is >= 0 and <= 2 &&
            parameters.TryGetValue("variant", out variant) && variant is >= 0 and <= 15;
    }

    private static string NormalizeChoice(string value)
    {
        var text = value.Trim().ToLowerInvariant();
        return text switch
        {
            "a" or "option a" => "a",
            "b" or "option b" => "b",
            "c" or "option c" => "c",
            _ => text
        };
    }

    private static bool ValidateEquivalence(
        IReadOnlyDictionary<string, int>? parameters,
        out int a,
        out int b,
        out int c,
        out int operation,
        out int variant)
    {
        a = b = c = operation = variant = 0;
        return parameters is not null &&
            parameters.TryGetValue("a", out a) && a is >= 2 and <= 7 &&
            parameters.TryGetValue("b", out b) && b is >= -6 and <= 6 &&
            parameters.TryGetValue("c", out c) && c is >= -100 and <= 150 &&
            parameters.TryGetValue("operation", out operation) && operation is >= 0 and <= 4 &&
            parameters.TryGetValue("variant", out variant) && variant is >= 0 and <= 15;
    }

    private static string NormalizeEquivalence(string value)
    {
        var text = value.Trim().ToLowerInvariant().Replace('_', '-').Replace(' ', '-');
        return text switch
        {
            "same" or "equivalent" => "equivalent",
            "not" or "not-equivalent" or "different" => "not-equivalent",
            _ => text
        };
    }

    private static bool ValidateSolutionCheck(
        IReadOnlyDictionary<string, int>? parameters,
        out int a,
        out int b,
        out int c,
        out int x,
        out int y,
        out int variant)
    {
        a = b = c = x = y = variant = 0;
        return parameters is not null &&
            parameters.TryGetValue("a", out a) && a is >= 2 and <= 8 &&
            parameters.TryGetValue("b", out b) && b is >= -7 and <= 7 && b != 0 &&
            parameters.TryGetValue("c", out c) && c is >= -200 and <= 300 &&
            parameters.TryGetValue("x", out x) && x is >= -5 and <= 10 &&
            parameters.TryGetValue("y", out y) && y is >= -5 and <= 10 &&
            parameters.TryGetValue("variant", out variant) && variant is >= 0 and <= 15;
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
