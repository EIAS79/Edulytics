using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;
/// <summary>Official numbers for original torn tape plus independent virtual practice;
/// real ruler and dialogue teacher-reviewed.</summary>
public static class UsCcssG2Unit3TenthLessonTornTape
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U03:L10";
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Verify original four true/false equations T,T,F,T, and correct third equation",
        "Observe real notebook drawing: Han8in, Jada from2 to10 equals8in",
        "Pupils physically measure a real 2–10in classroom object from different nonzero ruler marks",
        "Observe independently written start + ? = end and end − start = length equations",
        "For original Lin desk teacher checks actual long-side image, not invented source endpoints",
        "Source short desk side starts16 ends34, and34−16=18in",
        "MLR2 pair discussion, physical ruler alignment and accurate inch labels require teacher"
    ];
    private static bool Parse(string? s,int min,int max,out int n)=>
        int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out n)&&n>=min&&n<=max;
    public static Review SourceTrueFalse(string? a,string? b,string? c,string? d)
    {
        var ok=a=="True"&&b=="True"&&c=="False"&&d=="True";
        return new(ok,true,ok?"T,T,F,T is correct; 12−4=8 does not equal10−3=7."
            :"Check10−0=12−2;8−4=10−6;12−4≠10−3;15−2=13−0.");
    }
    public static Review SourceRuler(string? jadaStart,string? jadaEnd,string? notebook,
        string? deskStart,string? deskEnd,string? shortSide)
    {
        var ok=Parse(jadaStart,0,30,out var a)&&a==2&&
            Parse(jadaEnd,0,30,out var b)&&b==10&&
            Parse(notebook,0,30,out var c)&&c==8&&
            Parse(deskStart,0,50,out var d)&&d==16&&
            Parse(deskEnd,0,50,out var e)&&e==34&&
            Parse(shortSide,0,50,out var f)&&f==18;
        return new(ok,true,ok?"Jada10−2=8in; Lin's SHORT side34−16=18in. Real physical endpoints, long-side image and discussion remain teacher reviewed."
            :"Length=end-start, not end tick. Jada2→10 gives8in and Lin16→34 gives18in short side.");
    }
    public static Review Independent(string? a,string? b,string? c,string? exitA,string? exitB)
    {
        var ok=Parse(a,0,100,out var x)&&x==8&&Parse(b,0,100,out var y)&&y==9&&
            Parse(c,0,100,out var z)&&z==12&&Parse(exitA,0,100,out var u)&&u==9&&
            Parse(exitB,0,100,out var v)&&v==13;
        return new(ok,false,ok?"Virtual first-party start/end differences3→11=8,4→13=9,6→18=12,7→16=9,13→26=13."
            :"Count equal intervals, or subtract starting tick from ending tick; these digital pictures are NOT original source ruler images.");
    }
}
