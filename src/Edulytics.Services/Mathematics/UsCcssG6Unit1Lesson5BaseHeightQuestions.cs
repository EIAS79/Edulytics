using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Original IM/OUR Grade 6 U01:L05 source-specific numeric Practice. The
/// publisher's diagrams, algebraic explanations, freehand height construction,
/// and choice of corresponding base must also receive teacher assessment.
/// Never substitute AccessIM v360 or claim a number proves a student's drawing.
/// </summary>
internal static class UsCcssG6Unit1Lesson5BaseHeightQuestions
{
    public const string Area = "usccss.g6.u01.l05.parallelogram_base_height_area";
    public const string Height = "usccss.g6.u01.l05.corresponding_height";
    public const string ValidHeight = "usccss.g6.u01.l05.perpendicular_height_claim";
    public const string Scale = "usccss.g6.u01.l05.area_scaling";
    public const string SmallTriangle = "usccss.g6.u01.l05.small_triangle_fraction";

    internal sealed record Problem(string Family, string Prompt, string Explanation,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) =>
        family is Area or Height or ValidHeight or Scale or SmallTriangle;

    public static Problem Build(string family, Random random)
    {
        if (!Supports(family))
            throw new ArgumentException("Unknown first-edition IM Grade 6 U01:L05 family", nameof(family));
        ArgumentNullException.ThrowIfNull(random);
        var variant = random.Next(Count(family));
        var parameters = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["variant"] = variant
        };
        var (prompt, explanation) = family switch
        {
            Area => AreaProblem(variant),
            Height => HeightProblem(variant),
            ValidHeight => ValidProblem(variant),
            Scale => ScaleProblem(variant),
            _ => TriangleProblem(variant)
        };
        return new Problem(family, prompt,
            explanation + " A teacher checks the student's perpendicular " +
            "construction, selected base, two corresponding pairs, written formula " +
            "and explanatory diagrams; numeric correctness alone is insufficient.",
            AssessmentItemType.Numeric, parameters);
    }

    public static string Solve(string family, IReadOnlyDictionary<string, int> parameters)
    {
        if (!Valid(family, parameters))
            throw new ArgumentException("Invalid source base-height parameters", nameof(parameters));
        var variant = parameters["variant"];
        return family switch
        {
            Area => AreaAnswer(variant).ToString(CultureInfo.InvariantCulture),
            Height => variant switch
            {
                0 => "4", 1 => "4.8", 2 => "4", 3 => "6", _ => throw new ArgumentOutOfRangeException()
            },
            ValidHeight => (variant switch
            {
                0 => 0, 1 => 1, 2 => 0, 3 => 1, 4 => 0, 5 => 1, 6 => 0,
                7 => 1, 8 => 0, 9 => 1, 10 => 1, 11 => 0,
                _ => throw new ArgumentOutOfRangeException()
            }).ToString(CultureInfo.InvariantCulture),
            Scale => (variant switch
            {
                0 => 2, 1 => 3, 2 => 100, 3 => 4, 4 => 9, 5 => 10000,
                _ => throw new ArgumentOutOfRangeException()
            }).ToString(CultureInfo.InvariantCulture),
            SmallTriangle => variant == 0 ? "1/3" : "27",
            _ => throw new ArgumentException("Unrecognized family", nameof(family))
        };
    }

    public static bool Verify(string family, IReadOnlyDictionary<string, int>? parameters,
        string? response)
    {
        if (!Valid(family, parameters) || string.IsNullOrWhiteSpace(response) ||
            response != response.Trim())
            return false;
        var canonical = Solve(family, parameters!);
        if (family == Height || family == SmallTriangle)
        {
            // Fractions equivalent to 1/3 or 4.8 are accepted exactly;
            // nonterminating decimal approximations of 1/3 are not accepted.
            var (numerator, denominator) = family == SmallTriangle &&
                parameters!["variant"] == 0
                    ? (1L, 3L)
                    : family == Height && parameters!["variant"] == 1
                        ? (24L, 5L)
                        : (long.Parse(canonical, CultureInfo.InvariantCulture), 1L);
            return ExactRational(response, numerator, denominator);
        }
        return string.Equals(response, canonical, StringComparison.Ordinal);
    }

    private static bool ExactRational(string response, long n, long d)
    {
        var values = response.Split('/');
        if (values.Length == 2 &&
            long.TryParse(values[0], NumberStyles.None, CultureInfo.InvariantCulture, out var num) &&
            long.TryParse(values[1], NumberStyles.None, CultureInfo.InvariantCulture, out var den) &&
            den > 0 && den <= 1_000_000 && num >= 0 && num <= 1_000_000)
            return num * d == n * den;

        if (values.Length != 1 ||
            !decimal.TryParse(response, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out var value) || value < 0)
            return false;
        return value * d == n;
    }

    private static bool Valid(string? family, IReadOnlyDictionary<string, int>? parameters) =>
        Supports(family) && parameters is { Count: 1 } &&
        parameters.TryGetValue("variant", out int variant) &&
        variant >= 0 && variant < Count(family!);

    private static int Count(string family) => family switch
    {
        Area => 121,
        Height => 4,
        ValidHeight => 12,
        Scale => 6,
        SmallTriangle => 2,
        _ => 0
    };

    private static int AreaAnswer(int variant) => variant switch
    {
        0 => 42, // 5.1: 7 x 6 source shaded grid
        1 => 24, // 5.3 A: 4 x 6
        2 => 15, // 5.3 B: 5 x 3
        3 => 6,  // 5.3 C: 2 x 3
        4 => 8,  // 5.3 D: vertical base 4, horizontal height 2
        5 => 8,  // Practice 3A: 4 x 2
        6 => 10, // Practice 3B: 5 x 2
        7 => 8,  // Practice 3C: 2 x 4
        8 => 36, // Practice 5A: 9 x 4
        9 => 20, // Practice 5B: 5 x 4
        10 => 24, // Source summary: base 6, height 4 OR base 5, height 4.8
        _ => (2 + ((variant - 11) % 11)) *
             (2 + ((variant - 11) / 11)) // 110 additional source-mechanic area variants
    };

    private static (string, string) AreaProblem(int variant)
    {
        var (description, reason) = variant switch
        {
            0 => ("Lesson 5.1 original shaded grid: base 7 and perpendicular height 6 square-grid units.",
                  "Compare the two different perpendicular cuts and their identical 7-by-6 rectangles."),
            1 => ("Lesson 5.3 Figure A: a 4-unit horizontal base and 6-unit perpendicular height.",
                  "Figure A is a 4-by-6 grid rectangle, also a parallelogram."),
            2 => ("Lesson 5.3 Figure B: a 5-unit horizontal base and 3-unit perpendicular height.",
                  "Figure B is a slanted 5-by-3 area model."),
            3 => ("Lesson 5.3 Figure C: a 2-unit horizontal base and 3-unit perpendicular height.",
                  "Figure C is a narrow 2-by-3 area model."),
            4 => ("Lesson 5.3 Figure D: a 4-unit VERTICAL base and 2-unit HORIZONTAL perpendicular height.",
                  "A base need not be horizontal: the 4-by-2 related rectangle gives the area."),
            5 => ("Original practice problem 3 Figure A: base 4, corresponding perpendicular height 2.",
                  "The area is the product of a base and its corresponding height."),
            6 => ("Original practice problem 3 Figure B: base 5, corresponding perpendicular height 2.",
                  "The area is the product of a base and its corresponding height."),
            7 => ("Original practice problem 3 Figure C: base 2, corresponding perpendicular height 4.",
                  "The area is the product of a base and its corresponding height."),
            8 => ("Original practice problem 5 Figure A: base 9 cm, perpendicular height 4 cm.",
                  "Multiply 9 cm by 4 cm, obtaining square centimeters."),
            9 => ("Original practice problem 5 Figure B: base 5 cm, perpendicular height 4 cm.",
                  "Multiply 5 cm by 4 cm, obtaining square centimeters."),
            10 => ("Original student lesson summary: base 6 and its perpendicular height 4.",
                   "The other corresponding pair is base 5 and height 4.8; both products are 24."),
            _ => ("New source-mechanic variant: a parallelogram has a base of " +
                   (2 + ((variant - 11) % 11)).ToString(CultureInfo.InvariantCulture) +
                   " units and a CORRESPONDING PERPENDICULAR height of " +
                   (2 + ((variant - 11) / 11)).ToString(CultureInfo.InvariantCulture) + " units.",
                   "Only the height perpendicular to the chosen base yields the rectangle's area.")
        };
        return ("Find the area, in square units (or cm² when shown), of this figure: " +
                description, reason);
    }

    private static (string, string) HeightProblem(int variant) => variant switch
    {
        0 => ("Original practice problem 4: for a parallelogram with one base 6 " +
              "units and another side 5 units, a 4-unit height corresponds to " +
              "base 6. What is the corresponding height for the 6-unit base?",
              "The dashed perpendicular segment corresponding to base 6 is 4."),
        1 => ("Original lesson summary: area 24 square units is found with " +
              "base 5 units. Find its corresponding perpendicular height; " +
              "an exact decimal or fraction is acceptable.",
              "24 divided by 5 is 4.8, not the 4-unit height belonging to base 6."),
        2 => ("Edulytiks-original source-skill exit check: area 36 cm², " +
              "chosen base 9 cm. What perpendicular height in cm corresponds?",
              "36 divided by 9 is 4, so 9 × 4 = 36."),
        3 => ("Edulytiks-original exit check: the same area 36 cm², " +
              "chosen base 6 cm. What perpendicular height in cm corresponds?",
              "36 divided by 6 is 6, so 6 × 6 = 36."),
        _ => throw new ArgumentOutOfRangeException(nameof(variant))
    };

    private static (string, string) ValidProblem(int variant) => variant switch
    {
        0 => ("Original 5.2: Only a HORIZONTAL side may be a base. True? 1 yes, 0 no.",
              "Any of four sides may be the base."),
        1 => ("Original 5.2: Any side of a parallelogram may be its base. True? 1 yes, 0 no.",
              "The base can be any chosen side."),
        2 => ("Original 5.2: A valid height may form any angle with its corresponding base. True? 1 yes, 0 no.",
              "The corresponding height must meet the extended base line at 90 degrees."),
        3 => ("Original 5.2: A base and its corresponding height must be perpendicular. True? 1 yes, 0 no.",
              "A corresponding height is the perpendicular distance to the opposite base line."),
        4 => ("Original 5.2: A valid height can be drawn only INSIDE a parallelogram. True? 1 yes, 0 no.",
              "A valid height may lie outside a slanted parallelogram."),
        5 => ("Original 5.2: An outside height may be valid if perpendicular to the extended base. True? 1 yes, 0 no.",
              "Perpendicularity and correct opposite parallel lines matter, not location."),
        6 => ("Original 5.2: The chosen base cannot be extended to meet a height. True? 1 yes, 0 no.",
              "Extending the base line can make a valid outside height visible."),
        7 => ("Original 5.2 five-figure diagram A: dashed height meets horizontal base at a right angle. Correct? 1 yes, 0 no.",
              "The segment is perpendicular and connects the opposite base lines."),
        8 => ("Original 5.2 five-figure diagram B: base is SLANTED, but the dashed segment is vertical and NOT perpendicular to that base. Valid? 1 yes, 0 no.",
              "A vertical segment is not a valid height for a diagonal base unless perpendicular."),
        9 => ("Original 5.2 five-figure diagram C: dashed height is OUTSIDE the parallelogram but meets the extended base at 90 degrees. Correct? 1 yes, 0 no.",
              "An exterior perpendicular is valid."),
        10 => ("Original 5.2 five-figure diagram D: base is VERTICAL and the dashed height is horizontal between opposite sides. Correct? 1 yes, 0 no.",
               "Horizontal is perpendicular to the chosen vertical base."),
        11 => ("Original 5.2 five-figure diagram E: height is drawn along a SLANTED side rather than perpendicular to a horizontal base. Correct? 1 yes, 0 no.",
               "Slanted side is not the perpendicular height."),
        _ => throw new ArgumentOutOfRangeException(nameof(variant))
    };

    private static (string, string) ScaleProblem(int variant)
    {
        int multiplier = variant switch { 0 => 2, 1 => 3, 2 => 100, 3 => 2,
            4 => 3, 5 => 100, _ => throw new ArgumentOutOfRangeException(nameof(variant)) };
        bool both = variant >= 3;
        return ("Original 5.3 extension: " + (both
                    ? "BOTH base and its corresponding height"
                    : "the perpendicular HEIGHT ONLY, with the base unchanged") +
                " are multiplied by " + multiplier.ToString(CultureInfo.InvariantCulture) +
                ". By what factor does the parallelogram's AREA change? Enter the multiplier as an integer.",
                both ? "The area factor is the square of the dimension scale factor."
                     : "Only one factor in the base-times-height product changed.");
    }

    private static (string, string) TriangleProblem(int variant) => variant switch
    {
        0 => ("Original practice problem 7: one square meter is divided into " +
              "9 identical small squares, and each into two identical triangles. " +
              "What is the exact area, in square meters, of SIX of those triangles? " +
              "Enter an exact fraction.",
              "Each small triangle is 1/18 m². Six have area 6/18 = 1/3 m²."),
        1 => ("Original practice problem 7: the same small triangles each " +
              "cover 1/18 square meter. How MANY are required to cover exactly " +
              "1½ square meters, without gaps or overlaps? Enter an integer.",
              "1½ divided by 1/18 gives 27 triangles."),
        _ => throw new ArgumentOutOfRangeException(nameof(variant))
    };
}
