using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

public static class UsCcssAlg1Unit4FunctionQuestions
{
    public const string FunctionRelationFamily = "functions.relations.unique_output.classify";
    public const string FunctionNotationFamily = "functions.notation.evaluate_context";
    public const string FunctionDomainRangeFamily = "functions.domain_range.context_bounds";

    public sealed record Problem(string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) =>
        family is FunctionRelationFamily or FunctionNotationFamily or FunctionDomainRangeFamily;

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

        if (family == FunctionDomainRangeFamily)
        {
            var duration = random.Next(2, 9);
            var rate = random.Next(2, 10);
            var start = duration * rate;
            var variant = random.Next(0, 16);
            return new(family,
                $"A tank starts with {start} litres and drains at {rate} litres per hour. V(t)={start}-{rate}t is modeled only from t=0 through t={duration} hours. Enter the endpoint form exactly as D=0,{duration};R=0,{start}. Explain with the teacher why the context restricts both sets.",
                $"The modeled domain is 0 through {duration} hours and the range is 0 through {start} litres.",
                AssessmentItemType.ShortAnswer,
                new Dictionary<string, int>(StringComparer.Ordinal)
                {
                    ["duration"] = duration, ["rate"] = rate,
                    ["start"] = start, ["variant"] = variant
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

        if (family == FunctionDomainRangeFamily)
        {
            if (!ValidateDomainRange(parameters, out var duration, out _, out var start, out _))
                throw new InvalidOperationException("Invalid domain-range parameters.");
            return $"D=0,{duration};R=0,{start}";
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

        if (family == FunctionDomainRangeFamily)
        {
            if (!ValidateDomainRange(parameters, out _, out _, out _, out _)) return false;
            var normalized = new string(answer.Where(c => !char.IsWhiteSpace(c)).ToArray()).ToUpperInvariant();
            return normalized == Solve(family, parameters).ToUpperInvariant();
        }

        if (family != FunctionRelationFamily ||
            !ValidateRelation(parameters, out _, out _, out _, out _, out _))
            return false;

        var relationAnswer = answer.Trim().ToLowerInvariant() switch
        {
            "yes" or "y" or "true" => "yes",
            "no" or "n" or "false" => "no",
            _ => ""
        };
        return relationAnswer.Length > 0 && relationAnswer == Solve(family, parameters);
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

    private static bool ValidateDomainRange(IReadOnlyDictionary<string, int>? p,
        out int duration, out int rate, out int start, out int variant)
    {
        duration = rate = start = variant = 0;
        return p is not null &&
            p.TryGetValue("duration", out duration) && duration is >= 2 and <= 8 &&
            p.TryGetValue("rate", out rate) && rate is >= 2 and <= 9 &&
            p.TryGetValue("start", out start) && start == duration * rate &&
            p.TryGetValue("variant", out variant) && variant is >= 0 and <= 15;
    }
}
