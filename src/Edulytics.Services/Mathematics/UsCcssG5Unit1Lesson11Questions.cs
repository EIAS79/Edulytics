using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>IM 2021 G5 U01 L11 original 300,180,270,216 packed-box and garden contexts.
/// Numeric subskill only; contextual drawings and nonoverlapping garden redesign require teacher.</summary>
internal static class UsCcssG5Unit1Lesson11Questions
{
    public const string Family="usccss.g5.u01.l11.real_world_prism_volume";
    private static readonly string[] SourceEquivalentPrompts=["Original 2021 Han source: an empty packed box fits equal cubic-unit cubes in 10 rows by 6 columns per layer and 5 layers high. How many identical cubes fit without gaps or overlaps? Explain cube-count reasoning.","Original 2021 Clare source: an art-supply container measures 4 feet wide by 9 feet long by 5 feet tall. What volume in cubic feet? Check units and base-area times height.","Original 2021 Mai source: a closet floor is 30 square feet and its ceiling is 9 feet high. Find the closet volume in cubic feet. Explain the relationship of base area and perpendicular height.","Original 2021 raised-bed garden: two nonoverlapping prism sections measure 3 by 8 by 4 feet and 10 by 3 by 4 feet. What total garden volume in cubic feet? Redesign the garden with the same volume for teacher review."];
    private static readonly int[] IndependentAnswers=[300,180,270,216];
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>string.Equals(family,Family,StringComparison.Ordinal);
    public static Problem Build(string family,Random random){
        if(!Supports(family))throw new InvalidOperationException("Not G5 L11");
        ArgumentNullException.ThrowIfNull(random);
        var v=random.Next(IndependentAnswers.Length);
        return new Problem(family,SourceEquivalentPrompts[v],"Teacher reviews contextual units, physical/sketched 300-cube box and two-part 216-ft3 garden redesign, including no double-counting at joints.",AssessmentItemType.Numeric,
            new Dictionary<string,int>(StringComparer.Ordinal){["variant"]=v});
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p){
        if(!Valid(family,p))throw new InvalidOperationException("Invalid G5 L11 source variant");
        return IndependentAnswers[p["variant"]].ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? a)=>Valid(family,p)&&string.Equals(a,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p)=>Supports(family)&&p is not null&&p.Count==1&&p.TryGetValue("variant",out var v)&&v>=0&&v<IndependentAnswers.Length;
}
