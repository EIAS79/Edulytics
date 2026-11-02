using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;
/// <summary>Optional numeric companion, NOT actual protected diagram cards or partner story authoring.</summary>
public static class UsCcssG2Unit2FifteenthLessonTapeStoriesCenter
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U02:L15";
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Observe original Number Talk compensation35−19=35−20+1",
        "Student chooses a real publisher Tape Diagrams Stage5 card, with genuine printed numbers",
        "Student authors a mathematical STORY and a matching EQUATION fitting those bars",
        "Real partner solves the student-created story and explains relation of total/parts or compare gap",
        "Observe actual choice-center use of source Target Numbers, Capture Squares or Five in a Row rules",
        "Numerical first-party Edulytics tape examples are not publisher blackline cards",
        "Teacher must verify authentic partner interaction; numeric online response never proves source gameplay"
    ];
    private static bool N(string? s,int lo,int hi,out int v)=>
        int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out v)&&v>=lo&&v<=hi;
    public static Review Talk(string? a,string? b,string? c,string? d)
    {
        var ok=N(a,0,100,out var w)&&w==15&&N(b,0,100,out var x)&&x==25&&
            N(c,0,100,out var y)&&y==15&&N(d,0,100,out var z)&&z==16;
        return new(ok,true,ok?"Original results15,25,15,16, compensation adds one back. Teacher observes reasoning."
            :"25−10=15,35−10=25,35−20=15,35−19=16. Subtract20 then add1.");
    }
    public static Review FirstPartyDiagrams(string? total,string? part,string? gap)
    {
        var ok=N(total,0,100,out var t)&&t==49&&N(part,0,100,out var p)&&p==27&&
            N(gap,0,100,out var g)&&g==25;
        return new(ok,false,ok?"First-party illustrative tape answers:32+17=49,72−45=27,64−39=25."
            :"The independently authored bars represent49 total,27 missing part and25 compare gap.");
    }
    public static Review Exit(string? compensate,string? addition,string? subtraction)
    {
        var ok=N(compensate,0,100,out var a)&&a==26&&N(addition,0,100,out var b)&&b==75&&
            N(subtraction,0,100,out var c)&&c==44;
        return new(ok,false,ok?"Original Edulytics maths45−19=26,57+18=75,80−36=44."
            :"Use45−20+1,57+18 and80−36. No publisher Cool-down is claimed.");
    }
}
