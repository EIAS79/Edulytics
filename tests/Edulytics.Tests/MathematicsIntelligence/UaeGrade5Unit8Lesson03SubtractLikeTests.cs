using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Stage ABC authored; not run until Stage D.
public sealed class UaeGrade5Unit8Lesson03SubtractLikeTests
{
 [Fact] public void TwentyEighthsAreSubtractedAndSimplifiedExactly()
 {
  const string f="uae.g5.u08.l03.subtract_like_extended_fraction";
  var p=new Dictionary<string,int>{{"denominator",28},{"a",14},{"b",8}};
  Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,"٣/١٤"));
  Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"6/28"));
 }
 [Fact] public void ExistingFractionSubtractionFamiliesRemainRouted()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L5:GENERAL:08:03",out var c));
  Assert.Equal("fractions.subtract",c!.SkillId);
  Assert.Equal(4,c.AllowedQuestionFamilies.Count);
  foreach(var f in c.AllowedQuestionFamilies)Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
 }
}