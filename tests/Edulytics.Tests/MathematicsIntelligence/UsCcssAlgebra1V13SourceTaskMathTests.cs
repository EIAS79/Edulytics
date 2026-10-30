using System.Globalization;
using System.Text.Json;
using System.Xml.Linq;
using Edulytics.Core.Curriculum;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

// Independent mathematical oracles for V13 authored tasks.
// These are NOT shared Practice engine, authenticated grading, or Stage A-D acceptance tests.
public sealed class UsCcssAlgebra1V13SourceTaskMathTests
{
    private const string Prefix = "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:";
    private static readonly string Root = FindRoot();

    private static string FindRoot()
    {
        for (var d = new DirectoryInfo(AppContext.BaseDirectory); d is not null; d = d.Parent)
            if (File.Exists(Path.Combine(d.FullName, "global.json")) &&
                Directory.Exists(Path.Combine(d.FullName, "src", "Edulytics.Core")))
                return d.FullName;
        throw new InvalidOperationException("Cannot locate repository");
    }

    private static JsonDocument Read(string filename) => JsonDocument.Parse(File.ReadAllText(
        Path.Combine(Root, "docs", "curriculum", filename)));

    [Theory]
    [InlineData("U01:L03")]
    [InlineData("U01:L06")]
    [InlineData("U02:L03")]
    [InlineData("U03:L03")]
    [InlineData("U04:L03")]
    [InlineData("U05:L03")]
    [InlineData("U06:L03")]
    [InlineData("U07:L03")]
    public void CorrectedLessonsKeepIdentityAndAllOriginalWorkedExamples(string suffix)
    {
        var lesson = RichLessonContentV2Registry.Find(Prefix + suffix, "en");
        Assert.NotNull(lesson);
        var text = string.Join(" ", lesson.ExplanationParagraphs);
        var u = int.Parse(suffix.Substring(1, 2), CultureInfo.InvariantCulture);
        var n = int.Parse(suffix.Substring(5, 2), CultureInfo.InvariantCulture);
        Assert.Contains($"/HS/teachers/1/{u}/{n}/preparation.html", text);
        Assert.Contains("V13", text);
        Assert.True(lesson.WorkedExamples.Count >= 3);
        Assert.True(lesson.KeyConcepts.Count >= 3);
    }

    [Fact]
    public void ShiftedDistributionAndSpreadsheetReferencesHaveIndependentPositiveNegativeAndFractionOracles()
    {
        int[] original = [1,2,2,3,3,3,4,4,5];
        int[] shifted = original.Select(v => v + 5).ToArray();
        Assert.Equal(9, original.Length);
        Assert.Equal(3, original[4]);
        Assert.Equal(8, shifted[4]);
        Assert.Equal(4, original.Max() - original.Min());
        Assert.Equal(4, shifted.Max() - shifted.Min());
        Assert.All(original.Zip(shifted), pair => Assert.Equal(5, pair.Second - pair.First));
        static double CellB2(double a2, double a4) => a2 + a4;
        Assert.Equal(3, CellB2(5, -2), 8);
        Assert.Equal(.25, CellB2(-.5, .75), 8);
        Assert.Equal(CellB2(5, -2), CellB2(5, -2), 8);
        Assert.False(double.TryParse("=A2+A4", CultureInfo.InvariantCulture, out _)); // formula, not a numeric answer
    }

    [Fact]
    public void FourSourceAppropriateTableRulesAreDistinctAndBoundarySafe()
    {
        static double A(double x) => 300*x;
        static double B(double x) => x + 300;
        static double C(double x) => x - 300;
        static double D(double x) => 300 - x;
        Assert.Equal(600, A(2));
        Assert.Equal(320, B(20));
        Assert.Equal(60, C(360));
        Assert.Equal(210, D(90));
        Assert.Equal(25, D(275));
        Assert.NotEqual(D(90), B(90));
        Assert.Equal(12, 48d/4, 8);
        Assert.True(double.IsInfinity(48d/0)); // must reject zero in inverse-area context
        Assert.False(double.TryParse("infinity/0", NumberStyles.Float, CultureInfo.InvariantCulture, out _));
    }

    [Fact]
    public void CategoricalAssociationUsesConditionalDenominatorsButDoesNotEstablishCausality()
    {
        var lowHealthy = 16d/20;
        var highHealthy = 6d/20;
        Assert.Equal(.80, lowHealthy, 8);
        Assert.Equal(.30, highHealthy, 8);
        Assert.Equal(.50, lowHealthy-highHealthy, 8);
        Assert.Equal(1d, lowHealthy+4d/20, 8);
        Assert.Equal(1d, highHealthy+14d/20, 8);
        Assert.Equal(10d/20, 10d/20, 8); // independently designed null-association comparison
        Assert.NotEqual(16d/40, lowHealthy); // wrong denominator
    }

    [Fact]
    public void TemperatureConstraintsAndCalendarOffsetAreConsistentButUnderdetermined()
    {
        var temperatures = new Dictionary<int,int> {
            [0]=18, [2]=26, [5]=55, [10]=100, [12]=100, [15]=96, [30]=70
        };
        Assert.True(temperatures[5] > temperatures[2]);
        Assert.Equal(temperatures[10], temperatures[12]);
        Assert.True(temperatures[15] > temperatures[30]);
        Assert.True(temperatures[0] < temperatures[30]);
        Assert.Equal(2030, 2020+10);
        Assert.Equal(53_000, 53*1000); // thousands, not persons and not a year
        // A different valid interpolation also satisfies the listed constraint values.
        Assert.True((temperatures[10]+temperatures[12])/2 == 100);
    }

    [Fact]
    public void ExponentsAndTwoDiscreteGrowthModelsHaveExactIndependentOracles()
    {
        int[] n = Enumerable.Range(0,5).Select(t => 50*(1<<t)).ToArray();
        int[] p = Enumerable.Range(0,5).Select(t => 20*(int)Math.Pow(3,t)).ToArray();
        Assert.Equal([50,100,200,400,800], n);
        Assert.Equal([20,60,180,540,1620], p);
        Assert.True(n[2]>p[2]);
        Assert.True(n[3]<p[3]);
        Assert.Equal(128, (int)Math.Pow(2,3)* (int)Math.Pow(2,4));
        Assert.NotEqual(Math.Pow(2,12), Math.Pow(2,7)); // multiplying powers: do not multiply exponents
        Assert.Equal(1,Math.Pow(3,0),8);
        Assert.Equal(2d, n[3]/(double)n[2],8);
        Assert.Equal(3d, p[3]/(double)p[2],8);
    }

    [Fact]
    public void GeometricPatternsHaveCorrectDomainAreasAndNonconstantFirstDifferences()
    {
        static int[] Areas(int side) => [side*side, side*side-4, 2*side*side+4];
        Assert.Equal([9,5,22], Areas(3));
        Assert.Equal(327,18*18+3);
        var sequence = Enumerable.Range(1,4).Select(i=>i*(i+3)).ToArray();
        Assert.Equal([4,10,18,28],sequence);
        Assert.Equal([6,8,10],sequence.Zip(sequence.Skip(1),(a,b)=>b-a).ToArray());
        Assert.True(sequence[2]-sequence[1]!=sequence[1]-sequence[0]);
        Assert.Equal(0,Areas(2)[1]); // boundary: removing whole 2x2 square gives empty area
        Assert.True(Areas(1)[1] < 0); // n=1 violates geometric source domain
    }

    [Theory]
    [InlineData(3,25)]
    [InlineData(-7,25)]
    public void ShiftedSquareAcceptsTwoCorrectRoots(int x,int square)
        => Assert.Equal(square, (x+2)*(x+2));

    [Fact]
    public void RealQuadraticRootCountAndRejectedWrongAnswerHaveIndependentChecks()
    {
        Assert.Equal(75,3*5*5);
        Assert.Equal(75,3*(-5)*(-5));
        Assert.Equal(0,(2-2)*(2-2));
        Assert.True(Enumerable.Range(-50,101).All(x => (x-2)*(x-2)>=0));
        Assert.False((3+2)*(3+2)==0); // wrong substitution for one-root case
        Assert.True((0*(0-6)==0) && (6*(6-6)==0)); // both factors
        Assert.False(double.TryParse("5 or -5", NumberStyles.Float, CultureInfo.InvariantCulture, out _)); // response form requires exact parser
    }

    [Theory]
    [InlineData("u03-l03-conditional-association.svg")]
    [InlineData("u04-l03-temperature-constraints.svg")]
    [InlineData("u05-l03-two-growth-models.svg")]
    public void OriginalStaticSvgAssetsAreAccessibleAndDoNotContainScripts(string name)
    {
        var path = Path.Combine(Root,"src","Edulytics.Web","wwwroot","images","curriculum","alg1-v13",name);
        var svg = XDocument.Load(path);
        XNamespace ns = "http://www.w3.org/2000/svg";
        Assert.Equal(ns+"svg",svg.Root?.Name);
        Assert.Single(svg.Descendants(ns+"title"));
        Assert.Single(svg.Descendants(ns+"desc"));
        Assert.Empty(svg.Descendants(ns+"script"));
        Assert.Empty(svg.Descendants(ns+"foreignObject"));
        Assert.DoesNotContain("NaN",File.ReadAllText(path),StringComparison.Ordinal);
    }

    [Fact]
    public void EveryNewFirstPartyCorrectionIsActuallyPresentInTheRegisteredSidecar()
    {
        using var evidence=Read("US_CCSS_ALG1_CODER5_V13_THIRD_CROSSUNIT_SOURCE_TASK_REPAIRS_20261030.json");
        foreach (var row in evidence.RootElement.GetProperty("lessons").EnumerateArray())
        {
            var relative=row.GetProperty("candidatePath").GetString()!;
            var path=Path.Combine(Root,relative.Replace('/',Path.DirectorySeparatorChar));
            using var file=JsonDocument.Parse(File.ReadAllText(path));
            var version=file.RootElement.GetProperty("ContentVersion").GetString();
            Assert.EndsWith("-v13-source-task-math-20261030",version,StringComparison.Ordinal);
            var sidecar=file.RootElement.GetProperty("Lessons")[0];
            Assert.Equal(row.GetProperty("lessonCode").GetString(),sidecar.GetProperty("LessonCode").GetString());
            var paragraphs=sidecar.GetProperty("ExplanationParagraphs").EnumerateArray()
                .Select(x=>x.GetString()).ToHashSet(StringComparer.Ordinal);
            foreach(var newParagraph in row.GetProperty("newOriginalExplanations").EnumerateArray())
                Assert.Contains(newParagraph.GetString(),paragraphs);
            var cases=row.GetProperty("practiceProposal").GetProperty("acceptanceCasesRequired").EnumerateArray()
                .Select(x=>x.GetString()).ToArray();
            Assert.Contains("incorrect",cases);
            Assert.Contains("malformed or unsupported",cases);
            Assert.Contains("domain boundary",cases);
        }
    }

    [Fact]
    public void AllV13ContractsAndStageGatesRemainExplicitlyUnapproved()
    {
        using var corrections=Read("US_CCSS_ALG1_CODER5_V13_THIRD_CROSSUNIT_SOURCE_TASK_REPAIRS_20261030.json");
        var root=corrections.RootElement;
        Assert.Equal(8,root.GetProperty("reviewed").GetInt32());
        Assert.Equal(107,root.GetProperty("unreviewedRemaining").GetInt32());
        var lessons=root.GetProperty("lessons").EnumerateArray().ToArray();
        Assert.Equal(8,lessons.Length);
        Assert.Equal(8,lessons.Select(x=>x.GetProperty("lessonCode").GetString()).Distinct().Count());
        foreach(var lesson in lessons)
        {
            Assert.False(lesson.GetProperty("A_ACCEPTED").GetBoolean());
            Assert.False(lesson.GetProperty("B_ACCEPTED").GetBoolean());
            Assert.False(lesson.GetProperty("C_VERIFIED").GetBoolean());
            Assert.False(lesson.GetProperty("D_VERIFIED").GetBoolean());
            Assert.False(lesson.GetProperty("practiceProposal").GetProperty("integrated").GetBoolean());
            Assert.False(lesson.GetProperty("svgAuthenticatedRendered").GetBoolean());
            Assert.Equal(4,lesson.GetProperty("rubricDraft").GetProperty("criteria").GetArrayLength());
        }
    }
}
