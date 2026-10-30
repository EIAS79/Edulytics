using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// IM 2021 G5 U01:L03 original Student Facing 4x2x5, 4x3x5, 4x4x5 drawings.
/// Full rectangular prisms have hidden cubes; teacher checks drawing/rotation.
/// </summary>
internal static class UsCcssG5Unit1Lesson3Questions
{
    public const string Family = "usccss.g5.u01.l03.count_hidden_prism_layers";
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family) =>
        string.Equals(family,Family,StringComparison.Ordinal);
    public static Problem Build(string family, Random random)
    {
        if (!Supports(family)) throw new InvalidOperationException("Not G5 L03");
        ArgumentNullException.ThrowIfNull(random);
        var depth=random.Next(2,5);
        var p=new Dictionary<string,int>(StringComparer.Ordinal) {["depth"]=depth};
        var prompt=$"A fully packed rectangular prism from the 2021 cube-layer drawings " +
            $"is 4 cubes across, {depth} cubes deep and 5 cubes high. " +
            "Every layer is complete, including cubes hidden from view. " +
            "How many identical unit cubes fill the entire solid? " +
            "Show two ways to group layers for teacher review.";
        var solution="Count each full layer and all five layers. A valid rotated " +
            "model may group equal cubes in a different order.";
        return new Problem(family,prompt,solution,AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if (!Valid(family,p)) throw new InvalidOperationException("Bad G5 L03 parameters");
        return (4*p["depth"]*5).ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer) =>
        Valid(family,p) && string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p) =>
        Supports(family) && p is not null && p.Count==1 &&
        p.TryGetValue("depth",out var depth) && depth is >=2 and <=4;
}
