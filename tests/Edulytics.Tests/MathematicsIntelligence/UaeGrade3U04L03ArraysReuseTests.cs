using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Source test not executed in Stage A–B–C.
public sealed class UaeGrade3U04L03ArraysReuseTests
{
 [Fact] public void Grade3ArraysReuseExistingExactPracticeFamily()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:04:03",out var c));
  var f=Assert.Single(c!.AllowedQuestionFamilies);
  Assert.Equal("uae.g2.u02.l05.repeated_add_arrays",f);
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
  Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
  Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
 }
}
