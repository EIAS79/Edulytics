using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Core.Mathematics.Verification;
using Edulytics.Services.Mathematics.Contracts;

namespace Edulytics.Services.Mathematics.Verification;

public sealed class ExactMechanicsModelVerifier : IMathematicsVerifier
{
    public MathematicsVerificationResult Verify(MathematicsSolveRequest request, MathematicsSolveResult result)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(result);
        if (request.Problem is not FunctionCallNode call)
        {
            return MechanicsVerification.Unsupported("Mechanics verification requires the original declared mechanics model.");
        }

        return call.FunctionName switch
        {
            "mechanics_constant_acceleration_velocity_exact" => VerifyVelocity(call, result),
            "mechanics_constant_acceleration_displacement_exact" => VerifyDisplacement(call, result),
            "mechanics_newton_second_law_force_exact" => VerifyNewtonSecond(call, result),
            "mechanics_newton_third_law_reaction_exact" => VerifyNewtonThird(call, result),
            "mechanics_equilibrium_balancing_force_1d_exact" => VerifyEquilibrium(call, result),
            "mechanics_impulse_momentum_exact" => VerifyImpulseMomentum(call, result),
            "mechanics_work_energy_exact" => VerifyWorkEnergy(call, result),
            "mechanics_power_exact" => VerifyPower(call, result),
            _ => MechanicsVerification.Unsupported($"Unsupported Mechanics V2 model: {call.FunctionName}.")
        };
    }

    private static MathematicsVerificationResult VerifyVelocity(FunctionCallNode call, MathematicsSolveResult result)
    {
        if (call.Arguments.Count != 3
            || !MechanicsVerification.TryRead(call.Arguments[0], VerifyDimensions.Velocity, out var u)
            || !MechanicsVerification.TryRead(call.Arguments[1], VerifyDimensions.Acceleration, out var a)
            || !MechanicsVerification.TryRead(call.Arguments[2], VerifyDimensions.Time, out var t)
            || t.Numerator.Sign < 0
            || !VerifyArithmetic.TryMultiply(a, t, out var delta)
            || !VerifyArithmetic.TryAdd(u, delta, out var expected))
        {
            return MechanicsVerification.Unsupported("Independent constant-acceleration velocity recomputation failed closed.");
        }
        return MechanicsVerification.Compare(result, expected, "m_per_s", "independent-constant-acceleration-velocity-recomputation");
    }

    private static MathematicsVerificationResult VerifyDisplacement(FunctionCallNode call, MathematicsSolveResult result)
    {
        if (call.Arguments.Count != 3
            || !MechanicsVerification.TryRead(call.Arguments[0], VerifyDimensions.Velocity, out var u)
            || !MechanicsVerification.TryRead(call.Arguments[1], VerifyDimensions.Acceleration, out var a)
            || !MechanicsVerification.TryRead(call.Arguments[2], VerifyDimensions.Time, out var t)
            || t.Numerator.Sign < 0
            || !VerifyArithmetic.TryMultiply(u, t, out var ut)
            || !VerifyArithmetic.TryMultiply(t, t, out var tt)
            || !VerifyArithmetic.TryMultiply(a, tt, out var att)
            || !VerifyArithmetic.TryDivide(att, MechanicsVerification.Two, out var halfAtt)
            || !VerifyArithmetic.TryAdd(ut, halfAtt, out var expected))
        {
            return MechanicsVerification.Unsupported("Independent constant-acceleration displacement recomputation failed closed.");
        }
        return MechanicsVerification.Compare(result, expected, "m", "independent-constant-acceleration-displacement-recomputation");
    }

    private static MathematicsVerificationResult VerifyNewtonSecond(FunctionCallNode call, MathematicsSolveResult result)
    {
        if (call.Arguments.Count != 2
            || !MechanicsVerification.TryRead(call.Arguments[0], VerifyDimensions.Mass, out var mass)
            || mass.Numerator.Sign <= 0
            || !MechanicsVerification.TryRead(call.Arguments[1], VerifyDimensions.Acceleration, out var acceleration)
            || !VerifyArithmetic.TryMultiply(mass, acceleration, out var expected))
        {
            return MechanicsVerification.Unsupported("Independent Newton second-law recomputation failed closed.");
        }
        return MechanicsVerification.Compare(result, expected, "N", "independent-newton-second-law-recomputation");
    }

    private static MathematicsVerificationResult VerifyNewtonThird(FunctionCallNode call, MathematicsSolveResult result)
    {
        if (call.Arguments.Count != 1
            || !MechanicsVerification.TryRead(call.Arguments[0], VerifyDimensions.Force, out var force))
        {
            return MechanicsVerification.Unsupported("Independent Newton third-law recomputation failed closed.");
        }
        return MechanicsVerification.Compare(result, VerifyArithmetic.Negate(force), "N", "independent-newton-third-law-recomputation");
    }

    private static MathematicsVerificationResult VerifyEquilibrium(FunctionCallNode call, MathematicsSolveResult result)
    {
        if (call.Arguments.Count != 1 || call.Arguments[0] is not VectorNode forces || forces.Components.Count is < 1 or > 8)
        {
            return MechanicsVerification.Unsupported("Independent equilibrium verification requires the original bounded force vector.");
        }
        var sum = MechanicsVerification.Zero;
        foreach (var node in forces.Components)
        {
            if (!MechanicsVerification.TryRead(node, VerifyDimensions.Force, out var force)
                || !VerifyArithmetic.TryAdd(sum, force, out sum))
            {
                return MechanicsVerification.Unsupported("Independent equilibrium recomputation failed closed.");
            }
        }
        return MechanicsVerification.Compare(result, VerifyArithmetic.Negate(sum), "N", "independent-equilibrium-resultant-recomputation");
    }

    private static MathematicsVerificationResult VerifyImpulseMomentum(FunctionCallNode call, MathematicsSolveResult result)
    {
        if (call.Arguments.Count != 2
            || !MechanicsVerification.TryRead(call.Arguments[0], VerifyDimensions.Momentum, out var initial)
            || !MechanicsVerification.TryRead(call.Arguments[1], VerifyDimensions.Momentum, out var impulse)
            || !VerifyArithmetic.TryAdd(initial, impulse, out var expected))
        {
            return MechanicsVerification.Unsupported("Independent impulse-momentum recomputation failed closed.");
        }
        return MechanicsVerification.Compare(result, expected, "kg_m_per_s", "independent-impulse-momentum-recomputation");
    }

    private static MathematicsVerificationResult VerifyWorkEnergy(FunctionCallNode call, MathematicsSolveResult result)
    {
        if (call.Arguments.Count != 2
            || !MechanicsVerification.TryRead(call.Arguments[0], VerifyDimensions.Energy, out var initial)
            || initial.Numerator.Sign < 0
            || !MechanicsVerification.TryRead(call.Arguments[1], VerifyDimensions.Energy, out var work)
            || !VerifyArithmetic.TryAdd(initial, work, out var expected)
            || expected.Numerator.Sign < 0)
        {
            return MechanicsVerification.Unsupported("Independent work-energy recomputation failed closed.");
        }
        return MechanicsVerification.Compare(result, expected, "J", "independent-work-energy-recomputation");
    }

    private static MathematicsVerificationResult VerifyPower(FunctionCallNode call, MathematicsSolveResult result)
    {
        if (call.Arguments.Count != 2
            || !MechanicsVerification.TryRead(call.Arguments[0], VerifyDimensions.Energy, out var work)
            || !MechanicsVerification.TryRead(call.Arguments[1], VerifyDimensions.Time, out var time)
            || time.Numerator.Sign <= 0
            || !VerifyArithmetic.TryDivide(work, time, out var expected))
        {
            return MechanicsVerification.Unsupported("Independent power recomputation failed closed.");
        }
        return MechanicsVerification.Compare(result, expected, "W", "independent-power-recomputation");
    }
}

internal readonly record struct VerifyDimension(int Mass, int Length, int Time);

internal static class VerifyDimensions
{
    public static readonly VerifyDimension Mass = new(1, 0, 0);
    public static readonly VerifyDimension Length = new(0, 1, 0);
    public static readonly VerifyDimension Time = new(0, 0, 1);
    public static readonly VerifyDimension Velocity = new(0, 1, -1);
    public static readonly VerifyDimension Acceleration = new(0, 1, -2);
    public static readonly VerifyDimension Force = new(1, 1, -2);
    public static readonly VerifyDimension Momentum = new(1, 1, -1);
    public static readonly VerifyDimension Energy = new(1, 2, -2);
    public static readonly VerifyDimension Power = new(1, 2, -3);

    private static readonly IReadOnlyDictionary<string, VerifyDimension> Units = new Dictionary<string, VerifyDimension>(StringComparer.Ordinal)
    {
        ["kg"] = Mass,
        ["m"] = Length,
        ["s"] = Time,
        ["m_per_s"] = Velocity,
        ["m_per_s2"] = Acceleration,
        ["N"] = Force,
        ["kg_m_per_s"] = Momentum,
        ["N_s"] = Momentum,
        ["J"] = Energy,
        ["W"] = Power
    };

    public static bool TryGet(string unit, out VerifyDimension dimension) => Units.TryGetValue(unit, out dimension);
}

internal static class MechanicsVerification
{
    public static readonly ExactRational Zero = new(BigInteger.Zero, BigInteger.One);
    public static readonly ExactRational Two = new(new BigInteger(2), BigInteger.One);

    public static bool TryRead(MathNode node, VerifyDimension expectedDimension, out ExactRational value)
    {
        value = default;
        if (node is not FunctionCallNode quantity
            || quantity.FunctionName != "quantity_si"
            || quantity.Arguments.Count != 2
            || quantity.Arguments[1] is not SymbolNode unit
            || !VerifyDimensions.TryGet(unit.Name, out var dimension)
            || dimension != expectedDimension)
        {
            return false;
        }
        switch (quantity.Arguments[0])
        {
            case IntegerNode integer:
                value = new ExactRational(integer.Value, BigInteger.One);
                break;
            case RationalNode rational when !rational.Value.Denominator.IsZero:
                value = rational.Value;
                break;
            default:
                return false;
        }
        return VerifyArithmetic.IsValid(value);
    }

    public static MathematicsVerificationResult Compare(MathematicsSolveResult result, ExactRational expected, string expectedUnit, string method)
    {
        if (result.Status != MathematicsSolveStatus.Solved
            || result.ExactResult is not FunctionCallNode exact
            || result.SolutionSet is not FiniteSolutionSet finite
            || finite.Values.Count != 1
            || finite.Values[0] is not FunctionCallNode setValue
            || !TryReadExactUnit(exact, expectedUnit, out var actual)
            || !TryReadExactUnit(setValue, expectedUnit, out var actualSet)
            || actual != expected
            || actualSet != expected)
        {
            return Rejected("Mechanics output does not equal the independently recomputed value with the required SI dimension.");
        }

        return new MathematicsVerificationResult(
            MathematicsVerificationStatus.Verified,
            [new MathematicsVerificationEvidence(method, "The mechanics result was independently recomputed from the original physical model and SI dimensions using bounded exact-rational arithmetic.")],
            []);
    }

    private static bool TryReadExactUnit(FunctionCallNode quantity, string expectedUnit, out ExactRational value)
    {
        value = default;
        if (quantity.FunctionName != "quantity_si"
            || quantity.Arguments.Count != 2
            || quantity.Arguments[1] is not SymbolNode unit
            || unit.Name != expectedUnit)
        {
            return false;
        }
        switch (quantity.Arguments[0])
        {
            case IntegerNode integer:
                value = new ExactRational(integer.Value, BigInteger.One);
                return VerifyArithmetic.IsValid(value);
            case RationalNode rational when !rational.Value.Denominator.IsZero:
                value = rational.Value;
                return VerifyArithmetic.IsValid(value);
            default:
                return false;
        }
    }

    public static MathematicsVerificationResult Unsupported(string diagnostic) =>
        new(MathematicsVerificationStatus.Unsupported, [], [diagnostic]);

    private static MathematicsVerificationResult Rejected(string diagnostic) =>
        new(MathematicsVerificationStatus.Rejected, [], [diagnostic]);
}

internal static class VerifyArithmetic
{
    private const int MaxBits = 4096;
    private const long MaxIntermediateBits = (MaxBits * 2L) + 1L;

    public static bool TryAdd(ExactRational left, ExactRational right, out ExactRational result)
    {
        result = default;
        if (!IsValid(left) || !IsValid(right)
            || AddBits(Math.Max(AddBits(Bits(left.Numerator), Bits(right.Denominator)), AddBits(Bits(right.Numerator), Bits(left.Denominator))), 1) > MaxIntermediateBits
            || AddBits(Bits(left.Denominator), Bits(right.Denominator)) > MaxIntermediateBits)
        {
            return false;
        }
        result = left + right;
        return IsValid(result);
    }

    public static bool TryMultiply(ExactRational left, ExactRational right, out ExactRational result)
    {
        result = default;
        if (!IsValid(left) || !IsValid(right)
            || AddBits(Bits(left.Numerator), Bits(right.Numerator)) > MaxIntermediateBits
            || AddBits(Bits(left.Denominator), Bits(right.Denominator)) > MaxIntermediateBits)
        {
            return false;
        }
        result = left * right;
        return IsValid(result);
    }

    public static bool TryDivide(ExactRational left, ExactRational right, out ExactRational result)
    {
        result = default;
        if (!IsValid(left) || !IsValid(right) || right.Numerator.IsZero
            || AddBits(Bits(left.Numerator), Bits(right.Denominator)) > MaxIntermediateBits
            || AddBits(Bits(left.Denominator), Bits(right.Numerator)) > MaxIntermediateBits)
        {
            return false;
        }
        result = left / right;
        return IsValid(result);
    }

    public static ExactRational Negate(ExactRational value) => new(BigInteger.Negate(value.Numerator), value.Denominator);
    public static bool IsValid(ExactRational value) => !value.Denominator.IsZero && Bits(value.Numerator) <= MaxBits && Bits(value.Denominator) <= MaxBits;
    private static long Bits(BigInteger value) => value.IsZero ? 0 : BigInteger.Abs(value).GetBitLength();
    private static long AddBits(long left, long right) => left > long.MaxValue - right ? long.MaxValue : left + right;
}
