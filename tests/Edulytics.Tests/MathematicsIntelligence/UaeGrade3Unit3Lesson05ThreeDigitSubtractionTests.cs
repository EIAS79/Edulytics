using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Source written during Stage C; NOT_RUN.
public sealed class UaeGrade3Unit3Lesson05ThreeDigitSubtractionTests
{
 [Fact] public void ExistingFourExactSubtractionFamiliesAreReused()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:03:05",out var c));
  Assert.Equal(4,c!.AllowedQuestionFamilies.Count);
  foreach(var f in c.AllowedQuestionFamilies)
  {
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
   Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
  }
 }
 [Fact] public void GeneratedQuestionsVerifyAndRejectAlteredValues()
 {
  var e=new ExactSkillContractQuestionEngine();
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:03:05",out var c));
  foreach(var f in c!.AllowedQuestionFamilies)
  foreach(var q in e.Generate("g3-u03-l05",f,[f],ExactSkillQuestionDifficulty.Standard,5,20261109,[]))
  {
   Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
   var p=q.Parameters.ToDictionary(k=>k.Key,v=>v.Value);p["result"]++;
   Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,q.CorrectAnswer));
  }
 }
}
