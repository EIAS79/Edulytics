using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Written only, Stage D NOT_RUN.
public sealed class UaeGrade3U01L05RoundingHundredsTests
{
 [Fact] public void FamilyIsRegisteredOnExactExistingEngine()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:01:05",out var c));
  var family=Assert.Single(c!.AllowedQuestionFamilies);
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
  Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
  Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
 }
 [Theory] [InlineData(1483,1500)] [InlineData(1450,1500)]
 [InlineData(1449,1400)] [InlineData(1990,2000)] [InlineData(9999,10000)] [InlineData(5000,5000)]
 public void NearestHundredMathIsExact(int n,int expected)=>Assert.Equal(expected,(n+50)/100*100);
 [Fact] public void GeneratedQuestionsVerifyAndRejectTampering()
 {
  var e=new ExactSkillContractQuestionEngine();
  const string f="uae.g3.u01.l05.round_nearest_hundred";
  foreach(var q in e.Generate("uae-g3-u01-l05",f,[f],ExactSkillQuestionDifficulty.Standard,8,20261109,[]))
  {
   Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
   var altered=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
   altered["result"]+=100;
   Assert.False(ExactSkillContractQuestionEngine.Verify(f,altered,q.CorrectAnswer));
   Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"not a number"));
  }
 }
}
