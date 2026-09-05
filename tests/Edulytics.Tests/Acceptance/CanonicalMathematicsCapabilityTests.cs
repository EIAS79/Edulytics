using Edulytics.Core.Entities;
using Edulytics.Core.MathematicsGeneration;
using Edulytics.Services.Assessments;
using Edulytics.Services.MathematicsGeneration;

namespace Edulytics.Tests.Acceptance;

public sealed class CanonicalMathematicsCapabilityTests
{
    [Theory]
    [InlineData("", "Add and subtract whole numbers", CanonicalMathematicsSkill.WholeNumberComputation)]
    [InlineData("7.NS.1", "Apply operations with integers", CanonicalMathematicsSkill.WholeNumberComputation)]
    [InlineData("", "Solve a one-step equation", CanonicalMathematicsSkill.OneStepLinearEquation)]
    [InlineData("", "Find a fraction of a quantity", CanonicalMathematicsSkill.FractionOfQuantity)]
    [InlineData("", "Calculate a percentage of a quantity", CanonicalMathematicsSkill.PercentageOfQuantity)]
    [InlineData("", "Use ratio and unit rate", CanonicalMathematicsSkill.UnitRateAndProportion)]
    public void Mapper_TranslatesCurriculumVocabularyToCanonicalSkill(
        string code,
        string description,
        CanonicalMathematicsSkill expected)
    {
        var skills = CanonicalMathematicsSkillMapper.Resolve(code, description);

        Assert.Contains(expected, skills);
    }

    [Fact]
    public void Mapper_UnknownOutcome_FailsClosed()
    {
        var skills = CanonicalMathematicsSkillMapper.Resolve(
            "CAM:OUT:UNKNOWN",
            "Describe the historical context of a source.");

        Assert.Empty(skills);
        Assert.False(NativeMathematicsOutcomeProfileResolver.Supports(
            "CAM:OUT:UNKNOWN",
            "Describe the historical context of a source."));
    }

    [Fact]
    public void NativeProvider_AdvertisesCapabilitiesByCanonicalSkill()
    {
        IMathematicsGenerationCapabilityProvider provider =
            new NativeMathematicsGenerationCapabilityProvider();

        Assert.Equal("edulytics-native-mathematics", provider.ProviderKey);
        Assert.True(provider.Supports(CanonicalMathematicsSkill.WholeNumberComputation));
        Assert.Equal(
            MathematicsGeneratorFamily.IntegerComputation,
            Assert.Single(provider.ResolveFamilies(
                [CanonicalMathematicsSkill.WholeNumberComputation])));
    }

    [Fact]
    public void Resolver_ProfileCarriesCanonicalSkillAndProviderFamily()
    {
        var outcome = new LearningOutcome
        {
            Id = Guid.NewGuid(),
            Code = "CURRICULUM-X:ARITHMETIC-1",
            Description = "Calculate with whole numbers."
        };

        var profile = NativeMathematicsOutcomeProfileResolver.Resolve(outcome);

        Assert.NotNull(profile);
        Assert.Contains(
            CanonicalMathematicsSkill.WholeNumberComputation,
            profile!.CanonicalSkills);
        Assert.Contains(
            MathematicsGeneratorFamily.IntegerComputation,
            profile.AllowedFamilies);
    }
}
