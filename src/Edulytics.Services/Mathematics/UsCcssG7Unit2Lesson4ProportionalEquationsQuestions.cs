using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// G7 U02:L04 (OUR/IM 2017-2019): y=kx equations from proportional
/// recipe, flight, and bakery tables. This is first-party exact arithmetic
/// on the existing lesson-scoped engine; equation-writing explanations
/// and student-generated tables remain teacher reviewed.
/// </summary>
internal static class UsCcssG7Unit2Lesson4ProportionalEquationsQuestions
{
    public const string Family = "usccss.g7.u02.l04.proportional_equations";
    internal sealed record Problem(
        string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) => family == Family;

    public static Problem Build(string family, Random random)
    {
        if (!Supports(family))
            throw new InvalidOperationException("Unsupported G7 U02 L04 question family.");

        var p = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["mode"] = random.Next(0, 7),
            ["feature"] = random.Next(0, 4),
            ["variant"] = random.Next(0, 2)
        };
        int mode = p["mode"], f = p["feature"], variant = p["variant"];
        string text;
        if (mode == 0 || mode == 5)
        {
            string[] contexts =
            [
                "A recipe serves 6 people from 2 cups of rice. Let y be people served and x be cups of rice.",
                "A recipe serves 3 people from 6 spring rolls. Let y be people served and x be spring rolls.",
                "A flight travels 915 miles in 1.5 hours. Let y be distance in miles and x be elapsed time in hours.",
                "A bakery uses 8 tablespoons of honey for 10 cups of flour. Let y be flour cups and x be honey tablespoons."
            ];
            text = contexts[f] + (mode == 0
                ? " Find the exact constant k in the relationship y=kx."
                : " Instead let x be the SECOND quantity and y be the FIRST. Find k in y=kx.");
        }
        else if (mode == 1)
        {
            long[] cups = [1, 3, 12, 43];
            text = "A first-party rice recipe serves 3 people per cup. " +
                "Write y=3x for the number y of people served by x cups. " +
                "For x="+cups[f]+", find y. The same equation must satisfy ALL table rows.";
        }
        else if (mode == 2)
        {
            long[] rolls = [1, 10, 16, 25];
            text = "A spring-roll recipe uses 6 rolls for 3 servings. " +
                "The constant number of servings per roll is 1/2. " +
                "For x="+rolls[f]+" rolls, find y servings from y=(1/2)x.";
        }
        else if (mode == 3)
        {
            string[] times = ["1", "1.5", "2.5", "3.5"];
            text = "A plane covers 915 miles in 1.5 hours at constant speed. " +
                "Let d be distance in miles and t be hours. " +
                "Find d at t="+times[f]+". Use d=kt and calculate k from the observed pair.";
        }
        else if (mode == 4)
        {
            long[] tablespoons = [1, 8, 15, 17];
            text = "A bakery uses 8 tablespoons of honey with 10 cups of flour. " +
                "Let h be honey tablespoons and f be flour cups. " +
                "For h="+tablespoons[f]+", calculate f from f=kh.";
        }
        else
        {
            long[] inputs = [10, 10, 2, 8];
            long[] correct = [30, 5, 1220, 10];
            long observed = correct[f] + (variant == 1 ? 1 : 0);
            string[] equations =
            [
                "y=3x for people y served by rice cups x",
                "y=(1/2)x for servings y from spring rolls x",
                "d=610t for distance d and time t",
                "f=(5/4)h for flour cups f and honey tablespoons h"
            ];
            text = "A proposed equation is "+equations[f]+
                ". An observed table row reports input "+inputs[f]+
                " and output "+observed+
                ". Is this row consistent with the equation? Enter 1 for YES or 0 for NO.";
        }
        return new(Family, text +
            " Enter the exact numeric response (a fraction is allowed). " +
            "Explain the units and equation to a teacher; calculations do not replace a complete written justification.",
            "Derive the coefficient as output/input for the specified direction. " +
            "The proportional equation has no additive intercept, " +
            "each row must share the same constant, and swapping quantities inverts k.",
            AssessmentItemType.ShortAnswer, p);
    }

    public static string Solve(string family, IReadOnlyDictionary<string, int> p)
    {
        if (!Valid(family, p))
            throw new InvalidOperationException("Invalid G7 U02 L04 parameters.");
        var (numerator, denominator) = Expected(p);
        long gcd = Gcd(numerator, denominator);
        numerator /= gcd;
        denominator /= gcd;
        return denominator == 1
            ? numerator.ToString(CultureInfo.InvariantCulture)
            : numerator.ToString(CultureInfo.InvariantCulture) + "/" +
              denominator.ToString(CultureInfo.InvariantCulture);
    }

    public static bool Verify(string family, IReadOnlyDictionary<string, int> p, string? answer)
    {
        if (!Valid(family, p) || string.IsNullOrWhiteSpace(answer) || answer.Length > 36)
            return false;
        string[] parts = answer.Trim().Split('/');
        decimal value;
        if (parts.Length == 2)
        {
            if (!decimal.TryParse(parts[0], NumberStyles.None,
                    CultureInfo.InvariantCulture, out decimal numerator) ||
                !decimal.TryParse(parts[1], NumberStyles.None,
                    CultureInfo.InvariantCulture, out decimal denominator) ||
                numerator < 0 || numerator > 1000000 ||
                denominator <= 0 || denominator > 1000000)
                return false;
            value = numerator / denominator;
        }
        else if (parts.Length == 1)
        {
            if (!decimal.TryParse(parts[0], NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out value) ||
                value < 0 || value > 1000000)
                return false;
        }
        else return false;
        var (n, d) = Expected(p);
        return value * d == n;
    }

    private static (long N, long D) Expected(IReadOnlyDictionary<string, int> p)
    {
        int mode = p["mode"], f = p["feature"];
        return mode switch
        {
            0 => f switch
            {
                0 => (3, 1), 1 => (1, 2), 2 => (610, 1), _ => (5, 4)
            },
            1 => (3 * new long[] { 1, 3, 12, 43 }[f], 1),
            2 => (new long[] { 1, 10, 16, 25 }[f], 2),
            3 => (610 * new long[] { 2, 3, 5, 7 }[f], 2),
            4 => (5 * new long[] { 1, 8, 15, 17 }[f], 4),
            5 => f switch
            {
                0 => (1, 3), 1 => (2, 1), 2 => (1, 610), _ => (4, 5)
            },
            _ => (p["variant"] == 0 ? 1 : 0, 1)
        };
    }
    private static long Gcd(long a, long b)
    {
        while (b != 0) { long t = a % b; a = b; b = t; }
        return a;
    }
    private static bool Valid(string? family, IReadOnlyDictionary<string, int>? p) =>
        Supports(family) && p is not null && p.Count == 3 &&
        p.TryGetValue("mode", out int mode) && mode is >= 0 and <= 6 &&
        p.TryGetValue("feature", out int feature) && feature is >= 0 and <= 3 &&
        p.TryGetValue("variant", out int variant) && variant is 0 or 1;
}
