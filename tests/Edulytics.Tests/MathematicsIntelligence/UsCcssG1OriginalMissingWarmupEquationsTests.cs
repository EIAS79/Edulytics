using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Source exact outstanding 2021 G1 U08 L02/L04/L05 student equations.
/// Authored A-C, no CI or tests executed.
/// </summary>
public sealed class UsCcssG1OriginalMissingWarmupEquationsTests
{
    [Theory]
    [InlineData(138,8,5,0,9,"MISSING=3")]
    [InlineData(138,8,3,0,10,"MISSING=5")]
    [InlineData(138,7,4,0,11,"MISSING=3")]
    [InlineData(138,8,2,0,12,"MISSING=6")]
    [InlineData(138,8,4,0,13,"MISSING=4")]
    [InlineData(140,10,7,0,9,"RESULT=17")]
    [InlineData(140,8,2,7,10,"RESULT=17")]
    [InlineData(140,10,5,2,11,"RESULT=17")]
    [InlineData(141,57,20,59,10,"TRUE=NO;ACTUAL=77;STATED=59")]
    [InlineData(141,66,4,62,11,"TRUE=YES;ACTUAL=62;STATED=62")]
    [InlineData(141,17,76,59,12,"TRUE=NO;ACTUAL=93;STATED=59")]
    public void OriginalStudentValuesAndUnknownPositionsArePreserved(
        int lesson,int a,int b,int c,int mode,string expected)
    {
        var family=UsCcssG1FullGradeUnit8ReviewQuestions.Families[lesson-137];
        var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",c},{"mode",mode}};
        Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Valid(family,p));
        Assert.Equal(expected,UsCcssG1FullGradeUnit8ReviewQuestions.Solve(family,p));
        Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(family,p,expected));
        Assert.False(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(
            family,p,expected+";SOURCE_PICTURE=CERTIFIED"));
        p["a"]=a+1;
        Assert.False(UsCcssG1FullGradeUnit8ReviewQuestions.Valid(family,p));
    }

    [Theory]
    [InlineData(138,9,13)]
    [InlineData(140,9,11)]
    [InlineData(141,10,12)]
    public void NewExactModesAreReachableByOriginalPracticeFamily(
        int lesson,int firstMode,int lastMode)
    {
        var family=UsCcssG1FullGradeUnit8ReviewQuestions.Families[lesson-137];
        var problems=Enumerable.Range(0,1700)
            .Select(i=>UsCcssG1FullGradeUnit8ReviewQuestions.Build(
                family,new Random(i*43+lesson))).ToArray();
        foreach(int mode in Enumerable.Range(firstMode,lastMode-firstMode+1))
            Assert.Contains(problems,x=>x.Parameters["mode"]==mode);
    }
}
