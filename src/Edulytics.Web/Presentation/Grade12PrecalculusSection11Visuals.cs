using Edulytics.Core.Mathematics.Visuals;

namespace Edulytics.Web.Presentation;

/// <summary>
/// Independently authored, typed graph instruction for G12 Precalculus 1.1.
/// Reuses the existing accessible mathematical SVG renderer. These four
/// teaching models are not original publisher figure reproductions.
/// </summary>
public static class Grade12PrecalculusSection11Visuals
{
    public static IReadOnlyList<MathematicalVisualSpec> Build()
    {
        var sideways = new List<MathematicalSegment>();
        for (var y = -3m; y < 3m; y += 0.25m)
        {
            var next = y + 0.25m;
            sideways.Add(new MathematicalSegment(
                new MathematicalPoint(y * y, y),
                new MathematicalPoint(next * next, next)));
        }

        return
        [
            new FunctionGraphVisualSpec(
                new MathematicalAxisWindow(-4m, 4m, -2m, 10m, 1m, 2m),
                new MathematicalCurveSpec("precalc-section11-square",
                    MathematicalCurveKind.Polynomial, [0m, 0m, 1m], "y=x²"),
                [
                    new MathematicalPoint(-2m, 4m, "(-2,4)"),
                    new MathematicalPoint(2m, 4m, "(2,4)")
                ],
                new MathematicalVisualAccessibility(
                    "Quadratic curve: a function, but not one-to-one",
                    "The upward-opening curve y=x squared passes the vertical-line test. " +
                    "It fails the horizontal-line test because the two distinct inputs -2 and 2 share output 4.")),
            new FunctionGraphVisualSpec(
                new MathematicalAxisWindow(-3m, 3m, -8m, 8m, 1m, 2m),
                new MathematicalCurveSpec("precalc-section11-line",
                    MathematicalCurveKind.Polynomial, [-1m, 2m], "y=2x-1"),
                [
                    new MathematicalPoint(-1m, -3m, "(-1,-3)"),
                    new MathematicalPoint(1m, 1m, "(1,1)")
                ],
                new MathematicalVisualAccessibility(
                    "Nonhorizontal line: a one-to-one function",
                    "The line y=2x-1 has one output per input and distinct outputs for distinct inputs. " +
                    "Every vertical and every horizontal line intersects it at most once.")),
            new CoordinatePlaneVisualSpec(
                new MathematicalAxisWindow(-1m, 9m, -4m, 4m, 1m, 1m),
                [
                    new MathematicalPoint(1m, 1m, "(1,1)"),
                    new MathematicalPoint(1m, -1m, "(1,-1)")
                ],
                sideways,
                new MathematicalVisualAccessibility(
                    "Sideways parabola: fails vertical-line test",
                    "The relation x=y squared has an upper and a lower branch. " +
                    "At input x=1 both outputs y=1 and y=-1 occur; therefore it is not a function of x.")),
            new CoordinatePlaneVisualSpec(
                new MathematicalAxisWindow(-5m, 5m, -5m, 5m, 1m, 1m),
                [
                    new MathematicalPoint(1m, 3m, "(1,3)"),
                    new MathematicalPoint(1m, -3m, "(1,-3)")
                ],
                [
                    new MathematicalSegment(new MathematicalPoint(0m, 3m),
                        new MathematicalPoint(5m, 3m)),
                    new MathematicalSegment(new MathematicalPoint(-5m, -3m),
                        new MathematicalPoint(2m, -3m))
                ],
                new MathematicalVisualAccessibility(
                    "Overlapping input domains: two open-ended horizontal rays",
                    "The upper ray is y=3 for x>0, open at x=0 and continuing right. " +
                    "The lower ray is y=-3 for x<2, open at x=2 and continuing left. " +
                    "At x=1 both y=3 and y=-3 occur, so their union is not a function."))
            {
                OpenPoints =
                [
                    new MathematicalPoint(0m, 3m, "open"),
                    new MathematicalPoint(2m, -3m, "open")
                ]
            }
        ];
    }
}
