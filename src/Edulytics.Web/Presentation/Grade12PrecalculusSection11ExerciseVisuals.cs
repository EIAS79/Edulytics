using Edulytics.Core.Mathematics.Visuals;

namespace Edulytics.Web.Presentation;

/// <summary>
/// First-party graph tasks for Section 1.1. These are mathematical
/// skill-equivalent sketches, not copies or exact replacements for every
/// publisher figure. Exercise 52 remains an instructor graph-read task.
/// </summary>
public static class Grade12PrecalculusSection11ExerciseVisuals
{
    private static readonly IReadOnlyDictionary<int, MathematicalVisualSpec> Specs = Build();

    public static int Count => Specs.Count;

    public static MathematicalVisualSpec? Find(int originalExerciseNumber) =>
        Specs.TryGetValue(originalExerciseNumber, out var spec) ? spec : null;

    public static IReadOnlyDictionary<int, MathematicalVisualSpec> All => Specs;

    private static IReadOnlyDictionary<int, MathematicalVisualSpec> Build()
    {
        static MathematicalAxisWindow Window(decimal xmin, decimal xmax,
            decimal ymin, decimal ymax, decimal tick = 1m) =>
            new(xmin, xmax, ymin, ymax, tick, tick);

        static MathematicalVisualAccessibility Access(string title, string detail) =>
            new(title, detail);

        static MathematicalPoint Point(decimal x, decimal y, string? label = null) =>
            new(x, y, label);

        static MathematicalSegment Segment(decimal x0, decimal y0,
            decimal x1, decimal y1) =>
            new(Point(x0, y0), Point(x1, y1));

        static IReadOnlyList<MathematicalSegment> Polyline(
            params MathematicalPoint[] points)
        {
            var segments = new List<MathematicalSegment>(points.Length - 1);
            for (var i = 1; i < points.Length; i++)
                segments.Add(new MathematicalSegment(points[i - 1], points[i]));
            return segments;
        }

        static IReadOnlyList<MathematicalSegment> Sample(
            Func<decimal, decimal> f, decimal xmin, decimal xmax, int steps)
        {
            var points = new MathematicalPoint[steps + 1];
            for (var i = 0; i <= steps; i++)
            {
                var x = xmin + (xmax - xmin) * i / steps;
                points[i] = Point(x, f(x));
            }
            return Polyline(points);
        }

        static IReadOnlyList<MathematicalSegment> Ellipse(decimal cx, decimal cy,
            decimal rx, decimal ry)
        {
            const int steps = 48;
            var points = new MathematicalPoint[steps + 1];
            for (var i = 0; i <= steps; i++)
            {
                var theta = 2d * Math.PI * i / steps;
                points[i] = Point(
                    cx + rx * (decimal)Math.Cos(theta),
                    cy + ry * (decimal)Math.Sin(theta));
            }
            return Polyline(points);
        }

        static CoordinatePlaneVisualSpec Coordinate(
            MathematicalAxisWindow window,
            IReadOnlyList<MathematicalSegment> segments,
            string title, string detail,
            IReadOnlyList<MathematicalPoint>? points = null) =>
            new(window, points ?? [], segments, Access(title, detail));

        static FunctionGraphVisualSpec Polynomial(
            MathematicalAxisWindow window, int exercise, string title,
            string detail, params decimal[] coefficients) =>
            new(window,
                new MathematicalCurveSpec($"g12-u01-l01-ex{exercise}",
                    MathematicalCurveKind.Polynomial, coefficients),
                [], Access(title, detail));

        var result = new Dictionary<int, MathematicalVisualSpec>
        {
            [40] = Polynomial(Window(-2m,2m,-4m,4m),40,
                "Exercise 40: curved relation",
                "A cubic-like graph can have a turning maximum and minimum while remaining single-valued in x.",
                0m,-3m,0m,1m),
            [41] = Coordinate(Window(-1m,5m,-3m,3m),
                Polyline(Point(4m,-2m),Point(1m,-1m),Point(0m,0m),
                    Point(1m,1m),Point(4m,2m)),
                "Exercise 41: sideways parabola",
                "At x=1 two distinct plotted outputs exist. Apply the vertical-line test."),
            [42] = Coordinate(Window(-4m,4m,-4m,4m),
                Sample(x=>1m/x,-4m,-0.25m,32)
                    .Concat(Sample(x=>1m/x,0.25m,4m,32)).ToArray(),
                "Exercise 42: reciprocal branches",
                "Separate first- and third-quadrant branches approach the axes but no allowed x has two y-values."),
            [43] = Coordinate(Window(-4m,4m,-3m,4m),
                Polyline(Point(-4m,-1m),Point(-1m,-1m),Point(2m,2m),Point(4m,2m)),
                "Exercise 43: piecewise function",
                "The horizontal and slanted pieces connect at common points and never assign conflicting outputs."),
            [44] = Coordinate(Window(-1m,5m,-4m,1m),
                Ellipse(2m,-2m,2m,1m),
                "Exercise 44: closed ellipse",
                "Some vertical lines meet the ellipse twice, once on the upper arc and once on the lower."),
            [45] = Polynomial(Window(-2m,2m,-5m,5m),45,
                "Exercise 45: smooth rising S-shape",
                "A vertical line meets a single cubic curve once even if its tangent is horizontal.",
                0m,0m,0m,1m),
            [46] = Grade12PrecalculusSection11Visuals.Build()[4],
            [47] = Polynomial(Window(-4m,4m,-4m,4m),47,
                "Exercise 47: decreasing nonvertical line",
                "A nonvertical straight line assigns one y-value per x-value.",
                0m,-1m),
            [48] = Coordinate(Window(-4m,4m,-4m,4m),
                new [] { Segment(-3m,-3m,3m,3m), Segment(-3m,3m,3m,-3m) },
                "Exercise 48: two crossed lines",
                "For any input except the crossing, the X relation includes two different outputs."),
            [49] = Polynomial(Window(-4m,4m,-4m,4m),49,
                "Exercise 49: increasing nonvertical line",
                "Each input maps to exactly one output on the increasing straight line.",
                0m,1m),
            [50] = Coordinate(Window(-5m,1m,-5m,1m),
                Ellipse(-2m,-2m,2m,2m),
                "Exercise 50: closed circle",
                "A vertical line through the interior intersects upper and lower circle arcs."),
            [51] = new FunctionGraphVisualSpec(
                Window(-6m,6m,-2m,2m),
                new MathematicalCurveSpec("g12-u01-l01-ex51",
                    MathematicalCurveKind.Sine,[1m,1m,0m,0m]),
                [],Access("Exercise 51: continuous wave",
                "A wave can repeat an output at different x-values while still being a function.")),
            [52] = Coordinate(Window(-2.5m,8m,-0.5m,4m),
                Sample(x => (decimal)Math.Sqrt((double)(x + 2m)),
                    -2m,8m,40),
                "Exercise 52: first-party increasing concave-down graph",
                "Independently drawn y=sqrt(x+2) is a teaching analogue, not the original exact graph. " +
                "Read f(-1)=1 and the y=3 intersection near x=7 on THIS curve. " +
                "Do not treat the original source graph as an exact algebraic rule.",
                [Point(-1m,1m,"(-1,1)"), Point(7m,3m,"(7,3)")]),
            [53] = Polynomial(Window(-3m,3m,-4m,5m),53,
                "Exercise 53: W-shaped graph values",
                "The quartic reaches y=-3 at x=-2 and x=2; its central point is f(0)=1.",
                1m,0m,-2m,0m,0.25m),
            [54] = Coordinate(Window(-7m,5m,-4m,5m),
                Polyline(Point(-7m,0m),Point(-3m,4m),Point(4m,-3m)),
                "Exercise 54: inverted V graph values",
                "The vertex is (-3,4); f(4)=-3, and the level y=1 meets x=-6 and x=0."),
            [55] = Coordinate(Window(-5m,2m,-5m,4m),
                Ellipse(-2m,-1m,2m,3m),
                "Exercise 55: ellipse relation",
                "This closed oval fails the vertical-line test."),
            [56] = Coordinate(Window(-3m,5m,-1m,5m),
                Polyline(Point(-3m,5m),Point(1m,1m),Point(5m,5m)),
                "Exercise 56: absolute-value V",
                "The V passes vertical-line but fails horizontal-line for outputs above the vertex."),
            [57] = Polynomial(Window(-2m,2m,-5m,5m),57,
                "Exercise 57: strictly increasing S",
                "A strictly increasing cubic passes vertical and horizontal line tests.",
                0m,0m,0m,1m),
            [58] = Polynomial(Window(-4m,4m,-4m,4m),58,
                "Exercise 58: strictly decreasing graph",
                "A decreasing nonhorizontal line passes vertical and horizontal line tests.",
                0m,-1m),
            [59] = new CoordinatePlaneVisualSpec(
                Window(-3.5m,3.5m,-3m,3m,0.785398163m),
                [],
                Polyline(Point(-3.141592654m,0m),Point(-1.570796327m,0m),
                    Point(0m,0.3m),Point(0.785398163m,0.7m),
                    Point(1.570796327m,0m),Point(3.141592654m,-2m)),
                Access("Exercise 59: π-labelled graph, not one-to-one",
                "An independent first-party graph uses the source's horizontal π-radian labels. " +
                "Its horizontal-line test fails because repeated outputs appear on the flat " +
                "portion and on the rise and fall. Teacher checks exact original geometry."))
            {
                XAxisTicks =
                [
                    new(-3.141592654m,"−π"),
                    new(-1.570796327m,"−π/2"),
                    new(0m,"0"),
                    new(1.570796327m,"π/2"),
                    new(3.141592654m,"π")
                ]
            }
        };

        if (result.Count != 20)
            throw new InvalidOperationException("Section 1.1 exercise diagram count changed.");
        foreach (var spec in result.Values)
            MathematicalVisualSpecValidator.Validate(spec);
        return result;
    }
}
