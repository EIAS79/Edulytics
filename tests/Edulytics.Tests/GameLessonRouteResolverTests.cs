using Edulytics.Web.GameRouting;

namespace Edulytics.Tests;

public sealed class GameLessonRouteResolverTests
{
    [Theory]
    [InlineData("Classify shapes", "GEOMETRY", "SHAPE_IDENTIFY_COMPARE")]
    [InlineData("Compare angles", "GEOMETRY", "ANGLE_LAB")]
    [InlineData("Read a bar chart", "DATA_STATISTICS", "BAR_CHART_BUILD")]
    [InlineData("Organise data in a table", "DATA_STATISTICS", "TABLE_BUILD")]
    public void UaeGeometryAndDataUsesLessonTitleBeforeBroadDataMatch(
        string lessonTitle,
        string expectedWorkspace,
        string expectedMechanic)
    {
        var route = GameLessonRouteResolver.Resolve(
            "PED:UAE-MOE-MATH:L3:COMMON:TEST",
            "Geometry and Data",
            lessonTitle);

        Assert.True(route.IsPlayable);
        Assert.Equal("ar", route.Locale);
        Assert.Equal(expectedWorkspace, route.Workspace);
        Assert.Equal(expectedMechanic, route.Mechanic);
        Assert.Equal(GameLessonRouter.UniversalRendererKey, route.RendererKey);
    }
}
