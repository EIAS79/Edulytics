using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;
/// <summary>IM 2021 Grade 5 U01 L04: complete-dimension layer counts, no false inference.
/// The source's physical models, open explanations and rubric remain teacher reviewed.</summary>
internal static class UsCcssG5Unit1Lesson4Questions
{
    public const string Family="usccss.g5.u01.l04.full_prism_layer_groups";
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>string.Equals(family,Family,StringComparison.Ordinal);
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Not G5 lesson 4");
        ArgumentNullException.ThrowIfNull(random);
        
var w=random.Next(2,10),d=random.Next(2,10),h=random.Next(2,9);
var p=new Dictionary<string,int>(StringComparer.Ordinal){["width"]=w,["depth"]=d,["height"]=h};
var prompt=$"Edulytiks complete-dimension analogue: a packed prism is {w} cubes "+
    $"wide, {d} cubes deep, and {h} full layers high. All three dimensions "+
    "are provided; no invisible dimension is being guessed. How many cubes fill it? "+
    "The incomplete original 2021 drawings require separate teacher inference.";
return new Problem(family,prompt,"Count a full base and all layers. "+
    "Do not infer a definite volume for a partially given source figure.",
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
        return p.Count==3&&p.TryGetValue("width",out var w)&&w is >=2 and <=9&&
p.TryGetValue("depth",out var d)&&d is >=2 and <=9&&
p.TryGetValue("height",out var h)&&h is >=2 and <=8;
    }
}
