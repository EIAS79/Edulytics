using Edulytics.Core.MathematicsGeneration;
using Edulytics.Services.Assessments;

namespace Edulytics.Tests.Acceptance;

public sealed class MathematicsAiCapabilityMatrixTests
{
    [Fact]
    public void ReviewedNativeSkill_IsClassifiedAsVerifiedAi()
    {
        var capability = MathematicsAiCapabilityMatrix.Resolve(
            "CCSS:K.OA.A.5",
            "Fluently add and subtract within 5.");

        Assert.Equal(MathematicsAiCapabilityLevel.VerifiedAi, capability.Level);
        Assert.True(capability.CanGenerateVerified);
        Assert.True(capability.CanGenerate);
        Assert.Equal("edulytics-native-mathematics", capability.ProviderKey);
        Assert.Contains(
            CanonicalMathematicsSkill.WholeNumberAdditionAndSubtraction,
            capability.CanonicalSkills);
        Assert.Equal(
            MathematicsGeneratorFamily.IntegerComputation,
            Assert.Single(capability.VerifiedFamilies));
    }

    [Fact]
    public void MultiDigitStandardAlgorithm_UsesContextualAiWithoutClaimingNativeVerification()
    {
        var capability = MathematicsAiCapabilityMatrix.Resolve(
            "CCSS:4.NBT.B.4",
            "Fluently add and subtract multi-digit whole numbers using the standard algorithm.");

        Assert.Equal(MathematicsAiCapabilityLevel.AiAssisted, capability.Level);
        Assert.False(capability.CanGenerateVerified);
        Assert.True(capability.CanGenerateAssisted);
        Assert.True(capability.CanGenerate);
        Assert.Empty(capability.VerifiedFamilies);
        Assert.Equal(
            MathematicsGeneratorFamily.CurriculumContextCheck,
            Assert.Single(capability.GenerationFamilies));
        Assert.Equal("edulytics-contextual-mathematics", capability.ProviderKey);
    }

    [Fact]
    public void ReviewedWholeNumberOperationSet_IsClassifiedAsVerifiedAi()
    {
        var capability = MathematicsAiCapabilityMatrix.Resolve(
            "CURRICULUM-X:ARITHMETIC",
            "Add, subtract, multiply and divide whole numbers.");

        Assert.Equal(MathematicsAiCapabilityLevel.VerifiedAi, capability.Level);
        Assert.True(capability.CanGenerateVerified);
        Assert.Equal("edulytics-native-mathematics", capability.ProviderKey);
        Assert.Equal(
            MathematicsGeneratorFamily.IntegerComputation,
            Assert.Single(capability.VerifiedFamilies));
        Assert.Contains(
            CanonicalMathematicsSkill.WholeNumberMultiplication,
            capability.CanonicalSkills);
        Assert.Contains(
            CanonicalMathematicsSkill.WholeNumberDivision,
            capability.CanonicalSkills);
    }

    [Fact]
    public void RecognizableCurriculumMathematicsOutcome_IsAiAssistedWhenNoNativeFamilyMatches()
    {
        var capability = MathematicsAiCapabilityMatrix.Resolve(
            "CAM:OUT:0096:1Ni.02",
            "Cambridge Mathematics reference objective. Addition, subtraction and doubles.");

        Assert.Equal(MathematicsAiCapabilityLevel.AiAssisted, capability.Level);
        Assert.True(capability.CanGenerateAssisted);
        Assert.True(capability.CanGenerate);
        Assert.Equal("edulytics-contextual-mathematics", capability.ProviderKey);
        Assert.Equal(
            MathematicsGeneratorFamily.CurriculumContextCheck,
            Assert.Single(capability.GenerationFamilies));
    }

    [Fact]
    public void UnknownNonMathematicsOutcome_RemainsManualOnly()
    {
        var capability = MathematicsAiCapabilityMatrix.Resolve(
            "CAM:OUT:UNKNOWN",
            "Describe the historical context of a source.");

        Assert.Equal(MathematicsAiCapabilityLevel.ManualOnly, capability.Level);
        Assert.False(capability.CanGenerate);
        Assert.Empty(capability.CanonicalSkills);
        Assert.Equal("NoCanonicalSkillMapping", capability.ReasonCode);
    }

    [Fact]
    public void CapabilityLevelsRemainMutuallyExclusive()
    {
        var native = MathematicsAiCapabilityMatrix.Resolve(null, "Add whole numbers.");
        var assisted = MathematicsAiCapabilityMatrix.Resolve(null, "Solve a geometry problem involving area.");
        var manual = MathematicsAiCapabilityMatrix.Resolve("X", "Describe a historical source.");

        Assert.Equal(MathematicsAiCapabilityLevel.VerifiedAi, native.Level);
        Assert.Equal(MathematicsAiCapabilityLevel.AiAssisted, assisted.Level);
        Assert.Equal(MathematicsAiCapabilityLevel.ManualOnly, manual.Level);
        Assert.True(native.CanGenerateVerified);
        Assert.False(native.CanGenerateAssisted);
        Assert.False(assisted.CanGenerateVerified);
        Assert.True(assisted.CanGenerateAssisted);
        Assert.False(manual.CanGenerate);
    }
}
