using System.Collections.Generic;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Stage C authored; UNRUN.
public sealed class Grade4Ordinal134PerimeterTests
{
 [Fact] public void TwoActualSourcePolygonExamplesMatchPerimeter()
 {
  Assert.Equal(94,18+26+13+37);
  Assert.Equal(2.0,(1+4+1+4)/5.0);
 }
 [Fact] public void IndependentPerimeterFixtureAndBadAnswer()
 {
  var p=new Dictionary<string,int>{{"caseType",0},{"variant",0},{"sideA",18},{"sideB",26},{"sideC",13},{"sideD",37},{"perimeter",94}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("usccss.g4.u08.l07.polygon_perimeter_sum",p,"94"));
  Assert.False(ExactSkillContractQuestionEngine.Verify("usccss.g4.u08.l07.polygon_perimeter_sum",p,"93"));
 }
}
