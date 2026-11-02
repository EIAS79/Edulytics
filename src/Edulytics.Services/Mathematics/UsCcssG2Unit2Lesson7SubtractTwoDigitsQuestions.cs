using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Random original arithmetic without inventing source game-card values.</summary>
internal static class UsCcssG2Unit2Lesson7SubtractTwoDigitsQuestions
{
    public const string Trade="usccss.g2.u02.l07.subtract_two_digits_trade";
    public const string Exchange="usccss.g2.u02.l07.trade_ten_ones_count";
    public const string Inverse="usccss.g2.u02.l07.check_inverse_addition";
    public const string FirstParty="usccss.g2.u02.l07.firstparty_two_digit_subtraction";
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f is Trade or Exchange or Inverse or FirstParty;
    public static Problem Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Not G2 U02 L07 exact math");
        int upperTens=rng.Next(3,10),lowerTens=rng.Next(1,upperTens);
        int upperOnes=rng.Next(0,9),lowerOnes=rng.Next(upperOnes+1,10);
        int upper=10*upperTens+upperOnes,lower=10*lowerTens+lowerOnes;
        var p=new Dictionary<string,int>(StringComparer.Ordinal){
            ["upper"]=upper,["lower"]=lower,["upperTens"]=upperTens,
            ["lowerTens"]=lowerTens,["upperOnes"]=upperOnes,["lowerOnes"]=lowerOnes};
        string prompt=family switch {
            Exchange=>$"Original Edulytics practice: show {upper} as {upperTens} tens+{upperOnes} ones. Trade one ten to subtract {lower}. How many ONES after trading?",
            Inverse=>$"First-party inverse check: {upper}−{lower}=? Find the missing number in {lower}+?={upper}.",
            _=>$"First-party two-digit subtraction (NOT an original game card): {upper}−{lower}=? Exchange one ten when subtracting by place."};
        string explain=$"{upper}={(upperTens-1)} tens+{upperOnes+10} ones; {upper}−{lower}={upper-lower}.";
        return new Problem(family,prompt,explain,AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid two-digit exchange question");
        return (family==Exchange?p["upperOnes"]+10:p["upper"]-p["lower"])
            .ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)=>
        Supports(family)&&p.Count==6&&p.TryGetValue("upper",out var upper)&&
        p.TryGetValue("lower",out var lower)&&p.TryGetValue("upperTens",out var ut)&&
        ut is >=3 and <=9&&p.TryGetValue("lowerTens",out var lt)&&lt>=1&&lt<ut&&
        p.TryGetValue("upperOnes",out var uo)&&uo is >=0 and <=8&&
        p.TryGetValue("lowerOnes",out var lo)&&lo is >=1 and <=9&&lo>uo&&
        upper==10*ut+uo&&lower==10*lt+lo&&upper>lower;
}
