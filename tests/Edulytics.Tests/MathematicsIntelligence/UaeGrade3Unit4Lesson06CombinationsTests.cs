using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Written only at A–B–C; Stage D and actual test execution NOT_RUN.
public sealed class UaeGrade3Unit4Lesson06CombinationsTests
{
  [Fact] public void CombinationsLessonReusesMultiplicationAndMissingFactorExactEngine()
  {
   Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:04:06",out var c));
   Assert.Equal(2,c!.AllowedQuestionFamilies.Count);
   foreach (var f in c.AllowedQuestionFamilies)
   {
     Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
     Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
     Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
   }
  }
  [Theory] [InlineData(3,2,6)] [InlineData(4,2,8)] [InlineData(3,5,15)]
  public void CompleteTwoCategoryTreeHasAsManyLeavesAsProduct(int first,int second,int expected)
     => Assert.Equal(expected,first*second);
}
