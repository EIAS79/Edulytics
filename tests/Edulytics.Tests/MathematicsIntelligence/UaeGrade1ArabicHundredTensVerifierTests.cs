using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Reproduces the full-catalogue Practice V2 blocker that occurred when a
/// valid Grade 1 subtraction removed all ten tens from 100.
/// </summary>
public sealed class UaeGrade1ArabicHundredTensVerifierTests
{
    [Theory]
    [InlineData("uae.g1.u07.l06.subtract_tens")]
    [InlineData("uae.g1.u07.l07.count_back_tens")]
    public void RemovingAllTenTens_FromOneHundred_IsExactAndGradedCorrectly(string family)
    {
        var parameters = new Dictionary<string, int>
        {
            ["a"] = 100, ["b"] = 100, ["mode"] = 0, ["result"] = 0
        };
        Assert.True(ExactSkillContractQuestionEngine.Verify(family, parameters, "0"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family, parameters, "٠"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family, parameters, "10"));

        var invalid = new Dictionary<string, int>(parameters)
        {
            ["b"] = 110,
            ["result"] = -10
        };
        Assert.False(ExactSkillContractQuestionEngine.Verify(family, invalid, "-10"));
    }

    [Theory]
    [InlineData("uae.g1.u07.l06.subtract_tens")]
    [InlineData("uae.g1.u07.l07.count_back_tens")]
    public void OriginalTensFamily_GeneratedSamplesStillPassIndependentVerifier(string family)
    {
        var engine = new ExactSkillContractQuestionEngine();
        for (var seed = 1; seed <= 80; seed++)
        {
            var item = Assert.Single(engine.Generate(
                "uae-g1-one-hundred-tens-boundary",
                family,
                [family],
                ExactSkillQuestionDifficulty.Standard,
                1,
                seed,
                []));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                item.Family, item.Parameters, item.CorrectAnswer));
        }
    }
}
