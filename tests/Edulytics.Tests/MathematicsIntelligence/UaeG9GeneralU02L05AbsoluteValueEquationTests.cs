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
            Prefix + "midpoint"
        };
        var questions = engine.Generate("UAE-G9-GENERAL",
            "PED:UAE-MOE-MATH:L9:GENERAL:02:05:ABSOLUTE-VALUE-EQUATIONS",
            families, ExactSkillQuestionDifficulty.Standard, 5, 20261119,
            Array.Empty<string>());
        Assert.Equal(5, questions.Count);
        foreach (var q in questions)
        {
            Assert.StartsWith(Prefix, q.Family);
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family, q.Parameters, q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(q.Family, q.Parameters, "invalid"));
        }
    }
}
