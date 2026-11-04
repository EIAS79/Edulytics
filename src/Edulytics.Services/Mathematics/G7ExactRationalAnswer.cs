using System.Globalization;
using System.Numerics;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Exact arithmetic answer verification for bounded Grade 7 rational
/// questions. Uses integer cross-products, never a rounded decimal quotient.
/// </summary>
public static class G7ExactRationalAnswer
{
    public static bool Matches(
        string? submitted, long expectedNumerator, long expectedDenominator,
        long maxComponent = 100000000)
    {
        if (string.IsNullOrWhiteSpace(submitted) ||
            submitted.Length > 36 || expectedDenominator <= 0)
            return false;

        var text = submitted.Trim();
        var pieces = text.Split('/');
        if (pieces.Length is < 1 or > 2)
            return false;

        if (!TryUnsigned(pieces[0], out var numerator))
        {
            if (pieces.Length == 2 || !decimal.TryParse(text,
                NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture,
                out var finite) || finite < 0 || finite > maxComponent)
                return false;

            // Decimal.GetBits gives the exact scaled integer held by decimal.
            int[] bits = decimal.GetBits(finite);
            numerator = (new BigInteger((uint)bits[2]) << 64) |
                        (new BigInteger((uint)bits[1]) << 32) |
                        (uint)bits[0];
            var scale = (bits[3] >> 16) & 0xFF;
            return numerator * expectedDenominator ==
                   new BigInteger(expectedNumerator) * BigInteger.Pow(10, scale);
        }

        if (numerator > maxComponent)
            return false;
        if (pieces.Length == 1)
            return numerator * expectedDenominator == expectedNumerator;
        if (!TryUnsigned(pieces[1], out var denominator) ||
            denominator <= 0 || denominator > maxComponent)
            return false;
        return numerator * expectedDenominator ==
               new BigInteger(expectedNumerator) * denominator;
    }

    private static bool TryUnsigned(string value, out BigInteger result)
    {
        result = BigInteger.Zero;
        return value.Length > 0 && value.Length <= 36 &&
            BigInteger.TryParse(value, NumberStyles.None,
                CultureInfo.InvariantCulture, out result) && result >= 0;
    }
}
