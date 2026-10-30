using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Bounded diagnostic classification for Coder 8 L01 M07/M08/M10.
/// Unlike the inverse-computation family, it intentionally includes singular,
/// non-square and right-hand-side dimension-mismatch inputs. Classifying
/// validity does not auto-grade a learner's explanatory reasoning.
/// </summary>
public static class UsCcssHsMatrixInverseEligibilityQuestions
{
    public const string Family = "usccss.hs.matrix.inverse_eligibility";
    public sealed record Evaluation(string Status, string? Classification);

    public static bool Supports(string? family) => family == Family;

    public static IReadOnlyDictionary<string, int> ParametersForCase(int caseMode, int variation = 0)
    {
        if (caseMode is < 0 or > 3 || variation is < 0 or > 8)
            throw new ArgumentOutOfRangeException(nameof(caseMode));
        var p = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["columns"] = caseMode == 2 ? 3 : 2,
            ["rhsLength"] = caseMode == 1 ? 3 : 2,
            ["a00"] = 2 + variation, ["a01"] = 1, ["a02"] = 0,
            ["a10"] = 1, ["a11"] = 1, ["a12"] = 0,
            ["b0"] = 5, ["b1"] = 3, ["b2"] = caseMode == 1 ? 1 : 0
        };
        if (caseMode == 0)
        {
            p["a00"] = 1 + variation;
            p["a01"] = 2;
            p["a10"] = 2 * p["a00"];
            p["a11"] = 4;
        }
        else if (caseMode == 2)
        {
            p["a02"] = 3;
            p["a12"] = 2;
        }
        return p;
    }

    public static UsCcssHsFourthCourseQuestions.Item Build(string family, Random random)
    {
        if (!Supports(family)) throw new ArgumentException("Unsupported family.", nameof(family));
        var p = ParametersForCase(random.Next(0, 4), random.Next(0, 9));
        var width = p["columns"];
        var matrix = string.Join(";", Enumerable.Range(0, 2).Select(row =>
            string.Join(",", Enumerable.Range(0, width).Select(column =>
                p[$"a{row}{column}"].ToString(CultureInfo.InvariantCulture)))));
        var right = string.Join(",", Enumerable.Range(0, p["rhsLength"]).Select(i =>
            p[$"b{i}"].ToString(CultureInfo.InvariantCulture)));
        return new UsCcssHsFourthCourseQuestions.Item(
            family,
            $"Given A=[{matrix}] and b=[{right}], classify whether A^-1 b is defined. " +
            "Choose exactly one: invertible, singular, dimension_mismatch, not_square.",
            "A two-sided inverse exists only for a non-singular square matrix; A^-1 b additionally requires matching dimensions.",
            AssessmentItemType.ShortAnswer, p);
    }

    public static Evaluation Evaluate(IReadOnlyDictionary<string, int> p)
    {
        if (p is null || !p.TryGetValue("columns", out var width) ||
            !p.TryGetValue("rhsLength", out var length) ||
            width is not (2 or 3) || length is not (2 or 3))
            return new Evaluation("MALFORMED", null);
        foreach (var key in new[] { "a00", "a01", "a02", "a10", "a11", "a12", "b0", "b1", "b2" })
            if (!p.TryGetValue(key, out var value) || Math.Abs((long)value) > 100)
                return new Evaluation("MALFORMED", null);
        if (width == 2 && (p["a02"] != 0 || p["a12"] != 0))
            return new Evaluation("MALFORMED", null);
        if (length == 2 && p["b2"] != 0)
            return new Evaluation("MALFORMED", null);
        if (width == 3 && length != 2)
            return new Evaluation("MALFORMED", null);

        if (width != 2) return new Evaluation("OK", "not_square");
        var determinant = (long)p["a00"] * p["a11"] - (long)p["a01"] * p["a10"];
        // Reject ambiguous cases carrying both a singularity and b-size error.
        if (length != 2 && determinant == 0)
            return new Evaluation("MALFORMED", null);
        if (determinant == 0) return new Evaluation("OK", "singular");
        if (length != 2) return new Evaluation("OK", "dimension_mismatch");
        return new Evaluation("OK", "invertible");
    }

    public static string Solve(string family, IReadOnlyDictionary<string, int> p)
    {
        if (!Supports(family)) throw new ArgumentException("Unsupported family.", nameof(family));
        var result = Evaluate(p);
        if (result.Status != "OK" || result.Classification is null)
            throw new ArgumentException("Malformed matrix dimensions or coefficients.", nameof(p));
        return result.Classification;
    }

    public static bool Verify(string family, IReadOnlyDictionary<string, int> p, string? answer)
    {
        if (!Supports(family) || answer is null || answer.Length > 32) return false;
        var result = Evaluate(p);
        return result.Status == "OK" &&
               string.Equals(answer.Trim(), result.Classification, StringComparison.OrdinalIgnoreCase);
    }
}
