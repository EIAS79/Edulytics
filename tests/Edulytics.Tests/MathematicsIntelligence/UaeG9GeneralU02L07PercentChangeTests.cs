using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade 9 General U02L07 Stage A-C tests; only run during Stage D.</summary>
public sealed class UaeG9GeneralU02L07PercentChangeTests
{
    private const string Lesson = "PED:UAE-MOE-MATH:L9:GENERAL:02:07:PERCENT-CHANGE";

    [Theory]
    [InlineData("percentages.core.increase_decrease", 100, 20, 1, "120", "80")]
    [InlineData("percentages.core.increase_decrease", 100, 20, -1, "80", "120")]
    [InlineData("percentages.core.of_quantity", 200, 15, 1, "30", "15")]
    public void ExistingExactPercentVerifierAcceptsCorrectAndRejectsWrong(
        string family, int quantity, int percent, int direction,
        string correct, string incorrect)
    {
        var p = new Dictionary<string, int>
        {
            ["quantity"] = quantity, ["percent"] = percent,
            ["direction"] = direction
        };
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family, p, correct));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family, p, incorrect));
    }

    [Fact]
    public void ScopedArabicGenerationUsesExistingPercentFamilies()
    {
        var engine = new ExactSkillContractQuestionEngine();
        var families = new[]
        {
            "percentages.core.increase_decrease",
            "percentages.core.of_quantity"
        };
        var questions = engine.Generate("UAE-G9-GENERAL", Lesson, families,
            ExactSkillQuestionDifficulty.Standard, 2, 20261120,
            Array.Empty<string>());
        Assert.Equal(2, questions.Count);
        foreach (var q in questions)
        {
            Assert.Contains(q.Family, families);
            Assert.True(q.Prompt.Any(c => c >= '\u0600' && c <= '\u06FF'));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                q.Family, q.Parameters, q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                q.Family, q.Parameters, "-999"));
        }
    }

    [Fact]
    public void ReuseDoesNotTranslateOtherCurricula()
    {
        var engine = new ExactSkillContractQuestionEngine();
        var q = Assert.Single(engine.Generate("CAMBRIDGE", "CAMBRIDGE-L9-PERCENT",
            new[] { "percentages.core.increase_decrease" },
            ExactSkillQuestionDifficulty.Standard, 1, 23,
            Array.Empty<string>()));
        Assert.True(q.Prompt.StartsWith("Increase ", StringComparison.Ordinal) ||
                    q.Prompt.StartsWith("Decrease ", StringComparison.Ordinal));
    }
}
