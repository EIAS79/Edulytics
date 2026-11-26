using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
public class UaeG10CircleSecantAngleTests
{
 [Theory][InlineData(80,120,100)][InlineData(50,100,75)] public void Inside(int a,int b,int angle)=>Assert.Equal(2*angle,a+b);
 [Fact] public void Outside(){Assert.Equal(58,(150-34)/2);Assert.Equal(72,180-108);Assert.Equal(122,(360-116)/2);}
 [Fact] public void Algebra(){Assert.Equal(160,3*30+10+30+30);Assert.Equal(88,5*20+20-(20+12));}
}
