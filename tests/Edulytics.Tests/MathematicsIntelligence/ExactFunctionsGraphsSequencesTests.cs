using System.Numerics;
using System.Text.Json;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Services.Mathematics.Generation;
using Edulytics.Services.Mathematics.Solving;
using Edulytics.Services.Mathematics.Verification;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class ExactFunctionsGraphsSequencesSolverTests
{
    [Fact]
    public void FunctionEvaluation_IsExact_AndIndependentVerifierRejectsMutation()
    {
        var problem = new FunctionCallNode(
            "evaluate_exact",
            [
                new DivideNode(
                    new AddNode([
                        new MultiplyNode([I(3), new SymbolNode("x")]),
                        I(1)
                    ]),
                    I(2)),
                new SymbolNode("x"),
                R(1, 3)
            ]);
        var request = new MathematicsSolveRequest(problem, [], []);
        var solver = new ExactFunctionEvaluationSolver();
        var verifier = new ExactFunctionEvaluationVerifier();

        var result = solver.Solve(request);

        Assert.Equal(MathematicsSolveStatus.Solved, result.Status);
        Assert.Equal(I(1), result.ExactResult);
        Assert.True(verifier.Verify(request, result).IsVerified);

        var wrong = result with
        {
            ExactResult = I(99),
            SolutionSet = new FiniteSolutionSet([I(99)])
        };
        Assert.False(verifier.Verify(request, wrong).IsVerified);
    }

    [Fact]
    public void FunctionEvaluation_FailsClosedOnDivisionByZeroAndUnexpectedSymbol()
    {
        var solver = new ExactFunctionEvaluationSolver();

        var divideByZero = new MathematicsSolveRequest(
            new FunctionCallNode(
                "evaluate_exact",
                [new DivideNode(I(1), I(0)), new SymbolNode("x"), I(2)]),
            [],
            []);
        var unexpectedSymbol = new MathematicsSolveRequest(
            new FunctionCallNode(
                "evaluate_exact",
                [new AddNode([new SymbolNode("x"), new SymbolNode("y")]), new SymbolNode("x"), I(2)]),
            [],
            []);

        Assert.Equal(MathematicsSolveStatus.Unsupported, solver.Solve(divideByZero).Status);
        Assert.Equal(MathematicsSolveStatus.Unsupported, solver.Solve(unexpectedSymbol).Status);
    }

    [Fact]
    public void FunctionEvaluation_RejectsNestedPowerExplosionBeforeBigIntegerGrowth()
    {
        MathNode expression = I(2);
        for (var level = 0; level < 6; level++)
        {
            expression = new PowerNode(expression, I(12));
        }

        var request = new MathematicsSolveRequest(
            new FunctionCallNode("evaluate_exact", [expression, new SymbolNode("x"), I(0)]),
            [],
            []);
        var solver = new ExactFunctionEvaluationSolver();

        var result = solver.Solve(request);

        Assert.Equal(MathematicsSolveStatus.ResourceLimit, result.Status);
        Assert.Contains(result.Diagnostics, diagnostic => diagnostic.Contains("bit-length budget", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void GraphSampling_ProducesExactOrderedPairs_AndRejectsDuplicateInputs()
    {
        var expression = new AddNode([
            new PowerNode(new SymbolNode("x"), I(2)),
            I(1)
        ]);
        var request = new MathematicsSolveRequest(
            new FunctionCallNode(
                "sample_graph_exact",
                [expression, new SymbolNode("x"), new VectorNode([I(-1), I(0), I(2)])]),
            [],
            []);
        var solver = new ExactGraphSamplingSolver();
        var verifier = new ExactGraphSamplingVerifier();

        var result = solver.Solve(request);

        Assert.Equal(MathematicsSolveStatus.Solved, result.Status);
        var vector = Assert.IsType<VectorNode>(result.ExactResult);
        Assert.Equal(3, vector.Components.Count);
        Assert.True(verifier.Verify(request, result).IsVerified);

        var duplicateRequest = new MathematicsSolveRequest(
            new FunctionCallNode(
                "sample_graph_exact",
                [expression, new SymbolNode("x"), new VectorNode([I(1), I(1)])]),
            [],
            []);
        Assert.Equal(MathematicsSolveStatus.Unsupported, solver.Solve(duplicateRequest).Status);
    }

    [Fact]
    public void GraphVerifier_RejectsMutatedCoordinate()
    {
        var request = new MathematicsSolveRequest(
            new FunctionCallNode(
                "sample_graph_exact",
                [
                    new AddNode([new SymbolNode("x"), I(2)]),
                    new SymbolNode("x"),
                    new VectorNode([I(0), I(1)])
                ]),
            [],
            []);
        var solver = new ExactGraphSamplingSolver();
        var verifier = new ExactGraphSamplingVerifier();
        var result = solver.Solve(request);
        var exact = Assert.IsType<VectorNode>(result.ExactResult);
        var mutatedPoints = exact.Components.ToArray();
        mutatedPoints[1] = new VectorNode([I(1), I(999)]);
        var wrong = result with
        {
            ExactResult = new VectorNode(mutatedPoints),
            SolutionSet = new FiniteSolutionSet(mutatedPoints)
        };

        Assert.False(verifier.Verify(request, wrong).IsVerified);
    }

    [Fact]
    public void ArithmeticAndGeometricSequences_AreExactAndIndependentlyVerified()
    {
        var solver = new ExactSequenceTermSolver();
        var verifier = new ExactSequenceTermVerifier();

        var arithmetic = new MathematicsSolveRequest(
            new FunctionCallNode("arithmetic_nth_term", [R(1, 2), R(3, 2), I(5)]),
            [],
            []);
        var arithmeticResult = solver.Solve(arithmetic);
        Assert.Equal(R(13, 2), arithmeticResult.ExactResult);
        Assert.True(verifier.Verify(arithmetic, arithmeticResult).IsVerified);

        var geometric = new MathematicsSolveRequest(
            new FunctionCallNode("geometric_nth_term", [R(3, 2), I(-2), I(4)]),
            [],
            []);
        var geometricResult = solver.Solve(geometric);
        Assert.Equal(I(-12), geometricResult.ExactResult);
        Assert.True(verifier.Verify(geometric, geometricResult).IsVerified);
    }

    [Fact]
    public void SequenceSolver_EnforcesResourceBounds()
    {
        var solver = new ExactSequenceTermSolver();
        var arithmetic = new MathematicsSolveRequest(
            new FunctionCallNode("arithmetic_nth_term", [I(1), I(1), I(10001)]),
            [],
            []);
        var geometric = new MathematicsSolveRequest(
            new FunctionCallNode("geometric_nth_term", [I(1), I(2), I(65)]),
            [],
            []);

        Assert.Equal(MathematicsSolveStatus.ResourceLimit, solver.Solve(arithmetic).Status);
        Assert.Equal(MathematicsSolveStatus.ResourceLimit, solver.Solve(geometric).Status);
    }

    private static IntegerNode I(int value) => new(new BigInteger(value));
    private static RationalNode R(int numerator, int denominator) =>
        new(new ExactRational(new BigInteger(numerator), new BigInteger(denominator)));
}

public sealed class ExactFunctionsGraphsSequencesQuestionFactoryTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void FunctionFactory_ProducesVerifiedDeterministicProblem(int difficultyBand)
    {
        var factory = new ExactFunctionEvaluationQuestionFactory(
            new ExactFunctionEvaluationSolver(),
            new ExactFunctionEvaluationVerifier());

        var first = factory.Generate(20260916, difficultyBand);
        var second = factory.Generate(20260916, difficultyBand);

        Assert.Equal(ExactFunctionEvaluationQuestionFactory.FamilyId, first.QuestionFamilyId);
        Assert.Equal("functions.evaluate.exact", first.Skill.Value);
        Assert.True(first.Verification.IsVerified);
        Assert.Equal(JsonSerializer.Serialize(first.Problem), JsonSerializer.Serialize(second.Problem));
        Assert.Equal(JsonSerializer.Serialize(first.ExpectedAnswer), JsonSerializer.Serialize(second.ExpectedAnswer));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void GraphFactory_ProducesVerifiedUniqueCoordinateSet(int difficultyBand)
    {
        var factory = new ExactGraphSamplingQuestionFactory(
            new ExactGraphSamplingSolver(),
            new ExactGraphSamplingVerifier());

        var generated = factory.Generate(81173, difficultyBand);

        Assert.Equal(ExactGraphSamplingQuestionFactory.FamilyId, generated.QuestionFamilyId);
        Assert.Equal("functions.graph.sample.coordinates", generated.Skill.Value);
        Assert.True(generated.Verification.IsVerified);
        var vector = Assert.IsType<VectorNode>(generated.ExpectedAnswer);
        Assert.Equal(difficultyBand == 1 ? 3 : difficultyBand == 2 ? 5 : 7, vector.Components.Count);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void ArithmeticSequenceFactory_ProducesVerifiedProblem(int difficultyBand)
    {
        var factory = new ExactArithmeticSequenceQuestionFactory(
            new ExactSequenceTermSolver(),
            new ExactSequenceTermVerifier());

        var generated = factory.Generate(411, difficultyBand);

        Assert.Equal(ExactArithmeticSequenceQuestionFactory.FamilyId, generated.QuestionFamilyId);
        Assert.Equal("sequences.arithmetic.nth_term", generated.Skill.Value);
        Assert.True(generated.Verification.IsVerified);
        Assert.Equal(MathematicsSolveStatus.Solved, generated.SolveResult.Status);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void GeometricSequenceFactory_ProducesVerifiedProblem(int difficultyBand)
    {
        var factory = new ExactGeometricSequenceQuestionFactory(
            new ExactSequenceTermSolver(),
            new ExactSequenceTermVerifier());

        var generated = factory.Generate(977, difficultyBand);

        Assert.Equal(ExactGeometricSequenceQuestionFactory.FamilyId, generated.QuestionFamilyId);
        Assert.Equal("sequences.geometric.nth_term", generated.Skill.Value);
        Assert.True(generated.Verification.IsVerified);
        Assert.Equal(MathematicsSolveStatus.Solved, generated.SolveResult.Status);
    }
}
