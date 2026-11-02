using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;

/// <summary>Original Edulytics generated arrow arithmetic, not publisher diagram pixels, student source drawings or restricted assessment.</summary>
internal static class UsCcssG2Unit4ArrowLessonsQuestions
{
    private const string Prefix="usccss.g2.u04.l";
    private static readonly IReadOnlySet<string> Known=new HashSet<string>(StringComparer.Ordinal){
        "usccss.g2.u04.l07.right_jump_destination",
        "usccss.g2.u04.l07.left_jump_destination",
        "usccss.g2.u04.l07.directed_jump_length",
        "usccss.g2.u04.l07.start_end_comparison",
        "usccss.g2.u04.l08.addition_endpoint",
        "usccss.g2.u04.l08.subtraction_endpoint",
        "usccss.g2.u04.l08.arrow_span",
        "usccss.g2.u04.l08.count_back_tens",
        "usccss.g2.u04.l09.takeaway_difference",
        "usccss.g2.u04.l09.count_on_unknown_addend",
        "usccss.g2.u04.l09.fictional_string_remaining",
        "usccss.g2.u04.l09.point_difference"
    };
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family is not null&&Known.Contains(family);
    private static int Lesson(string family)=>int.Parse(family.Substring(Prefix.Length,2),CultureInfo.InvariantCulture);
    private static string Kind(string family)=>family[(Prefix.Length+3)..];
    public static Problem Build(string family,Random r)
    {
        if(!Supports(family))throw new InvalidOperationException("Unknown G2 directional arithmetic");
        int start=r.Next(0,81),jump=r.Next(1,21),end=start+jump;
        int whole=r.Next(30,101),part=r.Next(1,whole);
        int countStart=new[]{95,98,99}[r.Next(3)],countStep=10;
        var p=new Dictionary<string,int>(StringComparer.Ordinal){
            ["lesson"]=Lesson(family),["start"]=start,["jump"]=jump,["end"]=end,
            ["whole"]=whole,["part"]=part,["countStart"]=countStart,["countStep"]=countStep
        };
        int value;string prompt;
        switch(Kind(family))
        {
            case "right_jump_destination":
            case "addition_endpoint":
                value=end;prompt=$"FIRST-PARTY equal-unit line: begin at {start} and jump RIGHT {jump} spaces. At what number does the arrow end?";break;
            case "left_jump_destination":
            case "subtraction_endpoint":
            case "takeaway_difference":
            case "fictional_string_remaining":
                value=whole-part;prompt=$"FIRST-PARTY starting at {whole}, move LEFT by {part} equal unit spaces. What is the endpoint? This is not a source pupil string.";break;
            case "directed_jump_length":
            case "arrow_span":
                value=jump;prompt=$"FIRST-PARTY abstract number line with right arrow from {start} to {end}. How many unit intervals is the arrow?";break;
            case "start_end_comparison":
                value=end-start;prompt=$"FIRST-PARTY dot at {start} and one at {end}. How many unit intervals MORE is the farther-right point?";break;
            case "count_back_tens":
                value=countStart-30;prompt=$"FIRST-PARTY count BACK three jumps of TEN from {countStart}. What whole number do you reach?";break;
            case "count_on_unknown_addend":
            case "point_difference":
                value=part;prompt=$"FIRST-PARTY number line: what is the length of a RIGHTWARD jump from {whole-part} to {whole} that represents the unknown addend?";break;
            default:throw new InvalidOperationException("Unrecognized exact family");
        }
        return new(family,prompt,$"Exact Edulytics-generated result {value}; source diagrams require teacher observation.",AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid number-line arrow coordinates");
        int value=Kind(family) switch{
            "right_jump_destination" or "addition_endpoint"=>p["end"],
            "left_jump_destination" or "subtraction_endpoint" or "takeaway_difference" or "fictional_string_remaining"=>p["whole"]-p["part"],
            "directed_jump_length" or "arrow_span" or "start_end_comparison"=>p["jump"],
            "count_back_tens"=>p["countStart"]-30,
            "count_on_unknown_addend" or "point_difference"=>p["part"],
            _=>throw new InvalidOperationException("Unknown numeric family")
        };
        return value.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)=>
        family is not null&&Supports(family)&&p.Count==8&&
        p.TryGetValue("lesson",out var l)&&l==Lesson(family)&&
        p.TryGetValue("start",out var s)&&s>=0&&s<=80&&
        p.TryGetValue("jump",out var jump)&&jump>=1&&jump<=20&&
        p.TryGetValue("end",out var end)&&end==s+jump&&end<=100&&
        p.TryGetValue("whole",out var whole)&&whole>=30&&whole<=100&&
        p.TryGetValue("part",out var part)&&part>=1&&part<whole&&
        p.TryGetValue("countStart",out var cs)&&(cs is 95 or 98 or 99)&&
        p.TryGetValue("countStep",out var step)&&step==10;
}
