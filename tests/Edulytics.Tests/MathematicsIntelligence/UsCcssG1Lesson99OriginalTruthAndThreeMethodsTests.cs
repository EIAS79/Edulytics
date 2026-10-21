using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Original public 2021 U05 L11 warm-up statements and litter-story
/// numbers. The 3 pupil methods are original subject matter represented
/// by first-party numeric steps; source drawings/oral explanations are
/// teacher assessed. Authored without running deferred tests/CI.
/// </summary>
public sealed class UsCcssG1Lesson99OriginalTruthAndThreeMethodsTests
{
    private const string Family="usccss.g1.u05.l11.explain_addition_method";

    [Theory]
    [InlineData(24,3,6,"TRUE=NO;ACTUAL=27;STATED=54")]
    [InlineData(42,5,7,"TRUE=YES;ACTUAL=47;STATED=47")]
    [InlineData(42,30,8,"TRUE=NO;ACTUAL=72;STATED=45")]
    public void OriginalTrueFalseWarmupsHaveCorrectIndependentLeftHandSums(
        int a,int b,int mode,string expected)
    {
        var p=new Dictionary<string,int>{{"a",a},{"b",b},{"mode",mode}};
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,p));
        Assert.Equal(expected,UsCcssG1FullGradeUnit5AdditionQuestions.Solve(Family,p));
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,p,expected));
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
            Family,p,expected.Replace("TRUE=YES","TRUE=NO").Replace("TRUE=NO","TRUE=UNSURE")));
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,p,expected+";ORAL_APPROVED=YES"));
        p["b"]=b+1;
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,p));
    }

    [Fact]
    public void LitterStoryRequiresSevenMathematicallyConsistentFields()
    {
        var p=new Dictionary<string,int>{{"a",37},{"b",25},{"mode",9}};
        const string correct="JADA_TENS=50;JADA_ONES=12;KIRAN_FIRST=42;KIRAN_SECOND=62;TYLER_FIRST=57;TYLER_SECOND=62;TOTAL=62";
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,p));
        Assert.Equal(correct,UsCcssG1FullGradeUnit5AdditionQuestions.Solve(Family,p));
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,p,correct));
        foreach(var wrong in new[]
        {
            correct.Replace("JADA_TENS=50","JADA_TENS=40"),
            correct.Replace("JADA_ONES=12","JADA_ONES=2"),
            correct.Replace("KIRAN_FIRST=42","KIRAN_FIRST=40"),
            correct.Replace("KIRAN_SECOND=62","KIRAN_SECOND=52"),
            correct.Replace("TYLER_FIRST=57","TYLER_FIRST=52"),
            correct.Replace("TYLER_SECOND=62","TYLER_SECOND=57"),
            correct.Replace("TOTAL=62","TOTAL=61"),
            "TOTAL=62"
        })
            Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,p,wrong));
        p["a"]=38;
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,p));
    }

    [Fact]
    public void AllTenModesGenerateValidQuestionsAndPreserveOriginalTwoStories()
    {
        var tasks=Enumerable.Range(0,1500).Select(i=>
            UsCcssG1FullGradeUnit5AdditionQuestions.Build(Family,new Random(i*31+19)))
            .ToArray();
        foreach(int mode in Enumerable.Range(0,10))
            Assert.Contains(tasks,q=>q.Parameters["mode"]==mode);
        Assert.All(tasks,q=>
        {
            Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,q.Parameters));
            Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,q.Parameters,
                UsCcssG1FullGradeUnit5AdditionQuestions.Solve(Family,q.Parameters)));
        });
        Assert.All(tasks.Where(q=>q.Parameters["mode"]==9),
            q=>Assert.Contains("EDULYTICS FIRST-PARTY STEPS",q.Prompt));
        Assert.Contains(tasks,q=>q.Parameters["mode"]==4&&q.Prompt.Contains("37+25"));
        Assert.Contains(tasks,q=>q.Parameters["mode"]==5&&q.Prompt.Contains("23+68"));
    }
}
