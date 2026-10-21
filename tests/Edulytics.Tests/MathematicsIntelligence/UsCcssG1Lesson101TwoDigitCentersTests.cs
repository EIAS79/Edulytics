using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Source goals for U05 L13 are two-digit addition in Target Numbers Stage 3
/// and with composing in Five in a Row Stage 6; a numeric answer cannot
/// certify source board, cards, turn-taking or drawings. Not yet executed.
/// </summary>
public sealed class UsCcssG1Lesson101TwoDigitCentersTests
{
    private const string Family="usccss.g1.u05.l13.center_add_within100";

    [Theory]
    [InlineData(26,38,4,"TENS=6;ONES=4;NEW_TEN=YES;TOTAL=64")]
    [InlineData(54,25,5,"TENS=7;ONES=9;NEW_TEN=NO;TOTAL=79")]
    public void SourceTeachingExamplesGradeRegroupingAndPlaceValue(
        int a,int b,int mode,string expected)
    {
        var p=new Dictionary<string,int>{{"a",a},{"b",b},{"mode",mode}};
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,p));
        Assert.Equal(expected,UsCcssG1FullGradeUnit5AdditionQuestions.Solve(Family,p));
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,p,expected));
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
            Family,p,expected.Replace("TOTAL="+(a+b),"TOTAL="+(a+b+1))));
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
            Family,p,expected+";PARTNER_GAME=COMPLETED"));
        p["b"]=b+1;
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,p));
    }

    [Fact]
    public void GeneratedTwoDigitQuestionsRequireAppropriateCarryClass()
    {
        var items=Enumerable.Range(0,2400).Select(i=>
            UsCcssG1FullGradeUnit5AdditionQuestions.Build(
                Family,new Random(i*37+13))).ToArray();
        foreach(int mode in Enumerable.Range(0,8))
            Assert.Contains(items,q=>q.Parameters["mode"]==mode);
        Assert.All(items,q=>
        {
            Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,q.Parameters));
            Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
                Family,q.Parameters,UsCcssG1FullGradeUnit5AdditionQuestions.Solve(Family,q.Parameters)));
        });
        Assert.All(items.Where(q=>q.Parameters["mode"]>=6),q=>
        {
            var p=q.Parameters;
            Assert.InRange(p["a"],10,89);
            Assert.InRange(p["b"],10,89);
            Assert.Equal(p["mode"]==6,p["a"]%10+p["b"]%10>=10);
            Assert.True(p["a"]+p["b"]<=100);
            Assert.Contains("NOT auto-certified",q.Prompt);
        });
    }
}
