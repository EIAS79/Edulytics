using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Services.Mathematics.Contracts;

namespace Edulytics.Services.Mathematics.Solving;

public sealed class ExactPolynomialDerivativeSolver : IMathematicsSolver
{
    public MathematicsSolveResult Solve(MathematicsSolveRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (request.Problem is not DerivativeNode derivative
            || derivative.Order != 1
            || derivative.Expression is null
            || derivative.Variable is null)
        {
            return ExactCalculusV2.Unsupported(request, "Polynomial differentiation requires a well-formed first-order DerivativeNode.");
        }

        if (!ExactCalculusPolynomial.TryParse(
                derivative.Expression,
                derivative.Variable.Name,
                maxDegree: 8,
                out var polynomial,
                out var parseError,
                out var resourceLimit))
        {
            return resourceLimit
                ? ExactCalculusV2.ResourceLimit(request, parseError)
                : ExactCalculusV2.Unsupported(request, parseError);
        }

        var coefficients = new Dictionary<int, ExactRational>();
        foreach (var pair in polynomial)
        {
            if (pair.Key == 0)
            {
                continue;
            }

            if (!ExactResourceBudget.TryMultiply(
                    pair.Value,
                    new ExactRational(new BigInteger(pair.Key), BigInteger.One),
                    out var coefficient))
            {
                return ExactCalculusV2.ResourceLimit(request, "Polynomial derivative coefficient exceeds the exact arithmetic budget.");
            }
            if (coefficient != ExactResourceBudget.Zero)
            {
                coefficients[pair.Key - 1] = coefficient;
            }
        }

        var result = ExactCalculusV2.ToPolynomialNode(coefficients, derivative.Variable.Name);
        return ExactCalculusV2.Solved(
            request,
            result,
            "exact-polynomial-first-derivative",
            "calculus-polynomial-derivative-exact-v1");
    }
}

public sealed class ExactPolynomialDefiniteIntegralSolver : IMathematicsSolver
{
    public MathematicsSolveResult Solve(MathematicsSolveRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (request.Problem is not IntegralNode integral
            || integral.Integrand is null
            || integral.Variable is null
            || integral.LowerBound is null
            || integral.UpperBound is null)
        {
            return ExactCalculusV2.Unsupported(request, "Polynomial integration in this shadow slice requires a well-formed definite IntegralNode with both bounds.");
        }

        if (!ExactCalculusV2.TryPreflight(integral.LowerBound, out var preflightError, out var resourceLimit)
            || !ExactCalculusV2.TryPreflight(integral.UpperBound, out preflightError, out resourceLimit))
        {
            return resourceLimit
                ? ExactCalculusV2.ResourceLimit(request, preflightError)
                : ExactCalculusV2.Unsupported(request, preflightError);
        }
        if (!ExactCalculusV2.TryReadScalar(integral.LowerBound, out var lower)
            || !ExactCalculusV2.TryReadScalar(integral.UpperBound, out var upper))
        {
            return ExactCalculusV2.Unsupported(request, "Definite integral bounds must be exact integer or rational scalars.");
        }

        if (!ExactCalculusPolynomial.TryParse(
                integral.Integrand,
                integral.Variable.Name,
                maxDegree: 8,
                out var polynomial,
                out var parseError,
                out resourceLimit))
        {
            return resourceLimit
                ? ExactCalculusV2.ResourceLimit(request, parseError)
                : ExactCalculusV2.Unsupported(request, parseError);
        }

        var total = ExactResourceBudget.Zero;
        foreach (var pair in polynomial)
        {
            var antiderivativeDegree = pair.Key + 1;
            var divisor = new ExactRational(new BigInteger(antiderivativeDegree), BigInteger.One);
            if (!ExactResourceBudget.TryDivide(pair.Value, divisor, out var antiderivativeCoefficient)
                || !ExactResourceBudget.TryPow(upper, antiderivativeDegree, out var upperPower)
                || !ExactResourceBudget.TryPow(lower, antiderivativeDegree, out var lowerPower)
                || !ExactCalculusV2.TrySubtract(upperPower, lowerPower, out var powerDifference)
                || !ExactResourceBudget.TryMultiply(antiderivativeCoefficient, powerDifference, out var term)
                || !ExactResourceBudget.TryAdd(total, term, out total))
            {
                return ExactCalculusV2.ResourceLimit(request, "Definite polynomial integration exceeds the exact arithmetic budget.");
            }
        }

        var result = ExactCalculusV2.ToNode(total);
        return ExactCalculusV2.Solved(
            request,
            result,
            "exact-polynomial-definite-integral",
            "calculus-polynomial-integral-exact-v1");
    }
}

/// <summary>
/// Solver-side polynomial parser/normalizer for the Calculus V2 slice. Every
/// coefficient operation is routed through ExactResourceBudget before a potentially
/// large BigInteger result can be retained. This deliberately does not reuse the
/// verifier parser, preserving solver/verifier implementation independence.
/// </summary>
internal static class ExactCalculusPolynomial
{
    private const int MaxNodes = 128;
    private const int MaxDepth = 24;
    private const int MaxExponent = 32;

    public static bool TryParse(
        MathNode node,
        string variable,
        int maxDegree,
        out IReadOnlyDictionary<int, ExactRational> polynomial,
        out string error,
        out bool resourceLimit)
    {
        var remaining = MaxNodes;
        if (TryParseCore(
                node,
                variable,
                maxDegree,
                depth: 0,
                ref remaining,
                out var result,
                out error,
                out resourceLimit))
        {
            polynomial = Clean(result);
            return true;
        }

        polynomial = new Dictionary<int, ExactRational>();
        return false;
    }

    private static bool TryParseCore(
        MathNode node,
        string variable,
        int maxDegree,
        int depth,
        ref int remaining,
        out Dictionary<int, ExactRational> polynomial,
        out string error,
        out bool resourceLimit)
    {
        polynomial = new Dictionary<int, ExactRational>();
        if (depth > MaxDepth || --remaining < 0)
        {
            error = "Calculus polynomial normalization exceeds the supported depth/node budget.";
            resourceLimit = true;
            return false;
        }

        switch (node)
        {
            case IntegerNode integer:
                return Constant(new ExactRational(integer.Value, BigInteger.One), out polynomial, out error, out resourceLimit);

            case RationalNode rational when !rational.Value.Denominator.IsZero:
                return Constant(rational.Value, out polynomial, out error, out resourceLimit);

            case RationalNode:
                error = "Calculus polynomial contains a malformed rational with zero denominator.";
                resourceLimit = false;
                return false;

            case SymbolNode symbol when string.Equals(symbol.Name, variable, StringComparison.Ordinal):
                polynomial[1] = ExactResourceBudget.One;
                error = string.Empty;
                resourceLimit = false;
                return true;

            case SymbolNode symbol:
                error = $"Unexpected symbol {symbol.Name}; calculus polynomial is bound to {variable}.";
                resourceLimit = false;
                return false;

            case NegateNode negate:
                if (!TryParseCore(
                        negate.Operand,
                        variable,
                        maxDegree,
                        depth + 1,
                        ref remaining,
                        out var operand,
                        out error,
                        out resourceLimit))
                {
                    return false;
                }
                return TryScale(
                    operand,
                    new ExactRational(-1, 1),
                    out polynomial,
                    out error,
                    out resourceLimit);

            case AddNode add:
                var sum = new Dictionary<int, ExactRational>();
                foreach (var term in add.Terms)
                {
                    if (!TryParseCore(
                            term,
                            variable,
                            maxDegree,
                            depth + 1,
                            ref remaining,
                            out var next,
                            out error,
                            out resourceLimit)
                        || !TryAdd(sum, next, out sum, out error, out resourceLimit))
                    {
                        return false;
                    }
                }
                polynomial = sum;
                error = string.Empty;
                resourceLimit = false;
                return true;

            case MultiplyNode multiply:
                var product = new Dictionary<int, ExactRational> { [0] = ExactResourceBudget.One };
                foreach (var factor in multiply.Factors)
                {
                    if (!TryParseCore(
                            factor,
                            variable,
                            maxDegree,
                            depth + 1,
                            ref remaining,
                            out var next,
                            out error,
                            out resourceLimit)
                        || !TryMultiply(product, next, maxDegree, out product, out error, out resourceLimit))
                    {
                        return false;
                    }
                }
                polynomial = product;
                error = string.Empty;
                resourceLimit = false;
                return true;

            case DivideNode divide:
                if (!TryParseCore(
                        divide.Numerator,
                        variable,
                        maxDegree,
                        depth + 1,
                        ref remaining,
                        out var numerator,
                        out error,
                        out resourceLimit)
                    || !TryParseCore(
                        divide.Denominator,
                        variable,
                        maxDegree,
                        depth + 1,
                        ref remaining,
                        out var denominator,
                        out error,
                        out resourceLimit))
                {
                    return false;
                }

                denominator = Clean(denominator);
                if (denominator.Count > 1 || (denominator.Count == 1 && !denominator.ContainsKey(0)))
                {
                    error = "Calculus polynomial division is supported only by an exact scalar constant.";
                    resourceLimit = false;
                    return false;
                }

                var divisor = denominator.TryGetValue(0, out var d) ? d : ExactResourceBudget.Zero;
                if (divisor == ExactResourceBudget.Zero)
                {
                    error = "Calculus polynomial division by zero is unsupported.";
                    resourceLimit = false;
                    return false;
                }
                if (!ExactResourceBudget.TryDivide(ExactResourceBudget.One, divisor, out var reciprocal))
                {
                    error = "Calculus polynomial scalar division exceeds the exact arithmetic budget.";
                    resourceLimit = true;
                    return false;
                }
                return TryScale(numerator, reciprocal, out polynomial, out error, out resourceLimit);

            case PowerNode power when power.Exponent is IntegerNode exponentNode
                                      && exponentNode.Value >= BigInteger.Zero
                                      && exponentNode.Value <= new BigInteger(MaxExponent):
                if (!TryParseCore(
                        power.Base,
                        variable,
                        maxDegree,
                        depth + 1,
                        ref remaining,
                        out var basePolynomial,
                        out error,
                        out resourceLimit))
                {
                    return false;
                }

                var exponent = (int)exponentNode.Value;
                var baseDegree = basePolynomial.Count == 0 ? 0 : basePolynomial.Keys.Max();
                if (baseDegree > 0 && (long)baseDegree * exponent > maxDegree)
                {
                    error = $"Calculus polynomial degree exceeds the supported limit of {maxDegree}.";
                    resourceLimit = false;
                    return false;
                }

                var powered = new Dictionary<int, ExactRational> { [0] = ExactResourceBudget.One };
                for (var i = 0; i < exponent; i++)
                {
                    if (!TryMultiply(
                            powered,
                            basePolynomial,
                            maxDegree,
                            out powered,
                            out error,
                            out resourceLimit))
                    {
                        return false;
                    }
                }
                polynomial = powered;
                error = string.Empty;
                resourceLimit = false;
                return true;

            case PowerNode:
                error = $"Calculus polynomial powers must be integer constants from 0 through {MaxExponent}.";
                resourceLimit = false;
                return false;

            default:
                error = $"Node type {node.GetType().Name} is outside the exact calculus polynomial subset.";
                resourceLimit = false;
                return false;
        }
    }

    private static bool Constant(
        ExactRational value,
        out Dictionary<int, ExactRational> polynomial,
        out string error,
        out bool resourceLimit)
    {
        polynomial = new Dictionary<int, ExactRational>();
        if (value.Denominator.IsZero)
        {
            error = "Calculus polynomial contains a malformed rational scalar.";
            resourceLimit = false;
            return false;
        }
        if (!ExactResourceBudget.IsScalarWithinLimit(value))
        {
            error = "Calculus polynomial scalar exceeds the supported 4096-bit budget.";
            resourceLimit = true;
            return false;
        }
        if (value != ExactResourceBudget.Zero)
        {
            polynomial[0] = value;
        }
        error = string.Empty;
        resourceLimit = false;
        return true;
    }

    private static bool TryAdd(
        IReadOnlyDictionary<int, ExactRational> left,
        IReadOnlyDictionary<int, ExactRational> right,
        out Dictionary<int, ExactRational> result,
        out string error,
        out bool resourceLimit)
    {
        result = new Dictionary<int, ExactRational>();
        foreach (var degree in left.Keys.Concat(right.Keys).Distinct())
        {
            var l = left.TryGetValue(degree, out var lv) ? lv : ExactResourceBudget.Zero;
            var r = right.TryGetValue(degree, out var rv) ? rv : ExactResourceBudget.Zero;
            if (!ExactResourceBudget.TryAdd(l, r, out var value))
            {
                error = "Calculus polynomial addition exceeds the exact arithmetic budget.";
                resourceLimit = true;
                return false;
            }
            if (value != ExactResourceBudget.Zero)
            {
                result[degree] = value;
            }
        }
        error = string.Empty;
        resourceLimit = false;
        return true;
    }

    private static bool TryScale(
        IReadOnlyDictionary<int, ExactRational> source,
        ExactRational factor,
        out Dictionary<int, ExactRational> result,
        out string error,
        out bool resourceLimit)
    {
        result = new Dictionary<int, ExactRational>();
        foreach (var pair in source)
        {
            if (!ExactResourceBudget.TryMultiply(pair.Value, factor, out var value))
            {
                error = "Calculus polynomial scaling exceeds the exact arithmetic budget.";
                resourceLimit = true;
                return false;
            }
            if (value != ExactResourceBudget.Zero)
            {
                result[pair.Key] = value;
            }
        }
        error = string.Empty;
        resourceLimit = false;
        return true;
    }

    private static bool TryMultiply(
        IReadOnlyDictionary<int, ExactRational> left,
        IReadOnlyDictionary<int, ExactRational> right,
        int maxDegree,
        out Dictionary<int, ExactRational> result,
        out string error,
        out bool resourceLimit)
    {
        result = new Dictionary<int, ExactRational>();
        foreach (var l in left)
        {
            foreach (var r in right)
            {
                var degree = l.Key + r.Key;
                if (degree > maxDegree)
                {
                    error = $"Calculus polynomial degree exceeds the supported limit of {maxDegree}.";
                    resourceLimit = false;
                    return false;
                }

                if (!ExactResourceBudget.TryMultiply(l.Value, r.Value, out var coefficientProduct))
                {
                    error = "Calculus polynomial multiplication exceeds the exact arithmetic budget.";
                    resourceLimit = true;
                    return false;
                }

                var existing = result.TryGetValue(degree, out var current)
                    ? current
                    : ExactResourceBudget.Zero;
                if (!ExactResourceBudget.TryAdd(existing, coefficientProduct, out var combined))
                {
                    error = "Calculus polynomial coefficient accumulation exceeds the exact arithmetic budget.";
                    resourceLimit = true;
                    return false;
                }

                if (combined == ExactResourceBudget.Zero)
                {
                    result.Remove(degree);
                }
                else
                {
                    result[degree] = combined;
                }
            }
        }
        error = string.Empty;
        resourceLimit = false;
        return true;
    }

    private static Dictionary<int, ExactRational> Clean(IReadOnlyDictionary<int, ExactRational> source) =>
        source
            .Where(pair => pair.Value != ExactResourceBudget.Zero)
            .ToDictionary(pair => pair.Key, pair => pair.Value);
}

internal static class ExactCalculusV2
{
    private const int MaxNodeCount = 128;
    private const int MaxDepth = 24;

    public static bool TryPreflight(MathNode node, out string error, out bool resourceLimit)
    {
        var remaining = MaxNodeCount;
        return TryPreflightCore(node, 0, ref remaining, out error, out resourceLimit);
    }

    private static bool TryPreflightCore(MathNode node, int depth, ref int remaining, out string error, out bool resourceLimit)
    {
        if (depth > MaxDepth || --remaining < 0)
        {
            error = "Calculus expression exceeds the supported depth/node budget.";
            resourceLimit = true;
            return false;
        }

        switch (node)
        {
            case IntegerNode integer:
                if (BitLength(integer.Value) > ExactResourceBudget.MaxScalarBitLength)
                {
                    error = "Calculus integer scalar exceeds the supported 4096-bit budget.";
                    resourceLimit = true;
                    return false;
                }
                break;
            case RationalNode rational:
                if (rational.Value.Denominator.IsZero)
                {
                    error = "Calculus input contains a malformed rational with zero denominator.";
                    resourceLimit = false;
                    return false;
                }
                if (!ExactResourceBudget.IsScalarWithinLimit(rational.Value))
                {
                    error = "Calculus rational scalar exceeds the supported 4096-bit budget.";
                    resourceLimit = true;
                    return false;
                }
                break;
            case NegateNode negate:
                return TryPreflightCore(negate.Operand, depth + 1, ref remaining, out error, out resourceLimit);
            case AddNode add:
                foreach (var term in add.Terms)
                    if (!TryPreflightCore(term, depth + 1, ref remaining, out error, out resourceLimit)) return false;
                break;
            case MultiplyNode multiply:
                foreach (var factor in multiply.Factors)
                    if (!TryPreflightCore(factor, depth + 1, ref remaining, out error, out resourceLimit)) return false;
                break;
            case DivideNode divide:
                if (!TryPreflightCore(divide.Numerator, depth + 1, ref remaining, out error, out resourceLimit)
                    || !TryPreflightCore(divide.Denominator, depth + 1, ref remaining, out error, out resourceLimit)) return false;
                break;
            case PowerNode power:
                if (!TryPreflightCore(power.Base, depth + 1, ref remaining, out error, out resourceLimit)
                    || !TryPreflightCore(power.Exponent, depth + 1, ref remaining, out error, out resourceLimit)) return false;
                break;
            case RootNode root:
                return TryPreflightCore(root.Radicand, depth + 1, ref remaining, out error, out resourceLimit);
            case EquationNode equation:
                if (!TryPreflightCore(equation.Left, depth + 1, ref remaining, out error, out resourceLimit)
                    || !TryPreflightCore(equation.Right, depth + 1, ref remaining, out error, out resourceLimit)) return false;
                break;
            case InequalityNode inequality:
                if (!TryPreflightCore(inequality.Left, depth + 1, ref remaining, out error, out resourceLimit)
                    || !TryPreflightCore(inequality.Right, depth + 1, ref remaining, out error, out resourceLimit)) return false;
                break;
            case FunctionCallNode function:
                foreach (var argument in function.Arguments)
                    if (!TryPreflightCore(argument, depth + 1, ref remaining, out error, out resourceLimit)) return false;
                break;
            case VectorNode vector:
                foreach (var component in vector.Components)
                    if (!TryPreflightCore(component, depth + 1, ref remaining, out error, out resourceLimit)) return false;
                break;
            case MatrixNode matrix:
                foreach (var row in matrix.Rows)
                    foreach (var cell in row)
                        if (!TryPreflightCore(cell, depth + 1, ref remaining, out error, out resourceLimit)) return false;
                break;
            case DerivativeNode derivative:
                return TryPreflightCore(derivative.Expression, depth + 1, ref remaining, out error, out resourceLimit);
            case IntegralNode integral:
                if (!TryPreflightCore(integral.Integrand, depth + 1, ref remaining, out error, out resourceLimit)) return false;
                if (integral.LowerBound is not null && !TryPreflightCore(integral.LowerBound, depth + 1, ref remaining, out error, out resourceLimit)) return false;
                if (integral.UpperBound is not null && !TryPreflightCore(integral.UpperBound, depth + 1, ref remaining, out error, out resourceLimit)) return false;
                break;
        }

        error = string.Empty;
        resourceLimit = false;
        return true;
    }

    public static bool TryReadScalar(MathNode node, out ExactRational value)
    {
        switch (node)
        {
            case IntegerNode integer:
                value = new ExactRational(integer.Value, BigInteger.One);
                return ExactResourceBudget.IsScalarWithinLimit(value);
            case RationalNode rational when !rational.Value.Denominator.IsZero:
                value = rational.Value;
                return ExactResourceBudget.IsScalarWithinLimit(value);
            default:
                value = default;
                return false;
        }
    }

    public static bool TrySubtract(ExactRational left, ExactRational right, out ExactRational result)
    {
        var negativeRight = new ExactRational(BigInteger.Negate(right.Numerator), right.Denominator);
        return ExactResourceBudget.TryAdd(left, negativeRight, out result);
    }

    public static MathNode ToPolynomialNode(IReadOnlyDictionary<int, ExactRational> coefficients, string variable)
    {
        var terms = new List<MathNode>();
        foreach (var pair in coefficients.Where(p => p.Value != ExactResourceBudget.Zero).OrderByDescending(p => p.Key))
        {
            if (pair.Key == 0)
            {
                terms.Add(ToNode(pair.Value));
                continue;
            }

            MathNode power = pair.Key == 1
                ? new SymbolNode(variable)
                : new PowerNode(new SymbolNode(variable), new IntegerNode(new BigInteger(pair.Key)));

            if (pair.Value == ExactResourceBudget.One)
                terms.Add(power);
            else if (pair.Value == new ExactRational(-1, 1))
                terms.Add(new NegateNode(power));
            else
                terms.Add(new MultiplyNode([ToNode(pair.Value), power]));
        }
        return terms.Count switch
        {
            0 => new IntegerNode(BigInteger.Zero),
            1 => terms[0],
            _ => new AddNode(terms)
        };
    }

    public static MathNode ToNode(ExactRational value) =>
        value.Denominator == BigInteger.One ? new IntegerNode(value.Numerator) : new RationalNode(value);

    public static MathematicsSolveResult Solved(MathematicsSolveRequest request, MathNode result, string strategy, string version) =>
        new(
            MathematicsSolveStatus.Solved,
            result,
            new FiniteSolutionSet([result]),
            request.Assumptions,
            strategy,
            new MathematicsSolutionTrace([]),
            "edulytics-native-calculus",
            version,
            []);

    public static MathematicsSolveResult Unsupported(MathematicsSolveRequest request, string diagnostic) =>
        new(MathematicsSolveStatus.Unsupported, null, null, request.Assumptions, null, null, "edulytics-native-calculus", "calculus-exact-v1", [diagnostic]);

    public static MathematicsSolveResult ResourceLimit(MathematicsSolveRequest request, string diagnostic) =>
        new(MathematicsSolveStatus.ResourceLimit, null, null, request.Assumptions, null, null, "edulytics-native-calculus", "calculus-exact-v1", [diagnostic]);

    private static long BitLength(BigInteger value) => value.IsZero ? 0 : BigInteger.Abs(value).GetBitLength();
}
