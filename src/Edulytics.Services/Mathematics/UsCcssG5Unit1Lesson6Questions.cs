using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;
/// <summary>IM 2021 Grade 5 U01 L06: true/false volume expressions matched to source geometry.
/// The source's physical models, open explanations and rubric remain teacher reviewed.</summary>
internal static class UsCcssG5Unit1Lesson6Questions
{
    public const string Family="usccss.g5.u01.l06.match_volume_expression";
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>string.Equals(family,Family,StringComparison.Ordinal);
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Not G5 lesson 6");
        ArgumentNullException.ThrowIfNull(random);
        
int w=random.Next(2,11),d=random.Next(2,11),h=random.Next(2,11);
if(d==2&&h==2)d=3;
var choice=random.Next(2);
var expression=choice==0?$"({w} × {d}) × {h}":$"{w} × ({d} + {h})";
var p=new Dictionary<string,int>(StringComparer.Ordinal){["width"]=w,["depth"]=d,["height"]=h,["choice"]=choice};
var prompt=$"Original 2021 source goal: a packed prism has side lengths {w}, {d}, "+
    $"and {h} units. Does expression {expression} give the correct volume? "+
    "Enter 1 for yes or 0 for no; show geometric justification to a teacher.";
return new Problem(family,prompt,"Complete layer expressions multiply all dimensions; "+
    "addition of depth and height is not the geometric layer product. "+
    "Card sort explanations are teacher reviewed.",
    AssessmentItemType.Numeric,p);

    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid source math parameters");
        return (p["choice"]==0?1:0).ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p)
    {
        if(!Supports(family)||p is null)return false;
        return p.Count==4&&p.TryGetValue("width",out var w)&&w is >=2 and <=10&&
p.TryGetValue("depth",out var d)&&d is >=2 and <=10&&
p.TryGetValue("height",out var h)&&h is >=2 and <=10&&!(d==2&&h==2)&&
p.TryGetValue("choice",out var choice)&&(choice is 0 or 1);
    }
}
