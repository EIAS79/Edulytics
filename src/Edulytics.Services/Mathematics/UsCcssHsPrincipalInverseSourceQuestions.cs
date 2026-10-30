using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Typed inverse-trigonometry principal-branch source task. Real-domain and
/// wrong-branch states never become auto-graded numeric successes. Allowed
/// answers are a finite exact angle vocabulary or bounded radians.
/// </summary>
public static class UsCcssHsPrincipalInverseSourceQuestions
{
    public const string Family = "trig.principal_inverse_source_branches";
    public sealed record Result(string Status, string? Exact, double? Radians);
    private sealed record Example(string Prompt, string Answer, double PiCoefficient);

    public static bool Supports(string? family) => family == Family;

    public static UsCcssHsFourthCourseQuestions.Item Build(string family, Random random)
    {
        if (!Supports(family))
            throw new ArgumentException("Unsupported principal inverse family.", nameof(family));
        var mode = random.Next(0, 19);
        var example = ExampleForMode(mode);
        return new UsCcssHsFourthCourseQuestions.Item(
            family, example.Prompt + " Answer exactly in radians using pi or enter decimal radians.",
            "Apply the principal inverse range, not an arbitrary inverse branch.",
            AssessmentItemType.ShortAnswer,
            new Dictionary<string, int> { ["mode"] = mode });
    }

    public static Result Evaluate(IReadOnlyDictionary<string, int> p)
    {
        if (!p.TryGetValue("mode", out var mode))
            return new Result("MALFORMED", null, null);
        if (mode is 100 or 101)
            return new Result("OUT_OF_DOMAIN", null, null);
        if (mode == 102)
            return new Result("WRONG_BRANCH", null, null);
        if (mode is < 0 or >= 19)
            return new Result("MALFORMED", null, null);
        var ex = ExampleForMode(mode);
        return new Result("OK", ex.Answer, ex.PiCoefficient * Math.PI);
    }

    public static string Solve(string family, IReadOnlyDictionary<string, int> parameters)
    {
        if (!Supports(family))
            throw new ArgumentException("Unknown principal inverse family.", nameof(family));
        var result = Evaluate(parameters);
        if (result.Status != "OK" || result.Exact is null)
            throw new ArgumentException("Invalid principal inverse input.");
        return result.Exact;
    }

    public static bool Verify(string family, IReadOnlyDictionary<string, int> parameters, string? answer)
    {
        if (!Supports(family) || answer is null || answer.Length > 64)
            return false;
        var result = Evaluate(parameters);
        if (result.Status != "OK" || result.Exact is null ||
            result.Radians is null) return false;
        var token = string.Concat(answer.Trim().Where(x => !char.IsWhiteSpace(x)))
            .Replace("π", "pi", StringComparison.Ordinal);
        if (token == result.Exact) return true;
        if (token.EndsWith("rad", StringComparison.OrdinalIgnoreCase))
            token = token[..^3];
        return double.TryParse(token, NumberStyles.Float,
                   CultureInfo.InvariantCulture, out var candidate) &&
               double.IsFinite(candidate) &&
               Math.Abs(candidate - result.Radians.Value) <= 1e-9;
    }

    private static Example ExampleForMode(int mode) => mode switch
    {
        0 => new("Find principal arcsin(sin(5pi/6)).", "pi/6", 1d / 6),
        1 => new("Find principal arcsin(-sqrt(3)/2).", "-pi/3", -1d / 3),
        2 => new("Find principal arccos(-1/2).", "2pi/3", 2d / 3),
        3 => new("Find principal arctan(1).", "pi/4", 1d / 4),
        4 => new("Find principal arcsin(1/2).", "pi/6", 1d / 6),
        5 => new("Find principal arcsin(-1/2).", "-pi/6", -1d / 6),
        6 => new("Find principal arcsin(sqrt(3)/2).", "pi/3", 1d / 3),
        7 => new("Find principal arccos(1/2).", "pi/3", 1d / 3),
        8 => new("Find principal arccos(sqrt(3)/2).", "pi/6", 1d / 6),
        9 => new("Find principal arccos(-sqrt(3)/2).", "5pi/6", 5d / 6),
        10 => new("Find principal arctan(sqrt(3)).", "pi/3", 1d / 3),
        11 => new("Find principal arctan(1/sqrt(3)).", "pi/6", 1d / 6),
        12 => new("Find principal arctan(-1).", "-pi/4", -1d / 4),
        13 => new("Find principal arcsin(0).", "0", 0),
        14 => new("Find principal arccos(0).", "pi/2", 1d / 2),
        15 => new("Find principal arctan(0).", "0", 0),
        16 => new("Find principal arcsin(1).", "pi/2", 1d / 2),
        17 => new("Find principal arccos(-1).", "pi", 1),
        18 => new("Find principal arctan(-sqrt(3)).", "-pi/3", -1d / 3),
        _ => throw new ArgumentOutOfRangeException(nameof(mode))
    };
}
