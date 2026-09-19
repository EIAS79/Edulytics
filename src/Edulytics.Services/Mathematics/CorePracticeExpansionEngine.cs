using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Reusable deterministic exact Practice families promoted from repeated,
/// evidence-backed curriculum targets. This expansion stays separate from the
/// core kernel so new shared families can be audited without lesson-specific
/// generators.
/// </summary>
internal static class CorePracticeExpansionEngine
{
    private static readonly IReadOnlySet<string> Families =
        new HashSet<string>(StringComparer.Ordinal)
        {
            "sequences.core.arithmetic_nth_term",
            "sequences.core.arithmetic_rule_offset",
            "sequences.core.geometric_nth_term",
            "percentages.core.of_quantity",
            "percentages.core.change",
            "ratio.proportion.divide_total",
            "ratio.proportion.unit_rate",
            "ratio.proportion.equivalent_ratio",
            "geometry.polygons.triangle_missing_angle",
            "geometry.polygons.quadrilateral_missing_angle",
            "geometry.perimeter_area.triangle_area",
            "geometry.coordinate.linear_graphs.gradient",
            "geometry.coordinate.linear_graphs.evaluate",
            "geometry.coordinate.linear_graphs.intercept",
            "statistics.mean_median_range.mean",
            "statistics.mean_median_range.median",
            "statistics.mean_median_range.range",
            "probability.theoretical.single_event",
            "probability.theoretical.two_independent_events"
        };

    internal sealed record Problem(
        string Family,
        string Prompt,
        string Solution,
        AssessmentItemType ItemType,
        IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) =>
        !string.IsNullOrWhiteSpace(family) && Families.Contains(family.Trim());

    public static Problem Build(string family, Random random, int scale) =>
        family switch
        {
            "sequences.core.arithmetic_nth_term" => BuildArithmeticNthTerm(random, scale),
            "sequences.core.arithmetic_rule_offset" => BuildArithmeticRuleOffset(random, scale),
            "sequences.core.geometric_nth_term" => BuildGeometricNthTerm(random, scale),
            "percentages.core.of_quantity" => BuildPercentageOfQuantity(random, scale),
            "percentages.core.change" => BuildPercentageChange(random, scale),
            "ratio.proportion.divide_total" => BuildDivideRatio(random, scale),
            "ratio.proportion.unit_rate" => BuildUnitRate(random, scale),
            "ratio.proportion.equivalent_ratio" => BuildEquivalentRatio(random, scale),
            "geometry.polygons.triangle_missing_angle" => BuildTriangleAngle(random),
            "geometry.polygons.quadrilateral_missing_angle" => BuildQuadrilateralAngle(random),
            "geometry.perimeter_area.triangle_area" => BuildTriangleArea(random, scale),
            "geometry.coordinate.linear_graphs.gradient" => BuildGradient(random, scale),
            "geometry.coordinate.linear_graphs.evaluate" => BuildLinearEvaluate(random, scale),
            "geometry.coordinate.linear_graphs.intercept" => BuildLinearIntercept(random, scale),
            "statistics.mean_median_range.mean" => BuildMean(random, scale),
            "statistics.mean_median_range.median" => BuildMedian(random, scale),
            "statistics.mean_median_range.range" => BuildRange(random, scale),
            "probability.theoretical.single_event" => BuildSimpleProbability(random, scale),
            "probability.theoretical.two_independent_events" => BuildTwoIndependentEvents(random),
            _ => throw new InvalidOperationException($"Unsupported expanded Practice family: {family}")
        };

    public static string Solve(string family, IReadOnlyDictionary<string, int> p) =>
        family switch
        {
            "sequences.core.arithmetic_nth_term" or
            "sequences.core.arithmetic_rule_offset" or
            "sequences.core.geometric_nth_term" or
            "percentages.core.of_quantity" or
            "percentages.core.change" or
            "ratio.proportion.divide_total" or
            "ratio.proportion.unit_rate" or
            "ratio.proportion.equivalent_ratio" or
            "geometry.polygons.triangle_missing_angle" or
            "geometry.polygons.quadrilateral_missing_angle" or
            "geometry.perimeter_area.triangle_area" or
            "geometry.coordinate.linear_graphs.gradient" or
            "geometry.coordinate.linear_graphs.evaluate" or
            "geometry.coordinate.linear_graphs.intercept" or
            "statistics.mean_median_range.mean" or
            "statistics.mean_median_range.median" or
            "statistics.mean_median_range.range" =>
                p["expected"].ToString(CultureInfo.InvariantCulture),

            "probability.theoretical.single_event" or
            "probability.theoretical.two_independent_events" =>
                SimplifyFraction(p["numerator"], p["denominator"]),

            _ => throw new InvalidOperationException($"Unsupported expanded Practice solver family: {family}")
        };

    public static bool Verify(
        string family,
        IReadOnlyDictionary<string, int> p,
        string answer)
    {
        if (!Supports(family))
            return false;

        if (family.StartsWith("probability.theoretical.", StringComparison.Ordinal))
        {
            if (!TryParseFraction(answer, out var n, out var d) || d <= 0)
                return false;

            return p["denominator"] > 0 &&
                p["numerator"] >= 0 &&
                p["numerator"] <= p["denominator"] &&
                n * p["denominator"] == p["numerator"] * d;
        }

        if (!int.TryParse(answer, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value))
            return false;

        if (!p.TryGetValue("expected", out var expected) || value != expected)
            return false;

        return family switch
        {
            "sequences.core.arithmetic_nth_term" =>
                p["n"] > 0 &&
                expected == p["first"] + (p["n"] - 1) * p["difference"],

            "sequences.core.arithmetic_rule_offset" =>
                expected == p["first"] - p["difference"],

            "sequences.core.geometric_nth_term" =>
                p["n"] > 0 && p["ratio"] > 0 &&
                expected == checked(p["first"] * IntPow(p["ratio"], p["n"] - 1)),

            "percentages.core.of_quantity" =>
                p["percent"] is >= 0 and <= 100 &&
                p["quantity"] >= 0 &&
                p["percent"] * p["quantity"] % 100 == 0 &&
                expected * 100 == p["percent"] * p["quantity"],

            "percentages.core.change" =>
                p["percent"] is > 0 and <= 100 &&
                p["direction"] is -1 or 1 &&
                expected * 100 == p["original"] * (100 + p["direction"] * p["percent"]),

            "ratio.proportion.divide_total" =>
                p["partA"] > 0 && p["partB"] > 0 &&
                p["total"] % (p["partA"] + p["partB"]) == 0 &&
                expected == p["partA"] * (p["total"] / (p["partA"] + p["partB"])),

            "ratio.proportion.unit_rate" =>
                p["quantity"] > 0 &&
                expected * p["quantity"] == p["total"],

            "ratio.proportion.equivalent_ratio" =>
                p["baseSecond"] > 0 && p["targetSecond"] > 0 &&
                p["baseFirst"] * p["targetSecond"] == expected * p["baseSecond"],

            "geometry.polygons.triangle_missing_angle" =>
                p["angleA"] > 0 && p["angleB"] > 0 && expected > 0 &&
                p["angleA"] + p["angleB"] + expected == 180,

            "geometry.polygons.quadrilateral_missing_angle" =>
                p["angleA"] > 0 && p["angleB"] > 0 && p["angleC"] > 0 && expected > 0 &&
                p["angleA"] + p["angleB"] + p["angleC"] + expected == 360,

            "geometry.perimeter_area.triangle_area" =>
                p["base"] > 0 && p["height"] > 0 &&
                expected * 2 == p["base"] * p["height"],

            "geometry.coordinate.linear_graphs.gradient" =>
                p["x2"] != p["x1"] &&
                (p["y2"] - p["y1"]) % (p["x2"] - p["x1"]) == 0 &&
                expected == (p["y2"] - p["y1"]) / (p["x2"] - p["x1"]),

            "geometry.coordinate.linear_graphs.evaluate" =>
                expected == p["gradient"] * p["x"] + p["intercept"],

            "geometry.coordinate.linear_graphs.intercept" =>
                expected == p["y"] - p["gradient"] * p["x"],

            "statistics.mean_median_range.mean" =>
                expected * 5 == p["d0"] + p["d1"] + p["d2"] + p["d3"] + p["d4"],

            "statistics.mean_median_range.median" =>
                expected == Median5(p["d0"], p["d1"], p["d2"], p["d3"], p["d4"]),

            "statistics.mean_median_range.range" =>
                expected == Range5(p["d0"], p["d1"], p["d2"], p["d3"], p["d4"]),

            _ => false
        };
    }

    private static Problem BuildArithmeticNthTerm(Random random, int scale)
    {
        var first = random.Next(-5 * scale, 8 * scale + 1);
        var difference = NonZero(random, -4 - scale, 5 + scale);
        var n = random.Next(5, 10 + scale * 5);
        var expected = checked(first + (n - 1) * difference);
        return P(
            "sequences.core.arithmetic_nth_term",
            $"The arithmetic sequence starts {first}, {first + difference}, {first + 2 * difference}, ... Find term {n}.",
            "Identify the common difference and use aₙ = a₁ + (n−1)d. Substitute the term number and verify by stepping through the pattern.",
            ("first", first), ("difference", difference), ("n", n), ("expected", expected));
    }

    private static Problem BuildArithmeticRuleOffset(Random random, int scale)
    {
        var difference = NonZero(random, -4 - scale, 5 + scale);
        var offset = random.Next(-8 * scale, 8 * scale + 1);
        var first = difference + offset;
        return P(
            "sequences.core.arithmetic_rule_offset",
            $"The sequence begins {first}, {first + difference}, {first + 2 * difference}, {first + 3 * difference}, ... Its nth term is {difference}n + c. Find c.",
            "The coefficient of n is the common difference. Substitute n=1 and the first term to solve for c, then verify on later terms.",
            ("first", first), ("difference", difference), ("expected", offset));
    }

    private static Problem BuildGeometricNthTerm(Random random, int scale)
    {
        var first = random.Next(1, 4 + scale);
        var ratio = random.Next(2, 4);
        var n = random.Next(3, Math.Min(7, 4 + scale + 2));
        var expected = checked(first * IntPow(ratio, n - 1));
        return P(
            "sequences.core.geometric_nth_term",
            $"The geometric sequence starts {first}, {first * ratio}, {first * ratio * ratio}, ... Find term {n}.",
            "Identify the constant ratio and use aₙ = a₁rⁿ⁻¹. Verify by repeated multiplication.",
            ("first", first), ("ratio", ratio), ("n", n), ("expected", expected));
    }

    private static Problem BuildPercentageOfQuantity(Random random, int scale)
    {
        int[] choices = [10, 20, 25, 40, 50, 60, 75, 80];
        var percent = choices[random.Next(choices.Length)];
        var quantity = 20 * random.Next(2, 6 + scale * 2);
        var expected = percent * quantity / 100;
        return P(
            "percentages.core.of_quantity",
            $"Find {percent}% of {quantity}.",
            "Write the percentage as a fraction over 100 or decimal multiplier, multiply by the quantity, then check the result against the original scale.",
            ("percent", percent), ("quantity", quantity), ("expected", expected));
    }

    private static Problem BuildPercentageChange(Random random, int scale)
    {
        int[] choices = [10, 20, 25, 50];
        var percent = choices[random.Next(choices.Length)];
        var direction = random.Next(0, 2) == 0 ? 1 : -1;
        var original = 20 * random.Next(3, 7 + scale * 2);
        var expected = original * (100 + direction * percent) / 100;
        var verb = direction > 0 ? "increase" : "decrease";
        return P(
            "percentages.core.change",
            $"{verb[0].ToString().ToUpperInvariant() + verb[1..]} {original} by {percent}%.",
            "Find the percentage change from the original quantity, then add or subtract it. Verify using the multiplier method.",
            ("percent", percent), ("direction", direction), ("original", original), ("expected", expected));
    }

    private static Problem BuildDivideRatio(Random random, int scale)
    {
        var partA = random.Next(2, 6 + scale);
        var partB = random.Next(2, 6 + scale);
        var unit = random.Next(2, 8 + scale * 2);
        var total = (partA + partB) * unit;
        var expected = partA * unit;
        return P(
            "ratio.proportion.divide_total",
            $"A total of {total} is divided in the ratio {partA}:{partB}. Find the first share.",
            "Add the ratio parts to find the number of equal units, divide the total by that count, then multiply by the first ratio part.",
            ("partA", partA), ("partB", partB), ("total", total), ("expected", expected));
    }

    private static Problem BuildUnitRate(Random random, int scale)
    {
        var quantity = random.Next(2, 7 + scale);
        var rate = random.Next(2, 10 + scale * 2);
        var total = quantity * rate;
        return P(
            "ratio.proportion.unit_rate",
            $"{quantity} items cost {total}. Find the cost per item.",
            "Divide the total by the number of items to obtain the unit rate, then multiply back to verify.",
            ("quantity", quantity), ("total", total), ("expected", rate));
    }

    private static Problem BuildEquivalentRatio(Random random, int scale)
    {
        var baseSecond = random.Next(2, 7 + scale);
        var rate = random.Next(2, 9 + scale * 2);
        var baseFirst = baseSecond * rate;
        var factor = random.Next(2, 5 + scale);
        var targetSecond = baseSecond * factor;
        var expected = baseFirst * factor;
        return P(
            "ratio.proportion.equivalent_ratio",
            $"Complete the equivalent ratio: {baseFirst}:{baseSecond} = ?:{targetSecond}.",
            "Equivalent ratios scale both corresponding quantities by the same factor. Verify both quotients are equal.",
            ("baseFirst", baseFirst), ("baseSecond", baseSecond), ("targetSecond", targetSecond), ("expected", expected));
    }

    private static Problem BuildTriangleAngle(Random random)
    {
        var a = random.Next(25, 100);
        var b = random.Next(25, 145 - a);
        var expected = 180 - a - b;
        return P(
            "geometry.polygons.triangle_missing_angle",
            $"A triangle has interior angles {a}° and {b}°. Find the third angle.",
            "Interior angles of a triangle total 180°. Subtract the two known angles and verify all three sum to 180°.",
            ("angleA", a), ("angleB", b), ("expected", expected));
    }

    private static Problem BuildQuadrilateralAngle(Random random)
    {
        var a = random.Next(60, 111);
        var b = random.Next(60, 111);
        var c = random.Next(60, 111);
        var expected = 360 - a - b - c;
        if (expected <= 10)
        {
            c = 80;
            expected = 360 - a - b - c;
        }

        return P(
            "geometry.polygons.quadrilateral_missing_angle",
            $"A quadrilateral has three interior angles {a}°, {b}° and {c}°. Find the fourth angle.",
            "Interior angles of a quadrilateral total 360°. Subtract the three known angles and verify the total.",
            ("angleA", a), ("angleB", b), ("angleC", c), ("expected", expected));
    }

    private static Problem BuildTriangleArea(Random random, int scale)
    {
        var baseLength = 2 * random.Next(2, 8 + scale * 2);
        var height = random.Next(2, 8 + scale * 2);
        var expected = baseLength * height / 2;
        return P(
            "geometry.perimeter_area.triangle_area",
            $"A triangle has base {baseLength} and perpendicular height {height}. Find its area.",
            "Use area = ½ × base × perpendicular height. Verify the units are square units.",
            ("base", baseLength), ("height", height), ("expected", expected));
    }

    private static Problem BuildGradient(Random random, int scale)
    {
        var x1 = random.Next(-5 * scale, 5 * scale + 1);
        var run = random.Next(1, 5 + scale);
        var gradient = NonZero(random, -4 - scale, 5 + scale);
        var y1 = random.Next(-6 * scale, 6 * scale + 1);
        var x2 = x1 + run;
        var y2 = y1 + gradient * run;
        return P(
            "geometry.coordinate.linear_graphs.gradient",
            $"Find the gradient of the line through ({x1}, {y1}) and ({x2}, {y2}).",
            "Use change in y divided by change in x, then verify the second point from the first using the gradient.",
            ("x1", x1), ("y1", y1), ("x2", x2), ("y2", y2), ("expected", gradient));
    }

    private static Problem BuildLinearEvaluate(Random random, int scale)
    {
        var gradient = NonZero(random, -4 - scale, 5 + scale);
        var intercept = random.Next(-8 * scale, 8 * scale + 1);
        var x = random.Next(-5 * scale, 5 * scale + 1);
        var expected = gradient * x + intercept;
        return P(
            "geometry.coordinate.linear_graphs.evaluate",
            $"For y = {gradient}x + {intercept}, find y when x = {x}.",
            "Substitute the x-coordinate into the linear rule and simplify. The resulting ordered pair must lie on the line.",
            ("gradient", gradient), ("intercept", intercept), ("x", x), ("y", expected), ("expected", expected));
    }

    private static Problem BuildLinearIntercept(Random random, int scale)
    {
        var gradient = NonZero(random, -4 - scale, 5 + scale);
        var intercept = random.Next(-8 * scale, 8 * scale + 1);
        var x = random.Next(-5 * scale, 5 * scale + 1);
        var y = gradient * x + intercept;
        return P(
            "geometry.coordinate.linear_graphs.intercept",
            $"A line has gradient {gradient} and passes through ({x}, {y}). Find its y-intercept.",
            "Use y = mx + b. Substitute the point and gradient, rearrange for b, then verify the point satisfies the resulting line.",
            ("gradient", gradient), ("x", x), ("y", y), ("expected", intercept));
    }

    private static Problem BuildMean(Random random, int scale)
    {
        var mean = random.Next(3, 12 + scale * 3);
        var a = random.Next(1, 4 + scale);
        var b = random.Next(1, 4 + scale);
        var data = new[] { mean - a, mean - b, mean, mean + b, mean + a };
        Shuffle(random, data);
        return DataProblem(
            "statistics.mean_median_range.mean",
            data,
            $"Find the mean of {string.Join(", ", data)}.",
            "Add all values and divide by the number of values. Check by comparing deviations around the calculated mean.",
            mean);
    }

    private static Problem BuildMedian(Random random, int scale)
    {
        var start = random.Next(1, 8 + scale * 3);
        var data = new[]
        {
            start,
            start + random.Next(1, 4 + scale),
            start + 5 + scale,
            start + 8 + scale,
            start + 12 + scale
        };
        Array.Sort(data);
        var expected = data[2];
        Shuffle(random, data);
        return DataProblem(
            "statistics.mean_median_range.median",
            data,
            $"Find the median of {string.Join(", ", data)}.",
            "Order the five values from least to greatest and take the middle value.",
            expected);
    }

    private static Problem BuildRange(Random random, int scale)
    {
        var min = random.Next(0, 8 + scale);
        var max = min + random.Next(6, 14 + scale * 3);
        var data = new[]
        {
            min,
            min + 2,
            min + 3 + scale,
            max - 2,
            max
        };
        Shuffle(random, data);
        return DataProblem(
            "statistics.mean_median_range.range",
            data,
            $"Find the range of {string.Join(", ", data)}.",
            "Identify the maximum and minimum values and subtract minimum from maximum.",
            max - min);
    }

    private static Problem BuildSimpleProbability(Random random, int scale)
    {
        var total = random.Next(4, 10 + scale * 2);
        var favourable = random.Next(1, total);
        return PFraction(
            "probability.theoretical.single_event",
            $"A bag contains {total} equally likely counters, {favourable} of which are red. Find P(red) as a fraction.",
            "For equally likely outcomes, probability = favourable outcomes ÷ total outcomes. Simplify the fraction.",
            favourable,
            total,
            ("favourable", favourable), ("total", total));
    }

    private static Problem BuildTwoIndependentEvents(Random random)
    {
        var successA = random.Next(1, 5);
        var totalA = random.Next(successA + 1, 7);
        var successB = random.Next(1, 5);
        var totalB = random.Next(successB + 1, 7);
        var numerator = successA * successB;
        var denominator = totalA * totalB;
        return PFraction(
            "probability.theoretical.two_independent_events",
            $"Event A has probability {successA}/{totalA} and independent event B has probability {successB}/{totalB}. Find P(A and B).",
            "For independent events multiply the probabilities, then simplify. Verify the result is between 0 and 1.",
            numerator,
            denominator,
            ("successA", successA), ("totalA", totalA), ("successB", successB), ("totalB", totalB));
    }

    private static Problem DataProblem(
        string family,
        int[] data,
        string prompt,
        string solution,
        int expected) =>
        P(
            family,
            prompt,
            solution,
            ("d0", data[0]), ("d1", data[1]), ("d2", data[2]), ("d3", data[3]), ("d4", data[4]),
            ("expected", expected));

    private static Problem PFraction(
        string family,
        string prompt,
        string solution,
        int numerator,
        int denominator,
        params (string Name, int Value)[] extra)
    {
        var parameters = extra
            .Append(("numerator", numerator))
            .Append(("denominator", denominator))
            .ToArray();
        return P(family, prompt, solution, AssessmentItemType.ShortAnswer, parameters);
    }

    private static Problem P(
        string family,
        string prompt,
        string solution,
        params (string Name, int Value)[] parameters) =>
        P(family, prompt, solution, AssessmentItemType.Numeric, parameters);

    private static Problem P(
        string family,
        string prompt,
        string solution,
        AssessmentItemType itemType,
        params (string Name, int Value)[] parameters) =>
        new(
            family,
            prompt,
            solution,
            itemType,
            parameters.ToDictionary(x => x.Name, x => x.Value, StringComparer.Ordinal));

    private static int NonZero(Random random, int minInclusive, int maxExclusive)
    {
        var value = 0;
        while (value == 0)
            value = random.Next(minInclusive, maxExclusive);
        return value;
    }

    private static int IntPow(int value, int exponent)
    {
        var result = 1;
        for (var i = 0; i < exponent; i++)
            result = checked(result * value);
        return result;
    }

    private static int Median5(params int[] values)
    {
        var copy = values.ToArray();
        Array.Sort(copy);
        return copy[2];
    }

    private static int Range5(params int[] values) =>
        values.Max() - values.Min();

    private static void Shuffle(Random random, int[] values)
    {
        for (var i = values.Length - 1; i > 0; i--)
        {
            var j = random.Next(0, i + 1);
            (values[i], values[j]) = (values[j], values[i]);
        }
    }

    private static string SimplifyFraction(int numerator, int denominator)
    {
        if (denominator <= 0)
            throw new InvalidOperationException("Fraction denominator must be positive.");
        var gcd = Gcd(Math.Abs(numerator), denominator);
        return $"{numerator / gcd}/{denominator / gcd}";
    }

    private static bool TryParseFraction(string answer, out int numerator, out int denominator)
    {
        numerator = 0;
        denominator = 1;
        var parts = answer.Trim().Split('/', StringSplitOptions.TrimEntries);
        if (parts.Length == 1)
        {
            if (!int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out numerator))
                return false;
            denominator = 1;
            return true;
        }

        return parts.Length == 2 &&
            int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out numerator) &&
            int.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out denominator) &&
            denominator != 0;
    }

    private static int Gcd(int a, int b)
    {
        a = Math.Abs(a);
        b = Math.Abs(b);
        while (b != 0)
        {
            var t = a % b;
            a = b;
            b = t;
        }
        return a == 0 ? 1 : a;
    }
}
