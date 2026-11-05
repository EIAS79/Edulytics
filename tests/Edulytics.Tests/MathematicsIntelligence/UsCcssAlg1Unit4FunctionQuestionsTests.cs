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
}
