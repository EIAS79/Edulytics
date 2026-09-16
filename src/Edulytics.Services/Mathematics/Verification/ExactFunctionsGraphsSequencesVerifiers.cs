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
    public MathematicsVerificationResult Verify(MathematicsSolveRequest request, MathematicsSolveResult result)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(result);

        if (!VerifierFunctionsV2.TryParseSequenceRequest(request.Problem, out var kind, out var first, out var stepOrRatio, out var n, out var error))
        {
            return VerifierFunctionsV2.Unsupported(error);
        }
        if (!VerifierResourceBudget.IsScalarWithinLimit(first) || !VerifierResourceBudget.IsScalarWithinLimit(stepOrRatio))
        {
            return VerifierFunctionsV2.Unsupported("Sequence parameters exceed the independent verifier exact scalar bit-length budget.");
        }

        ExactRational expected;
        string method;
        if (kind == "arithmetic_nth_term")
        {
            if (n > 10000)
            {
                return VerifierFunctionsV2.Unsupported("Arithmetic sequence index exceeds the verified bound of 10000.");
            }
            var indexFactor = new ExactRational(n - BigInteger.One, BigInteger.One);
            if (!VerifierResourceBudget.TryMultiply(stepOrRatio, indexFactor, out var delta)
                || !VerifierResourceBudget.TryAdd(first, delta, out expected))
            {
                return VerifierFunctionsV2.Unsupported("Arithmetic nth-term verification exceeds the exact result bit-length budget.");
            }
            method = "independent-arithmetic-closed-form";
        }
        else
        {
            if (n > 64)
            {
                return VerifierFunctionsV2.Unsupported("Geometric sequence index exceeds the verified exact-power bound of 64.");
            }
            if (!VerifierResourceBudget.TryPow(stepOrRatio, (int)(n - BigInteger.One), out var ratioPower)
                || !VerifierResourceBudget.TryMultiply(first, ratioPower, out expected))
            {
                return VerifierFunctionsV2.Unsupported("Geometric nth-term verification exceeds the exact result bit-length budget.");
            }
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
}

internal static class VerifierExactFunctionEvaluator
{
    private const int MaxExponent = 12;
    private const int MaxNodeCount = 128;
    private const int MaxDepth = 24;

    public static bool TryEvaluate(MathNode node, string variable, ExactRational input, out ExactRational value, out string error)
    {
        if (!VerifierResourceBudget.IsScalarWithinLimit(input))
        {
            value = default;
            error = "Independent verification input exceeds the exact scalar bit-length budget.";
            return false;
        }

        var remainingNodes = MaxNodeCount;
        return TryEvaluateCore(node, variable, input, depth: 0, ref remainingNodes, out value, out error);
    }

    private static bool TryEvaluateCore(
        MathNode node,
        string variable,
        ExactRational input,
        int depth,
        ref int remainingNodes,
        out ExactRational value,
        out string error)
    {
        if (depth > MaxDepth || --remainingNodes < 0)
        {
            value = default;
            error = "Independent exact verification exceeds the expression depth/node budget.";
            return false;
        }

        switch (node)
        {
            case IntegerNode integer:
                value = new ExactRational(integer.Value, BigInteger.One);
                return FinishScalar(value, out error);
            case RationalNode rational:
                value = rational.Value;
                return FinishScalar(value, out error);
            case SymbolNode symbol when string.Equals(symbol.Name, variable, StringComparison.Ordinal):
                value = input;
                error = string.Empty;
                return true;
            case SymbolNode symbol:
                value = default;
                error = $"Unexpected symbol {symbol.Name} in exact verification.";
                return false;
            case NegateNode negate:
                if (!TryEvaluateCore(negate.Operand, variable, input, depth + 1, ref remainingNodes, out var operand, out error))
                {
                    value = default;
                    return false;
                }
                value = new ExactRational(-operand.Numerator, operand.Denominator);
                return FinishScalar(value, out error);
            case AddNode add:
                var sum = VerifierResourceBudget.Zero;
                foreach (var term in add.Terms)
                {
                    if (!TryEvaluateCore(term, variable, input, depth + 1, ref remainingNodes, out var termValue, out error))
                    {
                        value = default;
                        return false;
                    }
                    if (!VerifierResourceBudget.TryAdd(sum, termValue, out sum))
                    {
                        value = default;
                        error = "Independent exact addition exceeds the result bit-length budget.";
                        return false;
                    }
                }
                value = sum;
                error = string.Empty;
                return true;
            case MultiplyNode multiply:
                var product = VerifierResourceBudget.One;
                foreach (var factor in multiply.Factors)
                {
                    if (!TryEvaluateCore(factor, variable, input, depth + 1, ref remainingNodes, out var factorValue, out error))
                    {
                        value = default;
                        return false;
                    }
                    if (!VerifierResourceBudget.TryMultiply(product, factorValue, out product))
                    {
                        value = default;
                        error = "Independent exact multiplication exceeds the result bit-length budget.";
                        return false;
                    }
                }
                value = product;
                error = string.Empty;
                return true;
            case DivideNode divide:
                if (!TryEvaluateCore(divide.Numerator, variable, input, depth + 1, ref remainingNodes, out var numerator, out error)
                    || !TryEvaluateCore(divide.Denominator, variable, input, depth + 1, ref remainingNodes, out var denominator, out error))
                {
                    value = default;
                    return false;
                }
                if (denominator == VerifierResourceBudget.Zero)
                {
                    value = default;
                    error = "Independent exact verification encountered division by zero.";
                    return false;
                }
                if (!VerifierResourceBudget.TryDivide(numerator, denominator, out value))
                {
                    error = "Independent exact division exceeds the result bit-length budget.";
                    return false;
                }
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
                if (!TryEvaluateCore(power.Base, variable, input, depth + 1, ref remainingNodes, out var baseValue, out error))
                {
                    value = default;
                    return false;
                }
                if (!VerifierResourceBudget.TryPow(baseValue, (int)exponentNode.Value, out value))
                {
                    error = "Independent exact power exceeds the result bit-length budget.";
                    return false;
                }
                error = string.Empty;
                return true;
            default:
                value = default;
                error = $"Node type {node.GetType().Name} is outside the independently verified exact rational function subset.";
                return false;
        }
    }

    private static bool FinishScalar(ExactRational value, out string error)
    {
        if (!VerifierResourceBudget.IsScalarWithinLimit(value))
        {
            error = "Independent exact scalar exceeds the supported bit-length budget.";
            return false;
        }
        error = string.Empty;
        return true;
    }
}

internal static class VerifierResourceBudget
{
    private const int MaxScalarBitLength = 4096;
    public static readonly ExactRational Zero = new(BigInteger.Zero, BigInteger.One);
    public static readonly ExactRational One = new(BigInteger.One, BigInteger.One);

    public static bool IsScalarWithinLimit(ExactRational value) =>
        BitLength(value.Numerator) <= MaxScalarBitLength
        && BitLength(value.Denominator) <= MaxScalarBitLength;

    public static bool TryAdd(ExactRational left, ExactRational right, out ExactRational value)
    {
        value = default;
        var numeratorBits = Math.Max(
            SaturatingAdd(BitLength(left.Numerator), BitLength(right.Denominator)),
            SaturatingAdd(BitLength(right.Numerator), BitLength(left.Denominator))) + 1;
        var denominatorBits = SaturatingAdd(BitLength(left.Denominator), BitLength(right.Denominator));
        if (numeratorBits > MaxScalarBitLength || denominatorBits > MaxScalarBitLength)
        {
            return false;
        }
        value = left + right;
        return IsScalarWithinLimit(value);
    }

    public static bool TryMultiply(ExactRational left, ExactRational right, out ExactRational value)
    {
        value = default;
        var numeratorBits = SaturatingAdd(BitLength(left.Numerator), BitLength(right.Numerator));
        var denominatorBits = SaturatingAdd(BitLength(left.Denominator), BitLength(right.Denominator));
        if (numeratorBits > MaxScalarBitLength || denominatorBits > MaxScalarBitLength)
        {
            return false;
        }
        value = left * right;
        return IsScalarWithinLimit(value);
    }

    public static bool TryDivide(ExactRational numerator, ExactRational denominator, out ExactRational value)
    {
        value = default;
        if (denominator == Zero)
        {
            return false;
        }
        var numeratorBits = SaturatingAdd(BitLength(numerator.Numerator), BitLength(denominator.Denominator));
        var denominatorBits = SaturatingAdd(BitLength(numerator.Denominator), BitLength(denominator.Numerator));
        if (numeratorBits > MaxScalarBitLength || denominatorBits > MaxScalarBitLength)
        {
            return false;
        }
        value = numerator / denominator;
        return IsScalarWithinLimit(value);
    }

    public static bool TryPow(ExactRational value, int exponent, out ExactRational result)
    {
        result = default;
        if (exponent < 0)
        {
            return false;
        }
        if (exponent == 0)
        {
            result = One;
            return true;
        }
        var numeratorBits = SaturatingMultiply(BitLength(value.Numerator), exponent);
        var denominatorBits = SaturatingMultiply(BitLength(value.Denominator), exponent);
        if (numeratorBits > MaxScalarBitLength || denominatorBits > MaxScalarBitLength)
        {
            return false;
        }

        var current = One;
        for (var i = 0; i < exponent; i++)
        {
            if (!TryMultiply(current, value, out current))
            {
                return false;
            }
        }
        result = current;
        return true;
    }

    private static long BitLength(BigInteger value) =>
        value.IsZero ? 0 : BigInteger.Abs(value).GetBitLength();

    private static long SaturatingAdd(long left, long right) =>
        left > long.MaxValue - right ? long.MaxValue : left + right;

    private static long SaturatingMultiply(long value, int factor) =>
        factor == 0 ? 0 : value > long.MaxValue / factor ? long.MaxValue : value * factor;
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
            || !VerifierResourceBudget.IsScalarWithinLimit(value)
            || result.SolutionSet is not FiniteSolutionSet finite
            || finite.Values.Count != 1
            || !TryReadScalar(finite.Values[0], out var setValue)
            || setValue != value)
        {
            error = "Solved scalar result must contain one bounded exact value consistently in ExactResult and FiniteSolutionSet.";
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
