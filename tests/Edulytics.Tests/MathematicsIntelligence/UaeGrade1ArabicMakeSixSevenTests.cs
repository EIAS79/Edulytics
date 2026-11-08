using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// WRITTEN_NOT_RUN during Stage A–B–C.
public sealed class UaeGrade1ArabicMakeSixSevenTests
{
 [Fact]
 public void MakeSixSeven_OnlyAcceptsCorrectMissingPart()
 {
  const string code="PED:UAE-MOE-MATH:L1:COMMON:02:08";
  const string family="uae.g1.u02.l08.make_six_seven";
  Assert.True(LessonPracticeContractRegistry.TryResolve(code,out var contract));
  Assert.NotNull(contract);
  Assert.Equal(new[]{family},contract.AllowedQuestionFamilies);
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
  var items=new ExactSkillContractQuestionEngine().Generate("stage18",code,new[]{family},
    ExactSkillQuestionDifficulty.Standard,8,148,Array.Empty<string>());
  Assert.NotEmpty(items);
  Assert.All(items,q=>{
   Assert.InRange(q.Parameters["total"],6,7);
   Assert.Equal(q.Parameters["total"],q.Parameters["left"]+q.Parameters["right"]);
   Assert.Equal(q.Parameters["right"].ToString(),q.CorrectAnswer);
   Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
   Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"99"));
  });
 }
}
