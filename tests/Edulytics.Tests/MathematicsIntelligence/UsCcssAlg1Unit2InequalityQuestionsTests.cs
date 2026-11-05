using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssAlg1Unit2InequalityQuestionsTests
{
    private static Dictionary<string,int> Args(int mode=0) => new()
    {
        ["a"]=2, ["b"]=3, ["c"]=20, ["x"]=10, ["y"]=0,
        ["mode"]=mode, ["minimum"]=5, ["variant"]=0
    };

    [Theory]
    [InlineData(0,"a")]
    [InlineData(1,"b")]
    [InlineData(2,"c")]
    public void ModelOperatorMatchesWording(int mode,string expected)
    {
        Assert.Equal(expected,UsCcssAlg1Unit2InequalityQuestions.Solve(
            UsCcssAlg1Unit2InequalityQuestions.ModelOperator,Args(mode)));
    }

    [Fact]
    public void BoundaryInclusionIsExact()
    {
        Assert.Equal("yes",UsCcssAlg1Unit2InequalityQuestions.Solve(
            UsCcssAlg1Unit2InequalityQuestions.OneVariableCheck,Args(0)));
        Assert.Equal("no",UsCcssAlg1Unit2InequalityQuestions.Solve(
            UsCcssAlg1Unit2InequalityQuestions.OneVariableCheck,Args(2)));
    }

    [Fact]
    public void SystemRequiresBothConditions()
    {
        Assert.Equal("yes",UsCcssAlg1Unit2InequalityQuestions.Solve(
            UsCcssAlg1Unit2InequalityQuestions.SystemCheck,Args()));
        var invalid=Args();invalid["minimum"]=10;invalid["x"]=9;
        Assert.Equal("no",UsCcssAlg1Unit2InequalityQuestions.Solve(
            UsCcssAlg1Unit2InequalityQuestions.SystemCheck,invalid));
    }
}
