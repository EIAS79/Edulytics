using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Services.Mathematics.Solving;
using Edulytics.Services.Mathematics.Verification;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class ExactQuadraticEquationSolverTests
{
    private readonly ExactQuadraticEquationSolver solver = new();
    private readonly ExactQuadraticEquationVerifier verifier = new();

    [Fact]
    public void PolynomialNormalizer_ExpandsAndCollectsExactly()
    {
        var expression = new MultiplyNode([
            new AddNode([new SymbolNode("x"), I(2)]),
            new AddNode([new SymbolNode("x"), I(-3)])
        ]);

        var result = new ExactPolynomialNormalizer().Normalize(expression, "x", 8);

        Assert.True(result.IsSupported);
        Assert.Equal(2, result.Degree);
        Assert.Equal(new ExactRational(1, 1), result.Coefficients[2]);
        Assert.Equal(new ExactRational(-1, 1), result.Coefficients[1]);
        Assert.Equal(new ExactRational(-6, 1), result.Coefficients[0]);
        Assert.NotNull(result.CanonicalExpression);
    }

    [Fact]
    public void Solve_ReturnsTwoExactRationalRoots()
    {
        var request = Request(Quadratic(1, -5, 6));

        var result = solver.Solve(request);

        Assert.Equal(MathematicsSolveStatus.Solved, result.Status);
        var finite = Assert.IsType<FiniteSolutionSet>(result.SolutionSet);
        Assert.Equal(2, finite.Values.Count);
        Assert.Contains(finite.Values, node => node is IntegerNode integer && integer.Value == 2);
        Assert.Contains(finite.Values, node => node is IntegerNode integer && integer.Value == 3);
        Assert.True(verifier.Verify(request, result).IsVerified);
    }

    [Fact]
    public void Solve_ReturnsOneRepeatedRootWhenDiscriminantIsZero()
    {
        var request = Request(Quadratic(1, -4, 4));

        var result = solver.Solve(request);

        Assert.Equal(MathematicsSolveStatus.Solved, result.Status);
        var finite = Assert.IsType<FiniteSolutionSet>(result.SolutionSet);
        var root = Assert.Single(finite.Values);
        Assert.Equal(new IntegerNode(new BigInteger(2)), root);
        Assert.True(verifier.Verify(request, result).IsVerified);
    }

    [Fact]
    public void Solve_PreservesIrrationalRootsAsExactSurds()
    {
        var request = Request(Quadratic(1, 1, -1));

        var result = solver.Solve(request);

        Assert.Equal(MathematicsSolveStatus.Solved, result.Status);
        var finite = Assert.IsType<FiniteSolutionSet>(result.SolutionSet);
        Assert.Equal(2, finite.Values.Count);
        Assert.All(finite.Values, node => Assert.IsType<DivideNode>(node));
        Assert.True(verifier.Verify(request, result).IsVerified);
    }

    [Fact]
    public void Solve_ClassifiesNegativeDiscriminantAsNoRealSolution()
    {
        var request = Request(Quadratic(1, 1, 1));

        var result = solver.Solve(request);

        Assert.Equal(MathematicsSolveStatus.NoSolution, result.Status);
        Assert.IsType<EmptySolutionSet>(result.SolutionSet);
        Assert.Null(result.ExactResult);
        Assert.True(verifier.Verify(request, result).IsVerified);
    }

    [Fact]
    public void Solve_RejectsCubicInput()
    {
        var x = new SymbolNode("x");
        var equation = new EquationNode(
            new AddNode([
                new PowerNode(x, I(3)),
                I(-1)
            ]),
            I(0));

        var result = solver.Solve(Request(equation));

        Assert.Equal(MathematicsSolveStatus.Unsupported, result.Status);
    }

    [Fact]
    public void Verifier_RejectsTamperedRootSet()
    {
        var request = Request(Quadratic(1, -5, 6));
        var correct = solver.Solve(request);
        var tamperedRoots = new MathNode[] { I(2), I(4) };
        var tampered = correct with
        {
            ExactResult = new VectorNode(tamperedRoots),
            SolutionSet = new FiniteSolutionSet(tamperedRoots)
        };

        var verification = verifier.Verify(request, tampered);

        Assert.False(verification.IsVerified);
    }

    private static MathematicsSolveRequest Request(EquationNode equation) =>
        new(equation, [], []);

    private static EquationNode Quadratic(int a, int b, int c)
    {
        var x = new SymbolNode("x");
        return new EquationNode(
            new AddNode([
                new MultiplyNode([I(a), new PowerNode(x, I(2))]),
                new MultiplyNode([I(b), x]),
                I(c)
            ]),
            I(0));
    }

    private static IntegerNode I(int value) => new(new BigInteger(value));
}
