using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Services.Mathematics.Contracts;

namespace Edulytics.Services.Mathematics.Solving;

/// <summary>
/// Exact-rational V2 solver for single-variable affine inequalities.
/// Supports expressions reducible to a*x+b relation c and returns a typed
/// interval/union solution set. The relation is reversed when dividing by a
/// negative coefficient. This provider is shadow-only until an explicit
/// curriculum readiness gate enables learner-facing routing.
/// </summary>
public sealed class ExactLinearInequalitySolver : IMathematicsSolver
{
    public const string ProviderName = "edulytics-native-linear-inequality";
    public const string ProviderVersion = "linear-inequality-exact-v1";

    public MathematicsSolveResult Solve(MathematicsSolveRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.Problem is not InequalityNode inequality)
        {
            return Unsupported("ExactLinearInequalitySolver only supports InequalityNode problems.");
        }

        if (!ExactLinearForm.TryCreate(inequality.Left, out var left, out var leftError))
        {
            return Unsupported(leftError ?? "Left side is outside the affine expression subset.");
        }

        if (!ExactLinearForm.TryCreate(inequality.Right, out var right, out var rightError))
        {
            return Unsupported(rightError ?? "Right side is outside the affine expression subset.");
        }

        if (!ExactLinearForm.TryMergeVariable(left.Variable, right.Variable, out var variable))
        {
            return Unsupported("Inequality contains more than one distinct variable.");
        }

        var coefficient = left.Coefficient - right.Coefficient;
        var constant = left.Constant - right.Constant;
        var zero = ExactLinearForm.Zero;

        if (coefficient == zero)
        {
            var isAlwaysTrue = EvaluateRelation(constant, inequality.Relation, zero);
            if (isAlwaysTrue)
            {
                return new MathematicsSolveResult(
                    MathematicsSolveStatus.Indeterminate,
                    inequality,
                    new AllRealNumbersSolutionSet(),
                    request.Assumptions.ToArray(),
                    "linear-inequality.normalize",
                    new MathematicsSolutionTrace([]),
                    ProviderName,
                    ProviderVersion,
                    ["Variable terms cancel and the remaining constant relation is true for every real value."]);
            }

            return new MathematicsSolveResult(
                MathematicsSolveStatus.NoSolution,
                inequality,
                new EmptySolutionSet(),
                request.Assumptions.ToArray(),
                "linear-inequality.normalize",
                new MathematicsSolutionTrace([]),
                ProviderName,
                ProviderVersion,
                ["Variable terms cancel and the remaining constant relation is false for every real value."]);
        }

        if (string.IsNullOrWhiteSpace(variable))
        {
            return Unsupported("Inequality does not contain a variable to solve for.");
        }

        var negativeConstant = new ExactRational(-constant.Numerator, constant.Denominator);
        var boundary = negativeConstant / coefficient;
        var solvedRelation = coefficient.CompareTo(zero) < 0
            ? Reverse(inequality.Relation)
            : inequality.Relation;

        var symbol = new SymbolNode(variable);
        var coefficientNode = ToNode(coefficient);
        var negativeConstantNode = ToNode(negativeConstant);
        var boundaryNode = ToNode(boundary);

        var normalized = new InequalityNode(
            new MultiplyNode([coefficientNode, symbol]),
            inequality.Relation,
            negativeConstantNode);
        var solved = new InequalityNode(symbol, solvedRelation, boundaryNode);
        var solutionSet = BuildSolutionSet(solvedRelation, boundaryNode);

        var trace = new MathematicsSolutionTrace(
        [
            new MathematicsSolutionStep(
                "collect-linear-terms",
                inequality,
                "linear_inequality.collect_terms",
                "Collect variable terms on one side and constants on the other.",
                normalized,
                "Equivalent affine terms are combined exactly using rational arithmetic.",
                request.Assumptions.ToArray(),
                true),
            new MathematicsSolutionStep(
                "divide-by-coefficient",
                normalized,
                coefficient.CompareTo(zero) < 0
                    ? "linear_inequality.divide_negative_and_reverse"
                    : "linear_inequality.divide_positive",
                coefficient.CompareTo(zero) < 0
                    ? "Divide both sides by the negative non-zero coefficient and reverse the inequality relation."
                    : "Divide both sides by the positive non-zero coefficient.",
                solved,
                coefficient.CompareTo(zero) < 0
                    ? "Dividing an inequality by a negative value reverses its order relation."
                    : "Dividing an inequality by a positive non-zero value preserves its order relation.",
                request.Assumptions.ToArray(),
                true)
        ]);

        return new MathematicsSolveResult(
            MathematicsSolveStatus.Solved,
            solved,
            solutionSet,
            request.Assumptions.ToArray(),
            "linear-inequality.collect-isolate-and-order",
            trace,
            ProviderName,
            ProviderVersion,
            []);
    }

    internal static InequalityRelation Reverse(InequalityRelation relation) => relation switch
    {
        InequalityRelation.LessThan => InequalityRelation.GreaterThan,
        InequalityRelation.LessThanOrEqual => InequalityRelation.GreaterThanOrEqual,
        InequalityRelation.GreaterThan => InequalityRelation.LessThan,
        InequalityRelation.GreaterThanOrEqual => InequalityRelation.LessThanOrEqual,
        InequalityRelation.NotEqual => InequalityRelation.NotEqual,
        _ => throw new ArgumentOutOfRangeException(nameof(relation), relation, "Unsupported inequality relation.")
    };

    internal static bool EvaluateRelation(
        ExactRational left,
        InequalityRelation relation,
        ExactRational right)
    {
        var comparison = left.CompareTo(right);
        return relation switch
        {
            InequalityRelation.LessThan => comparison < 0,
            InequalityRelation.LessThanOrEqual => comparison <= 0,
            InequalityRelation.GreaterThan => comparison > 0,
            InequalityRelation.GreaterThanOrEqual => comparison >= 0,
            InequalityRelation.NotEqual => comparison != 0,
            _ => false
        };
    }

    private static SolutionSet BuildSolutionSet(InequalityRelation relation, MathNode boundary) => relation switch
    {
        InequalityRelation.LessThan => new IntervalSolutionSet(
            null,
            boundary,
            IntervalBoundary.Open,
            IntervalBoundary.Open),
        InequalityRelation.LessThanOrEqual => new IntervalSolutionSet(
            null,
            boundary,
            IntervalBoundary.Open,
            IntervalBoundary.Closed),
        InequalityRelation.GreaterThan => new IntervalSolutionSet(
            boundary,
            null,
            IntervalBoundary.Open,
            IntervalBoundary.Open),
        InequalityRelation.GreaterThanOrEqual => new IntervalSolutionSet(
            boundary,
            null,
            IntervalBoundary.Closed,
            IntervalBoundary.Open),
        InequalityRelation.NotEqual => new UnionSolutionSet(
        [
            new IntervalSolutionSet(null, boundary, IntervalBoundary.Open, IntervalBoundary.Open),
            new IntervalSolutionSet(boundary, null, IntervalBoundary.Open, IntervalBoundary.Open)
        ]),
        _ => throw new ArgumentOutOfRangeException(nameof(relation), relation, "Unsupported inequality relation.")
    };

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
