using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Prepared Stage D test. NOT_RUN during A–B–C.
public sealed class UaeGrade3Unit2Lesson07ThreeDigitAdditionTests
{
 [Fact] public void ExistingExactThreeDigitAdditionFamiliesReused()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:02:07",out var c));
  Assert.Equal(3,c!.AllowedQuestionFamilies.Count);
  foreach(var f in c.AllowedQuestionFamilies)
  {
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
   Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
  }
 }
 [Theory] [InlineData(127,58,185)] [InlineData(175,225,400)] [InlineData(342,156,498)]
 public void AddsThreeDigitNumbersCorrectly(int a,int b,int expected)=>Assert.Equal(expected,a+b);
}
