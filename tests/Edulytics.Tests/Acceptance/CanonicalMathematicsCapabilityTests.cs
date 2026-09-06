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
    public void UnknownNonMathematicsOutcome_FailsClosed()
    {
        const string description = "Describe the historical context of a source.";
        var skills = CanonicalMathematicsSkillMapper.Resolve("CAM:OUT:UNKNOWN", description);
        var capability = MathematicsAiCapabilityMatrix.Resolve("CAM:OUT:UNKNOWN", description);

        Assert.Empty(skills);
        Assert.Equal(MathematicsAiCapabilityLevel.ManualOnly, capability.Level);
        Assert.False(capability.CanGenerate);
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
        var capability = MathematicsAiCapabilityMatrix.Resolve(null, description);

        Assert.Contains(expectedSkill, skills);
        Assert.True(provider.Supports(expectedSkill));
        Assert.Equal(
            MathematicsGeneratorFamily.IntegerComputation,
            Assert.Single(provider.ResolveFamilies(skills)));
        Assert.Equal(MathematicsAiCapabilityLevel.VerifiedAi, capability.Level);
        Assert.True(capability.CanGenerateVerified);
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
        Assert.Equal(
            MathematicsAiCapabilityLevel.VerifiedAi,
            MathematicsAiCapabilityMatrix.Resolve(null, description).Level);
    }

    [Theory]
    [InlineData("CCSS:K.OA.A.1", "Represent addition and subtraction with objects, fingers, drawings, verbal explanations, expressions, or equations.")]
    [InlineData("CCSS:1.OA.D.7", "Understand the meaning of the equal sign, and determine if equations involving addition and subtraction are true or false.")]
    [InlineData("CCSS:1.NBT.C.4", "Add within 100 using concrete models or drawings and explain the reasoning used.")]
    [InlineData("CCSS:1.OA.A.2", "Solve word problems that call for addition of three whole numbers whose sum is less than or equal to 20.")]
    [InlineData("CCSS:1.OA.D.8", "Determine the unknown whole number in an addition or subtraction equation relating three whole numbers.")]
    public void BroadReasoningOutcomes_DoNotOverclaimNativeIntegerComputation_ButRemainContextual(
        string code,
        string description)
    {
        var skills = CanonicalMathematicsSkillMapper.Resolve(code, description);
        var capability = MathematicsAiCapabilityMatrix.Resolve(code, description);

        Assert.DoesNotContain(CanonicalMathematicsSkill.WholeNumberAdditionAndSubtraction, skills);
        Assert.DoesNotContain(CanonicalMathematicsSkill.WholeNumberAddition, skills);
        Assert.DoesNotContain(CanonicalMathematicsSkill.WholeNumberSubtraction, skills);
        Assert.Equal(MathematicsAiCapabilityLevel.AiAssisted, capability.Level);
        Assert.True(capability.CanGenerateAssisted);
        Assert.False(capability.CanGenerateVerified);
    }

    [Theory]
    [InlineData("CCSS:K.OA.A.5", "Fluently add and subtract within 5.")]
    [InlineData("CCSS:2.NBT.B.5", "Fluently add and subtract within 100 using strategies based on place value.")]
    [InlineData("CCSS:2.OA.B.2", "Fluently add and subtract within 20 using mental strategies.")]
    [InlineData("CCSS:3.NBT.A.2", "Fluently add and subtract within 1000 using strategies and algorithms based on place value.")]
    public void ReviewedAddSubtractFluencyOutcomes_KeepVerifiedIntegerCoverage(
        string code,
        string description)
    {
        var skills = CanonicalMathematicsSkillMapper.Resolve(code, description);
        var capability = MathematicsAiCapabilityMatrix.Resolve(code, description);

        Assert.Contains(CanonicalMathematicsSkill.WholeNumberAdditionAndSubtraction, skills);
        Assert.Equal(MathematicsAiCapabilityLevel.VerifiedAi, capability.Level);
        Assert.True(capability.CanGenerateVerified);
    }

    [Theory]
    [InlineData("CCSS:3.OA.C.7", "Fluently multiply and divide within 100, using strategies such as the relationship between multiplication and division.")]
    [InlineData("CCSS:4.NBT.B.4", "Fluently add and subtract multi-digit whole numbers using the standard algorithm.")]
    [InlineData("CCSS:4.NBT.B.5", "Multiply a whole number of up to four digits by a one-digit whole number, and multiply two two-digit numbers.")]
    [InlineData("CCSS:5.NBT.B.5", "Fluently multiply multi-digit whole numbers using the standard algorithm.")]
    [InlineData("CCSS:5.NBT.B.6", "Find whole-number quotients of whole numbers with up to four-digit dividends and two-digit divisors.")]
    [InlineData("CCSS:6.NS.B.2", "Fluently divide multi-digit numbers using the standard algorithm.")]
    public void OperandAndAlgorithmShapeStandards_AreContextualUntilNativeProfilesCarryThoseConstraints(
        string code,
        string description)
    {
        var skills = CanonicalMathematicsSkillMapper.Resolve(code, description);
        var capability = MathematicsAiCapabilityMatrix.Resolve(code, description);

        Assert.Empty(skills);
        Assert.Equal(MathematicsAiCapabilityLevel.AiAssisted, capability.Level);
        Assert.True(capability.CanGenerateAssisted);
    }

    [Theory]
    [InlineData("CCSS:5.NF.B.7", "Apply and extend previous understandings of division to divide unit fractions by whole numbers and whole numbers by unit fractions.")]
    [InlineData("CCSS:6.NS.B.3", "Fluently add, subtract, multiply, and divide multi-digit decimals using the standard algorithm for each operation.")]
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
        Assert.Equal(
            MathematicsAiCapabilityLevel.AiAssisted,
            MathematicsAiCapabilityMatrix.Resolve(code, description).Level);
    }

    [Theory]
    [InlineData("CCSS:5.NF.B.4", "Apply and extend understanding of multiplication to multiply a fraction.")]
    [InlineData("CCSS:3.G.A.2", "Express the area of each part as a unit fraction of the whole.")]
    [InlineData("CCSS:7.SP.C.8", "The probability of a compound event is the fraction of outcomes in the sample space for which the event occurs.")]
    public void IncidentalFractionVocabulary_DoesNotPretendToBeNativeFractionOfQuantity(
        string code,
        string description)
    {
        var skills = CanonicalMathematicsSkillMapper.Resolve(code, description);
        Assert.DoesNotContain(CanonicalMathematicsSkill.FractionOfQuantity, skills);
        Assert.Equal(
            MathematicsAiCapabilityLevel.AiAssisted,
            MathematicsAiCapabilityMatrix.Resolve(code, description).Level);
    }

    [Theory]
    [InlineData("CCSS:6.RP.A.3", "Use ratio and rate reasoning to solve real-world and mathematical problems, including unit rate and percent of a quantity as a rate per 100.")]
    [InlineData("CCSS:7.RP.A.1", "Compute unit rates associated with ratios of fractions.")]
    [InlineData("PL:REQ:TECHNICAL", "Proporcjonalność prosta")]
    [InlineData("MAT.1.07.01", "النسب والتناسب")]
    public void BroaderRatioAndProportionSemantics_AreContextualUntilMatchingNativeGeneratorExists(
        string code,
        string description)
    {
        var skills = CanonicalMathematicsSkillMapper.Resolve(code, description);
        Assert.DoesNotContain(CanonicalMathematicsSkill.UnitRateAndProportion, skills);
        Assert.Equal(
            MathematicsAiCapabilityLevel.AiAssisted,
            MathematicsAiCapabilityMatrix.Resolve(code, description).Level);
    }

    [Theory]
    [InlineData("Solve a one-step equation")]
    [InlineData("حل معادلات الخطوة الواحدة")]
    [InlineData("Solve linear equations in one variable")]
    public void EquationSemantics_AreContextualUntilNativeEquationFamilyIsCorrected(string description)
    {
        var skills = CanonicalMathematicsSkillMapper.Resolve(null, description);
        var capability = MathematicsAiCapabilityMatrix.Resolve(null, description);

        Assert.DoesNotContain(CanonicalMathematicsSkill.OneStepLinearEquation, skills);
        Assert.Equal(MathematicsAiCapabilityLevel.AiAssisted, capability.Level);
        Assert.True(capability.CanGenerateAssisted);
    }

    [Theory]
    [InlineData("MAT.2.02.04", "حل معادلات متعددة الخطوات")]
    [InlineData("MAT.2.02.07", "حل معادلات تتضمن قيمة مطلقة")]
    [InlineData("MAT.1.07.02", "النسبة المئوية للتغير")]
    public void BroaderArabicTopics_DoNotOverclaimReviewedNativeCoverage_ButRemainContextual(
        string code,
        string description)
    {
        var skills = CanonicalMathematicsSkillMapper.Resolve(code, description);
        Assert.Empty(skills);
        Assert.Equal(
            MathematicsAiCapabilityLevel.AiAssisted,
            MathematicsAiCapabilityMatrix.Resolve(code, description).Level);
    }

    [Fact]
    public void PercentageOutcome_RemainsNativeAndDoesNotEnableIntegerComputation()
    {
        var outcome = new LearningOutcome
        {
            Id = Guid.NewGuid(),
            Code = "CURRICULUM-X:PERCENT-1",
            Description = "Calculate a percentage of a quantity."
        };

        var profile = NativeMathematicsOutcomeProfileResolver.Resolve(outcome);
        var capability = NativeMathematicsOutcomeProfileResolver.ResolveCapability(outcome);

        Assert.NotNull(profile);
        Assert.False(profile!.IsContextualAssisted);
        Assert.Single(profile.CanonicalSkills);
        Assert.Equal(CanonicalMathematicsSkill.PercentageOfQuantity, profile.CanonicalSkills[0]);
        Assert.Equal(MathematicsGeneratorFamily.PercentageOfQuantity, Assert.Single(profile.AllowedFamilies));
        Assert.Equal(MathematicsAiCapabilityLevel.VerifiedAi, capability.Level);
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
    public void Resolver_NativeProfileCarriesCanonicalSkillAndProviderFamily()
    {
        var outcome = new LearningOutcome
        {
            Id = Guid.NewGuid(),
            Code = "CURRICULUM-X:ARITHMETIC-1",
            Description = "Add and subtract whole numbers."
        };

        var profile = NativeMathematicsOutcomeProfileResolver.Resolve(outcome);

        Assert.NotNull(profile);
        Assert.False(profile!.IsContextualAssisted);
        Assert.Contains(
            CanonicalMathematicsSkill.WholeNumberAdditionAndSubtraction,
            profile.CanonicalSkills);
        Assert.Contains(MathematicsGeneratorFamily.IntegerComputation, profile.AllowedFamilies);
    }
}
