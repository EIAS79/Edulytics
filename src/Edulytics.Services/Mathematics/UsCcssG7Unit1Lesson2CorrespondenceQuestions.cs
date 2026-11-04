using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Grade 7 Unit 1 Lesson 2 source-scoped first-party Practice.
/// Only objective correspondence / scale-ratio selection is machine checked.
/// Physical tracing, drawings, oral and written reasoning require a teacher.
/// </summary>
internal static class UsCcssG7Unit1Lesson2CorrespondenceQuestions
{
    public const string Parts = "usccss.g7.u01.l02.corresponding_parts";
    public const string Scale = "usccss.g7.u01.l02.scale_factor";

    internal sealed record Problem(string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) => family is Parts or Scale;

    public static Problem Build(string family, Random random)
    {
        if (!Supports(family))
            throw new InvalidOperationException("Unsupported Grade 7 L02 family.");

        if (family == Scale)
        {
            var p = new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["factor"] = random.Next(0, 3),
                ["reverse"] = random.Next(0, 2),
                ["distorted"] = random.Next(0, 2)
            };
            decimal k = Factor(p["factor"]);
            decimal a = 3m * k, b = 4m * k;
            decimal c = (p["distorted"] == 1 ? 4.5m : 5m) * k;
            string direction = p["reverse"] == 0
                ? "from the original triangle to the proposed copy"
                : "from the proposed copy back to the original triangle";
            string prompt = "An original right triangle has side lengths 3, 4, 5 units. " +
                "A proposed copy has corresponding side lengths " +
                Format(a) + ", " + Format(b) + ", " + Format(c) +
                " units. Determine the scale factor " + direction +
                ". Check ALL three matching sides, not just one. Enter the " +
                "number of the mathematically correct choice: " +
                "1 = 1/2; 2 = 3/2; 3 = 2; 4 = no uniform scale; 5 = 2/3.";
            return new(family, prompt,
                "Compare all three corresponding length ratios. When scaling back, " +
                "use the reciprocal; a changed third side invalidates uniform scaling. " +
                "A geometric construction and explanation must be teacher-reviewed.",
                AssessmentItemType.Numeric, p);
        }

        var parts = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["shift"] = random.Next(0, 3),
            ["part"] = random.Next(0, 3),
            ["asked"] = random.Next(0, 3),
            ["factor"] = random.Next(0, 3),
            ["dx"] = random.Next(-3, 4),
            ["dy"] = random.Next(-3, 4)
        };
        return BuildParts(parts);
    }

    private static Problem BuildParts(IReadOnlyDictionary<string, int> p)
    {
        decimal k = Factor(p["factor"]);
        // Original A=(0,0), B=(4,0), C=(0,3).
        // A cyclic relabeling of the scaled copy avoids giving away corresponding
        // parts merely by using alphabetical naming conventions.
        var original = new (decimal X, decimal Y)[]
        {
            (0m, 0m), (4m, 0m), (0m, 3m)
        };
        var copy = new (decimal X, decimal Y)[3];
        for (int vertex = 0; vertex < 3; vertex++)
        {
            int copyLabel = (vertex + p["shift"]) % 3;
            copy[copyLabel] = (
                original[vertex].X * k + p["dx"],
                original[vertex].Y * k + p["dy"]);
        }

        string[] point = ["A", "B", "C"];
        string[] sides = ["AB", "BC", "CA"];
        string[] angle = ["BAC", "ABC", "BCA"];
        string what = p["part"] switch
        {
            0 => "vertex " + point[p["asked"]],
            1 => "segment " + sides[p["asked"]],
            _ => "angle " + angle[p["asked"]]
        };
        string options = p["part"] == 1
            ? "1=DE; 2=EF; 3=FD"
            : "1=D; 2=E; 3=F";
        string prompt = "Triangles ABC and DEF are scaled copies. " +
            "Original coordinates A=(0,0), B=(4,0), C=(0,3); " +
            "copy coordinates D=" + Format(copy[0]) +
            ", E=" + Format(copy[1]) + ", F=" + Format(copy[2]) + ". " +
            "Which part of the copy corresponds to " + what + "? " +
            "Choose its numeric label: " + options + ". " +
            "Match the geometrical positions, not the letters. " +
            "Corresponding angles retain their measure.";
        return new(Parts, prompt,
            "Match the corresponding vertex locations, then trace the matching " +
            "segment or angle. All matched distances use one positive factor; " +
            "angle drawings and verbal justification are teacher-reviewed.",
            AssessmentItemType.Numeric, p);
    }

    public static string Solve(string family, IReadOnlyDictionary<string, int> p)
    {
        if (!Valid(family, p))
            throw new InvalidOperationException("Invalid Grade 7 L02 task parameters.");
        if (family == Scale)
        {
            if (p["distorted"] == 1)
                return "4";
            return p["reverse"] == 0
                ? p["factor"] switch { 0 => "1", 1 => "2", _ => "3" }
                : p["factor"] switch { 0 => "3", 1 => "5", _ => "1" };
        }
        // A/B/C -> D/E/F relabeled by the same cyclic shift.
        return ((p["asked"] + p["shift"]) % 3 + 1).ToString(CultureInfo.InvariantCulture);
    }

    public static bool Verify(string family, IReadOnlyDictionary<string, int> p,
        string? answer) =>
        Valid(family, p) &&
        string.Equals(Solve(family, p), answer, StringComparison.Ordinal);

    private static bool Valid(string? family, IReadOnlyDictionary<string, int>? p)
    {
        if (!Supports(family) || p is null)
            return false;
        if (family == Scale)
            return p.Count == 3 &&
                p.TryGetValue("factor", out var f) && f is >= 0 and <= 2 &&
                p.TryGetValue("reverse", out var r) && r is 0 or 1 &&
                p.TryGetValue("distorted", out var d) && d is 0 or 1;
        return p.Count == 6 &&
            p.TryGetValue("shift", out var shift) && shift is >= 0 and <= 2 &&
            p.TryGetValue("part", out var part) && part is >= 0 and <= 2 &&
            p.TryGetValue("asked", out var asked) && asked is >= 0 and <= 2 &&
            p.TryGetValue("factor", out var factor) && factor is >= 0 and <= 2 &&
            p.TryGetValue("dx", out var dx) && dx is >= -3 and <= 3 &&
            p.TryGetValue("dy", out var dy) && dy is >= -3 and <= 3;
    }

    private static decimal Factor(int n) => n switch
    {
        0 => 0.5m, 1 => 1.5m, _ => 2m
    };

    private static string Format(decimal x) =>
        x.ToString(CultureInfo.InvariantCulture);

    private static string Format((decimal X, decimal Y) p) =>
        "(" + Format(p.X) + "," + Format(p.Y) + ")";
}
