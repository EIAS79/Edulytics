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
    public const string Mental = "usccss.g1.u02.center.mental_subtraction_within10";

    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family) =>
        family is Pets or Shake or Cover or Compare or Capture or Mental;

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
                var total=random.Next(1,11);
                p["total"]=total;
                prompt=$"Shake and Spill: there are exactly {total} two-color counters. "+
                     "Write at least TWO DIFFERENT red-and-yellow arrangements, each with a matching full addition equation. "+
                    "Write the number of RED counters first and YELLOW counters second (red+yellow=total), "+
                    "separating your equations with semicolons. "+
                    $"For example, for 3 counters: 1+2=3; 2+1=3. If you have time, find ALL combinations for {total}.";
                solution="Each arrangement needs two colored parts whose total matches the whole. Changing the color counts can reverse the addends; a complete list is an optional challenge.";
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
                    "WRITE a complete addition equation with the missing number filled in OR "+
                    "a correct subtraction equation for the SAME whole and visible part. "+
                    "Either strategy is valid. You may write both separated by a semicolon.";
                solution="Count on from what is visible to the whole, or subtract the visible from the total. Both are valid strategies; write a matching equation.";
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
                       "WRITE a complete matching addition equation (two different orders are optional).",
                    1=>$"Story B: A class has {total} reptiles altogether, {a} are turtles, and the rest are lizards. "+
                       "How many lizards? WRITE a completed addition equation OR a related subtraction equation; both are optional.",
                    _=>$"Story C: A class has {total} reptiles. We do not know how many are turtles or lizards. "+
                       "WRITE TWO DIFFERENT positive-number addition equations showing two possible splits; "+
                       "each equation must equal the total. Do not repeat a split with the addends reversed."
                };
                prompt+=" Separate any equations with a semicolon. At the end write "+
                    "UNKNOWN=TOTAL, UNKNOWN=PART or UNKNOWN=BOTH to identify what the story asks for. "+
                    "Use a box around the unknown when you draw the matching equation on paper.";
                solution="Total unknown: combine both groups; one part unknown: find the missing addend; both parts unknown: more than one decomposition can be valid.";
                break;
            }
            case Mental:
            {
                var start=random.Next(4,11);
                var amount=random.Next(1,3);
                p["start"]=start;p["amount"]=amount;
                prompt=$"Number Talk warm-up: Solve {start} - {amount} mentally. "+
                    "Count back, use the count sequence or a related addition fact. "+
                    "WRITE the missing difference. Show your counting strategy to your teacher or partner.";
                solution="Count back one or two steps and check with addition.";
                break;
            }
            case Capture:
            {
                var target=random.Next(6,10);
                var first=random.Next(Math.Max(1,target-5),Math.Min(5,target-1)+1);
                var second=target-first;
                var scenario=random.Next(2);
                int mask=scenario==0?0:new[]{7,11,13,14}[random.Next(4)];
                p["first"]=first;p["second"]=second;p["mask"]=mask;
                prompt=$"Capture Squares Stage 1: Roll two number cubes: {first} and {second}. "+
                    "On this FIRST-PARTY practice board, A1 is 6, A2 is 7, B1 is 8 and B2 is 9. "+
                    (mask==0
                        ?"The board has no marked sides yet. Find your sum and start by marking the TOP side of the matching square. "
                        :"Three sides of the matching square are ALREADY marked (shown with thick edges). Find the ONLY remaining side and close it. ")+
                    "WRITE CELL:SIDE:RESULT, where RESULT is CONTINUE if no square is captured or CAPTURE when the fourth side closes the square. "+
                    "For example, A1:TOP:CONTINUE. A full two-player Capture Squares game still requires a teacher or partner.";
                solution="Add the dice and locate the matching sum. Choose the required unused side; closing a fourth side captures the square. A first side does not.";
                break;
            }
            default:throw new InvalidOperationException("Unknown family");
        }
        return new Problem(family,prompt,solution,
            family==Mental?AssessmentItemType.Numeric:AssessmentItemType.ShortAnswer,p);
    }

    public static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family))return false;
        bool Has(string k,out int v)=>p.TryGetValue(k,out v);
        return family switch
        {
            Pets =>p.Count==2&&Has("a",out var a)&&Has("b",out var b) &&
                a is >=2 and <=5 && b is >=2 and <=5 && a!=b && a+b<=10,
            Shake=>p.Count==1&&Has("total",out var total)&&total is >=1 and <=10,
            Cover=>p.Count==2&&Has("total",out var whole)&&Has("visible",out var visible) &&
                whole is >=6 and <=10 && visible>=2 && visible<whole,
            Compare=>p.Count==3&&Has("total",out var n)&&Has("a",out var part)&&Has("mode",out var mode) &&
                n is >=6 and <=10 && part>=2 && part<n && mode is >=0 and <=2,
            Mental=>p.Count==2 && Has("start",out var number) &&
                Has("amount",out var taken) && number is >=4 and <=10 &&
                taken is >=1 and <=2 && taken<number,
            Capture=>p.Count==3&&Has("first",out var first)&&Has("second",out var second) &&
                Has("mask",out var mask) && (mask==0 || mask is 7 or 11 or 13 or 14) &&
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
            Shake=>string.Join(";",Enumerable.Range(0,p["total"]==1?2:p["total"]/2+1)
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
            } + ";UNKNOWN=" + (p["mode"] switch {0=>"TOTAL",1=>"PART",_=>"BOTH"}),
            Mental=>(p["start"]-p["amount"]).ToString(CultureInfo.InvariantCulture),
            Capture=>$"{((p["first"]+p["second"]) switch {6=>"A1",7=>"A2",8=>"B1",_=>"B2"})}:{(p["mask"] switch {7=>"LEFT",11=>"BOTTOM",13=>"RIGHT",_=>"TOP"})}:{(p["mask"]==0?"CONTINUE":"CAPTURE")}",
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

    private static bool DistinctColorPartitions(string expected,string submitted)
    {
        var canonical=Parts(expected);
        var actual=Parts(submitted);
        if(actual.Length<2 || actual.Length>12)return false;
        var total=int.Parse(canonical[0].Split('=')[1],CultureInfo.InvariantCulture);
        var sets=new HashSet<string>(StringComparer.Ordinal);
        foreach(var piece in actual)
        {
            var m=Regex.Match(piece,@"^(\d+)\+(\d+)=(\d+)$");
            if(!m.Success)return false;
            var a=int.Parse(m.Groups[1].Value);var b=int.Parse(m.Groups[2].Value);
            var t=int.Parse(m.Groups[3].Value);
            if(a+b!=total || t!=total)return false;
            if(!sets.Add($"{a}+{b}={total}"))return false;
        }
        return sets.Count>=2;
    }

    private static bool EitherMissingAddendEquation(string expected,string submitted)
    {
        var source=Parts(expected);
        if(source.Length!=2)return false;
        var m=Regex.Match(source[0],@"^(\d+)\+(\d+)=(\d+)$");
        if(!m.Success)return false;
        var visible=int.Parse(m.Groups[1].Value);
        var hidden=int.Parse(m.Groups[2].Value);
        var total=int.Parse(m.Groups[3].Value);
        var allowed=new HashSet<string>(StringComparer.Ordinal)
        {
            $"{visible}+{hidden}={total}",$"{hidden}+{visible}={total}",
            $"{total}-{visible}={hidden}"
        };
        var equations=Parts(submitted);
        return equations.Length is >=1 and <=2 &&
            equations.Distinct(StringComparer.Ordinal).Count()==equations.Length &&
            equations.All(allowed.Contains);
    }

    private static bool CompareUnknowns(string expected,string submitted)
    {
        var original=Parts(expected);var entries=Parts(submitted);
        if(original.Length!=3 || entries.Length<2)return false;
        var label=original[2];
        if(entries[^1]!=label)return false;
        var equations=entries[..^1];
        if(label=="UNKNOWN=BOTH")
            return TwoDifferentDecompositions(
                string.Join(";",original.Take(2)),string.Join(";",equations));
        return equations.Length is >=1 and <=2 &&
            equations.Distinct(StringComparer.Ordinal).Count()==equations.Length &&
            equations.All(e=>original.Take(2).Contains(e,StringComparer.Ordinal));
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
        if(family==Mental)return string.Equals(Compact(response),expected,StringComparison.Ordinal);
        if(family==Capture)
        {
            var actual=Regex.Match(Compact(response).ToUpperInvariant(),
                @"^(A1|A2|B1|B2):(TOP|RIGHT|BOTTOM|LEFT):(CONTINUE|CAPTURE)$");
            return actual.Success && Compact(response).Equals(expected,StringComparison.OrdinalIgnoreCase);
        }
        if(family==Shake)return DistinctColorPartitions(expected,response);
        if(family==Cover)return EitherMissingAddendEquation(expected,response);
        if(family==Compare)return CompareUnknowns(expected,response);
        return TwoEquationSet(expected,response);
    }

    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string response)=>
        Valid(family,p) && GradePersistedAnswer(family,Solve(family,p),response);
}
