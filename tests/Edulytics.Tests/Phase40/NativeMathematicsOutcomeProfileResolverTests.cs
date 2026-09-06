using Edulytics.Core.Entities;
using Edulytics.Core.MathematicsGeneration;
using Edulytics.Services.Assessments;

namespace Edulytics.Tests.Phase40;

public sealed class NativeMathematicsOutcomeProfileResolverTests
{
    [Theory]
    [InlineData("MATH-UNIT-RATE", "Use a unit rate to find the total quantity", MathematicsGeneratorFamily.UnitRateWordProblem)]
    [InlineData("MATH-FRACTION-OF", "Find a fraction of a quantity", MathematicsGeneratorFamily.FractionOfQuantity)]
    [InlineData("MATH-PERCENT", "Calculate a percentage of a quantity", MathematicsGeneratorFamily.PercentageOfQuantity)]
    [InlineData("CCSS:K.OA.A.5", "Fluently add and subtract within 5.", MathematicsGeneratorFamily.IntegerComputation)]
    public void Resolve_MapsClearlySupportedOutcomeToTrustedNativeFamily(
        string code,
        string description,
        MathematicsGeneratorFamily expected)
    {
        var outcome = new LearningOutcome { Id = Guid.NewGuid(), Code = code, Description = description };

        var profile = NativeMathematicsOutcomeProfileResolver.Resolve(outcome);
        var capability = NativeMathematicsOutcomeProfileResolver.ResolveCapability(outcome);

        Assert.NotNull(profile);
        Assert.False(profile!.IsContextualAssisted);
        Assert.True(capability.CanGenerateVerified);
        Assert.Contains(expected, profile.AllowedFamilies);
    }

    [Theory]
    [InlineData("CCSS:4.NBT.B.4", "Fluently add and subtract multi-digit whole numbers using the standard algorithm.")]
    [InlineData("MATH-ONE-STEP", "Solve a one-step equation")]
    [InlineData("CCSS:5.NF.B.4", "Apply and extend understanding of multiplication to multiply a fraction")]
    [InlineData("CCSS:7.G.A.1", "Solve problems involving scale drawings of geometric figures")]
    public void Resolve_UsesContextualAssistedFamilyWhenNativeCoverageIsIncomplete(
        string code,
        string description)
    {
        var outcome = new LearningOutcome
        {
            Id = Guid.NewGuid(),
            Code = code,
            Description = description
        };

        var profile = NativeMathematicsOutcomeProfileResolver.Resolve(outcome);
        var capability = NativeMathematicsOutcomeProfileResolver.ResolveCapability(outcome);

        Assert.NotNull(profile);
        Assert.True(profile!.IsContextualAssisted);
        Assert.Equal(
            MathematicsGeneratorFamily.CurriculumContextCheck,
            Assert.Single(profile.AllowedFamilies));
        Assert.Equal(MathematicsAiCapabilityLevel.AiAssisted, capability.Level);
        Assert.True(capability.CanGenerateAssisted);
        Assert.False(capability.CanGenerateVerified);
    }

    [Fact]
    public void Resolve_FailsClosedForNonMathematicsOutcome()
    {
        var outcome = new LearningOutcome
        {
            Id = Guid.NewGuid(),
            Code = "CAM:OUT:UNKNOWN",
            Description = "Describe the historical context of a source."
        };

        Assert.Null(NativeMathematicsOutcomeProfileResolver.Resolve(outcome));
        Assert.False(NativeMathematicsOutcomeProfileResolver.Supports(outcome));
    }
}
