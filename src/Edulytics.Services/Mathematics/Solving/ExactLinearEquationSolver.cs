using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Services.Mathematics.Contracts;

namespace Edulytics.Services.Mathematics.Solving;

/// <summary>
/// Exact-rational V2 vertical slice for single-variable affine equations.
/// Supports expressions reducible to a*x+b=c using Integer/Rational/Symbol,
/// Negate, Add, Multiply-by-constant and Divide-by-nonzero-constant nodes.
/// This provider is intentionally not wired into learner-facing generation yet.
/// </summary>
public sealed class ExactLinearEquationSolver : IMathematicsSolver
{
    public const string ProviderName = "edulytics-native-linear";
    public const string ProviderVersion = "linear-exact-v1";

    public MathematicsSolveResult Solve(MathematicsSolveRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.Problem is not EquationNode equation)
        {
            return Unsupported("ExactLinearEquationSolver only supports EquationNode problems.");
        }

        if (!ExactLinearForm.TryCreate(equation.Left, out var left, out var leftError))
        {
            return Unsupported(leftError ?? "Left side is outside the affine expression subset.");
        }

        if (!ExactLinearForm.TryCreate(equation.Right, out var right, out var rightError))
        {
            return Unsupported(rightError ?? "Right side is outside the affine expression subset.");
        }

        if (!ExactLinearForm.TryMergeVariable(left.Variable, right.Variable, out var variable))
        {
            return Unsupported("Equation contains more than one distinct variable.");
        }

        var coefficient = left.Coefficient - right.Coefficient;
        var constant = left.Constant - right.Constant;
        var zero = ExactLinearForm.Zero;

        if (coefficient == zero)
        {
            if (constant == zero)
            {
                return new MathematicsSolveResult(
                    MathematicsSolveStatus.Indeterminate,
                    null,
                    new AllRealNumbersSolutionSet(),
                    request.Assumptions.ToArray(),
                    "linear.collect-and-isolate",
                    new MathematicsSolutionTrace([]),
                    ProviderName,
                    ProviderVersion,
                    ["Both sides reduce to the same affine expression; every real value satisfies the equation."]);
            }

            return new MathematicsSolveResult(
                MathematicsSolveStatus.NoSolution,
                null,
                new EmptySolutionSet(),
                request.Assumptions.ToArray(),
                "linear.collect-and-isolate",
                new MathematicsSolutionTrace([]),
                ProviderName,
                ProviderVersion,
                ["Variable terms cancel but a non-zero constant remains; the equation is inconsistent."]);
        }

        if (string.IsNullOrWhiteSpace(variable))
        {
            return Unsupported("Equation does not contain a variable to solve for.");
        }

        var solution = new ExactRational(
            -constant.Numerator * coefficient.Denominator,
            constant.Denominator * coefficient.Numerator);

        var symbol = new SymbolNode(variable);
        var coefficientNode = ToNode(coefficient);
        var negativeConstantNode = ToNode(new ExactRational(-constant.Numerator, constant.Denominator));
        var solutionNode = ToNode(solution);

        var normalizedEquation = new EquationNode(
            new MultiplyNode([coefficientNode, symbol]),
            negativeConstantNode);
        var solvedEquation = new EquationNode(symbol, solutionNode);

        var trace = new MathematicsSolutionTrace(
        [
            new MathematicsSolutionStep(
                "collect-linear-terms",
                equation,
                "linear.collect_terms",
                "Collect variable terms on one side and constants on the other.",
                normalizedEquation,
                "Equivalent terms are combined exactly using rational arithmetic.",
                request.Assumptions.ToArray(),
                true),
            new MathematicsSolutionStep(
                "divide-by-coefficient",
                normalizedEquation,
                "linear.divide_nonzero_coefficient",
                "Divide both sides by the non-zero variable coefficient.",
                solvedEquation,
                "Division by a non-zero coefficient preserves equality.",
                request.Assumptions.ToArray(),
                true)
        ]);

        return new MathematicsSolveResult(
            MathematicsSolveStatus.Solved,
            solutionNode,
            new FiniteSolutionSet([solutionNode]),
            request.Assumptions.ToArray(),
            "linear.collect-and-isolate",
            trace,
            ProviderName,
            ProviderVersion,
            []);
    }

    private static MathNode ToNode(ExactRational value) =>
        value.Denominator == System.Numerics.BigInteger.One
            ? new IntegerNode(value.Numerator)
            : new RationalNode(value);

    private static MathematicsSolveResult Unsupported(string diagnostic) =>
        new(
            MathematicsSolveStatus.Unsupported,
            null,
            null,
            [],
            null,
            null,
            ProviderName,
            ProviderVersion,
            [diagnostic]);
}

internal readonly record struct ExactLinearForm(
    string? Variable,
    ExactRational Coefficient,
    ExactRational Constant)
{
    internal static readonly ExactRational Zero = new(0, 1);
    private static readonly ExactRational One = new(1, 1);

    public static bool TryCreate(MathNode node, out ExactLinearForm form, out string? error)
    {
        ArgumentNullException.ThrowIfNull(node);

        switch (node)
        {
            case IntegerNode integer:
                form = new ExactLinearForm(null, Zero, new ExactRational(integer.Value, 1));
                error = null;
                return true;

            case RationalNode rational:
                form = new ExactLinearForm(null, Zero, rational.Value);
                error = null;
                return true;

            case SymbolNode symbol:
                form = new ExactLinearForm(symbol.Name, One, Zero);
                error = null;
                return true;

            case NegateNode negate:
                if (!TryCreate(negate.Operand, out var operand, out error))
                {
                    form = default;
                    return false;
                }

                form = new ExactLinearForm(
                    operand.Variable,
                    Negate(operand.Coefficient),
                    Negate(operand.Constant));
                error = null;
                return true;

            case AddNode add:
                var aggregate = new ExactLinearForm(null, Zero, Zero);
                foreach (var term in add.Terms)
                {
                    if (!TryCreate(term, out var next, out error))
                    {
                        form = default;
                        return false;
                    }

                    if (!TryMergeVariable(aggregate.Variable, next.Variable, out var mergedVariable))
                    {
                        form = default;
                        error = "Expression contains more than one distinct variable.";
                        return false;
                    }

                    aggregate = new ExactLinearForm(
                        mergedVariable,
                        aggregate.Coefficient + next.Coefficient,
                        aggregate.Constant + next.Constant);
                }

                form = aggregate;
                error = null;
                return true;

            case MultiplyNode multiply:
                if (multiply.Factors.Count == 0)
                {
                    form = new ExactLinearForm(null, Zero, One);
                    error = null;
                    return true;
                }

                var product = new ExactLinearForm(null, Zero, One);
                foreach (var factor in multiply.Factors)
                {
                    if (!TryCreate(factor, out var next, out error))
                    {
                        form = default;
                        return false;
                    }

                    if (!TryMultiply(product, next, out product, out error))
                    {
                        form = default;
                        return false;
                    }
                }

                form = product;
                error = null;
                return true;

            case DivideNode divide:
                if (!TryCreate(divide.Numerator, out var numerator, out error))
                {
                    form = default;
                    return false;
                }

                if (!TryCreate(divide.Denominator, out var denominator, out error))
                {
                    form = default;
                    return false;
                }

                if (denominator.Coefficient != Zero || denominator.Variable is not null)
                {
                    form = default;
                    error = "Division by an expression containing a variable is outside the affine subset.";
                    return false;
                }

                if (denominator.Constant == Zero)
                {
                    form = default;
                    error = "Division by zero is invalid.";
                    return false;
                }

                form = new ExactLinearForm(
                    numerator.Variable,
                    numerator.Coefficient / denominator.Constant,
                    numerator.Constant / denominator.Constant);
                error = null;
                return true;

            default:
                form = default;
                error = $"Node type {node.GetType().Name} is outside the exact linear subset.";
                return false;
        }
    }

    public static bool TryMergeVariable(string? left, string? right, out string? merged)
    {
        if (left is null)
        {
            merged = right;
            return true;
        }

        if (right is null)
        {
            merged = left;
            return true;
        }

        if (string.Equals(left, right, StringComparison.Ordinal))
        {
            merged = left;
            return true;
        }

        merged = null;
        return false;
    }

    private static bool TryMultiply(
        ExactLinearForm left,
        ExactLinearForm right,
        out ExactLinearForm product,
        out string? error)
    {
        if (left.Coefficient != Zero && right.Coefficient != Zero)
        {
            product = default;
            error = "Multiplication would create a non-linear variable term.";
            return false;
        }

        if (!TryMergeVariable(left.Variable, right.Variable, out var variable))
        {
            product = default;
            error = "Multiplication contains more than one distinct variable.";
            return false;
        }

        product = new ExactLinearForm(
            variable,
            left.Coefficient * right.Constant + left.Constant * right.Coefficient,
            left.Constant * right.Constant);
        error = null;
        return true;
    }

    private static ExactRational Negate(ExactRational value) =>
        new(-value.Numerator, value.Denominator);
}
