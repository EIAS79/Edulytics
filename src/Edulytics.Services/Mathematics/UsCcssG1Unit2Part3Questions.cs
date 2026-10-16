using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Original IM 2021 G1 U02 L08-L10 learner-facing exact source-goal Practice.
/// Covers a known-total covered addend, switching story unknowns, and
/// Capture Squares sums. Physical games and diagrams require separate UAT.
/// </summary>
internal static class UsCcssG1Unit2Part3Questions
{
    public const string CoveredPart="usccss.g1.u02.shake_cover.addend_unknown";
    public const string StoryUnknown="usccss.g1.u02.story.result_part_or_both_unknown";
    public const string CaptureSquares="usccss.g1.u02.center.capture_squares.sum_to10";
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family) =>
        family is CoveredPart or StoryUnknown or CaptureSquares;

    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))
            throw new InvalidOperationException("Unrecognized G1 U02 L08-L10 source family");
        var p=new Dictionary<string,int>(StringComparer.Ordinal);
        string prompt,solution;
        if(family==CoveredPart)
        {
            var total=random.Next(6,11);
            var visible=random.Next(1,total);
            var side=random.Next(2);
            var color=random.Next(2);
            p["total"]=total;p["visible"]=visible;p["side"]=side;p["color"]=color;
            prompt=$"Shake, Spill, and Cover: There are {total} counters altogether. "+
                $"{visible} {(color==0?"red":"yellow")} counters are visible. "+
                $"How many {(color==0?"yellow":"red")} counters are covered by a cup? "+
                (side==0
                    ?$"Complete {visible} + [ ? ] = {total}. "
                    :$"Complete {total} = {visible} + [ ? ]. ")+
                "Enter ONLY the covered amount, not the total.";
            solution="Show the known visible part on a 10-frame, count on to the known "+
                "total, and put a box around the missing addend. Reverse the equation "+
                "to show the equals sign has the same meaning on either side.";
        }
        else if(family==StoryUnknown)
        {
            var total=random.Next(5,11);
            var part=random.Next(1,total);
            var mode=random.Next(3);
            var flip=random.Next(2);
            p["total"]=total;p["part"]=part;p["mode"]=mode;p["flip"]=flip;
            if(mode==0)
            {
                prompt=$"There are {part} cats and {total-part} birds. "+
                    $"How many pets in all? Write {part} + {total-part} = [ ? ]. "+
                    "Make a labelled drawing and enter the total.";
                solution="Both parts are known. Add them to find the whole. "+
                    "Place the answer box around the total, not a given group.";
            }
            else if(mode==1)
            {
                prompt=$"There are {total} pets in all, including {part} cats. "+
                    $"How many birds? Write {part} + [ ? ] = {total}. "+
                    "Show the two groups and enter the missing group size.";
                solution="The total and one part are known. Count on from the known "+
                    "part to the whole and box the missing part.";
            }
            else
            {
                var a=$"{part} + {total-part} = {total}";
                var b=$"{part} + {total-part+1} = {total}";
                prompt=$"A class has {total} pets in two groups, but neither "+
                    "group size is known. Which possible decomposition is true? "+
                    (flip==0?$"1) {a}; 2) {b}.":$"1) {b}; 2) {a}.")+
                    " Enter 1 or 2, then find another valid pair and explain it.";
                solution="When both parts are unknown, several different pairs "+
                    "can make the same whole. Check the two addends and imagine "+
                    "a different true partition of the total.";
            }
        }
        else
        {
            var a=random.Next(1,7);
            var b=random.Next(1,Math.Min(7,11-a));
            var kind=random.Next(3);
            p["a"]=a;p["b"]=b;p["kind"]=kind;
            var method=new[]{"a 10-frame","counting on","two groups of counters"}[kind];
            prompt=$"Capture Squares: Your two number cubes show {a} and {b}. "+
                $"Use {method} to find their sum. Which number should you "+
                "locate on the game board before drawing an allowed line? "+
                "Enter the sum, then tell a partner how you found it.";
            solution="Add the two cube values (not subtract), point to their "+
                "sum on the game board, and only draw a legal adjacent-dot line "+
                "there. A square is captured only if a line completes it.";
        }
        return new Problem(family,prompt,solution,AssessmentItemType.Numeric,p);
    }

    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))
            throw new InvalidOperationException("Malformed original G1 U02 L08-L10 parameters");
        var v=family switch
        {
            CoveredPart=>p["total"]-p["visible"],
            StoryUnknown=>p["mode"]==0?p["total"]:
                p["mode"]==1?p["total"]-p["part"]:p["flip"]==0?1:2,
            CaptureSquares=>p["a"]+p["b"],
            _=>throw new InvalidOperationException("Unsupported source family")
        };
        return v.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer) =>
        Valid(family,p) && string.Equals(answer,Solve(family,p),StringComparison.Ordinal);

    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family))return false;
        if(family==CoveredPart)
            return p.Count==4 && p.TryGetValue("total",out var total) &&
                p.TryGetValue("visible",out var visible) &&
                p.TryGetValue("side",out var side) &&
                p.TryGetValue("color",out var color) &&
                total is >=6 and <=10 && visible>=1 && visible<total &&
                side is 0 or 1 && color is 0 or 1;
        if(family==StoryUnknown)
            return p.Count==4 && p.TryGetValue("total",out var total) &&
                p.TryGetValue("part",out var part) &&
                p.TryGetValue("mode",out var mode) &&
                p.TryGetValue("flip",out var flip) &&
                total is >=5 and <=10 && part>=1 && part<total &&
                mode is >=0 and <=2 && flip is 0 or 1;
        return p.Count==3 && p.TryGetValue("a",out var a) &&
            p.TryGetValue("b",out var b) &&
            p.TryGetValue("kind",out var kind) &&
            a is >=1 and <=6 && b is >=1 and <=6 &&
            a+b<=10 && kind is >=0 and <=2;
    }
}
