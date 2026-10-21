using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Original public 2021 G1 U05 L09: 17+36, with four warm-up equations.
/// Graded numeric decomposition is not a claim of copied publisher drawings
/// or of actual teacher-facilitated Grab and Add handfuls.
/// Authored only; Stage D and .NET tests not run.
/// </summary>
public sealed class UsCcssG1Lesson97OriginalSeventeenPlusThirtySixTests
{
    private const string Family="usccss.g1.u05.l09.two_digit_composing";

    [Theory]
    [InlineData(38,2,4,40)]
    [InlineData(40,3,5,43)]
    [InlineData(38,5,6,43)]
    [InlineData(38,15,7,53)]
    public void OriginalWarmupTasksGradeSourceArithmeticAndTrueNewTen(
        int a,int b,int mode,int sum)
    {
        var p=new Dictionary<string,int>{{"a",a},{"b",b},{"mode",mode}};
        var expected=$"NEW_TEN={(a%10+b%10>=10?"YES":"NO")};TOTAL={sum}";
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,p));
        Assert.Equal(expected,UsCcssG1FullGradeUnit5AdditionQuestions.Solve(Family,p));
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,p,expected));
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,p,
            expected.Replace("TOTAL="+sum,"TOTAL="+(sum+1))));
    }

    [Fact]
    public void OriginalStudentSeventeenPlusThirtySixRequiresComposedTenAndThreeOnes()
    {
        var p=new Dictionary<string,int>{{"a",17},{"b",36},{"mode",8}};
        const string correct="TENS=5;ONES=3;TOTAL=53";
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,p));
        Assert.Equal(correct,UsCcssG1FullGradeUnit5AdditionQuestions.Solve(Family,p));
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,p,correct));
        foreach(var wrong in new[]
        {
            "TENS=4;ONES=13;TOTAL=53",
            "TENS=4;ONES=3;TOTAL=53",
            "TENS=5;ONES=13;TOTAL=53",
            "TENS=5;ONES=3;TOTAL=43",
            "TOTAL=53",
            correct+";REAL_HANDful=YES"
        })
            Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,p,wrong));
        p["a"]=18;
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,p));
        p["a"]=17;
        p["b"]=35;
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,p));
    }

    [Fact]
    public void NineOriginalAndGeneratedModesAreReachableAndMathematicallySound()
    {
        var tasks=Enumerable.Range(0,1500)
            .Select(i=>UsCcssG1FullGradeUnit5AdditionQuestions.Build(
                Family,new Random(i*47+11))).ToArray();
        foreach(int mode in Enumerable.Range(0,9))
            Assert.Contains(tasks,q=>q.Parameters["mode"]==mode);
        Assert.All(tasks,q=>
        {
            Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,q.Parameters));
            Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,q.Parameters,
                UsCcssG1FullGradeUnit5AdditionQuestions.Solve(Family,q.Parameters)));
        });
        Assert.All(tasks.Where(q=>q.Parameters["mode"]==8),q=>
        {
            Assert.Contains("17+36",q.Prompt);
            Assert.Contains("not digitally certified",q.Prompt);
        });
    }
}
