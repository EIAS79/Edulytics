using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Written only; NOT_RUN in Stage A-B-C.
public sealed class UaeGrade3Unit3Lesson06FourDigitSubtractionTests
{
 [Fact] public void ExactFourDigitAndReusedWordProblemsAreAvailable()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:03:06",out var c));
  Assert.Equal(2,c!.AllowedQuestionFamilies.Count);
  foreach(var f in c.AllowedQuestionFamilies)
  {
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
   Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
  }
 }
 [Fact] public void GeneratedAnswersAreMathematicallyChecked()
 {
  var engine=new ExactSkillContractQuestionEngine();
  const string f="uae.g3.u03.l06.four_digit_subtract_exact";
  foreach(var item in engine.Generate("g3-u03-l06",f,[f],ExactSkillQuestionDifficulty.Standard,8,20261109,[]))
  {
   Assert.True(ExactSkillContractQuestionEngine.Verify(f,item.Parameters,item.CorrectAnswer));
   var p=item.Parameters.ToDictionary(k=>k.Key,v=>v.Value);p["result"]++;
   Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,item.CorrectAnswer));
  }
 }
}
