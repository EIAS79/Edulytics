using System.Globalization;
using Edulytics.Core.Enums;
using Tangram = Edulytics.Core.Mathematics.Practice.UsCcssG6Unit1Lesson2TangramArea;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Lesson-local, source-specific, exact tangram area question adapter.
/// NOT wired to LessonPracticeContractRegistry: that three-file shared integration
/// needs its own exclusive writer grant. Drawings and justifications are teacher reviewed.
/// </summary>
internal static class UsCcssG6Unit1Lesson2TangramQuestions
{
    public const string PieceArea = "usccss.g6.u01.l02.tangram_piece_area";
    public const string SameArea = "usccss.g6.u01.l02.tangram_equal_area";
    internal sealed record Problem(string Family, string Prompt, string Explanation,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) => family is PieceArea or SameArea;

    public static Problem Build(string family, Random random)
    {
        if (!Supports(family)) throw new ArgumentException("Unknown tangram family", nameof(family));
        ArgumentNullException.ThrowIfNull(random);
        int variant = random.Next(family == PieceArea ? 6 : 4);
        var parameters = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["variant"] = variant
        };
        string prompt;
        if (family == PieceArea)
        {
            var description = variant switch
            {
                0 => "one small right triangle",
                1 => "two small right triangles",
                2 => "all four small right triangles",
                3 => "one medium right triangle",
                4 => "one large right triangle",
                _ => "the full tangram kit"
            };
            prompt = $"An area-1 square is the unit. A small right triangle has area 1/2, " +
                $"a medium triangle area 1 and a large triangle area 2. " +
                $"The kit has one square, four small triangles, one medium triangle, " +
                $"and two large triangles. What is the TOTAL AREA, in square units, of {description}? " +
                "Enter a number or exact fraction; do not count pieces as square units.";
        }
        else
        {
            var (a, b) = Pair(variant);
            prompt = $"Two selections from the same source tangram kit are compared: " +
                $"{Describe(a)} versus {Describe(b)}. Do their PIECE-AREA TOTALS match? " +
                "Enter 1 for equal area or 0 for different area. " +
                "Area equality alone does not prove the pieces can form a requested outline " +
                "without gaps or overlaps; drawings require teacher review.";
        }
        return new Problem(family, prompt,
            "Add nonoverlapping pieces in square units (small=1/2, medium=1, large=2, square=1). " +
            "Equal totals are only a numerical prerequisite; a teacher checks constructed figures.",
            AssessmentItemType.Numeric, parameters);
    }

    public static string Solve(string family, IReadOnlyDictionary<string, int> parameters)
    {
        if (!Valid(family, parameters)) throw new ArgumentException("Invalid tangram parameters");
        int variant = parameters["variant"];
        if (family == PieceArea)
        {
            int halfUnits = HalfUnits(PieceSelection(variant));
            return halfUnits % 2 == 0
                ? (halfUnits / 2).ToString(CultureInfo.InvariantCulture)
                : (halfUnits / 2).ToString(CultureInfo.InvariantCulture) + ".5";
        }
        var (a, b) = Pair(variant);
        return HalfUnits(a) == HalfUnits(b) ? "1" : "0";
    }

    public static bool Verify(string family, IReadOnlyDictionary<string, int>? parameters,
        string? studentAnswer)
    {
        if (!Valid(family, parameters) || string.IsNullOrEmpty(studentAnswer))
            return false;
        int variant = parameters!["variant"];
        if (family == SameArea)
            return studentAnswer is "0" or "1" &&
                studentAnswer == Solve(family, parameters);
        int correctHalfUnits = HalfUnits(PieceSelection(variant));
        if (decimal.TryParse(studentAnswer,
                NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out var response))
            return response * 2 == correctHalfUnits;
        var parts = studentAnswer.Split('/');
        if (parts.Length != 2 ||
            !int.TryParse(parts[0], NumberStyles.AllowLeadingSign,
                CultureInfo.InvariantCulture, out var numerator) ||
            !int.TryParse(parts[1], NumberStyles.None,
                CultureInfo.InvariantCulture, out var denominator) ||
            denominator <= 0)
            return false;
        return (long)numerator * 2 == (long)denominator * correctHalfUnits;
    }

    private static bool Valid(string? family, IReadOnlyDictionary<string, int>? parameters) =>
        Supports(family) && parameters is { Count: 1 } &&
        parameters.TryGetValue("variant", out int v) &&
        v >= 0 && v < (family == PieceArea ? 6 : 4);

    private static int HalfUnits(Tangram.Pieces pieces)
    {
        // The existing source-local kernel checks inventory and gives exact half-square units.
        var check = Tangram.Check(pieces, Tangram.FullKitAreaInHalfSquareUnits);
        if (!check.ValidInventory) throw new InvalidOperationException("Invalid source kit");
        return check.AreaInHalfSquareUnits;
    }

    private static Tangram.Pieces PieceSelection(int variant) => variant switch
    {
        0 => new(0, 1, 0, 0),
        1 => new(0, 2, 0, 0),
        2 => new(0, 4, 0, 0),
        3 => new(0, 0, 1, 0),
        4 => new(0, 0, 0, 1),
        5 => Tangram.FullKit,
        _ => throw new ArgumentOutOfRangeException(nameof(variant))
    };

    private static (Tangram.Pieces First, Tangram.Pieces Second) Pair(int variant) => variant switch
    {
        0 => (PieceSelection(2), PieceSelection(4)), // 2 equals 2
        1 => (PieceSelection(1), PieceSelection(3)), // 1 equals 1
        2 => (new Tangram.Pieces(0, 0, 0, 2), Tangram.FullKit), // 4 != 8
        3 => (PieceSelection(2), PieceSelection(3)), // 2 != 1
        _ => throw new ArgumentOutOfRangeException(nameof(variant))
    };

    private static string Describe(Tangram.Pieces p) =>
        $"{p.Squares} square(s), {p.SmallTriangles} small triangle(s), " +
        $"{p.MediumTriangles} medium triangle(s), {p.LargeTriangles} large triangle(s)";
}
