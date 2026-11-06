using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// First-party Grade 7 U01:L05 classification and reciprocal numeric checks.
/// Card sorting, cutting/scaling puzzle pieces and student drawings are
/// teacher-led; no original publisher master is copied.
/// </summary>
internal static class UsCcssG7Unit1Lesson5FactorQuestions
{
    public const string Family = "usccss.g7.u01.l05.factor_size_and_reciprocal";

    internal sealed record Problem(string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);

    // These rational factors cover smaller, unchanged and larger copies and
    // are closed under taking reciprocals. Index is an internal choice ID only.
    private static readonly string[] Factors =
    [
        "1/2", "2/3", "1", "3/2", "2", "3", "1/3"
    ];

    private static readonly int[] ReciprocalIndex =
    [
        4, 3, 2, 1, 0, 6, 5
    ];

    public static bool Supports(string? family) =>
        string.Equals(family, Family, StringComparison.Ordinal);

    public static Problem Build(string family, Random random)
    {
        if (!Supports(family))
            throw new InvalidOperationException("Unsupported L05 factor family.");
        var p = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["factor"] = random.Next(0, Factors.Length),
            ["mode"] = random.Next(0, 2)
        };
        int selected = p["factor"];
        string prompt = p["mode"] == 0
            ? "An original figure is scaled by k=" + Factors[selected] +
              " to create its copy. Will the corresponding lengths be " +
              "shorter, unchanged or longer? Enter 1 for shorter, 2 for " +
              "unchanged, 3 for longer. Compare lengths, not perimeter or area."
            : "An original figure is scaled by k=" + Factors[selected] +
              " to make Figure B. Which factor takes Figure B BACK to the " +
              "original Figure A? Choose the correct number: " +
              "1 = 1/2; 2 = 2/3; 3 = 1; 4 = 3/2; 5 = 2; 6 = 3; 7 = 1/3. " +
              "The reverse factor is reciprocal, not the original factor.";
        string solution = "A positive factor less than 1 reduces all " +
            "corresponding lengths, k=1 leaves them unchanged and k>1 " +
            "enlarges them. The factor for the reverse direction is 1/k. " +
            "Physical card sorting, drawing and rebuilding puzzle pieces " +
            "require a teacher-reviewed product.";
        return new(Family, prompt, solution, AssessmentItemType.Numeric, p);
    }

    public static string Solve(string family, IReadOnlyDictionary<string, int> p)
    {
        if (!Valid(family, p))
            throw new InvalidOperationException("Invalid G7 L05 parameters.");
        int n = p["factor"];
        if (p["mode"] == 1)
            return (ReciprocalIndex[n] + 1).ToString(
                System.Globalization.CultureInfo.InvariantCulture);
        return n switch
        {
            0 or 1 or 6 => "1", // k less than 1
            2 => "2",         // k = 1
            _ => "3"         // k greater than 1
        };
    }

    public static bool Verify(string family, IReadOnlyDictionary<string, int> p,
        string? answer) =>
        Valid(family, p) &&
        string.Equals(Solve(family, p), answer, StringComparison.Ordinal);

    private static bool Valid(string? family, IReadOnlyDictionary<string, int>? p) =>
        Supports(family) && p is not null && p.Count == 2 &&
        p.TryGetValue("factor", out var factor) && factor is >= 0 and <= 6 &&
        p.TryGetValue("mode", out var mode) && mode is 0 or 1;
}
