using Edulytics.Web.GameRouting;

namespace Edulytics.Tests.Acceptance;

public sealed class MathematicsV2ProductMigrationPilotTests
{
    [Theory]
    [InlineData(
        MathematicsV2ProductMigrationPolicy.TwoUnknownsLessonCode,
        "Additive Structures and Relationships",
        "Solve problems with 2 unknowns: Reason and Apply",
        "Use two linked unknowns and two independent equations.",
        "TWO_UNKNOWNS")]
    [InlineData(
        MathematicsV2ProductMigrationPolicy.ScaleReadingLessonCode,
        "Number and Place Value",
        "Reading scales with 2, 4, 5 or 10 intervals: Reason and Apply",
        "Read equal intervals on a scale and identify the marked value.",
        "SCALE_READING")]
    [InlineData(
        MathematicsV2ProductMigrationPolicy.FractionCompareLessonCode,
        "Fractions",
        "Compare fractions with different denominators: Build the Idea",
        "Compare unlike denominators using equivalent fractions and common denominators.",
        "FRACTION_COMPARE_UNLIKE")]
    public void EnabledPilot_RoutesOnlyApprovedExactLessons(
        string lessonCode,
        string unitTitle,
        string lessonTitle,
        string context,
        string expectedMechanic)
    {
        var route = GameLessonRouteResolver.Resolve(
            lessonCode,
            unitTitle,
            lessonTitle,
            context,
            requireLessonGrounding: true,
            enableMathematicsV2Pilot: true);

        Assert.True(route.IsPlayable);
        Assert.Equal(expectedMechanic, route.Mechanic);
        Assert.Equal(MathematicsV2ProductMigrationPolicy.RendererKey, route.RendererKey);
        Assert.Equal(MathematicsV2ProductMigrationPolicy.ClassificationSource, route.ClassificationSource);
    }

    [Fact]
    public void KillSwitchOff_PreservesAcceptedLegacyRoute()
    {
        var route = GameLessonRouteResolver.Resolve(
            MathematicsV2ProductMigrationPolicy.TwoUnknownsLessonCode,
            "Additive Structures and Relationships",
            "Solve problems with 2 unknowns: Reason and Apply",
            "Use two linked unknowns and two independent equations.",
            requireLessonGrounding: true,
            enableMathematicsV2Pilot: false);

        Assert.True(route.IsPlayable);
        Assert.Equal(GameLessonRouteResolver.LessonGroundedRendererKey, route.RendererKey);
        Assert.Equal("exact-lesson-skill-v2", route.ClassificationSource);
    }

    [Fact]
    public void SameTitleOutsideAllowList_CannotEnterPilot()
    {
        var route = GameLessonRouteResolver.Resolve(
            "PED:CAMBRIDGE-INTL-MATH:S6:NOT-PILOT",
            "Additive Structures and Relationships",
            "Solve problems with 2 unknowns: Reason and Apply",
            "Use two linked unknowns and two independent equations.",
            requireLessonGrounding: true,
            enableMathematicsV2Pilot: true);

        Assert.True(route.IsPlayable);
        Assert.Equal(GameLessonRouteResolver.LessonGroundedRendererKey, route.RendererKey);
        Assert.Equal("exact-lesson-skill-v2", route.ClassificationSource);
    }

    [Fact]
    public void WrongMechanicForAllowListedLesson_FailsClosedToExistingRoute()
    {
        Assert.False(MathematicsV2ProductMigrationPolicy.ShouldUsePilot(
            MathematicsV2ProductMigrationPolicy.TwoUnknownsLessonCode,
            "SCALE_READING",
            enabled: true));
    }

    [Theory]
    [InlineData("true", true)]
    [InlineData("TRUE", true)]
    [InlineData("1", true)]
    [InlineData("false", false)]
    [InlineData("0", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void EnvironmentKillSwitch_IsExplicitAndFailClosed(string? value, bool expected)
    {
        Assert.Equal(expected, MathematicsV2ProductMigrationPolicy.IsEnabledValue(value));
    }

    [Fact]
    public void PilotRenderer_ReusesTheAcceptedExactSkillRuntime()
    {
        var root = FindRoot();
        var view = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/StudentPractice/Game.cshtml"));

        Assert.Contains("MathematicsV2ProductMigrationPolicy.RendererKey", view, StringComparison.Ordinal);
        Assert.Contains("lesson-grounded-practice-v2.js", view, StringComparison.Ordinal);
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
