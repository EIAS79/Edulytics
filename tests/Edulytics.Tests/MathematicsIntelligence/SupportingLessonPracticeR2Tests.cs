using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class SupportingLessonPracticeR2Tests
{
    [Fact]
    public void HighConfidenceSupportingCandidatesAreExplicitlyPromoted()
    {
        var expected = new[]
        {
            "PED:CAMBRIDGE-INTL-MATH:L7:SHARED:01:06:RATES-AND-UNIT-RATES",
            "PED:CAMBRIDGE-INTL-MATH:L8:SHARED:01:06:RATES-AND-UNIT-RATES",
            "PED:CAMBRIDGE-INTL-MATH:L9:SHARED:01:06:RATES-AND-UNIT-RATES",
            "PED:CAMBRIDGE-INTL-MATH:S2:2AS-1:BUILD",
            "PED:CAMBRIDGE-INTL-MATH:S2:2AS-1:APPLY",
            "PED:CAMBRIDGE-INTL-MATH:S2:2AS-3:BUILD",
            "PED:CAMBRIDGE-INTL-MATH:S2:2AS-3:APPLY",
            "PED:CAMBRIDGE-INTL-MATH:S2:2AS-4:BUILD",
            "PED:CAMBRIDGE-INTL-MATH:S2:2AS-4:APPLY",
            "PED:CAMBRIDGE-INTL-MATH:S2:2NF-1:BUILD",
            "PED:CAMBRIDGE-INTL-MATH:S2:2NF-1:APPLY",
            "PED:CAMBRIDGE-INTL-MATH:S3:3AS-2:BUILD",
            "PED:CAMBRIDGE-INTL-MATH:S3:3AS-2:APPLY",
            "PED:CAMBRIDGE-INTL-MATH:S3:3NF-1:BUILD",
            "PED:CAMBRIDGE-INTL-MATH:S3:3NF-1:APPLY",
            "PED:UAE-MOE-MATH:L10:ADVANCED:01:06:RATES-AND-UNIT-RATES",
            "PED:UAE-MOE-MATH:L10:GENERAL:01:06:RATES-AND-UNIT-RATES",
            "PED:UAE-MOE-MATH:L11:ADVANCED:01:06:RATES-AND-UNIT-RATES",
            "PED:UAE-MOE-MATH:L11:GENERAL:01:06:RATES-AND-UNIT-RATES",
            "PED:UAE-MOE-MATH:L12:ADVANCED:01:06:RATES-AND-UNIT-RATES",
            "PED:UAE-MOE-MATH:L12:GENERAL:01:06:RATES-AND-UNIT-RATES",
            "PED:UAE-MOE-MATH:L3:COMMON:02:02:EQUIVALENT-FRACTIONS",
            "PED:UAE-MOE-MATH:L4:COMMON:02:01:EQUIVALENT-FRACTIONS",
            "PED:UAE-MOE-MATH:L5:ADVANCED:02:01:EQUIVALENT-FRACTIONS",
            "PED:UAE-MOE-MATH:L5:ADVANCED:03:03:UNIT-RATE",
            "PED:UAE-MOE-MATH:L5:GENERAL:02:01:EQUIVALENT-FRACTIONS",
            "PED:UAE-MOE-MATH:L5:GENERAL:03:03:UNIT-RATE",
            "PED:UAE-MOE-MATH:L7:ADVANCED:01:06:RATES-AND-UNIT-RATES",
            "PED:UAE-MOE-MATH:L7:GENERAL:01:06:RATES-AND-UNIT-RATES",
            "PED:UAE-MOE-MATH:L8:ADVANCED:01:06:RATES-AND-UNIT-RATES",
            "PED:UAE-MOE-MATH:L8:GENERAL:01:06:RATES-AND-UNIT-RATES",
            "PED:UAE-MOE-MATH:L9:ADVANCED:01:06:RATES-AND-UNIT-RATES",
            "PED:UAE-MOE-MATH:L9:GENERAL:01:06:RATES-AND-UNIT-RATES"
        };

        Assert.Equal(33, expected.Length);
        Assert.Equal(40, LessonPracticeContractRegistry.All.Count);

        foreach (var code in expected)
        {
            Assert.True(LessonPracticeContractRegistry.TryResolve(code, out var contract));
            Assert.NotNull(contract);
            Assert.Equal("READY_VERIFIED", contract!.Readiness);
            Assert.Equal("SupportingLesson", contract.SourceType);
            Assert.NotEmpty(contract.AllowedQuestionFamilies);
        }
    }

    [Fact]
    public void WholeNumberFamiliesRemainExactAndReconstructableAcrossDifficultyBands()
    {
        var contracts = LessonPracticeContractRegistry.All
            .Where(x => string.Equals(
                x.SkillId,
                "number.whole.add_subtract",
                StringComparison.Ordinal))
            .ToArray();

        Assert.Equal(12, contracts.Length);

        var engine = new Stage18SkillContractPracticeEngine();
        var seed = 9000;
        foreach (var contract in contracts)
        {
            foreach (var difficulty in new[]
                     {
                         StudentPrivatePracticeDifficulty.AtClassLevel,
                         StudentPrivatePracticeDifficulty.Stretch,
                         StudentPrivatePracticeDifficulty.Challenge
                     })
            {
                var legacy = contract.ToLegacyStage18Contract();
                var items = engine.Generate(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    legacy,
                    difficulty,
                    3,
                    seed++,
                    [],
                    Guid.NewGuid());

                Assert.Equal(3, items.Count);
                Assert.All(items, item =>
                {
                    Assert.StartsWith("number.whole.add_subtract.", item.GenerationFamily);
                    Assert.True(Stage18SkillContractPracticeEngine.VerifyPersistedItem(legacy, item));
                    Assert.Contains("\"solverVerified\":true", item.ValidationMetadataJson, StringComparison.Ordinal);
                });
            }
        }
    }
}
