using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Independent first-party numerical examples; source actual partner sales not inferred.</summary>
internal static class UsCcssG2Unit2Lesson16MarketInventoryQuestions
{
    public const string Missing="usccss.g2.u02.l16.inventory_missing_category";
    public const string Remaining="usccss.g2.u02.l16.sale_remaining_quantity";
    public const string Restock="usccss.g2.u02.l16.restock_to_original_amount";
    public const string Total="usccss.g2.u02.l16.total_sales_three_categories";
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f is Missing or Remaining or Restock or Total;
    public static Problem Build(string f,Random random)
    {
        if(!Supports(f))throw new InvalidOperationException("Not Grade2 U02 L16 market practice");
        var small=random.Next(0,10);
        var second=random.Next(11,78);
        var third=100-small-second;
        if(third<=10){second=50;third=100-small-second;}
        var saleSmall=random.Next(0,small+1);
        var saleSecond=random.Next(0,second+1);
        var saleThird=random.Next(0,third+1);
        var p=new Dictionary<string,int>(StringComparer.Ordinal){
            ["small"]=small,["second"]=second,["third"]=third,
            ["saleSmall"]=saleSmall,["saleSecond"]=saleSecond,["saleThird"]=saleThird};
        string prompt=f switch {
            Missing=>$"First-party store has100 items. Snacks:{small}, cards:{second}, pencils:?. How many pencils so there are three categories, one under10 and two over10?",
            Remaining=>$"First-party shop has {second} cards. It sells {saleSecond} cards. How many remain?",
            Restock=>$"First-party shop began with {third} pencils and sold {saleThird}. How many pencils must be RESTOCKED to restore the starting count?",
            _=>$"First-party shop had {small},{second},{third} items by category and sold {saleSmall},{saleSecond},{saleThird}. How many items in TOTAL were sold?"
        };
        var value=f switch{Missing=>third,Remaining=>second-saleSecond,Restock=>saleThird,_=>saleSmall+saleSecond+saleThird};
        string result=$"Inventory:{small}+{second}+{third}=100. Sales:{saleSmall}+{saleSecond}+{saleThird}={saleSmall+saleSecond+saleThird}. Remaining equals stock minus sold and restock equals sold.";
        return new Problem(f,prompt,result,AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid three-stock market parameters");
        int result=family switch {
            Missing=>p["third"],Remaining=>p["second"]-p["saleSecond"],
            Restock=>p["saleThird"],Total=>p["saleSmall"]+p["saleSecond"]+p["saleThird"],
            _=>throw new InvalidOperationException("Unrecognised market question")
        };
        return result.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? f,IReadOnlyDictionary<string,int> p)=>
        Supports(f)&&p.Count==6&&p.TryGetValue("small",out var a)&&a is >=0 and <=9&&
        p.TryGetValue("second",out var b)&&b>10&&
        p.TryGetValue("third",out var c)&&c>10&&a+b+c==100&&
        p.TryGetValue("saleSmall",out var sa)&&sa>=0&&sa<=a&&
        p.TryGetValue("saleSecond",out var sb)&&sb>=0&&sb<=b&&
        p.TryGetValue("saleThird",out var sc)&&sc>=0&&sc<=c;
}
