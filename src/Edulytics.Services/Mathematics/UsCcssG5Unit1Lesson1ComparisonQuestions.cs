using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Source-aligned digital comparison subskill, not a grading mechanism for
/// original physical construction or reasoning. No prism dimension formula.
/// </summary>
internal static class UsCcssG5Unit1Lesson1ComparisonQuestions
{
    public const string Compare = "usccss.g5.u01.l01.compare_connected_cube_models";

    public sealed record Problem(
        string Family,
        string Prompt,
        string Solution,
        AssessmentItemType ItemType,
        IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) =>
        string.Equals(family, Compare, StringComparison.Ordinal);

    public static Problem Build(string family, Random random)
    {
        if (!Supports(family))
            throw new InvalidOperationException("Unsupported Grade 5 L01 cube comparison");
        ArgumentNullException.ThrowIfNull(random);

        var a = random.Next(6, 11);
        var delta = random.Next(3) switch { 0 => -2, 1 => 0, _ => 2 };
        var b = a + delta;
        var parameters = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["a"] = a,
            ["b"] = b
        };
        var prompt =
            $"Object A is one connected row of {a} equal cubes. " +
            $"Object B has a connected bottom row of {b - 2} equal cubes, " +
            "and two cubes directly above the first two cubes, each sharing a face. " +
            "No cubes overlap. Which takes up more space? Enter 1 for A, " +
            "2 for B, or 3 for equal. Explain your reasoning to the teacher. " +
            "Numeric Practice does not assess construction or explanation.";
        var solution = "Count each congruent cube once. Equal counts mean equal " +
            "volume regardless of arrangement. Explain and build for teacher review.";
        return new Problem(family, prompt, solution, AssessmentItemType.Numeric, parameters);
    }

    public static string Solve(string family, IReadOnlyDictionary<string, int> p)
    {
        if (!Valid(family, p))
            throw new InvalidOperationException("Invalid Grade 5 L01 parameters");
        return (p["a"] > p["b"] ? 1 : p["a"] < p["b"] ? 2 : 3)
            .ToString(CultureInfo.InvariantCulture);
    }

    public static bool Verify(string family,
        IReadOnlyDictionary<string, int> p, string? answer) =>
        Valid(family, p) &&
        string.Equals(answer, Solve(family, p), StringComparison.Ordinal);

    private static bool Valid(string? family, IReadOnlyDictionary<string, int>? p) =>
        Supports(family) && p is not null && p.Count == 2 &&
        p.TryGetValue("a", out var a) && a is >= 6 and <= 10 &&
        p.TryGetValue("b", out var b) && b is >= 4 and <= 12 &&
        b - a is -2 or 0 or 2;
}
