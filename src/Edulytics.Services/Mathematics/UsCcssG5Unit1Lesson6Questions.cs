using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;
/// <summary>Original 2021 Grade 5 U01 L06: true/false prism expression matching.
/// Correct numerical choice is digital, geometric reasoning stays teacher reviewed.</summary>
internal static class UsCcssG5Unit1Lesson6Questions
{
    public const string Family="usccss.g5.u01.l06.match_volume_expression";
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family) =>
        string.Equals(family,Family,StringComparison.Ordinal);
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family)) throw new InvalidOperationException("Not G5 L06");
        ArgumentNullException.ThrowIfNull(random);
        var model=random.Next(5);
        var width=model switch {0=>6,1=>4,2=>3,3=>4,_=>10};
        var depth=model switch {0=>3,1=>5,2=>4,3=>4,_=>6};
        var height=model switch {0=>4,1=>8,2=>5,3=>4,_=>3};
        var choice=random.Next(2);
        var expression=choice==0?
            $"({width} × {depth}) × {height}":
            $"{width} × ({depth} + {height})";
        var p=new Dictionary<string,int>(StringComparer.Ordinal)
        {["model"]=model,["choice"]=choice};
        var prompt=$"A packed prism has side lengths {width}, {depth}, and {height} "+
            $"in unit cubes. Does the expression {expression} compute its volume? "+
            "Enter 1 for yes or 0 for no. Explain how the factors describe "+
            "the actual prism geometry to a teacher.";
        var solution="Equivalent layer count requires all three factors to be "+
            "multiplied. Adding side lengths does not represent complete layers.";
        return new Problem(family,prompt,solution,AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Bad G5 L06 choice");
        return (p["choice"]==0?1:0).ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer) =>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p) =>
        Supports(family)&&p is not null&&p.Count==2&&
        p.TryGetValue("model",out var model)&&model is >=0 and <=4&&
        p.TryGetValue("choice",out var choice)&&choice is 0 or 1;
}
