using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Core.Mathematics.Verification;
using Edulytics.Services.Mathematics.Contracts;

namespace Edulytics.Services.Mathematics.Verification;

public sealed class ExactBisectionIterationVerifier : IMathematicsVerifier
{
    public MathematicsVerificationResult Verify(MathematicsSolveRequest request, MathematicsSolveResult result)
    {
        ArgumentNullException.ThrowIfNull(request); ArgumentNullException.ThrowIfNull(result);
        if (request.Problem is not FunctionCallNode call || call.FunctionName != "numerical_bisection_fixed_exact" || call.Arguments.Count != 5
            || call.Arguments[1] is not SymbolNode variable
            || !NumericalMethodsVerification.TryReadScalar(call.Arguments[2], out var lower)
            || !NumericalMethodsVerification.TryReadScalar(call.Arguments[3], out var upper)
            || !NumericalMethodsVerification.TryReadBoundedPositiveInteger(call.Arguments[4], 8, out var iterations)
            || lower.CompareTo(upper) >= 0)
        {
            return NumericalMethodsVerification.Unsupported("Independent bisection verification requires the original bounded exact request.");
        }
        if (!NumericalVerificationEvaluator.TryEvaluate(call.Arguments[0], variable.Name, lower, false, out var fLower, out _)
            || !NumericalVerificationEvaluator.TryEvaluate(call.Arguments[0], variable.Name, upper, false, out var fUpper, out _))
        {
            return NumericalMethodsVerification.Unsupported("Independent bisection verification could not evaluate the original polynomial within its resource budget.");
        }

        var left = lower; var right = upper; var fLeft = fLower;
        if (fLower.Numerator.IsZero) right = left;
        else if (fUpper.Numerator.IsZero) left = right;
        else
        {
            if (fLower.Numerator.Sign == fUpper.Numerator.Sign)
                return NumericalMethodsVerification.Unsupported("Original bisection endpoints do not bracket a sign change.");
            for (var i = 0; i < iterations; i++)
            {
                if (!NumericalVerificationArithmetic.TryAdd(left, right, out var sum)
                    || !NumericalVerificationArithmetic.TryDivide(sum, NumericalMethodsVerification.Two, out var midpoint)
                    || !NumericalVerificationEvaluator.TryEvaluate(call.Arguments[0], variable.Name, midpoint, false, out var fMid, out _))
                    return NumericalMethodsVerification.Unsupported("Independent bisection recomputation exceeded its bounded exact arithmetic budget.");
                if (fMid.Numerator.IsZero) { left = midpoint; right = midpoint; break; }
                if (fLeft.Numerator.Sign != fMid.Numerator.Sign) right = midpoint;
                else { left = midpoint; fLeft = fMid; }
            }
        }
        return NumericalMethodsVerification.CompareBracket(result, left, right, "independent-bisection-fixed-iteration-recomputation");
    }
}

public sealed class ExactNewtonIterationVerifier : IMathematicsVerifier
{
    public MathematicsVerificationResult Verify(MathematicsSolveRequest request, MathematicsSolveResult result)
    {
        ArgumentNullException.ThrowIfNull(request); ArgumentNullException.ThrowIfNull(result);
        if (request.Problem is not FunctionCallNode call || call.FunctionName != "numerical_newton_fixed_exact" || call.Arguments.Count != 4
            || call.Arguments[1] is not SymbolNode variable
            || !NumericalMethodsVerification.TryReadScalar(call.Arguments[2], out var current)
            || !NumericalMethodsVerification.TryReadBoundedPositiveInteger(call.Arguments[3], 6, out var iterations))
        {
            return NumericalMethodsVerification.Unsupported("Independent Newton verification requires the original bounded exact request.");
        }
        for (var i = 0; i < iterations; i++)
        {
            if (!NumericalVerificationEvaluator.TryEvaluate(call.Arguments[0], variable.Name, current, true, out var value, out var derivative))
                return NumericalMethodsVerification.Unsupported("Independent Newton recomputation could not evaluate the original polynomial.");
            if (value.Numerator.IsZero) break;
            if (derivative.Numerator.IsZero)
                return NumericalMethodsVerification.Unsupported("Original Newton iteration encounters a zero derivative.");
            if (!NumericalVerificationArithmetic.TryDivide(value, derivative, out var correction)
                || !NumericalVerificationArithmetic.TrySubtract(current, correction, out current))
                return NumericalMethodsVerification.Unsupported("Independent Newton recomputation exceeded its bounded exact arithmetic budget.");
        }
        return NumericalMethodsVerification.CompareScalar(result, current, "independent-newton-fixed-iteration-recomputation");
    }
}

public sealed class ExactTrapezoidalRuleVerifier : IMathematicsVerifier
{
    public MathematicsVerificationResult Verify(MathematicsSolveRequest request, MathematicsSolveResult result)
    {
        ArgumentNullException.ThrowIfNull(request); ArgumentNullException.ThrowIfNull(result);
        if (request.Problem is not FunctionCallNode call || call.FunctionName != "numerical_trapezoidal_fixed_exact" || call.Arguments.Count != 5
            || call.Arguments[1] is not SymbolNode variable
            || !NumericalMethodsVerification.TryReadScalar(call.Arguments[2], out var lower)
            || !NumericalMethodsVerification.TryReadScalar(call.Arguments[3], out var upper)
            || !NumericalMethodsVerification.TryReadBoundedPositiveInteger(call.Arguments[4], 8, out var subdivisions)
            || lower.CompareTo(upper) >= 0)
        {
            return NumericalMethodsVerification.Unsupported("Independent trapezoidal verification requires the original bounded exact request.");
        }
        if (!NumericalVerificationArithmetic.TrySubtract(upper, lower, out var width)
            || !NumericalVerificationArithmetic.TryDivide(width, new ExactRational(new BigInteger(subdivisions), BigInteger.One), out var h)
            || !NumericalVerificationEvaluator.TryEvaluate(call.Arguments[0], variable.Name, lower, false, out var first, out _)
            || !NumericalVerificationEvaluator.TryEvaluate(call.Arguments[0], variable.Name, upper, false, out var last, out _)
            || !NumericalVerificationArithmetic.TryAdd(first, last, out var weightedSum))
        {
            return NumericalMethodsVerification.Unsupported("Independent trapezoidal setup exceeded its bounded exact arithmetic budget.");
        }
        var x = lower;
        for (var i = 1; i < subdivisions; i++)
        {
            if (!NumericalVerificationArithmetic.TryAdd(x, h, out x)
                || !NumericalVerificationEvaluator.TryEvaluate(call.Arguments[0], variable.Name, x, false, out var sample, out _)
                || !NumericalVerificationArithmetic.TryMultiply(NumericalMethodsVerification.Two, sample, out var doubled)
                || !NumericalVerificationArithmetic.TryAdd(weightedSum, doubled, out weightedSum))
                return NumericalMethodsVerification.Unsupported("Independent trapezoidal sampling exceeded its bounded exact arithmetic budget.");
        }
        if (!NumericalVerificationArithmetic.TryDivide(h, NumericalMethodsVerification.Two, out var halfWidth)
            || !NumericalVerificationArithmetic.TryMultiply(halfWidth, weightedSum, out var expected))
            return NumericalMethodsVerification.Unsupported("Independent trapezoidal estimate exceeded its bounded exact arithmetic budget.");
        return NumericalMethodsVerification.CompareScalar(result, expected, "independent-trapezoidal-fixed-subdivision-recomputation");
    }
}

internal static class NumericalVerificationEvaluator
{
    private const int MaxNodes = 128; private const int MaxDepth = 24; private const int MaxExponent = 8;
    public static bool TryEvaluate(MathNode node, string variable, ExactRational x, bool derivativeRequired, out ExactRational value, out ExactRational derivative)
    {
        value = default; derivative = default;
        if (string.IsNullOrWhiteSpace(variable) || !NumericalVerificationArithmetic.IsValid(x)) return false;
        var remaining = MaxNodes;
        return Core(node, variable, x, derivativeRequired, 0, ref remaining, out value, out derivative);
    }
    private static bool Core(MathNode node, string variable, ExactRational x, bool derivativeRequired, int depth, ref int remaining, out ExactRational value, out ExactRational derivative)
    {
        value = NumericalMethodsVerification.Zero; derivative = NumericalMethodsVerification.Zero;
        if (node is null || depth > MaxDepth || --remaining < 0) return false;
        switch (node)
        {
            case IntegerNode integer:
                value = new ExactRational(integer.Value, BigInteger.One); return NumericalVerificationArithmetic.IsValid(value);
            case RationalNode rational when !rational.Value.Denominator.IsZero:
                value = rational.Value; return NumericalVerificationArithmetic.IsValid(value);
            case SymbolNode symbol when string.Equals(symbol.Name, variable, StringComparison.Ordinal):
                value = x; derivative = derivativeRequired ? NumericalMethodsVerification.One : NumericalMethodsVerification.Zero; return true;
            case NegateNode negate:
                if (!Core(negate.Operand, variable, x, derivativeRequired, depth + 1, ref remaining, out var nv, out var nd)) return false;
                value = NumericalVerificationArithmetic.Negate(nv); derivative = NumericalVerificationArithmetic.Negate(nd); return true;
            case AddNode add:
                value = NumericalMethodsVerification.Zero; derivative = NumericalMethodsVerification.Zero;
                foreach (var term in add.Terms)
                {
                    if (!Core(term, variable, x, derivativeRequired, depth + 1, ref remaining, out var tv, out var td)
                        || !NumericalVerificationArithmetic.TryAdd(value, tv, out value)
                        || (derivativeRequired && !NumericalVerificationArithmetic.TryAdd(derivative, td, out derivative))) return false;
                }
                return true;
            case MultiplyNode multiply:
                value = NumericalMethodsVerification.One; derivative = NumericalMethodsVerification.Zero;
                foreach (var factor in multiply.Factors)
                {
                    if (!Core(factor, variable, x, derivativeRequired, depth + 1, ref remaining, out var fv, out var fd)) return false;
                    var pv = value; var pd = derivative;
                    if (!NumericalVerificationArithmetic.TryMultiply(pv, fv, out value)) return false;
                    if (derivativeRequired)
                    {
                        if (!NumericalVerificationArithmetic.TryMultiply(pd, fv, out var a)
                            || !NumericalVerificationArithmetic.TryMultiply(pv, fd, out var b)
                            || !NumericalVerificationArithmetic.TryAdd(a, b, out derivative)) return false;
                    }
                }
                return true;
            case DivideNode divide:
                if (!Core(divide.Numerator, variable, x, derivativeRequired, depth + 1, ref remaining, out var n, out var nd)
                    || !Core(divide.Denominator, variable, x, true, depth + 1, ref remaining, out var d, out var dd)
                    || !dd.Numerator.IsZero || d.Numerator.IsZero
                    || !NumericalVerificationArithmetic.TryDivide(n, d, out value)
                    || (derivativeRequired && !NumericalVerificationArithmetic.TryDivide(nd, d, out derivative))) return false;
                return true;
            case PowerNode power when power.Exponent is IntegerNode exponentNode && exponentNode.Value >= BigInteger.Zero && exponentNode.Value <= new BigInteger(MaxExponent):
                if (!Core(power.Base, variable, x, derivativeRequired, depth + 1, ref remaining, out var bv, out var bd)) return false;
                var e = (int)exponentNode.Value;
                if (!NumericalVerificationArithmetic.TryPow(bv, e, out value)) return false;
                derivative = NumericalMethodsVerification.Zero;
                if (derivativeRequired && e > 0)
                {
                    if (!NumericalVerificationArithmetic.TryPow(bv, e - 1, out var p)
                        || !NumericalVerificationArithmetic.TryMultiply(new ExactRational(new BigInteger(e), BigInteger.One), p, out var coefficient)
                        || !NumericalVerificationArithmetic.TryMultiply(coefficient, bd, out derivative)) return false;
                }
                return true;
            default: return false;
        }
    }
}

internal static class NumericalVerificationArithmetic
{
    private const int MaxBits = 4096; private const long MaxIntermediateBits = (MaxBits * 2L) + 1L;
    public static bool TryAdd(ExactRational a, ExactRational b, out ExactRational result) => AddSub(a, b, false, out result);
    public static bool TrySubtract(ExactRational a, ExactRational b, out ExactRational result) => AddSub(a, b, true, out result);
    private static bool AddSub(ExactRational a, ExactRational b, bool subtract, out ExactRational result)
    {
        result = default; if (!IsValid(a) || !IsValid(b)) return false;
        var x = AddBits(Bits(a.Numerator), Bits(b.Denominator)); var y = AddBits(Bits(b.Numerator), Bits(a.Denominator));
        if (AddBits(Math.Max(x, y), 1) > MaxIntermediateBits || AddBits(Bits(a.Denominator), Bits(b.Denominator)) > MaxIntermediateBits) return false;
        result = subtract ? a - b : a + b; return IsValid(result);
    }
    public static bool TryMultiply(ExactRational a, ExactRational b, out ExactRational result)
    {
        result = default; if (!IsValid(a) || !IsValid(b) || AddBits(Bits(a.Numerator), Bits(b.Numerator)) > MaxIntermediateBits || AddBits(Bits(a.Denominator), Bits(b.Denominator)) > MaxIntermediateBits) return false;
        result = a * b; return IsValid(result);
    }
    public static bool TryDivide(ExactRational a, ExactRational b, out ExactRational result)
    {
        result = default; if (!IsValid(a) || !IsValid(b) || b.Numerator.IsZero || AddBits(Bits(a.Numerator), Bits(b.Denominator)) > MaxIntermediateBits || AddBits(Bits(a.Denominator), Bits(b.Numerator)) > MaxIntermediateBits) return false;
        result = a / b; return IsValid(result);
    }
    public static bool TryPow(ExactRational value, int exponent, out ExactRational result)
    {
        result = NumericalMethodsVerification.One; if (!IsValid(value) || exponent is < 0 or > 8) return false;
        for (var i = 0; i < exponent; i++) if (!TryMultiply(result, value, out result)) return false; return true;
    }
    public static ExactRational Negate(ExactRational v) => new(BigInteger.Negate(v.Numerator), v.Denominator);
    public static bool IsValid(ExactRational v) => !v.Denominator.IsZero && Bits(v.Numerator) <= MaxBits && Bits(v.Denominator) <= MaxBits;
    private static long Bits(BigInteger v) => v.IsZero ? 0 : BigInteger.Abs(v).GetBitLength();
    private static long AddBits(long a, long b) => a > long.MaxValue - b ? long.MaxValue : a + b;
}

internal static class NumericalMethodsVerification
{
    public static readonly ExactRational Zero = new(BigInteger.Zero, BigInteger.One);
    public static readonly ExactRational One = new(BigInteger.One, BigInteger.One);
    public static readonly ExactRational Two = new(new BigInteger(2), BigInteger.One);
    public static bool TryReadScalar(MathNode node, out ExactRational value)
    {
        switch (node)
        {
            case IntegerNode i: value = new ExactRational(i.Value, BigInteger.One); return NumericalVerificationArithmetic.IsValid(value);
            case RationalNode r when !r.Value.Denominator.IsZero: value = r.Value; return NumericalVerificationArithmetic.IsValid(value);
            default: value = default; return false;
        }
    }
    public static bool TryReadBoundedPositiveInteger(MathNode node, int max, out int value)
    {
        value = 0; if (node is not IntegerNode i || i.Value < BigInteger.One || i.Value > new BigInteger(max)) return false; value = (int)i.Value; return true;
    }
    public static MathematicsVerificationResult CompareScalar(MathematicsSolveResult result, ExactRational expected, string method)
    {
        if (result.Status != MathematicsSolveStatus.Solved || result.ExactResult is null || result.SolutionSet is not FiniteSolutionSet finite || finite.Values.Count != 1) return Rejected("Numerical solved result must contain one exact-arithmetic method output.");
        if (!TryReadScalar(result.ExactResult, out var actual) || actual != expected || !TryReadScalar(finite.Values[0], out var setValue) || setValue != expected) return Rejected("Numerical method output does not equal the independently recomputed fixed-step result.");
        return Verified(method);
    }
    public static MathematicsVerificationResult CompareBracket(MathematicsSolveResult result, ExactRational expectedLower, ExactRational expectedUpper, string method)
    {
        if (result.Status != MathematicsSolveStatus.Solved || result.ExactResult is not VectorNode vector || vector.Components.Count != 2 || result.SolutionSet is not FiniteSolutionSet finite || finite.Values.Count != 1 || finite.Values[0] is not VectorNode setVector || setVector.Components.Count != 2) return Rejected("Bisection result must be a two-value exact-rational bracket in ExactResult and SolutionSet.");
        if (!TryReadScalar(vector.Components[0], out var lower) || !TryReadScalar(vector.Components[1], out var upper) || lower != expectedLower || upper != expectedUpper
            || !TryReadScalar(setVector.Components[0], out var setLower) || !TryReadScalar(setVector.Components[1], out var setUpper) || setLower != expectedLower || setUpper != expectedUpper) return Rejected("Bisection bracket does not equal the independently recomputed fixed-iteration bracket.");
        return Verified(method);
    }
    private static MathematicsVerificationResult Verified(string method) => new(MathematicsVerificationStatus.Verified, [new MathematicsVerificationEvidence(method, "The numerical-method output was independently recomputed from the original polynomial and fixed method parameters using bounded exact-rational arithmetic.")], []);
    public static MathematicsVerificationResult Unsupported(string diagnostic) => new(MathematicsVerificationStatus.Unsupported, [], [diagnostic]);
    private static MathematicsVerificationResult Rejected(string diagnostic) => new(MathematicsVerificationStatus.Rejected, [], [diagnostic]);
}
