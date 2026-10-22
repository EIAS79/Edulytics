using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Original randomized seed-equation stories; source task structure, not publisher cards.</summary>
internal static class UsCcssG2Unit2Lesson13StoryEquationsQuestions
{
    public const string Start="usccss.g2.u02.l13.add_to_start_unknown";
    public const string Change="usccss.g2.u02.l13.add_to_change_unknown";
    public const string Total="usccss.g2.u02.l13.put_together_total_unknown";
    public const string Part="usccss.g2.u02.l13.put_together_part_unknown";
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f is Start or Change or Total or Part;
    public static Problem Build(string family,Random r)
    {
        if(!Supports(family))throw new InvalidOperationException("Not G2 U02 L13 exact story");
        int a=r.Next(20,51),b=r.Next(11,Math.Min(101-a,45));
        int total=a+b;
        var p=new Dictionary<string,int>(StringComparer.Ordinal){{"a",a},{"b",b},{"total",total}};
        string prompt=family switch{
            Start=>$"First-party Lin-style start-unknown story: a student started with ? seeds, WON {b}, now has {total}. How many seeds were at the START? Equation ?+{b}={total}.",
            Change=>$"First-party Andre-style change-unknown story: a student started with {a} seeds, WON ?, now has {total}. How many were WON? Equation {a}+?={total}.",
            Total=>$"First-party Diego-style total-unknown story: a student gathered {a} yellow seeds and {b} blue seeds. How many IN ALL? Equation {a}+{b}=?",
            _=>$"First-party Noah/Kiran-style part-unknown story: two students gathered {total} seeds, one gathered {a}, and the other gathered ?. How many did the OTHER gather? Equation {a}+?={total}."
        };
        return new Problem(family,prompt,$"{a}+{b}={total}, so the named unknown is "+(family==Start?a:family==Total?total:b)+".",AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid G2 U02 L13 unknown parameters");
        return (family==Start?p["a"]:family==Total?p["total"]:p["b"])
            .ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)=>
        Supports(family)&&p.Count==3&&p.TryGetValue("a",out var a)&&a is >=20 and <=50&&
        p.TryGetValue("b",out var b)&&b is >=11 and <=44&&p.TryGetValue("total",out var t)&&
        t==a+b&&t<=100;
}
