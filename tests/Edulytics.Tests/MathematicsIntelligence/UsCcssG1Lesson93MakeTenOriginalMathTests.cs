using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Original 2021 Grade 1 Unit 5 lesson 5 source numbers, first-party numerical
/// assessment only. Student-chosen methods, visual work and actual Add Em Up
/// Partner rounds remain teacher-observed. Authored during A-C, not run.
/// </summary>
public sealed class UsCcssG1Lesson93MakeTenOriginalMathTests
{
    private const string Family="usccss.g1.u05.l05.make_new_ten";

    [Theory]
    [InlineData(37,7,1,"MOVE=3;REST=4;TOTAL=44")]
    [InlineData(47,8,2,"MOVE=3;REST=5;TOTAL=55")]
    [InlineData(26,8,3,"MOVE=4;REST=4;TOTAL=34")]
    public void SourceSpecificExamplesGradeBothPartsOfTheSplitAndTheTotal(
        int a,int b,int mode,string expected)
    {
        var p=new Dictionary<string,int>{{"a",a},{"b",b},{"mode",mode}};
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,p));
        Assert.Equal(expected,UsCcssG1FullGradeUnit5AdditionQuestions.Solve(Family,p));
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,p,expected));
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
            Family,p,expected.Replace("TOTAL="+(a+b),"TOTAL="+(a+b+1))));
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
            Family,p,expected.Replace("MOVE="+(10-a%10),"MOVE="+(11-a%10))));
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
            Family,p,expected+";PARTNER_PLAY=CLAIMED"));
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
            Family,p,"TOTAL="+(a+b)));
    }

    [Fact]
    public void ExactSourceModesRejectWrongOperandsOrCarryFreeAdditions()
    {
        foreach(var (a,b,mode) in new (int,int,int)[]
        {
            (47,9,2),(46,8,2),(37,8,1),(26,7,3),
            (43,5,0),(90,9,0),(47,8,4)
        })
            Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(
                Family,new Dictionary<string,int>{{"a",a},{"b",b},{"mode",mode}}));
    }

    [Fact]
    public void GeneratedProblemsRetainEveryLessonModeAndTruthfulSourceBoundary()
    {
        var problems=Enumerable.Range(0,1200).Select(i=>
            UsCcssG1FullGradeUnit5AdditionQuestions.Build(Family,new Random(41*i+19)))
            .ToArray();
        foreach(var mode in Enumerable.Range(0,4))
            Assert.Contains(problems,p=>p.Parameters["mode"]==mode);
        Assert.All(problems,p=>
        {
            Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,p.Parameters));
            Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
                Family,p.Parameters,UsCcssG1FullGradeUnit5AdditionQuestions.Solve(Family,p.Parameters)));
        });
        Assert.Contains(problems,p=>p.Parameters["mode"]==2&&p.Prompt.Contains("8+47"));
        Assert.Contains(problems,p=>p.Parameters["mode"]==1&&p.Prompt.Contains("37 dollars"));
        Assert.Contains(problems,p=>p.Parameters["mode"]==3&&p.Prompt.Contains("8 students"));
    }
}
