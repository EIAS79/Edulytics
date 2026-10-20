using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Independently authored Edulytics formative on source skill, not copied from the
/// protected 2021 publisher Cool-down. These tests are authored, not run at A-C.
/// </summary>
public sealed class UsCcssG1Lesson81IndependentComparisonExitTests
{
    [Fact]
    public void IndependentComparisonExitRequiresCorrectSignAndTwoTensCounts()
    {
        const string family="usccss.g1.u04.l16.write_comparison";
        var p=new Dictionary<string,int>
        {
            ["a"]=63,["b"]=36,["c"]=0,["mode"]=14
        };
        const string correct="SYMBOL=>;TENS_LEFT=6;TENS_RIGHT=3";
        Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Valid(family,p));
        Assert.Equal(correct,UsCcssG1FullGradeUnit4FinalQuestions.Solve(family,p));
        Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Verify(family,p,correct));
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(family,p,
            "SYMBOL=<;TENS_LEFT=6;TENS_RIGHT=3"));
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(family,p,
            "SYMBOL=>;TENS_LEFT=3;TENS_RIGHT=6"));
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(family,p,
            "SYMBOL=>;TENS_LEFT=6"));
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(family,p,correct+";EXTRA=0"));
        p["a"]=64;
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Valid(family,p));
    }

    [Fact]
    public void AllOriginalOpenNumberBoxModesStillAcceptMultipleCorrectAnswers()
    {
        const string family="usccss.g1.u04.l16.write_comparison";
        var p=new Dictionary<string,int>
        {
            ["a"]=78,["b"]=0,["c"]=0,["mode"]=9
        };
        Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Verify(family,p,"NUMBER=79"));
        Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Verify(family,p,"NUMBER=99"));
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(family,p,"NUMBER=78"));
        p["a"]=0; p["mode"]=12;
        Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Verify(family,p,"LEFT=12;RIGHT=21"));
        Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Verify(family,p,"LEFT=22;RIGHT=23"));
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(family,p,"LEFT=23;RIGHT=22"));
    }

    [Fact]
    public void GeneratorMakesSourceModesAndIndependentExitWithCorrectOracles()
    {
        const string family="usccss.g1.u04.l16.write_comparison";
        var problems=Enumerable.Range(0,1500)
            .Select(i=>UsCcssG1FullGradeUnit4FinalQuestions.Build(
                family,new Random(i*47+19))).ToArray();
        foreach(int mode in Enumerable.Range(0,15))
            Assert.Contains(problems,q=>q.Parameters["mode"]==mode);
        Assert.All(problems.Where(x=>x.Parameters["mode"]==14),q=>
        {
            Assert.Contains("EDULYTICS INDEPENDENT FORMATIVE",q.Prompt);
            Assert.Contains("not the inaccessible publisher Cool-down",q.Prompt);
            Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Verify(family,q.Parameters,
                UsCcssG1FullGradeUnit4FinalQuestions.Solve(family,q.Parameters)));
        });
    }
}
