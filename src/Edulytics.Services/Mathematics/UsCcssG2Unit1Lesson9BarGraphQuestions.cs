using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// First-party unit-one vertical scale, four-bar categorical graphs for
/// original IM 2021 G2 U01 L09 numerical subskills. Source student-generated
/// true statements and class discussion remain teacher-scored.
/// </summary>
internal static class UsCcssG2Unit1Lesson9BarGraphQuestions
{
    public const string Height="usccss.g2.u01.l09.bar_category_height";
    public const string Combine="usccss.g2.u01.l09.bar_combine_or_compare";
    public const string Total="usccss.g2.u01.l09.bar_total";
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);

    public static bool Supports(string? family)=>family is Height or Combine or Total;

    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported G2 U01 L09 bar graph");
        var p=new Dictionary<string,int>(StringComparer.Ordinal);
        for(var i=0;i<4;i++)p[$"bar{i}"]=random.Next(0,15);
        var museum=p["bar0"];
        var zoo=p["bar1"];
        var park=p["bar2"];
        var library=p["bar3"];
        var title="Bar graph: favorite school-trip locations. " +
            "Horizontal categories Museum, Zoo, Park, Library. " +
            "Vertical NUMBERED SCALE from 0 to 14, one student per tick. " +
            $"Museum height {museum}; Zoo height {zoo}; " +
            $"Park height {park}; Library height {library}. ";
        var explanation="Read the tick-marked vertical scale and find the correct bar height; " +
            "then add, compare or total categories as the question asks. " +
            "A numeric answer does not replace original student-written graph statements.";
        if(family==Height)
        {
            var target=random.Next(4);
            p["target"]=target;
            var name=new[]{"Museum","Zoo","Park","Library"}[target];
            title+=$"How many students selected {name}? Enter the exact bar height.";
        }
        else if(family==Combine)
        {
            var op=random.Next(2);p["operation"]=op;
            title+=op==0
                ?"How many selected Museum or Zoo altogether?"
                :"How many MORE selected Museum than Zoo, or Zoo than Museum? Enter the absolute difference.";
        }
        else title+="How many students selected ANY of the four categories in total?";
        return new Problem(family,title,explanation,AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid source bar-graph parameters");
        var n=family switch
        {
            Height=>p[$"bar{p["target"]}"],
            Combine=>p["operation"]==0
                ?p["bar0"]+p["bar1"]
                :Math.Abs(p["bar0"]-p["bar1"]),
            Total=>Enumerable.Range(0,4).Sum(i=>p[$"bar{i}"]),
            _=>throw new InvalidOperationException("Unexpected L09 family")
        };
        return n.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p) && string.Equals(answer,Solve(family,p),StringComparison.Ordinal);

    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family) || p.Count!=(family==Total?4:5))return false;
        for(var i=0;i<4;i++)
            if(!p.TryGetValue($"bar{i}",out var height) || height is <0 or >14)
                return false;
        if(family==Height)return p.TryGetValue("target",out var target) && target is >=0 and <=3;
        if(family==Combine)return p.TryGetValue("operation",out var op) && op is 0 or 1;
        return true;
    }
}
