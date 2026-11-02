using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Services.Mathematics.Solving;
using Edulytics.Services.Mathematics.Verification;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class ExactFunctionsGraphsSequencesBoundaryTests
{
    private static readonly BigInteger BoundaryValue = BigInteger.One << 4095;

    [Fact]
    public void FunctionEvaluation_Preserves4096BitValueThroughIdentityShapedOperations()
    {
        var solver = new ExactFunctionEvaluationSolver();
        var verifier = new ExactFunctionEvaluationVerifier();
        var boundary = new IntegerNode(BoundaryValue);
        MathNode[] expressions =
        [
            new AddNode([boundary, new IntegerNode(BigInteger.Zero)]),
            new MultiplyNode([boundary, new IntegerNode(BigInteger.One)]),
            new DivideNode(boundary, new IntegerNode(BigInteger.One)),
            new PowerNode(boundary, new IntegerNode(BigInteger.One))
        ];

        foreach (var expression in expressions)
        {
            var request = new MathematicsSolveRequest(
                new FunctionCallNode("evaluate_exact", [expression, new SymbolNode("x"), new IntegerNode(BigInteger.Zero)]),
                [],
                []);

            var result = solver.Solve(request);

            Assert.Equal(MathematicsSolveStatus.Solved, result.Status);
            Assert.Equal(boundary, result.ExactResult);
            Assert.True(verifier.Verify(request, result).IsVerified);
        }
    }

    [Fact]
    public void SequenceEvaluation_Preserves4096BitValueThroughIdentityCases()
    {
        var solver = new ExactSequenceTermSolver();
        var verifier = new ExactSequenceTermVerifier();
        var boundary = new IntegerNode(BoundaryValue);

        var arithmetic = new MathematicsSolveRequest(
            new FunctionCallNode(
                "arithmetic_nth_term",
                [boundary, new IntegerNode(BigInteger.Zero), new IntegerNode(new BigInteger(10000))]),
            [],
            []);
        var geometric = new MathematicsSolveRequest(
            new FunctionCallNode(
                "geometric_nth_term",
                [boundary, new IntegerNode(BigInteger.One), new IntegerNode(new BigInteger(64))]),
            [],
            []);

        var arithmeticResult = solver.Solve(arithmetic);
        var geometricResult = solver.Solve(geometric);

        Assert.Equal(MathematicsSolveStatus.Solved, arithmeticResult.Status);
        Assert.Equal(boundary, arithmeticResult.ExactResult);
        Assert.True(verifier.Verify(arithmetic, arithmeticResult).IsVerified);

        Assert.Equal(MathematicsSolveStatus.Solved, geometricResult.Status);
        Assert.Equal(boundary, geometricResult.ExactResult);
        Assert.True(verifier.Verify(geometric, geometricResult).IsVerified);
    }

    [Fact]
    public void FunctionEvaluation_StillRejectsResultAbove4096BitContract()
    {
        var boundary = new IntegerNode(BoundaryValue);
        var request = new MathematicsSolveRequest(
            new FunctionCallNode(
                "evaluate_exact",
                [new AddNode([boundary, boundary]), new SymbolNode("x"), new IntegerNode(BigInteger.Zero)]),
            [],
            []);

        var result = new ExactFunctionEvaluationSolver().Solve(request);

        Assert.Equal(MathematicsSolveStatus.ResourceLimit, result.Status);
    }
}
