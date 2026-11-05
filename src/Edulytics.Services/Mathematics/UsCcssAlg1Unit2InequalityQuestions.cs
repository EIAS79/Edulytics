using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Exact, bounded inequality components for Algebra 1 U02.
/// Construction of graphs and open modelling explanations are teacher-reviewed.
/// </summary>
public static class UsCcssAlg1Unit2InequalityQuestions
{
    public const string ModelOperator = "algebra.inequality.context.operator_choice";
    public const string OneVariableCheck = "algebra.inequality.one_variable.candidate_check";
    public const string TwoVariableCheck = "algebra.inequality.two_variable.candidate_check";
    public const string SystemCheck = "algebra.inequality.system.candidate_check";

    public sealed record Problem(
        string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) =>
        family is ModelOperator or OneVariableCheck or TwoVariableCheck or SystemCheck;

    public static Problem Build(string family, Random random)
    {
        if (!Supports(family))
            throw new InvalidOperationException("Unsupported inequality family.");
        ArgumentNullException.ThrowIfNull(random);
        var a = random.Next(2, 10);
        var b = random.Next(2, 8);
        var c = random.Next(15, 61);
        var x = random.Next(0, 16);
        var y = random.Next(0, 16);
        var mode = random.Next(0, 4);
        var minimum = random.Next(2, 11);
        var variant = random.Next(0, 16);
        var parameters = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["a"] = a, ["b"] = b, ["c"] = c, ["x"] = x, ["y"] = y,
            ["mode"] = mode, ["minimum"] = minimum, ["variant"] = variant
        };

        if (family == ModelOperator)
        {
            var choice = random.Next(0, 3);
            parameters["mode"] = choice;
            var wording = choice switch
            {
                0 => "at most",
                1 => "at least",
                2 => "strictly fewer than",
                _ => throw new InvalidOperationException()
            };
            return new(family,
                $"An item costs {a} dollars. A total of {wording} {c} dollars is permitted. " +
                $"Let x be the number of items. Choose the matching inequality: " +
                $"A) {a}x <= {c}; B) {a}x >= {c}; C) {a}x < {c}. " +
                "Enter A, B, or C. Explain the domain restriction with the teacher.",
                "Translate at most to <=, at least to >=, and strictly fewer to <.",
                AssessmentItemType.ShortAnswer, parameters);
        }

        if (family == OneVariableCheck)
            return new(family,
                $"Does x={x} satisfy {a}x{Operator(mode)}{c}? Enter yes or no. " +
                "Explain the entire interval and endpoint inclusion with the teacher.",
                "Substitute x and compare both sides.",
                AssessmentItemType.ShortAnswer, parameters);

        if (family == TwoVariableCheck)
            return new(family,
                $"Does point ({x},{y}) satisfy {a}x+{b}y{Operator(mode)}{c}? " +
                "Enter yes or no. Graph the boundary and solution region with the teacher.",
                "Substitute both coordinates and compare both sides.",
                AssessmentItemType.ShortAnswer, parameters);

        return new(family,
            $"A plan requires {a}x+{b}y <= {c} and x >= {minimum}, with x,y >= 0. " +
            $"Is the pair ({x},{y}) feasible? Enter yes or no. " +
            "Construct and justify the overlap region with the teacher.",
            "Check every stated constraint and the nonnegative domain.",
            AssessmentItemType.ShortAnswer, parameters);
    }

    public static string Solve(string family, IReadOnlyDictionary<string, int> parameters)
    {
        if (!Supports(family) || !Valid(parameters, out var a, out var b, out var c,
            out var x, out var y, out var mode, out var minimum, out _))
            throw new InvalidOperationException("Invalid inequality parameters.");

        if (family == ModelOperator)
            return mode switch { 0 => "a", 1 => "b", 2 => "c", _ => throw new InvalidOperationException() };

        bool result = family switch
        {
            OneVariableCheck => Compare(a * x, c, mode),
            TwoVariableCheck => Compare(a * x + b * y, c, mode),
            SystemCheck => a * x + b * y <= c && x >= minimum && x >= 0 && y >= 0,
            _ => throw new InvalidOperationException()
        };
        return result ? "yes" : "no";
    }

    public static bool Verify(string family, IReadOnlyDictionary<string, int> parameters, string? answer)
    {
        if (!Supports(family) || string.IsNullOrWhiteSpace(answer) ||
            !Valid(parameters, out _, out _, out _, out _, out _, out _, out _))
            return false;
        var v = answer.Trim().ToLowerInvariant();
        if (family == ModelOperator)
        {
            v = v switch { "option a" => "a", "option b" => "b", "option c" => "c", _ => v };
            return v == Solve(family, parameters);
        }
        v = v switch { "y" or "true" => "yes", "n" or "false" => "no", _ => v };
        return v == Solve(family, parameters);
    }

    private static bool Compare(int left, int right, int mode) => mode switch
    {
        0 => left <= right,
        1 => left >= right,
        2 => left < right,
        3 => left > right,
        _ => false
    };

    private static string Operator(int mode) => mode switch
    {
        0 => "<=", 1 => ">=", 2 => "<", 3 => ">",
        _ => throw new InvalidOperationException()
    };

    private static bool Valid(IReadOnlyDictionary<string, int>? parameters,
        out int a, out int b, out int c, out int x, out int y,
        out int mode, out int minimum, out int variant)
    {
        a = b = c = x = y = mode = minimum = variant = 0;
        return parameters is not null &&
            parameters.TryGetValue("a", out a) && a is >= 2 and <= 9 &&
            parameters.TryGetValue("b", out b) && b is >= 2 and <= 7 &&
            parameters.TryGetValue("c", out c) && c is >= 15 and <= 60 &&
            parameters.TryGetValue("x", out x) && x is >= 0 and <= 15 &&
            parameters.TryGetValue("y", out y) && y is >= 0 and <= 15 &&
            parameters.TryGetValue("mode", out mode) && mode is >= 0 and <= 3 &&
            parameters.TryGetValue("minimum", out minimum) && minimum is >= 2 and <= 10 &&
            parameters.TryGetValue("variant", out variant) && variant is >= 0 and <= 15;
    }
}
