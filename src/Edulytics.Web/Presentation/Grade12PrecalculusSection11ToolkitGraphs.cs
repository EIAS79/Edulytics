using Edulytics.Core.Mathematics.Visuals;

namespace Edulytics.Web.Presentation;

/// <summary>
/// Independent original visuals for all nine basic function families used
/// in Section 1.1. This is a teaching set, not a reproduction of textbook
/// figures. Domain boundaries and branches are explicitly documented.
/// </summary>
public static class Grade12PrecalculusSection11ToolkitGraphs
{
    public sealed record Family(string Title, string Domain, string Range,
        CoordinatePlaneVisualSpec Graph);

    public static readonly IReadOnlyList<Family> All = Build();

    private static IReadOnlyList<Family> Build()
    {
        var set = new List<Family>();
        Add("Constant y=1","all real","{1}",-3m,3m,-2m,3m,
            [(-3m,3m,x=>1m)]);
        Add("Identity y=x","all real","all real",-3m,3m,-3m,3m,
            [(-3m,3m,x=>x)]);
        Add("Absolute value y=|x|","all real","[0,∞)",-3m,3m,-1m,3m,
            [(-3m,3m,x=>Math.Abs(x))]);
        Add("Quadratic y=x²","all real","[0,∞)",-2m,2m,-1m,4m,
            [(-2m,2m,x=>x*x)]);
        Add("Cubic y=x³","all real","all real",-1.5m,1.5m,-4m,4m,
            [(-1.5m,1.5m,x=>x*x*x)]);
        Add("Reciprocal y=1/x","x≠0","y≠0",-3m,3m,-3m,3m,
            [(-3m,-.4m,x=>1m/x),(.4m,3m,x=>1m/x)]);
        Add("Reciprocal square y=1/x²","x≠0","(0,∞)",-3m,3m,-1m,3m,
            [(-3m,-.65m,x=>1m/(x*x)),(.65m,3m,x=>1m/(x*x))]);
        Add("Square root y=√x","x≥0","[0,∞)",-1m,4m,-1m,3m,
            [(0m,4m,x=>(decimal)Math.Sqrt((double)x))]);
        Add("Cube root y=∛x","all real","all real",-4m,4m,-2m,2m,
            [(-4m,4m,x=>(decimal)Math.Cbrt((double)x))]);
        if (set.Count != 9)
            throw new InvalidOperationException("All nine toolkit families are required.");
        return set;

        void Add(string title, string domain, string range,
            decimal xmin, decimal xmax, decimal ymin, decimal ymax,
            (decimal Start, decimal End, Func<decimal,decimal> Function)[] intervals)
        {
            var segments = new List<MathematicalSegment>();
            foreach (var (start,end,f) in intervals)
            {
                var last = new MathematicalPoint(start, f(start));
                for (var index=1;index<=40;index++)
                {
                    var x=start+(end-start)*index/40m;
                    var next=new MathematicalPoint(x,f(x));
                    segments.Add(new MathematicalSegment(last,next));
                    last=next;
                }
            }
            var spec = new CoordinatePlaneVisualSpec(
                new MathematicalAxisWindow(xmin,xmax,ymin,ymax,1m,1m),
                [],segments,
                new MathematicalVisualAccessibility(
                    title,
                    $"First-party teaching model. Real domain: {domain}; range: {range}. " +
                    "Separate branches do not include x=0 for reciprocal functions; " +
                    "√x begins at x=0. Read values and explain the permitted domain."));
            MathematicalVisualSpecValidator.Validate(spec);
            set.Add(new Family(title,domain,range,spec));
        }
    }
}
