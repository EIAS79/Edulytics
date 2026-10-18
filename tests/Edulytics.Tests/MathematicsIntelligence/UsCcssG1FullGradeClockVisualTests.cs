using System.Text.Json;
using Edulytics.Web.Presentation;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG1FullGradeClockVisualTests
{
    private static string Parameters(int hour, int mode) =>
        JsonSerializer.Serialize(new
        {
            parameters = new { a = hour, b = 2, c = 0, mode }
        });

    [Theory]
    [InlineData("usccss.g1.u07.l13.hour_clock", 7, 0)]
    [InlineData("usccss.g1.u07.l14.half_hour_clock", 7, 1)]
    [InlineData("usccss.g1.u07.l15.write_hours_halfhours", 12, 0)]
    [InlineData("usccss.g1.u07.l15.write_hours_halfhours", 12, 3)]
    [InlineData("usccss.g1.u07.l17.center_geometry_and_time", 3, 1)]
    public void TimePracticeNowDrawsReadableOriginalFirstPartyAnalogClock(
        string family, int hour, int mode)
    {
        var svg = PracticeMathVisualRenderer.RenderSvg(family, Parameters(hour, mode));
        Assert.NotNull(svg);
        Assert.Contains("role='img'", svg, StringComparison.Ordinal);
        Assert.Contains("data-role='clock-face'", svg, StringComparison.Ordinal);
        Assert.Contains("data-role='hour-hand'", svg, StringComparison.Ordinal);
        Assert.Contains("data-role='minute-hand'", svg, StringComparison.Ordinal);
        Assert.Contains("Read the hands", svg, StringComparison.Ordinal);
        Assert.DoesNotContain($"TIME={hour}:30", svg, StringComparison.Ordinal);
    }

    [Fact]
    public void FullAndHalfHoursAreVisuallyDifferentAndNotPreAnswered()
    {
        const string family = "usccss.g1.u07.l15.write_hours_halfhours";
        for (int hour = 1; hour <= 12; hour++)
        {
            var whole = PracticeMathVisualRenderer.RenderSvg(family, Parameters(hour, 0));
            var half = PracticeMathVisualRenderer.RenderSvg(family, Parameters(hour, 1));
            Assert.NotNull(whole);
            Assert.NotNull(half);
            Assert.NotEqual(whole, half);
        }
    }

    [Fact]
    public void ArithmeticOnlyCenterAndMalformedParametersDoNotShowFalseClocks()
    {
        Assert.Null(PracticeMathVisualRenderer.RenderSvg(
            "usccss.g1.u07.l17.center_geometry_and_time", Parameters(4, 0)));
        Assert.Null(PracticeMathVisualRenderer.RenderSvg(
            "usccss.g1.u07.l14.half_hour_clock", Parameters(13, 1)));
        Assert.Null(PracticeMathVisualRenderer.RenderSvg(
            "usccss.g1.u07.l14.half_hour_clock", "{invalid"));
    }
}
