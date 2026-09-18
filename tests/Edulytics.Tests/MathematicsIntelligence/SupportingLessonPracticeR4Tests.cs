using System.Text.Json;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class SupportingLessonPracticeR4Tests
{
    [Fact]
    public void AmbiguityDecisionsClassifyPrimarySecondaryAndPrerequisiteSkills()
    {
        var root = FindRoot();
        using var document = JsonDocument.Parse(File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Core/Mathematics/Curriculum/supporting-ambiguity-decisions.r4.v1.json")));

        var decisions = document.RootElement.GetProperty("decisions").EnumerateArray().ToArray();
        Assert.Equal(12, decisions.Length);
        Assert.All(decisions, row =>
        {
            Assert.Equal("RESOLVED", row.GetProperty("decision").GetString());
            Assert.False(string.IsNullOrWhiteSpace(row.GetProperty("primarySkill").GetString()));
        });
    }

    [Theory]
    [InlineData("PED:CAMBRIDGE-INTL-MATH:S3:3F-4:BUILD", "fractions.add_subtract.within_one")]
    [InlineData("PED:CAMBRIDGE-INTL-MATH:S4:4F-3:APPLY", "fractions.add_subtract.common_denominator_mixed")]
    [InlineData("PED:UAE-MOE-MATH:L3:COMMON:02:04:ADD-AND-SUBTRACT-RELATED-FRACTIONS", "fractions.add_subtract.related")]
    [InlineData("PED:UAE-MOE-MATH:L6:ADVANCED:02:02:ADD-AND-SUBTRACT-UNLIKE-FRACTIONS", "fractions.add_subtract.unlike_denominators")]
    public void FractionAmbiguityResolvesToFractionTargetNotWholeNumberTarget(string lessonCode, string family)
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(lessonCode, out var contract));
        Assert.NotNull(contract);
        Assert.Equal("fractions.add_subtract", contract!.SkillId);
        Assert.Contains(contract.AllowedQuestionFamilies, x => string.Equals(x, family, StringComparison.Ordinal));
    }

    [Fact]
    public void EveryFractionAddSubtractFamilyGeneratesAndVerifiesExactRationalAnswers()
    {
        var contracts = LessonPracticeContractRegistry.All
            .Where(x => x.SkillId == "fractions.add_subtract")
            .ToArray();

        Assert.NotEmpty(contracts);
        var engine = new Stage18SkillContractPracticeEngine();
        var seed = 16000;

        foreach (var contract in contracts)
        {
            var legacy = contract.ToLegacyStage18Contract();
            var items = engine.Generate(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                legacy,
                StudentPrivatePracticeDifficulty.Challenge,
                Math.Max(3, legacy.AllowedQuestionFamilies.Count),
                seed++,
                [],
                Guid.NewGuid());

            Assert.All(items, item =>
            {
                Assert.True(Stage18SkillContractPracticeEngine.VerifyPersistedItem(legacy, item));
                Assert.Contains(@"""solverVerified"":true", item.ValidationMetadataJson, StringComparison.Ordinal);
                Assert.Contains(@"""broadFallbackUsed"":false", item.ValidationMetadataJson, StringComparison.Ordinal);
            });
        }
    }

    [Fact]
    public void TwoUnknownBuildLessonUsesTwoUnknownPrimarySkill()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(
            "PED:CAMBRIDGE-INTL-MATH:S6:6AS-MD-4:BUILD",
            out var contract));
        Assert.NotNull(contract);
        Assert.Equal("algebra.relationships.two_unknowns", contract!.SkillId);
        Assert.DoesNotContain(
            contract.AllowedQuestionFamilies,
            family => family.StartsWith("number.whole.add_subtract.", StringComparison.Ordinal));
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
