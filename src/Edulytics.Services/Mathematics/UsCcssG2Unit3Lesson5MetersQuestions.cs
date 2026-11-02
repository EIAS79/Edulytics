using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Exact first-party meter arithmetic, physically laid tape only assessed by teacher.</summary>
internal static class UsCcssG2Unit3Lesson5MetersQuestions
{
    public const string Mixed="usccss.g2.u03.l05.meters_centimeters_to_cm";
    public const string Remainder="usccss.g2.u03.l05.cm_to_remaining_centimeters";
    public const string Iterate="usccss.g2.u03.l05.full_meter_sticks_plus_remainder";
    public const string Gap="usccss.g2.u03.l05.compare_reptile_tape_lengths";
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f is Mixed or Remainder or Iterate or Gap;
    public static Problem Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Not G2 U03 L05");
        var p=new Dictionary<string,int>(StringComparer.Ordinal);
        string prompt,solution;
        if(family==Gap)
        {
            int longCm=rng.Next(180,401),shortCm=rng.Next(50,longCm-25);
            p["longCm"]=longCm;p["shortCm"]=shortCm;
            prompt=$"Original Edulytics floor-tape comparison: strip P has length {longCm} cm; strip Q has {shortCm} cm. How many centimeters LONGER is P? These are first-party numbers.";
            solution=$"{longCm}−{shortCm}={longCm-shortCm} cm.";
        }
        else
        {
            int meters=rng.Next(1,5),cm=rng.Next(1,100),total=100*meters+cm;
            p["meters"]=meters;p["remainingCm"]=cm;p["totalCm"]=total;
            prompt=family switch{
                Mixed=>$"FIRST-PARTY length: {meters} meters and {cm} centimeters. How many CENTIMETERS in total?",
                Remainder=>$"FIRST-PARTY tape is {total} cm. After taking {meters} full100-cm meterstick lengths, how many centimeters REMAIN?",
                _=>$"FIRST-PARTY tape: {meters} complete meterstick placements and {cm} extra cm, no gaps or overlaps. What is the total in CENTIMETERS?"
            };
            solution=$"{meters}×100+{cm}={total} cm, {meters} meter(s), remainder{cm} cm.";
        }
        return new Problem(family,prompt,solution,AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid meter math parameters");
        var ans=family==Gap?p["longCm"]-p["shortCm"]:
            family==Remainder?p["remainingCm"]:p["totalCm"];
        return ans.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family))return false;
        if(family==Gap)return p.Count==2&&p.TryGetValue("longCm",out var l)&&
            l is >=180 and <=400&&p.TryGetValue("shortCm",out var s)&&s>=50&&s<l-25;
        return p.Count==3&&p.TryGetValue("meters",out var m)&&m is >=1 and <=4&&
            p.TryGetValue("remainingCm",out var cm)&&cm is >=1 and <=99&&
            p.TryGetValue("totalCm",out var t)&&t==100*m+cm;
    }
}
