using System.Numerics;
using System.Text.Json;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Services.Mathematics.Generation;
using Edulytics.Services.Mathematics.Solving;
using Edulytics.Services.Mathematics.Verification;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class ExactNumericalMethodsSolverTests
{
    [Fact]
    public void Bisection_ReturnsExactBracketAfterFixedIterations_AndRejectsMutation()
    {
        var solver = new ExactBisectionIterationSolver();
        var verifier = new ExactBisectionIterationVerifier();
        var x = new SymbolNode("x");
        var polynomial = new AddNode([new PowerNode(x, I(2)), new NegateNode(I(2))]);
        var request = Request(new FunctionCallNode("numerical_bisection_fixed_exact", [polynomial, x, I(1), I(2), I(2)]));
        var result = solver.Solve(request);

        Assert.Equal(MathematicsSolveStatus.Solved, result.Status);
        var bracket = Assert.IsType<VectorNode>(result.ExactResult);
        Assert.Equal(R(5, 4), bracket.Components[0]);
        Assert.Equal(R(3, 2), bracket.Components[1]);
        Assert.True(verifier.Verify(request, result).IsVerified);

        var wrongBracket = new VectorNode([I(1), I(2)]);
        var wrong = result with { ExactResult = wrongBracket, SolutionSet = new FiniteSolutionSet([wrongBracket]) };
        Assert.False(verifier.Verify(request, wrong).IsVerified);
    }

    [Fact]
    public void Bisection_FailsClosedWithoutBracket_AndCollapsesOnExactRoot()
    {
        var solver = new ExactBisectionIterationSolver();
        var x = new SymbolNode("x");
        var noRoot = new AddNode([new PowerNode(x, I(2)), I(1)]);
        var unsupported = solver.Solve(Request(new FunctionCallNode("numerical_bisection_fixed_exact", [noRoot, x, I(-1), I(1), I(3)])));
        Assert.Equal(MathematicsSolveStatus.Unsupported, unsupported.Status);

        var exactRoot = new AddNode([x, new NegateNode(I(1))]);
        var solved = solver.Solve(Request(new FunctionCallNode("numerical_bisection_fixed_exact", [exactRoot, x, I(0), I(2), I(4)])));
        var bracket = Assert.IsType<VectorNode>(solved.ExactResult);
        Assert.Equal(I(1), bracket.Components[0]);
        Assert.Equal(I(1), bracket.Components[1]);
    }

    [Fact]
    public void Newton_ReturnsExactRationalIterate_AndRejectsMutation()
    {
        var solver = new ExactNewtonIterationSolver();
        var verifier = new ExactNewtonIterationVerifier();
        var x = new SymbolNode("x");
        var polynomial = new AddNode([new PowerNode(x, I(2)), new NegateNode(I(2))]);
        var request = Request(new FunctionCallNode("numerical_newton_fixed_exact", [polynomial, x, I(1), I(2)]));
        var result = solver.Solve(request);

        Assert.Equal(R(17, 12), result.ExactResult);
        Assert.True(verifier.Verify(request, result).IsVerified);
        var wrong = result with { ExactResult = I(1), SolutionSet = new FiniteSolutionSet([I(1)]) };
        Assert.False(verifier.Verify(request, wrong).IsVerified);
    }

    [Fact]
    public void Newton_FailsClosedOnZeroDerivative()
    {
        var solver = new ExactNewtonIterationSolver();
        var x = new SymbolNode("x");
        var polynomial = new AddNode([new PowerNode(x, I(2)), I(1)]);
        var result = solver.Solve(Request(new FunctionCallNode("numerical_newton_fixed_exact", [polynomial, x, I(0), I(2)])));
        Assert.Equal(MathematicsSolveStatus.Unsupported, result.Status);
    }

    [Fact]
    public void TrapezoidalRule_ReturnsExactArithmeticEstimate_AndVerifierRejectsMutation()
    {
        var solver = new ExactTrapezoidalRuleSolver();
        var verifier = new ExactTrapezoidalRuleVerifier();
        var x = new SymbolNode("x");
        var polynomial = new PowerNode(x, I(2));
        var request = Request(new FunctionCallNode("numerical_trapezoidal_fixed_exact", [polynomial, x, I(0), I(2), I(2)]));
        var result = solver.Solve(request);

        Assert.Equal(I(3), result.ExactResult);
        Assert.True(verifier.Verify(request, result).IsVerified);
        var wrong = result with { ExactResult = R(8, 3), SolutionSet = new FiniteSolutionSet([R(8, 3)]) };
        Assert.False(verifier.Verify(request, wrong).IsVerified);
    }

    [Fact]
    public void NumericalMethods_RejectMalformedOversizedAndOutOfRangeInputs()
    {
        var x = new SymbolNode("x");
        var malformed = new RationalNode(default(ExactRational));
        var bisection = new ExactBisectionIterationSolver();
        Assert.Equal(MathematicsSolveStatus.Unsupported,
            bisection.Solve(Request(new FunctionCallNode("numerical_bisection_fixed_exact", [x, x, malformed, I(2), I(2)]))).Status);

        var huge = new IntegerNode(BigInteger.One << 5000);
        var newton = new ExactNewtonIterationSolver();
        Assert.Equal(MathematicsSolveStatus.ResourceLimit,
            newton.Solve(Request(new FunctionCallNode("numerical_newton_fixed_exact", [x, x, huge, I(2)]))).Status);

        var trapezoid = new ExactTrapezoidalRuleSolver();
        Assert.Equal(MathematicsSolveStatus.Unsupported,
            trapezoid.Solve(Request(new FunctionCallNode("numerical_trapezoidal_fixed_exact", [x, x, I(0), I(1), I(9)]))).Status);

        var unsupportedPower = new PowerNode(x, I(9));
        Assert.Equal(MathematicsSolveStatus.Unsupported,
            newton.Solve(Request(new FunctionCallNode("numerical_newton_fixed_exact", [unsupportedPower, x, I(1), I(1)]))).Status);
    }

    [Fact]
    public void NumericalMethods_RespectResourceBudgetDuringPolynomialEvaluation()
    {
        var x = new SymbolNode("x");
        MathNode expression = new IntegerNode(BigInteger.One << 4095);
        expression = new PowerNode(expression, I(8));
        var solver = new ExactNewtonIterationSolver();
        var result = solver.Solve(Request(new FunctionCallNode("numerical_newton_fixed_exact", [expression, x, I(1), I(1)])));
        Assert.Equal(MathematicsSolveStatus.ResourceLimit, result.Status);
    }

    private static MathematicsSolveRequest Request(MathNode problem) => new(problem, [], []);
    private static IntegerNode I(int value) => new(new BigInteger(value));
    private static RationalNode R(int numerator, int denominator) => new(new ExactRational(new BigInteger(numerator), new BigInteger(denominator)));
}

public sealed class ExactNumericalMethodsQuestionFactoryTests
{
    [Theory, InlineData(1), InlineData(2), InlineData(3)]
    public void BisectionFactory_IsVerifiedAndDeterministic(int band) => AssertDeterministic(
        new ExactBisectionIterationQuestionFactory(new ExactBisectionIterationSolver(), new ExactBisectionIterationVerifier()),
        (factory, seed, difficulty) => factory.Generate(seed, difficulty), band,
        ExactBisectionIterationQuestionFactory.FamilyId, "numerical.roots.bisection.iterate");

    [Theory, InlineData(1), InlineData(2), InlineData(3)]
    public void NewtonFactory_IsVerifiedAndDeterministic(int band) => AssertDeterministic(
        new ExactNewtonIterationQuestionFactory(new ExactNewtonIterationSolver(), new ExactNewtonIterationVerifier()),
        (factory, seed, difficulty) => factory.Generate(seed, difficulty), band,
        ExactNewtonIterationQuestionFactory.FamilyId, "numerical.roots.newton.iterate");

    [Theory, InlineData(1), InlineData(2), InlineData(3)]
    public void TrapezoidalFactory_IsVerifiedAndDeterministic(int band) => AssertDeterministic(
        new ExactTrapezoidalRuleQuestionFactory(new ExactTrapezoidalRuleSolver(), new ExactTrapezoidalRuleVerifier()),
        (factory, seed, difficulty) => factory.Generate(seed, difficulty), band,
        ExactTrapezoidalRuleQuestionFactory.FamilyId, "numerical.integration.trapezoidal.estimate");

    private static void AssertDeterministic<TFactory>(
        TFactory factory,
        Func<TFactory, int, int, VerifiedGeneratedMathematicsProblem> generate,
        int band,
        string family,
        string skill)
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
