using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Written at A–B–C, NOT_RUN pending Stage D.
public sealed class UaeGrade3Unit5Lesson05InverseFactsTests
{
 [Fact] public void ExistingTwoFamiliesCoverFactFamilyNumericParts()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:05:05",out var c));
  Assert.Equal(2,c!.AllowedQuestionFamilies.Count);
  foreach(var f in c.AllowedQuestionFamilies)
  {
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
   Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
  }
 }
 [Theory] [InlineData(4,8,32)] [InlineData(2,7,14)] [InlineData(3,6,18)]
 public void MultiplicationAndTwoInverseDivisionsHold(int a,int b,int total)
 {
  Assert.Equal(total,a*b);
  Assert.Equal(total,b*a);
  Assert.Equal(a,total/b);
  Assert.Equal(b,total/a);
 }
}
