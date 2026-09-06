using Edulytics.Core.Entities;
using Edulytics.Core.MathematicsGeneration;
using Edulytics.Services.Assessments;
using Edulytics.Services.MathematicsGeneration;

namespace Edulytics.Tests.Acceptance;

public sealed class CanonicalMathematicsCapabilityTests
{
    [Theory]
    [InlineData("", "Add and subtract whole numbers", CanonicalMathematicsSkill.WholeNumberAdditionAndSubtraction)]
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
    [InlineData(
        "CCSS:1.OA.A.2",
        "Solve word problems that call for addition of three whole numbers whose sum is less than or equal to 20.")]
    [InlineData(
        "CCSS:1.OA.D.8",
        "Determine the unknown whole number in an addition or subtraction equation relating three whole numbers.")]
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

    [Theory]
    [InlineData(
        "CCSS:5.NF.B.4",
        "Apply and extend understanding of multiplication to multiply a fraction.")]
    [InlineData(
        "CCSS:3.G.A.2",
        "Express the area of each part as a unit fraction of the whole.")]
    [InlineData(
        "CCSS:7.SP.C.8",
        "The probability of a compound event is the fraction of outcomes in the sample space for which the event occurs.")]
    public void IncidentalFractionOfVocabulary_DoesNotPretendToBeFractionOfQuantity(
        string code,
        string description)
    {
        var skills = CanonicalMathematicsSkillMapper.Resolve(code, description);

        Assert.DoesNotContain(CanonicalMathematicsSkill.FractionOfQuantity, skills);
        Assert.False(NativeMathematicsOutcomeProfileResolver.Supports(code, description));
    }

    [Theory]
    [InlineData(
        "CCSS:6.RP.A.3",
        "Use ratio and rate reasoning to solve real-world and mathematical problems, including unit rate and percent of a quantity as a rate per 100.")]
    [InlineData(
        "CCSS:7.RP.A.1",
        "Compute unit rates associated with ratios of fractions.")]
    [InlineData(
        "PL:REQ:TECHNICAL",
        "Proporcjonalność prosta")]
    [InlineData(
        "MAT.1.07.01",
        "النسب والتناسب")]
    public void BroaderRatioAndProportionSemantics_RemainClosedUntilMatchingGeneratorExists(
        string code,
        string description)
    {
        var skills = CanonicalMathematicsSkillMapper.Resolve(code, description);

        Assert.DoesNotContain(CanonicalMathematicsSkill.UnitRateAndProportion, skills);
        Assert.False(NativeMathematicsOutcomeProfileResolver.Supports(code, description));
    }

    [Theory]
    [InlineData("Solve a one-step equation")]
    [InlineData("حل معادلات الخطوة الواحدة")]
    [InlineData("Solve linear equations in one variable")]
    public void EquationSemantics_RemainClosedUntilOneStepGeneratorIsCorrected(string description)
    {
        var skills = CanonicalMathematicsSkillMapper.Resolve(null, description);

        Assert.DoesNotContain(CanonicalMathematicsSkill.OneStepLinearEquation, skills);
        Assert.False(NativeMathematicsOutcomeProfileResolver.Supports(null, description));
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
