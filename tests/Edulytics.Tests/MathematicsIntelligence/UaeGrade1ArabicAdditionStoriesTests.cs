using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// WRITTEN_NOT_RUN — UAE Grade 1 Stage A-B-C
public sealed class UaeGrade1ArabicAdditionStoriesTests
{
 [Fact]
 public void GradeOneAdditionStories_RouteToBoundedTwoPartExactArithmetic()
 {
    const string code = "PED:UAE-MOE-MATH:L1:COMMON:02:01";
    const string family = "uae.g1.u02.l01.addition_stories";
    Assert.True(LessonPracticeContractRegistry.TryResolve(code, out var contract));
    Assert.NotNull(contract);
    Assert.Equal(new[] { family }, contract.AllowedQuestionFamilies);
    Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
    var questions = new ExactSkillContractQuestionEngine().Generate("stage18", code,
        new[] {family}, ExactSkillQuestionDifficulty.Standard, 4, 42, Array.Empty<string>());
    Assert.All(questions, q =>
    {
        var left=q.Parameters["left"]; var right=q.Parameters["right"];
        Assert.InRange(left,1,7); Assert.InRange(right,1,4);
        Assert.InRange(left+right,2,10);
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,
            string.Concat((left+right).ToString().Select(c=>(char)(c-'0'+'٠')))));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"99"));
    });
 }
}
