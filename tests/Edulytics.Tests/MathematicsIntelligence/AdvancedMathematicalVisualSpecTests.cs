using Edulytics.Core.Mathematics.Visuals;
using Edulytics.Web.Presentation;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class AdvancedMathematicalVisualSpecTests
{
    [Fact]
    public void FunctionGraphRendererIsDeterministic()
    {
        var spec = new FunctionGraphVisualSpec(
            Window(),
            new MathematicalCurveSpec(
                "quadratic",
                MathematicalCurveKind.Polynomial,
                [0m, 0m, 1m],
                "y = x²"),
            [new MathematicalPoint(0m, 0m, "O")],
            Accessibility("Quadratic graph"));

        var first = AdvancedMathematicsVisualRenderer.RenderSvg(spec);
        var second = AdvancedMathematicsVisualRenderer.RenderSvg(spec);

        Assert.Equal(first, second);
        Assert.Contains("<svg", first, StringComparison.Ordinal);
        Assert.Contains("amv-curve", first, StringComparison.Ordinal);
        Assert.Contains("y = x²", first, StringComparison.Ordinal);
    }

    [Fact]
    public void MultiFunctionGraphSupportsTrigonometricCurves()
    {
        var spec = new MultiFunctionGraphVisualSpec(
            new MathematicalAxisWindow(
                -7m, 7m, -2m, 2m, 1m, 1m),
            [
                new MathematicalCurveSpec(
                    "sin",
                    MathematicalCurveKind.Sine,
                    [1m, 1m, 0m, 0m],
                    "sin x"),
                new MathematicalCurveSpec(
                    "cos",
                    MathematicalCurveKind.Cosine,
                    [1m, 1m, 0m, 0m],
                    "cos x")
            ],
            [],
            Accessibility("Sine and cosine"));

        var svg = AdvancedMathematicsVisualRenderer.RenderSvg(spec);

        Assert.Contains("sin x", svg, StringComparison.Ordinal);
        Assert.Contains("cos x", svg, StringComparison.Ordinal);
        Assert.Contains("amv-series-1", svg, StringComparison.Ordinal);
    }

    [Fact]
    public void RegionOfIntegrationRendersTypedShadedRegion()
    {
        var spec = new RegionOfIntegrationVisualSpec(
            new MathematicalAxisWindow(
                -1m, 4m, -2m, 6m, 1m, 1m),
            new MathematicalCurveSpec(
                "upper",
                MathematicalCurveKind.Polynomial,
                [4m, 0m, -1m],
                "4 − x²"),
            new MathematicalCurveSpec(
                "lower",
                MathematicalCurveKind.Polynomial,
                [0m],
                "0"),
            -1m,
            1m,
            [
                new MathematicalPoint(-1m, 3m),
                new MathematicalPoint(1m, 3m)
            ],
            Accessibility("Area between curves"));

        var svg = AdvancedMathematicsVisualRenderer.RenderSvg(spec);

        Assert.Contains("amv-region", svg, StringComparison.Ordinal);
        Assert.Contains("4 − x²", svg, StringComparison.Ordinal);
    }

    [Fact]
    public void MatrixRendererEscapesLearnerFacingLabels()
    {
        var spec = new MatrixDisplayVisualSpec(
            [
                new MathematicalMatrix(
                    2,
                    2,
                    ["1", "2", "3", "4"],
                    "<A & B>")
            ],
            Accessibility("Matrix display"));

        var svg = AdvancedMathematicsVisualRenderer.RenderSvg(spec);

        Assert.DoesNotContain("<A & B>", svg, StringComparison.Ordinal);
        Assert.Contains("&lt;A &amp; B&gt;", svg, StringComparison.Ordinal);
        Assert.DoesNotContain("<script", svg, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ValidatorRejectsUnboundedOrInvalidContracts()
    {
        var invalidWindow = new FunctionGraphVisualSpec(
            new MathematicalAxisWindow(
                5m, 5m, -1m, 1m, 1m, 1m),
            new MathematicalCurveSpec(
                "line",
                MathematicalCurveKind.Polynomial,
                [0m, 1m]),
            [],
            Accessibility("Invalid graph"));

        Assert.Throws<ArgumentOutOfRangeException>(
            () => MathematicalVisualSpecValidator.Validate(invalidWindow));

        var oversizedMatrix = new MatrixDisplayVisualSpec(
            [
                new MathematicalMatrix(
                    11,
                    1,
                    Enumerable.Range(1, 11)
                        .Select(x => x.ToString())
                        .ToArray())
            ],
            Accessibility("Invalid matrix"));

        Assert.Throws<ArgumentOutOfRangeException>(
            () => MathematicalVisualSpecValidator.Validate(oversizedMatrix));
    }

    [Fact]
    public void ArgandAndVectorVisualsRemainTypedAndDeterministic()
    {
        var argand = new ArgandDiagramVisualSpec(
            Window(),
            [
                new MathematicalComplexPoint(
                    2m, 3m, "z")
            ],
            Accessibility("Argand diagram"));

        var vector = new VectorDiagramVisualSpec(
            Window(),
            [
                new MathematicalVectorArrow(
                    new MathematicalPoint(0m, 0m),
                    new MathematicalPoint(3m, 2m),
                    "v")
            ],
            [],
            Accessibility("Vector diagram"));

        var argandSvg =
            AdvancedMathematicsVisualRenderer.RenderSvg(argand);
        var vectorSvg =
            AdvancedMathematicsVisualRenderer.RenderSvg(vector);

        Assert.Contains("Re", argandSvg, StringComparison.Ordinal);
        Assert.Contains("Im", argandSvg, StringComparison.Ordinal);
        Assert.Contains("marker-end", vectorSvg, StringComparison.Ordinal);
    }

    private static MathematicalAxisWindow Window() =>
        new(-5m, 5m, -5m, 5m, 1m, 1m);

    private static MathematicalVisualAccessibility Accessibility(
        string title) =>
        new(title, $"Deterministic visual for {title}.");
}
