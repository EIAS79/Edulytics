using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Original first-party numeric distributions with ONE empty tick;
/// source Group C is checked separately in direct student activities.</summary>
internal static class UsCcssG2Unit3Lesson15PencilLinePlotQuestions
{
    public const string Frequency="usccss.g2.u03.l15.pencil_plot_frequency";
    public const string Total="usccss.g2.u03.l15.pencil_plot_total";
    public const string Mode="usccss.g2.u03.l15.pencil_plot_modal_length";
    public const string Empty="usccss.g2.u03.l15.pencil_plot_empty_tick";
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f is Frequency or Total or Mode or Empty;
    public static Problem Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Not G2 U03 L15 line plot");
        int first=rng.Next(9,16),mode=first+1,blank=first+2,last=first+3;
        int a=rng.Next(1,4),b=rng.Next(4,7),c=rng.Next(1,4);
        var p=new Dictionary<string,int>(StringComparer.Ordinal){
            ["first"]=first,["mode"]=mode,["blank"]=blank,["last"]=last,
            ["firstCount"]=a,["modeCount"]=b,["lastCount"]=c};
        var display=$"FICTIONAL pencil lengths in centimeters: {first}: {a} Xs, {mode}: {b} Xs, {blank}: zero Xs, {last}: {c} Xs";
        var prompt=family switch{
            Frequency=>$"{display}. How many pencils measured {first} cm?",
            Total=>$"{display}. How many pencils were measured in total?",
            Mode=>$"{display}. Which centimeter LENGTH was most common?",
            _=>$"{display}. What whole-centimeter label must be INCLUDED between {mode} and {last}, although it has NO Xs?"
        };
        var result=family switch{Frequency=>a,Total=>a+b+c,Mode=>mode,_=>blank};
        return new Problem(family,prompt,$"Equal-spaced consecutive centimeter labels include {blank}; answer {result}. Not genuine pupil measurements.",AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid pencil distribution");
        var x=family switch{Frequency=>p["firstCount"],Total=>p["firstCount"]+p["modeCount"]+p["lastCount"],
            Mode=>p["mode"],Empty=>p["blank"],_=>throw new InvalidOperationException("Unknown family")};
        return x.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? f,IReadOnlyDictionary<string,int> p)=>
        Supports(f)&&p.Count==7&&p.TryGetValue("first",out var first)&&first is >=9 and <=15&&
        p.TryGetValue("mode",out var mode)&&mode==first+1&&
        p.TryGetValue("blank",out var blank)&&blank==first+2&&
        p.TryGetValue("last",out var last)&&last==first+3&&
        p.TryGetValue("firstCount",out var a)&&a is >=1 and <=3&&
        p.TryGetValue("modeCount",out var b)&&b is >=4 and <=6&&
        p.TryGetValue("lastCount",out var c)&&c is >=1 and <=3;
}
