using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Services.Mathematics.Contracts;

namespace Edulytics.Services.Mathematics.Solving;

public sealed class ExactPolynomialDerivativeSolver : IMathematicsSolver
{
    private readonly ExactPolynomialNormalizer normalizer = new();

    public MathematicsSolveResult Solve(MathematicsSolveRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (request.Problem is not DerivativeNode derivative || derivative.Order != 1)
        {
            return ExactCalculusV2.Unsupported(request, "Polynomial differentiation requires a first-order DerivativeNode.");
        }

        if (!ExactCalculusV2.TryPreflight(derivative.Expression, out var preflightError, out var resourceLimit))
        {
            return resourceLimit
                ? ExactCalculusV2.ResourceLimit(request, preflightError)
                : ExactCalculusV2.Unsupported(request, preflightError);
        }

        var normalized = normalizer.Normalize(derivative.Expression, derivative.Variable.Name, maxDegree: 8);
        if (!normalized.IsSupported)
        {
            return ExactCalculusV2.Unsupported(request, string.Join("; ", normalized.Diagnostics));
        }
        if (!ExactCalculusV2.CoefficientsWithinBudget(normalized.Coefficients))
        {
            return ExactCalculusV2.ResourceLimit(request, "Normalized polynomial coefficients exceed the exact scalar budget.");
        }

        var coefficients = new Dictionary<int, ExactRational>();
        foreach (var pair in normalized.Coefficients)
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
    private readonly ExactPolynomialNormalizer normalizer = new();

    public MathematicsSolveResult Solve(MathematicsSolveRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (request.Problem is not IntegralNode integral
            || integral.LowerBound is null
            || integral.UpperBound is null)
        {
            return ExactCalculusV2.Unsupported(request, "Polynomial integration in this shadow slice requires both definite bounds.");
        }

        if (!ExactCalculusV2.TryPreflight(integral.Integrand, out var preflightError, out var resourceLimit)
            || !ExactCalculusV2.TryPreflight(integral.LowerBound, out preflightError, out resourceLimit)
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

        var normalized = normalizer.Normalize(integral.Integrand, integral.Variable.Name, maxDegree: 8);
        if (!normalized.IsSupported)
        {
            return ExactCalculusV2.Unsupported(request, string.Join("; ", normalized.Diagnostics));
        }
        if (!ExactCalculusV2.CoefficientsWithinBudget(normalized.Coefficients))
        {
            return ExactCalculusV2.ResourceLimit(request, "Normalized polynomial coefficients exceed the exact scalar budget.");
        }

        var total = ExactResourceBudget.Zero;
        foreach (var pair in normalized.Coefficients)
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

    public static bool CoefficientsWithinBudget(IReadOnlyDictionary<int, ExactRational> coefficients) =>
        coefficients.Values.All(value => !value.Denominator.IsZero && ExactResourceBudget.IsScalarWithinLimit(value));

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
