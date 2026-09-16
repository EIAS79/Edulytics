using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Core.Mathematics.Verification;
using Edulytics.Services.Mathematics.Contracts;

namespace Edulytics.Services.Mathematics.Verification;

public sealed class ExactFunctionEvaluationVerifier : IMathematicsVerifier
{
    public MathematicsVerificationResult Verify(MathematicsSolveRequest request, MathematicsSolveResult result)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(result);

        if (!VerifierFunctionsV2.TryParseEvaluationRequest(request.Problem, out var expression, out var variable, out var input, out var error))
        {
            return VerifierFunctionsV2.Unsupported(error);
        }
        if (!VerifierExactFunctionEvaluator.TryEvaluate(expression!, variable!, input, out var expected, out error))
        {
            return VerifierFunctionsV2.Unsupported(error);
        }
        if (!VerifierFunctionsV2.TryReadSolvedScalar(result, out var reported, out error))
        {
            return VerifierFunctionsV2.Rejected(error);
        }
        if (reported != expected)
        {
            return VerifierFunctionsV2.Rejected("Reported function value does not equal an independent exact evaluation of the original expression.");
        }

        return VerifierFunctionsV2.Verified("independent-exact-function-evaluation", "The reported value exactly matches independent evaluation of the original function expression at the requested input.");
    }
}

public sealed class ExactGraphSamplingVerifier : IMathematicsVerifier
{
    public MathematicsVerificationResult Verify(MathematicsSolveRequest request, MathematicsSolveResult result)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(result);

        if (!VerifierFunctionsV2.TryParseGraphRequest(request.Problem, out var expression, out var variable, out var inputs, out var error))
        {
            return VerifierFunctionsV2.Unsupported(error);
        }
        if (result.Status != MathematicsSolveStatus.Solved
            || result.ExactResult is not VectorNode exactVector
            || result.SolutionSet is not FiniteSolutionSet finite
            || exactVector.Components.Count != inputs!.Count
            || finite.Values.Count != inputs.Count)
        {
            return VerifierFunctionsV2.Rejected("Graph result must contain exactly one ordered pair for every requested x-value in both ExactResult and SolutionSet.");
        }

        for (var i = 0; i < inputs.Count; i++)
        {
            if (!VerifierExactFunctionEvaluator.TryEvaluate(expression!, variable!, inputs[i], out var expectedY, out error))
            {
                return VerifierFunctionsV2.Unsupported(error);
            }
            if (!TryReadPoint(exactVector.Components[i], out var x, out var y)
                || x != inputs[i]
                || y != expectedY)
            {
                return VerifierFunctionsV2.Rejected($"Graph point {i + 1} does not exactly match the requested x-value and independently evaluated y-value.");
            }
            if (!TryReadPoint(finite.Values[i], out var setX, out var setY)
                || setX != x
                || setY != y)
            {
                return VerifierFunctionsV2.Rejected("Graph ExactResult and finite SolutionSet are inconsistent.");
            }
        }

        return VerifierFunctionsV2.Verified("independent-exact-graph-evaluation", "Every reported graph coordinate exactly matches independent evaluation of the original function expression.");
    }

    private static bool TryReadPoint(MathNode node, out ExactRational x, out ExactRational y)
    {
        x = default;
        y = default;
        return node is VectorNode point
            && point.Components.Count == 2
            && VerifierFunctionsV2.TryReadScalar(point.Components[0], out x)
            && VerifierFunctionsV2.TryReadScalar(point.Components[1], out y);
    }
}

public sealed class ExactSequenceTermVerifier : IMathematicsVerifier
{
    private static readonly ExactRational One = new(BigInteger.One, BigInteger.One);

    public MathematicsVerificationResult Verify(MathematicsSolveRequest request, MathematicsSolveResult result)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(result);

        if (!VerifierFunctionsV2.TryParseSequenceRequest(request.Problem, out var kind, out var first, out var stepOrRatio, out var n, out var error))
        {
            return VerifierFunctionsV2.Unsupported(error);
        }

        ExactRational expected;
        string method;
        if (kind == "arithmetic_nth_term")
        {
            if (n > 10000)
            {
                return VerifierFunctionsV2.Unsupported("Arithmetic sequence index exceeds the verified bound of 10000.");
            }
            expected = first + new ExactRational(n - BigInteger.One, BigInteger.One) * stepOrRatio;
            method = "independent-arithmetic-closed-form";
        }
        else
        {
            if (n > 64)
            {
                return VerifierFunctionsV2.Unsupported("Geometric sequence index exceeds the verified exact-power bound of 64.");
            }
            expected = first * Pow(stepOrRatio, (int)(n - BigInteger.One));
            method = "independent-geometric-closed-form";
        }

        if (!VerifierFunctionsV2.TryReadSolvedScalar(result, out var reported, out error))
        {
            return VerifierFunctionsV2.Rejected(error);
        }
        if (reported != expected)
        {
            return VerifierFunctionsV2.Rejected("Reported sequence term does not equal the independently derived exact nth term.");
        }

        return VerifierFunctionsV2.Verified(method, "The reported sequence term matches an independent exact closed-form calculation from the original parameters.");
    }

    private static ExactRational Pow(ExactRational value, int exponent)
    {
        var result = One;
        for (var i = 0; i < exponent; i++)
        {
            result *= value;
        }
        return result;
    }
}

internal static class VerifierExactFunctionEvaluator
{
    private const int MaxExponent = 12;
    private static readonly ExactRational Zero = new(BigInteger.Zero, BigInteger.One);
    private static readonly ExactRational One = new(BigInteger.One, BigInteger.One);

    public static bool TryEvaluate(MathNode node, string variable, ExactRational input, out ExactRational value, out string error)
    {
        switch (node)
        {
            case IntegerNode integer:
                value = new ExactRational(integer.Value, BigInteger.One);
                error = string.Empty;
                return true;
            case RationalNode rational:
                value = rational.Value;
                error = string.Empty;
                return true;
            case SymbolNode symbol when string.Equals(symbol.Name, variable, StringComparison.Ordinal):
                value = input;
                error = string.Empty;
                return true;
            case SymbolNode symbol:
                value = default;
                error = $"Unexpected symbol {symbol.Name} in exact verification.";
                return false;
            case NegateNode negate:
                if (!TryEvaluate(negate.Operand, variable, input, out var operand, out error))
                {
                    value = default;
                    return false;
                }
                value = new ExactRational(-operand.Numerator, operand.Denominator);
                return true;
            case AddNode add:
                var sum = Zero;
                foreach (var term in add.Terms)
                {
                    if (!TryEvaluate(term, variable, input, out var termValue, out error))
                    {
                        value = default;
                        return false;
                    }
                    sum += termValue;
                }
                value = sum;
                error = string.Empty;
                return true;
            case MultiplyNode multiply:
                var product = One;
                foreach (var factor in multiply.Factors)
                {
                    if (!TryEvaluate(factor, variable, input, out var factorValue, out error))
                    {
                        value = default;
                        return false;
                    }
                    product *= factorValue;
                }
                value = product;
                error = string.Empty;
                return true;
            case DivideNode divide:
                if (!TryEvaluate(divide.Numerator, variable, input, out var numerator, out error)
                    || !TryEvaluate(divide.Denominator, variable, input, out var denominator, out error))
                {
                    value = default;
                    return false;
                }
                if (denominator == Zero)
                {
                    value = default;
                    error = "Independent exact verification encountered division by zero.";
                    return false;
                }
                value = numerator / denominator;
                error = string.Empty;
                return true;
            case PowerNode power:
                if (power.Exponent is not IntegerNode exponentNode
                    || exponentNode.Value < BigInteger.Zero
                    || exponentNode.Value > MaxExponent)
                {
                    value = default;
                    error = $"Independent exact verification supports integer exponents from 0 through {MaxExponent}.";
                    return false;
                }
                if (!TryEvaluate(power.Base, variable, input, out var baseValue, out error))
                {
                    value = default;
                    return false;
                }
                value = Pow(baseValue, (int)exponentNode.Value);
                error = string.Empty;
                return true;
            default:
                value = default;
                error = $"Node type {node.GetType().Name} is outside the independently verified exact rational function subset.";
                return false;
        }
    }

    private static ExactRational Pow(ExactRational value, int exponent)
    {
        var result = One;
        for (var i = 0; i < exponent; i++)
        {
            result *= value;
        }
        return result;
    }
}

internal static class VerifierFunctionsV2
{
    public static bool TryParseEvaluationRequest(MathNode problem, out MathNode? expression, out string? variable, out ExactRational input, out string error)
    {
        expression = null;
        variable = null;
        input = default;
        if (problem is not FunctionCallNode call
            || call.FunctionName != "evaluate_exact"
            || call.Arguments.Count != 3
            || call.Arguments[1] is not SymbolNode symbol
            || !TryReadScalar(call.Arguments[2], out input))
        {
            error = "Verifier requires evaluate_exact(expression, variable, exact rational input).";
            return false;
        }
        expression = call.Arguments[0];
        variable = symbol.Name;
        error = string.Empty;
        return true;
    }

    public static bool TryParseGraphRequest(MathNode problem, out MathNode? expression, out string? variable, out IReadOnlyList<ExactRational>? inputs, out string error)
    {
        expression = null;
        variable = null;
        inputs = null;
        if (problem is not FunctionCallNode call
            || call.FunctionName != "sample_graph_exact"
            || call.Arguments.Count != 3
            || call.Arguments[1] is not SymbolNode symbol
            || call.Arguments[2] is not VectorNode inputVector
            || inputVector.Components.Count is < 1 or > 7)
        {
            error = "Verifier requires sample_graph_exact(expression, variable, vector of 1..7 exact rational x-values).";
            return false;
        }

        var values = new List<ExactRational>(inputVector.Components.Count);
        foreach (var node in inputVector.Components)
        {
            if (!TryReadScalar(node, out var scalar))
            {
                error = "Every graph x-value must be exact.";
                return false;
            }
            if (values.Contains(scalar))
            {
                error = "Duplicate graph x-values are not verified.";
                return false;
            }
            values.Add(scalar);
        }
        expression = call.Arguments[0];
        variable = symbol.Name;
        inputs = values;
        error = string.Empty;
        return true;
    }

    public static bool TryParseSequenceRequest(MathNode problem, out string kind, out ExactRational first, out ExactRational stepOrRatio, out BigInteger n, out string error)
    {
        kind = string.Empty;
        first = default;
        stepOrRatio = default;
        n = default;
        if (problem is not FunctionCallNode call
            || (call.FunctionName != "arithmetic_nth_term" && call.FunctionName != "geometric_nth_term")
            || call.Arguments.Count != 3
            || !TryReadScalar(call.Arguments[0], out first)
            || !TryReadScalar(call.Arguments[1], out stepOrRatio)
            || call.Arguments[2] is not IntegerNode index
            || index.Value <= BigInteger.Zero)
        {
            error = "Verifier requires an exact arithmetic/geometric sequence request with positive integer n.";
            return false;
        }
        kind = call.FunctionName;
        n = index.Value;
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
            error = "Solved scalar result must contain one exact value consistently in ExactResult and FiniteSolutionSet.";
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
