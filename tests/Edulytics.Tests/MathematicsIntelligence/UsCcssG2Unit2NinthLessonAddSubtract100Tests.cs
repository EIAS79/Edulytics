using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit2NinthLessonAddSubtract100Tests
{
    [Fact]
    public void AllFourSourceArithmeticsMustMatchAndTeacherReviewIsRequired()
    {
        var v=UsCcssG2Unit2NinthLessonAddSubtract100.SourceFour("74","28","74","43");
        Assert.True(v.Correct);
        Assert.True(v.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit2NinthLessonAddSubtract100
            .SourceFour("74","28","73","43").Correct);
        Assert.True(UsCcssG2Unit2NinthLessonAddSubtract100.SourceSectionSummary("45").Correct);
        Assert.False(UsCcssG2Unit2NinthLessonAddSubtract100.SourceSectionSummary("55").Correct);
    }
    [Fact]
    public void OriginalEdulyticsExamplesAreNotClaimedPublisherCooldown()
    {
        var v=UsCcssG2Unit2NinthLessonAddSubtract100.Independent("73","27","84");
        Assert.True(v.Correct);
        Assert.False(v.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit2NinthLessonAddSubtract100.Independent("73","28","84").Correct);
        Assert.True(UsCcssG2Unit2NinthLessonAddSubtract100.TeacherRubric.Count>=6);
    }
}
