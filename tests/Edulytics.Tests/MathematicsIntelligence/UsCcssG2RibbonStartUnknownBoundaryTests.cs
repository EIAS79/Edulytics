using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG2RibbonStartUnknownBoundaryTests
{
    private sealed class MinimumRandom : Random
    {
        public override int Next(int minValue, int maxValue) => minValue;
    }

    private sealed class MaximumRandom : Random
    {
        public override int Next(int minValue, int maxValue) => maxValue - 1;
    }

    [Fact]
    public void StartUnknownMinimumCutAndRemainingBuildsValidThirtyInchRibbon()
    {
        var item = UsCcssG2Unit3Lesson11SareeRibbonQuestions.Build(
            UsCcssG2Unit3Lesson11SareeRibbonQuestions.Start,
            new MinimumRandom());

        Assert.Equal(14, item.Parameters["known"]);
        Assert.Equal(30, item.Parameters["whole"]);
        Assert.Equal(
            "30",
            UsCcssG2Unit3Lesson11SareeRibbonQuestions.Solve(
                item.Family, item.Parameters));
        Assert.True(UsCcssG2Unit3Lesson11SareeRibbonQuestions.Verify(
            item.Family, item.Parameters, "30"));
    }

    [Fact]
    public void StartUnknownMaximumCutAndRemainingStayWithinHundredInches()
    {
        var item = UsCcssG2Unit3Lesson11SareeRibbonQuestions.Build(
            UsCcssG2Unit3Lesson11SareeRibbonQuestions.Start,
            new MaximumRandom());

        Assert.Equal(47, item.Parameters["known"]);
        Assert.Equal(100, item.Parameters["whole"]);
        Assert.True(UsCcssG2Unit3Lesson11SareeRibbonQuestions.Verify(
            item.Family, item.Parameters, "100"));
        Assert.False(UsCcssG2Unit3Lesson11SareeRibbonQuestions.Verify(
            item.Family, item.Parameters, "99"));
    }
}
