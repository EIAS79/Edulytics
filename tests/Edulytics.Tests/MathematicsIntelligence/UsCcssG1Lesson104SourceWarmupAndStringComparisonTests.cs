using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Original 2021 U06 L02 number-talk equations and an independent
/// hypothetical same-reference-unit comparison. Real pencil choices,
/// four written length statements and desk side/leg string results
/// require teacher observation and cannot be certified by these fields.
/// </summary>
public sealed class UsCcssG1Lesson104SourceWarmupAndStringComparisonTests
{
    private const string Family="usccss.g1.u06.l02.indirect_length_comparison";
    private static Dictionary<string,int> P(int a,int b,int mode)=>new()
    {
        ["a"]=a,["b"]=b,["c"]=0,["mode"]=mode
    };

    [Theory]
    [InlineData(35,20,4,55)]
    [InlineData(35,25,5,60)]
    [InlineData(30,45,6,75)]
    [InlineData(37,45,7,82)]
    public void SourceMentalWarmupsHaveCorrectResults(int a,int b,int mode,int expected)
    {
        var p=P(a,b,mode);
        Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(Family,p));
        Assert.Equal($"RESULT={expected}",
            UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(Family,p));
        Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Verify(
            Family,p,$"RESULT={expected}"));
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Verify(
            Family,p,$"RESULT={expected-10}"));
        p["a"]=a+1;
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(Family,p));
    }

    [Fact]
    public void EquivalentReferenceUnitsGiveTruthfulLongerOrEqualRelations()
    {
        Assert.Equal("LONGER=A",
            UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(Family,P(12,8,0)));
        Assert.Equal("LONGER=B",
            UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(Family,P(8,12,1)));
        Assert.Equal("LONGER=EQUAL",
            UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(Family,P(9,9,2)));
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Verify(
            Family,P(12,8,0),"LONGER=B"));
    }

    [Fact]
    public void EveryModeGeneratesAndDoesNotClaimPhysicalMeasurementsWereObserved()
    {
        var questions=Enumerable.Range(0,1500)
            .Select(i=>UsCcssG1FullGradeUnit6MeasurementQuestions.Build(
                Family,new Random(i*43+11))).ToArray();
        foreach(int mode in Enumerable.Range(0,8))
            Assert.Contains(questions,q=>q.Parameters["mode"]==mode);
        Assert.All(questions,q=>
        {
            Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(Family,q.Parameters));
            Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Verify(
                Family,q.Parameters,UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(Family,q.Parameters)));
        });
        Assert.All(questions.Where(q=>q.Parameters["mode"]<4),
            q=>Assert.Contains("teacher-assessed",q.Prompt));
        Assert.All(questions.Where(q=>q.Parameters["mode"]>=4),
            q=>Assert.Contains("MENTAL WARM-UP",q.Prompt));
    }
}
