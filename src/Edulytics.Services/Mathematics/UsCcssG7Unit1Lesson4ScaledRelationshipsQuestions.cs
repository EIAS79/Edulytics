using System.Globalization;
using Edulytics.Core.Enums;
using Kernel = Edulytics.Core.Mathematics.Practice.UsCcssG7Unit1Lesson1ScaledCopyMath;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Objective Grade 7 Unit 1 Lesson 4 practice: distances between arbitrary
/// points scale, and matching angles alone do not establish a scaled copy.
/// Polygon validity reuses the bounded exact geometry kernel from Lesson 1.
/// Physical protractor readings/drawings/explanations remain teacher-scored.
/// </summary>
internal static class UsCcssG7Unit1Lesson4ScaledRelationshipsQuestions
{
    public const string Distance = "usccss.g7.u01.l04.hidden_corresponding_distance";
    public const string Angles = "usccss.g7.u01.l04.angle_and_distance_scaling";

    internal sealed record Problem(string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) => family is Distance or Angles;

    private static readonly Kernel.Vertex[] BaseRectangle =
    [
        new(0m, 0m), new(4m, 0m), new(4m, 3m), new(0m, 3m)
    ];

    public static Problem Build(string family, Random random)
    {
        if (!Supports(family))
            throw new InvalidOperationException("Unsupported Grade 7 Lesson 4 question family.");
        if (family == Distance)
        {
            var p = new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["factor"] = random.Next(0, 3),
                ["hidden"] = random.Next(1, 7),
                ["reverse"] = random.Next(0, 2)
            };
            decimal k = Factor(p["factor"]);
            int hiddenOriginal = 2 * p["hidden"];
            decimal hiddenCopy = hiddenOriginal * k;
            string prompt = "Two scaled W-shaped figures have corresponding visible segments " +
                "6 units in the original and " + Format(6m * k) +
                " units in its copy. The distance between TWO other points need " +
                "not be a drawn side; it still uses the same scale factor. " +
                (p["reverse"] == 0
                    ? "An unseen distance in the original is " + hiddenOriginal +
                      " units. What is its distance in the copy?"
                    : "An unseen distance in the copy is " + Format(hiddenCopy) +
                      " units. What is its distance in the original?") +
                " Enter the exact integer answer.";
            return new(family, prompt,
                "Use the scale factor of the known corresponding distances for " +
                "ALL distances between corresponding vertices. Reverse scaling " +
                "divides by that factor. Drawn geometric evidence and written " +
                "justification require the teacher.",
                AssessmentItemType.Numeric, p);
        }

        var pars = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["variant"] = random.Next(0, 25)
        };
        var target = GeometryCandidate(pars["variant"]);
        decimal claimed = pars["variant"] % 5 == 4 ? 0.5m : 2m;
        string angleNote = (pars["variant"] % 5) switch
        {
            0 or 1 or 2 or 4 => "All four corresponding angles are right angles.",
            _ => "The proposed copy has oblique angles even though both shapes use straight edges."
        };
        string q = "The original rectangle has vertices (0,0), (4,0), (4,3), (0,3). " +
            "The candidate polygon has boundary-order vertices " +
            Coordinates(target) + ". The claimed scale factor is " + Format(claimed) +
            ". " + angleNote +
            " Are these polygons scaled copies at that factor? " +
            "Check corresponding distances AND angles, including the diagonal. " +
            "Enter 1 for yes, or 0 for no.";
        return new(family, q,
            "A scaled copy preserves every corresponding angle and uses one " +
            "multiplier for all corresponding point-to-point distances. " +
            "Equal right angles alone do not justify unequal horizontal and vertical factors. " +
            "Student measurements with a protractor and written arguments need teacher review.",
            AssessmentItemType.Numeric, pars);
    }

    public static string Solve(string family, IReadOnlyDictionary<string, int> p)
    {
        if (!Valid(family, p))
            throw new InvalidOperationException("Invalid Grade 7 Lesson 4 parameters.");
        if (family == Distance)
        {
            decimal original = 2m * p["hidden"];
            decimal result = p["reverse"] == 0 ? original * Factor(p["factor"]) : original;
            return Format(result);
        }
        decimal factor = p["variant"] % 5 == 4 ? 0.5m : 2m;
        return Kernel.Check(BaseRectangle, GeometryCandidate(p["variant"]), factor)
            .IsScaledCopy ? "1" : "0";
    }

    public static bool Verify(string family, IReadOnlyDictionary<string, int> p,
        string? answer) =>
        Valid(family, p) &&
        string.Equals(Solve(family, p), answer, StringComparison.Ordinal);

    private static bool Valid(string? family, IReadOnlyDictionary<string, int>? p)
    {
        if (!Supports(family) || p is null)
            return false;
        if (family == Distance)
            return p.Count == 3 &&
                p.TryGetValue("factor", out var f) && f is >= 0 and <= 2 &&
                p.TryGetValue("hidden", out var hidden) && hidden is >= 1 and <= 6 &&
                p.TryGetValue("reverse", out var reverse) && reverse is 0 or 1;
        return p.Count == 1 &&
            p.TryGetValue("variant", out var variant) && variant is >= 0 and <= 24;
    }

    private static decimal Factor(int f) => f switch
    {
        0 => 2m, 1 => 2.5m, _ => 3m
    };

    private static Kernel.Vertex[] GeometryCandidate(int variant)
    {
        var phase = variant / 5;
        Kernel.Vertex[] shape = (variant % 5) switch
    {
        0 => [new(0m,0m),new(8m,0m),new(8m,6m),new(0m,6m)],
        1 => [new(0m,0m),new(8m,0m),new(8m,3m),new(0m,3m)],
        2 => [new(0m,0m),new(4m,0m),new(4m,6m),new(0m,6m)],
        3 => [new(0m,0m),new(8m,0m),new(11m,6m),new(3m,6m)],
        4 => [new(0m,0m),new(-2m,0m),new(-2m,-1.5m),new(0m,-1.5m)],
        _ => throw new InvalidOperationException("Invalid geometry variant")
        };
        // Coordinate translations are mathematically valid scaled-copy comparisons.
        return shape.Select(v => new Kernel.Vertex(v.X + 2m * phase, v.Y + phase)).ToArray();
    }

    private static string Coordinates(IReadOnlyList<Kernel.Vertex> points) =>
        string.Join(", ", points.Select(v => "(" + Format(v.X) + "," + Format(v.Y) + ")"));
    private static string Format(decimal n) =>
        n.ToString("0.############################", CultureInfo.InvariantCulture);
}
