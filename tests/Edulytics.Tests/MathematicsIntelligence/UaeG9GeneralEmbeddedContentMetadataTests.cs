using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UaeG9GeneralEmbeddedContentMetadataTests
{
    private const string Prefix = "PED:UAE-MOE-MATH:L9:GENERAL:";
    private const string First = Prefix + "01:01:VARIABLES-AND-EXPRESSIONS";

    [Fact]
    public void Unverified_82_lesson_draft_is_not_implicitly_embedded_or_reseeded()
    {
        var documents = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        Assert.DoesNotContain(documents.SelectMany(d => d.Lessons),
            lesson => lesson.LessonCode.StartsWith(Prefix, StringComparison.Ordinal));
        Assert.Null(RichLessonContentV2Registry.Find(First, "ar"));
        Assert.DoesNotContain(RichLessonContentV2Registry.AllDocuments.SelectMany(d => d.Lessons),
            lesson => lesson.LessonCode.StartsWith(Prefix, StringComparison.Ordinal));
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
