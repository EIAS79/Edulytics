using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Standalone authoring-stage oracles for U06 L05 through L17.
/// Accessible 2021 source text and first-party exact mathematics only.
/// Physical insects, original paper-clip pictures, real foot/shoe/desk
/// measures, child-written stories and partner games remain teacher-only.
/// These tests are AUTHORED, NOT EXECUTED until the Stage D gate.
/// </summary>
public sealed class UsCcssG1Lessons107To119SourceScopedRegressionTests
{
    private static readonly IReadOnlyList<string> Families =
        UsCcssG1FullGradeUnit6MeasurementQuestions.Families;

    private static Dictionary<string,int> Parameters(int a,int b,int c,int mode)=>new()
    {
        ["a"]=a,["b"]=b,["c"]=c,["mode"]=mode
    };

    [Theory]
    // L107: mathematical length expressed in connected-cube units.
    [InlineData(107,4,0,0,0,"LENGTH=4")]
    [InlineData(107,5,0,0,2,"LENGTH=5")]
    // L108: no gaps, gaps, overlaps and unequal-size unit counterexamples.
    [InlineData(108,6,0,0,4,"VALID=YES")]
    [InlineData(108,6,0,0,5,"VALID=NO")]
    [InlineData(108,6,0,0,6,"VALID=NO")]
    [InlineData(108,6,0,0,7,"VALID=NO")]
    // L109: one unchanged length, three unit sizes.
    [InlineData(109,6,2,0,4,"SMALL_UNITS=6;MEDIUM_UNITS=3;LARGE_UNITS=2")]
    [InlineData(109,12,2,0,4,"SMALL_UNITS=12;MEDIUM_UNITS=6;LARGE_UNITS=4")]
    // L110/L111: 100+ complete tens and ones.
    [InlineData(110,109,0,0,4,"TENS=10;ONES=9;TOTAL=109")]
    [InlineData(111,120,0,0,0,"NUMBER=120")]
    // L112: estimate and independently measured first-party lengths and source warmup.
    [InlineData(112,12,15,0,4,"ACTUAL=12;ESTIMATE=15;DIFFERENCE=3;DIRECTION=HIGH")]
    [InlineData(112,24,54,0,5,"TRUE=NO;ACTUAL=54")]
    [InlineData(112,24,64,0,6,"TRUE=YES;ACTUAL=64")]
    [InlineData(112,30,75,0,7,"TRUE=YES;ACTUAL=75")]
    [InlineData(112,32,77,0,8,"TRUE=NO;ACTUAL=77")]
    // L113: original four shoe-comparison story quantities.
    [InlineData(113,9,7,0,4,"TOTAL=16")]
    [InlineData(113,7,9,4,5,"TOTAL=20")]
    [InlineData(113,13,8,0,6,"DIFFERENCE=5")]
    [InlineData(113,8,17,0,7,"ELENA=9")]
    // L114: compare-bigger/compare-smaller UNKNOWN, not known difference.
    [InlineData(114,14,4,0,4,"LENGTH=10;DIFFERENCE=4")]
    [InlineData(114,10,4,0,5,"LENGTH=14;DIFFERENCE=4")]
    // L115: Elena removed nine beads, eleven remain; 20 to begin.
    [InlineData(115,20,9,0,4,"START=20")]
    // L116: each ORIGINAL classroom sticker story requires two equations.
    [InlineData(116,11,8,0,4,"MATCH=A,C;UNKNOWN=3")]
    [InlineData(116,19,11,0,5,"MATCH=A,C;UNKNOWN=8")]
    [InlineData(116,16,10,0,6,"MATCH=A,C;UNKNOWN=6")]
    [InlineData(116,14,9,0,7,"MATCH=A,C;TOTAL=14")]
    [InlineData(116,16,6,0,8,"MATCH=A,C;UNKNOWN=10")]
    [InlineData(116,14,9,0,9,"MATCH=A,C;UNKNOWN=14")]
    // L117: Clare gives twelve origami cranes to Han and retains seven.
    [InlineData(117,19,12,0,4,"START=19;EQUATION=19-12=7")]
    // L118: true grouping beyond 100 and four related mental calculations.
    [InlineData(118,107,0,0,4,"TENS=10;ONES=7;TOTAL=107")]
    [InlineData(118,65,21,0,5,"FIRST=86;SECOND=86;EQUAL=YES")]
    [InlineData(118,65,20,0,6,"RESULT=85")]
    [InlineData(118,85,1,0,7,"RESULT=86")]
    [InlineData(118,65,21,0,8,"RESULT=86")]
    [InlineData(118,65,21,0,9,"RESULT=86")]
    // L119 source is original authored stories, not a fixed story from source.
    [InlineData(119,12,5,0,0,"DIFFERENCE=5")]
    public void SourceBoundMathematicsPreservesOriginalValuesAndRejectsInventedEvidence(
        int lesson,int a,int b,int c,int mode,string expected)
    {
        var family=Families[lesson-103];
        var parameters=Parameters(a,b,c,mode);
        Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(family,parameters));
        Assert.Equal(expected,
            UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(family,parameters));
        Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Verify(
            family,parameters,expected));
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Verify(
            family,parameters,expected+";REAL_CLASSROOM_ACTIVITY=CERTIFIED"));
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Verify(
            family,parameters,"UNSUPPORTED=TRUE"));
    }

    [Fact]
    public void SourceSpecificInvalidInputsCannotBeSilentlyCountedAsValid()
    {
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(
            Families[6],Parameters(7,2,0,4))); // Three-unit tool must tile.
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(
            Families[7],Parameters(100,0,0,4))); // Original over 100 case.
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(
            Families[10],Parameters(9,9,0,4))); // Wrong source shoe pair.
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(
            Families[11],Parameters(14,5,0,4))); // Han's 4 fewer.
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(
            Families[12],Parameters(19,9,0,4))); // Starting total is 20.
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(
            Families[13],Parameters(12,8,0,4))); // Original story 11.
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(
            Families[14],Parameters(19,11,0,4))); // Original 12 cranes given.
    }

    [Theory]
    [InlineData(107)]
    [InlineData(108)]
    [InlineData(109)]
    [InlineData(110)]
    [InlineData(111)]
    [InlineData(112)]
    [InlineData(113)]
    [InlineData(114)]
    [InlineData(115)]
    [InlineData(116)]
    [InlineData(117)]
    [InlineData(118)]
    [InlineData(119)]
    public void SourceFamilyGeneratorProducesValidVerifiableQuestions(
        int lesson)
    {
        var family=Families[lesson-103];
        var tasks=Enumerable.Range(0,1200)
            .Select(i=>UsCcssG1FullGradeUnit6MeasurementQuestions.Build(
                family,new Random(47*i+lesson))).ToArray();
        Assert.All(tasks,q=>
        {
            Assert.Equal(family,q.Family);
            Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(family,q.Parameters));
            var correct=UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(family,q.Parameters);
            Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Verify(
                family,q.Parameters,correct));
            Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Verify(
                family,q.Parameters,correct+";UNVERIFIED=PUBLISHER_IMAGE"));
        });
        if(lesson==108)
            foreach(int mode in Enumerable.Range(0,8))
                Assert.Contains(tasks,t=>t.Parameters["mode"]==mode);
        if(lesson==116)
            foreach(int mode in Enumerable.Range(4,6))
                Assert.Contains(tasks,t=>t.Parameters["mode"]==mode);
    }
}
