using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Stage C only: test source WRITTEN_NOT_RUN.
public sealed class UaeGrade3Unit2Lesson01AdditionPropertiesTests
{
 [Fact] public void SharedG2FamiliesReusedAndNewAssociativeFamilyConnected()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:02:01",out var c));
  Assert.Equal(3,c!.AllowedQuestionFamilies.Count);
  Assert.Contains("uae.g2.u01.l01.commutative_add",c.AllowedQuestionFamilies);
  Assert.Contains("uae.g2.u01.l01.additive_zero",c.AllowedQuestionFamilies);
  Assert.Contains("uae.g3.u02.l01.associative_add",c.AllowedQuestionFamilies);
  foreach(var f in c.AllowedQuestionFamilies)
  {
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
   Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
  }
 }
 [Fact] public void AssociativePropertiesRecomputeAndTamperingFails()
 {
  var e=new ExactSkillContractQuestionEngine();
  const string f="uae.g3.u02.l01.associative_add";
  foreach(var item in e.Generate("uae-g3-u02-l01",f,[f],ExactSkillQuestionDifficulty.Standard,8,20261109,[]))
  {
   Assert.True(ExactSkillContractQuestionEngine.Verify(f,item.Parameters,item.CorrectAnswer));
   var p=item.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
   p["result"]++;
   Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,item.CorrectAnswer));
   Assert.False(ExactSkillContractQuestionEngine.Verify(f,item.Parameters,"invalid"));
  }
 }
}
