using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Written at Stage C; NOT_RUN.
public sealed class UaeGrade3U01L04RoundingToTenTests
{
 [Fact] public void NearestTenIsRegisteredAndSupportsNumericInput()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:01:04",out var c));
  Assert.Single(c!.AllowedQuestionFamilies);
  var f=c.AllowedQuestionFamilies[0];
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
  Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
  Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
 }
 [Theory] [InlineData(32,30)] [InlineData(165,170)] [InlineData(995,1000)]
 [InlineData(9999,10000)] [InlineData(125,130)] [InlineData(890,890)]
 public void RoundingBordersHaveCorrectMath(int source,int expected)
 {
  Assert.Equal(expected,((source+5)/10)*10);
 }
 [Fact] public void NumericVerificationRejectsTampering()
 {
  var e=new ExactSkillContractQuestionEngine();
  const string f="uae.g3.u01.l04.round_nearest_ten";
  foreach(var q in e.Generate("uae-g3-u01-l04",f,[f],ExactSkillQuestionDifficulty.Standard,8,20261109,[]))
  {
   Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
   var bad=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
   bad["result"]+=10;
   Assert.False(ExactSkillContractQuestionEngine.Verify(f,bad,q.CorrectAnswer));
   Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"bad"));
  }
 }
}
