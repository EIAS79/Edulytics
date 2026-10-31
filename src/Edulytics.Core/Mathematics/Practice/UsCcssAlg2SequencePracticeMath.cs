using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Course-owned arithmetic verifier for the First Edition ALG2 U01 sequence tasks.
/// This is deliberately NOT registered with the shared lesson Practice engine.
/// Explanations, legal-move proofs and open-ended rule choice require human scoring.
/// </summary>
public static class UsCcssAlg2SequencePracticeMath
{
    private const int MaximumResponseLength = 256;

    public static bool GradeHanoiMinimumMoves(int discs, string? response)
    {
        if (discs is < 1 or > 64 || string.IsNullOrWhiteSpace(response) ||
            response.Length > MaximumResponseLength) return false;

        BigInteger expected = (BigInteger.One << discs) - BigInteger.One;
        string compact = Regex.Replace(response, @"\s+", "");
        Match exponent = Regex.Match(compact, @"^2\^(\d{1,2})-1$");
        if (exponent.Success)
            return int.TryParse(exponent.Groups[1].Value, out int power) &&
                   power == discs;
        if (TryParseInteger(compact, out BigInteger number))
            return number == expected;

        // A bounded sum of positive powers-of-two is an accepted equivalent
        // numeric expression, not an arbitrary-code expression evaluator.
        string[] addends = compact.Split('+');
        if (addends.Length is < 2 or > 64) return false;
        BigInteger sum = BigInteger.Zero;
        foreach (string addend in addends)
        {
            if (!TryParseInteger(addend, out BigInteger term) || term < 0)
                return false;
            sum += term;
        }
        return sum == expected;
    }

    public static bool GradeCheckerMinimumMoves(int checkersPerSide, string? response)
    {
        if (checkersPerSide is < 1 or > 7 ||
            string.IsNullOrWhiteSpace(response) ||
            response.Length > MaximumResponseLength) return false;

        BigInteger expected = checkersPerSide * (checkersPerSide + 2);
        if (TryParseInteger(response, out BigInteger submitted))
            return submitted == expected;

        // The L01 source contract accepts the exact equivalent form
        // n*(n+2). Recognize only the bound, numeric instance; do not
        // evaluate arbitrary learner expressions as executable code.
        string compact = Regex.Replace(response, @"\s+", "");
        string n = checkersPerSide.ToString(CultureInfo.InvariantCulture);
        return compact == n + "*(" + n + "+2)" ||
               compact == n + "(" + n + "+2)";
    }

    public static bool GradeGeometricTerm(
        string? firstTerm, string? ratio, int firstIndex, int targetIndex, string? response)
    {
        if (firstIndex is < 0 or > 20 || targetIndex < firstIndex ||
            targetIndex > 20 || !Rational.TryParse(firstTerm, out var initial) ||
            !Rational.TryParse(ratio, out var r) ||
            !Rational.TryParse(response, out var submitted)) return false;

        Rational expected = initial;
        for (int n = firstIndex; n < targetIndex; n++)
            expected = expected.Multiply(r);
        return expected == submitted;
    }

    public static bool GradeArithmeticTerm(
        string? firstTerm, string? difference, int firstIndex, int targetIndex, string? response)
    {
        if (firstIndex is < 0 or > 20 || targetIndex < firstIndex ||
            targetIndex > 20 || !Rational.TryParse(firstTerm, out var initial) ||
            !Rational.TryParse(difference, out var increment) ||
            !Rational.TryParse(response, out var submitted)) return false;
        Rational expected = initial;
        for (int n = firstIndex; n < targetIndex; n++)
            expected = expected.Add(increment);
        return expected == submitted;
    }

    public static bool GradeAffineRecurrenceTerm(
        string? knownTerm, int knownIndex, int targetIndex,
        int multiplier, int offset, string? response)
    {
        if (knownIndex is < 1 or > 20 || targetIndex is < 1 or > 20 ||
            !Rational.TryParse(knownTerm, out var term) ||
            !Rational.TryParse(response, out var submitted)) return false;
        if (targetIndex < knownIndex && multiplier == 0) return false;

        for (int index = knownIndex; index < targetIndex; index++)
            term = term.Multiply(Rational.FromInteger(multiplier))
                       .Add(Rational.FromInteger(offset));
        for (int index = knownIndex; index > targetIndex; index--)
            term = term.Subtract(Rational.FromInteger(offset))
                       .Divide(Rational.FromInteger(multiplier));
        return term == submitted;
    }

    /// <summary>
    /// Credit-card source: month 1 is 1000, 2% monthly growth, no payments.
    /// One explicit rounding policy: retain exact rationals and round only
    /// the displayed month's final balance to nearest cent, halves up.
    /// Teacher must approve this policy before enabling automatic grading.
    /// </summary>
    public static bool GradeUnroundedMonthlyInterestAtDisplay(int month, string? response)
    {
        if (month is < 1 or > 12 || !Rational.TryParse(response, out var submitted))
            return false;
        Rational balance = Rational.FromInteger(1000);
        for (int m = 1; m < month; m++)
            balance = balance.Multiply(Rational.FromInteger(51)
                .Divide(Rational.FromInteger(50)));
        BigInteger scaled = balance.Numerator * 100;
        BigInteger cents = BigInteger.DivRem(scaled, balance.Denominator, out var remainder);
        if (remainder * 2 >= balance.Denominator) cents++;
        return submitted == Rational.FromInteger(cents)
            .Divide(Rational.FromInteger(100));
    }

    public static bool GradeSierpinskiCount(int step, string? response)
    {
        if (step is < 0 or > 12) return false;
        return TryParseInteger(response, out var submitted) &&
               submitted == BigInteger.Pow(3, step);
    }

    public static bool GradeSierpinskiPieceArea(int step, string? response)
    {
        if (step is < 0 or > 12 || !Rational.TryParse(response, out var submitted))
            return false;
        return submitted == Rational.FromInteger(256)
            .Divide(Rational.FromInteger(BigInteger.Pow(4, step)));
    }

    private static bool TryParseInteger(string? input, out BigInteger result)
    {
        result = default;
        return input is { Length: > 0 and <= MaximumResponseLength } &&
               Regex.IsMatch(input, @"^[+-]?\d+$") &&
               BigInteger.TryParse(input, NumberStyles.AllowLeadingSign,
                   CultureInfo.InvariantCulture, out result);
    }

    private readonly record struct Rational(BigInteger Numerator, BigInteger Denominator)
    {
        public static Rational FromInteger(BigInteger value) => new(value, BigInteger.One);

        public static bool TryParse(string? value, out Rational result)
        {
            result = default;
            if (string.IsNullOrWhiteSpace(value) || value.Length > MaximumResponseLength)
                return false;

            string token = value.Trim();
            string[] fraction = token.Split('/');
            if (fraction.Length == 2)
            {
                if (!TryParseInteger(fraction[0], out var numerator) ||
                    !TryParseInteger(fraction[1], out var denominator) ||
                    denominator.IsZero) return false;
                result = Normalize(numerator, denominator);
                return true;
            }
            if (fraction.Length != 1) return false;
            if (TryParseInteger(token, out var integer))
            {
                result = FromInteger(integer);
                return true;
            }
            if (!Regex.IsMatch(token, @"^[+-]?(?:\d+\.\d+|\.\d+|\d+\.)$"))
                return false;
            string sign = token.StartsWith("-", StringComparison.Ordinal) ? "-" : "";
            string unsigned = token.TrimStart('+', '-');
            string[] parts = unsigned.Split('.');
            string digits = (parts[0].Length == 0 ? "0" : parts[0]) + parts[1];
            if (!BigInteger.TryParse(sign + digits, NumberStyles.AllowLeadingSign,
                CultureInfo.InvariantCulture, out var decimalNumerator)) return false;
            result = Normalize(decimalNumerator, BigInteger.Pow(10, parts[1].Length));
            return true;
        }

        public Rational Add(Rational other) =>
            Normalize(Numerator * other.Denominator + other.Numerator * Denominator,
                Denominator * other.Denominator);
        public Rational Subtract(Rational other) =>
            Normalize(Numerator * other.Denominator - other.Numerator * Denominator,
                Denominator * other.Denominator);
        public Rational Multiply(Rational other) =>
            Normalize(Numerator * other.Numerator, Denominator * other.Denominator);
        public Rational Divide(Rational other)
        {
            if (other.Numerator.IsZero) throw new DivideByZeroException();
            return Normalize(Numerator * other.Denominator, Denominator * other.Numerator);
        }

        private static Rational Normalize(BigInteger numerator, BigInteger denominator)
        {
            if (denominator.IsZero) throw new DivideByZeroException();
            if (denominator.Sign < 0) { numerator = -numerator; denominator = -denominator; }
            BigInteger divisor = BigInteger.GreatestCommonDivisor(BigInteger.Abs(numerator), denominator);
            return new(numerator / divisor, denominator / divisor);
        }
    }
}
