using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Services.Mathematics.Contracts;

namespace Edulytics.Services.Mathematics.Solving;

public sealed class ExactRectangleAreaSolver : IMathematicsSolver
{
    public MathematicsSolveResult Solve(MathematicsSolveRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (!ExactGeometryV2.TryParsePositivePair(request.Problem, "rectangle_area_exact", out var width, out var height, out var error, out var resourceLimit))
        {
            return ExactGeometryV2.Failed(request, "geometry-rectangle-exact-v1", error, resourceLimit);
        }

        if (!ExactGeometryArithmetic.TryMultiply(width, height, out var area, out error))
        {
            return ExactGeometryV2.ResourceLimit(request, "geometry-rectangle-exact-v1", error);
        }

        return ExactGeometryV2.Solved(request, area, "exact-rectangle-area", "geometry-rectangle-exact-v1");
    }
}

public sealed class ExactRectanglePerimeterSolver : IMathematicsSolver
{
    public MathematicsSolveResult Solve(MathematicsSolveRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (!ExactGeometryV2.TryParsePositivePair(request.Problem, "rectangle_perimeter_exact", out var width, out var height, out var error, out var resourceLimit))
        {
            return ExactGeometryV2.Failed(request, "geometry-rectangle-exact-v1", error, resourceLimit);
        }

        if (!ExactGeometryArithmetic.TryAdd(width, height, out var halfPerimeter, out error)
            || !ExactGeometryArithmetic.TryMultiply(halfPerimeter, ExactGeometryV2.Two, out var perimeter, out error))
        {
            return ExactGeometryV2.ResourceLimit(request, "geometry-rectangle-exact-v1", error);
        }

        return ExactGeometryV2.Solved(request, perimeter, "exact-rectangle-perimeter", "geometry-rectangle-exact-v1");
    }
}

public sealed class ExactTriangleBaseHeightAreaSolver : IMathematicsSolver
{
    public MathematicsSolveResult Solve(MathematicsSolveRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (!ExactGeometryV2.TryParsePositivePair(request.Problem, "triangle_area_base_height_exact", out var @base, out var height, out var error, out var resourceLimit))
        {
            return ExactGeometryV2.Failed(request, "geometry-triangle-exact-v1", error, resourceLimit);
        }

        if (!ExactGeometryArithmetic.TryMultiply(@base, height, out var doubledArea, out error)
            || !ExactGeometryArithmetic.TryDivide(doubledArea, ExactGeometryV2.Two, out var area, out error))
        {
            return ExactGeometryV2.ResourceLimit(request, "geometry-triangle-exact-v1", error);
        }

        return ExactGeometryV2.Solved(request, area, "exact-triangle-base-height-area", "geometry-triangle-exact-v1");
    }
}

public sealed class ExactPythagoreanSolver : IMathematicsSolver
{
    public MathematicsSolveResult Solve(MathematicsSolveRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (request.Problem is not FunctionCallNode call || call.Arguments.Count != 2)
        {
            return ExactGeometryV2.Unsupported(request, "geometry-pythagorean-exact-v1", "Exact Pythagorean evaluation requires a supported two-argument geometry function.");
        }

        if (!ExactGeometryV2.TryReadPositiveScalar(call.Arguments[0], out var first, out var firstError, out var firstResource)
            || !ExactGeometryV2.TryReadPositiveScalar(call.Arguments[1], out var second, out var secondError, out var secondResource))
        {
            var diagnostic = !string.IsNullOrEmpty(firstError) ? firstError : secondError;
            return ExactGeometryV2.Failed(request, "geometry-pythagorean-exact-v1", diagnostic, firstResource || secondResource);
        }

        return call.FunctionName switch
        {
            "pythagorean_hypotenuse_exact" => SolveHypotenuse(request, first, second),
            "pythagorean_leg_exact" => SolveLeg(request, first, second),
            _ => ExactGeometryV2.Unsupported(request, "geometry-pythagorean-exact-v1", "Unsupported exact Pythagorean function.")
        };
    }

    private static MathematicsSolveResult SolveHypotenuse(MathematicsSolveRequest request, ExactRational a, ExactRational b)
    {
        if (!ExactGeometryArithmetic.TrySquare(a, out var a2, out var error)
            || !ExactGeometryArithmetic.TrySquare(b, out var b2, out error)
            || !ExactGeometryArithmetic.TryAdd(a2, b2, out var sum, out error))
        {
            return ExactGeometryV2.ResourceLimit(request, "geometry-pythagorean-exact-v1", error);
        }

        if (!ExactGeometryArithmetic.TryExactSquareRoot(sum, out var hypotenuse))
        {
            return ExactGeometryV2.Unsupported(request, "geometry-pythagorean-exact-v1", "The Pythagorean result is not an exact rational square root in this shadow slice.");
        }

        return ExactGeometryV2.Solved(request, hypotenuse, "exact-pythagorean-hypotenuse", "geometry-pythagorean-exact-v1");
    }

    private static MathematicsSolveResult SolveLeg(MathematicsSolveRequest request, ExactRational hypotenuse, ExactRational otherLeg)
    {
        if (hypotenuse.CompareTo(otherLeg) <= 0)
        {
            return ExactGeometryV2.Unsupported(request, "geometry-pythagorean-exact-v1", "The hypotenuse must be strictly greater than the known leg.");
        }

        if (!ExactGeometryArithmetic.TrySquare(hypotenuse, out var h2, out var error)
            || !ExactGeometryArithmetic.TrySquare(otherLeg, out var leg2, out error)
            || !ExactGeometryArithmetic.TrySubtract(h2, leg2, out var difference, out error))
        {
            return ExactGeometryV2.ResourceLimit(request, "geometry-pythagorean-exact-v1", error);
        }

        if (difference.Numerator.Sign <= 0 || !ExactGeometryArithmetic.TryExactSquareRoot(difference, out var missingLeg))
        {
            return ExactGeometryV2.Unsupported(request, "geometry-pythagorean-exact-v1", "The missing leg is not a positive exact rational square root in this shadow slice.");
        }

        return ExactGeometryV2.Solved(request, missingLeg, "exact-pythagorean-leg", "geometry-pythagorean-exact-v1");
    }
}

internal static class ExactGeometryArithmetic
{
    private const int MaxScalarBitLength = 4096;
    private const long MaxIntermediateBitLength = (MaxScalarBitLength * 2L) + 1L;

    public static bool TryAdd(ExactRational left, ExactRational right, out ExactRational result, out string error) =>
        TryAddSubtract(left, right, subtract: false, out result, out error);

    public static bool TrySubtract(ExactRational left, ExactRational right, out ExactRational result, out string error) =>
        TryAddSubtract(left, right, subtract: true, out result, out error);

    private static bool TryAddSubtract(ExactRational left, ExactRational right, bool subtract, out ExactRational result, out string error)
    {
        result = default;
        error = string.Empty;
        if (!IsValid(left) || !IsValid(right))
        {
            error = "Exact geometry arithmetic received a malformed or oversized rational scalar.";
            return false;
        }

        var leftTermBits = SaturatingAdd(BitLength(left.Numerator), BitLength(right.Denominator));
        var rightTermBits = SaturatingAdd(BitLength(right.Numerator), BitLength(left.Denominator));
        var numeratorBits = SaturatingAdd(Math.Max(leftTermBits, rightTermBits), 1);
        var denominatorBits = SaturatingAdd(BitLength(left.Denominator), BitLength(right.Denominator));
        if (numeratorBits > MaxIntermediateBitLength || denominatorBits > MaxIntermediateBitLength)
        {
            error = "Exact geometry addition/subtraction exceeds the bounded intermediate arithmetic budget.";
            return false;
        }

        result = subtract ? left - right : left + right;
        if (!IsValid(result))
        {
            error = "Exact geometry result exceeds the supported 4096-bit scalar budget.";
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
            error = "Exact geometry arithmetic received a malformed or oversized rational scalar.";
            return false;
        }

        if (SaturatingAdd(BitLength(left.Numerator), BitLength(right.Numerator)) > MaxIntermediateBitLength
            || SaturatingAdd(BitLength(left.Denominator), BitLength(right.Denominator)) > MaxIntermediateBitLength)
        {
            error = "Exact geometry multiplication exceeds the bounded intermediate arithmetic budget.";
            return false;
        }

        result = left * right;
        if (!IsValid(result))
        {
            error = "Exact geometry result exceeds the supported 4096-bit scalar budget.";
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
            error = "Exact geometry division received a malformed, oversized, or zero divisor.";
            return false;
        }

        if (SaturatingAdd(BitLength(left.Numerator), BitLength(right.Denominator)) > MaxIntermediateBitLength
            || SaturatingAdd(BitLength(left.Denominator), BitLength(right.Numerator)) > MaxIntermediateBitLength)
        {
            error = "Exact geometry division exceeds the bounded intermediate arithmetic budget.";
            return false;
        }

        result = left / right;
        if (!IsValid(result))
        {
            error = "Exact geometry result exceeds the supported 4096-bit scalar budget.";
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

    private static long BitLength(BigInteger value) =>
        value.IsZero ? 0 : BigInteger.Abs(value).GetBitLength();

    private static long SaturatingAdd(long left, long right) =>
        left > long.MaxValue - right ? long.MaxValue : left + right;
}

internal static class ExactGeometryV2
{
    private static readonly ExactRational Zero = new(BigInteger.Zero, BigInteger.One);
    public static readonly ExactRational Two = new(new BigInteger(2), BigInteger.One);

    public static bool TryParsePositivePair(
        MathNode problem,
        string functionName,
        out ExactRational first,
        out ExactRational second,
        out string error,
        out bool resourceLimit)
    {
        first = default;
        second = default;
        error = string.Empty;
        resourceLimit = false;
        if (problem is not FunctionCallNode call || call.FunctionName != functionName || call.Arguments.Count != 2)
        {
            error = $"{functionName} requires exactly two positive exact scalar arguments.";
            return false;
        }

        if (!TryReadPositiveScalar(call.Arguments[0], out first, out error, out resourceLimit))
        {
            return false;
        }
        return TryReadPositiveScalar(call.Arguments[1], out second, out error, out resourceLimit);
    }

    public static bool TryReadPositiveScalar(MathNode node, out ExactRational value, out string error, out bool resourceLimit)
    {
        value = default;
        error = string.Empty;
        resourceLimit = false;
        switch (node)
        {
            case IntegerNode integer:
                value = new ExactRational(integer.Value, BigInteger.One);
                break;
            case RationalNode rational when !rational.Value.Denominator.IsZero:
                value = rational.Value;
                break;
            case RationalNode:
                error = "Geometry scalar contains a zero denominator and is malformed.";
                return false;
            default:
                error = "Geometry dimensions must be exact integer or rational scalars.";
                return false;
        }

        if (!ExactGeometryArithmetic.IsValid(value))
        {
            error = "Geometry scalar exceeds the supported 4096-bit exact arithmetic budget.";
            resourceLimit = true;
            return false;
        }
        if (value.CompareTo(Zero) <= 0)
        {
            error = "Geometry dimensions must be strictly positive.";
            return false;
        }
        return true;
    }

    public static MathNode ToNode(ExactRational value) =>
        value.Denominator == BigInteger.One ? new IntegerNode(value.Numerator) : new RationalNode(value);

    public static MathematicsSolveResult Solved(MathematicsSolveRequest request, ExactRational result, string strategy, string version)
    {
        var node = ToNode(result);
        return new(
            MathematicsSolveStatus.Solved,
            node,
            new FiniteSolutionSet([node]),
            request.Assumptions,
            strategy,
            new MathematicsSolutionTrace([]),
            "edulytics-native-geometry",
            version,
            []);
    }

    public static MathematicsSolveResult Failed(MathematicsSolveRequest request, string version, string diagnostic, bool resourceLimit) =>
        resourceLimit ? ResourceLimit(request, version, diagnostic) : Unsupported(request, version, diagnostic);

    public static MathematicsSolveResult Unsupported(MathematicsSolveRequest request, string version, string diagnostic) =>
        new(MathematicsSolveStatus.Unsupported, null, null, request.Assumptions, null, null, "edulytics-native-geometry", version, [diagnostic]);

    public static MathematicsSolveResult ResourceLimit(MathematicsSolveRequest request, string version, string diagnostic) =>
        new(MathematicsSolveStatus.ResourceLimit, null, null, request.Assumptions, null, null, "edulytics-native-geometry", version, [diagnostic]);
}
