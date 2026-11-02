using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Final G4 2021 lesson. Stage C authored, deliberately UNRUN.
public sealed class Grade4Ordinal149NumberTalkTests
{
 [Fact] public void ExactOriginalFractionsSumsAndDivisionPatternsHold()
 {
  Assert.Equal(1.5,6.0/4);
  Assert.Equal(4.5,6.0*3/4);
  Assert.Equal(3*(6.0/4),6.0*3/4);
  Assert.Equal(44,15+29); Assert.Equal(88,30+58);
  Assert.Equal(2*(15+29),30+58);
  Assert.Equal(7,35/5); Assert.Equal(14,70/5);
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily("usccss.g4.u06.l23.four_operation_contexts"));
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily("usccss.g4.u06.l18.partial_quotients_recording"));
 }
}
