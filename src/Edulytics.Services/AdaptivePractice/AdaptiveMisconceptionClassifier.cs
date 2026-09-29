using System.Numerics;
using System.Text.RegularExpressions;
using System.Text.Json;
using Edulytics.Core.Entities;

namespace Edulytics.Services.AdaptivePractice;

public sealed record AdaptiveMisconceptionClassification(
    string MisconceptionId,
    decimal Confidence,
    int ObservationsRequiredToActivate);

/// <summary>
/// Conservative deterministic classifier for current learner-facing exact
/// answers. It never invents a misconception from an arbitrary wrong answer.
/// </summary>
public sealed partial class AdaptiveMisconceptionClassifier
{
    [GeneratedRegex(@"^([+-]?\d+)\s*/\s*([+-]?\d+)$",
        RegexOptions.CultureInvariant)]
    private static partial Regex FractionPattern();

    [GeneratedRegex(@"^[+-]?(?:\d+(?:[\.,]\d+)?|[\.,]\d+)$",
        RegexOptions.CultureInvariant)]
    private static partial Regex DecimalPattern();

    public AdaptiveMisconceptionClassification? Classify(
        AssessmentItem item,
        string submittedAnswer)
    {
        ArgumentNullException.ThrowIfNull(item);

        var actual = Normalize(submittedAnswer);
        var expected = Normalize(item.CorrectAnswer);

        if (string.IsNullOrWhiteSpace(actual) ||
            string.IsNullOrWhiteSpace(expected) ||
            string.Equals(
                actual,
                expected,
                StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        if (TryReverseRelation(expected, out var reversed) &&
            string.Equals(
                actual,
                reversed,
                StringComparison.Ordinal))
        {
            return new(
                "comparison.reversal",
                0.90m,
                2);
        }

        var rounding = TryClassifyWholeNumberRounding(
            item,
            actual,
            expected);
        if (rounding is not null)
            return rounding;

        if (!TryParseRational(expected, out var correct) ||
            !TryParseRational(actual, out var submitted))
        {
            return null;
        }

        if (!correct.Numerator.IsZero &&
            submitted == Rational.Create(
                correct.Denominator,
                correct.Numerator))
        {
            return new(
                "fraction.reciprocal",
                0.95m,
                2);
        }

        if (!correct.Numerator.IsZero &&
            submitted == Rational.Create(
                BigInteger.Negate(correct.Numerator),
                correct.Denominator))
        {
            return new(
                "sign.reversal",
                0.90m,
                2);
        }

        if (submitted == correct + Rational.One ||
            submitted == correct - Rational.One)
        {
            return new(
                "arithmetic.off_by_one",
                0.60m,
                3);
        }

        return null;
    }

    private static AdaptiveMisconceptionClassification?
        TryClassifyWholeNumberRounding(
            AssessmentItem item,
            string actual,
            string expected)
    {
        if (!string.Equals(
                item.GenerationFamily,
                "supporting.number.rounding",
                StringComparison.Ordinal) ||
            !int.TryParse(actual, out var submitted) ||
            !int.TryParse(expected, out var correct) ||
            !TryReadIntegerParameter(
                item.GenerationParametersJson,
                "value",
                out var value) ||
            !TryReadIntegerParameter(
                item.GenerationParametersJson,
                "place",
                out var place) ||
            place <= 0)
        {
            return null;
        }

        var lower = value / place * place;
        var upper = lower + place;

        if (submitted % place != 0)
        {
            return new(
                "rounding.lower_places_not_zeroed",
                0.98m,
                1);
        }

        if (submitted == upper &&
            correct == lower)
        {
            return new(
                "rounding.wrong_direction_up",
                0.99m,
                1);
        }

        if (submitted == lower &&
            correct == upper)
        {
            return new(
                "rounding.wrong_direction_down",
                0.99m,
                1);
        }

        if (Math.Abs(submitted - correct) == place)
        {
            return new(
                "rounding.adjacent_multiple",
                0.92m,
                1);
        }

        return null;
    }

    private static bool TryReadIntegerParameter(
        string? generationParametersJson,
        string name,
        out int value)
    {
        value = 0;

        if (string.IsNullOrWhiteSpace(
                generationParametersJson))
        {
            return false;
        }

        try
        {
            using var document =
                JsonDocument.Parse(
                    generationParametersJson);

            if (!document.RootElement.TryGetProperty(
                    "parameters",
                    out var parameters) ||
                !parameters.TryGetProperty(
                    name,
                    out var element) ||
                element.ValueKind !=
                    JsonValueKind.Number)
            {
                return false;
            }

            return element.TryGetInt32(out value);
        }
        catch (JsonException)
        {
            return false;
        }
    }

    private static bool TryReverseRelation(
        string expected,
        out string reversed)
    {
        reversed = expected switch
        {
            "<" => ">",
            ">" => "<",
            _ => string.Empty
        };

        return reversed.Length > 0;
    }

    private static bool TryParseRational(
        string input,
        out Rational value)
    {
        value = default;
        input = Normalize(input);

        if (input.EndsWith('%'))
        {
            if (!TryParseRational(
                    input[..^1],
                    out var percentage))
            {
                return false;
            }

            value = Rational.Create(
                percentage.Numerator,
                percentage.Denominator * 100);
            return true;
        }

        var fraction = FractionPattern().Match(input);
        if (fraction.Success &&
            BigInteger.TryParse(
                fraction.Groups[1].Value,
                out var numerator) &&
            BigInteger.TryParse(
                fraction.Groups[2].Value,
                out var denominator) &&
            denominator != BigInteger.Zero)
        {
            value = Rational.Create(
                numerator,
                denominator);
            return true;
        }

        if (!DecimalPattern().IsMatch(input))
            return false;

        var sign = 1;
        if (input[0] is '+' or '-')
        {
            if (input[0] == '-')
                sign = -1;
            input = input[1..];
        }

        var separator = input.IndexOfAny(['.', ',']);
        var scale = separator < 0
            ? 0
            : input.Length - separator - 1;
        var digits = separator < 0
            ? input
            : string.Concat(
                input.AsSpan(0, separator),
                input.AsSpan(separator + 1));

        if (!BigInteger.TryParse(
                digits,
                out var decimalNumerator))
        {
            return false;
        }

        value = Rational.Create(
            sign * decimalNumerator,
            BigInteger.Pow(10, scale));
        return true;
    }

    private static string Normalize(string? value) =>
        (value ?? string.Empty)
            .Trim()
            .Replace('\u2212', '-')
            .Replace('\u2013', '-')
            .Replace('\u2014', '-');

    private readonly record struct Rational(
        BigInteger Numerator,
        BigInteger Denominator)
    {
        public static Rational One { get; } =
            new(BigInteger.One, BigInteger.One);

        public static Rational Create(
            BigInteger numerator,
            BigInteger denominator)
        {
            if (denominator.IsZero)
                throw new DivideByZeroException();

            if (denominator.Sign < 0)
            {
                numerator = -numerator;
                denominator = -denominator;
            }

            var divisor =
                BigInteger.GreatestCommonDivisor(
                    BigInteger.Abs(numerator),
                    denominator);

            return new(
                numerator / divisor,
                denominator / divisor);
        }

        public static Rational operator +(
            Rational left,
            Rational right) =>
            Create(
                left.Numerator * right.Denominator +
                right.Numerator * left.Denominator,
                left.Denominator * right.Denominator);

        public static Rational operator -(
            Rational left,
            Rational right) =>
            Create(
                left.Numerator * right.Denominator -
                right.Numerator * left.Denominator,
                left.Denominator * right.Denominator);
    }
}
