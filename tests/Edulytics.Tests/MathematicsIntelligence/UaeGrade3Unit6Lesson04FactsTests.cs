using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Stage C source test only, deliberately NOT_RUN.
public sealed class UaeGrade3Unit6Lesson04FactsTests
{
 [Fact] public void FactFamiliesAreBoundToExistingLessonPractice()
 {
  const string code="PED:UAE-MOE-MATH:L3:COMMON:06:04";
  const string f="uae.g3.u06.l04.multiply_by_five";
  Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var contract));
  Assert.Contains(f,contract!.AllowedQuestionFamilies);
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
  Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
  Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
 }
 [Fact] public void GeneratedNumericAnswersRejectTampering()
 {
  const string f="uae.g3.u06.l04.multiply_by_five";
  var engine=new ExactSkillContractQuestionEngine();
  foreach(var item in engine.Generate("uae-g3-u06-l04",f,[f],ExactSkillQuestionDifficulty.Standard,8,20261109,[]))
  {
   Assert.Equal(item.Parameters["a"]*item.Parameters["b"],int.Parse(item.CorrectAnswer));
   Assert.True(ExactSkillContractQuestionEngine.Verify(f,item.Parameters,item.CorrectAnswer));
   var corrupted=item.Parameters.ToDictionary(x=>x.Key,x=>x.Value);corrupted["result"]++;
   Assert.False(ExactSkillContractQuestionEngine.Verify(f,corrupted,item.CorrectAnswer));
  }
 }
}
