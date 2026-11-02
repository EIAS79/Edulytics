using System.Globalization;
using System.Text.RegularExpressions;
namespace Edulytics.Core.Mathematics.Practice;
/// <summary>Check original source exact maths AND question-mark positions;
/// real source cards and student-drawn diagrams still teacher-reviewed.</summary>
public static class UsCcssG2Unit2ThirteenthLessonStoryEquations
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U02:L13";
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Observe real Which One Doesn't Belong? tape diagrams and the equation27+?=40",
        "Observe real Groups2–4 matching original story Cards A–I and matching source Equation Cards",
        "Verify two REAL source Card F/G/H/I problems solved using actual card numbers and equations",
        "Lin's start unknown ?+36=64=28 first, Andre's addend unknown32+?=57=25",
        "Diego22+48=?=70; Noah/Kiran53+?=92=39, appropriate story relationships and labels",
        "Student may use subtraction to find a missing addend while writing addition to model ADDING actions",
        "Authentic diagrams, source card sort and MLR8 conversations teacher-observed, not numeric auto-certification"
    ];
    private static bool N(string? s,int min,int max,out int x)=>
        int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out x)&&x>=min&&x<=max;
    private static string Normalize(string? s)=>
        Regex.Replace(s??"",@"\s+","").Replace('−','-');
    public static Review Warmup(string? missing)
    {
        var ok=N(missing,0,100,out var value)&&value==13; // 27 + ? = 40.
        return new(ok,true,ok
            ?"27+13=40. Teacher checks the source tape diagram and explanation."
            :"From 27+?=40, count 13 more. Original diagram interpretation is teacher-reviewed.");
    }
    // Historical test/client alias; the authoritative teacher/source boundary is SourceAnswers.
    public static Review SourceFour(string? lin,string? andre,string? diego,string? kiran)=>
        SourceAnswers(lin,andre,diego,kiran);
    public static Review SourceAnswers(string? lin,string? andre,string? diego,string? kiran)
    {
        var ok=N(lin,0,100,out var a)&&a==28&&N(andre,0,100,out var b)&&b==25&&
            N(diego,0,100,out var c)&&c==70&&N(kiran,0,100,out var d)&&d==39;
        return new(ok,true,ok?"Source unknowns28,25,70,39 correct. Teacher reviews story diagrams and peer math discourse."
            :"Lin64−36=28; Andre57−32=25; Diego22+48=70; Kiran92−53=39.");
    }
    public static Review SourceQuestionMarks(string? lin,string? andre,string? diego,string? kiran)
    {
        var ok=(Normalize(lin) is "?+36=64" or "36+?=64")&&
            (Normalize(andre) is "32+?=57" or "?+32=57")&&
            (Normalize(diego) is "22+48=?" or "48+22=?")&&
            (Normalize(kiran) is "53+?=92" or "?+53=92");
        return new(ok,true,ok?"Question marks reflect original source start/change/total/part unknowns. Student diagrams are teacher-scored."
            :"Model action/parts: ?+36=64;32+?=57;22+48=?;53+?=92. Related subtraction may compute the addend.");
    }
    public static Review Independent(string? start,string? added,string? total,string? part)
    {
        var ok=N(start,0,100,out var a)&&a==48&&N(added,0,100,out var b)&&b==37&&
            N(total,0,100,out var c)&&c==67&&N(part,0,100,out var d)&&d==37;
        return new(ok,false,ok?"Original Edulytics first-party ?+27=75:48;46+?=83:37;29+38=67;54+?=91:37."
            :"Find unknown values from the actual story relationship; first-party numbers not source publisher cards.");
    }
}
