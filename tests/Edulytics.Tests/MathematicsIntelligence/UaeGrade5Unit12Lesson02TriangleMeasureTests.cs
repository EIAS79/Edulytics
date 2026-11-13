using System.Globalization;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;

// UAE Grade 5 U12 L02, Arabic Student pp.883–888.
// Digitally verifiable triangle reasoning; ruler/protractor work is teacher reviewed.
public sealed class UaeGrade5Unit12Lesson02TriangleMeasureTests
{
 [Theory]
 [InlineData("uae.g5.u12.l02.congruent_side_count")]
 [InlineData("uae.g5.u12.l02.acute_angle_count")]
 [InlineData("uae.g5.u12.l02.right_angle_count")]
 [InlineData("uae.g5.u12.l02.obtuse_angle_count")]
 [InlineData("uae.g5.u12.l02.third_angle")]
 public void TriangleSideAndAngleAnswersAreExact(string family)
 {
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
  Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var route));
  Assert.NotNull(route);
  var engine=new ExactSkillContractQuestionEngine();
  foreach(int seed in new[]{101,202,303,404})
  {
   var generated=engine.Generate("PED:UAE-MOE-MATH:L5:GENERAL:12:02",
     family,[family],ExactSkillQuestionDifficulty.Standard,5,seed,[]);
   Assert.NotEmpty(generated);
   foreach(var q in generated)
   {
    int a=q.Parameters["a"],b=q.Parameters["b"],c=q.Parameters["c"];
    int oracle=family switch
    {
     "uae.g5.u12.l02.congruent_side_count"=>
       a==b&&b==c?3:a==b||b==c||a==c?2:0,
     "uae.g5.u12.l02.acute_angle_count"=>
       (a<90?1:0)+(b<90?1:0)+(c<90?1:0),
     "uae.g5.u12.l02.right_angle_count"=>
       (a==90?1:0)+(b==90?1:0)+(c==90?1:0),
     "uae.g5.u12.l02.obtuse_angle_count"=>
       (a>90?1:0)+(b>90?1:0)+(c>90?1:0),
     _=>180-a-b
    };
    if(family=="uae.g5.u12.l02.congruent_side_count")
       Assert.True(a+b>c && a+c>b && b+c>a);
    else
       Assert.Equal(180,a+b+c);
    Assert.Equal(oracle.ToString(CultureInfo.InvariantCulture),q.CorrectAnswer);
    Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
    string arabic=new(q.CorrectAnswer.Select(ch=>(char)('٠'+ch-'0')).ToArray());
    Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,arabic));
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"99999"));
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"invalid"));
    var forged=q.Parameters.ToDictionary(kv=>kv.Key,kv=>kv.Value);
    forged.Add("expected",oracle);
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,forged,q.CorrectAnswer));
   }
  }
 }
}
