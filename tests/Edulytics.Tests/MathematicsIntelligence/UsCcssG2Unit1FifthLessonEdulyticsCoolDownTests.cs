using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Author Stage C; reserve execution until Stage D.</summary>
public sealed class UsCcssG2Unit1FifthLessonEdulyticsCoolDownTests
{
    [Theory]
    [InlineData("38 + 12",true)]
    [InlineData("36 + 14",true)]
    [InlineData("32 + 18",true)]
    [InlineData("38 + 21",false)]
    [InlineData("88 + 12",false)]
    [InlineData("38 + 10",false)]
    [InlineData("8 + 42",false)]
    public void TwoTwoDigitNumbersRequireFourActualCardsAndOptimalDistance(
        string expression,bool correct)
    {
        var r=UsCcssG2Unit1FifthLessonEdulyticsCoolDown
            .CheckFourCardTwoDigitExpression(expression);
        Assert.Equal(correct,r.Correct);
        Assert.True(r.TeacherReviewNeeded);
    }
    [Fact]
    public void TensOnesAndRegroupingQuestionsScoreOriginalSkills()
    {
        Assert.True(UsCcssG2Unit1FifthLessonEdulyticsCoolDown.CheckMentalTen("44").Correct);
        Assert.True(UsCcssG2Unit1FifthLessonEdulyticsCoolDown.CheckRegrouping("42").Correct);
        Assert.True(UsCcssG2Unit1FifthLessonEdulyticsCoolDown.CheckFiftySum("50").Correct);
        Assert.False(UsCcssG2Unit1FifthLessonEdulyticsCoolDown.CheckRegrouping("32").Correct);
    }
}
