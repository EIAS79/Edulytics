using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Written test only. NOT_RUN until Stage D.
public sealed class UaeGrade3U04L02RepeatedAdditionReuseTests
{
 [Fact] public void ReusedGrade2FamilySupportsGrade3RepeatedAdditionLesson()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:04:02",out var c));
  var f=Assert.Single(c!.AllowedQuestionFamilies);
  Assert.Equal("uae.g2.u02.l04.repeated_add_equal_groups",f);
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
  Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
  Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
 }
}
