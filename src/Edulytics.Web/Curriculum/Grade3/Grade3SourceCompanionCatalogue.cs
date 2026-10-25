using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Text.Json;

namespace Edulytics.Web.Curriculum.Grade3;

/// <summary>
/// Provisional first-party questions for G3 L09–L143. This registry deliberately
/// does NOT certify original publisher task, artwork, curriculum or Rich V2 parity.
/// </summary>
public static class Grade3SourceCompanionCatalogue
{
    public const string ResourceName = "Edulytics.Web.Grade3.UnverifiedOriginalCompanions";

    /// <summary>
    /// Mathematically redrawn data from the text and alternative-text description
    /// of the 2021 U01 L09/L10 scaled picture graph (not publisher artwork).
    /// Visual appearance, image rights and full 2021 diagram parity are not certified.
    /// </summary>
    public static IReadOnlyList<Grade3RoadSignCategory> Local2021RoadSignGraph { get; } =
        Array.AsReadOnly(new[]
        {
            new Grade3RoadSignCategory("Stop signs", 2),
            new Grade3RoadSignCategory("Yield signs", 1),
            new Grade3RoadSignCategory("Speed limit signs", 3),
            new Grade3RoadSignCategory("Street signs", 5)
        });

    public const int SourceGraphSignsPerSymbol = 2;

    private static readonly Lazy<IReadOnlyDictionary<string, Grade3CompanionLesson>> Values =
        new(Load, LazyThreadSafetyMode.ExecutionAndPublication);

    public static bool TryGet(string? code, out Grade3CompanionLesson? lesson)
    {
        if (code is not null && Values.Value.TryGetValue(code, out var found))
        {
            lesson = found;
            return true;
        }

        lesson = null;
        return false;
    }

    public static IReadOnlyCollection<string> Codes => Values.Value.Keys.ToArray();

    private static IReadOnlyDictionary<string, Grade3CompanionLesson> Load()
    {
        using var stream = typeof(Grade3SourceCompanionCatalogue).Assembly
            .GetManifestResourceStream(ResourceName)
            ?? throw new InvalidOperationException("Grade 3 source companion bank was not embedded.");
        var bank = JsonSerializer.Deserialize<Grade3CompanionBank>(stream,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            ?? throw new InvalidOperationException("Grade 3 companion bank is missing.");
        var result = new Dictionary<string, Grade3CompanionLesson>(StringComparer.Ordinal);
        foreach (var item in bank.Lessons)
        {
            if (item.Ordinal < 9 || item.Ordinal > 143 ||
                !item.LessonCode.StartsWith("PED:US-CCSS-MATH:G3:", StringComparison.Ordinal) ||
                !result.TryAdd(item.LessonCode, item))
                throw new InvalidOperationException("Invalid/duplicate Grade 3 lesson bank identity.");
            if (item.IndependentPractice is null || item.OriginalEdulyticsCoolDown is null)
                throw new InvalidOperationException("Missing first-party task for " + item.LessonCode);
        }
        if (result.Count != 135)
            throw new InvalidOperationException($"Expected 135 first-pass Grade 3 companion lessons, got {result.Count}.");
        return result;
    }

    /// <summary>
    /// Strict numeric checker. Equivalent reduced/unreduced rational answers are
    /// accepted, but drawings and explanations are ALWAYS teacher-reviewed.
    /// </summary>
    public static Grade3CompanionFeedback Grade(
        Grade3OriginalQuestion q, string? submitted)
    {
        if (q.AnswerType == "teacher")
            return new(null, true,
                "Open construction, dialogue or explanation: only teacher review can assess this activity.");

        if (string.IsNullOrWhiteSpace(submitted) || submitted.Length > 100)
            return new(false, true, "Enter an answer and explain your strategy to your teacher.");

        if (q.AnswerType == "shape-category-set")
        {
            // Source G3 U07 L05 has six set-valued names; square also
            // qualifies as both rectangle and rhombus. Drawings and
            // category explanations remain a teacher's responsibility.
            var categories = new[] { "triangle", "quadrilateral", "pentagon",
                "hexagon", "rhombus", "rectangle", "square" };
            var actual = submitted.Split(',', StringSplitOptions.TrimEntries);
            var expected = q.Expected?.Split('|', StringSplitOptions.TrimEntries);
            var valid = expected is not null &&
                actual.Length is >= 1 and <= 7 &&
                actual.Length == actual.Distinct(StringComparer.OrdinalIgnoreCase).Count() &&
                actual.All(x => categories.Contains(x, StringComparer.OrdinalIgnoreCase)) &&
                expected.All(x => categories.Contains(x, StringComparer.OrdinalIgnoreCase));
            var correct = valid && actual.Length == expected!.Length &&
                actual.ToHashSet(StringComparer.OrdinalIgnoreCase)
                    .SetEquals(expected);
            return new(correct, true, correct
                ? "All geometric categories match this inspected 2021 source figure. Teacher still reviews the justification."
                : "Choose all and only valid category names (comma separated). Squares are also rectangles, rhombuses and quadrilaterals.");
        }

        if (q.AnswerType == "place-value-parts")
        {
            if (!int.TryParse(q.Expected, NumberStyles.None, CultureInfo.InvariantCulture,
                out var whole) || whole is < 1 or > 1000)
                return new(null, true, "The target whole needs teacher review.");
            var parts = submitted.Trim().Split(',');
            if (parts.Length != 3)
                return new(false, true, "Enter three whole-number counts in hundreds,tens,ones order.");
            var counts = new int[3];
            for (var i = 0; i < 3; i++)
            {
                var literal = parts[i].Trim();
                if (!Regex.IsMatch(literal, @"^[0-9]{1,4}$", RegexOptions.CultureInvariant) ||
                    !int.TryParse(literal, NumberStyles.None, CultureInfo.InvariantCulture, out counts[i]) ||
                    counts[i] > 1000)
                    return new(false, true, "Use non-negative whole-number counts up to 1,000.");
            }
            // Non-canonical exchanges are intentional: 5,7,2 and 4,17,2
            // can name the same whole. Do not force one preferred grouping.
            var correct = 100 * counts[0] + 10 * counts[1] + counts[2] == whole;
            return new(correct, true, correct
                ? "These hundreds, tens and ones represent the target whole. Explain an exchange and compare another representation with your teacher."
                : "Check the values of hundreds, tens and ones and whether their total equals the target whole.");
        }

        if (q.AnswerType == "linear-length-multiset")
        {
            // Complete list of missing sides: source U07 L09 asks for three.
            var expected = q.Expected?.Split('|', StringSplitOptions.TrimEntries);
            var supplied = submitted.Split(',', StringSplitOptions.TrimEntries);
            if (expected is null || expected.Length is < 2 or > 6 ||
                supplied.Length != expected.Length)
                return new(false, true, "Supply every missing side length and unit, separated by commas.");
            var matched = new bool[expected.Length];
            foreach (var value in supplied)
            {
                if (!TryReadLengthWithUnit(value, out var number, out var denom, out var unit))
                    return new(false, true, "Each missing side needs its source length unit.");
                var index = -1;
                for (var k = 0; k < expected.Length; k++)
                {
                    if (matched[k] || !TryReadLengthWithUnit(expected[k],
                        out var target, out var targetDenom, out var targetUnit) ||
                        unit != targetUnit || number * targetDenom != target * denom)
                        continue;
                    index = k;
                    break;
                }
                if (index < 0)
                    return new(false, true, "Check ALL missing sides and their units; repeated side lengths matter.");
                matched[index] = true;
            }
            return new(true, true,
                "All missing sides match; teacher reviews original shape reasoning.");
        }

        if (q.AnswerType == "length-with-unit")
        {
            if (!TryReadLengthWithUnit(q.Expected, out var expectedLength, out var expectedDenominator, out var expectedUnit))
                return new(null, true, "The expected length needs teacher review.");
            var correct = TryReadLengthWithUnit(submitted, out var actualLength, out var actualDenominator, out var actualUnit) &&
                actualLength * expectedDenominator == expectedLength * actualDenominator && actualUnit == expectedUnit;
            return new(correct, true, correct
                ? "The length and unit match. Your teacher reviews how you used the labelled sides to find it."
                : "Check the missing length and include its linear unit; a square unit describes area.");
        }

        if (q.AnswerType == "rectangle-dimensions")
        {
            var rule = q.DimensionConstraint;
            if (rule is null || rule.TargetArea is < 1 or > 1000000 ||
                rule.MaximumSide is < 1 or > 999)
                return new(null, true, "Rectangle constraints require teacher review.");
            // The source table bounds matter: 2 by 12 has area 24 but
            // cannot be drawn inside its nine-by-nine data-cell region.
            var match = Regex.Match(submitted.Trim(), @"^([0-9]{1,3})\s*,\s*([0-9]{1,3})$",
                RegexOptions.CultureInvariant);
            if (!match.Success)
                return new(false, true, "Enter two positive whole-number side lengths separated by a comma.");
            var a = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
            var b = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
            var correct = a > 0 && b > 0 && a <= rule.MaximumSide &&
                b <= rule.MaximumSide && a * b == rule.TargetArea;
            return new(correct, true, correct
                ? "These sides meet the area and table bounds. Other rectangles may work; show your drawing and reasoning."
                : "Check the product and whether both sides fit inside the stated table.");
        }

        if (q.AnswerType == "coverage-combination")
        {
            var rule = q.CoverageConstraint;
            if (rule is null || rule.ContainerAreas is null || rule.RequiredArea is < 1 or > 1000000 ||
                rule.ContainerAreas.Count is < 1 or > 8 ||
                rule.ContainerAreas.Any(area => area is < 1 or > 1000000))
                return new(null, true, "Coverage constraints are unavailable. Teacher review required.");

            // Counts in the stated container order, not a unique preferred buy.
            // No unstated price, waste or minimum-can optimization is imposed.
            var counts = submitted.Trim().Split(',');
            if (counts.Length != rule.ContainerAreas.Count)
                return new(false, true, "Enter a whole-number count for every container size, separated by commas.");
            long coverage = 0;
            for (var i = 0; i < counts.Length; i++)
            {
                var countText = counts[i].Trim();
                if (!Regex.IsMatch(countText, @"^[0-9]{1,3}$", RegexOptions.CultureInvariant) ||
                    !int.TryParse(countText, NumberStyles.None, CultureInfo.InvariantCulture, out var count))
                    return new(false, true, "Container counts must be non-negative whole numbers from 0 to 999.");
                coverage += (long)count * rule.ContainerAreas[i];
            }
            var correct = coverage >= rule.RequiredArea;
            return new(correct, true, correct
                ? "This purchase covers the required area. Other purchases may also work; explain your choice to your teacher."
                : "This purchase does not cover the entire area. Check the coverage of all chosen containers.");
        }

        if (q.AnswerType == "constrained-side-length")
        {
            var rule = q.RectangleConstraint;
            if (rule is null || rule.FixedSide is < 1 or > 10000 ||
                rule.MinimumArea is < 1 or > 1000000 ||
                rule.MaximumArea < rule.MinimumArea || rule.MaximumArea > 1000000 ||
                rule.LinearUnit != "ft")
                return new(null, true, "Rectangle constraints are unavailable. Teacher review required.");

            // Exact rational comparison includes both endpoints, without
            // rounding 20/3 feet or silently demanding whole-foot lengths.
            if (!TryReadPositiveFeet(submitted, out var numerator, out var denominator))
                return new(false, true, "Enter the other side in feet, using a whole number, decimal or fraction followed by ft.");
            var scaledArea = rule.FixedSide * numerator;
            var correct = scaledArea >= rule.MinimumArea * denominator &&
                          scaledArea <= rule.MaximumArea * denominator;
            return new(correct, true, correct
                ? "This side length meets both area limits. Your teacher reviews the labelled garden and explanation."
                : "Multiply your side length by the fixed side and check both the minimum and maximum area.");
        }

        if (q.AnswerType == "area-quantity-match")
        {
            // IM K–5 G3 U02 L07 first-edition 2021 student PDF p4:
            // four quantities and four everyday objects. Check BOTH the
            // numerical magnitude and the unit, never just the number.
            // The student's rationale stays teacher-reviewed.
            static string? CanonicalQuantity(string? raw)
            {
                if (string.IsNullOrWhiteSpace(raw)) return null;
                var normalized = Regex.Replace(raw.Trim().ToLowerInvariant()
                        .Replace("²", "2"), @"\s+", "")
                    .Replace("^", "")
                    .Replace("square", "")
                    .Replace("sq.", "")
                    .Replace("sq", "")
                    .Replace("inches", "in").Replace("inch", "in")
                    .Replace("feet", "ft").Replace("foot", "ft")
                    .Replace("centimeters", "cm").Replace("centimeter", "cm")
                    .Replace("meters", "m").Replace("meter", "m");
                return normalized is "9in2" or "3ft2" or "55cm2" or "55m2"
                    ? normalized : null;
            }
            var actual = CanonicalQuantity(submitted);
            var required = CanonicalQuantity(q.Expected);
            var correct = actual is not null && required is not null &&
                string.Equals(actual, required, StringComparison.Ordinal);
            return new(correct, true, correct
                ? "The source quantity and square-area unit match; explain the object-size reasoning to your teacher."
                : "Select the complete original area quantity WITH its square unit. A number alone or a different unit cannot pass.");
        }

        if (q.AnswerType == "source-card-letter")
        {
            // Grade 3 Unit 2 Lesson 5 — independently drawn first-party
            // A–G rectangle cards with source-equivalent row/column geometry.
            // Only the CARD label is auto-checked; the student's explanation,
            // physical matching and area-unit reasoning still require a teacher.
            var value = submitted.Trim().ToUpperInvariant();
            var expected = q.Expected?.Trim().ToUpperInvariant();
            var valid = value.Length == 1 && value[0] is >= 'A' and <= 'G' &&
                expected is not null && expected.Length == 1 &&
                expected[0] is >= 'A' and <= 'G';
            var correct = valid && string.Equals(value, expected, StringComparison.Ordinal);
            return new(correct, true, correct
                ? "This letter matches the first-party rectangle model. Explain the equal square rows/columns to your teacher."
                : "Select one card A–G. Compare the source expression with BOTH side lengths and explain.");
        }

        if (q.AnswerType == "yes-no")
        {
            var value = submitted.Trim().ToLowerInvariant();
            var expected = q.Expected?.Trim().ToLowerInvariant();
            var valid = value is "yes" or "no";
            var correct = valid && value == expected;
            return new(correct, true, correct
                ? "Yes/no matches the model. The teacher must still review the explanation."
                : "Check the number of groups and how many are in each group, then explain.");
        }

        if (q.AnswerType == "comparison")
        {
            var ok = submitted.Trim() is ">" or "<" or "=" &&
                string.Equals(submitted.Trim(), q.Expected, StringComparison.Ordinal);
            return new(ok, true, ok ? "The comparison symbol matches this first-party example."
                : "Compare the quantities using the same whole.");
        }

        if (q.AnswerType == "multiplication-expression-either-order")
        {
            // 2021 G3 U01 L18 explicitly permits either factor orientation
            // when the array/model explanation supports the choice.
            // Keep this separate from order-sensitive Grade 3 families.
            const string expression =
                @"^([0-9]{1,3})\s*[xX×*]\s*([0-9]{1,3})(?:\s*=\s*([0-9]{1,5}))?$";
            var actual = Regex.Match(submitted.Trim(), expression,
                RegexOptions.CultureInvariant);
            var expected = Regex.Match(q.Expected ?? "", expression,
                RegexOptions.CultureInvariant);
            if (!actual.Success || !expected.Success)
                return new(false, true,
                    "Write a multiplication expression with the two source factors. A numeric total alone is not enough.");
            var a = int.Parse(actual.Groups[1].Value, CultureInfo.InvariantCulture);
            var b = int.Parse(actual.Groups[2].Value, CultureInfo.InvariantCulture);
            var e = int.Parse(expected.Groups[1].Value, CultureInfo.InvariantCulture);
            var f = int.Parse(expected.Groups[2].Value, CultureInfo.InvariantCulture);
            var validProduct = !actual.Groups[3].Success ||
                int.Parse(actual.Groups[3].Value, CultureInfo.InvariantCulture) == a * b;
            var correct = validProduct && ((a == e && b == f) || (a == f && b == e));
            return new(correct, true, correct
                ? "The factor pair matches either accepted orientation. Teacher reviews how rows/columns represent each factor."
                : "Use the same array factor pair and check any written product. Explain your row/column interpretation.");
        }

        if (q.AnswerType == "multiplication-expression")
        {
            // In Grade 3 this family assesses a written expression, NOT only
            // its product. The first factor identifies the equal-group count,
            // the second the number in each group. Other justified models
            // are always eligible for human consideration, never auto-certified.
            const string syntax = @"^([0-9]{1,3})\s*[xX×*]\s*([0-9]{1,3})(?:\s*=\s*([0-9]{1,5}))?$";
            var actual = Regex.Match(submitted.Trim(), syntax, RegexOptions.CultureInvariant);
            var expected = Regex.Match(q.Expected ?? "", syntax, RegexOptions.CultureInvariant);
            if (!actual.Success || !expected.Success)
                return new(false, true, "Write groups × objects per group; a product alone does not demonstrate the requested expression.");

            var groups = int.Parse(actual.Groups[1].Value, CultureInfo.InvariantCulture);
            var each = int.Parse(actual.Groups[2].Value, CultureInfo.InvariantCulture);
            var requiredGroups = int.Parse(expected.Groups[1].Value, CultureInfo.InvariantCulture);
            var requiredEach = int.Parse(expected.Groups[2].Value, CultureInfo.InvariantCulture);
            if (actual.Groups[3].Success &&
                int.Parse(actual.Groups[3].Value, CultureInfo.InvariantCulture) != groups * each)
                return new(false, true, "The stated total contradicts the factors. Check your expression.");

            var correct = groups == requiredGroups && each == requiredEach;
            return new(correct, true, correct
                ? "The expression matches the number and size of the groups; teacher still reviews the drawing."
                : "Check which number describes the groups and which describes the members in each group. Teacher reviews alternative justifications.");
        }

        if (q.AnswerType == "multiplication-unknown-equation")
        {
            // The source requires an EQUATION with a single missing factor or
            // product. Accept ? / □ / _ and either factor order or equality side.
            // Numeric solutions are checked separately; this cannot replace them.
            static string? Signature(string? raw)
            {
                if (string.IsNullOrWhiteSpace(raw)) return null;
                var s = Regex.Replace(raw, @"\s+", "")
                    .Replace("×", "x").Replace("*", "x").Replace("X", "x")
                    .Replace("□", "?").Replace("▢", "?").Replace("_", "?");
                var sides = s.Split('=');
                if (sides.Length != 2) return null;
                var m = Regex.Match(sides[0], @"^([0-9]{1,3}|\?)x([0-9]{1,3}|\?)$",
                    RegexOptions.CultureInvariant);
                var product = sides[1];
                if (!m.Success)
                {
                    m = Regex.Match(sides[1], @"^([0-9]{1,3}|\?)x([0-9]{1,3}|\?)$",
                        RegexOptions.CultureInvariant);
                    product = sides[0];
                }
                if (!m.Success || !Regex.IsMatch(product, @"^([0-9]{1,5}|\?)$"))
                    return null;
                var a = m.Groups[1].Value;
                var b = m.Groups[2].Value;
                if ((a == "?" ? 1 : 0) + (b == "?" ? 1 : 0) +
                    (product == "?" ? 1 : 0) != 1)
                    return null;
                if (string.CompareOrdinal(a, b) > 0) (a, b) = (b, a);
                return a + "x" + b + "=" + product;
            }
            var required = Signature(q.Expected);
            var actual = Signature(submitted);
            var correct = required is not null && actual == required;
            return new(correct, true, correct
                ? "The unknown is in the correct factor/product role; solve the number separately and explain."
                : "Write an equation with exactly one unknown symbol; keep the given factors and total in the correct roles.");
        }

        if (q.AnswerType == "multiplication-equation-either-order")
        {
            // Scoped to lessons opting into the original 2021 rule:
            // either factor order is valid, but equality and product are mandatory.
            if (!TryReadProductEquation(submitted, out var first, out var second, out var total) ||
                !TryReadProductEquation(q.Expected, out var a, out var b, out var wanted))
                return new(false, true, "Write a complete true multiplication equation; a number alone is insufficient.");
            var correct = total == wanted &&
                ((first == a && second == b) || (first == b && second == a));
            return new(correct, true, correct
                ? "Factors and product agree. The teacher must still review your drawing and explanation."
                : "Recheck the two factors and product against the source groups.");
        }

        if (q.AnswerType == "multiplication-equation")
        {
            if (!TryReadProductEquation(submitted, out var ag, out var ap, out var at) ||
                !TryReadProductEquation(q.Expected, out var eg, out var ep, out var et))
                return new(false, true, "Write a complete true multiplication equation, such as 3 × 5 = 15. A numerical total alone is insufficient.");

            var correct = ag == eg && ap == ep && at == et;
            return new(correct, true, correct
                ? "Both factors and product match the intended source groups. The drawing still needs teacher review."
                : "The product or factor roles differ from the expected model. A teacher can examine mathematically valid alternative representations.");
        }

        if (q.AnswerType == "commutative-equation-pair")
        {
            // Two COMPLETE true equations, one for each orientation. A bare
            // shared product does not demonstrate the commutative relationship.
            // Either ordering of the two equations is allowed.
            var actual = Regex.Split(submitted.Trim(), @"\s*[;,\r\n]+\s*");
            var required = Regex.Split(q.Expected ?? "", @"\s*[;,\r\n]+\s*");
            if (actual.Length != 2 || required.Length != 2 ||
                !TryReadProductEquation(actual[0], out var ag1, out var ae1, out var ap1) ||
                !TryReadProductEquation(actual[1], out var ag2, out var ae2, out var ap2) ||
                !TryReadProductEquation(required[0], out var rg1, out var re1, out var rp1) ||
                !TryReadProductEquation(required[1], out var rg2, out var re2, out var rp2))
                return new(false, true,
                    "Write BOTH multiplication equations, separated by a semicolon. Include each product.");

            var genuineSwap = ag1 == ae2 && ae1 == ag2 && ap1 == ap2;
            var ordered = ag1 == rg1 && ae1 == re1 && ap1 == rp1 &&
                          ag2 == rg2 && ae2 == re2 && ap2 == rp2;
            var reversed = ag1 == rg2 && ae1 == re2 && ap1 == rp2 &&
                           ag2 == rg1 && ae2 == re1 && ap2 == rp1;
            var correct = genuineSwap && (ordered || reversed);
            return new(correct, true, correct
                ? "The two full equations have reversed factors and an unchanged product. Teacher reviews the drawings and explanation."
                : "Check both equations: reversed group factors should keep the same total.");
        }

        if (q.AnswerType == "area-with-unit")
        {
            if (!TryReadAreaWithUnit(submitted, out var actual, out var actualUnit) ||
                !TryReadAreaWithUnit(q.Expected, out var expected, out var requiredUnit))
                return new(false, true,
                    "Include a number AND its square unit, such as 21 cm² or 24 in².");

            var correct = actual == expected &&
                string.Equals(actualUnit, requiredUnit, StringComparison.Ordinal);
            return new(correct, true, correct
                ? "The number and square unit agree with this original Edulytics example. Teacher reviews the tiled drawing."
                : "Check the area quantity AND its squared unit (cm², in², ft² or m²). Different units cannot pass.");
        }

        if (q.AnswerType == "integer")
        {
            if (!int.TryParse(submitted.Trim(), NumberStyles.Integer,
                    CultureInfo.InvariantCulture, out var value) ||
                !int.TryParse(q.Expected, NumberStyles.Integer,
                    CultureInfo.InvariantCulture, out var expected))
                return new(false, true, "Enter a whole number.");

            return new(value == expected, true,
                value == expected ? "The number is correct for this original example."
                    : "Review the context and quantities. Drawing or reasoning still requires a teacher.");
        }

        if (q.AnswerType == "clock-time")
        {
            if (!TryClockTime(submitted, out var actual) ||
                !TryClockTime(q.Expected, out var expected))
                return new(false, true,
                    "Write a valid 12-hour clock time such as 3:51 p.m.; include a.m. or p.m.");

            var correct = actual == expected;
            return new(correct, true,
                correct ? "The complete time is mathematically correct. Clock drawing and strategy still require a teacher."
                        : "Recheck both the hour and minute values; crossing an hour changes the hour hand.");
        }

        if (q.AnswerType == "rational")
        {
            if (!TryFraction(submitted, out var an, out var ad) ||
                !TryFraction(q.Expected, out var en, out var ed))
                return new(false, true, "Write a valid fraction such as 3/4.");
            // Value equivalence alone is not sufficient when the question specifies
            // how to represent the partition (e.g. "in eighths").
            if (q.RequiredDenominator is int denominator && ad != denominator)
                return new(false, true,
                    $"Write the answer in {denominator}ths as requested; an equivalent value with a different denominator does not demonstrate the specified partition.");
            // Long arithmetic avoids overflow with bounded fractions.
            var correct = (long)an * ed == (long)en * ad;
            return new(correct, true, correct
                ? "Equivalent fraction accepted for this original example."
                : "Use equal parts of the same whole, then check numerator and denominator.");
        }

        return new(null, true, "Unknown task type. Teacher review only.");
    }

    // Unit-bearing lengths use bounded exact fractions or finite decimals.
    private static bool TryReadPositiveFeet(string text, out long numerator, out long denominator)
    {
        numerator = 0;
        denominator = 1;
        var match = Regex.Match(text.Trim(),
            @"^([0-9]{1,4}(?:\.[0-9]{1,6}|/[0-9]{1,4}| +[0-9]{1,4}/[0-9]{1,4})?)\s*(?:ft|feet|foot)$",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        if (!match.Success) return false;
        var value = match.Groups[1].Value;
        var decimalPoint = value.IndexOf('.');
        if (decimalPoint >= 0)
        {
            numerator = long.Parse(value.Remove(decimalPoint, 1), CultureInfo.InvariantCulture);
            for (var i = decimalPoint + 1; i < value.Length; i++) denominator *= 10;
        }
        else
        {
            if (!TryFraction(value, out var n, out var d)) return false;
            numerator = n;
            denominator = d;
        }
        return numerator > 0;
    }

    private static bool TryReadLengthWithUnit(
        string? answer, out long numerator, out long denominator, out string unit)
    {
        numerator = 0;
        denominator = 1;
        unit = "";
        if (string.IsNullOrWhiteSpace(answer) || answer.Length > 100) return false;
        var match = Regex.Match(answer.Trim(),
            @"^([0-9]{1,4}(?:\.[0-9]{1,6}|/[0-9]{1,4}| +[0-9]{1,4}/[0-9]{1,4})?)\s*(cm|m|ft|in|inch(?:es)?|centimet(?:er|re)s?|met(?:er|re)s?|f(?:oo|ee)t)$",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        if (!match.Success) return false;
        var value = match.Groups[1].Value;
        var point = value.IndexOf('.');
        if (point >= 0)
        {
            numerator = long.Parse(value.Remove(point, 1), CultureInfo.InvariantCulture);
            for (var i = point + 1; i < value.Length; i++) denominator *= 10;
        }
        else
        {
            if (!TryFraction(value, out var n, out var d)) return false;
            numerator = n;
            denominator = d;
        }
        var literal = match.Groups[2].Value.ToLowerInvariant();
        unit = literal == "cm" || literal.StartsWith("centimet", StringComparison.Ordinal)
            ? "cm" : literal == "m" || literal.StartsWith("met", StringComparison.Ordinal) ? "m"
            : literal == "in" || literal.StartsWith("inch", StringComparison.Ordinal) ? "in" : "ft";
        // Bounded exact cross-products stay below 10^16; no rounding or
        // unit conversion. Equivalent whole, decimal and fraction forms work.
        return numerator > 0 && denominator > 0;
    }

    private static bool TryReadAreaWithUnit(
        string? answer, out int amount, out string squareUnit)
    {
        amount = 0;
        squareUnit = "";
        if (string.IsNullOrWhiteSpace(answer) || answer.Length > 100)
            return false;
        var match = Regex.Match(answer.Trim(),
            @"^([0-9]{1,5})\s*(cm(?:\^?2|²)|in(?:\^?2|²)|m(?:\^?2|²)|ft(?:\^?2|²)|square\s+centimet(?:er|re)s?|square\s+inch(?:es)?|square\s+met(?:er|re)s?|square\s+f(?:oo|ee)t|square\s+units?)$",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        if (!match.Success ||
            !int.TryParse(match.Groups[1].Value, NumberStyles.None,
                CultureInfo.InvariantCulture, out amount))
            return false;
        var literal = Regex.Replace(match.Groups[2].Value.ToLowerInvariant(), @"\s+", "");
        squareUnit = literal.StartsWith("cm", StringComparison.Ordinal) ||
                     literal.StartsWith("squarecentimet", StringComparison.Ordinal)
            ? "cm²"
            : literal.StartsWith("ft", StringComparison.Ordinal) ||
              literal.StartsWith("squarefeet", StringComparison.Ordinal) ||
              literal.StartsWith("squarefoot", StringComparison.Ordinal)
                ? "ft²"
                : literal.StartsWith("m", StringComparison.Ordinal) ||
                  literal.StartsWith("squaremet", StringComparison.Ordinal)
                    ? "m²" : literal.StartsWith("squareunit", StringComparison.Ordinal)
                        ? "square units" : "in²";
        return true;
    }

    // The sides of an equality may be reversed, but a mere scalar total is
    // not a multiplication equation. Group/factor order is part of the
    // task-specific representation; alternatives remain human-reviewable.
    private static bool TryReadProductEquation(
        string? submitted, out int groups, out int each, out int product)
    {
        groups = each = product = 0;
        if (string.IsNullOrWhiteSpace(submitted) || submitted.Length > 100)
            return false;
        const string multiplication = @"([0-9]{1,3})\s*[xX×*]\s*([0-9]{1,3})";
        const string number = @"([0-9]{1,5})";
        var text = submitted.Trim();
        var forward = Regex.Match(text,
            "^" + multiplication + @"\s*=\s*" + number + "$",
            RegexOptions.CultureInvariant);
        var reversed = Regex.Match(text,
            "^" + number + @"\s*=\s*" + multiplication + "$",
            RegexOptions.CultureInvariant);
        if (forward.Success)
        {
            groups = int.Parse(forward.Groups[1].Value, CultureInfo.InvariantCulture);
            each = int.Parse(forward.Groups[2].Value, CultureInfo.InvariantCulture);
            product = int.Parse(forward.Groups[3].Value, CultureInfo.InvariantCulture);
        }
        else if (reversed.Success)
        {
            product = int.Parse(reversed.Groups[1].Value, CultureInfo.InvariantCulture);
            groups = int.Parse(reversed.Groups[2].Value, CultureInfo.InvariantCulture);
            each = int.Parse(reversed.Groups[3].Value, CultureInfo.InvariantCulture);
        }
        else return false;
        return groups > 0 && each > 0 && groups * each == product;
    }

    private static bool TryClockTime(string? text, out int minutesSinceMidnight)
    {
        minutesSinceMidnight = 0;
        if (string.IsNullOrWhiteSpace(text) || text.Length > 24) return false;
        // 12-hour digital notation only. Without context, an analogue dial
        // alone cannot distinguish a.m. from p.m., so the prompt supplies it.
        var match = Regex.Match(text.Trim(),
            @"^(0?[1-9]|1[0-2]):([0-5][0-9])\s*([ap])\.?m\.?$",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        if (!match.Success) return false;
        var hour = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
        var minute = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
        var pm = string.Equals(match.Groups[3].Value, "p", StringComparison.OrdinalIgnoreCase);
        minutesSinceMidnight = (hour % 12 + (pm ? 12 : 0)) * 60 + minute;
        return true;
    }

    private static bool TryFraction(string? text, out int numerator, out int denominator)
    {
        numerator = 0;
        denominator = 0;
        if (text is null || text.Length > 24) return false;
        // A legitimate length such as "1 3/4" is the same number as 7/4.
        // Grade 3 measurement lessons explicitly use mixed inch lengths.
        var parts = text.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 2)
        {
            if (!int.TryParse(parts[0], NumberStyles.None, CultureInfo.InvariantCulture, out var whole) ||
                whole is < 0 or > 10000 ||
                !TryFraction(parts[1], out var fractionalNumerator, out var fractionalDenominator) ||
                fractionalNumerator < 0 || fractionalNumerator >= fractionalDenominator)
                return false;
            var total = (long)whole * fractionalDenominator + fractionalNumerator;
            if (total > 10000) return false;
            numerator = (int)total;
            denominator = fractionalDenominator;
            return true;
        }
        if (parts.Length != 1) return false;
        var bits = parts[0].Split('/');
        if (bits.Length == 1 &&
            int.TryParse(bits[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out var n))
        {
            numerator = n;
            denominator = 1;
            return numerator is >= 0 and <= 10000;
        }
        if (bits.Length != 2 ||
            !int.TryParse(bits[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out numerator) ||
            !int.TryParse(bits[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out denominator))
            return false;

        return numerator is >= 0 and <= 10000 && denominator is > 0 and <= 10000;
    }
}

public sealed record Grade3RoadSignCategory(string Category, int SymbolCount)
{
    public int TotalSigns => SymbolCount * Grade3SourceCompanionCatalogue.SourceGraphSignsPerSymbol;
}

public sealed record Grade3CompanionFeedback(bool? NumericCorrect, bool TeacherReview, string Message);

public sealed class Grade3CompanionBank
{
    public List<Grade3CompanionLesson> Lessons { get; set; } = [];
}

public sealed class Grade3CompanionLesson
{
    public int Ordinal { get; set; }
    public string LessonCode { get; set; } = "";
    public string Title { get; set; } = "";
    public int UnitNumber { get; set; }
    public int LessonNumber { get; set; }
    public string TeacherLearningGoals { get; set; } = "";
    public string StudentFacingGoal { get; set; } = "";
    public List<string> SourceActivityNames { get; set; } = [];
    // Source document evidence is first-party metadata, not an assertion of
    // publisher artwork reproduction or of classroom sign-off.
    public bool OriginalStudentTaskPdfRead { get; set; }
    public bool OriginalTeacherBlacklineMasterPdfRead { get; set; }
    public int SourceCardCount { get; set; }
    // An optional, independently mapped 1:1 set of publicly verifiable
    // source numeric questions. Still separate from teacher-only components.
    public List<Grade3OriginalQuestion> SourceSubtasks { get; set; } = [];
    public string FirstPartyFamily { get; set; } = "";
    public Grade3OriginalQuestion? SourceBasedCheck { get; set; }
    public Grade3SourceCheckProvenance? SourceBasedCheckProvenance { get; set; }
    public bool ReviewedStudentFacingText { get; set; }
    public Grade3OriginalQuestion? IndependentPractice { get; set; }
    public Grade3OriginalQuestion? OriginalEdulyticsCoolDown { get; set; }
    public string OpenTeacherTask { get; set; } = "";
    public string TeacherReviewNote { get; set; } = "";
    public Grade3TeachingGuidance? IndependentTeachingGuidance { get; set; }
    public bool ReadyForStageD { get; set; }
}

public sealed class Grade3TeachingGuidance
{
    public string SourceGoal { get; set; } = "";
    public string SourceLearningGoals { get; set; } = "";
    public List<string> TeachingSteps { get; set; } = [];
    public string Caution { get; set; } = "";
}
public sealed class Grade3SourceCheckProvenance
{
    public string LocalHtml { get; set; } = "";
    public string Verified { get; set; } = "";
    public bool FigurePixelsInspected { get; set; }
}
public sealed class Grade3OriginalQuestion
{
    public string Id { get; set; } = "";
    public string SourceActivity { get; set; } = "";
    public string Prompt { get; set; } = "";
    public string AnswerType { get; set; } = "";
    public string? Expected { get; set; }
    /// <summary>When a representation in named equal parts is demanded, its denominator must match.</summary>
    public int? RequiredDenominator { get; set; }
    public Grade3CoverageConstraint? CoverageConstraint { get; set; }
    public Grade3RectangleConstraint? RectangleConstraint { get; set; }
    public Grade3DimensionConstraint? DimensionConstraint { get; set; }
}

public sealed class Grade3DimensionConstraint
{
    public int TargetArea { get; set; }
    public int MaximumSide { get; set; }
}

public sealed class Grade3CoverageConstraint
{
    public int RequiredArea { get; set; }
    public List<int> ContainerAreas { get; set; } = [];
}

public sealed class Grade3RectangleConstraint
{
    public int FixedSide { get; set; }
    public int MinimumArea { get; set; }
    public int MaximumArea { get; set; }
    public string LinearUnit { get; set; } = "";
}

