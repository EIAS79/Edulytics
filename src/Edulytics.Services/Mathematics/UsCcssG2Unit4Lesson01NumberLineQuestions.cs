using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>Independent exact number-line exercises, not physical paper or publisher assessment.</summary>
internal static class UsCcssG2Unit4Lesson01NumberLineQuestions
{
    public const string Missing="usccss.g2.u04.l01.missing_tick_label";
    public const string Zero="usccss.g2.u04.l01.point_from_zero";
    public const string Greater="usccss.g2.u04.l01.greater_point";
    public const string Gap="usccss.g2.u04.l01.unit_interval_gap";
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family is Missing or Zero or Greater or Gap;
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unknown G2 U04 L01 family");
        int end=family is Missing or Greater?10:20;
        int first=random.Next(0,end),second=random.Next(first+1,end+1);
        int target=random.Next(1,end);
        var parameters=new Dictionary<string,int>(StringComparer.Ordinal){
            ["start"]=0,["end"]=end,["step"]=1,
            ["first"]=first,["second"]=second,["target"]=target
        };
        var prompt=family switch{
            Missing=>$"Independently authored 0..10 line with equal one-unit intervals: a tick is after {target-1} and before {target+1}, with no label. Which WHOLE NUMBER labels that tick?",
            Zero=>$"Independently authored 0..20 number line, zero anchored and equally spaced: put a point {target} UNIT INTERVALS right of zero. What whole-number label is under the point?",
            Greater=>$"Independently authored 0..10 number line has dots at {first} and {second}. What WHOLE NUMBER labels the dot farther RIGHT?",
            _=>$"Independently authored 0..20 line has two dots at {first} and {second}. How many one-unit INTERVALS apart are they?"
        };
        int answer=family switch{Missing=>target,Zero=>target,Greater=>second,_=>second-first};
        return new(family,prompt,$"Verified first-party number-line calculation: {answer}; equal physical spacing on student-made paper is not digitally certified.",
            AssessmentItemType.Numeric,parameters);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid number-line coordinates");
        int answer=family switch{
            Missing=>p["target"],
            Zero=>p["target"],
            Greater=>p["second"],
            Gap=>p["second"]-p["first"],
            _=>throw new InvalidOperationException("Unsupported family")
        };
        return answer.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||p.Count!=6||
           !p.TryGetValue("start",out var zero)||zero!=0||
           !p.TryGetValue("end",out var end)||end!=(family is Missing or Greater?10:20)||
           !p.TryGetValue("step",out var step)||step!=1||
           !p.TryGetValue("first",out var first)||first<0||first>=end||
           !p.TryGetValue("second",out var second)||second<=first||second>end||
           !p.TryGetValue("target",out var target)||target<1||target>=end)
            return false;
        return true;
    }
}
