using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// WRITTEN_NOT_RUN — Stage D targeted test authored during A-B-C.
public sealed class UaeGrade1ArabicAddZeroTests
{
 [Fact]
 public void AddZero_ExactFamily_KeepsOtherOperand()
 {
  const string code="PED:UAE-MOE-MATH:L1:COMMON:02:04";
  const string family="uae.g1.u02.l04.add_zero";
  Assert.True(LessonPracticeContractRegistry.TryResolve(code,out var contract));
  Assert.NotNull(contract);
  Assert.Equal(new[]{family},contract.AllowedQuestionFamilies);
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
  var items=new ExactSkillContractQuestionEngine().Generate("stage18",code,new[]{family},
    ExactSkillQuestionDifficulty.Standard,12,144,Array.Empty<string>());
  Assert.NotEmpty(items);
  Assert.All(items,q=>{
    Assert.Contains(" + ",q.Prompt);
    Assert.Contains(" = ",q.Prompt);
    Assert.True(q.Parameters["left"] == 0 || q.Parameters["right"] == 0);
    Assert.InRange(q.Parameters["left"] + q.Parameters["right"],1,9);
    Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"99"));
  });
 }
}
