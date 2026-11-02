using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit2TenthLessonCenterDay2Tests
{
    [Fact]
    public void SourceWarmupAndTwoAlternativeDiceChoicesAreChecked()
    {
        var r=UsCcssG2Unit2TenthLessonCenterDay2.Warmup("38","5","16");
        Assert.True(r.Correct);Assert.True(r.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit2TenthLessonCenterDay2.Warmup("38","6","16").Correct);
        Assert.True(UsCcssG2Unit2TenthLessonCenterDay2
            .DiceMath("73","24","49","52","21").Correct);
        Assert.False(UsCcssG2Unit2TenthLessonCenterDay2
            .DiceMath("73","24","49","52","19").Correct);
    }
    [Fact]
    public void FirstPartyExitIsNotAuthenticDiceOrTeacherAssessedWinner()
    {
        var ok=UsCcssG2Unit2TenthLessonCenterDay2.Independent("47","38","46");
        Assert.True(ok.Correct);Assert.False(ok.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit2TenthLessonCenterDay2.Independent("47","39","46").Correct);
        Assert.True(UsCcssG2Unit2TenthLessonCenterDay2.TeacherRubric.Count>=6);
    }
}
