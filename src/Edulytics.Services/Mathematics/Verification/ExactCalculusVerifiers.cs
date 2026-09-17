using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Core.Mathematics.Verification;
using Edulytics.Services.Mathematics.Contracts;

namespace Edulytics.Services.Mathematics.Verification;

public sealed class ExactPolynomialDerivativeVerifier : IMathematicsVerifier
{
    public MathematicsVerificationResult Verify(MathematicsSolveRequest request, MathematicsSolveResult result)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(result);
        if (request.Problem is not DerivativeNode derivative || derivative.Order != 1)
        {
            return VerifierCalculusV2.Unsupported("Independent calculus verification requires a first-order DerivativeNode.");
        }
        if (!VerifierPolynomial.TryParse(derivative.Expression, derivative.Variable.Name, 8, out var original, out var error))
        {
            return VerifierCalculusV2.Unsupported(error);
        }

        var expected = new Dictionary<int, ExactRational>();
        foreach (var pair in original)
        {
            if (pair.Key == 0) continue;
            if (!VerifierCalcArithmetic.TryMultiply(pair.Value, new ExactRational(pair.Key, 1), out var coefficient))
            {
                return VerifierCalculusV2.Unsupported("Independent derivative verification exceeded its exact arithmetic budget.");
            }
            if (coefficient != VerifierCalcArithmetic.Zero)
                expected[pair.Key - 1] = coefficient;
        }

        if (!VerifierCalculusV2.TryReadSolvedPolynomial(result, derivative.Variable.Name, 7, out var reported, out error))
        {
            return VerifierCalculusV2.Rejected(error);
        }
        if (!VerifierCalculusV2.PolynomialsEqual(expected, reported))
        {
            return VerifierCalculusV2.Rejected("Reported derivative does not equal the independently differentiated original polynomial.");
        }

        return VerifierCalculusV2.Verified(
            "independent-exact-polynomial-differentiation",
            "The derivative coefficients were independently derived from the original polynomial and matched against the reported result.");
    }
}

public sealed class ExactPolynomialDefiniteIntegralVerifier : IMathematicsVerifier
{
    public MathematicsVerificationResult Verify(MathematicsSolveRequest request, MathematicsSolveResult result)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(result);
        if (request.Problem is not IntegralNode integral || integral.LowerBound is null || integral.UpperBound is null)
        {
            return VerifierCalculusV2.Unsupported("Independent calculus verification requires a definite IntegralNode with both bounds.");
        }
        if (!VerifierPolynomial.TryParse(integral.Integrand, integral.Variable.Name, 8, out var polynomial, out var error)
            || !VerifierCalculusV2.TryReadScalar(integral.LowerBound, out var lower)
            || !VerifierCalculusV2.TryReadScalar(integral.UpperBound, out var upper))
        {
            return VerifierCalculusV2.Unsupported(string.IsNullOrWhiteSpace(error) ? "Integral bounds must be exact integer or rational scalars." : error);
        }

        var expected = VerifierCalcArithmetic.Zero;
        foreach (var pair in polynomial)
        {
            var degree = pair.Key + 1;
            if (!VerifierCalcArithmetic.TryDivide(pair.Value, new ExactRational(degree, 1), out var antiCoefficient)
                || !VerifierCalcArithmetic.TryPow(upper, degree, out var upperPower)
                || !VerifierCalcArithmetic.TryPow(lower, degree, out var lowerPower)
                || !VerifierCalcArithmetic.TrySubtract(upperPower, lowerPower, out var difference)
                || !VerifierCalcArithmetic.TryMultiply(antiCoefficient, difference, out var term)
                || !VerifierCalcArithmetic.TryAdd(expected, term, out expected))
            {
                return VerifierCalculusV2.Unsupported("Independent definite-integral verification exceeded its exact arithmetic budget.");
            }
        }

        if (!VerifierCalculusV2.TryReadSolvedScalar(result, out var reported, out error))
        {
            return VerifierCalculusV2.Rejected(error);
        }
        if (reported != expected)
        {
            return VerifierCalculusV2.Rejected("Reported definite integral does not equal the independently recomputed exact value from the original integrand and bounds.");
        }

        return VerifierCalculusV2.Verified(
            "independent-exact-polynomial-definite-integral",
            "The definite integral was independently recomputed from the original polynomial coefficients and exact bounds.");
    }
}

internal static class VerifierCalculusV2
{
    public static bool TryReadSolvedPolynomial(MathematicsSolveResult result, string variable, int maxDegree, out IReadOnlyDictionary<int, ExactRational> polynomial, out string error)
    {
        polynomial = new Dictionary<int, ExactRational>();
        if (result.Status != MathematicsSolveStatus.Solved || result.ExactResult is null
            || result.SolutionSet is not FiniteSolutionSet finite || finite.Values.Count != 1)
        {
            error = "Solved derivative must contain ExactResult and a one-value FiniteSolutionSet.";
            return false;
        }
        if (!VerifierPolynomial.TryParse(result.ExactResult, variable, maxDegree, out var exactPolynomial, out error)
            || !VerifierPolynomial.TryParse(finite.Values[0], variable, maxDegree, out var setPolynomial, out error))
        {
            return false;
        }
        if (!PolynomialsEqual(exactPolynomial, setPolynomial))
        {
            error = "Derivative ExactResult and SolutionSet disagree.";
            return false;
        }
        polynomial = exactPolynomial;
        error = string.Empty;
        return true;
    }

    public static bool TryReadSolvedScalar(MathematicsSolveResult result, out ExactRational value, out string error)
    {
        value = default;
        if (result.Status != MathematicsSolveStatus.Solved || result.ExactResult is null
            || !TryReadScalar(result.ExactResult, out value)
            || result.SolutionSet is not FiniteSolutionSet finite || finite.Values.Count != 1
            || !TryReadScalar(finite.Values[0], out var setValue) || setValue != value)
        {
            error = "Solved definite integral must be represented consistently as one exact scalar in ExactResult and SolutionSet.";
            return false;
        }
        error = string.Empty;
        return true;
    }

    public static bool TryReadScalar(MathNode node, out ExactRational value)
    {
        switch (node)
        {
            case IntegerNode integer:
                value = new ExactRational(integer.Value, BigInteger.One);
                return VerifierCalcArithmetic.IsValid(value);
            case RationalNode rational when !rational.Value.Denominator.IsZero:
                value = rational.Value;
                return VerifierCalcArithmetic.IsValid(value);
            default:
                value = default;
                return false;
        }
    }

    public static bool PolynomialsEqual(IReadOnlyDictionary<int, ExactRational> left, IReadOnlyDictionary<int, ExactRational> right)
    {
        var degrees = left.Keys.Concat(right.Keys).Distinct();
        return degrees.All(degree => Get(left, degree) == Get(right, degree));
    }

    private static ExactRational Get(IReadOnlyDictionary<int, ExactRational> polynomial, int degree) =>
        polynomial.TryGetValue(degree, out var value) ? value : VerifierCalcArithmetic.Zero;

    public static MathematicsVerificationResult Verified(string method, string description) =>
        new(MathematicsVerificationStatus.Verified, [new MathematicsVerificationEvidence(method, description)], []);

    public static MathematicsVerificationResult Rejected(string diagnostic) =>
        new(MathematicsVerificationStatus.Rejected, [], [diagnostic]);

    public static MathematicsVerificationResult Unsupported(string diagnostic) =>
        new(MathematicsVerificationStatus.Unsupported, [], [diagnostic]);
}

internal static class VerifierPolynomial
{
    private const int MaxNodes = 128;
    private const int MaxDepth = 24;
    private const int MaxExponent = 32;

    public static bool TryParse(MathNode node, string variable, int maxDegree, out IReadOnlyDictionary<int, ExactRational> polynomial, out string error)
    {
        var remaining = MaxNodes;
        if (TryParseCore(node, variable, maxDegree, 0, ref remaining, out var result, out error))
        {
            polynomial = Clean(result);
            return true;
        }
        polynomial = new Dictionary<int, ExactRational>();
        return false;
    }

    private static bool TryParseCore(MathNode node, string variable, int maxDegree, int depth, ref int remaining, out Dictionary<int, ExactRational> polynomial, out string error)
    {
        polynomial = new Dictionary<int, ExactRational>();
        if (depth > MaxDepth || --remaining < 0)
        {
            error = "Independent calculus verifier exceeded its expression depth/node budget.";
            return false;
        }

        switch (node)
        {
            case IntegerNode integer:
                return Constant(new ExactRational(integer.Value, BigInteger.One), out polynomial, out error);
            case RationalNode rational when !rational.Value.Denominator.IsZero:
                return Constant(rational.Value, out polynomial, out error);
            case RationalNode:
                error = "Independent calculus verifier rejects a rational with zero denominator.";
                return false;
            case SymbolNode symbol when string.Equals(symbol.Name, variable, StringComparison.Ordinal):
                polynomial[1] = VerifierCalcArithmetic.One;
                error = string.Empty;
                return true;
            case SymbolNode symbol:
                error = $"Unexpected symbol {symbol.Name}; calculus verifier is bound to {variable}.";
                return false;
            case NegateNode negate:
                if (!TryParseCore(negate.Operand, variable, maxDegree, depth + 1, ref remaining, out var operand, out error)) return false;
                return TryScale(operand, new ExactRational(-1, 1), out polynomial, out error);
            case AddNode add:
                var sum = new Dictionary<int, ExactRational>();
                foreach (var term in add.Terms)
                {
                    if (!TryParseCore(term, variable, maxDegree, depth + 1, ref remaining, out var next, out error)
                        || !TryAdd(sum, next, out sum, out error)) return false;
                }
                polynomial = sum;
                error = string.Empty;
                return true;
            case MultiplyNode multiply:
                var product = new Dictionary<int, ExactRational> { [0] = VerifierCalcArithmetic.One };
                foreach (var factor in multiply.Factors)
                {
                    if (!TryParseCore(factor, variable, maxDegree, depth + 1, ref remaining, out var next, out error)
                        || !TryMultiply(product, next, maxDegree, out product, out error)) return false;
                }
                polynomial = product;
                error = string.Empty;
                return true;
            case DivideNode divide:
                if (!TryParseCore(divide.Numerator, variable, maxDegree, depth + 1, ref remaining, out var numerator, out error)
                    || !TryParseCore(divide.Denominator, variable, maxDegree, depth + 1, ref remaining, out var denominator, out error)) return false;
                denominator = Clean(denominator);
                if (denominator.Count > 1 || (denominator.Count == 1 && !denominator.ContainsKey(0)))
                {
                    error = "Independent polynomial verification only supports division by an exact scalar constant.";
                    return false;
                }
                var divisor = denominator.TryGetValue(0, out var d) ? d : VerifierCalcArithmetic.Zero;
                if (divisor == VerifierCalcArithmetic.Zero || !VerifierCalcArithmetic.TryDivide(VerifierCalcArithmetic.One, divisor, out var reciprocal))
                {
                    error = "Independent polynomial verification rejects division by zero or an oversized scalar.";
                    return false;
                }
                return TryScale(numerator, reciprocal, out polynomial, out error);
            case PowerNode power when power.Exponent is IntegerNode exponentNode
                                      && exponentNode.Value >= BigInteger.Zero
                                      && exponentNode.Value <= new BigInteger(MaxExponent):
                if (!TryParseCore(power.Base, variable, maxDegree, depth + 1, ref remaining, out var basePolynomial, out error)) return false;
                var exponent = (int)exponentNode.Value;
                var baseDegree = basePolynomial.Count == 0 ? 0 : basePolynomial.Keys.Max();
                if (baseDegree > 0 && (long)baseDegree * exponent > maxDegree)
                {
                    error = $"Independent polynomial degree exceeds the supported limit of {maxDegree}.";
                    return false;
                }
                var powered = new Dictionary<int, ExactRational> { [0] = VerifierCalcArithmetic.One };
                for (var i = 0; i < exponent; i++)
                {
                    if (!TryMultiply(powered, basePolynomial, maxDegree, out powered, out error)) return false;
                }
                polynomial = powered;
                error = string.Empty;
                return true;
            case PowerNode:
                error = $"Independent polynomial verification supports only bounded non-negative integer powers through {MaxExponent}.";
                return false;
            default:
                error = $"Node type {node.GetType().Name} is outside the independent exact polynomial verifier subset.";
                return false;
        }
    }

    private static bool Constant(ExactRational value, out Dictionary<int, ExactRational> polynomial, out string error)
    {
        polynomial = new Dictionary<int, ExactRational>();
        if (!VerifierCalcArithmetic.IsValid(value))
        {
            error = "Independent polynomial verifier rejects a malformed or oversized scalar.";
            return false;
        }
        if (value != VerifierCalcArithmetic.Zero) polynomial[0] = value;
        error = string.Empty;
        return true;
    }

    private static bool TryAdd(IReadOnlyDictionary<int, ExactRational> left, IReadOnlyDictionary<int, ExactRational> right, out Dictionary<int, ExactRational> result, out string error)
    {
        result = new Dictionary<int, ExactRational>();
        foreach (var degree in left.Keys.Concat(right.Keys).Distinct())
        {
            var l = left.TryGetValue(degree, out var lv) ? lv : VerifierCalcArithmetic.Zero;
            var r = right.TryGetValue(degree, out var rv) ? rv : VerifierCalcArithmetic.Zero;
            if (!VerifierCalcArithmetic.TryAdd(l, r, out var value))
            {
                error = "Independent polynomial addition exceeds the exact arithmetic budget.";
                return false;
            }
            if (value != VerifierCalcArithmetic.Zero) result[degree] = value;
        }
        error = string.Empty;
        return true;
    }

    private static bool TryScale(IReadOnlyDictionary<int, ExactRational> source, ExactRational factor, out Dictionary<int, ExactRational> result, out string error)
    {
        result = new Dictionary<int, ExactRational>();
        foreach (var pair in source)
        {
            if (!VerifierCalcArithmetic.TryMultiply(pair.Value, factor, out var value))
            {
                error = "Independent polynomial scaling exceeds the exact arithmetic budget.";
                return false;
            }
            if (value != VerifierCalcArithmetic.Zero) result[pair.Key] = value;
        }
        error = string.Empty;
        return true;
    }

    private static bool TryMultiply(IReadOnlyDictionary<int, ExactRational> left, IReadOnlyDictionary<int, ExactRational> right, int maxDegree, out Dictionary<int, ExactRational> result, out string error)
    {
        result = new Dictionary<int, ExactRational>();
        foreach (var l in left)
        {
            foreach (var r in right)
            {
                var degree = l.Key + r.Key;
                if (degree > maxDegree)
                {
                    error = $"Independent polynomial degree exceeds the supported limit of {maxDegree}.";
                    return false;
                }
                if (!VerifierCalcArithmetic.TryMultiply(l.Value, r.Value, out var product))
                {
                    error = "Independent polynomial multiplication exceeds the exact arithmetic budget.";
                    return false;
                }
                var existing = result.TryGetValue(degree, out var current) ? current : VerifierCalcArithmetic.Zero;
                if (!VerifierCalcArithmetic.TryAdd(existing, product, out var combined))
                {
                    error = "Independent polynomial coefficient accumulation exceeds the exact arithmetic budget.";
                    return false;
                }
                if (combined == VerifierCalcArithmetic.Zero) result.Remove(degree);
                else result[degree] = combined;
            }
        }
        error = string.Empty;
        return true;
    }

    private static Dictionary<int, ExactRational> Clean(IReadOnlyDictionary<int, ExactRational> source) =>
        source.Where(pair => pair.Value != VerifierCalcArithmetic.Zero).ToDictionary(pair => pair.Key, pair => pair.Value);
}

internal static class VerifierCalcArithmetic
{
    private const int MaxBits = 4096;
    private const long MaxIntermediateBits = (MaxBits * 2L) + 1L;
    public static readonly ExactRational Zero = new(BigInteger.Zero, BigInteger.One);
    public static readonly ExactRational One = new(BigInteger.One, BigInteger.One);

    public static bool IsValid(ExactRational value) =>
        !value.Denominator.IsZero && BitLength(value.Numerator) <= MaxBits && BitLength(value.Denominator) <= MaxBits;

    public static bool TryAdd(ExactRational left, ExactRational right, out ExactRational result)
    {
        result = default;
        if (!IsValid(left) || !IsValid(right)) return false;
        var numeratorBits = Math.Max(SafeAdd(BitLength(left.Numerator), BitLength(right.Denominator)), SafeAdd(BitLength(right.Numerator), BitLength(left.Denominator))) + 1;
        var denominatorBits = SafeAdd(BitLength(left.Denominator), BitLength(right.Denominator));
        if (numeratorBits > MaxIntermediateBits || denominatorBits > MaxIntermediateBits) return false;
        result = left + right;
        return IsValid(result);
    }

    public static bool TrySubtract(ExactRational left, ExactRational right, out ExactRational result) =>
        TryAdd(left, new ExactRational(BigInteger.Negate(right.Numerator), right.Denominator), out result);

    public static bool TryMultiply(ExactRational left, ExactRational right, out ExactRational result)
    {
        result = default;
        if (!IsValid(left) || !IsValid(right)) return false;
        if (SafeAdd(BitLength(left.Numerator), BitLength(right.Numerator)) > MaxIntermediateBits
            || SafeAdd(BitLength(left.Denominator), BitLength(right.Denominator)) > MaxIntermediateBits) return false;
        result = left * right;
        return IsValid(result);
    }

    public static bool TryDivide(ExactRational left, ExactRational right, out ExactRational result)
    {
        result = default;
        if (!IsValid(left) || !IsValid(right) || right.Numerator.IsZero) return false;
        if (SafeAdd(BitLength(left.Numerator), BitLength(right.Denominator)) > MaxIntermediateBits
            || SafeAdd(BitLength(left.Denominator), BitLength(right.Numerator)) > MaxIntermediateBits) return false;
        result = left / right;
        return IsValid(result);
    }

    public static bool TryPow(ExactRational value, int exponent, out ExactRational result)
    {
        result = One;
        if (!IsValid(value) || exponent < 0 || exponent > 9) return false;
        for (var i = 0; i < exponent; i++)
            if (!TryMultiply(result, value, out result)) return false;
        return true;
    }

    private static long BitLength(BigInteger value) => value.IsZero ? 0 : BigInteger.Abs(value).GetBitLength();
    private static long SafeAdd(long left, long right) => left > long.MaxValue - right ? long.MaxValue : left + right;
}
