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
}
