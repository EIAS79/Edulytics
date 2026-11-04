using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG6ScaledRatioSessionTests
{
    [Theory]
    [InlineData("usccss.g6.u02.l02.labeled_two_row_ratios", 2)]
    [InlineData("usccss.g6.u02.l02.three_ingredient_ratio_cards", 3)]
    public void ExtendedDiagramSessionsKeepExactOrderedCountRatios(string family, int rows)
    {
        var engine = new ExactSkillContractQuestionEngine();
        var generated = engine.Generate(
            "grade6-ordered-ratio-extended", family, [family],
            ExactSkillQuestionDifficulty.Standard, 10, 20261104, []);

        Assert.Equal(10, generated.Count);
        Assert.Equal(10, generated.Select(x => x.ExposureFingerprint)
            .Distinct(StringComparer.Ordinal).Count());

        foreach (var question in generated)
        {
            Assert.Equal(family, question.Family);
            Assert.Equal(rows, question.CorrectAnswer.Split(':').Length);
            Assert.Contains("identical copies", question.Prompt, StringComparison.Ordinal);
            Assert.Equal(1, question.Parameters["g6ScaledRatio"]);
            Assert.InRange(question.Parameters["copies"], 2, 19);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family, question.Parameters, question.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                family, question.Parameters, "0:0"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                family, question.Parameters, "1:0:0"));
        }

        var original = Assert.Single(engine.Generate(
            "grade6-ordered-ratio-original", family, [family],
            ExactSkillQuestionDifficulty.Standard, 1, 3333, []));
        Assert.False(original.Parameters.ContainsKey("g6ScaledRatio"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            family, original.Parameters, original.CorrectAnswer));
    }
}
