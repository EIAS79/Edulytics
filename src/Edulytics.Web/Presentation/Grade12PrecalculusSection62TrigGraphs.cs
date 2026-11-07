using Edulytics.Core.Mathematics.Visuals;
namespace Edulytics.Web.Presentation;
/// <summary>First-party discontinuous trig graphs. No publisher illustration reproduced.</summary>
public static class Grade12PrecalculusSection62TrigGraphs
{
    public sealed record Task(int SourceFigureSkill,string Prompt,CoordinatePlaneVisualSpec Graph);
    public static readonly IReadOnlyList<Task> All=Build();
    private static IReadOnlyList<Task> Build()
    {
        var items=new List<Task>();
        Add(1,"tan",2d,4d,-16d,0d);
        Add(2,"sec",2d,0.78539816d,0.78539816d,0d);
        Add(3,"csc",6d,1.0471976d,3.1415927d,0d);
        Add(4,"tan",1d,1.5707963d,0d,0d);
        Add(5,"tan",1d,1d,-1.5707963d,0d);
        Add(6,"tan",4d,1d,0d,0d);
        Add(7,"tan",1d,1d,0.78539816d,0d);
        Add(8,"tan",3.1415927d,3.1415927d,-3.1415927d,-3.1415927d);
        Add(9,"csc",2d,1d,0d,0d);
        Add(10,"csc",-0.25d,1d,0d,0d);
        Add(11,"sec",4d,3d,0d,0d);
        Add(12,"cot",-3d,2d,0d,0d);
        Add(13,"sec",7d,5d,0d,0d);
        Add(14,"csc",0.9d,3.1415927d,0d,0d);
        Add(15,"csc",2d,1d,0.78539816d,-1d);
        Add(16,"sec",-1d,1d,-1.0471976d,-2d);
        Add(17,"csc",1.4d,1d,-0.78539816d,0d);
        Add(18,"cot",5d,1d,1.5707963d,-3d);
        Add(19,"tan",2d,1d,0d,0d);
        return items;
        void Add(int n,string kind,double A,double B,double C,double D)
        {
            var segments=new List<MathematicalSegment>();
            MathematicalPoint? previous=null;
            for(int i=0;i<=700;i++)
            {
                var x=-2*Math.PI+4*Math.PI*i/700d;
                var s=Math.Sin(B*x+C);var c=Math.Cos(B*x+C);
                var invalid=(kind=="tan"||kind=="sec")?Math.Abs(c)<.02d:Math.Abs(s)<.02d;
                if(invalid) {previous=null;continue;}
                var y=kind switch
                {
                    "tan"=>A*s/c+D, "cot"=>A*c/s+D,
                    "sec"=>A/c+D, "csc"=>A/s+D, _=>0d
                };
                if(double.IsNaN(y)||double.IsInfinity(y)||Math.Abs(y)>10d)
                {previous=null;continue;}
                var next=new MathematicalPoint((decimal)x,(decimal)y);
                if(previous is not null && Math.Abs((double)(next.Y-previous.Y))<2d)
                    segments.Add(new MathematicalSegment(previous,next));
                previous=next;
            }
            var graph=new CoordinatePlaneVisualSpec(
                new MathematicalAxisWindow(-7m,7m,-10m,10m,2m,2m),
                [],segments,new MathematicalVisualAccessibility(
                    $"Independent trigonometric figure skill {n}",
                    $"Independent {kind} curve, undefined at denominator zero; not publisher artwork."));
            MathematicalVisualSpecValidator.Validate(graph);
            items.Add(new Task(n,$"{kind} transformed mathematical graph; verify domain and asymptotes",graph));
        }
    }
}
