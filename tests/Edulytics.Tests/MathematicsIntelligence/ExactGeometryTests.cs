using System.Numerics;
using System.Text.Json;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Services.Mathematics.Generation;
using Edulytics.Services.Mathematics.Solving;
using Edulytics.Services.Mathematics.Verification;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class ExactGeometrySolverTests
{
    [Fact]
    public void RectangleArea_UsesExactRationalArithmetic_AndVerifierRejectsMutation()
    {
        var solver = new ExactRectangleAreaSolver();
        var verifier = new ExactRectangleAreaVerifier();
        var request = Request(new FunctionCallNode("rectangle_area_exact", [R(3, 2), R(5, 3)]));

        var result = solver.Solve(request);

        Assert.Equal(MathematicsSolveStatus.Solved, result.Status);
        Assert.Equal(R(5, 2), result.ExactResult);
        Assert.True(verifier.Verify(request, result).IsVerified);

        var wrong = result with { ExactResult = I(3), SolutionSet = new FiniteSolutionSet([I(3)]) };
        Assert.False(verifier.Verify(request, wrong).IsVerified);
    }

    [Fact]
    public void RectanglePerimeter_AndTriangleArea_ReturnCanonicalExactScalars()
    {
        var perimeterSolver = new ExactRectanglePerimeterSolver();
        var perimeterVerifier = new ExactRectanglePerimeterVerifier();
        var perimeterRequest = Request(new FunctionCallNode("rectangle_perimeter_exact", [R(3, 2), I(2)]));
        var perimeter = perimeterSolver.Solve(perimeterRequest);
        Assert.Equal(I(7), perimeter.ExactResult);
        Assert.True(perimeterVerifier.Verify(perimeterRequest, perimeter).IsVerified);

        var triangleSolver = new ExactTriangleBaseHeightAreaSolver();
        var triangleVerifier = new ExactTriangleBaseHeightAreaVerifier();
        var triangleRequest = Request(new FunctionCallNode("triangle_area_base_height_exact", [I(3), I(5)]));
        var triangle = triangleSolver.Solve(triangleRequest);
        Assert.Equal(R(15, 2), triangle.ExactResult);
        Assert.True(triangleVerifier.Verify(triangleRequest, triangle).IsVerified);
    }

    [Fact]
    public void GeometryDimensions_FailClosedOnNonPositiveMalformedAndOversizedValues()
    {
        var solver = new ExactRectangleAreaSolver();
        var verifier = new ExactRectangleAreaVerifier();

        var zero = Request(new FunctionCallNode("rectangle_area_exact", [I(0), I(2)]));
        Assert.Equal(MathematicsSolveStatus.Unsupported, solver.Solve(zero).Status);

        var negative = Request(new FunctionCallNode("rectangle_area_exact", [I(-1), I(2)]));
        Assert.Equal(MathematicsSolveStatus.Unsupported, solver.Solve(negative).Status);

        var malformed = Request(new FunctionCallNode("rectangle_area_exact", [new RationalNode(default(ExactRational)), I(2)]));
        var malformedResult = solver.Solve(malformed);
        Assert.Equal(MathematicsSolveStatus.Unsupported, malformedResult.Status);
        Assert.False(verifier.Verify(malformed, malformedResult).IsVerified);

        var huge = new IntegerNode(BigInteger.One << 3000);
        var oversizedResult = solver.Solve(Request(new FunctionCallNode("rectangle_area_exact", [huge, huge])));
        Assert.Equal(MathematicsSolveStatus.ResourceLimit, oversizedResult.Status);
    }

    [Fact]
    public void Pythagorean_SolvesExactHypotenuseAndMissingLegIncludingRationalScale()
    {
        var solver = new ExactPythagoreanSolver();
        var verifier = new ExactPythagoreanVerifier();

        var hypotenuseRequest = Request(new FunctionCallNode("pythagorean_hypotenuse_exact", [I(3), I(4)]));
        var hypotenuse = solver.Solve(hypotenuseRequest);
        Assert.Equal(I(5), hypotenuse.ExactResult);
        Assert.True(verifier.Verify(hypotenuseRequest, hypotenuse).IsVerified);

        var legRequest = Request(new FunctionCallNode("pythagorean_leg_exact", [I(13), I(5)]));
        var leg = solver.Solve(legRequest);
        Assert.Equal(I(12), leg.ExactResult);
        Assert.True(verifier.Verify(legRequest, leg).IsVerified);

        var rationalRequest = Request(new FunctionCallNode("pythagorean_hypotenuse_exact", [R(3, 2), I(2)]));
        var rational = solver.Solve(rationalRequest);
        Assert.Equal(R(5, 2), rational.ExactResult);
        Assert.True(verifier.Verify(rationalRequest, rational).IsVerified);
    }

    [Fact]
    public void Pythagorean_FailsClosedOnIrrationalOrImpossibleGeometry_AndRejectsMutation()
    {
        var solver = new ExactPythagoreanSolver();
        var verifier = new ExactPythagoreanVerifier();

        var irrational = Request(new FunctionCallNode("pythagorean_hypotenuse_exact", [I(1), I(1)]));
        Assert.Equal(MathematicsSolveStatus.Unsupported, solver.Solve(irrational).Status);

        var impossible = Request(new FunctionCallNode("pythagorean_leg_exact", [I(5), I(5)]));
        Assert.Equal(MathematicsSolveStatus.Unsupported, solver.Solve(impossible).Status);

        var valid = Request(new FunctionCallNode("pythagorean_hypotenuse_exact", [I(5), I(12)]));
        var result = solver.Solve(valid);
        var wrong = result with { ExactResult = I(12), SolutionSet = new FiniteSolutionSet([I(12)]) };
        Assert.False(verifier.Verify(valid, wrong).IsVerified);
    }

    private static MathematicsSolveRequest Request(MathNode problem) => new(problem, [], []);
    private static IntegerNode I(int value) => new(new BigInteger(value));
    private static RationalNode R(int numerator, int denominator) =>
        new(new ExactRational(new BigInteger(numerator), new BigInteger(denominator)));
}

public sealed class ExactGeometryQuestionFactoryTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void RectangleAreaFactory_IsVerifiedAndDeterministic(int difficultyBand) =>
        AssertDeterministic(
            new ExactRectangleAreaQuestionFactory(new ExactRectangleAreaSolver(), new ExactRectangleAreaVerifier()),
            (factory, seed, band) => factory.Generate(seed, band), difficultyBand,
            ExactRectangleAreaQuestionFactory.FamilyId, "geometry.rectangle.area");

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void RectanglePerimeterFactory_IsVerifiedAndDeterministic(int difficultyBand) =>
        AssertDeterministic(
            new ExactRectanglePerimeterQuestionFactory(new ExactRectanglePerimeterSolver(), new ExactRectanglePerimeterVerifier()),
            (factory, seed, band) => factory.Generate(seed, band), difficultyBand,
            ExactRectanglePerimeterQuestionFactory.FamilyId, "geometry.rectangle.perimeter");

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void TriangleAreaFactory_IsVerifiedAndDeterministic(int difficultyBand) =>
        AssertDeterministic(
            new ExactTriangleBaseHeightAreaQuestionFactory(new ExactTriangleBaseHeightAreaSolver(), new ExactTriangleBaseHeightAreaVerifier()),
            (factory, seed, band) => factory.Generate(seed, band), difficultyBand,
            ExactTriangleBaseHeightAreaQuestionFactory.FamilyId, "geometry.triangle.area.base_height");

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void PythagoreanFactory_IsVerifiedAndDeterministic(int difficultyBand) =>
        AssertDeterministic(
            new ExactPythagoreanQuestionFactory(new ExactPythagoreanSolver(), new ExactPythagoreanVerifier()),
            (factory, seed, band) => factory.Generate(seed, band), difficultyBand,
            ExactPythagoreanQuestionFactory.FamilyId, "geometry.right_triangle.pythagorean");

    private static void AssertDeterministic<TFactory>(
        TFactory factory,
        Func<TFactory, int, int, VerifiedGeneratedMathematicsProblem> generate,
        int difficultyBand,
        string expectedFamily,
        string expectedSkill)
    {
        var first = generate(factory, 20260916, difficultyBand);
        var second = generate(factory, 20260916, difficultyBand);
        Assert.Equal(expectedFamily, first.QuestionFamilyId);
        Assert.Equal(expectedSkill, first.Skill.Value);
        Assert.True(first.Verification.IsVerified);
        Assert.Equal(JsonSerializer.Serialize(first.Problem), JsonSerializer.Serialize(second.Problem));
        Assert.Equal(JsonSerializer.Serialize(first.ExpectedAnswer), JsonSerializer.Serialize(second.ExpectedAnswer));
    }
}
