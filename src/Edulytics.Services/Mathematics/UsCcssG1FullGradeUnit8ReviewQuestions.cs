using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Original G1 U08 L01-L10 digital calculation and interpretation subsets.
/// Student-created riddles, oral PLC and hands-on 120-count activities require
/// separate human evidence; auto-correct answers DO NOT certify authorship.
/// </summary>
public static class UsCcssG1FullGradeUnit8ReviewQuestions
{
    public static readonly IReadOnlyList<string> Families =
    [
        "usccss.g1.u08.l01.addition_fluency_within10",
        "usccss.g1.u08.l02.inverse_add_subtract",
        "usccss.g1.u08.l03.fluency_within20",
        "usccss.g1.u08.l04.change_unknown_story",
        "usccss.g1.u08.l05.missing_part_story",
        "usccss.g1.u08.l06.compare_difference_story",
        "usccss.g1.u08.l07.count_on_to120",
        "usccss.g1.u08.l08.same_number_tens_ones",
        "usccss.g1.u08.l09.solve_number_riddle",
        "usccss.g1.u08.l10.write_riddle_numeric_check"
    ];
    public sealed record Problem(string Family,string Prompt,string Explanation,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    private static int Index(string? family)
    {
        if(family==null)return -1;
        for(int i=0;i<Families.Count;i++)
            if(Families[i]==family)return i;
        return -1;
    }
    public static bool Supports(string? family)=>Index(family)>=0;
    public static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        int n=Index(family);
        if(n<0||p.Count!=4||
           !p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
           !p.TryGetValue("c",out int c)||!p.TryGetValue("mode",out int mode)||
           mode<0||mode>3)return false;
        return n switch
        {
            0=>a>=1&&a<=9&&b>=1&&b<=9&&a+b<=10&&c==0,
            1=>a>=1&&a<=9&&b>=1&&b<=9&&a+b<=10&&c==0,
            2=>a>=2&&a<=19&&b>=1&&b<=9&&a+b<=20&&a>=b&&c==0,
            3=>a>=2&&a<=19&&b>=1&&b<=9&&a+b<=20&&c==0,
            4=>a>=2&&a<=19&&b>=1&&b<=9&&a+b<=20&&c==0,
            5=>a>=7&&a<=20&&b>=1&&b<a&&c==0,
            6=>a>=1&&a<=119&&b>=1&&b<=120-a&&c==0,
            7=>a>=20&&a<=99&&b==0&&c==0,
            8=>a>=20&&a<=99&&b==10&&c==0,
            9=>a>=20&&a<=99&&b==10&&c==0,
            _=>false
        };
    }
    public static Problem Build(string family,Random random)
    {
        int n=Index(family);
        if(n<0)throw new InvalidOperationException("Unreviewed Grade1 U08 family");
        int mode=random.Next(4),a=0,b=0,c=0;
        for(int attempts=0;attempts<1000;attempts++)
        {
            if(n<=1){a=random.Next(1,10);b=random.Next(1,10);}
            else if(n<=4){a=random.Next(2,20);b=random.Next(1,10);}
            else if(n==5){a=random.Next(7,21);b=random.Next(1,a);}
            else if(n==6){a=random.Next(1,120);b=random.Next(1,Math.Min(6,121-a));}
            else{a=random.Next(20,100);b=n>=8?10:0;}
            var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",c},{"mode",mode}};
            if(!Valid(family,p))continue;
            var whole=a+b;
            string prompt=n switch
            {
                0=>$"Find the sum within ten: {a}+{b}. Write RESULT=number.",
                1=>$"The two parts are {a} and {b}, with whole {whole}. If {a}+{b}={whole}, remove the {b} part from the whole. Write LEFT=number;WHOLE=number.",
                2=>mode%2==0?$"Add within 20: {a}+{b}. Write RESULT=number.":
                    $"Subtract within 20: {a}-{b}. Write RESULT=number.",
                3=>mode%2==0?$"There are {a} counters and some JOIN. Now there are {whole}. How many joined? Write CHANGE=number;FINAL=number.":
                    $"There are {whole} counters. Some LEAVE. Now {a} remain. How many left? Write CHANGE=number;FINAL=number.",
                4=>$"There are {whole} objects total; one part has {a} objects. How many are in the other part? Write MISSING=number;TOTAL=number.",
                5=>$"There are {a} blue cubes and {b} red cubes. How many MORE blue cubes than red cubes? Write DIFFERENCE=number.",
                6=>$"Start counting at {a} (NOT at one); count ON by {b} steps, remaining at most 120. Write LAST=number.",
                7=>$"Make {a} in two ways. Exchange ONE ten-rod for ten ones. Write NEW_TENS=number;NEW_ONES=number;TOTAL=number.",
                8=>$"Number riddle: The secret two-digit number is 10 LESS than {a+10}, and has {a/10} tens and {a%10} ones. Write NUMBER=number.",
                9=>$"Review a hypothetical student-written number riddle (not proof of authorship): the secret is {a}, has {a/10} tens and {a%10} ones, and is ten less than {a+10}. Verify its numerical clues. Write NUMBER=number;VALID=YES.",
                _=>throw new InvalidOperationException()
            };
            return new(family,prompt,"Use first-grade addition, difference, grouping and place value reasoning. Actual student-authored riddles, drawings and PLC explanations require independent teacher evidence.",AssessmentItemType.ShortAnswer,p);
        }
        throw new InvalidOperationException("Could not sample original-scope Grade 1 review problem");
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid original-scope G1 U08 instance");
        int n=Index(family),a=p["a"],b=p["b"],mode=p["mode"];
        return n switch
        {
            0=>$"RESULT={a+b}",
            1=>$"LEFT={a};WHOLE={a+b}",
            2=>$"RESULT={(mode%2==0?a+b:a-b)}",
            3=>mode%2==0?$"CHANGE={b};FINAL={a+b}":$"CHANGE={b};FINAL={a}",
            4=>$"MISSING={b};TOTAL={a+b}",
            5=>$"DIFFERENCE={a-b}",
            6=>$"LAST={a+b}",
            7=>$"NEW_TENS={a/10-1};NEW_ONES={a%10+10};TOTAL={a}",
            8=>$"NUMBER={a}",
            9=>$"NUMBER={a};VALID=YES",
            _=>throw new InvalidOperationException()
        };
    }
    private static string Normalize(string? s)=>
        string.Concat((s??"").Where(c=>!char.IsWhiteSpace(c))).ToUpperInvariant();
    public static bool Verify(string? family,IReadOnlyDictionary<string,int> p,string? response)=>
        Valid(family,p)&&!string.IsNullOrWhiteSpace(response)&&
        Normalize(Solve(family!,p))==Normalize(response);
}
