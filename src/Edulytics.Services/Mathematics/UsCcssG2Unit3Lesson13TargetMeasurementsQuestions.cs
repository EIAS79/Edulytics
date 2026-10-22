using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Original numerical variants; NEVER assert real ruler readings or actual center win.</summary>
internal static class UsCcssG2Unit3Lesson13TargetMeasurementsQuestions
{
    public const string Inches="usccss.g2.u03.l13.target_inches_absolute_error";
    public const string Cm="usccss.g2.u03.l13.target_centimeters_absolute_error";
    public const string Compose="usccss.g2.u03.l13.number_puzzle_compose_ten";
    public const string Decompose="usccss.g2.u03.l13.number_puzzle_decompose_ten";
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family is Inches or Cm or Compose or Decompose;
    public static Problem Build(string family,Random r)
    {
        if(!Supports(family))throw new InvalidOperationException("Not Grade2 target measurement family");
        var p=new Dictionary<string,int>(StringComparer.Ordinal);
        string prompt,reason;
        if(family is Inches or Cm)
        {
            var lim=family==Inches?10:30;
            var target=r.Next(1,lim+1);
            var measured=Math.Clamp(target+r.Next(-4,5),0,lim+5);
            p["target"]=target;p["measured"]=measured;p["unit"]=family==Inches?1:2;
            prompt=$"FIRST-PARTY ruler illustration: target {target} "+(family==Inches?"inches":"centimeters")+
              $", measured {measured} in the SAME unit. What nonnegative DIFFERENCE (error) does the STOP partner score?";
            reason=$"Error=|{target}−{measured}|={Math.Abs(target-measured)}. Only actual teacher-observed eight rounds determine a winner.";
        }
        else
        {
            var tenA=r.Next(1,5),tenB=r.Next(1,5),oneA=r.Next(5,10);
            var oneB=r.Next(10-oneA,10);
            var a=10*tenA+oneA,b=10*tenB+oneB;
            if(a+b>100)throw new InvalidOperationException("Generated >100");
            p["a"]=a;p["b"]=b;
            if(family==Compose)
                prompt=$"FIRST-PARTY missing addend (not source printed board): {a}+?={a+b}. What number makes it true? Make a new ten.";
            else
                prompt=$"FIRST-PARTY missing subtrahend (not source printed board): {a+b}−?={a}. What number makes it true? Decompose a ten.";
            reason=$"{a}+{b}={a+b}; {a+b}−{b}={a}. Ones sum{oneA}+{oneB} composes a ten.";
        }
        return new Problem(family,prompt,reason,AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid target/puzzle parameters");
        return (family is Inches or Cm?Math.Abs(p["target"]-p["measured"]):p["b"])
            .ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family))return false;
        if(family is Inches or Cm)
        {
            int lim=family==Inches?10:30,u=family==Inches?1:2;
            return p.Count==3&&p.TryGetValue("target",out var t)&&t>=1&&t<=lim&&
                p.TryGetValue("measured",out var m)&&m>=0&&m<=lim+5&&
                p.TryGetValue("unit",out var unit)&&unit==u;
        }
        return p.Count==2&&p.TryGetValue("a",out var a)&&a is >=10 and <=49&&
            p.TryGetValue("b",out var b)&&b is >=10 and <=49&&a+b<=100&&(a%10+b%10)>=10;
    }
}
