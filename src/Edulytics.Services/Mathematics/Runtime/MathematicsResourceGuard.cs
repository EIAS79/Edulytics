using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Runtime;

namespace Edulytics.Services.Mathematics.Runtime;

public sealed class MathematicsResourceLimitException : InvalidOperationException
{
    public MathematicsResourceLimitException(string message) : base(message)
    {
    }
}

/// <summary>
/// Central fail-closed resource guard for Mathematics Intelligence.
/// The guard validates raw input, AST shape, polynomial degree, matrix size,
/// estimated memory, request fan-out, timeout and concurrency budgets.
/// </summary>
public static class MathematicsResourceGuard
{
    private const long EstimatedBytesPerAstNode = 256L;

    public static int ValidateInputText(
        string input,
        MathematicsResourceLimits? limits = null)
    {
        ArgumentNullException.ThrowIfNull(input);
        var effective = limits ?? MathematicsResourceLimits.ProductionDefaults;

        if (input.Length > effective.MaxInputLength)
            throw Limit($"Mathematics input exceeds {effective.MaxInputLength} characters.");

        var tokenCount = EstimateTokenCount(input);
        if (tokenCount > effective.MaxTokenCount)
            throw Limit($"Mathematics input exceeds {effective.MaxTokenCount} tokens.");

        return tokenCount;
    }

    public static void ValidateGenerationRequest(
        string fingerprintNamespace,
        string scopeKey,
        IReadOnlyList<string> allowedQuestionFamilies,
        int questionCount,
        IReadOnlyCollection<string> excludedExposureFingerprints,
        MathematicsResourceLimits? limits = null)
    {
        ArgumentNullException.ThrowIfNull(allowedQuestionFamilies);
        ArgumentNullException.ThrowIfNull(excludedExposureFingerprints);
        var effective = limits ?? MathematicsResourceLimits.ProductionDefaults;

        ValidateInputText(fingerprintNamespace ?? string.Empty, effective);
        ValidateInputText(scopeKey ?? string.Empty, effective);

        if (questionCount is < 1 or > 50)
            throw Limit("Mathematics generation question count is outside the reviewed [1,50] range.");

        if (allowedQuestionFamilies.Count is < 1 or > 64)
            throw Limit("Mathematics generation family fan-out exceeds the reviewed limit.");

        if (excludedExposureFingerprints.Count > 10000)
            throw Limit("Mathematics generation exposure history exceeds the reviewed limit.");

        var estimated =
            allowedQuestionFamilies.Sum(x => (long)(x?.Length ?? 0) * sizeof(char)) +
            excludedExposureFingerprints.Sum(x => (long)(x?.Length ?? 0) * sizeof(char));

        if (estimated > effective.MaxEstimatedMemoryBytes)
            throw Limit("Mathematics generation request exceeds the estimated memory budget.");
    }

    public static MathematicsAstBudgetReport AnalyzeAst(
        MathNode root,
        MathematicsResourceLimits? limits = null)
    {
        ArgumentNullException.ThrowIfNull(root);
        var effective = limits ?? MathematicsResourceLimits.ProductionDefaults;

        var stack = new Stack<(MathNode Node, int Depth)>();
        stack.Push((root, 1));

        var nodes = 0;
        var maxDepth = 0;
        var maxRows = 0;
        var maxColumns = 0;

        while (stack.Count > 0)
        {
            var (node, depth) = stack.Pop();
            nodes++;
            maxDepth = Math.Max(maxDepth, depth);

            if (nodes > effective.MaxAstNodes)
                throw Limit($"Mathematics AST exceeds {effective.MaxAstNodes} nodes.");
            if (depth > effective.MaxAstDepth)
                throw Limit($"Mathematics AST exceeds depth {effective.MaxAstDepth}.");

            if (node is MatrixNode matrix)
            {
                maxRows = Math.Max(maxRows, matrix.Rows.Count);
                maxColumns = Math.Max(
                    maxColumns,
                    matrix.Rows.Count == 0 ? 0 : matrix.Rows.Max(row => row.Count));

                if (matrix.Rows.Count > effective.MaxMatrixDimension ||
                    matrix.Rows.Any(row => row.Count > effective.MaxMatrixDimension))
                {
                    throw Limit(
                        $"Matrix dimensions exceed the reviewed {effective.MaxMatrixDimension}x{effective.MaxMatrixDimension} limit.");
                }
            }

            foreach (var child in Children(node))
                stack.Push((child, depth + 1));
        }

        var estimatedBytes = checked(nodes * EstimatedBytesPerAstNode);
        if (estimatedBytes > effective.MaxEstimatedMemoryBytes)
            throw Limit("Mathematics AST exceeds the estimated memory budget.");

        var degree = TryPolynomialDegree(root, effective.MaxPolynomialDegree + 1);
        if (degree is > 0 && degree > effective.MaxPolynomialDegree)
        {
            throw Limit(
                $"Polynomial degree {degree} exceeds the reviewed maximum {effective.MaxPolynomialDegree}.");
        }

        return new MathematicsAstBudgetReport(
            nodes,
            maxDepth,
            degree,
            maxRows,
            maxColumns,
            estimatedBytes);
    }

    private static MathematicsResourceLimitException Limit(string message)
    {
        MathematicsObservability.Record(MathematicsMetricKind.AlignmentRejection);
        return new MathematicsResourceLimitException(message);
    }

    private static int EstimateTokenCount(string value)
    {
        var count = 0;
        var inWord = false;

        foreach (var ch in value)
        {
            var word = char.IsLetterOrDigit(ch) || ch is '_' or '.';
            if (word)
            {
                if (!inWord)
                    count++;
                inWord = true;
                continue;
            }

            inWord = false;
            if (!char.IsWhiteSpace(ch))
                count++;
        }

        return count;
    }

    private static IEnumerable<MathNode> Children(MathNode node)
    {
        switch (node)
        {
            case NegateNode negate:
                yield return negate.Operand;
                break;
            case AddNode add:
                foreach (var term in add.Terms) yield return term;
                break;
            case MultiplyNode multiply:
                foreach (var factor in multiply.Factors) yield return factor;
                break;
            case DivideNode divide:
                yield return divide.Numerator;
                yield return divide.Denominator;
                break;
            case PowerNode power:
                yield return power.Base;
                yield return power.Exponent;
                break;
            case RootNode root:
                yield return root.Radicand;
                break;
            case EquationNode equation:
                yield return equation.Left;
                yield return equation.Right;
                break;
            case EquationSystemNode system:
                foreach (var equation in system.Equations) yield return equation;
                break;
            case InequalityNode inequality:
                yield return inequality.Left;
                yield return inequality.Right;
                break;
            case FunctionCallNode call:
                foreach (var argument in call.Arguments) yield return argument;
                break;
            case VectorNode vector:
                foreach (var component in vector.Components) yield return component;
                break;
            case MatrixNode matrix:
                foreach (var row in matrix.Rows)
                foreach (var cell in row)
                    yield return cell;
                break;
            case DerivativeNode derivative:
                yield return derivative.Expression;
                yield return derivative.Variable;
                break;
            case IntegralNode integral:
                yield return integral.Integrand;
                yield return integral.Variable;
                if (integral.LowerBound is not null) yield return integral.LowerBound;
                if (integral.UpperBound is not null) yield return integral.UpperBound;
                break;
        }
    }

    private static int? TryPolynomialDegree(MathNode node, int stopAt)
    {
        switch (node)
        {
            case IntegerNode:
            case RationalNode:
                return 0;
            case SymbolNode:
                return 1;
            case NegateNode negate:
                return TryPolynomialDegree(negate.Operand, stopAt);
            case AddNode add:
            {
                var max = 0;
                foreach (var term in add.Terms)
                {
                    var degree = TryPolynomialDegree(term, stopAt);
                    if (degree is null) return null;
                    max = Math.Max(max, degree.Value);
                    if (max >= stopAt) return max;
                }
                return max;
            }
            case MultiplyNode multiply:
            {
                var total = 0;
                foreach (var factor in multiply.Factors)
                {
                    var degree = TryPolynomialDegree(factor, stopAt);
                    if (degree is null) return null;
                    total = Math.Min(stopAt, total + degree.Value);
                    if (total >= stopAt) return total;
                }
                return total;
            }
            case PowerNode power when power.Exponent is IntegerNode exponent &&
                                          exponent.Value >= BigInteger.Zero &&
                                          exponent.Value <= int.MaxValue:
            {
                var baseDegree = TryPolynomialDegree(power.Base, stopAt);
                if (baseDegree is null) return null;
                var exp = (int)exponent.Value;
                if (baseDegree.Value == 0 || exp == 0) return 0;
                var product = (long)baseDegree.Value * exp;
                return (int)Math.Min(stopAt, product);
            }
            case EquationNode equation:
            {
                var left = TryPolynomialDegree(equation.Left, stopAt);
                var right = TryPolynomialDegree(equation.Right, stopAt);
                return left is null || right is null ? null : Math.Max(left.Value, right.Value);
            }
            case EquationSystemNode system:
            {
                var max = 0;
                foreach (var equation in system.Equations)
                {
                    var degree = TryPolynomialDegree(equation, stopAt);
                    if (degree is null) return null;
                    max = Math.Max(max, degree.Value);
                }
                return max;
            }
            case InequalityNode inequality:
            {
                var left = TryPolynomialDegree(inequality.Left, stopAt);
                var right = TryPolynomialDegree(inequality.Right, stopAt);
                return left is null || right is null ? null : Math.Max(left.Value, right.Value);
            }
            default:
                return null;
        }
    }
}

public sealed class MathematicsExecutionBudget
{
    private readonly MathematicsResourceLimits limits;
    private readonly SemaphoreSlim semaphore;

    public MathematicsExecutionBudget(MathematicsResourceLimits? limits = null)
    {
        this.limits = limits ?? MathematicsResourceLimits.ProductionDefaults;
        if (this.limits.MaxConcurrentOperations < 1)
            throw new ArgumentOutOfRangeException(nameof(limits));

        semaphore = new SemaphoreSlim(
            this.limits.MaxConcurrentOperations,
            this.limits.MaxConcurrentOperations);
    }

    public async Task<T> RunAsync<T>(
        Func<CancellationToken, Task<T>> operation,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(operation);

        using var waitBudget = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        waitBudget.CancelAfter(limits.SolverTimeout);

        try
        {
            await semaphore.WaitAsync(waitBudget.Token);
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            MathematicsObservability.Record(MathematicsMetricKind.Timeout);
            throw new TimeoutException("Mathematics concurrency queue exceeded the solver time budget.");
        }

        try
        {
            using var executionBudget = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            executionBudget.CancelAfter(limits.SolverTimeout);

            try
            {
                var result = await operation(executionBudget.Token);
                MathematicsObservability.Record(MathematicsMetricKind.SolverSuccess);
                return result;
            }
            catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
            {
                MathematicsObservability.Record(MathematicsMetricKind.Timeout);
                throw new TimeoutException("Mathematics operation exceeded the solver time budget.");
            }
        }
        finally
        {
            semaphore.Release();
        }
    }
}
