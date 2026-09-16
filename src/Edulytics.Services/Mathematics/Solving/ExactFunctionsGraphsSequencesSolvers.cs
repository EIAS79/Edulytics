using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Services.Mathematics.Contracts;

namespace Edulytics.Services.Mathematics.Solving;

public sealed class ExactFunctionEvaluationSolver : IMathematicsSolver
{
    public MathematicsSolveResult Solve(MathematicsSolveRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (!ExactFunctionsV2.TryParseEvaluationRequest(request.Problem, out var expression, out var variable, out var input, out var error))
        {
            return ExactFunctionsV2.Unsupported(request, "edulytics-native-functions", "functions-exact-v1", error);
        }

        if (!ExactFunctionSolverEvaluator.TryEvaluate(expression!, variable!, input, out var value, out error, out var failureKind))
        {
            return failureKind == ExactEvaluationFailureKind.ResourceLimit
                ? ExactFunctionsV2.ResourceLimit(request, "edulytics-native-functions", "functions-exact-v1", error)
                : ExactFunctionsV2.Unsupported(request, "edulytics-native-functions", "functions-exact-v1", error);
        }

        var result = ExactFunctionsV2.ToNode(value);
        return ExactFunctionsV2.Solved(request, result, new FiniteSolutionSet([result]), "exact-function-evaluation", "edulytics-native-functions", "functions-exact-v1");
    }
}

public sealed class ExactGraphSamplingSolver : IMathematicsSolver
{
    public MathematicsSolveResult Solve(MathematicsSolveRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (!ExactFunctionsV2.TryParseGraphRequest(request.Problem, out var expression, out var variable, out var inputs, out var error))
        {
            return ExactFunctionsV2.Unsupported(request, "edulytics-native-functions", "functions-exact-v1", error);
        }

        var points = new List<MathNode>(inputs!.Count);
        foreach (var input in inputs)
        {
            if (!ExactFunctionSolverEvaluator.TryEvaluate(expression!, variable!, input, out var output, out error, out var failureKind))
            {
                return failureKind == ExactEvaluationFailureKind.ResourceLimit
                    ? ExactFunctionsV2.ResourceLimit(request, "edulytics-native-functions", "functions-exact-v1", error)
                    : ExactFunctionsV2.Unsupported(request, "edulytics-native-functions", "functions-exact-v1", error);
            }

            points.Add(new VectorNode([ExactFunctionsV2.ToNode(input), ExactFunctionsV2.ToNode(output)]));
        }

        var result = new VectorNode(points);
        return ExactFunctionsV2.Solved(request, result, new FiniteSolutionSet(points), "exact-graph-coordinate-sampling", "edulytics-native-functions", "functions-exact-v1");
    }
}

public sealed class ExactSequenceTermSolver : IMathematicsSolver
{
    public MathematicsSolveResult Solve(MathematicsSolveRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (!ExactFunctionsV2.TryParseSequenceRequest(request.Problem, out var kind, out var first, out var stepOrRatio, out var n, out var error))
        {
            return ExactFunctionsV2.Unsupported(request, "edulytics-native-sequences", "sequences-exact-v1", error);
        }
        if (!ExactResourceBudget.IsScalarWithinLimit(first) || !ExactResourceBudget.IsScalarWithinLimit(stepOrRatio))
        {
            return ExactFunctionsV2.ResourceLimit(request, "edulytics-native-sequences", "sequences-exact-v1", "Sequence parameters exceed the supported exact scalar bit-length budget.");
        }

        ExactRational value;
        string strategy;
        if (kind == "arithmetic_nth_term")
        {
            if (n > 10000)
            {
                return ExactFunctionsV2.ResourceLimit(request, "edulytics-native-sequences", "sequences-exact-v1", "Arithmetic sequence index exceeds the supported bound of 10000.");
            }

            var indexFactor = new ExactRational(n - BigInteger.One, BigInteger.One);
            if (!ExactResourceBudget.TryMultiply(stepOrRatio, indexFactor, out var delta)
                || !ExactResourceBudget.TryAdd(first, delta, out value))
            {
                return ExactFunctionsV2.ResourceLimit(request, "edulytics-native-sequences", "sequences-exact-v1", "Arithmetic nth-term evaluation exceeds the supported exact result bit-length budget.");
            }
            strategy = "arithmetic-sequence-closed-form";
        }
        else
        {
            if (n > 64)
            {
                return ExactFunctionsV2.ResourceLimit(request, "edulytics-native-sequences", "sequences-exact-v1", "Geometric sequence index exceeds the supported exact-power bound of 64.");
            }
            if (!ExactResourceBudget.TryPow(stepOrRatio, (int)(n - BigInteger.One), out var ratioPower)
                || !ExactResourceBudget.TryMultiply(first, ratioPower, out value))
            {
                return ExactFunctionsV2.ResourceLimit(request, "edulytics-native-sequences", "sequences-exact-v1", "Geometric nth-term evaluation exceeds the supported exact result bit-length budget.");
            }
            strategy = "geometric-sequence-closed-form";
        }

        var result = ExactFunctionsV2.ToNode(value);
        return ExactFunctionsV2.Solved(request, result, new FiniteSolutionSet([result]), strategy, "edulytics-native-sequences", "sequences-exact-v1");
    }
}

internal enum ExactEvaluationFailureKind
{
    None = 0,
    Unsupported = 1,
    ResourceLimit = 2
}

internal static class ExactFunctionSolverEvaluator
{
    private const int MaxExponent = 12;
    private const int MaxNodeCount = 128;
    private const int MaxDepth = 24;

    public static bool TryEvaluate(
        MathNode node,
        string variable,
        ExactRational input,
        out ExactRational value,
        out string error,
        out ExactEvaluationFailureKind failureKind)
    {
        if (!ExactResourceBudget.IsScalarWithinLimit(input))
        {
            value = default;
            error = "Function input exceeds the supported exact scalar bit-length budget.";
            failureKind = ExactEvaluationFailureKind.ResourceLimit;
            return false;
        }

        var remainingNodes = MaxNodeCount;
        return TryEvaluateCore(node, variable, input, depth: 0, ref remainingNodes, out value, out error, out failureKind);
    }

    private static bool TryEvaluateCore(
        MathNode node,
        string variable,
        ExactRational input,
        int depth,
        ref int remainingNodes,
        out ExactRational value,
        out string error,
        out ExactEvaluationFailureKind failureKind)
    {
        if (depth > MaxDepth || --remainingNodes < 0)
        {
            value = default;
            error = "Exact function evaluation exceeds the supported expression depth/node budget.";
            failureKind = ExactEvaluationFailureKind.ResourceLimit;
            return false;
        }

        switch (node)
        {
            case IntegerNode integer:
                value = new ExactRational(integer.Value, BigInteger.One);
                return FinishScalar(value, out error, out failureKind);
            case RationalNode rational:
                value = rational.Value;
                return FinishScalar(value, out error, out failureKind);
            case SymbolNode symbol when string.Equals(symbol.Name, variable, StringComparison.Ordinal):
                value = input;
                error = string.Empty;
                failureKind = ExactEvaluationFailureKind.None;
                return true;
            case SymbolNode symbol:
                value = default;
                error = $"Unexpected symbol {symbol.Name}; exact evaluation supports one declared variable.";
                failureKind = ExactEvaluationFailureKind.Unsupported;
                return false;
            case NegateNode negate:
                if (!TryEvaluateCore(negate.Operand, variable, input, depth + 1, ref remainingNodes, out var operand, out error, out failureKind))
                {
                    value = default;
                    return false;
                }
                value = new ExactRational(-operand.Numerator, operand.Denominator);
                return FinishScalar(value, out error, out failureKind);
            case AddNode add:
                var sum = ExactResourceBudget.Zero;
                foreach (var term in add.Terms)
                {
                    if (!TryEvaluateCore(term, variable, input, depth + 1, ref remainingNodes, out var termValue, out error, out failureKind))
                    {
                        value = default;
                        return false;
                    }
                    if (!ExactResourceBudget.TryAdd(sum, termValue, out sum))
                    {
                        value = default;
                        error = "Exact addition exceeds the supported result bit-length budget.";
                        failureKind = ExactEvaluationFailureKind.ResourceLimit;
                        return false;
                    }
                }
                value = sum;
                error = string.Empty;
                failureKind = ExactEvaluationFailureKind.None;
                return true;
            case MultiplyNode multiply:
                var product = ExactResourceBudget.One;
                foreach (var factor in multiply.Factors)
                {
                    if (!TryEvaluateCore(factor, variable, input, depth + 1, ref remainingNodes, out var factorValue, out error, out failureKind))
                    {
                        value = default;
                        return false;
                    }
                    if (!ExactResourceBudget.TryMultiply(product, factorValue, out product))
                    {
                        value = default;
                        error = "Exact multiplication exceeds the supported result bit-length budget.";
                        failureKind = ExactEvaluationFailureKind.ResourceLimit;
                        return false;
                    }
                }
                value = product;
                error = string.Empty;
                failureKind = ExactEvaluationFailureKind.None;
                return true;
            case DivideNode divide:
                if (!TryEvaluateCore(divide.Numerator, variable, input, depth + 1, ref remainingNodes, out var numerator, out error, out failureKind)
                    || !TryEvaluateCore(divide.Denominator, variable, input, depth + 1, ref remainingNodes, out var denominator, out error, out failureKind))
                {
                    value = default;
                    return false;
                }
                if (denominator == ExactResourceBudget.Zero)
                {
                    value = default;
                    error = "Exact function evaluation encountered division by zero.";
                    failureKind = ExactEvaluationFailureKind.Unsupported;
                    return false;
                }
                if (!ExactResourceBudget.TryDivide(numerator, denominator, out value))
                {
                    error = "Exact division exceeds the supported result bit-length budget.";
                    failureKind = ExactEvaluationFailureKind.ResourceLimit;
                    return false;
                }
                error = string.Empty;
                failureKind = ExactEvaluationFailureKind.None;
                return true;
            case PowerNode power:
                if (power.Exponent is not IntegerNode exponentNode
                    || exponentNode.Value < BigInteger.Zero
                    || exponentNode.Value > MaxExponent)
                {
                    value = default;
                    error = $"Exact function evaluation supports integer exponents from 0 through {MaxExponent}.";
                    failureKind = ExactEvaluationFailureKind.Unsupported;
                    return false;
                }
                if (!TryEvaluateCore(power.Base, variable, input, depth + 1, ref remainingNodes, out var baseValue, out error, out failureKind))
                {
                    value = default;
                    return false;
                }
                if (!ExactResourceBudget.TryPow(baseValue, (int)exponentNode.Value, out value))
                {
                    error = "Exact power exceeds the supported result bit-length budget.";
                    failureKind = ExactEvaluationFailureKind.ResourceLimit;
                    return false;
                }
                error = string.Empty;
                failureKind = ExactEvaluationFailureKind.None;
                return true;
            default:
                value = default;
                error = $"Node type {node.GetType().Name} is outside the exact rational function-evaluation subset.";
                failureKind = ExactEvaluationFailureKind.Unsupported;
                return false;
        }
    }

    private static bool FinishScalar(
        ExactRational value,
        out string error,
        out ExactEvaluationFailureKind failureKind)
    {
        if (!ExactResourceBudget.IsScalarWithinLimit(value))
        {
            error = "Exact scalar exceeds the supported bit-length budget.";
            failureKind = ExactEvaluationFailureKind.ResourceLimit;
            return false;
        }

        error = string.Empty;
        failureKind = ExactEvaluationFailureKind.None;
        return true;
    }
}

internal static class ExactResourceBudget
{
    public const int MaxScalarBitLength = 4096;
    private const long MaxIntermediateBitLength = (MaxScalarBitLength * 2L) + 1L;
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
        if (numeratorBits > MaxIntermediateBitLength || denominatorBits > MaxIntermediateBitLength)
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
        if (numeratorBits > MaxIntermediateBitLength || denominatorBits > MaxIntermediateBitLength)
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
        if (numeratorBits > MaxIntermediateBitLength || denominatorBits > MaxIntermediateBitLength)
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
        if (numeratorBits > MaxIntermediateBitLength || denominatorBits > MaxIntermediateBitLength)
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

internal static class ExactFunctionsV2
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
            error = "Function evaluation requires evaluate_exact(expression, variable, exact rational input).";
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
            error = "Graph sampling requires sample_graph_exact(expression, variable, vector of 1..7 exact rational x-values).";
            return false;
        }

        var values = new List<ExactRational>(inputVector.Components.Count);
        foreach (var node in inputVector.Components)
        {
            if (!TryReadScalar(node, out var scalar))
            {
                error = "Every graph x-value must be an exact integer or rational.";
                return false;
            }
            if (values.Contains(scalar))
            {
                error = "Graph sampling does not accept duplicate x-values.";
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
            error = "Sequence evaluation requires arithmetic_nth_term(a1, d, n) or geometric_nth_term(a1, r, n) with exact scalars and positive integer n.";
            return false;
        }

        kind = call.FunctionName;
        n = index.Value;
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

    public static MathNode ToNode(ExactRational value) =>
        value.Denominator == BigInteger.One ? new IntegerNode(value.Numerator) : new RationalNode(value);

    public static MathematicsSolveResult Solved(MathematicsSolveRequest request, MathNode exactResult, SolutionSet solutionSet, string strategy, string provider, string version) =>
        new(MathematicsSolveStatus.Solved, exactResult, solutionSet, request.Assumptions, strategy, new MathematicsSolutionTrace([]), provider, version, []);

    public static MathematicsSolveResult Unsupported(MathematicsSolveRequest request, string provider, string version, string diagnostic) =>
        new(MathematicsSolveStatus.Unsupported, null, null, request.Assumptions, null, null, provider, version, [diagnostic]);

    public static MathematicsSolveResult ResourceLimit(MathematicsSolveRequest request, string provider, string version, string diagnostic) =>
        new(MathematicsSolveStatus.ResourceLimit, null, null, request.Assumptions, null, null, provider, version, [diagnostic]);
}
