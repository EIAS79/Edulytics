using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
public class UaeG10CircleAreaTests
{
 [Theory][InlineData(6,30,3)][InlineData(36,330,1188)] public void Sector(int r,int angle,int coefficient)=>Assert.Equal(360*coefficient,r*r*angle);
 [Fact] public void SegmentAndComposite(){Assert.Equal(10.27,System.Math.Round(9*System.Math.PI-18,2));Assert.Equal(30.90,System.Math.Round(144-36*System.Math.PI,2));Assert.Equal(55,8*8-3*3);}
 [Fact] public void Inverse(){Assert.Equal(90,360*20/80);Assert.Equal(36,12*360/120);}
}
