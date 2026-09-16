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

        if (!ExactFunctionSolverEvaluator.TryEvaluate(expression!, variable!, input, out var value, out error))
        {
            return ExactFunctionsV2.Unsupported(request, "edulytics-native-functions", "functions-exact-v1", error);
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
            if (!ExactFunctionSolverEvaluator.TryEvaluate(expression!, variable!, input, out var output, out error))
            {
                return ExactFunctionsV2.Unsupported(request, "edulytics-native-functions", "functions-exact-v1", error);
            }

            points.Add(new VectorNode([ExactFunctionsV2.ToNode(input), ExactFunctionsV2.ToNode(output)]));
        }

        var result = new VectorNode(points);
        return ExactFunctionsV2.Solved(request, result, new FiniteSolutionSet(points), "exact-graph-coordinate-sampling", "edulytics-native-functions", "functions-exact-v1");
    }
}

public sealed class ExactSequenceTermSolver : IMathematicsSolver
{
    private static readonly ExactRational One = new(BigInteger.One, BigInteger.One);

    public MathematicsSolveResult Solve(MathematicsSolveRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (!ExactFunctionsV2.TryParseSequenceRequest(request.Problem, out var kind, out var first, out var stepOrRatio, out var n, out var error))
        {
            return ExactFunctionsV2.Unsupported(request, "edulytics-native-sequences", "sequences-exact-v1", error);
        }

        ExactRational value;
        string strategy;
        if (kind == "arithmetic_nth_term")
        {
            if (n > 10000)
            {
                return ExactFunctionsV2.ResourceLimit(request, "edulytics-native-sequences", "sequences-exact-v1", "Arithmetic sequence index exceeds the supported bound of 10000.");
            }

            value = first + new ExactRational(n - BigInteger.One, BigInteger.One) * stepOrRatio;
            strategy = "arithmetic-sequence-closed-form";
        }
        else
        {
            if (n > 64)
            {
                return ExactFunctionsV2.ResourceLimit(request, "edulytics-native-sequences", "sequences-exact-v1", "Geometric sequence index exceeds the supported exact-power bound of 64.");
            }

            value = first * ExactFunctionsV2.Pow(stepOrRatio, (int)(n - BigInteger.One));
            strategy = "geometric-sequence-closed-form";
        }

        var result = ExactFunctionsV2.ToNode(value);
        return ExactFunctionsV2.Solved(request, result, new FiniteSolutionSet([result]), strategy, "edulytics-native-sequences", "sequences-exact-v1");
    }
}

internal static class ExactFunctionSolverEvaluator
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
                error = $"Unexpected symbol {symbol.Name}; exact evaluation supports one declared variable.";
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
                    error = "Exact function evaluation encountered division by zero.";
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
                    error = $"Exact function evaluation supports integer exponents from 0 through {MaxExponent}.";
                    return false;
                }
                if (!TryEvaluate(power.Base, variable, input, out var baseValue, out error))
                {
                    value = default;
                    return false;
                }
                value = ExactFunctionsV2.Pow(baseValue, (int)exponentNode.Value);
                error = string.Empty;
                return true;
            default:
                value = default;
                error = $"Node type {node.GetType().Name} is outside the exact rational function-evaluation subset.";
                return false;
        }
    }
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

    public static ExactRational Pow(ExactRational value, int exponent)
    {
        var result = new ExactRational(BigInteger.One, BigInteger.One);
        for (var i = 0; i < exponent; i++)
        {
            result *= value;
        }
        return result;
    }

    public static MathematicsSolveResult Solved(MathematicsSolveRequest request, MathNode exactResult, SolutionSet solutionSet, string strategy, string provider, string version) =>
        new(MathematicsSolveStatus.Solved, exactResult, solutionSet, request.Assumptions, strategy, new MathematicsSolutionTrace([]), provider, version, []);

    public static MathematicsSolveResult Unsupported(MathematicsSolveRequest request, string provider, string version, string diagnostic) =>
        new(MathematicsSolveStatus.Unsupported, null, null, request.Assumptions, null, null, provider, version, [diagnostic]);

    public static MathematicsSolveResult ResourceLimit(MathematicsSolveRequest request, string provider, string version, string diagnostic) =>
        new(MathematicsSolveStatus.ResourceLimit, null, null, request.Assumptions, null, null, provider, version, [diagnostic]);
}
