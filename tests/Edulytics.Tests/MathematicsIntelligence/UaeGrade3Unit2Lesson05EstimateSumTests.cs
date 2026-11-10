using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Authored for Stage D. NOT_RUN in A–B–C.
public sealed class UaeGrade3Unit2Lesson05EstimateSumTests
{
 [Fact] public void FamilyResolvesToNumericInput()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:02:05",out var c));
  var f=Assert.Single(c!.AllowedQuestionFamilies);
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
  Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
  Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
 }
 [Theory] [InlineData(153,138,10,290)] [InlineData(153,138,100,300)]
 [InlineData(380,437,100,800)] [InlineData(3333,4555,1000,8000)]
 public void RoundsEachAddendIndependently(int a,int b,int place,int expected)
 {
  Assert.Equal(expected,((a+place/2)/place)*place+((b+place/2)/place)*place);
 }
 [Fact] public void GeneratedScoredEstimatesRejectAlteredResult()
 {
  var e=new ExactSkillContractQuestionEngine();
  const string f="uae.g3.u02.l05.estimate_sum_by_rounding";
  foreach(var q in e.Generate("uae-g3-u02-l05",f,[f],ExactSkillQuestionDifficulty.Standard,8,20261109,[]))
  {
   Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
   var p=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
   p["result"]+=100;
   Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,q.CorrectAnswer));
  }
 }
}
