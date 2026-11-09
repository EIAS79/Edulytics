using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Stage C written only; Stage D test execution NOT_RUN.
public sealed class UaeGrade3Unit5Lesson06WordModelTests
{
 [Fact] public void NewTwoStepFamilyAndExistingEqualGroupsUseExactEngine()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:05:06",out var c));
  Assert.Equal(2,c!.AllowedQuestionFamilies.Count);
  foreach(var f in c.AllowedQuestionFamilies)
  {
    Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
    Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
    Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
  }
 }
 [Fact] public void ModeledWordProblemAnswersVerifiedAgainstParameters()
 {
  var engine=new ExactSkillContractQuestionEngine();
  const string f="uae.g3.u05.l06.modeled_division_word_problem";
  foreach(var q in engine.Generate("uae-g3-u05-l06",f,[f],ExactSkillQuestionDifficulty.Standard,12,20261109,[]))
  {
   Assert.Equal(q.Parameters["start"],q.Parameters["remaining"]+q.Parameters["used"]);
   Assert.Equal(q.Parameters["remaining"],q.Parameters["groups"]*q.Parameters["each"]);
   Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
   var altered=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);altered["remaining"]++;
   Assert.False(ExactSkillContractQuestionEngine.Verify(f,altered,q.CorrectAnswer));
  }
 }
}
