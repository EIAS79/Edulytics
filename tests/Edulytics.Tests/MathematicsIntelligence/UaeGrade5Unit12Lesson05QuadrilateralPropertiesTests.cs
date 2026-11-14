using System.Globalization;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;

// UAE Grade 5 U12 L05, student Arabic printed pages 903-908.
// Physical quadrilateral measurements are teacher-reviewed.
public sealed class UaeGrade5Unit12Lesson05QuadrilateralPropertiesTests
{
 [Theory]
 [InlineData("uae.g5.u12.l05.opposite_parallel_pairs")]
 [InlineData("uae.g5.u12.l05.opposite_equal_pairs")]
 [InlineData("uae.g5.u12.l05.right_angle_count")]
 [InlineData("uae.g5.u12.l05.opposite_equal_angle_pairs")]
 public void QuadrilateralOppositePropertiesAreExact(string family)
 {
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
  Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var route));
  Assert.NotNull(route);
  var engine=new ExactSkillContractQuestionEngine();
  foreach(int seed in new[]{202,303,404})
  {
   var questions=engine.Generate("PED:UAE-MOE-MATH:L5:GENERAL:12:05",
     family,[family],ExactSkillQuestionDifficulty.Standard,5,seed,[]);
   Assert.NotEmpty(questions);
   foreach(var q in questions)
   {
    int kind=q.Parameters["kind"];
    Assert.InRange(kind,0,7);
    int oracle=family switch
    {
     "uae.g5.u12.l05.opposite_parallel_pairs"=>new[]{2,2,2,2,1,1,0,0}[kind],
     "uae.g5.u12.l05.opposite_equal_pairs"=>new[]{2,2,2,2,0,0,0,0}[kind],
     "uae.g5.u12.l05.right_angle_count"=>new[]{4,4,0,0,0,2,0,0}[kind],
     _=>new[]{2,2,2,2,0,0,0,1}[kind]
    };
    Assert.Equal(oracle.ToString(CultureInfo.InvariantCulture),q.CorrectAnswer);
    Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
    string ar=new(q.CorrectAnswer.Select(ch=>(char)('٠'+ch-'0')).ToArray());
    Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,ar));
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"99999"));
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"bad"));
    var forged=q.Parameters.ToDictionary(kv=>kv.Key,kv=>kv.Value);
    forged.Add("result",oracle);
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,forged,q.CorrectAnswer));
   }
  }
 }
}
