using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Grade 1 U05 L07: exact 2021 Jada number pairs plus regression for
/// original four different open-ended smudged conditions. Tests authored
/// but not executed during A-C. Source artwork and pupil explanations
/// remain human assessed.
/// </summary>
public sealed class UsCcssG1Lesson95OriginalJadaNewTenTests
{
    private const string Family="usccss.g1.u05.l07.detect_new_ten";

    [Theory]
    [InlineData(45,5,6,"NEW_TEN=YES;TOTAL=50")]
    [InlineData(9,63,7,"NEW_TEN=YES;TOTAL=72")]
    [InlineData(26,3,8,"NEW_TEN=NO;TOTAL=29")]
    [InlineData(8,47,9,"NEW_TEN=YES;TOTAL=55")]
    public void OriginalJadaPairsGradeBothCarryDecisionAndSum(
        int a,int b,int mode,string expected)
    {
        var p=new Dictionary<string,int>{{"a",a},{"b",b},{"mode",mode}};
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,p));
        Assert.Equal(expected,UsCcssG1FullGradeUnit5AdditionQuestions.Solve(Family,p));
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,p,expected));
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
            Family,p,expected.Replace("NEW_TEN=YES","NEW_TEN=NO")
                              .Replace("NEW_TEN=NO","NEW_TEN=MAYBE")));
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
            Family,p,expected.Replace("TOTAL="+(a+b),"TOTAL="+(a+b+1))));
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
            Family,p,expected+";PUBLISHER_IMAGE=YES"));
        p["a"]=a+1;
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,p));
    }

    [Theory]
    [InlineData(32,8,2,"MISSING=9;NEW_TEN=YES;TOTAL=41")]
    [InlineData(16,0,3,"MISSING=2;NEW_TEN=NO;TOTAL=18")]
    [InlineData(8,12,4,"MISSING=49;NEW_TEN=YES;TOTAL=57")]
    [InlineData(8,11,5,"MISSING=41;NEW_TEN=NO;TOTAL=49")]
    public void OpenMissingValueModeStillAcceptsAnotherMathematicallyValidChoice(
        int known,int chosen,int mode,string alternative)
    {
        var p=new Dictionary<string,int>{{"a",known},{"b",chosen},{"mode",mode}};
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,p));
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,p,alternative));
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
            Family,p,alternative+";INVENTED=1"));
    }

    [Fact]
    public void SeededGeneratorCoversAllTenModesWithNoUnverifiedSourceConditions()
    {
        var generated=Enumerable.Range(0,1800)
            .Select(i=>UsCcssG1FullGradeUnit5AdditionQuestions.Build(
                Family,new Random(i*47+7))).ToArray();
        foreach(int mode in Enumerable.Range(0,10))
            Assert.Contains(generated,q=>q.Parameters["mode"]==mode);
        Assert.All(generated,q=>
        {
            Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,q.Parameters));
            Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
                Family,q.Parameters,UsCcssG1FullGradeUnit5AdditionQuestions.Solve(Family,q.Parameters)));
        });
        Assert.All(generated.Where(q=>q.Parameters["mode"]>=6),
            q=>Assert.Contains("JADA STUDENT TASK",q.Prompt));
        Assert.All(generated.Where(q=>q.Parameters["mode"]>=2&&q.Parameters["mode"]<=5),
            q=>Assert.Contains("COULD",q.Prompt));
    }
}
