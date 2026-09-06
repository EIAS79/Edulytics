using Edulytics.Core.Entities;
using Edulytics.Core.MathematicsGeneration;
using Edulytics.Services.Assessments;

namespace Edulytics.Tests.Acceptance;

public sealed class CrossCurriculumSemanticHintTests
{
    [Fact]
    public void PolishDirectProportion_RemainsClosedUntilProportionGeneratorExists()
    {
        var skills = CanonicalMathematicsSkillMapper.Resolve(
            "PL:REQ:TECHNICAL",
            "Proporcjonalność prosta");

        Assert.DoesNotContain(CanonicalMathematicsSkill.UnitRateAndProportion, skills);
        Assert.False(NativeMathematicsOutcomeProfileResolver.Supports(
            "PL:REQ:TECHNICAL",
            "Proporcjonalność prosta"));
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

        Assert.Null(profile);
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
    public void MixedUaeEquationHint_RemainsClosedInsteadOfSelectingOneNarrowLesson()
    {
        var outcome = new LearningOutcome
        {
            Id = Guid.NewGuid(),
            Code = "UAE:STD:MAT.2.02.04",
            Description = "Reference-only UAE Mathematics standard",
            GenerationSemanticHint =
                "حل معادلات الخطوة الواحدة | حل معادلات متعددة الخطوات | حل معادلات تتضمن متغيرًا في كل طرف"
        };

        Assert.Null(NativeMathematicsOutcomeProfileResolver.Resolve(outcome));
        Assert.False(NativeMathematicsOutcomeProfileResolver.Supports(outcome));
    }

    [Fact]
    public void CambridgeEarlyAdditionHint_RemainsClosedUntilExplicitSkillMappingIsReviewed()
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
