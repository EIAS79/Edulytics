using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Independent story number variants within50, source structures kept distinct.</summary>
internal static class UsCcssG2Unit2Lesson11SeedsStoryQuestions
{
    public const string CompareGap="usccss.g2.u02.l11.compare_difference_unknown";
    public const string TakeFrom="usccss.g2.u02.l11.take_from_result_unknown";
    public const string MissingPart="usccss.g2.u02.l11.put_together_missing_part";
    public const string Smaller="usccss.g2.u02.l11.compare_smaller_unknown";
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family is CompareGap or TakeFrom or MissingPart or Smaller;
    public static Problem Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Not G2 U02 L11 source type");
        int t=rng.Next(2,6),lo=rng.Next(1,t),ones=t==5?0:rng.Next(0,9),lessOnes=rng.Next(ones+1,10);
        int whole=10*t+ones,known=10*lo+lessOnes,result=whole-known;
        var p=new Dictionary<string,int>(StringComparer.Ordinal){{"whole",whole},{"known",known},{"wholeTens",t},{"wholeOnes",ones},{"knownTens",lo},{"knownOnes",lessOnes}};
        string prompt=family switch {
            CompareGap=>$"First-party seed comparison: Diego has {whole} packets and Jada has {known}. How many MORE does Diego have?",
            TakeFrom=>$"First-party seed takeaway: Lin started with {whole} seeds and gave Priya {known}. How many seeds REMAIN?",
            MissingPart=>$"First-party missing part: Noah made a design of {whole} seeds, {known} yellow and the rest blue. How many are BLUE?",
            _=>$"First-party smaller unknown: Elena has {whole} pumpkin seeds. Andre has {known} FEWER. How many does Andre have?"
        };
        return new Problem(family,prompt,$"{whole}−{known}={result}; represent whole/part or larger/smaller, trade one ten if subtracting by place.",AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid G2 U02 L11 story parameters");
        return (p["whole"]-p["known"]).ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)=>
        Supports(family)&&p.Count==6&&p.TryGetValue("whole",out var whole)&&
        p.TryGetValue("known",out var known)&&
        p.TryGetValue("wholeTens",out var wt)&&wt is >=2 and <=5&&
        p.TryGetValue("wholeOnes",out var wo)&&wo is >=0 and <=8&&
        p.TryGetValue("knownTens",out var kt)&&kt>=1&&kt<wt&&
        p.TryGetValue("knownOnes",out var ko)&&ko is >=1 and <=9&&ko>wo&&
        whole==10*wt+wo&&whole<=50&&known==10*kt+ko&&known<whole;
}
