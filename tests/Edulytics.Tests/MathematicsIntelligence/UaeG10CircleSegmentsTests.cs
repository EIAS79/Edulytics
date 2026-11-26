using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
public class UaeG10CircleSegmentsTests
{
 [Theory][InlineData(1)][InlineData(8)] public void Products(int k){Assert.Equal(3*k*8*k,4*k*6*k);Assert.Equal(3*k*12*k,4*k*9*k);Assert.Equal(6*k*6*k,3*k*(3*k+9*k));}
 [Fact] public void Sagitta(){Assert.Equal(81,3*(2*15-3));Assert.Equal(64,4*16);}
 [Fact] public void PositiveRoot(){Assert.Equal(36,4*(4+5));Assert.Equal(144,6*(6+18));Assert.Equal(8.06,System.Math.Round(System.Math.Sqrt(65),2));}
}
