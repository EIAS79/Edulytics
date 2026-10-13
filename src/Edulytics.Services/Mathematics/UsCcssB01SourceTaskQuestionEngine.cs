using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// 2021 Illustrative Mathematics K5 B01 lesson-purpose-aligned FIRST PARTY
/// task families. Source-specific tasks are generated from structured numbers,
/// never inferred from the generic title alone. This does not grant academic
/// certification; it supplies a real runtime for subsequent L5/L6 verification.
/// </summary>
internal static class UsCcssB01SourceTaskQuestionEngine
{
    public sealed record Problem(
        string Family, string Prompt, string Solution, AssessmentItemType ItemType,
        IReadOnlyDictionary<string, int> Parameters);

    private static readonly HashSet<string> Families = new(StringComparer.Ordinal)
    {
        "usccss.b01.triangle.attributes",
        "usccss.b01.partition.halves_fourths",
        "usccss.b01.number_line.equal_intervals",
        "usccss.b01.clock.half_quarters",
        "usccss.b01.clock.read_half_hour",
        "usccss.b01.clock.read_five_minutes",
        "usccss.b01.rectangle.unit_squares",
        "usccss.b01.measure.map_lengths",
        "usccss.b01.graph.choose_scale",
        "usccss.b01.array.equal_groups",
        "usccss.b01.geometry.composite_rectangle_area",
        "usccss.b01.division.unknown_factor",
        "usccss.b01.fraction.name_parts",
        "usccss.b01.measure.quarter_inch",
        "usccss.b01.factors.factor_pairs",
        "usccss.b01.factors.prime_composite",
        "usccss.b01.factors.locker_square",
        "usccss.b01.factors.factors_multiples",
        "usccss.b01.fraction.whole_multiply",
        "usccss.b01.fraction.decompose_same_denominator",
        "usccss.b01.place_value.ten_times",
        "usccss.b01.market.inventory_100"
    };

    public static bool Supports(string? family) =>
        !string.IsNullOrWhiteSpace(family) && Families.Contains(family);

    private static Dictionary<string, int> D(params (string Key, int Value)[] pairs) =>
        pairs.ToDictionary(x => x.Key, x => x.Value, StringComparer.Ordinal);

    private static Problem Make(string family, string prompt, string explanation,
        AssessmentItemType type, Dictionary<string, int> p) =>
        new(family, prompt, explanation, type, p);

    public static Problem Build(string f, Random r, ExactSkillQuestionDifficulty difficulty)
    {
        if (!Supports(f)) throw new InvalidOperationException("Unknown B01 source-task family.");
        var level = (int)difficulty;
        int a, b, n, d, m;
        switch (f)
        {
            case "usccss.b01.market.inventory_100":
                // 2021 IM G2 U02 L16 optional market: one stock category <10,
                // two categories >10, total exactly 100; transactions subtract
                // sales from the real inventory.
                a = r.Next(2, 10);
                b = r.Next(20, 61);
                n = 100 - a - b;
                d = r.Next(1, Math.Min(b, 12));
                m = r.Next(0, 2);
                return Make(f,
                    $"Your market starts with {a} apples, {b} notebooks and {n} pencils " +
                    $"(100 items altogether). A customer buys {d} notebooks. " +
                    (m == 0 ? "How many notebooks remain in stock?" :
                              "How many pencils were in stock before the sale?"),
                    "Model three inventory quantities totaling 100; subtract sales " +
                    "from the relevant item, and do not subtract unrelated stock.",
                    AssessmentItemType.Numeric,
                    D(("small", a), ("second", b), ("third", n), ("sold", d), ("mode", m)));

            case "usccss.b01.triangle.attributes":
                m = r.Next(1, 4);
                n = r.Next(0, 4);
                return Make(f,
                    "Imagine " + (n == 0 ? "a narrow triangle" :
                        n == 1 ? "a tilted triangle" :
                        n == 2 ? "a triangle with unequal side lengths" :
                                 "a right-angled triangle") +
                    ". Which property MUST every triangle have? " +
                    (m == 1 ? "1) Three straight sides; 2) Three equal sides; 3) Four sides." :
                     m == 2 ? "1) Three equal sides; 2) Three straight sides; 3) Four sides." :
                              "1) Four sides; 2) Three equal sides; 3) Three straight sides.") +
                    " Enter the number of the correct choice.",
                    "A triangle is a closed shape with exactly three straight sides. Equal side lengths are not required.",
                    AssessmentItemType.Numeric, D(("position", m), ("style", n)));

            case "usccss.b01.partition.halves_fourths":
                d = r.Next(0, 2) == 0 ? 2 : 4;
                m = r.Next(0, 2);
                return Make(f,
                    $"A {(m == 0 ? "circle" : "rectangle")} is partitioned into {d} EQUAL pieces. " +
                    "What is one piece called: half or fourth?",
                    "A half is one of two equal parts; a fourth is one of four equal parts.",
                    AssessmentItemType.ShortAnswer, D(("denominator", d), ("shape", m)));

            case "usccss.b01.number_line.equal_intervals":
                a = r.Next(1, 4 + level);
                b = r.Next(1, 7);
                return Make(f,
                    $"A number line has equally spaced ticks at 0, {a * b}, __, {a * b * 3}. " +
                    "What is the missing tick value?",
                    "Consecutive ticks have equal differences: add the constant step again.",
                    AssessmentItemType.Numeric, D(("step", a * b)));

            // Publisher G1 U07 L14-L15: hour/half-hour hand positions.
            case "usccss.b01.clock.read_half_hour":
                a = r.Next(1, 13);
                m = r.Next(0, 2);
                return Make(f,
                    m == 0
                        ? $"On an analog clock the hour hand points to {a} and " +
                          "the minute hand points to 12. Write the time (h:mm)."
                        : $"On an analog clock the hour hand is halfway between {a} " +
                          $"and {(a%12)+1}, and the minute hand points to 6. " +
                          "Write the time (h:mm).",
                    "On the hour the minute hand points to 12; at half past " +
                    "it points to 6 and the hour hand is halfway between numbers.",
                    AssessmentItemType.ShortAnswer, D(("hour", a), ("minutes", 30*m)));

            // Publisher G2 U06 L12: numbered clock ticks are five minutes apart.
            case "usccss.b01.clock.read_five_minutes":
                a = r.Next(1, 13);
                m = r.Next(0, 12);
                return Make(f,
                    $"The analog clock's minute hand points to " +
                    $"{(m==0 ? 12 : m)}, and the hour hand has reached or passed {a}. " +
                    "Write the time (h:mm).",
                    "Each number on an analog clock represents five minutes. " +
                    "Count the minute-hand position in groups of five.",
                    AssessmentItemType.ShortAnswer, D(("hour", a), ("minutes", m*5)));

            case "usccss.b01.clock.half_quarters":
                m = r.Next(0, 3);
                n = r.Next(1, 13);
                return Make(f,
                    $"On an analog clock, the hour hand is near {n} and the minute hand points to " +
                    (m == 0 ? "3" : m == 1 ? "6" : "9") +
                    ". How many minutes past the hour is that?",
                    "Each number on the minute scale represents five minutes.",
                    AssessmentItemType.Numeric, D(("position", m), ("hour", n)));

            case "usccss.b01.rectangle.unit_squares":
                a = r.Next(2, 4 + level);
                b = r.Next(2, Math.Max(3, 25 / a) + 1);
                return Make(f,
                    $"A rectangle is partitioned into {a} rows of {b} EQUAL unit squares " +
                    "with no gaps or overlaps. How many unit squares cover it?",
                    "Count the same number of unit squares in every row, then multiply rows by columns.",
                    AssessmentItemType.Numeric, D(("rows", a), ("columns", b)));

            case "usccss.b01.measure.map_lengths":
                a = r.Next(2, 11);
                b = r.Next(2, Math.Min(21 - a, 11));
                m = r.Next(0, 2);
                return Make(f,
                    m == 0
                        ? $"Two adjoining paths on a map measure {a} cm and {b} cm. " +
                          "What is their combined measured length in centimeters?"
                        : $"Two route lengths measured on a map are {a+b} cm and {a} cm. " +
                          "How many centimeters longer is the first?",
                    "Add adjoining lengths, or subtract to compare the measured paths.",
                    AssessmentItemType.Numeric, D(("a", a), ("b", b), ("mode", m)));

            case "usccss.b01.graph.choose_scale":
                a = new[] { 2, 5, 10 }[r.Next(0, 3)];
                n = r.Next(0, 5);
                return Make(f,
                    $"A bar graph should show counts {a}, {a*(n%3+1)}, {a*(3+(n%2))}, and {4*a}. " +
                    $"Which convenient vertical-axis scale fits the data in exactly four equal steps: " +
                    $"1, {a}, or {a*2}? Enter the chosen step size.",
                    "Choose equal intervals so the largest bar reaches the fourth tick.",
                    AssessmentItemType.Numeric, D(("step", a), ("dataVariant", n)));

            case "usccss.b01.array.equal_groups":
                a = r.Next(2, 5 + level);
                b = r.Next(2, Math.Min(11, 101 / a));
                return Make(f,
                    $"Arrange objects into {a} equal rows with {b} objects in each row. " +
                    "How many objects are in the rectangular array?",
                    "An array contains rows × objects per row; its total can also be counted by equal groups.",
                    AssessmentItemType.Numeric, D(("rows", a), ("columns", b)));

            case "usccss.b01.geometry.composite_rectangle_area":
                a = r.Next(2, 6); b = r.Next(2, 7); n = r.Next(1, 5);
                return Make(f,
                    $"A floor consists of two non-overlapping rectangles, both {a} units tall, " +
                    $"with widths {b} units and {n} units. What is their COMBINED area in square units?",
                    "Find each rectangular area, add them, and check by multiplying the combined width by height.",
                    AssessmentItemType.Numeric, D(("height", a), ("width1", b), ("width2", n)));

            case "usccss.b01.division.unknown_factor":
                a = r.Next(2, 11); b = r.Next(2, 11);
                return Make(f,
                    $"An unknown number of equal groups each has {a} objects. " +
                    $"Altogether there are {a*b} objects. What is the unknown number of groups?",
                    "Use the missing-factor equation: groups × objects in each group = total.",
                    AssessmentItemType.Numeric, D(("objectsPerGroup", a), ("groups", b)));

            case "usccss.b01.fraction.name_parts":
                d = new[] { 2, 3, 4, 6, 8 }[r.Next(0, 5)];
                n = r.Next(1, d);
                return Make(f,
                    $"One whole is partitioned into {d} EQUAL parts and {n} of those parts are shaded. " +
                    "Write the shaded fraction in numerator/denominator form (do not simplify).",
                    "The numerator counts shaded parts; the denominator counts equal pieces in the whole.",
                    AssessmentItemType.ShortAnswer, D(("numerator", n), ("denominator", d)));

            case "usccss.b01.measure.quarter_inch":
                n = r.Next(0, 2) == 0 ? 1 : 3;
                a = r.Next(0, 4);
                return Make(f,
                    $"An inch ruler is marked in FOURTHS. A segment stops at {a} whole inch(es) " +
                    $"plus {n} quarter-inch mark(s). Write its length as an improper fraction " +
                    "of an inch (keep denominator 4).",
                    "Count whole inches as 4 quarter-inch parts each, then add the remaining parts.",
                    AssessmentItemType.ShortAnswer, D(("quarters", n), ("wholeInches", a)));

            case "usccss.b01.factors.factor_pairs":
                a = r.Next(2, 9); b = r.Next(2, 10);
                return Make(f,
                    $"A rectangular tile design has an area of {a*b} unit squares. " +
                    $"One side is {a} unit squares long. What is its other factor/side length?",
                    "A factor pair makes the area: first side × second side = area.",
                    AssessmentItemType.Numeric, D(("firstFactor", a), ("secondFactor", b)));

            case "usccss.b01.factors.prime_composite":
                n = r.Next(2, 101);
                return Make(f,
                    $"Classify {n}: prime or composite? " +
                    "A prime number has exactly two positive factors.",
                    "Check whether any whole number from 2 through the square root divides the number.",
                    AssessmentItemType.ShortAnswer, D(("number", n)));

            case "usccss.b01.factors.locker_square":
                n = r.Next(1, 21);
                return Make(f,
                    $"In the locker problem, locker {n} is toggled once for every positive factor of {n}. " +
                    "It starts closed. Is it open or closed after all toggles?",
                    "Only a perfect square has an odd number of positive factors, so it ends open.",
                    AssessmentItemType.ShortAnswer, D(("locker", n)));

            case "usccss.b01.factors.factors_multiples":
                a = r.Next(2, 11); b = r.Next(2, 11);
                m = r.Next(0, 2);
                n = r.Next(0, 2);
                return Make(f,
                    m == 0
                        ? $"Is {a} a factor of {a*b+(n==0 ? 0 : 1)}? Answer yes or no."
                        : $"Is {a*b+(n==0 ? 0 : 1)} a multiple of {a}? Answer yes or no.",
                    "A factor divides the number without a remainder; a multiple is a product with a whole number.",
                    AssessmentItemType.ShortAnswer, D(("a", a), ("b", b), ("mode", m), ("offset", n)));

            case "usccss.b01.fraction.whole_multiply":
                d = r.Next(3, 10); n = r.Next(2, d); a = r.Next(2, 6);
                return Make(f,
                    $"Find {a} groups of the non-unit fraction {n}/{d}. " +
                    "Write the result as a fraction, simplifying if possible.",
                    "Add equal fractions or multiply the numerator by the number of groups, and simplify.",
                    AssessmentItemType.ShortAnswer, D(("numerator", n), ("denominator", d), ("whole", a)));

            case "usccss.b01.fraction.decompose_same_denominator":
                d = r.Next(4, 11); n = r.Next(2, d+1); a = r.Next(1, n);
                return Make(f,
                    $"Decompose {n}/{d} as {a}/{d} + __/{d}. " +
                    "What fraction completes this equality (keep the same denominator)?",
                    "With identical denominators, the missing numerator is the difference of the numerators.",
                    AssessmentItemType.ShortAnswer, D(("wholeN", n), ("firstN", a), ("denominator", d)));

            case "usccss.b01.place_value.ten_times":
                a = r.Next(1, 10); b = r.Next(1, 4);
                var power = b == 1 ? 10 : b == 2 ? 100 : 1000;
                return Make(f,
                    $"The digit {a} has value {a*power} in one number and value {a*power*10} " +
                    "in another number. How many times as large is its place value in the second?",
                    "Moving a digit one place to the left multiplies its value by ten.",
                    AssessmentItemType.Numeric, D(("digit", a), ("place", power)));
            default: throw new InvalidOperationException("Unknown source-target problem family.");
        }
    }

    private static int Gcd(int a, int b)
    {
        while (b != 0) { (a, b) = (b, a % b); }
        return a;
    }

    private static string ReducedFraction(int n, int d)
    {
        var g = Gcd(n, d);
        return (n / g).ToString(CultureInfo.InvariantCulture) + "/" +
            (d / g).ToString(CultureInfo.InvariantCulture);
    }

    public static string Solve(string f, IReadOnlyDictionary<string, int> p)
    {
        int P(string key) => p[key];
        return f switch
        {
            "usccss.b01.market.inventory_100" =>
                (P("mode") == 0 ? P("second") - P("sold") : P("third"))
                    .ToString(CultureInfo.InvariantCulture),
            "usccss.b01.triangle.attributes" => P("position").ToString(CultureInfo.InvariantCulture),
            "usccss.b01.partition.halves_fourths" => P("denominator") == 2 ? "half" : "fourth",
            "usccss.b01.number_line.equal_intervals" =>
                (2 * P("step")).ToString(CultureInfo.InvariantCulture),
            "usccss.b01.clock.read_half_hour" or
            "usccss.b01.clock.read_five_minutes" =>
                $"{P("hour")}:{P("minutes"):00}",
            "usccss.b01.clock.half_quarters" =>
                (P("position") + 1).ToString(CultureInfo.InvariantCulture) switch
                {
                    "1" => "15", "2" => "30", "3" => "45", _ => throw new InvalidOperationException()
                },
            "usccss.b01.rectangle.unit_squares" or
            "usccss.b01.array.equal_groups" =>
                (P("rows") * P("columns")).ToString(CultureInfo.InvariantCulture),
            "usccss.b01.measure.map_lengths" =>
                (P("mode") == 0 ? P("a") + P("b") : P("b"))
                    .ToString(CultureInfo.InvariantCulture),
            "usccss.b01.graph.choose_scale" => P("step").ToString(CultureInfo.InvariantCulture),
            "usccss.b01.geometry.composite_rectangle_area" =>
                (P("height") * (P("width1") + P("width2"))).ToString(CultureInfo.InvariantCulture),
            "usccss.b01.division.unknown_factor" =>
                P("groups").ToString(CultureInfo.InvariantCulture),
            "usccss.b01.fraction.name_parts" =>
                $"{P("numerator")}/{P("denominator")}",
            "usccss.b01.measure.quarter_inch" =>
                $"{P("wholeInches") * 4 + P("quarters")}/4",
            "usccss.b01.factors.factor_pairs" =>
                P("secondFactor").ToString(CultureInfo.InvariantCulture),
            "usccss.b01.factors.prime_composite" =>
                IsPrime(P("number")) ? "prime" : "composite",
            "usccss.b01.factors.locker_square" =>
                IsPerfectSquare(P("locker")) ? "open" : "closed",
            "usccss.b01.factors.factors_multiples" =>
                P("offset") == 0 ? "yes" : "no",
            "usccss.b01.fraction.whole_multiply" =>
                ReducedFraction(P("numerator") * P("whole"), P("denominator")),
            "usccss.b01.fraction.decompose_same_denominator" =>
                $"{P("wholeN") - P("firstN")}/{P("denominator")}",
            "usccss.b01.place_value.ten_times" => "10",
            _ => throw new InvalidOperationException("Unknown B01 family")
        };
    }

    private static bool IsPrime(int number)
    {
        if (number < 2) return false;
        for (var i = 2; i * i <= number; i++)
            if (number % i == 0) return false;
        return true;
    }

    private static bool IsPerfectSquare(int x)
    {
        if (x < 1) return false;
        var r = (int)Math.Sqrt(x);
        return r * r == x;
    }

    // The verifier must reject a modified parameter payload even if someone
    // supplies an answer consistent with those modified parameters.
    private static bool ValidDomain(string family, IReadOnlyDictionary<string, int> p)
    {
        bool Range(string key,int lo,int hi) =>
            p.TryGetValue(key,out var v) && v>=lo && v<=hi;
        int N(string key) => p[key];
        return family switch
        {
            "usccss.b01.market.inventory_100" =>
                Range("small",2,9) && Range("second",20,60) &&
                Range("third",11,78) &&
                N("small") + N("second") + N("third") == 100 &&
                Range("sold",1,11) && N("sold") < N("second") &&
                Range("mode",0,1),
            "usccss.b01.triangle.attributes" =>
                Range("position",1,3) && Range("style",0,3),
            "usccss.b01.partition.halves_fourths" =>
                (N("denominator") is 2 or 4) && Range("shape",0,1),
            "usccss.b01.number_line.equal_intervals" => Range("step",1,40),
            "usccss.b01.clock.read_half_hour" =>
                Range("hour",1,12) && (N("minutes") is 0 or 30),
            "usccss.b01.clock.read_five_minutes" =>
                Range("hour",1,12) && Range("minutes",0,55) &&
                N("minutes")%5==0,
            "usccss.b01.clock.half_quarters" =>
                Range("position",0,2) && Range("hour",1,12),
            "usccss.b01.rectangle.unit_squares" =>
                Range("rows",2,7) && Range("columns",2,13) &&
                N("rows")*N("columns")<=35,
            "usccss.b01.measure.map_lengths" =>
                Range("a",2,10) && Range("b",2,10) &&
                N("a")+N("b")<=20 && Range("mode",0,1),
            "usccss.b01.graph.choose_scale" =>
                (N("step") is 2 or 5 or 10) && Range("dataVariant",0,4),
            "usccss.b01.array.equal_groups" =>
                Range("rows",2,7) && Range("columns",2,11) &&
                N("rows")*N("columns")<=100,
            "usccss.b01.geometry.composite_rectangle_area" =>
                Range("height",2,5) && Range("width1",2,6) &&
                Range("width2",1,4),
            "usccss.b01.division.unknown_factor" =>
                Range("objectsPerGroup",2,10) && Range("groups",2,10),
            "usccss.b01.fraction.name_parts" =>
                (N("denominator") is 2 or 3 or 4 or 6 or 8) &&
                Range("numerator",1,N("denominator")-1),
            "usccss.b01.measure.quarter_inch" =>
                (N("quarters") is 1 or 3) && Range("wholeInches",0,3),
            "usccss.b01.factors.factor_pairs" =>
                Range("firstFactor",2,8) && Range("secondFactor",2,9),
            "usccss.b01.factors.prime_composite" => Range("number",2,100),
            "usccss.b01.factors.locker_square" => Range("locker",1,20),
            "usccss.b01.factors.factors_multiples" =>
                Range("a",2,10) && Range("b",2,10) &&
                Range("mode",0,1) && Range("offset",0,1),
            "usccss.b01.fraction.whole_multiply" =>
                Range("denominator",3,9) &&
                Range("numerator",2,N("denominator")-1) && Range("whole",2,5),
            "usccss.b01.fraction.decompose_same_denominator" =>
                Range("denominator",4,10) &&
                Range("wholeN",2,N("denominator")) &&
                Range("firstN",1,N("wholeN")-1),
            "usccss.b01.place_value.ten_times" =>
                Range("digit",1,9) && (N("place") is 10 or 100 or 1000),
            _ => false
        };
    }

    public static bool Verify(string family, IReadOnlyDictionary<string, int> p, string answer)
    {
        if (!Supports(family) || answer is null) return false;
        try
        {
            if (!ValidDomain(family,p)) return false;
            // Independent CI oracles cross-check the mathematical result from
            // the structured generation parameters across all 19 families.
            return string.Equals(answer.Trim(), Solve(family,p),
                StringComparison.OrdinalIgnoreCase);
        }
        catch (Exception e) when (e is KeyNotFoundException or
            InvalidOperationException or DivideByZeroException or OverflowException)
        {
            return false;
        }
    }
}
