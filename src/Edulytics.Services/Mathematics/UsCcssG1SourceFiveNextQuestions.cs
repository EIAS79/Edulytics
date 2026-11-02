using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>V1 G1-009, original IM/Kendall Hunt 2021 G1 U03 lessons 4–8.
/// Each family measures a source student action, never substitutes a broad
/// outcome-generated fact. Open partner arguments remain teacher evaluated.</summary>
public static class UsCcssG1SourceFiveNextQuestions
{
    public const string MakeTen="usccss.g1.u03.l04.make_ten.complement";
    public const string PatternTen="usccss.g1.u03.l04.make_ten.systematic_pattern";
    public const string RelatedDifference="usccss.g1.u03.l05.difference.related_addition";
    public const string SubtractionPattern="usccss.g1.u03.l05.difference.number_string";
    public const string StartUnknown="usccss.g1.u03.l06.story.start_unknown";
    public const string OtherStories="usccss.g1.u03.l06.story.other_unknown_positions";
    public const string CompareCards="usccss.g1.u03.l07.center.compare_cards";
    public const string FindPair="usccss.g1.u03.l07.center.find_pair_to_ten";
    public const string TenUnit="usccss.g1.u03.l08.ten_unit.compose";
    public const string FourTeens="usccss.g1.u03.l08.ten_unit.four_teen_numbers";
    public static readonly IReadOnlyList<string> Families=
        [MakeTen,PatternTen,RelatedDifference,SubtractionPattern,
         StartUnknown,OtherStories,CompareCards,FindPair,TenUnit,FourTeens];
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f is not null&&Families.Contains(f,StringComparer.Ordinal);
    public static string Normalize(string? s)=>string.Concat((s??"").Where(c=>!char.IsWhiteSpace(c))).ToUpperInvariant();
    public static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||p.Count!=4||!p.TryGetValue("a",out var a)||
           !p.TryGetValue("b",out var b)||!p.TryGetValue("c",out var c)||
           !p.TryGetValue("mode",out var mode)||a<0||a>19||b<0||b>19||
           c<0||c>19||mode<0||mode>3)return false;
        return family switch
        {
            MakeTen=>a<=10,
            PatternTen=>a<=8 && mode<=1,
            RelatedDifference=>a is >=4 and <=10 && b>=1 && b<a,
            SubtractionPattern=>a is >=4 and <=10 && b>=1 && b+2<=a,
            StartUnknown=>a is >=3 and <=10 && b>=1 && b<a,
            OtherStories=>a is >=2 and <=7 && b>=1 && a+b<=10,
            CompareCards=>a is >=2 and <=9 && b is >=1 and <=8,
            FindPair=>a is >=0 and <=10,
            TenUnit=>a is >=10 and <=19,
            FourTeens=>mode<=1 && (mode==0?a is >=10 and <=16:a is >=13 and <=19),
            _=>false
        };
    }
    private static string Eq(int x,int y)=>$"{x}+{y}={x+y}";
    public static Problem Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Unreviewed G1-009 family.");
        int a=rng.Next(1,9),b=rng.Next(1,5),c=rng.Next(0,4),mode=rng.Next(4);
        string prompt;
        switch(family)
        {
            case MakeTen:
                a=rng.Next(0,11);
                prompt=$"Shake and Spill: Ten counters are split into {a} red and some yellow. " +
                    "Write the missing yellow part and TWO related addition equations. " +
                    "Format YELLOW=number;FIRST=number+number=10;SECOND=number+number=10.";
                break;
            case PatternTen:
                a=rng.Next(0,9);mode=rng.Next(2);
                prompt=mode==0
                    ?$"ALL WAYS TO MAKE TEN: begin with {a} red counters. Show THREE decompositions as RED increases by 1 each time: {a}, then {a+1}, then {a+2}. Format FIRST=number+number=10;SECOND=number+number=10;THIRD=number+number=10. Explain the pattern."
                    :$"ALL WAYS TO MAKE TEN: begin with {10-a} yellow counters. Show THREE decompositions as YELLOW decreases by 1 each time: {10-a}, then {9-a}, then {8-a}. Write YELLOW FIRST in each equation. Format FIRST=number+number=10;SECOND=number+number=10;THIRD=number+number=10. Explain the pattern.";
                break;
            case RelatedDifference:
                a=rng.Next(4,11);b=rng.Next(1,a);
                prompt=$"Find the difference {a} - {b}. A learner can take away {b}, count on from {b} to {a}, " +
                    $"or use the related addition fact {b}+□={a}. " +
                    "Write DIFF=number;CHECK=number+number=number. Tell a partner which method you used.";
                break;
            case SubtractionPattern:
                a=rng.Next(4,11);b=rng.Next(1,a-1);
                prompt=$"Subtraction number string: {a}-{b}; {a}-{b+1}; {a}-{b+2}. " +
                    "As the number taken away grows by one, how does the difference change? " +
                    "Write FIRST=number;SECOND=number;THIRD=number;PATTERN=DECREASES.";
                break;
            case StartUnknown:
                a=rng.Next(3,11);b=rng.Next(1,a);
                prompt=$"Shake and Spill story: Some counters were in a cup. Then {b} more were added. " +
                    $"Now there are {a} counters. How many counters were there AT THE START? " +
                    "Write START=number;EQUATION=number+number=number. " +
                    "Represent the start with a box; explain how you know.";
                break;
            case OtherStories:
                a=rng.Next(2,8);b=rng.Next(1,11-a);
                prompt=mode switch
                {
                    0=>$"Story: {a} counters started in a cup. More were added, and there are {a+b} now. What is the CHANGE? Write UNKNOWN=number;EQUATION=number+number=number.",
                    1=>$"Story: {a} red and {b} yellow counters spilled. How many counters in TOTAL? Write UNKNOWN=number;EQUATION=number+number=number.",
                    2=>$"Story: {a+b} counters were in the cup and {b} fell out. How many REMAIN? Write UNKNOWN=number;EQUATION=number-number=number.",
                    _=>$"Story: A cup has {a+b} counters. {a} are red. How many are yellow? Write UNKNOWN=number;EQUATION=number+number=number."
                };
                break;
            case CompareCards:
                a=rng.Next(2,10);b=rng.Next(1,9);
                prompt=$"COMPARE cards: the first expression is {a}-1; the second is {b}+1. " +
                    "Evaluate BOTH cards and decide which card wins (FIRST, SECOND, or TIE). " +
                    "Write FIRST=number;SECOND=number;WIN=FIRST or SECOND or TIE.";
                break;
            case FindPair:
                a=rng.Next(0,11);
                prompt=$"Find the Pair center: you turn over a card showing {a}. " +
                    "Which second number card makes a total of TEN? Write PAIR=number;EQUATION=number+number=10. " +
                    "This is a digital mathematical model; physical partner gameplay is teacher facilitated.";
                break;
            case TenUnit:
                a=rng.Next(10,20);
                prompt=$"Ten as ONE unit: you have {a} connecting cubes. Bundle exactly 10 ones into a ten, " +
                    "and leave the other cubes as separate ones. " +
                    "Write TENS=number;ONES=number;TOTAL=number. Ten ones are one ten, not ten tens.";
                break;
            case FourTeens:
                mode=rng.Next(2);
                a=mode==0?rng.Next(10,17):rng.Next(13,20);
                prompt=mode==0
                  ?$"Build FOUR teen numbers with connecting cubes: {a}, {a+1}, {a+2}, {a+3}. Keep ONE group of ten in each and list leftover ones in the shown order. Format ONES=number,number,number,number. Explain what stays the same."
                  :$"Build FOUR teen numbers with connecting cubes: {a}, {a-1}, {a-2}, {a-3}. Keep ONE group of ten in each and list leftover ones in the shown order. Format ONES=number,number,number,number. Explain what stays the same.";
                break;
            default:throw new InvalidOperationException();
        }
        var p=new Dictionary<string,int>(StringComparer.Ordinal)
        {["a"]=a,["b"]=b,["c"]=c,["mode"]=mode};
        if(!Valid(family,p))throw new InvalidOperationException("G1-009 generator invalid parameters.");
        return new Problem(family,prompt,
            "Use counters, number relationships and the meaning of each unknown. Partner explanations remain teacher-reviewed.",
            AssessmentItemType.ShortAnswer,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid original 2021 task.");
        var a=p["a"];var b=p["b"];var mode=p["mode"];
        return family switch
        {
            MakeTen=>$"YELLOW={10-a};FIRST={Eq(a,10-a)};SECOND={Eq(10-a,a)}",
            PatternTen=>mode==0?$"FIRST={Eq(a,10-a)};SECOND={Eq(a+1,9-a)};THIRD={Eq(a+2,8-a)}":$"FIRST={Eq(10-a,a)};SECOND={Eq(9-a,a+1)};THIRD={Eq(8-a,a+2)}",
            RelatedDifference=>$"DIFF={a-b};CHECK={Eq(b,a-b)}",
            SubtractionPattern=>$"FIRST={a-b};SECOND={a-b-1};THIRD={a-b-2};PATTERN=DECREASES",
            StartUnknown=>$"START={a-b};EQUATION={Eq(a-b,b)}",
            OtherStories=>mode switch
            {
                0=>$"UNKNOWN={b};EQUATION={Eq(a,b)}",
                1=>$"UNKNOWN={a+b};EQUATION={Eq(a,b)}",
                2=>$"UNKNOWN={a};EQUATION={a+b}-{b}={a}",
                _=>$"UNKNOWN={b};EQUATION={Eq(a,b)}"
            },
            CompareCards=>$"FIRST={a-1};SECOND={b+1};WIN={(a-1==b+1?"TIE":a-1>b+1?"FIRST":"SECOND")}",
            FindPair=>$"PAIR={10-a};EQUATION={Eq(a,10-a)}",
            TenUnit=>$"TENS=1;ONES={a-10};TOTAL={a}",
            FourTeens=>mode==0?$"ONES={a-10},{a-9},{a-8},{a-7}":$"ONES={a-10},{a-11},{a-12},{a-13}",
            _=>throw new InvalidOperationException()
        };
    }
    // Structured responses accept an equivalent addend order, but NEVER
    // unrelated true equations or omissions of a required part. Use the same
    // comparator for freshly generated and persisted responses.
    private static bool AdditionEquivalent(string expected,string actual)
    {
        var e=System.Text.RegularExpressions.Regex.Match(expected,@"^(\d+)\+(\d+)=(\d+)$");
        var a=System.Text.RegularExpressions.Regex.Match(actual,@"^(\d+)\+(\d+)=(\d+)$");
        if(!e.Success||!a.Success)return false;
        int x=int.Parse(e.Groups[1].Value),y=int.Parse(e.Groups[2].Value),
            z=int.Parse(e.Groups[3].Value),u=int.Parse(a.Groups[1].Value),
            v=int.Parse(a.Groups[2].Value),w=int.Parse(a.Groups[3].Value);
        return x+y==z&&u+v==w&&z==w&&
               ((x==u&&y==v)||(x==v&&y==u));
    }
    private static bool SameAnswer(string family,string expected,string? response)
    {
        if(string.IsNullOrWhiteSpace(response))return false;
        var exp=Normalize(expected);var got=Normalize(response);
        if(exp==got)return true;
        var eParts=exp.Split(';');var aParts=got.Split(';');
        if(eParts.Length!=aParts.Length)return false;
        var e=new Dictionary<string,string>(StringComparer.Ordinal);
        var a=new Dictionary<string,string>(StringComparer.Ordinal);
        foreach(var (parts,map) in new[]{(eParts,e),(aParts,a)})
            foreach(var item in parts)
            {
                int cut=item.IndexOf('=');
                if(cut<1||cut>=item.Length-1||!map.TryAdd(item[..cut],item[(cut+1)..]))
                    return false;
            }
        if(e.Count!=a.Count||e.Keys.Except(a.Keys,StringComparer.Ordinal).Any())return false;
        foreach(var (key,value) in e)
        {
            if(a[key]==value)continue;
            if(family==MakeTen&&key is "FIRST" or "SECOND")
            {
                if(!AdditionEquivalent(value,a[key])&&
                   !AdditionEquivalent(e[key=="FIRST"?"SECOND":"FIRST"],a[key]))
                    return false;
            }
            else if(key is "EQUATION" or "CHECK" && AdditionEquivalent(value,a[key]))
            {
                // Addition may be written in either commutative order.
            }
            else return false;
        }
        if(family==MakeTen&&e["FIRST"]!=e["SECOND"])
        {
            // Both related ordered equations must occur; repeating one
            // identical answer twice is not two related equations.
            if(a["FIRST"]==a["SECOND"])return false;
        }
        return true;
    }
    public static bool Verify(string? family,IReadOnlyDictionary<string,int> p,string? answer)=>
        Valid(family,p)&&SameAnswer(family!,Solve(family!,p),answer);
    public static bool GradePersistedAnswer(string? family,string? expected,string? answer)=>
        Supports(family)&&!string.IsNullOrWhiteSpace(expected)&&
        SameAnswer(family!,expected,answer);
}
