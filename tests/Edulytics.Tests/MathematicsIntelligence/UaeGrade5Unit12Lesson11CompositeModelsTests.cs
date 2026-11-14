using System.Globalization;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// UAE G5 unit12 lesson11: original Arabic book pp. 941–946.
// Counting a base plus non-overlapping raised section is additive, not multiplicative.
public sealed class UaeGrade5Unit12Lesson11CompositeModelsTests
{
 [Theory]
 [InlineData("uae.g5.u12.l11.base_layer_cubes")]
 [InlineData("uae.g5.u12.l11.upper_layers_cubes")]
 [InlineData("uae.g5.u12.l11.composite_total_cubes")]
 [InlineData("uae.g5.u12.l11.incorrect_product_correction")]
 public void CompositeUnitCubesAreAddedWithoutOverlaps(string family)
 {
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
  Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var route));
  Assert.NotNull(route);
  var engine=new ExactSkillContractQuestionEngine();
  foreach(int seed in new[]{111,222,333})
  {
   var questions=engine.Generate("PED:UAE-MOE-MATH:L5:GENERAL:12:11",
      family,[family],ExactSkillQuestionDifficulty.Standard,5,seed,[]);
   Assert.NotEmpty(questions);
   foreach(var q in questions)
   {
    int a=q.Parameters["a"],b=q.Parameters["b"],c=q.Parameters["c"];
    Assert.True(a>0&&b>0&&c>0);
    int baseCubes=(a+2)*(b+2), upperCubes=a*b*c;
    int oracle=family switch
    {
     "uae.g5.u12.l11.base_layer_cubes"=>baseCubes,
     "uae.g5.u12.l11.upper_layers_cubes"=>upperCubes,
     _=>baseCubes+upperCubes
    };
    Assert.Equal(oracle.ToString(CultureInfo.InvariantCulture),q.CorrectAnswer);
    Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
    string arabic=new(q.CorrectAnswer.Select(ch=>(char)('٠'+ch-'0')).ToArray());
    Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,arabic));
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"wrong"));
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"999999"));
    var injected=q.Parameters.ToDictionary(kv=>kv.Key,kv=>kv.Value);
    injected.Add("expected",oracle);
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,injected,q.CorrectAnswer));
   }
  }
 }
}
