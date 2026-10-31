using System.Globalization;
using Edulytics.Core.Enums;
using Kernel = Edulytics.Core.Mathematics.Practice.UsCcssG6Unit1Lesson1TilingMath;

namespace Edulytics.Services.Mathematics;

// Exact numeric checks only; drawings and explanations remain teacher reviewed.
internal static class UsCcssG6Unit1Lesson1TilingQuestions
{
    public const string Area = "usccss.g6.u01.l01.triangular_unit_tile_area";
    public const string Coverage = "usccss.g6.u01.l01.structured_tiling_coverage";
    internal sealed record Problem(string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family) => family is Area or Coverage;
    public static Problem Build(string family, Random random)
    {
        if (!Supports(family)) throw new InvalidOperationException("Unknown G6 tiling family");
        var values = new Dictionary<string,int>(StringComparer.Ordinal);
        string prompt;
        string solution;
        if (family == Area)
        {
            int hexagons = random.Next(1,9);
            int color = random.Next(0,3);
            values["hexagons"] = hexagons;
            values["color"] = color;
            string tile = color switch { 0 => "red trapezoids", 1 => "blue rhombi", _ => "green triangles" };
            prompt = $"Each of the original eight hexagons has 3 red trapezoids, 4 blue rhombi " +
                $"and 7 green triangles. Each red piece is 3 triangular units, each blue " +
                $"piece 2 triangular units, each green piece 1 triangular unit. " +
                $"For {hexagons} of these hexagons, find the TOTAL AREA of the {tile} " +
                "in triangular units, NOT their number of pieces.";
            solution = "Multiply hexagons by pieces per hexagon and triangular units per piece.";
        }
        else
        {
            int pattern = random.Next(0,4);
            values["pattern"] = pattern;
            string configuration = pattern switch
            {
                0 => "S(0,0), S(0,1), T(1,0,0), T(1,0,1), T(1,1,0), T(1,1,1)",
                1 => "S(0,0), S(1,0), T(0,1,0), T(0,1,1), T(1,1,0), T(1,1,1)",
                2 => "S(0,0), S(0,1), T(1,0,0), T(1,0,1), T(1,1,0), T(1,0,1)",
                _ => "S(0,0), S(0,1), T(1,0,0), T(1,0,1), T(1,1,0)"
            };
            prompt = "A 2 by 2 square must be tiled using exactly two unit squares " +
                "and four congruent half-square triangles, without a gap or overlap. " +
                "Cells are (x,y) with x,y each 0 or 1. S(x,y) covers both halves " +
                "of that unit cell; T(x,y,h) covers triangle half h=0 or 1, " +
                "using the same diagonal. The placed pieces are: " + configuration +
                ". Is this a valid complete tiling with the REQUIRED pieces? " +
                "Enter 1 for yes or 0 for no.";
            solution = "Check disjoint occupancy of all eight halves AND piece inventory; area sum is insufficient.";
        }
        return new(family,prompt,solution,AssessmentItemType.Numeric,values);
    }
    public static string Solve(string family, IReadOnlyDictionary<string,int> p)
    {
        if (!Valid(family,p)) throw new InvalidOperationException("Invalid G6 tiling parameters");
        if (family == Area)
        {
            var areas = Kernel.TryMeasureHexagonColors(p["hexagons"],3,4,7)!.Value;
            long total = p["color"] switch
            {
                0 => areas.RedTriangularUnits,
                1 => areas.BlueTriangularUnits,
                _ => areas.GreenTriangularUnits
            };
            return total.ToString(CultureInfo.InvariantCulture);
        }
        return Kernel.CheckTwoByTwo(Pieces(p["pattern"])).CoversExactly ? "1" : "0";
    }

    public static bool Verify(string family, IReadOnlyDictionary<string,int> p, string? answer) =>
        Valid(family,p) && string.Equals(Solve(family,p),answer,StringComparison.Ordinal);

    private static bool Valid(string? family, IReadOnlyDictionary<string,int>? p)
    {
        if (!Supports(family) || p is null) return false;
        if (family == Area)
            return p.Count == 2 && p.TryGetValue("hexagons",out var h) &&
                h is >=1 and <=8 && p.TryGetValue("color",out var c) && c is >=0 and <=2;
        return p.Count == 1 && p.TryGetValue("pattern",out var variant) &&
            variant is >=0 and <=3;
    }
    private static IReadOnlyList<Kernel.Tile> Pieces(int variant)
    {
        static Kernel.Tile S(int x,int y) => new(x,y,Kernel.PieceKind.UnitSquare);
        static Kernel.Tile T(int x,int y,int half) =>
            new(x,y,Kernel.PieceKind.HalfSquareTriangle,half,0);
        return variant switch
        {
            0 => [S(0,0),S(0,1),T(1,0,0),T(1,0,1),T(1,1,0),T(1,1,1)],
            1 => [S(0,0),S(1,0),T(0,1,0),T(0,1,1),T(1,1,0),T(1,1,1)],
            2 => [S(0,0),S(0,1),T(1,0,0),T(1,0,1),T(1,1,0),T(1,0,1)],
            3 => [S(0,0),S(0,1),T(1,0,0),T(1,0,1),T(1,1,0)],
            _ => throw new InvalidOperationException("Invalid G6 pattern")
        };
    }
}
