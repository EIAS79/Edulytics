using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Source tests only: intentionally NOT_RUN until Stage D.
public sealed class UaeGrade3Unit6Lesson08ExactFactsTests
{
 [Fact] public void ExactFamilyIsRegisteredOnExistingPracticeEngine()
 {
  const string f="uae.g3.u06.l08.multiply_by_tens";
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:06:08",out var c));
  Assert.Contains(f,c!.AllowedQuestionFamilies);
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
  Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var interaction));
  Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,interaction!.InteractionKind);
 }
 [Fact] public void GeneratedResultsAreValidAndCannotBeTamperedWith()
 {
  var engine=new ExactSkillContractQuestionEngine();
  const string f="uae.g3.u06.l08.multiply_by_tens";
  foreach(var q in engine.Generate("uae-g3-u06-l08",f,[f],ExactSkillQuestionDifficulty.Standard,8,20261109,[]))
  {
   Assert.Equal(q.Parameters["a"]*q.Parameters["b"],int.Parse(q.CorrectAnswer));
   Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
   var p=q.Parameters.ToDictionary(z=>z.Key,z=>z.Value);p["result"]++;
   Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,q.CorrectAnswer));
  }
 }
}
