using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Services.Mathematics.Contracts;

namespace Edulytics.Services.Mathematics.Solving;

public sealed class ExactMechanicsModelSolver : IMathematicsSolver
{
    public MathematicsSolveResult Solve(MathematicsSolveRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (request.Problem is not FunctionCallNode call)
        {
            return MechanicsModel.Unsupported(request, "Mechanics V2 requires a declared mechanics model function.");
        }

        return call.FunctionName switch
        {
            "mechanics_constant_acceleration_velocity_exact" => SolveVelocity(request, call),
            "mechanics_constant_acceleration_displacement_exact" => SolveDisplacement(request, call),
            "mechanics_newton_second_law_force_exact" => SolveNewtonSecondLaw(request, call),
            "mechanics_newton_third_law_reaction_exact" => SolveNewtonThirdLaw(request, call),
            "mechanics_equilibrium_balancing_force_1d_exact" => SolveEquilibrium(request, call),
            "mechanics_impulse_momentum_exact" => SolveImpulseMomentum(request, call),
            "mechanics_work_energy_exact" => SolveWorkEnergy(request, call),
            "mechanics_power_exact" => SolvePower(request, call),
            _ => MechanicsModel.Unsupported(request, $"Unsupported Mechanics V2 model: {call.FunctionName}.")
        };
    }

    private static MathematicsSolveResult SolveVelocity(MathematicsSolveRequest request, FunctionCallNode call)
    {
        if (call.Arguments.Count != 3)
        {
            return MechanicsModel.Unsupported(request, "Constant-acceleration velocity requires initial velocity, acceleration and elapsed time.");
        }
        if (!MechanicsModel.TryReadQuantity(call.Arguments[0], MechanicsDimensions.Velocity, out var u, out var error, out var resourceLimit)
            || !MechanicsModel.TryReadQuantity(call.Arguments[1], MechanicsDimensions.Acceleration, out var a, out error, out resourceLimit)
            || !MechanicsModel.TryReadQuantity(call.Arguments[2], MechanicsDimensions.Time, out var t, out error, out resourceLimit))
        {
            return MechanicsModel.Failed(request, error, resourceLimit);
        }
        if (t.Numerator.Sign < 0)
        {
            return MechanicsModel.Unsupported(request, "Elapsed time cannot be negative.");
        }
        if (!MechanicsArithmetic.TryMultiply(a, t, out var deltaV)
            || !MechanicsArithmetic.TryAdd(u, deltaV, out var v))
        {
            return MechanicsModel.ResourceLimit(request, "Constant-acceleration velocity computation exceeds the exact arithmetic budget.");
        }
        return MechanicsModel.Solved(request, v, "m_per_s", "constant-acceleration-v-equals-u-plus-at", "mechanics-kinematics-exact-v1");
    }

    private static MathematicsSolveResult SolveDisplacement(MathematicsSolveRequest request, FunctionCallNode call)
    {
        if (call.Arguments.Count != 3)
        {
            return MechanicsModel.Unsupported(request, "Constant-acceleration displacement requires initial velocity, acceleration and elapsed time.");
        }
        if (!MechanicsModel.TryReadQuantity(call.Arguments[0], MechanicsDimensions.Velocity, out var u, out var error, out var resourceLimit)
            || !MechanicsModel.TryReadQuantity(call.Arguments[1], MechanicsDimensions.Acceleration, out var a, out error, out resourceLimit)
            || !MechanicsModel.TryReadQuantity(call.Arguments[2], MechanicsDimensions.Time, out var t, out error, out resourceLimit))
        {
            return MechanicsModel.Failed(request, error, resourceLimit);
        }
        if (t.Numerator.Sign < 0)
        {
            return MechanicsModel.Unsupported(request, "Elapsed time cannot be negative.");
        }
        if (!MechanicsArithmetic.TryMultiply(u, t, out var ut)
            || !MechanicsArithmetic.TryMultiply(t, t, out var tSquared)
            || !MechanicsArithmetic.TryMultiply(a, tSquared, out var atSquared)
            || !MechanicsArithmetic.TryDivide(atSquared, MechanicsModel.Two, out var halfAtSquared)
            || !MechanicsArithmetic.TryAdd(ut, halfAtSquared, out var displacement))
        {
            return MechanicsModel.ResourceLimit(request, "Constant-acceleration displacement computation exceeds the exact arithmetic budget.");
        }
        return MechanicsModel.Solved(request, displacement, "m", "constant-acceleration-s-equals-ut-plus-half-at-squared", "mechanics-kinematics-exact-v1");
    }

    private static MathematicsSolveResult SolveNewtonSecondLaw(MathematicsSolveRequest request, FunctionCallNode call)
    {
        if (call.Arguments.Count != 2)
        {
            return MechanicsModel.Unsupported(request, "Newton's second law requires mass and acceleration.");
        }
        if (!MechanicsModel.TryReadQuantity(call.Arguments[0], MechanicsDimensions.Mass, out var mass, out var error, out var resourceLimit)
            || !MechanicsModel.TryReadQuantity(call.Arguments[1], MechanicsDimensions.Acceleration, out var acceleration, out error, out resourceLimit))
        {
            return MechanicsModel.Failed(request, error, resourceLimit);
        }
        if (mass.Numerator.Sign <= 0)
        {
            return MechanicsModel.Unsupported(request, "Newton's second-law model requires positive mass.");
        }
        if (!MechanicsArithmetic.TryMultiply(mass, acceleration, out var force))
        {
            return MechanicsModel.ResourceLimit(request, "Newton's second-law computation exceeds the exact arithmetic budget.");
        }
        return MechanicsModel.Solved(request, force, "N", "newton-second-law-f-equals-ma", "mechanics-newton-exact-v1");
    }

    private static MathematicsSolveResult SolveNewtonThirdLaw(MathematicsSolveRequest request, FunctionCallNode call)
    {
        if (call.Arguments.Count != 1)
        {
            return MechanicsModel.Unsupported(request, "Newton's third law requires one declared action force.");
        }
        if (!MechanicsModel.TryReadQuantity(call.Arguments[0], MechanicsDimensions.Force, out var force, out var error, out var resourceLimit))
        {
            return MechanicsModel.Failed(request, error, resourceLimit);
        }
        return MechanicsModel.Solved(request, MechanicsArithmetic.Negate(force), "N", "newton-third-law-equal-opposite-reaction", "mechanics-newton-exact-v1");
    }

    private static MathematicsSolveResult SolveEquilibrium(MathematicsSolveRequest request, FunctionCallNode call)
    {
        if (call.Arguments.Count != 1 || call.Arguments[0] is not VectorNode forces || forces.Components.Count is < 1 or > 8)
        {
            return MechanicsModel.Unsupported(request, "One-dimensional equilibrium requires a vector containing 1..8 declared force quantities.");
        }
        var sum = MechanicsModel.Zero;
        foreach (var node in forces.Components)
        {
            if (!MechanicsModel.TryReadQuantity(node, MechanicsDimensions.Force, out var force, out var error, out var resourceLimit))
            {
                return MechanicsModel.Failed(request, error, resourceLimit);
            }
            if (!MechanicsArithmetic.TryAdd(sum, force, out sum))
            {
                return MechanicsModel.ResourceLimit(request, "Equilibrium force accumulation exceeds the exact arithmetic budget.");
            }
        }
        return MechanicsModel.Solved(request, MechanicsArithmetic.Negate(sum), "N", "equilibrium-resultant-zero-balancing-force", "mechanics-equilibrium-exact-v1");
    }

    private static MathematicsSolveResult SolveImpulseMomentum(MathematicsSolveRequest request, FunctionCallNode call)
    {
        if (call.Arguments.Count != 2)
        {
            return MechanicsModel.Unsupported(request, "Impulse-momentum requires initial momentum and impulse.");
        }
        if (!MechanicsModel.TryReadQuantity(call.Arguments[0], MechanicsDimensions.Momentum, out var initialMomentum, out var error, out var resourceLimit)
            || !MechanicsModel.TryReadQuantity(call.Arguments[1], MechanicsDimensions.Momentum, out var impulse, out error, out resourceLimit))
        {
            return MechanicsModel.Failed(request, error, resourceLimit);
        }
        if (!MechanicsArithmetic.TryAdd(initialMomentum, impulse, out var finalMomentum))
        {
            return MechanicsModel.ResourceLimit(request, "Impulse-momentum computation exceeds the exact arithmetic budget.");
        }
        return MechanicsModel.Solved(request, finalMomentum, "kg_m_per_s", "impulse-equals-change-in-momentum", "mechanics-momentum-exact-v1");
    }

    private static MathematicsSolveResult SolveWorkEnergy(MathematicsSolveRequest request, FunctionCallNode call)
    {
        if (call.Arguments.Count != 2)
        {
            return MechanicsModel.Unsupported(request, "Work-energy requires initial kinetic energy and net work.");
        }
        if (!MechanicsModel.TryReadQuantity(call.Arguments[0], MechanicsDimensions.Energy, out var initialEnergy, out var error, out var resourceLimit)
            || !MechanicsModel.TryReadQuantity(call.Arguments[1], MechanicsDimensions.Energy, out var work, out error, out resourceLimit))
        {
            return MechanicsModel.Failed(request, error, resourceLimit);
        }
        if (initialEnergy.Numerator.Sign < 0)
        {
            return MechanicsModel.Unsupported(request, "Initial kinetic energy cannot be negative.");
        }
        if (!MechanicsArithmetic.TryAdd(initialEnergy, work, out var finalEnergy))
        {
            return MechanicsModel.ResourceLimit(request, "Work-energy computation exceeds the exact arithmetic budget.");
        }
        if (finalEnergy.Numerator.Sign < 0)
        {
            return MechanicsModel.Unsupported(request, "The declared work-energy model would produce negative kinetic energy.");
        }
        return MechanicsModel.Solved(request, finalEnergy, "J", "work-energy-delta-k-equals-work", "mechanics-energy-exact-v1");
    }

    private static MathematicsSolveResult SolvePower(MathematicsSolveRequest request, FunctionCallNode call)
    {
        if (call.Arguments.Count != 2)
        {
            return MechanicsModel.Unsupported(request, "Average power requires work and positive elapsed time.");
        }
        if (!MechanicsModel.TryReadQuantity(call.Arguments[0], MechanicsDimensions.Energy, out var work, out var error, out var resourceLimit)
            || !MechanicsModel.TryReadQuantity(call.Arguments[1], MechanicsDimensions.Time, out var time, out error, out resourceLimit))
        {
            return MechanicsModel.Failed(request, error, resourceLimit);
        }
        if (time.Numerator.Sign <= 0)
        {
            return MechanicsModel.Unsupported(request, "Power model requires positive elapsed time.");
        }
        if (!MechanicsArithmetic.TryDivide(work, time, out var power))
        {
            return MechanicsModel.ResourceLimit(request, "Power computation exceeds the exact arithmetic budget.");
        }
        return MechanicsModel.Solved(request, power, "W", "power-equals-work-over-time", "mechanics-power-exact-v1");
    }
}

internal readonly record struct MechanicsDimension(int Mass, int Length, int Time);

internal static class MechanicsDimensions
{
    public static readonly MechanicsDimension Mass = new(1, 0, 0);
    public static readonly MechanicsDimension Length = new(0, 1, 0);
    public static readonly MechanicsDimension Time = new(0, 0, 1);
    public static readonly MechanicsDimension Velocity = new(0, 1, -1);
    public static readonly MechanicsDimension Acceleration = new(0, 1, -2);
    public static readonly MechanicsDimension Force = new(1, 1, -2);
    public static readonly MechanicsDimension Momentum = new(1, 1, -1);
    public static readonly MechanicsDimension Energy = new(1, 2, -2);
    public static readonly MechanicsDimension Power = new(1, 2, -3);

    private static readonly IReadOnlyDictionary<string, MechanicsDimension> Units =
        new Dictionary<string, MechanicsDimension>(StringComparer.Ordinal)
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

    public static bool TryGet(string unit, out MechanicsDimension dimension) => Units.TryGetValue(unit, out dimension);
}

internal static class MechanicsModel
{
    public static readonly ExactRational Zero = new(BigInteger.Zero, BigInteger.One);
    public static readonly ExactRational Two = new(new BigInteger(2), BigInteger.One);

    public static bool TryReadQuantity(MathNode node, MechanicsDimension expectedDimension, out ExactRational value, out string error, out bool resourceLimit)
    {
        value = default;
        resourceLimit = false;
        if (node is not FunctionCallNode quantity
            || quantity.FunctionName != "quantity_si"
            || quantity.Arguments.Count != 2
            || quantity.Arguments[1] is not SymbolNode unit
            || !MechanicsDimensions.TryGet(unit.Name, out var actualDimension))
        {
            error = "Mechanics quantities must use quantity_si(exactValue, canonicalUnit).";
            return false;
        }
        if (actualDimension != expectedDimension)
        {
            error = $"Mechanics dimensional analysis rejected unit {unit.Name} for the required model quantity.";
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
            case RationalNode:
                error = "Mechanics quantity contains a malformed rational with zero denominator.";
                return false;
            default:
                error = "Mechanics quantities require exact integer or rational scalar values.";
                return false;
        }
        if (!MechanicsArithmetic.IsValid(value))
        {
            error = "Mechanics scalar exceeds the supported 4096-bit exact arithmetic budget.";
            resourceLimit = true;
            return false;
        }
        error = string.Empty;
        return true;
    }

    public static FunctionCallNode Quantity(ExactRational value, string unit) =>
        new("quantity_si", [ToNode(value), new SymbolNode(unit)]);

    public static MathNode ToNode(ExactRational value) =>
        value.Denominator == BigInteger.One ? new IntegerNode(value.Numerator) : new RationalNode(value);

    public static MathematicsSolveResult Solved(MathematicsSolveRequest request, ExactRational value, string unit, string strategy, string version)
    {
        var result = Quantity(value, unit);
        return new MathematicsSolveResult(
            MathematicsSolveStatus.Solved,
            result,
            new FiniteSolutionSet([result]),
            request.Assumptions,
            strategy,
            new MathematicsSolutionTrace([]),
            "edulytics-native-mechanics",
            version,
            []);
    }

    public static MathematicsSolveResult Failed(MathematicsSolveRequest request, string error, bool resourceLimit) =>
        resourceLimit ? ResourceLimit(request, error) : Unsupported(request, error);

    public static MathematicsSolveResult Unsupported(MathematicsSolveRequest request, string diagnostic) =>
        new(MathematicsSolveStatus.Unsupported, null, null, request.Assumptions, null, null, "edulytics-native-mechanics", "mechanics-models-exact-v1", [diagnostic]);

    public static MathematicsSolveResult ResourceLimit(MathematicsSolveRequest request, string diagnostic) =>
        new(MathematicsSolveStatus.ResourceLimit, null, null, request.Assumptions, null, null, "edulytics-native-mechanics", "mechanics-models-exact-v1", [diagnostic]);
}

internal static class MechanicsArithmetic
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
