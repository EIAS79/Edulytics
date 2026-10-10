using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCommonCoreExactStandardFamilyTests
{
    private static readonly string[] NarrowFamilies =
    [
        "usccss.geometry.first_quadrant_point",
        "usccss.fractions.unit_divide_whole",
        "usccss.fractions.whole_divide_unit",
        "usccss.algebra.expression_words",
        "usccss.algebra.expression_coefficient",
        "usccss.algebra.expression_power_value",
        "usccss.number.signed_rational_multiply",
        "usccss.number.signed_rational_divide",
        "usccss.number.rational_terminating_decimal"
    ];

    [Fact]
    public void AllNarrowGradeSpecificFamiliesGenerateAndIndependentlyVerifyManySeeds()
    {
        var engine = new ExactSkillContractQuestionEngine();
        var successes = 0;
        foreach (var family in NarrowFamilies)
        {
            foreach (var difficulty in new[]
            {
                ExactSkillQuestionDifficulty.Standard,
                ExactSkillQuestionDifficulty.Stretch,
                ExactSkillQuestionDifficulty.Challenge
            })
            {
                for (var i = 0; i < 12; i++)
                {
                    var question = Assert.Single(engine.Generate(
                        "ccss-grade-specific-audit",
                        "PED:US-CCSS-MATH:G5:U01:L01",
                        [family],
                        difficulty,
                        1,
                        6300000 + successes,
                        []));
                    Assert.Equal(family, question.Family);
                    Assert.True(ExactSkillContractQuestionEngine.Verify(
                        question.Family, question.Parameters, question.CorrectAnswer));
                    if (family == "usccss.geometry.first_quadrant_point")
                    {
                        Assert.True(question.Parameters["x"] >= 0);
                        Assert.True(question.Parameters["y"] >= 0);
                        Assert.DoesNotContain("midpoint", question.Prompt, StringComparison.OrdinalIgnoreCase);
                    }
                    successes++;
                }
            }
        }
        Assert.Equal(9 * 3 * 12, successes);
    }
}