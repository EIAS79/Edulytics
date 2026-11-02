using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// IM 2021 L19 explicitly permits 37 as 36+1 or 35+2 as well as tens/ones.
/// Tests authored during A-C; neither .NET nor CI executed.
/// </summary>
public sealed class UsCcssG1Lesson84Multiple37RepresentationsTests
{
    private const string Family="usccss.g1.u04.l19.regroup_one_ten";

    [Fact]
    public void SourceAcceptsAnyTwoDistinctCorrectAddendPairsForThirtySeven()
    {
        var p=new Dictionary<string,int>{{"a",37},{"b",0},{"c",0},{"mode",4}};
        Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Valid(Family,p));
        Assert.Equal("PAIR1=36+1;PAIR2=35+2",
            UsCcssG1FullGradeUnit4FinalQuestions.Solve(Family,p));
        foreach(var valid in new[]{
            "PAIR1=36+1;PAIR2=35+2",
            "PAIR1=30+7;PAIR2=20+17",
            "PAIR1=0+37;PAIR2=1+36",
            "PAIR1=35+2;PAIR2=30+7"
        })Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Verify(Family,p,valid));
        foreach(var wrong in new[]{
            "PAIR1=36+1;PAIR2=36+1",
            "PAIR1=36+1;PAIR2=1+36",
            "PAIR1=36+1;PAIR2=35+3",
            "PAIR1=36+1;PAIR2=34+2",
            "PAIR1=-1+38;PAIR2=35+2",
            "PAIR1=36+1",
            "PAIR1=36+1;PAIR2=35+2;FAKE=0"
        })Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(Family,p,wrong));
        p["a"]=38;
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Valid(Family,p));
    }

    [Fact]
    public void OriginalSixtyFiveProhibitionAndTwoTensWaysContractsStayUnchanged()
    {
        var sixtyFive=new Dictionary<string,int>{{"a",65},{"b",0},{"c",0},{"mode",0}};
        Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Verify(
            Family,sixtyFive,"TENS=5;ONES=15"));
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(
            Family,sixtyFive,"TENS=6;ONES=5"));
        var thirtySeven=new Dictionary<string,int>{{"a",37},{"b",0},{"c",0},{"mode",1}};
        Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Verify(
            Family,thirtySeven,"TENS=3;ONES=7;ALT_TENS=2;ALT_ONES=17"));
    }

    [Fact]
    public void AllFiveModeRangesRemainGenerated()
    {
        var all=Enumerable.Range(0,1300).Select(i=>
            UsCcssG1FullGradeUnit4FinalQuestions.Build(Family,new Random(i*37+21))).ToArray();
        foreach(int mode in Enumerable.Range(0,5))
            Assert.Contains(all,q=>q.Parameters["mode"]==mode);
        Assert.All(all.Where(x=>x.Parameters["mode"]==4),q=>
        {
            Assert.Contains("TWO DIFFERENT addition expressions",q.Prompt);
            Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Verify(
                Family,q.Parameters,UsCcssG1FullGradeUnit4FinalQuestions.Solve(Family,q.Parameters)));
        });
    }
}
