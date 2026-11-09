using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Tests authored only; not executed during A-B-C.
public sealed class UaeGrade3Unit2Lesson04MentalAdditionTests
{
 [Fact] public void FamilyIsLessonScopedAndUsesExistingNumericInteraction()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:02:04",out var c));
  var f=Assert.Single(c!.AllowedQuestionFamilies);
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
  Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
  Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
 }
 [Fact] public void SumRemainsExactUnderMentalCompensationAndRejectsTampering()
 {
  var engine=new ExactSkillContractQuestionEngine();
  const string f="uae.g3.u02.l04.mental_addition";
  foreach(var q in engine.Generate("uae-g3-u02-l04",f,[f],ExactSkillQuestionDifficulty.Standard,8,20261109,[]))
  {
   Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
   var p=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
   p["result"]++;
   Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,q.CorrectAnswer));
  }
 }
}
