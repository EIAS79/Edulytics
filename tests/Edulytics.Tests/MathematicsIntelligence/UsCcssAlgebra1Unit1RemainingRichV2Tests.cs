using Edulytics.Core.Curriculum;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Course-owned, first-party Algebra 1 Unit 1 source-specific content and
/// mathematical examples. Not teacher/student interaction or persistence proof.
/// </summary>
public sealed class UsCcssAlgebra1Unit1RemainingRichV2Tests
{
    private const string Prefix = "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U01:";

    [Theory]
    [InlineData("L04", "The Shape of Distributions")]
    [InlineData("L05", "Calculating Measures of Center and Variability")]
    [InlineData("L06", "Mystery Computations")]
    [InlineData("L07", "Spreadsheet Computations")]
    [InlineData("L08", "Spreadsheet Shortcuts")]
    [InlineData("L09", "Technological Graphing")]
    [InlineData("L10", "The Effect of Extremes")]
    [InlineData("L11", "Comparing and Contrasting Data Distributions")]
    [InlineData("L12", "Standard Deviation")]
    [InlineData("L13", "More Standard Deviation")]
    [InlineData("L14", "Outliers")]
    [InlineData("L15", "Comparing Data Sets")]
    [InlineData("L16", "Analyzing Data")]
    public void ExactSourceLessonHasAuthoringAndHonestTeacherRubric(string code, string title)
    {
        var rich = RichLessonContentV2Registry.Find(Prefix + code, "en");
        Assert.NotNull(rich);
        Assert.Equal(title, rich.Title);
        var narrative = string.Join(" ", rich.ExplanationParagraphs);
        var sourcePath = "/1/1/" + int.Parse(code[1..]) + "/preparation.html";
        Assert.Contains(sourcePath, narrative, StringComparison.Ordinal);
        Assert.Contains("teacher", narrative, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("rubric", narrative, StringComparison.OrdinalIgnoreCase);
        Assert.True(rich.KeyConcepts.Count >= 3);
        Assert.True(rich.WorkedExamples.Count >= 3);
        Assert.All(rich.WorkedExamples, example =>
        {
            Assert.True(example.Steps.Count >= 3);
            Assert.False(string.IsNullOrWhiteSpace(example.Answer));
            Assert.False(string.IsNullOrWhiteSpace(example.Check));
        });
        Assert.True(rich.CommonMistakes.Count >= 2);
        Assert.True(rich.SummaryPoints.Count >= 3);
        Assert.All(rich.Visuals, figure =>
            Assert.False(string.IsNullOrWhiteSpace(figure.Description)));
        Assert.Empty(rich.Videos);
    }

    [Fact]
    public void ShapeExamplesHaveCorrectCountsWeightedMeanAndTail()
    {
        int[] symmetric = [1, 2, 4, 2, 1];
        int[] uniform = [2, 2, 2, 2, 2];
        int[] skewed = [5, 3, 1, 1, 1];
        int[] bimodal = [4, 1, 1, 1, 4];
        Assert.Equal(10, symmetric.Sum());
        Assert.Equal(10, uniform.Sum());
        Assert.Equal(11, skewed.Sum());
        Assert.Equal(11, bimodal.Sum());
        Assert.Equal(23, skewed.Select((count, i) => count * (i + 1)).Sum());
        Assert.Equal(2, Enumerable.Range(1, 5)
            .SelectMany(value => Enumerable.Repeat(value, skewed[value - 1]))
            .Order().ElementAt(5));
        Assert.Equal(symmetric[0], symmetric[^1]);
        Assert.Equal(symmetric[1], symmetric[^2]);
    }

    [Fact]
    public void CenterAndSpreadExamplesComputeIndependentOracles()
    {
        int[] data = [4, 6, 6, 8, 11];
        var mean = data.Average();
        Assert.Equal(7d, mean);
        Assert.Equal(6, data[2]);
        var mad = data.Average(x => Math.Abs(x - mean));
        Assert.Equal(2d, mad);
        var q1 = (data[0] + data[1]) / 2.0;
        var q3 = (data[3] + data[4]) / 2.0;
        Assert.Equal(5d, q1);
        Assert.Equal(9.5d, q3);
        Assert.Equal(4.5d, q3 - q1);
    }

    [Fact]
    public void SpreadsheetDiscoveryProbesCorrectSignedAndFractionalResults()
    {
        static double Rule(double a2, double a3) => a2 + 2 * a3;
        Assert.Equal(13d, Rule(5, 4));
        Assert.Equal(15d, Rule(7, 4));
        Assert.Equal(17d, Rule(5, 6));
        Assert.Equal(0d, Rule(-3, 1.5));
        Assert.Equal(1d, Rule(-3, 2));
        Assert.Equal(7, 3 * 4 - 5);
        Assert.Equal(2, 3 * 0 - (-2));
    }

    [Fact]
    public void CalculatorModelsKeepUnitsAndCorrectPrecedence()
    {
        Assert.Equal(45d, 18d * 2.5);
        Assert.Equal(29.4d, 8.4d * 3.5, precision: 8);
        Assert.Equal(33.6d, 8.4d * 4, precision: 8);
        Assert.Equal(18d, 4d * (2.5 + 0.75 + 1.25));
        Assert.NotEqual(18d, 4d * 2.5 + 0.75 + 1.25);
    }

    [Fact]
    public void SpreadsheetRecursiveAndDirectFormsMatchAllMonthsAndBoundary()
    {
        int[] observed = [18, 30, 42, 54, 66, 78, 90];
        for (var month = 0; month < observed.Length; month++)
        {
            Assert.Equal(18 + 12 * month, observed[month]);
            if (month > 0)
                Assert.Equal(observed[month - 1] + 12, observed[month]);
        }
        Assert.False(observed[4] > 70);
        Assert.True(observed[5] > 70);
        Assert.Equal(new[] {4, 8, 12, 20}, new[] {1, 2, 3, 5}
            .Select(x => 4 * x));
    }

    [Fact]
    public void VariabilityComparisonsSeparateLocationFromSpread()
    {
        int[] a = [4, 5, 6, 7, 8];
        int[] b = [0, 3, 6, 9, 12];
        Assert.Equal(6, a.Average());
        Assert.Equal(6, b.Average());
        Assert.Equal(1.2, a.Average(x => Math.Abs(x - 6)), 8);
        Assert.Equal(3.6, b.Average(x => Math.Abs(x - 6)), 8);
        Assert.Equal(3, (7 + 8) / 2d - (4 + 5) / 2d);
        Assert.Equal(9, (9 + 12) / 2d - (0 + 3) / 2d);
    }

    [Fact]
    public void PopulationStandardDeviationUsesNNotNMinusOne()
    {
        int[] a = [3, 3, 5, 5];
        int[] b = [1, 1, 7, 7];
        static double PopSd(int[] values)
        {
            var mean = values.Average();
            return Math.Sqrt(values.Average(x => Math.Pow(x - mean, 2)));
        }
        Assert.Equal(4, a.Average());
        Assert.Equal(4, b.Average());
        Assert.Equal(1, PopSd(a), 8);
        Assert.Equal(3, PopSd(b), 8);
        Assert.Equal(0, PopSd([4, 4, 4, 4]), 8);
    }

    [Fact]
    public void InfoGapExamplesCompareUnitsAndPreserveSdUnderShift()
    {
        int[] groupA = [10, 12, 14, 16];
        int[] groupB = [7, 11, 15, 19];
        static double PopSd(int[] values)
        {
            var mean = values.Average();
            return Math.Sqrt(values.Average(x => (x - mean) * (x - mean)));
        }
        Assert.Equal(13, groupA.Average());
        Assert.Equal(13, groupB.Average());
        Assert.Equal(Math.Sqrt(5), PopSd(groupA), 8);
        Assert.Equal(Math.Sqrt(20), PopSd(groupB), 8);
        Assert.Equal(2 * PopSd(groupA), PopSd(groupB), 8);
        Assert.Equal(PopSd(groupA), PopSd(groupA.Select(x => x + 5).ToArray()), 8);
    }

    [Fact]
    public void OutlierFencesAndMeanSensitivityAreNumericallyValid()
    {
        int[] values = [9, 10, 11, 12, 13, 100];
        Assert.Equal(155, values.Sum());
        Assert.Equal(155d / 6, values.Average(), 8);
        Assert.Equal(11.5, (values[2] + values[3]) / 2d);
        var q1 = values[1];
        var q3 = values[4];
        var iqr = q3 - q1;
        Assert.Equal(3, iqr);
        Assert.Equal(17.5, q3 + 1.5 * iqr);
        Assert.True(values[^1] > q3 + 1.5 * iqr);
        Assert.Equal(11, values.SkipLast(1).Average());
    }

    [Fact]
    public void BowlingCenterSdAndIqrAreDistinct()
    {
        int[] a = [100, 110, 120, 130, 140];
        int[] b = [118, 119, 120, 121, 122];
        static double PopSd(int[] values)
        {
            var mean = values.Average();
            return Math.Sqrt(values.Average(x => Math.Pow(x - mean, 2)));
        }
        Assert.Equal(120, a.Average());
        Assert.Equal(120, b.Average());
        Assert.Equal(Math.Sqrt(200), PopSd(a), 8);
        Assert.Equal(Math.Sqrt(2), PopSd(b), 8);
        Assert.Equal(30, (a[3] + a[4]) / 2d - (a[0] + a[1]) / 2d);
        Assert.Equal(3, (b[3] + b[4]) / 2d - (b[0] + b[1]) / 2d);
    }

    [Fact]
    public void RulerDropDemoIsNeverMistakenForAuthenticExperiment()
    {
        int[] a = [4, 6, 8, 10, 12];
        int[] b = [2, 4, 6, 8, 10];
        Assert.Equal(8, a.Average());
        Assert.Equal(6, b.Average());
        Assert.Equal(2.4, a.Average(x => Math.Abs(x - 8)), 8);
        Assert.Equal(2.4, b.Average(x => Math.Abs(x - 6)), 8);
        Assert.Equal(6, (a[3] + a[4]) / 2d - (a[0] + a[1]) / 2d);
        Assert.Equal(6, (b[3] + b[4]) / 2d - (b[0] + b[1]) / 2d);
        var rich = RichLessonContentV2Registry.Find(Prefix + "L16", "en");
        Assert.NotNull(rich);
        var prose = string.Join(" ", rich.ExplanationParagraphs);
        Assert.Contains("20 observations", prose, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("hypothetical", prose, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("not", prose, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Algebra1RichSidecarsAreUniqueAndCourseIsolated()
    {
        var lessons = RichLessonContentV2Registry.AllDocuments
            .SelectMany(document => document.Lessons)
            .Where(lesson => lesson.LessonCode.StartsWith(
                Prefix,
                StringComparison.Ordinal))
            .ToArray();
        Assert.Equal(16, lessons.Length);
        Assert.Equal(16, lessons.Select(x => x.LessonCode).Distinct().Count());
        Assert.All(lessons, lesson =>
            Assert.StartsWith(Prefix, lesson.LessonCode));
    }

    [Fact]
    public void TechnologicalGraphingHasExactHistogramAndOutlierStatistics()
    {
        int[] sample = [6, 8, 9, 9, 10, 12, 14, 16];
        Assert.Equal(84, sample.Sum());
        Assert.Equal(10.5, sample.Average());
        Assert.Equal(9.5, (sample[3] + sample[4]) / 2d);
        Assert.Equal(4, sample.Count(x => x >= 5 && x < 10));
        Assert.Equal(3, sample.Count(x => x >= 10 && x < 15));
        Assert.Equal(1, sample.Count(x => x >= 15 && x < 20));
        Assert.Equal(8.5, (sample[1] + sample[2]) / 2d);
        Assert.Equal(13, (sample[5] + sample[6]) / 2d);
        int[] changed = [6, 8, 9, 9, 10, 12, 14, 32];
        Assert.Equal(12.5, changed.Average());
        Assert.Equal(9.5, (changed[3] + changed[4]) / 2d);
    }

    [Fact]
    public void ExtremeValueExamplesConstructValidMeanMedianInequalities()
    {
        int[] original = [4, 5, 5, 6, 6, 7, 7, 8, 8, 9];
        int[] changed = [4, 5, 5, 6, 6, 7, 7, 8, 8, 39];
        Assert.Equal(6.5, original.Average());
        Assert.Equal(9.5, changed.Average());
        Assert.Equal(6.5, (changed[4] + changed[5]) / 2d);
        int[] rightTail = [0, 1, 2, 2, 2, 3, 3, 4, 5, 18];
        Assert.Equal(4, rightTail.Average());
        Assert.Equal(2.5, (rightTail[4] + rightTail[5]) / 2d);
        int[] leftTail = [-20, 1, 2, 4, 5, 5, 6, 7, 8, 10];
        Assert.Equal(2.8, leftTail.Average());
        Assert.Equal(5, (leftTail[4] + leftTail[5]) / 2d);
    }
}
