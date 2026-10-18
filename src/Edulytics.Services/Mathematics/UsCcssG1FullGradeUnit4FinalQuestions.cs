using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// G1 original Unit 4 L16-L23: digital place-value and comparison tasks.
/// Numeric answers never certify center participation, learner drawings,
/// teacher-led PLC discourse, or physically estimated real collections.
/// </summary>
public static class UsCcssG1FullGradeUnit4FinalQuestions
{
    public static readonly IReadOnlyList<string> Families=
    [
        "usccss.g1.u04.l16.write_comparison",
        "usccss.g1.u04.l17.order_three_numbers",
        "usccss.g1.u04.l18.center_count_compare_order",
        "usccss.g1.u04.l19.regroup_one_ten",
        "usccss.g1.u04.l20.equivalent_tens_ones",
        "usccss.g1.u04.l21.compare_nonstandard_forms",
        "usccss.g1.u04.l22.center_place_value",
        "usccss.g1.u04.l23.estimate_and_count"
    ];
    public sealed record Problem(string Family,string Prompt,string Explanation,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family!=null&&Families.Contains(family,StringComparer.Ordinal);
    public static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||p.Count!=4||
           !p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
           !p.TryGetValue("c",out int c)||!p.TryGetValue("mode",out int mode)||
           mode<0||mode>3)return false;
        return family switch
        {
            "usccss.g1.u04.l16.write_comparison" or
            "usccss.g1.u04.l17.order_three_numbers" or
            "usccss.g1.u04.l18.center_count_compare_order" or
            "usccss.g1.u04.l22.center_place_value" =>
                a>=20&&a<=99&&b>=20&&b<=99&&c>=20&&c<=99,
            "usccss.g1.u04.l19.regroup_one_ten" or
            "usccss.g1.u04.l20.equivalent_tens_ones" =>
                a>=20&&a<=99&&b==0&&c==0,
            "usccss.g1.u04.l21.compare_nonstandard_forms" =>
                a>=20&&a<=99&&b>=20&&b<=99&&c==0,
            "usccss.g1.u04.l23.estimate_and_count" =>
                a>=20&&a<=99&&b>=10&&b<=99&&c==0,
            _=>false
        };
    }
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unreviewed Grade 1 U04 family");
        int mode=random.Next(4);
        int a=random.Next(20,100),b=random.Next(20,100),c=random.Next(20,100);
        if(family==Families[3]||family==Families[4]){b=0;c=0;}
        if(family==Families[5])c=0;
        if(family==Families[7]){b=Math.Clamp(a+random.Next(-9,10),10,99);c=0;}
        var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",c},{"mode",mode}};
        if(!Valid(family,p))throw new InvalidOperationException("Invalid Grade 1 tens-and-ones problem");
        string prompt=family switch
        {
            "usccss.g1.u04.l16.write_comparison"=>
                $"Write the TRUE comparison for {a} and {b} using one of >, <, =. Answer SYMBOL=sign.",
            "usccss.g1.u04.l17.order_three_numbers"=>
                mode%2==0?
                    $"Order {a}, {b}, {c} from LEAST to GREATEST. Write ORDER=number,number,number.":
                    $"Order {a}, {b}, {c} from GREATEST to LEAST. Write ORDER=number,number,number.",
            "usccss.g1.u04.l18.center_count_compare_order"=>
                mode%2==0?$"Center numeric task: order {a}, {b}, {c} from least to greatest. Write ORDER=number,number,number.":
                    $"Center numeric task: compare {a} and {b}. Write SYMBOL=sign.",
            "usccss.g1.u04.l19.regroup_one_ten"=>
                $"Make {a} in another way: exchange ONE full ten for ten single ones. Write TENS=number;ONES=number.",
            "usccss.g1.u04.l20.equivalent_tens_ones"=>
                $"A picture shows {a/10-1} ten-rods and {a%10+10} loose ones. State the original two-digit NUMBER. Write NUMBER=number.",
            "usccss.g1.u04.l21.compare_nonstandard_forms"=>
                $"Compare {a/10-1} tens and {a%10+10} ones with {b/10} tens and {b%10} ones. Write SYMBOL=sign.",
            "usccss.g1.u04.l22.center_place_value"=>
                mode%2==0?$"Center numeric task: compare two-digit numbers {a} and {b}. Write SYMBOL=sign.":
                    $"Center numeric task: identify number made of {a/10} tens and {a%10} ones. Write NUMBER=number.",
            "usccss.g1.u04.l23.estimate_and_count"=>
                $"Optional classroom investigation: your provisional estimate was {b}; a picture clearly contains {a/10} FULL tens bundles and {a%10} additional objects. Calculate the exact count and absolute estimation gap. Write COUNT=number;GAP=number. Real estimation behavior requires separate evidence.",
            _=>throw new InvalidOperationException()
        };
        return new(family,prompt,"Compare or regroup actual place-value quantities, not just loose digits. Center turn-taking, student estimation, partner discussion and physical resources require teacher review.",AssessmentItemType.ShortAnswer,p);
    }
    private static string Sign(int a,int b)=>a>b?">":a<b?"<":"=";
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid Grade 1 U04 source parameters");
        int a=p["a"],b=p["b"],c=p["c"],mode=p["mode"];
        return family switch
        {
            "usccss.g1.u04.l16.write_comparison"=>$"SYMBOL={Sign(a,b)}",
            "usccss.g1.u04.l17.order_three_numbers"=>
                "ORDER="+string.Join(",",mode%2==0?new[]{a,b,c}.Order():new[]{a,b,c}.OrderDescending()),
            "usccss.g1.u04.l18.center_count_compare_order"=>
                mode%2==0?"ORDER="+string.Join(",",new[]{a,b,c}.Order()):$"SYMBOL={Sign(a,b)}",
            "usccss.g1.u04.l19.regroup_one_ten"=>$"TENS={a/10-1};ONES={a%10+10}",
            "usccss.g1.u04.l20.equivalent_tens_ones"=>$"NUMBER={a}",
            "usccss.g1.u04.l21.compare_nonstandard_forms"=>$"SYMBOL={Sign(a,b)}",
            "usccss.g1.u04.l22.center_place_value"=>
                mode%2==0?$"SYMBOL={Sign(a,b)}":$"NUMBER={a}",
            "usccss.g1.u04.l23.estimate_and_count"=>$"COUNT={a};GAP={Math.Abs(a-b)}",
            _=>throw new InvalidOperationException()
        };
    }
    private static string Normalize(string? s)=>string.Concat((s??"").Where(c=>!char.IsWhiteSpace(c))).ToUpperInvariant();
    public static bool Verify(string? family,IReadOnlyDictionary<string,int> p,string? value)=>
        Valid(family,p)&&!string.IsNullOrWhiteSpace(value)&&Normalize(Solve(family!,p))==Normalize(value);
}
