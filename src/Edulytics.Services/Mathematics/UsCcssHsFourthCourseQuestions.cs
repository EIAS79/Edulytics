using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Isolated high-school Fourth Courses numerical Practice kernel.
/// Every accepted answer is reconstructed from bounded integer parameters;
/// no expression evaluation, arbitrary C# evaluation, or approximate string matching.
/// Academic proof/explanation tasks are deliberately outside automatic grading.
/// </summary>
public static class UsCcssHsFourthCourseQuestions
{
    public const string MatrixCoefficients = "usccss.hs.matrix.coefficients_2x2";
    public const string MatrixSolve = "usccss.hs.matrix.solve_2x2";
    public const string MatrixInverse = "usccss.hs.matrix.inverse_2x2";
    public const string MatrixClassification = "usccss.hs.matrix.classify_2x2";
    public const string LogEvaluate = "usccss.hs.log.inverse_power";
    public const string LogCompare = "usccss.hs.log.compare_order";
    public const string TrigReflection = "trig.unit_circle_reflection_exact";
    public const string TrigPrincipalInverse = "trig.principal_inverse";
    public const string TrigModel = "trig.model_all_time_solutions";
    public const string CavalieriVolume = "solid_geometry.cavalieri_sphere_volume";
    public const string FocalConics = "coordinates.focal_distance_conics";

    private static readonly HashSet<string> Families = new(StringComparer.Ordinal)
    {
        MatrixCoefficients, MatrixSolve, MatrixInverse, MatrixClassification,
        LogEvaluate, LogCompare, TrigReflection, TrigPrincipalInverse,
        TrigModel, CavalieriVolume, FocalConics
    };

    public sealed record Item(
        string Family, string Prompt, string Solution, AssessmentItemType ItemType,
        IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) => family is not null && Families.Contains(family);

    public static Item Build(string family, Random random)
    {
        if (!Supports(family))
            throw new ArgumentException("Unsupported Fourth Courses family.", nameof(family));

        if (family is MatrixCoefficients or MatrixSolve or MatrixInverse or MatrixClassification)
        {
            var mode = family == MatrixClassification ? random.Next(0, 3) : 0;
            int a, b, c, d;
            do
            {
                a = random.Next(-4, 5);
                b = random.Next(-4, 5);
                c = random.Next(-4, 5);
                d = random.Next(-4, 5);
            }
            while (a * d == b * c);
            if (mode != 0)
            {
                a = random.Next(1, 5);
                b = random.Next(1, 5);
                c = 2 * a;
                d = 2 * b;
            }
            var x = random.Next(-4, 5);
            var y = random.Next(-4, 5);
            var e = a * x + b * y;
            var f = c * x + d * y + (mode == 2 ? 1 : 0);
            var parameters = P(("a", a), ("b", b), ("c", c), ("d", d),
                ("e", e), ("f", f), ("mode", mode));
            var equation = $"System: {a}x + ({b})y = {e}; {c}x + ({d})y = {f}.";
            var (prompt, solution) = family switch
            {
                MatrixCoefficients => ($"{equation} Write A and b as a,b;c,d|e,f using row order.",
                    "Coefficient rows follow the original variable order x,y; the right column is b."),
                MatrixInverse => ($"For A = [{a},{b};{c},{d}], write A inverse as four row-major entries: a,b;c,d. Fractions allowed.",
                    "Check each row-column product against the identity matrix; det(A) must be nonzero."),
                MatrixSolve => ($"{equation} Give the ordered solution x,y (fractions allowed).",
                    "Use the original two-equation coefficient system; substitute into both original equations."),
                _ => ($"{equation} Classify: unique, infinite, or none.",
                    "Check coefficient and augmented ranks: consistent singular means infinite solutions; inconsistent means none.")
            };
            return new Item(family, prompt, solution, AssessmentItemType.ShortAnswer, parameters);
        }

        if (family == LogEvaluate)
        {
            var b = random.Next(2, 6);
            var exponent = random.Next(-3, 6);
            var argument = exponent < 0 ? $"1/{Pow(b, -exponent)}" : Pow(b, exponent).ToString(CultureInfo.InvariantCulture);
            return new Item(family,
                $"Evaluate log base {b} of ({argument}). Enter the exponent.",
                "The logarithm is the unique exponent producing the positive argument from the positive base.",
                AssessmentItemType.ShortAnswer, P(("base", b), ("exponent", exponent)));
        }
        if (family == LogCompare)
        {
            var decreasing = random.Next(0, 2);
            var low = random.Next(1, 7);
            var high = low + random.Next(1, 5);
            return new Item(family,
                $"Compare log base {(decreasing == 1 ? "1/2" : "2")} of {low} __ log base {(decreasing == 1 ? "1/2" : "2")} of {high}. Enter < or >.",
                "Logarithms increase for base >1 and decrease for base strictly between 0 and 1.",
                AssessmentItemType.ShortAnswer, P(("decreasing", decreasing), ("low", low), ("high", high)));
        }
        if (family == TrigReflection)
        {
            var angle = random.Next(0, 4);
            var function = random.Next(0, 2);
            var reflection = random.Next(0, 3);
            var angleQuarterTurns = reflection switch
            {
                0 => 2 - angle,   // pi-theta
                1 => 2 + angle,   // pi+theta
                _ => 4 - angle   // 2pi-theta
            };
            return new Item(family,
                $"Find {(function == 0 ? "sin" : "cos")}({(reflection == 0 ? "π−θ" : reflection == 1 ? "π+θ" : "2π−θ")}) when θ={angle}π/2. Answer -1, 0 or 1.",
                "Use the reflection/periodicity identity and the original unit-circle coordinates.",
                AssessmentItemType.ShortAnswer, P(("angle", angle), ("function", function), ("reflection", reflection), ("quarterTurns", angleQuarterTurns)));
        }
        if (family == TrigPrincipalInverse)
        {
            var function = random.Next(0, 3);
            // Seven valid exact inputs per function provide >10 distinct
            // nonduplicated prompts per session without approximate arithmetic.
            var argument = random.Next(-3, 4);
            var literal = function == 2
                ? argument switch { -3 => "-sqrt(3)", -2 => "-1/sqrt(3)", 2 => "1/sqrt(3)", 3 => "sqrt(3)", _ => argument.ToString(CultureInfo.InvariantCulture) }
                : argument switch { -3 => "-sqrt(3)/2", -2 => "-1/2", 2 => "1/2", 3 => "sqrt(3)/2", _ => argument.ToString(CultureInfo.InvariantCulture) };
            return new Item(family,
                $"Principal {(function == 0 ? "arcsin" : function == 1 ? "arccos" : "arctan")}({literal}) = ? Give the coefficient of π (e.g. 1/2 for π/2).",
                "Respect the principal range: asin [-π/2,π/2], acos [0,π], atan (-π/2,π/2).",
                AssessmentItemType.ShortAnswer, P(("function", function), ("argument", argument)));
        }
        if (family == TrigModel)
        {
            var period = random.Next(1, 7) * 4;
            var amplitude = random.Next(1, 6) * 2;
            var offset = random.Next(-4, 5);
            // 0, 1/2, -1/2, 1, -1 and invalid (>1) sine targets.
            // This closed exact set avoids approximating or omitting a branch.
            var targetMode = random.Next(0, 6);
            var endInclusive = random.Next(0, 2);
            var delta = targetMode switch
            {
                0 => 0, 1 => amplitude / 2, 2 => -amplitude / 2,
                3 => amplitude, 4 => -amplitude, _ => 2 * amplitude
            };
            var target = offset + delta;
            var closeMark = endInclusive == 1 ? "≤" : "<";
            return new Item(family,
                $"A(t)={offset}+{amplitude}sin(2πt/{period}), 0≤t{closeMark}{period} seconds. List ALL times when A(t)={target}, ascending as exact fractions in seconds; write none if no real solution.",
                "Check both sine branches and the stated interval endpoints. Reject outside-range targets.",
                AssessmentItemType.ShortAnswer, P(("period", period), ("amplitude", amplitude), ("offset", offset),
                    ("targetMode", targetMode), ("target", target), ("endInclusive", endInclusive)));
        }
        if (family == CavalieriVolume)
        {
            var radius = random.Next(1, 16);
            return new Item(family,
                $"A sphere has radius {radius} cm. Its volume is kπ cm³. Find the exact rational coefficient k.",
                "Cavalieri's equal-height cross-section comparison motivates V=4πr³/3; the informal proof needs teacher review.",
                AssessmentItemType.ShortAnswer, P(("radius", radius)));
        }
        {
            var hyperbola = random.Next(0, 2);
            var c = random.Next(2, 6);
            var a = hyperbola == 1 ? random.Next(1, c) : c + random.Next(1, 5);
            return new Item(family,
                $"An axis-aligned {(hyperbola == 1 ? "hyperbola" : "ellipse")} has center (0,0), focal half-distance c={c}, and semiaxis a={a}. Find b².",
                "For an ellipse b²=a²−c², a>c; for a hyperbola b²=c²−a², c>a. Full conic construction/proof is teacher-reviewed.",
                AssessmentItemType.ShortAnswer, P(("hyperbola", hyperbola), ("c", c), ("a", a)));
        }
    }

    public static string Solve(string family, IReadOnlyDictionary<string, int> p)
    {
        if (!Supports(family) || !ValidParameters(family, p))
            throw new ArgumentException("Invalid exact Fourth Courses question parameters.");
        // Other families intentionally have no matrix coefficients.
        // Access matrix parameters only for the four matrix question families.
        var isMatrix = family is MatrixCoefficients or MatrixSolve or
            MatrixInverse or MatrixClassification;
        var a = isMatrix ? Get(p, "a") : 0;
        var b = isMatrix ? Get(p, "b") : 0;
        var c = isMatrix ? Get(p, "c") : 0;
        var d = isMatrix ? Get(p, "d") : 0;
        var e = isMatrix ? Get(p, "e") : 0;
        var f = isMatrix ? Get(p, "f") : 0;
        var det = isMatrix ? checked(a * d - b * c) : 0;
        return family switch
        {
            MatrixCoefficients => $"{a},{b};{c},{d}|{e},{f}",
            MatrixClassification => det != 0 ? "unique" : a * f == c * e && b * f == d * e ? "infinite" : "none",
            MatrixSolve => $"{Format(d * e - b * f, det)},{Format(a * f - c * e, det)}",
            MatrixInverse => $"{Format(d, det)},{Format(-b, det)};{Format(-c, det)},{Format(a, det)}",
            LogEvaluate => Get(p, "exponent").ToString(CultureInfo.InvariantCulture),
            LogCompare => Get(p, "decreasing") == 1 ? ">" : "<",
            TrigReflection => (Get(p, "function") == 0
                ? (Get(p, "quarterTurns") % 4) switch { 1 => 1, 3 or -1 => -1, _ => 0 }
                : ((Get(p, "quarterTurns") % 4 + 4) % 4) switch { 0 => 1, 2 => -1, _ => 0 }).ToString(CultureInfo.InvariantCulture),
            TrigPrincipalInverse => Principal(Get(p, "function"), Get(p, "argument")),
            TrigModel => TrigModelAnswer(p),
            CavalieriVolume => Format(4 * Get(p, "radius") * Get(p, "radius") * Get(p, "radius"), 3),
            FocalConics => (Get(p, "hyperbola") == 1
                ? Get(p, "c") * Get(p, "c") - Get(p, "a") * Get(p, "a")
                : Get(p, "a") * Get(p, "a") - Get(p, "c") * Get(p, "c")).ToString(CultureInfo.InvariantCulture),
            _ => throw new InvalidOperationException("Unknown family")
        };
    }

    public static bool Verify(string family, IReadOnlyDictionary<string, int> p, string? answer)
    {
        if (!Supports(family) || answer is null || answer.Length > 128 || !ValidParameters(family, p))
            return false;
        var expected = Solve(family, p);
        if (family == MatrixClassification)
            return string.Equals(answer.Trim(), expected, StringComparison.OrdinalIgnoreCase);
        if (family == LogCompare || (family == TrigModel && ModelMode(p) == 5))
            return string.Equals(answer.Trim(), expected, StringComparison.Ordinal);
        if (family is MatrixCoefficients or MatrixInverse)
        {
            var parts = answer.Split('|');
            var canonical = expected.Split('|');
            if (parts.Length != canonical.Length) return false;
            for (var i = 0; i < parts.Length; i++)
            {
                var rows = parts[i].Split(';');
                var expectedRows = canonical[i].Split(';');
                if (rows.Length != expectedRows.Length) return false;
                for (var j = 0; j < rows.Length; j++)
                {
                    var cells = rows[j].Split(',');
                    var expectedCells = expectedRows[j].Split(',');
                    if (cells.Length != expectedCells.Length) return false;
                    for (var k = 0; k < cells.Length; k++)
                        if (!SameRational(cells[k], expectedCells[k])) return false;
                }
            }
            return true;
        }
        if (family is MatrixSolve or TrigModel)
        {
            var entered = answer.Split(',');
            var solved = expected.Split(',');
            return entered.Length == solved.Length &&
                   entered.Zip(solved).All(pair =>
                       family == TrigModel
                           ? SameModelTime(pair.First, pair.Second)
                           : SameRational(pair.First, pair.Second));
        }
        return SameRational(answer, expected);
    }

    private static bool ValidParameters(string family, IReadOnlyDictionary<string, int> p)
    {
        try
        {
            if (family is MatrixCoefficients or MatrixInverse or MatrixSolve or MatrixClassification)
            {
                var det = checked(Get(p, "a") * Get(p, "d") - Get(p, "b") * Get(p, "c"));
                if (Math.Abs(Get(p, "a")) > 20 || Math.Abs(Get(p, "b")) > 20 ||
                    Math.Abs(Get(p, "c")) > 20 || Math.Abs(Get(p, "d")) > 20 ||
                    Math.Abs(Get(p, "e")) > 200 || Math.Abs(Get(p, "f")) > 200)
                    return false;
                return family == MatrixClassification || det != 0;
            }
            return family switch
            {
                LogEvaluate => Get(p, "base") is >= 2 and <= 8 && Get(p, "exponent") is >= -8 and <= 8,
                LogCompare => Get(p, "decreasing") is 0 or 1 && Get(p, "low") > 0 && Get(p, "high") > Get(p, "low"),
                TrigReflection => Get(p, "angle") is >= 0 and <= 3 && Get(p, "function") is 0 or 1 &&
                    Get(p, "reflection") is >= 0 and <= 2 && Get(p, "quarterTurns") ==
                    (Get(p, "reflection") switch { 0 => 2 - Get(p, "angle"), 1 => 2 + Get(p, "angle"), _ => 4 - Get(p, "angle") }),
                TrigPrincipalInverse => Get(p, "function") is >= 0 and <= 2 && Get(p, "argument") is >= -3 and <= 3,
                TrigModel => Get(p, "period") is >= 1 and <= 100 &&
                    Get(p, "amplitude") is >= 1 and <= 100 &&
                    (ModelMode(p) is not (1 or 2) || Get(p, "amplitude") % 2 == 0) &&
                    Math.Abs(Get(p, "offset")) <= 100 && ModelMode(p) is >= 0 and <= 5 &&
                    ModelEnd(p) is 0 or 1 &&
                    (!p.TryGetValue("target", out var suppliedTarget) ||
                     (ModelMode(p) == 5
                         ? suppliedTarget > Get(p, "offset") + Get(p, "amplitude") ||
                           suppliedTarget < Get(p, "offset") - Get(p, "amplitude")
                         : suppliedTarget == Get(p, "offset") + ModelTargetDelta(p))),
                CavalieriVolume => Get(p, "radius") is >= 1 and <= 100,
                FocalConics => Get(p, "hyperbola") is 0 or 1 &&
                    Get(p, "a") > 0 && Get(p, "c") > 0 &&
                    (Get(p, "hyperbola") == 1 ? Get(p, "a") < Get(p, "c") : Get(p, "a") > Get(p, "c")),
                _ => false
            };
        }
        catch (Exception ex) when (ex is KeyNotFoundException or OverflowException or ArgumentException)
        {
            return false;
        }
    }

    private static int ModelMode(IReadOnlyDictionary<string, int> p) =>
        p.TryGetValue("targetMode", out var mode) ? mode : 0;

    private static int ModelEnd(IReadOnlyDictionary<string, int> p) =>
        p.TryGetValue("endInclusive", out var inclusive) ? inclusive : 1;

    private static int ModelTargetDelta(IReadOnlyDictionary<string, int> p) =>
        ModelMode(p) switch
        {
            0 => 0, 1 => Get(p, "amplitude") / 2,
            2 => -Get(p, "amplitude") / 2,
            3 => Get(p, "amplitude"), 4 => -Get(p, "amplitude"),
            5 => 2 * Get(p, "amplitude"),
            _ => throw new ArgumentException("Invalid sine target mode.")
        };

    private static string TrigModelAnswer(IReadOnlyDictionary<string, int> p)
    {
        var period = Get(p, "period");
        return ModelMode(p) switch
        {
            0 => ModelEnd(p) == 1
                ? $"0,{Format(period, 2)},{period}"
                : $"0,{Format(period, 2)}",
            1 => $"{Format(period, 12)},{Format(5 * period, 12)}",
            2 => $"{Format(7 * period, 12)},{Format(11 * period, 12)}",
            3 => Format(period, 4),
            4 => Format(3 * period, 4),
            5 => "none",
            _ => throw new ArgumentException("Invalid sine target.")
        };
    }

    private static int Get(IReadOnlyDictionary<string, int> p, string key) => p[key];
    private static Dictionary<string, int> P(params (string Name, int Value)[] entries) =>
        entries.ToDictionary(e => e.Name, e => e.Value, StringComparer.Ordinal);

    private static int Pow(int b, int exponent)
    {
        var value = 1;
        for (var i = 0; i < exponent; i++) value = checked(value * b);
        return value;
    }

    private static string Principal(int fn, int arg) => fn switch
    {
        0 => arg switch
        {
            -3 => "-1/3", -2 => "-1/6", -1 => "-1/2", 0 => "0",
            1 => "1/2", 2 => "1/6", 3 => "1/3",
            _ => throw new ArgumentException()
        },
        1 => arg switch
        {
            -3 => "5/6", -2 => "2/3", -1 => "1", 0 => "1/2",
            1 => "0", 2 => "1/3", 3 => "1/6",
            _ => throw new ArgumentException()
        },
        2 => arg switch
        {
            -3 => "-1/3", -2 => "-1/6", -1 => "-1/4", 0 => "0",
            1 => "1/4", 2 => "1/6", 3 => "1/3",
            _ => throw new ArgumentException()
        },
        _ => throw new ArgumentException()
    };

    private static string Format(long numerator, long denominator)
    {
        if (denominator == 0) throw new ArgumentException("Zero denominator");
        if (denominator < 0) { numerator = -numerator; denominator = -denominator; }
        var gcd = Gcd(Math.Abs(numerator), denominator);
        numerator /= gcd; denominator /= gcd;
        return denominator == 1
            ? numerator.ToString(CultureInfo.InvariantCulture)
            : $"{numerator}/{denominator}";
    }

    private static long Gcd(long a, long b)
    {
        while (b != 0) { var t = a % b; a = b; b = t; }
        return Math.Max(a, 1);
    }

    // A displayed contextual time may be an exact fraction OR an equivalent
    // bounded decimal. Never parse learner-supplied algebraic expressions.
    private static bool SameModelTime(string entered, string expected)
    {
        if (SameRational(entered, expected)) return true;
        if (entered.Length > 32 || !TryRational(expected, out var numerator, out var denominator))
            return false;
        return double.TryParse(entered.Trim(), NumberStyles.Float,
                   CultureInfo.InvariantCulture, out var candidate) &&
               double.IsFinite(candidate) &&
               Math.Abs(candidate - (double)numerator / denominator) <= 1e-9;
    }

    private static bool SameRational(string entered, string expected)
    {
        if (!TryRational(entered, out var n1, out var d1) ||
            !TryRational(expected, out var n2, out var d2))
            return false;
        return n1 * d2 == n2 * d1;
    }

    private static bool TryRational(string text, out long numerator, out long denominator)
    {
        numerator = 0; denominator = 1;
        text = text.Trim();
        if (text.Length is 0 or > 24) return false;
        var parts = text.Split('/');
        if (parts.Length is < 1 or > 2 ||
            !long.TryParse(parts[0], NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out numerator) ||
            Math.Abs(numerator) > 1000000)
            return false;
        if (parts.Length == 2 &&
            (!long.TryParse(parts[1], NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out denominator) ||
             denominator == 0 || Math.Abs(denominator) > 1000000))
            return false;
        return true;
    }
}
