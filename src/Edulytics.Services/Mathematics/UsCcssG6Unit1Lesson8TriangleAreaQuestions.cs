using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Original OUR/IM G6 U01:L08 source: area via two copies (half of full
/// parallelogram), or cutting ONE triangle (same area as smaller parallelogram).
/// Only numerical area and precise statements are auto-checked.
/// </summary>
internal static class UsCcssG6Unit1Lesson8TriangleAreaQuestions
{
    public const string TriangleArea = "usccss.g6.u01.l08.triangle_half_area";
    public const string Preserve = "usccss.g6.u01.l08.decomposition_invariance";
    public const string Decimal = "usccss.g6.u01.l08.original_decimal_practice";
    internal sealed record Problem(string Family,string Prompt,string Explanation,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family) =>
        family is TriangleArea or Preserve or Decimal;
    private static int Count(string family) => family==TriangleArea?7:
        family==Preserve?7:family==Decimal?3:0;

    public static Problem Build(string family, Random random)
    {
        if(!Supports(family))throw new ArgumentException("Unknown original IM G6 L08 family",nameof(family));
        ArgumentNullException.ThrowIfNull(random);
        int v=random.Next(Count(family));
        var text=family==TriangleArea?TriangleProblem(v):
                 family==Preserve?MethodProblem(v):DecimalProblem(v);
        return new Problem(family,text.Item1,text.Item2+
           " Teacher reviews actual cuts, duplication count, rectangles, " +
           "perpendicular altitudes, diagrams and student's written proof.",
           AssessmentItemType.Numeric,new Dictionary<string,int>{{"variant",v}});
    }

    private static decimal Answer(string family,int v)=>family switch
    {
        TriangleArea=>v switch
        {
            0=>12m, // Source 8.1 Triangle M 6×4÷2
            1=>20m, // Existing worked example 8×5÷2
            2=>20m, // Existing contrasting 10×4÷2
            3=>8m, // Source summary parallelogram B 8×2÷2
            4=>12m, // Source summary parallelogram D 4×6÷2
            5=>4.5m, // Source summary: remaining area9 is TWO copies, one triangle4.5
            6=>12m, // Original practice2 grid base6 height4
            _=>throw new ArgumentOutOfRangeException(nameof(v))
        },
        Preserve=>v switch
        {
            0=>1m, 1=>1m, 2=>0m, 3=>1m, 4=>1m, 5=>1m, 6=>0m,
            _=>throw new ArgumentOutOfRangeException(nameof(v))
        },
        Decimal=>v switch
        {
            0=>7m, // Original practice5A parallelogram 3.5×2
            1=>0.6m, // Original practice5B area1.8/base3
            2=>5.1m, // Original practice5C area20.4/height4
            _=>throw new ArgumentOutOfRangeException(nameof(v))
        },
        _=>throw new ArgumentException("Unsupported family",nameof(family))
    };

    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new ArgumentException("Invalid L08 variant",nameof(p));
        return Answer(family,p["variant"]).ToString(CultureInfo.InvariantCulture);
    }

    public static bool Verify(string family,IReadOnlyDictionary<string,int>? p,string? s)
    {
        if(!Valid(family,p)||string.IsNullOrWhiteSpace(s)||s!=s.Trim())return false;
        var expected=Answer(family,p!["variant"]);
        if(family==Preserve)return s is ("0" or "1") &&
            s==expected.ToString(CultureInfo.InvariantCulture);
        var parts=s.Split('/');
        if(parts.Length==2)
        {
            if(!long.TryParse(parts[0],NumberStyles.None,CultureInfo.InvariantCulture,out long a)
               ||!long.TryParse(parts[1],NumberStyles.None,CultureInfo.InvariantCulture,out long b)
               ||a<0||b<=0||a>1000000||b>1000000)return false;
            return a == expected*b;
        }
        return parts.Length==1 &&
            decimal.TryParse(s,NumberStyles.AllowDecimalPoint,CultureInfo.InvariantCulture,out decimal num)
            && num>=0 && num==expected;
    }

    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p)=>
        Supports(family)&&p is {Count:1}&&p.TryGetValue("variant",out int v)&&
        v>=0&&v<Count(family!);

    private static (string,string) TriangleProblem(int v)=>v switch
    {
        0=>("Original 8.1: Han makes three different parallelograms from TWO copies of his triangle with perpendicular legs 6 and4 grid units. What is ONE triangle's area?","Each parallelogram area24, one copy12."),
        1=>("Existing source-aligned triangle example base8cm and perpendicular height5cm. Find area in cm².","Two copies give parallelogram area40, so one half20."),
        2=>("Existing triangle model base10m and perpendicular height4m. Find its area in m².","10×4÷2=20."),
        3=>("Original lesson summary: related parallelogram B is 8 by2 sq units. Find the area of ONE congruent triangular half.","Parallelogram16, one triangle8."),
        4=>("Original summary parallelogram D is 4 by6 sq units. Find one triangular half.","Parallelogram24, one triangle12."),
        5=>("Original source summary: after subtracting two right triangles, a related parallelogram has area 5×3−2×3=9 sq units and is made from TWO copies of the original triangle. What is ONE triangle area?","Two complete copies make area9, so a single triangle has area4.5; do not confuse this with a rearrangement of one triangle into a same-area parallelogram."),
        6=>("Original practice2 triangle on square grid has base6 and perpendicular height4 units. Find area in sq units.","Two equal copies make 6-by-4 parallelogram24, so one12."),
        _=>throw new ArgumentOutOfRangeException(nameof(v))
    };

    private static (string,string) MethodProblem(int v)=>v switch
    {
        0=>("Source 8.1: joining TWO complete identical triangle copies forms a parallelogram of TWICE the triangle's area. 1=true,0=false.","The doubled region has two full original triangles."),
        1=>("Source optional 8.3: cutting ONE triangle into a small triangle and trapezoid, then rearranging without loss, forms a parallelogram of the SAME area as original triangle. 1=true,0=false.","Only original pieces moved; no complete second triangle was added."),
        2=>("Source 8.3: cutting and rearranging ONE triangle without adding a copy must DOUBLE its area. 1=true,0=false.","Rearrangement preserves the original area."),
        3=>("Source 8.2: two triangles with same corresponding base-height measures have the same area even when shapes differ. 1=true,0=false.","Equal base-times-perpendicular-height halves match."),
        4=>("Source 8.2: a triangle whose perpendicular altitude falls OUTSIDE its outline still has area base×height÷2. 1=true,0=false.","The height is measured to the extended base line."),
        5=>("Source 8.3: the smaller parallelogram obtained from a single triangle's own cut pieces has the same area as that triangle. 1=true,0=false.","Conserving the pieces preserves area."),
        6=>("Source 8.1: every parallelogram that can be made from two whole copies of a triangle always has the SAME area as ONE copy. 1=true,0=false.","It has two identical copies, twice their individual area."),
        _=>throw new ArgumentOutOfRangeException(nameof(v))
    };

    private static (string,string) DecimalProblem(int v)=>v switch
    {
        0=>("Original practice5A prerequisite: parallelogram base3.5 units, corresponding height2 units. Find its area.","3.5×2=7."),
        1=>("Original practice5B prerequisite: parallelogram area1.8 sq units, selected base3 units. Find its corresponding height.","1.8÷3=0.6."),
        2=>("Original practice5C prerequisite: parallelogram area20.4 sq units, perpendicular height4 units. Find its base length.","20.4÷4=5.1."),
        _=>throw new ArgumentOutOfRangeException(nameof(v))
    };
}
