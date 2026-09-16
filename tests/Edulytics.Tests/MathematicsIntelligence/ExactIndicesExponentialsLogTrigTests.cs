using System.Numerics;
using System.Text.Json;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Services.Mathematics.Generation;
using Edulytics.Services.Mathematics.Solving;
using Edulytics.Services.Mathematics.Verification;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class ExactIndicesExponentialsLogTrigSolverTests
{
    [Fact]
    public void IndexPower_SupportsPositiveAndNegativeIntegerExponentsExactly()
    {
        var solver = new ExactIndexPowerSolver();
        var verifier = new ExactIndexPowerVerifier();

        var positive = Request(new FunctionCallNode("index_power_exact", [R(2, 3), I(3)]));
        var positiveResult = solver.Solve(positive);
        Assert.Equal(MathematicsSolveStatus.Solved, positiveResult.Status);
        Assert.Equal(R(8, 27), positiveResult.ExactResult);
        Assert.True(verifier.Verify(positive, positiveResult).IsVerified);

        var negative = Request(new FunctionCallNode("index_power_exact", [I(2), I(-3)]));
        var negativeResult = solver.Solve(negative);
        Assert.Equal(R(1, 8), negativeResult.ExactResult);
        Assert.True(verifier.Verify(negative, negativeResult).IsVerified);
    }

    [Fact]
    public void IndexPower_VerifierRejectsMutation_AndSolverFailsClosedOnUndefinedOrOversizedCases()
    {
        var solver = new ExactIndexPowerSolver();
        var verifier = new ExactIndexPowerVerifier();
        var request = Request(new FunctionCallNode("index_power_exact", [I(-3), I(3)]));
        var result = solver.Solve(request);
        var wrong = result with
        {
            ExactResult = I(27),
            SolutionSet = new FiniteSolutionSet([I(27)])
        };

        Assert.False(verifier.Verify(request, wrong).IsVerified);

        var zeroNegative = Request(new FunctionCallNode("index_power_exact", [I(0), I(-1)]));
        Assert.Equal(MathematicsSolveStatus.Unsupported, solver.Solve(zeroNegative).Status);

        var hugeBase = new IntegerNode((BigInteger.One << 4095) + BigInteger.One);
        var oversized = Request(new FunctionCallNode("index_power_exact", [hugeBase, I(2)]));
        Assert.Equal(MathematicsSolveStatus.ResourceLimit, solver.Solve(oversized).Status);
    }

    [Fact]
    public void IndexPower_FailsClosedOnMalformedZeroDenominatorRational()
    {
        var solver = new ExactIndexPowerSolver();
        var verifier = new ExactIndexPowerVerifier();
        var malformed = new RationalNode(default);

        foreach (var exponent in new[] { 0, 2 })
        {
            var request = Request(new FunctionCallNode("index_power_exact", [malformed, I(exponent)]));
            var exception = Record.Exception(() => solver.Solve(request));
            Assert.Null(exception);

            var result = solver.Solve(request);
            Assert.Equal(MathematicsSolveStatus.Unsupported, result.Status);
            Assert.False(verifier.Verify(request, result).IsVerified);
        }
    }

    [Fact]
    public void ExponentialSameBase_SolvesExactIntegerExponent_AndRejectsNonPower()
    {
        var solver = new ExactExponentialSameBaseSolver();
        var verifier = new ExactExponentialSameBaseVerifier();
        var request = Request(new FunctionCallNode("solve_exponential_same_base", [I(3), R(1, 9)]));

        var result = solver.Solve(request);

        Assert.Equal(MathematicsSolveStatus.Solved, result.Status);
        Assert.Equal(I(-2), result.ExactResult);
        Assert.True(verifier.Verify(request, result).IsVerified);

        var wrong = result with
        {
            ExactResult = I(2),
            SolutionSet = new FiniteSolutionSet([I(2)])
        };
        Assert.False(verifier.Verify(request, wrong).IsVerified);

        var notPower = Request(new FunctionCallNode("solve_exponential_same_base", [I(2), I(3)]));
        Assert.Equal(MathematicsSolveStatus.Unsupported, solver.Solve(notPower).Status);

        var invalidBase = Request(new FunctionCallNode("solve_exponential_same_base", [I(1), I(1)]));
        Assert.Equal(MathematicsSolveStatus.Unsupported, solver.Solve(invalidBase).Status);
    }

    [Fact]
    public void Logarithm_EvaluatesOnlyExactIntegerPowerRelationships()
    {
        var solver = new ExactLogarithmSolver();
        var verifier = new ExactLogarithmVerifier();
        var request = Request(new FunctionCallNode("log_exact", [I(4), R(1, 64)]));

        var result = solver.Solve(request);

        Assert.Equal(MathematicsSolveStatus.Solved, result.Status);
        Assert.Equal(I(-3), result.ExactResult);
        Assert.True(verifier.Verify(request, result).IsVerified);

        var nonExact = Request(new FunctionCallNode("log_exact", [I(10), I(2)]));
        Assert.Equal(MathematicsSolveStatus.Unsupported, solver.Solve(nonExact).Status);

        var invalidArgument = Request(new FunctionCallNode("log_exact", [I(2), I(0)]));
        Assert.Equal(MathematicsSolveStatus.Unsupported, solver.Solve(invalidArgument).Status);
    }

    [Fact]
    public void SpecialAngleTrig_ReturnsCanonicalExactValuesAndNormalizesDegrees()
    {
        var solver = new ExactSpecialAngleTrigonometrySolver();
        var verifier = new ExactSpecialAngleTrigonometryVerifier();

        var sin30 = Request(new FunctionCallNode("sin_degrees_exact", [I(30)]));
        var sin30Result = solver.Solve(sin30);
        Assert.Equal(R(1, 2), sin30Result.ExactResult);
        Assert.True(verifier.Verify(sin30, sin30Result).IsVerified);

        var cos150 = Request(new FunctionCallNode("cos_degrees_exact", [I(150)]));
        var cos150Result = solver.Solve(cos150);
        Assert.Equal(Neg(SqrtOver(3, 2)), cos150Result.ExactResult);
        Assert.True(verifier.Verify(cos150, cos150Result).IsVerified);

        var tan60 = Request(new FunctionCallNode("tan_degrees_exact", [I(60)]));
        var tan60Result = solver.Solve(tan60);
        Assert.Equal(new RootNode(I(3), 2), tan60Result.ExactResult);
        Assert.True(verifier.Verify(tan60, tan60Result).IsVerified);

        var sinNegative45 = Request(new FunctionCallNode("sin_degrees_exact", [I(-45)]));
        var normalizedResult = solver.Solve(sinNegative45);
        Assert.Equal(Neg(SqrtOver(2, 2)), normalizedResult.ExactResult);
        Assert.True(verifier.Verify(sinNegative45, normalizedResult).IsVerified);
    }

    [Fact]
    public void SpecialAngleTrig_EmitsCanonicalNegativeHalfRationals()
    {
        var solver = new ExactSpecialAngleTrigonometrySolver();
        var verifier = new ExactSpecialAngleTrigonometryVerifier();

        foreach (var request in new[]
        {
            Request(new FunctionCallNode("sin_degrees_exact", [I(210)])),
            Request(new FunctionCallNode("sin_degrees_exact", [I(330)])),
            Request(new FunctionCallNode("cos_degrees_exact", [I(120)])),
            Request(new FunctionCallNode("cos_degrees_exact", [I(240)]))
        })
        {
            var result = solver.Solve(request);
            Assert.Equal(MathematicsSolveStatus.Solved, result.Status);
            var rational = Assert.IsType<RationalNode>(result.ExactResult);
            Assert.Equal(new ExactRational(BigInteger.MinusOne, new BigInteger(2)), rational.Value);
            Assert.True(verifier.Verify(request, result).IsVerified);
        }
    }

    [Fact]
    public void SpecialAngleTrig_FailsClosedOnUndefinedAndUnsupportedAngles_AndRejectsMutation()
    {
        var solver = new ExactSpecialAngleTrigonometrySolver();
        var verifier = new ExactSpecialAngleTrigonometryVerifier();

        var undefined = Request(new FunctionCallNode("tan_degrees_exact", [I(90)]));
        Assert.Equal(MathematicsSolveStatus.Unsupported, solver.Solve(undefined).Status);

        var unsupported = Request(new FunctionCallNode("sin_degrees_exact", [I(10)]));
        Assert.Equal(MathematicsSolveStatus.Unsupported, solver.Solve(unsupported).Status);

        var valid = Request(new FunctionCallNode("cos_degrees_exact", [I(60)]));
        var result = solver.Solve(valid);
        var wrong = result with
        {
            ExactResult = I(1),
            SolutionSet = new FiniteSolutionSet([I(1)])
        };
        Assert.False(verifier.Verify(valid, wrong).IsVerified);
    }

    private static MathematicsSolveRequest Request(MathNode problem) => new(problem, [], []);
    private static IntegerNode I(int value) => new(new BigInteger(value));
    private static RationalNode R(int numerator, int denominator) =>
        new(new ExactRational(new BigInteger(numerator), new BigInteger(denominator)));
    private static MathNode SqrtOver(int radicand, int denominator) =>
        new DivideNode(new RootNode(I(radicand), 2), I(denominator));
    private static MathNode Neg(MathNode value) => new NegateNode(value);
}

public sealed class ExactIndicesExponentialsLogTrigQuestionFactoryTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void IndexFactory_ProducesVerifiedDeterministicProblem(int difficultyBand)
    {
        var factory = new ExactIndexPowerQuestionFactory(new ExactIndexPowerSolver(), new ExactIndexPowerVerifier());

        var first = factory.Generate(20260916, difficultyBand);
        var second = factory.Generate(20260916, difficultyBand);

        Assert.Equal(ExactIndexPowerQuestionFactory.FamilyId, first.QuestionFamilyId);
        Assert.Equal("indices.integer_power.evaluate", first.Skill.Value);
        Assert.True(first.Verification.IsVerified);
        Assert.Equal(JsonSerializer.Serialize(first.Problem), JsonSerializer.Serialize(second.Problem));
        Assert.Equal(JsonSerializer.Serialize(first.ExpectedAnswer), JsonSerializer.Serialize(second.ExpectedAnswer));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void ExponentialFactory_ProducesVerifiedDeterministicProblem(int difficultyBand)
    {
        var factory = new ExactExponentialSameBaseQuestionFactory(
            new ExactExponentialSameBaseSolver(),
            new ExactExponentialSameBaseVerifier());

        var first = factory.Generate(31337, difficultyBand);
        var second = factory.Generate(31337, difficultyBand);

        Assert.Equal(ExactExponentialSameBaseQuestionFactory.FamilyId, first.QuestionFamilyId);
        Assert.Equal("exponentials.same_base.solve", first.Skill.Value);
        Assert.True(first.Verification.IsVerified);
        Assert.Equal(JsonSerializer.Serialize(first.Problem), JsonSerializer.Serialize(second.Problem));
        Assert.Equal(JsonSerializer.Serialize(first.ExpectedAnswer), JsonSerializer.Serialize(second.ExpectedAnswer));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void LogarithmFactory_ProducesVerifiedDeterministicProblem(int difficultyBand)
    {
        var factory = new ExactLogarithmQuestionFactory(new ExactLogarithmSolver(), new ExactLogarithmVerifier());

        var first = factory.Generate(771, difficultyBand);
        var second = factory.Generate(771, difficultyBand);

        Assert.Equal(ExactLogarithmQuestionFactory.FamilyId, first.QuestionFamilyId);
        Assert.Equal("logarithms.evaluate.exact", first.Skill.Value);
        Assert.True(first.Verification.IsVerified);
        Assert.Equal(JsonSerializer.Serialize(first.Problem), JsonSerializer.Serialize(second.Problem));
        Assert.Equal(JsonSerializer.Serialize(first.ExpectedAnswer), JsonSerializer.Serialize(second.ExpectedAnswer));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void TrigonometryFactory_ProducesVerifiedDeterministicProblem(int difficultyBand)
    {
        var factory = new ExactSpecialAngleTrigonometryQuestionFactory(
            new ExactSpecialAngleTrigonometrySolver(),
            new ExactSpecialAngleTrigonometryVerifier());

        var first = factory.Generate(991, difficultyBand);
        var second = factory.Generate(991, difficultyBand);

        Assert.Equal(ExactSpecialAngleTrigonometryQuestionFactory.FamilyId, first.QuestionFamilyId);
        Assert.Equal("trigonometry.special_angles.evaluate", first.Skill.Value);
        Assert.True(first.Verification.IsVerified);
        Assert.Equal(JsonSerializer.Serialize(first.Problem), JsonSerializer.Serialize(second.Problem));
        Assert.Equal(JsonSerializer.Serialize(first.ExpectedAnswer), JsonSerializer.Serialize(second.ExpectedAnswer));
    }
}
