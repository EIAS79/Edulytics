using System.Text.Json;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class SupportingLessonPracticeR4Tests
{
    [Fact]
    public void AmbiguousDecisionRegistryCoversAllFourteenBaselineLessons()
    {
        using var document = JsonDocument.Parse(File.ReadAllText(Path.Combine(
            FindRoot(),
            "src/Edulytics.Core/Mathematics/Curriculum/supporting-ambiguous-decisions.r4.v1.json")));

        var root = document.RootElement;
        Assert.Equal(14, root.GetProperty("baselinePopulation").GetInt32());
        Assert.Equal(14, root.GetProperty("summary").GetProperty("resolved").GetInt32());
        Assert.Equal(13, root.GetProperty("summary").GetProperty("readyVerifiedPractice").GetInt32());
        Assert.Equal(1, root.GetProperty("summary").GetProperty("contentRepairRequired").GetInt32());

        var rows = root.GetProperty("decisions").EnumerateArray().ToArray();
        Assert.Equal(14, rows.Length);
        Assert.Equal(14, rows.Select(x => x.GetProperty("lessonCode").GetString()).Distinct().Count());

        Assert.All(rows, row =>
        {
            Assert.Single(row.GetProperty("primarySkills").EnumerateArray());
            Assert.NotEmpty(row.GetProperty("allowedQuestionFamilies").EnumerateArray());
            Assert.False(string.IsNullOrWhiteSpace(row.GetProperty("evidence").GetString()));
        });
    }

    [Fact]
    public void MentionedSecondarySkillsDoNotBecomeAutomaticPracticeTargets()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(
            "PED:UAE-MOE-MATH:L6:GENERAL:02:02:ADD-AND-SUBTRACT-UNLIKE-FRACTIONS",
            out var contract));
        Assert.NotNull(contract);
        Assert.Equal("fractions.add_subtract", contract!.SkillId);
        Assert.Equal(
            new[] { "fractions.add_subtract.unlike_denominators" },
            contract.AllowedQuestionFamilies);

        Assert.True(LessonPracticeContractRegistry.TryResolve(
            "PED:US-CCSS-MATH:G4:U02:L06",
            out var benchmark));
        Assert.NotNull(benchmark);
        Assert.Equal("fractions.compare.benchmarks", benchmark!.SkillId);
        Assert.Equal(
            new[] { "fractions.compare.benchmark_half" },
            benchmark.AllowedQuestionFamilies);
    }

    [Fact]
    public void ContentWeakTwoUnknownBuildLessonIsMappedButNotYetPracticeEnabled()
    {
        Assert.False(LessonPracticeContractRegistry.TryResolve(
            "PED:CAMBRIDGE-INTL-MATH:S6:6AS-MD-4:BUILD",
            out _));

        using var document = JsonDocument.Parse(File.ReadAllText(Path.Combine(
            FindRoot(),
            "src/Edulytics.Core/Mathematics/Curriculum/supporting-ambiguous-decisions.r4.v1.json")));

        var row = document.RootElement
            .GetProperty("decisions")
            .EnumerateArray()
            .Single(x => x.GetProperty("lessonCode").GetString() ==
                "PED:CAMBRIDGE-INTL-MATH:S6:6AS-MD-4:BUILD");

        Assert.Equal("CONTENT_REPAIR_REQUIRED", row.GetProperty("practiceDecision").GetString());
        Assert.Equal("algebra.relationships.two_unknowns",
            row.GetProperty("primarySkills")[0].GetString());
    }

    [Fact]
    public void EveryR4ReadyContractGeneratesAndIndependentlyVerifies()
    {
        using var document = JsonDocument.Parse(File.ReadAllText(Path.Combine(
            FindRoot(),
            "src/Edulytics.Core/Mathematics/Curriculum/supporting-ambiguous-decisions.r4.v1.json")));

        var readyCodes = document.RootElement
            .GetProperty("decisions")
            .EnumerateArray()
            .Where(x => x.GetProperty("practiceDecision").GetString() == "READY_VERIFIED")
            .Select(x => x.GetProperty("lessonCode").GetString()!)
            .ToHashSet(StringComparer.Ordinal);

        var engine = new Stage18SkillContractPracticeEngine();
        var seed = 15000;
        foreach (var code in readyCodes)
        {
            Assert.True(LessonPracticeContractRegistry.TryResolve(code, out var contract));
            Assert.NotNull(contract);

            var legacy = contract!.ToLegacyStage18Contract();
            var items = engine.Generate(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                legacy,
                StudentPrivatePracticeDifficulty.Stretch,
                Math.Max(2, legacy.AllowedQuestionFamilies.Count),
                seed++,
                [],
                Guid.NewGuid());

            Assert.All(items, item =>
            {
                Assert.Contains(
                    legacy.AllowedQuestionFamilies,
                    family => string.Equals(family, item.GenerationFamily, StringComparison.Ordinal));
                Assert.True(Stage18SkillContractPracticeEngine.VerifyPersistedItem(legacy, item));
                Assert.Contains(@"""solverVerified"":true", item.ValidationMetadataJson, StringComparison.Ordinal);
                Assert.Contains(@"""broadFallbackUsed"":false", item.ValidationMetadataJson, StringComparison.Ordinal);
            });
        }
    }

    [Theory]
    [InlineData("PED:CAMBRIDGE-INTL-MATH:S3:3F-4:BUILD", "fractions.add_subtract.related_denominators")]
    [InlineData("PED:CAMBRIDGE-INTL-MATH:S4:4F-3:APPLY", "fractions.add_subtract.mixed_same_denominator")]
    [InlineData("PED:UAE-MOE-MATH:L6:ADVANCED:02:02:ADD-AND-SUBTRACT-UNLIKE-FRACTIONS", "fractions.add_subtract.unlike_denominators")]
    [InlineData("PED:US-CCSS-MATH:G4:U02:L02", "fractions.representations.equal_parts")]
    public void ResolvedLessonUsesTargetSpecificFamily(string lessonCode, string family)
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(lessonCode, out var contract));
        Assert.NotNull(contract);
        Assert.Contains(contract!.AllowedQuestionFamilies, x => string.Equals(x, family, StringComparison.Ordinal));
    }

    private static string FindRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null &&
               !File.Exists(Path.Combine(directory.FullName, "Edulytics.sln")))
        {
            directory = directory.Parent;
        }

        return directory?.FullName
            ?? throw new InvalidOperationException("Repository root not found.");
    }
}
