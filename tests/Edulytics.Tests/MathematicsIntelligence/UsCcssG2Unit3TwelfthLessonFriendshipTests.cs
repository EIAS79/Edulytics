using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit3TwelfthLessonFriendshipTests
{
    [Fact]
    public void WarmupAndLinBothStepsRequireSourceCorrectAnswers()
    {
        var w=UsCcssG2Unit3TwelfthLessonFriendship.Warmup("True","True","True","False");
        Assert.True(w.Correct);Assert.True(w.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3TwelfthLessonFriendship.Warmup("True","True","True","True").Correct);
        var r=UsCcssG2Unit3TwelfthLessonFriendship.Lin("57","29");
        Assert.True(r.Correct);Assert.True(r.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3TwelfthLessonFriendship.Lin("57","64").Correct);
    }
    [Fact]
    public void BothLinkedHanClareAndreMaiAndIndependentExamplesAreExact()
    {
        Assert.True(UsCcssG2Unit3TwelfthLessonFriendship
            .TwoOtherStories("34","61","22","71").Correct);
        Assert.False(UsCcssG2Unit3TwelfthLessonFriendship
            .TwoOtherStories("34","61","22","27").Correct);
        var first=UsCcssG2Unit3TwelfthLessonFriendship.FirstParty("41","29","53");
        Assert.True(first.Correct);Assert.False(first.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3TwelfthLessonFriendship.FirstParty("41","29","29").Correct);
    }
}
