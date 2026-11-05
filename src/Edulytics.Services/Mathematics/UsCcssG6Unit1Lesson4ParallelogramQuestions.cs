using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// First-edition original IM 6-8 Grade 6 U01:L04 numeric evidence:
/// both pairs of parallel sides; decomposing/rearranging and enclosing
/// parallelograms. Students' actual drawings and area proofs stay human graded.
/// Source: 06_Grade_6_OUR_IM/1/4 (Kendall Hunt/OUR, NOT AccessIM v360).
/// </summary>
internal static class UsCcssG6Unit1Lesson4ParallelogramQuestions
{
    public const string ShadedArea = "usccss.g6.u01.l04.parallelogram_exact_area";
    public const string TwoPairs = "usccss.g6.u01.l04.two_pairs_parallel";
    public const string AreaInvariant = "usccss.g6.u01.l04.equal_area_rearrangement";

    internal sealed record Problem(string Family, string Prompt, string Explanation,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) =>
        family is ShadedArea or TwoPairs or AreaInvariant;

    public static Problem Build(string family, Random random)
    {
        if (!Supports(family))
            throw new ArgumentException("Unknown original IM parallelogram family", nameof(family));
        ArgumentNullException.ThrowIfNull(random);
        int variant = random.Next(family == ShadedArea ? 8 : 6);
        var parameters = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["variant"] = variant
        };
        var (prompt, explanation) = family switch
        {
            ShadedArea => AreaProblem(variant),
            TwoPairs => ClassificationProblem(variant),
            _ => ConservationProblem(variant)
        };
        return new Problem(family, prompt,
            explanation + " The teacher separately checks all constructions, " +
            "perpendicular heights, translated pieces, gaps and written reasoning; " +
            "a correct number does not certify a student diagram.",
            AssessmentItemType.Numeric, parameters);
    }

    public static string Solve(string family, IReadOnlyDictionary<string, int> parameters)
    {
        if (!Valid(family, parameters))
            throw new ArgumentException("Invalid original IM G6 L04 parameters", nameof(parameters));
        int variant = parameters["variant"];
        int answer = family switch
        {
            ShadedArea => Area(variant),
            TwoPairs => variant < 3 ? 1 : 0,
            _ => Invariant(variant)
        };
        return answer.ToString(CultureInfo.InvariantCulture);
    }

    public static bool Verify(string family, IReadOnlyDictionary<string, int>? parameters,
        string? response)
    {
        if (!Valid(family, parameters) || string.IsNullOrWhiteSpace(response))
            return false;
        if (family != ShadedArea && response is not ("0" or "1"))
            return false;
        return string.Equals(response, Solve(family, parameters!),
            StringComparison.Ordinal);
    }

    private static bool Valid(string? family, IReadOnlyDictionary<string, int>? parameters) =>
        Supports(family) && parameters is { Count: 1 } &&
        parameters.TryGetValue("variant", out int variant) &&
        variant >= 0 && variant < (family == ShadedArea ? 8 : 6);

    private static int Area(int variant) => variant switch
    {
        0 => 4 * 3, // Source Cool-down: cut-and-move 4x3 rectangle
        1 => 4 * 4, // Change perpendicular distance from 3 to 4
        2 => 7 * 4, // Existing source-aligned first-party 7x4 example
        3 => 6 * 4, // Existing first-party 6x4 teacher Cool-down P and Q
        4 => 4 * 6, // Source 4.3 C useful perpendicular dimensions, not 4.5 slant
        5 => 8 * 4 - 2 * (1 * 4 / 2), // Enclose and subtract two 1x4 right triangles
        6 => 5 * 3 - (1 * 3), // two half-area triangles combine into a 1x3 rectangle; no integer truncation
        7 => 11 * 4 - 2 * (5 * 4 / 2), // First-party Cool-down Q enclosure
        _ => throw new ArgumentOutOfRangeException(nameof(variant))
    };

    private static int Invariant(int variant) => variant switch
    {
        0 => 1, // Move end triangle to rectangle
        1 => 1, // Shift slant while keeping true perpendicular height 3
        2 => 0, // Increase true height from 3 to 4
        3 => 1, // Enclosure subtraction matches cut/rearrange area
        4 => 0, // Treat slanted side instead of perpendicular height as area measure
        5 => 1, // Same 6x4 dimensions with different top-vertex offset
        _ => throw new ArgumentOutOfRangeException(nameof(variant))
    };

    private static (string, string) AreaProblem(int variant) => variant switch
    {
        0 => ("Original 4.4 teaching diagram: a parallelogram is cut and translated " +
              "to form a rectangle 4 unit squares wide and 3 unit squares high. " +
              "Find the original parallelogram's area in square units.",
              "The nonoverlapping parts rearrange into a 4-by-3 rectangle; no area is lost."),
        1 => ("Source-aligned graph-paper construction: a parallelogram with two " +
              "parallel horizontal edges each 4 units long has perpendicular " +
              "distance 4 units. Cut and translate a corner to form a rectangle. " +
              "What area results?",
              "A complete rearrangement forms a 4-by-4 rectangle."),
        2 => ("Existing original lesson graph-paper example: a 7-unit bottom " +
              "edge and perpendicular distance of 4 units form a parallelogram. " +
              "Rearrange its end triangle to a rectangle. Find its area.",
              "The rearranged rectangle covers 7-by-4 unit squares."),
        3 => ("Edulytiks original lesson Cool-down construction P: vertices " +
              "(0,0),(6,0),(8,4),(2,4). Translate a triangular end to form " +
              "a 6-by-4 rectangle. What is its area in square units?",
              "The translated pieces fill the related 6-by-4 rectangle exactly."),
        4 => ("Original lesson 4.3 parallelogram C has a useful 4-unit segment " +
              "and perpendicular 6-unit segment; its slanted side measures 4.5. " +
              "Cut-and-move it to a rectangle. What is its area in square units?",
              "The source's useful perpendicular dimensions are 4 and 6, " +
              "not the slanted 4.5-unit side."),
        5 => ("Original-lesson method: a parallelogram sits in an enclosing " +
              "8-by-4 rectangle. Two exterior right triangles each have " +
              "perpendicular legs 1 and 4. Subtract them to find its area.",
              "Enclosing rectangle 32 minus the two exterior triangle areas 2 each gives 28."),
        6 => ("Original-lesson cut-and-enclose model: an outer 5-by-3 rectangle " +
              "contains a parallelogram and two exterior right triangles " +
              "with legs 1 and 3 each. Find the parallelogram's shaded area.",
              "Outer rectangle 15 minus two triangles of area 1.5 gives 12."),
        7 => ("Edulytiks original Cool-down construction Q: vertices " +
              "(0,0),(6,0),(11,4),(5,4). An 11-by-4 enclosure has two " +
              "exterior right triangles each with legs 5 and 4. " +
              "Find the parallelogram's area.",
              "Enclosure 44 minus two ten-unit triangles gives 24."),
        _ => throw new ArgumentOutOfRangeException(nameof(variant))
    };

    private static (string, string) ClassificationProblem(int variant) => variant switch
    {
        0 => ("Original lesson 4.1 Figure A: a slanted four-sided polygon " +
              "has BOTH pairs of opposite sides parallel. " +
              "Is it a parallelogram? Enter 1 for yes or 0 for no.",
              "A quadrilateral with both pairs of opposite sides parallel qualifies."),
        1 => ("Original lesson 4.1 Figure B: a rectangle has both opposite-side " +
              "pairs parallel. Is it a parallelogram? 1 yes, 0 no.",
              "Rectangles are a special class of parallelograms."),
        2 => ("Original lesson 4.1 Figure C: a square shown at an angle has " +
              "both opposite-side pairs parallel. Is it a parallelogram? 1 yes, 0 no.",
              "Orientation does not change a square's parallel opposite sides."),
        3 => ("Original lesson 4.1 Figure D is a quadrilateral with NO " +
              "opposite parallel side pairs. Is it a parallelogram? 1 yes, 0 no.",
              "Neither of the two required opposite pairs is parallel."),
        4 => ("Original lesson 4.1 Figure E has FIVE sides, although some " +
              "sides have equal lengths. Is it a parallelogram? 1 yes, 0 no.",
              "Every parallelogram must be a quadrilateral with exactly four sides."),
        5 => ("Original lesson 4.1 Figure F has only ONE pair of parallel " +
              "opposite sides. Is it a parallelogram? 1 yes, 0 no.",
              "One pair of parallel opposite sides is insufficient."),
        _ => throw new ArgumentOutOfRangeException(nameof(variant))
    };

    private static (string, string) ConservationProblem(int variant) => variant switch
    {
        0 => ("When a right triangular end is cut from a parallelogram and " +
              "translated without gaps or overlaps to the other end, does " +
              "its area equal the resulting rectangle? 1 yes, 0 no.",
              "Rigidly moving one unchanged piece conserves the sum of areas."),
        1 => ("Two original-lesson parallelograms share horizontal edges " +
              "length 4 and perpendicular height 3 but have different slants. " +
              "Do their areas agree? 1 yes, 0 no.",
              "Both can be rearranged to the same 4-by-3 rectangle."),
        2 => ("One parallelogram has a 4-unit horizontal edge and perpendicular " +
              "height 3; a second has the same edge but perpendicular height 4. " +
              "Are their areas equal? 1 yes, 0 no.",
              "Changing perpendicular height changes the related rectangle area."),
        3 => ("For the same unmodified parallelogram, do an exact cut-and-move " +
              "into a rectangle and an enclosure-minus-outside-triangles method " +
              "yield equal areas? 1 yes, 0 no.",
              "Both count precisely the same original region once."),
        4 => ("A student multiplies a parallelogram's chosen base by the " +
              "length of its slanted side instead of the perpendicular " +
              "distance. Is that calculation always its correct area? 1 yes, 0 no.",
              "Only the perpendicular distance to the opposite parallel line is suitable."),
        5 => ("Edulytiks original Cool-down: P and Q have horizontal edges " +
              "length 6, perpendicular height 4, different vertex offsets. " +
              "Do their areas agree? 1 yes, 0 no.",
              "Both can be rearranged to 6-by-4 rectangles."),
        _ => throw new ArgumentOutOfRangeException(nameof(variant))
    };
}
