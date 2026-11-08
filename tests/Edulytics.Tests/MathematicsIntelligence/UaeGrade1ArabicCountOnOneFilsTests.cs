using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// WRITTEN_NOT_RUN during Stage A–B–C.
public sealed class UaeGrade1ArabicCountOnOneFilsTests
{
    [Fact]
    public void OneFilsCoins_CountOnExactSum()
    {
        const string code = "PED:UAE-MOE-MATH:L1:COMMON:04:02";
        const string family = "uae.g1.u04.l02.count_on_fils";
        Assert.True(LessonPracticeContractRegistry.TryResolve(code, out var contract));
        Assert.NotNull(contract);
        Assert.Equal(new[] { family }, contract.AllowedQuestionFamilies);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var items = new ExactSkillContractQuestionEngine().Generate(
            "stage18", code, new[] { family }, ExactSkillQuestionDifficulty.Standard,
            10, 35002, Array.Empty<string>());
        Assert.NotEmpty(items);
        Assert.All(items, q =>
        {
            Assert.InRange(q.Parameters["start"], 4, 10);
            Assert.InRange(q.Parameters["added"], 1, 3);
            Assert.Equal((q.Parameters["start"] + q.Parameters["added"]).ToString(), q.CorrectAnswer);
            Assert.Contains("فلس", q.Prompt);
            Assert.True(ExactSkillContractQuestionEngine.Verify(family, q.Parameters, q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family, q.Parameters, "99"));
        });
    }
}
