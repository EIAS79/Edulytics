using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Independently authored HYPOTHETICAL 20–30 object collections. Numeric
/// analogues do not claim real physical sorting, verified classroom
/// collection, student original drawings, or actual partner discussions.
/// </summary>
internal static class UsCcssG2Unit1Lesson12SortAndDisplayQuestions
{
    public const string Count="usccss.g2.u01.l12.sort_three_category_count";
    public const string Combine="usccss.g2.u01.l12.sort_three_category_combine";
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family is Count or Combine;
    private static readonly string[] Labels=["Blocks","Cubes","Counters"];

    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Not original G2 U01 L12 numeric family");
        var total=random.Next(20,31);
        var a=random.Next(1,total-2);
        var b=random.Next(1,total-a);
        var c=total-a-b;
        var p=new Dictionary<string,int>(StringComparer.Ordinal)
            {["a"]=a,["b"]=b,["c"]=c};
        var intro=$"HYPOTHETICAL practice example (not actual classroom objects): " +
            $"a group of {total} items sorted into exactly 3 categories: " +
            $"Blocks {a}, Cubes {b}, Counters {c}. " +
            "A first-party picture graph uses one symbol per item. ";
        if(family==Count)
        {
            var idx=random.Next(3);
            p["target"]=idx;
            intro+=$"How many items should be represented in the {Labels[idx]} category?";
        }
        else
        {
            var first=random.Next(3);
            var second=random.Next(3);
            if(second==first)second=(second+1)%3;
            p["first"]=first;p["second"]=second;
            intro+=$"How many items are in the {Labels[first]} and {Labels[second]} categories altogether?";
        }
        return new Problem(family,intro,
            "Use the first-party sample counts; count or combine exactly the named categories. Genuine sorting is a separate teacher-reviewed physical activity.",
            AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid sample three-category source parameters");
        var values=new[]{p["a"],p["b"],p["c"]};
        var result=family==Count?values[p["target"]]:
            values[p["first"]]+values[p["second"]];
        return result.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p) && string.Equals(answer,Solve(family,p),StringComparison.Ordinal);

    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||p.Count!=(family==Count?4:5) ||
            !p.TryGetValue("a",out var a) ||
            !p.TryGetValue("b",out var b) ||
            !p.TryGetValue("c",out var c) ||
            a<1||b<1||c<1||a+b+c is <20 or >30)
            return false;
        if(family==Count)return p.TryGetValue("target",out var target) &&
            target is >=0 and <=2;
        return p.TryGetValue("first",out var first)&&first is >=0 and <=2 &&
            p.TryGetValue("second",out var second)&&second is >=0 and <=2 &&
            first!=second;
    }
}
