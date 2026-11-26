using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
public class UaeG10CircleEquationTests
{
 [Theory][InlineData(-8,8,1)][InlineData(0,0,6)][InlineData(8,-8,6)] public void StandardGeneral(int h,int k,int m){int r=5*m,f=h*h+k*k-r*r;Assert.Equal(r*r,h*h+k*k-f);Assert.Equal(r*r,9*m*m+16*m*m);}
 [Fact] public void Intersection(){Assert.Equal(25,4*4+3*3);Assert.Equal(-9,16-25);Assert.Equal(10,2*5);}
 [Fact] public void Degenerate(){Assert.Equal(0,-5+4+1);Assert.Equal(-3,-8+1+4);}
}
