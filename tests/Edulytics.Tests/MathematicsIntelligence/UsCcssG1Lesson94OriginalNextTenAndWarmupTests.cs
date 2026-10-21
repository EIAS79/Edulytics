using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Original 2021 U05 L06 numeric student-facing source tasks, excluding
/// copyright-controlled diagrams, pupil oral explanations and physical games.
/// Prepared for deferred Stage D; tests NOT executed during authoring.
/// </summary>
public sealed class UsCcssG1Lesson94OriginalNextTenAndWarmupTests
{
    private const string Family="usccss.g1.u05.l06.explain_make_ten_equations";

    [Theory]
    [InlineData(45,5,4)]
    [InlineData(38,2,5)]
    [InlineData(63,7,6)]
    [InlineData(24,6,7)]
    [InlineData(42,8,8)]
    [InlineData(57,3,9)]
    [InlineData(71,9,10)]
    [InlineData(89,1,11)]
    public void AllEightOriginalNextTenBlankEquationsHaveExactMissingValues(
        int start,int missing,int mode)
    {
        var p=new Dictionary<string,int>{{"a",start},{"b",missing},{"mode",mode}};
        var expected=$"MISSING={missing};TOTAL={start+missing}";
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,p));
        Assert.Equal(expected,UsCcssG1FullGradeUnit5AdditionQuestions.Solve(Family,p));
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,p,expected));
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
            Family,p,$"MISSING={missing+1};TOTAL={start+missing}"));
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
            Family,p,$"MISSING={missing};TOTAL={start+missing+10}"));
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
            Family,p,expected+";CERTIFY_TEACHER=YES"));
        p["b"]=missing+1;
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,p));
        p["b"]=missing;
        p["a"]=start-1;
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,p));
    }

    [Theory]
    [InlineData(8,2,12,10)]
    [InlineData(8,5,13,13)]
    [InlineData(9,8,14,17)]
    [InlineData(7,6,15,13)]
    public void FourOriginalMentalWarmupsAcceptCorrectTotalsAndNewTen(
        int a,int b,int mode,int sum)
    {
        var p=new Dictionary<string,int>{{"a",a},{"b",b},{"mode",mode}};
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,p));
        Assert.Equal($"TOTAL={sum};NEW_TEN=YES",
            UsCcssG1FullGradeUnit5AdditionQuestions.Solve(Family,p));
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
            Family,p,$"TOTAL={sum};NEW_TEN=YES"));
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
            Family,p,$"TOTAL={sum};NEW_TEN=NO"));
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
            Family,p,$"TOTAL={sum-1};NEW_TEN=YES"));
    }

    [Fact]
    public void PreviouslyExistingElenaAndreAndSixPlusSixtyEightAreUnchanged()
    {
        var elenaAndre=new Dictionary<string,int>{{"a",34},{"b",9},{"mode",2}};
        var sixPlus68=new Dictionary<string,int>{{"a",68},{"b",6},{"mode",3}};
        const string both="MOVE=6;REST=3;TOTAL=43;ELENA=34+6+3=43;ANDRE=9+1+33=43";
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
            Family,elenaAndre,both));
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
            Family,elenaAndre,"MOVE=6;REST=3;TOTAL=43"));
        Assert.Equal("MOVE=2;REST=4;TOTAL=74",
            UsCcssG1FullGradeUnit5AdditionQuestions.Solve(Family,sixPlus68));
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
            Family,sixPlus68,"MOVE=2;REST=4;TOTAL=74"));
    }

    [Fact]
    public void SeededGenerationCoversAllSixteenModesWithoutInvalidOperands()
    {
        var tasks=Enumerable.Range(0,2000).Select(i=>
            UsCcssG1FullGradeUnit5AdditionQuestions.Build(Family,new Random(i*43+21)))
            .ToArray();
        foreach(int mode in Enumerable.Range(0,16))
            Assert.Contains(tasks,q=>q.Parameters["mode"]==mode);
        Assert.All(tasks,q=>
        {
            Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,q.Parameters));
            Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
                Family,q.Parameters,UsCcssG1FullGradeUnit5AdditionQuestions.Solve(Family,q.Parameters)));
        });
        Assert.All(tasks.Where(q=>q.Parameters["mode"]>=4&&q.Parameters["mode"]<=11),
            q=>Assert.Contains("STUDENT EQUATION",q.Prompt));
        Assert.All(tasks.Where(q=>q.Parameters["mode"]>=12),
            q=>Assert.Contains("WARM-UP",q.Prompt));
    }
}
