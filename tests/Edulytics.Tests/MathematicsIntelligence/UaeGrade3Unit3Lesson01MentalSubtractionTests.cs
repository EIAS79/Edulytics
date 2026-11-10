using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Tests authored at Stage C, NOT_RUN.
public sealed class UaeGrade3Unit3Lesson01MentalSubtractionTests
{
 [Fact] public void LessonReusesGrade2ExactAndAddsMentalCompensation()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:03:01",out var c));
  Assert.Equal(3,c!.AllowedQuestionFamilies.Count);
  foreach(var f in c.AllowedQuestionFamilies)
  {
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var interaction));
   Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,interaction!.InteractionKind);
  }
 }
 [Fact] public void SubtractionGeneratedQuestionsVerifyAndRejectBadParameters()
 {
  var e=new ExactSkillContractQuestionEngine();
  const string f="uae.g3.u03.l01.mental_split_subtract";
  foreach(var q in e.Generate("uae-g3-u03-l01",f,[f],ExactSkillQuestionDifficulty.Standard,10,20261109,[]))
  {
   Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
   var altered=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
   altered["result"]++;
   Assert.False(ExactSkillContractQuestionEngine.Verify(f,altered,q.CorrectAnswer));
   Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"invalid"));
  }
 }
}
