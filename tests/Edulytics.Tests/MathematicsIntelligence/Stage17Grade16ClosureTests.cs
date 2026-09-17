using System.Text.Json;
using Edulytics.Web.GameRouting;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class Stage17Grade16ClosureTests
{
    [Fact]
    public void ProductionPolicyMatchesCommittedClosureManifest()
    {
        var root = FindRoot();
        using var document = JsonDocument.Parse(File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Core/Mathematics/Curriculum/stage17-grade1-6-production-manifest.v1.json")));

        var entries = document.RootElement.GetProperty("entries").EnumerateArray().ToArray();
        Assert.Equal(14, entries.Length);

        var manifestByCode = entries.ToDictionary(
            entry => entry.GetProperty("lessonCode").GetString()!,
            StringComparer.Ordinal);

        var policy = MathematicsV2ProductMigrationPolicy.ApprovedGrade16Entries;
        Assert.Equal(entries.Length, policy.Count);

        foreach (var entry in policy)
        {
            Assert.True(manifestByCode.TryGetValue(entry.LessonCode, out var manifest));
            Assert.Equal(entry.SkillId, manifest.GetProperty("skillId").GetString());
            Assert.Equal(entry.Domain, manifest.GetProperty("domain").GetString());
            Assert.Equal(entry.Grade, manifest.GetProperty("grade").GetInt32());
            Assert.Equal(entry.Mechanic, manifest.GetProperty("mechanic").GetString());
            Assert.Equal(entry.GenerationReadiness, manifest.GetProperty("generationReadiness").GetString());
            Assert.Equal(entry.UsesV2ShadowSolver, manifest.GetProperty("usesV2ShadowSolver").GetBoolean());
            Assert.InRange(entry.Grade, 1, 6);
            Assert.Equal("READY_VERIFIED", entry.GenerationReadiness);
            Assert.False(entry.UsesV2ShadowSolver);
        }

        Assert.Equal(14, policy.Select(x => x.LessonCode).Distinct(StringComparer.Ordinal).Count());
    }

    [Theory]
    [InlineData("TWO_UNKNOWNS")]
    [InlineData("SCALE_READING")]
    [InlineData("FRACTION_COMPARE_UNLIKE")]
    [InlineData("FRACTION_EQUIVALENT")]
    [InlineData("UNIT_RATE")]
    public void EveryStage17MechanicLoadsTheExactRuntime(string mechanic)
    {
        var root = FindRoot();
        var view = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/StudentPractice/Game.cshtml"));

        Assert.Contains($"Model.Route.Mechanic, \"{mechanic}\"", view, StringComparison.Ordinal);
        Assert.Contains("lesson-grounded-practice-v2.js", view, StringComparison.Ordinal);
    }

    [Fact]
    public void RepresentativeOfficialLessonsRouteByExactApprovedCode()
    {
        var equivalent = GameLessonRouteResolver.Resolve(
            "PED:US-CCSS-MATH:G4:U02:L08",
            "Fractions",
            "Equivalent Fractions on the Number Line PLC Activity",
            "Number-line equivalence evidence.",
            requireLessonGrounding: true,
            enableMathematicsV2Pilot: true);

        Assert.True(equivalent.IsPlayable);
        Assert.Equal("FRACTION_EQUIVALENT", equivalent.Mechanic);
        Assert.Equal(MathematicsV2ProductMigrationPolicy.RendererKey, equivalent.RendererKey);

        var unitRate = GameLessonRouteResolver.Resolve(
            MathematicsV2ProductMigrationPolicy.UnitRateLessonCode,
            "Ratios and Rates",
            "Equivalent Ratios Have the Same Unit Rates",
            "Equivalent ratios preserve the same unit rate.",
            requireLessonGrounding: true,
            enableMathematicsV2Pilot: true);

        Assert.True(unitRate.IsPlayable);
        Assert.Equal("UNIT_RATE", unitRate.Mechanic);
        Assert.Equal(MathematicsV2ProductMigrationPolicy.RendererKey, unitRate.RendererKey);
    }

    [Fact]
    public void SimilarButUnapprovedFractionLessonRemainsFailClosed()
    {
        const string lessonCode = "PED:US-CCSS-MATH:G4:U02:L13";

        Assert.False(MathematicsV2ProductMigrationPolicy.TryGetApprovedGrade16Entry(
            lessonCode,
            out _));

        Assert.False(MathematicsV2ProductMigrationPolicy.ShouldUseGrade16Rollout(
            lessonCode,
            "FRACTION_EQUIVALENT",
            enabled: true));
    }

    [Fact]
    public void ExactRuntimeContainsRepresentationSpecificFractionModesAndUnitRateVerification()
    {
        var root = FindRoot();
        var script = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/js/lesson-grounded-practice-v2.js"));

        Assert.Contains("fractionEquivalentNumberLineQuestion()", script, StringComparison.Ordinal);
        Assert.Contains("fractionEquivalentFactorQuestion()", script, StringComparison.Ordinal);
        Assert.Contains("baseN * d === n * baseD", script, StringComparison.Ordinal);
        Assert.Contains("Equivalent ratios have the same rate per 1 minute.", script, StringComparison.Ordinal);
        Assert.Contains("unitRateQuestion()", script, StringComparison.Ordinal);
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
