using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;
/// <summary>Numerical demonstrations only, never actual classroom desk or arm measurements.</summary>
public static class UsCcssG2Unit3EighteenthLessonYardstick
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U03:L18";
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Warmup answers8,2,7,14 with student3/5 addition and subtraction reasoning",
        "Authentic two-person paper strip at least36in and REAL 3×5 inch index card",
        "Student marks EVERY 1..36 inch position correctly with equally spaced intervals and zero anchor",
        "Partners switch paper tool roles halfway, compare rulers and physically revise errors",
        "Real two students measure SAME desk height and contribute genuine X sticky note to class line plot",
        "Class plot records genuine desk values, max/min gap and plausible differences from uneven marks",
        "Optional source hand from wrist to middle finger and forearm outside elbow to wrist for BOTH students",
        "Teacher examines TWO real partner-measurement equations and contextual statements",
        "Digital math checks never certify real precision instrument, personal measurements or class participation"
    ];
    private static bool N(string? s,int lo,int hi,out int n)=>
        int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out n)&&n>=lo&&n<=hi;
    public static Review Warmup(string? a,string? b,string? c,string? d)
    {
        bool ok=N(a,0,50,out var x)&&x==8&&N(b,0,50,out var y)&&y==2&&
            N(c,0,50,out var z)&&z==7&&N(d,0,50,out var w)&&w==14;
        return new(ok,true,ok?"Original Number Talk8,2,7,14 correct; teacher checks mental strategies."
            :"3+5=8,5−3=2,5−3+5=7,3+5+3+3=14.");
    }
    public static Review CardArithmetic(string? one,string? two,string? eleven,string? thirtySix)
    {
        bool ok=N(one,0,36,out var a)&&a==1&&N(two,0,36,out var b)&&b==2&&
            N(eleven,0,36,out var c)&&c==11&&N(thirtySix,0,36,out var d)&&d==36;
        return new(ok,true,ok?"Source card lengths3,5 allow 3+3−5=1,5−3=2,5+3+3=11 and six5s+two3s=36; physical accuracy teacher-only."
            :"One inch3+3−5, two inches5−3, eleven5+3+3, thirty-six6×5+2×3.");
    }
    public static Review Fictional(string? deskA,string? deskB,string? gap,string? remaining,string? handGap)
    {
        bool ok=N(deskA,0,36,out var a)&&a==28&&N(deskB,0,36,out var b)&&b==30&&
            N(gap,0,36,out var c)&&c==2&&N(remaining,0,36,out var d)&&d==19&&
            N(handGap,0,36,out var e)&&e==9;
        return new(ok,false,ok?"Fictional desk heights28 and30 differ2in;36−17=19in; hypothetical forearm16−hand7=9in; NONE are real source measurements."
            :"Separate fictional Edulytics numbers28,30,36−17,16−7; never label them as class measurements.");
    }
}
