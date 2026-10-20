using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Source 2021 Grade 1 Unit 5 Lesson 10: exact numeric composing tasks
/// and first-step completion. Teacher judges drawings and explanations.
/// Tests authored in A-C, deferred until Stage D.
/// </summary>
public sealed class UsCcssG1Lesson98OriginalTensAndOnesTests
{
    private const string Family="usccss.g1.u05.l10.tens_tens_ones_ones";

    [Theory]
    [InlineData(28,56,4,"TENS=8;ONES=4;TOTAL=84")]
    [InlineData(27,44,5,"TENS=7;ONES=1;TOTAL=71")]
    [InlineData(37,26,6,"TENS=6;ONES=3;TOTAL=63")]
    [InlineData(34,57,7,"TENS=9;ONES=1;TOTAL=91")]
    [InlineData(18,55,8,"TENS=7;ONES=3;TOTAL=73")]
    public void AllSourceMathCorrectlyAccountsForCarriedTen(
        int a,int b,int mode,string correct)
    {
        var p=new Dictionary<string,int>{{"a",a},{"b",b},{"mode",mode}};
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,p));
        Assert.Equal(correct,UsCcssG1FullGradeUnit5AdditionQuestions.Solve(Family,p));
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,p,correct));
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
            Family,p,correct.Replace("TOTAL="+(a+b),"TOTAL="+(a+b-10))));
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
            Family,p,correct+";PUBLISHER_PICTURE=CERTIFIED"));
        p["a"]=a+1;
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,p));
    }

    [Theory]
    [InlineData(28,56,9,"TENS_PART=70;ONES_PART=14;NEW_TENS=8;ONES=4;TOTAL=84")]
    [InlineData(27,44,10,"TENS_PART=60;ONES_PART=11;NEW_TENS=7;ONES=1;TOTAL=71")]
    public void OriginalFirstStepTasksNeedBothPartsAndRegrouping(
        int a,int b,int mode,string expected)
    {
        var p=new Dictionary<string,int>{{"a",a},{"b",b},{"mode",mode}};
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,p));
        Assert.Equal(expected,UsCcssG1FullGradeUnit5AdditionQuestions.Solve(Family,p));
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,p,expected));
        foreach(var wrong in new[]
        {
            expected.Replace("TENS_PART="+((a/10+b/10)*10),"TENS_PART=0"),
            expected.Replace("ONES_PART="+(a%10+b%10),"ONES_PART=0"),
            expected.Replace("NEW_TENS="+((a+b)/10),"NEW_TENS=0"),
            expected.Replace("ONES="+((a+b)%10),"ONES=0"),
            expected.Replace("TOTAL="+(a+b),"TOTAL=0"),
            "TOTAL="+(a+b)
        })
            Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,p,wrong));
    }

    [Fact]
    public void AllElevenModesAreStillReachableWithoutMisgradingAnyQuestion()
    {
        var items=Enumerable.Range(0,1800)
            .Select(i=>UsCcssG1FullGradeUnit5AdditionQuestions.Build(
                Family,new Random(i*37+19))).ToArray();
        foreach(int mode in Enumerable.Range(0,11))
            Assert.Contains(items,q=>q.Parameters["mode"]==mode);
        Assert.All(items,q=>
        {
            Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,q.Parameters));
            Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
                Family,q.Parameters,UsCcssG1FullGradeUnit5AdditionQuestions.Solve(Family,q.Parameters)));
        });
        Assert.All(items.Where(q=>q.Parameters["mode"]==9),
            q=>Assert.Contains("20+50",q.Prompt));
        Assert.All(items.Where(q=>q.Parameters["mode"]==10),
            q=>Assert.Contains("7+4",q.Prompt));
    }
}
