using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Core.Mathematics.Verification;
using Edulytics.Services.Mathematics.Solving;
using Edulytics.Services.Mathematics.Verification;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class ExactLinearEquationSolverTests
{
    private readonly ExactLinearEquationSolver solver = new();
    private readonly ExactLinearEquationVerifier verifier = new();

    [Fact]
    public void Solve_AxPlusBEqualsC_ReturnsExactIntegerAndTrace()
    {
        var equation = new EquationNode(
            new AddNode([
                new MultiplyNode([new IntegerNode(2), new SymbolNode("x")]),
                new IntegerNode(3)
            ]),
            new IntegerNode(11));

        var result = solver.Solve(Request(equation));

        Assert.Equal(MathematicsSolveStatus.Solved, result.Status);
        var exact = Assert.IsType<IntegerNode>(result.ExactResult);
        Assert.Equal(new BigInteger(4), exact.Value);
        Assert.NotNull(result.Trace);
        Assert.Equal(2, result.Trace!.Steps.Count);
        Assert.True(verifier.Verify(Request(equation), result).IsVerified);
    }

    [Fact]
    public void Solve_VariableOnBothSides_IsolatesExactly()
    {
        var equation = new EquationNode(
            new AddNode([
                new MultiplyNode([new IntegerNode(3), new SymbolNode("x")]),
                new IntegerNode(2)
            ]),
            new AddNode([
                new SymbolNode("x"),
                new IntegerNode(8)
            ]));

        var result = solver.Solve(Request(equation));

        Assert.Equal(MathematicsSolveStatus.Solved, result.Status);
        Assert.Equal(new BigInteger(3), Assert.IsType<IntegerNode>(result.ExactResult).Value);
        Assert.True(verifier.Verify(Request(equation), result).IsVerified);
    }

    [Fact]
    public void Solve_RationalRoot_PreservesExactFraction()
    {
        var equation = new EquationNode(
            new MultiplyNode([new IntegerNode(3), new SymbolNode("x")]),
            new IntegerNode(1));

        var result = solver.Solve(Request(equation));

        var rational = Assert.IsType<RationalNode>(result.ExactResult);
        Assert.Equal(new ExactRational(1, 3), rational.Value);
        Assert.True(verifier.Verify(Request(equation), result).IsVerified);
    }

    [Fact]
    public void Solve_DivisionByConstant_RemainsInExactSubset()
    {
        var equation = new EquationNode(
            new AddNode([
                new DivideNode(new SymbolNode("x"), new IntegerNode(2)),
                new IntegerNode(1)
            ]),
            new IntegerNode(3));

        var result = solver.Solve(Request(equation));

        Assert.Equal(new BigInteger(4), Assert.IsType<IntegerNode>(result.ExactResult).Value);
        Assert.True(verifier.Verify(Request(equation), result).IsVerified);
    }

    [Fact]
    public void Solve_InconsistentEquation_ReturnsNoSolutionAndVerifierConfirms()
    {
        var equation = new EquationNode(
            new AddNode([
                new MultiplyNode([new IntegerNode(2), new SymbolNode("x")]),
                new IntegerNode(1)
            ]),
            new AddNode([
                new MultiplyNode([new IntegerNode(2), new SymbolNode("x")]),
                new IntegerNode(3)
            ]));

        var result = solver.Solve(Request(equation));

        Assert.Equal(MathematicsSolveStatus.NoSolution, result.Status);
        Assert.IsType<EmptySolutionSet>(result.SolutionSet);
        Assert.True(verifier.Verify(Request(equation), result).IsVerified);
    }

    [Fact]
    public void Solve_Identity_ReturnsAllRealsAndVerifierConfirms()
    {
        var left = new AddNode([
            new MultiplyNode([new IntegerNode(2), new SymbolNode("x")]),
            new IntegerNode(1)
        ]);
        var equation = new EquationNode(left, left);

        var result = solver.Solve(Request(equation));

        Assert.Equal(MathematicsSolveStatus.Indeterminate, result.Status);
        Assert.IsType<AllRealNumbersSolutionSet>(result.SolutionSet);
        Assert.True(verifier.Verify(Request(equation), result).IsVerified);
    }

    [Fact]
    public void Verify_TamperedSolvedIdentity_IsRejected()
    {
        var equation = new EquationNode(new SymbolNode("x"), new SymbolNode("x"));
        var tampered = new MathematicsSolveResult(
            MathematicsSolveStatus.Solved,
            new IntegerNode(5),
            new FiniteSolutionSet([new IntegerNode(5)]),
            [],
            "tampered",
            null,
            "test",
            "1",
            []);

        var verification = verifier.Verify(Request(equation), tampered);

        Assert.Equal(MathematicsVerificationStatus.Rejected, verification.Status);
    }

    [Fact]
    public void Solve_NonlinearProduct_FailsClosedAsUnsupported()
    {
        var equation = new EquationNode(
            new MultiplyNode([new SymbolNode("x"), new SymbolNode("x")]),
            new IntegerNode(4));

        var result = solver.Solve(Request(equation));

        Assert.Equal(MathematicsSolveStatus.Unsupported, result.Status);
        Assert.Null(result.ExactResult);
    }

    [Fact]
    public void Verify_TamperedCandidate_IsRejectedByOriginalEquationSubstitution()
    {
        var equation = new EquationNode(
            new AddNode([
                new MultiplyNode([new IntegerNode(2), new SymbolNode("x")]),
                new IntegerNode(3)
            ]),
            new IntegerNode(11));
        var tampered = new MathematicsSolveResult(
            MathematicsSolveStatus.Solved,
            new IntegerNode(5),
            new FiniteSolutionSet([new IntegerNode(5)]),
            [],
            "tampered",
            null,
            "test",
            "1",
            []);

        var verification = verifier.Verify(Request(equation), tampered);

        Assert.Equal(MathematicsVerificationStatus.Rejected, verification.Status);
    }

    [Fact]
    public void Solve_TwoDifferentVariables_FailsClosed()
    {
        var equation = new EquationNode(
            new AddNode([new SymbolNode("x"), new SymbolNode("y")]),
            new IntegerNode(3));

        var result = solver.Solve(Request(equation));

        Assert.Equal(MathematicsSolveStatus.Unsupported, result.Status);
    }

    private static MathematicsSolveRequest Request(MathNode problem) =>
        new(problem, [], []);
}
