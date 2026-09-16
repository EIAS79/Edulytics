using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Services.Mathematics.Contracts;

namespace Edulytics.Services.Mathematics.Solving;

/// <summary>
/// Exact-rational V2 solver for two affine equations in exactly two variables.
/// Each equation is normalized to a*x + b*y + c = 0. Unique systems are solved
/// by exact determinant/Cramer arithmetic; singular systems are classified as
/// inconsistent or dependent without guessing a representative solution.
/// </summary>
public sealed class ExactLinearSystem2x2Solver : IMathematicsSolver
{
    public const string ProviderName = "edulytics-native-linear-system-2x2";
    public const string ProviderVersion = "linear-system-2x2-exact-v1";

    private static readonly ExactRational Zero = new(0, 1);

    public MathematicsSolveResult Solve(MathematicsSolveRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.Problem is not EquationSystemNode system || system.Equations.Count != 2)
        {
            return Unsupported("ExactLinearSystem2x2Solver requires exactly two equations.");
        }

        if (!ExactAffineExpression.TryCreate(system.Equations[0].Left, out var left1, out var error)
            || !ExactAffineExpression.TryCreate(system.Equations[0].Right, out var right1, out error))
        {
            return Unsupported(error ?? "First equation is outside the exact affine subset.");
        }

        if (!ExactAffineExpression.TryCreate(system.Equations[1].Left, out var left2, out error)
            || !ExactAffineExpression.TryCreate(system.Equations[1].Right, out var right2, out error))
        {
            return Unsupported(error ?? "Second equation is outside the exact affine subset.");
        }

        var equation1 = left1 - right1;
        var equation2 = left2 - right2;
        var variables = equation1.Coefficients.Keys
            .Concat(equation2.Coefficients.Keys)
            .Distinct(StringComparer.Ordinal)
            .OrderBy(x => x, StringComparer.Ordinal)
            .ToArray();

        if (variables.Length != 2)
        {
            return Unsupported(
                $"2x2 system solving requires exactly two distinct variables; found {variables.Length}.");
        }

        var xName = variables[0];
        var yName = variables[1];
        var a1 = equation1.GetCoefficient(xName);
        var b1 = equation1.GetCoefficient(yName);
        var c1 = equation1.Constant;
        var a2 = equation2.GetCoefficient(xName);
        var b2 = equation2.GetCoefficient(yName);
        var c2 = equation2.Constant;

        var determinant = a1 * b2 - a2 * b1;
        if (determinant == Zero)
        {
            var xConstantMinor = a1 * c2 - a2 * c1;
            var yConstantMinor = b1 * c2 - b2 * c1;
            if (xConstantMinor != Zero || yConstantMinor != Zero)
            {
                return new MathematicsSolveResult(
                    MathematicsSolveStatus.NoSolution,
                    null,
                    new EmptySolutionSet(),
                    request.Assumptions.ToArray(),
                    "linear-system.rank-classification",
                    new MathematicsSolutionTrace([]),
                    ProviderName,
                    ProviderVersion,
                    ["Coefficient determinant is zero while the augmented system is inconsistent; no ordered pair satisfies both equations."]);
            }

            return new MathematicsSolveResult(
                MathematicsSolveStatus.Indeterminate,
                null,
                null,
                request.Assumptions.ToArray(),
                "linear-system.rank-classification",
                new MathematicsSolutionTrace([]),
                ProviderName,
                ProviderVersion,
                ["Both equations are dependent; infinitely many ordered pairs satisfy the same affine relation."]);
        }

        var x = (b1 * c2 - c1 * b2) / determinant;
        var y = (c1 * a2 - a1 * c2) / determinant;
        var xNode = ToNode(x);
        var yNode = ToNode(y);
        var vector = new VectorNode([xNode, yNode]);

        var normalized = new EquationSystemNode([
            new EquationNode(
                new AddNode([
                    new MultiplyNode([ToNode(a1), new SymbolNode(xName)]),
                    new MultiplyNode([ToNode(b1), new SymbolNode(yName)]),
                    ToNode(c1)
                ]),
                new IntegerNode(0)),
            new EquationNode(
                new AddNode([
                    new MultiplyNode([ToNode(a2), new SymbolNode(xName)]),
                    new MultiplyNode([ToNode(b2), new SymbolNode(yName)]),
                    ToNode(c2)
                ]),
                new IntegerNode(0))
        ]);
        var solved = new EquationSystemNode([
            new EquationNode(new SymbolNode(xName), xNode),
            new EquationNode(new SymbolNode(yName), yNode)
        ]);

        var trace = new MathematicsSolutionTrace([
            new MathematicsSolutionStep(
                "normalize-two-equations",
                system,
                "linear_system.normalize_affine",
                "Move each equation to exact affine form a*x + b*y + c = 0.",
                normalized,
                "Equivalent terms are collected using exact rational arithmetic.",
                request.Assumptions.ToArray(),
                true),
            new MathematicsSolutionStep(
                "solve-nonsingular-system",
                normalized,
                "linear_system.cramer_exact",
                "Use the non-zero coefficient determinant to solve the two-variable system exactly.",
                solved,
                "A non-zero determinant guarantees one unique ordered-pair solution.",
                request.Assumptions.ToArray(),
                true)
        ]);

        return new MathematicsSolveResult(
            MathematicsSolveStatus.Solved,
            vector,
            new FiniteSolutionSet([vector]),
            request.Assumptions.ToArray(),
            "linear-system.cramer-exact",
            trace,
            ProviderName,
            ProviderVersion,
            []);
    }

    internal static MathNode ToNode(ExactRational value) =>
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

internal sealed class ExactAffineExpression
{
    private static readonly ExactRational Zero = new(0, 1);
    private static readonly ExactRational One = new(1, 1);

    private ExactAffineExpression(
        IReadOnlyDictionary<string, ExactRational> coefficients,
        ExactRational constant)
    {
        Coefficients = coefficients;
        Constant = constant;
    }

    public IReadOnlyDictionary<string, ExactRational> Coefficients { get; }
    public ExactRational Constant { get; }

    public ExactRational GetCoefficient(string variable) =>
        Coefficients.TryGetValue(variable, out var value) ? value : Zero;

    public static ExactAffineExpression operator -(ExactAffineExpression left, ExactAffineExpression right)
    {
        var keys = left.Coefficients.Keys
            .Concat(right.Coefficients.Keys)
            .Distinct(StringComparer.Ordinal);
        var result = new Dictionary<string, ExactRational>(StringComparer.Ordinal);
        foreach (var key in keys)
        {
            var coefficient = left.GetCoefficient(key) - right.GetCoefficient(key);
            if (coefficient != Zero)
            {
                result[key] = coefficient;
            }
        }

        return new ExactAffineExpression(result, left.Constant - right.Constant);
    }

    public static bool TryCreate(MathNode node, out ExactAffineExpression expression, out string? error)
    {
        ArgumentNullException.ThrowIfNull(node);

        switch (node)
        {
            case IntegerNode integer:
                expression = ConstantOnly(new ExactRational(integer.Value, 1));
                error = null;
                return true;
            case RationalNode rational:
                expression = ConstantOnly(rational.Value);
                error = null;
                return true;
            case SymbolNode symbol:
                expression = new ExactAffineExpression(
                    new Dictionary<string, ExactRational>(StringComparer.Ordinal)
                    {
                        [symbol.Name] = One
                    },
                    Zero);
                error = null;
                return true;
            case NegateNode negate:
                if (!TryCreate(negate.Operand, out var operand, out error))
                {
                    expression = null!;
                    return false;
                }

                expression = Scale(operand, new ExactRational(-1, 1));
                error = null;
                return true;
            case AddNode add:
                var sum = ConstantOnly(Zero);
                foreach (var term in add.Terms)
                {
                    if (!TryCreate(term, out var next, out error))
                    {
                        expression = null!;
                        return false;
                    }
                    sum = Add(sum, next);
                }
                expression = sum;
                error = null;
                return true;
            case MultiplyNode multiply:
                var constantFactor = One;
                ExactAffineExpression? variableFactor = null;
                foreach (var factor in multiply.Factors)
                {
                    if (!TryCreate(factor, out var next, out error))
                    {
                        expression = null!;
                        return false;
                    }
                    if (next.Coefficients.Count == 0)
                    {
                        constantFactor *= next.Constant;
                        continue;
                    }
                    if (variableFactor is not null)
                    {
                        expression = null!;
                        error = "Multiplication of two variable-containing expressions is non-linear.";
                        return false;
                    }
                    variableFactor = next;
                }
                expression = variableFactor is null
                    ? ConstantOnly(constantFactor)
                    : Scale(variableFactor, constantFactor);
                error = null;
                return true;
            case DivideNode divide:
                if (!TryCreate(divide.Numerator, out var numerator, out error)
                    || !TryCreate(divide.Denominator, out var denominator, out error))
                {
                    expression = null!;
                    return false;
                }
                if (denominator.Coefficients.Count != 0)
                {
                    expression = null!;
                    error = "Division by a variable-containing expression is outside the affine subset.";
                    return false;
                }
                if (denominator.Constant == Zero)
                {
                    expression = null!;
                    error = "Division by zero is invalid.";
                    return false;
                }
                expression = Scale(numerator, One / denominator.Constant);
                error = null;
                return true;
            default:
                expression = null!;
                error = $"Node type {node.GetType().Name} is outside the exact affine system subset.";
                return false;
        }
    }

    private static ExactAffineExpression ConstantOnly(ExactRational constant) =>
        new(new Dictionary<string, ExactRational>(StringComparer.Ordinal), constant);

    private static ExactAffineExpression Add(ExactAffineExpression left, ExactAffineExpression right)
    {
        var result = new Dictionary<string, ExactRational>(left.Coefficients, StringComparer.Ordinal);
        foreach (var pair in right.Coefficients)
        {
            var value = (result.TryGetValue(pair.Key, out var existing) ? existing : Zero) + pair.Value;
            if (value == Zero)
            {
                result.Remove(pair.Key);
            }
            else
            {
                result[pair.Key] = value;
            }
        }
        return new ExactAffineExpression(result, left.Constant + right.Constant);
    }

    private static ExactAffineExpression Scale(ExactAffineExpression expression, ExactRational scalar)
    {
        var coefficients = expression.Coefficients
            .Select(pair => new KeyValuePair<string, ExactRational>(pair.Key, pair.Value * scalar))
            .Where(pair => pair.Value != Zero)
            .ToDictionary(pair => pair.Key, pair => pair.Value, StringComparer.Ordinal);
        return new ExactAffineExpression(coefficients, expression.Constant * scalar);
    }
}
