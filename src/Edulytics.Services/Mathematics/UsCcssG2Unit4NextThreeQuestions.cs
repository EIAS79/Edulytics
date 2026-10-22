using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>12 separately authored exact families (Grade2 2.4.4-2.4.6). No original pupil rolls/images/board points reconstructed.</summary>
internal static class UsCcssG2Unit4NextThreeQuestions
{
    private const string Prefix="usccss.g2.u04.l";
    private static readonly IReadOnlySet<string> Known=new HashSet<string>(StringComparer.Ordinal){
        "usccss.g2.u04.l04.three_cube_sum",
        "usccss.g2.u04.l04.greater_three_cube_sum",
        "usccss.g2.u04.l04.two_cube_two_digit",
        "usccss.g2.u04.l04.two_digit_distance",
        "usccss.g2.u04.l05.new_midpoint",
        "usccss.g2.u04.l05.possible_between_50_55",
        "usccss.g2.u04.l05.possible_between_20_30",
        "usccss.g2.u04.l05.distance_to_benchmark",
        "usccss.g2.u04.l06.decompose_subtraction",
        "usccss.g2.u04.l06.fictional_spinner_move",
        "usccss.g2.u04.l06.exact_end_capture",
        "usccss.g2.u04.l06.to_five_cubes"
    };
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family is not null&&Known.Contains(family);
    private static string Kind(string family)=>family[(Prefix.Length+3)..];
    private static int Lesson(string family)=>int.Parse(family.Substring(Prefix.Length,2),CultureInfo.InvariantCulture);
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unknown first-party number line family");
        int lesson=Lesson(family),a=random.Next(1,7),b=random.Next(1,7),c=random.Next(1,7),
            d=random.Next(1,7),e=random.Next(1,7),f=random.Next(1,7);
        int low=random.Next(1,39),high=random.Next(low+1,41),point=random.Next(low,high+1);
        int start=random.Next(40,91),sub=random.Next(11,Math.Min(39,start)+1);
        int collected=random.Next(0,5),spin=new[]{2,5,10}[random.Next(3)],end=20;
        int marker=random.Next(0,end+1);
        // Unused values remain canonical so Verify rejects mutated inputs consistently.
        var p=new Dictionary<string,int>(StringComparer.Ordinal){
            ["lesson"]=lesson,["a"]=a,["b"]=b,["c"]=c,["d"]=d,["e"]=e,["f"]=f,
            ["low"]=low,["high"]=high,["point"]=point,["start"]=start,["sub"]=sub,
            ["collected"]=collected,["spin"]=spin,["end"]=end,["marker"]=marker
        };
        string prompt;int value;
        switch(Kind(family))
        {
            case "three_cube_sum":
                prompt=$"FICTIONAL standard number cubes show {a}, {b}, {c}. What is the sum to plot on an equal-unit number line0..20?";
                value=a+b+c;break;
            case "greater_three_cube_sum":
                prompt=$"FICTIONAL Partner A rolls {a},{b},{c} and Partner B rolls {d},{e},{f}. What is the GREATER total? Equal totals are permitted.";
                value=Math.Max(a+b+c,d+e+f);break;
            case "two_cube_two_digit":
                prompt=$"FICTIONAL two dice show {a} then {b}; placing the first at the tens position, what TWO-DIGIT number is created?";
                value=10*a+b;break;
            case "two_digit_distance":
                prompt=$"FICTIONAL two-digit pair {a}{b} and {c}{d}, placed on a 0..100 equal-unit number line: how many unit intervals BETWEEN them?";
                value=Math.Abs((10*a+b)-(10*c+d));break;
            case "new_midpoint":
                prompt="EDULYTICS-created line from0 to40. What number lies at its EXACT midpoint? This is not the unknown SOURCE warm-up dot.";
                value=20;break;
            case "possible_between_50_55":
                prompt="Edulytics original NUMBER RANGE reasoning: how many WHOLE numbers are strictly between50 and55? No exact publisher dot is asserted.";
                value=4;break;
            case "possible_between_20_30":
                prompt="Edulytics original NUMBER RANGE reasoning: how many WHOLE numbers are strictly between20 and30? No exact publisher point is asserted.";
                value=9;break;
            case "distance_to_benchmark":
                prompt=$"FICTIONAL evenly spaced 0..40 line: a point is at {point} and benchmark is {low}. How far apart are their unit positions?";
                value=point-low;break;
            case "decompose_subtraction":
                prompt=$"Original Edulytics EXTRA problem: subtract {sub} from {start}, decomposing the number subtracted into tens and ones.";
                value=start-sub;break;
            case "fictional_spinner_move":
                prompt=$"FICTIONAL game number line0..20: move {spin} units from zero. Where does the cube land? NOT the original source spinner gameboard.";
                value=spin;break;
            case "exact_end_capture":
                prompt=$"FICTIONAL track with FINAL tick20. Marker starts at {marker} and imaginary spin moves {spin}. Enter 1 if exactly20 (capture), otherwise0 (no capture); passing20 NEVER counts.";
                value=marker+spin==20?1:0;break;
            case "to_five_cubes":
                prompt=$"FICTIONAL center game player already collected {collected} cubes. How many MORE are needed to reach a total of FIVE?";
                value=5-collected;break;
            default:throw new InvalidOperationException("Unsupported exact family");
        }
        return new(family,prompt,$"First-party computed answer {value}; actual teacher-observed original activity NOT verified.",AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid source-safe number-line exercise parameters");
        int a=p["a"],b=p["b"],c=p["c"],d=p["d"],e=p["e"],f=p["f"];
        int answer=Kind(family) switch{
            "three_cube_sum"=>a+b+c,
            "greater_three_cube_sum"=>Math.Max(a+b+c,d+e+f),
            "two_cube_two_digit"=>10*a+b,
            "two_digit_distance"=>Math.Abs((10*a+b)-(10*c+d)),
            "new_midpoint"=>20,
            "possible_between_50_55"=>4,
            "possible_between_20_30"=>9,
            "distance_to_benchmark"=>p["point"]-p["low"],
            "decompose_subtraction"=>p["start"]-p["sub"],
            "fictional_spinner_move"=>p["spin"],
            "exact_end_capture"=>p["marker"]+p["spin"]==p["end"]?1:0,
            "to_five_cubes"=>5-p["collected"],
            _=>throw new InvalidOperationException("Unsupported first-party family")
        };
        return answer.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||p.Count!=16||
           !p.TryGetValue("lesson",out var lesson)||lesson!=Lesson(family)||
           !p.TryGetValue("a",out var a)||a<1||a>6||
           !p.TryGetValue("b",out var b)||b<1||b>6||
           !p.TryGetValue("c",out var c)||c<1||c>6||
           !p.TryGetValue("d",out var d)||d<1||d>6||
           !p.TryGetValue("e",out var e)||e<1||e>6||
           !p.TryGetValue("f",out var f)||f<1||f>6||
           !p.TryGetValue("low",out var low)||low<1||low>38||
           !p.TryGetValue("high",out var high)||high<=low||high>40||
           !p.TryGetValue("point",out var point)||point<low||point>high||
           !p.TryGetValue("start",out var start)||start<40||start>90||
           !p.TryGetValue("sub",out var sub)||sub<11||sub>39||sub>start||
           !p.TryGetValue("collected",out var collected)||collected<0||collected>4||
           !p.TryGetValue("spin",out var spin)||spin is not (2 or 5 or 10)||
           !p.TryGetValue("end",out var end)||end!=20||
           !p.TryGetValue("marker",out var marker)||marker<0||marker>end)
            return false;
        return true;
    }
}
