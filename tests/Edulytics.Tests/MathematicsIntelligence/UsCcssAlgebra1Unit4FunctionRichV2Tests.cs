using Edulytics.Core.Curriculum;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Exact source-aligned Algebra 1 Unit 4 function lessons.
/// Does not certify accessible deployed learner rendering or Practice grading.
/// </summary>
public sealed class UsCcssAlgebra1Unit4FunctionRichV2Tests
{
    private const string Prefix = "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:";

    [Theory]
    [InlineData("L01","Describing and Graphing Situations")]
    [InlineData("L02","Function Notation")]
    [InlineData("L03","Interpreting & Using Function Notation")]
    [InlineData("L04","Using Function Notation to Describe Rules (Part 1)")]
    [InlineData("L05","Using Function Notation to Describe Rules (Part 2)")]
    [InlineData("L06","Features of Graphs")]
    [InlineData("L07","Using Graphs to Find Average Rate of Change")]
    [InlineData("L08","Interpreting and Creating Graphs")]
    [InlineData("L09","Comparing Graphs")]
    [InlineData("L10","Domain and Range (Part 1)")]
    [InlineData("L11","Domain and Range (Part 2)")]
    [InlineData("L12","Piecewise Functions")]
    [InlineData("L13","Absolute Value Functions (Part 1)")]
    [InlineData("L14","Absolute Value Functions (Part 2)")]
    [InlineData("L15","Inverse Functions")]
    [InlineData("L16","Finding and Interpreting Inverse Functions")]
    [InlineData("L17","Writing Inverse Functions to Solve Problems")]
    [InlineData("L18","Using Functions to Model Battery Power")]
    public void SourceLinkedFunctionLessonContainsAccessibleOriginalReasoning(
        string code,string title)
    {
        var lesson=RichLessonContentV2Registry.Find(Prefix+code,"en");
        Assert.NotNull(lesson);
        Assert.Equal(title,lesson.Title);
        var n=int.Parse(code[1..]);
        Assert.Contains(lesson.ExplanationParagraphs, p =>
            p.Contains("/1/4/"+n+"/preparation.html",StringComparison.Ordinal));
        Assert.True(lesson.ExplanationParagraphs.Count>=3);
        Assert.True(lesson.KeyConcepts.Count>=3);
        Assert.True(lesson.WorkedExamples.Count>=3);
        Assert.All(lesson.WorkedExamples, example =>
        {
            Assert.True(example.Steps.Count>=3);
            Assert.False(string.IsNullOrWhiteSpace(example.Answer));
            Assert.False(string.IsNullOrWhiteSpace(example.Check));
        });
        Assert.True(lesson.CommonMistakes.Count>=2);
        Assert.True(lesson.SummaryPoints.Count>=3);
        Assert.NotEmpty(lesson.Visuals);
        Assert.All(lesson.Visuals, v=>Assert.False(string.IsNullOrWhiteSpace(v.Description)));
        Assert.Empty(lesson.Videos);
    }

    [Fact]
    public void SameInputGraphComparisonsAndIntersectionAreVerifiable()
    {
        static int A(int t)=>10+4*t;
        static int B(int t)=>2+6*t;
        Assert.Equal(26,A(4));
        Assert.Equal(A(4),B(4));
        Assert.True(A(2)>B(2));
        Assert.True(B(6)>A(6));
    }

    [Fact]
    public void DiscreteDomainAndStepFunctionRangesAreExact()
    {
        int[] domain=Enumerable.Range(5,8).ToArray();
        Assert.Equal([45,52,59,66,73,80,87,94],
            domain.Select(n=>7*n+10).ToArray());
        static double Fee(double t)=>t<2?4:t<4?7:10;
        Assert.Equal(4,Fee(1.5));
        Assert.Equal(7,Fee(2));
        Assert.Equal(10,Fee(4));
        Assert.Equal(10,Fee(6));
        static double Parcel(double w)=>w<=5?0.5*w:3;
        Assert.Equal(2.5,Parcel(5));
        Assert.Equal(3,Parcel(5.01));
    }

    [Fact]
    public void AbsoluteValuesAndInverseFunctionsRoundTripWithinDomains()
    {
        Assert.Equal(5,Math.Abs(2-5)+2);
        Assert.Equal(2,Math.Abs(5-5)+2);
        Assert.Equal(5,Math.Abs(8-5)+2);
        int[] guesses=[36,40,42,45,50];
        Assert.Equal([6,2,0,3,8],guesses.Select(x=>Math.Abs(x-42)).ToArray());
        static double BookBill(double n)=>9*n+5;
        static double BookCount(double bill)=>(bill-5)/9;
        Assert.Equal(5,BookCount(BookBill(5)));
        static double F(double c)=>1.8*c+32;
        static double C(double f)=>(f-32)/1.8;
        Assert.Equal(40,C(104),8);
        Assert.Equal(104,F(C(104)),8);
        static double Water(double t)=>80-2.5*t;
        static double Time(double v)=>(80-v)/2.5;
        Assert.Equal(47.5,Water(13));
        Assert.Equal(30,Time(5));
        Assert.Equal(13,Time(Water(13)));
    }

    [Fact]
    public void BatteryPredictionDeclaresExtrapolationBeyondObservedData()
    {
        static double Battery(double t)=>5+0.9*t;
        Assert.Equal(14,Battery(10),8);
        Assert.Equal(32,Battery(30),8);
        Assert.Equal(41,Battery(40),8);
        Assert.Equal(95/0.9,(100-5)/0.9,8);
        Assert.True((100-5)/0.9>40);
        var lesson=RichLessonContentV2Registry.Find(Prefix+"L18","en");
        Assert.NotNull(lesson);
        Assert.Contains("extrapolat",string.Join(" ",lesson.ExplanationParagraphs),StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void DroneGraphAnchorsExtremaAndIntervalsAreExact()
    {
        static double Height(double t) =>
            t <= 2 ? 6*t : t <= 5 ? 12 : 12-4*(t-5);
        Assert.Equal(0,Height(0));
        Assert.Equal(12,Height(2));
        Assert.Equal(12,Height(5));
        Assert.Equal(0,Height(8));
        Assert.Equal(12,Height(3.5));
        Assert.Equal(6,(Height(2)-Height(0))/2);
        Assert.Equal(-4,(Height(8)-Height(5))/3);
    }

    [Fact]
    public void TemperatureAverageRatesDivideByElapsedTime()
    {
        double first=(4-12d)/(2-0d);
        double second=(-2-4d)/(6-2d);
        Assert.Equal(-4,first);
        Assert.Equal(-1.5,second);
        Assert.True(Math.Abs(first)>Math.Abs(second));
        Assert.Equal(-10,(50-90d)/(5-1d));
    }

    [Fact]
    public void ModeledFlagPiecewiseRulesAreContinuousAndBounded()
    {
        static double Height(double t) =>
            t<=3 ? 2*t : t<=8 ? 6 : 22-2*t;
        Assert.Equal(0,Height(0));
        Assert.Equal(6,Height(3));
        Assert.Equal(6,Height(8));
        Assert.Equal(0,Height(11));
        foreach(var t in Enumerable.Range(0,111).Select(i=>i/10d))
            Assert.InRange(Height(t),0,6);
        Assert.Equal(2,(Height(3)-Height(0))/3);
        Assert.Equal(-2,(Height(11)-Height(8))/3);
    }
}
