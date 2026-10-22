using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;
/// <summary>Checks published source Group C pencil lengths and independent fictional data;
/// real group measurements and MLR8 compare-and-connect require teacher.</summary>
public static class UsCcssG2Unit3FifteenthLessonPencilPlot
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U03:L15";
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Four original source Number Talk differences and tens-first strategy",
        "In authentic groups of four measure 10–12 real pencils in centimeters from ruler zero",
        "TWO different measurers check each physical pencil; real numbers recorded once each",
        "Source Group C line plot displays10..16 consecutive labels including unmeasured11 and15",
        "One equal-size aligned X per pencil, title and pencil-length(cm) axis label",
        "Nonzero scale is allowed but x-axis labels remain true zero-based centimeter lengths",
        "MLR8 discussion compares authentic student plots and mode10cm versus longest16cm"
    ];
    private static bool N(string? raw,int low,int high,out int v)=>
        int.TryParse(raw,NumberStyles.None,CultureInfo.InvariantCulture,out v)&&v>=low&&v<=high;
    public static Review Warmup(string? a,string? b,string? c,string? d)
    {
        var ok=N(a,0,100,out var x)&&x==27&&N(b,0,100,out var y)&&y==23&&
            N(c,0,100,out var z)&&z==26&&N(d,0,100,out var w)&&w==21;
        return new(ok,true,ok?"Original mental subtraction27,23,26,21 correct; teacher reviews strategy."
            :"47−20=27,47−24=23,36−10=26,36−15=21.");
    }
    public static Review SourceDataset(string? total,string? mode,string? shortest,string? longest,string? longerThan12)
    {
        var ok=N(total,0,40,out var a)&&a==10&&N(mode,0,30,out var b)&&b==10&&
            N(shortest,0,30,out var c)&&c==10&&N(longest,0,30,out var d)&&d==16&&
            N(longerThan12,0,40,out var e)&&e==5;
        return new(ok,false,ok?"Published Group C 10 pencils; mode10cm, shortest10cm, longest16cm and five longer than12cm."
            :"Count the ten public Group C values. Longest and shortest are horizontal labels, mode10cm has three Xs.");
    }
    public static Review SourceTicks(string? missing11,string? missing15,string? tenCount,string? sixteenCount)
    {
        var ok=N(missing11,0,10,out var a)&&a==0&&N(missing15,0,10,out var b)&&b==0&&
            N(tenCount,0,10,out var c)&&c==3&&N(sixteenCount,0,10,out var d)&&d==1;
        return new(ok,false,ok?"Correct complete10..16 centimeter scale includes zero Xs at11 and15;10 has3 Xs and16 has1."
            :"Do not skip scale labels11 and15 even though no pencil has those lengths.");
    }
    public static Review Independent(string? mode,string? total,string? range)
    {
        var ok=N(mode,0,30,out var a)&&a==13&&N(total,0,40,out var b)&&b==10&&
            N(range,0,30,out var c)&&c==5;
        return new(ok,false,ok?"FIRST-PARTY fictional plot mode13cm, total10, range16−11=5cm."
            :"Edulytics sample 11:2,12:1,13:3,14:2,15:1,16:1; mode13, total10 and range5.");
    }
}
