using System.Globalization;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;

// UAE Grade 5 Unit 12 Lesson 01, Arabic book pp. 877-882.
// Numeric classifications are exact; independent drawing remains teacher-reviewed.
public sealed class UaeGrade5Unit12Lesson01PolygonsTests
{
 [Theory]
 [InlineData("uae.g5.u12.l01.polygon_or_nonpolygon")]
 [InlineData("uae.g5.u12.l01.polygon_side_count")]
 [InlineData("uae.g5.u12.l01.polygon_vertex_count")]
 [InlineData("uae.g5.u12.l01.regular_polygon")]
 public void SourceAlignedPolygonFamiliesHaveIndependentClassification(string family)
 {
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
  Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
  Assert.NotNull(interaction);
  var engine=new ExactSkillContractQuestionEngine();
  for(int seed=100;seed<108;seed++)
  {
   int target=family is "uae.g5.u12.l01.polygon_side_count" or
     "uae.g5.u12.l01.polygon_vertex_count" ? 6 : 8;
   var generated=engine.Generate("PED:UAE-MOE-MATH:L5:GENERAL:12:01",family,
     [family],ExactSkillQuestionDifficulty.Standard,target,seed,[]);
   Assert.NotEmpty(generated);
   foreach(var problem in generated)
   {
    int v=problem.Parameters["variant"];
    int expected=family switch
    {
     "uae.g5.u12.l01.polygon_or_nonpolygon"=>
       v is 0 or 3 or 5 or 7 ? 1 : 0,
     "uae.g5.u12.l01.regular_polygon"=>
       v is 0 or 2 or 4 or 6 ? 1 : 0,
     _=>3+v
    };
    Assert.Equal(expected.ToString(CultureInfo.InvariantCulture),problem.CorrectAnswer);
    Assert.True(ExactSkillContractQuestionEngine.Verify(family,problem.Parameters,problem.CorrectAnswer));
    string arabicDigits=new(problem.CorrectAnswer.Select(c=>(char)('٠'+c-'0')).ToArray());
    Assert.True(ExactSkillContractQuestionEngine.Verify(family,problem.Parameters,arabicDigits));
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,problem.Parameters,"99999"));
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,problem.Parameters,"invalid"));
    var injected=problem.Parameters.ToDictionary(kv=>kv.Key,kv=>kv.Value);
    injected.Add("expected",expected);
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,injected,problem.CorrectAnswer));
   }
  }
 }
}
