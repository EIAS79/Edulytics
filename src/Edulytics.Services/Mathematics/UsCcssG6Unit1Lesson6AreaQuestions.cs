using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Grade 6 original OUR/IM 6–8 U01:L06 — area/missing dimensions, equal-area
/// reasoning and the original shaded-rectangle practice. Independent numeric
/// verifier; drawing and explanation are always reviewed by a teacher.
/// </summary>
internal static class UsCcssG6Unit1Lesson6AreaQuestions
{
    public const string Dimensions = "usccss.g6.u01.l06.area_missing_dimension";
    public const string EqualArea = "usccss.g6.u01.l06.equal_area_reasoning";
    public const string Composite = "usccss.g6.u01.l06.composite_area";
    internal sealed record Problem(string Family, string Prompt, string Explanation,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) =>
        family is Dimensions or EqualArea or Composite;

    private static int Count(string family) =>
        family == Dimensions ? 13 : family == EqualArea ? 8 : family == Composite ? 3 : 0;

    public static Problem Build(string family, Random random)
    {
        if (!Supports(family))
            throw new ArgumentException("Unsupported original IM G6 U01:L06 family", nameof(family));
        ArgumentNullException.ThrowIfNull(random);
        int variant = random.Next(Count(family));
        var text = family == Dimensions ? NumericProblem(variant)
                 : family == EqualArea ? EqualityProblem(variant) : CompositeProblem(variant);
        return new Problem(family, text.Prompt,
            text.Explanation + " Teacher assesses the chosen corresponding " +
            "base-height pair, original diagram, hand-drawn construction, and " +
            "written geometric justification independently of numeric feedback.",
            AssessmentItemType.Numeric,
            new Dictionary<string,int>(StringComparer.Ordinal) { ["variant"]=variant });
    }

    public static string Solve(string family, IReadOnlyDictionary<string,int> parameters)
    {
        if (!Valid(family, parameters))
            throw new ArgumentException("Malformed original IM L06 source variant",nameof(parameters));
        int v=parameters["variant"];
        decimal answer=family switch
        {
            Dimensions => v switch
            {
                0 => 120m, // Source activity 6.2: 8 × 15
                1 => 12m, // Source activity 6.2: base 10, area 120
                2 => 45m, // Existing first-party 9 × 5
                3 => 15m, // Existing first-party 60 ÷ 4
                4 => 2m, // Original practice 3A: area 10, base 5
                5 => 3m, // Original practice 3B: area 21, height 7
                6 => 5m, // Original practice 3C: area 25, height 5
                7 => 4730m, // Original practice 4: Dockland Building 86 × 55
                8 => 14m, // Original practice 2: 4 × 3.5
                9 => 16m, // Original practice 2: 0.8 × 20 (greatest)
                10 => 13.5m, // Original practice 2: 6 × 2.25
                11 => 14m, // Original practice 2: 10 × 1.4
                12 => 18m, // Source 6.2 same-area first-party P/Q base6 × height3
                _ => throw new ArgumentOutOfRangeException()
            },
            EqualArea => v switch
            {
                0 => 1m, 1 => 0m, 2 => 1m, 3 => 1m,
                4 => 1m, 5 => 1m, 6 => 1m, 7 => 0m,
                _ => throw new ArgumentOutOfRangeException()
            },
            Composite => v switch
            {
                0 => 48m, // Original 6.2 four shaded 5×2.4 parallelograms
                1 => 80m, // Original practice 6 tilted inner rectangle 14×10 - exterior 60
                2 => 24m, // Edulytiks-original Cool-down P/Q 8×3
                _ => throw new ArgumentOutOfRangeException()
            },
            _ => throw new ArgumentException("Unknown source family",nameof(family))
        };
        return answer.ToString(CultureInfo.InvariantCulture);
    }

    public static bool Verify(string family, IReadOnlyDictionary<string,int>? parameters,
        string? answer)
    {
        if (!Valid(family,parameters) || answer is null ||
            answer.Length == 0 || answer != answer.Trim())
            return false;
        if (family == EqualArea)
            return answer is ("0" or "1") && answer == Solve(family,parameters!);
        decimal expected=decimal.Parse(Solve(family,parameters!),CultureInfo.InvariantCulture);
        var parts=answer.Split('/');
        if(parts.Length==2)
        {
            if(!long.TryParse(parts[0],NumberStyles.None,CultureInfo.InvariantCulture,out long numerator)
                || !long.TryParse(parts[1],NumberStyles.None,CultureInfo.InvariantCulture,out long denominator)
                || denominator<=0 || numerator<0 || denominator>1000000 || numerator>1000000)
                return false;
            return numerator == expected*denominator;
        }
        return parts.Length==1 &&
            decimal.TryParse(answer,NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,out decimal numeric) &&
            numeric>=0 && numeric==expected;
    }

    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p) =>
        Supports(family) && p is {Count:1} && p.TryGetValue("variant",out int v) &&
        v>=0 && v<Count(family!);

    private static (string Prompt,string Explanation) NumericProblem(int v) => v switch
    {
        0 => ("Original 6.2 Parallelogram B has a 15-cm base with its corresponding 8-cm height. Find its area in cm².",
             "15 × 8 = 120 square centimeters."),
        1 => ("Original 6.2 Parallelogram B has area 120 cm². A second corresponding base measures 10 cm. Find its perpendicular height in cm.",
             "Area conservation: 8 × 15 = 10 × h, hence h=12."),
        2 => ("A parallelogram has 9-cm base and its corresponding perpendicular height 5 cm. Find area in cm².",
             "A = 9 × 5 = 45."),
        3 => ("The area is 60 m² and perpendicular height 4 m. Find the corresponding base length in meters.",
             "60 ÷ 4 = 15."),
        4 => ("Original practice 3A: a parallelogram has area 10 sq units and selected base 5 units. Find its height.",
             "10 ÷ 5 = 2."),
        5 => ("Original practice 3B: area 21 sq units and the perpendicular height is 7 units. Find its base length.",
             "21 ÷ 7 = 3."),
        6 => ("Original practice 3C: area 25 sq units and corresponding height 5 units. Find its base length.",
             "25 ÷ 5 = 5."),
        7 => ("Original practice 4, Hamburg Dockland Building: a parallelogram-shaped facade has 86 m length and 55 m corresponding height. Find its area in m².",
             "86 × 55 = 4730 square meters."),
        8 => ("Original practice 2, first pair: b = 4 cm, h = 3.5 cm. Find its area in cm².",
             "4 × 3.5 = 14."),
        9 => ("Original practice 2, second pair: b = 0.8 cm, h = 20 cm. Find its area in cm².",
             "0.8 × 20 = 16, the largest of the four proposed areas."),
        10 => ("Original practice 2, third pair: b = 6 cm, h = 2.25 cm. Find its area in cm².",
              "6 × 2.25 = 13.5."),
        11 => ("Original practice 2, fourth pair: b = 10 cm, h = 1.4 cm. Find its area in cm².",
              "10 × 1.4 = 14."),
        12 => ("Source 6.2 skill: draw two distinct parallelograms with base6 and perpendicular height3. What is each area in sq units?",
              "Both have area 6 × 3 = 18, regardless of slant."),
        _ => throw new ArgumentOutOfRangeException(nameof(v))
    };

    private static (string Prompt,string Explanation) EqualityProblem(int v) => v switch
    {
        0 => ("Original practice 1: figure A has base5 height3; B has base3 height5. Same area? Enter 1=yes,0=no.",
              "Each area is 15."),
        1 => ("Original practice 1: figure C is a 4×4 square and A has base5 height3. Same area? 1=yes,0=no.",
              "16 is not 15."),
        2 => ("Original practice 1: figure D has vertical base3 and horizontal distance5; compare with A base5 height3. Same area? 1=yes,0=no.",
              "Both areas are 15."),
        3 => ("Two noncongruent parallelograms with base6 height3 have equal area. 1=true,0=false.",
              "Both products are 18."),
        4 => ("Original practice 2: does b0.8,h20 give the greatest area among 4×3.5,0.8×20,6×2.25,10×1.4? 1=yes,0=no.",
              "Their areas are 14,16,13.5,14."),
        5 => ("Does a base4 height3.5 parallelogram have greater area than base6 height2.25? 1=yes,0=no.",
              "14 > 13.5."),
        6 => ("Could base5 height3 and base6 height2.5 parallelograms have equal area? 1=yes,0=no.",
              "Both products equal 15."),
        7 => ("Original lesson synthesis: two parallelograms with equal bases and heights must have IDENTICAL outlines. 1=true,0=false.",
              "Equal base-height products guarantee area, not congruent outlines."),
        _ => throw new ArgumentOutOfRangeException(nameof(v))
    };

    private static (string Prompt,string Explanation) CompositeProblem(int v) => v switch
    {
        0 => ("Original 6.2 shaded figure: four disjoint identical parallelograms each have base5 inches and perpendicular height2.4 inches. Find their TOTAL shaded area in in², not the unknown central unshaded area.",
              "Four times (5 × 2.4) = 48 square inches. Outer dimensions not verified for central area."),
        1 => ("Original practice 6: a 14-cm-by-10-cm rectangle contains an inclined inner rectangle. Each of two opposite corner triangles has legs2 and6, and the other two have legs12 and4. Find shaded inner area in cm².",
              "Outer area 140 minus 2×(2×6÷2) minus 2×(12×4÷2) = 80."),
        2 => ("Edulytiks independent Cool-down, not publisher 6.3: two slanted parallelograms have base8 and perpendicular height3, but different offsets. Find each area in sq units.",
              "8 × 3 = 24. Teacher confirms both distinct diagrams and right angles."),
        _ => throw new ArgumentOutOfRangeException(nameof(v))
    };
}
