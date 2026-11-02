using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Core.Mathematics.Verification;
using Edulytics.Services.Mathematics.Contracts;
using Edulytics.Services.Mathematics.Solving;

namespace Edulytics.Services.Mathematics.Verification;

/// <summary>
/// Independent verification for the exact linear-equation vertical slice.
/// Solved roots are checked by substituting the candidate value into the original
/// equation rather than replaying the solver trace.
/// </summary>
public sealed class ExactLinearEquationVerifier : IMathematicsVerifier
{
    public MathematicsVerificationResult Verify(
        MathematicsSolveRequest request,
        MathematicsSolveResult result)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(result);

        if (request.Problem is not EquationNode equation)
        {
            return Unsupported("Verifier only supports EquationNode problems.");
        }

        if (!ExactLinearForm.TryCreate(equation.Left, out var left, out var leftError))
        {
            return Unsupported(leftError ?? "Left side is outside the affine subset.");
        }

        if (!ExactLinearForm.TryCreate(equation.Right, out var right, out var rightError))
        {
            return Unsupported(rightError ?? "Right side is outside the affine subset.");
        }

        if (!ExactLinearForm.TryMergeVariable(left.Variable, right.Variable, out var variable))
        {
            return Unsupported("Equation contains more than one distinct variable.");
        }

        var coefficient = left.Coefficient - right.Coefficient;
        var constant = left.Constant - right.Constant;
        var zero = ExactLinearForm.Zero;

        if (result.Status == MathematicsSolveStatus.NoSolution)
        {
            var valid = coefficient == zero && constant != zero;
            return valid
                ? Verified("affine-consistency", "Variable terms cancel and a non-zero constant remains.")
                : Rejected("Solver reported no solution, but the normalized affine equation does not prove inconsistency.");
        }

        if (result.Status == MathematicsSolveStatus.Indeterminate)
        {
            var valid = coefficient == zero && constant == zero;
            return valid
                ? Verified("affine-identity", "Both sides reduce to the same affine expression.")
                : Rejected("Solver reported an identity, but the normalized affine equation is not identically true.");
        }

        if (result.Status != MathematicsSolveStatus.Solved)
        {
            return Unsupported($"Verification for solve status {result.Status} is not implemented by this verifier.");
        }

        // A solved scalar result is only valid when the normalized equation has a
        // non-zero variable coefficient. Degenerate equations must be represented
        // as either NoSolution or Indeterminate; substitution alone is insufficient
        // to distinguish an identity from a genuine singleton solution.
        if (coefficient == zero)
        {
            return Rejected(
                "Solver reported a singleton solution for a degenerate equation; normalized variable coefficient is zero.");
        }

        if (string.IsNullOrWhiteSpace(variable))
        {
            return Rejected("Solved result was returned for an equation with no variable.");
        }

        if (!TryReadExactValue(result.ExactResult, out var candidate))
        {
            return Rejected("Solved result does not expose an exact integer or rational value.");
        }

        if (!ExactExpressionEvaluator.TryEvaluate(
                equation.Left,
                variable,
                candidate,
                out var leftValue,
                out var leftEvalError))
        {
            return Unsupported(leftEvalError ?? "Left side could not be evaluated exactly.");
        }

        if (!ExactExpressionEvaluator.TryEvaluate(
                equation.Right,
                variable,
                candidate,
                out var rightValue,
                out var rightEvalError))
        {
            return Unsupported(rightEvalError ?? "Right side could not be evaluated exactly.");
        }

        if (leftValue != rightValue)
        {
            return Rejected("Candidate solution does not satisfy the original equation by exact substitution.");
        }

        return Verified(
            "exact-substitution",
            $"Substitution into the original equation gives equal exact rational values ({leftValue}).");
    }

    private static bool TryReadExactValue(MathNode? node, out ExactRational value)
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
        new(
            MathematicsVerificationStatus.Rejected,
            [],
            [diagnostic]);

    private static MathematicsVerificationResult Unsupported(string diagnostic) =>
        new(
            MathematicsVerificationStatus.Unsupported,
            [],
            [diagnostic]);
}

internal static class ExactExpressionEvaluator
{
    private static readonly ExactRational Zero = new(0, 1);
    private static readonly ExactRational One = new(1, 1);

    public static bool TryEvaluate(
        MathNode node,
        string variableName,
        ExactRational variableValue,
        out ExactRational result,
        out string? error)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentException.ThrowIfNullOrWhiteSpace(variableName);

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
                if (!string.Equals(symbol.Name, variableName, StringComparison.Ordinal))
                {
                    result = default;
                    error = $"Unexpected symbol {symbol.Name}; verifier expected {variableName}.";
                    return false;
                }

                result = variableValue;
                error = null;
                return true;

            case NegateNode negate:
                if (!TryEvaluate(negate.Operand, variableName, variableValue, out var operand, out error))
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
                    if (!TryEvaluate(term, variableName, variableValue, out var termValue, out error))
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
                    if (!TryEvaluate(factor, variableName, variableValue, out var factorValue, out error))
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
                if (!TryEvaluate(
                        divide.Numerator,
                        variableName,
                        variableValue,
                        out var numerator,
                        out error))
                {
                    result = default;
                    return false;
                }

                if (!TryEvaluate(
                        divide.Denominator,
                        variableName,
                        variableValue,
                        out var denominator,
                        out error))
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
                error = $"Node type {node.GetType().Name} is not supported by the exact substitution verifier.";
                return false;
        }
    }
}
