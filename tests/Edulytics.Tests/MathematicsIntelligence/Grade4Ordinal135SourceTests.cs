using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Stage C authored; UNRUN.
public sealed class Grade4Ordinal135SourceTests
{
 [Fact] public void SourceFractionEqualitiesAndMissingPerimeter()
 {
  Assert.Equal(12.0,1.2+2.4+3.6+4.8,6);
  Assert.Equal(5.0,10.0-(1+2+3+4)/2.0);
  Assert.Equal(16,64-16-18-14);
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily("geometry.perimeter_area.rectangle_missing_side"));
 }
}
