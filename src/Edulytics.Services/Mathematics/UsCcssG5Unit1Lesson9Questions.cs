using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>IM 2021 G5 U01 L09: measured composite volume with genuine no-overlap decompositions.
/// Numerical subskill alone does NOT grade visual missing-edge inference.</summary>
internal static class UsCcssG5Unit1Lesson9Questions
{
    public const string Family="usccss.g5.u01.l09.measured_composite_decompositions";
    private static readonly string[] SourceEquivalentPrompts=["Edulytiks accessible source-geometry counterpart: a stepped solid has two nonoverlapping rectangular prisms 7 ft by 5 ft by 2 ft and 2 ft by 2 ft by 2 ft. Find their combined volume in cubic feet. Justify the missing-edge inference to the teacher.","Edulytiks source-geometry counterpart: a composed figure is split without overlap into 5-by-5-by-7-inch and 4-by-3-by-7-inch prisms. Find the total volume in cubic inches. The teacher checks that this cut corresponds to a valid drawing.","Exact original 2021 figure expression: (5 × 8 × 6) + (5 × 4 × 9) cubic inches, for two nonoverlapping portions. Evaluate the total, then show why (5 × 4 × 3) + (5 × 12 × 6) also gives the same volume."];
    private static readonly int[] IndependentAnswers=[78,259,420];
    private static string PromptFor(int variant)
    {
        if(variant<SourceEquivalentPrompts.Length)return SourceEquivalentPrompts[variant];
        return $"Edulytiks-original supplementary measured composite: two nonoverlapping rectangular parts have dimensions {variant+2} by 3 by 2 inches and {variant+1} by 2 by 4 inches. Find the combined volume in cubic inches, and justify your cut and units to the teacher.";
    }
    private static int AnswerFor(int variant) =>
        variant<IndependentAnswers.Length?IndependentAnswers[variant]:14*variant+20;
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>string.Equals(family,Family,StringComparison.Ordinal);
    public static Problem Build(string family,Random random){
        if(!Supports(family))throw new InvalidOperationException("Not G5 L09");
        ArgumentNullException.ThrowIfNull(random);
        var v=random.Next(12);
        return new Problem(family,PromptFor(v),"Teacher reviews missing-edge deductions, two genuine geometric cuts and feet-vs-inches labels; numerical totals alone do not certify a correctly marked original source figure.",AssessmentItemType.Numeric,
            new Dictionary<string,int>(StringComparer.Ordinal){["variant"]=v});
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p){
        if(!Valid(family,p))throw new InvalidOperationException("Invalid G5 L09 source model");
        return AnswerFor(p["variant"]).ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? a)=>Valid(family,p)&&string.Equals(a,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p)=>Supports(family)&&p is not null&&p.Count==1&&p.TryGetValue("variant",out var v)&&v>=0&&v<12;
}
