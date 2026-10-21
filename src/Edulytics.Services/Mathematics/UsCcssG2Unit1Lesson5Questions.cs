using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Source-bounded digital subskills for IM 2021 Grade 2 U01 L05.
/// The optimization's numeric minimum does not score student-authored
/// two-digit equations or original partner play; either needs teacher review.
/// </summary>
internal static class UsCcssG2Unit1Lesson5Questions
{
    public const string SevenCards = "usccss.g2.u01.l05.seven_cards_two_numbers_min_gap";
    public const string PlaceValue = "usccss.g2.u01.l05.add_within50_place_value";

    public sealed record Problem(string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) => family is SevenCards or PlaceValue;

    public static Problem Build(string family, Random random)
    {
        if (!Supports(family)) throw new InvalidOperationException("Unknown Grade 2 U01 L05 family");
        var p = new Dictionary<string, int>(StringComparer.Ordinal);
        string prompt, solution;
        if (family == SevenCards)
        {
            // The source example includes duplicate 6 cards. Do not impose
            // distinct digit values: four physical card INDICES must differ.
            var cards = Enumerable.Range(0, 7)
                .Select(_ => random.Next(0, 10)).ToArray();
            // At least two nonzero tens cards needed for a legal pair.
            if (cards.Count(x => x != 0) < 2)
            {
                cards[0] = 3;
                cards[1] = 4;
            }
            for (var i = 0; i < 7; i++)
                p[$"card{i}"] = cards[i];
            prompt = $"How Close, target 50. Seven digit cards: {string.Join(", ", cards)}. " +
                "Choose exactly FOUR different physical cards (the same digit may " +
                "appear on two different cards), then arrange them into TWO " +
                "two-digit numbers with no leading zero. Add the two numbers. " +
                "What is the SMALLEST possible distance from 50? Enter the distance.";
            solution = "Choose four card positions and form two real two-digit " +
                "numbers. Check arrangements and compare the absolute gap to 50. " +
                "A second equally good arrangement can also be correct; " +
                "the student's recorded equation remains teacher-assessed.";
        }
        else
        {
            var left = random.Next(10, 41);
            var right = random.Next(1, 51 - left);
            p["left"] = left; p["right"] = right;
            prompt = $"Find {left} + {right} (total no more than 50). " +
                "Use place value, compose a ten, or another valid method. Enter the sum.";
            solution = "Add tens to tens and ones to ones; exchange ten ones " +
                "for a ten when necessary. Check with another method or cubes. " +
                "The original student explains the method to a partner or teacher.";
        }
        return new Problem(family, prompt, solution, AssessmentItemType.Numeric, p);
    }

    public static string Solve(string family, IReadOnlyDictionary<string, int> p)
    {
        if (!Valid(family, p))
            throw new InvalidOperationException("Invalid Grade 2 U01 L05 task parameters");
        if (family == PlaceValue)
            return (p["left"] + p["right"]).ToString(CultureInfo.InvariantCulture);

        var c = Enumerable.Range(0, 7).Select(i => p[$"card{i}"]).ToArray();
        var gap = int.MaxValue;
        // Distinct physical card positions, not distinct digit values.
        for (var a = 0; a < 7; a++)
        for (var b = 0; b < 7; b++)
        for (var d = 0; d < 7; d++)
        for (var e = 0; e < 7; e++)
        {
            if (a == b || a == d || a == e || b == d || b == e || d == e)
                continue;
            if (c[a] == 0 || c[d] == 0) continue; // two-digit addends
            var total = 10 * c[a] + c[b] + 10 * c[d] + c[e];
            gap = Math.Min(gap, Math.Abs(total - 50));
        }
        if (gap == int.MaxValue)
            throw new InvalidOperationException("No legal card arrangement");
        return gap.ToString(CultureInfo.InvariantCulture);
    }

    public static bool Verify(string family, IReadOnlyDictionary<string, int> p,
        string answer) =>
        Valid(family, p) && string.Equals(answer, Solve(family, p), StringComparison.Ordinal);

    private static bool Valid(string? family, IReadOnlyDictionary<string, int> p)
    {
        if (family == SevenCards)
        {
            if (p.Count != 7) return false;
            var nonzero = 0;
            for (var i = 0; i < 7; i++)
            {
                if (!p.TryGetValue($"card{i}", out var card) || card is < 0 or > 9)
                    return false;
                if (card != 0) nonzero++;
            }
            return nonzero >= 2;
        }
        return family == PlaceValue && p.Count == 2 &&
            p.TryGetValue("left", out var a) && p.TryGetValue("right", out var b) &&
            a is >= 10 and <= 40 && b >= 1 && a + b <= 50;
    }
}
