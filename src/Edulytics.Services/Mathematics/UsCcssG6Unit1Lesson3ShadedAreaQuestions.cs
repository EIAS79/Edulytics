using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Original IM 6-8 G6 U01:L03 (Reasoning to Find Area) source-specific numeric
/// adapters for known shaded areas and area conservation. No geometry drawing
/// or written explanation is auto-scored by these numeric families.
/// </summary>
internal static class UsCcssG6Unit1Lesson3ShadedAreaQuestions
{
    public const string Shaded = "usccss.g6.u01.l03.source_shaded_area";
    public const string Conservation = "usccss.g6.u01.l03.area_conservation";
    internal sealed record Problem(string Family, string Prompt, string Explanation,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) => family is Shaded or Conservation;

    public static Problem Build(string family, Random random)
    {
        if (!Supports(family))
            throw new ArgumentException("Unknown original IM area family", nameof(family));
        ArgumentNullException.ThrowIfNull(random);
        var variant = random.Next(6);
        var parameters = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["variant"] = variant
        };
        var (prompt, explanation) = family == Shaded
            ? ShadedProblem(variant)
            : ConservationProblem(variant);

        return new Problem(family, prompt,
            explanation + " A teacher checks the student's diagram, cutting or " +
            "rearrangement, square-area units and written justification; one " +
            "correct number never verifies the shape construction.",
            AssessmentItemType.Numeric, parameters);
    }

    public static string Solve(string family, IReadOnlyDictionary<string, int> parameters)
    {
        if (!Valid(family, parameters))
            throw new ArgumentException("Invalid original IM G6 L03 parameters", nameof(parameters));
        var variant = parameters["variant"];
        return (family == Shaded ? ShadedArea(variant) : EqualArea(variant))
            .ToString(CultureInfo.InvariantCulture);
    }

    public static bool Verify(string family, IReadOnlyDictionary<string, int>? parameters,
        string? response)
    {
        if (!Valid(family, parameters) || string.IsNullOrEmpty(response))
            return false;
        if (family == Conservation)
            return (response is "0" or "1") &&
                response == EqualArea(parameters!["variant"]).ToString(CultureInfo.InvariantCulture);
        if (!int.TryParse(response, NumberStyles.None,
                CultureInfo.InvariantCulture, out var number))
            return false;
        return number == ShadedArea(parameters!["variant"]);
    }

    private static bool Valid(string? family, IReadOnlyDictionary<string, int>? parameters) =>
        Supports(family) && parameters is { Count: 1 } &&
        parameters.TryGetValue("variant", out var variant) &&
        variant is >= 0 and < 6;

    private static int ShadedArea(int variant) => variant switch
    {
        0 => 6 * 6 - 3 * 3, // Lesson 3.2: 6x6 grid, missing 3x3 square
        1 => 2 * (5 * 3) / 2, // Lesson 3.3 A: two 5cm-by-3cm right triangles
        2 => 4 * (2 * 4) / 2, // Lesson 3.3 B: four 2cm-by-4cm right triangles
        3 => 5 * 5 - 2 * 2, // Lesson 3.3 C: 5cm square minus 2cm square
        4 => 5 * 2 + 4 * 2 + 2 * 2, // Lesson practice: three known rectangles
        5 => 80 * 40, // Lesson practice: bathroom wall area unchanged by tile choice
        _ => throw new ArgumentOutOfRangeException(nameof(variant))
    };

    private static int EqualArea(int variant) => variant switch
    {
        0 => 1, // Lesson 3.1: rearrange Figure B to match A
        1 => 0, // Practice: two 2-inch tiles vs one 4-inch tile
        2 => 1, // Practice: four 4-inch tiles vs one 8-inch tile
        3 => 1, // Practice: wall area invariant under square tile size
        4 => 1, // Practice: Noah's plots move same triangle without loss
        5 => 0, // Subtracting a 2x2 hole from a 5x5 square changes shaded area
        _ => throw new ArgumentOutOfRangeException(nameof(variant))
    };

    private static (string Prompt, string Explanation) ShadedProblem(int variant) =>
        variant switch
        {
            0 => ("Lesson 3.2, shaded Figure B: a 6-by-6 unit-square grid has " +
                  "an unshaded 3-by-3 square. What is the shaded area in square units?",
                  "Find the enclosing square's area and subtract the unshaded inner square."),
            1 => ("Lesson 3.3, Figure A: two non-overlapping right triangles each " +
                  "have perpendicular sides of 5 cm and 3 cm. Find their combined area in cm².",
                  "Decompose the parallelogram into two congruent half-rectangles."),
            2 => ("Lesson 3.3, Figure B: four non-overlapping right triangles " +
                  "each have perpendicular legs of 2 cm and 4 cm. What is the shaded area in cm²?",
                  "Find each half-rectangle triangle area, then multiply by four."),
            3 => ("Lesson 3.3, Figure C: a square of side 5 cm surrounds an " +
                  "unshaded square of side 2 cm. What is the shaded area in cm²?",
                  "Subtract the inner square from the enclosing square."),
            4 => ("Original lesson practice: a shaded shape decomposes into " +
                  "three non-overlapping rectangles of 5×2, 4×2 and 2×2 unit squares. " +
                  "Find its area in square units.",
                  "Use area additivity for the three non-overlapping rectangles."),
            5 => ("Original lesson practice: a bathroom wall measures " +
                  "80 inches by 40 inches. What is its area in square inches?",
                  "The area of the rectangle is length times width, regardless of tile size."),
            _ => throw new ArgumentOutOfRangeException(nameof(variant))
        };

    private static (string Prompt, string Explanation) ConservationProblem(int variant) =>
        variant switch
        {
            0 => ("Lesson 3.1: Figure B is cut and rearranged without removing " +
                  "pieces, gaps or overlaps until it matches Figure A. " +
                  "Are their shaded areas equal? Enter 1 for yes or 0 for no.",
                  "Moving intact pieces without overlap preserves their total area."),
            1 => ("Original practice: do two square tiles with 2-inch sides " +
                  "cover the same area as one square tile with a 4-inch side? " +
                  "Enter 1 for yes or 0 for no.",
                  "Compare two copies of 2×2 with one copy of 4×4."),
            2 => ("Original practice: do four square tiles with 4-inch sides " +
                  "cover the same area as one square tile with an 8-inch side? " +
                  "Enter 1 for yes or 0 for no.",
                  "Compare four copies of 4×4 with one copy of 8×8."),
            3 => ("Original practice: when a wall of 80×40 inches is tiled fully, " +
                  "without gaps or overlaps, does its total covered area stay " +
                  "unchanged whether 8-inch, 4-inch or 2-inch square tiles are used? " +
                  "Enter 1 for yes or 0 for no.",
                  "The wall area does not depend on the chosen square tile size."),
            4 => ("Original practice: two same-height plots differ only because " +
                  "one triangular piece was moved from one side to the other without " +
                  "loss or overlap. Do they have equal areas? Enter 1 for yes or 0 for no.",
                  "Rearranging the identical triangular piece conserves area."),
            5 => ("Compared with a solid square of side 5 cm, a square of side " +
                  "5 cm with a 2-by-2 cm unshaded hole has the same SHADED area. " +
                  "Is that true? Enter 1 for true or 0 for false.",
                  "A missing inner square reduces the shaded area."),
            _ => throw new ArgumentOutOfRangeException(nameof(variant))
        };
}
