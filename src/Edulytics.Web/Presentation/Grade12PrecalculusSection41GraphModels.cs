using Edulytics.Core.Mathematics.Visuals;

namespace Edulytics.Web.Presentation;

/// <summary>First-party Section 4.1 source-skill graph alternatives.
/// They do not reproduce textbook illustrations. Reuses existing typed SVG
/// renderer; Stage D runtime has not been executed.</summary>
public static class Grade12PrecalculusSection41GraphModels
{
    public sealed record GraphTask(int OriginalSourceFigureSkill, string Prompt,
        CoordinatePlaneVisualSpec Graph);
    public static readonly IReadOnlyList<GraphTask> All = Build();

    private static IReadOnlyList<GraphTask> Build()
    {
        var items = new List<GraphTask>();
        Add(1, -2m, 3m, 0m, 30m, [x => (decimal)Math.Pow(3d,(double)x)],
            "Exponential growth: y=3^x, positive and never touching y=0.");
        Add(2, 0m, 5m, 0m, 100m,
            [x => 20m+5m*x, x => 20m*(decimal)Math.Pow(1.3d,(double)x)],
            "Compare linear 20+5x and exponential 20(1.3)^x.");
        Add(3, 0m, 8m, 0m, 230m,
            [x => 80m*(decimal)Math.Pow(1.12d,(double)x)],
            "Population P(t)=80(1.12)^t, t>=0; interpretation is a model, not indefinite truth.");
        Add(4, -3m, 4m, 0m, 20m,
            [x => 2.5m*(decimal)Math.Pow(.65d,(double)x)],
            "Decay y=2.5(0.65)^x; decreasing and bounded below by zero.");
        Add(5, -3m, 4m, 0m, 40m,
            [x => 2m*(decimal)Math.Pow(2d,(double)x)],
            "Use (0,2) and (2,8) to recover a=2 and b=2.");
        Add(6, -3m, 4m, 0m, 20m,
            [x => (decimal)Math.Sqrt(2d)*(decimal)Math.Pow(1.4d,(double)x)],
            "Graph with irrational initial value sqrt(2), growth factor 1.4.");
        return items;

        void Add(int n, decimal xmin, decimal xmax, decimal ymin,
            decimal ymax, Func<decimal, decimal>[] functions, string prompt)
        {
            var lines = new List<MathematicalSegment>();
            foreach (var f in functions)
            {
                MathematicalPoint? previous = null;
                for (var index=0; index<=120; index++)
                {
                    var x=xmin+(xmax-xmin)*index/120m;
                    var y=f(x);
                    if (y<ymin || y>ymax)
                    {
                        previous=null;
                        continue;
                    }
                    var point=new MathematicalPoint(x,y);
                    if (previous is not null)
                        lines.Add(new MathematicalSegment(previous,point));
                    previous=point;
                }
            }
            var model=new CoordinatePlaneVisualSpec(
                new MathematicalAxisWindow(xmin,xmax,ymin,ymax,1m,10m),
                [],lines,new MathematicalVisualAccessibility(
                    $"First-party exponential figure skill {n}",
                    prompt+" Independently authored mathematical graph, not an OpenStax image."));
            MathematicalVisualSpecValidator.Validate(model);
            items.Add(new GraphTask(n,prompt,model));
        }
    }
}
