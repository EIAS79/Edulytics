using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

internal static class FoundationPracticeEngine
{
    private static readonly IReadOnlySet<string> Families =
        new HashSet<string>(StringComparer.Ordinal)
        {
            "supporting.number.count_cardinality",
            "supporting.sequences.next_term",
            "supporting.number.numeral_identity",
            "supporting.number.teen_decompose",
            "supporting.number.ordinal_position",
            "number.whole.add_subtract.complement_10",
            "number.whole.add_subtract.within_20.reasonable",
            "number.whole.multiply.double",
            "supporting.measurement.calendar_next_day",
            "supporting.measurement.clock_half_hour",
            "supporting.geometry.face_count",
            "supporting.geometry.position_direction",
            "supporting.measurement.choose_tool",
            "supporting.statistics.category_total",
            "supporting.statistics.compare_category_counts",
            "supporting.algebra.systems_inequalities.mixed",
            "supporting.sequences.skip_count",
            "supporting.geometry.angle_measure",
            "supporting.statistics.statistical_question",
            "supporting.statistics.center_or_variation",
            "supporting.measurement.unit_iteration",
            "supporting.measurement.indirect_compare",
            "supporting.number.estimate_closer",
            "supporting.measurement.money_marked_value",
            "supporting.geometry.orientation_invariant"
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
            "supporting.number.count_cardinality" => CountCardinality(random, scale),
            "supporting.sequences.next_term" => NextTerm(random, scale),
            "supporting.number.numeral_identity" => NumeralIdentity(random),
            "supporting.number.teen_decompose" => TeenDecompose(random),
            "supporting.number.ordinal_position" => OrdinalPosition(random),
            "number.whole.add_subtract.complement_10" => ComplementTen(random),
            "number.whole.add_subtract.within_20.reasonable" => ReasonableWithinTwenty(random),
            "number.whole.multiply.double" => DoubleNumber(random),
            "supporting.measurement.calendar_next_day" => CalendarNextDay(random),
            "supporting.measurement.clock_half_hour" => ClockHalfHour(random),
            "supporting.geometry.face_count" => FaceCount(random),
            "supporting.geometry.position_direction" => PositionDirection(random),
            "supporting.measurement.choose_tool" => ChooseTool(random),
            "supporting.statistics.category_total" => CategoryTotal(random, scale),
            "supporting.statistics.compare_category_counts" => CompareCategoryCounts(random, scale),
            "supporting.algebra.systems_inequalities.mixed" => SystemsInequalities(random, scale),
            "supporting.sequences.skip_count" => SkipCount(random, scale),
            "supporting.geometry.angle_measure" => AngleMeasure(random),
            "supporting.statistics.statistical_question" => StatisticalQuestion(random),
            "supporting.statistics.center_or_variation" => CenterOrVariation(random, scale),
            "supporting.measurement.unit_iteration" => UnitIteration(random, scale),
            "supporting.measurement.indirect_compare" => IndirectCompare(random, scale),
            "supporting.number.estimate_closer" => EstimateCloser(random, scale),
            "supporting.measurement.money_marked_value" => MoneyMarkedValue(random),
            "supporting.geometry.orientation_invariant" => OrientationInvariant(random),
            _ => throw new InvalidOperationException(
                $"Unsupported Foundation Practice family: {family}")
        };

    public static string Solve(
        string family,
        IReadOnlyDictionary<string, int> p) =>
        family switch
        {
            "supporting.number.count_cardinality" =>
                p["count"].ToString(CultureInfo.InvariantCulture),
            "supporting.sequences.next_term" =>
                (p["current"] + p["direction"] * p["step"])
                    .ToString(CultureInfo.InvariantCulture),
            "supporting.number.numeral_identity" =>
                p["value"].ToString(CultureInfo.InvariantCulture),
            "supporting.number.teen_decompose" =>
                (p["value"] - 10).ToString(CultureInfo.InvariantCulture),
            "supporting.number.ordinal_position" =>
                Ordinal(p["position"]),
            "number.whole.add_subtract.complement_10" =>
                (10 - p["part"]).ToString(CultureInfo.InvariantCulture),
            "number.whole.add_subtract.within_20.reasonable" =>
                (p["operation"] == 0
                    ? p["left"] + p["right"]
                    : p["left"] - p["right"])
                .ToString(CultureInfo.InvariantCulture),
            "number.whole.multiply.double" =>
                (2 * p["value"]).ToString(CultureInfo.InvariantCulture),
            "supporting.measurement.calendar_next_day" =>
                DayName(Mod7(p["day"] + p["direction"])),
            "supporting.measurement.clock_half_hour" =>
                ClockAnswer(p["hour"], p["startHalf"]),
            "supporting.geometry.face_count" =>
                FaceCountForShape(p["shape"]).ToString(CultureInfo.InvariantCulture),
            "supporting.geometry.position_direction" =>
                (p["start"] + p["direction"] * p["steps"])
                .ToString(CultureInfo.InvariantCulture),
            "supporting.measurement.choose_tool" =>
                ToolName(p["context"]),
            "supporting.statistics.category_total" =>
                (p["a"] + p["b"] + p["c"]).ToString(CultureInfo.InvariantCulture),
            "supporting.statistics.compare_category_counts" =>
                p["a"] > p["b"] ? "A" : p["b"] > p["a"] ? "B" : "equal",
            "supporting.algebra.systems_inequalities.mixed" =>
                p["mode"] == 0
                    ? $"x={p["x"]}, y={p["y"]}"
                    : $"x<{p["k"]}",
            "supporting.sequences.skip_count" =>
                (p["current"] + p["step"]).ToString(CultureInfo.InvariantCulture),
            "supporting.geometry.angle_measure" =>
                p["angle"].ToString(CultureInfo.InvariantCulture),
            "supporting.statistics.statistical_question" =>
                p["statistical"] == 1 ? "statistical" : "not statistical",
            "supporting.statistics.center_or_variation" =>
                p["kind"] == 0 ? "center" : "variation",
            "supporting.measurement.unit_iteration" =>
                p["units"].ToString(CultureInfo.InvariantCulture),
            "supporting.measurement.indirect_compare" =>
                p["aOffset"] > p["bOffset"] ? "A" : "B",
            "supporting.number.estimate_closer" =>
                p["firstError"] <= p["secondError"] ? "first" : "second",
            "supporting.measurement.money_marked_value" =>
                p["value"].ToString(CultureInfo.InvariantCulture),
            "supporting.geometry.orientation_invariant" =>
                ShapeName(p["shape"]),
            _ => throw new InvalidOperationException(
                $"Unsupported Foundation Practice solver family: {family}")
        };

    public static bool Verify(
        string family,
        IReadOnlyDictionary<string, int> p,
        string answer)
    {
        if (!Supports(family))
            return false;

        return string.Equals(
            Normalize(answer),
            Normalize(Solve(family, p)),
            StringComparison.Ordinal);
    }

    private static Problem CountCardinality(Random r, int scale)
    {
        var count = r.Next(0, Math.Min(21, 9 + scale * 5));
        return P(
            "supporting.number.count_cardinality",
            $"A group contains {count} counters. How many counters are in the group?",
            "Count every object exactly once. The final number word is the total.",
            ("count", count));
    }

    private static Problem NextTerm(Random r, int scale)
    {
        var step = r.Next(1, Math.Min(6, 2 + scale * 2));
        var direction = r.Next(0, 2) == 0 ? 1 : -1;
        var current = direction > 0
            ? r.Next(1, 20)
            : r.Next(step + 1, 21);
        var previous2 = current - 2 * direction * step;
        var previous1 = current - direction * step;
        if (previous2 < 0)
        {
            direction = 1;
            previous2 = Math.Max(0, current - 2 * step);
            previous1 = previous2 + step;
            current = previous1 + step;
        }

        return P(
            "supporting.sequences.next_term",
            $"Continue the number sequence: {previous2}, {previous1}, {current}, __.",
            "Find the constant step between consecutive terms and apply it once more.",
            ("current", current),
            ("step", step),
            ("direction", direction));
    }

    private static Problem NumeralIdentity(Random r)
    {
        var value = r.Next(0, 21);
        return P(
            "supporting.number.numeral_identity",
            $"Write the numeral for the number word “{NumberWord(value)}”.",
            "Match the spoken or written number name to the numeral representing the same quantity.",
            AssessmentItemType.ShortAnswer,
            ("value", value));
    }

    private static Problem TeenDecompose(Random r)
    {
        var value = r.Next(11, 21);
        return P(
            "supporting.number.teen_decompose",
            $"Complete the decomposition: {value} = 10 + __.",
            "A teen number is one ten plus its remaining ones.",
            ("value", value));
    }

    private static Problem OrdinalPosition(Random r)
    {
        var position = r.Next(1, 11);
        return P(
            "supporting.number.ordinal_position",
            $"Counting from the left, an object is in position {position}. Write its ordinal position (for example, 1st).",
            "Ordinal numbers describe position in an ordered line.",
            AssessmentItemType.ShortAnswer,
            ("position", position));
    }

    private static Problem ComplementTen(Random r)
    {
        var part = r.Next(0, 11);
        return P(
            "number.whole.add_subtract.complement_10",
            $"Complete the number bond: {part} + __ = 10.",
            "A complement to 10 is the amount needed to make a total of 10.",
            ("part", part));
    }

    private static Problem ReasonableWithinTwenty(Random r)
    {
        var add = r.Next(0, 2) == 0;
        if (add)
        {
            var left = r.Next(1, 11);
            var right = r.Next(1, 21 - left);
            return P(
                "number.whole.add_subtract.within_20.reasonable",
                $"Estimate whether {left} + {right} should be larger than either addend, then calculate the exact total.",
                "For positive whole numbers, addition increases the amount. Calculate and check the result against that expectation.",
                ("left", left),
                ("right", right),
                ("operation", 0));
        }

        var larger = r.Next(2, 21);
        var smaller = r.Next(0, larger + 1);
        return P(
            "number.whole.add_subtract.within_20.reasonable",
            $"Estimate whether {larger} - {smaller} should be no greater than {larger}, then calculate the exact difference.",
            "Subtracting a non-negative amount cannot increase the starting whole number. Calculate and check the result.",
            ("left", larger),
            ("right", smaller),
            ("operation", 1));
    }

    private static Problem DoubleNumber(Random r)
    {
        var value = r.Next(0, 11);
        return P(
            "number.whole.multiply.double",
            $"Double {value}.",
            "A double is two equal groups, so calculate value + value.",
            ("value", value));
    }

    private static Problem CalendarNextDay(Random r)
    {
        var day = r.Next(0, 7);
        var direction = r.Next(0, 2) == 0 ? 1 : -1;
        var word = direction > 0 ? "tomorrow" : "yesterday";
        return P(
            "supporting.measurement.calendar_next_day",
            $"If today is {DayName(day)}, what day is {word}?",
            "Days follow a repeating seven-day cycle. Move one position forward for tomorrow or one backward for yesterday.",
            AssessmentItemType.ShortAnswer,
            ("day", day),
            ("direction", direction));
    }

    private static Problem ClockHalfHour(Random r)
    {
        var hour = r.Next(1, 13);
        var startHalf = r.Next(0, 2);
        var start = startHalf == 0
            ? $"{hour}:00"
            : $"{hour}:30";
        return P(
            "supporting.measurement.clock_half_hour",
            $"What time is 30 minutes after {start}?",
            "Half an hour is 30 minutes. Add 30 minutes and move to the next hour when necessary.",
            AssessmentItemType.ShortAnswer,
            ("hour", hour),
            ("startHalf", startHalf));
    }

    private static Problem FaceCount(Random r)
    {
        var shape = r.Next(0, 3);
        var variant = r.Next(0, 12);
        var name = shape switch
        {
            0 => "cube",
            1 => "cuboid",
            _ => "triangular prism"
        };
        var prompt = (variant % 3) switch
        {
            0 => $"How many flat faces does a {name} have?",
            1 => $"A solid model is a {name}. Count all of its flat faces.",
            _ => $"Imagine a {name} being turned so every face can be seen. How many flat faces are there altogether?"
        };
        return P(
            "supporting.geometry.face_count",
            prompt,
            "Count each flat polygonal surface exactly once; turning the solid does not change its number of faces.",
            ("shape", shape),
            ("variant", variant));
    }

    private static Problem PositionDirection(Random r)
    {
        var direction = r.Next(0, 2) == 0 ? 1 : -1;
        var steps = r.Next(1, 5);
        var start = direction > 0
            ? r.Next(0, 11)
            : r.Next(steps, 15);
        var word = direction > 0 ? "right" : "left";
        return P(
            "supporting.geometry.position_direction",
            $"Start at position {start} on a number line and move {steps} step(s) to the {word}. Where do you stop?",
            "A move right increases position; a move left decreases position.",
            ("start", start),
            ("steps", steps),
            ("direction", direction));
    }

    private static Problem ChooseTool(Random r)
    {
        var context = r.Next(0, 3);
        var variant = r.Next(0, 8);
        var objectName = context switch
        {
            0 => (variant % 4) switch
            {
                0 => "the length of a book",
                1 => "the width of a desk",
                2 => "the height of a plant",
                _ => "the length of a pencil"
            },
            1 => (variant % 4) switch
            {
                0 => "the mass of a bag of apples",
                1 => "the mass of a school bag",
                2 => "the mass of a parcel",
                _ => "the mass of a watermelon"
            },
            _ => (variant % 4) switch
            {
                0 => "the volume of water in a jug",
                1 => "the amount of juice in a bottle",
                2 => "the liquid in a measuring container",
                _ => "the capacity of a small bucket"
            }
        };
        return P(
            "supporting.measurement.choose_tool",
            $"Which tool is most appropriate for measuring {objectName}?",
            "Choose a tool designed for the attribute being measured: length, mass, or liquid capacity.",
            AssessmentItemType.ShortAnswer,
            ("context", context),
            ("variant", variant));
    }

    private static Problem CategoryTotal(Random r, int scale)
    {
        var a = r.Next(1, 5 + scale * 2);
        var b = r.Next(1, 5 + scale * 2);
        var c = r.Next(1, 5 + scale * 2);
        return P(
            "supporting.statistics.category_total",
            $"A category table has counts A={a}, B={b}, C={c}. How many data items are shown altogether?",
            "Add the category frequencies to find the total number of data items.",
            ("a", a), ("b", b), ("c", c));
    }

    private static Problem CompareCategoryCounts(Random r, int scale)
    {
        var a = r.Next(1, 8 + scale * 3);
        var b = r.Next(1, 8 + scale * 3);
        return P(
            "supporting.statistics.compare_category_counts",
            $"A chart shows category A={a} and category B={b}. Which category has more items? Enter A, B, or equal.",
            "Compare the two category frequencies directly.",
            AssessmentItemType.ShortAnswer,
            ("a", a), ("b", b));
    }

    private static Problem EstimateCloser(Random r, int scale)
    {
        var exact = r.Next(5, 20 + scale * 5);
        var nearError = r.Next(1, 3 + scale);
        var farError = nearError + r.Next(4, 10 + scale * 3);
        var near = Math.Max(0, exact + (r.Next(0, 2) == 0 ? -nearError : nearError));
        var far = Math.Max(0, exact + (r.Next(0, 2) == 0 ? -farError : farError));
        var firstIsNear = r.Next(0, 2) == 0;
        var first = firstIsNear ? near : far;
        var second = firstIsNear ? far : near;
        return P(
            "supporting.number.estimate_closer",
            $"A collection is counted exactly as {exact}. Which earlier estimate was more reasonable, the first estimate {first} or the second estimate {second}? Enter first or second.",
            "A reasonable estimate should be close to the exact count. Compare the absolute errors.",
            AssessmentItemType.ShortAnswer,
            ("firstError", Math.Abs(first - exact)),
            ("secondError", Math.Abs(second - exact)),
            ("exact", exact));
    }

    private static Problem MoneyMarkedValue(Random r)
    {
        int[] values = [1, 2, 5, 10, 20, 50];
        var value = values[r.Next(values.Length)];
        var variant = r.Next(0, 8);
        var prompt = (variant % 3) switch
        {
            0 => $"A coin or note is clearly marked with the value {value}. What value does it represent?",
            1 => $"A piece of money shows the denomination {value}. State its monetary value.",
            _ => $"You are sorting money by the printed denomination. An item is marked {value}. What value should you record?"
        };
        return P(
            "supporting.measurement.money_marked_value",
            prompt,
            "Read the printed denomination; physical size does not determine monetary value.",
            AssessmentItemType.ShortAnswer,
            ("value", value),
            ("variant", variant));
    }

    private static Problem OrientationInvariant(Random r)
    {
        var shape = r.Next(0, 3);
        var name = ShapeName(shape);
        var turns = r.Next(1, 5);
        return P(
            "supporting.geometry.orientation_invariant",
            $"A {name} is rotated by {turns} quarter-turn(s). What shape is it after the rotation?",
            "Rotation changes orientation but does not change defining shape properties.",
            AssessmentItemType.ShortAnswer,
            ("shape", shape),
            ("turns", turns));
    }

    private static Problem IndirectCompare(Random r, int scale)
    {
        var aOffset = r.Next(1, 4 + scale);
        var bOffset = -r.Next(1, 4 + scale);
        if (r.Next(0, 2) == 0)
            (aOffset, bOffset) = (bOffset, aOffset);

        var aPhrase = aOffset > 0
            ? $"{Math.Abs(aOffset)} unit(s) longer than C"
            : $"{Math.Abs(aOffset)} unit(s) shorter than C";
        var bPhrase = bOffset > 0
            ? $"{Math.Abs(bOffset)} unit(s) longer than C"
            : $"{Math.Abs(bOffset)} unit(s) shorter than C";

        return P(
            "supporting.measurement.indirect_compare",
            $"Object A is {aPhrase}. Object B is {bPhrase}. Which object is longer, A or B?",
            "Use object C as the common reference. The object farther above C, or less far below C, is longer.",
            AssessmentItemType.ShortAnswer,
            ("aOffset", aOffset),
            ("bOffset", bOffset));
    }

    private static Problem UnitIteration(Random r, int scale)
    {
        var units = r.Next(2, 9 + scale * 3);
        return P(
            "supporting.measurement.unit_iteration",
            $"An object is covered end-to-end by {units} equal unit tiles with no gaps or overlaps. What is its length in unit tiles?",
            "When equal units cover an object end-to-end with no gaps or overlaps, the measurement is the number of units used.",
            ("units", units));
    }

    private static Problem SkipCount(Random r, int scale)
    {
        int[] steps = [2, 5, 10, 100];
        var step = steps[r.Next(Math.Min(steps.Length, 2 + scale))];
        var current = step * r.Next(1, 8 + scale * 2);
        return P(
            "supporting.sequences.skip_count",
            $"Continue the skip-counting sequence: {current - 2 * step}, {current - step}, {current}, __.",
            "Find the constant skip-counting interval and add it once more.",
            ("current", current), ("step", step));
    }

    private static Problem AngleMeasure(Random r)
    {
        var angle = r.Next(10, 171);
        return P(
            "supporting.geometry.angle_measure",
            $"A protractor has its baseline at 0° and the other ray passes through {angle}°. What is the angle measure?",
            "Read the scale that starts at the aligned 0° baseline and ends at the second ray.",
            ("angle", angle));
    }

    private static Problem StatisticalQuestion(Random r)
    {
        var variant = r.Next(0, 12);
        var statistical = variant % 2;
        var prompt = variant switch
        {
            0 => "How many minutes did Alex spend reading today?",
            1 => "How many minutes do students in this class spend reading each day?",
            2 => "What is Mia's shoe size?",
            3 => "What shoe sizes do students in Year 6 wear?",
            4 => "How long did one bus journey take this morning?",
            5 => "How long do bus journeys on this route usually take?",
            6 => "How many goals did this team score in yesterday's match?",
            7 => "How many goals does this team score per match across the season?",
            8 => "What was today's temperature at noon?",
            9 => "How does the noon temperature vary over this month?",
            10 => "How many pages are in this particular book?",
            _ => "How many pages are in the books students choose from the class library?"
        };
        return P(
            "supporting.statistics.statistical_question",
            $"Classify the question as statistical or not statistical: “{prompt}”",
            "A statistical question anticipates variability across a group or repeated observations.",
            AssessmentItemType.ShortAnswer,
            ("statistical", statistical),
            ("variant", variant));
    }

    private static Problem CenterOrVariation(Random r, int scale)
    {
        var kind = r.Next(0, 2);
        var value = r.Next(1, 10 + scale * 3);
        var term = kind == 0 ? "mean" : "range";
        return P(
            "supporting.statistics.center_or_variation",
            $"A data summary reports a {term} of {value}. Does the {term} describe center or variation?",
            "Measures such as mean/median describe center; measures such as range/IQR describe variation.",
            AssessmentItemType.ShortAnswer,
            ("kind", kind), ("value", value));
    }

    private static Problem SystemsInequalities(Random r, int scale)
    {
        var mode = r.Next(0, 2);
        if (mode == 0)
        {
            var x = r.Next(-5 - scale, 6 + scale);
            var y = r.Next(-5 - scale, 6 + scale);
            var sum = x + y;
            var difference = x - y;
            return P(
                "supporting.algebra.systems_inequalities.mixed",
                $"Solve the system x + y = {sum} and x - y = {difference}. Give x and y.",
                "Add the equations to eliminate y, solve for x, then substitute to find y and check both equations.",
                AssessmentItemType.ShortAnswer,
                ("mode", 0), ("x", x), ("y", y), ("k", 0));
        }

        var a = r.Next(1, 5 + scale);
        var k = r.Next(-5 - scale, 6 + scale);
        var b = r.Next(-6 - scale, 7 + scale);
        var right = a * k + b;
        return P(
            "supporting.algebra.systems_inequalities.mixed",
            $"Solve the inequality {a}x + {b} < {right}.",
            "Subtract the constant and divide by the positive coefficient; because the coefficient is positive, the inequality direction stays the same.",
            AssessmentItemType.ShortAnswer,
            ("mode", 1), ("x", 0), ("y", 0), ("k", k));
    }

    private static string ClockAnswer(int hour, int startHalf)
    {
        if (startHalf == 0)
            return $"{hour}:30";
        var next = hour == 12 ? 1 : hour + 1;
        return $"{next}:00";
    }

    private static int FaceCountForShape(int shape) =>
        shape switch
        {
            0 => 6,
            1 => 6,
            2 => 5,
            _ => throw new InvalidOperationException("Unknown foundation solid shape.")
        };

    private static string ShapeName(int shape) =>
        shape switch
        {
            0 => "triangle",
            1 => "rectangle",
            2 => "square",
            _ => throw new InvalidOperationException("Unknown foundation 2D shape.")
        };

    private static string ToolName(int context) =>
        context switch
        {
            0 => "ruler",
            1 => "scale",
            2 => "measuring jug",
            _ => throw new InvalidOperationException("Unknown measurement context.")
        };

    private static int Mod7(int value) => ((value % 7) + 7) % 7;

    private static string DayName(int day) =>
        day switch
        {
            0 => "Monday",
            1 => "Tuesday",
            2 => "Wednesday",
            3 => "Thursday",
            4 => "Friday",
            5 => "Saturday",
            6 => "Sunday",
            _ => throw new InvalidOperationException("Invalid day index.")
        };

    private static string Ordinal(int n)
    {
        var suffix = n % 100 is 11 or 12 or 13
            ? "th"
            : (n % 10) switch
            {
                1 => "st",
                2 => "nd",
                3 => "rd",
                _ => "th"
            };
        return n.ToString(CultureInfo.InvariantCulture) + suffix;
    }

    private static string NumberWord(int value) =>
        value switch
        {
            0 => "zero", 1 => "one", 2 => "two", 3 => "three", 4 => "four",
            5 => "five", 6 => "six", 7 => "seven", 8 => "eight", 9 => "nine",
            10 => "ten", 11 => "eleven", 12 => "twelve", 13 => "thirteen",
            14 => "fourteen", 15 => "fifteen", 16 => "sixteen",
            17 => "seventeen", 18 => "eighteen", 19 => "nineteen",
            20 => "twenty",
            _ => throw new InvalidOperationException("Foundation numeral outside 0-20.")
        };

    private static string Normalize(string value) =>
        string.Join(
            " ",
            (value ?? string.Empty)
                .Trim()
                .ToLowerInvariant()
                .Split(
                    [' ', '\t', '\r', '\n'],
                    StringSplitOptions.RemoveEmptyEntries));

    private static Problem P(
        string family,
        string prompt,
        string solution,
        params (string Key, int Value)[] parameters) =>
        P(family, prompt, solution, AssessmentItemType.ShortAnswer, parameters);

    private static Problem P(
        string family,
        string prompt,
        string solution,
        AssessmentItemType itemType,
        params (string Key, int Value)[] parameters) =>
        new(
            family,
            prompt,
            solution,
            itemType,
            parameters.ToDictionary(x => x.Key, x => x.Value, StringComparer.Ordinal));
}
