using Edulytics.Core.Mathematics.Visuals;
namespace Edulytics.Web.Presentation;
/// <summary>Original polar curves as mathematical sketches, not source images.</summary>
public static class Grade12PrecalculusSection84PolarCurveModels
{
    public sealed record Task(int SourceFigureSkill,string Prompt,CoordinatePlaneVisualSpec Graph);
    public static readonly IReadOnlyList<Task> All=Build();
    private static IReadOnlyList<Task> Build()
    {
        var items=new List<Task>();
        Add(1,"circle",3d);
        Add(2,"cardioid",4d);
        Add(3,"limacon",5d);
        Add(4,"rose",2d);
        Add(5,"lemniscate",3d);
        Add(6,"spiral",4d);
        Add(7,"circle",5d);
        Add(8,"cardioid",2d);
        Add(9,"limacon",3d);
        Add(10,"rose",4d);
        Add(11,"lemniscate",5d);
        Add(12,"spiral",2d);
        Add(13,"circle",3d);
        Add(14,"cardioid",4d);
        Add(15,"limacon",5d);
        Add(16,"rose",2d);
        Add(17,"lemniscate",3d);
        Add(18,"spiral",4d);
        Add(19,"circle",5d);
        Add(20,"cardioid",2d);
        Add(21,"limacon",3d);
        return items;
        void Add(int n,string kind,double A)
        {
            var lines=new List<MathematicalSegment>();
            MathematicalPoint? prev=null;
            for(int i=0;i<=120;i++)
            {
                var theta=4d*Math.PI*i/120d;
                var v=kind switch
                {
                    "circle"=>A*Math.Cos(theta),
                    "cardioid"=>A*(1d+Math.Cos(theta))/2d,
                    "limacon"=>A*.5d+Math.Sin(theta)*A,
                    "rose"=>A*Math.Cos(3d*theta),
                    "lemniscate"=>Math.Sqrt(Math.Max(0d,Math.Cos(2d*theta)))*A,
                    _=>theta*A/(4d*Math.PI)
                };
                if(kind=="lemniscate"&&Math.Cos(2d*theta)<0d)
                {prev=null;continue;}
                var next=new MathematicalPoint((decimal)(v*Math.Cos(theta)),
                    (decimal)(v*Math.Sin(theta)));
                if(prev is not null && Math.Abs((double)(next.X-prev.X))<1.5d &&
                                      Math.Abs((double)(next.Y-prev.Y))<1.5d)
                    lines.Add(new MathematicalSegment(prev,next));
                prev=next;
            }
            var graph=new CoordinatePlaneVisualSpec(
                new MathematicalAxisWindow(-9m,9m,-9m,9m,3m,3m),
                [],lines,new MathematicalVisualAccessibility(
                    $"First-party polar curve source skill {n}",
                    $"Independent {kind} polar curve with scale {A}."));
            MathematicalVisualSpecValidator.Validate(graph);
            items.Add(new Task(n,$"Polar {kind} mathematical investigation",graph));
        }
    }
}
