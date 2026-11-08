using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// WRITTEN_NOT_RUN during A-B-C; Stage D will execute it.
public sealed class UaeGrade1ArabicSubtractFourFiveTests
{
 [Fact] public void SubtractFromFourOrFive_ResultsAreExact()
 {
  const string code="PED:UAE-MOE-MATH:L1:COMMON:03:08",family="uae.g1.u03.l08.subtract_4_5";
  Assert.True(LessonPracticeContractRegistry.TryResolve(code,out var contract));
  Assert.NotNull(contract);
  Assert.Equal(new[]{family},contract.AllowedQuestionFamilies);
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
  var items=new ExactSkillContractQuestionEngine().Generate("stage18",code,new[]{family},ExactSkillQuestionDifficulty.Standard,8,2708,Array.Empty<string>());
  Assert.NotEmpty(items);
  Assert.All(items,q=>{
   Assert.InRange(q.Parameters["whole"],4,5);
   Assert.InRange(q.Parameters["removed"],0,q.Parameters["whole"]);
   Assert.Equal((q.Parameters["whole"]-q.Parameters["removed"]).ToString(),q.CorrectAnswer);
   Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
   Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"99"));
  });
 }
}
