using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Grade 4 2021 ordinal 126; Stage C authored, UNRUN.
public sealed class Grade4Ordinal126SourceMathematicsStageCTests
{
  [Fact] public void ReusesPreviouslyDefinedExactPracticeFamily()
  {
    Assert.True(ExactSkillContractQuestionEngine.SupportsFamily("usccss.g4.u07.l13.additive_angle_part"));
  }
  [Fact] public void SourceNumericalBenchmarksAreConsistent()
  {
    Assert.Equal(28,90-62);
    Assert.Equal(180,71+109);
  }
}
