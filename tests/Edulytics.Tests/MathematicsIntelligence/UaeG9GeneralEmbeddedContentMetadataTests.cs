using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UaeG9GeneralEmbeddedContentMetadataTests
{
    private const string Prefix = "PED:UAE-MOE-MATH:L9:GENERAL:";
    private const string First = Prefix + "01:01:VARIABLES-AND-EXPRESSIONS";

    [Fact]
    public void Owner_approved_G9_first_party_82_lesson_pack_is_embedded_without_fake_publisher_parity()
    {
        var documents = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        var document = Assert.Single(documents.Where(d =>
            d.Lessons.Any(x =>
                x.LessonCode.StartsWith(Prefix, StringComparison.Ordinal))));
        Assert.Equal("UAE-MOE-MATH", document.PackCode);
        Assert.Equal("ar", document.AcademicLanguage);
        Assert.Equal("MOE-2026-2027-T1", document.VersionCode);
        Assert.Equal("Published", document.Status.ToString());
        Assert.Equal(82, document.Lessons.Count);
        Assert.Equal(82, document.Lessons
            .Select(x => x.LessonCode)
            .Distinct(StringComparer.Ordinal).Count());
        Assert.All(document.Lessons, lesson =>
        {
            Assert.StartsWith(Prefix, lesson.LessonCode);
            Assert.Contains(lesson.Translations, t => t.CultureCode == "ar");
        });
        // The 82 original Arabic RichV2 files now have matching runtime schema
        // and full explanation, key concept, worked example and mistake fields.
        // This is NOT a publisher literal-parity or automatic Practice certificate.
        Assert.NotNull(RichLessonContentV2Registry.Find(First, "ar"));
        var richDocuments = RichLessonContentV2Registry.AllDocuments
            .Where(d => d.Lessons.Any(x =>
                x.LessonCode.StartsWith(Prefix, StringComparison.Ordinal)))
            .ToArray();
        Assert.Equal(82, richDocuments.Length);
        Assert.All(richDocuments, rich =>
        {
            Assert.Equal(1, rich.SchemaVersion);
            Assert.Equal("UAE-MOE-MATH", rich.PackCode);
            Assert.Single(rich.Lessons);
        });
        var richLessons = richDocuments.SelectMany(d => d.Lessons).ToArray();
        Assert.Equal(82, richLessons.Select(x => x.LessonCode)
            .Distinct(StringComparer.Ordinal).Count());
        Assert.Equal(document.Lessons.Select(x => x.LessonCode).Order(),
            richLessons.Select(x => x.LessonCode).Order());
        Assert.All(richLessons, lesson =>
        {
            Assert.Equal("ar", lesson.CultureCode);
            Assert.True(lesson.ExplanationParagraphs.Count >= 2);
            Assert.True(lesson.KeyConcepts.Count >= 3);
            Assert.True(lesson.WorkedExamples.Count >= 3);
            Assert.True(lesson.CommonMistakes.Count >= 2);
            Assert.True(lesson.SummaryPoints.Count >= 3);
            Assert.All(lesson.WorkedExamples, example =>
            {
                Assert.True(example.Steps.Count >= 3);
                Assert.False(string.IsNullOrWhiteSpace(example.Method));
            });
        });
    }

    [Fact]
    public void Draft_grade9_practice_contracts_are_not_exposed_as_verified_runtime_capabilities()
    {
        Assert.All(LessonPracticeContractRegistry.All, contract =>
            Assert.Equal("READY_VERIFIED", contract.Readiness));
        Assert.DoesNotContain(LessonPracticeContractRegistry.All,
            contract => contract.LessonCode.StartsWith(Prefix, StringComparison.Ordinal));
        Assert.False(LessonPracticeContractRegistry.TryResolve(
            "PED:UAE-MOE-MATH:L9:GENERAL:13:09:PARALLELOGRAM-TRIANGLE-AREA",
            out _));
    }

    [Fact]
    public void Arabic_coordinate_graph_parser_preserves_two_lines_and_intersection_without_editing_future_lessons()
    {
        var equations = RichLessonCoordinateDiagram.ParseLines(
            "خطا ص=−٣س+١٠ وص=س−٢؛ التقاطع عند (٣،١).");
        Assert.Equal(2, equations.Count);
        Assert.Equal(-3d, equations[0].Slope);
        Assert.Equal(10d, equations[0].Intercept);
        Assert.Equal(1d, equations[1].Slope);
        Assert.Equal(-2d, equations[1].Intercept);
        Assert.Equal(1d, 3d * equations[0].Slope + equations[0].Intercept);
        Assert.Equal(1d, 3d * equations[1].Slope + equations[1].Intercept);

        var points = RichLessonCoordinateDiagram.ParsePoints(["(٣،١)"]);
        var intersection = Assert.Single(points);
        Assert.Equal(3d, intersection.X);
        Assert.Equal(1d, intersection.Y);
    }

    [Fact]
    public void Signed_inequality_boundary_is_dashed_and_student_test_point_is_below_it()
    {
        var boundary = Assert.Single(RichLessonCoordinateDiagram.ParseLines(
            "ص<٣س−٢، خط متقطع يمر (٠،−٢) و(١،١)، ويظلّل أسفله."));
        Assert.Equal(3d, boundary.Slope);
        Assert.Equal(-2d, boundary.Intercept);
        Assert.True(boundary.IsDashed);
        Assert.True(boundary.ShadeBelow);
        Assert.False(boundary.ShadeAbove);
        var points = RichLessonCoordinateDiagram.ParsePoints(
            ["(٠،−٢)", "(١،١)", "(٢،٢) نقطة تحقق"]);
        Assert.Equal(3, points.Count);
        Assert.True(points[2].Y < boundary.Slope * points[2].X + boundary.Intercept);
        Assert.Empty(RichLessonCoordinateDiagram.ParseLines("تعليمات بدون معادلة"));
        Assert.Empty(RichLessonCoordinateDiagram.ParsePoints(["بيانات وصفية غير رقمية"]));
    }
}
