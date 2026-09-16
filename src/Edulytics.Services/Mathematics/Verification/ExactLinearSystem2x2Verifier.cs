using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Core.Mathematics.Verification;
using Edulytics.Services.Mathematics.Contracts;
using Edulytics.Services.Mathematics.Solving;

namespace Edulytics.Services.Mathematics.Verification;

/// <summary>
/// Independent verifier for the exact 2x2 affine-system slice. Unique ordered
/// pairs are checked by substituting both coordinates into both original equations;
/// singular classifications are re-derived from coefficient/augmented minors.
/// </summary>
public sealed class ExactLinearSystem2x2Verifier : IMathematicsVerifier
{
    private static readonly ExactRational Zero = new(0, 1);

    public MathematicsVerificationResult Verify(
        MathematicsSolveRequest request,
        MathematicsSolveResult result)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(result);

        if (request.Problem is not EquationSystemNode system || system.Equations.Count != 2)
        {
            return Unsupported("Verifier requires exactly two equations.");
        }

        if (!TryNormalize(system.Equations[0], out var equation1, out var error)
            || !TryNormalize(system.Equations[1], out var equation2, out error))
        {
            return Unsupported(error ?? "System is outside the exact affine subset.");
        }

        var variables = equation1.Coefficients.Keys
            .Concat(equation2.Coefficients.Keys)
            .Distinct(StringComparer.Ordinal)
            .OrderBy(x => x, StringComparer.Ordinal)
            .ToArray();
        if (variables.Length != 2)
        {
            return Unsupported("Verifier requires exactly two distinct variables.");
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
            var inconsistent = a1 * c2 - a2 * c1 != Zero
                || b1 * c2 - b2 * c1 != Zero;
            if (inconsistent)
            {
                return result.Status == MathematicsSolveStatus.NoSolution
                       && result.SolutionSet is EmptySolutionSet
                    ? Verified(
                        "augmented-rank-inconsistency",
                        "Coefficient determinant is zero and an augmented minor is non-zero; the two lines have no common ordered pair.")
                    : Rejected("Singular inconsistent system was not classified as no-solution.");
            }

            return result.Status == MathematicsSolveStatus.Indeterminate
                   && result.ExactResult is null
                ? Verified(
                    "dependent-rank-classification",
                    "Coefficient and augmented minors are zero; the two affine equations are dependent and have infinitely many common points.")
                : Rejected("Dependent system was not classified as indeterminate.");
        }

        if (result.Status != MathematicsSolveStatus.Solved)
        {
            return Rejected("Non-singular 2x2 system must return one exact ordered-pair solution.");
        }

        if (result.ExactResult is not VectorNode vector || vector.Components.Count != 2
            || !TryReadExact(vector.Components[0], out var x)
            || !TryReadExact(vector.Components[1], out var y))
        {
            return Rejected("Solved 2x2 system must expose an exact two-component vector.");
        }

        if (result.SolutionSet is not FiniteSolutionSet finite
            || finite.Values.Count != 1
            || finite.Values[0] is not VectorNode finiteVector
            || finiteVector.Components.Count != 2
            || !TryReadExact(finiteVector.Components[0], out var finiteX)
            || !TryReadExact(finiteVector.Components[1], out var finiteY)
            || finiteX != x
            || finiteY != y)
        {
            return Rejected("Finite solution set does not match the reported ordered pair.");
        }

        var values = new Dictionary<string, ExactRational>(StringComparer.Ordinal)
        {
            [xName] = x,
            [yName] = y
        };

        for (var i = 0; i < system.Equations.Count; i++)
        {
            if (!ExactMultiSymbolEvaluator.TryEvaluate(
                    system.Equations[i].Left,
                    values,
                    out var left,
                    out var leftError))
            {
                return Unsupported(leftError ?? $"Could not evaluate left side of equation {i + 1}.");
            }
            if (!ExactMultiSymbolEvaluator.TryEvaluate(
                    system.Equations[i].Right,
                    values,
                    out var right,
                    out var rightError))
            {
                return Unsupported(rightError ?? $"Could not evaluate right side of equation {i + 1}.");
            }
            if (left != right)
            {
                return Rejected($"Reported ordered pair fails exact substitution in original equation {i + 1}.");
            }
        }

        return Verified(
            "exact-two-equation-substitution",
            $"Ordered pair ({x}, {y}) satisfies both original equations exactly; the non-zero determinant proves uniqueness.");
    }

    private static bool TryNormalize(
        EquationNode equation,
        out ExactAffineExpression normalized,
        out string? error)
    {
        if (!ExactAffineExpression.TryCreate(equation.Left, out var left, out error)
            || !ExactAffineExpression.TryCreate(equation.Right, out var right, out error))
        {
            normalized = null!;
            return false;
        }

        normalized = left - right;
        error = null;
        return true;
    }

    private static bool TryReadExact(MathNode node, out ExactRational value)
    {
        switch (node)
        {
            case IntegerNode integer:
                value = new ExactRational(integer.Value, 1);
                return true;
            case RationalNode rational:
                value = rational.Value;
                return true;
            default:
                value = default;
                return false;
        }
    }

    private static MathematicsVerificationResult Verified(string method, string description) =>
        new(
            MathematicsVerificationStatus.Verified,
            [new MathematicsVerificationEvidence(method, description)],
            []);

    private static MathematicsVerificationResult Rejected(string diagnostic) =>
        new(MathematicsVerificationStatus.Rejected, [], [diagnostic]);

    private static MathematicsVerificationResult Unsupported(string diagnostic) =>
        new(MathematicsVerificationStatus.Unsupported, [], [diagnostic]);
}

internal static class ExactMultiSymbolEvaluator
{
    private static readonly ExactRational Zero = new(0, 1);
    private static readonly ExactRational One = new(1, 1);

    public static bool TryEvaluate(
        MathNode node,
        IReadOnlyDictionary<string, ExactRational> values,
        out ExactRational result,
        out string? error)
    {
        switch (node)
        {
            case IntegerNode integer:
                result = new ExactRational(integer.Value, 1);
                error = null;
                return true;
            case RationalNode rational:
                result = rational.Value;
                error = null;
                return true;
            case SymbolNode symbol:
                if (!values.TryGetValue(symbol.Name, out result))
                {
                    error = $"No exact value was supplied for symbol {symbol.Name}.";
                    return false;
                }
                error = null;
                return true;
            case NegateNode negate:
                if (!TryEvaluate(negate.Operand, values, out var operand, out error))
                {
                    result = default;
                    return false;
                }
                result = new ExactRational(-operand.Numerator, operand.Denominator);
                error = null;
                return true;
            case AddNode add:
                var sum = Zero;
                foreach (var term in add.Terms)
                {
                    if (!TryEvaluate(term, values, out var termValue, out error))
                    {
                        result = default;
                        return false;
                    }
                    sum += termValue;
                }
                result = sum;
                error = null;
                return true;
            case MultiplyNode multiply:
                var product = One;
                foreach (var factor in multiply.Factors)
                {
                    if (!TryEvaluate(factor, values, out var factorValue, out error))
                    {
                        result = default;
                        return false;
                    }
                    product *= factorValue;
                }
                result = product;
                error = null;
                return true;
            case DivideNode divide:
                if (!TryEvaluate(divide.Numerator, values, out var numerator, out error)
                    || !TryEvaluate(divide.Denominator, values, out var denominator, out error))
                {
                    result = default;
                    return false;
                }
                if (denominator == Zero)
                {
                    result = default;
                    error = "Substitution produced division by zero.";
                    return false;
                }
                result = numerator / denominator;
                error = null;
                return true;
            default:
                result = default;
                error = $"Node type {node.GetType().Name} is unsupported by the exact multi-symbol evaluator.";
                return false;
        }
    }
}
