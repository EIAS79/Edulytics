using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Teacher-scored genuine survey vs machine-scored representations.</summary>
public sealed class UsCcssG2Unit1EighteenthLessonClassSurveysTests
{
    private static UsCcssG2Unit1EighteenthLessonClassSurveys.Survey GetThree()
    {
        Assert.True(UsCcssG2Unit1EighteenthLessonClassSurveys.TryReadSurvey(
            "Which reading activity?","Bar","Reading choices","3",
            ["Stories","Comics","Poetry",""],["9","6","4",""],out var survey));
        return survey!;
    }
    [Fact]
    public void RealClassroomClaimsNotInferredFromValidEnteredCounts()
    {
        var survey=GetThree();
        Assert.Equal(19,survey.Total);
        var graph=UsCcssG2Unit1EighteenthLessonClassSurveys.CheckGraph(survey,"1","0","1");
        Assert.True(graph.Correct);
        Assert.True(graph.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit1EighteenthLessonClassSurveys
            .CheckGraph(survey,"1","3","2").Correct);
        Assert.False(UsCcssG2Unit1EighteenthLessonClassSurveys.TryReadSurvey(
            "Q?","Bar","Graph","3",
            ["Stories","stories","Poetry",""],["9","6","4",""],out _));
        Assert.False(UsCcssG2Unit1EighteenthLessonClassSurveys.TryReadSurvey(
            "Q?","Bar","Graph","3",
            ["Stories","Comics","Poetry","Other"],["9","6","4","1"],out _));
        Assert.False(UsCcssG2Unit1EighteenthLessonClassSurveys.TryReadSurvey(
            "Q?","Bar","Graph","4",
            ["A","B","C","D"],["90","6","4","1"],out _));
    }
    [Fact]
    public void CategorySumGapAndTapeRequireCorrectEquations()
    {
        var survey=GetThree();
        var correct=UsCcssG2Unit1EighteenthLessonClassSurveys.CheckPair(
            survey,"1","2","15","9+6=15","3","9-6=3","3","6+3=9");
        Assert.True(correct.Correct);
        Assert.True(correct.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit1EighteenthLessonClassSurveys.CheckPair(
            survey,"1","2","15","9+6=15","3","9-6=3","3","9+6=15").Correct);
        Assert.False(UsCcssG2Unit1EighteenthLessonClassSurveys.CheckPair(
            survey,"1","1","18","9+9=18","0","9-9=0","0","9-9=0").Correct);
        Assert.True(UsCcssG2Unit1EighteenthLessonClassSurveys
            .CheckCoolDown("22","10","6","3").Correct);
        Assert.False(UsCcssG2Unit1EighteenthLessonClassSurveys
            .CheckCoolDown("22","10","6","4").Correct);
    }
}
