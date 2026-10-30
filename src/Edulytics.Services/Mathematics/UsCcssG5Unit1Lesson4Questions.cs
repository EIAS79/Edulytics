using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>IM 2021 G5 U01:L04 complete measured prisms only.
/// When the source drawing omits a dimension, exact volume is NOT inferred.
/// The incomplete-source-drawing estimation is separately teacher-reviewed.
/// </summary>
internal static class UsCcssG5Unit1Lesson4Questions
{
    public const string Family="usccss.g5.u01.l04.full_prism_layer_groups";
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family) =>
        string.Equals(family,Family,StringComparison.Ordinal);
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Not G5 L04");
        ArgumentNullException.ThrowIfNull(random);
        var model=random.Next(3);
        var width=model==0?2:3;
        var depth=model==0?3:model==1?5:2;
        var height=model==0?4:model==1?2:5;
        var p=new Dictionary<string,int>(StringComparer.Ordinal){["model"]=model};
        var prompt=$"A fully packed 2021 example prism is {width} cubes wide, "+
            $"{depth} cubes deep and {height} complete layers high. "+
            "All dimensions are given; no invisible dimension is being guessed. "+
            "How many congruent cubes fill it? Show a layer grouping to the teacher.";
        return new Problem(family,prompt,
            "Count the cubes in one complete layer and repeat for every layer. "+
            "Estimations from an incomplete figure must be justified to the teacher.",
            AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Bad G5 L04 model");
        return (p["model"]==0?24:30).ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer) =>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p) =>
        Supports(family)&&p is not null&&p.Count==1&&
        p.TryGetValue("model",out var model)&&model is >=0 and <=2;
}
