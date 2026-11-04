using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Original OUR/IM G6 6.1.7: diagonal congruence, triangle composition,
/// fractional base-height practice; independent numeric checks only.
/// Physical tracing, polygons, teacher discussions and justifications
/// remain teacher-reviewed, not replaced by numeric labels.
/// </summary>
internal static class UsCcssG6Unit1Lesson7TriangleQuestions
{
    public const string Half = "usccss.g6.u01.l07.parallelogram_diagonal_half";
    public const string Fraction = "usccss.g6.u01.l07.original_fraction_dimensions";
    public const string Shape = "usccss.g6.u01.l07.triangle_composition_logic";
    internal sealed record Problem(string Family,string Prompt,string Explanation,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);

    public static bool Supports(string? family) => family is Half or Fraction or Shape;
    private static int Count(string family) => family==Half ? 7:family==Fraction?3:family==Shape?9:0;

    public static Problem Build(string family,Random random)
    {
        if(!Supports(family)) throw new ArgumentException("Unknown original G6 L07 family",nameof(family));
        ArgumentNullException.ThrowIfNull(random);
        int v=random.Next(Count(family));
        var e=family==Half?HalfProblem(v):family==Fraction?FractionProblem(v):ShapeProblem(v);
        return new Problem(family,e.Item1,e.Item2+
            " Teacher reviews source-matched diagonal/tracing construction, " +
            "congruent pieces, non-right rectangles, and student's reasoning.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"variant",v}});
    }

    private static (long,long) Answer(string family,int v) => family switch
    {
        Half => v switch
        {
            0 => (20,1), // First-party worked example base8 height5, half40
            1 => (12,1), // Original first-party P=(0,0),(6,0),(8,4),(2,4)
            2 => (9,1), // First-party independent Cool-down b9 h2
            3 => (24,1), // Two triangles area12 compose area24
            4 => (15,2), // Two identical 3×5 half-rectangles
            5 => (6,1), // triangle (0,0),(4,0),(1,3) area6
            6 => (3,1), // 3 sides can each be joined to a congruent copy
            _ => throw new ArgumentOutOfRangeException(nameof(v))
        },
        Fraction => v switch
        {
            0 => (6,1), // Original practice5A 9 × (2/3) = 6
            1 => (4,3), // Original practice5B 12 /9 = 4/3
            2 => (28,1), // Original practice5C 7/(1/4)=28
            _ => throw new ArgumentOutOfRangeException(nameof(v))
        },
        Shape => v switch
        {
            0 => (1,1), // diagonal of any parallelogram makes identical triangles
            1 => (0,1), // diagonal of every quadrilateral makes identical triangles
            2 => (1,1), // any two identical triangles compose a parallelogram
            3 => (0,1), // any two non-right triangles always make rectangle
            4 => (1,1), // matching right triangles may make rectangle
            5 => (0,1), // source 7.2 C/E nonparallelogram general nonexample
            6 => (1,1), // source 7.2 A/B/D parallelogram general example
            7 => (0,1), // equal numerical area proves identical shape
            8 => (1,1), // two congruent triangles have equal area
            _ => throw new ArgumentOutOfRangeException(nameof(v))
        },
        _ => throw new ArgumentException("Unknown family",nameof(family))
    };

    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new ArgumentException("Invalid variant",nameof(p));
        var (num,den)=Answer(family,p["variant"]);
        if(den==1)return num.ToString(CultureInfo.InvariantCulture);
        return num.ToString(CultureInfo.InvariantCulture)+"/"+
               den.ToString(CultureInfo.InvariantCulture);
    }

    public static bool Verify(string family,IReadOnlyDictionary<string,int>? p,string? response)
    {
        if(!Valid(family,p)||string.IsNullOrWhiteSpace(response)||
            response!=response.Trim())return false;
        if(family==Shape)
            return response is ("0" or "1")&&response==Solve(family,p!);
        var (num,den)=Answer(family,p!["variant"]);
        var parts=response.Split('/');
        if(parts.Length==2)
        {
            if(!long.TryParse(parts[0],NumberStyles.None,CultureInfo.InvariantCulture,out long a)
                || !long.TryParse(parts[1],NumberStyles.None,CultureInfo.InvariantCulture,out long b)
                || a<0 || b<=0 || a>1000000 || b>1000000)return false;
            return a*den==num*b;
        }
        if(parts.Length!=1||!decimal.TryParse(response,
            NumberStyles.AllowDecimalPoint,CultureInfo.InvariantCulture,out decimal value)
            ||value<0)return false;
        return value*den==num;
    }

    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p)=>
        Supports(family)&&p is {Count:1}&&p.TryGetValue("variant",out int v)
        &&v>=0&&v<Count(family!);

    private static (string,string) HalfProblem(int v)=>v switch
    {
        0=>("Two matching triangles come from a diagonal in a parallelogram with base8cm and perpendicular height5cm. Find ONE triangle area in cm².","Parallelogram area40, each half20."),
        1=>("Grade6 U01 L07 first-party coordinate parallelogram P=(0,0),(6,0),(8,4),(2,4). Draw a diagonal; find ONE of the two equal triangle areas.","Parallelogram area24; each equal triangle12."),
        2=>("Edulytiks authored Cool-down, NOT original protected publisher item: a parallelogram has base9 and height2. Draw a diagonal. What is the area of each matching triangle?","Whole area18, each part9. Teacher checks congruence and diagonal drawing."),
        3=>("Two identical triangles have area12cm² EACH. Arrange the pieces without gaps or overlaps to make a parallelogram. What is its area?","Two copies total24."),
        4=>("A triangle with base3 units and perpendicular height5 units makes half of a matching parallelogram. Find its area in square units.","The parallelogram is area15, so each triangle7.5."),
        5=>("A triangle has vertices (0,0),(4,0),(1,3). What is its area in square units?","The perpendicular base-height pair is4 by3, so each triangular half has area6."),
        6=>("Grade6 source 7.3: two matching triangle copies can be joined to create a parallelogram along any side. How many distinct side choices are there?","Any triangle has3 sides."),
        _=>throw new ArgumentOutOfRangeException(nameof(v))
    };

    private static (string,string) FractionProblem(int v)=>v switch
    {
        0=>("Original practice5A: parallelogram base9 units, height2/3 unit. Find its area in square units.","9×(2/3)=6."),
        1=>("Original practice5B: parallelogram area12 square units, base9 units. Find corresponding height as exact fraction.","12÷9=4/3 units."),
        2=>("Original practice5C: parallelogram area7 square units and height1/4 unit. Find its base.","7÷(1/4)=28 units."),
        _=>throw new ArgumentOutOfRangeException(nameof(v))
    };

    private static (string,string) ShapeProblem(int v)=>v switch
    {
        0=>("Source 7.2: a diagonal of ANY parallelogram splits it into two congruent triangles. True? 1=yes,0=no.","Opposite parallel sides and a half-turn give matching triangles."),
        1=>("Source 7.2: the diagonal of EVERY quadrilateral splits it into two congruent triangles. True? 1=yes,0=no.","In general quadrilateral halves differ; C and E are source nonexamples."),
        2=>("Source 7.3: any two identical triangles can be arranged into SOME parallelogram. 1=true,0=false.","Join a side to a half-turned equal side without overlaps."),
        3=>("Source 7.3: two identical SCALENE NON-RIGHT triangles always can be composed into a rectangle. 1=true,0=false.","A rectangle made from two triangles separated by a diagonal requires a right triangle."),
        4=>("Source 7.3: two congruent RIGHT triangles can form a rectangle by joining appropriate matching edges. 1=true,0=false.","An opposite pair along hypotenuse makes the rectangle."),
        5=>("Source 7.2: general NONPARALLELOGRAM source shapes C and E must admit a diagonal into two identical triangles. 1=true,0=false.","Their original opposite-side relationships do not force congruent diagonal halves."),
        6=>("Source 7.2: original parallelogram shapes A, B, D have a diagonal making two matching triangles. 1=true,0=false.","Parallelogram diagonal halves are congruent."),
        7=>("Source 7.3: equal numerical areas alone prove that two triangles are congruent. 1=true,0=false.","Equal area can occur with different shapes."),
        8=>("Source 7.3: two congruent triangles always have equal area. 1=true,0=false.","A rigid motion preserves area."),
        _=>throw new ArgumentOutOfRangeException(nameof(v))
    };
}
