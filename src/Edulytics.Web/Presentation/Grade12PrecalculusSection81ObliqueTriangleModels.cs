using Edulytics.Core.Mathematics.Visuals;
namespace Edulytics.Web.Presentation;
/// <summary>Independently authored oblique triangle mathematics, not source art.</summary>
public static class Grade12PrecalculusSection81ObliqueTriangleModels
{
    public sealed record Task(int SourceFigureSkill,string Prompt,CoordinatePlaneVisualSpec Graph);
    public static readonly IReadOnlyList<Task> All=Build();
    private static IReadOnlyList<Task> Build()
    {
        var tasks=new List<Task>();
        Add(1,37d,41d);
        Add(2,44d,52d);
        Add(3,51d,63d);
        Add(4,58d,74d);
        Add(5,65d,40d);
        Add(6,72d,51d);
        Add(7,34d,62d);
        Add(8,41d,73d);
        Add(9,48d,39d);
        Add(10,55d,50d);
        Add(11,62d,61d);
        Add(12,69d,72d);
        Add(13,31d,38d);
        Add(14,38d,49d);
        Add(15,45d,60d);
        Add(16,52d,71d);
        Add(17,59d,37d);
        Add(18,66d,48d);
        Add(19,73d,59d);
        Add(20,35d,70d);
        Add(21,42d,36d);
        Add(22,49d,47d);
        Add(23,56d,58d);
        Add(24,63d,69d);
        Add(25,70d,35d);
        Add(26,32d,46d);
        Add(27,39d,57d);
        Add(28,46d,68d);
        Add(29,53d,34d);
        Add(30,60d,45d);
        Add(31,67d,56d);
        Add(32,74d,67d);
        Add(33,36d,33d);
        Add(34,43d,44d);
        Add(35,50d,55d);
        return tasks;
        void Add(int n,double A,double B)
        {
            var C=180d-A-B;
            var across=5d*Math.Sin(B*Math.PI/180d)/Math.Sin(C*Math.PI/180d);
            var x=across*Math.Cos(A*Math.PI/180d);
            var y=across*Math.Sin(A*Math.PI/180d);
            var p0=new MathematicalPoint(0m,0m);
            var p1=new MathematicalPoint(5m,0m);
            var p2=new MathematicalPoint((decimal)x,(decimal)y);
            var segments=new List<MathematicalSegment>
            {new(p0,p1),new(p1,p2),new(p2,p0)};
            var graph=new CoordinatePlaneVisualSpec(
                new MathematicalAxisWindow(-2m,10m,-1m,10m,2m,2m),
                [],segments,new MathematicalVisualAccessibility(
                    $"Oblique triangle source skill {n}",
                    $"Independent triangle with angles {A}, {B}, {C}; use Law of Sines."));
            MathematicalVisualSpecValidator.Validate(graph);
            tasks.Add(new Task(n,$"A={A:0}°, B={B:0}°, C={C:0}°; base5",graph));
        }
    }
}
