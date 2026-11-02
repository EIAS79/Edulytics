using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Grade 1 U04 L17. Original publicly accessible 2021 figure ALT maths is represented
/// through first-party text, not copied artwork. Tests authored; Stage D execution deferred.
/// </summary>
public sealed class UsCcssG1Lesson82SourceRepresentationAndIndependentExitTests
{
    private const string Family="usccss.g1.u04.l17.order_three_numbers";

    [Fact]
    public void OriginalFourAccessibleRepresentationValuesHaveCorrectOrderedOracle()
    {
        var p=new Dictionary<string,int>{{"a",15},{"b",33},{"c",51},{"mode",7}};
        Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Valid(Family,p));
        Assert.Equal("ORDER=15,33,51,53",
            UsCcssG1FullGradeUnit4FinalQuestions.Solve(Family,p));
        Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Verify(Family,p,"ORDER=15,33,51,53"));
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(Family,p,"ORDER=15,33,53,51"));
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(Family,p,"ORDER=15,33,51"));
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(Family,p,"ORDER=15,33,51,53,54"));
        p["a"]=16;
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Valid(Family,p));
    }

    [Fact]
    public void IndependentOrderingExitSeparatelyChecksTensAndOnesOrder()
    {
        var p=new Dictionary<string,int>{{"a",43},{"b",34},{"c",40},{"mode",8}};
        Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Valid(Family,p));
        Assert.Equal("ORDER=4,34,40,43",
            UsCcssG1FullGradeUnit4FinalQuestions.Solve(Family,p));
        Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Verify(Family,p,"ORDER=4,34,40,43"));
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(Family,p,"ORDER=34,4,40,43"));
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(Family,p,"ORDER=4,34,43,40"));
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(Family,p,"ORDER=4,34,40,43;BONUS=1"));
        p["b"]=35;
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Valid(Family,p));
    }

    [Fact]
    public void OriginalChoiceUnderTwentyFiveAcceptsZeroWithoutAcceptingNegatives()
    {
        var p=new Dictionary<string,int>{{"a",25},{"b",0},{"c",0},{"mode",6}};
        Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Valid(Family,p));
        Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Verify(Family,p,"NUMBER=0"));
        Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Verify(Family,p,"NUMBER=24"));
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(Family,p,"NUMBER=-1"));
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(Family,p,"NUMBER=25"));
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(Family,p,"NUMBER=0;EXTRA=1"));
    }

    [Fact]
    public void OriginalAndIndependentModesRemainInGeneratorWithoutRunningTestsYet()
    {
        var items=Enumerable.Range(0,1500)
            .Select(i=>UsCcssG1FullGradeUnit4FinalQuestions.Build(Family,new Random(i*37+9))).ToArray();
        foreach(int mode in Enumerable.Range(0,9))
            Assert.Contains(items,q=>q.Parameters["mode"]==mode);
        Assert.All(items.Where(x=>x.Parameters["mode"]==7),q=>
        {
            Assert.Contains("SOURCE-ALIGNED ACCESSIBLE NUMBER DESCRIPTIONS",q.Prompt);
            Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Verify(
                Family,q.Parameters,"ORDER=15,33,51,53"));
        });
        Assert.All(items.Where(x=>x.Parameters["mode"]==8),q=>
        {
            Assert.Contains("INDEPENDENT EDULYTICS FORMATIVE",q.Prompt);
            Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Verify(
                Family,q.Parameters,"ORDER=4,34,40,43"));
        });
    }
}
