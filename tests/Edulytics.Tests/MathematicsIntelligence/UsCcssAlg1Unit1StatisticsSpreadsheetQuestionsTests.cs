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

    [Fact]
    public void IqrOracleUsesMedianOfHalves()
    {
        var p = new Dictionary<string, int>
        {
            ["base"] = 4, ["step"] = 3, ["variant"] = 2
        };
        Assert.Equal("18", UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.Solve(
            UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.IqrFamily, p));
        Assert.True(UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.Verify(
            UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.IqrFamily, p, "18"));
    }

    [Fact]
    public void MadOracleAcceptsEquivalentFraction()
    {
        var p = new Dictionary<string, int>
        {
            ["base"] = 20, ["step"] = 2, ["variant"] = 5
        };
        Assert.Equal("2.4", UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.Solve(
            UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.MadFamily, p));
        Assert.True(UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.Verify(
            UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.MadFamily, p, "12/5"));
        Assert.False(UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.Verify(
            UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.MadFamily, p, "2.5"));
    }

    [Fact]
    public void SpreadsheetFormulaOracleUsesCellReferencesAndOrderOfOperations()
    {
        var p = new Dictionary<string, int>
        {
            ["a"] = 5, ["b"] = 4, ["c"] = 3, ["op"] = 0, ["variant"] = 0
        };
        Assert.Equal("13", UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.Solve(
            UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.SpreadsheetFormulaFamily, p));
        Assert.True(UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.Verify(
            UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.SpreadsheetFormulaFamily, p, "13"));
        Assert.False(UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.Verify(
            UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.SpreadsheetFormulaFamily, p, "17"));
    }

    [Fact]
    public void RelativeFillOracleUsesPreviousRowReference()
    {
        var p = new Dictionary<string, int>
        {
            ["start"] = 18, ["step"] = 12, ["row"] = 7, ["variant"] = 1
        };
        Assert.Equal("90", UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.Solve(
            UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.SpreadsheetRecursiveFillFamily, p));
        Assert.True(UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.Verify(
            UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.SpreadsheetRecursiveFillFamily, p, "90"));
    }

    [Fact]
    public void HistogramOracleRespectsHalfOpenRightEndpoint()
    {
        var p = new Dictionary<string, int>
        {
            ["start"] = 5, ["width"] = 2, ["c0"] = 4, ["c1"] = 3,
            ["c2"] = 1, ["target"] = 1, ["variant"] = 0
        };
        Assert.Equal("3", UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.Solve(
            UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.HistogramBinFamily, p));
        Assert.True(UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.Verify(
            UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.HistogramBinFamily, p, "3"));
    }

    [Fact]
    public void ExtremeReplacementOracleUpdatesMeanWithoutInventingMedianChange()
    {
        var p = new Dictionary<string, int>
        {
            ["center"] = 6, ["delta"] = 30, ["variant"] = 0
        };
        Assert.Equal("9.5", UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.Solve(
            UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.ExtremeMeanReplacementFamily, p));
        Assert.True(UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.Verify(
            UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.ExtremeMeanReplacementFamily, p, "19/2"));
    }

    [Fact]
    public void PopulationSdOracleUsesNNotNMinusOne()
    {
        var p = new Dictionary<string, int>
        {
            ["center"] = 5, ["spread"] = 3, ["variant"] = 0
        };
        Assert.Equal("3", UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.Solve(
            UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.PopulationSdFamily, p));
        Assert.True(UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.Verify(
            UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.PopulationSdFamily, p, "3"));
    }

    [Fact]
    public void OutlierFenceOracleIncludesBothFencesAndClassifiesLargestValue()
    {
        var lower = new Dictionary<string, int>
        {
            ["center"] = 11, ["delta"] = 15, ["side"] = 0, ["variant"] = 0
        };
        var upper = new Dictionary<string, int>(lower) { ["side"] = 1 };
        Assert.Equal("5.5", UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.Solve(
            UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.OutlierFenceFamily, lower));
        Assert.Equal("17.5", UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.Solve(
            UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.OutlierFenceFamily, upper));
        Assert.True(UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.Verify(
            UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.OutlierFlagFamily, upper, "yes"));

        var notFlagged = new Dictionary<string, int>(upper) { ["delta"] = 6 };
        Assert.True(UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.Verify(
            UsCcssAlg1Unit1StatisticsSpreadsheetQuestions.OutlierFlagFamily, notFlagged, "no"));
    }
}
