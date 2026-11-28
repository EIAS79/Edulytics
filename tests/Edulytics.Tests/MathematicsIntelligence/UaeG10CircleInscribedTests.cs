using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
public class UaeG10CircleInscribedTests
{
 [Theory][InlineData(120,60)][InlineData(250,125)][InlineData(180,90)] public void AngleHalfArc(int arc,int angle)=>Assert.Equal(arc,2*angle);
 [Fact] public void CyclicAlgebra()=>Assert.Equal(180,3*29+15+2*29+20);
 [Fact] public void Diameter()=>Assert.Equal(225,9*9+12*12);
}
