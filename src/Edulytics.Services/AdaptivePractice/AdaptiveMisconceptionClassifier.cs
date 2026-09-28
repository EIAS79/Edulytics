using System.Numerics;
using System.Text.RegularExpressions;
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
