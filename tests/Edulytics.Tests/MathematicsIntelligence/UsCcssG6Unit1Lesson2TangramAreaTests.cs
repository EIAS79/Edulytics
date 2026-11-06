using Edulytics.Core.Mathematics.Practice;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// G6 U01:L02 source-owned authoring fixtures, AUTHORED_NOT_RUN.
/// These check exact area only; no C route or human-scored geometry is claimed.
/// </summary>
public sealed class UsCcssG6Unit1Lesson2TangramAreaTests
{
    private static readonly UsCcssG6Unit1Lesson2TangramArea.Pieces Full =
        UsCcssG6Unit1Lesson2TangramArea.FullKit;

    [Fact]
    public void FullSourceKit_HasEightSquareUnits_NotFour()
    {
        Assert.Equal(new UsCcssG6Unit1Lesson2TangramArea.Pieces(1, 4, 1, 2), Full);
        var eight = UsCcssG6Unit1Lesson2TangramArea.CheckFullKit(16);
        Assert.True(eight.ValidInventory);
        Assert.True(eight.ExactTargetArea);
        Assert.Equal(16, eight.AreaInHalfSquareUnits);
        Assert.True(eight.TeacherReviewRequired);
        Assert.False(eight.GeometryVerified);
        var falseSideTwo = UsCcssG6Unit1Lesson2TangramArea.CheckFullKit(8);
        Assert.False(falseSideTwo.ExactTargetArea);
        Assert.Equal("AREA_MISMATCH", falseSideTwo.Status);
    }

    [Theory]
    [InlineData(0, 2, 0, 0, 2)] // 2 small triangles make 1 square unit
    [InlineData(0, 4, 0, 0, 4)] // 4 small triangles make 2 square units
    [InlineData(0, 0, 1, 0, 2)] // medium triangle has area 1
    [InlineData(0, 0, 0, 1, 4)] // large triangle has area 2
    [InlineData(0, 0, 0, 2, 8)] // two large triangles have area 4
    [InlineData(1, 4, 1, 2, 16)] // all pieces have area 8
    public void AuthorizedPieceInventory_ExactAreaNeverCertifiesDrawing(
        int squares, int small, int medium, int large, int target)
    {
        var selection = new UsCcssG6Unit1Lesson2TangramArea.Pieces(
            squares, small, medium, large);
        var result = UsCcssG6Unit1Lesson2TangramArea.Check(selection, target);
        Assert.True(result.ValidInventory);
        Assert.True(result.ExactTargetArea);
        Assert.Equal(target, result.AreaInHalfSquareUnits);
        Assert.Equal("AREA_MATCH_REQUIRES_TEACHER_REVIEW", result.Status);
        Assert.True(result.TeacherReviewRequired);
        Assert.False(result.GeometryVerified);
    }

    [Theory]
    [InlineData(-1, 0, 0, 0)]
    [InlineData(2, 0, 0, 0)]
    [InlineData(0, 5, 0, 0)]
    [InlineData(0, 0, 2, 0)]
    [InlineData(0, 0, 0, 3)]
    [InlineData(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue)]
    public void OutOfInventoryAndInvalidCounts_FailClosed(
        int squares, int small, int medium, int large)
    {
        var result = UsCcssG6Unit1Lesson2TangramArea.Check(
            new UsCcssG6Unit1Lesson2TangramArea.Pieces(squares, small, medium, large), 4);
        Assert.False(result.ValidInventory);
        Assert.False(result.ExactTargetArea);
        Assert.Equal("PIECES_OUTSIDE_SOURCE_KIT", result.Status);
        Assert.False(result.GeometryVerified);
    }

    [Fact]
    public void NullOrUnsupportedTarget_AndMissingPieces_FailClosed()
    {
        Assert.Equal("MALFORMED_SELECTION_OR_TARGET",
            UsCcssG6Unit1Lesson2TangramArea.Check(null, 4).Status);
        Assert.Equal("MALFORMED_SELECTION_OR_TARGET",
            UsCcssG6Unit1Lesson2TangramArea.Check(Full, 0).Status);
        Assert.Equal("MALFORMED_SELECTION_OR_TARGET",
            UsCcssG6Unit1Lesson2TangramArea.Check(Full, 17).Status);
        Assert.Equal("NO_PIECES",
            UsCcssG6Unit1Lesson2TangramArea.Check(new UsCcssG6Unit1Lesson2TangramArea.Pieces(0, 0, 0, 0), 4).Status);
    }

    [Fact]
    public void TwoDifferentInventoriesCanMatchArea_ButNotVerifySilhouettes()
    {
        var fourSmalls = new UsCcssG6Unit1Lesson2TangramArea.Pieces(0, 4, 0, 0);
        var oneLarge = new UsCcssG6Unit1Lesson2TangramArea.Pieces(0, 0, 0, 1);
        Assert.True(UsCcssG6Unit1Lesson2TangramArea.SameExactArea(
            fourSmalls, oneLarge, 4));
        Assert.False(UsCcssG6Unit1Lesson2TangramArea.SameExactArea(
            fourSmalls, oneLarge, 8));
        // A student's two distinct *arrangements* could even use identical
        // inventories. Their spatial difference and gap-free shape are human
        // review requirements; neither is certified by SameExactArea.
        Assert.True(UsCcssG6Unit1Lesson2TangramArea.SameExactArea(
            fourSmalls, fourSmalls, 4));
    }
}
