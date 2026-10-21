using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Grade 1 U06 L01 first-party equal-unit strip checks only. Physical object
/// selection, foot/finger reference lengths, start-endpoint alignment,
/// tracings, pupil explanations and real classroom centers need a teacher.
/// Authored during A-C, not run until Stage D.
/// </summary>
public sealed class UsCcssG1Lesson103EqualUnitLengthComparisonTests
{
    private const string Family="usccss.g1.u06.l01.compare_lengths";
    private static Dictionary<string,int> P(int a,int b,int c,int mode)=>new()
    {
        ["a"]=a,["b"]=b,["c"]=c,["mode"]=mode
    };

    [Theory]
    [InlineData(12,5,0,0,"LONGER=RED;DIFFERENCE=7")]
    [InlineData(5,12,0,2,"LONGER=BLUE;DIFFERENCE=7")]
    [InlineData(8,8,0,0,"LONGER=EQUAL;DIFFERENCE=0")]
    [InlineData(12,5,19,1,"ORDER=B,A,C")]
    [InlineData(12,5,19,3,"ORDER=C,A,B")]
    public void OriginalCompareAndOrderGoalsHaveCorrectIndependentMath(
        int a,int b,int c,int mode,string expected)
    {
        var p=P(a,b,c,mode);
        Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(Family,p));
        Assert.Equal(expected,UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(Family,p));
        Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Verify(Family,p,expected));
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Verify(
            Family,p,expected+";TEACHER_CONFIRMED=YES"));
    }

    [Fact]
    public void OriginalThreeObjectOrdersRequireThreeDifferentMeasuredLengths()
    {
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(Family,P(8,8,12,1)));
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(Family,P(8,12,8,3)));
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(Family,P(8,12,12,1)));
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(Family,P(8,12,0,1)));
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Verify(
            Family,P(12,5,19,1),"ORDER=A,B,C"));
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Verify(
            Family,P(12,5,19,3),"ORDER=B,A,C"));
    }

    [Fact]
    public void AllFourModesAreGeneratableAndClaimsDoNotFakeClassroomObservations()
    {
        var questions=Enumerable.Range(0,1400)
            .Select(i=>UsCcssG1FullGradeUnit6MeasurementQuestions.Build(
                Family,new Random(i*41+3))).ToArray();
        foreach(int mode in Enumerable.Range(0,4))
            Assert.Contains(questions,q=>q.Parameters["mode"]==mode);
        Assert.All(questions,q=>
        {
            Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(Family,q.Parameters));
            Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Verify(
                Family,q.Parameters,UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(Family,q.Parameters)));
        });
        Assert.All(questions.Where(q=>q.Parameters["mode"]%2==0),
            q=>Assert.Contains("learner-made drawing needs teacher review",q.Prompt));
    }
}
