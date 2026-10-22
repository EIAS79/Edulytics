using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit3FifteenthLessonPencilPlotTests
{
    [Fact]
    public void SourceMentalSubtractionsAndGroupCCountAreExact()
    {
        var talk=UsCcssG2Unit3FifteenthLessonPencilPlot.Warmup("27","23","26","21");
        Assert.True(talk.Correct);Assert.True(talk.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3FifteenthLessonPencilPlot.Warmup("27","23","26","20").Correct);
        Assert.True(UsCcssG2Unit3FifteenthLessonPencilPlot.SourceDataset("10","10","10","16","5").Correct);
        Assert.False(UsCcssG2Unit3FifteenthLessonPencilPlot.SourceDataset("10","16","10","16","5").Correct);
    }
    [Fact]
    public void UnmeasuredTicksRemainLabeledAndOriginalExtraNumbersSeparate()
    {
        Assert.True(UsCcssG2Unit3FifteenthLessonPencilPlot.SourceTicks("0","0","3","1").Correct);
        Assert.False(UsCcssG2Unit3FifteenthLessonPencilPlot.SourceTicks("1","0","3","1").Correct);
        var independent=UsCcssG2Unit3FifteenthLessonPencilPlot.Independent("13","10","5");
        Assert.True(independent.Correct);Assert.False(independent.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3FifteenthLessonPencilPlot.Independent("13","9","5").Correct);
        Assert.True(UsCcssG2Unit3FifteenthLessonPencilPlot.TeacherRubric.Count>=6);
    }
}
