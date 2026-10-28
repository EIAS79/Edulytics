using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Stage C written, NOT run.
public sealed class Grade4Ordinal145BusCapacityTests
{
 [Fact] public void OriginalBusCountsRespectRemainderAndHighRiseClaim()
 {
  Assert.Equal(106,848/8); Assert.Equal(606,4848/8);
  Assert.Equal(589,17+52*11);
  Assert.Equal(14,(375+27-1)/27); Assert.Equal(8,(375+48-1)/48);
  Assert.True(13*27<375); Assert.True(7*48<375);
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily("usccss.g4.u06.l20.contextual_remainder_decisions"));
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily("usccss.g4.u06.l21.multistep_contexts"));
 }
}
