using System.Numerics;
using System.Text.Json;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Services.Mathematics.Generation;
using Edulytics.Services.Mathematics.Solving;
using Edulytics.Services.Mathematics.Verification;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class ExactCalculusSolverTests
{
    [Fact]
    public void Derivative_NormalizesPolynomialAndVerifierRejectsMutation()
    {
        var x = new SymbolNode("x");
        var expression = new AddNode([
            new PowerNode(x, I(3)),
            new MultiplyNode([I(2), x]),
            I(5)
        ]);
        var request = Request(new DerivativeNode(expression, new SymbolNode("x"), 1));
        var solver = new ExactPolynomialDerivativeSolver();
        var verifier = new ExactPolynomialDerivativeVerifier();

        var result = solver.Solve(request);

        Assert.Equal(MathematicsSolveStatus.Solved, result.Status);
        var normalized = new ExactPolynomialNormalizer().Normalize(result.ExactResult!, "x", 8);
        Assert.True(normalized.IsSupported);
        Assert.Equal(new ExactRational(3, 1), normalized.Coefficients[2]);
        Assert.Equal(new ExactRational(2, 1), normalized.Coefficients[0]);
        Assert.True(verifier.Verify(request, result).IsVerified);

        var wrong = result with { ExactResult = I(0), SolutionSet = new FiniteSolutionSet([I(0)]) };
        Assert.False(verifier.Verify(request, wrong).IsVerified);
    }

    [Fact]
    public void Derivative_HandlesExpandedProductsThroughPolynomialNormalization()
    {
        var x = new SymbolNode("x");
        var expression = new MultiplyNode([
            new AddNode([x, I(1)]),
            new AddNode([new SymbolNode("x"), I(-1)])
        ]);
        var request = Request(new DerivativeNode(expression, new SymbolNode("x"), 1));
        var result = new ExactPolynomialDerivativeSolver().Solve(request);

        Assert.Equal(MathematicsSolveStatus.Solved, result.Status);
        var normalized = new ExactPolynomialNormalizer().Normalize(result.ExactResult!, "x", 8);
        Assert.True(normalized.IsSupported);
        Assert.Equal(new ExactRational(2, 1), normalized.Coefficients[1]);
        Assert.Single(normalized.Coefficients);
    }

    [Fact]
    public void DefiniteIntegral_EvaluatesExactlyIncludingRationalBounds()
    {
        var x = new SymbolNode("x");
        var solver = new ExactPolynomialDefiniteIntegralSolver();
        var verifier = new ExactPolynomialDefiniteIntegralVerifier();

        var squareRequest = Request(new IntegralNode(new PowerNode(x, I(2)), new SymbolNode("x"), I(0), I(3)));
        var squareResult = solver.Solve(squareRequest);
        Assert.Equal(I(9), squareResult.ExactResult);
        Assert.True(verifier.Verify(squareRequest, squareResult).IsVerified);

        var rationalRequest = Request(new IntegralNode(new SymbolNode("x"), new SymbolNode("x"), I(0), R(1, 2)));
        var rationalResult = solver.Solve(rationalRequest);
        Assert.Equal(R(1, 8), rationalResult.ExactResult);
        Assert.True(verifier.Verify(rationalRequest, rationalResult).IsVerified);
    }

    [Fact]
    public void Calculus_VerifierAcceptsBoundedConstantPowersAbovePolynomialDegree()
    {
        var constantPower = new PowerNode(I(2), I(9));

        var derivativeRequest = Request(new DerivativeNode(constantPower, new SymbolNode("x"), 1));
        var derivativeSolver = new ExactPolynomialDerivativeSolver();
        var derivativeVerifier = new ExactPolynomialDerivativeVerifier();
        var derivative = derivativeSolver.Solve(derivativeRequest);
        Assert.Equal(MathematicsSolveStatus.Solved, derivative.Status);
        Assert.Equal(I(0), derivative.ExactResult);
        Assert.True(derivativeVerifier.Verify(derivativeRequest, derivative).IsVerified);

        var integralRequest = Request(new IntegralNode(constantPower, new SymbolNode("x"), I(0), I(1)));
        var integralSolver = new ExactPolynomialDefiniteIntegralSolver();
        var integralVerifier = new ExactPolynomialDefiniteIntegralVerifier();
        var integral = integralSolver.Solve(integralRequest);
        Assert.Equal(MathematicsSolveStatus.Solved, integral.Status);
        Assert.Equal(I(512), integral.ExactResult);
        Assert.True(integralVerifier.Verify(integralRequest, integral).IsVerified);
    }

    [Fact]
    public void Calculus_FailsClosedOnUnsupportedMalformedAndOversizedInputs()
    {
        var derivativeSolver = new ExactPolynomialDerivativeSolver();
        var integralSolver = new ExactPolynomialDefiniteIntegralSolver();

        var secondOrder = Request(new DerivativeNode(new PowerNode(new SymbolNode("x"), I(3)), new SymbolNode("x"), 2));
        Assert.Equal(MathematicsSolveStatus.Unsupported, derivativeSolver.Solve(secondOrder).Status);

        var unexpectedSymbol = Request(new DerivativeNode(new SymbolNode("y"), new SymbolNode("x"), 1));
        Assert.Equal(MathematicsSolveStatus.Unsupported, derivativeSolver.Solve(unexpectedSymbol).Status);

        var malformed = Request(new DerivativeNode(new RationalNode(default(ExactRational)), new SymbolNode("x"), 1));
        Assert.Equal(MathematicsSolveStatus.Unsupported, derivativeSolver.Solve(malformed).Status);

        var oversized = Request(new DerivativeNode(new IntegerNode(BigInteger.One << 4097), new SymbolNode("x"), 1));
        Assert.Equal(MathematicsSolveStatus.ResourceLimit, derivativeSolver.Solve(oversized).Status);

        var indefinite = Request(new IntegralNode(new SymbolNode("x"), new SymbolNode("x")));
        Assert.Equal(MathematicsSolveStatus.Unsupported, integralSolver.Solve(indefinite).Status);
    }

    [Fact]
    public void Calculus_FailsClosedBeforeNestedConstantPowersCanExplodeBigIntegers()
    {
        MathNode nestedPower = new IntegerNode(BigInteger.One << 4095);
        for (var i = 0; i < 4; i++)
        {
            nestedPower = new PowerNode(nestedPower, I(32));
        }

        var derivativeRequest = Request(new DerivativeNode(nestedPower, new SymbolNode("x"), 1));
        var derivative = new ExactPolynomialDerivativeSolver().Solve(derivativeRequest);
        Assert.Equal(MathematicsSolveStatus.ResourceLimit, derivative.Status);

        var integralRequest = Request(new IntegralNode(nestedPower, new SymbolNode("x"), I(0), I(1)));
        var integral = new ExactPolynomialDefiniteIntegralSolver().Solve(integralRequest);
        Assert.Equal(MathematicsSolveStatus.ResourceLimit, integral.Status);
    }

    [Fact]
    public void DefiniteIntegralVerifier_RejectsMutatedAnswer()
    {
        var request = Request(new IntegralNode(new SymbolNode("x"), new SymbolNode("x"), I(0), I(2)));
        var solver = new ExactPolynomialDefiniteIntegralSolver();
        var verifier = new ExactPolynomialDefiniteIntegralVerifier();
        var result = solver.Solve(request);
        Assert.True(verifier.Verify(request, result).IsVerified);

        var wrong = result with { ExactResult = I(3), SolutionSet = new FiniteSolutionSet([I(3)]) };
        Assert.False(verifier.Verify(request, wrong).IsVerified);
    }

    private static MathematicsSolveRequest Request(MathNode problem) => new(problem, [], []);
    private static IntegerNode I(int value) => new(new BigInteger(value));
    private static RationalNode R(int numerator, int denominator) => new(new ExactRational(numerator, denominator));
}

public sealed class ExactCalculusQuestionFactoryTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void DerivativeFactory_IsVerifiedAndDeterministic(int difficultyBand)
    {
        var factory = new ExactPolynomialDerivativeQuestionFactory(
            new ExactPolynomialDerivativeSolver(),
            new ExactPolynomialDerivativeVerifier());
        AssertDeterministic(factory.Generate, difficultyBand, ExactPolynomialDerivativeQuestionFactory.FamilyId, "calculus.polynomial.differentiate");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void DefiniteIntegralFactory_IsVerifiedAndDeterministic(int difficultyBand)
    {
        var factory = new ExactPolynomialDefiniteIntegralQuestionFactory(
            new ExactPolynomialDefiniteIntegralSolver(),
            new ExactPolynomialDefiniteIntegralVerifier());
        AssertDeterministic(factory.Generate, difficultyBand, ExactPolynomialDefiniteIntegralQuestionFactory.FamilyId, "calculus.polynomial.integrate.definite");
    }

    private static void AssertDeterministic(
        Func<int, int, VerifiedGeneratedMathematicsProblem> generate,
        int difficultyBand,
        string expectedFamily,
        string expectedSkill)
    {
        var first = generate(20260917, difficultyBand);
        var second = generate(20260917, difficultyBand);
        Assert.Equal(expectedFamily, first.QuestionFamilyId);
        Assert.Equal(expectedSkill, first.Skill.Value);
        Assert.True(first.Verification.IsVerified);
        Assert.Equal(JsonSerializer.Serialize(first.Problem), JsonSerializer.Serialize(second.Problem));
        Assert.Equal(JsonSerializer.Serialize(first.ExpectedAnswer), JsonSerializer.Serialize(second.ExpectedAnswer));
    }
}
