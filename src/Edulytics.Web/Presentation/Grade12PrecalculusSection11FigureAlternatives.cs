using Edulytics.Core.Mathematics.Visuals;

namespace Edulytics.Web.Presentation;

/// <summary>
/// Independent, non-source-image visual alternatives to the 15 identified
/// illustrations in Precalculus Section 1.1. The original pictures are not
/// copied, and these mathematical explanations are not claims of pixel parity.
/// </summary>
public static class Grade12PrecalculusSection11FigureAlternatives
{
    public sealed record Figure(
        int OriginalFigureOrdinal, string Topic, string FirstPartyAlternative,
        int? LinkedFirstPartyGraphExercise, MathematicalVisualSpec? AddedDrawing);

    private static MathematicalAxisWindow Window(decimal xmin, decimal xmax,
        decimal ymin, decimal ymax) => new(xmin,xmax,ymin,ymax,1m,1m);

    // Independent analytic circle model: unlike the graph of a function,
    // this relation has two real outputs at the same x. Neither the source
    // artwork nor its pixel layout is reused.
    private static CoordinatePlaneVisualSpec CircleCounterexample()
    {
        const int samples = 96;
        var segments = new List<MathematicalSegment>();
        var previous = new MathematicalPoint(2m, 0m);
        for (var i = 1; i <= samples; i++)
        {
            var theta = 2d * Math.PI * i / samples;
            var next = new MathematicalPoint(
                (decimal)(2d * Math.Cos(theta)),
                (decimal)(2d * Math.Sin(theta)));
            segments.Add(new MathematicalSegment(previous, next));
            previous = next;
        }
        return new CoordinatePlaneVisualSpec(
            Window(-3m, 3m, -3m, 3m),
            [
                new MathematicalPoint(0m, 2m, "(0,2)"),
                new MathematicalPoint(0m, -2m, "(0,-2)")
            ],
            segments,
            new MathematicalVisualAccessibility(
                "First-party circle: not a function of x",
                "The independent relation x squared plus y squared equals 4. At x=0, " +
                "the graph has two outputs y=2 and y=-2. It fails the vertical-line test " +
                "and cannot be classified as a one-to-one function of x."));
    }

    public static readonly IReadOnlyList<Figure> All =
    [
        new(1,"Relation arrow diagrams",
            "Multiple inputs can point to one output without breaking functionhood. " +
            "One input pointing to two different outputs makes a relation nonfunctional. " +
            "Use independent ordered pairs {(a,1),(b,1)} and {(a,1),(a,2)} to compare.",
            41,null),
        new(2,"Three-item menu with repeated price",
            "A first-party table with Muffin $2.00, Scone $3.00 and Pastry $3.00 " +
            "defines price as a function of item. The inverse item-from-price is not a function.",
            null,null),
        new(3,"Forward menu mapping",
            "Each named item points to one price; several names may share the same price.",
            null,null),
        new(4,"Reverse menu mapping",
            "The repeated $3.00 input would point to both Scone and Pastry, so the " +
            "price-to-item relation fails the unique-output requirement.",
            null,null),
        new(5,"Nonnumeric function input",
            "A month name maps to its number of days; in a nonleap year f(March)=31.",
            null,null),
        new(6,"Graph corroborating a quadratic result",
            "The first-party graph h(p)=p²+2p has h(-3)=h(1)=3, h(4)=24. " +
            "Repeated outputs do not contradict functionhood.",
            null,new FunctionGraphVisualSpec(
                Window(-4m,5m,-2m,30m),
                new MathematicalCurveSpec("g12-s11-fig06",
                    MathematicalCurveKind.Polynomial,[0m,2m,1m],"h(p)=p²+2p"),
                [new MathematicalPoint(-3m,3m,"(-3,3)"),
                 new MathematicalPoint(1m,3m,"(1,3)"),
                 new MathematicalPoint(4m,24m,"(4,24)")],
                new MathematicalVisualAccessibility(
                    "Equivalent quadratic function values",
                    "The first-party parabola passes through (-3,3), (1,3) and (4,24)."))),
        new(7,"Read values from plotted function",
            "Read f(2)=1 by the curve height and find all inputs with output 4.",
            null,Grade12PrecalculusSection11Visuals.Build()[2]),
        new(8,"Locate highlighted evaluation point",
            "The input 2 locates the point (2,1) on the first-party parabolic curve.",
            null,Grade12PrecalculusSection11Visuals.Build()[2]),
        new(9,"Locate all inverse graph-read inputs",
            "The horizontal level y=4 meets first-party y=(x-1)² at (-1,4) and (3,4).",
            null,Grade12PrecalculusSection11Visuals.Build()[2]),
        new(10,"Sampled points on a continuous non-monotonic function",
            "An original first-party continuous piecewise curve passes through " +
            "(0,2) and (6,1) but also rises and falls between sampled values. " +
            "Knowing two plotted points does not imply the curve is a straight " +
            "line, a monotonic function, or fully determined outside its graph.",
            null,new CoordinatePlaneVisualSpec(
                Window(-6m,8m,-3m,4m),
                [new MathematicalPoint(0m,2m,"(0,2)"),
                 new MathematicalPoint(6m,1m,"(6,1)")],
                [
                    new MathematicalSegment(new MathematicalPoint(-5m,-2m),
                        new MathematicalPoint(0m,2m)),
                    new MathematicalSegment(new MathematicalPoint(0m,2m),
                        new MathematicalPoint(3m,0.5m)),
                    new MathematicalSegment(new MathematicalPoint(3m,0.5m),
                        new MathematicalPoint(6m,1m)),
                    new MathematicalSegment(new MathematicalPoint(6m,1m),
                        new MathematicalPoint(8m,2m))
                ],
                new MathematicalVisualAccessibility(
                    "Function graph includes two sampled outputs but is non-monotonic",
                    "An independent continuous, piecewise-linear function rises to " +
                    "a local maximum (0,2), falls to a low point near (3,0.5), " +
                    "and rises through (6,1). Every x has one y, while the two " +
                    "highlighted samples do not determine the remaining curve."))),
        new(11,"Vertical-line comparison",
            "A nonvertical line passes the test; a sideways parabola and a circle fail " +
            "because some inputs intersect their graphs twice.",41,null),
        new(12,"Three vertical-line classification examples",
            "Two independent first-party graph models pass the vertical-line test; " +
            "a sideways parabola fails. Compare outputs at one chosen input.",41,null),
        new(13,"Source Figure 13: circle fails the vertical-line test",
            "An independent circle x²+y²=4 has points (0,2) and (0,-2). " +
            "The same x has two real outputs, so the relation is not a function of x.",
            null,CircleCounterexample()),
        new(14,"Source Figure 14: absolute-value V is a function",
            "The independent V-shaped graph y=|x| has vertex (0,0) and exactly " +
            "one output for every real x. Every vertical line hits at most once: " +
            "this relation IS a function, as required by the original Try It 11 skill.",
            null,Grade12PrecalculusSection11ToolkitGraphs.All
                .Single(family => family.Title == "Absolute value y=|x|").Graph),
        new(15,"Horizontal-line test with three repeated outputs",
            "The original source illustrates that a horizontal line can meet " +
            "a function curve at two or even three different inputs. " +
            "In this independent cubic y=x³−4x, the horizontal level y=0 " +
            "meets x=−2, x=0 and x=2, so the function is not one-to-one.",
            null,new FunctionGraphVisualSpec(
                Window(-3m,3m,-6m,6m),
                new MathematicalCurveSpec("g12-s11-fig15-three-horizontal-roots",
                    MathematicalCurveKind.Polynomial,[0m,-4m,0m,1m],"y=x³−4x"),
                [new MathematicalPoint(-2m,0m,"(-2,0)"),
                 new MathematicalPoint(0m,0m,"(0,0)"),
                 new MathematicalPoint(2m,0m,"(2,0)")],
                new MathematicalVisualAccessibility(
                    "Continuous cubic is a function but not one-to-one",
                    "The cubic graph y=x cubed minus four x passes the vertical-line " +
                    "test, but horizontal y=0 crosses three times at x=-2,0,2. " +
                    "This recreates the source's three-intersection skill without " +
                    "copying source curve artwork.")))
    ];

    public static Figure? Find(int originalFigure) =>
        All.FirstOrDefault(item => item.OriginalFigureOrdinal == originalFigure);
}
