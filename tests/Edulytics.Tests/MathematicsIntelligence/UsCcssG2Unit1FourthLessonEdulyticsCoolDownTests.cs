using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Stage C authored, execution withheld until Stage D.</summary>
public sealed class UsCcssG2Unit1FourthLessonEdulyticsCoolDownTests
{
    [Theory]
    [InlineData("5 + 8 + 6",true)]
    [InlineData("6 + 5 + 8",true)]
    [InlineData("8 + 6 + 4",false)]
    [InlineData("8 + 8 + 5",false)]
    [InlineData("10 + 8 + 2",false)]
    [InlineData("5 + 8",false)]
    public void ExactlyThreeAvailableCardsMustMinimizeDistance(
        string attempt,bool correct)
    {
        var r=UsCcssG2Unit1FourthLessonEdulyticsCoolDown
            .VerifyOriginalThreeCardEquation(attempt);
        Assert.Equal(correct,r.Correct);
        Assert.True(r.TeacherReviewNeeded);
    }

    [Fact]
    public void MultipleWithinTwentyStrategiesShareExactOutcomes()
    {
        Assert.True(UsCcssG2Unit1FourthLessonEdulyticsCoolDown
            .VerifySubtractSeventeenNine("8").Correct);
        Assert.True(UsCcssG2Unit1FourthLessonEdulyticsCoolDown
            .VerifySevenPlusSix("13").Correct);
        Assert.False(UsCcssG2Unit1FourthLessonEdulyticsCoolDown
            .VerifySubtractSeventeenNine("9").Correct);
        Assert.Equal(4,UsCcssG2Unit1FourthLessonEdulyticsCoolDown.TeacherRubric.Count);
    }
}
