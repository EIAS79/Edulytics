using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class MixedImproperPracticeRoutingTests
{
    [Fact]
    public void MixedImproperLesson_RoutesToMixedImproperFamily_NotFractionSimplify()
    {
        var ok = SupportingPracticeTargetRuleRegistry.TryResolve(
            null,
            "Convert between mixed numbers and improper fractions",
            out var rule);

        Assert.True(ok);
        Assert.NotNull(rule);
        Assert.Equal("mixed-improper", rule!.Id);
        Assert.Equal("MIXED_IMPROPER", rule.Mechanic);
        Assert.Contains(
            "supporting.fractions.mixed_to_improper",
            rule.Families);
        Assert.DoesNotContain(
            "supporting.fractions.simplify",
            rule.Families);
    }
}
