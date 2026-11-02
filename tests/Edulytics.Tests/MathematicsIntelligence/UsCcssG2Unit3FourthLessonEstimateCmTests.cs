using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit3FourthLessonEstimateCmTests
{
    [Fact]
    public void SourceRulerMark1To5CountsFourRealCmIntervals()
    {
        var r=UsCcssG2Unit3FourthLessonEstimateCm.RulerImage("ABD","4");
        Assert.True(r.Correct);Assert.True(r.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3FourthLessonEstimateCm.RulerImage("ABCD","4").Correct);
        Assert.False(UsCcssG2Unit3FourthLessonEstimateCm.RulerImage("ABD","5").Correct);
    }
    [Fact]
    public void FirstPartyTickAndLongMeasurementCheckUnitsAndNoOverlaps()
    {
        Assert.True(UsCcssG2Unit3FourthLessonEstimateCm.FirstPartyTicks("2","17","15").Correct);
        Assert.False(UsCcssG2Unit3FourthLessonEstimateCm.FirstPartyTicks("2","17","17").Correct);
        Assert.True(UsCcssG2Unit3FourthLessonEstimateCm.IteratedRuler("2","8","68").Correct);
        Assert.False(UsCcssG2Unit3FourthLessonEstimateCm.IteratedRuler("2","8","78").Correct);
        Assert.True(UsCcssG2Unit3FourthLessonEstimateCm.EstimateOrder("10","18","27").Correct);
        Assert.False(UsCcssG2Unit3FourthLessonEstimateCm.EstimateOrder("20","18","27").Correct);
        Assert.True(UsCcssG2Unit3FourthLessonEstimateCm.TeacherRubric.Count>=6);
    }
}
