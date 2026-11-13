using System.Globalization;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// UAE G5 U12 lesson13 p.953-958: model the problem, do not double count
// the shared start/end station on a closed circular track.
public sealed class UaeGrade5Unit12Lesson13ModelProblemSolvingTests
{
 [Theory]
 [InlineData("uae.g5.u12.l13.model_total_units")]
 [InlineData("uae.g5.u12.l13.model_missing_layers")]
 [InlineData("uae.g5.u12.l13.model_loop_stations")]
 [InlineData("uae.g5.u12.l13.model_tiles_per_row")]
 public void ModelBasedProblemSolvingUsesValidIndependentOracle(string family)
 {
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
  Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var route));
  Assert.NotNull(route);
  var engine=new ExactSkillContractQuestionEngine();
  foreach(int seed in new[]{117,227,337})
  {
   var problems=engine.Generate("PED:UAE-MOE-MATH:L5:GENERAL:12:13",
      family,[family],ExactSkillQuestionDifficulty.Standard,5,seed,[]);
   Assert.NotEmpty(problems);
   foreach(var q in problems)
   {
    int a=q.Parameters["a"],b=q.Parameters["b"],c=q.Parameters["c"];
    Assert.True(a>0 && b>0 && c>0);
    int oracle=family switch
    {
     "uae.g5.u12.l13.model_total_units"=>a*b*c,
     "uae.g5.u12.l13.model_missing_layers"=>c,
     _=>a
    };
    Assert.Equal(oracle.ToString(CultureInfo.InvariantCulture),q.CorrectAnswer);
    Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
    string arabic=new(q.CorrectAnswer.Select(ch=>(char)('٠'+ch-'0')).ToArray());
    Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,arabic));
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"invalid"));
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"999999"));
    var forged=q.Parameters.ToDictionary(kv=>kv.Key,kv=>kv.Value);
    forged.Add("expected",oracle);
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,forged,q.CorrectAnswer));
   }
  }
 }
}
