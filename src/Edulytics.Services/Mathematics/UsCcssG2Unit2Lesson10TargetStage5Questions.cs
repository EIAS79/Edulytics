using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>First-party dice arithmetic: three independent cube slots,
/// choose TWO DISTINCT positions; not real game participation.</summary>
internal static class UsCcssG2Unit2Lesson10TargetStage5Questions
{
    public const string Make="usccss.g2.u02.l10.dice_make_two_digit";
    public const string Borrow="usccss.g2.u02.l10.dice_subtract_decompose";
    public const string NoBorrow="usccss.g2.u02.l10.dice_subtract_no_decompose";
    public const string Remainder="usccss.g2.u02.l10.dice_valid_remainder";
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f is Make or Borrow or NoBorrow or Remainder;
    public static Problem Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Not Grade2 Stage5 dice family");
        var current=family==Borrow?rng.Next(71,100):family==NoBorrow?rng.Next(75,100):rng.Next(70,101);
        int ones=current%10,tenFace,oneFace;
        if(family==Borrow)
        {
            if(ones==9)current--; // make the maximum chosen ones face valid
            ones=current%10;
            if(ones>=6){current-=ones-5;ones=5;}
            oneFace=rng.Next(ones+1,7);
            tenFace=rng.Next(1,7);
        }
        else if(family==NoBorrow)
        {
            if(ones==0)current++;
            ones=current%10;
            oneFace=rng.Next(1,Math.Min(ones,6)+1);
            tenFace=rng.Next(1,7);
        }
        else {tenFace=rng.Next(1,7);oneFace=rng.Next(1,7);}
        var third=rng.Next(1,7);
        // Slot 0 is the tens cube; slot1 is the ones cube; both physically distinct.
        var amount=10*tenFace+oneFace;
        var p=new Dictionary<string,int>(StringComparer.Ordinal){
            ["current"]=current,["die0"]=tenFace,["die1"]=oneFace,["die2"]=third,
            ["tensIndex"]=0,["onesIndex"]=1,["amount"]=amount};
        var prompt=family==Make
            ?$"FIRST-PARTY demonstration: three cubes show {tenFace}, {oneFace}, {third}. Choose first cube as TENS and second as ONES. What two-digit number did you create?"
            :$"FIRST-PARTY Target Numbers Stage5 numerical turn: current {current}, three rolled cube faces {tenFace}, {oneFace}, {third}. Select FIRST cube for tens and SECOND for ones (different cubes), subtract {amount}. What remainder is left? "+
             (family==Borrow?"Trade a ten if needed.":family==NoBorrow?"Ones can be taken directly.":"Do not go below zero.");
        var solution=$"Two distinct cubes form {amount}; {current}−{amount}={current-amount}.";
        return new Problem(family,prompt,solution,AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid 3-cube numeric question");
        var result=family==Make?p["amount"]:p["current"]-p["amount"];
        return result.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||p.Count!=7||
            !p.TryGetValue("current",out var current)||current is <70 or >100||
            !p.TryGetValue("die0",out var d0)||d0 is <1 or >6||
            !p.TryGetValue("die1",out var d1)||d1 is <1 or >6||
            !p.TryGetValue("die2",out var d2)||d2 is <1 or >6||
            !p.TryGetValue("tensIndex",out var ti)||ti is <0 or >2||
            !p.TryGetValue("onesIndex",out var oi)||oi is <0 or >2||ti==oi||
            !p.TryGetValue("amount",out var amount))return false;
        var d=new[]{d0,d1,d2};
        if(amount!=10*d[ti]+d[oi]||amount>current)return false;
        if(family==Borrow)return current%10<d[oi];
        if(family==NoBorrow)return current%10>=d[oi];
        return true;
    }
}
