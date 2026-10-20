using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Independently authored Grade 1 Unit 5 L04 math using the original 2021
/// Kiran/Priya marble quantities. Does not assert actual Notice/Wonder
/// discourse or physical center participation. Authored, NOT executed.
/// </summary>
public sealed class UsCcssG1Lesson92SourceMarblesFirstPartyTests
{
    private const string Family="usccss.g1.u05.l04.center_no_compose";

    [Fact]
    public void SourceMarbleCountsHaveCorrectAdditionAndComparison()
    {
        var p=new Dictionary<string,int>{{"a",14},{"b",23},{"mode",4}};
        const string right="SUM=37;MORE=PRIYA;DIFFERENCE=9";
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,p));
        Assert.Equal(right,UsCcssG1FullGradeUnit5AdditionQuestions.Solve(Family,p));
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,p,right));
        foreach(var wrong in new[]
        {
            "SUM=36;MORE=PRIYA;DIFFERENCE=9",
            "SUM=37;MORE=KIRAN;DIFFERENCE=9",
            "SUM=37;MORE=PRIYA;DIFFERENCE=8",
            "SUM=37;MORE=PRIYA",
            right+";EXTRA=0"
        })
            Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,p,wrong));

        p["a"]=15;
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,p));
        p["a"]=14;
        p["b"]=24;
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,p));
        p["b"]=23;
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(
            "usccss.g1.u05.l03.write_no_compose_equations",p));
    }

    [Fact]
    public void EverySourceAwareModeIsGeneratedAndPracticeStaysScoped()
    {
        var questions=Enumerable.Range(0,1400)
            .Select(i=>UsCcssG1FullGradeUnit5AdditionQuestions.Build(
                Family,new Random(i*29+17))).ToArray();
        foreach(int mode in Enumerable.Range(0,5))
            Assert.Contains(questions,x=>x.Parameters["mode"]==mode);
        var exact=questions.Where(x=>x.Parameters["mode"]==4).ToArray();
        Assert.NotEmpty(exact);
        Assert.All(exact,x=>
        {
            Assert.Contains("EDULYTICS FIRST-PARTY NUMERIC FOLLOW-UP",x.Prompt);
            Assert.Contains("teacher-assessed and not auto-certified",x.Prompt);
            Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
                Family,x.Parameters,"SUM=37;MORE=PRIYA;DIFFERENCE=9"));
        });
    }
}
