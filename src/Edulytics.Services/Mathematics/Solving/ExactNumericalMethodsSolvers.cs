using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Services.Mathematics.Contracts;

namespace Edulytics.Services.Mathematics.Solving;

public sealed class ExactBisectionIterationSolver : IMathematicsSolver
{
    public MathematicsSolveResult Solve(MathematicsSolveRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (request.Problem is not FunctionCallNode call
            || call.FunctionName != "numerical_bisection_fixed_exact"
            || call.Arguments.Count != 5
            || call.Arguments[1] is not SymbolNode variable)
        {
            return ExactNumericalMethodsV2.Unsupported(request, "Bisection requires numerical_bisection_fixed_exact(polynomial, variable, lower, upper, iterations).");
        }

        if (!ExactNumericalMethodsV2.TryReadScalar(call.Arguments[2], out var lower, out var error, out var resourceLimit)
            || !ExactNumericalMethodsV2.TryReadScalar(call.Arguments[3], out var upper, out error, out resourceLimit)
            || !ExactNumericalMethodsV2.TryReadBoundedPositiveInteger(call.Arguments[4], 8, out var iterations, out error, out resourceLimit))
        {
            return ExactNumericalMethodsV2.Failed(request, error, resourceLimit);
        }
        if (lower.CompareTo(upper) >= 0)
        {
            return ExactNumericalMethodsV2.Unsupported(request, "Bisection requires lower < upper.");
        }

        if (!ExactNumericalPolynomialEvaluator.TryEvaluate(call.Arguments[0], variable.Name, lower, false, out var fLower, out _, out error, out resourceLimit)
            || !ExactNumericalPolynomialEvaluator.TryEvaluate(call.Arguments[0], variable.Name, upper, false, out var fUpper, out _, out error, out resourceLimit))
        {
            return ExactNumericalMethodsV2.Failed(request, error, resourceLimit);
        }

        if (fLower.Numerator.IsZero)
        {
            return ExactNumericalMethodsV2.Solved(request, ExactNumericalMethodsV2.Bracket(lower, lower), "exact-arithmetic-bisection-bracket", "numerical-bisection-exact-arithmetic-v1");
        }
        if (fUpper.Numerator.IsZero)
        {
            return ExactNumericalMethodsV2.Solved(request, ExactNumericalMethodsV2.Bracket(upper, upper), "exact-arithmetic-bisection-bracket", "numerical-bisection-exact-arithmetic-v1");
        }
        if (fLower.Numerator.Sign == fUpper.Numerator.Sign)
        {
            return ExactNumericalMethodsV2.Unsupported(request, "Bisection requires endpoint function values with opposite signs or an exact endpoint root.");
        }

        var left = lower;
        var right = upper;
        var fLeft = fLower;
        for (var i = 0; i < iterations; i++)
        {
            if (!ExactNumericalArithmetic.TryAdd(left, right, out var sum)
                || !ExactNumericalArithmetic.TryDivide(sum, ExactNumericalMethodsV2.Two, out var midpoint))
            {
                return ExactNumericalMethodsV2.ResourceLimit(request, "Bisection midpoint computation exceeds the exact arithmetic budget.");
            }
            if (!ExactNumericalPolynomialEvaluator.TryEvaluate(call.Arguments[0], variable.Name, midpoint, false, out var fMid, out _, out error, out resourceLimit))
            {
                return ExactNumericalMethodsV2.Failed(request, error, resourceLimit);
            }
            if (fMid.Numerator.IsZero)
            {
                left = midpoint;
                right = midpoint;
                break;
            }
            if (fLeft.Numerator.Sign != fMid.Numerator.Sign)
            {
                right = midpoint;
            }
            else
            {
                left = midpoint;
                fLeft = fMid;
            }
        }

        return ExactNumericalMethodsV2.Solved(request, ExactNumericalMethodsV2.Bracket(left, right), "exact-arithmetic-bisection-bracket", "numerical-bisection-exact-arithmetic-v1");
    }
}

public sealed class ExactNewtonIterationSolver : IMathematicsSolver
{
    public MathematicsSolveResult Solve(MathematicsSolveRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (request.Problem is not FunctionCallNode call
            || call.FunctionName != "numerical_newton_fixed_exact"
            || call.Arguments.Count != 4
            || call.Arguments[1] is not SymbolNode variable)
        {
            return ExactNumericalMethodsV2.Unsupported(request, "Newton iteration requires numerical_newton_fixed_exact(polynomial, variable, start, iterations).");
        }

        if (!ExactNumericalMethodsV2.TryReadScalar(call.Arguments[2], out var current, out var error, out var resourceLimit)
            || !ExactNumericalMethodsV2.TryReadBoundedPositiveInteger(call.Arguments[3], 6, out var iterations, out error, out resourceLimit))
        {
            return ExactNumericalMethodsV2.Failed(request, error, resourceLimit);
        }

        for (var i = 0; i < iterations; i++)
        {
            if (!ExactNumericalPolynomialEvaluator.TryEvaluate(call.Arguments[0], variable.Name, current, true, out var value, out var derivative, out error, out resourceLimit))
            {
                return ExactNumericalMethodsV2.Failed(request, error, resourceLimit);
            }
            if (derivative.Numerator.IsZero)
            {
                return ExactNumericalMethodsV2.Unsupported(request, "Newton iteration encountered a zero derivative and fails closed.");
            }
            if (value.Numerator.IsZero)
            {
                break;
            }
            if (!ExactNumericalArithmetic.TryDivide(value, derivative, out var correction)
                || !ExactNumericalArithmetic.TrySubtract(current, correction, out current))
            {
                return ExactNumericalMethodsV2.ResourceLimit(request, "Newton iteration exceeds the exact arithmetic budget.");
            }
        }

        return ExactNumericalMethodsV2.Solved(request, ExactNumericalMethodsV2.ToNode(current), "exact-arithmetic-newton-iterate", "numerical-newton-exact-arithmetic-v1");
    }
}

public sealed class ExactTrapezoidalRuleSolver : IMathematicsSolver
{
    public MathematicsSolveResult Solve(MathematicsSolveRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (request.Problem is not FunctionCallNode call
            || call.FunctionName != "numerical_trapezoidal_fixed_exact"
            || call.Arguments.Count != 5
            || call.Arguments[1] is not SymbolNode variable)
        {
            return ExactNumericalMethodsV2.Unsupported(request, "Trapezoidal estimation requires numerical_trapezoidal_fixed_exact(polynomial, variable, lower, upper, subdivisions).");
        }

        if (!ExactNumericalMethodsV2.TryReadScalar(call.Arguments[2], out var lower, out var error, out var resourceLimit)
            || !ExactNumericalMethodsV2.TryReadScalar(call.Arguments[3], out var upper, out error, out resourceLimit)
            || !ExactNumericalMethodsV2.TryReadBoundedPositiveInteger(call.Arguments[4], 8, out var subdivisions, out error, out resourceLimit))
        {
            return ExactNumericalMethodsV2.Failed(request, error, resourceLimit);
        }
        if (lower.CompareTo(upper) >= 0)
        {
            return ExactNumericalMethodsV2.Unsupported(request, "Trapezoidal estimation requires lower < upper.");
        }

        if (!ExactNumericalArithmetic.TrySubtract(upper, lower, out var width))
        {
            return ExactNumericalMethodsV2.ResourceLimit(request, "Trapezoidal interval width exceeds the exact arithmetic budget.");
        }
        if (!ExactNumericalArithmetic.TryDivide(width, new ExactRational(new BigInteger(subdivisions), BigInteger.One), out var h))
        {
            return ExactNumericalMethodsV2.ResourceLimit(request, "Trapezoidal step width exceeds the exact arithmetic budget.");
        }
        if (!ExactNumericalPolynomialEvaluator.TryEvaluate(call.Arguments[0], variable.Name, lower, false, out var first, out _, out error, out resourceLimit)
            || !ExactNumericalPolynomialEvaluator.TryEvaluate(call.Arguments[0], variable.Name, upper, false, out var last, out _, out error, out resourceLimit))
        {
            return ExactNumericalMethodsV2.Failed(request, error, resourceLimit);
        }

        if (!ExactNumericalArithmetic.TryAdd(first, last, out var weightedSum))
        {
            return ExactNumericalMethodsV2.ResourceLimit(request, "Trapezoidal endpoint accumulation exceeds the exact arithmetic budget.");
        }
        var x = lower;
        for (var i = 1; i < subdivisions; i++)
        {
            if (!ExactNumericalArithmetic.TryAdd(x, h, out x))
            {
                return ExactNumericalMethodsV2.ResourceLimit(request, "Trapezoidal sample position exceeds the exact arithmetic budget.");
            }
            if (!ExactNumericalPolynomialEvaluator.TryEvaluate(call.Arguments[0], variable.Name, x, false, out var value, out _, out error, out resourceLimit))
            {
                return ExactNumericalMethodsV2.Failed(request, error, resourceLimit);
            }
            if (!ExactNumericalArithmetic.TryMultiply(ExactNumericalMethodsV2.Two, value, out var doubled)
                || !ExactNumericalArithmetic.TryAdd(weightedSum, doubled, out weightedSum))
            {
                return ExactNumericalMethodsV2.ResourceLimit(request, "Trapezoidal weighted accumulation exceeds the exact arithmetic budget.");
            }
        }

        if (!ExactNumericalArithmetic.TryDivide(h, ExactNumericalMethodsV2.Two, out var halfWidth)
            || !ExactNumericalArithmetic.TryMultiply(halfWidth, weightedSum, out var estimate))
        {
            return ExactNumericalMethodsV2.ResourceLimit(request, "Trapezoidal estimate exceeds the exact arithmetic budget.");
        }

        return ExactNumericalMethodsV2.Solved(request, ExactNumericalMethodsV2.ToNode(estimate), "exact-arithmetic-trapezoidal-estimate", "numerical-trapezoidal-exact-arithmetic-v1");
    }
}

internal static class ExactNumericalPolynomialEvaluator
{
    private const int MaxNodes = 128;
    private const int MaxDepth = 24;
    private const int MaxExponent = 8;

    public static bool TryEvaluate(MathNode node, string variable, ExactRational x, bool derivativeRequired,
        out ExactRational value, out ExactRational derivative, out string error, out bool resourceLimit)
    {
        value = default;
        derivative = default;
        if (string.IsNullOrWhiteSpace(variable))
        {
            error = "Numerical polynomial variable is missing.";
            resourceLimit = false;
            return false;
        }
        if (!ExactNumericalArithmetic.IsValid(x))
        {
            error = "Numerical evaluation point exceeds the supported exact-rational scalar budget.";
            resourceLimit = true;
            return false;
        }
        var remaining = MaxNodes;
        return TryEvaluateCore(node, variable, x, derivativeRequired, 0, ref remaining, out value, out derivative, out error, out resourceLimit);
    }

    private static bool TryEvaluateCore(MathNode node, string variable, ExactRational x, bool derivativeRequired, int depth, ref int remaining,
        out ExactRational value, out ExactRational derivative, out string error, out bool resourceLimit)
    {
        value = ExactNumericalMethodsV2.Zero;
        derivative = ExactNumericalMethodsV2.Zero;
        if (node is null)
        {
            error = "Numerical polynomial contains a null node.";
            resourceLimit = false;
            return false;
        }
        if (depth > MaxDepth || --remaining < 0)
        {
            error = "Numerical polynomial exceeds the supported depth/node budget.";
            resourceLimit = true;
            return false;
        }

        switch (node)
        {
            case IntegerNode integer:
                value = new ExactRational(integer.Value, BigInteger.One);
                if (!ExactNumericalArithmetic.IsValid(value)) { error = "Numerical integer scalar exceeds the 4096-bit budget."; resourceLimit = true; return false; }
                error = string.Empty; resourceLimit = false; return true;
            case RationalNode rational when !rational.Value.Denominator.IsZero:
                value = rational.Value;
                if (!ExactNumericalArithmetic.IsValid(value)) { error = "Numerical rational scalar exceeds the 4096-bit budget."; resourceLimit = true; return false; }
                error = string.Empty; resourceLimit = false; return true;
            case RationalNode:
                error = "Numerical polynomial contains a malformed rational with zero denominator."; resourceLimit = false; return false;
            case SymbolNode symbol when string.Equals(symbol.Name, variable, StringComparison.Ordinal):
                value = x; derivative = derivativeRequired ? ExactNumericalMethodsV2.One : ExactNumericalMethodsV2.Zero;
                error = string.Empty; resourceLimit = false; return true;
            case SymbolNode symbol:
                error = $"Unexpected symbol {symbol.Name}; numerical polynomial is bound to {variable}."; resourceLimit = false; return false;
            case NegateNode negate:
                if (!TryEvaluateCore(negate.Operand, variable, x, derivativeRequired, depth + 1, ref remaining, out var nv, out var nd, out error, out resourceLimit)) return false;
                value = ExactNumericalArithmetic.Negate(nv); derivative = ExactNumericalArithmetic.Negate(nd); return true;
            case AddNode add:
                value = ExactNumericalMethodsV2.Zero; derivative = ExactNumericalMethodsV2.Zero;
                foreach (var term in add.Terms)
                {
                    if (!TryEvaluateCore(term, variable, x, derivativeRequired, depth + 1, ref remaining, out var tv, out var td, out error, out resourceLimit)) return false;
                    if (!ExactNumericalArithmetic.TryAdd(value, tv, out value) || (derivativeRequired && !ExactNumericalArithmetic.TryAdd(derivative, td, out derivative)))
                    { error = "Numerical polynomial addition exceeds the exact arithmetic budget."; resourceLimit = true; return false; }
                }
                error = string.Empty; resourceLimit = false; return true;
            case MultiplyNode multiply:
                value = ExactNumericalMethodsV2.One; derivative = ExactNumericalMethodsV2.Zero;
                foreach (var factor in multiply.Factors)
                {
                    if (!TryEvaluateCore(factor, variable, x, derivativeRequired, depth + 1, ref remaining, out var fv, out var fd, out error, out resourceLimit)) return false;
                    var previousValue = value;
                    var previousDerivative = derivative;
                    if (!ExactNumericalArithmetic.TryMultiply(previousValue, fv, out value))
                    { error = "Numerical polynomial multiplication exceeds the exact arithmetic budget."; resourceLimit = true; return false; }
                    if (derivativeRequired)
                    {
                        if (!ExactNumericalArithmetic.TryMultiply(previousDerivative, fv, out var left)
                            || !ExactNumericalArithmetic.TryMultiply(previousValue, fd, out var right)
                            || !ExactNumericalArithmetic.TryAdd(left, right, out derivative))
                        { error = "Numerical polynomial derivative propagation exceeds the exact arithmetic budget."; resourceLimit = true; return false; }
                    }
                }
                error = string.Empty; resourceLimit = false; return true;
            case DivideNode divide:
                if (ContainsVariable(divide.Denominator, variable))
                { error = "Numerical polynomial division is supported only by an exact scalar constant denominator."; resourceLimit = false; return false; }
                if (!TryEvaluateCore(divide.Numerator, variable, x, derivativeRequired, depth + 1, ref remaining, out var numValue, out var numDerivative, out error, out resourceLimit)
                    || !TryEvaluateCore(divide.Denominator, variable, x, false, depth + 1, ref remaining, out var denValue, out _, out error, out resourceLimit)) return false;
                if (denValue.Numerator.IsZero) { error = "Numerical polynomial division by zero is unsupported."; resourceLimit = false; return false; }
                if (!ExactNumericalArithmetic.TryDivide(numValue, denValue, out value)
                    || (derivativeRequired && !ExactNumericalArithmetic.TryDivide(numDerivative, denValue, out derivative)))
                { error = "Numerical polynomial scalar division exceeds the exact arithmetic budget."; resourceLimit = true; return false; }
                error = string.Empty; resourceLimit = false; return true;
            case PowerNode power when power.Exponent is IntegerNode exponentNode
                                      && exponentNode.Value >= BigInteger.Zero
                                      && exponentNode.Value <= new BigInteger(MaxExponent):
                if (!TryEvaluateCore(power.Base, variable, x, derivativeRequired, depth + 1, ref remaining, out var baseValue, out var baseDerivative, out error, out resourceLimit)) return false;
                var exponent = (int)exponentNode.Value;
                if (!ExactNumericalArithmetic.TryPow(baseValue, exponent, out value))
                { error = "Numerical polynomial power exceeds the exact arithmetic budget."; resourceLimit = true; return false; }
                derivative = ExactNumericalMethodsV2.Zero;
                if (derivativeRequired && exponent > 0)
                {
                    if (!ExactNumericalArithmetic.TryPow(baseValue, exponent - 1, out var reducedPower)
                        || !ExactNumericalArithmetic.TryMultiply(new ExactRational(new BigInteger(exponent), BigInteger.One), reducedPower, out var factor)
                        || !ExactNumericalArithmetic.TryMultiply(factor, baseDerivative, out derivative))
                    { error = "Numerical polynomial power derivative exceeds the exact arithmetic budget."; resourceLimit = true; return false; }
                }
                error = string.Empty; resourceLimit = false; return true;
            case PowerNode:
                error = $"Numerical polynomial powers must be integer constants from 0 through {MaxExponent}."; resourceLimit = false; return false;
            default:
                error = $"Node type {node.GetType().Name} is outside the bounded numerical polynomial subset."; resourceLimit = false; return false;
        }
    }

    private static bool ContainsVariable(MathNode node, string variable)
    {
        if (node is null) return true;
        return node switch
        {
            SymbolNode symbol => string.Equals(symbol.Name, variable, StringComparison.Ordinal),
            NegateNode negate => ContainsVariable(negate.Operand, variable),
            AddNode add => add.Terms.Any(term => ContainsVariable(term, variable)),
            MultiplyNode multiply => multiply.Factors.Any(factor => ContainsVariable(factor, variable)),
            DivideNode divide => ContainsVariable(divide.Numerator, variable) || ContainsVariable(divide.Denominator, variable),
            PowerNode power => ContainsVariable(power.Base, variable) || ContainsVariable(power.Exponent, variable),
            _ => false
        };
    }
}

internal static class ExactNumericalArithmetic
{
    private const int MaxBits = 4096;
    private const long MaxIntermediateBits = (MaxBits * 2L) + 1L;

    public static bool TryAdd(ExactRational left, ExactRational right, out ExactRational result) => TryAddSubtract(left, right, false, out result);
    public static bool TrySubtract(ExactRational left, ExactRational right, out ExactRational result) => TryAddSubtract(left, right, true, out result);
    private static bool TryAddSubtract(ExactRational left, ExactRational right, bool subtract, out ExactRational result)
    {
        result = default;
        if (!IsValid(left) || !IsValid(right)) return false;
        var leftBits = AddBits(Bits(left.Numerator), Bits(right.Denominator));
        var rightBits = AddBits(Bits(right.Numerator), Bits(left.Denominator));
        if (AddBits(Math.Max(leftBits, rightBits), 1) > MaxIntermediateBits || AddBits(Bits(left.Denominator), Bits(right.Denominator)) > MaxIntermediateBits) return false;
        result = subtract ? left - right : left + right;
        return IsValid(result);
    }
    public static bool TryMultiply(ExactRational left, ExactRational right, out ExactRational result)
    {
        result = default;
        if (!IsValid(left) || !IsValid(right)
            || AddBits(Bits(left.Numerator), Bits(right.Numerator)) > MaxIntermediateBits
            || AddBits(Bits(left.Denominator), Bits(right.Denominator)) > MaxIntermediateBits) return false;
        result = left * right;
        return IsValid(result);
    }
    public static bool TryDivide(ExactRational left, ExactRational right, out ExactRational result)
    {
        result = default;
        if (!IsValid(left) || !IsValid(right) || right.Numerator.IsZero
            || AddBits(Bits(left.Numerator), Bits(right.Denominator)) > MaxIntermediateBits
            || AddBits(Bits(left.Denominator), Bits(right.Numerator)) > MaxIntermediateBits) return false;
        result = left / right;
        return IsValid(result);
    }
    public static bool TryPow(ExactRational value, int exponent, out ExactRational result)
    {
        result = ExactNumericalMethodsV2.One;
        if (!IsValid(value) || exponent is < 0 or > 8) return false;
        for (var i = 0; i < exponent; i++) if (!TryMultiply(result, value, out result)) return false;
        return true;
    }
    public static ExactRational Negate(ExactRational value) => new(BigInteger.Negate(value.Numerator), value.Denominator);
    public static bool IsValid(ExactRational value) => !value.Denominator.IsZero && Bits(value.Numerator) <= MaxBits && Bits(value.Denominator) <= MaxBits;
    private static long Bits(BigInteger value) => value.IsZero ? 0 : BigInteger.Abs(value).GetBitLength();
    private static long AddBits(long a, long b) => a > long.MaxValue - b ? long.MaxValue : a + b;
}

internal static class ExactNumericalMethodsV2
{
    public static readonly ExactRational Zero = new(BigInteger.Zero, BigInteger.One);
    public static readonly ExactRational One = new(BigInteger.One, BigInteger.One);
    public static readonly ExactRational Two = new(new BigInteger(2), BigInteger.One);

    public static bool TryReadScalar(MathNode node, out ExactRational value, out string error, out bool resourceLimit)
    {
        resourceLimit = false;
        switch (node)
        {
            case IntegerNode integer: value = new ExactRational(integer.Value, BigInteger.One); break;
            case RationalNode rational when !rational.Value.Denominator.IsZero: value = rational.Value; break;
            case RationalNode: value = default; error = "Numerical scalar contains a malformed rational with zero denominator."; return false;
            default: value = default; error = "Numerical methods accept exact integer or rational scalar parameters only."; return false;
        }
        if (!ExactNumericalArithmetic.IsValid(value)) { error = "Numerical scalar exceeds the supported 4096-bit budget."; resourceLimit = true; return false; }
        error = string.Empty; return true;
    }

    public static bool TryReadBoundedPositiveInteger(MathNode node, int maximum, out int value, out string error, out bool resourceLimit)
    {
        value = 0; resourceLimit = false;
        if (node is not IntegerNode integer) { error = "Iteration/subdivision count must be an exact integer."; return false; }
        if (integer.Value < BigInteger.One || integer.Value > new BigInteger(maximum)) { error = $"Iteration/subdivision count must be between 1 and {maximum}."; return false; }
        value = (int)integer.Value; error = string.Empty; return true;
    }

    public static MathNode ToNode(ExactRational value) => value.Denominator == BigInteger.One ? new IntegerNode(value.Numerator) : new RationalNode(value);
    public static VectorNode Bracket(ExactRational lower, ExactRational upper) => new([ToNode(lower), ToNode(upper)]);

    public static MathematicsSolveResult Solved(MathematicsSolveRequest request, MathNode result, string strategy, string version) =>
        new(MathematicsSolveStatus.Solved, result, new FiniteSolutionSet([result]), request.Assumptions, strategy, new MathematicsSolutionTrace([]), "edulytics-native-numerical-methods", version, []);
    public static MathematicsSolveResult Failed(MathematicsSolveRequest request, string diagnostic, bool resourceLimit) =>
        resourceLimit ? ResourceLimit(request, diagnostic) : Unsupported(request, diagnostic);
    public static MathematicsSolveResult Unsupported(MathematicsSolveRequest request, string diagnostic) =>
        new(MathematicsSolveStatus.Unsupported, null, null, request.Assumptions, null, null, "edulytics-native-numerical-methods", "numerical-methods-exact-arithmetic-v1", [diagnostic]);
    public static MathematicsSolveResult ResourceLimit(MathematicsSolveRequest request, string diagnostic) =>
        new(MathematicsSolveStatus.ResourceLimit, null, null, request.Assumptions, null, null, "edulytics-native-numerical-methods", "numerical-methods-exact-arithmetic-v1", [diagnostic]);
}
