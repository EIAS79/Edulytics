using System.Globalization;
using Edulytics.Core.Enums;
using Geometry = Edulytics.Core.Mathematics.Practice.UsCcssG7Unit1Lesson1ScaledCopyMath;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// IM 2021 Grade 7 U01:L01 numeric recognition of uniform scaled copies.
/// Drawing, constructing and written justifications remain TEACHER_REVIEW_REQUIRED.
/// Reuses the course-owned bounded exact polygon correspondence oracle from #608.
/// </summary>
internal static class UsCcssG7Unit1Lesson1ScaledCopyQuestions
{
    public const string Family = "usccss.g7.u01.l01.scaled_copy_correspondence";

    internal sealed record Problem(string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) =>
        string.Equals(family, Family, StringComparison.Ordinal);

    private static readonly Geometry.Vertex[][] OriginalPolygons =
    [
        [new(0m, 0m), new(6m, 0m), new(2m, 4m)],
        [new(0m, 0m), new(4m, 0m), new(4m, 2m), new(0m, 2m)],
        [new(0m, 0m), new(6m, 0m), new(8m, 4m), new(2m, 6m)]
    ];

    public static Problem Build(string family, Random random)
    {
        if (!Supports(family))
            throw new InvalidOperationException("Unsupported G7 L01 scaled-copy question family.");
        var parameters = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["shape"] = random.Next(0, 3),
            ["factor"] = random.Next(0, 3),
            ["transform"] = random.Next(0, 8),
            ["dx"] = random.Next(-4, 5),
            ["dy"] = random.Next(-4, 5)
        };
        var (original, candidate, factor) = CreatePolygons(parameters);
        string factorText = parameters["factor"] switch
        {
            0 => "1/2",
            1 => "1",
            _ => "2"
        };
        string prompt = "Which figure is a scaled copy? For these TWO polygons, " +
            "is the second a uniform scaled copy of the first at scale factor k=" +
            factorText + "? Uniform dilation may be translated, rotated or reflected. " +
            "Coordinates for first polygon in vertex order: " + Format(original) +
            ". Coordinates for second polygon: " + Format(candidate) +
            ". Enter 1 for YES (all corresponding distances and angles scale uniformly), " +
            "or 0 for NO (nonuniform change, wrong factor or invalid copy).";
        string solution = "Compare every corresponding distance (including diagonals), " +
            "using the stated k. Rotation, translation and reversed vertex traversal " +
            "do not change similarity; unequal stretch, shear or wrong k fail. " +
            "A drawn construction and its mathematical justification require teacher review.";
        return new(Family, prompt, solution, AssessmentItemType.Numeric, parameters);
    }

    public static string Solve(string family, IReadOnlyDictionary<string, int> parameters)
    {
        if (!Supports(family) || !Valid(parameters))
            throw new InvalidOperationException("Invalid G7 scaled-copy task parameters.");
        var (original, candidate, factor) = CreatePolygons(parameters);
        return Geometry.Check(original, candidate, factor).IsScaledCopy ? "1" : "0";
    }

    public static bool Verify(string family, IReadOnlyDictionary<string, int> parameters, string? answer)
    {
        if (!Supports(family) || !Valid(parameters) || answer is not ("1" or "0"))
            return false;
        // Independent reconstruction from source parameters, not a prompt/answer cache.
        var (original, candidate, factor) = CreatePolygons(parameters);
        bool isCopy = Geometry.Check(original, candidate, factor).IsScaledCopy;
        return isCopy == (answer == "1");
    }

    private static bool Valid(IReadOnlyDictionary<string, int>? p) =>
        p is not null && p.Count == 5 &&
        p.TryGetValue("shape", out int shape) && shape is >= 0 and <= 2 &&
        p.TryGetValue("factor", out int factor) && factor is >= 0 and <= 2 &&
        p.TryGetValue("transform", out int transform) && transform is >= 0 and <= 7 &&
        p.TryGetValue("dx", out int dx) && dx is >= -4 and <= 4 &&
        p.TryGetValue("dy", out int dy) && dy is >= -4 and <= 4;

    private static (Geometry.Vertex[] Original, Geometry.Vertex[] Candidate, decimal Factor)
        CreatePolygons(IReadOnlyDictionary<string, int> p)
    {
        var original = OriginalPolygons[p["shape"]];
        decimal factor = p["factor"] switch { 0 => 0.5m, 1 => 1m, _ => 2m };
        int mode = p["transform"];
        int dx = p["dx"], dy = p["dy"];
        var candidate = new Geometry.Vertex[original.Length];
        for (int i = 0; i < original.Length; i++)
        {
            decimal x = original[i].X * factor;
            decimal y = original[i].Y * factor;
            candidate[i] = mode switch
            {
                1 => new(-y + dx, x + dy),                 // 90-degree rotation
                2 => new(-x + dx, -y + dy),                // 180-degree rotation
                3 => new(x * 2m + dx, y + dy),             // nonuniform stretch
                4 => new(x + y + dx, y + dy),              // shear
                6 => new(x * 2m + dx, y * 2m + dy),       // incorrect scale factor
                7 => new(-x + dx, y + dy),                // reflection; reversed traversal
                _ => new(x + dx, y + dy)                  // dilation and translation
            };
        }
        if (mode == 2 || mode == 7)
            Array.Reverse(candidate);
        if (mode == 5)
            candidate[1] = candidate[1] with { X = candidate[1].X + 1m };
        return (original, candidate, factor);
    }

    private static string Format(IReadOnlyList<Geometry.Vertex> points) =>
        string.Join(", ", points.Select(v =>
            "(" + v.X.ToString(CultureInfo.InvariantCulture) + "," +
            v.Y.ToString(CultureInfo.InvariantCulture) + ")"));
}
