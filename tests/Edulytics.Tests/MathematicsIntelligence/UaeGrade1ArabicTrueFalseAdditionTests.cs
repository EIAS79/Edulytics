using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// WRITTEN_NOT_RUN during A-B-C, execute in Stage D.
public sealed class UaeGrade1ArabicTrueFalseAdditionTests
{
 [Fact]
 public void AdditionEquality_ArabicTrueFalseAndIndependentVerification()
 {
  const string code="PED:UAE-MOE-MATH:L1:COMMON:02:13";
  const string family="uae.g1.u02.l13.true_false_addition";
  Assert.True(LessonPracticeContractRegistry.TryResolve(code,out var contract));
  Assert.NotNull(contract);
  Assert.Equal(new[]{family},contract.AllowedQuestionFamilies);
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
  var items=new ExactSkillContractQuestionEngine().Generate("stage18",code,new[]{family},
    ExactSkillQuestionDifficulty.Standard,8,153,Array.Empty<string>());
  Assert.NotEmpty(items);
  Assert.All(items,q=>{
   Assert.True(q.CorrectAnswer is "صحيحة" or "خطأ");
   Assert.Contains("=",q.Prompt);
   Assert.InRange(q.Parameters["left"]+q.Parameters["right"],0,10);
   Assert.Equal(q.Parameters["left"]+q.Parameters["right"]==q.Parameters["shown"]?"صحيحة":"خطأ",q.CorrectAnswer);
   Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
   Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"غير معروف"));
  });
 }
}
