using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Original first-party numeric scenarios corresponding to four source story structures.</summary>
internal static class UsCcssG2Unit2Lesson12MancalaQuestions
{
    public const string Smaller="usccss.g2.u02.l12.compare_smaller_unknown";
    public const string Total="usccss.g2.u02.l12.put_together_total_unknown";
    public const string Remain="usccss.g2.u02.l12.take_apart_remaining_unknown";
    public const string Side="usccss.g2.u02.l12.put_together_missing_side";
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f is Smaller or Total or Remain or Side;
    public static Problem Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Not G2 U02 L12 exact supplement");
        int aTens=rng.Next(3,8),bTens=rng.Next(1,9-aTens);
        int aOnes=rng.Next(0,9),bOnes=rng.Next(aOnes+1,10);
        int a=10*aTens+aOnes,b=10*bTens+bOnes;
        var p=new Dictionary<string,int>(StringComparer.Ordinal){
            ["a"]=a,["b"]=b,["aTens"]=aTens,["bTens"]=bTens,["aOnes"]=aOnes,["bOnes"]=bOnes};
        string prompt=family switch{
            Smaller=>$"Edulytics original compare: Clare captured {a} counters; Han captured {b} FEWER. How many did Han capture?",
            Total=>$"Edulytics original combine: Clare has {a} counters, Han has {b}. How many are on the board ALTOGETHER?",
            Remain=>$"Edulytics original take-apart: Clare has {a} counters in all and holds {b}. How many are on the board?",
            _=>$"Edulytics original missing side: board has {a} counters in all, Clare's side has {b}. How many on Han's side?"
        };
        int result=family==Total?a+b:a-b;
        return new Problem(family,prompt,$"{a}"+(family==Total?"+":"−")+b+$"={result}. Teacher must still verify authentic source diagram matching.",AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid source-type Mancala arithmetic");
        return (family==Total?p["a"]+p["b"]:p["a"]-p["b"]).ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||p.Count!=6||
            !p.TryGetValue("a",out var a)||!p.TryGetValue("b",out var b)||
            !p.TryGetValue("aTens",out var at)||at is <3 or >7||
            !p.TryGetValue("bTens",out var bt)||bt<1||at+bt>8||
            !p.TryGetValue("aOnes",out var ao)||ao is <0 or >8||
            !p.TryGetValue("bOnes",out var bo)||bo is <1 or >9||bo<=ao)return false;
        return a==10*at+ao&&b==10*bt+bo&&a>b&&a+b<=100;
    }
}
