using Edulytics.Core.Entities;
using Edulytics.Core.MathematicsGeneration;
using Edulytics.Services.Assessments;
using Edulytics.Services.MathematicsGeneration;

namespace Edulytics.Tests.Acceptance;

public sealed class CrossCurriculumSemanticHintTests
{
    [Fact]
    public void PolishDirectProportion_MapsToReviewedUnitRateFamily()
    {
        var skills = CanonicalMathematicsSkillMapper.Resolve(
            "PL:REQ:TECHNICAL",
            "Proporcjonalność prosta");
        var provider = new NativeMathematicsGenerationCapabilityProvider();

        Assert.Contains(CanonicalMathematicsSkill.UnitRateAndProportion, skills);
        Assert.Equal(
            MathematicsGeneratorFamily.UnitRateWordProblem,
            Assert.Single(provider.ResolveFamilies(skills)));
    }

    [Fact]
    public void Resolver_UsesTransientPedagogicalSemanticHintWithoutChangingOfficialDescription()
    {
        const string officialDescription =
            "VII-VIII requirement PL:VII-VIII:PROPORCJONALNOSC:core:1:001";
        var outcome = new LearningOutcome
        {
            Id = Guid.NewGuid(),
            Code = "PL:REQ:PL:VII-VIII:PROPORCJONALNOSC:core:1:001",
            Description = officialDescription,
            GenerationSemanticHint =
                "Proporcjonalność prosta :: Proporcjonalność prosta — Lesson 01"
        };

        var profile = NativeMathematicsOutcomeProfileResolver.Resolve(outcome);

        Assert.NotNull(profile);
        Assert.Equal(officialDescription, outcome.Description);
        Assert.Contains(CanonicalMathematicsSkill.UnitRateAndProportion, profile!.CanonicalSkills);
        Assert.Contains(MathematicsGeneratorFamily.UnitRateWordProblem, profile.AllowedFamilies);
    }

    [Theory]
    [InlineData("Równania z jedną niewiadomą")]
    [InlineData("Równania i nierówności")]
    [InlineData("Obliczenia procentowe")]
    [InlineData("Ułamki zwykłe i dziesiętne")]
    public void BroadPolishUnitTitles_RemainManualUntilMatchingGeneratorExists(
        string semanticHint)
    {
        var outcome = new LearningOutcome
        {
            Id = Guid.NewGuid(),
            Code = "PL:REQ:TECHNICAL",
            Description = "Reference-only curriculum requirement",
            GenerationSemanticHint = semanticHint
        };

        Assert.Null(NativeMathematicsOutcomeProfileResolver.Resolve(outcome));
        Assert.False(NativeMathematicsOutcomeProfileResolver.Supports(outcome));
    }

    [Fact]
    public void CambridgeEarlyAdditionHint_RemainsClosedUntilLevelAwareGenerationIsEnabled()
    {
        var outcome = new LearningOutcome
        {
            Id = Guid.NewGuid(),
            Code = "CAM:OUT:0096:1Ni.02",
            Description = "Cambridge reference objective 1Ni.02",
            GenerationSemanticHint =
                "Addition, Subtraction and Doubles :: Join Groups to Add"
        };

        Assert.Null(NativeMathematicsOutcomeProfileResolver.Resolve(outcome));
    }
}
