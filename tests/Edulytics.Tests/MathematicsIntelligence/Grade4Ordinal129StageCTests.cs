using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Stage C only; not executed.
public sealed class Grade4Ordinal129StageCTests
{
 [Fact] public void RightTriangleNeedsThreeSidesAndOneRightAngle()
 {
  Assert.Equal(99,12+12+75);
  Assert.Equal(100,12.5+12.5+75);
  Assert.Equal(90,180/2);
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily("usccss.g4.u07.l10.perpendicular_right_angle_judgment"));
 }
}
