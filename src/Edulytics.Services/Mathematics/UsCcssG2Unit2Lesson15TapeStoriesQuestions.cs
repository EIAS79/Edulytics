using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>First-party numerical tape diagrams independent of original copyrighted game cards.</summary>
internal static class UsCcssG2Unit2Lesson15TapeStoriesQuestions
{
    public const string Total="usccss.g2.u02.l15.tape_combine_total";
    public const string Missing="usccss.g2.u02.l15.tape_part_missing";
    public const string Gap="usccss.g2.u02.l15.tape_compare_gap";
    public const string Compensate="usccss.g2.u02.l15.compensate_minus19";
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f is Total or Missing or Gap or Compensate;
    public static Problem Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Wrong optional tape family");
        int a,b;
        if(family==Total){a=rng.Next(15,50);b=rng.Next(11,Math.Min(101-a,41));}
        else if(family==Compensate){a=rng.Next(30,100);b=19;}
        else{a=rng.Next(50,100);b=rng.Next(15,a-5);}
        var p=new Dictionary<string,int>(StringComparer.Ordinal){{"a",a},{"b",b}};
        string prompt=family switch{
            Total=>$"FIRST-PARTY invented tape: one part is{a} and another part is{b}. How many altogether? This is NOT the printed publisher card.",
            Missing=>$"FIRST-PARTY invented tape: whole is{a}, known part is{b}. How much is the unknown part?",
            Gap=>$"FIRST-PARTY invented compare tape: larger amount{a}, smaller amount{b}. What is the difference?",
            _=>$"Original Edulytics compensation: calculate {a}−19 by doing {a}−20+1."
        };
        var value=family==Total?a+b:a-b;
        return new Problem(family,prompt,$"{a}"+(family==Total?"+":"−")+b+$"={value}. Digital numeric answer does not prove original partner story.",AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid G2 optional tape parameters");
        return (family==Total?p["a"]+p["b"]:p["a"]-p["b"])
            .ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||p.Count!=2||!p.TryGetValue("a",out var a)||!p.TryGetValue("b",out var b))return false;
        return family switch{
            Total=>a is >=15 and <=49&&b is >=11 and <=40&&a+b<=100,
            Missing or Gap=>a is >=50 and <=99&&b>=15&&b<a-5,
            Compensate=>a is >=30 and <=99&&b==19,
            _=>false
        };
    }
}
