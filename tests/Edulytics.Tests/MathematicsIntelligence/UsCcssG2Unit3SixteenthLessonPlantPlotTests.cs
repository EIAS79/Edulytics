using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit3SixteenthLessonPlantPlotTests
{
    [Fact]
    public void OriginalGroupBHasCorrectNonzeroStartCountsAndComparison()
    {
        var talk=UsCcssG2Unit3SixteenthLessonPlantPlot.Warmup("21","31","23","43");
        Assert.True(talk.Correct);Assert.True(talk.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3SixteenthLessonPlantPlot.Warmup("21","31","23","42").Correct);
        Assert.True(UsCcssG2Unit3SixteenthLessonPlantPlot
            .Source("25","35","10","4","10","33").Correct);
        Assert.False(UsCcssG2Unit3SixteenthLessonPlantPlot
            .Source("25","35","10","5","10","33").Correct);
        Assert.True(UsCcssG2Unit3SixteenthLessonPlantPlot.TickCounts("2","3","0","0").Correct);
        Assert.False(UsCcssG2Unit3SixteenthLessonPlantPlot.TickCounts("2","3","1","0").Correct);
    }
    [Fact]
    public void FirstPartyNinePlantSampleIsNeverConfusedWithGroupB()
    {
        var sample=UsCcssG2Unit3SixteenthLessonPlantPlot.Fictional("24","28","26","3");
        Assert.True(sample.Correct);Assert.False(sample.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3SixteenthLessonPlantPlot.Fictional("25","28","26","3").Correct);
        Assert.True(UsCcssG2Unit3SixteenthLessonPlantPlot.TeacherRubric.Count>=7);
    }
}
