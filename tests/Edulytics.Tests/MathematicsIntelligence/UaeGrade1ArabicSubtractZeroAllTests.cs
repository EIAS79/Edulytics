using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// WRITTEN_NOT_RUN — Stage D target test.
public sealed class UaeGrade1ArabicSubtractZeroAllTests
{
 [Fact]
 public void SubtractZeroOrEntireWhole_ReturnsWholeOrZero()
 {
  const string code="PED:UAE-MOE-MATH:L1:COMMON:03:04";
  const string family="uae.g1.u03.l04.subtract_zero_or_all";
  Assert.True(LessonPracticeContractRegistry.TryResolve(code,out var contract));
  Assert.NotNull(contract);
  Assert.Equal(new[]{family},contract.AllowedQuestionFamilies);
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
  var items=new ExactSkillContractQuestionEngine().Generate("stage18",code,new[]{family},
    ExactSkillQuestionDifficulty.Standard,8,203,Array.Empty<string>());
  Assert.NotEmpty(items);
  Assert.All(items,q=>{
   Assert.InRange(q.Parameters["whole"],1,10);
   Assert.True(q.Parameters["removed"]==0||q.Parameters["removed"]==q.Parameters["whole"]);
   Assert.Equal((q.Parameters["whole"]-q.Parameters["removed"]).ToString(),q.CorrectAnswer);
   Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
   Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"99"));
  });
 }
}
