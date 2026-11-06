using System.Globalization;
using Edulytics.Core.Enums;
using Area = Edulytics.Core.Mathematics.Practice.UsCcssG5U03L01AreaModel;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// IM Grade 5 U03:L01: 1/3 or 1/4 of one half OF THE ORIGINAL WHOLE.
/// This adapter only grades bounded, explicitly selected digital subskills.
/// Drawings, explanations and identification of a learner's own whole
/// remain TEACHER_REVIEW_REQUIRED and are never automatically passed.
/// </summary>
internal static class UsCcssG5U03L01FractionOfHalfQuestions
{
    public const string ShadeFamily = "usccss.g5.u03.l01.fraction_of_half_original_whole";
    public const string CompareFamily = "usccss.g5.u03.l01.compare_original_whole_grids";

    internal sealed record Problem(string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) =>
        family is ShadeFamily or CompareFamily;

    public static Problem Build(string family, Random random)
    {
        if (!Supports(family))
            throw new InvalidOperationException("Unsupported Grade 5 U03 L01 exact family.");
        ArgumentNullException.ThrowIfNull(random);
        var parameters = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["parts"] = random.Next(3, 5),
            ["half"] = random.Next(0, 2),
            ["subcut"] = random.Next(0, 2),
            ["slot"] = random.Next(0, 3),
            ["relation"] = random.Next(0, 4),
            ["otherHalf"] = random.Next(0, 2),
            ["otherSubcut"] = random.Next(0, 2)
        };
        if (!TryDiagrams(family, parameters, out var first, out var second) || first is null)
            throw new InvalidOperationException("Invalid Grade 5 area-grid candidate.");

        string cut = parameters["half"] == 0 ? "horizontal (top half)" : "vertical (left half)";
        string subdivisions = parameters["subcut"] == 0 ? "horizontal strips" : "vertical strips";
        string whole = "The ORIGINAL WHOLE is the complete 24 by 24 rectangle; not just the selected half. ";
        if (family == ShadeFamily)
        {
            string correct = $"1/{first.DenominatorOfOriginalWhole}";
            string wrongHalfWhole = $"1/{parameters["parts"]}";
            string wrongDouble = $"2/{parameters["parts"]}";
            var options = parameters["slot"] switch
            {
                0 => new[] { correct, wrongHalfWhole, wrongDouble },
                1 => new[] { wrongHalfWhole, correct, wrongDouble },
                _ => new[] { wrongHalfWhole, wrongDouble, correct }
            };
            string prompt = whole +
                $"Split it in half by a {cut} cut; split that selected half into {parameters["parts"]} equally sized {subdivisions}. " +
                $"Exactly ONE of those equal pieces in the selected half is shaded, with no overlap. " +
                $"What fraction OF THE ORIGINAL WHOLE is shaded? Select 1: {options[0]}; 2: {options[1]}; 3: {options[2]}. " +
                "The teacher assesses the drawing and explanation separately.";
            return new(family, prompt,
                $"The selected half is 1/2 of the original whole, and one of {parameters["parts"]} equal parts of that half is 1/{first.DenominatorOfOriginalWhole} of the original whole. Drawings/explanations require teacher review.",
                AssessmentItemType.Numeric, parameters);
        }

        if (second is null)
            throw new InvalidOperationException("The comparison requires both original-whole diagrams.");
        var otherCut = parameters["otherHalf"] == 0 ? "horizontal top-half" : "vertical left-half";
        string original = second.OriginalWidth == first.OriginalWidth &&
                          second.OriginalHeight == first.OriginalHeight
            ? "the SAME complete 24 by 24 original whole"
            : "a DIFFERENT 48 by 24 original whole";
        string promptCompare = whole +
            $"Diagram A: choose the {cut} and shade one of {parameters["parts"]} equal parts of the selected half. " +
            $"Diagram B: choose the {otherCut} of {original} and shade one of {second.SubdivisionsOfHalf} equal parts of its selected half. " +
            "Do both shaded parts represent the SAME FRACTION of the SAME ORIGINAL WHOLE? Enter 1=YES or 0=NO. " +
            "Draw the comparison and justify it for teacher review.";
        return new(family, promptCompare,
            "Only equally divided, nonoverlapping digital grids representing the identical original whole can be compared. " +
            "The fraction is 1/(2 times the equal subdivisions of the selected half). " +
            "A learner's drawing and explanation require teacher review.",
            AssessmentItemType.Numeric, parameters);
    }

    public static string Solve(string family, IReadOnlyDictionary<string, int> parameters)
    {
        if (!TryDiagrams(family, parameters, out var first, out var second) || first is null)
            throw new InvalidOperationException("Invalid Grade 5 U03:L01 source grid parameters.");
        if (family == ShadeFamily)
            return (parameters["slot"] + 1).ToString(CultureInfo.InvariantCulture);
        return Area.RepresentsSameAreaOfOriginalWhole(first, second) ? "1" : "0";
    }

    public static bool Verify(string family, IReadOnlyDictionary<string, int> parameters, string? answer)
    {
        if (string.IsNullOrEmpty(answer) ||
            !TryDiagrams(family, parameters, out var first, out var second) || first is null)
            return false;
        string expected = family == ShadeFamily
            ? (parameters["slot"] + 1).ToString(CultureInfo.InvariantCulture)
            : Area.RepresentsSameAreaOfOriginalWhole(first, second) ? "1" : "0";
        return string.Equals(answer, expected, StringComparison.Ordinal);
    }

    private static bool TryDiagrams(
        string? family, IReadOnlyDictionary<string, int>? p,
        out Area.Diagram? first, out Area.Diagram? second)
    {
        first = null;
        second = null;
        if (!Supports(family) || p is null || p.Count != 7 ||
            !p.TryGetValue("parts", out var parts) || parts is not (3 or 4) ||
            !p.TryGetValue("half", out var half) || half is < 0 or > 1 ||
            !p.TryGetValue("subcut", out var subcut) || subcut is < 0 or > 1 ||
            !p.TryGetValue("slot", out var slot) || slot is < 0 or > 2 ||
            !p.TryGetValue("relation", out var relation) || relation is < 0 or > 3 ||
            !p.TryGetValue("otherHalf", out var otherHalf) || otherHalf is < 0 or > 1 ||
            !p.TryGetValue("otherSubcut", out var otherSubcut) || otherSubcut is < 0 or > 1)
            return false;

        // Use the existing course-owned oracle: an equal grid covering the
        // entire original whole and shading one cell inside the chosen half.
        if (!Area.TryCreate(24, 24, parts,
            half == 0 ? Area.CutDirection.Horizontal : Area.CutDirection.Vertical,
            subcut == 0 ? Area.CutDirection.Horizontal : Area.CutDirection.Vertical,
            out first) || first is null || !Area.IsValidDigitalGrid(first, first.Cells))
            return false;

        if (family == ShadeFamily)
            return true;

        int otherParts = relation == 1 ? (parts == 3 ? 4 : 3) : parts;
        int otherWidth = relation == 2 ? 48 : 24;
        if (!Area.TryCreate(otherWidth, 24, otherParts,
            otherHalf == 0 ? Area.CutDirection.Horizontal : Area.CutDirection.Vertical,
            otherSubcut == 0 ? Area.CutDirection.Horizontal : Area.CutDirection.Vertical,
            out second) || second is null || !Area.IsValidDigitalGrid(second, second.Cells))
            return false;
        return true;
    }
}
