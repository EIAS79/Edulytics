using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// WRITTEN_NOT_RUN in A-B-C.
public sealed class UaeGrade1ArabicMakeEightTests
{
 [Fact]
 public void Eight_ComposedOfTwoExactParts()
 {
  const string code="PED:UAE-MOE-MATH:L1:COMMON:02:09";
  const string family="uae.g1.u02.l09.make_eight";
  Assert.True(LessonPracticeContractRegistry.TryResolve(code,out var contract));
  Assert.NotNull(contract);
  Assert.Equal(new[]{family},contract.AllowedQuestionFamilies);
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
  var items=new ExactSkillContractQuestionEngine().Generate("stage18",code,new[]{family},
    ExactSkillQuestionDifficulty.Standard,8,149,Array.Empty<string>());
  Assert.NotEmpty(items);
  Assert.All(items,q=>{
   Assert.Equal(8,q.Parameters["total"]);
   Assert.Equal(8,q.Parameters["left"]+q.Parameters["right"]);
   Assert.Equal(q.Parameters["right"].ToString(),q.CorrectAnswer);
   Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
   Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"99"));
  });
 }
}
