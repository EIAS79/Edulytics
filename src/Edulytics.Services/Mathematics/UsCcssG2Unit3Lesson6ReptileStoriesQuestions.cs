using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Original parameterized comparisons; author-supplied source reptile lists remain for teacher game.</summary>
internal static class UsCcssG2Unit3Lesson6ReptileStoriesQuestions
{
    public const string Bigger="usccss.g2.u03.l06.bigger_unknown_shorter_word";
    public const string Smaller="usccss.g2.u03.l06.smaller_unknown_longer_word";
    public const string Difference="usccss.g2.u03.l06.difference_unknown_cm";
    public const string Reptiles="usccss.g2.u03.l06.opposite_reptile_lists_gap";
    private static readonly string[] A=["day gecko","komodo dragon","baby cobra","iguana"];
    private static readonly int[] AL=[28,98,46,65];
    private static readonly string[] B=["ribbon snake","gila monster","baby alligator","ringneck snake"];
    private static readonly int[] BL=[83,55,71,38];
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f is Bigger or Smaller or Difference or Reptiles;
    public static Problem Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Not G2 U03 L06 reptiles");
        var p=new Dictionary<string,int>(StringComparer.Ordinal);
        string prompt,solution;int answer;
        if(family==Reptiles)
        {
            int ai=rng.Next(0,4),bi=rng.Next(0,4);
            p["aIndex"]=ai;p["bIndex"]=bi;
            answer=Math.Abs(AL[ai]-BL[bi]);
            prompt=$"SOURCE list values, independent numeric comparison: partner A's {A[ai]} is {AL[ai]} cm and B's {B[bi]} is {BL[bi]} cm. What is the DIFFERENCE in centimeters? These were not genuinely secret partner selections.";
            solution=$"|{AL[ai]}−{BL[bi]}|={answer} cm.";
        }
        else
        {
            int smaller=rng.Next(20,66),diff=rng.Next(10,101-smaller);
            int bigger=smaller+diff;
            p["smaller"]=smaller;p["difference"]=diff;p["bigger"]=bigger;
            if(family==Bigger)
            {
                answer=bigger;
                prompt=$"A first-party reptile is {smaller} cm. It is {diff} cm SHORTER than its friend's reptile. How long is the BIGGER friend's reptile?";
            }
            else if(family==Smaller)
            {
                answer=smaller;
                prompt=$"One first-party reptile is {bigger} cm and it is {diff} cm LONGER than another. How long is the SMALLER reptile?";
            }
            else
            {
                answer=diff;
                prompt=$"Two first-party reptiles are {bigger} cm and {smaller} cm. How many centimeters LONGER is the bigger one?";
            }
            solution=$"Bigger {bigger} cm, smaller {smaller} cm, difference {diff} cm; answer {answer}.";
        }
        return new Problem(family,prompt,solution,AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid reptile story parameters");
        var answer=family switch{
            Reptiles=>Math.Abs(AL[p["aIndex"]]-BL[p["bIndex"]]),
            Bigger=>p["bigger"],Smaller=>p["smaller"],Difference=>p["difference"],
            _=>throw new InvalidOperationException("Unknown reptile type")
        };
        return answer.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family))return false;
        if(family==Reptiles)return p.Count==2&&p.TryGetValue("aIndex",out var a)&&a is >=0 and <=3&&
            p.TryGetValue("bIndex",out var b)&&b is >=0 and <=3;
        return p.Count==3&&p.TryGetValue("smaller",out var s)&&s is >=20 and <=65&&
            p.TryGetValue("difference",out var d)&&d is >=10 and <=80&&
            p.TryGetValue("bigger",out var l)&&l==s+d&&l<=100;
    }
}
