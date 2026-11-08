using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// WRITTEN_NOT_RUN — Stage D test prepared during A-B-C.
public sealed class UaeGrade1ArabicDrawDiagramTests
{
 [Fact]
 public void DrawingStrategy_ReusesExactTakeAwayCounterModel()
 {
  const string code="PED:UAE-MOE-MATH:L1:COMMON:03:06";
  const string family="uae.g1.u03.l01.subtraction_stories";
  Assert.True(LessonPracticeContractRegistry.TryResolve(code,out var contract));
  Assert.NotNull(contract);
  Assert.Equal(new[]{family},contract.AllowedQuestionFamilies);
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
  var items=new ExactSkillContractQuestionEngine().Generate("stage18",code,new[]{family},
    ExactSkillQuestionDifficulty.Standard,8,205,Array.Empty<string>());
  Assert.NotEmpty(items);
  Assert.All(items,q=>{
   Assert.Contains("قبل:",q.Prompt);
   Assert.Contains("بعد:",q.Prompt);
   Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
   Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"99"));
  });
 }
}
