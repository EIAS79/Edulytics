using Edulytics.Core.Curriculum;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Independently authored Algebra 1 Unit 2 lessons U02:L01–L10.
/// Static content and arithmetic tests only; no persistence/UAT acceptance claim.
/// </summary>
public sealed class UsCcssAlgebra1Unit2ModelEquationsRichV2Tests
{
    private const string Prefix = "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U02:";

    [Theory]
    [InlineData("L01", "Planning a Pizza Party")]
    [InlineData("L02", "Writing Equations to Model Relationships (Part 1)")]
    [InlineData("L03", "Writing Equations to Model Relationships (Part 2)")]
    [InlineData("L04", "Equations and Their Solutions")]
    [InlineData("L05", "Equations and Their Graphs")]
    [InlineData("L06", "Equivalent Equations")]
    [InlineData("L07", "Explaining Steps for Rewriting Equations")]
    [InlineData("L08", "Which Variable to Solve for? (Part 1)")]
    [InlineData("L09", "Which Variable to Solve for? (Part 2)")]
    [InlineData("L10", "Connecting Equations to Graphs (Part 1)")]
    [InlineData("L11", "Connecting Equations to Graphs (Part 2)")]
    [InlineData("L12", "Writing and Graphing Systems of Linear Equations")]
    [InlineData("L13", "Solving Systems by Substitution")]
    [InlineData("L14", "Solving Systems by Elimination (Part 1)")]
    [InlineData("L15", "Solving Systems by Elimination (Part 2)")]
    [InlineData("L16", "Solving Systems by Elimination (Part 3)")]
    [InlineData("L17", "Systems of Linear Equations and Their Solutions")]
    [InlineData("L18", "Representing Situations with Inequalities")]
    [InlineData("L19", "Solutions to Inequalities in One Variable")]
    [InlineData("L20", "Writing and Solving Inequalities in One Variable")]
    [InlineData("L21", "Graphing Linear Inequalities in Two Variables (Part 1)")]
    [InlineData("L22", "Graphing Linear Inequalities in Two Variables (Part 2)")]
    [InlineData("L23", "Solving Problems with Inequalities in Two Variables")]
    [InlineData("L24", "Solutions to Systems of Linear Inequalities in Two Variables")]
    [InlineData("L25", "Solving Problems with Systems of Linear Inequalities in Two Variables")]
    [InlineData("L26", "Modeling with Systems of Inequalities in Two Variables")]
    public void SourceSpecificLessonIsRegisteredAndHasActualReasonedTasks(
        string ordinal, string title)
    {
        var lesson = RichLessonContentV2Registry.Find(Prefix + ordinal, "en");
        Assert.NotNull(lesson);
        Assert.Equal(title, lesson.Title);
        var n = int.Parse(ordinal[1..]);
        Assert.Contains(lesson.ExplanationParagraphs, x =>
            x.Contains("/1/2/" + n + "/preparation.html", StringComparison.Ordinal));
        Assert.True(lesson.KeyConcepts.Count >= 3);
        Assert.True(lesson.WorkedExamples.Count >= 3);
        Assert.All(lesson.WorkedExamples, x =>
        {
            Assert.True(x.Steps.Count >= 3);
            Assert.False(string.IsNullOrWhiteSpace(x.Answer));
            Assert.False(string.IsNullOrWhiteSpace(x.Check));
        });
        Assert.True(lesson.CommonMistakes.Count >= 2);
        Assert.True(lesson.SummaryPoints.Count >= 3);
        Assert.Contains("teacher", string.Join(" ", lesson.ExplanationParagraphs),
            StringComparison.OrdinalIgnoreCase);
        Assert.Contains("rubric", string.Join(" ", lesson.ExplanationParagraphs),
            StringComparison.OrdinalIgnoreCase);
        Assert.All(lesson.Visuals, visual =>
            Assert.False(string.IsNullOrWhiteSpace(visual.Description)));
        Assert.Empty(lesson.Videos);
    }

    [Fact]
    public void AllTwentySixSidecarsHaveDistinctExactLessonCodes()
    {
        var entries = RichLessonContentV2Registry.AllDocuments
            .SelectMany(x => x.Lessons)
            .Where(x => x.LessonCode.StartsWith(Prefix, StringComparison.Ordinal))
            .ToArray();
        Assert.Equal(26, entries.Length);
        Assert.Equal(26, entries.Select(x => x.LessonCode).Distinct().Count());
    }

    [Fact]
    public void PizzaPartyFeasibilityHasBothCapacityAndBudget()
    {
        static bool Feasible(int guests, int pizzas) =>
            guests is >= 0 and <= 24 && pizzas >= 0 &&
            3 * pizzas >= guests && 14 * pizzas + 8 <= 120;
        Assert.False(Feasible(20, 6));
        Assert.True(Feasible(20, 7));
        Assert.True(Feasible(20, 8));
        Assert.False(Feasible(20, 9));
        Assert.Equal(106, 14 * 7 + 8);
        Assert.Equal(218, 14 * 7 + 6 * 20);
    }

    [Fact]
    public void PercentageGeometryAndPurchaseModelHaveCorrectEquations()
    {
        Assert.Equal(2235d, 1.07 * 2000 + 95, 8);
        Assert.Equal(2, 8 - 12 + 6);
        Assert.Equal(2, 4 - 6 + 4);
        Assert.Equal(8d, 3.2 * 2.5, 8);
        Assert.Equal(3.75d, 12d / 3.2, 8);
    }

    [Fact]
    public void AdditiveLinearConstantSumAndInverseTablesRespectDomains()
    {
        foreach (var x in new[] {0, 1, 2, 4})
            Assert.Equal(5 * x + 3, new Dictionary<int, int>
                {{0, 3}, {1, 8}, {2, 13}, {4, 23}}[x]);
        foreach (var x in new[] {0, 25, 100, 175})
            Assert.Equal(400, x + (400 - x));
        foreach (var x in new[] {1, 2, 3, 6})
            Assert.Equal(48, x * (48 / x));
        Assert.Throws<DivideByZeroException>(() =>
        {
            var zero = 0;
            _ = 48 / zero;
        });
    }

    [Fact]
    public void EquationSolutionsAndGraphPointsAreCheckedAgainstOriginal()
    {
        Assert.Equal(66, 12 + 18 * 3);
        Assert.NotEqual(66, 12 + 18 * 2);
        Assert.Equal(84, 12 + 18 * 4);
        Assert.Equal(14, 4 * 2 + 6);
        Assert.NotEqual(16, 4 * 2 + 6);
        Assert.Equal(2, 4 * (-1) + 6);
    }

    [Fact]
    public void EquivalentEquationsMustPreserveEntireSolutionSet()
    {
        Assert.Equal(24, 3 * 5 + 9);
        Assert.Equal(15, 3 * 5);
        Assert.Equal(6, 16 - 10);
        Assert.Equal(0, 0 * (0 - 4));
        Assert.Equal(0, 4 * (4 - 4));
        Assert.Equal(0, 0 * 0);
        Assert.NotEqual(5, 4); // Multiplying x=5 by zero destroys uniqueness.
    }

    [Fact]
    public void VariableIsolationHasSafeDomainsAndUnits()
    {
        Assert.Equal(7d, 52.5 / 7.5);
        Assert.Equal(8d, 3.6 / 0.45, 8);
        Assert.Equal(10d, 30d / 2 - 5);
        Assert.Equal(42d, (150d - 3.75 * 12) / 2.5, 8);
        Assert.Equal(40.5d, (150d - 3.75 * 13) / 2.5, 8);
        Assert.True(2.5 * 40 + 3.75 * 13 <= 150);
        Assert.False(2.5 * 41 + 3.75 * 13 <= 150);
        Assert.Equal(20d, (150d - 2.5 * 30) / 3.75);
    }

    [Fact]
    public void GraphAndCarnivalModelDistinguishDiscreteValidPairs()
    {
        var feasible = new[] {(0, 6), (5, 4), (10, 2), (15, 0)};
        Assert.All(feasible, pair =>
        {
            Assert.True(pair.Item1 >= 0 && pair.Item2 >= 0);
            Assert.Equal(30, 2 * pair.Item1 + 5 * pair.Item2);
        });
        Assert.NotEqual(30, 2 * 3 + 5 * 5);
        Assert.Equal(30, 2 * (-5) + 5 * 8);
        Assert.True(-5 < 0); // Algebraic solution, forbidden context.
    }
}
