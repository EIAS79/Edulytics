using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// WRITTEN_NOT_RUN — Stage D test prepared during A–B–C, not executed.
public sealed class UaeGrade1ArabicTrueFalseSubtractionTests
{
    [Fact]
    public void SubtractionEquality_ClassifiesTrueAndFalseCorrectly()
    {
        const string code = "PED:UAE-MOE-MATH:L1:COMMON:03:14";
        const string family = "uae.g1.u03.l14.true_false_subtraction";
        Assert.True(LessonPracticeContractRegistry.TryResolve(code, out var contract));
        Assert.NotNull(contract);
        Assert.Equal(new[] { family }, contract.AllowedQuestionFamilies);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var items = new ExactSkillContractQuestionEngine().Generate(
            "stage18", code, new[] { family }, ExactSkillQuestionDifficulty.Standard,
            12, 330014, Array.Empty<string>());
        Assert.NotEmpty(items);
        Assert.All(items, item =>
        {
            Assert.True(item.CorrectAnswer is "صحيحة" or "خطأ");
            Assert.InRange(item.Parameters["whole"], 1, 10);
            Assert.InRange(item.Parameters["removed"], 0, item.Parameters["whole"]);
            Assert.Equal(
                item.Parameters["whole"] - item.Parameters["removed"] == item.Parameters["shown"] ? "صحيحة" : "خطأ",
                item.CorrectAnswer);
            Assert.True(ExactSkillContractQuestionEngine.Verify(family, item.Parameters, item.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family, item.Parameters, "غير معروف"));
        });
    }
}
