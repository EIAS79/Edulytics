using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;
/// <summary>Numeric source stories only: real tape diagrams, student conversation and blocks teacher-reviewed.</summary>
public static class UsCcssG2Unit2EleventhLessonSeedsStories
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U02:L11";
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Observe students identify difference, change, missing part and smaller unknown without confusing story type",
        "Source Diego42 versus Jada16 asks HOW MANY MORE, not combined total",
        "Source Lin31 gives15; Noah37 total28 yellow; Elena50 and Andre23 fewer",
        "Observe actual connecting cubes, base-ten blocks, equations and teacher-presented tape diagrams",
        "Compare learner-created tape diagrams showing relationships vs base-ten models showing computations",
        "Observe MLR2 partner language and 5 Practices discussions in real classroom",
        "Digital answers neither copy protected Tyler's Seeds assessment nor certify authentic physical work"
    ];
    private static bool Read(string? s,int low,int high,out int n)=>
        int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out n)&&n>=low&&n<=high;
    public static Review SourceStories(string? gap,string? left,string? blue,string? smaller)
    {
        var ok=Read(gap,0,50,out var g)&&g==26&&Read(left,0,50,out var l)&&l==16&&
            Read(blue,0,50,out var b)&&b==9&&Read(smaller,0,50,out var s)&&s==27;
        return new(ok,true,ok?"Source answer order26,16,9,27 matches four different story types; diagrams need teacher."
            :"Diego42−16=26 more; Lin31−15=16 left; Noah37−28=9 blue; Andre50−23=27.");
    }
    public static Review FirstParty(string? compare,string? change,string? part)
    {
        var ok=Read(compare,0,50,out var a)&&a==26&&Read(change,0,50,out var b)&&b==16&&
            Read(part,0,50,out var c)&&c==18;
        return new(ok,false,ok?"Independent checks44−18=26 more,33−17=16 left,45−27=18 missing."
            :"Review the different question structures. These are first-party values, not source seeds.");
    }
    public static Review Exit(string? first,string? second)
    {
        var ok=Read(first,0,50,out var x)&&x==27&&Read(second,0,50,out var y)&&y==24;
        return new(ok,false,ok?"Independent exit46−19=27,40−16=24."
            :"Exchange a ten when subtracting46−19 and40−16.");
    }
}
