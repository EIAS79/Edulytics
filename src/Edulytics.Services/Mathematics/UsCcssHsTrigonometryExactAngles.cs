using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Source-angle unit-circle identities for Coder 9 L01. Uses a discrete exact
/// angle domain rather than trigonometric floating-point equality, and returns
/// an explicit OUT_OF_DOMAIN status for tangent poles instead of scoring them.
/// </summary>
public static class UsCcssHsTrigonometryExactAngles
{
    public const string Family = "trig.unit_circle_source_exact_angles";
    public sealed record Result(string Status, string? Exact, double? Value);
    private static readonly int[] SeedAngles = [2, 3, 4, 6];

    public static bool Supports(string? family) => family == Family;

    public static UsCcssHsFourthCourseQuestions.Item Build(string family, Random random)
    {
        if (!Supports(family)) throw new ArgumentException("Unsupported exact-angle family.");
        var attempts = 0;
        while (attempts++ < 80)
        {
            var twelfths = SeedAngles[random.Next(SeedAngles.Length)];
            var function = random.Next(0, 3);
            var transformation = random.Next(0, 3);
            var p = new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["twelfths"] = twelfths,
                ["function"] = function,
                ["transform"] = transformation
            };
            var result = Evaluate(p);
            if (result.Status != "OK") continue;
            var fn = function switch { 0 => "sin", 1 => "cos", _ => "tan" };
            var theta = twelfths switch
            {
                2 => "π/6", 3 => "π/4", 4 => "π/3", _ => "π/2"
            };
            var argument = transformation switch
            {
                0 => "π−θ", 1 => "π+θ", _ => "2π−θ"
            };
            return new UsCcssHsFourthCourseQuestions.Item(
                family, $"Evaluate exactly {fn}({argument}) with θ={theta}. Enter a reduced radical or signed rational; do not submit an expression involving arbitrary variables.",
                "Use unit-circle quadrant signs, periodicity and the exact special-angle coordinates.",
                AssessmentItemType.ShortAnswer, p);
        }
        throw new InvalidOperationException("No bounded valid trigonometric sample available.");
    }

    public static Result Evaluate(IReadOnlyDictionary<string, int> p)
    {
        if (!Valid(p)) return new Result("MALFORMED", null, null);
        var angle = p["twelfths"];
        var transformed = p["transform"] switch
        {
            0 => 12 - angle, 1 => 12 + angle, _ => 24 - angle
        };
        var t = ((transformed % 24) + 24) % 24;
        if (p["function"] == 2)
        {
            var unit = t % 12;
            if (unit == 6) return new Result("OUT_OF_DOMAIN", null, null);
            var negative = unit > 6;
            var abs = negative ? 12 - unit : unit;
            var exact = abs switch
            {
                0 => "0", 2 => "sqrt(3)/3", 3 => "1", 4 => "sqrt(3)",
                _ => throw new ArgumentOutOfRangeException(nameof(p))
            };
            return Make(exact, negative);
        }
        var sin = p["function"] == 0;
        var whole = t > 12 ? 24 - t : t;
        var negativeValue = sin ? t > 12 : whole > 6;
        var reference = whole > 6 ? 12 - whole : whole;
        var magnitude = (sin, reference) switch
        {
            (true, 0) or (false, 6) => "0",
            (true, 2) or (false, 4) => "1/2",
            (true, 3) or (false, 3) => "sqrt(2)/2",
            (true, 4) or (false, 2) => "sqrt(3)/2",
            (true, 6) or (false, 0) => "1",
            _ => throw new ArgumentOutOfRangeException(nameof(p))
        };
        return Make(magnitude, negativeValue);
    }

    public static string Solve(string family, IReadOnlyDictionary<string, int> p)
    {
        if (!Supports(family)) throw new ArgumentException("Unsupported family.", nameof(family));
        var result = Evaluate(p);
        if (result.Status != "OK" || result.Exact is null)
            throw new ArgumentException("Domain or parameter restriction.");
        return result.Exact;
    }

    public static bool Verify(string family, IReadOnlyDictionary<string, int> p, string? answer)
    {
        if (!Supports(family) || answer is null || answer.Length > 48) return false;
        var expected = Evaluate(p);
        // A tangent pole is a mathematical domain check, NOT a numerical
        // assessment item. Do not report a fictitious zero or one as its score.
        if (expected.Status != "OK" || expected.Exact is null ||
            expected.Value is null) return false;
        var token = string.Concat(answer.Trim().Where(x => !char.IsWhiteSpace(x)));
        if (token == expected.Exact) return true;
        var alternate = expected.Exact switch
        {
            "sqrt(2)/2" => "1/sqrt(2)",
            "-sqrt(2)/2" => "-1/sqrt(2)",
            "sqrt(3)/3" => "1/sqrt(3)",
            "-sqrt(3)/3" => "-1/sqrt(3)",
            _ => ""
        };
        if (alternate.Length > 0 && token == alternate) return true;
        return double.TryParse(token, NumberStyles.Float,
                   CultureInfo.InvariantCulture, out var value) &&
               double.IsFinite(value) &&
               Math.Abs(value - expected.Value.Value) <= 1e-9;
    }

    private static bool Valid(IReadOnlyDictionary<string, int> p) =>
        p.TryGetValue("twelfths", out var twelfths) &&
        SeedAngles.Contains(twelfths) &&
        p.TryGetValue("function", out var function) && function is >= 0 and <= 2 &&
        p.TryGetValue("transform", out var transform) && transform is >= 0 and <= 2;

    private static Result Make(string magnitude, bool negative)
    {
        var sign = negative && magnitude != "0" ? "-" : "";
        var numeric = magnitude switch
        {
            "0" => 0d, "1/2" => 0.5, "1" => 1d,
            "sqrt(2)/2" => Math.Sqrt(2) / 2,
            "sqrt(3)/2" => Math.Sqrt(3) / 2,
            "sqrt(3)/3" => Math.Sqrt(3) / 3,
            "sqrt(3)" => Math.Sqrt(3),
            _ => throw new ArgumentException("Unknown exact-angle magnitude.")
        };
        return new Result("OK", sign + magnitude, negative ? -numeric : numeric);
    }
}
