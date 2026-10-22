using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>First-party source-like physical iteration counts; no abstract grade2 unit conversion test.</summary>
internal static class UsCcssG2Unit3Lesson9FishFeetQuestions
{
    public const string One="usccss.g2.u03.l09.one_foot_twelve_tiles";
    public const string Repeat="usccss.g2.u03.l09.repeat_full_ruler_spans";
    public const string Extra="usccss.g2.u03.l09.repeat_and_extra_inch_tiles";
    public const string Compare="usccss.g2.u03.l09.compare_inches_and_feet_counts";
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f is One or Repeat or Extra or Compare;
    public static Problem Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Not fish feet repeated-unit family");
        // One-foot challenge measures an authentically meaningful missing part
        // of a 12-tile foot ruler; repeated-unit tasks use more independent spans.
        int spans=family==One?1:rng.Next(1,11),extra=family==Extra?rng.Next(1,12):0;
        int covered=family==One?rng.Next(0,12):0;
        var p=new Dictionary<string,int>(StringComparer.Ordinal){{"spans",spans},{"extraTiles",extra},{"coveredTiles",covered}};
        int length=12*spans+extra;
        var prompt=family switch{
            One=>$"FIRST-PARTY ONE-FOOT tile strip: 12 equal one-inch spaces in all, {covered} already covered by inch tiles. How many MORE one-inch tiles complete exactly one foot?",
            Repeat=>$"FIRST-PARTY picture: a model tape has {spans} consecutive full12-inch ruler spans without gaps. How many one-inch INTERVALS are marked?",
            Extra=>$"FIRST-PARTY picture: a tape has {spans} complete12-inch ruler spans plus {extra} extra one-inch tiles. How many one-inch intervals in the whole tape?",
            _=>$"FIRST-PARTY counted-unit comparison: same tape measured with {spans} complete foot-long ruler spans. How many SMALL 1-inch marks cover this same tape? Larger units take fewer repetitions."
        };
        return new Problem(family,prompt,family==One
            ? $"One foot contains 12 equal one-inch spaces. {covered} covered + {12-covered} missing = 12; genuine publisher measuring stays teacher-observed."
            : $"Count {spans} consecutive complete ruler spans of12 one-inch tiles plus{extra}, getting {length} one-inch spaces.",AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid counted tape model");
        return (family==One?12-p["coveredTiles"]:12*p["spans"]+p["extraTiles"]).ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)=>
        Supports(family)&&p.Count==3&&p.TryGetValue("spans",out var spans)&&spans is >=1 and <=10&&
        p.TryGetValue("extraTiles",out var extra)&&
        (family==Extra?extra is >=1 and <=11:extra==0)&&
        p.TryGetValue("coveredTiles",out var covered)&&
        (family==One?(spans==1&&covered is >=0 and <=11):covered==0);
}
