using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

public static class UsCcssAlg1Unit4FunctionQuestions
{
    public const string FunctionRelationFamily = "functions.relations.unique_output.classify";
    public const string FunctionNotationFamily = "functions.notation.evaluate_context";

    public sealed record Problem(string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) =>
        family is FunctionRelationFamily or FunctionNotationFamily;

    public static Problem Build(string family, Random random)
    {
        ArgumentNullException.ThrowIfNull(random);

        if (family == FunctionNotationFamily)
        {
            var initial = random.Next(0, 21);
            var rate = random.Next(1, 10);
            var input = random.Next(0, 7);
            var variant = random.Next(0, 16);
            var output = initial + rate * input;
            return new(family,
                $"A named function H gives a quantity in metres after t hours and obeys H(t)={initial}+{rate}t. Evaluate H({input}). Enter the integer output only. Explain with the teacher what the input and output mean in context.",
                $"H({input})={output}.",
                AssessmentItemType.ShortAnswer,
                new Dictionary<string, int>(StringComparer.Ordinal)
                {
                    ["initial"] = initial, ["rate"] = rate,
                    ["input"] = input, ["variant"] = variant
                });
        }

        if (family != FunctionRelationFamily)
            throw new InvalidOperationException("Unsupported family.");

        var x = random.Next(-4, 9);
        var a = random.Next(-9, 10);
        var b = random.Next(-9, 10);
        var repeats = random.Next(0, 2);
        if (repeats == 1 && a == b) b = a == 9 ? 8 : a + 1;
        var lastX = repeats == 1 ? x : x + 1;
        var relationVariant = random.Next(0, 16);
        return new(family,
            $"Is the relation ({x},{a}), ({x + 2},{a + 3}), ({lastX},{b}) a function of its first coordinate? Enter yes or no. Explain graph features with the teacher.",
            "A function assigns exactly one output to each input.",
            AssessmentItemType.ShortAnswer,
            new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["x"] = x, ["a"] = a, ["b"] = b,
                ["repeats"] = repeats, ["variant"] = relationVariant
            });
    }

    public static string Solve(string family, IReadOnlyDictionary<string, int> parameters)
    {
        if (family == FunctionNotationFamily)
        {
            if (!ValidateNotation(parameters, out var initial, out var rate, out var input, out _))
                throw new InvalidOperationException("Invalid function-notation parameters.");
            return (initial + rate * input).ToString();
        }

        if (family != FunctionRelationFamily ||
            !ValidateRelation(parameters, out _, out var a, out var b, out var repeats, out _))
            throw new InvalidOperationException("Invalid relation parameters.");
        return repeats == 1 && a != b ? "no" : "yes";
    }

    public static bool Verify(string family, IReadOnlyDictionary<string, int> parameters, string? answer)
    {
        if (string.IsNullOrWhiteSpace(answer)) return false;

        if (family == FunctionNotationFamily)
        {
            if (!ValidateNotation(parameters, out _, out _, out _, out _) ||
                !int.TryParse(answer.Trim(), out var submitted))
                return false;
            return submitted.ToString() == Solve(family, parameters);
        }

        if (family != FunctionRelationFamily ||
            !ValidateRelation(parameters, out _, out _, out _, out _, out _))
            return false;

        var normalized = answer.Trim().ToLowerInvariant() switch
        {
            "yes" or "y" or "true" => "yes",
            "no" or "n" or "false" => "no",
            _ => ""
        };
        return normalized.Length > 0 && normalized == Solve(family, parameters);
    }

    private static bool ValidateRelation(IReadOnlyDictionary<string, int>? p,
        out int x, out int a, out int b, out int repeats, out int variant)
    {
        x = a = b = repeats = variant = 0;
        return p is not null &&
            p.TryGetValue("x", out x) && x is >= -4 and <= 8 &&
            p.TryGetValue("a", out a) && a is >= -9 and <= 9 &&
            p.TryGetValue("b", out b) && b is >= -9 and <= 10 &&
            p.TryGetValue("repeats", out repeats) && repeats is >= 0 and <= 1 &&
            p.TryGetValue("variant", out variant) && variant is >= 0 and <= 15;
    }

    private static bool ValidateNotation(IReadOnlyDictionary<string, int>? p,
        out int initial, out int rate, out int input, out int variant)
    {
        initial = rate = input = variant = 0;
        return p is not null &&
            p.TryGetValue("initial", out initial) && initial is >= 0 and <= 20 &&
            p.TryGetValue("rate", out rate) && rate is >= 1 and <= 9 &&
            p.TryGetValue("input", out input) && input is >= 0 and <= 6 &&
            p.TryGetValue("variant", out variant) && variant is >= 0 and <= 15;
    }
}
