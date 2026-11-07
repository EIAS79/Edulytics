using Edulytics.Core.Mathematics.Visuals;
namespace Edulytics.Web.Presentation;
/// <summary>First-party sinusoidal figure skill alternatives, non-copy of source.</summary>
public static class Grade12PrecalculusSection61SineCosineGraphs
{
    public sealed record Task(int SourceFigureSkill,string Prompt,CoordinatePlaneVisualSpec Graph);
    public static readonly IReadOnlyList<Task> All=Build();
    private static IReadOnlyList<Task> Build()
    {
        var items=new List<Task>();
        Add(1,2d,1d,0d,0d,true);
        Add(2,0.66666667d,1d,0d,0d,false);
        Add(3,-3d,1d,0d,0d,true);
        Add(4,4d,1d,0d,0d,true);
        Add(5,2d,1d,0d,0d,false);
        Add(6,1d,2d,0d,0d,false);
        Add(7,2d,0.5d,0d,0d,true);
        Add(8,4d,3.1415927d,0d,0d,false);
        Add(9,3d,1.2d,0d,0d,false);
        Add(10,3d,8d,32d,5d,true);
        Add(11,2d,3d,-21d,4d,true);
        Add(12,5d,5d,20d,-2d,true);
        Add(13,2d,1d,-2.6179939d,0d,true);
        Add(14,-1d,1d,1.0471976d,1d,false);
        Add(15,4d,2d,1.5707963d,-3d,false);
        Add(16,-1d,0.5d,5.2359878d,0d,true);
        Add(17,4d,1.5707963d,-4.712389d,7d,true);
        Add(18,2d,2d,-1.5707963d,-1d,true);
        Add(19,3d,0.5d,0d,0d,false);
        Add(20,-2d,3d,0.78539816d,1d,true);
        Add(21,1d,1d,-1.5707963d,-1d,false);
        Add(22,2d,2d,0d,0d,true);
        Add(23,3d,0.5d,0.78539816d,1d,false);
        Add(24,-2d,3d,-1.5707963d,-1d,true);
        Add(25,1d,1d,0d,0d,false);
        Add(26,2d,2d,0.78539816d,1d,true);
        Add(27,3d,0.5d,-1.5707963d,-1d,false);
        Add(28,-2d,3d,0d,0d,true);
        Add(29,1d,1d,0.78539816d,1d,false);
        Add(30,2d,2d,-1.5707963d,-1d,true);
        Add(31,3d,0.5d,0d,0d,false);
        Add(32,-2d,3d,0.78539816d,1d,true);
        Add(33,1d,1d,-1.5707963d,-1d,false);
        return items;
        void Add(int n,double A,double B,double C,double D,bool sine)
        {
            var segments=new List<MathematicalSegment>();
            MathematicalPoint? previous=null;
            for(int i=0;i<=120;i++)
            {
                var x=-2*Math.PI+4*Math.PI*i/120d;
                var y=A*(sine?Math.Sin(B*x+C):Math.Cos(B*x+C))+D;
                var point=new MathematicalPoint((decimal)x,(decimal)y);
                if(previous is not null)segments.Add(new MathematicalSegment(previous,point));
                previous=point;
            }
            var max=Math.Abs(A)+Math.Abs(D)+1d;
            var graph=new CoordinatePlaneVisualSpec(
                new MathematicalAxisWindow(-7m,7m,-10m,10m,2m,2m),
                [],segments,new MathematicalVisualAccessibility(
                    $"Independent sinusoidal source skill {n}",
                    "Original Edulytics sine/cosine transformed wave, not publisher artwork."));
            MathematicalVisualSpecValidator.Validate(graph);
            items.Add(new Task(n,$"Amplitude {Math.Abs(A):0.##}; period {2*Math.PI/Math.Abs(B):0.##}; midline {D:0.##}",graph));
        }
    }
}
