using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Independently generated original integer questions with verifiable
/// one-ten exchange; source task TYPES not copied publisher response sheets.</summary>
internal static class UsCcssG2Unit2Lesson5DecomposeTenQuestions
{
    public const string Trade="usccss.g2.u02.l05.trade_ten_subtract_ones";
    public const string CountBack="usccss.g2.u02.l05.count_back_to_ten";
    public const string Exchange="usccss.g2.u02.l05.tens_after_exchange";
    public const string Diego="usccss.g2.u02.l05.diego_two_step_subtract";
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? s)=>s is Trade or CountBack or Exchange or Diego;
    public static Problem Build(string family,Random r)
    {
        if(!Supports(family))throw new InvalidOperationException("Wrong G2 L05 question");
        var tens=r.Next(2,10);var ones=r.Next(0,9);var remove=r.Next(ones+1,10);
        var n=tens*10+ones;
        var p=new Dictionary<string,int>(StringComparer.Ordinal){
            ["tens"]=tens,["ones"]=ones,["remove"]=remove,["whole"]=n
        };
        string prompt,solution;
        if(family==Exchange)
        {
            prompt=$"A new Edulytics block model has {n} (={tens} tens and {ones} ones). To subtract {remove} ones by place, exchange one ten for ten ones. How many ONES exist right after trading?";
            solution=$"{n}={(tens-1)} tens+{ones+10} ones before subtracting; answer {ones+10}.";
        }
        else if(family==CountBack)
        {
            prompt=$"Original Edulytics number line: {n}−{remove}. First subtract {ones} ones to reach {tens*10}, then take the remaining {remove-ones}. What is the final difference?";
            solution=$"{n}−{ones}={tens*10}; {tens*10}−{remove-ones}={n-remove}.";
        }
        else if(family==Diego)
        {
            p["firstTens"]=r.Next(1,tens);
            var middle=n-10*p["firstTens"];
            prompt=$"New Diego-style first-party model: begin with {n}; remove {p["firstTens"]} tens; then {remove} ones. How many remain? Break one more ten into singles when necessary.";
            solution=$"{n}−{10*p["firstTens"]}={middle}; {middle}−{remove}={middle-remove}.";
        }
        else
        {
            prompt=$"New Edulytics base-ten subtraction: {n}−{remove}. Trade one ten because {ones} ones are insufficient. Find the difference.";
            solution=$"{n}={(tens-1)} tens+{ones+10} ones; remove {remove} -> {n-remove}.";
        }
        return new Problem(family,prompt,solution,AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid G2 L05 parameters");
        var result=family switch{
            Exchange=>p["ones"]+10,
            Diego=>p["whole"]-10*p["firstTens"]-p["remove"],
            Trade or CountBack=>p["whole"]-p["remove"],
            _=>throw new InvalidOperationException("Wrong family")
        };
        return result.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||!p.TryGetValue("tens",out var tens)||tens is <2 or >9||
           !p.TryGetValue("ones",out var ones)||ones is <0 or >8||
           !p.TryGetValue("remove",out var remove)||remove is <1 or >9||remove<=ones||
           !p.TryGetValue("whole",out var whole)||whole!=10*tens+ones)return false;
        if(family==Diego)
            return p.Count==5&&p.TryGetValue("firstTens",out var first)
                &&first>=1&&first<tens&&whole-10*first-remove>=0;
        return p.Count==4;
    }
}
