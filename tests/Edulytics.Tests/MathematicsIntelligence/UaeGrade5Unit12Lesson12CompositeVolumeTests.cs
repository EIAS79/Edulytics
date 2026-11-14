using System.Globalization;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// UAE Grade5 U12 L12, Arabic printed pp.947-952.
// Components must be non-overlapping when volumes are added;
// subtract a defined cut-out when starting from an enclosing prism.
public sealed class UaeGrade5Unit12Lesson12CompositeVolumeTests
{
 [Theory]
 [InlineData("uae.g5.u12.l12.sum_two_prisms")]
 [InlineData("uae.g5.u12.l12.base_plus_top")]
 [InlineData("uae.g5.u12.l12.prism_with_removed_section")]
 [InlineData("uae.g5.u12.l12.upper_volume_from_total")]
 public void ExactCompositeVolumeHasCorrectAdditiveOrSubtractiveOracle(string family)
 {
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
  Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var route));
  Assert.NotNull(route);
  var engine=new ExactSkillContractQuestionEngine();
  foreach(var seed in new[]{111,222,333})
  {
   var problems=engine.Generate("PED:UAE-MOE-MATH:L5:GENERAL:12:12",
      family,[family],ExactSkillQuestionDifficulty.Standard,5,seed,[]);
   Assert.NotEmpty(problems);
   foreach(var q in problems)
   {
    int a=q.Parameters["a"],b=q.Parameters["b"],c=q.Parameters["c"];
    Assert.True(a>0&&b>0&&c>0);
    int baseVolume=(a+2)*(b+2)*c;
    int topVolume=2*a*b;
    int oracle=family switch
    {
     "uae.g5.u12.l12.sum_two_prisms"=>a*b*c+(a+1)*b*(c+1),
     "uae.g5.u12.l12.base_plus_top"=>baseVolume+topVolume,
     "uae.g5.u12.l12.prism_with_removed_section"=>(a+2)*(b+2)*(c+2)-a*b*c,
     _=>topVolume
    };
    Assert.True(oracle>0);
    Assert.Equal(oracle.ToString(CultureInfo.InvariantCulture),q.CorrectAnswer);
    Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
    string arabic=new(q.CorrectAnswer.Select(ch=>(char)('٠'+ch-'0')).ToArray());
    Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,arabic));
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"wrong"));
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"999999"));
    var tampered=q.Parameters.ToDictionary(kv=>kv.Key,kv=>kv.Value);
    tampered.Add("expected",oracle);
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,tampered,q.CorrectAnswer));
   }
  }
 }
}
