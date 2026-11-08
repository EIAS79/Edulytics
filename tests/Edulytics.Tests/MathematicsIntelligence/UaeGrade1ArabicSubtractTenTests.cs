using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Stage A–B–C: WRITTEN_NOT_RUN
public sealed class UaeGrade1ArabicSubtractTenTests
{
 [Fact]
 public void SubtractFromTen_UsesFixedWholeAndExactDifference()
 {
  const string code="PED:UAE-MOE-MATH:L1:COMMON:03:12";
  const string family="uae.g1.u03.l12.subtract_10";
  Assert.True(LessonPracticeContractRegistry.TryResolve(code,out var contract));
  Assert.NotNull(contract);
  Assert.Equal(new[]{family},contract.AllowedQuestionFamilies);
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
  var questions=new ExactSkillContractQuestionEngine().Generate("stage18",code,new[]{family},ExactSkillQuestionDifficulty.Standard,12,3110,Array.Empty<string>());
  Assert.NotEmpty(questions);
  Assert.All(questions,q=>{
   Assert.Equal(10,q.Parameters["whole"]);
   Assert.InRange(q.Parameters["removed"],0,10);
   Assert.Equal((10-q.Parameters["removed"]).ToString(),q.CorrectAnswer);
   Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
   Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"99"));
  });
 }
}
