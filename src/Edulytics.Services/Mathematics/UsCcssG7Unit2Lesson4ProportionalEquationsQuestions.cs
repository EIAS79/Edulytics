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
    // Reuse this Grade 7 proportional-equation engine for the next lesson:
    // both y=kx and its inverse x=y/k with directed measurement units.
    public const string ReciprocalFamily = "usccss.g7.u02.l05.reciprocal_proportional_equations";
    internal sealed record Problem(
        string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) => family is Family or ReciprocalFamily;

    public static Problem Build(string family, Random random)
    {
        if (!Supports(family))
            throw new InvalidOperationException("Unsupported G7 U02 proportional question family.");
        if (family == ReciprocalFamily) return BuildReciprocal(random);

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
        var (numerator, denominator) = family == ReciprocalFamily
            ? ExpectedReciprocal(p) : Expected(p);
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
        var (n, d) = family == ReciprocalFamily
            ? ExpectedReciprocal(p) : Expected(p);
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

    private static Problem BuildReciprocal(Random random)
    {
        var p = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["mode"] = random.Next(0, 7),
            ["feature"] = random.Next(0, 4),
            ["variant"] = random.Next(0, 2)
        };
        int mode = p["mode"], f = p["feature"], reverse = p["variant"];
        string prompt;
        if (mode == 0)
        {
            string[] contexts =
            [
                "100 centimeters correspond to 1 meter",
                "8 gallons fill a cooler in 5 minutes at constant flow",
                "a shrimp eats 3/5 gram of food per day",
                "each building block weighs 28 pounds"
            ];
            prompt = contexts[f] +
                (reverse == 0
                    ? ". Find the numerical constant for the relationship as stated (output per 1 input)."
                    : ". Find the RECIPROCAL constant after swapping the input and output quantities.");
        }
        else if (mode == 1)
        {
            decimal[] meters = [0.94m, 1.67m, 57.24m, 2.5m];
            decimal[] centimeters = [250m, 78.2m, 123.9m, 10m];
            prompt = reverse == 0
                ? "One meter is 100 centimeters. How many CENTIMETERS is " +
                    Format(meters[f]) + " meters?"
                : "One centimeter is 1/100 meter. How many METERS is " +
                    Format(centimeters[f]) + " centimeters?";
        }
        else if (mode == 2)
        {
            int[] times = [1, 3, 5, 10];
            int[] quantities = [1, 3, 5, 10];
            prompt = reverse == 0
                ? "A cooler fills 8 gallons in 5 minutes at a fixed rate. " +
                  "How many gallons flow in " + times[f] + " minutes?"
                : "A cooler fills 8 gallons in 5 minutes at a fixed rate. " +
                  "How many MINUTES are needed for " + quantities[f] + " gallons?";
        }
        else if (mode == 3)
        {
            int[] times = [3, 6, 12, 15];
            prompt = reverse == 0
                ? "After adjusting a faucet, 1 gallon flows in 3 minutes. " +
                  "How many gallons fill in " + times[f] + " minutes?"
                : "After adjusting a faucet, 1 gallon flows in 3 minutes. " +
                  "How many minutes fill " + times[f] + " gallons?";
        }
        else if (mode == 4)
        {
            int[] amounts = [1, 7, 30, 100];
            prompt = reverse == 0
                ? "A shrimp receives 1/5 gram per feeding, 3 feedings per day. " +
                  "How many grams does it receive in " + amounts[f] + " days?"
                : "A shrimp receives 3/5 gram per day. " +
                  "How many days correspond to " + amounts[f] +
                  " grams of food, assuming a constant rate?";
        }
        else if (mode == 5)
        {
            int[] values = [1, 7, 30, 50];
            long[] grams = [6, 42, 180, 300];
            prompt = reverse == 0
                ? "10 shrimp each need 3/5 gram per day. " +
                  "How many grams of food for all 10 over " + values[f] + " days?"
                : "A tank's 10 shrimp use 6 grams of food daily. " +
                  "How many days will " + grams[f] + " grams last?";
        }
        else
        {
            int[] meters = [1000, 3500, 500, 75];
            int[] kilometers = [1, 4, 2, 3];
            prompt = reverse == 0
                ? "Write y=(1/1000)x for kilometers y and meters x. " +
                  "How many kilometers are " + meters[f] + " meters?"
                : "Write x=1000y for meters x and kilometers y. " +
                  "How many meters are " + kilometers[f] + " kilometers?";
        }
        return new(ReciprocalFamily,
            prompt + " Enter the exact number or fraction; explain which units the coefficient has.",
            "Find the forward ratio y/x, the inverse ratio x/y, and state their different units. " +
            "These constants multiply to 1, but actual quantities must not be swapped. " +
            "A complete model or explanatory table requires teacher assessment.",
            AssessmentItemType.ShortAnswer,p);
    }

    private static string Format(decimal value) =>
        value.ToString(CultureInfo.InvariantCulture);

    private static (long N, long D) ExpectedReciprocal(IReadOnlyDictionary<string,int> p)
    {
        int kind=p["mode"], f=p["feature"], rev=p["variant"];
        if (kind == 0)
        {
            long[] numerators = [100, 8, 3, 28];
            long[] denominators = [1, 5, 5, 1];
            return rev == 0
                ? (numerators[f], denominators[f])
                : (denominators[f], numerators[f]);
        }
        if (kind == 1)
        {
            long[] hundredthsMeters = [94, 167, 5724, 250];
            long[] tenthsCm = [2500, 782, 1239, 100];
            return rev == 0
                ? (hundredthsMeters[f], 1)
                : (tenthsCm[f], 1000);
        }
        if (kind == 2)
        {
            long[] amounts = [1, 3, 5, 10];
            return rev == 0 ? (8 * amounts[f], 5) : (5 * amounts[f], 8);
        }
        if (kind == 3)
        {
            long[] amounts = [3, 6, 12, 15];
            return rev == 0 ? (amounts[f], 3) : (3 * amounts[f], 1);
        }
        if (kind == 4)
        {
            long[] amounts = [1, 7, 30, 100];
            return rev == 0 ? (3 * amounts[f], 5) : (5 * amounts[f], 3);
        }
        if (kind == 5)
        {
            long[] days = [1, 7, 30, 50];
            long[] grams = [6, 42, 180, 300];
            return rev == 0 ? (6 * days[f], 1) : (grams[f], 6);
        }
        long[] values = [1000, 3500, 500, 75];
        long[] kilometers = [1, 4, 2, 3];
        return rev == 0 ? (values[f], 1000) : (1000 * kilometers[f], 1);
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
