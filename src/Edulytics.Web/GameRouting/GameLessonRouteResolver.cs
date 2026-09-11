namespace Edulytics.Web.GameRouting;

/// <summary>
/// Application-facing lesson route resolver. It keeps the stable router contract
/// while applying corrections that must be evaluated before broad unit matches.
/// </summary>
public static class GameLessonRouteResolver
{
    public static GameLessonRoute Resolve(
        string lessonCode,
        string unitTitle,
        string lessonTitle)
    {
        if (IsUaeGeometryAndData(lessonCode, unitTitle))
        {
            var title = Normalize(lessonTitle);
            if (ContainsAny(title, "data", "graph", "chart", "table"))
            {
                return new GameLessonRoute(
                    "UAE-MOE-MATH",
                    "DATA_STATISTICS",
                    DataMechanic(title),
                    GameLessonRouter.UniversalRendererKey,
                    "ar",
                    true,
                    "uae-mixed-title-v2");
            }

            if (ContainsAny(title, "shape", "geometry", "angle", "position", "symmetry"))
            {
                return new GameLessonRoute(
                    "UAE-MOE-MATH",
                    "GEOMETRY",
                    GeometryMechanic(title),
                    GameLessonRouter.UniversalRendererKey,
                    "ar",
                    true,
                    "uae-mixed-title-v2");
            }

            return new GameLessonRoute(
                "UAE-MOE-MATH",
                "COMPOSITE_SESSION",
                "COMPOSITE_SESSION",
                GameLessonRouter.UniversalRendererKey,
                "ar",
                true,
                "uae-mixed-unit-v2");
        }

        return GameLessonRouter.Resolve(lessonCode, unitTitle, lessonTitle);
    }

    private static bool IsUaeGeometryAndData(string lessonCode, string unitTitle) =>
        lessonCode.Contains(":UAE-MOE-MATH:", StringComparison.OrdinalIgnoreCase) &&
        Normalize(unitTitle).Contains("geometry and data", StringComparison.Ordinal);

    private static string DataMechanic(string title)
    {
        if (ContainsAny(title, "bar chart", "bar graph")) return "BAR_CHART_BUILD";
        if (ContainsAny(title, "pictogram")) return "PICTOGRAM_BUILD";
        if (ContainsAny(title, "table")) return "TABLE_BUILD";
        if (ContainsAny(title, "mean", "median", "mode", "range")) return "STATISTIC_CALCULATE";
        if (ContainsAny(title, "sort", "categor")) return "DATA_SORT";
        return "PLOT_READ";
    }

    private static string GeometryMechanic(string title)
    {
        if (ContainsAny(title, "angle")) return "ANGLE_LAB";
        if (ContainsAny(title, "symmetry")) return "SYMMETRY_MIRROR";
        if (ContainsAny(title, "position", "direction")) return "POSITION_ROUTE";
        if (ContainsAny(title, "coordinate")) return "COORDINATE_SHAPE";
        return "SHAPE_IDENTIFY_COMPARE";
    }

    private static string Normalize(string value) =>
        (value ?? string.Empty).Trim().ToLowerInvariant();

    private static bool ContainsAny(string value, params string[] terms) =>
        terms.Any(term => value.Contains(term, StringComparison.Ordinal));
}
