using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>First-party exact variants of source subtraction method structure.</summary>
internal static class UsCcssG2Unit2Lesson8DifferentWaysQuestions
{
    public const string TensFirst="usccss.g2.u02.l08.tens_first_then_exchange";
    public const string TradeFirst="usccss.g2.u02.l08.exchange_first_then_tens";
    public const string Residual="usccss.g2.u02.l08.count_residual_after_tens";
    public const string Inverse="usccss.g2.u02.l08.inverse_check_two_digit";
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f is TensFirst or TradeFirst or Residual or Inverse;
    public static Problem Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Not G2 U02 L08 exact family");
        int topTens=rng.Next(3,10),bottomTens=rng.Next(1,topTens),topOnes=rng.Next(0,9),bottomOnes=rng.Next(topOnes+1,10);
        int top=topTens*10+topOnes,bottom=bottomTens*10+bottomOnes;
        var p=new Dictionary<string,int>(StringComparer.Ordinal){
            ["top"]=top,["bottom"]=bottom,["topTens"]=topTens,["bottomTens"]=bottomTens,
            ["topOnes"]=topOnes,["bottomOnes"]=bottomOnes};
        string prompt=family switch{
            TensFirst=>$"Original Edulytics Clare-style tens-FIRST subtraction: {top}−{bottom}. Subtract {10*bottomTens} first, trade ten in the remainder, then remove {bottomOnes} ones. What is the FINAL difference?",
            TradeFirst=>$"Original Edulytics Lin-style TRADE-FIRST subtraction: {top}−{bottom}. Decompose a ten first, then remove ones and tens. What is the FINAL difference?",
            Residual=>$"Before exchanging, {top}−{10*bottomTens} subtracts TENS first. What is the intermediate remaining number?",
            _=>$"Check an original Edulytics regrouping: {top}−{bottom}=? Enter the missing part in {bottom}+?={top}."
        };
        return new Problem(family,prompt,$"{top}−{bottom}={top-bottom}. Tens first leaves {top-10*bottomTens}. One ten can be exchanged to subtract {bottomOnes} ones.",AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid G2 U02 L08 exact parameters");
        int value=family==Residual?p["top"]-10*p["bottomTens"]:p["top"]-p["bottom"];
        return value.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)=>
        Supports(family)&&p.Count==6&&p.TryGetValue("top",out var a)&&p.TryGetValue("bottom",out var b)&&
        p.TryGetValue("topTens",out var at)&&at is >=3 and <=9&&p.TryGetValue("bottomTens",out var bt)&&bt>=1&&bt<at&&
        p.TryGetValue("topOnes",out var ao)&&ao is >=0 and <=8&&p.TryGetValue("bottomOnes",out var bo)&&bo>ao&&bo<=9&&
        a==10*at+ao&&b==10*bt+bo&&a>b;
}
