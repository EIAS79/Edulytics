using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>First-party numeric data for source-type line plots; actual Han image unavailable.</summary>
internal static class UsCcssG2Unit3Lesson16PlantHeightsQuestions
{
    public const string Frequency="usccss.g2.u03.l16.plant_plot_frequency";
    public const string Total="usccss.g2.u03.l16.plant_plot_total";
    public const string Greater="usccss.g2.u03.l16.plant_plot_greater_than";
    public const string Range="usccss.g2.u03.l16.plant_plot_range";
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f is Frequency or Total or Greater or Range;
    public static Problem Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Not G2 U03 L16 heights");
        int start=rng.Next(20,37),a=rng.Next(1,4),b=rng.Next(2,5),c=rng.Next(1,4),d=rng.Next(1,4);
        var p=new Dictionary<string,int>(StringComparer.Ordinal){
            ["start"]=start,["a"]=a,["b"]=b,["c"]=c,["d"]=d};
        var plot=$"FICTIONAL plant heights (cm): {start}: {a} X; {start+1}: {b} X; {start+2}: 0 X; {start+3}: {c} X; {start+4}: {d} X";
        var prompt=family switch{
            Frequency=>$"{plot}. How many plants are {start+1}cm tall?",
            Total=>$"{plot}. How many plants are shown altogether?",
            Greater=>$"{plot}. How many plants are STRICTLY taller than {start+1}cm?",
            _=>$"{plot}. How many centimeters separate the tallest and shortest measured plants?"
        };
        var sol=family switch{Frequency=>b,Total=>a+b+c+d,Greater=>c+d,_=>4};
        return new Problem(family,prompt,$"All tick labels are consecutive; answer {sol}. Fictional measurements are NOT published Group B data.",AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid fictional line-plot parameters");
        int x=family switch{Frequency=>p["b"],Total=>p["a"]+p["b"]+p["c"]+p["d"],
            Greater=>p["c"]+p["d"],Range=>4,_=>throw new InvalidOperationException("Unknown family")};
        return x.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? f,IReadOnlyDictionary<string,int> p)=>
        Supports(f)&&p.Count==5&&p.TryGetValue("start",out var s)&&s is >=20 and <=36&&
        p.TryGetValue("a",out var a)&&a is >=1 and <=3&&
        p.TryGetValue("b",out var b)&&b is >=2 and <=4&&
        p.TryGetValue("c",out var c)&&c is >=1 and <=3&&
        p.TryGetValue("d",out var d)&&d is >=1 and <=3;
}
