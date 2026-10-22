using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>32 distinct arithmetic structures, source-authentic context but NEW first-party values only.</summary>
internal static class UsCcssG2Unit5ComparisonQuestions
{
    private const string Prefix="usccss.g2.u05.l";
    private static readonly IReadOnlySet<string> Families=new HashSet<string>(StringComparer.Ordinal){
        "usccss.g2.u05.l07.compare_100_boolean",
        "usccss.g2.u05.l07.mystery_htoval",
        "usccss.g2.u05.l07.mystery_tens_digit",
        "usccss.g2.u05.l07.centers_sum_to_100",
        "usccss.g2.u05.l08.scale_count_by_tens",
        "usccss.g2.u05.l08.scale_count_by_hundreds",
        "usccss.g2.u05.l08.number_line_coordinate",
        "usccss.g2.u05.l08.window_interval_count",
        "usccss.g2.u05.l09.further_right_number",
        "usccss.g2.u05.l09.numeric_comparison_gap",
        "usccss.g2.u05.l09.place_value_tens_compare",
        "usccss.g2.u05.l09.open_estimation_interval",
        "usccss.g2.u05.l10.mental_add_tens",
        "usccss.g2.u05.l10.compare_hundreds_first",
        "usccss.g2.u05.l10.base_ten_equivalence",
        "usccss.g2.u05.l10.greater_base_ten_quantity",
        "usccss.g2.u05.l11.less_greater_numeric",
        "usccss.g2.u05.l11.inequality_gap",
        "usccss.g2.u05.l11.largest_card_permutation",
        "usccss.g2.u05.l11.smallest_card_permutation",
        "usccss.g2.u05.l12.lowest_three_digit",
        "usccss.g2.u05.l12.highest_three_digit",
        "usccss.g2.u05.l12.next_in_sorted_order",
        "usccss.g2.u05.l12.distance_between_ranked",
        "usccss.g2.u05.l13.picture_unit_value",
        "usccss.g2.u05.l13.make_three_digit_cards",
        "usccss.g2.u05.l13.valid_order_slot",
        "usccss.g2.u05.l13.missing_to_target",
        "usccss.g2.u05.l14.groups_of_five_count",
        "usccss.g2.u05.l14.groups_of_ten_count",
        "usccss.g2.u05.l14.hundreds_tens_remainder",
        "usccss.g2.u05.l14.fictional_object_total"
    };
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f!=null&&Families.Contains(f);
    private static int Lesson(string f)=>int.Parse(f.Substring(Prefix.Length,2),CultureInfo.InvariantCulture);
    private static string Kind(string f)=>f[(Prefix.Length+3)..];
    private static int Value(int h,int t,int o)=>100*h+10*t+o;
    public static Problem Build(string family,Random r)
    {
        if(!Supports(family))throw new InvalidOperationException("Unknown U5 source aligned family");
        int h=r.Next(1,10),t=r.Next(0,10),o=r.Next(0,10),a=Value(h,t,o);
        int h2=r.Next(1,10),t2=r.Next(0,10),o2=r.Next(0,10),b=Value(h2,t2,o2);
        int first=Math.Min(a,b),last=Math.Max(a,b),d1=r.Next(1,10),d2=r.Next(0,10),d3=r.Next(0,10);
        int ten=r.Next(1,10)*10,groups=r.Next(1,20),extra=r.Next(0,10),n=r.Next(100,901);
        int step=new[]{1,10,100}[r.Next(3)],zero=r.Next(0,4),width=r.Next(10,50);
        var p=new Dictionary<string,int>(StringComparer.Ordinal){
            ["lesson"]=Lesson(family),["h"]=h,["t"]=t,["o"]=o,
            ["h2"]=h2,["t2"]=t2,["o2"]=o2,["d1"]=d1,["d2"]=d2,["d3"]=d3,
            ["ten"]=ten,["groups"]=groups,["extra"]=extra,
            ["n"]=n,["step"]=step,["zero"]=zero,["width"]=width
        };
        var kind=Kind(family);int answer;string prompt;
        switch(kind)
        {
            case "compare_100_boolean":
                answer=a>100?1:0;prompt=$"FIRST-PARTY: Enter1 if numeral {a} is greater than100, otherwise0.";break;
            case "mystery_htoval":
                answer=a;prompt=$"FICTIONAL source-style mystery number with {h} hundreds,{t} tens,{o} ones. What 3-digit number?";break;
            case "mystery_tens_digit":
                answer=t;prompt=$"FICTIONAL mystery number {a}. What digit is in the TENS place?";break;
            case "centers_sum_to_100":
                answer=100-extra;prompt=$"FICTIONAL addition-to100 puzzle: {extra}+?=100. What missing number?";break;
            case "scale_count_by_tens":
                answer=ten+10;prompt=$"FICTIONAL 0..100 number line labels in tens: after{ten}, what multiple of10 comes next?";break;
            case "scale_count_by_hundreds":
                answer=h*100;prompt=$"FICTIONAL 0..1000 line in steps100: after {h} equal hundred-spaces from0, what is the number?";break;
            case "number_line_coordinate":
                answer=a;prompt=$"FIRST-PARTY physical-scale-neutral position at numeral{a}: what value labels the dot if zero-based coordinates are correct?";break;
            case "window_interval_count":
                answer=10;prompt=$"FIRST-PARTY line from{zero*100} to{zero*100+10*step} inclusive at equal step{step}. How many INTERVALS between the endpoints?";break;
            case "further_right_number":
                answer=last;prompt=$"FIRST-PARTY number line shows two points at{a} and{b}. What VALUE is further RIGHT?";break;
            case "numeric_comparison_gap":
                answer=last-first;prompt=$"FIRST-PARTY scale: how many 1-unit distances between{first} and{last}?";break;
            case "place_value_tens_compare":
                answer=t>t2?1:0;prompt=$"FIRST-PARTY compare digits in the TENS place: first numeral {a}, second numeral {b}. Enter1 if FIRST tens digit is GREATER, otherwise0.";break;
            case "open_estimation_interval":
                answer=width;prompt=$"FIRST-PARTY interval from{zero*100} to{zero*100+width}. What is the numeric width? Not an exact ORIGINAL estimation dot.";break;
            case "mental_add_tens":
                answer=a+ten;prompt=$"FIRST-PARTY mental place-value addition: {a}+{ten}. What total?";break;
            case "compare_hundreds_first":
                answer=h>h2?1:0;prompt=$"FIRST-PARTY numbers {a} and{b}. Enter1 if FIRST number has MORE hundreds, otherwise0.";break;
            case "base_ten_equivalence":
                answer=a;prompt=$"FIRST-PARTY equivalent blocks: {h-1} hundreds, {t+10} tens,{o} ones. What TOTAL numeral after normalizing?";break;
            case "greater_base_ten_quantity":
                answer=last;prompt=$"FIRST-PARTY compare two block values {a} and{b}. Which quantity is greater?";break;
            case "less_greater_numeric":
                answer=a>b?1:0;prompt=$"FIRST-PARTY: Enter1 if {a}>{b}, otherwise0.";break;
            case "inequality_gap":
                answer=last-first;prompt=$"FIRST-PARTY compare {first} and{last}. How many more ones does greater value have?";break;
            case "largest_card_permutation":
                var high=new[]{d1,d2,d3}.OrderByDescending(v=>v).ToArray();
                answer=100*high[0]+10*high[1]+high[2];
                prompt=$"FICTIONAL number cards {d1},{d2},{d3}. Largest possible THREE-digit numeral? Cards are not source actual turn.";break;
            case "smallest_card_permutation":
                var low=new[]{d1,d2,d3}.OrderBy(v=>v).ToList();
                var nonzero=low.FindIndex(v=>v>0);
                if(nonzero>0){(low[0],low[nonzero])=(low[nonzero],low[0]);}
                answer=100*low[0]+10*low[1]+low[2];
                prompt=$"FICTIONAL 3 digit cards {d1},{d2},{d3}. Smallest possible THREE-digit number, no leading0?";break;
            case "lowest_three_digit":
                answer=first;prompt=$"FIRST-PARTY pair {a},{b}. What is the LOWER value for ascending order?";break;
            case "highest_three_digit":
                answer=last;prompt=$"FIRST-PARTY pair {a},{b}. What is the HIGHER value for descending order?";break;
            case "next_in_sorted_order":
                answer=last;prompt=$"FIRST-PARTY two numbers are{first} then{last} in ASCENDING order. What is last?";break;
            case "distance_between_ranked":
                answer=last-first;prompt=$"FIRST-PARTY equally spaced number line: what is the distance from{first} to{last}?";break;
            case "picture_unit_value":
                answer=a;prompt=$"FICTIONAL base-ten sketch: {h} hundreds,{t} tens,{o} ones. What numerical value?";break;
            case "make_three_digit_cards":
                answer=Value(d1,d2,d3);prompt=$"FICTIONAL chosen digit cards {d1},{d2},{d3} in hundreds/tens/ones order. What number?";break;
            case "valid_order_slot":
                answer=a>b?1:0;prompt=$"FICTIONAL ordered board has LEFT value{b}, candidate{a}. Enter1 if candidate can go immediately RIGHT of left value, otherwise0.";break;
            case "missing_to_target":
                answer=1000-n;prompt=$"FIRST-PARTY counting target1000: already{n} items. How many more to reach exactly1000?";break;
            case "groups_of_five_count":
                answer=groups*5+extra;prompt=$"FICTIONAL bean groups: {groups} groups of5 and {extra} loose beans. How many TOTAL beans? Not source class collection.";break;
            case "groups_of_ten_count":
                answer=groups*10+extra;prompt=$"FICTIONAL counter: {groups} groups of10 and {extra} beans ungrouped. How many total?";break;
            case "hundreds_tens_remainder":
                answer=a;prompt=$"FIRST-PARTY three-digit collection: {h} hundreds,{t} tens,{o} ones. How many objects?";break;
            case "fictional_object_total":
                answer=n;prompt=$"FICTIONAL paper says there are {n} objects. Expressed as total ones, what is the count? No original pupil sticky note asserted.";break;
            default:throw new InvalidOperationException("Unknown first-party math subtype");
        }
        return new(family,prompt,$"Verified independently authored answer {answer}; publisher diagram, games and counts need teacher.",AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid first-party Grade2 U05 structure");
        int h=p["h"],t=p["t"],o=p["o"],h2=p["h2"],t2=p["t2"],o2=p["o2"];
        int a=Value(h,t,o),b=Value(h2,t2,o2),first=Math.Min(a,b),last=Math.Max(a,b),
           d1=p["d1"],d2=p["d2"],d3=p["d3"],groups=p["groups"],extra=p["extra"];
        int[] low=[d1,d2,d3];Array.Sort(low);
        if(low[0]==0){var swap=Array.FindIndex(low,x=>x>0);if(swap>0){(low[0],low[swap])=(low[swap],low[0]);}}
        int[] high=[d1,d2,d3];Array.Sort(high);Array.Reverse(high);
        int value=Kind(family) switch{
            "compare_100_boolean"=>a>100?1:0,
            "mystery_htoval" or "number_line_coordinate" or "base_ten_equivalence" or "picture_unit_value" or "hundreds_tens_remainder"=>a,
            "mystery_tens_digit"=>t,
            "centers_sum_to_100"=>100-extra,
            "scale_count_by_tens"=>p["ten"]+10,
            "scale_count_by_hundreds"=>h*100,
            "window_interval_count"=>10,
            "further_right_number" or "greater_base_ten_quantity" or "highest_three_digit" or "next_in_sorted_order"=>last,
            "numeric_comparison_gap" or "inequality_gap" or "distance_between_ranked"=>last-first,
            "place_value_tens_compare"=>t>t2?1:0,
            "open_estimation_interval"=>p["width"],
            "mental_add_tens"=>a+p["ten"],
            "compare_hundreds_first"=>h>h2?1:0,
            "less_greater_numeric" or "valid_order_slot"=>a>b?1:0,
            "largest_card_permutation"=>100*high[0]+10*high[1]+high[2],
            "smallest_card_permutation"=>100*low[0]+10*low[1]+low[2],
            "lowest_three_digit"=>first,
            "make_three_digit_cards"=>Value(d1,d2,d3),
            "missing_to_target"=>1000-p["n"],
            "groups_of_five_count"=>groups*5+extra,
            "groups_of_ten_count"=>groups*10+extra,
            "fictional_object_total"=>p["n"],
            _=>throw new InvalidOperationException("Unrecognized numeric family")
        };
        return value.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||p.Count!=17||
           !p.TryGetValue("lesson",out var l)||l!=Lesson(family)||
           !p.TryGetValue("h",out var h)||h<1||h>9||
           !p.TryGetValue("t",out var t)||t<0||t>9||
           !p.TryGetValue("o",out var o)||o<0||o>9||
           !p.TryGetValue("h2",out var h2)||h2<1||h2>9||
           !p.TryGetValue("t2",out var t2)||t2<0||t2>9||
           !p.TryGetValue("o2",out var o2)||o2<0||o2>9||
           !p.TryGetValue("d1",out var d1)||d1<1||d1>9||
           !p.TryGetValue("d2",out var d2)||d2<0||d2>9||
           !p.TryGetValue("d3",out var d3)||d3<0||d3>9||
           !p.TryGetValue("ten",out var ten)||ten<10||ten>90||ten%10!=0||
           !p.TryGetValue("groups",out var groups)||groups<1||groups>19||
           !p.TryGetValue("extra",out var extra)||extra<0||extra>9||
           !p.TryGetValue("n",out var n)||n<100||n>900||
           !p.TryGetValue("step",out var step)||step is not (1 or 10 or 100)||
           !p.TryGetValue("zero",out var z)||z<0||z>3||
           !p.TryGetValue("width",out var width)||width<10||width>49)
            return false;
        return true;
    }
}
