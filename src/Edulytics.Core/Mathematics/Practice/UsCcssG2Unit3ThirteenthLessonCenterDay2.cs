using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;
/// <summary>Unit3 L13 original public numerical arithmetic and independent
/// measurement examples; no claim of actual 8-round Target Measurements.</summary>
public static class UsCcssG2Unit3ThirteenthLessonCenterDay2
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U03:L13";
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Observe original mental 58+10,58+12,58+13,67+14 and explanations of a new ten",
        "Observe actual pair Target Measurements with straightedge, physical ruler and zero marks",
        "Each target is at most 10 inches OR 30 centimeters, and actual/target units match",
        "Partner who says STOP receives absolute measurement-difference score, not drawer",
        "Observe eight genuine rounds, role changes and lowest aggregate score",
        "Observe authentic Number Puzzles Stage4 with printed digit cards and tens composition/decomposition",
        "Digital arithmetic cannot certify real measurement, card play or partner conversation"
    ];
    private static bool Number(string? raw,int min,int max,out int v)=>
        int.TryParse(raw,NumberStyles.None,CultureInfo.InvariantCulture,out v)&&v>=min&&v<=max;
    public static Review Warmup(string? a,string? b,string? c,string? d)
    {
        var ok=Number(a,0,100,out var x)&&x==68&&Number(b,0,100,out var y)&&y==70&&
            Number(c,0,100,out var z)&&z==71&&Number(d,0,100,out var w)&&w==81;
        return new(ok,true,ok?"Source mental answers68,70,71,81 valid; teacher listens for make-ten reasoning."
            :"58+10=68,58+12=70,58+13=71,67+14=81.");
    }
    public static Review OriginalArithmetic(string? addMissing,string? subMissing)
    {
        var ok=Number(addMissing,0,100,out var x)&&x==25&&
            Number(subMissing,0,100,out var y)&&y==26;
        return new(ok,false,ok?"First-party Number Puzzles arithmetic48+25=73 and64−26=38."
            :"First-party missing numbers:48+?=73→25 and64−?=38→26, not authentic printed cards.");
    }
    public static Review ExampleMeasurement(string? unit,string? target,string? measured,string? score)
    {
        int max=unit=="cm"?30:unit=="in"?10:-1;
        var ok=max>0&&Number(target,1,max,out var t)&&Number(measured,0,40,out var m)
            &&Number(score,0,40,out var s)&&s==Math.Abs(m-t);
        return new(ok,true,ok?$"Arithmetic difference is {score} {unit}; teacher must check physical length and STOP scorer."
            :"Select inches target≤10 or centimeters target≤30; use same units and absolute difference.");
    }
    public static Review IndependentExit(string? cmScore,string? inScore,string? total)
    {
        var ok=Number(cmScore,0,40,out var a)&&a==3&&Number(inScore,0,40,out var b)&&b==2&&
            Number(total,0,100,out var c)&&c==81;
        return new(ok,false,ok?"First-party exit |21−18|=3 cm,|5−7|=2 in and67+14=81."
            :"Absolute length differences:18→21cm=3cm;7→5in=2in;67+14=81.");
    }
}
