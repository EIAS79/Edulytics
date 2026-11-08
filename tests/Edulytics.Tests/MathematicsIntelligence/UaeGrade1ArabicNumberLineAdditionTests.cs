using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// WRITTEN_NOT_RUN during Stage A–B–C.
public sealed class UaeGrade1ArabicNumberLineAdditionTests
{
    [Fact]
    public void AddOnNumberLine_StaysWithinZeroToTwelve()
    {
        const string code = "PED:UAE-MOE-MATH:L1:COMMON:04:03";
        const string family = "uae.g1.u04.l03.add_on_number_line";
        Assert.True(LessonPracticeContractRegistry.TryResolve(code, out var contract));
        Assert.NotNull(contract);
        Assert.Equal(new[] { family }, contract.AllowedQuestionFamilies);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var items = new ExactSkillContractQuestionEngine().Generate(
            "stage18", code, new[] { family }, ExactSkillQuestionDifficulty.Standard,
            8, 36003, Array.Empty<string>());
        Assert.NotEmpty(items);
        Assert.All(items, q =>
        {
            Assert.InRange(q.Parameters["hops"], 1, 4);
            Assert.InRange(q.Parameters["start"] + q.Parameters["hops"], 1, 12);
            Assert.Equal((q.Parameters["start"] + q.Parameters["hops"]).ToString(), q.CorrectAnswer);
            Assert.Contains("يمينًا", q.Prompt);
            Assert.True(ExactSkillContractQuestionEngine.Verify(family, q.Parameters, q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family, q.Parameters, "99"));
        });
    }
}
