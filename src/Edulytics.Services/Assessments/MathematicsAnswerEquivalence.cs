using System.Numerics;
using System.Text.RegularExpressions;

namespace Edulytics.Services.Assessments;

/// <summary>
/// Conservative scalar-answer equivalence for automatically scored Mathematics
/// short-answer questions. This deliberately does not evaluate arbitrary
/// expressions; it only normalizes representations that are unambiguously the
/// same scalar value (for example 1/2, 0.5, 0,5 and 50%).
/// </summary>
public static partial class MathematicsAnswerEquivalence
{
    [GeneratedRegex(@"^[A-Za-z][A-Za-z0-9_]*\s*=\s*(.+)$", RegexOptions.CultureInvariant)]
    private static partial Regex AssignmentPattern();

    [GeneratedRegex(@"^([+-]?\d+)\s+(\d+)\s*/\s*(\d+)$", RegexOptions.CultureInvariant)]
    private static partial Regex MixedNumberPattern();

    [GeneratedRegex(@"^([+-]?\d+)\s*/\s*([+-]?\d+)$", RegexOptions.CultureInvariant)]
    private static partial Regex FractionPattern();

    [GeneratedRegex(@"^[+-]?(?:\d+(?:[\.,]\d+)?|[\.,]\d+)$", RegexOptions.CultureInvariant)]
    private static partial Regex DecimalPattern();

    public static bool AreEquivalent(string? actual, string? expected)
    {
        var normalizedActual = Normalize(actual);
        var normalizedExpected = Normalize(expected);

        var actualIsScalar = TryParseScalar(normalizedActual, out var actualValue);
        var expectedIsScalar = TryParseScalar(normalizedExpected, out var expectedValue);

        if (actualIsScalar && expectedIsScalar)
            return actualValue == expectedValue;

        // A single separator followed by groups of three digits is ambiguous
        // across supported locales (for example 1,000 or 1.000). Never treat
        // that form as a decimal. It is accepted only when the other answer
        // resolves to the same unambiguous grouped integer.
        var actualIsGrouped = TryParseGroupedInteger(normalizedActual, out var actualGrouped);
        var expectedIsGrouped = TryParseGroupedInteger(normalizedExpected, out var expectedGrouped);

        if (actualIsGrouped && expectedIsScalar)
            return actualGrouped == expectedValue;
        if (actualIsScalar && expectedIsGrouped)
            return actualValue == expectedGrouped;
        if (actualIsGrouped && expectedIsGrouped)
            return actualGrouped == expectedGrouped;

        return string.Equals(
            CollapseWhitespace(normalizedActual),
            CollapseWhitespace(normalizedExpected),
            StringComparison.OrdinalIgnoreCase);
    }

    private static bool TryParseScalar(string value, out Rational rational)
    {
        rational = default;
        value = PrepareScalarLexeme(value);

        if (value.EndsWith('%'))
        {
            var inner = value[..^1].Trim();
            if (!TryParseScalarWithoutPercent(inner, out var percentage))
                return false;

            rational = Rational.Create(
                percentage.Numerator,
                percentage.Denominator * 100);
            return true;
        }

        return TryParseScalarWithoutPercent(value, out rational);
    }

    private static bool TryParseScalarWithoutPercent(string value, out Rational rational)
    {
        rational = default;

        var mixed = MixedNumberPattern().Match(value);
        if (mixed.Success &&
            BigInteger.TryParse(mixed.Groups[1].Value, out var whole) &&
            BigInteger.TryParse(mixed.Groups[2].Value, out var numerator) &&
            BigInteger.TryParse(mixed.Groups[3].Value, out var denominator) &&
            denominator != BigInteger.Zero)
        {
            var mixedSign = mixed.Groups[1].Value.StartsWith("-", StringComparison.Ordinal) ? -1 : 1;
            var absoluteWhole = BigInteger.Abs(whole);
            var combined = absoluteWhole * denominator + numerator;
            rational = Rational.Create(mixedSign * combined, denominator);
            return true;
        }

        var fraction = FractionPattern().Match(value);
        if (fraction.Success &&
            BigInteger.TryParse(fraction.Groups[1].Value, out var fractionNumerator) &&
            BigInteger.TryParse(fraction.Groups[2].Value, out var fractionDenominator) &&
            fractionDenominator != BigInteger.Zero)
        {
            rational = Rational.Create(fractionNumerator, fractionDenominator);
            return true;
        }

        if (!DecimalPattern().IsMatch(value) || LooksLikeGroupedInteger(value))
            return false;

        var decimalSign = 1;
        if (value[0] is '+' or '-')
        {
            if (value[0] == '-') decimalSign = -1;
            value = value[1..];
        }

        var separatorIndex = value.IndexOfAny(['.', ',']);
        var scale = separatorIndex < 0 ? 0 : value.Length - separatorIndex - 1;
        var digits = separatorIndex < 0
            ? value
            : string.Concat(value.AsSpan(0, separatorIndex), value.AsSpan(separatorIndex + 1));

        if (digits.Length == 0 || !BigInteger.TryParse(digits, out var decimalNumerator))
            return false;

        var decimalDenominator = BigInteger.Pow(10, scale);
        rational = Rational.Create(decimalSign * decimalNumerator, decimalDenominator);
        return true;
    }

    private static bool TryParseGroupedInteger(string value, out Rational rational)
    {
        rational = default;
        value = PrepareScalarLexeme(value);
        if (!LooksLikeGroupedInteger(value))
            return false;

        var sign = 1;
        if (value[0] is '+' or '-')
        {
            if (value[0] == '-') sign = -1;
            value = value[1..];
        }

        var separator = value.Contains(',') ? ',' : '.';
        var digits = value.Replace(separator.ToString(), string.Empty, StringComparison.Ordinal);
        if (!BigInteger.TryParse(digits, out var integer))
            return false;

        rational = Rational.Create(sign * integer, BigInteger.One);
        return true;
    }

    private static bool LooksLikeGroupedInteger(string value)
    {
        value = PrepareScalarLexeme(value);
        if (value.Length == 0 || value.EndsWith('%'))
            return false;

        if (value[0] is '+' or '-')
            value = value[1..];

        var hasComma = value.Contains(',');
        var hasDot = value.Contains('.');
        if (hasComma == hasDot)
            return false;

        var separator = hasComma ? ',' : '.';
        var groups = value.Split(separator);
        if (groups.Length < 2 ||
            groups[0].Length is < 1 or > 3 ||
            groups[0][0] == '0' ||
            groups[0].Any(ch => !char.IsDigit(ch)))
        {
            return false;
        }

        return groups.Skip(1).All(group =>
            group.Length == 3 && group.All(char.IsDigit));
    }

    private static string PrepareScalarLexeme(string value)
    {
        value = TrimOuterParentheses(value);
        var assignment = AssignmentPattern().Match(value);
        if (assignment.Success)
            value = TrimOuterParentheses(assignment.Groups[1].Value.Trim());
        return value;
    }

    private static string Normalize(string? value) =>
        (value ?? string.Empty)
            .Trim()
            .Replace('\u2212', '-')
            .Replace('\u2013', '-')
            .Replace('\u2014', '-');

    private static string CollapseWhitespace(string value) =>
        Regex.Replace(value, @"\s+", " ").Trim();

    private static string TrimOuterParentheses(string value)
    {
        while (value.Length >= 2 && value[0] == '(' && value[^1] == ')' && HasSingleOuterPair(value))
            value = value[1..^1].Trim();

        return value;
    }

    private static bool HasSingleOuterPair(string value)
    {
        var depth = 0;
        for (var index = 0; index < value.Length; index++)
        {
            if (value[index] == '(') depth++;
            else if (value[index] == ')') depth--;

            if (depth == 0 && index < value.Length - 1)
                return false;
            if (depth < 0)
                return false;
        }

        return depth == 0;
    }

    private readonly record struct Rational(BigInteger Numerator, BigInteger Denominator)
    {
        public static Rational Create(BigInteger numerator, BigInteger denominator)
        {
            if (denominator.Sign < 0)
            {
                numerator = -numerator;
                denominator = -denominator;
            }

            var divisor = BigInteger.GreatestCommonDivisor(BigInteger.Abs(numerator), denominator);
            return new Rational(numerator / divisor, denominator / divisor);
        }
    }
}
