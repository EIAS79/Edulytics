using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssAlg1Unit1StatisticsSpreadsheetQuestionsTests
{
    [Theory]
    [InlineData(0, "uniform")]
    [InlineData(1, "symmetric bell-shaped")]
    [InlineData(2, "right-skewed")]
    [InlineData(3, "left-skewed")]
    [InlineData(4, "bimodal")]
    public void DistributionShapeOracleMatchesCanonicalLabels(int shape, string expected)
    {
        var p = new Dictionary<string, int>
        {
            ["shape"] = shape, ["shift"] = 2, ["scale"] = 2, ["variant"] = 0
        };

        Assert.Equal(expected,
            UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.Solve(
                UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.DistributionShapeFamily, p));
        Assert.True(UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.Verify(
            UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.DistributionShapeFamily, p, expected));
    }

    [Fact]
    public void ShapeVerifierAcceptsEquivalentWordingAndRejectsWrongTail()
    {
        var p = new Dictionary<string, int>
        {
            ["shape"] = 2, ["shift"] = 0, ["scale"] = 1, ["variant"] = 4
        };
        Assert.True(UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.Verify(
            UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.DistributionShapeFamily, p, "skewed right"));
        Assert.False(UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.Verify(
            UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.DistributionShapeFamily, p, "left-skewed"));
    }
}
