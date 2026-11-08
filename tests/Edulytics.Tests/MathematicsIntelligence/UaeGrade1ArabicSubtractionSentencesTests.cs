using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// WRITTEN_NOT_RUN — Stage D test created in Stage A–B–C.
public sealed class UaeGrade1ArabicSubtractionSentencesTests
{
 [Fact]
 public void SubtractionEquations_UseMinusEqualAndExactDifference()
 {
  const string code="PED:UAE-MOE-MATH:L1:COMMON:03:03";
  const string family="uae.g1.u03.l03.subtraction_number_sentences";
  Assert.True(LessonPracticeContractRegistry.TryResolve(code,out var contract));
  Assert.NotNull(contract);
  Assert.Equal(new[]{family},contract.AllowedQuestionFamilies);
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
  var items=new ExactSkillContractQuestionEngine().Generate("stage18",code,new[]{family},
    ExactSkillQuestionDifficulty.Standard,8,202,Array.Empty<string>());
  Assert.NotEmpty(items);
  Assert.All(items,q=>{
   Assert.Contains("−",q.Prompt);
   Assert.Contains("=",q.Prompt);
   Assert.Equal((q.Parameters["whole"]-q.Parameters["removed"]).ToString(),q.CorrectAnswer);
   Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
   Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"99"));
  });
 }
}
