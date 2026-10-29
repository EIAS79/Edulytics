using System.Globalization;
using System.Xml.Linq;
using Edulytics.Core.Curriculum;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Grade 7 owned geometry source/asset tests. These are NOT browser,
/// authenticated rendering, teacher scoring or Practice-engine tests.
/// </summary>
public sealed class USCCSSG7ScaleConstructionVisualTests
{
    private const string AssetRoot =
        "src/Edulytics.Web/wwwroot/curriculum/us-ccss-g7/geometry/";

    [Theory]
    [InlineData("PED:US-CCSS-MATH:G7:U01:L01", "u01-l01-scaled-vs-stretched.svg", 3)]
    [InlineData("PED:US-CCSS-MATH:G7:U01:L02", "u01-l02-corresponding-triangles.svg", 2)]
    [InlineData("PED:US-CCSS-MATH:G7:U01:L03", "u01-l03-scaled-l-construction.svg", 3)]
    [InlineData("PED:US-CCSS-MATH:G7:U01:L04", "u01-l04-hidden-distances-and-angles.svg", 2)]
    [InlineData("PED:US-CCSS-MATH:G7:U01:L05", "u01-l05-size-and-inverse-scale.svg", 3)]
    [InlineData("PED:US-CCSS-MATH:G7:U01:L06", "u01-l06-tiling-and-area-factors.svg", 3)]
    public void Grade7Diagram_IsOriginalAccessibleSvg_WithAnAuthoredLessonAndOptInComponent(
        string code, string file, int expectedFigures)
    {
        var repoRoot = FindRoot();
        var svgPath = Path.Combine(repoRoot, AssetRoot, file);
        Assert.True(File.Exists(svgPath), svgPath);
        var text = File.ReadAllText(svgPath);
        var xml = XDocument.Parse(text);
        XNamespace ns = "http://www.w3.org/2000/svg";

        Assert.Equal(ns + "svg", xml.Root?.Name);
        Assert.Contains(xml.Descendants(ns + "title"), e =>
            !string.IsNullOrWhiteSpace(e.Value));
        Assert.Contains(xml.Descendants(ns + "desc"), e =>
            e.Value.Length >= 80 &&
            e.Value.Contains("scale", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain("<script", text, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("<foreignObject", text, StringComparison.OrdinalIgnoreCase);
        Assert.Equal(expectedFigures, xml.Descendants(ns + "polygon").Count());

        // Figures must not extend outside their panels. This checks the
        // corrected triple-scale L layout as well as the earlier figures.
        var panels = xml.Descendants(ns + "rect")
            .Where(e => (string?)e.Attribute("class") == "panel")
            .ToArray();
        var polygons = xml.Descendants(ns + "polygon")
            .Select(ReadPolygon)
            .ToArray();
        Assert.Equal(expectedFigures, panels.Length);
        for (var i = 0; i < polygons.Length; i++)
        {
            var left = Attr(panels[i], "x");
            var top = Attr(panels[i], "y");
            var right = left + Attr(panels[i], "width");
            var bottom = top + Attr(panels[i], "height");
            Assert.All(polygons[i], point =>
            {
                Assert.InRange(point.X, left, right);
                Assert.InRange(point.Y, top, bottom);
            });
        }

        var richLesson = Assert.Single(
            RichLessonContentV2Registry.AllDocuments
                .SelectMany(d => d.Lessons),
            l => l.LessonCode == code);
        Assert.True(richLesson.WorkedExamples.Count >= 4);
        Assert.NotEmpty(richLesson.Visuals);

        var component = File.ReadAllText(Path.Combine(
            repoRoot,
            "src/Edulytics.Web/Views/Shared/_USCCSSG7ScaleConstructionFigures.cshtml"));
        Assert.Contains(code, component, StringComparison.Ordinal);
        Assert.Contains("/curriculum/us-ccss-g7/geometry/" + file, component, StringComparison.Ordinal);
        Assert.Contains("asp-append-version", component, StringComparison.Ordinal);
        Assert.Contains("alt=\"@altText\"", component, StringComparison.Ordinal);
        Assert.Contains("teacher", component, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ScaledF_DoublesBothAxes_AndRejectsHorizontalOnlyDistortion()
    {
        var shapes = LoadPolygons("u01-l01-scaled-vs-stretched.svg");
        Assert.Equal(3, shapes.Length);
        AssertUniformScale(shapes[0], shapes[1], 2);
        AssertAxisScale(shapes[0], shapes[2], 2, 1);
        Assert.False(IsUniformScale(shapes[0], shapes[2], 2));
    }

    [Fact]
    public void MatchedRightTriangles_HaveCommonScaleThreeHalves()
    {
        var shapes = LoadPolygons("u01-l02-corresponding-triangles.svg");
        Assert.Equal(2, shapes.Length);
        AssertUniformScale(shapes[0], shapes[1], 1.5);
        Assert.Equal(3, shapes[0].Length);
    }

    [Fact]
    public void AllSixLVertices_ScaleByHalfAndThree_WithOneSharedDrawingUnit()
    {
        var shapes = LoadPolygons("u01-l03-scaled-l-construction.svg");
        Assert.Equal(3, shapes.Length);
        AssertUniformScale(shapes[0], shapes[1], .5);
        AssertUniformScale(shapes[0], shapes[2], 3);
        Assert.Equal(6, shapes[0].Length);
    }

    [Fact]
    public void HiddenDistances_AlsoScale_AndCorrespondingRightAnglesAreUnchanged()
    {
        var shapes = LoadPolygons("u01-l04-hidden-distances-and-angles.svg");
        Assert.Equal(2, shapes.Length);
        AssertUniformScale(shapes[0], shapes[1], 2);
        static double Distance((double X, double Y) a, (double X, double Y) b)
            => Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        var sourceDiagonal = Distance(shapes[0][0], shapes[0][2]);
        var copiedDiagonal = Distance(shapes[1][0], shapes[1][2]);
        Assert.Equal(5 * 20, sourceDiagonal, 8);
        Assert.Equal(10 * 20, copiedDiagonal, 8);
        Assert.Equal(2, copiedDiagonal / sourceDiagonal, 8);
    }

    [Fact]
    public void InverseScaleDrawings_ShowReductionAndEnlargementAtOneDrawingUnit()
    {
        var shapes = LoadPolygons("u01-l05-size-and-inverse-scale.svg");
        Assert.Equal(3, shapes.Length);
        AssertUniformScale(shapes[1], shapes[0], .5);
        AssertUniformScale(shapes[1], shapes[2], 1.5);
        AssertUniformScale(shapes[0], shapes[1], 2);
        AssertUniformScale(shapes[2], shapes[1], 2d / 3d);
    }

    [Fact]
    public void TilingFigures_PreserveIdenticalUnitSize_AndAreaIsSquaredScale()
    {
        var shapes = LoadPolygons("u01-l06-tiling-and-area-factors.svg");
        Assert.Equal(3, shapes.Length);
        AssertUniformScale(shapes[0], shapes[1], 2);
        AssertUniformScale(shapes[0], shapes[2], 3);
        static double Area((double X, double Y)[] points)
            => (points.Max(p => p.X) - points.Min(p => p.X)) *
               (points.Max(p => p.Y) - points.Min(p => p.Y));
        Assert.Equal(3 * 15 * 15, Area(shapes[0]), 8);
        Assert.Equal(12 * 15 * 15, Area(shapes[1]), 8);
        Assert.Equal(27 * 15 * 15, Area(shapes[2]), 8);
        Assert.Equal(4, Area(shapes[1]) / Area(shapes[0]), 8);
        Assert.Equal(9, Area(shapes[2]) / Area(shapes[0]), 8);
    }

    [Theory]
    [InlineData("u01-l01-scaled-vs-stretched.svg", "scale factor 2")]
    [InlineData("u01-l02-corresponding-triangles.svg", "scale factor 1.5")]
    [InlineData("u01-l03-scaled-l-construction.svg", "scale factor one half")]
    [InlineData("u01-l04-hidden-distances-and-angles.svg", "scale factor 2")]
    [InlineData("u01-l05-size-and-inverse-scale.svg", "scale factor three-halves")]
    [InlineData("u01-l06-tiling-and-area-factors.svg", "scale factor 2")]
    public void SvgDescriptions_DescribeTheActualMathematicalScaleFactor(
        string svgName, string expectedDescription)
    {
        var doc = XDocument.Load(Path.Combine(FindRoot(), AssetRoot, svgName));
        XNamespace ns = "http://www.w3.org/2000/svg";
        var desc = Assert.Single(doc.Descendants(ns + "desc")).Value;
        Assert.Contains(expectedDescription, desc, StringComparison.OrdinalIgnoreCase);
        Assert.True(desc.Length >= 80);
    }

    [Fact]
    public void SixGrade7ReferenceFigures_DoNotClaimStudentSubmissionOrTeacherScoring()
    {
        var component = File.ReadAllText(Path.Combine(
            FindRoot(),
            "src/Edulytics.Web/Views/Shared/_USCCSSG7ScaleConstructionFigures.cshtml"));
        Assert.Contains("This reference figure does not submit, score or save student work", component);
        Assert.Contains("A teacher must inspect", component);
        for (var lesson = 1; lesson <= 6; lesson++)
        {
            var code = "PED:US-CCSS-MATH:G7:U01:L" + lesson.ToString("00");
            Assert.Contains(code, component, StringComparison.Ordinal);
        }
        // This verifies Grade 7's optional partial, NOT an authenticated
        // view invocation or teacher-scored persistent submission.
    }

    private static (double X, double Y)[][] LoadPolygons(string file)
    {
        var svg = XDocument.Load(Path.Combine(FindRoot(), AssetRoot, file));
        XNamespace ns = "http://www.w3.org/2000/svg";
        return svg.Descendants(ns + "polygon").Select(ReadPolygon).ToArray();
    }

    private static (double X, double Y)[] ReadPolygon(XElement polygon)
        => ((string)polygon.Attribute("points")!).Split(
                ' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(pair =>
            {
                var coords = pair.Split(',');
                Assert.Equal(2, coords.Length);
                return (
                    double.Parse(coords[0], CultureInfo.InvariantCulture),
                    double.Parse(coords[1], CultureInfo.InvariantCulture));
            })
            .ToArray();

    private static double Attr(XElement element, string name)
        => double.Parse((string)element.Attribute(name)!, CultureInfo.InvariantCulture);

    private static bool IsUniformScale(
        (double X, double Y)[] original,
        (double X, double Y)[] copy,
        double scale)
    {
        if (original.Length != copy.Length) return false;
        for (var i = 0; i < original.Length; i++)
        {
            var dx = copy[i].X - copy[0].X;
            var dy = copy[i].Y - copy[0].Y;
            if (Math.Abs(dx - scale * (original[i].X - original[0].X)) > 1e-8 ||
                Math.Abs(dy - scale * (original[i].Y - original[0].Y)) > 1e-8)
                return false;
        }
        return true;
    }

    private static void AssertUniformScale(
        (double X, double Y)[] original,
        (double X, double Y)[] copy,
        double scale)
        => Assert.True(IsUniformScale(original, copy, scale),
            "Coordinates do not show the claimed uniform scale " + scale);

    private static void AssertAxisScale(
        (double X, double Y)[] original,
        (double X, double Y)[] copy,
        double horizontal,
        double vertical)
    {
        Assert.Equal(original.Length, copy.Length);
        for (var i = 0; i < original.Length; i++)
        {
            Assert.True(Math.Abs(
                (copy[i].X - copy[0].X) -
                horizontal * (original[i].X - original[0].X)) < 1e-8);
            Assert.True(Math.Abs(
                (copy[i].Y - copy[0].Y) -
                vertical * (original[i].Y - original[0].Y)) < 1e-8);
        }
    }

    private static string FindRoot()
    {
        DirectoryInfo? dir = new(AppContext.BaseDirectory);
        while (dir is not null)
        {
            if (File.Exists(Path.Combine(dir.FullName, "Edulytics.sln")))
                return dir.FullName;
            dir = dir.Parent;
        }
        throw new DirectoryNotFoundException("Edulytics solution root not found.");
    }
}
