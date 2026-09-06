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
    [InlineData("MAT.2.02.04", "حل معادلات الخطوة الواحدة", CanonicalMathematicsSkill.OneStepLinearEquation)]
    [InlineData("MAT.1.07.01", "النسب والتناسب", CanonicalMathematicsSkill.UnitRateAndProportion)]
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
    public void ExactWholeNumberOperation_HasReviewedNativeCoverage(
        string description,
        CanonicalMathematicsSkill expectedSkill)
    {
        var skills = CanonicalMathematicsSkillMapper.Resolve(null, description);
        var provider = new NativeMathematicsGenerationCapabilityProvider();

        Assert.Contains(expectedSkill, skills);
        Assert.True(provider.Supports(expectedSkill));
        Assert.Equal(
            MathematicsGeneratorFamily.IntegerComputation,
            Assert.Single(provider.ResolveFamilies(skills)));
        Assert.True(NativeMathematicsOutcomeProfileResolver.Supports(null, description));
    }

    [Fact]
    public void MixedReviewedWholeNumberOperations_ResolveToOneVerifiedFamily()
    {
        const string description = "Add, subtract, multiply and divide whole numbers.";
        var skills = CanonicalMathematicsSkillMapper.Resolve(null, description);
        var provider = new NativeMathematicsGenerationCapabilityProvider();

        Assert.Contains(CanonicalMathematicsSkill.WholeNumberAddition, skills);
        Assert.Contains(CanonicalMathematicsSkill.WholeNumberSubtraction, skills);
        Assert.Contains(CanonicalMathematicsSkill.WholeNumberMultiplication, skills);
        Assert.Contains(CanonicalMathematicsSkill.WholeNumberDivision, skills);
        Assert.Equal(
            MathematicsGeneratorFamily.IntegerComputation,
            Assert.Single(provider.ResolveFamilies(skills)));
        Assert.True(NativeMathematicsOutcomeProfileResolver.Supports(null, description));
    }

    [Theory]
    [InlineData(
        "CCSS:K.OA.A.1",
        "Represent addition and subtraction with objects, fingers, mental images, drawings, sounds, acting out situations, verbal explanations, expressions, or equations.")]
    [InlineData(
        "CCSS:1.OA.D.7",
        "Understand the meaning of the equal sign, and determine if equations involving addition and subtraction are true or false.")]
    [InlineData(
        "CCSS:1.NBT.C.4",
        "Add within 100, including adding a two-digit number and a one-digit number, using concrete models or drawings and explain the reasoning used.")]
    public void BroadOaNbtReasoningOutcomes_DoNotOverclaimIntegerComputation(
        string code,
        string description)
    {
        var skills = CanonicalMathematicsSkillMapper.Resolve(code, description);

        Assert.DoesNotContain(CanonicalMathematicsSkill.WholeNumberAdditionAndSubtraction, skills);
        Assert.DoesNotContain(CanonicalMathematicsSkill.WholeNumberAddition, skills);
        Assert.DoesNotContain(CanonicalMathematicsSkill.WholeNumberSubtraction, skills);
        Assert.False(NativeMathematicsOutcomeProfileResolver.Supports(code, description));
    }

    [Theory]
    [InlineData(
        "CCSS:K.OA.A.5",
        "Fluently add and subtract within 5.",
        CanonicalMathematicsSkill.WholeNumberAdditionAndSubtraction)]
    [InlineData(
        "CCSS:3.OA.C.7",
        "Fluently multiply and divide within 100.",
        CanonicalMathematicsSkill.WholeNumberMultiplication)]
    public void ExplicitFluencyOutcomes_KeepReviewedIntegerCoverage(
        string code,
        string description,
        CanonicalMathematicsSkill expectedSkill)
    {
        var skills = CanonicalMathematicsSkillMapper.Resolve(code, description);

        Assert.Contains(expectedSkill, skills);
        Assert.True(NativeMathematicsOutcomeProfileResolver.Supports(code, description));
    }

    [Theory]
    [InlineData(
        "CCSS:5.NF.B.7",
        "Apply and extend previous understandings of division to divide unit fractions by whole numbers and whole numbers by unit fractions.")]
    [InlineData(
        "CCSS:6.NS.B.3",
        "Fluently add, subtract, multiply, and divide multi-digit decimals using the standard algorithm for each operation.")]
    public void FractionAndDecimalContexts_DoNotBecomeWholeNumberComputation(
        string code,
        string description)
    {
        var skills = CanonicalMathematicsSkillMapper.Resolve(code, description);

        Assert.DoesNotContain(CanonicalMathematicsSkill.WholeNumberAdditionAndSubtraction, skills);
        Assert.DoesNotContain(CanonicalMathematicsSkill.WholeNumberAddition, skills);
        Assert.DoesNotContain(CanonicalMathematicsSkill.WholeNumberSubtraction, skills);
        Assert.DoesNotContain(CanonicalMathematicsSkill.WholeNumberMultiplication, skills);
        Assert.DoesNotContain(CanonicalMathematicsSkill.WholeNumberDivision, skills);
    }

    [Fact]
    public void GeneralFractionMultiplication_DoesNotPretendToBeFractionOfQuantity()
    {
        const string code = "CCSS:5.NF.B.4";
        const string description =
            "Apply and extend understanding of multiplication to multiply a fraction.";

        var skills = CanonicalMathematicsSkillMapper.Resolve(code, description);

        Assert.DoesNotContain(CanonicalMathematicsSkill.FractionOfQuantity, skills);
        Assert.DoesNotContain(CanonicalMathematicsSkill.WholeNumberMultiplication, skills);
        Assert.False(NativeMathematicsOutcomeProfileResolver.Supports(code, description));
    }

    [Fact]
    public void RatioRateVocabulary_WithRpLocator_MapsToUnitRate()
    {
        var skills = CanonicalMathematicsSkillMapper.Resolve(
            "CCSS:6.RP.A.3",
            "Use ratio and rate reasoning.");

        Assert.Contains(CanonicalMathematicsSkill.UnitRateAndProportion, skills);
        Assert.True(NativeMathematicsOutcomeProfileResolver.Supports(
            "CCSS:6.RP.A.3",
            "Use ratio and rate reasoning."));
    }

    [Theory]
    [InlineData("MAT.2.02.04", "حل معادلات متعددة الخطوات")]
    [InlineData("MAT.2.02.07", "حل معادلات تتضمن قيمة مطلقة")]
    [InlineData("MAT.1.07.02", "النسبة المئوية للتغير")]
    public void BroaderArabicTopics_DoNotOverclaimReviewedNativeCoverage(
        string code,
        string description)
    {
        var skills = CanonicalMathematicsSkillMapper.Resolve(code, description);

        Assert.Empty(skills);
        Assert.False(NativeMathematicsOutcomeProfileResolver.Supports(code, description));
    }

    [Fact]
    public void ReviewedArabicOutcomeSemantics_AreAvailableThroughSharedResolver()
    {
        var equation = new LearningOutcome
        {
            Id = Guid.NewGuid(),
            Code = "MAT.2.02.04",
            Description = "حل معادلات الخطوة الواحدة"
        };
        var proportion = new LearningOutcome
        {
            Id = Guid.NewGuid(),
            Code = "MAT.1.07.01",
            Description = "النسب والتناسب"
        };

        var equationProfile = NativeMathematicsOutcomeProfileResolver.Resolve(equation);
        var proportionProfile = NativeMathematicsOutcomeProfileResolver.Resolve(proportion);

        Assert.NotNull(equationProfile);
        Assert.Contains(CanonicalMathematicsSkill.OneStepLinearEquation, equationProfile!.CanonicalSkills);
        Assert.Contains(MathematicsGeneratorFamily.OneStepEquation, equationProfile.AllowedFamilies);

        Assert.NotNull(proportionProfile);
        Assert.Contains(CanonicalMathematicsSkill.UnitRateAndProportion, proportionProfile!.CanonicalSkills);
        Assert.Contains(MathematicsGeneratorFamily.UnitRateWordProblem, proportionProfile.AllowedFamilies);
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
    public void NativeProvider_AdvertisesReviewedCanonicalCapabilities()
    {
        IMathematicsGenerationCapabilityProvider provider =
            new NativeMathematicsGenerationCapabilityProvider();

        Assert.Equal("edulytics-native-mathematics", provider.ProviderKey);
        Assert.True(provider.Supports(CanonicalMathematicsSkill.WholeNumberAdditionAndSubtraction));
        Assert.True(provider.Supports(CanonicalMathematicsSkill.WholeNumberAddition));
        Assert.True(provider.Supports(CanonicalMathematicsSkill.WholeNumberSubtraction));
        Assert.True(provider.Supports(CanonicalMathematicsSkill.WholeNumberMultiplication));
        Assert.True(provider.Supports(CanonicalMathematicsSkill.WholeNumberDivision));
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
