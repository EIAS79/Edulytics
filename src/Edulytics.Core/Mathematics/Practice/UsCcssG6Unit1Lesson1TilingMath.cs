namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Source-scoped geometry arithmetic for IM Grade 6 U01:L01, "Tiling the Plane".
/// This helper is NOT registered in LessonPracticeContractRegistry. A selected
/// lesson Practice route and teacher-reviewed free construction are separate.
/// </summary>
public static class UsCcssG6Unit1Lesson1TilingMath
{
    public enum PieceKind { UnitSquare, HalfSquareTriangle }

    /// <param name="Half">0 or 1 for a triangle; -1 for a full square.</param>
    /// <param name="Diagonal">0 or 1 for the common triangle cut; -1 for a square.</param>
    public readonly record struct Tile(int X, int Y, PieceKind Kind, int Half = -1, int Diagonal = -1);

    public readonly record struct Coverage(bool CoversExactly, int CoveredHalfCells, string Failure);

    /// <summary>
    /// Checks an EXACT 2x2 first-party structured mosaic: two full squares and
    /// four triangular half-squares. Each cell has two disjoint halves sharing
    /// one diagonal. The piece area sum alone cannot identify overlap or a gap.
    /// </summary>
    public static Coverage CheckTwoByTwo(IReadOnlyList<Tile>? pieces)
    {
        if (pieces is null || pieces.Count != 6)
            return new(false, 0, "INVALID_PIECE_COUNT");

        Span<int> masks = stackalloc int[4];
        Span<int> diagonals = stackalloc int[4];
        diagonals.Fill(-1);
        int squares = 0;
        int triangles = 0;

        foreach (var piece in pieces)
        {
            if (piece.X is < 0 or > 1 || piece.Y is < 0 or > 1)
                return new(false, CountHalves(masks), "OUT_OF_BOUNDS");

            int cell = piece.Y * 2 + piece.X;
            int occupancy;
            if (piece.Kind == PieceKind.UnitSquare &&
                piece.Half == -1 && piece.Diagonal == -1)
            {
                squares++;
                occupancy = 3;
            }
            else if (piece.Kind == PieceKind.HalfSquareTriangle &&
                     piece.Half is 0 or 1 && piece.Diagonal is 0 or 1)
            {
                triangles++;
                if (diagonals[cell] >= 0 && diagonals[cell] != piece.Diagonal)
                    return new(false, CountHalves(masks), "MISMATCHED_DIAGONAL");
                diagonals[cell] = piece.Diagonal;
                occupancy = 1 << piece.Half;
            }
            else
            {
                return new(false, CountHalves(masks), "MALFORMED_PIECE");
            }

            if ((masks[cell] & occupancy) != 0)
                return new(false, CountHalves(masks), "OVERLAP");

            masks[cell] |= occupancy;
        }

        int occupied = CountHalves(masks);
        if (squares != 2 || triangles != 4)
            return new(false, occupied, "WRONG_INVENTORY");
        if (occupied != 8)
            return new(false, occupied, "GAP");
        return new(true, occupied, "COMPLETE_TILING");
    }

    private static int CountHalves(ReadOnlySpan<int> masks)
    {
        int occupied = 0;
        foreach (int cellMask in masks)
        {
            if ((cellMask & 1) != 0) occupied++;
            if ((cellMask & 2) != 0) occupied++;
        }
        return occupied;
    }

    public readonly record struct HexagonAreas(
        long RedTrapezoidCount, long BlueRhombusCount, long GreenTriangleCount,
        long RedTriangularUnits, long BlueTriangularUnits,
        long GreenTriangularUnits, long TotalTriangularUnits);

    /// <summary>
    /// Compare areas in TRIANGULAR UNITS: red trapezoid=3,
    /// blue rhombus=2, green triangle=1. Counts are not the areas.
    /// This arithmetic does not validate an actual polygon tiling.
    /// </summary>
    public static HexagonAreas? TryMeasureHexagonColors(
        int hexagons, int redPerHexagon, int bluePerHexagon, int greenPerHexagon)
    {
        if (hexagons <= 0 || redPerHexagon < 0 ||
            bluePerHexagon < 0 || greenPerHexagon < 0)
            return null;
        try
        {
            checked
            {
                long redCount = (long)hexagons * redPerHexagon;
                long blueCount = (long)hexagons * bluePerHexagon;
                long greenCount = (long)hexagons * greenPerHexagon;
                long redArea = redCount * 3;
                long blueArea = blueCount * 2;
                long greenArea = greenCount;
                return new(
                    redCount, blueCount, greenCount,
                    redArea, blueArea, greenArea, redArea + blueArea + greenArea);
            }
        }
        catch (OverflowException)
        {
            return null;
        }
    }
}
