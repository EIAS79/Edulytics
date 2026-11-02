using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit3SeventeenthLessonLinePlotCenterTests
{
    [Fact]
    public void OriginalSubtractionsAreCorrectAndNeedTeacherReasoning()
    {
        var r=UsCcssG2Unit3SeventeenthLessonLinePlotCenter.Warmup("50","49","39","38");
        Assert.True(r.Correct);Assert.True(r.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3SeventeenthLessonLinePlotCenter.Warmup("50","49","39","39").Correct);
    }
    [Fact]
    public void FictionalEightObjectsAreSeparateFromRealPlotAndIndependentArithmetic()
    {
        var r=UsCcssG2Unit3SeventeenthLessonLinePlotCenter.Fictional("8","2","2","9","14","4");
        Assert.True(r.Correct);Assert.False(r.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3SeventeenthLessonLinePlotCenter.Fictional("9","2","2","9","14","4").Correct);
        Assert.True(UsCcssG2Unit3SeventeenthLessonLinePlotCenter.Independent("57","56").Correct);
        Assert.False(UsCcssG2Unit3SeventeenthLessonLinePlotCenter.Independent("57","55").Correct);
        Assert.True(UsCcssG2Unit3SeventeenthLessonLinePlotCenter.TeacherRubric.Count>=7);
    }
}
