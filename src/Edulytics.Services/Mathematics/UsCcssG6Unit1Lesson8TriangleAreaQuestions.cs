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
    // Reuse the existing triangle-area engine for the next original IM lesson.
    public const string L09Area = "usccss.g6.u01.l09.triangle_formula_source_area";
    public const string L09Height = "usccss.g6.u01.l09.triangle_height_validity";
    internal sealed record Problem(string Family,string Prompt,string Explanation,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family) =>
        family is TriangleArea or Preserve or Decimal or L09Area or L09Height;
    private static int Count(string family) => family==TriangleArea?7:
        family==Preserve?7:family==Decimal?3:family==L09Area?14:
        family==L09Height?7:0;

    public static Problem Build(string family, Random random)
    {
        if(!Supports(family))throw new ArgumentException("Unknown original IM G6 L08 family",nameof(family));
        ArgumentNullException.ThrowIfNull(random);
        int v=random.Next(Count(family));
        var text=family==TriangleArea?TriangleProblem(v):
                 family==Preserve?MethodProblem(v):
                 family==Decimal?DecimalProblem(v):
                 family==L09Area?L09AreaProblem(v):L09HeightProblem(v);
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
        L09Area=>v switch
        {
            0=>30m, // Existing L09 b10 h6
            1=>12m, // Existing L09 area30 h5 -> b12
            2=>12m, // Original practice2 A b4 h6
            3=>16m, // Original practice2 B b8 h4
            4=>12m, // Original practice2 C b6 h4
            5=>15m, // Original 9.3 A base5 height6
            6=>8m, // Original 9.3 B right 4×4/2
            7=>10.5m, // Original 9.3 C vertical side7, outside height3
            8=>14m, // Original 9.3 D right base8 height3.5
            9=>15m, // Original 9.3 E base6 and outside height5
            10=>18m, // Original practice4 square6×6, triangle left full side6 opposite vertex on right
            11=>13.5m, // Original practice5 diagonal halves 3×9/2
            12=>82m, // Original practice6 octagon: 10×10 - four right triangles 3×3/2
            13=>4.5m, // Source student summary base3 height3/2
            _=>throw new ArgumentOutOfRangeException(nameof(v))
        },
        L09Height=>v switch
        {
            0=>1m,1=>0m,2=>0m,3=>0m,4=>1m,5=>0m,6=>0m,
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
        if(family==Preserve||family==L09Height)return s is ("0" or "1") &&
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

    private static (string,string) L09AreaProblem(int v)=>v switch
    {
        0=>("Edulytiks source-skill example: a triangle has base 10 cm and corresponding perpendicular height6 cm. Find area in cm².","Half of related parallelogram area60 is30."),
        1=>("Edulytiks source-skill example: triangle area30m², corresponding height5m. Find its base in m.","Solve b=2×30÷5=12."),
        2=>("Original practice2 triangle A on grid: base4 units, height6 units. Find triangle area.","4×6÷2=12."),
        3=>("Original practice2 triangle B on grid: base8 units, height4 units. Find triangle area.","8×4÷2=16."),
        4=>("Original practice2 triangle C on grid: base6 units, height4 units. Find triangle area.","6×4÷2=12."),
        5=>("Original activity9.3 triangle A: a base is 5 cm and its corresponding perpendicular height is 6 cm. Find area.","5×6÷2=15."),
        6=>("Original activity9.3 triangle B is right with two perpendicular legs4cm and4cm. Find area.","4×4÷2=8."),
        7=>("Original activity9.3 triangle C: its VERTICAL side is7cm; the opposite vertex lies3cm horizontally away. A slanted side3.5cm is a distractor. Find area.","7×3÷2=10.5; the slanted3.5 is NOT the perpendicular height."),
        8=>("Original activity9.3 triangle D: two perpendicular legs8cm and3.5cm; its slanted side8.73cm is a distractor. Find area.","8×3.5÷2=14."),
        9=>("Original activity9.3 triangle E: horizontal base6cm and OUTSIDE perpendicular height5cm; a slanted 10cm side is not its height. Find area.","6×5÷2=15."),
        10=>("Original practice4: a triangle has a full vertical base equal to the left side of a6×6 square and its opposite vertex touches the right side (2 from top,4 from bottom). Find shaded area.","The perpendicular horizontal height to the full left base is6, so area6×6÷2=18."),
        11=>("Original practice5: a 3-unit base and9-unit perpendicular height parallelogram is divided by its diagonal. Find ONE triangular half area.","Total area27, each congruent triangle13.5."),
        12=>("Original practice6: an octagon fits a 10-by-10 inch square; four corners are removed as right triangles with legs3in and3in. Find exact octagon area in sq inches.","Outer area100 minus4×(3×3÷2)=18 gives82; smaller than100."),
        13=>("Original student summary triangle: a base of3cm and corresponding perpendicular height3cm gives what area?","3×3÷2=4.5."),
        _=>throw new ArgumentOutOfRangeException(nameof(v))
    };

    private static (string,string) L09HeightProblem(int v)=>v switch
    {
        0=>("Original9.1 statement1: ANY side of a triangle can serve as base. Enter1=true,0=false.","Every triangle has three candidate side bases."),
        1=>("Original9.1 statement2: a triangle has ONLY ONE possible corresponding height overall. 1=true,0=false.","A triangle has three choices of base and corresponding perpendicular height."),
        2=>("Original9.1 statement3: the corresponding height is ALWAYS one of the triangle sides. 1=true,0=false.","Only for a right triangle can a corresponding height coincide with another side."),
        3=>("Original9.1 statement4: a corresponding height must meet the base at an ACUTE angle. 1=true,0=false.","A valid altitude is perpendicular, 90 degrees."),
        4=>("Original9.1 statement5: a corresponding height must be perpendicular to the chosen base. 1=true,0=false.","The height is perpendicular distance from the base line to the opposite vertex."),
        5=>("Original9.1 statement6: after choosing the base, only ONE location of a height segment can represent its distance to the opposite vertex. 1=true,0=false.","Perpendicular distance has many representative segments between the parallel base and opposite-vertex lines."),
        6=>("Original9.1 statement7: a representative height segment MUST pass through the opposite vertex. 1=true,0=false.","A translated perpendicular with the same length may represent the height; it need not pass through the vertex."),
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
