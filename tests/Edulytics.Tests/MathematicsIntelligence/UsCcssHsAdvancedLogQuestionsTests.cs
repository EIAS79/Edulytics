using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Tests the additional Coder 8 L03/L06/L07-L15 numeric and classification
/// kernel. L13 open proof is excluded from machine scoring.
/// </summary>
public sealed class UsCcssHsAdvancedLogQuestionsTests
{
    private static readonly string[] Families =
    [
        UsCcssHsAdvancedLogQuestions.EvaluateDecreasingBase,
        UsCcssHsAdvancedLogQuestions.SolveShiftedDomain,
        UsCcssHsAdvancedLogQuestions.InvalidRealLog,
        UsCcssHsAdvancedLogQuestions.CompareDifferentBases,
        UsCcssHsAdvancedLogQuestions.CheckProductIdentity,
        UsCcssHsAdvancedLogQuestions.ChangeBase,
        UsCcssHsAdvancedLogQuestions.InverseIdentity
    ];

    [Fact]
    public void SevenAdditionalBoundedLogFamiliesGenerateAndVerifyExactAnswers()
    {
        foreach (var family in Families)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            var prompts = new HashSet<string>(StringComparer.Ordinal);
            for (var seed = 1; seed <= 20; seed++)
            {
                var item = UsCcssHsAdvancedLogQuestions.Build(family, new Random(seed));
                var correct = UsCcssHsAdvancedLogQuestions.Solve(family, item.Parameters);
                Assert.True(UsCcssHsAdvancedLogQuestions.Verify(family, item.Parameters, correct));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family, item.Parameters, correct));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family, item.Parameters, "<script>alert(1)</script>"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family, item.Parameters, new string('9', 300)));
                prompts.Add(item.Prompt);
            }
            Assert.True(prompts.Count >= 3, $"Insufficient variation in {family}");
        }
    }

    [Fact]
    public void DecreasingBaseLogarithmHasNegativeExponentAndAcceptsEquivalentFractions()
    {
        var p = new Dictionary<string, int> { ["exponent"] = -2 };
        Assert.Equal("-2", UsCcssHsAdvancedLogQuestions.Solve(
            UsCcssHsAdvancedLogQuestions.EvaluateDecreasingBase, p));
        Assert.True(UsCcssHsAdvancedLogQuestions.Verify(
            UsCcssHsAdvancedLogQuestions.EvaluateDecreasingBase, p, "-4/2"));
        Assert.False(UsCcssHsAdvancedLogQuestions.Verify(
            UsCcssHsAdvancedLogQuestions.EvaluateDecreasingBase, p, "2"));
    }

    [Fact]
    public void ShiftedLogDomainRejectsExcludedArgumentAndAcceptsExactRationals()
    {
        var p = new Dictionary<string, int>
        {
            ["base"] = 2, ["shift"] = 3, ["exponent"] = 1
        };
        Assert.Equal("5", UsCcssHsAdvancedLogQuestions.Solve(
            UsCcssHsAdvancedLogQuestions.SolveShiftedDomain, p));
        Assert.True(UsCcssHsAdvancedLogQuestions.Verify(
            UsCcssHsAdvancedLogQuestions.SolveShiftedDomain, p, "5/1"));
        Assert.False(UsCcssHsAdvancedLogQuestions.Verify(
            UsCcssHsAdvancedLogQuestions.SolveShiftedDomain, p, "3"));
        p["exponent"] = -1;
        Assert.Equal("7/2", UsCcssHsAdvancedLogQuestions.Solve(
            UsCcssHsAdvancedLogQuestions.SolveShiftedDomain, p));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void RealLogarithmDomainExclusionsAreClassifiedNotNumericallyEvaluated(int mode)
    {
        var p = new Dictionary<string, int> { ["mode"] = mode, ["argument"] = 5 };
        Assert.Equal("undefined", UsCcssHsAdvancedLogQuestions.Solve(
            UsCcssHsAdvancedLogQuestions.InvalidRealLog, p));
        Assert.True(UsCcssHsAdvancedLogQuestions.Verify(
            UsCcssHsAdvancedLogQuestions.InvalidRealLog, p, "UNDEFINED"));
        Assert.False(UsCcssHsAdvancedLogQuestions.Verify(
            UsCcssHsAdvancedLogQuestions.InvalidRealLog, p, "0"));
    }

    [Fact]
    public void DifferentBasesReverseComparisonForFractionalArgument()
    {
        var p = new Dictionary<string, int>
        {
            ["largerBase"] = 4, ["smallerBase"] = 2,
            ["numerator"] = 3, ["denominator"] = 1
        };
        Assert.Equal("<", UsCcssHsAdvancedLogQuestions.Solve(
            UsCcssHsAdvancedLogQuestions.CompareDifferentBases, p));
        Assert.True(UsCcssHsAdvancedLogQuestions.Verify(
            UsCcssHsAdvancedLogQuestions.CompareDifferentBases, p, "<"));
        p["numerator"] = 1;
        p["denominator"] = 3;
        Assert.Equal(">", UsCcssHsAdvancedLogQuestions.Solve(
            UsCcssHsAdvancedLogQuestions.CompareDifferentBases, p));
    }

    [Fact]
    public void LogarithmOfSumIsNotTheProductIdentityAndBaseChangeIsExplicitlyBounded()
    {
        var p = new Dictionary<string, int>
        {
            ["base"] = 2, ["x"] = 2, ["y"] = 4,
            ["productCorrect"] = 0, ["rhsArgument"] = 6
        };
        Assert.Equal("false", UsCcssHsAdvancedLogQuestions.Solve(
            UsCcssHsAdvancedLogQuestions.CheckProductIdentity, p));
        Assert.False(UsCcssHsAdvancedLogQuestions.Verify(
            UsCcssHsAdvancedLogQuestions.CheckProductIdentity, p, "true"));
        p["productCorrect"] = 1;
        p["rhsArgument"] = 8;
        Assert.Equal("true", UsCcssHsAdvancedLogQuestions.Solve(
            UsCcssHsAdvancedLogQuestions.CheckProductIdentity, p));

        var log = new Dictionary<string, int> { ["base"] = 2, ["target"] = 7 };
        Assert.True(UsCcssHsAdvancedLogQuestions.Verify(
            UsCcssHsAdvancedLogQuestions.ChangeBase, log, "ln(7)/ln(2)"));
        Assert.True(UsCcssHsAdvancedLogQuestions.Verify(
            UsCcssHsAdvancedLogQuestions.ChangeBase, log, "x=log_2(7)"));
        Assert.False(UsCcssHsAdvancedLogQuestions.Verify(
            UsCcssHsAdvancedLogQuestions.ChangeBase, log, "7/2"));
        Assert.False(UsCcssHsAdvancedLogQuestions.Verify(
            UsCcssHsAdvancedLogQuestions.ChangeBase, log, "Math.log(7)/Math.log(2)"));
    }

    [Fact]
    public void InvalidParametersAndUntrustedExpressionsNeverReceiveCorrectGrades()
    {
        var bad = new Dictionary<string, int> { ["base"] = 1, ["shift"] = 3, ["exponent"] = 1 };
        Assert.False(UsCcssHsAdvancedLogQuestions.Verify(
            UsCcssHsAdvancedLogQuestions.SolveShiftedDomain, bad, "5"));
        var p = new Dictionary<string, int> { ["base"] = 2, ["argument"] = 7 };
        Assert.True(UsCcssHsAdvancedLogQuestions.Verify(
            UsCcssHsAdvancedLogQuestions.InverseIdentity, p, "14/2"));
        Assert.False(UsCcssHsAdvancedLogQuestions.Verify(
            UsCcssHsAdvancedLogQuestions.InverseIdentity, p, "2^log_2(7)"));
    }
}
