using Edulytics.Web.GameRouting;

namespace Edulytics.Tests;

public sealed class GameLessonRouterTests
{
    [Theory]
    [InlineData(GameLessonRouter.CountTouchLessonCode, "Number Sense and Sequences", "Count, Touch, and Check", "OBJECT_COUNTING", "COUNT_TOUCH", "count-touch")]
    [InlineData(GameLessonRouter.AngleExplorerLessonCode, "Geometry and Measure", "Compare, estimate, measure and draw angles: Build the Idea", "GEOMETRY", "ANGLE_LAB", "angle-explorer")]
    [InlineData(GameLessonRouter.FractionForgeLessonCode, "Fractions", "Build the Idea — Fractions and decimals", "FRACTION_DECIMAL_PERCENT", "FRACTION_EQUIVALENCE", "fraction-forge")]
    public void ExactSpecializedLessonsRemainPlayable(
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
    [InlineData("PED:US-CCSS-MATH:G6:U01:L03", "Area and Surface Area", "Surface area of a rectangular prism", "GEOMETRY", "en")]
    [InlineData("PED:PL-NATIONAL-MATH:L5:KLASA-V:CORE:PL-REQ-CORE-3-000", "Ułamki zwykłe i dziesiętne", "Ułamki zwykłe i dziesiętne — Lesson 03", "FRACTION_DECIMAL_PERCENT", "pl")]
    [InlineData("PED:UAE-MOE-MATH:L5:COMMON:08:03:PERIMETER", "Geometry and Measure", "Perimeter of rectangles", "GEOMETRY", "ar")]
    [InlineData("PED:UK-NC-ENG-MATH:L5:YEAR-5:GEOMETRY:ANGLES", "Geometry - properties of shapes", "Know angles are measured in degrees", "GEOMETRY", "en")]
    [InlineData("PED:CAMBRIDGE-INTL-MATH:S6:6N-2:CHECK", "Fractions", "Compare fractions", "FRACTION_DECIMAL_PERCENT", "en")]
    public void SupportedCurriculaArePlayableThroughUniversalWorkspace(
        string lessonCode,
        string unitTitle,
        string lessonTitle,
        string workspace,
        string locale)
    {
        var route = GameLessonRouter.Resolve(lessonCode, unitTitle, lessonTitle);

        Assert.True(route.IsPlayable);
        Assert.Equal(GameLessonRouter.UniversalRendererKey, route.RendererKey);
        Assert.Equal(workspace, route.Workspace);
        Assert.Equal(locale, route.Locale);
        Assert.NotEqual("NEEDS_REVIEW", route.Workspace);
    }

    [Theory]
    [InlineData("PED:PL-NATIONAL-MATH:L1:KLASA-I:EDUKACJA-WCZESNOSZKOLNA:PL-REQ-CORE-1-001", "Osiągnięcia w zakresie rozumienia stosunków przestrzennych i cech wielkościowych", "Lesson 01", "GEOMETRY", "POSITION_ROUTE")]
    [InlineData("PED:PL-NATIONAL-MATH:L1:KLASA-I:EDUKACJA-WCZESNOSZKOLNA:PL-REQ-CORE-2-002", "Osiągnięcia w zakresie rozumienia stosunków przestrzennych i cech wielkościowych", "Lesson 02", "MEASUREMENT", "MEASURE_COMPARE")]
    [InlineData("PED:PL-NATIONAL-MATH:L4:KLASA-IV:CORE:PL-REQ-CORE-6-000", "Działania na liczbach naturalnych", "Lesson 06", "OPERATIONS", "FACTOR_MULTIPLE_ARRAY")]
    [InlineData("PED:PL-NATIONAL-MATH:L5:KLASA-V:CORE:PL-REQ-CORE-2-000", "Obliczenia w geometrii", "Lesson 02", "GEOMETRY", "PERIMETER_TRACE")]
    [InlineData("PED:PL-NATIONAL-MATH:L6:KLASA-VI:CORE:PL-REQ-CORE-9-000", "Obliczenia praktyczne", "Lesson 09", "RATIO_ALGEBRA", "SPEED_DISTANCE_TIME")]
    public void PolishGenericLessonTitlesUseCoreSourceIndex(
        string lessonCode,
        string unitTitle,
        string lessonTitle,
        string workspace,
        string mechanic)
    {
        var route = GameLessonRouter.Resolve(lessonCode, unitTitle, lessonTitle);

        Assert.True(route.IsPlayable);
        Assert.Equal(workspace, route.Workspace);
        Assert.Equal(mechanic, route.Mechanic);
        Assert.Equal("pl-source-key", route.ClassificationSource);
    }

    [Theory]
    [MemberData(nameof(CurrentCurriculumUnits))]
    public void EveryCurrentGradeOneToSixUnitFamilyHasAPlayableNonFallbackRoute(
        string lessonCode,
        string unitTitle,
        string sampleTitle)
    {
        var route = GameLessonRouter.Resolve(lessonCode, unitTitle, sampleTitle);

        Assert.True(route.IsPlayable);
        Assert.NotNull(route.RendererKey);
        Assert.NotEqual("NEEDS_REVIEW", route.Workspace);
        Assert.NotEqual("NEEDS_REVIEW", route.Mechanic);
        Assert.NotEqual("supported-framework-safety-net", route.ClassificationSource);
    }

    [Fact]
    public void ActualUsCenterDayFamilyUsesCompositeSessionInsteadOfGenericQuiz()
    {
        var route = GameLessonRouter.Resolve(
            "PED:US-CCSS-MATH:G5:U99:L99",
            "Putting It All Together",
            "Center Day");

        Assert.True(route.IsPlayable);
        Assert.Equal("COMPOSITE_SESSION", route.Workspace);
        Assert.Equal("COMPOSITE_SESSION", route.Mechanic);
        Assert.Equal(GameLessonRouter.UniversalRendererKey, route.RendererKey);
    }

    [Fact]
    public void UnsupportedFrameworkStillRequiresReview()
    {
        var route = GameLessonRouter.Resolve("PED:OTHER-MATH:L1:X", "Numbers", "Count");

        Assert.False(route.IsPlayable);
        Assert.Null(route.RendererKey);
        Assert.Equal("NEEDS_REVIEW", route.Workspace);
    }

    public static IEnumerable<object[]> CurrentCurriculumUnits()
    {
        foreach (var unit in new[]
        {
            "Adding Within 100", "Adding and Subtracting Within 20", "Adding, Subtracting, and Working with Data",
            "Addition and Subtraction Story Problems", "Geometry and Time", "Length Measurements Within 120 Units",
            "Numbers to 99", "Putting It All Together", "Adding and Subtracting within 1,000",
            "Adding and Subtracting within 100", "Addition and Subtraction on the Number Line", "Equal Groups",
            "Geometry, Time, and Money", "Measuring Length", "Numbers to 1,000", "Area and Multiplication",
            "Fractions as Numbers", "Introducing Multiplication", "Measuring Length, Time, Liquid Volume, and Weight",
            "Relating Multiplication to Division", "Two-dimensional Shapes and Perimeter",
            "Wrapping Up Addition and Subtraction Within 1,000", "Angles and Angle Measurement", "Extending Operations to Fractions",
            "Factors and Multiples", "Fraction Equivalence and Comparison", "From Hundredths to Hundred-thousands",
            "Multiplicative Comparison and Measurement", "Multiplying and Dividing Multi-digit Numbers",
            "Properties of Two-dimensional Shapes", "Finding Volume", "Fractions as Quotients and Fraction Multiplication",
            "More Decimal and Fraction Operations", "Multiplying and Dividing Fractions", "Place Value Patterns and Decimal Operations",
            "Shapes on the Coordinate Plane", "Wrapping Up Multiplication and Division with Multi-Digit Numbers",
            "Area and Surface Area", "Arithmetic in Base Ten", "Data Sets and Distributions", "Dividing Fractions",
            "Expressions and Equations", "Introducing Ratios", "Putting it All Together", "Rational Numbers", "Unit Rates and Percentages"
        })
            yield return ["PED:US-CCSS-MATH:G6:UNIT:TEST", unit, UnitSampleTitle(unit)];

        foreach (var unit in new[]
        {
            "Data", "Geometry", "Measurement", "Number Sense", "Patterns and Relations", "Fractions", "Geometry and Data",
            "Multiplication and Division", "Number and Place Value", "Number and Calculation", "Fractions and Decimals",
            "Number and Operations", "Statistics", "Advanced Reasoning", "Fractions, Decimals and Percentages",
            "Geometry and Measure", "Number", "Ratio and Proportion", "Ratio and Algebra"
        })
            yield return ["PED:UAE-MOE-MATH:L6:COMMON:TEST", unit, UnitSampleTitle(unit)];

        foreach (var unit in new[]
        {
            "Addition, Subtraction and Doubles", "Categorical Data", "Fractions and Money", "Number Sense and Sequences",
            "Shape, Measure and Position", "Time", "Additive Structures and Relationships", "Geometry and Measure",
            "Multiplicative Structures and Relationships", "Number Facts and Fluency", "Number and Place Value", "Fractions"
        })
            yield return ["PED:CAMBRIDGE-INTL-MATH:S6:UNIT:TEST", unit, UnitSampleTitle(unit)];

        foreach (var unit in new[]
        {
            "Geometry - position and direction", "Geometry - properties of shapes", "Measurement",
            "Number - addition and subtraction", "Number - fractions", "Number - multiplication and division",
            "Number - number and place value", "Statistics", "Number - fractions (including decimals)",
            "Number - fractions (including decimals and percentages)", "Algebra", "Ratio and proportion",
            "Number - Fractions (including decimals and percentages)", "Number - addition, subtraction, multiplication and division"
        })
            yield return ["PED:UK-NC-ENG-MATH:L6:YEAR-6:TEST", unit, UnitSampleTitle(unit)];

        foreach (var unit in new[]
        {
            "Osiągnięcia w zakresie czytania tekstów matematycznych", "Osiągnięcia w zakresie posługiwania się liczbami",
            "Osiągnięcia w zakresie rozumienia liczb i ich własności", "Osiągnięcia w zakresie rozumienia pojęć geometrycznych",
            "Osiągnięcia w zakresie rozumienia stosunków przestrzennych i cech wielkościowych",
            "Osiągnięcia w zakresie stosowania matematyki w sytuacjach życiowych oraz w innych",
            "Bryły", "Działania na liczbach naturalnych", "Działania na ułamkach zwykłych i dziesiętnych", "Elementy algebry",
            "Elementy statystyki opisowej", "Kąty", "Liczby całkowite", "Liczby naturalne w dziesiątkowym układzie pozycyjnym",
            "Obliczenia praktyczne", "Obliczenia w geometrii", "Proste i odcinki", "Ułamki zwykłe i dziesiętne",
            "Wielokąty, koła i okręgi", "Zadania tekstowe"
        })
            yield return ["PED:PL-NATIONAL-MATH:L6:KLASA-VI:CORE:PL-REQ-CORE-1-000", unit, unit + " — Lesson 01"];
    }

    private static string UnitSampleTitle(string unit) => unit switch
    {
        "Adding, Subtracting, and Working with Data" => "Represent data in a chart",
        "Geometry and Time" => "Identify shapes",
        "Geometry, Time, and Money" => "Tell time on a clock",
        "Measuring Length, Time, Liquid Volume, and Weight" => "Measure liquid volume",
        "Multiplicative Comparison and Measurement" => "Compare by multiplication",
        "Place Value Patterns and Decimal Operations" => "Use decimal place value",
        "Putting It All Together" or "Putting it All Together" => "Center Day",
        "Geometry and Data" => "Classify shapes",
        "Geometry and Measure" => "Measure and draw angles",
        "Fractions and Money" => "Recognise one half",
        "Shape, Measure and Position" => "Describe position and direction",
        _ => unit
    };
}
