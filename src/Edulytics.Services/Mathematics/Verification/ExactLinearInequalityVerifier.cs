using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Core.Mathematics.Verification;
using Edulytics.Services.Mathematics.Contracts;
using Edulytics.Services.Mathematics.Solving;

namespace Edulytics.Services.Mathematics.Verification;

/// <summary>
/// Independent verification for exact single-variable affine inequalities.
/// Verification does not replay the solver trace. It checks the claimed boundary
/// against the original inequality, validates points on both sides of the boundary,
/// and confirms that the typed interval/union solution set matches the normalized
/// solved relation.
/// </summary>
public sealed class ExactLinearInequalityVerifier : IMathematicsVerifier
{
    private static readonly ExactRational Zero = new(0, 1);
    private static readonly ExactRational One = new(1, 1);

    public MathematicsVerificationResult Verify(
        MathematicsSolveRequest request,
        MathematicsSolveResult result)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(result);

        if (request.Problem is not InequalityNode inequality)
        {
            return Unsupported("Verifier only supports InequalityNode problems.");
        }

        if (!ExactLinearForm.TryCreate(inequality.Left, out var left, out var leftError))
        {
            return Unsupported(leftError ?? "Left side is outside the affine subset.");
        }

        if (!ExactLinearForm.TryCreate(inequality.Right, out var right, out var rightError))
        {
            return Unsupported(rightError ?? "Right side is outside the affine subset.");
        }

        if (!ExactLinearForm.TryMergeVariable(left.Variable, right.Variable, out var variable))
        {
            return Unsupported("Inequality contains more than one distinct variable.");
        }

        var coefficient = left.Coefficient - right.Coefficient;
        var constant = left.Constant - right.Constant;

        if (coefficient == Zero)
        {
            var alwaysTrue = ExactLinearInequalitySolver.EvaluateRelation(
                constant,
                inequality.Relation,
                Zero);

            if (alwaysTrue)
            {
                return result.Status == MathematicsSolveStatus.Indeterminate
                       && result.SolutionSet is AllRealNumbersSolutionSet
                    ? Verified(
                        "constant-relation-all-reals",
                        "Variable terms cancel and the original constant inequality is true for every real value.")
                    : Rejected("Degenerate inequality is true for all reals, but the solver did not return the all-real solution set.");
            }

            return result.Status == MathematicsSolveStatus.NoSolution
                   && result.SolutionSet is EmptySolutionSet
                ? Verified(
                    "constant-relation-empty",
                    "Variable terms cancel and the original constant inequality is false for every real value.")
                : Rejected("Degenerate inequality is false for all reals, but the solver did not return the empty solution set.");
        }

        if (result.Status != MathematicsSolveStatus.Solved)
        {
            return Rejected($"Non-degenerate affine inequality returned unexpected solve status {result.Status}.");
        }

        if (string.IsNullOrWhiteSpace(variable))
        {
            return Rejected("Solved result was returned for an inequality with no variable.");
        }

        if (result.ExactResult is not InequalityNode solved
            || solved.Left is not SymbolNode solvedSymbol
            || !string.Equals(solvedSymbol.Name, variable, StringComparison.Ordinal)
            || !TryReadExactValue(solved.Right, out var boundary))
        {
            return Rejected("Solved result must be a normalized variable-to-exact-boundary inequality.");
        }

        if (!SolutionSetMatches(result.SolutionSet, solved.Relation, boundary))
        {
            return Rejected("Typed solution set does not match the normalized solved inequality.");
        }

        if (!TryEvaluateDifference(inequality, variable, boundary, out var boundaryDifference, out var boundaryError))
        {
            return Unsupported(boundaryError ?? "Could not evaluate the original inequality at the claimed boundary.");
        }

        if (boundaryDifference != Zero)
        {
            return Rejected("Claimed interval boundary is not the exact zero of the original affine difference.");
        }

        var below = boundary - One;
        var above = boundary + One;

        if (!TryEvaluateTruth(inequality, variable, below, out var originalBelow, out var belowError)
            || !TryEvaluateTruth(inequality, variable, above, out var originalAbove, out var aboveError))
        {
            return Unsupported(belowError ?? aboveError ?? "Could not evaluate points around the claimed boundary.");
        }

        var normalizedBelow = ExactLinearInequalitySolver.EvaluateRelation(below, solved.Relation, boundary);
        var normalizedAbove = ExactLinearInequalitySolver.EvaluateRelation(above, solved.Relation, boundary);
        if (originalBelow != normalizedBelow || originalAbove != normalizedAbove)
        {
            return Rejected("Normalized relation has the wrong orientation relative to the original inequality.");
        }

        if (!TryEvaluateTruth(inequality, variable, boundary, out var originalBoundaryTruth, out var boundaryTruthError))
        {
            return Unsupported(boundaryTruthError ?? "Could not evaluate boundary inclusion.");
        }

        var normalizedBoundaryTruth = ExactLinearInequalitySolver.EvaluateRelation(
            boundary,
            solved.Relation,
            boundary);
        if (originalBoundaryTruth != normalizedBoundaryTruth)
        {
            return Rejected("Boundary inclusion/exclusion does not match the original inequality relation.");
        }

        return Verified(
            "exact-boundary-and-sign-check",
            $"Boundary {boundary} is exact; original and normalized inequalities agree below, at, and above the boundary.");
    }

    private static bool TryEvaluateDifference(
        InequalityNode inequality,
        string variable,
        ExactRational value,
        out ExactRational difference,
        out string? error)
    {
        if (!ExactExpressionEvaluator.TryEvaluate(
                inequality.Left,
                variable,
                value,
                out var left,
                out error))
        {
            difference = default;
            return false;
        }

        if (!ExactExpressionEvaluator.TryEvaluate(
                inequality.Right,
                variable,
                value,
                out var right,
                out error))
        {
            difference = default;
            return false;
        }

        difference = left - right;
        error = null;
        return true;
    }

    private static bool TryEvaluateTruth(
        InequalityNode inequality,
        string variable,
        ExactRational value,
        out bool isTrue,
        out string? error)
    {
        if (!ExactExpressionEvaluator.TryEvaluate(
                inequality.Left,
                variable,
                value,
                out var left,
                out error))
        {
            isTrue = false;
            return false;
        }

        if (!ExactExpressionEvaluator.TryEvaluate(
                inequality.Right,
                variable,
                value,
                out var right,
                out error))
        {
            isTrue = false;
            return false;
        }

        isTrue = ExactLinearInequalitySolver.EvaluateRelation(left, inequality.Relation, right);
        error = null;
        return true;
    }

    private static bool SolutionSetMatches(
        SolutionSet? solutionSet,
        InequalityRelation relation,
        ExactRational boundary)
    {
        return relation switch
        {
            InequalityRelation.LessThan =>
                solutionSet is IntervalSolutionSet interval
                && interval.Lower is null
                && TryReadExactValue(interval.Upper, out var upper)
                && upper == boundary
                && interval.UpperBoundary == IntervalBoundary.Open,

            InequalityRelation.LessThanOrEqual =>
                solutionSet is IntervalSolutionSet interval
                && interval.Lower is null
                && TryReadExactValue(interval.Upper, out var upper)
                && upper == boundary
                && interval.UpperBoundary == IntervalBoundary.Closed,

            InequalityRelation.GreaterThan =>
                solutionSet is IntervalSolutionSet interval
                && TryReadExactValue(interval.Lower, out var lower)
                && lower == boundary
                && interval.Upper is null
                && interval.LowerBoundary == IntervalBoundary.Open,

            InequalityRelation.GreaterThanOrEqual =>
                solutionSet is IntervalSolutionSet interval
                && TryReadExactValue(interval.Lower, out var lower)
                && lower == boundary
                && interval.Upper is null
                && interval.LowerBoundary == IntervalBoundary.Closed,

            InequalityRelation.NotEqual =>
                solutionSet is UnionSolutionSet union
                && union.Sets.Count == 2
                && union.Sets[0] is IntervalSolutionSet left
                && left.Lower is null
                && TryReadExactValue(left.Upper, out var leftUpper)
                && leftUpper == boundary
                && left.UpperBoundary == IntervalBoundary.Open
                && union.Sets[1] is IntervalSolutionSet right
                && TryReadExactValue(right.Lower, out var rightLower)
                && rightLower == boundary
                && right.Upper is null
                && right.LowerBoundary == IntervalBoundary.Open,

            _ => false
        };
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
