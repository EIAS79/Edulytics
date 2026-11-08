using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// WRITTEN_NOT_RUN until Stage D.
public sealed class UaeGrade1ArabicSubtractEightTests
{
 [Fact] public void FixedMinuend_IsExact()
 {
  const string code="PED:UAE-MOE-MATH:L1:COMMON:03:10",family="uae.g1.u03.l10.subtract_8";
  Assert.True(LessonPracticeContractRegistry.TryResolve(code,out var contract));
  Assert.NotNull(contract);
  Assert.Equal(new[]{family},contract.AllowedQuestionFamilies);
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
  var items=new ExactSkillContractQuestionEngine().Generate("stage18",code,new[]{family},ExactSkillQuestionDifficulty.Standard,8,2929,Array.Empty<string>());
  Assert.NotEmpty(items);
  Assert.All(items,q=>{
    Assert.Equal(8,q.Parameters["whole"]);
    Assert.InRange(q.Parameters["removed"],0,8);
    Assert.Equal((8-q.Parameters["removed"]).ToString(),q.CorrectAnswer);
    Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"99"));
  });
 }
}
