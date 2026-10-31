using Edulytics.Core.Mathematics.Practice;
using Xunit;
using Kernel = Edulytics.Core.Mathematics.Practice.UsCcssG6Unit1Lesson1TilingMath;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Authored for eventual Stage D: NOT_RUN in A-B-C.
/// This tests a candidate Grade 6 geometry kernel, NOT selected Practice routing.</summary>
public sealed class UsCcssG6Unit1Lesson1TilingMathTests
{
    private static Kernel.Tile Square(int x, int y) =>
        new(x, y, Kernel.PieceKind.UnitSquare);

    private static Kernel.Tile Triangle(int x, int y, int half, int diagonal) =>
        new(x, y, Kernel.PieceKind.HalfSquareTriangle, half, diagonal);

    private static Kernel.Tile[] Vertical() =>
    [
        Square(0, 0), Square(0, 1),
        Triangle(1, 0, 0, 0), Triangle(1, 0, 1, 0),
        Triangle(1, 1, 0, 1), Triangle(1, 1, 1, 1)
    ];

    private static Kernel.Tile[] Horizontal() =>
    [
        Square(0, 0), Square(1, 0),
        Triangle(0, 1, 0, 1), Triangle(0, 1, 1, 1),
        Triangle(1, 1, 0, 0), Triangle(1, 1, 1, 0)
    ];

    [Fact]
    public void SourceMosaics_AcceptDifferentGapFreePartitionsOfFourSquareUnits()
    {
        Assert.NotEqual(Vertical(), Horizontal());
        foreach (var mosaic in new[] { Vertical(), Horizontal() })
        {
            var result = Kernel.CheckTwoByTwo(mosaic);
            Assert.True(result.CoversExactly);
            Assert.Equal(8, result.CoveredHalfCells);
            Assert.Equal("COMPLETE_TILING", result.Failure);
        }
    }

    [Fact]
    public void NumericAreaFour_DoesNotConcealOverlappingHalfAndEmptyHalf()
    {
        var pieces = Vertical();
        pieces[^1] = Triangle(1, 0, 1, 0); // duplicate one half; leave (1,1) unfilled
        Assert.Equal(2 * 1 + 4 * 0.5, 4.0); // inventory area remains four
        var result = Kernel.CheckTwoByTwo(pieces);
        Assert.False(result.CoversExactly);
        Assert.Equal("OVERLAP", result.Failure);
    }

    [Fact]
    public void EquivalentHalfIndicesWithDifferentDiagonal_AreNotFalselyNonOverlapping()
    {
        var pieces = Vertical();
        pieces[3] = Triangle(1, 0, 1, 1);
        Assert.Equal("MISMATCHED_DIAGONAL", Kernel.CheckTwoByTwo(pieces).Failure);
    }

    [Fact]
    public void SourceEightHexagons_AreaWeightsAreNotTileCounts()
    {
        var areas = Kernel.TryMeasureHexagonColors(8, 3, 4, 7);
        Assert.NotNull(areas);
        Assert.Equal(24, areas.Value.RedTrapezoidCount);
        Assert.Equal(32, areas.Value.BlueRhombusCount);
        Assert.Equal(56, areas.Value.GreenTriangleCount);
        Assert.Equal(72, areas.Value.RedTriangularUnits);
        Assert.Equal(64, areas.Value.BlueTriangularUnits);
        Assert.Equal(56, areas.Value.GreenTriangularUnits);
        Assert.Equal(192, areas.Value.TotalTriangularUnits);
        Assert.True(areas.Value.RedTriangularUnits > areas.Value.BlueTriangularUnits);
        Assert.True(areas.Value.BlueTriangularUnits > areas.Value.GreenTriangularUnits);
    }

    [Fact]
    public void InvalidGeometryAndPieceInventory_FailClosed()
    {
        Assert.False(Kernel.CheckTwoByTwo(null).CoversExactly);
        Assert.False(Kernel.CheckTwoByTwo(Vertical()[..5]).CoversExactly);
        var outside = Vertical();
        outside[5] = Triangle(2, 1, 1, 1);
        Assert.Equal("OUT_OF_BOUNDS", Kernel.CheckTwoByTwo(outside).Failure);
        var wrongHalf = Vertical();
        wrongHalf[5] = Triangle(1, 1, 2, 1);
        Assert.Equal("MALFORMED_PIECE", Kernel.CheckTwoByTwo(wrongHalf).Failure);
        var wrongKind = Vertical();
        wrongKind[5] = new Kernel.Tile(1, 1, (Kernel.PieceKind)99);
        Assert.Equal("MALFORMED_PIECE", Kernel.CheckTwoByTwo(wrongKind).Failure);
        var wrongInventory = new Kernel.Tile[] {
            Square(0,0), Square(1,0), Square(0,1),
            Triangle(1,1,0,0), Triangle(1,1,1,0),
            Triangle(1,1,0,0)
        };
        Assert.False(Kernel.CheckTwoByTwo(wrongInventory).CoversExactly);
    }

    [Fact]
    public void InvalidHexagonCountsAndOverflow_FailClosed()
    {
        Assert.Null(Kernel.TryMeasureHexagonColors(0, 3, 4, 7));
        Assert.Null(Kernel.TryMeasureHexagonColors(8, -1, 4, 7));
        Assert.Null(Kernel.TryMeasureHexagonColors(8, 3, -1, 7));
        Assert.Null(Kernel.TryMeasureHexagonColors(8, 3, 4, -1));
        Assert.Null(Kernel.TryMeasureHexagonColors(int.MaxValue, int.MaxValue, 0, 0));
    }
}
