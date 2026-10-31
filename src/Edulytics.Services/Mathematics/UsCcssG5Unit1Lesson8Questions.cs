using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>2021 IM G5 U01 L08 nonoverlap prism cube sums; teacher scores drawing cuts.</summary>
internal static class UsCcssG5Unit1Lesson8Questions
{
    public const string Family="usccss.g5.u01.l08.add_nonoverlap_prism_volumes";
    private static readonly string[] SourceEquivalentPrompts=["Original 2021 Put It Together: Partner A builds a packed 12-cube prism and Partner B a packed 10-cube prism. They attach along a face without overlapping cubes. How many equal-sized cubes in the new figure? Explain your decomposition to the teacher.","Original 2021 Diego and Jada figure: one nonoverlapping 3-by-2-by-2 prism is attached to a 3-by-2-by-3 prism along a face. What is their total volume in cubic units?","Original 2021 Figure Made of Prisms: a fully packed 3-by-5-by-3 prism has ONE additional cube attached outside at a face, not overlapping it. What is the whole connected figure volume in cubic units?"];
    private static readonly int[] IndependentAnswers=[22,30,46];
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>string.Equals(family,Family,StringComparison.Ordinal);
    public static Problem Build(string family,Random random){
        if(!Supports(family))throw new InvalidOperationException("Not G5 L08");
        ArgumentNullException.ThrowIfNull(random);
        var v=random.Next(IndependentAnswers.Length);
        return new Problem(family,SourceEquivalentPrompts[v],"Build and explain nonoverlapping attached prisms; show different valid cuts and check no double-counting. WODB and the original publisher Cool-down are not auto-scored.",AssessmentItemType.Numeric,
            new Dictionary<string,int>(StringComparer.Ordinal){["variant"]=v});
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p){
        if(!Valid(family,p))throw new InvalidOperationException("Bad G5 L08 source variant");
        return IndependentAnswers[p["variant"]].ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? a)=>Valid(family,p)&&string.Equals(a,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p)=>Supports(family)&&p is not null&&p.Count==1&&p.TryGetValue("variant",out var v)&&v>=0&&v<IndependentAnswers.Length;
}
