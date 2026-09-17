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
    public void Stage17Catalogue_ContainsOnlyCurrentApprovedGrade16Entries()
    {
        var entries = MathematicsV2ProductMigrationPolicy.ApprovedGrade16Entries;

        Assert.Equal(3, entries.Count);
        Assert.All(entries, entry =>
        {
            Assert.InRange(entry.Grade, 1, 6);
            Assert.False(string.IsNullOrWhiteSpace(entry.LessonCode));
            Assert.False(string.IsNullOrWhiteSpace(entry.SkillId));
            Assert.False(string.IsNullOrWhiteSpace(entry.Domain));
            Assert.False(string.IsNullOrWhiteSpace(entry.Mechanic));
            Assert.Equal("READY_VERIFIED", entry.GenerationReadiness);
            Assert.False(entry.UsesV2ShadowSolver);
        });

        Assert.Equal(3, entries.Select(entry => entry.LessonCode).Distinct(StringComparer.Ordinal).Count());
        Assert.Equal(3, entries.Select(entry => entry.SkillId).Distinct(StringComparer.Ordinal).Count());
    }

    [Theory]
    [InlineData("PED:US-CCSS-MATH:G7:U06:L15", "INEQUALITY_SOLVE")]
    [InlineData("PED:US-CCSS-MATH:G8:U04:L05", "LINEAR_EQUATION_SOLVE")]
    [InlineData("PED:CAMBRIDGE-INTL-MATH:S5:NOT-APPROVED", "FRACTION_COMPARE_UNLIKE")]
    [InlineData("PED:CAMBRIDGE-INTL-MATH:S1:NOT-APPROVED", "SCALE_READING")]
    public void Stage17Gate_FailsClosedForNonApprovedLessons(string lessonCode, string mechanic)
    {
        Assert.False(MathematicsV2ProductMigrationPolicy.ShouldUseGrade16Rollout(
            lessonCode,
            mechanic,
            enabled: true));
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
        Assert.False(MathematicsV2ProductMigrationPolicy.ShouldUseGrade16Rollout(
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

    [Theory]
    [InlineData("true", "false", true)]
    [InlineData("1", "false", true)]
    [InlineData("false", "true", false)]
    [InlineData("0", "true", false)]
    [InlineData(null, "true", true)]
    [InlineData("", "1", true)]
    [InlineData(null, null, false)]
    public void Stage17EnvironmentGate_ExplicitValueWinsAndLegacyFlagRemainsCompatible(
        string? stage17Value,
        string? legacyPilotValue,
        bool expected)
    {
        Assert.Equal(
            expected,
            MathematicsV2ProductMigrationPolicy.IsEnabledFromValues(stage17Value, legacyPilotValue));
    }

    [Fact]
    public void PilotCompatibilityApi_DelegatesToStage17ReadinessGate()
    {
        Assert.Equal(
            MathematicsV2ProductMigrationPolicy.ShouldUseGrade16Rollout(
                MathematicsV2ProductMigrationPolicy.FractionCompareLessonCode,
                "FRACTION_COMPARE_UNLIKE",
                enabled: true),
            MathematicsV2ProductMigrationPolicy.ShouldUsePilot(
                MathematicsV2ProductMigrationPolicy.FractionCompareLessonCode,
                "FRACTION_COMPARE_UNLIKE",
                enabled: true));
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
