using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;
/// <summary>Source Group B plant height calculations and independently authored fictional sample;
/// image-only Han plot and real learner work need human teacher review.</summary>
public static class UsCcssG2Unit3SixteenthLessonPlantPlot
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U03:L16";
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Number Talk21,31,23,43; explain mental composition of a ten",
        "Original Group B plant lengths exactly25,25,26,26,27,30,33,33,33,35 cm",
        "Plot starts at25cm not necessarily zero; labels every whole centimeter up to35 including zero-X lengths",
        "Plot has one equal-size vertically aligned X for each of TEN genuine source table entries",
        "Distinguish tallest35cm, mode33cm, shortest25cm, spread10cm and four above30cm",
        "View the ACTUAL additional Han line-plot image before deciding whether tallest29cm, total or valid assertion",
        "Authentic partner plots, MLR8 comparison and student's source image interpretations require educator review",
        "Fictional Edulytics nine-plant data NEVER presented as source Group B or real pupils"
    ];
    private static bool N(string? text,int lo,int hi,out int n)=>
        int.TryParse(text,NumberStyles.None,CultureInfo.InvariantCulture,out n)&&n>=lo&&n<=hi;
    public static Review Warmup(string? a,string? b,string? c,string? d)
    {
        var ok=N(a,0,100,out var x)&&x==21&&N(b,0,100,out var y)&&y==31&&
            N(c,0,100,out var z)&&z==23&&N(d,0,100,out var w)&&w==43;
        return new(ok,true,ok?"Source mental sums21,31,23,43 correct. Teacher checks reasoning."
            :"15+5+1=21,25+6=31,16+7=23,37+6=43.");
    }
    public static Review Source(string? min,string? max,string? difference,string? above30,string? total,string? mode)
    {
        var ok=N(min,0,100,out var a)&&a==25&&N(max,0,100,out var b)&&b==35&&
            N(difference,0,100,out var c)&&c==10&&N(above30,0,100,out var d)&&d==4&&
            N(total,0,100,out var e)&&e==10&&N(mode,0,100,out var f)&&f==33;
        return new(ok,false,ok?"Published Group B:25cm min,35cm max,10cm gap,4 above30cm,10 plants, modal height33cm."
            :"From the ten original Group B heights25,25,26,26,27,30,33,33,33,35 find shortest25, tallest35, spread10, four taller than30.");
    }
    public static Review TickCounts(string? count25,string? count33,string? zero28,string? zero34)
    {
        var ok=N(count25,0,20,out var a)&&a==2&&N(count33,0,20,out var b)&&b==3&&
            N(zero28,0,20,out var c)&&c==0&&N(zero34,0,20,out var d)&&d==0;
        return new(ok,false,ok?"Line plot25 has2 X,33 has3 X; blank28 and34 still need consecutive tick labels."
            :"Source heights25:2,26:2,27:1,28:0,29:0,30:1,31:0,32:0,33:3,34:0,35:1.");
    }
    public static Review Fictional(string? min,string? max,string? mode,string? above26)
    {
        var ok=N(min,0,100,out var a)&&a==24&&N(max,0,100,out var b)&&b==28&&
            N(mode,0,100,out var c)&&c==26&&N(above26,0,100,out var d)&&d==3;
        return new(ok,false,ok?"First-party sample range24–28, mode26, three above26; this is NOT Group B."
            :"Edulytics fictional heights24,25,25,26,26,26,27,28,28. Count original fictional figures only.");
    }
}
