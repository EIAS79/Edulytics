using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Core.Mathematics.Verification;
using Edulytics.Services.Mathematics.Solving;
using Edulytics.Services.Mathematics.Verification;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class ExactLinearInequalitySolverTests
{
    private readonly ExactLinearInequalitySolver solver = new();
    private readonly ExactLinearInequalityVerifier verifier = new();

    [Fact]
    public void Solve_PositiveCoefficient_ReturnsOpenUpperInterval()
    {
        var inequality = new InequalityNode(
            new AddNode([
                new MultiplyNode([new IntegerNode(2), new SymbolNode("x")]),
                new IntegerNode(3)
            ]),
            InequalityRelation.LessThan,
            new IntegerNode(11));

        var result = solver.Solve(Request(inequality));

        Assert.Equal(MathematicsSolveStatus.Solved, result.Status);
        var solved = Assert.IsType<InequalityNode>(result.ExactResult);
        Assert.Equal(InequalityRelation.LessThan, solved.Relation);
        Assert.Equal(new BigInteger(4), Assert.IsType<IntegerNode>(solved.Right).Value);

        var interval = Assert.IsType<IntervalSolutionSet>(result.SolutionSet);
        Assert.Null(interval.Lower);
        Assert.Equal(new BigInteger(4), Assert.IsType<IntegerNode>(interval.Upper).Value);
        Assert.Equal(IntervalBoundary.Open, interval.UpperBoundary);
        Assert.True(verifier.Verify(Request(inequality), result).IsVerified);
    }

    [Fact]
    public void Solve_NegativeCoefficient_ReversesRelation()
    {
        var inequality = new InequalityNode(
            new AddNode([
                new MultiplyNode([new IntegerNode(-2), new SymbolNode("x")]),
                new IntegerNode(3)
            ]),
            InequalityRelation.LessThanOrEqual,
            new IntegerNode(11));

        var result = solver.Solve(Request(inequality));

        var solved = Assert.IsType<InequalityNode>(result.ExactResult);
        Assert.Equal(InequalityRelation.GreaterThanOrEqual, solved.Relation);
        Assert.Equal(new BigInteger(-4), Assert.IsType<IntegerNode>(solved.Right).Value);
        var interval = Assert.IsType<IntervalSolutionSet>(result.SolutionSet);
        Assert.Equal(new BigInteger(-4), Assert.IsType<IntegerNode>(interval.Lower).Value);
        Assert.Equal(IntervalBoundary.Closed, interval.LowerBoundary);
        Assert.True(verifier.Verify(Request(inequality), result).IsVerified);
    }

    [Fact]
    public void Solve_RationalBoundary_PreservesExactValue()
    {
        var inequality = new InequalityNode(
            new MultiplyNode([new IntegerNode(3), new SymbolNode("x")]),
            InequalityRelation.GreaterThan,
            new IntegerNode(1));

        var result = solver.Solve(Request(inequality));

        var solved = Assert.IsType<InequalityNode>(result.ExactResult);
        Assert.Equal(new ExactRational(1, 3), Assert.IsType<RationalNode>(solved.Right).Value);
        Assert.True(verifier.Verify(Request(inequality), result).IsVerified);
    }

    [Fact]
    public void Solve_NotEqual_ReturnsTwoOpenIntervals()
    {
        var inequality = new InequalityNode(
            new AddNode([
                new MultiplyNode([new IntegerNode(3), new SymbolNode("x")]),
                new IntegerNode(6)
            ]),
            InequalityRelation.NotEqual,
            new IntegerNode(0));

        var result = solver.Solve(Request(inequality));

        var solved = Assert.IsType<InequalityNode>(result.ExactResult);
        Assert.Equal(InequalityRelation.NotEqual, solved.Relation);
        Assert.Equal(new BigInteger(-2), Assert.IsType<IntegerNode>(solved.Right).Value);
        var union = Assert.IsType<UnionSolutionSet>(result.SolutionSet);
        Assert.Equal(2, union.Sets.Count);
        Assert.True(verifier.Verify(Request(inequality), result).IsVerified);
    }

    [Fact]
    public void Solve_AlwaysTrueDegenerateInequality_ReturnsAllReals()
    {
        var inequality = new InequalityNode(
            new AddNode([new MultiplyNode([new IntegerNode(2), new SymbolNode("x")]), new IntegerNode(1)]),
            InequalityRelation.LessThan,
            new AddNode([new MultiplyNode([new IntegerNode(2), new SymbolNode("x")]), new IntegerNode(3)]));

        var result = solver.Solve(Request(inequality));

        Assert.Equal(MathematicsSolveStatus.Indeterminate, result.Status);
        Assert.IsType<AllRealNumbersSolutionSet>(result.SolutionSet);
        Assert.True(verifier.Verify(Request(inequality), result).IsVerified);
    }

    [Fact]
    public void Solve_AlwaysFalseDegenerateInequality_ReturnsEmptySet()
    {
        var inequality = new InequalityNode(
            new AddNode([new MultiplyNode([new IntegerNode(2), new SymbolNode("x")]), new IntegerNode(4)]),
            InequalityRelation.LessThan,
            new AddNode([new MultiplyNode([new IntegerNode(2), new SymbolNode("x")]), new IntegerNode(3)]));

        var result = solver.Solve(Request(inequality));

        Assert.Equal(MathematicsSolveStatus.NoSolution, result.Status);
        Assert.IsType<EmptySolutionSet>(result.SolutionSet);
        Assert.True(verifier.Verify(Request(inequality), result).IsVerified);
    }

    [Fact]
    public void Verify_TamperedOrientation_IsRejected()
    {
        var inequality = new InequalityNode(
            new MultiplyNode([new IntegerNode(-2), new SymbolNode("x")]),
            InequalityRelation.LessThan,
            new IntegerNode(8));
        var tamperedBoundary = new IntegerNode(-4);
        var tampered = new MathematicsSolveResult(
            MathematicsSolveStatus.Solved,
            new InequalityNode(new SymbolNode("x"), InequalityRelation.LessThan, tamperedBoundary),
            new IntervalSolutionSet(null, tamperedBoundary, IntervalBoundary.Open, IntervalBoundary.Open),
            [],
            "tampered",
            null,
            "test",
            "1",
            []);

        var verification = verifier.Verify(Request(inequality), tampered);

        Assert.Equal(MathematicsVerificationStatus.Rejected, verification.Status);
    }

    [Fact]
    public void Solve_NonlinearInequality_FailsClosed()
    {
        var inequality = new InequalityNode(
            new MultiplyNode([new SymbolNode("x"), new SymbolNode("x")]),
            InequalityRelation.LessThan,
            new IntegerNode(4));

        var result = solver.Solve(Request(inequality));

        Assert.Equal(MathematicsSolveStatus.Unsupported, result.Status);
    }

    private static MathematicsSolveRequest Request(MathNode problem) =>
        new(problem, [], []);
}
