using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;
/// <summary>Exact original Group B numeric plot, NOT the image-only Han line plot.</summary>
public static class UsCcssG2Unit3SixteenthLessonPlantData
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U03:L16";
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Observe mental Number Talk additions and making-ten justification",
        "Each source Group B line plot shows10 plant heights in centimeters and all consecutive25..35 ticks",
        "Source plot includes five empty ticks 28,29,31,32,34 rather than skipping labels",
        "Check source shortest25cm, tallest35cm, difference10cm, mode33cm and4 heights>30cm",
        "Display authentic separate Han line plot image before any claim about 29cm maximum or total",
        "Observe MLR8 partner discussion and personally drafted line plots, rather than inventing image data",
        "Keep plant heights centimeters separate from section-summary teachers' hand span inches"
    ];
    private static bool N(string? s,int lo,int hi,out int v)=>
        int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out v)&&v>=lo&&v<=hi;
    public static Review Warmup(string? a,string? b,string? c,string? d)
    {
        var ok=N(a,0,100,out var x)&&x==21&&N(b,0,100,out var y)&&y==31&&
            N(c,0,100,out var z)&&z==23&&N(d,0,100,out var w)&&w==43;
        return new(ok,true,ok?"Original warm-up sums21,31,23,43; teacher reviews make-ten methods."
            :"15+5+1=21;25+6=31;16+7=23;37+6=43.");
    }
    public static Review GroupB(string? shortest,string? tallest,string? difference,string? mode,string? over30,string? total)
    {
        var ok=N(shortest,0,100,out var a)&&a==25&&N(tallest,0,100,out var b)&&b==35&&
            N(difference,0,100,out var c)&&c==10&&N(mode,0,100,out var d)&&d==33&&
            N(over30,0,100,out var e)&&e==4&&N(total,0,100,out var f)&&f==10;
        return new(ok,false,ok?"Published Group B: shortest25cm, tallest35cm, difference10cm, mode33cm, 4 taller than30cm, 10 plants."
            :"Use source Group B table to mark each value and count Xs. This is NOT Han's separate plotted image.");
    }
    public static Review Fictional(string? total,string? mode,string? range,string? over24)
    {
        var ok=N(total,0,100,out var a)&&a==8&&N(mode,0,100,out var b)&&b==24&&
            N(range,0,100,out var c)&&c==6&&N(over24,0,100,out var d)&&d==2;
        return new(ok,false,ok?"First-party sample21,21,22,24,24,24,26,27cm:8 values, mode24cm, range6cm, two over24cm."
            :"This is an independently authored sample: mode24, max27, min21, eight entries, two >24.");
    }
}
