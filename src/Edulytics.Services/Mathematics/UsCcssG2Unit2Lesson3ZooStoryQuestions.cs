using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>Edulytics-authored new story numbers with original Grade2 U02 L03
/// four mathematical story structures, no carry/borrow, reconstructable solver.</summary>
internal static class UsCcssG2Unit2Lesson3ZooStoryQuestions
{
    public const string Bus="usccss.g2.u02.l03.zoo_bus_start_unknown";
    public const string Monkey="usccss.g2.u02.l03.zoo_monkey_result_unknown";
    public const string Compare="usccss.g2.u02.l03.zoo_bears_compare_gap";
    public const string Birds="usccss.g2.u02.l03.zoo_birds_part_unknown";
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family is Bus or Monkey or Compare or Birds;
    public static Problem Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Not G2 U02 L03 source structure");
        var p=new Dictionary<string,int>(StringComparer.Ordinal);
        string prompt,solution;
        if(family is Bus or Birds)
        {
            var aTens=rng.Next(1,6);var bTens=rng.Next(1,10-aTens);
            var aOnes=rng.Next(0,9);var bOnes=rng.Next(0,10-aOnes);
            var part=aTens*10+aOnes; var other=bTens*10+bOnes; var total=part+other;
            p["part"]=part;p["other"]=other;p["total"]=total;
            prompt=family==Bus
                ?$"Original Edulytics NEW zoo-bus story: Some children were already on the bus. Then {other} got on, making {total}. How many were on FIRST?"
                :$"Original Edulytics NEW bird story: {other} birds are INSIDE and {total} are in all. How many birds are OUTSIDE?";
            solution=$"{total} − {other} = {part}; check {part} + {other} = {total}.";
        }
        else
        {
            var largerTens=rng.Next(3,10);
            var smallerTens=rng.Next(1,largerTens);
            var smallerOnes=rng.Next(0,9);
            var largerOnes=rng.Next(smallerOnes,10);
            var bigger=largerTens*10+largerOnes; var smaller=smallerTens*10+smallerOnes;
            p["bigger"]=bigger;p["smaller"]=smaller;
            prompt=family==Monkey
                ?$"Original Edulytics NEW monkey-house story: {bigger} students visited; then {smaller} LEFT. How many remain?"
                :$"Original Edulytics NEW compare story: {bigger} visited bears and {smaller} visited lions. How many MORE visited bears?";
            solution=$"{bigger} − {smaller} = {bigger-smaller}, using tens and ones without borrowing.";
        }
        return new Problem(family,prompt,solution,AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> parameters)
    {
        if(!Valid(family,parameters))throw new InvalidOperationException("Invalid source-style zoo parameters");
        var answer=family is Bus or Birds
            ?parameters["part"]
            :parameters["bigger"]-parameters["smaller"];
        return answer.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> parameters,string answer)=>
        Valid(family,parameters)&&string.Equals(answer,Solve(family,parameters),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family))return false;
        if(family is Bus or Birds)
        {
            return p.Count==3&&p.TryGetValue("part",out var a)&&a>=10&&a<=99
                &&p.TryGetValue("other",out var b)&&b>=10&&b<=99
                &&p.TryGetValue("total",out var total)&&total<=99&&total==a+b
                &&(a%10+b%10)<10;
        }
        return p.Count==2&&p.TryGetValue("bigger",out var high)&&high>=30&&high<=99
            &&p.TryGetValue("smaller",out var low)&&low>=10&&low<high
            &&high%10>=low%10&&high/10>low/10;
    }
}
