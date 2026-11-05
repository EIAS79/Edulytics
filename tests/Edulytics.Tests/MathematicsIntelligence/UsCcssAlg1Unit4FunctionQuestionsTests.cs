using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssAlg1Unit4FunctionQuestionsTests
{
    [Theory]
    [InlineData(0, 5, 8, "yes")]
    [InlineData(1, 5, 8, "no")]
    [InlineData(1, 5, 5, "yes")]
    public void SameInputRequiresOneUniqueOutput(int repeats, int a, int b, string expected)
    {
        var parameters = new Dictionary<string, int>
        {
            ["x"] = 2, ["a"] = a, ["b"] = b,
            ["repeats"] = repeats, ["variant"] = 0
        };
        Assert.Equal(expected, UsCcssAlg1Unit4FunctionQuestions.Solve(
            UsCcssAlg1Unit4FunctionQuestions.FunctionRelationFamily, parameters));
    }

    [Fact]
    public void FunctionNotationEvaluatesNamedInputExactly()
    {
        var parameters = new Dictionary<string, int>
        {
            ["initial"] = 20, ["rate"] = 15,
            ["input"] = 2, ["variant"] = 0
        };

        Assert.Equal("50", UsCcssAlg1Unit4FunctionQuestions.Solve(
            UsCcssAlg1Unit4FunctionQuestions.FunctionNotationFamily, parameters));
        Assert.True(UsCcssAlg1Unit4FunctionQuestions.Verify(
            UsCcssAlg1Unit4FunctionQuestions.FunctionNotationFamily, parameters, "50"));
        Assert.False(UsCcssAlg1Unit4FunctionQuestions.Verify(
            UsCcssAlg1Unit4FunctionQuestions.FunctionNotationFamily, parameters, "2"));
    }

    [Fact]
    public void FunctionNotationRejectsIncompleteParameters()
    {
        var parameters = new Dictionary<string, int>
        {
            ["initial"] = 20, ["rate"] = 15, ["input"] = 2
        };

        Assert.False(UsCcssAlg1Unit4FunctionQuestions.Verify(
            UsCcssAlg1Unit4FunctionQuestions.FunctionNotationFamily, parameters, "50"));
    }

    [Fact]
    public void DomainAndRangeRespectContextBounds()
    {
        var parameters = new Dictionary<string, int>
        {
            ["duration"] = 6, ["rate"] = 4, ["start"] = 24, ["variant"] = 0
        };

        Assert.Equal("D=0,6;R=0,24", UsCcssAlg1Unit4FunctionQuestions.Solve(
            UsCcssAlg1Unit4FunctionQuestions.FunctionDomainRangeFamily, parameters));
        Assert.True(UsCcssAlg1Unit4FunctionQuestions.Verify(
            UsCcssAlg1Unit4FunctionQuestions.FunctionDomainRangeFamily, parameters, "D=0,6; R=0,24"));
    }

    [Fact]
    public void PiecewiseFamilyChoosesInclusiveBoundaryRule()
    {
        var parameters = new Dictionary<string, int>
        {
            ["boundary"] = 3, ["leftSlope"] = 2, ["leftIntercept"] = 1,
            ["rightSlope"] = 5, ["rightIntercept"] = 4, ["side"] = 0, ["variant"] = 0
        };

        Assert.Equal("7", UsCcssAlg1Unit4FunctionQuestions.Solve(
            UsCcssAlg1Unit4FunctionQuestions.FunctionPiecewiseFamily, parameters));
        Assert.True(UsCcssAlg1Unit4FunctionQuestions.Verify(
            UsCcssAlg1Unit4FunctionQuestions.FunctionPiecewiseFamily, parameters, "7"));
    }


    [Fact]
    public void AbsoluteErrorIsNonnegativeDistanceFromTarget()
    {
        var parameters = new Dictionary<string, int>
        {
            ["target"] = 48, ["guess"] = 41, ["variant"] = 0
        };
        Assert.Equal("7", UsCcssAlg1Unit4FunctionQuestions.Solve(
            UsCcssAlg1Unit4FunctionQuestions.FunctionAbsoluteErrorFamily, parameters));
        Assert.True(UsCcssAlg1Unit4FunctionQuestions.Verify(
            UsCcssAlg1Unit4FunctionQuestions.FunctionAbsoluteErrorFamily, parameters, "7"));
    }

}
