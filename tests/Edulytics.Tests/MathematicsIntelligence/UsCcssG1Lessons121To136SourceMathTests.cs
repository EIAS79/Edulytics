using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Original 2021 G1 Unit 7 source-goal numeric/shape/clock proxies.
/// The source's actual blocks, cut-out foldings, shape cards, colorings,
/// pupil drawings, peer conversations and clock diagrams are NOT
/// automatically certified. Written during A-C, not executed until D.
/// </summary>
public sealed class UsCcssG1Lessons121To136SourceMathTests
{
    private static readonly IReadOnlyList<string> Families =
        UsCcssG1FullGradeUnit7GeometryTimeQuestions.Families;

    private static Dictionary<string,int> P(int a,int b,int c,int mode)=>new()
    {
        ["a"]=a,["b"]=b,["c"]=c,["mode"]=mode
    };

    [Theory]
    [InlineData(121,2,3,0,4,"PER_LAYER=6;LAYERS=2;TOTAL_CUBES=12")]
    [InlineData(122,4,3,0,4,"CURVED=CIRCLE;THREE_SIDES=TRIANGLE;FOUR_SIDES=SQUARE,RECTANGLE")]
    [InlineData(123,3,2,0,0,"SIDES=3;CORNERS=3")]
    [InlineData(123,3,2,0,1,"SIDES=4;CORNERS=4")]
    [InlineData(124,3,2,0,4,"TRIANGLES=A,B,C;NONTRIANGLES=D,E,F")]
    [InlineData(125,3,2,0,4,"RECTANGLE=YES;SQUARE=YES")]
    [InlineData(125,3,2,0,5,"RECTANGLE=YES;SQUARE=NO")]
    [InlineData(125,3,2,0,6,"RECTANGLE=NO;SQUARE=NO")]
    [InlineData(126,3,2,0,4,"TRIANGLES=6;TRAPEZOIDS=2;SAME_OUTLINE=YES")]
    [InlineData(127,3,2,0,4,"BOTTOM=2;TOP=1;TOTAL_CUBES=3")]
    [InlineData(127,6,4,8,5,"RESULT=18")]
    [InlineData(127,6,8,4,6,"RESULT=18")]
    [InlineData(127,6,8,5,7,"RESULT=19")]
    [InlineData(127,6,5,9,8,"RESULT=20")]
    [InlineData(128,3,2,0,4,"CIRCLE_PART=HALF;CIRCLE_PIECES=2;SQUARE_PART=HALF;SQUARE_PIECES=2")]
    [InlineData(128,3,2,0,5,"CIRCLE_PART=FOURTH;CIRCLE_PIECES=4;SQUARE_PART=FOURTH;SQUARE_PIECES=4")]
    [InlineData(129,3,2,0,2,"SORT=UNEQUAL;PIECES=2")]
    [InlineData(129,3,2,0,3,"SORT=EQUAL;PIECES=4")]
    [InlineData(130,3,2,0,0,"LARGER=HALF;FOURTHS_PER_HALF=2")]
    [InlineData(131,3,2,0,0,"RESULT=5")]
    [InlineData(132,7,3,0,4,"TIME=7:00")]
    [InlineData(133,7,3,0,4,"ORDER=C,A,B")]
    [InlineData(134,7,3,0,4,"TIME=7:30")]
    [InlineData(135,8,32,0,4,"RESULT=40")]
    [InlineData(135,6,12,0,8,"TIME=6:00;CORRECT=DIEGO")]
    [InlineData(136,3,2,0,1,"SHAPE=RECTANGLE")]
    public void SourceSpecificDigitalContractsGradeOnlyTruthfulMath(
        int ordinal,int a,int b,int c,int mode,string expected)
    {
        var family=Families[ordinal-120];
        var p=P(a,b,c,mode);
        Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Valid(family,p));
        Assert.Equal(expected,UsCcssG1FullGradeUnit7GeometryTimeQuestions.Solve(family,p));
        Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Verify(family,p,expected));
        Assert.False(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Verify(
            family,p,expected+";PUPIL_DRAWING=VERIFIED"));
    }

    [Fact]
    public void OriginalHalfPastHourHandQuestionAcceptsAnyHourNotJustGeneratedSeed()
    {
        var p=P(7,3,0,4);
        var family=Families[14];
        foreach(int hour in Enumerable.Range(1,12))
            Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Verify(
                family,p,$"TIME={hour}:30"));
        foreach(var invalid in new[]{"TIME=0:30","TIME=13:30","TIME=7:00",
             "TIME=3:31","TIME=7:30;DRAWN=TRUE","HOUR=7"})
            Assert.False(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Verify(
                family,p,invalid));
    }

    [Theory]
    [InlineData(121)]
    [InlineData(122)]
    [InlineData(123)]
    [InlineData(124)]
    [InlineData(125)]
    [InlineData(126)]
    [InlineData(127)]
    [InlineData(128)]
    [InlineData(129)]
    [InlineData(130)]
    [InlineData(131)]
    [InlineData(132)]
    [InlineData(133)]
    [InlineData(134)]
    [InlineData(135)]
    [InlineData(136)]
    public void EveryUnit7SourceFamilyBuildsScorableDigitalMathWithNoFakeArt(int ordinal)
    {
        var family=Families[ordinal-120];
        var questions=Enumerable.Range(0,900)
            .Select(i=>UsCcssG1FullGradeUnit7GeometryTimeQuestions.Build(
                family,new Random(i*37+ordinal))).ToArray();
        Assert.All(questions,q=>
        {
            Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Valid(family,q.Parameters));
            var answer=UsCcssG1FullGradeUnit7GeometryTimeQuestions.Solve(family,q.Parameters);
            Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Verify(family,q.Parameters,answer));
            Assert.False(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Verify(
                family,q.Parameters,answer+";SOURCE_PICTURE=APPROVED"));
        });
        if(ordinal is 127 or 132 or 134)
            Assert.Contains(questions,q=>q.Parameters["mode"]==4);
        if(ordinal==127)
            foreach(int mode in Enumerable.Range(5,4))
                Assert.Contains(questions,q=>q.Parameters["mode"]==mode);
    }
}
