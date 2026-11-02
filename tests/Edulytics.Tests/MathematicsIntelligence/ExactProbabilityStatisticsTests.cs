using System.Numerics;
using System.Text.Json;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Services.Mathematics.Generation;
using Edulytics.Services.Mathematics.Solving;
using Edulytics.Services.Mathematics.Verification;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class ExactProbabilityStatisticsSolverTests
{
    [Fact]
    public void SimpleProbability_IsExact_AndVerifierRejectsMutation()
    {
        var solver = new ExactSimpleProbabilitySolver();
        var verifier = new ExactSimpleProbabilityVerifier();
        var request = Request(new FunctionCallNode("probability_favourable_over_total_exact", [I(3), I(8)]));
        var result = solver.Solve(request);
        Assert.Equal(MathematicsSolveStatus.Solved, result.Status);
        Assert.Equal(R(3, 8), result.ExactResult);
        Assert.True(verifier.Verify(request, result).IsVerified);
        var wrong = result with { ExactResult = R(1, 2), SolutionSet = new FiniteSolutionSet([R(1, 2)]) };
        Assert.False(verifier.Verify(request, wrong).IsVerified);
    }

    [Fact]
    public void SimpleProbability_AcceptsBoundaryCounts_AndRejectsInvalidCounts()
    {
        var solver = new ExactSimpleProbabilitySolver();
        Assert.Equal(I(0), solver.Solve(Request(new FunctionCallNode("probability_favourable_over_total_exact", [I(0), I(7)]))).ExactResult);
        Assert.Equal(I(1), solver.Solve(Request(new FunctionCallNode("probability_favourable_over_total_exact", [I(7), I(7)]))).ExactResult);
        Assert.Equal(MathematicsSolveStatus.Unsupported, solver.Solve(Request(new FunctionCallNode("probability_favourable_over_total_exact", [I(1), I(0)]))).Status);
        Assert.Equal(MathematicsSolveStatus.Unsupported, solver.Solve(Request(new FunctionCallNode("probability_favourable_over_total_exact", [I(-1), I(5)]))).Status);
        Assert.Equal(MathematicsSolveStatus.Unsupported, solver.Solve(Request(new FunctionCallNode("probability_favourable_over_total_exact", [I(6), I(5)]))).Status);
        Assert.Equal(MathematicsSolveStatus.Unsupported, solver.Solve(Request(new FunctionCallNode("probability_favourable_over_total_exact", [R(1, 2), I(5)]))).Status);
    }

    [Fact]
    public void ComplementProbability_IsExact_AndFailsClosedOutsideProbabilityDomain()
    {
        var solver = new ExactProbabilityComplementSolver();
        var verifier = new ExactProbabilityComplementVerifier();
        var request = Request(new FunctionCallNode("probability_complement_exact", [R(2, 5)]));
        var result = solver.Solve(request);
        Assert.Equal(R(3, 5), result.ExactResult);
        Assert.True(verifier.Verify(request, result).IsVerified);
        Assert.Equal(I(1), solver.Solve(Request(new FunctionCallNode("probability_complement_exact", [I(0)]))).ExactResult);
        Assert.Equal(I(0), solver.Solve(Request(new FunctionCallNode("probability_complement_exact", [I(1)]))).ExactResult);
        Assert.Equal(MathematicsSolveStatus.Unsupported, solver.Solve(Request(new FunctionCallNode("probability_complement_exact", [R(-1, 4)]))).Status);
        Assert.Equal(MathematicsSolveStatus.Unsupported, solver.Solve(Request(new FunctionCallNode("probability_complement_exact", [R(5, 4)]))).Status);
        var malformed = new RationalNode(default(ExactRational));
        Assert.Equal(MathematicsSolveStatus.Unsupported, solver.Solve(Request(new FunctionCallNode("probability_complement_exact", [malformed]))).Status);
    }

    [Fact]
    public void ArithmeticMean_HandlesIntegerAndRationalDataExactly()
    {
        var solver = new ExactArithmeticMeanSolver();
        var verifier = new ExactArithmeticMeanVerifier();
        var integers = Request(new FunctionCallNode("statistics_mean_exact", [new VectorNode([I(1), I(2), I(6)])]));
        var integerResult = solver.Solve(integers);
        Assert.Equal(I(3), integerResult.ExactResult);
        Assert.True(verifier.Verify(integers, integerResult).IsVerified);

        var rationals = Request(new FunctionCallNode("statistics_mean_exact", [new VectorNode([R(1, 2), R(3, 2)])]));
        var rationalResult = solver.Solve(rationals);
        Assert.Equal(I(1), rationalResult.ExactResult);
        Assert.True(verifier.Verify(rationals, rationalResult).IsVerified);
    }

    [Fact]
    public void Mean_FailsClosedOnEmptyMalformedOversizedOrTooLongData()
    {
        var solver = new ExactArithmeticMeanSolver();
        Assert.Equal(MathematicsSolveStatus.Unsupported, solver.Solve(Request(new FunctionCallNode("statistics_mean_exact", [new VectorNode([])]))).Status);
        var malformed = new RationalNode(default(ExactRational));
        Assert.Equal(MathematicsSolveStatus.Unsupported, solver.Solve(Request(new FunctionCallNode("statistics_mean_exact", [new VectorNode([malformed])]))).Status);
        var tooLong = new VectorNode(Enumerable.Range(0, 9).Select(i => (MathNode)I(i)).ToArray());
        Assert.Equal(MathematicsSolveStatus.Unsupported, solver.Solve(Request(new FunctionCallNode("statistics_mean_exact", [tooLong]))).Status);
        var huge = new IntegerNode(BigInteger.One << 5000);
        Assert.Equal(MathematicsSolveStatus.ResourceLimit, solver.Solve(Request(new FunctionCallNode("statistics_mean_exact", [new VectorNode([huge])]))).Status);
    }

    [Fact]
    public void FrequencyMean_IsExact_AndRejectsInvalidTables()
    {
        var solver = new ExactFrequencyMeanSolver();
        var verifier = new ExactFrequencyMeanVerifier();
        var table = new MatrixNode([
            (IReadOnlyList<MathNode>)[I(2), I(1)],
            (IReadOnlyList<MathNode>)[I(5), I(3)]
        ]);
        var request = Request(new FunctionCallNode("statistics_frequency_mean_exact", [table]));
        var result = solver.Solve(request);
        Assert.Equal(R(17, 4), result.ExactResult);
        Assert.True(verifier.Verify(request, result).IsVerified);

        var negative = new MatrixNode([(IReadOnlyList<MathNode>)[I(2), I(-1)]]);
        Assert.Equal(MathematicsSolveStatus.Unsupported, solver.Solve(Request(new FunctionCallNode("statistics_frequency_mean_exact", [negative]))).Status);
        var zero = new MatrixNode([(IReadOnlyList<MathNode>)[I(2), I(0)], (IReadOnlyList<MathNode>)[I(5), I(0)]]);
        Assert.Equal(MathematicsSolveStatus.Unsupported, solver.Solve(Request(new FunctionCallNode("statistics_frequency_mean_exact", [zero]))).Status);
        var nonInteger = new MatrixNode([(IReadOnlyList<MathNode>)[I(2), R(1, 2)]]);
        Assert.Equal(MathematicsSolveStatus.Unsupported, solver.Solve(Request(new FunctionCallNode("statistics_frequency_mean_exact", [nonInteger]))).Status);
        var malformedShape = new MatrixNode([(IReadOnlyList<MathNode>)[I(2), I(1), I(3)]]);
        Assert.Equal(MathematicsSolveStatus.Unsupported, solver.Solve(Request(new FunctionCallNode("statistics_frequency_mean_exact", [malformedShape]))).Status);
    }

    private static MathematicsSolveRequest Request(MathNode problem) => new(problem, [], []);
    private static IntegerNode I(int value) => new(new BigInteger(value));
    private static RationalNode R(int numerator, int denominator) => new(new ExactRational(new BigInteger(numerator), new BigInteger(denominator)));
}

public sealed class ExactProbabilityStatisticsQuestionFactoryTests
{
    [Theory, InlineData(1), InlineData(2), InlineData(3)]
    public void SimpleProbabilityFactory_IsVerifiedAndDeterministic(int band) => AssertDeterministic(
        new ExactSimpleProbabilityQuestionFactory(new ExactSimpleProbabilitySolver(), new ExactSimpleProbabilityVerifier()), (f, s, b) => f.Generate(s, b), band,
        ExactSimpleProbabilityQuestionFactory.FamilyId, "probability.simple.evaluate.exact");

    [Theory, InlineData(1), InlineData(2), InlineData(3)]
    public void ComplementFactory_IsVerifiedAndDeterministic(int band) => AssertDeterministic(
        new ExactProbabilityComplementQuestionFactory(new ExactProbabilityComplementSolver(), new ExactProbabilityComplementVerifier()), (f, s, b) => f.Generate(s, b), band,
        ExactProbabilityComplementQuestionFactory.FamilyId, "probability.complement.evaluate.exact");

    [Theory, InlineData(1), InlineData(2), InlineData(3)]
    public void ArithmeticMeanFactory_IsVerifiedAndDeterministic(int band) => AssertDeterministic(
        new ExactArithmeticMeanQuestionFactory(new ExactArithmeticMeanSolver(), new ExactArithmeticMeanVerifier()), (f, s, b) => f.Generate(s, b), band,
        ExactArithmeticMeanQuestionFactory.FamilyId, "statistics.mean.arithmetic.exact");

    [Theory, InlineData(1), InlineData(2), InlineData(3)]
    public void FrequencyMeanFactory_IsVerifiedAndDeterministic(int band) => AssertDeterministic(
        new ExactFrequencyMeanQuestionFactory(new ExactFrequencyMeanSolver(), new ExactFrequencyMeanVerifier()), (f, s, b) => f.Generate(s, b), band,
        ExactFrequencyMeanQuestionFactory.FamilyId, "statistics.mean.frequency_table.exact");

    private static void AssertDeterministic<TFactory>(TFactory factory, Func<TFactory, int, int, VerifiedGeneratedMathematicsProblem> generate, int band, string family, string skill)
    {
        var first = generate(factory, 20260917, band);
        var second = generate(factory, 20260917, band);
        Assert.Equal(family, first.QuestionFamilyId);
        Assert.Equal(skill, first.Skill.Value);
        Assert.True(first.Verification.IsVerified);
        Assert.Equal(JsonSerializer.Serialize(first.Problem), JsonSerializer.Serialize(second.Problem));
        Assert.Equal(JsonSerializer.Serialize(first.ExpectedAnswer), JsonSerializer.Serialize(second.ExpectedAnswer));
    }
}
