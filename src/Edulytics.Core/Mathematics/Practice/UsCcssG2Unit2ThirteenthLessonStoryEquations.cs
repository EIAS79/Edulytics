using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;
/// <summary>Numeric source checks distinct from genuine printed card matching and student talk.</summary>
public static class UsCcssG2Unit2ThirteenthLessonStoryEquations
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U02:L13";
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Actual warm-up A-D source diagrams and unknown27+?=40 reviewed, not recreated",
        "Teacher witnesses original A–I story cards matched to original equation cards with explanations",
        "A story may match MORE THAN ONE equivalent equation if unknown quantities align",
        "Observe Lin ?+36=64, Andre32+?=57, Diego22+48=?, Noah/Kiran53+?=92",
        "Observe difference between equation representing ADDITION in story and SUBTRACTION used to solve it",
        "Check learner-authored tape diagram labels, physical base-ten models and MLR8 peer conversation",
        "Numeric companion and independent Edulytics exit do not certify printed cards, collaboration or restricted Cool-down"
    ];
    private static bool N(string? s,int lo,int hi,out int v)=>
        int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out v)&&v>=lo&&v<=hi;
    public static Review Warmup(string? missing)
    {
        var ok=N(missing,0,100,out var x)&&x==13;
        return new(ok,true,ok?"27+13=40 correct. Teacher compares the original source diagrams."
            :"27+?=40 gives13.");
    }
    public static Review SourceFour(string? lin,string? andre,string? diego,string? kiran)
    {
        var ok=N(lin,0,100,out var a)&&a==28&&N(andre,0,100,out var b)&&b==25&&
            N(diego,0,100,out var c)&&c==70&&N(kiran,0,100,out var d)&&d==39;
        return new(ok,true,ok?"Original Lin28, Andre25, Diego70, Kiran39 correct. Teacher checks equation form and diagrams."
            :"Lin?+36=64 gives28. Andre32+?=57 gives25. Diego22+48=70. Kiran53+?=92 gives39.");
    }
    public static Review Independent(string? lin,string? andre,string? sum,string? part)
    {
        var ok=N(lin,0,100,out var a)&&a==36&&N(andre,0,100,out var b)&&b==34&&
            N(sum,0,100,out var c)&&c==65&&N(part,0,100,out var d)&&d==35;
        return new(ok,false,ok?"Edulytics examples ?+27=63->36,41+?=75->34,18+47=65,56+?=91->35."
            :"Check first-party story relationships: use inverse subtraction to find unknown addends.");
    }
}
