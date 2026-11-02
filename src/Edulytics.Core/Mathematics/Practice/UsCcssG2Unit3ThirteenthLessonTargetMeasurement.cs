using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;
/// <summary>Numerical examples only: physical measurement and eight real rounds remain teacher-only.</summary>
public static class UsCcssG2Unit3ThirteenthLessonTargetMeasurement
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U03:L13";
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Observe source Number Talk making a ten: 58+10,58+12,58+13,67+14",
        "Real Stage1 measurement target is whole inches <=10 or whole centimeters <=30; actual line drawn with straightedge",
        "Observe real zero-aligned ruler for measurement and matching target/actual units",
        "STOP partner, not drawing partner, scores absolute error",
        "Observe eight genuine rounds, alternated roles and lowest accumulated error winning",
        "Observe original digit cards and Number Puzzles Stage4 addition composition and subtraction decomposition",
        "Teacher reviews center choice, actual partner conversation and original print gameboards",
        "Typed digital sums cannot certify original ruler measurements, eight rounds or authentic winner"
    ];
    private static bool N(string? s,int min,int max,out int x)=>
        int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out x)&&x>=min&&x<=max;
    public static Review Warmup(string? a,string? b,string? c,string? d)
    {
        var ok=N(a,0,100,out var x)&&x==68&&N(b,0,100,out var y)&&y==70&&
            N(c,0,100,out var z)&&z==71&&N(d,0,100,out var v)&&v==81;
        return new(ok,true,ok?"Source Number Talk answers68,70,71,81 correct. Teacher checks 67+14 made a ten."
            :"58+10=68;58+12=70;58+13=71;67+14=81.");
    }
    public static Review CheckTarget(string? unit,string? target,string? measured,string? difference)
    {
        var limit=unit=="in"?10:unit=="cm"?30:0;
        var ok=limit>0&&N(target,1,limit,out var t)&&N(measured,0,limit+10,out var m)&&
            N(difference,0,limit+10,out var err)&&err==Math.Abs(t-m);
        return new(ok,true,ok?"First-party numerical error is correct in the chosen unit. Teacher must check the physical line, real ruler and STOP partner's score."
            :"Use SAME unit, a whole-inch target1–10 or whole-centimeter target1–30, and nonnegative absolute difference.");
    }
    public static Review Puzzle(string? sum,string? missingAddend,string? difference,string? missingSubtrahend)
    {
        var ok=N(sum,0,100,out var s)&&s==65&&N(missingAddend,0,100,out var a)&&a==27&&
            N(difference,0,100,out var d)&&d==38&&N(missingSubtrahend,0,100,out var b)&&b==27;
        return new(ok,true,ok?"Independent 38+27=65 and65−27=38 demonstrate composing and decomposing. Actual printed Number Puzzles board remains teacher-reviewed."
            :"Independent puzzle equations:38+27=65 and65−27=38; do not claim these occur on source gameboard.");
    }
    public static Review Exit(string? first,string? second,string? third)
    {
        var ok=N(first,0,100,out var a)&&a==81&&N(second,0,30,out var b)&&b==2&&
            N(third,0,100,out var c)&&c==36;
        return new(ok,false,ok?"First-party exit67+14=81, |8cm−6cm|=2cm,63−27=36."
            :"Review67+14, absolute error for target8cm drawn6cm, and63−27.");
    }
}
