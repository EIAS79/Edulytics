using System.Numerics;
using System.Text.Json;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Services.Mathematics.Generation;
using Edulytics.Services.Mathematics.Solving;
using Edulytics.Services.Mathematics.Verification;
using Edulytics.Services.Mathematics.Visuals;
using Edulytics.Web.Presentation;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class ExactAreaBetweenCurvesTests
{
    [Fact]
    public void SolverSplitsAtIntersectionsAndComputesGeometricArea()
    {
        // f(x) = 4 - x^2, g(x) = 0, interval [-3, 3].
        // Exact intersections are -2 and 2. Geometric area is 46/3.
        var problem =
            new AreaBetweenCurvesNode(
                new AddNode(
                    [
                        I(4),
                        new NegateNode(
                            new PowerNode(
                                new SymbolNode("x"),
                                I(2)))
                    ]),
                I(0),
                new SymbolNode("x"),
                I(-3),
                I(3));

        var request =
            new MathematicsSolveRequest(
                problem,
                [],
                []);

        var solver =
            new ExactAreaBetweenCurvesSolver();
        var verifier =
            new ExactAreaBetweenCurvesVerifier();

        var result =
            solver.Solve(request);

        Assert.Equal(
            MathematicsSolveStatus.Solved,
            result.Status);
        Assert.Equal(
            R(46, 3),
            result.ExactResult);
        Assert.NotNull(result.Trace);
        Assert.Equal(
            4,
            result.Trace!.Steps.Count);

        var intersections =
            Assert.IsType<VectorNode>(
                result.Trace.Steps
                    .Single(step =>
                        step.StepId == "intersections")
                    .After);

        Assert.Equal(
            [I(-2), I(2)],
            intersections.Components);
        Assert.True(
            verifier.Verify(
                    request,
                    result)
                .IsVerified);
    }

    [Fact]
    public void IndependentVerifierRejectsMutatedArea()
    {
        var problem =
            new AreaBetweenCurvesNode(
                new AddNode(
                    [
                        I(1),
                        new NegateNode(
                            new PowerNode(
                                new SymbolNode("x"),
                                I(2)))
                    ]),
                I(0),
                new SymbolNode("x"),
                I(-2),
                I(2));

        var request =
            new MathematicsSolveRequest(
                problem,
                [],
                []);

        var solver =
            new ExactAreaBetweenCurvesSolver();
        var verifier =
            new ExactAreaBetweenCurvesVerifier();

        var result =
            solver.Solve(request);

        Assert.True(
            verifier.Verify(
                    request,
                    result)
                .IsVerified);

        var wrong =
            result with
            {
                ExactResult = I(99),
                SolutionSet =
                    new FiniteSolutionSet(
                        [I(99)])
            };

        Assert.False(
            verifier.Verify(
                    request,
                    wrong)
                .IsVerified);
    }

    [Fact]
    public void SolverFailsClosedOnIrrationalIntersections()
    {
        // x^2 - 2 = 0 has irrational intersections ±sqrt(2).
        var problem =
            new AreaBetweenCurvesNode(
                new AddNode(
                    [
                        new PowerNode(
                            new SymbolNode("x"),
                            I(2)),
                        I(-2)
                    ]),
                I(0),
                new SymbolNode("x"),
                I(-2),
                I(2));

        var result =
            new ExactAreaBetweenCurvesSolver()
                .Solve(
                    new MathematicsSolveRequest(
                        problem,
                        [],
                        []));

        Assert.Equal(
            MathematicsSolveStatus.Unsupported,
            result.Status);
        Assert.Contains(
            result.Diagnostics,
            diagnostic =>
                diagnostic.Contains(
                    "exact rationals",
                    StringComparison.OrdinalIgnoreCase));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void GeneratorIsDeterministicSolvedAndIndependentlyVerified(
        int difficultyBand)
    {
        var factory =
            new ExactAreaBetweenCurvesQuestionFactory(
                new ExactAreaBetweenCurvesSolver(),
                new ExactAreaBetweenCurvesVerifier());

        var first =
            factory.Generate(
                20260928,
                difficultyBand);
        var second =
            factory.Generate(
                20260928,
                difficultyBand);

        Assert.Equal(
            ExactAreaBetweenCurvesQuestionFactory.FamilyId,
            first.QuestionFamilyId);
        Assert.Equal(
            "calculus.area_between_curves",
            first.Skill.Value);
        Assert.True(
            first.Verification.IsVerified);
        Assert.Equal(
            JsonSerializer.Serialize(first.Problem),
            JsonSerializer.Serialize(second.Problem));
        Assert.Equal(
            JsonSerializer.Serialize(first.ExpectedAnswer),
            JsonSerializer.Serialize(second.ExpectedAnswer));
        Assert.Equal(
            "true",
            first.Parameters[
                "requiresPiecewiseAbsoluteArea"]);
    }

    [Fact]
    public void ExactSolutionProjectsToPiecewiseShadedTypedVisual()
    {
        var factory =
            new ExactAreaBetweenCurvesQuestionFactory(
                new ExactAreaBetweenCurvesSolver(),
                new ExactAreaBetweenCurvesVerifier());

        var generated =
            factory.Generate(
                42,
                2);

        var problem =
            Assert.IsType<AreaBetweenCurvesNode>(
                generated.Problem);

        Assert.True(
            AdvancedMathematicsVisualProjector
                .TryProjectAreaBetweenCurves(
                    problem,
                    generated.SolveResult,
                    out var visual));

        Assert.NotNull(visual);
        Assert.Equal(
            2,
            visual!.Intersections.Count);

        var svg =
            AdvancedMathematicsVisualRenderer
                .RenderSvg(visual);

        Assert.Equal(
            3,
            CountOccurrences(
                svg,
                "class='amv-region'"));
        Assert.Contains(
            "f(x)",
            svg,
            StringComparison.Ordinal);
        Assert.Contains(
            "g(x)",
            svg,
            StringComparison.Ordinal);
    }

    private static int CountOccurrences(
        string text,
        string value)
    {
        var count = 0;
        var index = 0;

        while ((index =
                    text.IndexOf(
                        value,
                        index,
                        StringComparison.Ordinal)) >= 0)
        {
            count++;
            index += value.Length;
        }

        return count;
    }

    private static IntegerNode I(
        int value) =>
        new(new BigInteger(value));

    private static RationalNode R(
        int numerator,
        int denominator) =>
        new(
            new ExactRational(
                numerator,
                denominator));
}
