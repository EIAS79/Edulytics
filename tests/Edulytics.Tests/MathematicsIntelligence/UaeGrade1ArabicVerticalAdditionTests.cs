using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// WRITTEN_NOT_RUN during Stage A-B-C.
public sealed class UaeGrade1ArabicVerticalAdditionTests
{
 [Fact]
 public void VerticalAddition_SumsTwoGroupsCorrectly()
 {
  const string code="PED:UAE-MOE-MATH:L1:COMMON:02:05";
  const string family="uae.g1.u02.l05.vertical_addition";
  Assert.True(LessonPracticeContractRegistry.TryResolve(code,out var contract));
  Assert.NotNull(contract);
  Assert.Equal(new[]{family},contract.AllowedQuestionFamilies);
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
  var items=new ExactSkillContractQuestionEngine().Generate("stage18",code,new[]{family},
    ExactSkillQuestionDifficulty.Standard,8,145,Array.Empty<string>());
  Assert.NotEmpty(items);
  Assert.All(items,q=>{
   Assert.Contains("+",q.Prompt);
   Assert.Contains("────",q.Prompt);
   Assert.InRange(q.Parameters["left"]+q.Parameters["right"],2,10);
   Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
   Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"99"));
  });
 }
}
