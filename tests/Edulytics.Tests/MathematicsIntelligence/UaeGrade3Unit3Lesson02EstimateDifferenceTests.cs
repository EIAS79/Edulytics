using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Stage D test code written here; NOT_RUN in A-B-C.
public sealed class UaeGrade3Unit3Lesson02EstimateDifferenceTests
{
 [Fact] public void FamilyIsRegisteredAndUsesNumericAnswer()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:03:02",out var c));
  var f=Assert.Single(c!.AllowedQuestionFamilies);
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
  Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
  Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
 }
 [Fact] public void GeneratedEstimatesVerifyAndRejectTamperedPlaces()
 {
  var e=new ExactSkillContractQuestionEngine();
  const string f="uae.g3.u03.l02.estimate_difference_by_rounding";
  foreach(var q in e.Generate("uae-g3-u03-l02",f,[f],ExactSkillQuestionDifficulty.Standard,10,20261109,[]))
  {
   Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
   var p=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
   p["placeA"]=17;
   Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,q.CorrectAnswer));
   Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"not-a-number"));
  }
 }
}
