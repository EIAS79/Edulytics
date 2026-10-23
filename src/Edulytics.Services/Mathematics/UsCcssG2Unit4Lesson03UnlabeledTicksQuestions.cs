using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>Fully independent sparse-labelled number line math; image-specific source dots remain teacher-only.</summary>
internal static class UsCcssG2Unit4Lesson03UnlabeledTicksQuestions
{
    public const string Before="usccss.g2.u04.l03.before_five_ten_anchor";
    public const string After="usccss.g2.u04.l03.after_five_ten_anchor";
    public const string Ticks="usccss.g2.u04.l03.window_one_unit_ticks";
    public const string Gap="usccss.g2.u04.l03.unit_point_gap";
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family is Before or After or Ticks or Gap;
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unknown original unit3 sparse marks family");
        var windows=new (int Min,int Max,int Step)[]{(0,30,5),(0,40,10),(20,60,10),(60,90,5)};
        var chosen=windows[random.Next(windows.Length)];
        int min=chosen.Min,max=chosen.Max,labels=chosen.Step;
        int anchor=min+random.Next(1,(max-min)/labels)*labels;
        int offset=random.Next(1,labels);
        int first=random.Next(min,max),second=random.Next(first+1,max+1);
        var p=new Dictionary<string,int>(StringComparer.Ordinal){
            ["windowStart"]=min,["windowEnd"]=max,["labelStep"]=labels,
            ["anchor"]=anchor,["offset"]=offset,["first"]=first,["second"]=second
        };
        var prompt=family switch{
            Before=>$"FIRST-PARTY 1-unit number line window {min}..{max}; ONLY multiples of {labels} are printed. How many as a WHOLE NUMBER is the point {offset} one-unit steps LEFT of printed label {anchor}?",
            After=>$"FIRST-PARTY 1-unit number line window {min}..{max}; ONLY multiples of {labels} are printed. Which WHOLE NUMBER is {offset} one-unit steps RIGHT of printed label {anchor}?",
            Ticks=>$"FIRST-PARTY equally spaced 1-unit number line window from {min} through {max} inclusive. How many tick marks INCLUDING both ends, even if only every {labels}th has a printed label?",
            _=>$"FIRST-PARTY number-line window {min}..{max} with points at {first} and {second}. How many equal ONE-unit intervals lie between the points?"
        };
        int answer=family switch{Before=>anchor-offset,After=>anchor+offset,Ticks=>max-min+1,_=>second-first};
        return new(family,prompt,$"Edulytics original answer {answer}; this does not certify source publisher image positioning.",AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid one-unit line parameters");
        int answer=family switch{
            Before=>p["anchor"]-p["offset"],After=>p["anchor"]+p["offset"],
            Ticks=>p["windowEnd"]-p["windowStart"]+1,
            Gap=>p["second"]-p["first"],
            _=>throw new InvalidOperationException("Unknown question")};
        return answer.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||p.Count!=7||
           !p.TryGetValue("windowStart",out var min)||
           !p.TryGetValue("windowEnd",out var max)||
           !p.TryGetValue("labelStep",out var label)||
           !((min==0&&max==30&&label==5)||
             (min==0&&max==40&&label==10)||
             (min==20&&max==60&&label==10)||
             (min==60&&max==90&&label==5))||
           !p.TryGetValue("anchor",out var anchor)||anchor<min+label||anchor>max-label||anchor%label!=0||
           !p.TryGetValue("offset",out var offset)||offset<1||offset>=label||
           !p.TryGetValue("first",out var first)||first<min||first>=max||
           !p.TryGetValue("second",out var second)||second<=first||second>max)
            return false;
        return true;
    }
}
