using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit2FifteenthLessonTapeStoriesCenterTests
{
    [Fact]
    public void WarmupMustCompensateByAddingOneBack()
    {
        var w=UsCcssG2Unit2FifteenthLessonTapeStoriesCenter.Talk("15","25","15","16");
        Assert.True(w.Correct);Assert.True(w.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit2FifteenthLessonTapeStoriesCenter.Talk("15","25","15","14").Correct);
    }
    [Fact]
    public void FirstPartyNumbersAreNotSourceGameCardEvidence()
    {
        var n=UsCcssG2Unit2FifteenthLessonTapeStoriesCenter.FirstPartyDiagrams("49","27","25");
        Assert.True(n.Correct);Assert.False(n.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit2FifteenthLessonTapeStoriesCenter.FirstPartyDiagrams("49","27","24").Correct);
        var e=UsCcssG2Unit2FifteenthLessonTapeStoriesCenter.Exit("26","75","44");
        Assert.True(e.Correct);Assert.False(e.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit2FifteenthLessonTapeStoriesCenter.Exit("25","75","44").Correct);
        Assert.True(UsCcssG2Unit2FifteenthLessonTapeStoriesCenter.TeacherRubric.Count>=6);
    }
}
