using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

public static class UsCcssAlg1Unit4FunctionQuestions
{
    public const string FunctionRelationFamily = "functions.relations.unique_output.classify";
    public const string FunctionNotationFamily = "functions.notation.evaluate_context";
    public const string FunctionDomainRangeFamily = "functions.domain_range.context_bounds";
    public const string FunctionPiecewiseFamily = "functions.piecewise.evaluate_interval";
    public const string FunctionAbsoluteErrorFamily = "functions.absolute_error.from_target";

    public sealed record Problem(string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) =>
        family is FunctionRelationFamily or FunctionNotationFamily or FunctionDomainRangeFamily or FunctionPiecewiseFamily or FunctionAbsoluteErrorFamily;

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

        if (family == FunctionPiecewiseFamily)
        {
            var boundary = random.Next(1, 6);
            var leftSlope = random.Next(1, 5);
            var leftIntercept = random.Next(0, 8);
            var rightSlope = random.Next(1, 5);
            var rightIntercept = random.Next(0, 8);
            var side = random.Next(0, 2);
            var input = side == 0 ? boundary : boundary + 1;
            var variant = random.Next(0, 16);
            var output = input <= boundary
                ? leftSlope * input + leftIntercept
                : rightSlope * input + rightIntercept;
            return new(family,
                $"A piecewise function P uses P(x)={leftSlope}x+{leftIntercept} for x<= {boundary}, and P(x)={rightSlope}x+{rightIntercept} for x>{boundary}. Find P({input}). Enter the integer output only. Explain which rule applies and how the endpoint should be drawn.",
                $"Because x={input} {(input <= boundary ? "is in the first interval" : "is in the second interval")}, P({input})={output}.",
                AssessmentItemType.ShortAnswer,
                new Dictionary<string, int>(StringComparer.Ordinal)
                {
                    ["boundary"] = boundary, ["leftSlope"] = leftSlope,
                    ["leftIntercept"] = leftIntercept, ["rightSlope"] = rightSlope,
                    ["rightIntercept"] = rightIntercept, ["side"] = side,
                    ["variant"] = variant
                });
        }

        if (family == FunctionAbsoluteErrorFamily)
        {
            var target = random.Next(20, 81);
            var delta = random.Next(1, 16) * (random.Next(0, 2) == 0 ? -1 : 1);
            var guess = target + delta;
            var variant = random.Next(0, 16);
            return new(family,
                $"The target value is {target} and a guess is {guess}. Find the absolute error |{guess}-{target}|. Enter the nonnegative integer only. Explain with the teacher why absolute error is a distance.",
                $"The absolute error is {Math.Abs(delta)}.",
                AssessmentItemType.ShortAnswer,
                new Dictionary<string, int>(StringComparer.Ordinal)
                {
                    ["target"] = target, ["guess"] = guess, ["variant"] = variant
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

        if (family == FunctionPiecewiseFamily)
        {
            if (!ValidatePiecewise(parameters, out var boundary, out var leftSlope, out var leftIntercept,
                    out var rightSlope, out var rightIntercept, out var side, out _))
                throw new InvalidOperationException("Invalid piecewise parameters.");
            var input = side == 0 ? boundary : boundary + 1;
            return (input <= boundary ? leftSlope * input + leftIntercept : rightSlope * input + rightIntercept).ToString();
        }

        if (family == FunctionAbsoluteErrorFamily)
        {
            if (!ValidateAbsoluteError(parameters, out var target, out var guess, out _))
                throw new InvalidOperationException("Invalid absolute-error parameters.");
            return Math.Abs(guess - target).ToString();
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

        if (family == FunctionPiecewiseFamily)
        {
            if (!ValidatePiecewise(parameters, out _, out _, out _, out _, out _, out _, out _) ||
                !int.TryParse(answer.Trim(), out var submitted))
                return false;
            return submitted.ToString() == Solve(family, parameters);
        }

        if (family == FunctionAbsoluteErrorFamily)
        {
            if (!ValidateAbsoluteError(parameters, out _, out _, out _) ||
                !int.TryParse(answer.Trim(), out var submitted))
                return false;
            return submitted.ToString() == Solve(family, parameters);
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

    private static bool ValidatePiecewise(IReadOnlyDictionary<string, int>? p,
        out int boundary, out int leftSlope, out int leftIntercept, out int rightSlope,
        out int rightIntercept, out int side, out int variant)
    {
        boundary = leftSlope = leftIntercept = rightSlope = rightIntercept = side = variant = 0;
        return p is not null &&
            p.TryGetValue("boundary", out boundary) && boundary is >= 1 and <= 5 &&
            p.TryGetValue("leftSlope", out leftSlope) && leftSlope is >= 1 and <= 4 &&
            p.TryGetValue("leftIntercept", out leftIntercept) && leftIntercept is >= 0 and <= 7 &&
            p.TryGetValue("rightSlope", out rightSlope) && rightSlope is >= 1 and <= 4 &&
            p.TryGetValue("rightIntercept", out rightIntercept) && rightIntercept is >= 0 and <= 7 &&
            p.TryGetValue("side", out side) && side is >= 0 and <= 1 &&
            p.TryGetValue("variant", out variant) && variant is >= 0 and <= 15;
    }

    private static bool ValidateAbsoluteError(IReadOnlyDictionary<string, int>? p,
        out int target, out int guess, out int variant)
    {
        target = guess = variant = 0;
        return p is not null &&
            p.TryGetValue("target", out target) && target is >= 20 and <= 80 &&
            p.TryGetValue("guess", out guess) && guess is >= 5 and <= 95 &&
            Math.Abs(guess - target) is >= 1 and <= 15 &&
            p.TryGetValue("variant", out variant) && variant is >= 0 and <= 15;
    }

}
