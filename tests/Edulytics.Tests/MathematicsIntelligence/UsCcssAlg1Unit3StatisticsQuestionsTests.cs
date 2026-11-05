using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssAlg1Unit3StatisticsQuestionsTests
{
    private static Dictionary<string,int> P(int option=0,int index=0,int sign=0) => new()
    {
        ["option"]=option,["index"]=index,["sign"]=sign,["variant"]=0
    };

    [Fact]
    public void TwoWayCountsAndDenominatorsAreDistinct()
    {
        Assert.Equal("21",UsCcssAlg1Unit3StatisticsQuestions.Solve(
            UsCcssAlg1Unit3StatisticsQuestions.TwoWayCount,P()));
        Assert.Equal("60",UsCcssAlg1Unit3StatisticsQuestions.Solve(
            UsCcssAlg1Unit3StatisticsQuestions.TwoWayRelative,P(0)));
        Assert.Equal("57.14",UsCcssAlg1Unit3StatisticsQuestions.Solve(
            UsCcssAlg1Unit3StatisticsQuestions.TwoWayRelative,P(1)));
        Assert.Equal("30",UsCcssAlg1Unit3StatisticsQuestions.Solve(
            UsCcssAlg1Unit3StatisticsQuestions.TwoWayRelative,P(2)));
    }

    [Fact]
    public void LinearRegressionAndResidualExamplesAreExact()
    {
        Assert.Equal("6.7",UsCcssAlg1Unit3StatisticsQuestions.Solve(
            UsCcssAlg1Unit3StatisticsQuestions.LinearPrediction,P(index:0)));
        Assert.Equal("-0.3",UsCcssAlg1Unit3StatisticsQuestions.Solve(
            UsCcssAlg1Unit3StatisticsQuestions.Residual,P(index:1)));
        Assert.Equal("-0.997",UsCcssAlg1Unit3StatisticsQuestions.Solve(
            UsCcssAlg1Unit3StatisticsQuestions.CorrelationRounded,P(sign:1)));
    }
}
