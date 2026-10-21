using System.Text.Json;
using Edulytics.Web.Presentation;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Only G1 new original 2021 U07 L13-L15 source clock tasks.
/// Visual reading does not claim to verify physical clock manipulation.</summary>
public sealed class UsCcssG1FullGradeAnalogClockSourceVisualTests
{
    private static string Clock(string family,int hour,int mode)
    {
        var p=new Dictionary<string,int>{{"a",hour},{"b",2},{"c",0},{"mode",mode}};
        Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Valid(family,p));
        var json=JsonSerializer.Serialize(new {parameters=p});
        return Assert.IsType<string>(PracticeMathVisualRenderer.RenderSvg(family,json));
    }
    [Theory]
    [InlineData("usccss.g1.u07.l13.hour_clock",7,0)]
    [InlineData("usccss.g1.u07.l14.half_hour_clock",7,1)]
    [InlineData("usccss.g1.u07.l15.write_hours_halfhours",12,0)]
    [InlineData("usccss.g1.u07.l15.write_hours_halfhours",12,1)]
    public void OriginalClockSourcesRenderNumberedAnalogFacesAndRealHands(string family,int hour,int mode)
    {
        var svg=Clock(family,hour,mode);
        Assert.Contains("<svg ",svg);
        Assert.Contains("role='img'",svg);
        Assert.Contains("aria-label=",svg);
        Assert.Contains("class='clock-hour'",svg);
        Assert.Contains("class='clock-minute'",svg);
        Assert.Contains(">12</text>",svg);
        Assert.Contains(">6</text>",svg);
        Assert.DoesNotContain("TIME=",svg);
        Assert.DoesNotContain("<image ",svg);
    }
    [Fact]
    public void ClockComparisonGivesAVisibleDigitalCardWithoutPrefillingSameAnswer()
    {
        const string f="usccss.g1.u07.l13.hour_clock";
        var matched=Clock(f,7,2);
        var other=Clock(f,7,3);
        Assert.Contains("Digital card: 7:00",matched);
        Assert.Contains("Digital card: 2:00",other);
        Assert.DoesNotContain("SAME=YES",matched);
        Assert.DoesNotContain("SAME=NO",other);
    }

    [Fact]
    public void DifferentTimeClockVisualRefusesToDrawAnActuallyMatchingDigitalCard()
    {
        const string family="usccss.g1.u07.l13.hour_clock";
        var invalid=new Dictionary<string,int>{
            ["a"]=7,["b"]=7,["c"]=0,["mode"]=3
        };
        Assert.False(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Valid(
            family,invalid));
        Assert.Null(PracticeMathVisualRenderer.RenderSvg(
            family,JsonSerializer.Serialize(new{parameters=invalid})));
        invalid["mode"]=2;
        Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Valid(
            family,invalid));
        var matched=PracticeMathVisualRenderer.RenderSvg(
            family,JsonSerializer.Serialize(new{parameters=invalid}));
        Assert.Contains("Digital card: 7:00",Assert.IsType<string>(matched));
    }

    [Fact]
    public void OriginalHalfClockSortHasWholeHourAndHalfPastDiagrams()
    {
        const string f="usccss.g1.u07.l14.half_hour_clock";
        var whole=Clock(f,7,2);
        var half=Clock(f,7,3);
        Assert.NotEqual(whole,half);
        Assert.Contains("class='clock-minute' x1='210' y1='130' x2='210' y2='57'",whole);
        Assert.Contains("class='clock-minute' x1='210' y1='130' x2='210' y2='203'",half);
    }

    [Fact]
    public void SourceCountMinutesHalfClockShowsMinuteHandAtSix()
    {
        const string f="usccss.g1.u07.l15.write_hours_halfhours";
        foreach(var mode in new[]{2,3})
        {
            var svg=Clock(f,4,mode);
            Assert.Contains("class='clock-minute' x1='210' y1='130' x2='210' y2='203'",svg);
            Assert.DoesNotContain("HOPS=6",svg);
        }
    }

    [Fact]
    public void HalfPastHourHandMovesHalfwayWhileMinuteHandPointsToSix()
    {
        const string hour="usccss.g1.u07.l13.hour_clock";
        const string half="usccss.g1.u07.l14.half_hour_clock";
        var full=Clock(hour,7,0);
        var fractional=Clock(half,7,0);
        Assert.NotEqual(full,fractional);
        Assert.Contains("class='clock-minute' x1='210' y1='130' x2='210' y2='57'",full);
        Assert.Contains("class='clock-minute' x1='210' y1='130' x2='210' y2='203'",fractional);
        Assert.NotEqual(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Solve(hour,
            new Dictionary<string,int>{{"a",7},{"b",2},{"c",0},{"mode",0}}),
            UsCcssG1FullGradeUnit7GeometryTimeQuestions.Solve(half,
            new Dictionary<string,int>{{"a",7},{"b",2},{"c",0},{"mode",0}}));
    }
    [Theory]
    [InlineData(6,0)]
    [InlineData(6,1)]
    public void SourceHardTimesRequiresAnalogClockReadingRatherThanCopyingDigits(
        int hour,int mode)
    {
        const string family="usccss.g1.u07.l16.daily_schedule_time";
        var svg=Clock(family,hour,mode);
        Assert.Contains("class='clock-hour'",svg);
        Assert.Contains("class='clock-minute'",svg);
        Assert.Contains("<svg ",svg);
    }

    [Fact]
    public void DoNotFabricateAnalogClockForUnrelatedClassroomCenter()
    {
        var json=JsonSerializer.Serialize(new {parameters=new {a=7,b=2,c=0,mode=0}});
        Assert.Null(PracticeMathVisualRenderer.RenderSvg(
            "usccss.g1.u07.l17.center_geometry_and_time",json));
        // The source-bound U08 count-on path now has a legitimate non-clock
        // drawing; assert it is truly a count-on diagram, not a clock.
        var countOn=Assert.IsType<string>(PracticeMathVisualRenderer.RenderSvg(
            "usccss.g1.u08.l07.count_on_to120",json));
        Assert.Contains("<svg ",countOn);
        Assert.Contains("count",countOn,StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("clock-hour",countOn);
        Assert.DoesNotContain("clock-minute",countOn);
    }
}
