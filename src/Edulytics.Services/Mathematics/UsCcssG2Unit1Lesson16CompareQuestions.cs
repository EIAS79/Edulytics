using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Source-aligned numerical Grade 2 U01 L16 comparisons, place-value
/// multiple-of-ten differences or make-ten one digit adds within 100.
/// Open Three Reads/school roleplay not auto-certified.
/// </summary>
internal static class UsCcssG2Unit1Lesson16CompareQuestions
{
    public const string Larger="usccss.g2.u01.l16.solve_compare_larger";
    public const string Smaller="usccss.g2.u01.l16.solve_compare_smaller";
    public const string Difference="usccss.g2.u01.l16.solve_compare_difference";
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family is Larger or Smaller or Difference;

    public static Problem Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported Grade2 U01 L16 compare");
        var small=rng.Next(14,71);
        var extra=rng.Next(2)==0 ? rng.Next(1,10) : rng.Next(1,4)*10;
        if(small+extra>100)extra=100-small;
        var big=small+extra;
        var p=new Dictionary<string,int>(StringComparer.Ordinal){
            ["small"]=small,["difference"]=extra,["big"]=big
        };
        var prompt=family switch{
            Larger=>$"New Edulytics library story: Arin returns {small} books. Noor returns {extra} MORE. With a labeled tape, how many books does Noor return?",
            Smaller=>$"New Edulytics library story: Arin reads {big} pages. Noor reads {extra} FEWER. What is the smaller number of pages Noor reads?",
            Difference=>$"New Edulytics reading story: Arin reads {big} pages, Noor {small}. How many FEWER pages does Noor read? Show the unknown gap on a tape.",
            _=>throw new InvalidOperationException("Unsupported source family")
        };
        return new Problem(family,prompt,
            $"The smaller part is {small}, the difference is {extra}, the bigger part is {big}. Make ten or use multiples of ten and check {small}+{extra}={big}.",
            AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Bad Grade2 compare parameters");
        var value=family switch{
            Larger=>p["big"],
            Smaller=>p["small"],
            Difference=>p["difference"],
            _=>throw new InvalidOperationException("Invalid family")
        };
        return value.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p) && string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)=>
        Supports(family) && p.Count==3 &&
        p.TryGetValue("small",out var small) && small is >=1 and <=99 &&
        p.TryGetValue("difference",out var difference) && difference is >=1 and <=99 &&
        p.TryGetValue("big",out var big) && big is >=2 and <=100 &&
        small+difference==big;
}
