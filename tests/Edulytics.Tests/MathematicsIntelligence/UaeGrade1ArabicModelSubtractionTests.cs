using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// WRITTEN_NOT_RUN — for Stage D.
public sealed class UaeGrade1ArabicModelSubtractionTests
{
 [Fact]
 public void PartPartWhole_ExactRemainingPart()
 {
  const string code="PED:UAE-MOE-MATH:L1:COMMON:03:02";
  const string family="uae.g1.u03.l02.model_subtraction";
  Assert.True(LessonPracticeContractRegistry.TryResolve(code,out var contract));
  Assert.NotNull(contract);
  Assert.Equal(new[]{family},contract.AllowedQuestionFamilies);
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
  var items=new ExactSkillContractQuestionEngine().Generate("stage18",code,new[]{family},
   ExactSkillQuestionDifficulty.Standard,8,201,Array.Empty<string>());
  Assert.NotEmpty(items);
  Assert.All(items,q=>{
   Assert.Contains("الكل",q.Prompt);
   Assert.InRange(q.Parameters["whole"],2,10);
   Assert.Equal((q.Parameters["whole"]-q.Parameters["removed"]).ToString(),q.CorrectAnswer);
   Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
   Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"99"));
  });
 }
}
