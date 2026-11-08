using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// WRITTEN_NOT_RUN in Stage A–B–C.
public sealed class UaeGrade1ArabicActOutProblemsTests
{
    [Fact]
    public void ActOut_GeneratesValidGroupsOrMissingPart()
    {
        const string code = "PED:UAE-MOE-MATH:L1:COMMON:04:06";
        const string family = "uae.g1.u04.l06.act_out_problem_solving";
        Assert.True(LessonPracticeContractRegistry.TryResolve(code, out var contract));
        Assert.NotNull(contract);
        Assert.Equal(new[] { family }, contract.AllowedQuestionFamilies);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var items = new ExactSkillContractQuestionEngine().Generate(
            "stage18", code, new[] { family }, ExactSkillQuestionDifficulty.Standard,
            8, 39006, Array.Empty<string>());
        Assert.NotEmpty(items);
        Assert.All(items, q =>
        {
            Assert.Equal(q.Parameters["a"] + q.Parameters["b"] + q.Parameters["c"], q.Parameters["total"]);
            Assert.InRange(q.Parameters["total"], 2, 20);
            Assert.Equal((q.Parameters["mode"] == 2 ? q.Parameters["b"] : q.Parameters["total"]).ToString(), q.CorrectAnswer);
            Assert.True(ExactSkillContractQuestionEngine.Verify(family, q.Parameters, q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family, q.Parameters, "99"));
        });
    }
}
