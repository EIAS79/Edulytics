using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Written Stage C tests: NOT RUN during A-B-C.
public sealed class UaeGrade3Unit3Lesson04BaseTenRegroupingTests
{
 [Fact] public void ExistingThreeExactFamiliesReusedWithoutCreatingNewEngine()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:03:04",out var c));
  Assert.Equal(3,c!.AllowedQuestionFamilies.Count);
  foreach(var f in c.AllowedQuestionFamilies)
  {
   Assert.StartsWith("uae.g2.u07.",f);
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
   Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
  }
 }
 [Fact] public void GeneratedRegroupingChecksExactResultAndRejectsTampering()
 {
  var e=new ExactSkillContractQuestionEngine();
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:03:04",out var c));
  foreach(var f in c!.AllowedQuestionFamilies)
  foreach(var q in e.Generate("g3-u03-l04",f,[f],ExactSkillQuestionDifficulty.Standard,5,20261109,[]))
  {
   Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
   var wrong=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
   wrong["result"]++;
   Assert.False(ExactSkillContractQuestionEngine.Verify(f,wrong,q.CorrectAnswer));
  }
 }
}
