using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// WRITTEN_NOT_RUN Stage A–B–C
public sealed class UaeGrade1ArabicModelAdditionTests
{
 [Fact]
 public void Grade1PartWholeModel_HasBoundedExactNumericContract()
 {
    const string code = "PED:UAE-MOE-MATH:L1:COMMON:02:02";
    const string family = "uae.g1.u02.l02.model_addition";
    Assert.True(LessonPracticeContractRegistry.TryResolve(code, out var contract));
    Assert.NotNull(contract);
    Assert.Equal(new[] { family }, contract.AllowedQuestionFamilies);
    Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
    var q = new ExactSkillContractQuestionEngine().Generate("stage18",code,new[] {family},
       ExactSkillQuestionDifficulty.Standard,4,53,Array.Empty<string>());
    Assert.All(q,item=>
    {
       Assert.Contains("الكل",item.Prompt);
       Assert.Equal(family,item.Family);
       Assert.InRange(item.Parameters["left"]+item.Parameters["right"],2,10);
       Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,item.CorrectAnswer));
       Assert.False(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,"100"));
    });
 }
}
