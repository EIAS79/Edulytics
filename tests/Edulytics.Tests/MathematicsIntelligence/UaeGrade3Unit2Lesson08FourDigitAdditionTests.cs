using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Prepared Stage D tests; NOT_RUN in A–B–C.
public sealed class UaeGrade3Unit2Lesson08FourDigitAdditionTests
{
 [Fact] public void FourDigitFamilyUsesSharedNumericEngine()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:02:08",out var c));
  var f=Assert.Single(c!.AllowedQuestionFamilies);
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
  Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
  Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
 }
 [Theory] [InlineData(4376,1365,5741)] [InlineData(3295,3999,7294)]
 [InlineData(5555,3555,9110)] [InlineData(4091,2238,6329)]
 public void FourDigitsAreAddedExactly(int a,int b,int sum)=>Assert.Equal(sum,a+b);
 [Fact] public void GeneratedQuestionVerificationRejectsTampering()
 {
  var e=new ExactSkillContractQuestionEngine();
  const string f="uae.g3.u02.l08.add_four_digit";
  foreach(var q in e.Generate("uae-g3-u02-l08",f,[f],ExactSkillQuestionDifficulty.Standard,8,20261109,[]))
  {
   Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
   var p=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);p["result"]++;
   Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,q.CorrectAnswer));
  }
 }
}
