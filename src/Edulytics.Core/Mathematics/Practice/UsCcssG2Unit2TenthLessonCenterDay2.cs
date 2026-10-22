using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;
/// <summary>Stage5 numeric demonstration only: teacher resolves the source end-rule discrepancy.</summary>
public static class UsCcssG2Unit2TenthLessonCenterDay2
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U02:L10";
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Observe real partner Notice and Wonder of66−28=38 and original base-ten diagram",
        "Stage5 students use real three number cubes and two different cubes for tens and ones",
        "Each real player starts at100, subtracts only legal two-digit amounts and never goes below zero",
        "Compare decomposing73−24=49 with no-exchange73−52=21 from source dice4,2,5",
        "Teacher resolves source nearest-zero-after-six versus first-under-ten game-end discrepancy",
        "Observe genuine Target Numbers/Capture Squares/Five in a Row choice center work",
        "Numeric companion does not certify dice were rolled, authentic winner or partner dialogue"
    ];
    private static bool N(string? s,int low,int high,out int v)=>
        int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out v)&&v>=low&&v<=high;
    public static Review Warmup(string? result,string? changedTens,string? changedOnes)
    {
        var ok=N(result,0,100,out var r)&&r==38&&N(changedTens,0,10,out var t)&&t==5&&
            N(changedOnes,0,19,out var o)&&o==16;
        return new(ok,true,ok?"Warm-up66=5 tens+16 ones;66−28=38. Teacher reviews authentic image."
            :"Trade one ten:66=50+16;16−8=8,50−20=30, total38.");
    }
    public static Review DiceMath(string? current,string? firstChoice,string? firstAnswer,
        string? secondChoice,string? secondAnswer)
    {
        var ok=N(current,73,73,out var start)&&
            N(firstChoice,10,99,out var first)&&first==24&&
            N(firstAnswer,0,100,out var a)&&a==49&&
            N(secondChoice,10,99,out var second)&&second==52&&
            N(secondAnswer,0,100,out var b)&&b==21;
        return new(ok,true,ok?"From real source sample dice4,2,5:73−24=49 with trade,73−52=21 without trade. These are ALTERNATIVE choices from the same start, not consecutive moves."
            :"With displayed cubes4,2,5 you can form24 or52; at start73 their differences are49 and21 respectively.");
    }
    public static Review Independent(string? a,string? b,string? c)
    {
        var ok=N(a,0,100,out var x)&&x==47&&N(b,0,100,out var y)&&y==38&&
            N(c,0,100,out var z)&&z==46;
        return new(ok,false,ok?"First-party maths82−35=47,94−56=38,70−24=46."
            :"Recheck first-party82−35=47,94−56=38 and70−24=46.");
    }
}
