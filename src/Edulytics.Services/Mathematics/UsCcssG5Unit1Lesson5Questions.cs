using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;
/// <summary>2021 IM Grade 5 U01:L05, source growing prism: 4x10 base,
/// five permitted heights. Teacher reviews rotated bases and invented questions.</summary>
internal static class UsCcssG5Unit1Lesson5Questions
{
    public const string Family="usccss.g5.u01.l05.growing_prism_fixed_base";
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family) =>
        string.Equals(family,Family,StringComparison.Ordinal);
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Not G5 L05");
        ArgumentNullException.ThrowIfNull(random);
        var heights=new[]{1,2,3,10,25};
        var h=heights[random.Next(heights.Length)];
        var p=new Dictionary<string,int>(StringComparer.Ordinal){["height"]=h};
        var prompt=$"A complete rectangular prism is built on the source's "+
            $"4-by-10 unit-cube base with {h} full layers. "+
            "How many congruent unit cubes does it contain? "+
            "Explain why choosing another face as base would preserve the volume.";
        var solution="A 4-by-10 base has 40 unit squares; each of h full "+
            "layers contributes 40 cubes. Physical base rotation and explanations "+
            "must be reviewed by the teacher.";
        return new Problem(family,prompt,solution,AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Bad G5 L05 height");
        return (40*p["height"]).ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer) =>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p) =>
        Supports(family)&&p is not null&&p.Count==1&&
        p.TryGetValue("height",out var h)&& (h is 1 or 2 or 3 or 10 or 25);
}
