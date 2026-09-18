using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class SupportingLessonPracticeR2Tests
{
    [Fact]
    public void RegistryContainsSevenBaselinePlusThirtyThreePromotedCandidates()
    {
        Assert.True(LessonPracticeContractRegistry.All.Count >= 40);

        var bySkill = LessonPracticeContractRegistry.All
            .GroupBy(x => x.SkillId, StringComparer.Ordinal)
            .ToDictionary(x => x.Key, x => x.Count(), StringComparer.Ordinal);

        Assert.True(bySkill["ratio.unit_rate"] >= 17);
        Assert.True(bySkill["number.whole.add_subtract"] >= 12);
        Assert.True(bySkill["fractions.equivalent"] >= 6);
    }

    [Fact]
    public void WholeNumberFamiliesAreLessonSpecificAndSolverVerified()
    {
        var engine = new Stage18SkillContractPracticeEngine();
        var contracts = LessonPracticeContractRegistry.All
            .Where(x => x.SkillId == "number.whole.add_subtract")
            .ToArray();

        Assert.Equal(12, contracts.Length);

        var seed = 9000;
        foreach (var contract in contracts)
        {
            var legacy = contract.ToLegacyStage18Contract();
            var items = engine.Generate(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                legacy,
                StudentPrivatePracticeDifficulty.Stretch,
                4,
                seed++,
                [],
                Guid.NewGuid());

            Assert.Equal(4, items.Count);
            Assert.All(items, item =>
            {
                Assert.Contains(
                    contract.AllowedQuestionFamilies,
                    family => string.Equals(
                        family,
                        item.GenerationFamily,
                        StringComparison.Ordinal));
                Assert.True(Stage18SkillContractPracticeEngine.VerifyPersistedItem(legacy, item));
                Assert.Contains(@"""solverVerified"":true", item.ValidationMetadataJson, StringComparison.Ordinal);
                Assert.Contains(@"""broadFallbackUsed"":false", item.ValidationMetadataJson, StringComparison.Ordinal);
            });
        }
    }

    [Theory]
    [InlineData("PED:CAMBRIDGE-INTL-MATH:S2:2AS-1:BUILD", "number.whole.add_subtract.across_ten.build")]
    [InlineData("PED:CAMBRIDGE-INTL-MATH:S2:2NF-1:APPLY", "number.whole.add_subtract.within_10.apply")]
    [InlineData("PED:CAMBRIDGE-INTL-MATH:S3:3AS-2:BUILD", "number.whole.add_subtract.columnar.build")]
    [InlineData("PED:CAMBRIDGE-INTL-MATH:S2:2AS-4:APPLY", "number.whole.add_subtract.within_100.apply")]
    public void CambridgeWholeNumberLessonsUseExactTargetFamily(string lessonCode, string family)
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(lessonCode, out var contract));
        Assert.NotNull(contract);
        Assert.Equal("number.whole.add_subtract", contract!.SkillId);
        Assert.Single(contract.AllowedQuestionFamilies);
        Assert.Equal(family, contract.AllowedQuestionFamilies[0]);
    }

    [Fact]
    public void PromotedHighConfidenceContractsDoNotClaimOfficialOutcomeMapping()
    {
        var root = FindRoot();
        var mappingJson = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json"));

        Assert.Contains(
            "PED:UAE-MOE-MATH:L12:ADVANCED:01:06:RATES-AND-UNIT-RATES",
            mappingJson,
            StringComparison.Ordinal);
        Assert.Contains(@"""officialOutcomeMapped"": false", mappingJson, StringComparison.Ordinal);
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
