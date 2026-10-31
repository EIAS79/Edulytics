using System.Text.Json;
using System.Xml.Linq;
using Edulytics.Core.Curriculum;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Coder 5 V11: independent source-task, first-party graph file and draft
/// teacher-rubric evidence. Does NOT claim academic acceptance, teacher
/// submission persistence or authenticated school/lesson resolution.
/// </summary>
public sealed class UsCcssAlgebra1V11GraphAndTeacherRubricTests
{
    private const string Prefix = "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:";
    private static readonly string Root = FindRepositoryRoot();

    private static string FindRepositoryRoot()
    {
        for (var dir = new DirectoryInfo(AppContext.BaseDirectory);
             dir is not null; dir = dir.Parent)
        {
            if (File.Exists(Path.Combine(dir.FullName, "global.json")) &&
                Directory.Exists(Path.Combine(dir.FullName, "src", "Edulytics.Web")))
                return dir.FullName;
        }
        throw new InvalidOperationException("Repository root not found.");
    }

    [Theory]
    [InlineData("U06:L11", "u06-l11-factored-graph")]
    [InlineData("U06:L16", "u06-l16-vertex-graph")]
    [InlineData("U07:L24", "u07-l24-quadratic-line")]
    public void OriginalDiagramIsAccessibleSafeAndContainsTwoDistinctActualCurves(
        string suffix, string fileStem)
    {
        var path = Path.Combine(
            Root, "src", "Edulytics.Web", "wwwroot", "images",
            "curriculum", "alg1-v11", fileStem + ".svg");
        Assert.True(File.Exists(path), path);
        var svg = XDocument.Load(path);
        Assert.NotNull(svg.Root);
        XNamespace ns = "http://www.w3.org/2000/svg";
        Assert.Equal(ns + "svg", svg.Root.Name);
        Assert.NotEmpty(svg.Descendants(ns + "title"));
        Assert.NotEmpty(svg.Descendants(ns + "desc"));
        Assert.Equal(2, svg.Descendants(ns + "path").Count());
        Assert.True(svg.Descendants(ns + "circle").Count() >= 4);
        Assert.Empty(svg.Descendants(ns + "script"));
        Assert.Empty(svg.Descendants(ns + "foreignObject"));
        Assert.All(svg.Descendants(ns + "path"), pathEl =>
        {
            Assert.False(string.IsNullOrWhiteSpace(pathEl.Attribute("d")?.Value));
            Assert.Equal("none", pathEl.Attribute("fill")?.Value);
        });
        var lesson = RichLessonContentV2Registry.Find(Prefix + suffix, "en");
        Assert.NotNull(lesson);
        Assert.True(lesson.WorkedExamples.Count >= 3);
        Assert.Contains(lesson.ExplanationParagraphs,
            p => p.StartsWith("Student task", StringComparison.OrdinalIgnoreCase) ||
                 p.StartsWith("Student culminating task", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void GalleryRendersOnlyTheThreeApprovedCourseCodeRoutes()
    {
        var view = File.ReadAllText(Path.Combine(
            Root, "src", "Edulytics.Web", "Views", "Shared",
            "_UsCcssAlgebra1GraphGallery.cshtml"));
        var host = File.ReadAllText(Path.Combine(
            Root, "src", "Edulytics.Web", "Views", "Shared",
            "_RichLessonContentV2.cshtml"));
        Assert.Contains("U06:L11", view, StringComparison.Ordinal);
        Assert.Contains("U06:L16", view, StringComparison.Ordinal);
        Assert.Contains("U07:L24", view, StringComparison.Ordinal);
        Assert.Contains("Model.LessonCode switch", view, StringComparison.Ordinal);
        Assert.Contains("_UsCcssAlgebra1GraphGallery", host, StringComparison.Ordinal);
        Assert.Contains("loading=\"lazy\"", view, StringComparison.Ordinal);
        Assert.Contains("alt=\"@explanation\"", view, StringComparison.Ordinal);
        Assert.DoesNotContain("src=\"@Model", view, StringComparison.Ordinal);
    }

    [Fact]
    public void SourceSpecificGraphTaskInstructionsPreserveTeacherAndStudentGoals()
    {
        var factored = RichLessonContentV2Registry.Find(Prefix + "U06:L11", "en");
        var vertex = RichLessonContentV2Registry.Find(Prefix + "U06:L16", "en");
        var model = RichLessonContentV2Registry.Find(Prefix + "U07:L24", "en");
        Assert.NotNull(factored);
        Assert.NotNull(vertex);
        Assert.NotNull(model);
        var f = string.Join(" ", factored.ExplanationParagraphs);
        var v = string.Join(" ", vertex.ExplanationParagraphs);
        var m = string.Join(" ", model.ExplanationParagraphs);
        Assert.Contains("SAME labeled coordinate axes", f, StringComparison.Ordinal);
        Assert.Contains("graphing technology", f, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("/HS/students/1/6/11/index.html", f, StringComparison.Ordinal);
        Assert.Contains("do NOT formally assess", v, StringComparison.Ordinal);
        Assert.Contains("vertex (-2,6)", v, StringComparison.Ordinal);
        Assert.Contains("/HS/students/1/6/16/index.html", v, StringComparison.Ordinal);
        Assert.Contains("two substantial", m, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("gallery-walk", m, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("without horizontal-position data", m, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("/HS/students/1/7/24/index.html", m, StringComparison.Ordinal);
    }

    [Fact]
    public void NumericGraphModelsRespectZerosVerticesAndIntersectionCoordinates()
    {
        static double G(double x) => -2 * (x - 1) * (x - 5);
        static double H(double x) => 2 * (x - 1) * (x - 5);
        Assert.Equal(0, G(1));
        Assert.Equal(0, H(5));
        Assert.Equal(8, G(3));
        Assert.Equal(-8, H(3));
        Assert.Equal(-10, G(0));
        Assert.Equal(10, H(0));

        static double V(double x) => -.5 * Math.Pow(x + 2, 2) + 6;
        static double Q(double x) => .5 * Math.Pow(x + 2, 2) + 6;
        Assert.Equal(6, V(-2));
        Assert.Equal(6, Q(-2));
        Assert.Equal(4, V(0));
        Assert.Equal(8, Q(0));
        Assert.Equal(V(-4), V(0));
        Assert.Equal(Q(-4), Q(0));

        static double Flight(double t) => -t * t + 6 * t + 2;
        static double Platform(double t) => t + 6;
        Assert.Equal(7, Flight(1));
        Assert.Equal(7, Platform(1));
        Assert.Equal(10, Flight(4));
        Assert.Equal(10, Platform(4));
        Assert.Equal(11, Flight(3));
        Assert.Equal(0, Flight(3 + Math.Sqrt(11)), 8);
        Assert.Equal(8, Flight(3 - Math.Sqrt(3)), 8);
        Assert.Equal(8, Flight(3 + Math.Sqrt(3)), 8);
    }

    [Fact]
    public void ThreeRubricsAreHumanScoredDraftsWithExplicitIdentityAndNoFakeGrade()
    {
        var file = Path.Combine(
            Root, "docs", "curriculum",
            "US_CCSS_ALG1_CODER5_V11_TEACHER_RUBRIC_CANDIDATES_20261029.json");
        using var doc = JsonDocument.Parse(File.ReadAllText(file));
        var root = doc.RootElement;
        Assert.Equal(3, root.GetProperty("draftCount").GetInt32());
        Assert.Equal(0, root.GetProperty("approvedTeacherRubricCount").GetInt32());
        Assert.Equal(0, root.GetProperty("verifiedTeacherWorkflowCount").GetInt32());
        var rubrics = root.GetProperty("teacherRubricCandidates").EnumerateArray().ToArray();
        Assert.Equal(3, rubrics.Select(r => r.GetProperty("lessonCode").GetString())
            .Distinct(StringComparer.Ordinal).Count());
        foreach (var rubric in rubrics)
        {
            Assert.StartsWith(Prefix, rubric.GetProperty("lessonCode").GetString()!);
            Assert.False(rubric.GetProperty("teacherWorkflowVerified").GetBoolean());
            Assert.False(rubric.GetProperty("gradingVerified").GetBoolean());
            Assert.False(rubric.GetProperty("feedbackPersistenceVerified").GetBoolean());
            Assert.True(rubric.GetProperty("noFakeGrading").GetBoolean());
            Assert.Equal(8, rubric.GetProperty("totalPoints").GetInt32());
            var criteria = rubric.GetProperty("criteria").EnumerateArray().ToArray();
            Assert.Equal(4, criteria.Length);
            Assert.All(criteria, c =>
            {
                Assert.Equal(0, c.GetProperty("min").GetInt32());
                Assert.Equal(2, c.GetProperty("max").GetInt32());
                Assert.True(c.GetProperty("expectedEvidence").GetString()!.Length > 20);
            });
            Assert.True(rubric.GetProperty("requiredTeacherWorkflow").GetArrayLength() >= 4);
        }
    }

    [Fact]
    public void All132ExistingTeacherRubricNarrativesAreIndexedWithoutFabricatedScores()
    {
        var file = Path.Combine(Root, "docs", "curriculum",
            "US_CCSS_ALG1_CODER5_V11_ALL_TEACHER_TASK_INVENTORY_20261029.json");
        using var doc = JsonDocument.Parse(File.ReadAllText(file));
        var root = doc.RootElement;
        Assert.Equal(132, root.GetProperty("existingNarrativeRubrics").GetInt32());
        Assert.Equal(3, root.GetProperty("structuredSourceSpotCheckedRubricDrafts").GetInt32());
        Assert.Equal(0, root.GetProperty("teacherRubricsApproved").GetInt32());
        Assert.Equal(0, root.GetProperty("realTeacherScoresPersisted").GetInt32());
        var lessons = root.GetProperty("lessons").EnumerateArray().ToArray();
        Assert.Equal(132, lessons.Length);
        Assert.Equal(132, lessons.Select(x => x.GetProperty("lessonCode").GetString())
            .Distinct(StringComparer.Ordinal).Count());
        Assert.All(lessons, lesson =>
        {
            Assert.False(lesson.GetProperty("independentlyAcademicallyAccepted").GetBoolean());
            Assert.False(lesson.GetProperty("verifiedTeacherScore").GetBoolean());
            Assert.False(lesson.GetProperty("privateFeedbackPersistenceVerified").GetBoolean());
            Assert.True(lesson.GetProperty("existingNarrativeRubricDrafts").GetArrayLength() > 0);
        });
    }

    [Fact]
    public void All132CanonicalLessonsRemainAvailableAndUnduplicated()
    {
        var rows = RichLessonContentV2Registry.AllDocuments
            .SelectMany(d => d.Lessons)
            .Where(l => l.LessonCode.StartsWith(Prefix, StringComparison.Ordinal))
            .ToArray();
        Assert.Equal(132, rows.Length);
        Assert.Equal(132, rows.Select(x => x.LessonCode).Distinct(StringComparer.Ordinal).Count());
    }
}
