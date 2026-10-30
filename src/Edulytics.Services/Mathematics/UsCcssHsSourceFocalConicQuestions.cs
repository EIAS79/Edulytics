using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Bounded exact axis-aligned ellipse/hyperbola equation task for Coder 9 L05.
/// Focal-distance geometry is an independent numerical oracle. Student
/// drawings, proofs and constructed focus strings remain teacher-reviewed.
/// </summary>
public static class UsCcssHsSourceFocalConicQuestions
{
    public const string Family = "coordinates.focal_conics_source_equation";
    public sealed record Result(string Status, string? Equation, int? BSquared);

    public static bool Supports(string? family) => family == Family;

    public static UsCcssHsFourthCourseQuestions.Item Build(string family, Random random)
    {
        if (!Supports(family)) throw new ArgumentException("Unknown conic family.");
        var hyperbola = random.Next(0, 2);
        var vertical = random.Next(0, 2);
        var c = random.Next(2, 7);
        var a = hyperbola == 1 ? random.Next(1, c) : random.Next(c + 1, c + 6);
        var cx = random.Next(-3, 4);
        var cy = random.Next(-3, 4);
        var p = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["hyperbola"] = hyperbola, ["vertical"] = vertical,
            ["a"] = a, ["c"] = c, ["centerX"] = cx, ["centerY"] = cy
        };
        return new UsCcssHsFourthCourseQuestions.Item(
            family,
            $"Write the canonical equation of the {(hyperbola == 1 ? "hyperbola" : "ellipse")} " +
            $"centered at ({cx},{cy}) with {(vertical == 1 ? "vertical" : "horizontal")} focal axis, " +
            $"a={a}, focal half-distance c={c}. Reject degenerate or contradictory focus data.",
            "For an ellipse require a>c and b²=a²−c²; for a hyperbola require c>a and b²=c²−a². " +
            "The focus distance relation must be checked; constructed diagrams and proofs require a teacher.",
            AssessmentItemType.ShortAnswer, p);
    }

    public static Result Evaluate(IReadOnlyDictionary<string, int> p)
    {
        if (!TryParameters(p, out var hyperbola, out var vertical, out var a,
                out var c, out var cx, out var cy))
            return new Result("OUT_OF_DOMAIN", null, null);
        var b2 = hyperbola ? checked(c * c - a * a) : checked(a * a - c * c);
        var major = vertical ? Squared("y", cy) : Squared("x", cx);
        var minor = vertical ? Squared("x", cx) : Squared("y", cy);
        var sep = hyperbola ? "-" : "+";
        var equation = $"{major}/{a * a}{sep}{minor}/{b2}=1";
        // Standard horizontal ellipse must show x^2/a^2+y^2/b^2.
        // Standard vertical ellipse is y^2/a^2+x^2/b^2.
        // Both are intentionally canonical with major/focal axis first.
        return new Result("OK", equation, b2);
    }

    public static string Solve(string family, IReadOnlyDictionary<string, int> p)
    {
        if (!Supports(family)) throw new ArgumentException("Unknown conic family.");
        var result = Evaluate(p);
        if (result.Status != "OK" || result.Equation is null)
            throw new ArgumentException("Invalid or degenerate conic parameters.");
        return result.Equation;
    }

    public static bool Verify(string family, IReadOnlyDictionary<string, int> p, string? answer)
    {
        if (!Supports(family) || answer is null || answer.Length > 128)
            return false;
        var result = Evaluate(p);
        if (result.Status != "OK" || result.Equation is null) return false;
        var normalized = string.Concat(answer.Where(ch => !char.IsWhiteSpace(ch)));
        if (normalized == result.Equation) return true;
        // Ellipse addition is commutative, but hyperbola subtraction is not.
        if (!p.TryGetValue("hyperbola", out var h) || h != 0) return false;
        var split = result.Equation.Split('+');
        return split.Length == 2 &&
            normalized == split[1][..^2] + "+" + split[0] + "=1";
    }

    /// <summary>
    /// Independent focal condition: ellipse SUM of distances equals 2a,
    /// hyperbola ABSOLUTE DIFFERENCE equals 2a. Reject all invalid conics.
    /// This validates coordinates but is not a substitute for authored proof.
    /// </summary>
    public static bool IsPointOnConic(
        IReadOnlyDictionary<string, int> p, double x, double y)
    {
        if (!TryParameters(p, out var hyperbola, out var vertical, out var a,
                out var c, out var cx, out var cy) ||
            !double.IsFinite(x) || !double.IsFinite(y))
            return false;
        var f1x = vertical ? cx : cx - c;
        var f2x = vertical ? cx : cx + c;
        var f1y = vertical ? cy - c : cy;
        var f2y = vertical ? cy + c : cy;
        var d1 = Math.Sqrt((x - f1x) * (x - f1x) + (y - f1y) * (y - f1y));
        var d2 = Math.Sqrt((x - f2x) * (x - f2x) + (y - f2y) * (y - f2y));
        var focal = hyperbola ? Math.Abs(d1 - d2) : d1 + d2;
        return Math.Abs(focal - 2.0 * a) <= 1e-9;
    }

    private static string Squared(string variable, int center)
    {
        if (center == 0) return $"{variable}^2";
        return center > 0
            ? $"({variable}-{center})^2"
            : $"({variable}+{-center})^2";
    }

    private static bool TryParameters(IReadOnlyDictionary<string, int> p,
        out bool hyperbola, out bool vertical, out int a, out int c,
        out int cx, out int cy)
    {
        hyperbola = false; vertical = false; a = c = cx = cy = 0;
        if (!p.TryGetValue("hyperbola", out var h) || h is < 0 or > 1 ||
            !p.TryGetValue("vertical", out var v) || v is < 0 or > 1 ||
            !p.TryGetValue("a", out a) || !p.TryGetValue("c", out c))
            return false;
        cx = p.TryGetValue("centerX", out var x) ? x : 0;
        cy = p.TryGetValue("centerY", out var y) ? y : 0;
        if (a is < 1 or > 50 || c is < 1 or > 50 ||
            cx is < -50 or > 50 || cy is < -50 or > 50) return false;
        hyperbola = h == 1;
        vertical = v == 1;
        return hyperbola ? c > a : a > c;
    }
}
