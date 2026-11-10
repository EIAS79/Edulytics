using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Written only, not executed in A–B–C.
public sealed class UaeGrade3Unit2Lesson03AddPatternsTests
{
 [Fact] public void AllExactFamiliesHaveNumericInteraction()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:02:03",out var c));
  Assert.Equal(3,c!.AllowedQuestionFamilies.Count);
  foreach(var f in c.AllowedQuestionFamilies)
  {
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
   Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
  }
 }
 [Fact] public void AdditiveStepVerifiesAndRejectsCorruptedSource()
 {
  var e=new ExactSkillContractQuestionEngine();
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:02:03",out var c));
  foreach(var f in c!.AllowedQuestionFamilies)
  foreach(var q in e.Generate("uae-g3-u02-l03",f,[f],ExactSkillQuestionDifficulty.Standard,4,20261109,[]))
  {
   Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
   var p=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
   p["step"]=3;
   Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,q.CorrectAnswer));
  }
 }
}
