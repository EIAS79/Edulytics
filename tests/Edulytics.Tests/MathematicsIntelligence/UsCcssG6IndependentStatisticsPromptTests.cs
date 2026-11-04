using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG6IndependentStatisticsPromptTests
{
    private const string Games = "130,150,120,170,130,120,160,160,190,140";
    private const string Han = "100,100,100,100,95,100,90,100,0";
    private const string Priya = "1,8,6,2,4";

    [Theory]
    [InlineData("usccss.g6.u08.l13.median_sorted_samples")]
    [InlineData("usccss.g6.u08.l14.mean_median_outlier_changes")]
    [InlineData("usccss.g6.u08.l14.mean_median_choice_reasoning")]
    [InlineData("usccss.g6.u08.l15.quartiles_five_number_summary")]
    public void IndependentlySampledStatisticsQuestionsCarryTheirOwnRequiredData(string family)
    {
        var engine = new ExactSkillContractQuestionEngine();
        var symmetric = 0;
        var dataDependent = 0;
        var observedVariants = new HashSet<int>();

        for (var seed = 1; seed <= 700; seed++)
        {
            var question = Assert.Single(engine.Generate(
                "grade6-statistical-source-self-contained", family, [family],
                ExactSkillQuestionDifficulty.Standard, 1, seed + 20261104, []));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                question.Family, question.Parameters, question.CorrectAnswer));
            Assert.True(question.Parameters.TryGetValue("variant", out var variant));
            observedVariants.Add(variant);

            if (question.Prompt.Contains("Same scores", StringComparison.OrdinalIgnoreCase))
                Assert.Fail("Statistical Practice prompt depends on a preceding random question.");

            if (question.Prompt.Contains("Game scores", StringComparison.OrdinalIgnoreCase))
            {
                Assert.Contains(Games, question.Prompt, StringComparison.Ordinal);
                dataDependent++;
            }

            if (question.Prompt.Contains("Han homework", StringComparison.OrdinalIgnoreCase) ||
                question.Prompt.Contains("zero homework outlier", StringComparison.OrdinalIgnoreCase))
            {
                Assert.Contains(Han, question.Prompt, StringComparison.Ordinal);
                dataDependent++;
            }

            if (question.Prompt.Contains("replace8 with18", StringComparison.OrdinalIgnoreCase))
            {
                Assert.Contains(Priya, question.Prompt, StringComparison.Ordinal);
                dataDependent++;
            }

            if (question.Prompt.Contains("EXACT symmetry", StringComparison.Ordinal))
            {
                Assert.Equal("1", question.CorrectAnswer);
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family, question.Parameters, "0"));
                symmetric++;
            }
        }

        Assert.True(observedVariants.Count >= 15,
            $"Statistics source variants not adequately sampled: {observedVariants.Count}");
        if (family.Contains(".l14.", StringComparison.Ordinal))
            Assert.True(dataDependent > 0);
        if (family.EndsWith("mean_median_choice_reasoning", StringComparison.Ordinal))
            Assert.True(symmetric > 0);
    }
}
