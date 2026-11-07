using Edulytics.Core.Mathematics.Visuals;
namespace Edulytics.Web.Presentation;
/// <summary>Independently plotted polar grid points, not source publisher art.</summary>
public static class Grade12PrecalculusSection83PolarModels
{
    public sealed record Task(int SourceFigureSkill,string Prompt,CoordinatePlaneVisualSpec Graph);
    public static readonly IReadOnlyList<Task> All=Build();
    private static IReadOnlyList<Task> Build()
    {
        var items=new List<Task>();
        Add(1,3d,0.78539816d);
        Add(2,4d,1.57079633d);
        Add(3,-5d,2.35619449d);
        Add(4,2d,3.14159265d);
        Add(5,3d,3.92699082d);
        Add(6,-4d,4.71238898d);
        Add(7,5d,5.49778714d);
        Add(8,2d,0.00000000d);
        Add(9,-3d,0.78539816d);
        Add(10,4d,1.57079633d);
        Add(11,5d,2.35619449d);
        Add(12,-2d,3.14159265d);
        Add(13,3d,3.92699082d);
        return items;
        void Add(int n,double r,double theta)
        {
            var lines=new List<MathematicalSegment>();
            for(int i=1;i<=96;i++)
            {
                var a=2*Math.PI*(i-1)/96d;var b=2*Math.PI*i/96d;
                lines.Add(new MathematicalSegment(
                    new MathematicalPoint((decimal)(3*Math.Cos(a)),(decimal)(3*Math.Sin(a))),
                    new MathematicalPoint((decimal)(3*Math.Cos(b)),(decimal)(3*Math.Sin(b)))));
            }
            var origin=new MathematicalPoint(0m,0m);
            var tip=new MathematicalPoint((decimal)(r*Math.Cos(theta)),(decimal)(r*Math.Sin(theta)));
            lines.Add(new MathematicalSegment(origin,tip));
            var graph=new CoordinatePlaneVisualSpec(
                new MathematicalAxisWindow(-7m,7m,-7m,7m,2m,2m),
                [],lines,new MathematicalVisualAccessibility(
                    $"Original polar figure skill {n}",
                    $"Independent polar point with signed radius {r} and angle {theta} radians."));
            MathematicalVisualSpecValidator.Validate(graph);
            items.Add(new Task(n,$"r={r:0.##}, θ={theta:0.##} rad",graph));
        }
    }
}
