namespace Edulytics.Web.GameRouting;

public sealed record GameLessonRoute(
    string FrameworkCode,
    string Workspace,
    string Mechanic,
    string? RendererKey,
    string Locale,
    bool IsPlayable,
    string ClassificationSource);

public sealed record StudentGameLaunchViewModel(
    Guid LessonId,
    string LessonCode,
    string Title,
    string UnitTitle,
    GameLessonRoute Route);

public static class GameLessonRouter
{
    public const string CountTouchLessonCode = "PED:CAMBRIDGE-INTL-MATH:S1:L01";
    public const string AngleExplorerLessonCode = "PED:CAMBRIDGE-INTL-MATH:S5:5G-1:BUILD";
    public const string FractionForgeLessonCode = "PED:CAMBRIDGE-INTL-MATH:S5:5N-2:BUILD";

    private static readonly string[] SupportedFrameworks =
    [
        "US-CCSS-MATH",
        "PL-NATIONAL-MATH",
        "UAE-MOE-MATH",
        "UK-NC-ENG-MATH",
        "CAMBRIDGE-INTL-MATH"
    ];

    public static GameLessonRoute Resolve(
        string lessonCode,
        string unitTitle,
        string lessonTitle)
    {
        var framework = FrameworkFromLessonCode(lessonCode);
        var locale = framework switch
        {
            "PL-NATIONAL-MATH" => "pl",
            "UAE-MOE-MATH" => "ar",
            _ => "en"
        };

        if (string.Equals(lessonCode, CountTouchLessonCode, StringComparison.Ordinal))
            return Playable(framework, "NUMBER_WORLD", "COUNT_TOUCH", "count-touch", locale, "exact-lesson");

        if (string.Equals(lessonCode, AngleExplorerLessonCode, StringComparison.Ordinal))
            return Playable(framework, "GEOMETRY_CANVAS", "ANGLE_EXPLORE", "angle-explorer", locale, "exact-lesson");

        if (string.Equals(lessonCode, FractionForgeLessonCode, StringComparison.Ordinal))
            return Playable(framework, "FRACTION_STUDIO", "FRACTION_EQUIVALENCE", "fraction-forge", locale, "exact-lesson");

        if (!SupportedFrameworks.Contains(framework, StringComparer.Ordinal))
            return Unsupported(framework, "NEEDS_REVIEW", "NEEDS_REVIEW", locale, "unsupported-framework");

        var text = Normalize($"{unitTitle} {lessonTitle} {lessonCode}");

        if (ContainsAny(text,
            "fraction", "fractions", "decimal", "decimals", "equivalent fraction",
            "ulamek", "ulamki", "ułamek", "ułamki", "كسر", "كسور"))
            return Unsupported(framework, "FRACTION_STUDIO", "FRACTION_EQUIVALENCE", locale, "taxonomy-keywords");

        if (ContainsAny(text,
            "perimeter", "area", "volume", "length", "mass", "capacity", "measure",
            "obwod", "obwód", "pole", "objetosc", "objętość", "miara", "pomiar",
            "محيط", "مساحة", "حجم", "قياس"))
            return Unsupported(framework, "MEASUREMENT_LAB", MeasurementMechanic(text), locale, "taxonomy-keywords");

        if (ContainsAny(text,
            "geometry", "geometric", "shape", "shapes", "angle", "angles", "triangle", "polygon",
            "position and direction", "symmetry", "transform", "coordinate",
            "geometr", "kat", "kąt", "kąty", "figura", "figury", "symetr",
            "هندس", "زاوي", "مثلث", "أشكال"))
            return Unsupported(framework, "GEOMETRY_CANVAS", GeometryMechanic(text), locale, "taxonomy-keywords");

        if (ContainsAny(text,
            "data", "graph", "chart", "table", "statistics", "probability",
            "dane", "wykres", "tabela", "statyst", "بيانات", "رسم بياني", "احتمال"))
            return Unsupported(framework, "DATA_LAB", "DATA_EXPLORE", locale, "taxonomy-keywords");

        if (ContainsAny(text,
            "time", "clock", "money", "coin", "currency", "calendar",
            "czas", "zegar", "pieniadz", "pieniądz", "moneta", "kalendarz",
            "وقت", "ساعة", "نقود", "مال"))
            return Unsupported(framework, "TIME_AND_MONEY", "TIME_MONEY_MODEL", locale, "taxonomy-keywords");

        if (ContainsAny(text,
            "pattern", "sequence", "algebra", "equation", "missing number",
            "wzor", "wzór", "ciag", "ciąg", "rownan", "równan",
            "نمط", "متتالية", "معادلة"))
            return Unsupported(framework, "PATTERN_MACHINE", "PATTERN_SEQUENCE", locale, "taxonomy-keywords");

        if (ContainsAny(text,
            "ratio", "proportion", "percent", "percentage",
            "proporcj", "procent", "نسبة", "مئوية"))
            return Unsupported(framework, "RATIO_PROPORTION", "RATIO_MODEL", locale, "taxonomy-keywords");

        if (ContainsAny(text,
            "multiply", "multiplication", "divide", "division", "add", "addition", "subtract", "subtraction",
            "mnozen", "mnożen", "dzielen", "dodaw", "odejm",
            "ضرب", "قسمة", "جمع", "طرح"))
            return Unsupported(framework, "OPERATIONS_ARENA", OperationsMechanic(text), locale, "taxonomy-keywords");

        if (ContainsAny(text,
            "number", "count", "place value", "round", "compare numbers", "order numbers",
            "liczb", "liczenie", "wartosc miejsc", "wartość miejsc",
            "عدد", "عد", "قيمة مكانية"))
            return Unsupported(framework, "NUMBER_WORLD", "NUMBER_SENSE", locale, "taxonomy-keywords");

        return Unsupported(framework, "NEEDS_REVIEW", "NEEDS_REVIEW", locale, "ambiguous-lesson");
    }

    public static bool IsSupportedFramework(string lessonCode) =>
        SupportedFrameworks.Contains(FrameworkFromLessonCode(lessonCode), StringComparer.Ordinal);

    private static GameLessonRoute Playable(
        string framework,
        string workspace,
        string mechanic,
        string renderer,
        string locale,
        string source) =>
        new(framework, workspace, mechanic, renderer, locale, true, source);

    private static GameLessonRoute Unsupported(
        string framework,
        string workspace,
        string mechanic,
        string locale,
        string source) =>
        new(framework, workspace, mechanic, null, locale, false, source);

    private static string FrameworkFromLessonCode(string lessonCode)
    {
        if (string.IsNullOrWhiteSpace(lessonCode) || !lessonCode.StartsWith("PED:", StringComparison.Ordinal))
            return "UNKNOWN";

        foreach (var framework in SupportedFrameworks)
        {
            if (lessonCode.StartsWith($"PED:{framework}:", StringComparison.Ordinal))
                return framework;
        }

        return "UNKNOWN";
    }

    private static string GeometryMechanic(string text)
    {
        if (ContainsAny(text, "angle", "angles", "kat", "kąt", "kąty", "زاوي")) return "ANGLE_EXPLORE";
        if (ContainsAny(text, "symmetry", "transform", "symetr")) return "TRANSFORM_SYMMETRY";
        if (ContainsAny(text, "position", "direction", "coordinate")) return "POSITION_ROUTE";
        if (ContainsAny(text, "build", "compose", "net")) return "SHAPE_BUILD_COMPOSE";
        return "SHAPE_SORT_PROPERTIES";
    }

    private static string MeasurementMechanic(string text)
    {
        if (ContainsAny(text, "perimeter", "obwod", "obwód", "محيط")) return "PERIMETER_TRACE";
        if (ContainsAny(text, "area", "pole", "مساحة")) return "AREA_TILE";
        if (ContainsAny(text, "volume", "objetosc", "objętość", "حجم")) return "VOLUME_BUILD";
        return "MEASURE_COMPARE";
    }

    private static string OperationsMechanic(string text)
    {
        if (ContainsAny(text, "multiply", "multiplication", "mnozen", "mnożen", "ضرب")) return "ARRAYS_GROUPS";
        if (ContainsAny(text, "divide", "division", "dzielen", "قسمة")) return "SHARE_DIVIDE";
        if (ContainsAny(text, "subtract", "subtraction", "odejm", "طرح")) return "SEPARATE_GROUPS";
        return "JOIN_GROUPS";
    }

    private static bool ContainsAny(string text, params string[] needles) =>
        needles.Any(text.Contains);

    private static string Normalize(string value) =>
        value.Trim().ToLowerInvariant();
}
