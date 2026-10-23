using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit3SixthLessonReptileStoriesTests
{
    [Fact]
    public void NumberTalkAndTwoReversedKeywordComparisonsAreExact()
    {
        var w=UsCcssG2Unit3SixthLessonReptileStories.NumberTalk("10","20","30","40");
        Assert.True(w.Correct);Assert.True(w.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3SixthLessonReptileStories.NumberTalk("10","20","30","50").Correct);
        var s=UsCcssG2Unit3SixthLessonReptileStories.OppositeKeywordStories("81","54");
        Assert.True(s.Correct);Assert.True(s.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3SixthLessonReptileStories.OppositeKeywordStories("43","88").Correct);
    }
    [Fact]
    public void EightSourcePetLengthsAndFirstPartyExitAreSeparatedFromAuthenticPartnerGame()
    {
        var l=UsCcssG2Unit3SixthLessonReptileStories.SourceReptileLengths(
            "28","98","46","65","83","55","71","38");
        Assert.True(l.Correct);Assert.True(l.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3SixthLessonReptileStories.SourceReptileLengths(
            "28","98","46","65","83","55","72","38").Correct);
        var e=UsCcssG2Unit3SixthLessonReptileStories.Independent("64","65","34");
        Assert.True(e.Correct);Assert.False(e.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3SixthLessonReptileStories.Independent("64","65","33").Correct);
    }
}
