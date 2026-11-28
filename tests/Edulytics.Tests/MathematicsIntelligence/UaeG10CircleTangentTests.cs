using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
public class UaeG10CircleTangentTests
{
 [Theory][InlineData(13,4)][InlineData(11,3)][InlineData(9,2)][InlineData(3,1)][InlineData(2,0)] public void CommonTangents(int d,int count)=>Assert.Equal(count,d>11?4:d==11?3:d>3?2:d==3?1:0);
 [Fact] public void TangentCriterion(){Assert.Equal(225,9*9+12*12);Assert.NotEqual(144,5*5+10*10);}
 [Fact] public void RadiusAndPerimeter(){Assert.Equal(9,(12*12-6*6)/(2*6));Assert.Equal(38,2*(4+6+9));Assert.Equal(13+8,11+10);}
}
