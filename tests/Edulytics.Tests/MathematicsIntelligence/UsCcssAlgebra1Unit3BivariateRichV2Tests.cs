using Edulytics.Core.Curriculum;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Independent evidence for source-specific first-party Algebra 1 Unit 3
/// bivariate and categorical statistics candidates. No authentic learner,
/// publisher-artwork equivalence, school grading or acceptance is implied.
/// </summary>
public sealed class UsCcssAlgebra1Unit3BivariateRichV2Tests
{
    private const string Prefix = "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U03:";

    [Theory]
    [InlineData("L01", "Two-way Tables")]
    [InlineData("L02", "Relative Frequency Tables")]
    [InlineData("L03", "Associations in Categorical Data")]
    [InlineData("L04", "Linear Models")]
    [InlineData("L05", "Fitting Lines")]
    [InlineData("L06", "Residuals")]
    [InlineData("L07", "The Correlation Coefficient")]
    [InlineData("L08", "Using the Correlation Coefficient")]
    [InlineData("L09", "Causal Relationships")]
    [InlineData("L10", "Fossils and Flags")]
    public void ExactSourceAndFirstPartyRubricArePresent(string code, string title)
    {
        var lesson = RichLessonContentV2Registry.Find(Prefix + code, "en");
        Assert.NotNull(lesson);
        Assert.Equal(title, lesson.Title);
        var sourceN = int.Parse(code[1..]);
        Assert.Contains(lesson.ExplanationParagraphs, paragraph =>
            paragraph.Contains("/1/3/" + sourceN + "/preparation.html", StringComparison.Ordinal));
        Assert.True(lesson.KeyConcepts.Count >= 3);
        Assert.True(lesson.WorkedExamples.Count >= 3);
        Assert.All(lesson.WorkedExamples, example =>
        {
            Assert.True(example.Steps.Count >= 3);
            Assert.False(string.IsNullOrWhiteSpace(example.Answer));
            Assert.False(string.IsNullOrWhiteSpace(example.Check));
        });
        Assert.True(lesson.CommonMistakes.Count >= 2);
        Assert.True(lesson.SummaryPoints.Count >= 3);
        Assert.Contains("teacher", string.Join(" ", lesson.ExplanationParagraphs),
            StringComparison.OrdinalIgnoreCase);
        Assert.Contains("rubric", string.Join(" ", lesson.ExplanationParagraphs),
            StringComparison.OrdinalIgnoreCase);
        Assert.All(lesson.Visuals, visual => Assert.False(
            string.IsNullOrWhiteSpace(visual.Description)));
    }

    [Fact]
    public void Unit3HasTenDistinctSourceIdentities()
    {
        var entries = RichLessonContentV2Registry.AllDocuments
            .SelectMany(document => document.Lessons)
            .Where(lesson => lesson.LessonCode.StartsWith(Prefix, StringComparison.Ordinal))
            .ToArray();
        Assert.Equal(10, entries.Length);
        Assert.Equal(10, entries.Select(x => x.LessonCode).Distinct().Count());
    }

    [Fact]
    public void TwoWayTableAndConditionalDenominatorsAreDistinct()
    {
        int[,] counts = {{12, 8}, {9, 11}};
        Assert.Equal(20, counts[0,0] + counts[0,1]);
        Assert.Equal(20, counts[1,0] + counts[1,1]);
        Assert.Equal(21, counts[0,0] + counts[1,0]);
        Assert.Equal(19, counts[0,1] + counts[1,1]);
        Assert.Equal(40, counts.Cast<int>().Sum());
        Assert.Equal(.30, 12d / 40, 8);
        Assert.Equal(.60, 12d / 20, 8);
        Assert.Equal(12d / 21, 4d / 7, 8);
        Assert.Equal(.15, 12d / 20 - 9d / 20, 8);
    }

    [Fact]
    public void HandFitModelResidualAndInterpolationAreCorrect()
    {
        int[] x = [0, 1, 2, 3];
        int[] y = [2, 5, 8, 12];
        var predicted = x.Select(n => 3 * n + 2).ToArray();
        Assert.Equal([2, 5, 8, 11], predicted);
        Assert.Equal(1, y[^1] - predicted[^1]);
        Assert.Equal(6.5, 3 * 1.5 + 2);
        Assert.Equal(17, 3 * 5 + 2);
    }

    [Fact]
    public void RegressionCoefficientsResidualsAndSumOfSquaresAreIndependent()
    {
        double[] x = [0, 1, 2, 3];
        double[] y = [2, 4, 7, 9];
        var mx = x.Average();
        var my = y.Average();
        var sxy = x.Zip(y, (a, b) => (a - mx) * (b - my)).Sum();
        var sxx = x.Sum(a => Math.Pow(a - mx, 2));
        var syy = y.Sum(b => Math.Pow(b - my, 2));
        Assert.Equal(12, sxy, 8);
        Assert.Equal(5, sxx, 8);
        Assert.Equal(29, syy, 8);
        var slope = sxy / sxx;
        var intercept = my - slope * mx;
        Assert.Equal(2.4, slope, 8);
        Assert.Equal(1.9, intercept, 8);
        var residual = x.Zip(y, (a,b) => b - (slope * a + intercept)).ToArray();
        Assert.Equal(0, residual.Sum(), 8);
        Assert.Equal(0.2, residual.Sum(e => e * e), 8);
        Assert.Equal(12 / Math.Sqrt(145d), sxy / Math.Sqrt(sxx * syy), 8);
    }

    [Fact]
    public void CurvatureCanHaveZeroLinearPearsonCorrelation()
    {
        double[] x = [-2, -1, 0, 1, 2];
        double[] y = [4, 1, 0, 1, 4];
        var mx = x.Average();
        var my = y.Average();
        var covarianceNumerator = x.Zip(y, (a,b) => (a-mx)*(b-my)).Sum();
        Assert.Equal(0, covarianceNumerator, 8);
        Assert.True(x.Sum(a => Math.Pow(a - mx, 2)) > 0);
        Assert.True(y.Sum(b => Math.Pow(b - my, 2)) > 0);
        Assert.All(Enumerable.Range(0, x.Length),
            i => Assert.Equal(x[i]*x[i], y[i]));
    }

    [Fact]
    public void PerfectCorrelationsDoNotDefineCausalityOrSlope()
    {
        double[] x = [1,2,3,4];
        double[] positiveY = [2,4,6,8];
        double[] negativeY = [8,6,4,2];
        static double Correlation(double[] a, double[] b)
        {
            var aa = a.Average();
            var bb = b.Average();
            var xy = a.Zip(b,(x,y) => (x-aa)*(y-bb)).Sum();
            var xx = a.Sum(x => Math.Pow(x-aa,2));
            var yy = b.Sum(y => Math.Pow(y-bb,2));
            return xy / Math.Sqrt(xx * yy);
        }
        Assert.Equal(1, Correlation(x, positiveY), 8);
        Assert.Equal(-1, Correlation(x, negativeY), 8);
        Assert.True(double.IsNaN(Correlation([3,3,3,3], positiveY)));
    }

    [Fact]
    public void ConstructedFlagsAndFossilExamplesAreClearlyNotRealResearch()
    {
        int[] widths = [10,15,20,25];
        int[] heights = [6,9,12,15];
        for (int i=0;i<widths.Length;i++)
            Assert.Equal(.6, (double)heights[i]/widths[i], 8);
        Assert.Equal(1, 18 - (3*5+2));
        var rich = RichLessonContentV2Registry.Find(Prefix+"L10","en");
        Assert.NotNull(rich);
        var text = string.Join(" ", rich.ExplanationParagraphs);
        Assert.Contains("hypothetical",text,StringComparison.OrdinalIgnoreCase);
        Assert.Contains("teacher",text,StringComparison.OrdinalIgnoreCase);
        Assert.Contains("not",text,StringComparison.OrdinalIgnoreCase);
    }
}
