using Edulytics.Core.Entities;
using Edulytics.Core.MathematicsGeneration;
using Edulytics.Services.Assessments;

namespace Edulytics.Tests.Acceptance;

public sealed class CrossCurriculumSemanticHintTests
{
    [Fact]
    public void PolishDirectProportion_UsesContextualAiWithoutClaimingNativeUnitRate()
    {
        var skills = CanonicalMathematicsSkillMapper.Resolve(
            "PL:REQ:TECHNICAL",
            "Proporcjonalność prosta");
        var capability = MathematicsAiCapabilityMatrix.Resolve(
            "PL:REQ:TECHNICAL",
            "Proporcjonalność prosta");

        Assert.DoesNotContain(CanonicalMathematicsSkill.UnitRateAndProportion, skills);
        Assert.Equal(MathematicsAiCapabilityLevel.AiAssisted, capability.Level);
        Assert.True(capability.CanGenerateAssisted);
        Assert.False(capability.CanGenerateVerified);
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
        Assert.True(profile!.IsContextualAssisted);
        Assert.Equal(
            MathematicsGeneratorFamily.CurriculumContextCheck,
            Assert.Single(profile.AllowedFamilies));
        Assert.Equal(officialDescription, outcome.Description);
        Assert.Equal(
            "Proporcjonalność prosta :: Proporcjonalność prosta — Lesson 01",
            outcome.GenerationSemanticHint);
    }

    [Theory]
    [InlineData("Równania z jedną niewiadomą")]
    [InlineData("Równania i nierówności")]
    [InlineData("Obliczenia procentowe")]
    [InlineData("Ułamki zwykłe i dziesiętne")]
    [InlineData("Proporcjonalność prosta")]
    public void BroadPolishUnitTitles_AreContextualAiUntilMatchingNativeGeneratorExists(
        string semanticHint)
    {
        var outcome = new LearningOutcome
        {
            Id = Guid.NewGuid(),
            Code = "PL:REQ:TECHNICAL",
            Description = "Reference-only curriculum requirement",
            GenerationSemanticHint = semanticHint
        };

        var profile = NativeMathematicsOutcomeProfileResolver.Resolve(outcome);
        var capability = NativeMathematicsOutcomeProfileResolver.ResolveCapability(outcome);

        Assert.NotNull(profile);
        Assert.True(profile!.IsContextualAssisted);
        Assert.Equal(MathematicsAiCapabilityLevel.AiAssisted, capability.Level);
        Assert.True(capability.CanGenerateAssisted);
    }

    [Fact]
    public void MixedUaeEquationHint_UsesContextualAiInsteadOfSelectingOneNarrowNativeLesson()
    {
        var outcome = new LearningOutcome
        {
            Id = Guid.NewGuid(),
            Code = "UAE:STD:MAT.2.02.04",
            Description = "Reference-only UAE Mathematics standard",
            GenerationSemanticHint =
                "حل معادلات الخطوة الواحدة | حل معادلات متعددة الخطوات | حل معادلات تتضمن متغيرًا في كل طرف"
        };

        var profile = NativeMathematicsOutcomeProfileResolver.Resolve(outcome);
        var capability = NativeMathematicsOutcomeProfileResolver.ResolveCapability(outcome);

        Assert.NotNull(profile);
        Assert.True(profile!.IsContextualAssisted);
        Assert.Equal(MathematicsAiCapabilityLevel.AiAssisted, capability.Level);
        Assert.False(capability.CanGenerateVerified);
    }

    [Fact]
    public void CambridgeEarlyAdditionHint_UsesContextualAiUntilExplicitNativeSkillMappingIsReviewed()
    {
        var outcome = new LearningOutcome
        {
            Id = Guid.NewGuid(),
            Code = "CAM:OUT:0096:1Ni.02",
            Description = "Cambridge Mathematics reference objective 1Ni.02",
            GenerationSemanticHint =
                "Addition, Subtraction and Doubles :: Join Groups to Add"
        };

        var profile = NativeMathematicsOutcomeProfileResolver.Resolve(outcome);
        var capability = NativeMathematicsOutcomeProfileResolver.ResolveCapability(outcome);

        Assert.NotNull(profile);
        Assert.True(profile!.IsContextualAssisted);
        Assert.Equal(MathematicsAiCapabilityLevel.AiAssisted, capability.Level);
    }
}
