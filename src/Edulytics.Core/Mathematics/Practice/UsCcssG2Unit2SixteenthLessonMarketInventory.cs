using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;
/// <summary>Strict three-product source constraints and first-party arithmetic;
/// human teacher observes true partner interactions and source sales reports.</summary>
public static class UsCcssG2Unit2SixteenthLessonMarketInventory
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U02:L16";
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Observe real Notice and Wonder Han shopping basket picture, do not fabricate image details",
        "Three actual product choices begin at exactly 100 items: one count below 10, other two each above 10",
        "Student independently proves the inventory sum and can subtract to find a missing stock",
        "Observe ONE real cube rolled three times per buyer, actual faces recorded and context-appropriate amounts formed",
        "A seller must verify adequate available stock BEFORE each real sale and update transactions",
        "Optional Sales Reports contain source actual starting, sold, remaining and restock quantities",
        "Teacher evaluates true MLR8 vocabulary and role-play; online example never proves real pair activity"
    ];
    private static bool Number(string? s,int min,int max,out int n)=>
        int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out n)&&n>=min&&n<=max;
    public static Review CheckOriginalInventory(string? a,string? b,string? c)
    {
        var ok=Number(a,0,100,out var i)&&Number(b,0,100,out var j)&&
            Number(c,0,100,out var k)&&i+j+k==100&&
            new[]{i,j,k}.Count(x=>x<10)==1&&new[]{i,j,k}.Count(x=>x>10)==2;
        return new(ok,true,ok?"Exactly 100; one stock count below10 and other two above10. Teacher checks actual product names."
            :"Choose exactly three stocks summing100: ONE strictly below10 and TWO strictly above10.");
    }
    public static Review FirstPartySales(string? startA,string? startB,string? startC,
        string? sellA,string? sellB,string? sellC,string? leftA,string? leftB,string? leftC,
        string? totalSold,string? totalLeft,string? totalRestock)
    {
        var ok=Number(startA,0,100,out var a)&&a==7&&
            Number(startB,0,100,out var b)&&b==42&&Number(startC,0,100,out var c)&&c==51&&
            Number(sellA,0,100,out var sa)&&sa==3&&Number(sellB,0,100,out var sb)&&sb==23&&
            Number(sellC,0,100,out var sc)&&sc==25&&
            Number(leftA,0,100,out var la)&&la==4&&Number(leftB,0,100,out var lb)&&lb==19&&
            Number(leftC,0,100,out var lc)&&lc==26&&
            Number(totalSold,0,100,out var ts)&&ts==51&&
            Number(totalLeft,0,100,out var tl)&&tl==49&&
            Number(totalRestock,0,100,out var tr)&&tr==51;
        return new(ok,false,ok?"First-party market: 100 initially,51 sold,49 left; restock51 to regain100. Not real source sales."
            :"First-party market starts7,42,51; sells3,23,25; left4,19,26; sold51, left49, restock51.");
    }
    public static Review CheckIndependentExit(string? left,string? restock)
    {
        var ok=Number(left,0,100,out var l)&&l==17&&Number(restock,0,100,out var r)&&r==19;
        return new(ok,false,ok?"Original Edulytics problem: started36, sold19, left17; restock19."
            :"Inventory36−19=17. Add exactly19 to return from17 to36.");
    }
}
