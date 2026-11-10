using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Stage D tests: WRITTEN_NOT_RUN during A-B-C.
public sealed class UaeGrade3Unit11Lesson07ExactFactsTests
{
 [Fact] public void FamilyIsLessonScopedAndUsesExactEngine()
 {
  const string f="uae.g3.u11.l07.work_backward";
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:11:07",out var c));
  Assert.Contains(f,c!.AllowedQuestionFamilies);
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
  Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var interaction));
  Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,interaction!.InteractionKind);
 }
 [Fact] public void FixedSeedAndTamperingAreCheckedIndependently()
 {
  const string f="uae.g3.u11.l07.work_backward";
  var engine=new ExactSkillContractQuestionEngine();
  foreach(var q in engine.Generate("uae-g3-u11-l07",f,[f],ExactSkillQuestionDifficulty.Standard,12,20261110,[]))
  {
   Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
   var tampered=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
   tampered["result"]++;
   Assert.False(ExactSkillContractQuestionEngine.Verify(f,tampered,q.CorrectAnswer));
  }
 }
}
