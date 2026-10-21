using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Graded numerical consequences of the original IM 2021 Grade 2 U01 L04
/// How Close center and within-20 arithmetic. Digital distance is NOT a claim
/// that the pupil authored a legal optimal three-card equation or explained
/// a make-ten/subtraction strategy; those remain teacher-scored.
/// </summary>
internal static class UsCcssG2Unit1Lesson4Questions
{
    public const string ClosestGap = "usccss.g2.u01.l04.five_card_min_distance";
    public const string WithinTwenty = "usccss.g2.u01.l04.within20_add_or_sub";

    public sealed record Problem(string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) => family is ClosestGap or WithinTwenty;

    public static Problem Build(string family, Random random)
    {
        if (!Supports(family))
            throw new InvalidOperationException("Unknown G2 original U01 L04 question family");

        var p = new Dictionary<string, int>(StringComparer.Ordinal);
        string prompt, solution;
        if (family == ClosestGap)
        {
            // Five distinct cards; the original teacher source removes all 10s.
            var cards = Enumerable.Range(0, 10).OrderBy(_ => random.Next()).Take(5).ToArray();
            for (var i = 0; i < cards.Length; i++)
                p[$"card{i}"] = cards[i];
            prompt = $"How Close? Here are five digit cards: {string.Join(", ", cards)}. " +
                "Choose any THREE different cards to add; the target is 20. " +
                "What is the smallest possible ABSOLUTE DISTANCE from 20? " +
                "Enter the distance (not your chosen three-card equation).";
            solution = "Check three different cards at a time and compare " +
                "the distance between each three-card sum and 20. " +
                "More than one three-card selection may tie; the digit-only " +
                "distance answer does not check or replace your recorded equation.";
        }
        else
        {
            var add = random.Next(2) == 0;
            var left = add ? random.Next(0, 21) : random.Next(0, 21);
            var right = add ? random.Next(0, 21 - left) : random.Next(0, left + 1);
            p["left"] = left; p["right"] = right; p["operation"] = add ? 0 : 1;
            prompt = $"Find {left} {(add ? "+" : "−")} {right} using a " +
                "strategy such as making a ten, counting on, or decomposing a number. " +
                "Enter the numerical result.";
            solution = "A correct student may use more than one valid strategy. " +
                "Use number bonds and ten-cube groups if helpful, then check " +
                "the result with the inverse operation. Written or oral strategy " +
                "explanations are evaluated separately by the teacher.";
        }
        return new Problem(family, prompt, solution, AssessmentItemType.Numeric, p);
    }

    public static string Solve(string family, IReadOnlyDictionary<string, int> p)
    {
        if (!Valid(family, p))
            throw new InvalidOperationException("Invalid Grade 2 U01 L04 task data");

        if (family == ClosestGap)
        {
            var c = Enumerable.Range(0, 5).Select(i => p[$"card{i}"]).ToArray();
            var best = int.MaxValue;
            for (var i = 0; i < 5; i++)
            for (var j = i + 1; j < 5; j++)
            for (var k = j + 1; k < 5; k++)
                best = Math.Min(best, Math.Abs(20 - (c[i] + c[j] + c[k])));
            return best.ToString(CultureInfo.InvariantCulture);
        }

        return (p["operation"] == 0 ?
            p["left"] + p["right"] : p["left"] - p["right"])
            .ToString(CultureInfo.InvariantCulture);
    }

    public static bool Verify(string family, IReadOnlyDictionary<string, int> p,
        string answer) =>
        Valid(family, p) && string.Equals(answer, Solve(family, p), StringComparison.Ordinal);

    private static bool Valid(string? family, IReadOnlyDictionary<string, int> p)
    {
        if (family == ClosestGap)
        {
            if (p.Count != 5) return false;
            var cards = new List<int>();
            for (var i = 0; i < 5; i++)
            {
                if (!p.TryGetValue($"card{i}", out var number) ||
                    number is < 0 or > 9) return false;
                cards.Add(number);
            }
            return cards.Distinct().Count() == 5;
        }
        if (family != WithinTwenty || p.Count != 3 ||
            !p.TryGetValue("left", out var left) ||
            !p.TryGetValue("right", out var right) ||
            !p.TryGetValue("operation", out var op) ||
            op is not (0 or 1) ||
            left is < 0 or > 20 || right < 0)
            return false;
        return op == 0 ? left + right <= 20 : right <= left;
    }
}
