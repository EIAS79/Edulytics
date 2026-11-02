using System.Globalization;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Narrow, age-appropriate Common Core question families. These are not
/// replacements for a whole curriculum's lesson-level academic certification.
/// Every parameter set has a deterministic independent recomputation.
/// </summary>
internal static partial class SupportingPracticeCompletionEngine
{
    private static Problem UsFirstQuadrantCoordinate(Random r, int scale)
    {
        var x = r.Next(1, Math.Min(35, 7 + scale * 4));
        var y = r.Next(1, Math.Min(35, 7 + scale * 4));
        return P("usccss.geometry.first_quadrant_point",
            $"On a coordinate grid, point P is {x} units right of the origin and {y} units above it. Write its ordered pair (x, y).",
            "The first coordinate is horizontal distance to the right; the second is vertical distance up. Both are nonnegative in the first quadrant.",
            ("x", x), ("y", y));
    }

    private static Problem UsUnitDivideWhole(Random r, int scale)
    {
        var d = r.Next(2, 6 + scale);
        var whole = r.Next(2, 5 + scale);
        return P("usccss.fractions.unit_divide_whole",
            $"Calculate 1/{d} ÷ {whole}. Give the fraction in simplest form.",
            "Divide one unit fraction by a whole number by multiplying its denominator by that whole number. Verify by multiplying the answer by the divisor.",
            ("denominator", d), ("whole", whole));
    }

    private static Problem UsWholeDivideUnit(Random r, int scale)
    {
        var d = r.Next(2, 6 + scale);
        var whole = r.Next(2, 5 + scale);
        return P("usccss.fractions.whole_divide_unit",
            $"How many portions of size 1/{d} are there in {whole} whole units?",
            "Each whole contains the denominator number of unit-fraction portions. Multiply the whole count by that denominator.",
            ("denominator", d), ("whole", whole));
    }

    private static Problem UsExpressionCoefficient(Random r, int scale)
    {
        var a = r.Next(2, 4 + 2 * scale);
        var b = r.Next(1, 6 + scale);
        return P("usccss.algebra.expression_coefficient",
            $"In the expression {a}x + {b}, what is the coefficient of x?",
            "A coefficient is the number multiplied by a variable.",
            ("coefficient", a), ("constant", b));
    }

    private static Problem UsExpressionWords(Random r, int scale)
    {
        var a = r.Next(2, 4 + 2 * scale);
        var b = r.Next(1, 7 + scale);
        return P("usccss.algebra.expression_words",
            $"Write an algebraic expression for the sum of {a} times x and {b}. Write it as ax + b.",
            "A product of a number and x is written ax. The sum adds the constant term.",
            ("coefficient", a), ("constant", b));
    }

    private static Problem UsExpressionPower(Random r, int scale)
    {
        var a = r.Next(2, 3 + scale);
        var x = r.Next(2, 4 + scale);
        var b = r.Next(1, 5 + scale);
        return P("usccss.algebra.expression_power_value",
            $"Evaluate {a}x² + {b} when x = {x}.",
            "Substitute x, evaluate the whole-number exponent before multiplication, and add the constant.",
            ("coefficient", a), ("x", x), ("constant", b));
    }

    private static Problem UsRationalMultiply(Random r, int scale)
    {
        var a = r.Next(1, 4 + scale);
        var b = r.Next(2, 5 + scale);
        var c = r.Next(1, 4 + scale);
        var d = r.Next(2, 5 + scale);
        var signed = r.Next(0, 2) == 0 ? -a : a;
        return P("usccss.number.signed_rational_multiply",
            $"Calculate ({signed}/{b}) × ({c}/{d}). Give the rational number in simplest form.",
            "Multiply signed numerators and denominators, preserve the sign, then cancel common factors.",
            ("n1", signed), ("d1", b), ("n2", c), ("d2", d));
    }

    private static Problem UsRationalDivide(Random r, int scale)
    {
        var a = r.Next(1, 4 + scale);
        var b = r.Next(2, 5 + scale);
        var c = r.Next(1, 4 + scale);
        var d = r.Next(2, 5 + scale);
        var signed = r.Next(0, 2) == 0 ? -c : c;
        return P("usccss.number.signed_rational_divide",
            $"Calculate ({a}/{b}) ÷ ({signed}/{d}). Give the rational number in simplest form.",
            "Divide by multiplying by the reciprocal of the nonzero divisor; preserve the sign and simplify.",
            ("n1", a), ("d1", b), ("n2", signed), ("d2", d));
    }

    private static Problem UsRationalDecimal(Random r, int scale)
    {
        int[] denominators = [2, 4, 5, 10, 20, 25];
        var d = denominators[r.Next(denominators.Length)];
        var numerator = r.Next(1, Math.Min(4 + scale, d));
        return P("usccss.number.rational_terminating_decimal",
            $"Convert the rational number {numerator}/{d} to a terminating decimal.",
            "Find an equivalent fraction with denominator 100, then place the decimal point two digits from the right.",
            ("numerator", numerator), ("denominator", d));
    }

    // Remaining Grade 6/7 subclauses: these families are exact and independently solved.
    private static Problem UsExponentProductValue(Random r, int scale)
    {
        var baseNumber = r.Next(2, 10 + scale * 3);
        var exponent = r.Next(2, 6);
        return P("usccss.algebra.exponent_product_value",
            $"Evaluate {baseNumber}^{exponent} by writing it as {exponent} equal factors and multiplying.",
            "An exponent represents repeated multiplication of the base. Do not multiply base by exponent.",
            ("base", baseNumber), ("exponent", exponent));
    }

    private static Problem UsSignedRateDisplacement(Random r, int scale)
    {
        var speed = r.Next(2, 8 + scale * 3) * (r.Next(2) == 0 ? -1 : 1);
        var time = r.Next(2, 7 + scale * 2);
        return P("usccss.number.signed_rate_displacement",
            $"A signed velocity is {speed} km/h for {time} hours. What is the signed displacement in kilometres?",
            "Signed displacement equals velocity multiplied by elapsed time. Preserve the direction sign.",
            ("velocity", speed), ("time", time));
    }

    private static Problem UsRepeatingDecimalDigit(Random r, int scale)
    {
        var denominator = r.Next(2) == 0 ? 3 : 9;
        var numerator = r.Next(1, denominator);
        var whole = r.Next(0, 8 + scale * 4);
        var improperNumerator = denominator * whole + numerator;
        return P("usccss.number.repeating_decimal_digit",
            $"Using long division, find the digit that repeats in the decimal expansion of {improperNumerator}/{denominator}. Enter the single repeated digit.",
            "Division by 3 or 9 in these cases produces a repeating decimal. Continue the remainder cycle to verify its digit.",
            ("numerator", numerator), ("denominator", denominator), ("whole", whole));
    }
}