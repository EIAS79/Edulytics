using Edulytics.Core.Mathematics.Visuals;

namespace Edulytics.Web.Presentation;

/// <summary>
/// Independently drawn inverse-function models for Section 3.8. Reuses the
/// established SVG coordinate plane renderer. None are publisher figures.
/// </summary>
public static class Grade12PrecalculusSection38InverseGraphs
{
    public sealed record InversePair(int RelatedExercise, string Title,
        string DomainConstraint, CoordinatePlaneVisualSpec Graph);

    public static readonly IReadOnlyList<InversePair> All = Build();

    private static IReadOnlyList<InversePair> Build()
    {
        var pairs = new List<InversePair>();
        Add(35, "Restricted quadratic and principal square root",
            "f(x)=(x-4)² on x≥4 and g(x)=4+√x on x≥0",
            x => (x-4m)*(x-4m), 4m, 6.3m,
            x => 4m+(decimal)Math.Sqrt((double)x), 0m, 7m);
        Add(36, "Cubic and cube-root inverse",
            "f(x)=x³+1 and g(x)=∛(x-1) on all real inputs",
            x => x*x*x+1m, -1.65m, 1.85m,
            x => (decimal)Math.Cbrt((double)(x-1m)), -5m, 8m);
        Add(32, "Radical and its restricted quadratic inverse",
            "f(x)=1+√(x-1) for x≥1 and g(x)=(x-1)²+1 for x≥1",
            x => 1m+(decimal)Math.Sqrt((double)(x-1m)), 1m, 8m,
            x => (x-1m)*(x-1m)+1m, 1m, 3.5m);
        Add(40, "Reciprocal and its inverse",
            "f(x)=4/x is its own inverse for x≠0; compare reflection in y=x",
            x => 4m/x, -6m, 6m,
            x => 4m/x, -6m, 6m, [0m], [0m]);
        return pairs;

        void Add(int exercise, string title, string condition,
            Func<decimal,decimal> f, decimal fStart, decimal fEnd,
            Func<decimal,decimal> inverse, decimal inverseStart, decimal inverseEnd,
            decimal[]? fPoles = null, decimal[]? inversePoles = null)
        {
            var segments = new List<MathematicalSegment>();
            Plot(f, fStart, fEnd, fPoles ?? [], segments);
            Plot(inverse, inverseStart, inverseEnd, inversePoles ?? [], segments);
            // y=x is the geometric reflection axis for both functions.
            segments.Add(new MathematicalSegment(
                new MathematicalPoint(-5m,-5m), new MathematicalPoint(8m,8m)));
            var spec = new CoordinatePlaneVisualSpec(
                new MathematicalAxisWindow(-6m,8m,-6m,8m,1m,1m),
                [], segments,
                new MathematicalVisualAccessibility(
                    title, condition + ". Model includes original curve, its inverse, " +
                    "and reference line y=x. Explain the swapped input-output pairs. " +
                    "Separate real-domain branches remain disconnected at poles."));
            MathematicalVisualSpecValidator.Validate(spec);
            pairs.Add(new InversePair(exercise,title,condition,spec));
        }

        static void Plot(Func<decimal,decimal> value, decimal lo, decimal hi,
            decimal[] poles, List<MathematicalSegment> segments)
        {
            MathematicalPoint? previous = null;
            decimal? priorX = null;
            // Two independently sampled curves share ONE maximum-128-segment
            // validated SVG. With 60 intervals each plus the y=x reflection
            // reference, the combined total is at most 121, never >128.
            const int intervalsPerCurve = 60;
            var step = (hi-lo)/intervalsPerCurve;
            for (var i=0;i<=intervalsPerCurve;i++)
            {
                var x=lo+i*step;
                if (poles.Any(p => Math.Abs(x-p)<.10m))
                {
                    previous=null;
                    priorX=null;
                    continue;
                }
                var y=value(x);
                if (y< -6m || y>8m)
                {
                    previous=null;
                    priorX=null;
                    continue;
                }
                var current = new MathematicalPoint(x,y);
                if (previous is not null && priorX is decimal px && x-px <= step*1.05m)
                    segments.Add(new MathematicalSegment(previous,current));
                previous=current;
                priorX=x;
            }
        }
    }
}
