using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Grade 9 General U02L06 source-skill tests authored during A-B-C.
/// Execute only during Stage D.
/// </summary>
public sealed class UaeG9GeneralU02L06RatioProportionTests
{
    private const string Lesson =
        "PED:UAE-MOE-MATH:L9:GENERAL:02:06:RATIOS-AND-PROPORTIONS";

    [Fact]
    public void EquivalentRatioUsesCrossMultiplicationAndIndependentVerifier()
    {
        const string family = "ratio.unit_rate.equivalent_ratio";
        var parameters = new Dictionary<string, int>
        {
            ["baseFirst"] = 8,
            ["baseSecond"] = 2,
            ["targetSecond"] = 6
        };
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family, parameters, "24"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family, parameters, "20"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family, parameters, "0"));
    }

    [Fact]
    public void Grade9ScopeLocalizesOnlySelectedExistingRatioFamilies()
    {
        var engine = new ExactSkillContractQuestionEngine();
        var families = new[]
        {
            "ratio.unit_rate.equivalent_ratio",
            "ratio.unit_rate.direct",
            "ratio.unit_rate.divide_total",
            "uae.g9.general.u02.l06.two_sided_proportion"
        };
        var questions = engine.Generate(
            "UAE-G9-GENERAL", Lesson, families,
            ExactSkillQuestionDifficulty.Standard, 4, 20261120,
            Array.Empty<string>());
        Assert.Equal(4, questions.Count);
        foreach (var question in questions)
        {
            Assert.Contains(question.Family, families);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                question.Family, question.Parameters, question.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                question.Family, question.Parameters, "-999"));
            Assert.True(question.Prompt.Any(c => c >= '\u0600' && c <= '\u06FF'));
        }
    }

    [Fact]
    public void TwoSidedProportionAcceptsOnlyValidRootsAndRejectsZeroDenominators()
    {
        const string family = "uae.g9.general.u02.l06.two_sided_proportion";
        var p = new Dictionary<string, int>
        {
            ["a"] = 2, ["b"] = 1, ["c"] = 5,
            ["d"] = 1, ["e"] = 4, ["f"] = 3
        };
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family, p, "17"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family, p, "١٧"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family, p, "16"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,
            new Dictionary<string, int>(p) { ["c"] = 0 }, "17"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,
            new Dictionary<string, int>(p) { ["f"] = 0 }, "17"));
    }

    [Fact]
    public void SharedRatioPromptsRemainUnmodifiedOutsideGrade9Scope()
    {
        var engine = new ExactSkillContractQuestionEngine();
        var question = Assert.Single(engine.Generate(
            "CAMBRIDGE-L9", "PED:CAMBRIDGE-INTL-MATH:L9:SHARED:01:06:RATES-AND-UNIT-RATES",
            new[] { "ratio.unit_rate.equivalent_ratio" },
            ExactSkillQuestionDifficulty.Standard, 1, 91,
            Array.Empty<string>()));
        Assert.Contains("minutes", question.Prompt);
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            question.Family, question.Parameters, question.CorrectAnswer));
    }
}
