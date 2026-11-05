using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssAlg1Unit2AlgebraQuestionsTests
{
    [Fact]
    public void ConstraintFeasibilityRequiresCapacityAndBudget()
    {
        var feasible = new Dictionary<string, int>
        {
            ["fixedCost"] = 8, ["itemCost"] = 14, ["capacity"] = 3,
            ["budget"] = 120, ["guests"] = 18, ["items"] = 6, ["variant"] = 0
        };
        Assert.Equal("yes", UsCcssAlg1Unit2AlgebraQuestions.Solve(
            UsCcssAlg1Unit2AlgebraQuestions.ConstraintFeasibilityFamily, feasible));

        var overBudget = new Dictionary<string, int>(feasible) { ["items"] = 9 };
        Assert.Equal("no", UsCcssAlg1Unit2AlgebraQuestions.Solve(
            UsCcssAlg1Unit2AlgebraQuestions.ConstraintFeasibilityFamily, overBudget));
    }

    [Fact]
    public void LinearRelationshipEquationUsesRateAndFixedAmount()
    {
        var p = new Dictionary<string, int>
        {
            ["rate"] = 7, ["fixedAmount"] = 25, ["variant"] = 0
        };
        Assert.Equal("y=7x+25", UsCcssAlg1Unit2AlgebraQuestions.Solve(
            UsCcssAlg1Unit2AlgebraQuestions.LinearRelationshipEquationFamily, p));
        Assert.True(UsCcssAlg1Unit2AlgebraQuestions.Verify(
            UsCcssAlg1Unit2AlgebraQuestions.LinearRelationshipEquationFamily, p, " y = 7*x + 25 "));
    }

    [Theory]
    [InlineData(0, "linear")]
    [InlineData(1, "constant-total")]
    [InlineData(2, "inverse-variation")]
    public void PatternClassificationMatchesRelation(int relation, string expected)
    {
        var p = new Dictionary<string, int>
        {
            ["relation"] = relation, ["m"] = 5, ["b"] = 3, ["k"] = 48, ["variant"] = 0
        };
        Assert.Equal(expected, UsCcssAlg1Unit2AlgebraQuestions.Solve(
            UsCcssAlg1Unit2AlgebraQuestions.PatternClassificationFamily, p));
    }

    [Fact]
    public void SolutionChecksEvaluateOriginalEquation()
    {
        var one = new Dictionary<string, int>
        {
            ["a"] = 3, ["b"] = 2, ["c"] = 17, ["x"] = 5, ["y"] = 0, ["variant"] = 0
        };
        Assert.Equal("yes", UsCcssAlg1Unit2AlgebraQuestions.Solve(
            UsCcssAlg1Unit2AlgebraQuestions.OneVariableSolutionCheckFamily, one));

        var two = new Dictionary<string, int>
        {
            ["a"] = 2, ["b"] = 3, ["c"] = 12, ["x"] = 3, ["y"] = 2, ["variant"] = 0
        };
        Assert.Equal("yes", UsCcssAlg1Unit2AlgebraQuestions.Solve(
            UsCcssAlg1Unit2AlgebraQuestions.TwoVariableSolutionCheckFamily, two));
    }

    [Fact]
    public void EquivalenceOracleDistinguishesTwoSidedAndOneSidedChanges()
    {
        var valid = new Dictionary<string, int>
        {
            ["a"] = 3, ["b"] = 2, ["c"] = 17, ["operation"] = 1, ["variant"] = 0
        };
        var invalid = new Dictionary<string, int>(valid) { ["operation"] = 3 };
        Assert.Equal("equivalent", UsCcssAlg1Unit2AlgebraQuestions.Solve(
            UsCcssAlg1Unit2AlgebraQuestions.EquationEquivalenceFamily, valid));
        Assert.Equal("not-equivalent", UsCcssAlg1Unit2AlgebraQuestions.Solve(
            UsCcssAlg1Unit2AlgebraQuestions.EquationEquivalenceFamily, invalid));
    }

    [Theory]
    [InlineData(0, 0, "a")]
    [InlineData(1, 1, "b")]
    [InlineData(3, 2, "c")]
    public void LiteralIsolationReturnsCorrectOption(int template, int correct, string expected)
    {
        var p = new Dictionary<string, int>
        {
            ["template"] = template, ["correct"] = correct, ["variant"] = 0
        };
        Assert.Equal(expected, UsCcssAlg1Unit2AlgebraQuestions.Solve(
            UsCcssAlg1Unit2AlgebraQuestions.LiteralEquationIsolateFamily, p));
    }

    [Fact]
    public void StandardFormOracleReturnsSlopeAndYIntercept()
    {
        var p = new Dictionary<string, int>
        {
            ["slope"] = -3, ["intercept"] = 7, ["scale"] = 2, ["variant"] = 0
        };
        Assert.Equal("-3,7", UsCcssAlg1Unit2AlgebraQuestions.Solve(
            UsCcssAlg1Unit2AlgebraQuestions.StandardFormSlopeInterceptFamily, p));
        Assert.True(UsCcssAlg1Unit2AlgebraQuestions.Verify(
            UsCcssAlg1Unit2AlgebraQuestions.StandardFormSlopeInterceptFamily, p, "(-3, 7)"));
    }
    [Fact]
    public void LinearSystemVerifiesBothEquationsAndRejectsWrongPair()
    {
        var parameters = new Dictionary<string, int>
        {
            ["a"] = 1, ["b"] = 1, ["c1"] = 11,
            ["d"] = 1, ["e"] = -1, ["c2"] = 5, ["variant"] = 0
        };
        Assert.Equal("8,3", UsCcssAlg1Unit2AlgebraQuestions.Solve(
            UsCcssAlg1Unit2AlgebraQuestions.LinearSystemSolveFamily, parameters));
        Assert.True(UsCcssAlg1Unit2AlgebraQuestions.Verify(
            UsCcssAlg1Unit2AlgebraQuestions.LinearSystemSolveFamily, parameters, "(8, 3)"));
        Assert.False(UsCcssAlg1Unit2AlgebraQuestions.Verify(
            UsCcssAlg1Unit2AlgebraQuestions.LinearSystemSolveFamily, parameters, "7,4"));
    }
    [Theory]
    [InlineData(0, "one")]
    [InlineData(1, "none")]
    [InlineData(2, "infinitely-many")]
    public void SystemCountClassifiesAllCases(int shape, string expected)
    {
        var p = new Dictionary<string, int>
        {
            ["shape"] = shape, ["sum"] = 5, ["difference"] = 3, ["variant"] = 0
        };
        Assert.Equal(expected, UsCcssAlg1Unit2AlgebraQuestions.Solve(
            UsCcssAlg1Unit2AlgebraQuestions.SystemSolutionCountFamily, p));
    }
}
