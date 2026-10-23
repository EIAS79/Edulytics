using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;
/// <summary>Independent fictional center numerical checks; real ruler measurements require teacher.</summary>
public static class UsCcssG2Unit3SeventeenthLessonLinePlotCenter
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U03:L17";
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Original mental subtraction Number Talk50,49,39,38",
        "Real partner measures ONE through EIGHT physical objects with real inch OR cm ruler",
        "Zero tick aligned to start of object; end rounded to nearest whole chosen unit, not mixed",
        "Student-created titled plot includes equally spaced whole-number tick labels and one equal-sized X per object",
        "Partner genuinely poses TWO questions answerable from that original plot and listens to answers",
        "Actual Choice Time center: Estimate and Measure Stage2, Number Puzzles Stages2–4 or Target Measurements Stage1",
        "Authentic student center choice, teacher supervision and peer feedback remain human reviewed",
        "Edulytics fictional objects NEVER substituted for real source measurements"
    ];
    private static bool N(string? s,int lo,int hi,out int v)=>
        int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out v)&&v>=lo&&v<=hi;
    public static Review Warmup(string? a,string? b,string? c,string? d)
    {
        bool ok=N(a,0,100,out var x)&&x==50&&N(b,0,100,out var y)&&y==49&&
            N(c,0,100,out var z)&&z==39&&N(d,0,100,out var w)&&w==38;
        return new(ok,true,ok?"Original mental numbers50,49,39,38 correct; teacher reviews decomposition."
            :"60−10=50,60−11=49,60−21=39,70−32=38.");
    }
    public static Review Fictional(string? total,string? at10,string? at12,string? min,string? max,string? over11)
    {
        var ok=N(total,0,8,out var a)&&a==8&&N(at10,0,8,out var b)&&b==2&&
            N(at12,0,8,out var c)&&c==2&&N(min,0,100,out var d)&&d==9&&
            N(max,0,100,out var e)&&e==14&&N(over11,0,8,out var f)&&f==4;
        return new(ok,false,ok?"Fictional8 object lengths9,10,10,11,12,12,13,14cm;2 at10 and12,4 above11."
            :"Read eight Edulytics fictional lengths9,10,10,11,12,12,13,14cm; do NOT call these real items.");
    }
    public static Review Independent(string? first,string? second)
    {
        var ok=N(first,0,100,out var a)&&a==57&&N(second,0,100,out var b)&&b==56;
        return new(ok,false,ok?"Independent first-party80−23=57,91−35=56."
            :"Revisit subtracting tens and then the remaining ones.");
    }
}
