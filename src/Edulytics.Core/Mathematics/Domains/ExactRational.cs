using System.Globalization;
using System.Numerics;
using System.Text.Json.Serialization;
using Edulytics.Core.Mathematics.Serialization;

namespace Edulytics.Core.Mathematics.Domains;

/// <summary>
/// Normalized exact rational number. The denominator is always positive and the
/// numerator/denominator are reduced by their greatest common divisor.
/// </summary>
[JsonConverter(typeof(ExactRationalJsonConverter))]
public readonly record struct ExactRational
{
    public ExactRational(BigInteger numerator, BigInteger denominator)
    {
        if (denominator.IsZero)
        {
            throw new DivideByZeroException("A rational denominator cannot be zero.");
        }

        if (denominator.Sign < 0)
        {
            numerator = BigInteger.Negate(numerator);
            denominator = BigInteger.Negate(denominator);
        }

        var gcd = BigInteger.GreatestCommonDivisor(BigInteger.Abs(numerator), denominator);
        Numerator = numerator / gcd;
        Denominator = denominator / gcd;
    }

    [JsonConverter(typeof(BigIntegerJsonConverter))]
    public BigInteger Numerator { get; }

    [JsonConverter(typeof(BigIntegerJsonConverter))]
    public BigInteger Denominator { get; }

    public static ExactRational FromInteger(BigInteger value) => new(value, BigInteger.One);

    public static ExactRational operator +(ExactRational left, ExactRational right) =>
        new(
            left.Numerator * right.Denominator + right.Numerator * left.Denominator,
            left.Denominator * right.Denominator);

    public static ExactRational operator -(ExactRational left, ExactRational right) =>
        new(
            left.Numerator * right.Denominator - right.Numerator * left.Denominator,
            left.Denominator * right.Denominator);

    public static ExactRational operator *(ExactRational left, ExactRational right) =>
        new(
            left.Numerator * right.Numerator,
            left.Denominator * right.Denominator);

    public static ExactRational operator /(ExactRational left, ExactRational right)
    {
        if (right.Numerator.IsZero)
        {
            throw new DivideByZeroException("Cannot divide by zero.");
        }

        return new(
            left.Numerator * right.Denominator,
            left.Denominator * right.Numerator);
    }

    public int CompareTo(ExactRational other) =>
        (Numerator * other.Denominator).CompareTo(other.Numerator * Denominator);

    public override string ToString() =>
        Denominator == BigInteger.One
            ? Numerator.ToString(CultureInfo.InvariantCulture)
            : FormattableString.Invariant($"{Numerator}/{Denominator}");
}
