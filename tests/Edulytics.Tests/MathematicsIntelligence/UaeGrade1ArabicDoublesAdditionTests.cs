using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// WRITTEN_NOT_RUN — Stage D will execute.
public sealed class UaeGrade1ArabicDoublesAdditionTests
{
    [Fact]
    public void Doubles_RequireTwoEqualGroupsAndExactSum()
    {
        const string code = "PED:UAE-MOE-MATH:L1:COMMON:04:04";
        const string family = "uae.g1.u04.l04.doubles_add";
        Assert.True(LessonPracticeContractRegistry.TryResolve(code, out var contract));
        Assert.NotNull(contract);
        Assert.Equal(new[] { family }, contract.AllowedQuestionFamilies);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var items = new ExactSkillContractQuestionEngine().Generate(
            "stage18", code, new[] { family }, ExactSkillQuestionDifficulty.Standard,
            8, 37004, Array.Empty<string>());
        Assert.NotEmpty(items);
        Assert.All(items, q =>
        {
            Assert.InRange(q.Parameters["equalAddend"], 1, 10);
            Assert.Equal((2 * q.Parameters["equalAddend"]).ToString(), q.CorrectAnswer);
            Assert.Contains("متساويتين", q.Prompt);
            Assert.True(ExactSkillContractQuestionEngine.Verify(family, q.Parameters, q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family, q.Parameters, "99"));
        });
    }
}
