using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Stage D acceptance cases prepared during A–B–C; do not execute until Stage D.
/// Covers |a(x-m)| = c with c positive, zero and negative.
/// </summary>
public sealed class UaeG9GeneralU02L05AbsoluteValueEquationTests
{
    private const string Prefix = "uae.g9.general.u02.l05.";

    [Theory]
    [InlineData("two_solutions", 2, 5, 4, "1,9", "1,8")]
    [InlineData("two_solutions", 2, 5, 4, "٩،١", "1,9,11")]
    [InlineData("one_solution", 3, -2, 7, "-2", "-3")]
    [InlineData("no_solution", 3, -2, 7, "لا يوجد حل", "-2")]
    [InlineData("evaluate_absolute", 3, -2, 7, "21", "-21")]
    [InlineData("midpoint", 3, -2, 7, "-2", "2")]
    [InlineData("composite_evaluation", 3, -2, 7, "11", "21")]
    [InlineData("outer_coefficient_no_solution", 3, -2, 7, "لا حل", "-2")]
    [InlineData("context_tolerance_endpoints", 1, 20, 3, "17,23", "17,22")]
    public void IndependentVerifierAcceptsCorrectAndRejectsWrong(
        string suffix, int a, int midpoint, int radius, string correct, string incorrect)
    {
        var parameters = new Dictionary<string, int>
        {
            ["a"] = a,
            ["m"] = midpoint,
            ["r"] = radius
        };
        var family = Prefix + suffix;
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family, parameters, correct));
        if (suffix == "no_solution")
            Assert.True(ExactSkillContractQuestionEngine.Verify(family, parameters, "لا حل"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family, parameters, incorrect));
    }

    [Fact]
    public void BoundaryValuesAndTamperedParametersAreRejected()
    {
        const string family = Prefix + "two_solutions";
        var p = new Dictionary<string, int>
        {
            ["a"] = 2, ["m"] = 5, ["r"] = 4
        };
        Assert.False(ExactSkillContractQuestionEngine.Verify(family, p, "1,1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family, p, "1,9,13"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family, p, "1,9,"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family, p, ",1,9"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,
            new Dictionary<string,int> { ["a"] = 0, ["m"] = 5, ["r"] = 4 }, "1,9"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,
            new Dictionary<string,int> { ["a"] = 2, ["m"] = 5 }, "1,9"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prefix + "unknown", p, "1,9"));
    }

    [Theory]
    [InlineData("one_solution", "+03")]
    [InlineData("one_solution", "3.0")]
    [InlineData("one_solution", "٣٫٠")]
    [InlineData("two_solutions", "1.0,9.0")]
    [InlineData("two_solutions", "٩٫٠،١٫٠")]
    public void EquivalentNumericNotationsAreAccepted(string familySuffix, string answer)
    {
        var parameters = new Dictionary<string, int>
        {
            ["a"] = 2, ["m"] = familySuffix == "one_solution" ? 3 : 5, ["r"] = 4
        };
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix + familySuffix, parameters, answer));
    }

    [Fact]
    public void TwoSolutionGeneratorPreparesIntegerOuterFactorAndFractionVariants()
    {
        var engine = new ExactSkillContractQuestionEngine();
        var seen = new HashSet<string>();
        for (var seed = 1; seed <= 60; seed++)
        {
            var q = Assert.Single(engine.Generate("UAE-G9-GENERAL",
                "PED:UAE-MOE-MATH:L9:GENERAL:02:05:ABSOLUTE-VALUE-EQUATIONS",
                new[] { Prefix + "two_solutions" },
                ExactSkillQuestionDifficulty.Standard, 1, seed,
                Array.Empty<string>()));
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family, q.Parameters, q.CorrectAnswer));
            seen.Add(q.Prompt.Contains(")/٢|", StringComparison.Ordinal) ? "fractional" :
                q.Prompt.Contains("٢|", StringComparison.Ordinal) ? "outer_factor" : "integer");
        }
        Assert.Contains("integer", seen);
        Assert.Contains("outer_factor", seen);
        Assert.Contains("fractional", seen);
    }

    [Fact]
    public void CompositeGeneratorCoversMultipleAbsoluteTermsWithVerifier()
    {
        var engine = new ExactSkillContractQuestionEngine();
        var multiAbsoluteSeen = false;
        for (var seed = 1; seed <= 48; seed++)
        {
            var q = Assert.Single(engine.Generate("UAE-G9-GENERAL",
                "PED:UAE-MOE-MATH:L9:GENERAL:02:05:ABSOLUTE-VALUE-EQUATIONS",
                new[] { Prefix + "composite_evaluation" },
                ExactSkillQuestionDifficulty.Standard, 1, seed,
                Array.Empty<string>()));
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family, q.Parameters, q.CorrectAnswer));
            if (q.Prompt.Contains("|ص|", StringComparison.Ordinal)) multiAbsoluteSeen = true;
        }
        Assert.True(multiAbsoluteSeen);
    }

    [Fact]
    public void GeneratedProblemsCarryAnswersThatIndependentVerifierAccepts()
    {
        var engine = new ExactSkillContractQuestionEngine();
        var families = new[]
        {
            Prefix + "two_solutions",
            Prefix + "one_solution",
            Prefix + "no_solution",
            Prefix + "evaluate_absolute",
            Prefix + "midpoint",
            Prefix + "composite_evaluation",
            Prefix + "outer_coefficient_no_solution",
            Prefix + "context_tolerance_endpoints"
        };
        var questions = engine.Generate("UAE-G9-GENERAL",
            "PED:UAE-MOE-MATH:L9:GENERAL:02:05:ABSOLUTE-VALUE-EQUATIONS",
            families, ExactSkillQuestionDifficulty.Standard, 7, 20261119,
            Array.Empty<string>());
        Assert.Equal(7, questions.Count);
        foreach (var q in questions)
        {
            Assert.StartsWith(Prefix, q.Family);
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family, q.Parameters, q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(q.Family, q.Parameters, "invalid"));
        }
    }
}
