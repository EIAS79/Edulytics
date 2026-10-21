using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG1Lesson146OriginalFourClueWritingTests
{
    private const string Family="usccss.g1.u08.l10.write_riddle_numeric_check";

    [Theory]
    [InlineData(4,60,70,"TRUE=NO;LEFT=60;RIGHT=70")]
    [InlineData(5,70,70,"TRUE=YES;LEFT=70;RIGHT=70")]
    [InlineData(6,80,69,"TRUE=NO;LEFT=80;RIGHT=69")]
    public void OriginalThreeWarmupStatementsRejectIncorrectTruthAndArithmetic(
        int mode,int left,int right,string expected)
    {
        var p=new Dictionary<string,int>
        {
            ["a"]=left,["b"]=right,["c"]=0,["mode"]=mode
        };
        Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Valid(Family,p));
        Assert.Equal(expected,UsCcssG1FullGradeUnit8ReviewQuestions.Solve(Family,p));
        Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(Family,p,expected));
        Assert.False(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(
            Family,p,"TRUE=YES;LEFT=60;RIGHT=60"));
        p["a"]++;
        Assert.False(UsCcssG1FullGradeUnit8ReviewQuestions.Valid(Family,p));
    }

    [Fact]
    public void StudentCanAuthorMultipleConsistentNumberBoundsAndAdditionClues()
    {
        var p=new Dictionary<string,int>
        {
            ["a"]=47,["b"]=40,["c"]=0,["mode"]=7
        };
        Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Valid(Family,p));
        foreach(var response in new[]
        {
            "NUMBER=47;LOWER=40;UPPER=50;TENS=4;ONES=7;ADD=40+7",
            "ADD=30+17;UPPER=48;NUMBER=47;LOWER=46;ONES=7;TENS=4",
            "NUMBER=47;LOWER=0;UPPER=121;TENS=4;ONES=7;ADD=0+47"
        })
            Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(Family,p,response));
        foreach(var wrong in new[]
        {
            "NUMBER=47;LOWER=47;UPPER=50;TENS=4;ONES=7;ADD=40+7",
            "NUMBER=47;LOWER=40;UPPER=47;TENS=4;ONES=7;ADD=40+7",
            "NUMBER=47;LOWER=40;UPPER=50;TENS=5;ONES=7;ADD=40+7",
            "NUMBER=47;LOWER=40;UPPER=50;TENS=4;ONES=7;ADD=40+8",
            "NUMBER=47;LOWER=40;UPPER=50;TENS=4;ONES=7;ADD=40+7;EXTRA=1",
            "NUMBER=47;LOWER=40;UPPER=50;TENS=4;ONES=7;ADD=45-2",
            "NUMBER=47;LOWER=40;UPPER=50;TENS=4;ONES=7;ADD=-1+48"
        })
            Assert.False(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(Family,p,wrong));
    }

    [Fact]
    public void StudentScaffoldHandlesWholeNumbersUpTo120AndOnlyChecksConsistency()
    {
        foreach(var number in new[]{1,9,10,47,99,100,112,120})
        {
            var p=new Dictionary<string,int>
            {
                ["a"]=number,["b"]=number/2,["c"]=0,["mode"]=8
            };
            Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Valid(Family,p));
            var exemplar=UsCcssG1FullGradeUnit8ReviewQuestions.Solve(Family,p);
            Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(Family,p,exemplar));
            var valid=$"NUMBER={number};LOWER={number-1};UPPER={number+1};TENS={number/10};ONES={number%10};ADD=1+{number-1}";
            Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(Family,p,valid));
        }
        var generated=Enumerable.Range(0,2000).Select(i=>
            UsCcssG1FullGradeUnit8ReviewQuestions.Build(Family,new Random(23*i+17))).ToArray();
        foreach(var mode in new[]{4,5,6,7,8})
            Assert.True(generated.Count(x=>x.Parameters["mode"]==mode)>100);
        Assert.All(generated,x=>Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(
            Family,x.Parameters,UsCcssG1FullGradeUnit8ReviewQuestions.Solve(Family,x.Parameters))));
    }
}
