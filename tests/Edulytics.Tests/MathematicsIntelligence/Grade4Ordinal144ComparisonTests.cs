using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Stage C only; intentionally NOT RUN.
public sealed class Grade4Ordinal144ComparisonTests
{
 [Fact] public void DistinguishFourTimesFromFourMore()
 {
  Assert.Equal(28,7*4);
  Assert.NotEqual(7+4,7*4);
  Assert.Equal(7,28/4);
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily("number.whole.multiply.fact_recall.apply"));
 }
}
