using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Independent sample data only, not real partner-measured objects.</summary>
internal static class UsCcssG2Unit3Lesson17CenterPlotQuestions
{
    public const string Frequency="usccss.g2.u03.l17.fictional_plot_frequency";
    public const string Total="usccss.g2.u03.l17.fictional_plot_total";
    public const string Range="usccss.g2.u03.l17.fictional_plot_range";
    public const string Above="usccss.g2.u03.l17.fictional_plot_above_threshold";
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f is Frequency or Total or Range or Above;
    public static Problem Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Not optional G2 Stage1 plot");
        int first=rng.Next(7,17),a=rng.Next(1,3),b=rng.Next(1,3),c=rng.Next(1,3),d=rng.Next(1,3),unit=rng.Next(0,2);
        var p=new Dictionary<string,int>(StringComparer.Ordinal){{"first",first},{"a",a},{"b",b},{"c",c},{"d",d},{"unit",unit}};
        string u=unit==0?"cm":"in";
        string visual=$"FICTIONAL measured objects in {u} (not real classmates): length {first}:{a} Xs, {first+1}:{b} Xs, {first+2}:{c} Xs, {first+3}:{d} Xs";
        string prompt=family switch{
            Frequency=>$"{visual}. How many fictional objects have length {first+1} {u}?",
            Total=>$"{visual}. How many fictional objects are measured overall (at most8)?",
            Range=>$"{visual}. What is the difference in {u} between longest and shortest?",
            _=>$"{visual}. How many objects are STRICTLY longer than {first+1} {u}?"
        };
        int answer=family switch{Frequency=>b,Total=>a+b+c+d,Range=>3,_=>c+d};
        return new Problem(family,prompt,$"Answer {answer}; the sample uses one uniform unit {u}, with at most8 objects.",AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid optional plot");
        var n=family switch{Frequency=>p["b"],Total=>p["a"]+p["b"]+p["c"]+p["d"],
            Range=>3,Above=>p["c"]+p["d"],_=>throw new InvalidOperationException("Unknown family")};
        return n.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)=>
        Supports(family)&&p.Count==6&&p.TryGetValue("first",out var first)&&first is >=7 and <=16&&
        p.TryGetValue("a",out var a)&&a is >=1 and <=2&&
        p.TryGetValue("b",out var b)&&b is >=1 and <=2&&
        p.TryGetValue("c",out var c)&&c is >=1 and <=2&&
        p.TryGetValue("d",out var d)&&d is >=1 and <=2&&
        p.TryGetValue("unit",out var u)&&u is 0 or 1;
}
