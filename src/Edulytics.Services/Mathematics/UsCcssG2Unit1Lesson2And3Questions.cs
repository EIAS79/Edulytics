using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Original-source-skill bounded numerical question analogues of IM 2021
/// Grade 2 U01 L02 and L03. Partner games, pupil-authored drawings/equations,
/// explanations and teacher norms cannot be certified by these families.
/// </summary>
internal static class UsCcssG2Unit1Lesson2And3Questions
{
    public const string TenHidden = "usccss.g2.u01.l02.hidden_ten_cubes";
    public const string TenRelated = "usccss.g2.u01.l02.related_ten_equation";
    public const string TenTower = "usccss.g2.u01.l02.match_ten_cube_tower";
    public const string TwentyHidden = "usccss.g2.u01.l03.hidden_twenty_cubes";
    public const string TwentyEquation = "usccss.g2.u01.l03.missing_in_twenty_equation";
    public const string TwentyChallenge = "usccss.g2.u01.l03.extension_unknown_minuend";

    public sealed record Problem(string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) =>
        family is TenHidden or TenRelated or TenTower or
            TwentyHidden or TwentyEquation or TwentyChallenge;

    public static Problem Build(string family, Random random)
    {
        if (!Supports(family))
            throw new InvalidOperationException("Unknown Grade 2 U01 L02-L03 family");

        var p = new Dictionary<string, int>(StringComparer.Ordinal);
        string prompt, solution;
        if (family == TenHidden)
        {
            var visible = random.Next(0, 11);
            p["visible"] = visible;
            prompt = $"A tower has 10 connecting cubes. One partner shows {visible} cubes and " +
                "hides the rest behind their back. How many cubes are hidden? " +
                $"Complete {visible} + ? = 10.";
            solution = "Ten cubes stay in the whole tower. Use ten minus the visible part " +
                "to find the hidden part; check by adding the two parts.";
        }
        else if (family == TenRelated)
        {
            var part = random.Next(0, 11);
            var form = random.Next(4);
            p["part"] = part; p["form"] = form;
            prompt = form switch
            {
                0 => $"Complete the ten-cube equation: {part} + ? = 10.",
                1 => $"Complete the ten-cube equation: ? + {part} = 10.",
                2 => $"Complete the related subtraction equation: 10 − {part} = ?.",
                _ => $"Complete the related subtraction equation: 10 − ? = {part}."
            };
            solution = "All four equations use the same ten-cube whole and two " +
                "complementary parts. Make the addition and subtraction facts agree.";
        }
        else if (family == TenTower)
        {
            // Published source letters represent A = 5+5 through E = 1+9;
            // learner sees original quantities, not copied publisher artwork.
            var blue = random.Next(1, 6);
            var shownYellow = 10 - blue;
            var form = random.Next(2);
            p["blue"] = blue; p["form"] = form;
            prompt = "Five cube-tower drawings each contain 10 cubes: " +
                "A: 5 blue + 5 yellow; B: 4 blue + 6 yellow; " +
                "C: 3 blue + 7 yellow; D: 2 blue + 8 yellow; " +
                "E: 1 blue + 9 yellow. " +
                (form == 0
                    ? $"Which tower matches 10 − {shownYellow} = ? when the unknown is blue cubes?"
                    : $"Which tower matches {blue} + ? = 10 when the first part is blue cubes?") +
                " Enter A=1, B=2, C=3, D=4, or E=5.";
            solution = "Use the specified blue and yellow cube counts, not just any " +
                "different arrangement making ten. Compare with the five labeled towers.";
        }
        else if (family == TwentyHidden)
        {
            var visible = random.Next(0, 21);
            var orientation = random.Next(2);
            p["visible"] = visible; p["orientation"] = orientation;
            prompt = $"There are two connecting-cube towers of 10, so 20 cubes in all. " +
                $"A partner shows {visible} and hides the others. " +
                (orientation == 0 ? $"Complete {visible} + ? = 20." : $"Complete ? + {visible} = 20.") +
                " How many are hidden?";
            solution = "Find the complement to two tens. First make ten with the visible " +
                "part when helpful; then account for the remaining ten.";
        }
        else if (family == TwentyEquation)
        {
            var part = random.Next(0, 21);
            var form = random.Next(5);
            p["part"] = part; p["form"] = form;
            prompt = form switch
            {
                0 => $"Complete the equation: {part} + ? = 20.",
                1 => $"Complete the equation: ? + {part} = 20.",
                2 => $"Complete the equation: 20 − ? = {part}.",
                3 => $"Complete the equation: 20 − {part} = ?.",
                _ => $"Complete the equation: 20 − ? = {20 - part}."
            };
            solution = "The whole is twenty. Relate addition and subtraction, and use " +
                "the two tens to verify the unknown part.";
        }
        else
        {
            var subtract = random.Next(1, 11);
            p["subtract"] = subtract;
            prompt = $"Extension from the source's 'if you have time' challenge: " +
                $"? − {subtract} = 20. What is the starting number? " +
                "It may be greater than 20.";
            solution = "Unlike an unknown part of 20, the unknown starting amount " +
                "is 20 plus the amount taken away. Check with subtraction.";
        }

        return new Problem(family, prompt, solution, AssessmentItemType.Numeric, p);
    }

    public static string Solve(string family, IReadOnlyDictionary<string, int> p)
    {
        if (!Valid(family, p))
            throw new InvalidOperationException("Invalid Grade 2 U01 L02-L03 parameters");

        var answer = family switch
        {
            TenHidden => 10 - p["visible"],
            TenRelated => 10 - p["part"],
            TenTower => 6 - p["blue"],
            TwentyHidden => 20 - p["visible"],
            TwentyEquation => p["form"] == 4
                ? p["part"] : 20 - p["part"],
            TwentyChallenge => 20 + p["subtract"],
            _ => throw new InvalidOperationException("Unsupported Grade 2 source family")
        };
        return answer.ToString(CultureInfo.InvariantCulture);
    }

    public static bool Verify(string family, IReadOnlyDictionary<string, int> p,
        string answer) =>
        Valid(family, p) && string.Equals(answer, Solve(family, p), StringComparison.Ordinal);

    private static bool Valid(string? family, IReadOnlyDictionary<string, int> p)
    {
        if (!Supports(family))
            return false;
        if (family == TwentyChallenge)
            return p.Count == 1 && p.TryGetValue("subtract", out var subtract) &&
                subtract is >= 1 and <= 10;
        if (family == TenTower)
            return p.Count == 2 && p.TryGetValue("blue", out var blue) &&
                blue is >= 1 and <= 5 && p.TryGetValue("form", out var towerForm) &&
                towerForm is 0 or 1;
        if (family == TenHidden || family == TwentyHidden)
            return p.Count == (family == TenHidden ? 1 : 2) &&
                p.TryGetValue("visible", out var visible) &&
                visible >= 0 && visible <= (family == TenHidden ? 10 : 20) &&
                (family == TenHidden || (p.TryGetValue("orientation", out var orientation) &&
                    orientation is 0 or 1));
        if (family == TenRelated)
            return p.Count == 2 && p.TryGetValue("part", out var part10) &&
                part10 is >= 0 and <= 10 && p.TryGetValue("form", out var form10) &&
                form10 is >= 0 and <= 3;
        return p.Count == 2 && p.TryGetValue("part", out var part20) &&
            part20 is >= 0 and <= 20 && p.TryGetValue("form", out var form20) &&
            form20 is >= 0 and <= 4;
    }
}
