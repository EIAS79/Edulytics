using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Stage C only, tests NOT_RUN.
public sealed class UaeGrade3Unit2Lesson02TablePatternsTests
{
 [Fact] public void PatternFamiliesAreRegisteredAndExact()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:02:02",out var c));
  Assert.Equal(3,c!.AllowedQuestionFamilies.Count);
  foreach(var f in c.AllowedQuestionFamilies)
  {
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
   Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
  }
 }
 [Fact] public void NumericAnswersRejectCorruptedGeneratedParameters()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:02:02",out var c));
  var e=new ExactSkillContractQuestionEngine();
  foreach(var f in c!.AllowedQuestionFamilies)
  foreach(var q in e.Generate("uae-g3-u02-l02",f,[f],ExactSkillQuestionDifficulty.Standard,4,20261109,[]))
  {
   Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
   var p=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);p["result"]++;
   Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,q.CorrectAnswer));
   Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"not a number"));
  }
 }
}
