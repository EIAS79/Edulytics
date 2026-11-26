using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
public class UaeG10CircleChordTests
{
    [Theory]
    [InlineData(5,3,8)]
    [InlineData(10,6,16)]
    [InlineData(20,12,32)]
    public void ChordLengthFollowsPythagoras(int r,int d,int c) => Assert.Equal(r*r,d*d+(c/2)*(c/2));
    [Fact] public void ApproximateDistance() => Assert.Equal(9.22,System.Math.Round(System.Math.Sqrt(121-36),2));
    [Fact] public void EqualChordExpression() => Assert.Equal(23,3*7+2);
}
