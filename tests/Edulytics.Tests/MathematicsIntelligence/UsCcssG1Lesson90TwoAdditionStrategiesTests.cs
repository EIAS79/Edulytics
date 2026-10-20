using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Independently authored numeric companion to accessible 2021 Grade 1 U05 L02.
/// Publisher images and oral explanation remain teacher-assessed.
/// Written during Stage A-C; deliberately not executed until Stage D.
/// </summary>
public sealed class UsCcssG1Lesson90TwoAdditionStrategiesTests
{
    private const string Family="usccss.g1.u05.l02.explain_no_compose_equations";
    private static Dictionary<string,int> OriginalParameters()=>new()
    {
        ["a"]=63,["b"]=25,["mode"]=6
    };

    [Fact]
    public void BothTrueMethodsRequireEveryIntermediateNumber()
    {
        var p=OriginalParameters();
        const string correct="ELENA_TENS=80;ELENA_ONES=8;ANDRE_FIRST=83;ANDRE_SECOND=88;TOTAL=88";
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,p));
        Assert.Equal(correct,UsCcssG1FullGradeUnit5AdditionQuestions.Solve(Family,p));
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,p,correct));

        foreach(string wrong in new[]
        {
            "ELENA_TENS=60;ELENA_ONES=8;ANDRE_FIRST=83;ANDRE_SECOND=88;TOTAL=88",
            "ELENA_TENS=80;ELENA_ONES=9;ANDRE_FIRST=83;ANDRE_SECOND=88;TOTAL=88",
            "ELENA_TENS=80;ELENA_ONES=8;ANDRE_FIRST=68;ANDRE_SECOND=88;TOTAL=88",
            "ELENA_TENS=80;ELENA_ONES=8;ANDRE_FIRST=83;ANDRE_SECOND=83;TOTAL=88",
            "ELENA_TENS=80;ELENA_ONES=8;ANDRE_FIRST=83;ANDRE_SECOND=88;TOTAL=87",
            "ELENA_TENS=80;ELENA_ONES=8;TOTAL=88",
            correct+";FAKE=0"
        })
            Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,p,wrong));
    }

    [Fact]
    public void SourceOperandsAndLessonFamilyAreGuarded()
    {
        var p=OriginalParameters();
        p["a"]=62;
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,p));
        p["a"]=63;
        p["b"]=24;
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,p));
        p["b"]=25;
        p["extra"]=1;
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,p));
        p.Remove("extra");
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(
            "usccss.g1.u05.l03.write_no_compose_equations",p));
    }

    [Fact]
    public void GeneratedPracticeIncludesNewStrategyAndRetainsAllPriorLesson90Modes()
    {
        var set=Enumerable.Range(0,1500).Select(i=>
            UsCcssG1FullGradeUnit5AdditionQuestions.Build(Family,new Random(i*37+13))).ToArray();
        foreach(var mode in Enumerable.Range(0,7))
            Assert.Contains(set,q=>q.Parameters["mode"]==mode);
        var strategy=set.Where(q=>q.Parameters["mode"]==6).ToArray();
        Assert.NotEmpty(strategy);
        Assert.All(strategy,q=>
        {
            Assert.Contains("EDULYTICS FIRST-PARTY TWO-STRATEGY CHECK",q.Prompt);
            Assert.Contains("NOT a claimed reconstruction",q.Prompt);
            Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
                Family,q.Parameters,UsCcssG1FullGradeUnit5AdditionQuestions.Solve(Family,q.Parameters)));
        });
    }
}
