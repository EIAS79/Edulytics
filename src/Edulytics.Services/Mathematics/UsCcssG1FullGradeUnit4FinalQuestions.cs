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
        bool tenCardTask=family==Families[1]&&p.TryGetValue("mode",out int proposedMode)&&proposedMode>=3&&proposedMode<=4;
        if(!Supports(family)||(tenCardTask?p.Count!=11:p.Count!=4)||
           !p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
           !p.TryGetValue("c",out int c)||!p.TryGetValue("mode",out int mode)||
           mode<0||mode>(family==Families[1]?4:family==Families[4]?7:3))return false;
        return family switch
        {
            "usccss.g1.u04.l16.write_comparison" or
            "usccss.g1.u04.l18.center_count_compare_order" or
            "usccss.g1.u04.l22.center_place_value" =>
                a>=20&&a<=99&&b>=20&&b<=99&&c>=20&&c<=99,
            "usccss.g1.u04.l17.order_three_numbers"=>
                mode>=3?ValidTenCardInsertion(p)&&
                    (mode!=4||OriginalFixedTenCards(p)):
                a>=20&&a<=99&&b>=20&&b<=99&&c>=20&&c<=99,
            "usccss.g1.u04.l19.regroup_one_ten" or
            "usccss.g1.u04.l20.equivalent_tens_ones" =>
                a>=20&&a<=99&&b==0&&c==0&&
                (family!=Families[3]||((mode!=0||a==65)&&(mode!=1||a==37)))&&
                (family!=Families[4]||(
                    (mode!=2||a==94)&&
                    (mode<4||(mode==4&&a==52)||(mode==5&&a==65)||
                               (mode==6&&a==49)||(mode==7&&a==36)))),
            "usccss.g1.u04.l21.compare_nonstandard_forms" =>
                a>=20&&a<=99&&b>=20&&b<=99&&c==0&&
                (mode!=2||(a==82&&b==72)),
            "usccss.g1.u04.l23.estimate_and_count" =>
                a>=20&&a<=99&&b>=10&&b<=99&&c==0,
            _=>false
        };
    }
    private static bool ValidTenCardInsertion(IReadOnlyDictionary<string,int> p)
    {
        string[] keys=["a","b","c","d","e","f","g","h","i","j"];
        var values=new List<int>();
        foreach(string key in keys)
        {
            if(!p.TryGetValue(key,out int value)||value<1||value>99)return false;
            values.Add(value);
        }
        return values.Distinct().Count()==10&&
               values[0]<values[1]&&values[1]<values[2]&&
               values[2]<values[3]&&values[3]<values[4];
    }

    private static bool OriginalFixedTenCards(IReadOnlyDictionary<string,int> p)=>
        p["a"]==1&&p["b"]==5&&p["c"]==10&&p["d"]==50&&p["e"]==99&&
        p["f"]==49&&p["g"]==8&&p["h"]==25&&p["i"]==98&&p["j"]==13;

    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unreviewed Grade 1 U04 family");
        int mode=random.Next(family==Families[1]?5:family==Families[4]?8:4);
        int a=random.Next(20,100),b=random.Next(20,100),c=random.Next(20,100);
        if(family==Families[3]||family==Families[4])
        {
            b=0;c=0;
            if(family==Families[3]&&mode==0)a=65; // No breaking or building ten-towers.
            if(family==Families[3]&&mode==1)a=37; // Multiple valid tens/ones ways.
            if(family==Families[4]&&mode==2)
                a=94; // Original 2021 Student Facing: ways to show 94.
            if(family==Families[4]&&mode>=4)
                a=mode switch{4=>52,5=>65,6=>49,7=>36,
                    _=>throw new InvalidOperationException("Unknown original bag")};
        }
        if(family==Families[5])
        {
            c=0;
            if(mode==2){a=82;b=72;} // Elena 5 tens +32 ones vs Kiran 7 tens +2.
        }
        if(family==Families[7]){b=Math.Clamp(a+random.Next(-9,10),10,99);c=0;}
        var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",c},{"mode",mode}};
        if(family==Families[1]&&mode==4)
        {
            // Exact 2021 Student Facing fixed five cards + five inserted cards.
            int[] original=[1,5,10,50,99,49,8,25,98,13];
            string[] keys=["a","b","c","d","e","f","g","h","i","j"];
            for(int i=0;i<keys.Length;i++)p[keys[i]]=original[i];
            a=1;b=5;c=10;
        }
        if(family==Families[1]&&mode==3)
        {
            // Original 2021 U04 L17: insert five numbers into a sorted
            // five-card sequence, then explain two placements to a teacher.
            int[] ten=Enumerable.Range(1,99).OrderBy(_=>random.Next()).Take(10).ToArray();
            int[] firstFive=ten.Take(5).Order().ToArray();
            string[] firstKeys=["a","b","c","d","e"];
            string[] addedKeys=["f","g","h","i","j"];
            for(int i=0;i<5;i++)p[firstKeys[i]]=firstFive[i];
            for(int i=0;i<5;i++)p[addedKeys[i]]=ten[i+5];
        }
        if(!Valid(family,p))throw new InvalidOperationException("Invalid Grade 1 tens-and-ones problem");
        string prompt=family switch
        {
            "usccss.g1.u04.l16.write_comparison"=>
                $"Write the TRUE comparison for {a} and {b} using one of >, <, =. Answer SYMBOL=sign.",
            "usccss.g1.u04.l17.order_three_numbers"=>
                mode>=3?
                    $"{(mode==4?"ORIGINAL 2021 STUDENT FACING: ":"GENERALIZED PRACTICE: ")}Five cards start in order: {a}, {b}, {c}, {p["d"]}, {p["e"]}. INSERT these five additional cards correctly: {p["f"]}, {p["g"]}, {p["h"]}, {p["i"]}, {p["j"]}. Write ALL ten numbers from least to greatest as ORDER=n1,n2,n3,n4,n5,n6,n7,n8,n9,n10. Explain why two inserted numbers belong where you placed them.":
                mode%2==0?
                    $"Order {a}, {b}, {c} from LEAST to GREATEST. Write ORDER=number,number,number.":
                    $"Order {a}, {b}, {c} from GREATEST to LEAST. Write ORDER=number,number,number.",
            "usccss.g1.u04.l18.center_count_compare_order"=>
                mode%2==0?$"Center numeric task: order {a}, {b}, {c} from least to greatest. Write ORDER=number,number,number.":
                    $"Center numeric task: compare {a} and {b}. Write SYMBOL=sign.",
            "usccss.g1.u04.l19.regroup_one_ten"=>
                mode==0?
                    "ORIGINAL 2021 G1 U04 L19 Activity 1: create 65 using a physical bag with FIVE existing ten-towers and FIFTEEN loose ones. You may not break existing towers or construct another tower. Write TENS=number;ONES=number to represent the permitted collection. Teacher reviews the actual drawing and physical arrangement.":
                mode==1?
                    "ORIGINAL 2021 G1 U04 L19 Activity 2: represent 37 in at least TWO different mathematically equivalent tens-and-ones ways. Write TENS=number;ONES=number;ALT_TENS=number;ALT_ONES=number. Teacher reviews the pupil's drawings or words.":
                    $"Show {a} in another way: exchange ONE full ten for ten single ones. Write TENS=number;ONES=number.",
            "usccss.g1.u04.l20.equivalent_tens_ones"=>
                mode==4?
                    "ORIGINAL 2021 MYSTERY BAG A: There are FIVE full ten-towers and TWO loose ones. Count the entire collection; write NUMBER=number.":
                mode==5?
                    "ORIGINAL 2021 MYSTERY BAG B: There are FOUR full ten-towers and TWENTY-FIVE loose ones. How many cubes total? Write NUMBER=number. Twenty-five loose ones must not be mistaken for 5 ones.":
                mode==6?
                    "ORIGINAL 2021 MYSTERY BAG C: A bag has 49 cubes, of which 29 are loose ones. How many full TEN-towers are there? Write TENS=number.":
                mode==7?
                    "ORIGINAL 2021 MYSTERY BAG D: A bag has 36 cubes, with TWO full ten-towers already inside. How many loose ONE cubes are in the bag? Write ONES=number.":
                $"Original How Many Ways?: Start with {a/10} ten-rods and {a%10} ones, making {a}. Exchange {Math.Min(a/10,mode+1)} full ten-rods for loose ones; find a SECOND valid regrouping, and calculate how many tens-and-ones regroupings exist when all zero-to-{a/10} complete ten-rods are allowed. Write NUMBER=number;TENS=number;ONES=number;ALT_TENS=number;ALT_ONES=number;WAYS=number. Teacher reviews original drawings and partner explanations.",
            "usccss.g1.u04.l21.compare_nonstandard_forms"=>
                $"Compare {a/10-Math.Min(mode+1,a/10)} tens and {a%10+10*Math.Min(mode+1,a/10)} ones with {b/10} tens and {b%10} ones. Write SYMBOL=sign.",
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
                mode>=3?"ORDER="+string.Join(",",
                    new[]{"a","b","c","d","e","f","g","h","i","j"}
                        .Select(key=>p[key]).Order()):
                    "ORDER="+string.Join(",",mode%2==0?new[]{a,b,c}.Order():new[]{a,b,c}.OrderDescending()),
            "usccss.g1.u04.l18.center_count_compare_order"=>
                mode%2==0?"ORDER="+string.Join(",",new[]{a,b,c}.Order()):$"SYMBOL={Sign(a,b)}",
            "usccss.g1.u04.l19.regroup_one_ten"=>mode==0?
                "TENS=5;ONES=15":
                mode==1?
                "TENS=3;ONES=7;ALT_TENS=2;ALT_ONES=17":
                $"TENS={a/10-1};ONES={a%10+10}",
            "usccss.g1.u04.l20.equivalent_tens_ones"=>mode switch
            {
                4=>"NUMBER=52",
                5=>"NUMBER=65",
                6=>"TENS=2",
                7=>"ONES=16",
                _=>$"NUMBER={a};TENS={a/10};ONES={a%10};ALT_TENS={a/10-Math.Min(a/10,mode+1)};ALT_ONES={a%10+10*Math.Min(a/10,mode+1)};WAYS={a/10+1}"
            },
            "usccss.g1.u04.l21.compare_nonstandard_forms"=>$"SYMBOL={Sign(a,b)}",
            "usccss.g1.u04.l22.center_place_value"=>
                mode%2==0?$"SYMBOL={Sign(a,b)}":$"NUMBER={a}",
            "usccss.g1.u04.l23.estimate_and_count"=>$"COUNT={a};GAP={Math.Abs(a-b)}",
            _=>throw new InvalidOperationException()
        };
    }
    private static string Normalize(string? s)=>string.Concat((s??"").Where(c=>!char.IsWhiteSpace(c))).ToUpperInvariant();
    // Original 2021 "How Many Ways?" permits the student to CHOOSE how
    // many whole tens to exchange. The authored reference answer offers one
    // valid trade, not the only correct nonstandard representation.
    private static bool VerifyStudentChosenTensTrade(IReadOnlyDictionary<string,int> p,string? response)
    {
        var fields=new Dictionary<string,string>(StringComparer.Ordinal);
        var parts=Normalize(response).Split(';');
        if(parts.Length!=6)return false;
        foreach(var part in parts)
        {
            var k=part.IndexOf('=');
            if(k<=0||k==part.Length-1||
               !fields.TryAdd(part[..k],part[(k+1)..]))return false;
        }
        if(fields.Count!=6||
            !fields.TryGetValue("NUMBER",out var number)||
            !fields.TryGetValue("TENS",out var tens)||
            !fields.TryGetValue("ONES",out var ones)||
            !fields.TryGetValue("ALT_TENS",out var alternateTens)||
            !fields.TryGetValue("ALT_ONES",out var alternateOnes)||
            !fields.TryGetValue("WAYS",out var ways)||
            !int.TryParse(number,out var n)||n!=p["a"]||
            !int.TryParse(tens,out var t)||t!=n/10||
            !int.TryParse(ones,out var o)||o!=n%10||
            !int.TryParse(alternateTens,out var altT)||altT<0||altT>=t||
            !int.TryParse(alternateOnes,out var altO)||altO<10||
            !int.TryParse(ways,out var reportedWays)||reportedWays!=t+1)return false;
        return 10*altT+altO==n;
    }

    private static bool VerifyStudent37Ways(string? response)
    {
        var parts=Normalize(response).Split(';');
        if(parts.Length!=4)return false;
        var map=new Dictionary<string,string>(StringComparer.Ordinal);
        foreach(var part in parts)
        {
            int pos=part.IndexOf('=');
            if(pos<=0||pos==part.Length-1||
               !map.TryAdd(part[..pos],part[(pos+1)..]))return false;
        }
        if(map.Count!=4||!map.TryGetValue("TENS",out var tens)||
           !map.TryGetValue("ONES",out var ones)||
           !map.TryGetValue("ALT_TENS",out var altTens)||
           !map.TryGetValue("ALT_ONES",out var altOnes)||
           !int.TryParse(tens,out var t)||t!=3||
           !int.TryParse(ones,out var o)||o!=7||
           !int.TryParse(altTens,out var altT)||altT<0||altT>=3||
           !int.TryParse(altOnes,out var altO)||altO<10)return false;
        return 10*altT+altO==37;
    }

    public static bool Verify(string? family,IReadOnlyDictionary<string,int> p,string? value)=>
        Valid(family,p)&&!string.IsNullOrWhiteSpace(value)&&
        (family==Families[3]&&p["mode"]==1?
            VerifyStudent37Ways(value):
            family==Families[4]&&p["mode"]<=3?VerifyStudentChosenTensTrade(p,value):
            Normalize(Solve(family!,p))==Normalize(value));
}
