using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Written for Stage D; NOT_RUN in A-B-C.
public sealed class UaeGrade3Unit7Lesson01ExactFactsTests
{
 [Fact] public void RegisteredSharedEngineAndArabicFamily()
 {
  const string f="uae.g3.u07.l01.multiply_by_three";
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:07:01",out var contract));
  Assert.Contains(f,contract!.AllowedQuestionFamilies);
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
  Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var interaction));
  Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,interaction!.InteractionKind);
 }
 [Fact] public void GoldenSeedAndTamperRejection()
 {
  var engine=new ExactSkillContractQuestionEngine();
  const string f="uae.g3.u07.l01.multiply_by_three";
  foreach(var item in engine.Generate("uae-g3-u07-l01",f,[f],ExactSkillQuestionDifficulty.Standard,12,20261110,[]))
  {
   Assert.Equal(item.Parameters["b"]*3,int.Parse(item.CorrectAnswer));
   Assert.True(ExactSkillContractQuestionEngine.Verify(f,item.Parameters,item.CorrectAnswer));
   var changed=item.Parameters.ToDictionary(z=>z.Key,z=>z.Value);changed["result"]++;
   Assert.False(ExactSkillContractQuestionEngine.Verify(f,changed,item.CorrectAnswer));
  }
 }
}
