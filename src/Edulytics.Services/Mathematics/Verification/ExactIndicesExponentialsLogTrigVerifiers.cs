using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Core.Mathematics.Verification;
using Edulytics.Services.Mathematics.Contracts;

namespace Edulytics.Services.Mathematics.Verification;

public sealed class ExactIndexPowerVerifier : IMathematicsVerifier
{
    public MathematicsVerificationResult Verify(MathematicsSolveRequest request, MathematicsSolveResult result)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(result);

        if (!VerifierIndicesExponentialsLogTrigV2.TryParseIndexPower(request.Problem, out var value, out var exponent, out var error))
        {
            return VerifierIndicesExponentialsLogTrigV2.Unsupported(error);
        }
        if (!VerifierExactPower.TryPowSigned(value, exponent, out var expected, out error))
        {
            return VerifierIndicesExponentialsLogTrigV2.Unsupported(error);
        }
        if (!VerifierIndicesExponentialsLogTrigV2.TryReadSolvedScalar(result, out var reported, out error))
        {
            return VerifierIndicesExponentialsLogTrigV2.Rejected(error);
        }
        if (reported != expected)
        {
            return VerifierIndicesExponentialsLogTrigV2.Rejected("Reported index value does not equal independent exact exponentiation of the original base and exponent.");
        }

        return VerifierIndicesExponentialsLogTrigV2.Verified(
            "independent-exact-index-power",
            "The reported value matches independent exact integer-power evaluation of the original request.");
    }
}

public sealed class ExactExponentialSameBaseVerifier : IMathematicsVerifier
{
    public MathematicsVerificationResult Verify(MathematicsSolveRequest request, MathematicsSolveResult result)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(result);

        if (!VerifierIndicesExponentialsLogTrigV2.TryParseBaseTarget(
                request.Problem,
                "solve_exponential_same_base",
                out var @base,
                out var target,
                out var error))
        {
            return VerifierIndicesExponentialsLogTrigV2.Unsupported(error);
        }
        if (!VerifierIndicesExponentialsLogTrigV2.ValidateExponentialDomain(@base, target, out error))
        {
            return VerifierIndicesExponentialsLogTrigV2.Unsupported(error);
        }
        if (!VerifierIndicesExponentialsLogTrigV2.TryReadSolvedInteger(result, out var exponent, out error))
        {
            return VerifierIndicesExponentialsLogTrigV2.Rejected(error);
        }
        if (exponent < -VerifierExactPower.MaxExponentMagnitude || exponent > VerifierExactPower.MaxExponentMagnitude)
        {
            return VerifierIndicesExponentialsLogTrigV2.Rejected("Reported exponential exponent is outside the independently verified range.");
        }
        if (!VerifierExactPower.TryPowSigned(@base, (int)exponent, out var powered, out error))
        {
            return VerifierIndicesExponentialsLogTrigV2.Unsupported(error);
        }
        if (powered != target)
        {
            return VerifierIndicesExponentialsLogTrigV2.Rejected("Reported exponent does not reproduce the original exponential target exactly.");
        }

        return VerifierIndicesExponentialsLogTrigV2.Verified(
            "independent-same-base-exponential-substitution",
            "Independent exact exponentiation of the original base by the reported integer exponent reproduces the original target.");
    }
}

public sealed class ExactLogarithmVerifier : IMathematicsVerifier
{
    public MathematicsVerificationResult Verify(MathematicsSolveRequest request, MathematicsSolveResult result)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(result);

        if (!VerifierIndicesExponentialsLogTrigV2.TryParseBaseTarget(
                request.Problem,
                "log_exact",
                out var @base,
                out var argument,
                out var error))
        {
            return VerifierIndicesExponentialsLogTrigV2.Unsupported(error);
        }
        if (!VerifierIndicesExponentialsLogTrigV2.ValidateExponentialDomain(@base, argument, out error))
        {
            return VerifierIndicesExponentialsLogTrigV2.Unsupported(error);
        }
        if (!VerifierIndicesExponentialsLogTrigV2.TryReadSolvedInteger(result, out var logarithm, out error))
        {
            return VerifierIndicesExponentialsLogTrigV2.Rejected(error);
        }
        if (logarithm < -VerifierExactPower.MaxExponentMagnitude || logarithm > VerifierExactPower.MaxExponentMagnitude)
        {
            return VerifierIndicesExponentialsLogTrigV2.Rejected("Reported logarithm is outside the independently verified integer range.");
        }
        if (!VerifierExactPower.TryPowSigned(@base, (int)logarithm, out var reconstructed, out error))
        {
            return VerifierIndicesExponentialsLogTrigV2.Unsupported(error);
        }
        if (reconstructed != argument)
        {
            return VerifierIndicesExponentialsLogTrigV2.Rejected("Reported logarithm does not reconstruct the original argument as an exact power of the original base.");
        }

        return VerifierIndicesExponentialsLogTrigV2.Verified(
            "independent-logarithm-inverse-exponentiation",
            "Independent exact exponentiation confirms the reported integer logarithm from the original base and argument.");
    }
}

public sealed class ExactSpecialAngleTrigonometryVerifier : IMathematicsVerifier
{
    public MathematicsVerificationResult Verify(MathematicsSolveRequest request, MathematicsSolveResult result)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(result);

        if (!VerifierIndicesExponentialsLogTrigV2.TryParseTrig(request.Problem, out var functionName, out var degrees, out var error))
        {
            return VerifierIndicesExponentialsLogTrigV2.Unsupported(error);
        }
        if (BigInteger.Abs(degrees).GetBitLength() > 64)
        {
            return VerifierIndicesExponentialsLogTrigV2.Unsupported("Angle exceeds the independent verifier normalization budget.");
        }
        if (!VerifierSpecialAngleTable.TryGet(functionName!, NormalizeDegrees(degrees), out var expected, out error))
        {
            return VerifierIndicesExponentialsLogTrigV2.Unsupported(error);
        }
        if (result.Status != MathematicsSolveStatus.Solved
            || result.ExactResult is null
            || result.ExactResult != expected
            || result.SolutionSet is not FiniteSolutionSet finite
            || finite.Values.Count != 1
            || finite.Values[0] != expected)
        {
            return VerifierIndicesExponentialsLogTrigV2.Rejected("Reported trigonometric value does not equal the independently reconstructed exact special-angle value.");
        }

        return VerifierIndicesExponentialsLogTrigV2.Verified(
            "independent-exact-special-angle-table",
            "The reported trigonometric value matches an independently reconstructed exact special-angle identity.");
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

internal static class VerifierExactPower
{
    public const int MaxExponentMagnitude = 32;
    private const int MaxScalarBitLength = 4096;
    private const long MaxIntermediateBitLength = (MaxScalarBitLength * 2L) + 1L;
    private static readonly ExactRational Zero = new(BigInteger.Zero, BigInteger.One);
    private static readonly ExactRational One = new(BigInteger.One, BigInteger.One);
    private static readonly ExactRational MinusOne = new(BigInteger.MinusOne, BigInteger.One);

    public static bool TryPowSigned(ExactRational value, int exponent, out ExactRational result, out string error)
    {
        result = default;
        error = string.Empty;
        if (Math.Abs((long)exponent) > MaxExponentMagnitude)
        {
            error = "Independent exact exponent verification is outside the supported integer exponent range.";
            return false;
        }
        if (!IsWithinLimit(value))
        {
            error = "Independent exact exponent verification exceeds the scalar bit-length budget.";
            return false;
        }
        if (exponent == 0)
        {
            if (value == Zero)
            {
                error = "Independent verification does not define 0^0.";
                return false;
            }
            result = One;
            return true;
        }
        if (value == Zero && exponent < 0)
        {
            error = "Independent verification cannot invert zero for a negative exponent.";
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
        var current = One;
        for (var i = 0; i < Math.Abs(exponent); i++)
        {
            var numeratorBits = SaturatingAdd(BitLength(current.Numerator), BitLength(powerBase.Numerator));
            var denominatorBits = SaturatingAdd(BitLength(current.Denominator), BitLength(powerBase.Denominator));
            if (numeratorBits > MaxIntermediateBitLength || denominatorBits > MaxIntermediateBitLength)
            {
                error = "Independent exact exponent verification exceeds its intermediate arithmetic budget.";
                return false;
            }
            current *= powerBase;
            if (!IsWithinLimit(current))
            {
                error = "Independent exact exponent verification exceeds the 4096-bit result budget.";
                return false;
            }
        }

        result = current;
        return true;
    }

    private static bool IsWithinLimit(ExactRational value) =>
        BitLength(value.Numerator) <= MaxScalarBitLength
        && BitLength(value.Denominator) <= MaxScalarBitLength;

    private static long BitLength(BigInteger value) =>
        value.IsZero ? 0 : BigInteger.Abs(value).GetBitLength();

    private static long SaturatingAdd(long left, long right) =>
        left > long.MaxValue - right ? long.MaxValue : left + right;
}

internal static class VerifierSpecialAngleTable
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
            ? "Independent verification identifies tangent as undefined at this angle."
            : "Independent verification supports only the declared exact special-angle set.";
        return false;
    }

    private static MathNode? Sin(int angle) => angle switch
    {
        0 or 180 => I(0),
        30 or 150 => Half(),
        45 or 135 => SqrtOver(2, 2),
        60 or 120 => SqrtOver(3, 2),
        90 => I(1),
        210 or 330 => Neg(Half()),
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
        120 or 240 => Neg(Half()),
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
    private static MathNode Half() => new RationalNode(new ExactRational(BigInteger.One, new BigInteger(2)));
    private static MathNode Sqrt(int radicand) => new RootNode(I(radicand), 2);
    private static MathNode SqrtOver(int radicand, int denominator) => new DivideNode(Sqrt(radicand), I(denominator));
    private static MathNode Neg(MathNode value) => new NegateNode(value);
}

internal static class VerifierIndicesExponentialsLogTrigV2
{
    private static readonly ExactRational Zero = new(BigInteger.Zero, BigInteger.One);
    private static readonly ExactRational One = new(BigInteger.One, BigInteger.One);

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
            error = "Verifier requires index_power_exact(exact scalar base, integer exponent).";
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
            error = $"Verifier requires {expectedFunction}(exact base, exact target/argument).";
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
            error = "Verifier requires an exact sine/cosine/tangent special-angle request in integer degrees.";
            return false;
        }

        functionName = call.FunctionName;
        degrees = angle.Value;
        error = string.Empty;
        return true;
    }

    public static bool ValidateExponentialDomain(ExactRational @base, ExactRational target, out string error)
    {
        if (@base.CompareTo(Zero) <= 0 || @base == One)
        {
            error = "Independent verification requires a positive exponential/logarithm base other than 1.";
            return false;
        }
        if (target.CompareTo(Zero) <= 0)
        {
            error = "Independent verification requires a positive exponential target/logarithm argument.";
            return false;
        }

        error = string.Empty;
        return true;
    }

    public static bool TryReadSolvedScalar(MathematicsSolveResult result, out ExactRational value, out string error)
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
            error = "Solved exact scalar must be represented consistently in ExactResult and a one-value FiniteSolutionSet.";
            return false;
        }

        error = string.Empty;
        return true;
    }

    public static bool TryReadSolvedInteger(MathematicsSolveResult result, out BigInteger value, out string error)
    {
        value = default;
        if (result.Status != MathematicsSolveStatus.Solved
            || result.ExactResult is not IntegerNode integer
            || result.SolutionSet is not FiniteSolutionSet finite
            || finite.Values.Count != 1
            || finite.Values[0] is not IntegerNode setInteger
            || setInteger.Value != integer.Value)
        {
            error = "Solved exponent/logarithm must be one exact integer consistently represented in ExactResult and FiniteSolutionSet.";
            return false;
        }

        value = integer.Value;
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
            case RationalNode rational:
                value = rational.Value;
                return true;
            default:
                value = default;
                return false;
        }
    }

    public static MathematicsVerificationResult Verified(string method, string description) =>
        new(MathematicsVerificationStatus.Verified, [new MathematicsVerificationEvidence(method, description)], []);

    public static MathematicsVerificationResult Rejected(string diagnostic) =>
        new(MathematicsVerificationStatus.Rejected, [], [diagnostic]);

    public static MathematicsVerificationResult Unsupported(string diagnostic) =>
        new(MathematicsVerificationStatus.Unsupported, [], [diagnostic]);
}
