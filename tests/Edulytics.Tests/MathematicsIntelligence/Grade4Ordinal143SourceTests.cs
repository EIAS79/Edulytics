using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Stage C authored; intentionally UNRUN.
public sealed class Grade4Ordinal143SourceTests
{
 [Fact] public void DivisionDecompositionsMatch2021Source()
 {
  Assert.Equal(16,48/3);
  Assert.Equal(160,480/3);
  Assert.Equal(1493,7465/5);
  Assert.Equal(409,3681/9);
  Assert.Equal(3681,400*9+81);
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily("usccss.g4.u06.l18.partial_quotients_recording"));
 }
}
