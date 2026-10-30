using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// IM 2021 G5 U01 L02 independently count an irregular connected cube model;
/// partner reconstruction and spoken explanation remain teacher assessed.
/// No assumption that all layers of an irregular model are equal.
/// </summary>
internal static class UsCcssG5Unit1Lesson2Questions
{
    public const string Family = "usccss.g5.u01.l02.count_irregular_connected_cubes";
    public sealed record Problem(string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) =>
        string.Equals(family, Family, StringComparison.Ordinal);

    public static Problem Build(string family, Random random)
    {
        if (!Supports(family))
            throw new InvalidOperationException("Unsupported G5 U01 L02 question");
        ArgumentNullException.ThrowIfNull(random);
        var bottom = random.Next(5, 7);
        var middle = random.Next(3, 5);
        var top = random.Next(0, 3);
        var p = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["bottom"] = bottom,
            ["middle"] = middle,
            ["top"] = top
        };
        var prompt = $"An irregular solid has a bottom row of {bottom} connected unit cubes; " +
            $"a second row of {middle} cubes placed directly on top of the leftmost cubes; " +
            $"and {top} cubes placed directly on top of the leftmost second-row cubes. " +
            "All cubes are the same size, share whole faces and do not overlap. " +
            "How many cubes are in this connected object? Count all cubes, " +
            "not just its silhouette. Show your counting and reconstruction to a teacher.";
        var solution = "Add each distinct row once, including hidden cubes. " +
            "Partner description, reconstruction and explanations require teacher review.";
        return new Problem(family, prompt, solution, AssessmentItemType.Numeric, p);
    }

    public static string Solve(string family, IReadOnlyDictionary<string, int> p)
    {
        if (!Valid(family, p))
            throw new InvalidOperationException("Invalid source irregular cube model");
        return (p["bottom"] + p["middle"] + p["top"])
            .ToString(CultureInfo.InvariantCulture);
    }

    public static bool Verify(string family,
        IReadOnlyDictionary<string, int> p, string? answer) =>
        Valid(family, p) &&
        string.Equals(answer, Solve(family, p), StringComparison.Ordinal);

    private static bool Valid(string? family, IReadOnlyDictionary<string, int>? p) =>
        Supports(family) && p is not null && p.Count == 3 &&
        p.TryGetValue("bottom", out var bottom) && bottom is >= 5 and <= 6 &&
        p.TryGetValue("middle", out var middle) && middle is >= 3 and <= 4 &&
        p.TryGetValue("top", out var top) && top is >= 0 and <= 2;
}
