using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// WRITTEN_NOT_RUN — prepared in A-B-C, execute in Stage D.
public sealed class UaeGrade1ArabicNearDoublesAdditionTests
{
    [Fact]
    public void NearDoubles_OnlyOneMoreOrLessThanDouble()
    {
        const string code = "PED:UAE-MOE-MATH:L1:COMMON:04:05";
        const string family = "uae.g1.u04.l05.near_doubles_add";
        Assert.True(LessonPracticeContractRegistry.TryResolve(code, out var contract));
        Assert.NotNull(contract);
        Assert.Equal(new[] { family }, contract.AllowedQuestionFamilies);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var items = new ExactSkillContractQuestionEngine().Generate(
            "stage18", code, new[] { family }, ExactSkillQuestionDifficulty.Standard,
            8, 38005, Array.Empty<string>());
        Assert.NotEmpty(items);
        Assert.All(items, q =>
        {
            Assert.InRange(q.Parameters["base"], 2, 9);
            Assert.True(q.Parameters["delta"] is -1 or 1);
            Assert.Equal((2 * q.Parameters["base"] + q.Parameters["delta"]).ToString(), q.CorrectAnswer);
            Assert.True(ExactSkillContractQuestionEngine.Verify(family, q.Parameters, q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family, q.Parameters, "99"));
        });
    }
}
