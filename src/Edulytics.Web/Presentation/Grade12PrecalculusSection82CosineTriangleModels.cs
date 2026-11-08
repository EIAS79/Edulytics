using Edulytics.Core.Mathematics.Visuals;
namespace Edulytics.Web.Presentation;
/// <summary>Original oblique triangle cosine law illustrations.</summary>
public static class Grade12PrecalculusSection82CosineTriangleModels
{
    public sealed record Task(int SourceFigureSkill,string Prompt,CoordinatePlaneVisualSpec Graph);
    public static readonly IReadOnlyList<Task> All=Build();
    private static IReadOnlyList<Task> Build()
    {
        var items=new List<Task>();
        Add(1,4d,5d,46d);
        Add(2,5d,6d,57d);
        Add(3,6d,7d,68d);
        Add(4,7d,8d,79d);
        Add(5,8d,9d,90d);
        Add(6,3d,10d,101d);
        Add(7,4d,4d,112d);
        Add(8,5d,5d,123d);
        Add(9,6d,6d,134d);
        Add(10,7d,7d,35d);
        Add(11,8d,8d,46d);
        Add(12,3d,9d,57d);
        Add(13,4d,10d,68d);
        Add(14,5d,4d,79d);
        Add(15,6d,5d,90d);
        Add(16,7d,6d,101d);
        Add(17,8d,7d,112d);
        return items;
        void Add(int n,double a,double b,double theta)
        {
            var p0=new MathematicalPoint(0m,0m);
            var p1=new MathematicalPoint((decimal)a,0m);
            var p2=new MathematicalPoint((decimal)(b*Math.Cos(theta*Math.PI/180d)),
                (decimal)(b*Math.Sin(theta*Math.PI/180d)));
            var lines=new List<MathematicalSegment>
            {new(p0,p1),new(p1,p2),new(p2,p0)};
            var graph=new CoordinatePlaneVisualSpec(
                new MathematicalAxisWindow(-12m,12m,-2m,12m,2m,2m),
                [],lines,new MathematicalVisualAccessibility(
                    $"Law of Cosines source figure skill {n}",
                    $"First-party triangle sides {a}, {b} enclosing {theta} degrees."));
            MathematicalVisualSpecValidator.Validate(graph);
            items.Add(new Task(n,$"SAS: {a:0}, {b:0}, angle {theta:0}°",graph));
        }
    }
}
