using Edulytics.Core.MathematicsGeneration;
using Edulytics.Services.Assessments;

namespace Edulytics.Tests.Acceptance;

public sealed class MathematicsAiCapabilityMatrixTests
{
    [Fact]
    public void ReviewedNativeSkill_IsClassifiedAsVerifiedAi()
    {
        var capability = MathematicsAiCapabilityMatrix.Resolve(
            "CCSS:4.NBT.B.4",
            "Fluently add and subtract multi-digit whole numbers.");

        Assert.Equal(MathematicsAiCapabilityLevel.VerifiedAi, capability.Level);
        Assert.True(capability.CanGenerateVerified);
        Assert.Equal("edulytics-native-mathematics", capability.ProviderKey);
        Assert.Contains(
            CanonicalMathematicsSkill.WholeNumberAdditionAndSubtraction,
            capability.CanonicalSkills);
        Assert.Equal(
            MathematicsGeneratorFamily.IntegerComputation,
            Assert.Single(capability.VerifiedFamilies));
    }

    [Fact]
    public void PartiallyUnsupportedSkillSet_IsManualOnlyFailClosed()
    {
        var capability = MathematicsAiCapabilityMatrix.Resolve(
            "CURRICULUM-X:ARITHMETIC",
            "Add, subtract, multiply and divide whole numbers.");

        Assert.Equal(MathematicsAiCapabilityLevel.ManualOnly, capability.Level);
        Assert.False(capability.CanGenerateVerified);
        Assert.Null(capability.ProviderKey);
        Assert.Empty(capability.VerifiedFamilies);
        Assert.Contains(
            CanonicalMathematicsSkill.WholeNumberMultiplication,
            capability.CanonicalSkills);
        Assert.Contains(
            CanonicalMathematicsSkill.WholeNumberDivision,
            capability.CanonicalSkills);
    }

    [Fact]
    public void UnknownOutcome_IsManualOnlyWithoutInventingASkill()
    {
        var capability = MathematicsAiCapabilityMatrix.Resolve(
            "CAM:OUT:UNKNOWN",
            "Reference-only curriculum outcome.");

        Assert.Equal(MathematicsAiCapabilityLevel.ManualOnly, capability.Level);
        Assert.Empty(capability.CanonicalSkills);
        Assert.Equal("NoCanonicalSkillMapping", capability.ReasonCode);
    }

    [Fact]
    public void AiAssisted_IsAReservedState_NotEmittedWithoutAConfiguredProvider()
    {
        var samples = new[]
        {
            MathematicsAiCapabilityMatrix.Resolve(null, "Add whole numbers."),
            MathematicsAiCapabilityMatrix.Resolve(null, "Multiply whole numbers."),
            MathematicsAiCapabilityMatrix.Resolve("CAM:OUT:UNKNOWN", "Reference-only curriculum outcome.")
        };

        Assert.DoesNotContain(
            samples,
            x => x.Level == MathematicsAiCapabilityLevel.AiAssisted);
    }
}
