using Edulytics.Core.Entities;
using Edulytics.Core.MathematicsGeneration;
using Edulytics.Services.Assessments;
using Edulytics.Services.MathematicsGeneration;

namespace Edulytics.Tests.Acceptance;

public sealed class CanonicalMathematicsCapabilityTests
{
    [Theory]
    [InlineData("", "Add and subtract whole numbers", CanonicalMathematicsSkill.WholeNumberAdditionAndSubtraction)]
    [InlineData("", "Solve a one-step equation", CanonicalMathematicsSkill.OneStepLinearEquation)]
    [InlineData("", "Find a fraction of a quantity", CanonicalMathematicsSkill.FractionOfQuantity)]
    [InlineData("", "Calculate a percentage of a quantity", CanonicalMathematicsSkill.PercentageOfQuantity)]
    [InlineData("", "Use a unit rate to solve the problem", CanonicalMathematicsSkill.UnitRateAndProportion)]
    public void Mapper_TranslatesReviewedVocabularyToCanonicalSkill(
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

    [Theory]
    [InlineData("Add whole numbers", CanonicalMathematicsSkill.WholeNumberAddition)]
    [InlineData("Subtract whole numbers", CanonicalMathematicsSkill.WholeNumberSubtraction)]
    [InlineData("Multiply whole numbers", CanonicalMathematicsSkill.WholeNumberMultiplication)]
    [InlineData("Divide whole numbers", CanonicalMathematicsSkill.WholeNumberDivision)]
    public void ExactArithmeticOperation_WithoutExactNativeGenerator_FailsClosed(
        string description,
        CanonicalMathematicsSkill expectedSkill)
    {
        var skills = CanonicalMathematicsSkillMapper.Resolve(null, description);
        var provider = new NativeMathematicsGenerationCapabilityProvider();

        Assert.Contains(expectedSkill, skills);
        Assert.False(provider.Supports(expectedSkill));
        Assert.Empty(provider.ResolveFamilies(skills));
        Assert.False(NativeMathematicsOutcomeProfileResolver.Supports(null, description));
    }

    [Fact]
    public void MixedSupportedAndUnsupportedOperations_DoNotReceivePartialAiCoverage()
    {
        const string description = "Add, subtract, multiply and divide whole numbers.";
        var skills = CanonicalMathematicsSkillMapper.Resolve(null, description);
        var provider = new NativeMathematicsGenerationCapabilityProvider();

        Assert.Contains(CanonicalMathematicsSkill.WholeNumberMultiplication, skills);
        Assert.Contains(CanonicalMathematicsSkill.WholeNumberDivision, skills);
        Assert.Empty(provider.ResolveFamilies(skills));
        Assert.False(NativeMathematicsOutcomeProfileResolver.Supports(null, description));
    }

    [Fact]
    public void PercentageOutcome_DoesNotAccidentallyEnableIntegerComputation()
    {
        var outcome = new LearningOutcome
        {
            Id = Guid.NewGuid(),
            Code = "CURRICULUM-X:PERCENT-1",
            Description = "Calculate a percentage of a quantity."
        };

        var profile = NativeMathematicsOutcomeProfileResolver.Resolve(outcome);

        Assert.NotNull(profile);
        Assert.Single(profile!.CanonicalSkills);
        Assert.Equal(CanonicalMathematicsSkill.PercentageOfQuantity, profile.CanonicalSkills[0]);
        Assert.Single(profile.AllowedFamilies);
        Assert.Equal(MathematicsGeneratorFamily.PercentageOfQuantity, profile.AllowedFamilies[0]);
    }

    [Fact]
    public void NativeProvider_AdvertisesOnlyReviewedCanonicalCapabilities()
    {
        IMathematicsGenerationCapabilityProvider provider =
            new NativeMathematicsGenerationCapabilityProvider();

        Assert.Equal("edulytics-native-mathematics", provider.ProviderKey);
        Assert.True(provider.Supports(CanonicalMathematicsSkill.WholeNumberAdditionAndSubtraction));
        Assert.False(provider.Supports(CanonicalMathematicsSkill.WholeNumberMultiplication));
        Assert.Equal(
            MathematicsGeneratorFamily.IntegerComputation,
            Assert.Single(provider.ResolveFamilies(
                [CanonicalMathematicsSkill.WholeNumberAdditionAndSubtraction])));
    }

    [Fact]
    public void Resolver_ProfileCarriesCanonicalSkillAndProviderFamily()
    {
        var outcome = new LearningOutcome
        {
            Id = Guid.NewGuid(),
            Code = "CURRICULUM-X:ARITHMETIC-1",
            Description = "Add and subtract whole numbers."
        };

        var profile = NativeMathematicsOutcomeProfileResolver.Resolve(outcome);

        Assert.NotNull(profile);
        Assert.Contains(
            CanonicalMathematicsSkill.WholeNumberAdditionAndSubtraction,
            profile!.CanonicalSkills);
        Assert.Contains(
            MathematicsGeneratorFamily.IntegerComputation,
            profile.AllowedFamilies);
    }
}
