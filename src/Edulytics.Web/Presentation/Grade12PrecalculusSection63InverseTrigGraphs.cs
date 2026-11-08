using Edulytics.Core.Mathematics.Visuals;
namespace Edulytics.Web.Presentation;
/// <summary>First-party principal inverse trig graphics, no source pixels.</summary>
public static class Grade12PrecalculusSection63InverseTrigGraphs
{
    public sealed record Task(int SourceFigureSkill,string Prompt,CoordinatePlaneVisualSpec Graph);
    public static readonly IReadOnlyList<Task> All=Build();
    private static IReadOnlyList<Task> Build()
    {
        var tasks=new List<Task>();
        Add(1,"asin",0.5d);
        Add(2,"acos",0.5d);
        Add(3,"atan",0.5d);
        Add(4,"asin",1d);
        Add(5,"acos",1d);
        Add(6,"atan",1d);
        Add(7,"asin",1.5d);
        Add(8,"acos",1.5d);
        Add(9,"atan",1.5d);
        Add(10,"asin",2d);
        Add(11,"acos",2d);
        Add(12,"atan",2d);
        return tasks;
        void Add(int n,string kind,double scale)
        {
            var segments=new List<MathematicalSegment>();
            MathematicalPoint? previous=null;
            for(int i=0;i<=120;i++)
            {
                var x=-2d+4d*i/120d;
                var u=x/scale;
                if(kind!="atan" && Math.Abs(u)>1d)
                {previous=null;continue;}
                var y=kind switch
                { "asin"=>Math.Asin(u), "acos"=>Math.Acos(u),
                  "atan"=>Math.Atan(u), _=>0d };
                var point=new MathematicalPoint((decimal)x,(decimal)y);
                if(previous is not null)
                    segments.Add(new MathematicalSegment(previous,point));
                previous=point;
            }
            var graph=new CoordinatePlaneVisualSpec(
                new MathematicalAxisWindow(-3m,3m,-4m,4m,1m,1m),
                [],segments,new MathematicalVisualAccessibility(
                    $"Inverse trig figure skill {n}",
                    $"Independent principal {kind} graph scaled horizontally; not source illustration."));
            MathematicalVisualSpecValidator.Validate(graph);
            tasks.Add(new Task(n,$"Principal inverse {kind}, input scale {scale}",graph));
        }
    }
}
