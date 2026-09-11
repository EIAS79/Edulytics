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
    public const string UniversalRendererKey = "workspace-runtime";

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
            return Playable(framework, "OBJECT_COUNTING", "COUNT_TOUCH", "count-touch", locale, "exact-lesson");

        if (string.Equals(lessonCode, AngleExplorerLessonCode, StringComparison.Ordinal))
            return Playable(framework, "GEOMETRY", "ANGLE_LAB", "angle-explorer", locale, "exact-lesson");

        if (string.Equals(lessonCode, FractionForgeLessonCode, StringComparison.Ordinal))
            return Playable(framework, "FRACTION_DECIMAL_PERCENT", "FRACTION_EQUIVALENCE", "fraction-forge", locale, "exact-lesson");

        if (!SupportedFrameworks.Contains(framework, StringComparer.Ordinal))
            return Unsupported(framework, "NEEDS_REVIEW", "NEEDS_REVIEW", locale, "unsupported-framework");

        var route = framework switch
        {
            "US-CCSS-MATH" => ResolveUs(unitTitle, lessonTitle),
            "PL-NATIONAL-MATH" => ResolvePolish(lessonCode, unitTitle, lessonTitle),
            "UAE-MOE-MATH" => ResolveUae(unitTitle, lessonTitle),
            "UK-NC-ENG-MATH" => ResolveUk(unitTitle, lessonTitle),
            "CAMBRIDGE-INTL-MATH" => ResolveCambridge(unitTitle, lessonTitle),
            _ => ("NEEDS_REVIEW", "NEEDS_REVIEW", "unsupported-framework")
        };

        return Playable(
            framework,
            route.Item1,
            route.Item2,
            UniversalRendererKey,
            locale,
            route.Item3);
    }

    public static bool IsSupportedFramework(string lessonCode) =>
        SupportedFrameworks.Contains(FrameworkFromLessonCode(lessonCode), StringComparer.Ordinal);

    private static (string Workspace, string Mechanic, string Source) ResolveUs(
        string unitTitle,
        string lessonTitle)
    {
        var unit = Normalize(unitTitle);
        var title = Normalize(lessonTitle);

        if (ContainsAny(unit, "putting it all together"))
            return Composite("us-composite-unit");

        if (ContainsAny(unit, "adding, subtracting, and working with data"))
        {
            if (ContainsAny(title, "data", "graph", "chart", "table")) return Data("PLOT_READ", "us-mixed-title");
            if (ContainsAny(title, "add", "subtract", "sum", "difference")) return Operations("RELATIONSHIP_MODEL", "us-mixed-title");
            return Composite("us-mixed-unit");
        }

        if (ContainsAny(unit, "geometry and time", "geometry, time, and money"))
        {
            if (ContainsAny(title, "time", "clock", "hour", "minute", "money", "coin", "dollar", "cent"))
                return TimeMoney(TimeMoneyMechanic(title), "us-mixed-title");
            if (ContainsAny(title, "shape", "geometry", "partition", "rectangle", "triangle", "quadrilateral"))
                return Geometry(GeometryMechanic(title), "us-mixed-title");
            return Composite("us-mixed-unit");
        }

        if (ContainsAny(unit, "measuring length, time, liquid volume, and weight"))
        {
            if (ContainsAny(title, "time", "clock", "minute", "hour")) return TimeMoney("TIMELINE", "us-mixed-title");
            return Measurement(MeasurementMechanic(title), "us-mixed-unit");
        }

        if (ContainsAny(unit, "multiplicative comparison and measurement"))
        {
            if (ContainsAny(title, "measure", "unit", "length", "distance", "convert")) return Measurement(MeasurementMechanic(title), "us-mixed-title");
            return Operations("RELATIONSHIP_MODEL", "us-mixed-unit");
        }

        if (ContainsAny(unit, "place value patterns and decimal operations"))
        {
            if (ContainsAny(title, "decimal", "hundredth", "tenth", "multiply", "divide", "add", "subtract"))
                return Fractions("DECIMAL_OPERATIONS", "us-mixed-title");
            if (ContainsAny(title, "place value", "round", "power of 10", "power of ten"))
                return NumberSystem("PLACE_VALUE_SCALE", "us-mixed-title");
            return Composite("us-mixed-unit");
        }

        if (ContainsAny(unit, "data sets", "working with data")) return Data("PLOT_READ", "us-unit");
        if (ContainsAny(unit, "fraction", "decimal")) return Fractions(FractionMechanic(title), "us-unit");
        if (ContainsAny(unit, "expressions and equations")) return RatioAlgebra("BALANCE_EQUATION", "us-unit");
        if (ContainsAny(unit, "ratio", "rates", "percentages")) return RatioAlgebra(RatioMechanic(title), "us-unit");
        if (ContainsAny(unit, "angle")) return Geometry("ANGLE_LAB", "us-unit");
        if (ContainsAny(unit, "two-dimensional shapes and perimeter")) return Geometry("PERIMETER_TRACE", "us-unit");
        if (ContainsAny(unit, "properties of two-dimensional shapes")) return Geometry("SHAPE_IDENTIFY_COMPARE", "us-unit");
        if (ContainsAny(unit, "shapes on the coordinate plane")) return Geometry("COORDINATE_SHAPE", "us-unit");
        if (ContainsAny(unit, "finding volume")) return Geometry("VOLUME_BUILD", "us-unit");
        if (ContainsAny(unit, "area and surface area")) return Geometry("SURFACE_AREA_BUILD", "us-unit");
        if (ContainsAny(unit, "area and multiplication")) return Geometry("AREA_TILE", "us-unit");
        if (ContainsAny(unit, "measuring length", "length measurements")) return Measurement(MeasurementMechanic(title), "us-unit");
        if (ContainsAny(unit, "equal groups")) return Operations("EQUAL_GROUPS_ARRAY", "us-unit");
        if (ContainsAny(unit, "factors and multiples")) return Operations("FACTOR_MULTIPLE_ARRAY", "us-unit");
        if (ContainsAny(unit, "addition and subtraction story problems")) return Reasoning("MULTI_STEP_SCENARIO", "us-unit");
        if (ContainsAny(unit, "addition and subtraction on the number line")) return NumberSystem("NUMBER_LINE_SEQUENCE", "us-unit");
        if (ContainsAny(unit, "adding", "subtracting", "addition", "subtraction", "multiplying", "dividing", "multiplication", "division", "arithmetic"))
            return Operations(OperationsMechanic(title + " " + unit), "us-unit");
        if (ContainsAny(unit, "numbers to", "hundredths to hundred-thousands", "rational numbers", "number and place value"))
            return NumberSystem(NumberMechanic(title), "us-unit");

        return Reasoning("MODEL_SELECT", "supported-framework-safety-net");
    }

    private static (string Workspace, string Mechanic, string Source) ResolveUae(
        string unitTitle,
        string lessonTitle)
    {
        var unit = Normalize(unitTitle);
        var title = Normalize(lessonTitle);

        if (ContainsAny(unit, "advanced reasoning")) return Reasoning("MULTI_STEP_SCENARIO", "uae-unit");
        if (ContainsAny(unit, "fractions")) return Fractions(FractionMechanic(title), "uae-unit");
        if (ContainsAny(unit, "statistics", "data")) return Data(DataMechanic(title), "uae-unit");
        if (ContainsAny(unit, "ratio and algebra", "ratio and proportion", "patterns and relations")) return RatioAlgebra(RatioMechanic(title), "uae-unit");
        if (ContainsAny(unit, "multiplication and division")) return Operations(OperationsMechanic(title + " " + unit), "uae-unit");
        if (ContainsAny(unit, "number and place value")) return NumberSystem(NumberMechanic(title), "uae-unit");

        if (ContainsAny(unit, "geometry and data"))
        {
            if (ContainsAny(title, "data", "graph", "chart", "table")) return Data(DataMechanic(title), "uae-mixed-title");
            if (ContainsAny(title, "shape", "geometry", "angle", "position", "symmetry")) return Geometry(GeometryMechanic(title), "uae-mixed-title");
            return Composite("uae-mixed-unit");
        }

        if (ContainsAny(unit, "geometry and measure"))
        {
            if (ContainsAny(title, "perimeter", "area", "volume", "shape", "angle", "triangle", "coordinate")) return Geometry(GeometryMechanic(title), "uae-mixed-title");
            if (ContainsAny(title, "length", "mass", "capacity", "measure", "unit", "temperature")) return Measurement(MeasurementMechanic(title), "uae-mixed-title");
            return Composite("uae-mixed-unit");
        }

        if (ContainsAny(unit, "geometry")) return Geometry(GeometryMechanic(title), "uae-unit");
        if (ContainsAny(unit, "measurement"))
        {
            if (ContainsAny(title, "time", "clock", "money")) return TimeMoney(TimeMoneyMechanic(title), "uae-title");
            return Measurement(MeasurementMechanic(title), "uae-unit");
        }

        if (ContainsAny(unit, "number sense"))
        {
            if (ContainsAny(title, "count", "object", "sort", "ordinal")) return ObjectCounting(ObjectCountingMechanic(title), "uae-title");
            return NumberSystem(NumberMechanic(title), "uae-unit");
        }

        if (ContainsAny(unit, "number and calculation", "number and operations", "number"))
        {
            if (ContainsAny(title, "add", "subtract", "multiply", "division", "divide", "factor", "multiple", "operation"))
                return Operations(OperationsMechanic(title), "uae-title");
            if (ContainsAny(title, "fraction", "decimal", "percent")) return Fractions(FractionMechanic(title), "uae-title");
            return NumberSystem(NumberMechanic(title), "uae-unit");
        }

        return Reasoning("MODEL_SELECT", "supported-framework-safety-net");
    }

    private static (string Workspace, string Mechanic, string Source) ResolveCambridge(
        string unitTitle,
        string lessonTitle)
    {
        var unit = Normalize(unitTitle);
        var title = Normalize(lessonTitle);

        if (ContainsAny(unit, "categorical data")) return Data(DataMechanic(title), "cambridge-unit");
        if (ContainsAny(unit, "time")) return TimeMoney(TimeMoneyMechanic(title), "cambridge-unit");
        if (ContainsAny(unit, "fractions and money"))
        {
            if (ContainsAny(title, "money", "coin", "currency")) return TimeMoney("MONEY_VALUE", "cambridge-mixed-title");
            if (ContainsAny(title, "fraction", "half", "quarter")) return Fractions(FractionMechanic(title), "cambridge-mixed-title");
            return Composite("cambridge-mixed-unit");
        }
        if (ContainsAny(unit, "number sense and sequences"))
        {
            if (ContainsAny(title, "count", "touch", "object", "sort", "ordinal", "subit")) return ObjectCounting(ObjectCountingMechanic(title), "cambridge-title");
            return NumberSystem(NumberMechanic(title), "cambridge-unit");
        }
        if (ContainsAny(unit, "shape, measure and position"))
        {
            if (ContainsAny(title, "measure", "length", "mass", "capacity", "compare size")) return Measurement(MeasurementMechanic(title), "cambridge-mixed-title");
            if (ContainsAny(title, "position", "direction", "shape", "geometry", "line", "angle")) return Geometry(GeometryMechanic(title), "cambridge-mixed-title");
            return Composite("cambridge-mixed-unit");
        }
        if (ContainsAny(unit, "geometry and measure"))
        {
            if (ContainsAny(title, "angle", "shape", "triangle", "quadrilateral", "symmetry", "coordinate", "perimeter", "area", "volume")) return Geometry(GeometryMechanic(title), "cambridge-title");
            if (ContainsAny(title, "measure", "length", "mass", "capacity", "unit")) return Measurement(MeasurementMechanic(title), "cambridge-title");
            return Geometry("SHAPE_IDENTIFY_COMPARE", "cambridge-unit");
        }
        if (ContainsAny(unit, "fractions")) return Fractions(FractionMechanic(title), "cambridge-unit");
        if (ContainsAny(unit, "number and place value")) return NumberSystem(NumberMechanic(title), "cambridge-unit");
        if (ContainsAny(unit, "additive structures", "multiplicative structures", "number facts", "addition, subtraction and doubles"))
            return Operations(OperationsMechanic(title + " " + unit), "cambridge-unit");

        return Reasoning("MODEL_SELECT", "supported-framework-safety-net");
    }

    private static (string Workspace, string Mechanic, string Source) ResolveUk(
        string unitTitle,
        string lessonTitle)
    {
        var unit = Normalize(unitTitle);
        var title = Normalize(lessonTitle);

        if (ContainsAny(unit, "geometry - position and direction")) return Geometry("POSITION_ROUTE", "uk-unit");
        if (ContainsAny(unit, "geometry - properties of shapes")) return Geometry(GeometryMechanic(title), "uk-unit");
        if (ContainsAny(unit, "measurement"))
        {
            if (ContainsAny(title, "time", "clock", "money", "pounds", "pence")) return TimeMoney(TimeMoneyMechanic(title), "uk-title");
            return Measurement(MeasurementMechanic(title), "uk-unit");
        }
        if (ContainsAny(unit, "number - fractions")) return Fractions(FractionMechanic(title), "uk-unit");
        if (ContainsAny(unit, "number - addition", "number - multiplication", "addition, subtraction, multiplication and division"))
            return Operations(OperationsMechanic(title + " " + unit), "uk-unit");
        if (ContainsAny(unit, "number - number and place value")) return NumberSystem(NumberMechanic(title), "uk-unit");
        if (ContainsAny(unit, "statistics")) return Data(DataMechanic(title), "uk-unit");
        if (ContainsAny(unit, "algebra")) return RatioAlgebra("BALANCE_EQUATION", "uk-unit");
        if (ContainsAny(unit, "ratio and proportion")) return RatioAlgebra(RatioMechanic(title), "uk-unit");

        return Reasoning("MODEL_SELECT", "supported-framework-safety-net");
    }

    private static (string Workspace, string Mechanic, string Source) ResolvePolish(
        string lessonCode,
        string unitTitle,
        string lessonTitle)
    {
        var unit = Normalize(unitTitle);
        var title = Normalize(lessonTitle);
        var core = CoreIndex(lessonCode);

        if (unit == "osiągnięcia w zakresie rozumienia stosunków przestrzennych i cech wielkościowych")
            return core switch
            {
                1 => Geometry("POSITION_ROUTE", "pl-source-key"),
                2 => Measurement("MEASURE_COMPARE", "pl-source-key"),
                3 => Geometry("LINE_RELATIONS", "pl-source-key"),
                _ => Composite("pl-source-unit")
            };

        if (unit == "osiągnięcia w zakresie rozumienia liczb i ich własności")
            return core switch
            {
                1 => NumberSystem("NUMBER_LINE_SEQUENCE", "pl-source-key"),
                2 => NumberSystem("SYMBOL_VALUE_MATCH", "pl-source-key"),
                3 => NumberSystem("PLACE_VALUE_BUILD", "pl-source-key"),
                4 => NumberSystem("COMPARE_ORDER", "pl-source-key"),
                _ => NumberSystem("NUMBER_LINE_SEQUENCE", "pl-source-unit")
            };

        if (unit == "osiągnięcia w zakresie posługiwania się liczbami")
            return core switch
            {
                1 => Operations("RELATIONSHIP_MODEL", "pl-source-key"),
                2 => Operations("FACT_FLUENCY", "pl-source-key"),
                3 => Composite("pl-source-key"),
                4 => Operations("WRITTEN_ALGORITHM", "pl-source-key"),
                _ => Operations("FACT_FLUENCY", "pl-source-unit")
            };

        if (unit == "osiągnięcia w zakresie czytania tekstów matematycznych")
            return core == 2
                ? Reasoning("CONSTRAINT_PUZZLE", "pl-source-key")
                : Reasoning("MULTI_STEP_SCENARIO", "pl-source-key");

        if (unit == "osiągnięcia w zakresie rozumienia pojęć geometrycznych")
            return core switch
            {
                1 => Geometry("SHAPE_BUILDER", "pl-source-key"),
                2 => Measurement("RULER_ALIGN", "pl-source-key"),
                3 => Geometry("PERIMETER_TRACE", "pl-source-key"),
                4 => Geometry("SYMMETRY_MIRROR", "pl-source-key"),
                _ => Geometry("SHAPE_IDENTIFY_COMPARE", "pl-source-unit")
            };

        if (unit == "osiągnięcia w zakresie stosowania matematyki w sytuacjach życiowych oraz w innych")
            return core switch
            {
                1 => Composite("pl-source-key"),
                2 => Fractions("FRACTION_PARTITION", "pl-source-key"),
                3 => TimeMoney("MONEY_VALUE", "pl-source-key"),
                4 => Composite("pl-source-key"),
                5 => Measurement("TEMPERATURE_SCALE", "pl-source-key"),
                6 => Reasoning("ESTIMATE_CHECK", "pl-source-key"),
                7 => Measurement("MASS_BALANCE", "pl-source-key"),
                8 => Reasoning("STRATEGY_GAME", "pl-source-key"),
                9 => Reasoning("MULTI_STEP_SCENARIO", "pl-source-key"),
                _ => Reasoning("MODEL_SELECT", "pl-source-unit")
            };

        if (unit == "liczby naturalne w dziesiątkowym układzie pozycyjnym")
            return core switch
            {
                1 => NumberSystem("SYMBOL_VALUE_MATCH", "pl-source-key"),
                2 => NumberSystem("NUMBER_LINE_SEQUENCE", "pl-source-key"),
                3 => NumberSystem("COMPARE_ORDER", "pl-source-key"),
                4 => NumberSystem("ESTIMATE_CHECK", "pl-source-key"),
                5 => NumberSystem("NUMERAL_SYSTEM_CONVERT", "pl-source-key"),
                _ => NumberSystem("PLACE_VALUE_BUILD", "pl-source-unit")
            };

        if (unit == "działania na liczbach naturalnych")
            return core switch
            {
                1 => Operations("FACT_FLUENCY", "pl-source-key"),
                2 or 3 => Operations("WRITTEN_ALGORITHM", "pl-source-key"),
                4 => Operations("OPERATION_PROPERTY", "pl-source-key"),
                5 => Operations("RELATIONSHIP_MODEL", "pl-source-key"),
                6 or 7 or 11 or 12 => Operations("FACTOR_MULTIPLE_ARRAY", "pl-source-key"),
                8 => Operations("POWER_BUILD", "pl-source-key"),
                9 => Operations("EXPRESSION_ORDER", "pl-source-key"),
                10 => Reasoning("ESTIMATE_CHECK", "pl-source-key"),
                13 => Operations("NUMBER_PROPERTY_SORT", "pl-source-key"),
                14 => Operations("PRIME_FACTOR_BUILD", "pl-source-key"),
                15 => Operations("SHARE_REMAINDER", "pl-source-key"),
                _ => Operations("FACT_FLUENCY", "pl-source-unit")
            };

        if (unit == "liczby całkowite")
            return core switch
            {
                1 => NumberSystem("INTEGER_CONTEXT", "pl-source-key"),
                2 => NumberSystem("NUMBER_LINE_SEQUENCE", "pl-source-key"),
                3 => NumberSystem("DISTANCE_ON_NUMBER_LINE", "pl-source-key"),
                4 => NumberSystem("COMPARE_ORDER", "pl-source-key"),
                5 => Operations("FACT_FLUENCY", "pl-source-key"),
                _ => NumberSystem("INTEGER_CONTEXT", "pl-source-unit")
            };

        if (unit == "ułamki zwykłe i dziesiętne")
            return core switch
            {
                1 or 2 or 13 => Fractions("FRACTION_MODEL", "pl-source-key"),
                3 or 4 => Fractions("FRACTION_EQUIVALENCE", "pl-source-key"),
                5 => Fractions("FRACTION_CONVERT", "pl-source-key"),
                6 => Fractions("DECIMAL_UNIT_CONVERT", "pl-source-key"),
                7 => Fractions("FRACTION_NUMBER_LINE", "pl-source-key"),
                8 or 9 or 10 => Fractions("FRACTION_DECIMAL_LINK", "pl-source-key"),
                11 => Fractions("ESTIMATE_CHECK", "pl-source-key"),
                12 => Fractions("FRACTION_COMPARE", "pl-source-key"),
                14 => Fractions("FRACTION_SCALE", "pl-source-key"),
                _ => Fractions("FRACTION_MODEL", "pl-source-unit")
            };

        if (unit == "działania na ułamkach zwykłych i dziesiętnych")
            return core switch
            {
                1 or 5 => Fractions("FRACTION_OPERATIONS", "pl-source-key"),
                2 or 6 => Fractions("DECIMAL_OPERATIONS", "pl-source-key"),
                3 => Fractions("FRACTION_COMPARE", "pl-source-key"),
                4 => Fractions("FRACTION_MODEL", "pl-source-key"),
                7 => Fractions("EXPRESSION_ORDER", "pl-source-key"),
                _ => Fractions("FRACTION_OPERATIONS", "pl-source-unit")
            };

        if (unit == "elementy algebry")
            return core == 3
                ? RatioAlgebra("BALANCE_EQUATION", "pl-source-key")
                : RatioAlgebra("EXPRESSION_BUILD", "pl-source-key");

        if (unit == "proste i odcinki")
            return core switch
            {
                1 or 2 => Geometry("LINE_RELATIONS", "pl-source-key"),
                3 => Geometry("LINE_DRAW", "pl-source-key"),
                4 => Measurement("RULER_ALIGN", "pl-source-key"),
                5 => Geometry("POINT_LINE_DISTANCE", "pl-source-key"),
                _ => Geometry("LINE_RELATIONS", "pl-source-unit")
            };

        if (unit == "kąty") return Geometry("ANGLE_LAB", "pl-source-key");

        if (unit == "wielokąty, koła i okręgi")
            return core switch
            {
                1 or 4 or 5 => Geometry("SHAPE_IDENTIFY_COMPARE", "pl-source-key"),
                2 => Geometry("SHAPE_BUILDER", "pl-source-key"),
                3 => Geometry("ANGLE_LAB", "pl-source-key"),
                6 => Geometry("CIRCLE_PARTS", "pl-source-key"),
                7 => Geometry("CIRCLE_CONSTRUCT", "pl-source-key"),
                8 => Composite("pl-source-key"),
                _ => Geometry("SHAPE_IDENTIFY_COMPARE", "pl-source-unit")
            };

        if (unit == "bryły")
            return core switch
            {
                1 or 2 => Geometry("SHAPE_IDENTIFY_COMPARE", "pl-source-key"),
                3 or 4 => Geometry("SHAPE_NET_BUILD", "pl-source-key"),
                5 => Geometry("RELATIONSHIP_MODEL", "pl-source-key"),
                _ => Geometry("SHAPE_NET_BUILD", "pl-source-unit")
            };

        if (unit == "obliczenia w geometrii")
            return core switch
            {
                1 => Geometry("ANGLE_LAB", "pl-source-key"),
                2 => Geometry("PERIMETER_TRACE", "pl-source-key"),
                3 or 5 => Geometry("AREA_TILE", "pl-source-key"),
                4 or 7 => Measurement("UNIT_CONVERSION", "pl-source-key"),
                6 => Geometry("VOLUME_BUILD", "pl-source-key"),
                _ => Geometry("AREA_TILE", "pl-source-unit")
            };

        if (unit == "obliczenia praktyczne")
            return core switch
            {
                1 or 2 => Fractions("PERCENT_MODEL", "pl-source-key"),
                3 => TimeMoney("TIMELINE", "pl-source-key"),
                4 => TimeMoney("CALENDAR_ORDER", "pl-source-key"),
                5 => Measurement("TEMPERATURE_SCALE", "pl-source-key"),
                6 or 7 => Measurement("UNIT_CONVERSION", "pl-source-key"),
                8 => RatioAlgebra("RATIO_SCALE", "pl-source-key"),
                9 => RatioAlgebra("SPEED_DISTANCE_TIME", "pl-source-key"),
                _ => Reasoning("MODEL_SELECT", "pl-source-unit")
            };

        if (unit == "elementy statystyki opisowej") return Data(core == 1 ? "DATA_SORT" : "PLOT_READ", "pl-source-key");
        if (unit == "zadania tekstowe") return Reasoning(ReasoningMechanic(core), "pl-source-key");

        if (ContainsAny(title, "ułam")) return Fractions("FRACTION_MODEL", "pl-title");
        if (ContainsAny(title, "kąt", "figura", "geometr")) return Geometry(GeometryMechanic(title), "pl-title");
        if (ContainsAny(title, "liczb")) return NumberSystem("NUMBER_LINE_SEQUENCE", "pl-title");
        return Reasoning("MODEL_SELECT", "supported-framework-safety-net");
    }

    private static string ObjectCountingMechanic(string text)
    {
        if (ContainsAny(text, "sort", "classify")) return "SORT_CLASSIFY";
        if (ContainsAny(text, "ordinal", "position")) return "ORDINAL_POSITION";
        if (ContainsAny(text, "subit", "pattern")) return "SUBITIZE_PATTERN";
        return "COUNT_TOUCH";
    }

    private static string NumberMechanic(string text)
    {
        if (ContainsAny(text, "place value", "digit", "hundred", "thousand", "tenth")) return "PLACE_VALUE_BUILD";
        if (ContainsAny(text, "compare", "order", "greater", "less")) return "COMPARE_ORDER";
        if (ContainsAny(text, "round", "estimate")) return "ESTIMATE_CHECK";
        if (ContainsAny(text, "integer", "negative")) return "INTEGER_CONTEXT";
        if (ContainsAny(text, "scale")) return "READ_SCALE_INTERVALS";
        return "NUMBER_LINE_SEQUENCE";
    }

    private static string OperationsMechanic(string text)
    {
        if (ContainsAny(text, "factor", "multiple", "prime")) return "FACTOR_MULTIPLE_ARRAY";
        if (ContainsAny(text, "array", "equal group", "multiply", "multiplication")) return "EQUAL_GROUPS_ARRAY";
        if (ContainsAny(text, "divide", "division", "share", "quotient")) return "SHARE_DIVIDE";
        if (ContainsAny(text, "subtract", "difference", "take away")) return "TAKE_AWAY";
        if (ContainsAny(text, "written", "algorithm", "multi-digit")) return "WRITTEN_ALGORITHM";
        if (ContainsAny(text, "property", "distribut")) return "OPERATION_PROPERTY";
        return "JOIN_COMBINE";
    }

    private static string FractionMechanic(string text)
    {
        if (ContainsAny(text, "percent")) return "PERCENT_MODEL";
        if (ContainsAny(text, "decimal")) return "FRACTION_DECIMAL_LINK";
        if (ContainsAny(text, "equivalent", "equivalence")) return "FRACTION_EQUIVALENCE";
        if (ContainsAny(text, "compare", "order")) return "FRACTION_COMPARE";
        if (ContainsAny(text, "multiply", "divide", "add", "subtract", "operation")) return "FRACTION_OPERATIONS";
        if (ContainsAny(text, "number line")) return "FRACTION_NUMBER_LINE";
        if (ContainsAny(text, "partition", "equal part")) return "FRACTION_PARTITION";
        return "FRACTION_MODEL";
    }

    private static string GeometryMechanic(string text)
    {
        if (ContainsAny(text, "perimeter", "obwód", "obwod", "محيط")) return "PERIMETER_TRACE";
        if (ContainsAny(text, "surface area")) return "SURFACE_AREA_BUILD";
        if (ContainsAny(text, "area", "pole", "مساحة")) return "AREA_TILE";
        if (ContainsAny(text, "volume", "objętość", "objetosc", "حجم")) return "VOLUME_BUILD";
        if (ContainsAny(text, "angle", "kąt", "kat", "زاوي")) return "ANGLE_LAB";
        if (ContainsAny(text, "symmetry", "symetr")) return "SYMMETRY_MIRROR";
        if (ContainsAny(text, "coordinate")) return "COORDINATE_SHAPE";
        if (ContainsAny(text, "position", "direction")) return "POSITION_ROUTE";
        if (ContainsAny(text, "net")) return "SHAPE_NET_BUILD";
        if (ContainsAny(text, "build", "construct", "compose")) return "SHAPE_BUILDER";
        if (ContainsAny(text, "circle")) return "CIRCLE_PARTS";
        if (ContainsAny(text, "line", "segment", "parallel", "perpendicular")) return "LINE_RELATIONS";
        return "SHAPE_IDENTIFY_COMPARE";
    }

    private static string MeasurementMechanic(string text)
    {
        if (ContainsAny(text, "temperature")) return "TEMPERATURE_SCALE";
        if (ContainsAny(text, "mass", "weight")) return "MASS_BALANCE";
        if (ContainsAny(text, "capacity", "liquid")) return "CAPACITY_FILL";
        if (ContainsAny(text, "convert", "conversion", "unit")) return "UNIT_CONVERSION";
        if (ContainsAny(text, "ruler", "length")) return "RULER_ALIGN";
        if (ContainsAny(text, "scale")) return "SCALE_READ";
        return "MEASURE_COMPARE";
    }

    private static string TimeMoneyMechanic(string text)
    {
        if (ContainsAny(text, "calendar", "date", "day", "month")) return "CALENDAR_ORDER";
        if (ContainsAny(text, "money", "coin", "currency", "pound", "pence", "dollar", "cent")) return "MONEY_VALUE";
        if (ContainsAny(text, "duration", "elapsed", "timeline")) return "TIMELINE";
        return "CLOCK_FACE";
    }

    private static string DataMechanic(string text)
    {
        if (ContainsAny(text, "bar chart", "bar graph")) return "BAR_CHART_BUILD";
        if (ContainsAny(text, "pictogram")) return "PICTOGRAM_BUILD";
        if (ContainsAny(text, "table")) return "TABLE_BUILD";
        if (ContainsAny(text, "mean", "median", "mode", "range")) return "STATISTIC_CALCULATE";
        if (ContainsAny(text, "distribution")) return "DISTRIBUTION_COMPARE";
        if (ContainsAny(text, "sort", "categor")) return "DATA_SORT";
        return "PLOT_READ";
    }

    private static string RatioMechanic(string text)
    {
        if (ContainsAny(text, "equation", "unknown", "solve")) return "BALANCE_EQUATION";
        if (ContainsAny(text, "speed", "distance", "time")) return "SPEED_DISTANCE_TIME";
        if (ContainsAny(text, "function", "pattern", "sequence")) return "FUNCTION_PATTERN";
        if (ContainsAny(text, "double number line", "rate")) return "DOUBLE_NUMBER_LINE";
        return "RATIO_SCALE";
    }

    private static string ReasoningMechanic(int core) => core switch
    {
        1 => "EVIDENCE_ORDER",
        2 => "MODEL_SELECT",
        3 => "RELATIONSHIP_MODEL",
        4 or 5 => "MULTI_STEP_SCENARIO",
        6 => "ERROR_ANALYSIS",
        7 => "CONSTRAINT_PUZZLE",
        _ => "MULTI_STEP_SCENARIO"
    };

    private static int CoreIndex(string lessonCode)
    {
        const string marker = "-CORE-";
        var start = lessonCode.IndexOf(marker, StringComparison.OrdinalIgnoreCase);
        if (start < 0) return 0;
        start += marker.Length;
        var end = lessonCode.IndexOf('-', start);
        if (end < 0) end = lessonCode.Length;
        return int.TryParse(lessonCode[start..end], out var result) ? result : 0;
    }

    private static (string, string, string) ObjectCounting(string mechanic, string source) => ("OBJECT_COUNTING", mechanic, source);
    private static (string, string, string) NumberSystem(string mechanic, string source) => ("NUMBER_SYSTEM", mechanic, source);
    private static (string, string, string) Operations(string mechanic, string source) => ("OPERATIONS", mechanic, source);
    private static (string, string, string) Fractions(string mechanic, string source) => ("FRACTION_DECIMAL_PERCENT", mechanic, source);
    private static (string, string, string) Geometry(string mechanic, string source) => ("GEOMETRY", mechanic, source);
    private static (string, string, string) Measurement(string mechanic, string source) => ("MEASUREMENT", mechanic, source);
    private static (string, string, string) TimeMoney(string mechanic, string source) => ("TIME_MONEY", mechanic, source);
    private static (string, string, string) Data(string mechanic, string source) => ("DATA_STATISTICS", mechanic, source);
    private static (string, string, string) RatioAlgebra(string mechanic, string source) => ("RATIO_ALGEBRA", mechanic, source);
    private static (string, string, string) Reasoning(string mechanic, string source) => ("REASONING_MODELING", mechanic, source);
    private static (string, string, string) Composite(string source) => ("COMPOSITE_SESSION", "COMPOSITE_SESSION", source);

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

    private static bool ContainsAny(string text, params string[] needles) =>
        needles.Any(text.Contains);

    private static string Normalize(string value) =>
        (value ?? string.Empty).Trim().ToLowerInvariant();
}
