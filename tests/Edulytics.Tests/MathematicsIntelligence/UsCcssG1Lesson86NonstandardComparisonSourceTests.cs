using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG1Lesson86NonstandardComparisonSourceTests
{
    [Theory]
    [InlineData(2,82,72,"SYMBOL=>")]
    [InlineData(4,52,42,"SYMBOL=>")]
    [InlineData(5,32,32,"SYMBOL==")]
    [InlineData(6,35,40,"SYMBOL=<")]
    public void Original2021Lesson21ComparesEquivalentAndUnequalNonstandardForms(
        int mode,int left,int right,string expected)
    {
        const string family="usccss.g1.u04.l21.compare_nonstandard_forms";
        var p=new Dictionary<string,int>
        {
            ["a"]=left,["b"]=right,["c"]=0,["mode"]=mode
        };
        Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Valid(family,p));
        Assert.Equal(expected,UsCcssG1FullGradeUnit4FinalQuestions.Solve(family,p));
        Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Verify(family,p,expected));
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(family,p,"SYMBOL=?"));
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(family,p,expected+";EXTRA=1"));
        p["a"]++;
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Valid(family,p));
    }

    [Fact]
    public void SourceAlignedNewCasesAreActuallyGeneratedAndRetainPriorCase()
    {
        const string family="usccss.g1.u04.l21.compare_nonstandard_forms";
        var counts=new int[7];
        for(int seed=0;seed<700;seed++)
        {
            var q=UsCcssG1FullGradeUnit4FinalQuestions.Build(
                family,new Random(seed*47+20261020));
            counts[q.Parameters["mode"]]++;
            var answer=UsCcssG1FullGradeUnit4FinalQuestions.Solve(family,q.Parameters);
            Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Verify(family,q.Parameters,answer));
            if(q.Parameters["mode"]==4)Assert.Contains("5 tens and 2 ones",q.Prompt);
            if(q.Parameters["mode"]==5)Assert.Contains("2 tens and 12 ones",q.Prompt);
            if(q.Parameters["mode"]==6)Assert.Contains("2 tens and 15 ones",q.Prompt);
        }
        foreach(var mode in new[]{2,4,5,6})
            Assert.True(counts[mode]>40);
    }
}
