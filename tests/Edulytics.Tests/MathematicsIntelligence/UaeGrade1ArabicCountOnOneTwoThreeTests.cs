using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// WRITTEN_NOT_RUN during Stage A–B–C.
public sealed class UaeGrade1ArabicCountOnOneTwoThreeTests
{
    [Fact]
    public void CountOn_OnlyGeneratesOneToThreeAdditionalSteps()
    {
        const string code = "PED:UAE-MOE-MATH:L1:COMMON:04:01";
        const string family = "uae.g1.u04.l01.count_on_1_2_3";
        Assert.True(LessonPracticeContractRegistry.TryResolve(code, out var contract));
        Assert.NotNull(contract);
        Assert.Equal(new[] { family }, contract.AllowedQuestionFamilies);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var items = new ExactSkillContractQuestionEngine().Generate(
            "stage18", code, new[] { family }, ExactSkillQuestionDifficulty.Standard,
            10, 34001, Array.Empty<string>());
        Assert.NotEmpty(items);
        Assert.All(items, q =>
        {
            Assert.InRange(q.Parameters["step"], 1, 3);
            Assert.InRange(q.Parameters["start"] + q.Parameters["step"], 1, 20);
            Assert.Equal((q.Parameters["start"] + q.Parameters["step"]).ToString(), q.CorrectAnswer);
            Assert.True(ExactSkillContractQuestionEngine.Verify(family, q.Parameters, q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family, q.Parameters, "99"));
        });
    }
}
