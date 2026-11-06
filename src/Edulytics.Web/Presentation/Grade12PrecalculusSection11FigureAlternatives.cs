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
        new(10,"Discrete points versus continuous curve",
            "A first-party line f(x)=2−x/6 contains (0,2) and (6,1). " +
            "The plotted points are samples; the continuous curve includes infinitely many inputs.",
            null,new CoordinatePlaneVisualSpec(
                Window(-1m,7m,0m,3m),
                [new MathematicalPoint(0m,2m,"(0,2)"),
                 new MathematicalPoint(6m,1m,"(6,1)")],
                [new MathematicalSegment(
                    new MathematicalPoint(0m,2m),
                    new MathematicalPoint(6m,1m))],
                new MathematicalVisualAccessibility(
                    "Function graph with sampled values",
                    "At x=0 output is exactly 2, and at x=6 output is exactly 1 on a straight first-party segment."))),
        new(11,"Vertical-line comparison",
            "A nonvertical line passes the test; a sideways parabola and a circle fail " +
            "because some inputs intersect their graphs twice.",41,null),
        new(12,"Three vertical-line classification examples",
            "Two independent first-party graph models pass the vertical-line test; " +
            "a sideways parabola fails. Compare outputs at one chosen input.",41,null),
        new(13,"Vertical-line failure highlighted",
            "The relation x=y² contains (1,1) and (1,-1). A vertical line at x=1 intersects twice.",
            41,null),
        new(14,"Independent vertical-line Try It alternative",
            "Sketch a closed ellipse and test x=0 on an ellipse centered at the origin; " +
            "two y-values show it is not a function. Original drawing remains unverified.",
            44,null),
        new(15,"Horizontal-line test of injectivity",
            "On first-party y=x², horizontal y=4 meets x=-2 and x=2. " +
            "Thus it is a function but not one-to-one over all real inputs.",
            null,Grade12PrecalculusSection11Visuals.Build()[0])
    ];

    public static Figure? Find(int originalFigure) =>
        All.FirstOrDefault(item => item.OriginalFigureOrdinal == originalFigure);
}
