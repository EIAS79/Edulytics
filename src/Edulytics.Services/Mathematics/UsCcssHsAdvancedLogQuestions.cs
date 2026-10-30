using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Exact, parameter-bounded log practice for the Coder 8 L07-L15 acceptance
/// scenarios. Does not attempt to autograde L13's open mathematical proof.
/// Invalid-domain questions are classification only; untrusted answers are
/// never evaluated as expressions.
/// </summary>
public static class UsCcssHsAdvancedLogQuestions
{
    public const string SolveShiftedDomain = "usccss.hs.log.shifted_domain_equation";
    public const string InvalidRealLog = "usccss.hs.log.invalid_real_domain";
    public const string CompareDifferentBases = "usccss.hs.log.compare_different_bases";
    public const string CheckProductIdentity = "usccss.hs.log.check_product_identity";
    public const string ChangeBase = "usccss.hs.log.change_of_base_equation";
    public const string InverseIdentity = "usccss.hs.log.inverse_identity";
    public static bool Supports(string? id) => id is
        SolveShiftedDomain or InvalidRealLog or CompareDifferentBases or
        CheckProductIdentity or ChangeBase or InverseIdentity;

    public static UsCcssHsFourthCourseQuestions.Item Build(string family, Random random)
    {
        if (!Supports(family))
            throw new ArgumentException("Unknown HS logarithm family.", nameof(family));
        var p = new Dictionary<string, int>(StringComparer.Ordinal);
        string prompt;
        switch (family)
        {
            case SolveShiftedDomain:
                p["base"] = random.Next(2, 7);
                p["shift"] = random.Next(1, 10);
                p["exponent"] = random.Next(-2, 4);
                // The solution x=shift+base^exponent is positive and
                // satisfies x-shift>0 even with negative exponents.
                prompt = $"Solve log base {p["base"]} of (x−{p["shift"]})={p["exponent"]} in the real domain. Enter x as an exact fraction.";
                break;
            case InvalidRealLog:
                p["mode"] = random.Next(0, 4);
                p["argument"] = random.Next(1, 10);
                prompt = p["mode"] switch
                {
                    0 => $"Classify real log base 1 of {p["argument"]}: defined or undefined?",
                    1 => $"Classify real log base 2 of −{p["argument"]}: defined or undefined?",
                    2 => "Classify real log base 2 of 0: defined or undefined?",
                    _ => $"Classify real log base −2 of {p["argument"]}: defined or undefined?"
                };
                break;
            case CompareDifferentBases:
                p["largerBase"] = random.Next(3, 12);
                p["smallerBase"] = random.Next(2, p["largerBase"]);
                p["numerator"] = random.Next(1, 13);
                p["denominator"] = random.Next(1, 13);
                // Avoid equality x=1; the relation would be equal.
                if (p["numerator"] == p["denominator"])
                    p["numerator"]++;
                prompt = $"Compare log base {p["largerBase"]} of ({p["numerator"]}/{p["denominator"]}) __ log base {p["smallerBase"]} of the same positive argument. Enter < or >.";
                break;
            case CheckProductIdentity:
                p["base"] = random.Next(2, 9);
                p["x"] = random.Next(2, 9);
                p["y"] = random.Next(2, 9);
                p["productCorrect"] = random.Next(0, 2);
                var right = p["productCorrect"] == 1 ? p["x"] * p["y"] : p["x"] + p["y"];
                if (p["productCorrect"] == 0 && right == p["x"] * p["y"])
                    right++;
                p["rhsArgument"] = right;
                prompt = $"Is log base {p["base"]} of {p["rhsArgument"]} = log base {p["base"]} of {p["x"]} + log base {p["base"]} of {p["y"]}? Answer true or false.";
                break;
            case ChangeBase:
                p["base"] = random.Next(2, 8);
                p["target"] = random.Next(2, 16);
                if (p["target"] == p["base"]) p["target"]++;
                prompt = $"Solve {p["base"]}^x={p["target"]} over the reals. Enter log_{p["base"]}({p["target"]}), ln({p["target"]})/ln({p["base"]}), or a correct decimal to 1e-9 absolute accuracy.";
                break;
            default:
                p["base"] = random.Next(2, 8);
                p["argument"] = random.Next(2, 18);
                prompt = $"Evaluate {p["base"]}^(log base {p["base"]} of {p["argument"]}) exactly.";
                break;
        }
        return new UsCcssHsFourthCourseQuestions.Item(
            family, prompt,
            "Apply positive-argument and valid-base restrictions, inverse laws, and monotonicity. Open proof requires a teacher-scored rubric.",
            AssessmentItemType.ShortAnswer, p);
    }

    public static string Solve(string family, IReadOnlyDictionary<string, int> p)
    {
        if (!Valid(family, p))
            throw new ArgumentException("Invalid logarithm question parameters.");
        return family switch
        {
            SolveShiftedDomain => ShiftedSolution(p),
            InvalidRealLog => "undefined",
            CompareDifferentBases => p["numerator"] > p["denominator"] ? "<" : ">",
            CheckProductIdentity => p["productCorrect"] == 1 ? "true" : "false",
            ChangeBase => $"log_{p["base"]}({p["target"]})",
            InverseIdentity => p["argument"].ToString(CultureInfo.InvariantCulture),
            _ => throw new ArgumentException("Unknown family.")
        };
    }

    public static bool Verify(string family, IReadOnlyDictionary<string, int> p, string? answer)
    {
        if (!Valid(family, p) || answer is null || answer.Length > 96)
            return false;
        var trimmed = answer.Trim();
        var expected = Solve(family, p);
        if (family is InvalidRealLog or CheckProductIdentity)
            return string.Equals(trimmed, expected, StringComparison.OrdinalIgnoreCase);
        if (family == CompareDifferentBases)
            return trimmed == expected;
        if (family == ChangeBase)
        {
            // Exact symbolic whitelist for the mathematically identical two
            // forms; no arbitrary expression parsing or unbounded algebra.
            var compact = string.Concat(trimmed.Where(c => !char.IsWhiteSpace(c)));
            if (compact.StartsWith("x=", StringComparison.OrdinalIgnoreCase))
                compact = compact[2..];
            if (compact == expected ||
                compact == $"ln({p["target"]})/ln({p["base"]})")
                return true;
            return double.TryParse(compact, NumberStyles.Float,
                    CultureInfo.InvariantCulture, out var candidate) &&
                double.IsFinite(candidate) &&
                Math.Abs(candidate - Math.Log(p["target"]) / Math.Log(p["base"])) <= 1e-9;
        }
        return EqualBoundedRationals(trimmed, expected);
    }

    private static bool Valid(string? family, IReadOnlyDictionary<string, int> p)
    {
        if (!Supports(family)) return false;
        try
        {
            return family switch
            {
                SolveShiftedDomain => R(p, "base") is >= 2 and <= 10 &&
                    R(p, "shift") is >= 0 and <= 30 && R(p, "exponent") is >= -3 and <= 6,
                InvalidRealLog => R(p, "mode") is >= 0 and <= 3 &&
                    R(p, "argument") is >= 1 and <= 100,
                CompareDifferentBases => R(p, "largerBase") is >= 3 and <= 20 &&
                    R(p, "smallerBase") >= 2 &&
                    R(p, "smallerBase") < R(p, "largerBase") &&
                    R(p, "numerator") is >= 1 and <= 100 &&
                    R(p, "denominator") is >= 1 and <= 100 &&
                    R(p, "numerator") != R(p, "denominator"),
                CheckProductIdentity => R(p, "base") is >= 2 and <= 10 &&
                    R(p, "x") is >= 2 and <= 20 && R(p, "y") is >= 2 and <= 20 &&
                    R(p, "productCorrect") is 0 or 1 &&
                    R(p, "rhsArgument") > 0 &&
                    R(p, "rhsArgument") == (R(p, "productCorrect") == 1
                        ? R(p, "x") * R(p, "y")
                        : R(p, "x") + R(p, "y") == R(p, "x") * R(p, "y")
                            ? R(p, "x") + R(p, "y") + 1
                            : R(p, "x") + R(p, "y")),
                ChangeBase => R(p, "base") is >= 2 and <= 10 && R(p, "target") is >= 1 and <= 100,
                InverseIdentity => R(p, "base") is >= 2 and <= 10 &&
                    R(p, "argument") is >= 1 and <= 100,
                _ => false
            };
        }
        catch (KeyNotFoundException) { return false; }
    }
    private static int R(IReadOnlyDictionary<string, int> p, string name) => p[name];
    private static string ShiftedSolution(IReadOnlyDictionary<string, int> p)
    {
        var b = p["base"];
        var power = p["exponent"];
        if (power >= 0)
        {
            var value = 1;
            for (var i = 0; i < power; i++) value = checked(value * b);
            return (p["shift"] + value).ToString(CultureInfo.InvariantCulture);
        }
        var d = 1;
        for (var i = 0; i < -power; i++) d = checked(d * b);
        return Rational(p["shift"] * d + 1, d);
    }
    private static string Rational(long n, long d)
    {
        var gcd = Gcd(Math.Abs(n), Math.Abs(d));
        return d / gcd == 1 ? (n / gcd).ToString(CultureInfo.InvariantCulture)
            : $"{n / gcd}/{d / gcd}";
    }
    private static long Gcd(long a, long b)
    {
        while (b != 0) { var t = a % b; a = b; b = t; }
        return Math.Max(a, 1);
    }
    private static bool EqualBoundedRationals(string answer, string expected)
    {
        if (answer.StartsWith("x=", StringComparison.OrdinalIgnoreCase))
            answer = answer[2..];
        if (!ParseRational(answer, out var n1, out var d1) ||
            !ParseRational(expected, out var n2, out var d2)) return false;
        return n1 * d2 == n2 * d1;
    }
    private static bool ParseRational(string value, out long n, out long d)
    {
        n = 0; d = 1;
        if (value.Length is < 1 or > 28) return false;
        var parts = value.Trim().Split('/');
        if (parts.Length is < 1 or > 2 ||
            !long.TryParse(parts[0], NumberStyles.AllowLeadingSign,
                CultureInfo.InvariantCulture, out n) || Math.Abs((decimal)n) > 1_000_000)
            return false;
        if (parts.Length == 2 &&
            (!long.TryParse(parts[1], NumberStyles.AllowLeadingSign,
                CultureInfo.InvariantCulture, out d) || d == 0 || Math.Abs((decimal)d) > 1_000_000))
            return false;
        return true;
    }
}
