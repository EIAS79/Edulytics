using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit2SeventhLessonSubtractTwoDigitsTests
{
    [Fact]
    public void SourceExamplesRequireCorrectTradeAndAnswer()
    {
        var r=UsCcssG2Unit2SeventhLessonSubtractTwoDigits
            .SourceProblems("18","39","3","16","8","13");
        Assert.True(r.Correct);
        Assert.True(r.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit2SeventhLessonSubtractTwoDigits
            .SourceProblems("18","49","3","16","8","13").Correct);
        Assert.False(UsCcssG2Unit2SeventhLessonSubtractTwoDigits
            .SourceProblems("18","39","4","16","8","13").Correct);
    }
    [Fact]
    public void IndependentExampleAndExitAreNotClaimedPublisherCards()
    {
        var r=UsCcssG2Unit2SeventhLessonSubtractTwoDigits.FirstParty("37","37");
        Assert.True(r.Correct);Assert.False(r.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit2SeventhLessonSubtractTwoDigits.FirstParty("37","38").Correct);
        Assert.True(UsCcssG2Unit2SeventhLessonSubtractTwoDigits.Exit("27","28","28").Correct);
        Assert.False(UsCcssG2Unit2SeventhLessonSubtractTwoDigits.Exit("27","38","28").Correct);
        Assert.True(UsCcssG2Unit2SeventhLessonSubtractTwoDigits.TeacherRubric.Count>=6);
    }
}
