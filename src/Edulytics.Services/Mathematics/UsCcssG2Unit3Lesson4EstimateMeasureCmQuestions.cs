using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Edulytics-authored calibrated integer ruler questions; cannot grade actual classroom objects.</summary>
internal static class UsCcssG2Unit3Lesson4EstimateMeasureCmQuestions
{
    public const string Zero="usccss.g2.u03.l04.cm_segment_zero_offset";
    public const string Shift="usccss.g2.u03.l04.cm_segment_shifted_mark";
    public const string Iterate="usccss.g2.u03.l04.cm_ruler_iteration";
    public const string Estimate="usccss.g2.u03.l04.cm_estimate_about_right";
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? s)=>s is Zero or Shift or Iterate or Estimate;
    public static Problem Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Not G2 U03 L04 cm family");
        var p=new Dictionary<string,int>(StringComparer.Ordinal);
        string prompt,solution;
        if(family is Zero or Shift)
        {
            int start=family==Zero?0:rng.Next(1,13);
            int finish=rng.Next(start+3,31);
            p["start"]=start;p["finish"]=finish;
            prompt=family==Zero
                ?$"FIRST-PARTY calibrated centimeter ruler: the object starts at the real ZERO tick, ends at tick {finish}. How long is it in CENTIMETERS?"
                :$"FIRST-PARTY centimeter ruler: an object starts at mark {start} and ends at mark {finish}. What is its length in cm? Do not mistake the ending LABEL for the distance.";
            solution=$"{finish}−{start}={finish-start} cm.";
        }
        else if(family==Iterate)
        {
            int count=rng.Next(1,3),remain=rng.Next(count==1?20:0,30),total=30*count+remain;
            p["count"]=count;p["remainder"]=remain;
            prompt=$"FIRST-PARTY classroom length model: exactly {count} full 30-cm ruler placement(s) plus a final {remain}-cm segment, with NO gaps or overlaps. How long in cm?";
            solution=$"{count}×30+{remain}={total} cm.";
        }
        else
        {
            int benchmark=rng.Next(8,31),below=benchmark-rng.Next(1,Math.Min(benchmark,7)),
            above=benchmark+rng.Next(1,8);
            p["below"]=below;p["benchmark"]=benchmark;p["above"]=above;
            prompt=$"FIRST-PARTY estimation-order model: Too low is {below} cm, too high is {above} cm, and the middle plausible target estimate is {benchmark} cm. Which number is the ABOUT-RIGHT estimate? This is NOT a measured real notebook.";
            solution=$"{below}<{benchmark}<{above}, so the middle estimate is {benchmark} cm.";
        }
        return new Problem(family,prompt,solution,AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid cm question parameters");
        int answer=family switch{
            Zero or Shift=>p["finish"]-p["start"],
            Iterate=>p["count"]*30+p["remainder"],
            Estimate=>p["benchmark"],
            _=>throw new InvalidOperationException("Unexpected cm family")
        };
        return answer.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family))return false;
        if(family is Zero or Shift)
            return p.Count==2&&p.TryGetValue("start",out var s)&&
                (family==Zero?s==0:s is >=1 and <=12)&&
                p.TryGetValue("finish",out var e)&&e is >=3 and <=30&&e>=s+3;
        if(family==Iterate)
            return p.Count==2&&p.TryGetValue("count",out var c)&&c is 1 or 2&&
                p.TryGetValue("remainder",out var rem)&&rem>=0&&rem<30&&
                c*30+rem is >=50 and <=90;
        return p.Count==3&&p.TryGetValue("below",out var low)&&
            p.TryGetValue("benchmark",out var mid)&&
            p.TryGetValue("above",out var high)&&low>=0&&mid is >=8 and <=30&&
            high<=100&&low<mid&&mid<high;
    }
}
