using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Core.Mathematics.Verification;
using Edulytics.Services.Mathematics.Contracts;

namespace Edulytics.Services.Mathematics.Verification;

public sealed class ExactRectangleAreaVerifier : IMathematicsVerifier
{
    public MathematicsVerificationResult Verify(MathematicsSolveRequest request, MathematicsSolveResult result)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(result);
        if (!VerifierGeometryV2.TryParsePositivePair(request.Problem, "rectangle_area_exact", out var width, out var height, out var error))
        {
            return VerifierGeometryV2.Unsupported(error);
        }
        if (!VerifierGeometryArithmetic.TryMultiply(width, height, out var expected, out error))
        {
            return VerifierGeometryV2.Unsupported(error);
        }
        return VerifierGeometryV2.CompareSolvedScalar(result, expected,
            "independent-exact-rectangle-area",
            "The reported area equals an independent exact multiplication of the original rectangle dimensions.");
    }
}

public sealed class ExactRectanglePerimeterVerifier : IMathematicsVerifier
{
    public MathematicsVerificationResult Verify(MathematicsSolveRequest request, MathematicsSolveResult result)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(result);
        if (!VerifierGeometryV2.TryParsePositivePair(request.Problem, "rectangle_perimeter_exact", out var width, out var height, out var error))
        {
            return VerifierGeometryV2.Unsupported(error);
        }
        if (!VerifierGeometryArithmetic.TryAdd(width, height, out var half, out error)
            || !VerifierGeometryArithmetic.TryMultiply(half, VerifierGeometryV2.Two, out var expected, out error))
        {
            return VerifierGeometryV2.Unsupported(error);
        }
        return VerifierGeometryV2.CompareSolvedScalar(result, expected,
            "independent-exact-rectangle-perimeter",
            "The reported perimeter equals an independent exact 2(width+height) evaluation from the original dimensions.");
    }
}

public sealed class ExactTriangleBaseHeightAreaVerifier : IMathematicsVerifier
{
    public MathematicsVerificationResult Verify(MathematicsSolveRequest request, MathematicsSolveResult result)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(result);
        if (!VerifierGeometryV2.TryParsePositivePair(request.Problem, "triangle_area_base_height_exact", out var @base, out var height, out var error))
        {
            return VerifierGeometryV2.Unsupported(error);
        }
        if (!VerifierGeometryArithmetic.TryMultiply(@base, height, out var doubled, out error)
            || !VerifierGeometryArithmetic.TryDivide(doubled, VerifierGeometryV2.Two, out var expected, out error))
        {
            return VerifierGeometryV2.Unsupported(error);
        }
        return VerifierGeometryV2.CompareSolvedScalar(result, expected,
            "independent-exact-triangle-base-height-area",
            "The reported area equals an independent exact base×height÷2 evaluation from the original dimensions.");
    }
}

public sealed class ExactPythagoreanVerifier : IMathematicsVerifier
{
    public MathematicsVerificationResult Verify(MathematicsSolveRequest request, MathematicsSolveResult result)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(result);
        if (request.Problem is not FunctionCallNode call || call.Arguments.Count != 2)
        {
            return VerifierGeometryV2.Unsupported("Independent Pythagorean verification requires a supported two-argument geometry function.");
        }
        if (!VerifierGeometryV2.TryReadPositiveScalar(call.Arguments[0], out var first, out var error)
            || !VerifierGeometryV2.TryReadPositiveScalar(call.Arguments[1], out var second, out error))
        {
            return VerifierGeometryV2.Unsupported(error);
        }

        ExactRational expected;
        switch (call.FunctionName)
        {
            case "pythagorean_hypotenuse_exact":
                if (!VerifierGeometryArithmetic.TrySquare(first, out var first2, out error)
                    || !VerifierGeometryArithmetic.TrySquare(second, out var second2, out error)
                    || !VerifierGeometryArithmetic.TryAdd(first2, second2, out var sum, out error))
                {
                    return VerifierGeometryV2.Unsupported(error);
                }
                if (!VerifierGeometryArithmetic.TryExactSquareRoot(sum, out expected))
                {
                    return VerifierGeometryV2.Unsupported("Independent verification found no exact rational Pythagorean hypotenuse.");
                }
                break;

            case "pythagorean_leg_exact":
                if (first.CompareTo(second) <= 0)
                {
                    return VerifierGeometryV2.Unsupported("Independent verification requires the hypotenuse to exceed the known leg.");
                }
                if (!VerifierGeometryArithmetic.TrySquare(first, out var h2, out error)
                    || !VerifierGeometryArithmetic.TrySquare(second, out var leg2, out error)
                    || !VerifierGeometryArithmetic.TrySubtract(h2, leg2, out var difference, out error))
                {
                    return VerifierGeometryV2.Unsupported(error);
                }
                if (difference.Numerator.Sign <= 0 || !VerifierGeometryArithmetic.TryExactSquareRoot(difference, out expected))
                {
                    return VerifierGeometryV2.Unsupported("Independent verification found no positive exact rational missing leg.");
                }
                break;

            default:
                return VerifierGeometryV2.Unsupported("Unsupported exact Pythagorean function.");
        }

        return VerifierGeometryV2.CompareSolvedScalar(result, expected,
            "independent-exact-pythagorean-identity",
            "The reported length matches an independent exact reconstruction from the original right-triangle dimensions.");
    }
}

internal static class VerifierGeometryArithmetic
{
    private const int MaxScalarBitLength = 4096;
    private const long MaxIntermediateBitLength = (MaxScalarBitLength * 2L) + 1L;

    public static bool TryAdd(ExactRational left, ExactRational right, out ExactRational result, out string error) =>
        TryAddSubtract(left, right, false, out result, out error);

    public static bool TrySubtract(ExactRational left, ExactRational right, out ExactRational result, out string error) =>
        TryAddSubtract(left, right, true, out result, out error);

    private static bool TryAddSubtract(ExactRational left, ExactRational right, bool subtract, out ExactRational result, out string error)
    {
        result = default;
        error = string.Empty;
        if (!IsValid(left) || !IsValid(right))
        {
            error = "Independent geometry verification received a malformed or oversized scalar.";
            return false;
        }

        var leftTermBits = SaturatingAdd(BitLength(left.Numerator), BitLength(right.Denominator));
        var rightTermBits = SaturatingAdd(BitLength(right.Numerator), BitLength(left.Denominator));
        var numeratorBits = SaturatingAdd(Math.Max(leftTermBits, rightTermBits), 1);
        var denominatorBits = SaturatingAdd(BitLength(left.Denominator), BitLength(right.Denominator));
        if (numeratorBits > MaxIntermediateBitLength || denominatorBits > MaxIntermediateBitLength)
        {
            error = "Independent geometry addition/subtraction exceeds its intermediate arithmetic budget.";
            return false;
        }

        result = subtract ? left - right : left + right;
        if (!IsValid(result))
        {
            error = "Independent geometry result exceeds the 4096-bit scalar budget.";
            return false;
        }
        return true;
    }

    public static bool TryMultiply(ExactRational left, ExactRational right, out ExactRational result, out string error)
    {
        result = default;
        error = string.Empty;
        if (!IsValid(left) || !IsValid(right))
        {
            error = "Independent geometry verification received a malformed or oversized scalar.";
            return false;
        }
        if (SaturatingAdd(BitLength(left.Numerator), BitLength(right.Numerator)) > MaxIntermediateBitLength
            || SaturatingAdd(BitLength(left.Denominator), BitLength(right.Denominator)) > MaxIntermediateBitLength)
        {
            error = "Independent geometry multiplication exceeds its intermediate arithmetic budget.";
            return false;
        }

        result = left * right;
        if (!IsValid(result))
        {
            error = "Independent geometry result exceeds the 4096-bit scalar budget.";
            return false;
        }
        return true;
    }

    public static bool TryDivide(ExactRational left, ExactRational right, out ExactRational result, out string error)
    {
        result = default;
        error = string.Empty;
        if (!IsValid(left) || !IsValid(right) || right.Numerator.IsZero)
        {
            error = "Independent geometry division received a malformed, oversized, or zero divisor.";
            return false;
        }
        if (SaturatingAdd(BitLength(left.Numerator), BitLength(right.Denominator)) > MaxIntermediateBitLength
            || SaturatingAdd(BitLength(left.Denominator), BitLength(right.Numerator)) > MaxIntermediateBitLength)
        {
            error = "Independent geometry division exceeds its intermediate arithmetic budget.";
            return false;
        }

        result = left / right;
        if (!IsValid(result))
        {
            error = "Independent geometry result exceeds the 4096-bit scalar budget.";
            return false;
        }
        return true;
    }

    public static bool TrySquare(ExactRational value, out ExactRational result, out string error) =>
        TryMultiply(value, value, out result, out error);

    public static bool TryExactSquareRoot(ExactRational value, out ExactRational result)
    {
        result = default;
        if (!IsValid(value) || value.Numerator.Sign < 0)
        {
            return false;
        }
        var numeratorRoot = IntegerSquareRoot(value.Numerator);
        var denominatorRoot = IntegerSquareRoot(value.Denominator);
        if (numeratorRoot * numeratorRoot != value.Numerator
            || denominatorRoot * denominatorRoot != value.Denominator)
        {
            return false;
        }
        result = new ExactRational(numeratorRoot, denominatorRoot);
        return IsValid(result);
    }

    private static BigInteger IntegerSquareRoot(BigInteger value)
    {
        if (value <= BigInteger.One)
        {
            return value;
        }
        var bitLength = BigInteger.Abs(value).GetBitLength();
        var x = BigInteger.One << (int)((bitLength + 1) / 2);
        while (true)
        {
            var y = (x + value / x) >> 1;
            if (y >= x)
            {
                return x;
            }
            x = y;
        }
    }

    internal static bool IsValid(ExactRational value) =>
        !value.Denominator.IsZero
        && BitLength(value.Numerator) <= MaxScalarBitLength
        && BitLength(value.Denominator) <= MaxScalarBitLength;

    private static long BitLength(BigInteger value) => value.IsZero ? 0 : BigInteger.Abs(value).GetBitLength();
    private static long SaturatingAdd(long left, long right) => left > long.MaxValue - right ? long.MaxValue : left + right;
}

internal static class VerifierGeometryV2
{
    private static readonly ExactRational Zero = new(BigInteger.Zero, BigInteger.One);
    public static readonly ExactRational Two = new(new BigInteger(2), BigInteger.One);

    public static bool TryParsePositivePair(MathNode problem, string functionName, out ExactRational first, out ExactRational second, out string error)
    {
        first = default;
        second = default;
        error = string.Empty;
        if (problem is not FunctionCallNode call || call.FunctionName != functionName || call.Arguments.Count != 2)
        {
            error = $"Independent verification requires {functionName} with exactly two positive exact scalar arguments.";
            return false;
        }
        return TryReadPositiveScalar(call.Arguments[0], out first, out error)
            && TryReadPositiveScalar(call.Arguments[1], out second, out error);
    }

    public static bool TryReadPositiveScalar(MathNode node, out ExactRational value, out string error)
    {
        value = default;
        error = string.Empty;
        switch (node)
        {
            case IntegerNode integer:
                value = new ExactRational(integer.Value, BigInteger.One);
                break;
            case RationalNode rational when !rational.Value.Denominator.IsZero:
                value = rational.Value;
                break;
            case RationalNode:
                error = "Independent verification rejects a rational with zero denominator.";
                return false;
            default:
                error = "Independent geometry verification requires exact integer or rational scalar dimensions.";
                return false;
        }
        if (!VerifierGeometryArithmetic.IsValid(value))
        {
            error = "Independent geometry verification rejects an oversized exact scalar.";
            return false;
        }
        if (value.CompareTo(Zero) <= 0)
        {
            error = "Independent geometry verification requires strictly positive dimensions.";
            return false;
        }
        return true;
    }

    public static MathematicsVerificationResult CompareSolvedScalar(
        MathematicsSolveResult result,
        ExactRational expected,
        string method,
        string description)
    {
        if (!TryReadSolvedScalar(result, out var reported, out var error))
        {
            return Rejected(error);
        }
        if (reported != expected)
        {
            return Rejected("Reported exact geometry value does not equal the independently recomputed value from the original request.");
        }
        return Verified(method, description);
    }

    private static bool TryReadSolvedScalar(MathematicsSolveResult result, out ExactRational value, out string error)
    {
        value = default;
        if (result.Status != MathematicsSolveStatus.Solved
            || result.ExactResult is null
            || !TryReadScalar(result.ExactResult, out value)
            || result.SolutionSet is not FiniteSolutionSet finite
            || finite.Values.Count != 1
            || !TryReadScalar(finite.Values[0], out var setValue)
            || setValue != value)
        {
            error = "Solved geometry scalar must be represented consistently in ExactResult and a one-value FiniteSolutionSet.";
            return false;
        }
        error = string.Empty;
        return true;
    }

    private static bool TryReadScalar(MathNode node, out ExactRational value)
    {
        switch (node)
        {
            case IntegerNode integer:
                value = new ExactRational(integer.Value, BigInteger.One);
                return true;
            case RationalNode rational when !rational.Value.Denominator.IsZero:
                value = rational.Value;
                return VerifierGeometryArithmetic.IsValid(value);
            default:
                value = default;
                return false;
        }
    }

    private static MathematicsVerificationResult Verified(string method, string description) =>
        new(MathematicsVerificationStatus.Verified, [new MathematicsVerificationEvidence(method, description)], []);

    private static MathematicsVerificationResult Rejected(string diagnostic) =>
        new(MathematicsVerificationStatus.Rejected, [], [diagnostic]);

    public static MathematicsVerificationResult Unsupported(string diagnostic) =>
        new(MathematicsVerificationStatus.Unsupported, [], [diagnostic]);
}
