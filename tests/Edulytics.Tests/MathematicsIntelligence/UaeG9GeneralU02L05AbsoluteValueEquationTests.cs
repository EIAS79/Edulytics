using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UaeG9GeneralU02L05AbsoluteValueEquationTests
{
    private const string Lesson = "PED:UAE-MOE-MATH:L9:GENERAL:02:05:ABSOLUTE-VALUE-EQUATIONS";
    private const string Prefix = "uae.g9.general.u02.l05.";

    [Fact]
    public void AllFiveExactFamiliesAreRegisteredAndLessonScoped()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson, out var contract));
        Assert.NotNull(contract);
        Assert.Equal(5, contract!.AllowedQuestionFamilies.Count);
        Assert.All(contract.AllowedQuestionFamilies, family =>
        {
            Assert.StartsWith(Prefix, family, StringComparison.Ordinal);
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        });
    }

    [Theory]
    [InlineData("two_solutions", 2, 5, 4, "1,9", "-")]
    [InlineData("two_solutions", 2, 5, 4, "٩،١", "1,9,13")]
    [InlineData("one_solution", 3, -2, 7, "-2", "2")]
    [InlineData("no_solution", 3, -2, 7, "لا يوجد حل", "4")]
    [InlineData("evaluate_absolute", 3, -2, 7, "21", "-21")]
    [InlineData("midpoint", 3, -2, 7, "-2", "5")]
    public void IndependentGraderHandlesAllCasesAndRejectsWrongAnswers(
        string suffix, int a, int midpoint, int radius, string correct, string wrong)
    {
        var p = new Dictionary<string, int>
        {
            ["a"] = a,
            ["m"] = midpoint,
            ["r"] = radius
        };
        var family = Prefix + suffix;
        Assert.True(ExactSkillContractQuestionEngine.Verify(family, p, correct));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family, p, wrong));
    }

    [Fact]
    public void ParameterTamperingAndInvalidFamiliesFailClosed()
    {
        var p = new Dictionary<string, int> { ["a"] = 2, ["m"] = 5, ["r"] = 4 };
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prefix + "two_solutions", 
            new Dictionary<string,int>{{"a",0},{"m",5},{"r",4}}, "1,9"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prefix + "two_solutions",
            new Dictionary<string,int>{{"a",2},{"m",5}}, "1,9"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prefix + "unknown", p, "1,9"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prefix + "two_solutions", p, "1,1"));
    }

    [Fact]
    public void GeneratedQuestionHasExactAnswerAndRejectsOneWrongAnswer()
    {
        var engine = new ExactSkillContractQuestionEngine();
        var family = Prefix + "two_solutions";
        var questions = engine.Generate("UAE-G9-GENERAL", Lesson, [family],
            ExactSkillQuestionDifficulty.Standard, 3, 20261119, []);
        Assert.Equal(3, questions.Count);
        foreach(var q in questions)
        {
            Assert.Equal(family, q.Family);
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family, q.Parameters, q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(q.Family, q.Parameters, "abc"));
            Assert.Contains("القيمة", q.Solution);
        }
    }
}
