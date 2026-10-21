using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// 2021 Grade 1 Unit 8 accessible original student-facing mathematical
/// source tasks. Student fluency from MEMORY, original photographs, real
/// counters, pupil-authored pages/stories, partner exchanges and galleries
/// require independent teacher assessment. Tests AUTHORED, not executed.
/// </summary>
public sealed class UsCcssG1Lessons137To146SourceMathTests
{
    private static readonly IReadOnlyList<string> Families =
        UsCcssG1FullGradeUnit8ReviewQuestions.Families;
    private static Dictionary<string,int> P(int a,int b,int mode)=>new()
    {
        ["a"]=a,["b"]=b,["c"]=0,["mode"]=mode
    };

    [Theory]
    [InlineData(137,4,5,0,"RESULT=9")]
    [InlineData(138,5,4,4,"RESULT=9")]
    [InlineData(138,4,5,5,"RESULT=9")]
    [InlineData(138,9,4,6,"RESULT=5")]
    [InlineData(138,9,5,7,"RESULT=4")]
    [InlineData(138,4,5,8,"MISSING_ADD=5;MISSING_SUB=4")]
    [InlineData(139,6,7,4,"RESULT=13")]
    [InlineData(139,13,7,5,"RESULT=6")]
    [InlineData(139,7,8,6,"RESULT=15")]
    [InlineData(139,15,7,7,"RESULT=8")]
    [InlineData(139,5,9,8,"ADD=5+9=14;SUB=14-8=6")]
    [InlineData(140,8,7,4,"CHANGE=7;FINAL=15")]
    [InlineData(140,17,12,5,"CHANGE=12;FINAL=5")]
    [InlineData(140,20,17,6,"CHANGE=17;FINAL=3")]
    [InlineData(140,6,8,7,"CHANGE=8;FINAL=14")]
    [InlineData(140,16,7,8,"CHANGE=7;FINAL=9")]
    [InlineData(141,7,11,4,"MISSING=11;TOTAL=18")]
    [InlineData(141,13,5,5,"MISSING=5;TOTAL=18")]
    [InlineData(141,6,9,6,"MISSING=9;TOTAL=15")]
    [InlineData(141,12,8,7,"MISSING=8;TOTAL=20")]
    [InlineData(141,9,8,8,"MATCH=A,C;MISSING=8")]
    [InlineData(141,11,5,9,"MATCH=A,C;MISSING=5")]
    [InlineData(142,12,7,4,"DIFFERENCE=5")]
    [InlineData(142,17,7,5,"DIFFERENCE=10")]
    [InlineData(142,13,9,6,"DIFFERENCE=4")]
    [InlineData(142,20,14,7,"DIFFERENCE=6")]
    [InlineData(143,103,0,2,"TENS=10;ONES=3;TOTAL=103")]
    [InlineData(144,84,0,4,"NUMBER=84;TENS=8;ONES=4")]
    [InlineData(144,84,0,5,"NUMBER=84;E1=20+64;E2=30+54;E3=40+44")]
    [InlineData(145,65,35,4,"TRUE=YES;LEFT=65;RIGHT=35")]
    [InlineData(145,65,65,5,"TRUE=YES;LEFT=65;RIGHT=65")]
    [InlineData(145,65,65,6,"TRUE=NO;LEFT=65;RIGHT=65")]
    [InlineData(145,34,44,7,"CANDIDATES=34,44,54")]
    [InlineData(145,24,20,8,"NUMBER=44")]
    [InlineData(145,51,32,9,"NUMBER=83")]
    [InlineData(146,60,70,4,"TRUE=NO;LEFT=60;RIGHT=70")]
    [InlineData(146,70,70,5,"TRUE=YES;LEFT=70;RIGHT=70")]
    [InlineData(146,80,69,6,"TRUE=NO;LEFT=80;RIGHT=69")]
    [InlineData(146,47,40,7,"NUMBER=47;LOWER=37;UPPER=57;TENS=4;ONES=7;ADD=40+7")]
    public void OriginalSourceNumbersAreTruthfulAndDoNotCertifyHumanActivity(
        int ordinal,int a,int b,int mode,string expected)
    {
        var family=Families[ordinal-137];
        var p=P(a,b,mode);
        Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Valid(family,p));
        Assert.Equal(expected,UsCcssG1FullGradeUnit8ReviewQuestions.Solve(family,p));
        Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(family,p,expected));
        Assert.False(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(
            family,p,expected+";ACTUAL_STUDENT_PAGE=SIGNED"));
    }

    [Fact]
    public void OriginalLesson139AllowsAnyOneAdditionAndSubtractionFromTheSixChoices()
    {
        var family=Families[2];
        var p=P(5,9,8);
        Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(
            family,p,"ADD=4+7=11;SUB=13-6=7"));
        Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(
            family,p,"ADD=6+4+4=14;SUB=15-9=6"));
        Assert.False(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(
            family,p,"ADD=4+7=12;SUB=13-6=7"));
        Assert.False(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(
            family,p,"ADD=5+9=14;SUB=12-5=7"));
        Assert.False(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(
            family,p,"ADD=5+9=14"));
    }

    [Fact]
    public void OriginalLesson144NumberBookRequiresThreeDistinctTrueAdditions()
    {
        var family=Families[7];
        var p=P(84,0,5);
        Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(
            family,p,"NUMBER=84;E1=10+74;E2=20+64;E3=40+44"));
        Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(
            family,p,"E3=4+80;E2=30+54;E1=0+84;NUMBER=84"));
        foreach(var bad in new[]{
            "NUMBER=84;E1=20+64;E2=64+20;E3=40+44",
            "NUMBER=84;E1=20+64;E2=30+54",
            "NUMBER=84;E1=20+64;E2=30+54;E3=40+45",
            "NUMBER=85;E1=20+64;E2=30+54;E3=40+44",
            "NUMBER=84;E1=-1+85;E2=30+54;E3=40+44"})
            Assert.False(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(family,p,bad));
        Assert.False(UsCcssG1FullGradeUnit8ReviewQuestions.Valid(
            family,P(119,0,4)));
    }

    [Fact]
    public void SourceLesson146FourClueScaffoldAcceptsAlternateValidMathButNotFalseClues()
    {
        var family=Families[9];
        var p=P(47,40,7);
        Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(
            family,p,"NUMBER=47;LOWER=5;UPPER=120;TENS=4;ONES=7;ADD=10+37"));
        Assert.False(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(
            family,p,"NUMBER=47;LOWER=47;UPPER=120;TENS=4;ONES=7;ADD=10+37"));
        Assert.False(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(
            family,p,"NUMBER=47;LOWER=5;UPPER=120;TENS=5;ONES=7;ADD=10+37"));
        Assert.False(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(
            family,p,"NUMBER=47;LOWER=5;UPPER=120;TENS=4;ONES=7;ADD=11+37"));
    }

    [Theory]
    [InlineData(137)]
    [InlineData(138)]
    [InlineData(139)]
    [InlineData(140)]
    [InlineData(141)]
    [InlineData(142)]
    [InlineData(143)]
    [InlineData(144)]
    [InlineData(145)]
    [InlineData(146)]
    public void EveryOriginalReviewLessonGeneratesSourceScopedArithmetic(int ordinal)
    {
        var family=Families[ordinal-137];
        var cases=Enumerable.Range(0,1200).Select(i=>
            UsCcssG1FullGradeUnit8ReviewQuestions.Build(family,new Random(i*37+ordinal))).ToArray();
        Assert.All(cases,q=>
        {
            Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Valid(family,q.Parameters));
            Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(
                family,q.Parameters,
                UsCcssG1FullGradeUnit8ReviewQuestions.Solve(family,q.Parameters)));
        });
        if(ordinal is 138 or 139)
            Assert.Contains(cases,q=>q.Parameters["mode"]==8);
        if(ordinal==144)
            Assert.Contains(cases,q=>q.Parameters["mode"]==5);
    }
}
