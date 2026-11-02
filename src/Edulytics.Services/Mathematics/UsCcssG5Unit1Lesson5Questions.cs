using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;
/// <summary>IM 2021 Grade 5 U01 L05: source-growing prisms with variable bases and 2021 heights.
/// The source's physical models, open explanations and rubric remain teacher reviewed.</summary>
internal static class UsCcssG5Unit1Lesson5Questions
{
    public const string Family="usccss.g5.u01.l05.growing_prism_fixed_base";
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>string.Equals(family,Family,StringComparison.Ordinal);
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Not G5 lesson 5");
        ArgumentNullException.ThrowIfNull(random);
        
int w=random.Next(2,11),d=random.Next(2,13);
var allowedHeights=new[]{1,2,3,10,25};
var h=allowedHeights[random.Next(allowedHeights.Length)];
var p=new Dictionary<string,int>(StringComparer.Ordinal){["width"]=w,["depth"]=d,["height"]=h};
var prompt=$"Edulytiks variation on the 2021 source Growing Prism 4-by-10 base: "+
    $"use a packed {w}-by-{d} unit-cube base and {h} equal full layers. "+
    "What is its volume in unit cubes? The source original 4-by-10 base "+
    "examples and rotations are addressed in lesson content.";
return new Problem(family,prompt,"Use the exact base area times full layer count. "+
    "Physical base rotations and invented questions must be reviewed by the teacher.",
    AssessmentItemType.Numeric,p);

    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid source math parameters");
        return (p["width"]*p["depth"]*p["height"]).ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p)
    {
        if(!Supports(family)||p is null)return false;
        return p.Count==3&&p.TryGetValue("width",out var w)&&w is >=2 and <=10&&
p.TryGetValue("depth",out var d)&&d is >=2 and <=12&&
p.TryGetValue("height",out var h)&&(h is 1 or 2 or 3 or 10 or 25);
    }
}
