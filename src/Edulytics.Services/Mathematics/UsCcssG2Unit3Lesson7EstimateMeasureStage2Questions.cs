using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Original illustrative numeric gaps, NEVER school objects physically measured.</summary>
internal static class UsCcssG2Unit3Lesson7EstimateMeasureStage2Questions
{
    public const string Below="usccss.g2.u03.l07.estimate_below_actual_gap";
    public const string Above="usccss.g2.u03.l07.estimate_above_actual_gap";
    public const string Gap="usccss.g2.u03.l07.absolute_estimate_gap";
    public const string Closest="usccss.g2.u03.l07.closest_of_two_estimates_gap";
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f is Below or Above or Gap or Closest;
    public static Problem Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Not G2 U03 L07");
        var actual=rng.Next(25,91),distance=rng.Next(3,Math.Min(16,actual)+1);
        int estimate=family==Above?actual+distance:actual-distance;
        if(estimate>100)estimate=actual-distance;
        var p=new Dictionary<string,int>(StringComparer.Ordinal){{"actual",actual},{"estimate",estimate}};
        string prompt,solution;
        if(family==Closest)
        {
            var estimateB=actual+rng.Next(1,Math.Min(10,101-actual));
            p["estimateB"]=estimateB;
            prompt=$"FIRST-PARTY illustration, not a real classroom reading: a picture's recorded length is {actual} cm. Guess A={estimate} cm, guess B={estimateB} cm. How many cm is the smaller of the TWO ABSOLUTE gaps?";
            solution=$"min(|{actual}−{estimate}|,|{actual}−{estimateB}|)={Math.Min(Math.Abs(actual-estimate),Math.Abs(actual-estimateB))} cm.";
        }
        else
        {
            prompt=$"FIRST-PARTY example, not a physical reading: an estimate is {estimate} cm, an illustrated actual length is {actual} cm. How many centimeters apart are estimate and actual?";
            solution=$"|{actual}−{estimate}|={Math.Abs(actual-estimate)} cm.";
        }
        return new Problem(family,prompt,solution,AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid estimate parameters");
        int diff=Math.Abs(p["actual"]-p["estimate"]);
        if(family==Closest)diff=Math.Min(diff,Math.Abs(p["actual"]-p["estimateB"]));
        return diff.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||p.Count!=(family==Closest?3:2)||
           !p.TryGetValue("actual",out var a)||a is <25 or >90||
           !p.TryGetValue("estimate",out var e)||e is <0 or >100||e==a)return false;
        if(family==Above&&e<=a)return false;
        if(family==Below&&e>=a)return false;
        if(family==Closest)return p.TryGetValue("estimateB",out var b)&&b is >=1 and <=100&&b!=a;
        return true;
    }
}
