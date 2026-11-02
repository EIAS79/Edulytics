using System.Collections.Generic;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Stage C; UNRUN.
public sealed class Grade4Ordinal131SymmetryAxisTests
{
 [Theory]
 [InlineData(0,4)] [InlineData(1,2)] [InlineData(2,3)] [InlineData(3,1)]
 [InlineData(4,0)] [InlineData(5,2)] [InlineData(6,1)] [InlineData(7,0)]
 public void EightDefinedShapesHaveCorrectReflectionCounts(int mode,int axes)
 {
  var p=new Dictionary<string,int>{{"caseType",mode},{"variant",0},{"axisCount",axes}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("usccss.g4.u08.l04.symmetry_axis_counts",p,axes.ToString()));
  Assert.False(ExactSkillContractQuestionEngine.Verify("usccss.g4.u08.l04.symmetry_axis_counts",p,(axes+1).ToString()));
 }
}
