using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsGrade8IntegerExponentOutcomeTests
{
    private const string Outcome = "CCSS:8.EE.A.1";
    private const string Family = "usccss.g8.integer_exponent_laws";

    [Fact]
    public void OfficialGrade8OutcomeRoutesToIntegerLawsAndNeverLogarithms()
    {
        Assert.True(OfficialOutcomePracticeRuleRegistry.TryResolve(Outcome, out var resolved));
        Assert.Equal("ccss-grade8-integer-exponent-laws", resolved!.RuleId);
        Assert.Equal("ccss-g8-integer-exponent-laws", resolved.TargetRule.Id);
        Assert.Equal([Family], resolved.TargetRule.Families);

        Assert.True(OfficialOutcomePracticeMapRegistry.TryResolve(Outcome, out var recorded));
        Assert.Equal(resolved.RuleId, recorded!.ResolutionRuleId);
        Assert.Equal(resolved.TargetRule.Id, recorded.TargetRuleId);
        Assert.Equal(400, recorded.Priority);
        Assert.DoesNotContain("supporting.logarithms.evaluate", resolved.TargetRule.Families);
        Assert.True(SupportingPracticeTargetRuleRegistry.TryGetById(recorded.TargetRuleId, out var target));
        Assert.Equal([Family], target!.Families);
    }

    [Fact]
    public void ProductQuotientAndPowerOfPowerGenerateExactIndependentlyGradedValues()
    {
        var engine = new ExactSkillContractQuestionEngine();
        var encountered = new HashSet<int>();
        var positive = false;
        var negative = false;
        var zero = false;
        foreach (var difficulty in new[]
        {
            ExactSkillQuestionDifficulty.Standard,
            ExactSkillQuestionDifficulty.Stretch,
            ExactSkillQuestionDifficulty.Challenge
        })
        {
            for (var seed = 0; seed < 48; seed++)
            {
                var question = Assert.Single(engine.Generate(
                    "us-g8-exponent-standard-check", "PED:US-CCSS-MATH:G8:U01:L01",
                    [Family], difficulty, 1, 810000 + seed + (int)difficulty * 1000, []));
                Assert.Equal(Family, question.Family);
                Assert.DoesNotContain("logarithm", question.Prompt, StringComparison.OrdinalIgnoreCase);
                var p = question.Parameters;
                Assert.InRange(p["base"], 2, 4);
                Assert.InRange(p["left"], -2, 2);
                Assert.InRange(p["right"], -2, 2);
                Assert.InRange(p["mode"], 0, 2);
                encountered.Add(p["mode"]);
                var exponent = p["mode"] switch
                {
                    0 => p["left"] + p["right"],
                    1 => p["left"] - p["right"],
                    _ => p["left"] * p["right"]
                };
                var result = exponent >= 0
                    ? (int)Math.Pow(p["base"], exponent)
                    : 1.0 / Math.Pow(p["base"], -exponent);
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    question.Family, question.Parameters, question.CorrectAnswer));
                var fraction = question.CorrectAnswer.Split('/');
                var actual = fraction.Length == 2
                    ? double.Parse(fraction[0], System.Globalization.CultureInfo.InvariantCulture) /
                      double.Parse(fraction[1], System.Globalization.CultureInfo.InvariantCulture)
                    : double.Parse(question.CorrectAnswer, System.Globalization.CultureInfo.InvariantCulture);
                Assert.Equal(result, actual, 12);
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    question.Family, question.Parameters, "999999"));
                if (exponent > 0) positive = true;
                if (exponent < 0) negative = true;
                if (exponent == 0) zero = true;
            }
        }
        Assert.Equal(3, encountered.Count);
        Assert.True(positive && negative && zero);
    }
}
