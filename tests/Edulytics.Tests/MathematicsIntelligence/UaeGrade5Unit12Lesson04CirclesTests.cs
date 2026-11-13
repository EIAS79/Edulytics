using System.Globalization;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;

// UAE Grade5 U12 L04 circles, Student Arabic source pp.897-902.
// Geometric construction by compass is teacher-reviewed.
public sealed class UaeGrade5Unit12Lesson04CirclesTests
{
 [Theory]
 [InlineData("uae.g5.u12.l04.radius_diameter_exact")]
 [InlineData("uae.g5.u12.l04.circle_segment_classify")]
 public void RadiusDiameterAndChordTasksHaveIndependentOracle(string family)
 {
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
  Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var route));
  Assert.NotNull(route);
  var engine=new ExactSkillContractQuestionEngine();
  foreach(var seed in new[]{101,202,303})
  {
   var questions=engine.Generate("PED:UAE-MOE-MATH:L5:GENERAL:12:04",
       family,[family],ExactSkillQuestionDifficulty.Standard,5,seed,[]);
   Assert.NotEmpty(questions);
   foreach(var q in questions)
   {
    var p=q.Parameters;
    int oracle=family=="uae.g5.u12.l04.radius_diameter_exact"
       ? p["mode"]==0 ? 2*p["radius"] : p["radius"]
       : p["variant"] % 4;
    Assert.Equal(oracle.ToString(CultureInfo.InvariantCulture),q.CorrectAnswer);
    Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,q.CorrectAnswer));
    string ar=new(q.CorrectAnswer.Select(c=>(char)('٠'+c-'0')).ToArray());
    Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,ar));
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"99999"));
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"invalid"));
    var forged=p.ToDictionary(x=>x.Key,x=>x.Value);
    forged.Add("expected",oracle);
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,forged,q.CorrectAnswer));
   }
  }
 }
}
