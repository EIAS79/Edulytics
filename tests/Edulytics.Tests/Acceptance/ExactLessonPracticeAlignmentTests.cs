using Edulytics.Web.GameRouting;

namespace Edulytics.Tests.Acceptance;

public sealed class ExactLessonPracticeAlignmentTests
{
    private const string CambridgeStage6LessonCode =
        "PED:CAMBRIDGE-INTL-MATH:S6:SUPPORTING:EXACT";

    [Fact]
    public void TwoUnknownsLesson_WinsOverIncidentalAdditiveAndRatioLanguage()
    {
        var context = string.Join(" ", new[]
        {
            "Solve problems with 2 unknowns: Reason and Apply.",
            "Addition and subtraction describe part-whole, change and difference relationships.",
            "Later stages also use these relationships to reason about unknowns, ratio and linked calculations.",
            "Represent the situation with equations or a number line and check the result independently."
        });

        var route = GameLessonRouteResolver.Resolve(
            CambridgeStage6LessonCode,
            "Additive Structures and Relationships",
            "Solve problems with 2 unknowns: Reason and Apply",
            context,
            requireLessonGrounding: true);

        Assert.True(route.IsPlayable);
        Assert.Equal(GameLessonRouteResolver.LessonGroundedRendererKey, route.RendererKey);
        Assert.Equal("REASONING_MODELING", route.Workspace);
        Assert.Equal("TWO_UNKNOWNS", route.Mechanic);
        Assert.Equal("exact-lesson-skill-v2", route.ClassificationSource);
    }

    [Fact]
    public void ReadingScalesLesson_UsesScaleReadingInsteadOfProportionOrPlaceValue()
    {
        var context = string.Join(" ", new[]
        {
            "Reading scales with 2, 4, 5 or 10 intervals: Reason and Apply.",
            "A digit's value depends on its position.",
            "Use a number line to reason about magnitude.",
            "Read equal intervals on a scale and identify the marked value."
        });

        var route = GameLessonRouteResolver.Resolve(
            CambridgeStage6LessonCode,
            "Number and Place Value",
            "Reading scales with 2, 4, 5 or 10 intervals: Reason and Apply",
            context,
            requireLessonGrounding: true);

        Assert.True(route.IsPlayable);
        Assert.Equal("MEASUREMENT", route.Workspace);
        Assert.Equal("SCALE_READING", route.Mechanic);
        Assert.Equal("exact-lesson-skill-v2", route.ClassificationSource);
    }

    [Fact]
    public void DifferentDenominatorFractionLesson_UsesExactComparisonMechanic()
    {
        var context = string.Join(" ", new[]
        {
            "Compare fractions with different denominators: Build the Idea.",
            "Fractions describe equal parts and numbers on a number line.",
            "Equivalent fractions preserve value even when numerator and denominator change together.",
            "Do not assume a larger denominator means a larger fraction."
        });

        var route = GameLessonRouteResolver.Resolve(
            CambridgeStage6LessonCode,
            "Fractions",
            "Compare fractions with different denominators: Build the Idea",
            context,
            requireLessonGrounding: true);

        Assert.True(route.IsPlayable);
        Assert.Equal("FRACTIONS", route.Workspace);
        Assert.Equal("FRACTION_COMPARE_UNLIKE", route.Mechanic);
        Assert.Equal("exact-lesson-skill-v2", route.ClassificationSource);
    }

    [Fact]
    public void IncidentalBroadKeywords_DoNotCreateAFalseExactLessonModel()
    {
        var route = GameLessonRouteResolver.Resolve(
            CambridgeStage6LessonCode,
            "Mixed review",
            "Reason and Apply",
            "Use addition, ratio, fraction and an equation to discuss several earlier ideas.",
            requireLessonGrounding: true);

        Assert.False(route.IsPlayable);
        Assert.Null(route.RendererKey);
        Assert.Equal("NEEDS_REVIEW", route.Workspace);
        Assert.Equal("lesson-grounding-required", route.ClassificationSource);
    }

    [Fact]
    public void ExactSkillRuntime_IsSeparatedFromLegacyBroadRelationshipRuntime()
    {
        var root = FindRoot();
        var view = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/StudentPractice/Game.cshtml"));
        var runtime = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/js/lesson-grounded-practice-v2.js"));

        Assert.Contains("isExactLessonGrounded", view, StringComparison.Ordinal);
        Assert.Contains("lesson-grounded-practice-v2.js", view, StringComparison.Ordinal);
        Assert.Contains("lesson-grounded-practice-v1.js", view, StringComparison.Ordinal);

        Assert.Contains("TWO_UNKNOWNS", runtime, StringComparison.Ordinal);
        Assert.Contains("SCALE_READING", runtime, StringComparison.Ordinal);
        Assert.Contains("FRACTION_COMPARE_UNLIKE", runtime, StringComparison.Ordinal);
        Assert.Contains("twoUnknownsQuestion", runtime, StringComparison.Ordinal);
        Assert.Contains("scaleReadingQuestion", runtime, StringComparison.Ordinal);
        Assert.Contains("fractionCompareQuestion", runtime, StringComparison.Ordinal);
        Assert.Contains("x + y", runtime, StringComparison.Ordinal);
        Assert.Contains("intervalOptions = [2, 4, 5, 10]", runtime, StringComparison.Ordinal);
        Assert.Contains("unlikeFractionPair", runtime, StringComparison.Ordinal);
    }

    private static string FindRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "Edulytics.sln")))
                return directory.FullName;
            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Edulytics solution root not found.");
    }
}
