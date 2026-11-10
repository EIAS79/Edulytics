using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Written for Stage D but deliberately not executed during A–B–C.
public sealed class UaeGrade3Unit2Lesson06BaseTenModelsTests
{
 [Fact] public void G3HandsOnReusesExistingG2ThreeDigitFamilies()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:02:06",out var c));
  Assert.Equal(3,c!.AllowedQuestionFamilies.Count);
  foreach(var f in c.AllowedQuestionFamilies)
  {
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var interaction));
   Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,interaction!.InteractionKind);
  }
 }
}
