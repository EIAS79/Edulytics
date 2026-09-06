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
    [InlineData("CCSS:4.NBT.B.4", "Fluently add and subtract multi-digit whole numbers", MathematicsGeneratorFamily.IntegerComputation)]
    public void Resolve_MapsClearlySupportedOutcomeToTrustedFamily(
        string code,
        string description,
        MathematicsGeneratorFamily expected)
    {
        var outcome = new LearningOutcome { Id = Guid.NewGuid(), Code = code, Description = description };

        var profile = NativeMathematicsOutcomeProfileResolver.Resolve(outcome);

        Assert.NotNull(profile);
        Assert.Contains(expected, profile!.AllowedFamilies);
    }

    [Fact]
    public void Resolve_FailsClosedForOneStepEquationUntilNativeFamilyIsCorrected()
    {
        var outcome = new LearningOutcome
        {
            Id = Guid.NewGuid(),
            Code = "MATH-ONE-STEP",
            Description = "Solve a one-step equation"
        };

        Assert.Null(NativeMathematicsOutcomeProfileResolver.Resolve(outcome));
    }

    [Fact]
    public void Resolve_FailsClosedForGeneralFractionMultiplication()
    {
        var outcome = new LearningOutcome
        {
            Id = Guid.NewGuid(),
            Code = "CCSS:5.NF.B.4",
            Description = "Apply and extend understanding of multiplication to multiply a fraction"
        };

        Assert.Null(NativeMathematicsOutcomeProfileResolver.Resolve(outcome));
    }

    [Fact]
    public void Resolve_FailsClosedForUnsupportedGeometryOutcome()
    {
        var outcome = new LearningOutcome
        {
            Id = Guid.NewGuid(),
            Code = "CCSS:7.G.A.1",
            Description = "Solve problems involving scale drawings of geometric figures"
        };

        Assert.Null(NativeMathematicsOutcomeProfileResolver.Resolve(outcome));
    }
}
