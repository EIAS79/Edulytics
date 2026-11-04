using System.Globalization;
using Edulytics.Core.Enums;
using G7Geometry = Edulytics.Core.Mathematics.Practice.UsCcssG7Unit1Lesson1ScaledCopyMath;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// First-party Grade 7 U01:L03 objective scaled-polygon and multiplication
/// tasks. Reuses the exact bounded L01 geometry kernel; construction and
/// critique of drawings or reasoning remain teacher-scored.
/// </summary>
internal static class UsCcssG7Unit1Lesson3DrawingQuestions
{
    public const string Polygon = "usccss.g7.u01.l03.scaled_l_polygon";
    public const string Additive = "usccss.g7.u01.l03.multiplicative_vs_additive";

    internal sealed record Problem(string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) => family is Polygon or Additive;

    private static readonly G7Geometry.Vertex[] Original =
    [
        new(0m,0m), new(6m,0m), new(6m,2m),
        new(4m,2m), new(4m,5m), new(0m,5m)
    ];

    public static Problem Build(string family, Random random)
    {
        if (!Supports(family))
            throw new InvalidOperationException("Unsupported G7 lesson 3 family.");
        if (family == Additive)
        {
            var p = new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["first"] = random.Next(2, 9),
                ["step"] = random.Next(1, 5),
                ["factor"] = random.Next(0, 2),
                ["method"] = random.Next(0, 2)
            };
            int first = p["first"], step = p["step"];
            decimal k = Factor(p["factor"]);
            decimal delta = first * (k - 1m);
            decimal[] original = [first, first + step, first + 2 * step];
            decimal[] candidate = original.Select(x =>
                p["method"] == 0 ? x * k : x + delta).ToArray();
            string prompt = "Original segment lengths are " +
                Numbers(original) + " units. A proposed drawing has corresponding " +
                "lengths " + Numbers(candidate) + ". The first length must become " +
                Format(first * k) + " units. Is the proposal a valid uniform " +
                "scaled copy? Check all corresponding ratios, not only the first " +
                "segment. Enter 1 if valid, 0 if an additive shortcut distorted " +
                "the remaining lengths.";
            return new(family, prompt,
                "A valid scale uses multiplication by the same factor on every " +
                "side; adding a fixed difference to unequal side lengths is " +
                "not the same transformation. A diagram and critique need teacher review.",
                AssessmentItemType.Numeric, p);
        }
        var pars = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["factor"] = random.Next(0, 2),
            ["transform"] = random.Next(0, 5),
            ["dx"] = random.Next(-2, 3),
            ["dy"] = random.Next(-2, 3)
        };
        var shape = Candidate(pars);
        decimal kDrawing = Factor(pars["factor"]);
        string question = "Original L-shaped polygon vertices in boundary order: " +
            Coordinates(Original) + ". Proposed copy vertices in order: " +
            Coordinates(shape) + ". The requested scale factor is " +
            (pars["factor"] == 0 ? "1/2" : "3") +
            ". A valid copy may be rotated or translated, but all corresponding " +
            "lengths and angles must be preserved at the requested scale. " +
            "Enter 1 for a true scaled drawing or 0 for an incorrect scale, " +
            "stretch or shear.";
        return new(family, question,
            "Compare all paired distances and angles; the requested factor is " +
            Format(kDrawing) + ". Use construction/tracing and a teacher rubric " +
            "for the student's actual drawn polygon and written explanation.",
            AssessmentItemType.Numeric, pars);
    }

    public static string Solve(string family, IReadOnlyDictionary<string, int> p)
    {
        if (!Valid(family, p))
            throw new InvalidOperationException("Invalid G7 lesson 3 source parameters.");
        if (family == Polygon)
            return G7Geometry.Check(Original, Candidate(p), Factor(p["factor"]))
                .IsScaledCopy ? "1" : "0";
        // Independent ratio comparison rather than deciding from method name.
        decimal[] original = [p["first"], p["first"] + p["step"],
            p["first"] + 2 * p["step"]];
        decimal k = Factor(p["factor"]);
        decimal delta = p["first"] * (k - 1m);
        decimal[] copy = original.Select(x =>
            p["method"] == 0 ? x * k : x + delta).ToArray();
        bool uniform = copy.Skip(1).Select((x, i) =>
            x * original[0] == copy[0] * original[i + 1]).All(ok => ok);
        return uniform ? "1" : "0";
    }

    public static bool Verify(string family, IReadOnlyDictionary<string, int> p,
        string? answer) =>
        Valid(family, p) &&
        string.Equals(Solve(family, p), answer, StringComparison.Ordinal);

    private static bool Valid(string? family, IReadOnlyDictionary<string, int>? p)
    {
        if (!Supports(family) || p is null)
            return false;
        if (family == Additive)
            return p.Count == 4 &&
                p.TryGetValue("first", out var n) && n is >= 2 and <= 8 &&
                p.TryGetValue("step", out var step) && step is >= 1 and <= 4 &&
                p.TryGetValue("factor", out var factor) && factor is 0 or 1 &&
                p.TryGetValue("method", out var mode) && mode is 0 or 1;
        return p.Count == 4 &&
            p.TryGetValue("factor", out var f) && f is 0 or 1 &&
            p.TryGetValue("transform", out var t) && t is >= 0 and <= 4 &&
            p.TryGetValue("dx", out var dx) && dx is >= -2 and <= 2 &&
            p.TryGetValue("dy", out var dy) && dy is >= -2 and <= 2;
    }

    private static G7Geometry.Vertex[] Candidate(IReadOnlyDictionary<string, int> p)
    {
        decimal k = Factor(p["factor"]);
        int mode = p["transform"], dx = p["dx"], dy = p["dy"];
        var result = new G7Geometry.Vertex[Original.Length];
        for (int i = 0; i < result.Length; i++)
        {
            decimal x = Original[i].X * k;
            decimal y = Original[i].Y * k;
            result[i] = mode switch
            {
                1 => new(-y + dx, x + dy),             // legitimate 90-degree rotation
                2 => new(x * 2m + dx, y + dy),        // nonuniform horizontal stretch
                3 => new(x + y + dx, y + dy),        // shear
                4 => new(x * 2m + dx, y * 2m + dy), // wrong claimed k
                _ => new(x + dx, y + dy)
            };
        }
        return result;
    }

    private static decimal Factor(int n) => n == 0 ? 0.5m : 3m;
    private static string Format(decimal n) =>
        n.ToString(CultureInfo.InvariantCulture);
    private static string Coordinates(IReadOnlyList<G7Geometry.Vertex> vertices) =>
        string.Join("; ", vertices.Select(v =>
            "(" + Format(v.X) + "," + Format(v.Y) + ")"));
    private static string Numbers(IEnumerable<decimal> numbers) =>
        string.Join(", ", numbers.Select(Format));
}
