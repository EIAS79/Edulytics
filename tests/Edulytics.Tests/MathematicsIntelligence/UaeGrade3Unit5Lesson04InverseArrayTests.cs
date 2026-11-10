using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Stage C only, NOT_RUN.
public sealed class UaeGrade3Unit5Lesson04InverseArrayTests
{
 [Fact] public void InverseModelsReuseAlreadyImplementedExactFamilies()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:05:04",out var c));
  Assert.Equal(2,c!.AllowedQuestionFamilies.Count);
  foreach(var f in c.AllowedQuestionFamilies)
  {
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
   Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
  }
 }
 [Theory] [InlineData(32,4,8)] [InlineData(18,3,6)] [InlineData(24,8,3)]
 public void RelatedMultiplicationRestoresDividend(int total,int divisor,int quotient)
 {
  Assert.Equal(total,divisor*quotient);
  Assert.Equal(quotient,total/divisor);
 }
}
