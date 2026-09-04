using Edulytics.Core.Entities;
using Edulytics.Core.MathematicsGeneration;
using Edulytics.Services.Assessments;

namespace Edulytics.Tests.Phase40;

public sealed class NativeMathematicsOutcomeProfileResolverTests
{
    [Theory]
    [InlineData("CCSS:6.RP.A.3", "Use ratio and rate reasoning", MathematicsGeneratorFamily.UnitRateWordProblem)]
    [InlineData("CCSS:5.NF.B.4", "Apply and extend understanding of multiplication to multiply a fraction", MathematicsGeneratorFamily.FractionOfQuantity)]
    [InlineData("CCSS:6.EE.B.7", "Solve real-world and mathematical problems by writing and solving equations", MathematicsGeneratorFamily.OneStepEquation)]
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
