using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Services.Mathematics.Contracts;

namespace Edulytics.Services.Mathematics.Solving;

public sealed class ExactIndexPowerSolver : IMathematicsSolver
{
    public MathematicsSolveResult Solve(MathematicsSolveRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (!ExactIndicesExponentialsLogTrigV2.TryParseIndexPower(request.Problem, out var value, out var exponent, out var error))
        {
            return ExactIndicesExponentialsLogTrigV2.Unsupported(request, "edulytics-native-indices", "indices-exact-v1", error);
        }

        if (!ExactIndicesExponentialsLogTrigArithmetic.TryPowSigned(value, exponent, out var result, out error, out var resourceLimit))
        {
            return resourceLimit
                ? ExactIndicesExponentialsLogTrigV2.ResourceLimit(request, "edulytics-native-indices", "indices-exact-v1", error)
                : ExactIndicesExponentialsLogTrigV2.Unsupported(request, "edulytics-native-indices", "indices-exact-v1", error);
        }

        var node = ExactIndicesExponentialsLogTrigV2.ToNode(result);
        return ExactIndicesExponentialsLogTrigV2.Solved(request, node, "exact-integer-index-power", "edulytics-native-indices", "indices-exact-v1");
    }
}

public sealed class ExactExponentialSameBaseSolver : IMathematicsSolver
{
    public MathematicsSolveResult Solve(MathematicsSolveRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (!ExactIndicesExponentialsLogTrigV2.TryParseBaseTarget(
                request.Problem,
                "solve_exponential_same_base",
                out var @base,
                out var target,
                out var error))
        {
            return ExactIndicesExponentialsLogTrigV2.Unsupported(request, "edulytics-native-exponentials", "exponentials-exact-v1", error);
        }

        if (!ExactIndicesExponentialsLogTrigArithmetic.TryFindIntegerExponent(@base, target, out var exponent, out error, out var resourceLimit))
        {
            return resourceLimit
                ? ExactIndicesExponentialsLogTrigV2.ResourceLimit(request, "edulytics-native-exponentials", "exponentials-exact-v1", error)
                : ExactIndicesExponentialsLogTrigV2.Unsupported(request, "edulytics-native-exponentials", "exponentials-exact-v1", error);
        }

        var node = new IntegerNode(exponent);
        return ExactIndicesExponentialsLogTrigV2.Solved(request, node, "same-base-exponential-inversion", "edulytics-native-exponentials", "exponentials-exact-v1");
    }
}

public sealed class ExactLogarithmSolver : IMathematicsSolver
{
    public MathematicsSolveResult Solve(MathematicsSolveRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (!ExactIndicesExponentialsLogTrigV2.TryParseBaseTarget(
                request.Problem,
                "log_exact",
                out var @base,
                out var argument,
                out var error))
        {
            return ExactIndicesExponentialsLogTrigV2.Unsupported(request, "edulytics-native-logarithms", "logarithms-exact-v1", error);
        }

        if (!ExactIndicesExponentialsLogTrigArithmetic.TryFindIntegerExponent(@base, argument, out var exponent, out error, out var resourceLimit))
        {
            return resourceLimit
                ? ExactIndicesExponentialsLogTrigV2.ResourceLimit(request, "edulytics-native-logarithms", "logarithms-exact-v1", error)
                : ExactIndicesExponentialsLogTrigV2.Unsupported(request, "edulytics-native-logarithms", "logarithms-exact-v1", error);
        }

        var node = new IntegerNode(exponent);
        return ExactIndicesExponentialsLogTrigV2.Solved(request, node, "exact-logarithm-as-integer-exponent", "edulytics-native-logarithms", "logarithms-exact-v1");
    }
}

public sealed class ExactSpecialAngleTrigonometrySolver : IMathematicsSolver
{
    public MathematicsSolveResult Solve(MathematicsSolveRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (!ExactIndicesExponentialsLogTrigV2.TryParseTrig(request.Problem, out var functionName, out var degrees, out var error))
        {
            return ExactIndicesExponentialsLogTrigV2.Unsupported(request, "edulytics-native-trigonometry", "trig-special-angle-exact-v1", error);
        }

        if (BigInteger.Abs(degrees).GetBitLength() > 64)
        {
            return ExactIndicesExponentialsLogTrigV2.ResourceLimit(
                request,
                "edulytics-native-trigonometry",
                "trig-special-angle-exact-v1",
                "Angle exceeds the supported exact special-angle normalization budget.");
        }

        if (!ExactSpecialAngleTable.TryGet(functionName!, NormalizeDegrees(degrees), out var exact, out error))
        {
            return ExactIndicesExponentialsLogTrigV2.Unsupported(request, "edulytics-native-trigonometry", "trig-special-angle-exact-v1", error);
        }

        return ExactIndicesExponentialsLogTrigV2.Solved(request, exact!, "exact-special-angle-table", "edulytics-native-trigonometry", "trig-special-angle-exact-v1");
    }

    private static int NormalizeDegrees(BigInteger degrees)
    {
        var normalized = degrees % 360;
        if (normalized.Sign < 0)
        {
            normalized += 360;
        }
        return (int)normalized;
    }
}

internal static class ExactIndicesExponentialsLogTrigArithmetic
{
    private const int MaxExponentMagnitude = 32;
    private const int MaxScalarBitLength = 4096;
    private const long MaxIntermediateBitLength = (MaxScalarBitLength * 2L) + 1L;
    private static readonly ExactRational Zero = new(BigInteger.Zero, BigInteger.One);
    private static readonly ExactRational One = new(BigInteger.One, BigInteger.One);
    private static readonly ExactRational MinusOne = new(BigInteger.MinusOne, BigInteger.One);

    public static bool TryPowSigned(
        ExactRational value,
        int exponent,
        out ExactRational result,
        out string error,
        out bool resourceLimit)
    {
        result = default;
        error = string.Empty;
        resourceLimit = false;

        if (Math.Abs((long)exponent) > MaxExponentMagnitude)
        {
            error = $"Exact index evaluation supports integer exponents from -{MaxExponentMagnitude} through {MaxExponentMagnitude}.";
            return false;
        }
        if (!IsWithinLimit(value))
        {
            error = "Index base exceeds the supported exact scalar bit-length budget or is malformed.";
            resourceLimit = true;
            return false;
        }
        if (exponent == 0)
        {
            if (value == Zero)
            {
                error = "0^0 is not defined by this exact index solver.";
                return false;
            }
            result = One;
            return true;
        }
        if (value == Zero && exponent < 0)
        {
            error = "Zero cannot be raised to a negative integer exponent.";
            return false;
        }
        if (value == One)
        {
            result = One;
            return true;
        }
        if (value == MinusOne)
        {
            result = exponent % 2 == 0 ? One : MinusOne;
            return true;
        }

        var powerBase = exponent < 0
            ? new ExactRational(value.Denominator, value.Numerator)
            : value;
        var count = Math.Abs(exponent);
        var current = One;
        for (var i = 0; i < count; i++)
        {
            var numeratorBits = SaturatingAdd(BitLength(current.Numerator), BitLength(powerBase.Numerator));
            var denominatorBits = SaturatingAdd(BitLength(current.Denominator), BitLength(powerBase.Denominator));
            if (numeratorBits > MaxIntermediateBitLength || denominatorBits > MaxIntermediateBitLength)
            {
                error = "Exact index evaluation exceeds the bounded intermediate arithmetic budget.";
                resourceLimit = true;
                return false;
            }

            current *= powerBase;
            if (!IsWithinLimit(current))
            {
                error = "Exact index result exceeds the supported 4096-bit scalar budget.";
                resourceLimit = true;
                return false;
            }
        }

        result = current;
        return true;
    }

    public static bool TryFindIntegerExponent(
        ExactRational @base,
        ExactRational target,
        out BigInteger exponent,
        out string error,
        out bool resourceLimit)
    {
        exponent = default;
        error = string.Empty;
        resourceLimit = false;

        if (!IsWithinLimit(@base) || !IsWithinLimit(target))
        {
            error = "Exponential/logarithmic operands exceed the supported exact scalar bit-length budget or are malformed.";
            resourceLimit = true;
            return false;
        }
        if (@base.CompareTo(Zero) <= 0 || @base == One)
        {
            error = "Exact exponential/logarithmic inversion requires a positive base other than 1.";
            return false;
        }
        if (target.CompareTo(Zero) <= 0)
        {
            error = "Exact exponential/logarithmic inversion requires a positive target/argument.";
            return false;
        }

        for (var candidate = -MaxExponentMagnitude; candidate <= MaxExponentMagnitude; candidate++)
        {
            if (!TryPowSigned(@base, candidate, out var powered, out _, out _))
            {
                continue;
            }
            if (powered == target)
            {
                exponent = new BigInteger(candidate);
                return true;
            }
        }

        error = $"Target is not an exact integer power of the supplied base within exponent range -{MaxExponentMagnitude}..{MaxExponentMagnitude}.";
        return false;
    }

    private static bool IsWithinLimit(ExactRational value) =>
        !value.Denominator.IsZero
        && BitLength(value.Numerator) <= MaxScalarBitLength
        && BitLength(value.Denominator) <= MaxScalarBitLength;

    private static long BitLength(BigInteger value) =>
        value.IsZero ? 0 : BigInteger.Abs(value).GetBitLength();

    private static long SaturatingAdd(long left, long right) =>
        left > long.MaxValue - right ? long.MaxValue : left + right;
}

internal static class ExactSpecialAngleTable
{
    public static bool TryGet(string functionName, int angle, out MathNode? value, out string error)
    {
        value = functionName switch
        {
            "sin_degrees_exact" => Sin(angle),
            "cos_degrees_exact" => Cos(angle),
            "tan_degrees_exact" => Tan(angle),
            _ => null
        };

        if (value is not null)
        {
            error = string.Empty;
            return true;
        }

        error = functionName == "tan_degrees_exact" && angle is 90 or 270
            ? "Tangent is undefined at odd multiples of 90 degrees."
            : "Angle is outside the supported exact special-angle set (multiples of 30 or 45 degrees).";
        return false;
    }

    private static MathNode? Sin(int angle) => angle switch
    {
        0 or 180 => I(0),
        30 or 150 => Half(),
        45 or 135 => SqrtOver(2, 2),
        60 or 120 => SqrtOver(3, 2),
        90 => I(1),
        210 or 330 => Half(-1),
        225 or 315 => Neg(SqrtOver(2, 2)),
        240 or 300 => Neg(SqrtOver(3, 2)),
        270 => I(-1),
        _ => null
    };

    private static MathNode? Cos(int angle) => angle switch
    {
        0 => I(1),
        30 or 330 => SqrtOver(3, 2),
        45 or 315 => SqrtOver(2, 2),
        60 or 300 => Half(),
        90 or 270 => I(0),
        120 or 240 => Half(-1),
        135 or 225 => Neg(SqrtOver(2, 2)),
        150 or 210 => Neg(SqrtOver(3, 2)),
        180 => I(-1),
        _ => null
    };

    private static MathNode? Tan(int angle) => angle switch
    {
        0 or 180 => I(0),
        30 or 210 => SqrtOver(3, 3),
        45 or 225 => I(1),
        60 or 240 => Sqrt(3),
        120 or 300 => Neg(Sqrt(3)),
        135 or 315 => I(-1),
        150 or 330 => Neg(SqrtOver(3, 3)),
        90 or 270 => null,
        _ => null
    };

    private static IntegerNode I(int value) => new(new BigInteger(value));
    private static MathNode Half(int sign = 1) =>
        new RationalNode(new ExactRational(new BigInteger(sign), new BigInteger(2)));
    private static MathNode Sqrt(int radicand) => new RootNode(I(radicand), 2);
    private static MathNode SqrtOver(int radicand, int denominator) => new DivideNode(Sqrt(radicand), I(denominator));
    private static MathNode Neg(MathNode value) => new NegateNode(value);
}

internal static class ExactIndicesExponentialsLogTrigV2
{
    public static bool TryParseIndexPower(MathNode problem, out ExactRational value, out int exponent, out string error)
    {
        value = default;
        exponent = default;
        if (problem is not FunctionCallNode call
            || call.FunctionName != "index_power_exact"
            || call.Arguments.Count != 2
            || !TryReadScalar(call.Arguments[0], out value)
            || call.Arguments[1] is not IntegerNode exponentNode
            || exponentNode.Value < int.MinValue
            || exponentNode.Value > int.MaxValue)
        {
            error = "Index evaluation requires index_power_exact(exact scalar base, integer exponent).";
            return false;
        }

        exponent = (int)exponentNode.Value;
        error = string.Empty;
        return true;
    }

    public static bool TryParseBaseTarget(
        MathNode problem,
        string expectedFunction,
        out ExactRational @base,
        out ExactRational target,
        out string error)
    {
        @base = default;
        target = default;
        if (problem is not FunctionCallNode call
            || call.FunctionName != expectedFunction
            || call.Arguments.Count != 2
            || !TryReadScalar(call.Arguments[0], out @base)
            || !TryReadScalar(call.Arguments[1], out target))
        {
            error = $"{expectedFunction} requires an exact scalar base and exact scalar target/argument.";
            return false;
        }

        error = string.Empty;
        return true;
    }

    public static bool TryParseTrig(MathNode problem, out string? functionName, out BigInteger degrees, out string error)
    {
        functionName = null;
        degrees = default;
        if (problem is not FunctionCallNode call
            || (call.FunctionName != "sin_degrees_exact"
                && call.FunctionName != "cos_degrees_exact"
                && call.FunctionName != "tan_degrees_exact")
            || call.Arguments.Count != 1
            || call.Arguments[0] is not IntegerNode angle)
        {
            error = "Exact trigonometry requires sin_degrees_exact(angle), cos_degrees_exact(angle), or tan_degrees_exact(angle) with an integer degree angle.";
            return false;
        }

        functionName = call.FunctionName;
        degrees = angle.Value;
        error = string.Empty;
        return true;
    }

    public static bool TryReadScalar(MathNode node, out ExactRational value)
    {
        switch (node)
        {
            case IntegerNode integer:
                value = new ExactRational(integer.Value, BigInteger.One);
                return true;
            case RationalNode rational when !rational.Value.Denominator.IsZero:
                value = rational.Value;
                return true;
            default:
                value = default;
                return false;
        }
    }

    public static MathNode ToNode(ExactRational value) =>
        value.Denominator == BigInteger.One ? new IntegerNode(value.Numerator) : new RationalNode(value);

    public static MathematicsSolveResult Solved(
        MathematicsSolveRequest request,
        MathNode exactResult,
        string strategy,
        string provider,
        string version) =>
        new(
            MathematicsSolveStatus.Solved,
            exactResult,
            new FiniteSolutionSet([exactResult]),
            request.Assumptions,
            strategy,
            new MathematicsSolutionTrace([]),
            provider,
            version,
            []);

    public static MathematicsSolveResult Unsupported(
        MathematicsSolveRequest request,
        string provider,
        string version,
        string diagnostic) =>
        new(MathematicsSolveStatus.Unsupported, null, null, request.Assumptions, null, null, provider, version, [diagnostic]);

    public static MathematicsSolveResult ResourceLimit(
        MathematicsSolveRequest request,
        string provider,
        string version,
        string diagnostic) =>
        new(MathematicsSolveStatus.ResourceLimit, null, null, request.Assumptions, null, null, provider, version, [diagnostic]);
}
