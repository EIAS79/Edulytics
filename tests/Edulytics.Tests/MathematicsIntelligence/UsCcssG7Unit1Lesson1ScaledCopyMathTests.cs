using Edulytics.Core.Mathematics.Practice;
using Xunit;
using Geometry = Edulytics.Core.Mathematics.Practice.UsCcssG7Unit1Lesson1ScaledCopyMath;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Authored but NOT_RUN (Stage D); no selected lesson Practice
/// route, authenticated learner validation, or teacher-scored drawing claim.</summary>
public sealed class UsCcssG7Unit1Lesson1ScaledCopyMathTests
{
    private static Geometry.Vertex V(decimal x, decimal y) => new(x, y);

    private static Geometry.Vertex[] Original() =>
        [V(0, 0), V(3, 0), V(0, 4)];

    [Fact]
    public void Positive_DilationTwoWithNinetyDegreeRotationAndTranslation()
    {
        var rotatedTranslated = new[]
        {
            V(10, 5), V(10, 11), V(2, 5)
        };
        var result = Geometry.Check(Original(), rotatedTranslated, 2m);
        Assert.True(result.IsScaledCopy);
        Assert.Equal("UNIFORM_SCALED_COPY", result.Code);
    }

    [Fact]
    public void Positive_DilationHalfWithTranslatedAndReversedVertexOrdering()
    {
        var half = new[] { V(1, 1), V(2.5m, 1), V(1, 3) };
        Assert.True(Geometry.Check(Original(), half, 0.5m).IsScaledCopy);
        Assert.True(Geometry.Check(Original(),
            [half[2], half[1], half[0]], 0.5m).IsScaledCopy);
        Assert.True(Geometry.Check(Original(),
            [half[1], half[2], half[0]], 0.5m).IsScaledCopy);
    }

    [Fact]
    public void Negative_HorizontalOnlyStretchAndIncorrectFactor()
    {
        Assert.False(Geometry.Check(Original(),
            [V(0, 0), V(6, 0), V(0, 4)], 2m).IsScaledCopy);
        Assert.False(Geometry.Check(Original(),
            [V(0, 0), V(6, 0), V(0, 9)], 2m).IsScaledCopy);
        Assert.False(Geometry.Check(Original(),
            [V(0, 0), V(6, 0), V(0, 8)], 0.5m).IsScaledCopy);
    }

    [Fact]
    public void Negative_SameFourSideLengthsButChangedAnglesFails()
    {
        var square = new[] { V(0, 0), V(5, 0), V(5, 5), V(0, 5) };
        var rhombus = new[] { V(0, 0), V(5, 0), V(8, 4), V(3, 4) };
        // Both have four side lengths of 5; diagonals/angles differ.
        Assert.False(Geometry.Check(square, rhombus, 1m).IsScaledCopy);
    }

    [Fact]
    public void Malformed_MissingWrongCardinalityRepeatedCollinearOrCrossingFailsClosed()
    {
        Assert.Equal("INVALID_VERTEX_COUNT", Geometry.Check(null, Original(), 2m).Code);
        Assert.Equal("INVALID_VERTEX_COUNT", Geometry.Check(
            Original(), [V(0, 0), V(3, 0)], 2m).Code);
        Assert.Equal("INVALID_FACTOR", Geometry.Check(Original(), Original(), 0m).Code);
        Assert.Equal("INVALID_FACTOR", Geometry.Check(Original(), Original(), -1m).Code);
        Assert.False(Geometry.Check(Original(),
            [V(0, 0), V(0, 0), V(1, 2)], 2m).IsScaledCopy);
        Assert.False(Geometry.Check(Original(),
            [V(0, 0), V(1, 1), V(2, 2)], 2m).IsScaledCopy);
        Assert.False(Geometry.Check(Original(),
            [V(0, 0), V(1_000_001m, 0), V(0, 1)], 2m).IsScaledCopy);
        // Non-adjacent edges B-C and D-A cross at (2,2); signed area !=0.
        var bowTie = new[] { V(0, 0), V(4, 0), V(0, 4), V(3, 3) };
        Assert.False(Geometry.Check(bowTie, bowTie, 1m).IsScaledCopy);
    }
}
