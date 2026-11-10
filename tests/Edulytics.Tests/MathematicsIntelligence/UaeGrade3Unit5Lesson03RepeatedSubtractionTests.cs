using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Stage C test source — NOT_RUN.
public sealed class UaeGrade3Unit5Lesson03RepeatedSubtractionTests
{
 [Fact] public void RepeatedSubtractionFamilyExtendsExactEngineAndReusesExistingGroups()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:05:03",out var c));
  Assert.Equal(2,c!.AllowedQuestionFamilies.Count);
  foreach(var f in c.AllowedQuestionFamilies)
  {
    Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
    Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
    Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
  }
 }
 [Fact] public void JumpCountMatchesQuotientAndCorruptedParametersFail()
 {
  var e=new ExactSkillContractQuestionEngine();
  const string f="uae.g3.u05.l03.repeated_subtraction_division";
  foreach(var q in e.Generate("uae-g3-u05-l03",f,[f],ExactSkillQuestionDifficulty.Standard,8,20261109,[]))
  {
    Assert.Equal(q.Parameters["total"],q.Parameters["quotient"]*q.Parameters["divisor"]);
    Assert.Equal(q.Parameters["quotient"].ToString(),q.CorrectAnswer);
    Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
    var bad=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
    bad["quotient"]++;
    Assert.False(ExactSkillContractQuestionEngine.Verify(f,bad,q.CorrectAnswer));
  }
 }
}
