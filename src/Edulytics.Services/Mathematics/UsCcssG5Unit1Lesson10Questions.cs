using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>IM 2021 G5 U01 L10 source 95 and 168 composite volumes;
/// only arithmetic source-subskills automatic, geometry and expressions teacher reviewed.</summary>
internal static class UsCcssG5Unit1Lesson10Questions
{
    public const string Family="usccss.g5.u01.l10.equivalent_composite_volume_expressions";
    private static readonly string[] SourceEquivalentPrompts=["Original 2021 source: a composed figure contains one 3 by 5 by 5 prism and a separate attached 2 by 2 by 5 prism, with no overlapping volume. Evaluate (3×5×5)+(2×2×5) in cubic units. Teacher checks the decomposition.","Original source equivalent subtraction: a 5-by-5-by-5 filled cubic region is missing a 2-by-3-by-5 prism; there is no overlap. What is the remaining volume in cubic units? Explain what was removed.","Exact original stepped measured shape: a 4 ft by 4 ft by 3 ft prism attaches without overlap to a 10 ft by 4 ft by 3 ft prism. Calculate the total cubic feet; compare 4×8×3 + 6×4×3 as another valid cut."];
    private static readonly int[] IndependentAnswers=[95,95,168];
    private static string PromptFor(int variant)
    {
        if(variant<SourceEquivalentPrompts.Length)return SourceEquivalentPrompts[variant];
        return $"Edulytiks-original supplementary composite subtraction: a {variant+6} by 5 by 3 unit prism has a 2 by 2 by 3 unit rectangular section removed, wholly contained within it. What volume remains? Teacher checks the geometric subtraction and the alternative partition.";
    }
    private static int AnswerFor(int variant) =>
        variant<IndependentAnswers.Length?IndependentAnswers[variant]:15*variant+78;
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>string.Equals(family,Family,StringComparison.Ordinal);
    public static Problem Build(string family,Random random){
        if(!Supports(family))throw new InvalidOperationException("Not G5 L10");
        ArgumentNullException.ThrowIfNull(random);
        var v=random.Next(12);
        return new Problem(family,PromptFor(v),"Teacher reviews two real prism partitions, equivalent expressions and correct choice of removed region. Right total with wrong geometry is not accepted as a full solution.",AssessmentItemType.Numeric,
            new Dictionary<string,int>(StringComparer.Ordinal){["variant"]=v});
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p){
        if(!Valid(family,p))throw new InvalidOperationException("Invalid G5 L10 source variant");
        return AnswerFor(p["variant"]).ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? a)=>Valid(family,p)&&string.Equals(a,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p)=>Supports(family)&&p is not null&&p.Count==1&&p.TryGetValue("variant",out var v)&&v>=0&&v<12;
}
