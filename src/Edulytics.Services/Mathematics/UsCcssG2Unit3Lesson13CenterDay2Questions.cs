using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Independent numeric companions to authentic ruler and digit-card centers.</summary>
internal static class UsCcssG2Unit3Lesson13CenterDay2Questions
{
    public const string Cm="usccss.g2.u03.l13.measurement_absolute_difference_cm";
    public const string Inch="usccss.g2.u03.l13.measurement_absolute_difference_in";
    public const string Add="usccss.g2.u03.l13.number_puzzle_add_missing";
    public const string Subtract="usccss.g2.u03.l13.number_puzzle_sub_missing";
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family is Cm or Inch or Add or Subtract;
    public static Problem Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Not target measurement/numeric puzzle");
        var p=new Dictionary<string,int>(StringComparer.Ordinal);
        string prompt,explain;
        if(family is Cm or Inch)
        {
            int max=family==Cm?30:10,target=rng.Next(1,max+1),measured=Math.Max(0,target+rng.Next(-5,6));
            p["target"]=target;p["measured"]=measured;
            var unit=family==Cm?"cm":"inches";
            prompt=$"FIRST-PARTY measurement arithmetic: a target is {target} {unit}, an illustrated result measures {measured} {unit}. What is the absolute difference? This does not prove that anyone drew or measured a real line.";
            explain=$"|{measured}−{target}|={Math.Abs(measured-target)} {unit}.";
        }
        else
        {
            var missing=rng.Next(11,36);
            if(family==Add)
            {
                var known=rng.Next(26,101-missing); var sum=known+missing;
                p["known"]=known;p["total"]=sum;
                prompt=$"FIRST-PARTY Number Puzzles-style missing addend: {known}+?={sum}. What number makes the equation true?";
                explain=$"{sum}−{known}={missing}.";
            }
            else
            {
                var result=rng.Next(12,51); var start=result+missing;
                p["start"]=start;p["result"]=result;
                prompt=$"FIRST-PARTY missing subtrahend: {start}−?={result}. What number makes the equation true?";
                explain=$"{start}−{result}={missing}.";
            }
        }
        return new Problem(family,prompt,explain,AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid G2 U03 L13 exact parameters");
        int n=family switch{
            Cm or Inch=>Math.Abs(p["measured"]-p["target"]),
            Add=>p["total"]-p["known"],Subtract=>p["start"]-p["result"],
            _=>throw new InvalidOperationException("Invalid G2 U03 L13 family")
        };
        return n.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||p.Count!=2)return false;
        if(family is Cm or Inch)
        {
            int max=family==Cm?30:10;
            return p.TryGetValue("target",out var t)&&t>=1&&t<=max&&
                p.TryGetValue("measured",out var m)&&m>=0&&m<=max+5&&Math.Abs(m-t)<=5;
        }
        if(family==Add)return p.TryGetValue("total",out var sum)&&sum is >=37 and <=100&&
            p.TryGetValue("known",out var k)&&k>=26&&sum-k is >=11 and <=35;
        return p.TryGetValue("start",out var a)&&a is >=23 and <=85&&
            p.TryGetValue("result",out var b)&&b is >=12 and <=50&&a-b is >=11 and <=35;
    }
}
