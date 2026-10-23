using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit3FourteenthLessonHandSpanLinePlotTests
{
    [Fact]
    public void FictionalModeCountAndHorizontalMeasurementExtremesAreDistinct()
    {
        var a=UsCcssG2Unit3FourteenthLessonHandSpanLinePlot.FictionalPlot("4","8","6","9");
        Assert.True(a.Correct);Assert.False(a.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3FourteenthLessonHandSpanLinePlot.FictionalPlot("4","8","3","9").Correct);
        Assert.False(UsCcssG2Unit3FourteenthLessonHandSpanLinePlot.FictionalPlot("1","8","6","9").Correct);
        Assert.True(UsCcssG2Unit3FourteenthLessonHandSpanLinePlot.CountAndRange("2","4").Correct);
        Assert.False(UsCcssG2Unit3FourteenthLessonHandSpanLinePlot.CountAndRange("4","2").Correct);
    }
    [Fact]
    public void RealHandMeasurementAlwaysRequiresHumanVerification()
    {
        Assert.True(UsCcssG2Unit3FourteenthLessonHandSpanLinePlot.RealMeasurement("6").TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3FourteenthLessonHandSpanLinePlot.RealMeasurement("11").Correct);
        Assert.True(UsCcssG2Unit3FourteenthLessonHandSpanLinePlot.TeacherRubric.Count>=6);
    }
}
