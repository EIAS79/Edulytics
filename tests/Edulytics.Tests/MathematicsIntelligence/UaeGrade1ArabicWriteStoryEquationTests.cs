using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// WRITTEN_NOT_RUN — Stage D targeted test prepared during A–B–C.
public sealed class UaeGrade1ArabicWriteStoryEquationTests
{
 [Fact]
 public void Stories_RequireTwoGroupEquationAndValidSum()
 {
  const string code="PED:UAE-MOE-MATH:L1:COMMON:02:06";
  const string family="uae.g1.u02.l06.write_story_equation";
  Assert.True(LessonPracticeContractRegistry.TryResolve(code,out var contract));
  Assert.NotNull(contract);
  Assert.Equal(new[]{family},contract.AllowedQuestionFamilies);
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
  var items=new ExactSkillContractQuestionEngine().Generate("stage18",code,new[]{family},
    ExactSkillQuestionDifficulty.Standard,6,146,Array.Empty<string>());
  Assert.NotEmpty(items);
  Assert.All(items,q=>{
   Assert.Contains("أفهم",q.Prompt);
   Assert.Contains("أتحقق",q.Prompt);
   Assert.Contains("+",q.Prompt);
   Assert.InRange(q.Parameters["left"]+q.Parameters["right"],2,10);
   Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
   Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"99"));
  });
 }
}
