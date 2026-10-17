using System.Globalization;
using System.Text.RegularExpressions;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Original, source-aligned IM K-5 2021 Grade 1 Unit 2 lessons 6–10.
/// One self-paced digital response family per instructional task. The real
/// physical Shake/Spill, partner gallery walk, and board-game center remain
/// teacher-facilitated and are NOT claimed as assessed classroom activity.
/// </summary>
public static class UsCcssG1Unit2NextFiveQuestions
{
    public const string Pets = "usccss.g1.u02.pets.write_two_equations";
    public const string Shake = "usccss.g1.u02.shake_spill.enumerate_partitions";
    public const string Cover = "usccss.g1.u02.shake_cover.write_missing_addend";
    public const string Compare = "usccss.g1.u02.story.compare_unknowns_equations";
    public const string Capture = "usccss.g1.u02.center.capture_squares.select_legal_edge";

    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family) =>
        family is Pets or Shake or Cover or Compare or Capture;

    private static string Compact(string s) =>
        string.Concat(s.Where(ch=>!char.IsWhiteSpace(ch)));
    private static string[] Parts(string s)=>
        s.Split(';',StringSplitOptions.TrimEntries|StringSplitOptions.RemoveEmptyEntries)
            .Select(Compact).ToArray();
    private static string Sum(int a,int b,int t)=>$"{a}+{b}={t}";

    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported G1 unit 2 lesson 6–10 family");
        var p=new Dictionary<string,int>(StringComparer.Ordinal);
        string prompt,solution;
        switch(family)
        {
            case Pets:
            {
                var a=random.Next(2,6);var b=random.Next(2,6);
                if(a==b)b=b==5?4:b+1;
                var t=a+b;
                p["a"]=a;p["b"]=b;
                prompt=$"A pet shelter cares for {a} cats and {b} dogs. There is no joining or leaving action. "+
                    "How many pets are there altogether? WRITE two different addition equations, "+
                    "one beginning with cats and one beginning with dogs. "+
                    "Separate them with a semicolon (example format: a+b=total; b+a=total). "+
                    "Explain to a partner why changing the order does not change the total.";
                solution="Put the two groups together; both orders have the same total.";
                break;
            }
            case Shake:
            {
                var total=random.Next(4,9);
                p["total"]=total;
                prompt=$"Shake and Spill: there are exactly {total} two-color counters. "+
                    "List EVERY different way to split them into two color groups without "+
                    "counting the reversed colors twice. Include zero counters of one color. "+
                    "WRITE each addition equation separated by semicolons. "+
                    $"For example, for 3 counters: 0+3=3; 1+2=3. Find all combinations for {total}.";
                solution="Start with 0 of the first color and increase by 1 until the first group meets the second; no partition may be missing or repeated.";
                break;
            }
            case Cover:
            {
                var total=random.Next(6,11);
                var visible=random.Next(2,total-1);
                p["total"]=total;p["visible"]=visible;
                prompt=$"Shake, Spill, and Cover: There are {total} counters in all; "+
                    $"{visible} are visible and the others are hidden under a cup. "+
                    "Count the visible counters and think how many are under the cup. "+
                    "WRITE the completed addition equation AND the related subtraction equation, "+
                    "separated by a semicolon (visible+hidden=total; total-visible=hidden).";
                solution="Count on from what is visible to the whole; subtraction checks the hidden part.";
                break;
            }
            case Compare:
            {
                var total=random.Next(6,11);
                var a=random.Next(2,total-1); var mode=random.Next(3);
                if(mode==0 && a==total-a)a--;
                var b=total-a;
                p["total"]=total;p["a"]=a;p["mode"]=mode;
                prompt=mode switch
                {
                    0=>$"Story A: {a} reptiles are turtles and {b} are lizards. How many reptiles in all? "+
                       "WRITE two addition equations in opposite orders for this total-unknown story.",
                    1=>$"Story B: A class has {total} reptiles altogether, {a} are turtles, and the rest are lizards. "+
                       "How many lizards? WRITE a completed addition equation AND a related subtraction equation.",
                    _=>$"Story C: A class has {total} reptiles. We do not know how many are turtles or lizards. "+
                       "WRITE TWO DIFFERENT positive-number addition equations showing two possible splits; "+
                       "each equation must equal the total. Do not repeat a split with the addends reversed."
                };
                prompt+=" Separate the two equations with a semicolon and identify what is unknown in each story.";
                solution="Total unknown: combine both groups; one part unknown: find the missing addend; both parts unknown: more than one decomposition can be valid.";
                break;
            }
            case Capture:
            {
                var target=random.Next(6,10);
                var first=random.Next(Math.Max(1,target-5),Math.Min(5,target-1)+1);
                var second=target-first;
                p["first"]=first;p["second"]=second;
                prompt=$"Capture Squares center: Roll two number cubes: {first} and {second}. "+
                    "Find their sum. On the FIRST-PARTY 2-by-2 game board, "+
                    "top-left A1 has 6, top-right A2 has 7, bottom-left B1 has 8, bottom-right B2 has 9. "+
                    "Choose the cell with the correct sum and ONE side to mark on that cell. "+
                    "WRITE a cell-and-side move such as A1:TOP, A2:RIGHT, B1:BOTTOM or B2:LEFT. "+
                    "An edge represents one legal turn on an initially empty board; capturing a square "+
                    "requires later turns with three existing sides, played with a teacher or partner.";
                solution="Add the two number cubes, find the matching square, then name one side to draw on the empty board. No square is captured after a single edge.";
                break;
            }
            default:throw new InvalidOperationException("Unknown family");
        }
        return new Problem(family,prompt,solution,AssessmentItemType.ShortAnswer,p);
    }

    public static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family))return false;
        bool Has(string k,out int v)=>p.TryGetValue(k,out v);
        return family switch
        {
            Pets =>p.Count==2&&Has("a",out var a)&&Has("b",out var b) &&
                a is >=2 and <=5 && b is >=2 and <=5 && a!=b && a+b<=10,
            Shake=>p.Count==1&&Has("total",out var total)&&total is >=4 and <=8,
            Cover=>p.Count==2&&Has("total",out var whole)&&Has("visible",out var visible) &&
                whole is >=6 and <=10 && visible>=2 && visible<whole,
            Compare=>p.Count==3&&Has("total",out var n)&&Has("a",out var part)&&Has("mode",out var mode) &&
                n is >=6 and <=10 && part>=2 && part<n && mode is >=0 and <=2,
            Capture=>p.Count==2&&Has("first",out var first)&&Has("second",out var second) &&
                first is >=1 and <=5 && second is >=1 and <=5 &&
                first+second is >=6 and <=9,
            _=>false
        };
    }

    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid G1 U02 L06-L10 source parameters");
        return family switch
        {
            Pets=>Sum(p["a"],p["b"],p["a"]+p["b"])+";"+
                Sum(p["b"],p["a"],p["a"]+p["b"]),
            Shake=>string.Join(";",Enumerable.Range(0,p["total"]/2+1)
                .Select(a=>Sum(a,p["total"]-a,p["total"]))),
            Cover=>Sum(p["visible"],p["total"]-p["visible"],p["total"])+";"+
                $"{p["total"]}-{p["visible"]}={p["total"]-p["visible"]}",
            Compare=>p["mode"] switch
            {
                0=>Sum(p["a"],p["total"]-p["a"],p["total"])+";"+
                    Sum(p["total"]-p["a"],p["a"],p["total"]),
                1=>Sum(p["a"],p["total"]-p["a"],p["total"])+";"+
                    $"{p["total"]}-{p["a"]}={p["total"]-p["a"]}",
                _=>Sum(1,p["total"]-1,p["total"])+";"+Sum(2,p["total"]-2,p["total"])
            },
            Capture=>$"{((p["first"]+p["second"]) switch {6=>"A1",7=>"A2",8=>"B1",_=>"B2"})}:TOP",
            _=>throw new InvalidOperationException()
        };
    }

    private static bool TwoEquationSet(string expected,string submitted)
    {
        var a=Parts(expected); var b=Parts(submitted);
        return a.Length==2&&b.Length==2 &&
            a.Distinct(StringComparer.Ordinal).Count()==2 &&
            b.Distinct(StringComparer.Ordinal).Count()==2 &&
            a.Order(StringComparer.Ordinal).SequenceEqual(b.Order(StringComparer.Ordinal));
    }

    private static bool EveryPartition(string expected,string submitted)
    {
        var canonical=Parts(expected);
        var actual=Parts(submitted);
        if(actual.Length!=canonical.Length)return false;
        var total=int.Parse(canonical[0].Split('=')[1],CultureInfo.InvariantCulture);
        var sets=new HashSet<string>(StringComparer.Ordinal);
        foreach(var piece in actual)
        {
            var m=Regex.Match(piece,@"^(\d+)\+(\d+)=(\d+)$");
            if(!m.Success)return false;
            var a=int.Parse(m.Groups[1].Value); var b=int.Parse(m.Groups[2].Value);
                var t=int.Parse(m.Groups[3].Value);
            if(a+b!=total || t!=total)return false;
            sets.Add($"{Math.Min(a,b)}+{Math.Max(a,b)}={total}");
        }
        return sets.Count==canonical.Length &&
            canonical.All(x=>sets.Contains(x));
    }

    private static bool TwoDifferentDecompositions(string expected,string submitted)
    {
        var answers=Parts(submitted); var baseParts=Parts(expected);
        if(baseParts.Length!=2 || answers.Length!=2)return false;
        var total=int.Parse(baseParts[0].Split('=')[1],CultureInfo.InvariantCulture);
        var pairs=new HashSet<string>(StringComparer.Ordinal);
        foreach(var expression in answers)
        {
            var m=Regex.Match(expression,@"^(\d+)\+(\d+)=(\d+)$");
            if(!m.Success)return false;
            var a=int.Parse(m.Groups[1].Value); var b=int.Parse(m.Groups[2].Value);
                var t=int.Parse(m.Groups[3].Value);
            if(a<1 || b<1 || a+b!=total || t!=total)return false;
            pairs.Add($"{Math.Min(a,b)},{Math.Max(a,b)}");
        }
        return pairs.Count==2;
    }

    public static bool GradePersistedAnswer(string? family,string expected,string? response)
    {
        if(!Supports(family)||response is null)return false;
        if(family==Capture)
        {
            var actual=Regex.Match(Compact(response).ToUpperInvariant(),
                @"^(A1|A2|B1|B2):(TOP|RIGHT|BOTTOM|LEFT)$");
            if(!actual.Success)return false;
            var ex=Regex.Match(expected,@"^(A1|A2|B1|B2):TOP$");
            return ex.Success &&
                actual.Groups[1].Value==ex.Groups[1].Value;
        }
        if(family==Shake)return EveryPartition(expected,response);
        if(family==Compare && expected.StartsWith("1+",StringComparison.Ordinal))
            return TwoDifferentDecompositions(expected,response);
        return TwoEquationSet(expected,response);
    }

    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string response)=>
        Valid(family,p) && GradePersistedAnswer(family,Solve(family,p),response);
}
