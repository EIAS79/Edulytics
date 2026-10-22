using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Exactly recomputed first-party Grade2 Unit5 mathematical families, no image-only source answer invention.</summary>
internal static class UsCcssG2Unit5FirstSixQuestions
{
    private const string Prefix="usccss.g2.u05.l";
    private static readonly IReadOnlySet<string> Known=new HashSet<string>(StringComparer.Ordinal){
        "usccss.g2.u05.l01.count_cross_hundred",
        "usccss.g2.u05.l01.base_ten_exchange",
        "usccss.g2.u05.l01.compose_one_hundred",
        "usccss.g2.u05.l01.hundred_plus_remainder",
        "usccss.g2.u05.l02.ten_rods_for_number",
        "usccss.g2.u05.l02.complete_missing_tens",
        "usccss.g2.u05.l02.hundreds_from_tens",
        "usccss.g2.u05.l02.count_tens_to_300",
        "usccss.g2.u05.l03.noncanonical_total",
        "usccss.g2.u05.l03.normalize_hundreds",
        "usccss.g2.u05.l03.normalize_tens",
        "usccss.g2.u05.l03.normalize_ones",
        "usccss.g2.u05.l04.three_digit_riddle",
        "usccss.g2.u05.l04.zero_tens_placeholder",
        "usccss.g2.u05.l04.reordered_units_value",
        "usccss.g2.u05.l04.digit_place_value",
        "usccss.g2.u05.l05.expanded_form_total",
        "usccss.g2.u05.l05.hundreds_place_component",
        "usccss.g2.u05.l05.max_digit_arrangement",
        "usccss.g2.u05.l05.min_digit_arrangement",
        "usccss.g2.u05.l06.name_to_three_digit",
        "usccss.g2.u05.l06.worded_place_value",
        "usccss.g2.u05.l06.expanded_to_number",
        "usccss.g2.u05.l06.name_unit_component"
    };
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family!=null&&Known.Contains(family);
    private static int Lesson(string family)=>int.Parse(family.Substring(Prefix.Length,2),CultureInfo.InvariantCulture);
    private static string Kind(string family)=>family[(Prefix.Length+3)..];
    private static string[] UnderTwenty=["zero","one","two","three","four","five","six","seven","eight","nine",
         "ten","eleven","twelve","thirteen","fourteen","fifteen","sixteen","seventeen","eighteen","nineteen"];
    private static string[] TensNames=["","","twenty","thirty","forty","fifty","sixty","seventy","eighty","ninety"];
    private static string Word(int n)
    {
        if(n<20)return UnderTwenty[n];
        if(n<100)return TensNames[n/10]+(n%10==0?"":"-"+UnderTwenty[n%10]);
        return UnderTwenty[n/100]+" hundred"+(n%100==0?"":" "+Word(n%100));
    }
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unknown G2 U05 independent exact family");
        int lesson=Lesson(family);
        int h=random.Next(1,7),t=random.Next(0,10),o=random.Next(0,10);
        int legacyT=random.Next(11,29),legacyO=random.Next(10,20);
        int value=100*h+10*t+o,noncanonical=100+10*legacyT+legacyO;
        int sourceHundreds=random.Next(1,8),hundredTens=random.Next(1,10),counting=random.Next(90,120);
        int missingHundredTens=random.Next(0,sourceHundreds*10+1),small=random.Next(1,100);
        int d1=random.Next(1,7),d2=random.Next(1,7),d3=random.Next(1,7);
        var p=new Dictionary<string,int>(StringComparer.Ordinal){
            ["lesson"]=lesson,["h"]=h,["t"]=t,["o"]=o,
            ["legacyT"]=legacyT,["legacyO"]=legacyO,
            ["sourceHundreds"]=sourceHundreds,["hundredTens"]=hundredTens,
            ["counting"]=counting,["missingHundredTens"]=missingHundredTens,["small"]=small,
            ["d1"]=d1,["d2"]=d2,["d3"]=d3
        };
        string prompt;int answer;
        switch(Kind(family))
        {
            case "count_cross_hundred":
                answer=counting+1;prompt=$"First-party Grade2 count by ONE from {counting}. Which integer comes next?";break;
            case "base_ten_exchange":
                answer=10*legacyT+legacyO;prompt=$"First-party blocks: {legacyT} TEN-rods and {legacyO} ones. How many total ones?";break;
            case "compose_one_hundred":
                answer=100;prompt=$"First-party classroom illustration: {hundredTens} complete tens rods, and {100-10*hundredTens} individual ones. What is the total?";break;
            case "hundred_plus_remainder":
                answer=100+small;prompt=$"First-party place value: 1 hundred plus {small} ones. What number is represented?";break;
            case "ten_rods_for_number":
                answer=10*sourceHundreds;prompt=$"First-party collection of {sourceHundreds} hundreds. How many TEN-rods have this total value?";break;
            case "complete_missing_tens":
                answer=sourceHundreds*10-missingHundredTens;
                prompt=$"First-party goal {100*sourceHundreds}: already {missingHundredTens} ten-rods, with NO hundred blocks. How many MORE ten-rods needed?";break;
            case "hundreds_from_tens":
                answer=sourceHundreds;prompt=$"First-party regroup {10*sourceHundreds} ten-rods. How many hundreds are equal in value?";break;
            case "count_tens_to_300":
                answer=10*(sourceHundreds+1);prompt=$"First-party tens count: you already counted {10*sourceHundreds} ten-rods; add TEN more rods. How many rods now?";break;
            case "noncanonical_total":
                answer=noncanonical;prompt=$"First-party loose blocks:1 hundred,{legacyT} tens,{legacyO} ones. Find total VALUE.";break;
            case "normalize_hundreds":
                answer=noncanonical/100;prompt=$"Normalize first-party 1 hundred,{legacyT} tens,{legacyO} ones into the FEWEST base-ten blocks. How many hundreds?";break;
            case "normalize_tens":
                answer=(noncanonical/10)%10;prompt=$"Normalize first-party 1 hundred,{legacyT} tens,{legacyO} ones. How many tens remain after exchanges?";break;
            case "normalize_ones":
                answer=noncanonical%10;prompt=$"Normalize first-party 1 hundred,{legacyT} tens,{legacyO} ones. How many ones remain?";break;
            case "three_digit_riddle":
                answer=value;prompt=$"Original Edulytics new riddle: {h} hundreds, {t} tens,{o} ones. Write the three-digit NUMBER.";break;
            case "zero_tens_placeholder":
                answer=100*h+o;prompt=$"First-party riddle with ZERO tens: {h} hundreds and {o} ones. Write the integer including its empty tens PLACE.";break;
            case "reordered_units_value":
                answer=value;prompt=$"First-party ADDITION of units given out of order: {o} ones+{h} hundreds+{t} tens. What numeral?";break;
            case "digit_place_value":
                answer=100*h;prompt=$"First-party numeral {value}: what VALUE does its hundreds-place digit represent?";break;
            case "expanded_form_total":
                answer=value;prompt=$"Independently authored expanded form: {100*h}+{10*t}+{o}. What numeral?";break;
            case "hundreds_place_component":
                answer=100*h;prompt=$"First-party expanded form of {value}: how much is the HUNDREDS component?";break;
            case "max_digit_arrangement":
                var high=new[]{d1,d2,d3}.OrderByDescending(x=>x).ToArray();
                answer=100*high[0]+10*high[1]+high[2];
                prompt=$"First-party hypothetical 3 number cubes show {d1},{d2},{d3}. Largest THREE-digit numeral from exactly these digits? NOT real publisher dice.";break;
            case "min_digit_arrangement":
                var low=new[]{d1,d2,d3}.OrderBy(x=>x).ToArray();
                answer=100*low[0]+10*low[1]+low[2];
                prompt=$"First-party hypothetical cubes {d1},{d2},{d3}; smallest THREE-digit numeral from these digits? Not source actual rolls.";break;
            case "name_to_three_digit":
                answer=value;prompt=$"First-party English name '{Word(value)}': which THREE-digit numeral matches this name?";break;
            case "worded_place_value":
                answer=value;prompt=$"First-party words: '{Word(value)}' represents how many ones altogether?";break;
            case "expanded_to_number":
                answer=value;prompt=$"First-party words and numerals: written as {100*h}+{10*t}+{o}. What THREE-digit number is this?";break;
            case "name_unit_component":
                answer=10*t;prompt=$"First-party number name '{Word(value)}'. How many ones does the TENS DIGIT represent?";break;
            default:throw new InvalidOperationException("Unrecognized U05 first-party family");
        }
        return new(family,prompt,$"Exact first-party computed number {answer}; no publisher diagram, word composition or pupil dice outcome certified.",AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("First-party parameter validation failed");
        int h=p["h"],t=p["t"],o=p["o"],hundreds=p["sourceHundreds"];
        int n=100*h+10*t+o,unc=100+10*p["legacyT"]+p["legacyO"];
        var sorted=new[]{p["d1"],p["d2"],p["d3"]};
        int v=Kind(family) switch{
            "count_cross_hundred"=>p["counting"]+1,
            "base_ten_exchange"=>10*p["legacyT"]+p["legacyO"],
            "compose_one_hundred"=>100,
            "hundred_plus_remainder"=>100+p["small"],
            "ten_rods_for_number"=>10*hundreds,
            "complete_missing_tens"=>10*hundreds-p["missingHundredTens"],
            "hundreds_from_tens"=>hundreds,
            "count_tens_to_300"=>10*(hundreds+1),
            "noncanonical_total"=>unc,
            "normalize_hundreds"=>unc/100,
            "normalize_tens"=>(unc/10)%10,
            "normalize_ones"=>unc%10,
            "three_digit_riddle" or "reordered_units_value" or "expanded_form_total" or "name_to_three_digit" or "worded_place_value" or "expanded_to_number"=>n,
            "zero_tens_placeholder"=>100*h+o,
            "digit_place_value" or "hundreds_place_component"=>100*h,
            "min_digit_arrangement"=>sorted.OrderBy(x=>x).Select((digit,i)=>digit*(i==0?100:i==1?10:1)).Sum(),
            "max_digit_arrangement"=>sorted.OrderByDescending(x=>x).Select((digit,i)=>digit*(i==0?100:i==1?10:1)).Sum(),
            "name_unit_component"=>10*t,
            _=>throw new InvalidOperationException("Unknown numeric skill family")
        };
        return v.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? a)=>
        Valid(family,p)&&string.Equals(a,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||p.Count!=14||
           !p.TryGetValue("lesson",out var n)||n!=Lesson(family)||
           !p.TryGetValue("h",out var h)||h<1||h>6||
           !p.TryGetValue("t",out var t)||t<0||t>9||
           !p.TryGetValue("o",out var o)||o<0||o>9||
           !p.TryGetValue("legacyT",out var lt)||lt<11||lt>28||
           !p.TryGetValue("legacyO",out var lo)||lo<10||lo>19||
           !p.TryGetValue("sourceHundreds",out var hd)||hd<1||hd>7||
           !p.TryGetValue("hundredTens",out var ht)||ht<1||ht>9||
           !p.TryGetValue("counting",out var count)||count<90||count>=120||
           !p.TryGetValue("missingHundredTens",out var miss)||miss<0||miss>hd*10||
           !p.TryGetValue("small",out var small)||small<1||small>99||
           !p.TryGetValue("d1",out var d1)||d1<1||d1>6||
           !p.TryGetValue("d2",out var d2)||d2<1||d2>6||
           !p.TryGetValue("d3",out var d3)||d3<1||d3>6) return false;
        return true;
    }
}
