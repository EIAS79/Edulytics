using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// WRITTEN_NOT_RUN — Stage D.
public sealed class UaeGrade1ArabicCompareGroupsTests
{
 [Fact]
 public void ComparingGroups_UsesExactDifference_NotSum()
 {
  const string code="PED:UAE-MOE-MATH:L1:COMMON:03:07";
  const string family="uae.g1.u03.l07.compare_groups";
  Assert.True(LessonPracticeContractRegistry.TryResolve(code,out var contract));
  Assert.NotNull(contract);
  Assert.Equal(new[]{family},contract.AllowedQuestionFamilies);
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
  var items=new ExactSkillContractQuestionEngine().Generate("stage18",code,new[]{family},
    ExactSkillQuestionDifficulty.Standard,8,126,Array.Empty<string>());
  Assert.NotEmpty(items);
  Assert.All(items,q=>{
   Assert.Contains("قارن",q.Prompt);
   Assert.Contains("أكثر",q.Prompt);
   Assert.InRange(q.Parameters["whole"],2,10);
   Assert.InRange(q.Parameters["removed"],1,q.Parameters["whole"]-1);
   Assert.Equal((q.Parameters["whole"]-q.Parameters["removed"]).ToString(),q.CorrectAnswer);
   Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
   Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"99"));
  });
 }
}
