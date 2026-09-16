using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Core.Mathematics.Verification;
using Edulytics.Services.Mathematics.Solving;
using Edulytics.Services.Mathematics.Verification;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class ExactLinearSystem2x2SolverTests
{
    private readonly ExactLinearSystem2x2Solver solver = new();
    private readonly ExactLinearSystem2x2Verifier verifier = new();

    [Fact]
    public void Solve_UniqueIntegerPair_ReturnsExactVectorAndVerifies()
    {
        var system = System(
            Equation(2, 1, 5),
            Equation(1, -1, 1));

        var result = solver.Solve(Request(system));

        Assert.Equal(MathematicsSolveStatus.Solved, result.Status);
        var vector = Assert.IsType<VectorNode>(result.ExactResult);
        Assert.Equal(new BigInteger(2), Assert.IsType<IntegerNode>(vector.Components[0]).Value);
        Assert.Equal(new BigInteger(1), Assert.IsType<IntegerNode>(vector.Components[1]).Value);
        Assert.Equal(MathematicsVerificationStatus.Verified, verifier.Verify(Request(system), result).Status);
    }

    [Fact]
    public void Solve_UniqueRationalPair_RemainsExact()
    {
        var system = System(
            Equation(1, 1, 1),
            Equation(1, -1, 0));

        var result = solver.Solve(Request(system));

        var vector = Assert.IsType<VectorNode>(result.ExactResult);
        Assert.Equal(new ExactRational(1, 2), Assert.IsType<RationalNode>(vector.Components[0]).Value);
        Assert.Equal(new ExactRational(1, 2), Assert.IsType<RationalNode>(vector.Components[1]).Value);
        Assert.True(verifier.Verify(Request(system), result).IsVerified);
    }

    [Fact]
    public void Solve_InconsistentParallelEquations_ReturnsNoSolution()
    {
        var system = System(
            Equation(1, 1, 1),
            Equation(2, 2, 3));

        var result = solver.Solve(Request(system));

        Assert.Equal(MathematicsSolveStatus.NoSolution, result.Status);
        Assert.IsType<EmptySolutionSet>(result.SolutionSet);
        Assert.True(verifier.Verify(Request(system), result).IsVerified);
    }

    [Fact]
    public void Solve_DependentEquations_ReturnsIndeterminate()
    {
        var system = System(
            Equation(1, 1, 1),
            Equation(2, 2, 2));

        var result = solver.Solve(Request(system));

        Assert.Equal(MathematicsSolveStatus.Indeterminate, result.Status);
        Assert.Null(result.ExactResult);
        Assert.True(verifier.Verify(Request(system), result).IsVerified);
    }

    [Fact]
    public void Verify_TamperedOrderedPair_IsRejected()
    {
        var system = System(
            Equation(2, 1, 5),
            Equation(1, -1, 1));
        var vector = new VectorNode([new IntegerNode(3), new IntegerNode(1)]);
        var tampered = new MathematicsSolveResult(
            MathematicsSolveStatus.Solved,
            vector,
            new FiniteSolutionSet([vector]),
            [],
            "tampered",
            null,
            "test",
            "1",
            []);

        Assert.Equal(
            MathematicsVerificationStatus.Rejected,
            verifier.Verify(Request(system), tampered).Status);
    }

    [Fact]
    public void Solve_NonlinearSystem_FailsClosed()
    {
        var nonlinear = new EquationNode(
            new MultiplyNode([new SymbolNode("x"), new SymbolNode("y")]),
            new IntegerNode(1));
        var system = System(nonlinear, Equation(1, 1, 2));

        Assert.Equal(MathematicsSolveStatus.Unsupported, solver.Solve(Request(system)).Status);
    }

    private static EquationSystemNode System(EquationNode first, EquationNode second) =>
        new([first, second]);

    private static EquationNode Equation(int a, int b, int c) =>
        new(
            new AddNode([
                new MultiplyNode([new IntegerNode(a), new SymbolNode("x")]),
                new MultiplyNode([new IntegerNode(b), new SymbolNode("y")])
            ]),
            new IntegerNode(c));

    private static MathematicsSolveRequest Request(MathNode problem) => new(problem, [], []);
}
