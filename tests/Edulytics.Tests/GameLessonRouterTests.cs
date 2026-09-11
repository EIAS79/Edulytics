using Edulytics.Web.GameRouting;

namespace Edulytics.Tests;

public sealed class GameLessonRouterTests
{
    [Theory]
    [InlineData(GameLessonRouter.CountTouchLessonCode, "Number Sense and Sequences", "Count, Touch, and Check", "NUMBER_WORLD", "COUNT_TOUCH", "count-touch")]
    [InlineData(GameLessonRouter.AngleExplorerLessonCode, "Geometry and Measure", "Compare, estimate, measure and draw angles: Build the Idea", "GEOMETRY_CANVAS", "ANGLE_EXPLORE", "angle-explorer")]
    [InlineData(GameLessonRouter.FractionForgeLessonCode, "Fractions and decimals", "Build the Idea — Fractions and decimals", "FRACTION_STUDIO", "FRACTION_EQUIVALENCE", "fraction-forge")]
    public void ExactImplementedLessonsArePlayable(
        string lessonCode,
        string unitTitle,
        string lessonTitle,
        string workspace,
        string mechanic,
        string renderer)
    {
        var route = GameLessonRouter.Resolve(lessonCode, unitTitle, lessonTitle);

        Assert.True(route.IsPlayable);
        Assert.Equal(workspace, route.Workspace);
        Assert.Equal(mechanic, route.Mechanic);
        Assert.Equal(renderer, route.RendererKey);
    }

    [Theory]
    [InlineData("PED:US-CCSS-MATH:G5:U07:L03", "Geometry", "Classify triangles and angles", "GEOMETRY_CANVAS", "en")]
    [InlineData("PED:PL-NATIONAL-MATH:L5:U07:L03", "Ułamki zwykłe", "Porównywanie ułamków", "FRACTION_STUDIO", "pl")]
    [InlineData("PED:UAE-MOE-MATH:L5:COMMON:08:03:PERIMETER", "Measurement", "Perimeter of rectangles", "MEASUREMENT_LAB", "ar")]
    [InlineData("PED:UK-NC-ENG-MATH:L5:YEAR-5:GEOMETRY:ANGLES", "Geometry", "Angles", "GEOMETRY_CANVAS", "en")]
    [InlineData("PED:CAMBRIDGE-INTL-MATH:S6:6N-2:BUILD", "Fractions and percentages", "Fractions", "FRACTION_STUDIO", "en")]
    public void SupportedCurriculaAreClassifiedWithoutPretendingRendererCoverage(
        string lessonCode,
        string unitTitle,
        string lessonTitle,
        string workspace,
        string locale)
    {
        var route = GameLessonRouter.Resolve(lessonCode, unitTitle, lessonTitle);

        Assert.False(route.IsPlayable);
        Assert.Null(route.RendererKey);
        Assert.Equal(workspace, route.Workspace);
        Assert.Equal(locale, route.Locale);
        Assert.NotEqual("NEEDS_REVIEW", route.Workspace);
    }

    [Fact]
    public void AmbiguousLessonIsMarkedNeedsReviewInsteadOfGenericQuiz()
    {
        var route = GameLessonRouter.Resolve(
            "PED:US-CCSS-MATH:G5:U99:L99",
            "Mixed review",
            "Center Day");

        Assert.False(route.IsPlayable);
        Assert.Equal("NEEDS_REVIEW", route.Workspace);
        Assert.Equal("NEEDS_REVIEW", route.Mechanic);
    }
}
