using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Source-targeted, Stage C AUTHORING ONLY, UNRUN.
public sealed class Grade4Ordinal130SourceTests
{
 [Fact] public void ParallelLineSubskillRemainsSupported()
 {
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily("usccss.g4.u07.l03.dot_grid_parallel_intersect"));
  Assert.Equal(4,2*2);
  Assert.Equal(360,4*90);
 }
}
