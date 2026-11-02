using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Stage C source card-sort contracts written only, not executed.</summary>
public sealed class UsCcssG2Unit1FifteenthLessonCompareCardsTests
{
    [Theory]
    [InlineData("A","TapeSmaller","Smaller","35","35+20=55","55-20=35",true)]
    [InlineData("A","TapeSmaller","Smaller","35","20+35=55","55-35=20",true)]
    [InlineData("A","TapeLarger","Smaller","35","35+20=55","55-20=35",false)]
    [InlineData("B","TapeLarger","Larger","54","34+20=54","54-20=34",true)]
    [InlineData("C","TapeDifference","Difference","12","50+12=62","62-50=12",true)]
    [InlineData("C","TapeDifference","Larger","12","50+12=62","62-50=12",false)]
    public void CompareStoriesRequireCorrectUnknownPositionAndAcceptEquivalentEquations(
        string story,string diagram,string unknown,string value,
        string equationA,string equationB,bool expected)
    {
        var review=UsCcssG2Unit1FifteenthLessonCompareCards
            .CheckCard(story,diagram,unknown,value,equationA,equationB);
        Assert.Equal(expected,review.Correct);
        Assert.True(review.TeacherReviewNeeded);
    }

    [Fact]
    public void WrongEquationsOrIdenticalCardsDoNotReceiveCorrectStatus()
    {
        var fail=UsCcssG2Unit1FifteenthLessonCompareCards
            .CheckCard("A","TapeSmaller","Smaller","35","35+20=55","35+20=55");
        Assert.False(fail.Correct);
        Assert.False(UsCcssG2Unit1FifteenthLessonCompareCards
            .CheckCard("A","TapeSmaller","Smaller","35","35+20=55","55-35=19").Correct);
        Assert.Equal(3,UsCcssG2Unit1FifteenthLessonCompareCards.Cards.Count);
    }

    [Fact]
    public void AllThreeOriginalExitPositionsAndNumberTalkAreChecked()
    {
        Assert.True(UsCcssG2Unit1FifteenthLessonCompareCards
            .CheckIndependentCoolDown("37","76","14").Correct);
        Assert.False(UsCcssG2Unit1FifteenthLessonCompareCards
            .CheckIndependentCoolDown("37","76","15").Correct);
        Assert.True(UsCcssG2Unit1FifteenthLessonCompareCards
            .CheckNumberTalk("14","14","13","14").Correct);
        Assert.False(UsCcssG2Unit1FifteenthLessonCompareCards
            .CheckNumberTalk("14","14","14","14").Correct);
        Assert.Equal(5,UsCcssG2Unit1FifteenthLessonCompareCards.TeacherRubric.Count);
    }
}
