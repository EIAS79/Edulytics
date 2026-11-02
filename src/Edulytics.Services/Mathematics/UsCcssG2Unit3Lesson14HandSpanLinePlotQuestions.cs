using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Never uses or fabricates live hand-span class counts; all dataset counts are first-party.</summary>
internal static class UsCcssG2Unit3Lesson14HandSpanLinePlotQuestions
{
    public const string Frequency="usccss.g2.u03.l14.line_plot_frequency";
    public const string Total="usccss.g2.u03.l14.line_plot_total_measurements";
    public const string Most="usccss.g2.u03.l14.line_plot_most_common_length";
    public const string Range="usccss.g2.u03.l14.line_plot_length_range";
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f is Frequency or Total or Most or Range;
    public static Problem Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Not G2 U03 L14 line plot");
        int first=rng.Next(3,7),mode=first+rng.Next(1,3),last=mode+rng.Next(1,3);
        int a=rng.Next(1,4),b=rng.Next(4,7),c=rng.Next(1,4);
        var p=new Dictionary<string,int>(StringComparer.Ordinal){
            ["first"]=first,["mode"]=mode,["last"]=last,
            ["firstCount"]=a,["modeCount"]=b,["lastCount"]=c};
        var data=$"{first} inches: {a} Xs; {mode} inches: {b} Xs; {last} inches: {c} Xs";
        var question=family switch{
            Frequency=>$"FICTIONAL hand-span line plot ({data}). How many measurements were {first} inches long?",
            Total=>$"FICTIONAL hand-span line plot ({data}). How many measurements in TOTAL (count all Xs)?",
            Most=>$"FICTIONAL hand-span line plot ({data}). Which NUMERICAL hand-span length in inches was most common?",
            _=>$"FICTIONAL hand-span line plot ({data}). How many inches separate the longest and shortest measurements?"
        };
        var value=family switch{Frequency=>a,Total=>a+b+c,Most=>mode,_=>last-first};
        return new Problem(family,question,$"Each X is one hand-span measurement; result {value}. The numbers are not genuine class data.",AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid fictional line plot");
        int result=family switch{Frequency=>p["firstCount"],Total=>p["firstCount"]+p["modeCount"]+p["lastCount"],
            Most=>p["mode"],Range=>p["last"]-p["first"],_=>throw new InvalidOperationException("Unknown plot")};
        return result.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? f,IReadOnlyDictionary<string,int> p)=>
        Supports(f)&&p.Count==6&&p.TryGetValue("first",out var first)&&first is >=3 and <=6&&
        p.TryGetValue("mode",out var mode)&&mode>first&&mode<=first+2&&
        p.TryGetValue("last",out var last)&&last>mode&&last<=mode+2&&last<=10&&
        p.TryGetValue("firstCount",out var a)&&a is >=1 and <=3&&
        p.TryGetValue("modeCount",out var b)&&b is >=4 and <=6&&
        p.TryGetValue("lastCount",out var c)&&c is >=1 and <=3;
}
