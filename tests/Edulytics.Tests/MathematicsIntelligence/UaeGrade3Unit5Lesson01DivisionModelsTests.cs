using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Written during A–B–C; NOT_RUN (deferred to Stage D).
public sealed class UaeGrade3Unit5Lesson01DivisionModelsTests
{
 [Fact] public void EqualGroupsFamilyRoutedToExistingExactEngine()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:05:01",out var c));
  var family=Assert.Single(c!.AllowedQuestionFamilies);
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
  Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
  Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
 }
 [Fact] public void GeneratedDivisionIsExactAndRejectsParameterTampering()
 {
  var engine=new ExactSkillContractQuestionEngine();
  const string f="uae.g3.u05.l01.model_equal_groups_division";
  foreach(var q in engine.Generate("g3-u05-l01",f,[f],ExactSkillQuestionDifficulty.Standard,10,20261109,[]))
  {
   Assert.Equal(q.Parameters["total"],q.Parameters["each"]*q.Parameters["groups"]);
   Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
   var altered=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
   altered["total"]++;
   Assert.False(ExactSkillContractQuestionEngine.Verify(f,altered,q.CorrectAnswer));
  }
 }
}
