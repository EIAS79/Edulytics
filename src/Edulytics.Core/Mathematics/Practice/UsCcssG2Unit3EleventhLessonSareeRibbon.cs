using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;
public static class UsCcssG2Unit3EleventhLessonSareeRibbon
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U03:L11";
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Source Notice and Wonder ribbon picture and54−16=? interpreted as remaining38 inches",
        "Priya44in cut18 leaves26; Andre's original dotted tape diagram whole and parts",
        "Elena58−27=31 and Han64−28=36 modeled with real base-ten blocks",
        "Priya cut25 and still has38: unknown INITIAL whole63, so ADD not subtract",
        "Partner Three Reads identifies story first, known values second, question-mark tape diagram third",
        "Authentic Indian saree silk source context treated respectfully; publisher art not falsely copied",
        "MLR8 speaking, real pair modeling and teacher-only More Saree Ribbon Cool-down not attested digitally"
    ];
    private static bool N(string? s,int low,int high,out int value)=>
        int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out value)&&value>=low&&value<=high;
    public static Review Warmup(string? answer)
    {
        var ok=N(answer,0,100,out var v)&&v==38;
        return new(ok,true,ok?"Source54−16=38 interpreted as length remaining; teacher reviews original image.":
            "Look for the remaining part when16 inches is removed from54.");
    }
    public static Review SourceFour(string? priya,string? elena,string? han,string? startUnknown)
    {
        var ok=N(priya,0,100,out var a)&&a==26&&N(elena,0,100,out var b)&&b==31&&
            N(han,0,100,out var c)&&c==36&&N(startUnknown,0,100,out var d)&&d==63;
        return new(ok,true,ok?"26in left;31in kept;36in left; Priya ORIGINAL length63in from25+38. Teacher reviews genuine tape diagrams.":
            "Take-from result unknown:44−18=26,58−27=31,64−28=36; START unknown:25+38=63.");
    }
    public static Review FirstParty(string? takeFrom,string? start,string? last)
    {
        var ok=N(takeFrom,0,100,out var a)&&a==36&&N(start,0,100,out var b)&&b==76&&
            N(last,0,100,out var c)&&c==55;
        return new(ok,false,ok?"Independent Edulytics72−36=36;29+47=76 initial;80−25=55.":
            "Compute original examples, not protected publisher assessment:72−36,29+47,80−25.");
    }
}
