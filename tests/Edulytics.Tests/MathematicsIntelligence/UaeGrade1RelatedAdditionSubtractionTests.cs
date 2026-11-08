using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// WRITTEN_NOT_RUN; Stage D executes tests.
public sealed class UaeGrade1RelatedAdditionSubtractionTests
{
 [Fact]
 public void AdditionFact_RevealsOneRelatedSubtractionResult()
 {
  const string code="PED:UAE-MOE-MATH:L1:COMMON:03:13";
  const string family="uae.g1.u03.l13.related_add_subtract";
  Assert.True(LessonPracticeContractRegistry.TryResolve(code,out var contract));
  Assert.NotNull(contract);
  Assert.Equal(new[]{family},contract.AllowedQuestionFamilies);
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
  var items=new ExactSkillContractQuestionEngine().Generate("stage18",code,new[]{family},ExactSkillQuestionDifficulty.Standard,12,3213,Array.Empty<string>());
  Assert.NotEmpty(items);
  Assert.All(items,q=>{
   Assert.Contains(" + ",q.Prompt);
   Assert.Contains(" − ",q.Prompt);
   Assert.InRange(q.Parameters["left"]+q.Parameters["right"],2,10);
   Assert.Equal((q.Parameters["removeLeft"]==1?q.Parameters["right"]:q.Parameters["left"]).ToString(),q.CorrectAnswer);
   Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
   Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"99"));
  });
 }
}
