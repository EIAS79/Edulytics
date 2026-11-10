using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Written for Stage D; NOT_RUN during Stage A-B-C.
public sealed class UaeGrade3Unit7Lesson08ExactFactsTests
{
 [Fact] public void FamilyIsEnabledForSpecificLessonOnly()
 {
  const string f="uae.g3.u07.l08.divide_zero_one";
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:07:08",out var c));
  Assert.Contains(f,c!.AllowedQuestionFamilies);
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
  Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var interaction));
  Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,interaction!.InteractionKind);
 }
 [Fact] public void GoldenSeedAndTamperedParameterDetection()
 {
  var engine=new ExactSkillContractQuestionEngine();
  const string f="uae.g3.u07.l08.divide_zero_one";
  foreach(var q in engine.Generate("uae-g3-u07-l08",f,[f],ExactSkillQuestionDifficulty.Standard,12,20261110,[]))
  {
   Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
   var modified=q.Parameters.ToDictionary(z=>z.Key,z=>z.Value);
   modified["result"]++;
   Assert.False(ExactSkillContractQuestionEngine.Verify(f,modified,q.CorrectAnswer));
  }
 }
}
