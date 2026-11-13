using System.Globalization;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// UAE G5 U11 L08 source pp835-840, StageD NOT_RUN.
public sealed class UaeGrade5Unit11Lesson08MeanTests
{
 [Theory]
 [InlineData("uae.g5.u11.l08.arithmetic_mean_five")]
 [InlineData("uae.g5.u11.l08.missing_value_given_mean")]
 [InlineData("uae.g5.u11.l08.mean_half_decimal")]
 public void ArithmeticMeanCanBeExactDecimalOrMissingValue(string family)
 {
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
  Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
  Assert.NotNull(interaction);
  var engine=new ExactSkillContractQuestionEngine();
  var questions=engine.Generate("PED:UAE-MOE-MATH:L5:GENERAL:11:08",
      family,[family],ExactSkillQuestionDifficulty.Standard,8,20261113,[]);
  foreach(var q in questions)
  {
   var p=q.Parameters;
   decimal answer=family.EndsWith("arithmetic_mean_five",StringComparison.Ordinal)?
     (p["a"]+p["b"]+p["c"]+p["d"]+p["e"])/5m:
      family.EndsWith("missing_value_given_mean",StringComparison.Ordinal)?
     5*p["m"]-p["a"]-p["b"]-p["c"]-p["d"]:
     (p["a"]+p["b"]+p["c"]+p["d"])/4m;
   Assert.Equal(answer.ToString("0.##",CultureInfo.InvariantCulture),q.CorrectAnswer);
   Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,q.CorrectAnswer));
   Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"99999"));
  }
 }
}
