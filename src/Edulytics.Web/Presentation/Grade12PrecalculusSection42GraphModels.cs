using Edulytics.Core.Mathematics.Visuals;

namespace Edulytics.Web.Presentation;

/// <summary>Independent source-skill alternatives for all 13 Section 4.2 figures.
/// SVG graph rendering reuses the existing mathematics renderer. This is not
/// a textbook illustration reproduction; tests reserved for Stage D.</summary>
public static class Grade12PrecalculusSection42GraphModels
{
    public sealed record GraphTask(int SourceFigureSkill, string Prompt,
        CoordinatePlaneVisualSpec Graph);
    public static readonly IReadOnlyList<GraphTask> All=Build();
    private static IReadOnlyList<GraphTask> Build()
    {
        var items=new List<GraphTask>();
        Add(1,-3m,3m,0m,30m,
            "Independent graph g(x)=3^x, y-intercept1, all-real domain, positive range and horizontal y=0.",
            [x => (decimal)Math.Pow(3d,(double)x)]);
        Add(2,-3m,3m,0m,30m,
            "Independent graph g(x)=(1/3)^x, horizontal y=0, y-intercept1.",
            [x => (decimal)Math.Pow(1d/3d,(double)x)]);
        Add(3,-3m,3m,0m,30m,
            "Independent graphs y=3^x and y=3^(−x), both through (0,1), related by reflection about y-axis.",
            [x => (decimal)Math.Pow(3d,(double)x), x => (decimal)Math.Pow(3d,(double)(-x))]);
        Add(4,-3m,3m,0m,50m,
            "Independent g(x)=(1/4)^x, points (−1,4),(0,1),(1,1/4).",
            [x => (decimal)Math.Pow(.25d,(double)x)]);
        Add(5,-3m,4m,-5m,35m,
            "Independent parent 2^x and translated 2^x+2, 2^x−2; asymptotes y=0,2,−2.",
            [x => (decimal)Math.Pow(2d,(double)x), x => (decimal)Math.Pow(2d,(double)x)+2m, x => (decimal)Math.Pow(2d,(double)x)-2m]);
        Add(6,-4m,4m,0m,70m,
            "Independent 2^x,2^(x+2),2^(x−2) showing left/right translations.",
            [x => (decimal)Math.Pow(2d,(double)x), x => (decimal)Math.Pow(2d,(double)(x+2m)), x => (decimal)Math.Pow(2d,(double)(x-2m))]);
        Add(7,-3m,4m,-5m,20m,
            "Independent y=2^(x+1)−3, asymptote y=−3, points (−1,−2),(0,−1),(1,1).",
            [x => (decimal)Math.Pow(2d,(double)(x+1m))-3m]);
        Add(8,-3m,4m,0m,60m,
            "Independent y=3·2^x and y=(1/3)·2^x, y-intercepts3 and1/3.",
            [x => 3m*(decimal)Math.Pow(2d,(double)x), x => (1m/3m)*(decimal)Math.Pow(2d,(double)x)]);
        Add(9,-3m,4m,0m,40m,
            "Independent y=4·(1/2)^x, intercept4, decay factor1/2.",
            [x => 4m*(decimal)Math.Pow(.5d,(double)x)]);
        Add(10,-3m,3m,-30m,30m,
            "Independent parent y=3^x, x-axis reflection −3^x and y-axis reflection 3^(−x).",
            [x => (decimal)Math.Pow(3d,(double)x), x => -(decimal)Math.Pow(3d,(double)x), x => (decimal)Math.Pow(3d,(double)(-x))]);
        Add(11,-3m,3m,-30m,0m,
            "Independent y=−(1/3)^x, negative range and y-intercept−1.",
            [x => -(decimal)Math.Pow(1d/3d,(double)x)]);
        Add(12,-2m,3m,0m,24m,
            "Independent A=2(.6)^x,B=4(.6)^x,C=2(.85)^x,D=2(1.3)^x,E=4(1.3)^x,F=2(1.7)^x. Compare a at x=0 and b ratios.",
            [x => 2m*(decimal)Math.Pow(.6d,(double)x), x => 4m*(decimal)Math.Pow(.6d,(double)x), x => 2m*(decimal)Math.Pow(.85d,(double)x), x => 2m*(decimal)Math.Pow(1.3d,(double)x), x => 4m*(decimal)Math.Pow(1.3d,(double)x), x => 2m*(decimal)Math.Pow(1.7d,(double)x)]);
        Add(13,-2m,3m,0m,40m,
            "Independent A=2(.5)^x,B=6(.8)^x,C=(.3)^x,D=3(1.8)^x,E=4(1.3)^x,F=.5(1.1)^x. Largest b:D, smallest b:C, largest a:B, smallest a:F.",
            [x => 2m*(decimal)Math.Pow(.5d,(double)x), x => 6m*(decimal)Math.Pow(.8d,(double)x), x => (decimal)Math.Pow(.3d,(double)x), x => 3m*(decimal)Math.Pow(1.8d,(double)x), x => 4m*(decimal)Math.Pow(1.3d,(double)x), x => .5m*(decimal)Math.Pow(1.1d,(double)x)]);

        return items;
        void Add(int number, decimal xmin, decimal xmax, decimal ymin,
            decimal ymax, string description, Func<decimal,decimal>[] functions)
        {
            var segments=new List<MathematicalSegment>();
            foreach (var func in functions)
            {
                MathematicalPoint? previous=null;
                for (var i=0; i<=120; i++)
                {
                    var x=xmin+(xmax-xmin)*i/120m;
                    var y=func(x);
                    if (y<ymin || y>ymax)
                    {
                        previous=null;
                        continue;
                    }
                    var point=new MathematicalPoint(x,y);
                    if (previous is not null)
                        segments.Add(new MathematicalSegment(previous,point));
                    previous=point;
                }
            }
            var graph=new CoordinatePlaneVisualSpec(
                new MathematicalAxisWindow(xmin,xmax,ymin,ymax,1m,5m),
                [],segments,new MathematicalVisualAccessibility(
                    $"Original Edulytics exponential graph skill {number}",
                    description+" Independently authored; no original source pixels copied."));
            MathematicalVisualSpecValidator.Validate(graph);
            items.Add(new GraphTask(number,description,graph));
        }
    }
}
