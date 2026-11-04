using System.Globalization;
using Edulytics.Core.Enums;
using Area = Edulytics.Core.Mathematics.Practice.UsCcssG5U03L02UnitFractionArea;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Lesson-local candidate ONLY: not registered in shared exact engine until
/// a new Admin three-path grant. Digital fractions of the ORIGINAL whole are
/// machine-checkable; original source drawing, Priya's explanation and
/// the publisher's separate shaded picture remain TEACHER_REVIEW_REQUIRED.
/// </summary>
public static class UsCcssG5U03L02UnitFractionQuestions
{
    public const string ProductFamily = "usccss.g5.u03.l02.unit_fraction_of_unit_fraction";
    public const string CompareFamily = "usccss.g5.u03.l02.same_original_whole_order_comparison";

    public sealed record Problem(string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);

    private static readonly (int First, int Second)[] Cases =
        [(3, 4), (4, 3), (2, 5), (5, 2)];

    public static bool Supports(string? family) =>
        family is ProductFamily or CompareFamily;

    public static Problem Build(string family, Random random)
    {
        if (!Supports(family))
            throw new InvalidOperationException("Not a Grade 5 U03:L02 digital family.");
        ArgumentNullException.ThrowIfNull(random);
        IReadOnlyDictionary<string, int> p = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["case"] = random.Next(4), ["layout"] = random.Next(4),
            ["slot"] = random.Next(3), ["comparison"] = random.Next(4),
            ["differentWhole"] = random.Next(2)
        };
        if (!TryCandidate(family, p, out var first, out var second) || first is null)
            throw new InvalidOperationException("Invalid original-whole grid.");
        var f = first.FirstDenominator;
        var s = first.SecondDenominator;
        var layout = $"with {(first.FirstCut == Area.Direction.Vertical ? "vertical" : "horizontal")} "+
            $"first cuts and {(first.SecondCut == Area.Direction.Vertical ? "vertical" : "horizontal")} second cuts";
        if (family == ProductFamily)
        {
            string correct = $"1/{first.DenominatorOfOriginalWhole}";
            string wrongFirst = $"1/{f}";
            string wrongSecond = $"1/{s}";
            string[] options = p["slot"] switch
            {
                0 => [correct, wrongFirst, wrongSecond],
                1 => [wrongFirst, correct, wrongSecond],
                _ => [wrongFirst, wrongSecond, correct]
            };
            string prompt = $"The ORIGINAL WHOLE is a complete 60 by 60 square. Divide it into {f} "+
                $"equal parts, then divide the FIRST selected part into {s} equal pieces, {layout}. "+
                $"Shade exactly ONE of those final equal pieces. What fraction of the ORIGINAL WHOLE "+
                $"is shaded? Select 1: {options[0]}; 2: {options[1]}; 3: {options[2]}. "+
                "Teacher separately checks learner-created drawings and explanations.";
            string explanation = $"One of {s} equal parts OF one {f}th means "+
                $"(1/{s}) × (1/{f}) = 1/{first.DenominatorOfOriginalWhole} "+
                "of the ORIGINAL WHOLE, not 1/"+s+" or 1/"+f+". "+
                "A numeric choice does not complete the original open classroom activity.";
            return new Problem(family, prompt, explanation, AssessmentItemType.Numeric, p);
        }

        if (second is null)
            throw new InvalidOperationException("Missing second comparison grid.");
        string relation = p["differentWhole"] == 0
            ? "the SAME original 60 by 60 square"
            : "a DIFFERENT original 120 by 60 rectangle";
        string comparePrompt = $"Two diagrams depict digital equal-cell partitions. "+
            $"A shades one of {s} equal parts of one {f}th of the original 60 by 60 whole. "+
            $"B shades one of {second.SecondDenominator} equal parts of one "+
            $"{second.FirstDenominator}th of {relation}. "+
            "Do A and B show the SAME fraction OF THE SAME ORIGINAL WHOLE? "+
            "Answer 1=YES or 0=NO. Teacher separately reviews the drawn "+
            "partitions, original-source shaded picture, and Priya's reasoning.";
        string compareExplanation = "The selected region must be measured against the "+
            "same original whole. Reverse partition order can preserve 1/12 or 1/10; "+
            "different originals or unlike denominators do not establish the same "+
            "fraction of the same original whole. Drawings and explanations require teacher review.";
        return new Problem(family, comparePrompt, compareExplanation,
            AssessmentItemType.Numeric, p);
    }

    public static string Solve(string family, IReadOnlyDictionary<string, int> p)
    {
        if (!TryCandidate(family, p, out var first, out var second) || first is null)
            throw new InvalidOperationException("Invalid source-bounded unit fraction item.");
        if (family == ProductFamily)
            return (p["slot"] + 1).ToString(CultureInfo.InvariantCulture);
        return Area.RepresentsSameAreaOfSameOriginalWhole(first, second) ? "1" : "0";
    }

    public static bool Verify(string family, IReadOnlyDictionary<string, int> p, string? answer) =>
        !string.IsNullOrEmpty(answer) &&
        TryCandidate(family, p, out var first, out var second) && first is not null &&
        string.Equals(answer, family == ProductFamily
            ? (p["slot"] + 1).ToString(CultureInfo.InvariantCulture)
            : Area.RepresentsSameAreaOfSameOriginalWhole(first, second) ? "1" : "0",
            StringComparison.Ordinal);

    private static bool TryCandidate(
        string? family, IReadOnlyDictionary<string, int>? p,
        out Area.Grid? first, out Area.Grid? second)
    {
        first = null; second = null;
        if (!Supports(family) || p is null || p.Count != 5 ||
            !p.TryGetValue("case", out int c) || c is < 0 or > 3 ||
            !p.TryGetValue("layout", out int layout) || layout is < 0 or > 3 ||
            !p.TryGetValue("slot", out int slot) || slot is < 0 or > 2 ||
            !p.TryGetValue("comparison", out int comparison) || comparison is < 0 or > 3 ||
            !p.TryGetValue("differentWhole", out int differentWhole) ||
            differentWhole is < 0 or > 1)
            return false;

        var (firstParts, secondParts) = Cases[c];
        if (!Area.TryCreate(60, 60, firstParts, secondParts,
            (layout & 1) == 0 ? Area.Direction.Horizontal : Area.Direction.Vertical,
            (layout & 2) == 0 ? Area.Direction.Vertical : Area.Direction.Horizontal,
            out first) || first is null || !Area.IsValidDigitalGrid(first, first.Cells))
            return false;

        if (family == ProductFamily)
            return true;

        var (otherFirst, otherSecond) = Cases[comparison];
        int width = differentWhole == 0 ? 60 : 120;
        return Area.TryCreate(width, 60, otherFirst, otherSecond,
            (layout & 1) == 0 ? Area.Direction.Vertical : Area.Direction.Horizontal,
            (layout & 2) == 0 ? Area.Direction.Horizontal : Area.Direction.Vertical,
            out second) && second is not null &&
            Area.IsValidDigitalGrid(second, second.Cells);
    }
}
