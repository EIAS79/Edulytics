using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG2Unit3EighteenthLessonYardstickTests
{
    [Fact]
    public void OriginalWarmupAndCardLengthEquationsAreExactButStillTeacherReviewed()
    {
        var warmup = UsCcssG2Unit3EighteenthLessonYardstick.Warmup("8", "2", "7", "14");
        Assert.True(warmup.Correct);
        Assert.True(warmup.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3EighteenthLessonYardstick.Warmup("8", "2", "8", "14").Correct);

        var card = UsCcssG2Unit3EighteenthLessonYardstick.CardArithmetic("1", "2", "11", "36");
        Assert.True(card.Correct);
        Assert.True(card.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3EighteenthLessonYardstick.CardArithmetic("1", "2", "11", "35").Correct);
        Assert.Contains(UsCcssG2Unit3EighteenthLessonYardstick.TeacherRubric,
            item => item.Contains("EVERY 1..36", StringComparison.Ordinal));
    }

    [Fact]
    public void MadeUpDeskAndArmArithmeticCannotCountAsRealSourceEvidence()
    {
        var fictional = UsCcssG2Unit3EighteenthLessonYardstick.Fictional("28", "30", "2", "19", "9");
        Assert.True(fictional.Correct);
        Assert.False(fictional.TeacherReviewNeeded);
        Assert.Contains("NONE are real source measurements", fictional.Feedback);
        Assert.False(UsCcssG2Unit3EighteenthLessonYardstick.Fictional("28", "30", "3", "19", "9").Correct);
        Assert.True(UsCcssG2Unit3EighteenthLessonYardstick.TeacherRubric.Count >= 9);
    }
}
